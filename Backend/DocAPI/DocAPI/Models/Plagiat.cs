using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models
{
    public class Plagiat<T>
    {
        public T Origin { get; private set; }
        public T FoundPlag { get; private set; }
        public String Source { get; private set; }

        public T BeforeOrigin { get; set; }
        public T AfterOrigin { get; set;  }
        public Plagiat(T text, T foundPlag, String source){
            this.Origin = text;
            this.FoundPlag = foundPlag;
            this.Source = source;
        }

           
    }
}
