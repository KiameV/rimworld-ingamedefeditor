using InGameDefEditor.Stats.Misc;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace InGameDefEditor.Stats
{
    [XmlRoot("CES", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
    public class AllStats
    {
        public List<ThingDefStats> thingDefStats = new List<ThingDefStats>();
        public List<ProjectileStats> projectileStats = new List<ProjectileStats>();
        public List<BiomeStats> biomeStats = new List<BiomeStats>();
    }
}
