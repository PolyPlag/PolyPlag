using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

namespace DocAPI.Models.TextAPI
{
    /// <summary>
    /// constructor for the search results of google
    /// </summary>
    public class HTTPRestRequest
    {
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }

        /// <summary>
        /// basic constructor
        /// </summary>
        public HTTPRestRequest()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }

        /// <summary>
        /// constructor defining the endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        public HTTPRestRequest(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }

        /// <summary>
        /// constructor defining the endpoint and the method
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        public HTTPRestRequest(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = "";
        }

        /// <summary>
        /// constructor defining the endpoint, method and the postdata
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        public HTTPRestRequest(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
        }

        /// <summary>
        /// execute the request
        /// </summary>
        /// <returns></returns>
        public dynamic MakeRequest()
        {
            return MakeRequest("");
        }

        /// <summary>
        /// execute the request passing parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public dynamic MakeRequest(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            //if postdata available and http request post
            if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
            {
                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                request.ContentLength = bytes.Length;

                //using ensures the correct disposal of limited ressources
                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                        return null;
                    }

                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }

                    return JsonConvert.DeserializeObject(responseValue);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}