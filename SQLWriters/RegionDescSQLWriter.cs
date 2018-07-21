using System;
using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SQLWriters
{
    static class RegionDescSQLWriter
    {
        public static void WriteFiles(ICollection<ACE.Database.Models.World.Encounter> input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Sort the input by landblock
            var sortedInput = new Dictionary<int, List<ACE.Database.Models.World.Encounter>>();

            foreach (var value in input)
            {
                if (sortedInput.TryGetValue(value.Landblock, out var list))
                    list.Add(value);
                else
                    sortedInput.Add(value.Landblock, new List<ACE.Database.Models.World.Encounter> { value });
            }

            foreach (var kvp in sortedInput)
            {
                string fileName = Util.IllegalInFileName.Replace(kvp.Key.ToString("X4"), "_");

                using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
                {
                    if (includeDELETEStatementBeforeInsert)
                    {
                        CreateSQLDELETEStatement(kvp.Value, writer, weenieNames);
                        writer.WriteLine();
                    }

                    CreateSQLINSERTStatement(kvp.Value, writer, weenieNames);
                }
            }
        }

        public static void CreateSQLDELETEStatement(IList<ACE.Database.Models.World.Encounter> input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            throw new NotImplementedException();
        }

        public static void CreateSQLINSERTStatement(IList<ACE.Database.Models.World.Encounter> input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            writer.WriteLine("INSERT INTO `encounter` (`landblock`, `weenie_Class_Id`, `cell_X`, `cell_Y`)");

            for (int i = 0; i < input.Count; i++)
            {
                var output = "";

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                weenieNames.TryGetValue(input[i].WeenieClassId, out string label);

                output += $"{input[i].Landblock}, " +
                          $"{input[i].WeenieClassId}, " +
                          $"{input[i].CellX}, " +
                          $"{input[i].CellY})" + $" /* {label} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }
    }
}
