using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    public class GoogleSearch 
    {
        public Uri API { get; set; }
        private const string apikey = "<placekey>";

        //custom google search (google and wikipedia)
        private static readonly GoogleCustomSearch[] customSearchs = {new GoogleCustomSearch("GoogleSearch", "017596910834659269996:ha07esn-p_y"),
                                                                      new GoogleCustomSearch("Wikipedia","017596910834659269996:bxqb12o3kro") };

        /// <summary>
        /// basic constructor
        /// </summary>
        /// <param name="API"></param>
        public GoogleSearch(Uri API)
        {
            this.API = API;            
        }

        /// <summary>
        /// query all google custom searches for a document statistic
        /// </summary>
        /// <param name="documentStatistics"></param>
        public void Check(DocumentStatistics documentStatistics)
        {
            foreach (string sentence in documentStatistics.getSentences())
            {
                LinkedList<GoogleSearchResult> googleSearchResults = new LinkedList<GoogleSearchResult>();

                foreach (GoogleCustomSearch customSearch in customSearchs)
                {
                    Uri apiUri = new Uri(API + "?key=" + apikey + "&cx=" + customSearch.id + "&q=" + sentence);
                    HTTPRestRequest httpRestRequest = new HTTPRestRequest(apiUri.ToString());
                    dynamic json = httpRestRequest.MakeRequest();

                    //no results
                    if (json != null && json.items != null)
                    {
                        int count = 0; 
                        foreach (dynamic item in json.items)
                        {
                            //A bit ugly but we need it because its the easiest way too break 
                            // out of the loop.
                            if(count >= 3)
                            {
                                break;
                            }

                            googleSearchResults.AddLast(new GoogleSearchResult(customSearch.name, (string)item.title, (string)item.link, (string)item.snippet));
                            count++;
                        }
                    }
                }

                documentStatistics.updateGoogleSearchResults(sentence, googleSearchResults);
            }
        }
    }
}
