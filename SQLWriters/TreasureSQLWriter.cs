using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SQLWriters
{
    static class TreasureSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.TreasureDeath> input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            foreach (var value in input)
                WriteFile(value, outputFolder, includeDELETEStatementBeforeInsert);
        }

        public static void WriteFile(ACE.Database.Models.World.TreasureDeath input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.TreasureDeathSQLWriter();

            string fileName = sqlWriter.GetDefaultFileName(input);

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName))
            {
                if (includeDELETEStatementBeforeInsert)
                {
                    sqlWriter.CreateSQLDELETEStatement(input, writer);
                    writer.WriteLine();
                }

                sqlWriter.CreateSQLINSERTStatement(input, writer);
            }
        }


        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.TreasureWielded> input, string outputFolder, IDictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            // Sort the input by TreasureType
            var sortedInput = new Dictionary<uint, List<ACE.Database.Models.World.TreasureWielded>>();

            foreach (var value in input)
            {
                if (!sortedInput.ContainsKey(value.TreasureType))
                    sortedInput.Add(value.TreasureType, new List<ACE.Database.Models.World.TreasureWielded>());

                sortedInput[value.TreasureType].Add(value);
            }


            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.TreasureWieldedSQLWriter();

            sqlWriter.WeenieNames = weenieNames;

            foreach (var value in sortedInput)
            {
                string fileName = sqlWriter.GetDefaultFileName(value.Value[0]);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileName))
                {
                    if (includeDELETEStatementBeforeInsert)
                    {
                        sqlWriter.CreateSQLDELETEStatement(value.Value, writer);
                        writer.WriteLine();
                    }

                    sqlWriter.CreateSQLINSERTStatement(value.Value, writer);
                }
            }
        }
    }
}
