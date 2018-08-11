using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DocAPI.Models
{
    public class DocumentContext 
    {
        

        public DocumentContext()
        {
            
            randGen = new Random();
            Documents = new ConcurrentDictionary<int, Document>();
        }
        private Random randGen;
        public ConcurrentDictionary<int,Document> Documents { get; set; }

        /// <summary>
        /// Generates a new ID for the Document. Which is used too identify it 
        /// over the REST API
        /// </summary>
        /// <returns>int ID</returns>
        public int GenerateId()
        {
            //Just pseudorandom! But i dont think its an security issue. Cuz no permanent data
            var id = randGen.Next(1, 100000000);

            //Makes sure the id is unique
            while(Documents.ContainsKey(id))
            {
                id = randGen.Next(1, 100000000);
            }
            return id;
        }
    }
}
