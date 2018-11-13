using InGameDefEditor.Stats.Misc;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace InGameDefEditor.Stats
{
    [XmlRoot("CES", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
    public class AllStats
    {
        public List<ThingDefStats> thingDefStats = new List<ThingDefStats>();
        public List<ProjectileDefStats> projectileStats = new List<ProjectileDefStats>();
        public List<BiomeDefStats> biomeStats = new List<BiomeDefStats>();
    }
}
