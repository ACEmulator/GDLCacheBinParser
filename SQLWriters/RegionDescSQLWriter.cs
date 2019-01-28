using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PhatACCacheBinParser.SQLWriters
{
    static class RegionDescSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.Encounter> input, string outputFolder, IDictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
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

            var sqlWriter = new ACE.Database.SQLFormatters.World.EncounterSQLWriter();

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
    }
}
