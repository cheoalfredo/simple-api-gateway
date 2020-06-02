using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyApiGw.Models
{

    public enum ListType { ByDefaultAllDenied, ByDefaultAllAllowed }
    public class BWItems
    {
        public IEnumerable<string> SourceIps { get; set; }
        public ListType AllowedOrDenied { get; set; }
    }
}
