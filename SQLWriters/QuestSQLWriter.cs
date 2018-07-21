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

            string fileName = input.Name;
            fileName = Util.IllegalInFileName.Replace(fileName, "_");

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            {
                if (includeDELETEStatementBeforeInsert)
                {
                    CreateSQLDELETEStatement(input, writer);
                    writer.WriteLine();
                }

                CreateSQLINSERTStatement(input, writer);
            }
        }

        public static void CreateSQLDELETEStatement(ACE.Database.Models.World.Quest input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `quest` WHERE `name` = '{input.Name.Replace("'", "''")}';");
            writer.WriteLine();
        }

        public static void CreateSQLINSERTStatement(ACE.Database.Models.World.Quest input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`)");
            writer.WriteLine("VALUES (" +
                             $"'{input.Name.Replace("'", "''")}', " +
                             $"{input.MinDelta}, " +
                             $"{input.MaxSolves}, " +
                             $"'{input.Message.Replace("'", "''")}'" +
                             ");");
        }
    }
}
