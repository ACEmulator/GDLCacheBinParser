using System;
using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SQLWriters
{
    static class HouseSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.HousePortal> input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Sort the input by house id
            var sortedInput = new Dictionary<uint, List<ACE.Database.Models.World.HousePortal>>();

            foreach (var value in input)
            {
                if (sortedInput.TryGetValue(value.HouseId, out var list))
                    list.Add(value);
                else
                    sortedInput.Add(value.HouseId, new List<ACE.Database.Models.World.HousePortal> { value });
            }

            foreach (var kvp in sortedInput)
            {
                string fileName = kvp.Key.ToString("00000");
                fileName = Util.IllegalInFileName.Replace(fileName, "_");

                using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
                {
                    if (includeDELETEStatementBeforeInsert)
                    {
                        CreateSQLDELETEStatement(kvp.Value, writer);
                        writer.WriteLine();
                    }

                    CreateSQLINSERTStatement(kvp.Value, writer);
                }
            }
        }

        public static void CreateSQLDELETEStatement(IList<ACE.Database.Models.World.HousePortal> input, StreamWriter writer)
        {
            throw new NotImplementedException();
        }

        public static void CreateSQLINSERTStatement(IList<ACE.Database.Models.World.HousePortal> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{input[i].HouseId}, " +
                          $"{input[i].ObjCellId}, " +
                          $"{input[i].OriginX}, " +
                          $"{input[i].OriginY}, " +
                          $"{input[i].OriginZ}, " +
                          $"{input[i].AnglesW}, " +
                          $"{input[i].AnglesX}, " +
                          $"{input[i].AnglesY}, " +
                          $"{input[i].AnglesZ})";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }
    }
}
