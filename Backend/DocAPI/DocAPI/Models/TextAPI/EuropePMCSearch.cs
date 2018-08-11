using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    public class EuropaPMCSearch
    {
        public Uri API { get; set; }

        public EuropaPMCSearch(Uri API)
        {
            this.API = API;
        }

        /// <summary>
        /// query pmc and store results in the document statistic
        /// </summary>
        /// <param name="documentStatistics"></param>
        public void Check(DocumentStatistics documentStatistics)
        {
            foreach (string sentence in documentStatistics.getSentences())
            {
                LinkedList<EuropaPMCSearchResult> europaPMCSearchResult = new LinkedList<EuropaPMCSearchResult>();

                Uri apiUri = new Uri(API + "query=" + sentence + "&resultType=core&synonym=NO&cursorMark=*&pageSize=3&format=json");

                HTTPRestRequest httpRestRequest = new HTTPRestRequest(apiUri.ToString());
                dynamic json = httpRestRequest.MakeRequest();

                if (json != null)
                {
                    foreach (dynamic item in json.resultList.result)
                    {
                        if (item.abstractText != null)
                        {
                            europaPMCSearchResult.AddLast(new EuropaPMCSearchResult((string)item.title, (string)item.authorString, (string)item.abstractText, (string)item.pubYear, (string)item.fullTextUrlList.fullTextUrl[0].url));
                        }
                    }
                }

                documentStatistics.updateEuropaPMCSearchResults(sentence, europaPMCSearchResult);
            }
        }
    }
}
