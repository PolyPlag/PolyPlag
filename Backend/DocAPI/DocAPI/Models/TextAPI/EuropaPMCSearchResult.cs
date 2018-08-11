using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    //stores results of a pmc search result
    public class EuropaPMCSearchResult
    {
        public string title { get; private set; }
        public string authorString { get; private set; }
		public string abstractText { get; private set; }
        public string pubYear { get; private set; }
        public string uri { get; private set; }

        /// <summary>
        /// constructor for the creation of the search result of pmc
        /// </summary>
        /// <param name="title"></param>
        /// <param name="authorString"></param>
        /// <param name="abstractText"></param>
        /// <param name="pubYear"></param>
        /// <param name="uri"></param>
        public EuropaPMCSearchResult(string title, string authorString, string abstractText, string pubYear, string uri)
        {
            this.title = title;
            this.authorString = authorString;
            this.abstractText = abstractText;
            this.pubYear = pubYear;
            this.uri = uri;
        }
    }
}
