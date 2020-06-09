using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyApiGw.Models
{

    public enum BLBehavior { ByDefaultAllDenied, ByDefaultAllAllowed }
    public class BWItems
    {        
        public IEnumerable<string> SourceIps { get; set; }        
        public BLBehavior AllowedOrDenied { get; set; }
    }
}
