using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    //statistisc about one sentence in a document
    public class SentenceStatistics
    {
        public string id { get; private set; }
        public string sentence { get; set; }
        public string detectedLanguage { get; set; }
        public List<String> keyPhrases { get; set; }
        public LinkedList<GoogleSearchResult> googleSearchResult { get; set; }
        public LinkedList<EuropaPMCSearchResult> europaPMCSearchResult { get; set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public SentenceStatistics()
        {
            keyPhrases = new List<string>();
        }

        /// <summary>
        /// constructor using parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sentence"></param>
        /// <param name="detecedLanguage"></param>
        /// <param name="keyPhrases"></param>
        public SentenceStatistics(string id, string sentence, string detecedLanguage, List<String> keyPhrases)
        {
            this.id = id;
            this.sentence = sentence;
            this.detectedLanguage = detectedLanguage;
            this.keyPhrases = keyPhrases;
        }
    }
}
