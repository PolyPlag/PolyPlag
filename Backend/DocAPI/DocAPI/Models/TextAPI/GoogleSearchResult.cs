using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    //stores results of a google search
    public class GoogleSearchResult
    {
        public string origin { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string snippet { get; set; }

        /// <summary>
        /// constructor for the search results of google
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="title"></param>
        /// <param name="link"></param>
        /// <param name="snippet"></param>
        public GoogleSearchResult(string origin, string title, string link, string snippet)
        {
            this.origin = origin;
            this.title = title;
            this.link = link;
            this.snippet = snippet;
        }
    }
}
