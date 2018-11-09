using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor
{
    static class Equipment
    {
        public static readonly SortedDictionary<string, ThingDef> ApparelDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> WeaponDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> ProjectileDefs = new SortedDictionary<string, ThingDef>();

        private static bool isInit = false;
        private static void Init()
        {
            if (!isInit)
            {
                foreach (ThingDef d in DefDatabase<ThingDef>.AllDefs)
                {
                    if (d.IsApparel)
                    {
                        ApparelDefs[d.label] = d;
                    }
                    if (d.IsWeapon)
                    {
                        WeaponDefs[d.label] = d;

                        if (d.IsWeaponUsingProjectiles && d.Verbs != null)
                        {
                            foreach (VerbProperties v in d.Verbs)
                            {
                                if (v.defaultProjectile != null)
                                {
                                    ProjectileDefs[v.defaultProjectile.label] = v.defaultProjectile;
                                }
                            }
                        }
                    }
                    if (d.defName.StartsWith("Arrow_") || 
                        d.defName.StartsWith("Bullet_") || 
                        d.defName.StartsWith("Proj_"))
                    {
                        ProjectileDefs[d.label] = d;
                    }
                }

                if (ApparelDefs.Count > 0)
                {
                    Backup.Init(ApparelDefs.Values, WeaponDefs.Values);
                    
                    isInit = true;
                }
            }
        }

        private static bool hasLoaded = false;
        public static void LoadData()
        {
            Init();
            if (!hasLoaded)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AllStats));

                    string path = GetStatsPath();
                    if (!File.Exists(path))
                    {
                        Log.Message("No Change Equipment Stats file found.");
                        return;
                    }

                    FileStream fs = null;
                    AllStats allStats = null;
                    try
                    {
                        fs = new FileStream(path, FileMode.Open);
                        allStats = (AllStats)serializer.Deserialize(fs);
                    }
                    catch(Exception e)
                    {
                        Log.Error(e.GetType().Name + Environment.NewLine + e.Message);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                    
                    if (allStats.projectileStats != null)
                    {
                        foreach (ProjectileStats s in allStats.projectileStats)
                        {
                            if (s.Initialize())
                            {
                                s.ApplyStats(s.Def);
                            }
                            else
                            {
                                Log.Warning("Unable to apply settings to " + s.defName);
                            }
                        }
                        allStats.projectileStats.Clear();
                    }
                    else
                    {
                        Log.Warning("No Projectiles");
                    }

                    if (allStats.apparelWeaponStats != null)
                    {
                        foreach (Stats s in allStats.apparelWeaponStats)
                        {
                            if (s.Initialize())
                            {
                                s.ApplyStats(s.Def);
                            }
                            else
                            {
                                Log.Warning("Unable to apply settings to " + s.defName);
                            }
                        }
                        allStats.apparelWeaponStats.Clear();
                    }
                    else
                    {
                        Log.Warning("No Apparel/Weapons");
                    }
                }
                catch
                {
                    // Ignore
                }
                finally
                {
                    hasLoaded = true;
                }
            }
        }

        internal static void SaveData()
        {
            AllStats allStats = new AllStats();
            foreach (ThingDef d in ApparelDefs.Values)
            {
                Stats s = new Stats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.apparelWeaponStats.Add(s);
                }
            }

            foreach (ThingDef d in WeaponDefs.Values)
            {
                Stats s = new Stats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.apparelWeaponStats.Add(s);
                }
            }

            foreach (ThingDef d in ProjectileDefs.Values)
            {
                ProjectileStats s = new ProjectileStats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.projectileStats.Add(s);
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(AllStats));
            FileStream fs = null;
            try
            {
                fs = new FileStream(GetStatsPath(), FileMode.Create);
                serializer.Serialize(fs, allStats);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private static string GetStatsPath()
        {
            string path = (string)typeof(GenFilePaths).GetMethod("FolderUnderSaveData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
            {
                "InGameDefEditor"
            });
            return path + "/stats.xml";
        }
    }
}