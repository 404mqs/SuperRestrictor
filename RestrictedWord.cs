using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SuperRestrictor
{
    public class RestrictedWord
    {
    
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("BypassPermission")]
        public string Bypass { get; set; }
        [XmlAttribute("Message")]
        public string Message { get; set; }

        public RestrictedWord() { }
   
    }
}
