using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using PhatACCacheBinParser.Seg6_LandBlockExtendedData;

namespace PhatACCacheBinParser.SQLWriters
{
    static class LandblockSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.LandblockInstances> input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Sort the input by landblock
            var sortedInput = new Dictionary<uint, List<ACE.Database.Models.World.LandblockInstances>>();

            foreach (var value in input)
            {
                var landblock = (value.ObjCellId >> 16);

                if (sortedInput.TryGetValue(landblock, out var list))
                    list.Add(value);
                else
                    sortedInput.Add(landblock, new List<ACE.Database.Models.World.LandblockInstances> { value });
            }

            var sqlWriter = new ACE.Database.SQLFormatters.World.LandblockInstancesWriter();

            sqlWriter.WeenieNames = weenieNames;

            Parallel.ForEach(sortedInput, kvp =>
            //foreach (var kvp in sortedInput)
            {
                string fileName = sqlWriter.GetDefaultFileName(kvp.Value[0]);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileName))
                {
                    if (includeDELETEStatementBeforeInsert)
                    {
                        sqlWriter.CreateSQLDELETEStatement(kvp.Value, writer);
                        writer.WriteLine();
                    }

                    sqlWriter.CreateSQLINSERTStatement(kvp.Value, writer);
                }
            });
        }

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
        }
    }
}
