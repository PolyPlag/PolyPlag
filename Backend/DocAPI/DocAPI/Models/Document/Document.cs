using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace DocAPI.Models
{
    public class Document : IParsableTextDocument
    {

        public String Content { get; set; }
        public List<String> Sources { get; set; }
        public List<Uri> URLSources { get; set; }
        public Boolean HasPlagiates { get; private set; }
        public Boolean Done { get; private set; }
        public List<Plagiat<String>> Plagiate { get; set; }
        public LinkedList<String> Sentences { get; }
        public Boolean Faulty { get; private set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }

        public Document(String Content)
        {
            Sources = new List<string>();
            URLSources = new List<Uri>();
            Sentences = new LinkedList<string>();
            this.Content = Content;
        }

        /// <summary>
        /// Parses the Content of an Word. It creates the sentences and
        /// checks for plagiates. Call this function only from a thread!
        /// So it doesnt block the api.
        /// </summary>
        public void ParseContent()
        {
            //No matter what fault we will have. We abort the operation
            //We clear the content and set faulty to true
            //Later we can expand this handle errors custom
            try
            {

                //We want the whole word in the memory
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(Content));
                WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true);

                // Assign a reference to the existing document parts.
                var parts = wordDocument.MainDocumentPart.Document.Descendants().FirstOrDefault();
                if (parts != null)
                {
                    foreach (var node in parts.ChildElements)
                    {

                        if (node is Paragraph)
                        {

                            var stringParagraph = "";
                            foreach (var text in node.Descendants<Text>())
                            {

                                stringParagraph += (text.InnerText);
                            }
                            //Creates sentences out of the whole paragraph
                            ParagraphToSentences(stringParagraph);

                            //Searching bibliography
                        }
                        else if (node is SdtBlock)
                        {
                            //LINQ searching for the bibliography  
                            var biblios = from child in node
                                        .SelectMany(c => c.ChildElements)
                                        .Where(cn => cn.LocalName == "sdt")
                                        .SelectMany(c2 => c2.ChildElements)
                                        .Where(cn2 => cn2.LocalName == "sdtPr")
                                        .SelectMany(c3 => c3.ChildElements)
                                        .Where(cn3 => cn3.LocalName == "bibliography")
                                          select child.Parent.Parent;
                            if (biblios != null)
                            {
                                var xmlSrc = from p in biblios
                                             .SelectMany(c => c.ChildElements)
                                             .Where(cn => cn.LocalName == "sdtContent")
                                             .SelectMany(c1 => c1.ChildElements)
                                             select p;
                                if (xmlSrc != null)
                                {
                                    //Match for URL
                                    Regex r = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*");

                                    //Parsing sources
                                    foreach (var xml in xmlSrc)
                                    {
                                        var src = "";
                                        foreach (var text in xml.Descendants<Run>())
                                        {
                                            //Remove rubbish
                                            if (text.InnerText != "BIBLIOGRAPHY" && text.InnerText.Length > 6)
                                            {
                                                src += text.InnerText;
                                            }
                                            Match m = r.Match(text.InnerText);
                                            while (m.Success)
                                            {
                                                URLSources.Add(new Uri(m.ToString()));
                                                m = m.NextMatch();
                                            }
                                        }
                                        Sources.Add(src);
                                    }
                                }

                            }
                        }
                    }
                }

                //Cleanup time
                wordDocument.Dispose();

                //Check the APIs
                CheckForPlagiate();
            }
            catch
            {
                Done = true;

                //We remove the whole content. So it doesnt fill up our memory
                Content = "Error";

                //Sets the faulty variable
                //So with the next get, we will cleanup this error.
                
                Faulty = true;

            }
        }

        /// <summary>
        /// This takes an paragraph from the word xml and parses it into
        /// seperate sentences. Which get added too the Datafield Sentences. 
        /// </summary>
        /// <param name="paragraph"></param>
        private void ParagraphToSentences(String paragraph)
        {
            var sentences = paragraph.Split(".", StringSplitOptions.RemoveEmptyEntries);

            //Parse sentence ending
            string pattern = @"(\S.+?[.!?])(?=\s+|$)";
            Regex regex = new Regex(pattern);

            foreach (var sentence in regex.Split(paragraph))
            {
                if(sentence.Length > 6)
                {
                    Sentences.AddLast(sentence);
                    SentenceCount++;

                    WordCount += sentence.Split(" ").Length + 1;
                }
            }
        }

        /// <summary>
        /// This method we call at the end of the rendering of the document.
        /// We use it, too create a new Adapter too check all the Sentences against
        /// the APIs
        /// </summary>
        private void CheckForPlagiate()
        {
            var docToSearchAdapter = new DocToSearchAdapter();
            var plag = docToSearchAdapter.CheckSentences(Sentences);
            this.Plagiate = plag;
            if(plag.Count != 0)
            {
                HasPlagiates = true;
            }
            this.Done = true;
        }
    }
}
