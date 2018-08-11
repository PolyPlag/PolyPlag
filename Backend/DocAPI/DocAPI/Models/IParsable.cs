using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models
{
    interface IParsable<T>
    {
        List<String> Sources { get; set; }
        List<Plagiat<T>> Plagiate { get; set; } 
        List<Uri> URLSources { get; set; }
        Boolean Done { get; }
        Boolean HasPlagiates { get; }
        Boolean Faulty { get; }
        void ParseContent(); 
    }
}
