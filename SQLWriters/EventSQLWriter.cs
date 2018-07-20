using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using PhatACCacheBinParser.SegB_GameEventDefDB;

namespace PhatACCacheBinParser.SQLWriters
{
    static class EventSQLWriter
    {
        public static void WriteFiles(GameEventDefDB gameEventDefDB, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            foreach (var gameEvent in gameEventDefDB.GameEventDefs)
            {
                string FileNameFormatter(GameEventDef obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(gameEvent);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string eventLine = "";

                    eventLine += $"     , ('{gameEvent.Name.Replace("'", "''")}', {(gameEvent.StartTime == -1 ? $"{gameEvent.StartTime}" : $"{gameEvent.StartTime} /* {DateTimeOffset.FromUnixTimeSeconds(gameEvent.StartTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, {(gameEvent.EndTime == -1 ? $"{gameEvent.EndTime}" : $"{gameEvent.EndTime} /* {DateTimeOffset.FromUnixTimeSeconds(gameEvent.EndTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, {(int)gameEvent.GameEventState})" + Environment.NewLine;

                    if (eventLine != "")
                    {
                        eventLine = $"{sqlCommand} INTO `event` (`name`, `start_Time`, `end_Time`, `state`)" + Environment.NewLine
                            + "VALUES " + eventLine.TrimStart("     ,".ToCharArray());
                        eventLine = eventLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(eventLine);
                    }
                }
            }
        }

        public static void WriteFiles(ICollection<ACE.Database.Models.World.Event> input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            foreach (var value in input)
                WriteFile(value, outputFolder, includeDELETEStatementBeforeInsert);
        }

        public static void WriteFile(ACE.Database.Models.World.Event input, string outputFolder, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string fileName = Util.IllegalInFileName.Replace(input.Name, "_");

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
                ExportToSQL(input, writer, includeDELETEStatementBeforeInsert);
        }

        public static void ExportToSQL(ACE.Database.Models.World.Event input, StreamWriter writer, bool includeDELETEStatementBeforeInsert = false)
        {
            if (includeDELETEStatementBeforeInsert)
            {
                writer.WriteLine($"DELETE FROM `event` WHERE `name` = '{input.Name.Replace("'", "''")}';");
                writer.WriteLine();
            }

            writer.WriteLine("INSERT INTO `event` (`name`, `start_Time`, `end_Time`, `state`)");
            writer.WriteLine($"VALUES ('{input.Name.Replace("'", "''")}', {input.StartTime}, {input.EndTime}, {input.State});");
        }
    }
}
