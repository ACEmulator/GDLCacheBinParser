using System;
using System.IO;

using PhatACCacheBinParser.Seg8_QuestDefDB;

namespace PhatACCacheBinParser.SQLWriters
{
    static class QuestSQLWriter
    {
        public static void WriteFiles(QuestDefDB questDefDB, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            foreach (var quest in questDefDB.QuestDefs)
            {
                string FileNameFormatter(QuestDef obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(quest);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    // `name`, `min_Delta`, `max_Solves`, `message`

                    var questLineHdr = $"{sqlCommand} INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`";
                    var questLine = $"('{quest.Name.Replace("'", "''")}', {quest.MinDelta}, {quest.MaxSolves}, '{quest.Message.Replace("'", "''")}'";

                    questLineHdr += ")" + Environment.NewLine + "VALUES ";
                    questLine += ");";

                    if (questLine != "")
                    {
                        writer.WriteLine(questLineHdr + questLine);
                    }
                }
            }
        }
    }
}
