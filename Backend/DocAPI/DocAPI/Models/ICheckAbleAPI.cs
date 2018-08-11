using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocAPI.Models
{
    interface ICheckAbleAPI<T>
    {
        Uri API { get; set; }

        List<Plagiat<T>> Check(List<T> origin, List<Uri> URLsources, List<String> sources);
    }
}
