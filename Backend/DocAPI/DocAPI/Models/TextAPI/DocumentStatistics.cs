using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    //provides all statistics information about a sentence 
    public class DocumentStatistics
    {
        private List<SentenceStatistics> sentenceStatistics { get; set; }
         
        /// <summary>
        /// basic constructor
        /// </summary>
        public DocumentStatistics()
        {
            this.sentenceStatistics = new List<SentenceStatistics>();
        }

        /// <summary>
        /// constructor providing a list of sentences
        /// </summary>
        /// <param name="origin"></param>
        public DocumentStatistics(List<String> origin)
        {
            this.sentenceStatistics = new List<SentenceStatistics>();
            for (int i = 0; i < origin.Count; i++)
            {
                sentenceStatistics.Add(new SentenceStatistics(i.ToString(), origin[i], "", new List<String>()));
            }
        }

        /// <summary>
        /// add a sentence to the statistic
        /// </summary>
        /// <param name="sentenceStatistic"></param>
        public void addSenteceStatistics(SentenceStatistics sentenceStatistic)
        {
            this.sentenceStatistics.Add(sentenceStatistic);
        }

        /// <summary>
        /// get batch input for azure cognitive services
        /// </summary>
        /// <returns></returns>
        public List<Input> getBatchInput()
        {
            List<Input> sentenceList = new List<Input>();

            foreach (SentenceStatistics element in sentenceStatistics)
            {
                sentenceList.Add(new Input(element.id, element.sentence));
            }

            return sentenceList;
        }

        /// <summary>
        /// get all sentences
        /// </summary>
        /// <returns></returns>
        public List<String> getSentences()
        {
            List<String> sentences = new List<string>();

            foreach (SentenceStatistics element in sentenceStatistics)
            {
                sentences.Add(element.sentence);
            }

            return sentences;
        }

        /// <summary>
        /// update sentence language based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        public void updateSentenceLanguage(string id, string language)
        {
            foreach (SentenceStatistics element in sentenceStatistics)
            {
                if (element.id.Equals(id))
                {
                    element.detectedLanguage = language;
                }
            }
        }

        /// <summary>
        /// get multibatch language for azure cognitive services
        /// </summary>
        /// <returns></returns>
        public List<MultiLanguageInput> getMultiLanguageBatchInput()
        {
            List<MultiLanguageInput> multiLanguageInput = new List<MultiLanguageInput>();

            foreach (SentenceStatistics element in sentenceStatistics)
            {
                multiLanguageInput.Add(new MultiLanguageInput(element.detectedLanguage, element.id, element.sentence));
            }

            return multiLanguageInput;
        }

        /// <summary>
        /// update key phrases based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyPhrases"></param>
        public void updateKeyPhares(string id, List<String> keyPhrases)
        {
            foreach (SentenceStatistics element in sentenceStatistics)
            {
                if (element.id.Equals(id))
                {
                    element.keyPhrases = keyPhrases;
                }
            }
        }

        /// <summary>
        /// update google search results for sentence based on sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="googleSearchResults"></param>
        public void updateGoogleSearchResults(string sentence, LinkedList<GoogleSearchResult> googleSearchResults)
        {
            foreach (SentenceStatistics element in sentenceStatistics)
            {
                if (element.sentence.Equals(sentence))
                {
                    element.googleSearchResult = googleSearchResults;

                }
            }
        }

        /// <summary>
        /// update pmc search results based on sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="europaPMCSearchResult"></param>
        public void updateEuropaPMCSearchResults(string sentence, LinkedList<EuropaPMCSearchResult> europaPMCSearchResult)
        {
            foreach (SentenceStatistics element in sentenceStatistics)
            {
                if (element.sentence.Equals(sentence))
                {
                    element.europaPMCSearchResult = europaPMCSearchResult;

                }
            }
        }

        /// <summary>
        /// temporary export for debug
        /// </summary>
        /// <returns></returns>
        public LinkedList<String> GetLinkedList()
        {
            LinkedList<string> linkedList = new LinkedList<string>();

            foreach (SentenceStatistics element in sentenceStatistics)
            {
                foreach (EuropaPMCSearchResult europaPMCSearchResult in element.europaPMCSearchResult)
                {
                    linkedList.AddLast(europaPMCSearchResult.title);

                }
            }
            return linkedList;
        }

        /// <summary>
        /// This class generates an list of possible plagiates
        /// </summary>
        /// <returns></returns>
        public List<Plagiat<String>> getPossiblePlagiates()
        {
            var possiblePlagiates = new List<Plagiat<String>>();
            foreach (SentenceStatistics sentenceStatistic in sentenceStatistics)
            {
                if (sentenceStatistic.googleSearchResult != null)
                {
                    foreach (GoogleSearchResult googleSearchResult in sentenceStatistic.googleSearchResult)
                    {
                        possiblePlagiates.Add(
                            new Plagiat<string>(sentenceStatistic.sentence, googleSearchResult.snippet, googleSearchResult.link)
                            );
                    }
                }
                if (sentenceStatistic.europaPMCSearchResult != null)
                {
                    foreach (EuropaPMCSearchResult epmcSearchResult in sentenceStatistic.europaPMCSearchResult)
                    {

                        possiblePlagiates.Add(
                            new Plagiat<string>(sentenceStatistic.sentence, epmcSearchResult.abstractText, epmcSearchResult.uri)
                            );
                    }
                }
            }
            return possiblePlagiates;
        }
    }
}
