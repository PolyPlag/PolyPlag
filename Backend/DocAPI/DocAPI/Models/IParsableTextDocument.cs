using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models
{
    interface IParsableTextDocument : IParsable<String>
    {
        LinkedList<String> Sentences { get; }
    }
}
