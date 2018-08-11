using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models.TextAPI
{
    //information for a custom google search
    public class GoogleCustomSearch
    {
        public string name { get; private set; }
        public string id { get; private set; }
    
        /// <summary>
        /// constructor for a google custom search
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public GoogleCustomSearch(string name, string id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
