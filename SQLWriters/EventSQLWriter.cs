using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PhatACCacheBinParser.SQLWriters
{
    static class EventSQLWriter
    {
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
            writer.WriteLine("VALUES (" +
                             $"'{input.Name.Replace("'", "''")}', " +
                             $"{(input.StartTime == -1 ? $"{input.StartTime}" : $"{input.StartTime} /* {DateTimeOffset.FromUnixTimeSeconds(input.StartTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, " +
                             $"{(input.EndTime == -1 ? $"{input.EndTime}" : $"{input.EndTime} /* {DateTimeOffset.FromUnixTimeSeconds(input.EndTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, " +
                             $"{input.State}" +
                             ");");
        }
    }
}
