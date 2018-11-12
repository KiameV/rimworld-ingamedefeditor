using InGameDefEditor.Stats;
using RimWorld;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor
{
    static class IOUtil
    {
        private static bool hasLoaded = false;

        public static void LoadData()
        {
            Defs.Initialize();
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
                    catch (Exception e)
                    {
                        Log.Error(e.GetType().Name + Environment.NewLine + e.Message);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }

                    //Log.Error("ThingDefs: " + allStats.thingDefStats.Count);
                    //Log.Error("ProjectileDefs: " + allStats.projectileStats.Count);

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

                    if (allStats.thingDefStats != null)
                    {
                        foreach (ThingDefStats s in allStats.thingDefStats)
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
                        allStats.thingDefStats.Clear();
                    }

                    if (allStats.biomeStats != null)
                    {
                        foreach (BiomeStats s in allStats.biomeStats)
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
                        allStats.thingDefStats.Clear();
                    }
                }
                catch
                {
                    // Ignore
                }
                finally
                {
                    hasLoaded = true;
                    Util.ClearDefDic();
                }
            }
        }

        public static void SaveData()
        {
            AllStats allStats = new AllStats();
            foreach (ThingDef d in Defs.ApparelDefs.Values)
            {
                ThingDefStats s = new ThingDefStats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.thingDefStats.Add(s);
                }
            }

            foreach (ThingDef d in Defs.WeaponDefs.Values)
            {
                ThingDefStats s = new ThingDefStats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.thingDefStats.Add(s);
                }
            }

            foreach (ThingDef d in Defs.ProjectileDefs.Values)
            {
                ProjectileStats s = new ProjectileStats(d);
                if (Backup.HasChanged(s))
                {
                    allStats.projectileStats.Add(s);
                }
            }

            foreach (BiomeDef d in Defs.BiomeDefs.Values)
            {
                BiomeStats s = new BiomeStats(d);
                if (Backup.HasChanged(s))
                {
                    //allStats.biomeStats.Add(s);
                }
            }

            //Log.Error("ThingDefs: " + allStats.thingDefStats.Count);
            //Log.Error("ProjectileDefs: " + allStats.projectileStats.Count);

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
