using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SuperRestrictor
{
    public class RestrictedVehicle
    {
        [XmlText]
        public ushort VehicleId { get; set; }
        [XmlAttribute("IgnorePermission")]
        public string Bypass { get; set; }
        [XmlAttribute("Message")]
        public string Message { get; set; }

        public RestrictedVehicle() { }

    }
}