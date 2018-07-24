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

            var sqlWriter = new ACE.Database.SQLFormatters.World.HousePortalSQLWriter();

            foreach (var kvp in sortedInput)
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
            }
        }
    }
}
