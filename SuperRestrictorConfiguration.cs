using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SuperRestrictor
{
    public class SuperRestrictorConfiguration : IRocketPluginConfiguration
    {
        [XmlArrayItem(ElementName = "Item")]
        public List<RestrictedItem> RestrictedItems;

        [XmlArrayItem(ElementName = "Vehicle")]
        public List<RestrictedVehicle> RestrictedVehicles;

        [XmlArrayItem(ElementName = "player")]
        public List<RestrictedName> RestrictedNames;

        [XmlArrayItem(ElementName = "word")]
        public List<RestrictedWord> RestrictedWords;

        public bool IgnoreAdmins;

        public void LoadDefaults()
        {
            RestrictedNames = new List<RestrictedName>
            {
                new RestrictedName { Message = "Message1", Bypass = "bypass.permission1", name = "Name1"},
                new RestrictedName { Bypass = "bypass.permission2", name = "Name2"},
                new RestrictedName { name = "Name3" }
            };

            RestrictedWords = new List<RestrictedWord>
            {
                new RestrictedWord { Message = "Message2", Bypass = "bypass.permission3", name = "Word1"},
                new RestrictedWord { Bypass = "bypass.permission4", name = "Word2"},
                new RestrictedWord { name = "Word3" }
            };

            RestrictedItems = new List<RestrictedItem>
            {
                new RestrictedItem { Message = "Message3", Bypass = "bypass.permission5", Id = 111 }, 
                new RestrictedItem { Bypass = "bypass.permission6", Id = 222 },
                new RestrictedItem { Id = 333 }
            };

            RestrictedVehicles = new List<RestrictedVehicle>
            {
                new RestrictedVehicle { Message = "Message4", Bypass = "bypass.permission7", VehicleId = 444 },
                new RestrictedVehicle { Bypass = "bypass.permission8", VehicleId = 555 },
                new RestrictedVehicle { VehicleId = 666 },
            };
            IgnoreAdmins = true;
        }
    }
}
