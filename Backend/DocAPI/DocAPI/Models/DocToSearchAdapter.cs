using DocAPI.Models.TextAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models
{
    public class DocToSearchAdapter
    {
        private AzureTextAnalytics azureTextAnalytics;
        public DocToSearchAdapter()
        {
            azureTextAnalytics = new AzureTextAnalytics();
        }

        /// <summary>
        /// Method too check all the sentences against the APIs
        /// </summary>
        /// <param name="sentences"></param>
        /// <returns>List<Plagiat<String> which has the definitive
        /// plagiates inside </returns>
        public List<Plagiat<String>> CheckSentences(LinkedList<String> sentences)
        {
            //Initialize the plagiates
            var plagiatList = new List<Plagiat<String>>();
            var counter = 0;
            foreach (string sentence in sentences)
            {   
                var check = azureTextAnalytics.Check(new List<String>() { sentence }, null, null);
                foreach(Plagiat<String> plag in check)
                {
                    if (SentenceSimilarEnough(plag.Origin, plag.FoundPlag, 70))
                    {
                        //Adds new plagiat if they are similar enough
                        plagiatList.Add(plag);

                        //Adding the phrases before and after it.
                        if (counter > 0)
                        {
                            plag.BeforeOrigin = sentences.ElementAt(counter - 1);
                        }
                        if (counter < (sentences.Count - 1))
                        {
                            plag.AfterOrigin = sentences.ElementAt(counter + 1);
                        }
                    }

                }
                counter += 1;
            }
            return plagiatList;
        }

        /// <summary>
        /// Used to determine if the sentence and the found plagiate are close 
        /// Truly close enough.
        /// </summary>
        /// <param name="sentence">the sentence in the document</param>
        /// <param name="plagiat">the plagiat</param>
        /// <param name="matchPercent">the threshold for a match</param>
        /// <returns></returns>

        private Boolean SentenceSimilarEnough(String sentence, String plagiat, int matchPercent) 
        {
            int length = sentence.Length;
            int found = 0;
            foreach (string word in sentence.Split(" ")){
                if (plagiat.Contains(word))
                {
                    found += word.Length;
                }
            } 
            if(((double)found/(double)length)*100 > matchPercent)
            {
                return true;
            }else
            {
                return false;
            }
            
        }
    }
}
