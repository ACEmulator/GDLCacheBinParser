using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using PhatACCacheBinParser.Common;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg1_RegionDescExtendedData;
using PhatACCacheBinParser.Seg6_LandBlockExtendedData;

namespace PhatACCacheBinParser.SQLWriters
{
    static class RegionDescSQLWriter
    {
        public static void WriteRegionFiles(RegionDescExtendedData regionDescExtendedData, Dictionary<uint, string> weenieNames)
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "1 RegionDescExtendedData" + "\\" + "\\SQL Old Method\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            foreach (var encounter in regionDescExtendedData.EncounterTables)
            {
                string FileNameFormatter(EncounterTable obj) => obj.Index.ToString("00000");

                string fileNameFormatter = FileNameFormatter(encounter);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string encounterLine = "";

                    foreach (var generator in encounter.Values)
                    {
                        weenieNames.TryGetValue(generator, out string label);
                        encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
                    }

                    if (encounterLine != "")
                    {
                        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`)" + Environment.NewLine
                            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
                        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(encounterLine);
                    }
                }
            }

            //string fileName = "EncounterMaps";

            //using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            //{
            //    foreach (var map in RegionDescExtendedData.EncounterMaps)
            //    {
            //        var parsed = map;

            //        string encounterLine = "";

            //        //        //foreach (var generator in encounter.Values)
            //        //        //{
            //        //        //    weenieNames.TryGetValue(generator, out string label);
            //        //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //        //}

            //        encounterLine += $"     , ({RegionDescExtendedData.EncounterMaps.IndexOf(map)}, {map.Index})" + $" /* {RegionDescExtendedData.EncounterMaps.IndexOf(map).ToString("X4")} */" + Environment.NewLine;

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `encounter_map` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        //        var counter = Interlocked.Increment(ref processedCounter);

            //        //        if ((counter % 1000) == 0)
            //        //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }
            //}

            //foreach (var map in RegionDescExtendedData.EncounterMaps)
            //{
            //    //string FileNameFormatter(EncounterMap obj) => obj.Index.ToString("00000");

            //    //string fileNameFormatter = FileNameFormatter(encounter);

            //    string fileNameFormatter = RegionDescExtendedData.EncounterMaps.IndexOf(map).ToString("X4");

            //    using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
            //    {
            //        var parsed = map;

            //        string encounterLine = "";

            //        //foreach (var generator in encounter.Values)
            //        //{
            //        //    weenieNames.TryGetValue(generator, out string label);
            //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //}

            //        encounterLine += $"     , ({RegionDescExtendedData.EncounterMaps.IndexOf(map)}, {map.Index})" + Environment.NewLine;

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `encounter_map` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        var counter = Interlocked.Increment(ref processedCounter);

            //        if ((counter % 1000) == 0)
            //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }
            //}

            //using (StreamWriter writer = new StreamWriter(outputFolder + "00000" + ".sql"))
            //{
            //    string encounterLine = "";

            //    encounterLine += $"     , (0, 0, '{Convert.ToBase64String(RegionDescExtendedData.EncounterMap)}')" + Environment.NewLine;

            //    if (encounterLine != "")
            //    {
            //        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //        writer.WriteLine(encounterLine);
            //    }
            //}

            //using (StreamWriter writer = new StreamWriter(outputFolder + "00001" + ".sql"))
            //{
            //    //string encounterLine = "";

            //    //encounterLine += $"     , (1, 0, '{Convert.ToBase64String(LandBlockData.TerrainData)}')" + Environment.NewLine;

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //string encounterLine = "";

            //    //encounterLine += $"     , (1, 0)" + Environment.NewLine;

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //var terrainDataStrings = WholeChunks(Convert.ToBase64String(LandBlockData.TerrainData), Convert.ToBase64String(RegionDescExtendedData.EncounterMap).Length);

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine = "";
            //    //    //UPDATE Table SET Field=CONCAT(IFNULL(Field, ''), 'Your extra HTML')
            //    //    //encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //    //    + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    //encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    encounterLine = $"UPDATE `encounter` SET `encounter_Map` = CONCAT(IFNULL(encounter_Map, ''), '{line}') WHERE `index` = 1;" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine += $"     , (1, 0, '{line}')" + Environment.NewLine;
            //    //}

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine = "";
            //    //    encounterLine += $"     , (1, 0, '{line}')" + Environment.NewLine;
            //    //    if (encounterLine != "")
            //    //    {
            //    //        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //        writer.WriteLine(encounterLine);
            //    //    }
            //    //}
            //}
        }

        static IEnumerable<string> WholeChunks(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }

        public static void WriteEnounterLandblockInstances(RegionDescExtendedData regionDescExtendedData, LandBlockData landBlockData, Dictionary<uint, string> weenieNames)
        {
            var encounters = new Dictionary<int, List<Encounter>>();
            
            for (var landblock = 0; landblock < (255 * 255); landblock++)
            {
                var block_x = (landblock & 0xFF00) >> 8;
                var block_y = (landblock & 0x00FF) >> 0;

                //var tbIndex = ((block_x * 255) + block_y) * 9 * 9;
                //var tbIndex = ((block_x * 255) + block_y) * 9;
                var tbIndex = ((block_x * 255) + block_y);

                //if (tlIndex > LandBlockData.TerrainLandblocks.Count)
                //    continue;

                var terrain_base = landBlockData.TerrainLandblocks[tbIndex];

                for (var cell_x = 0; cell_x < 8; cell_x++)
                {
                    for (var cell_y = 0; cell_y < 8; cell_y++)
                    {
                        var terrain = terrain_base.Terrain[(cell_x * 9) + cell_y];

                        int encounterIndex = (terrain >> 7) & 0xF;

                        var encounterMap = regionDescExtendedData.EncounterMaps[(block_x * 255) + block_y];
                        var encounterTable = regionDescExtendedData.EncounterTables.FirstOrDefault(t => t.Index == encounterMap.Index);

                        if (encounterTable == null)
                            continue;

                        var wcid = encounterTable.Values[encounterIndex];

                        // System.Diagnostics.Debug.WriteLine($"landblock = {landblock:X4} | terrain = {terrain} | encounterIndex = {encounterIndex} | encounterTable = {encounterMap.Index} | wcid = {wcid}");

                        if (wcid > 0)
                        {
                            var objCellId = (landblock << 16) | 0;

                            if (!encounters.ContainsKey(landblock))
                                encounters.Add(landblock, new List<Encounter>());

                            encounters[landblock].Add(new Encounter { Landblock = landblock, WeenieClassId = wcid, CellX = cell_x, CellY = cell_y });
                        }
                    }
                }
            }

            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "1 RegionDescExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            foreach (var landblock in encounters)
            {
                string fileNameFormatter = landblock.Key.ToString("X4");

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    var parsed = landblock.Value;

                    string encounterLine = "";

                    foreach (var encounter in parsed)
                    {
                        weenieNames.TryGetValue(encounter.WeenieClassId, out string label);
                        encounterLine += $"     , ({encounter.Landblock}, {encounter.WeenieClassId}, {encounter.CellX}, {encounter.CellY})" + $" /* {label} */" + Environment.NewLine;
                    }

                    if (encounterLine != "")
                    {
                        encounterLine = $"{sqlCommand} INTO `encounter` (`landblock`, `weenie_Class_Id`, `cell_X`, `cell_Y`)" + Environment.NewLine
                            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
                        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(encounterLine);
                    }
                }
            }
        }
    }
}
