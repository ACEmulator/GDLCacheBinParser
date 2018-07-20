using System;
using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Seg6_LandBlockExtendedData;

namespace PhatACCacheBinParser.SQLWriters
{
    static class LandblockSQLWriter
    {
        public static void WriteFiles(LandBlockData landBlockData, Dictionary<uint, string> weenieNames, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            uint highestWeenieFound = 0;

            //Parallel.For(0, parsedObjects.Count, i =>
            foreach (var landblock in landBlockData.Landblocks)
            {
                string FileNameFormatter(Landblock obj) => obj.Key.ToString("X4");

                string fileNameFormatter = FileNameFormatter(landblock);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    // var parsed = parsedObjects[i] as Landblock;
                    var parsed = landblock;

                    string instanceLine = "", sourcesLine = "", targetsLine = "";

                    Dictionary<uint, List<uint>> targets = new Dictionary<uint, List<uint>>();

                    Dictionary<uint, string> instanceNames = new Dictionary<uint, string>();

                    if (parsed.Weenies != null)
                    {
                        foreach (var instance in parsed.Weenies)
                        {
                            if (instance.WCID > highestWeenieFound)
                                highestWeenieFound = instance.WCID;

                            // Somebody goofed and a guid was used in two places... I'm not sure that it ultimately was a problem on retail worlds but this fixes it for ACE
                            if (instance.ID == 1975799995)
                            {
                                if (instance.WCID == 22775)
                                    instance.ID = 1975799994; // Unused guid.
                            }

                            //// ACE has a problem currently dealing with objects placed at xxxx0000 of a landblock, this moves any object to xxxx0001 for now
                            //string landblockHex = instance.Position.ObjCellID.ToString("X8");
                            //if (landblockHex.EndsWith("0000"))
                            //{
                            //    landblockHex = landblockHex.Substring(0, 4) + "0001";
                            //    instance.Position.ObjCellID = Convert.ToUInt32(landblockHex, 16);
                            //}

                            instanceLine += $"     , ({instance.WCID}, {instance.ID}, " +
                                $"{instance.Position.ObjCellID}, " +
                                $"{instance.Position.Origin.X}, {instance.Position.Origin.Y}, {instance.Position.Origin.Z}, " +
                                $"{instance.Position.Angles.W}, {instance.Position.Angles.X}, {instance.Position.Angles.Y}, {instance.Position.Angles.Z}" +
                            $") /* {weenieNames[instance.WCID]} */" + Environment.NewLine;

                            if (!instanceNames.ContainsKey(instance.ID))
                                instanceNames.Add(instance.ID, weenieNames[instance.WCID]);
                        }
                    }

                    if (parsed.Links != null)
                    {
                        foreach (var link in parsed.Links)
                        {
                            if (!targets.ContainsKey(link.Target))
                                targets.Add(link.Target, new List<uint>());

                            targets[link.Target].Add(link.Source);
                        }
                    }

                    int slotId = 1;
                    foreach (var link in targets)
                    {
                        targetsLine += $"UPDATE `landblock_instances` SET `link_Slot`='{slotId}', `link_Controller`={true} WHERE `guid`='{link.Key}'; /* {instanceNames[link.Key]} */" + Environment.NewLine; //+

                        foreach (var source in link.Value)
                        {
                            sourcesLine += $"UPDATE `landblock_instances` SET `link_Slot`='{slotId}' WHERE `guid`='{source}'; /* {instanceNames[link.Key]} <- {instanceNames[source]} */" + Environment.NewLine;
                        }

                        slotId++;
                    }

                    if (instanceLine != "")
                    {
                        instanceLine = $"{sqlCommand} INTO `landblock_instances` (`weenie_Class_Id`, `guid`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + instanceLine.TrimStart("     ,".ToCharArray());
                        instanceLine = instanceLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(instanceLine);
                    }

                    if (targetsLine != "")
                    {
                        targetsLine = targetsLine.TrimEnd(Environment.NewLine.ToCharArray()) + "" + Environment.NewLine;
                        writer.WriteLine(targetsLine);
                    }

                    if (sourcesLine != "")
                    {
                        sourcesLine = sourcesLine.TrimEnd(Environment.NewLine.ToCharArray()) + "" + Environment.NewLine;
                        writer.WriteLine(sourcesLine);
                    }
                }
                //});               
            }

            //string fileName = "TerrainData";

            //using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            //{
            //    foreach (var terrainLandblock in LandBlockData.TerrainLandblocks)
            //    {
            //        var parsed = terrainLandblock;

            //        string encounterLine = "";

            //        //        //foreach (var generator in encounter.Values)
            //        //        //{
            //        //        //    weenieNames.TryGetValue(generator, out string label);
            //        //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //        //}

            //        //encounterLine += $"     , ({LandBlockData.TerrainLandblocks.IndexOf(terrain)}, {map.Index})" + $" /* {LandBlockData.TerrainLandblocks.IndexOf(terrain).ToString("X4")} */" + Environment.NewLine;
            //        foreach (var terrain in terrainLandblock.Terrain)
            //        {
            //            encounterLine += $"     , ({LandBlockData.TerrainLandblocks.IndexOf(terrainLandblock)}, {terrain})" + $" /* {LandBlockData.TerrainLandblocks.IndexOf(terrainLandblock).ToString("X4")} */" + Environment.NewLine;
            //        }

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `terrain` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        //        var counter = Interlocked.Increment(ref processedCounter);

            //        //        if ((counter % 1000) == 0)
            //        //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }

            //}

            //parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
            //System.Diagnostics.Debug.WriteLine($"Highest Weenie Exported in WorldSpawn was: {highestWeenieFound}");
        }
    }
}
