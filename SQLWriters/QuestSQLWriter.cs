using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SQLWriters
{
    static class QuestSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.Quest> input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            foreach (var value in input)
                WriteFile(value, outputFolder, includeDELETEStatementBeforeInsert);
        }

        public static void WriteFile(ACE.Database.Models.World.Quest input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.QuestSQLWriter();

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
    }
}
