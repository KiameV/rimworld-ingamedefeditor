using System.Collections.Generic;
using System.Xml.Serialization;

namespace InGameDefEditor
{
    [XmlRoot("CES", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
    public class AllStats
    {
        public List<Stats> apparelWeaponStats = new List<Stats>();
        public List<ProjectileStats> projectileStats = new List<ProjectileStats>();
    }
}
