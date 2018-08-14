using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.SQLWriters
{
    static class WeenieSQLWriter
    {
        public static void WriteFiles(ICollection<ACE.Database.Models.World.Weenie> input, string outputFolder, Dictionary<uint, string> weenieNames, Dictionary<uint, List<ACE.Database.Models.World.TreasureWielded>> wieldedTreasure = null, Dictionary<uint, ACE.Database.Models.World.TreasureDeath> deathTreasure = null, Dictionary<uint, ACE.Database.Models.World.Weenie> weenies = null, bool includeDELETEStatementBeforeInsert = false)
        {
            var sqlWriter = new ACE.Database.SQLFormatters.World.WeenieSQLWriter();

            Parallel.ForEach(input, value =>
            //foreach (var value in input)
            {
                // Adjust the output folder based on the weenie type, creature type and item type
                var subFolder = sqlWriter.GetDefaultSubfolder(value);

                WriteFile(value, outputFolder + subFolder, weenieNames, wieldedTreasure, deathTreasure, weenies, includeDELETEStatementBeforeInsert);
            });
        }

        public static void WriteFile(ACE.Database.Models.World.Weenie input, string outputFolder, Dictionary<uint, string> weenieNames, Dictionary<uint, List<ACE.Database.Models.World.TreasureWielded>> wieldedTreasure = null, Dictionary<uint, ACE.Database.Models.World.TreasureDeath> deathTreasure = null, Dictionary<uint, ACE.Database.Models.World.Weenie> weenies = null, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.WeenieSQLWriter();

            sqlWriter.WeenieClassNames = WeenieClassNames.Values;
            sqlWriter.WeenieNames = weenieNames;
            sqlWriter.SpellNames = SpellNames.Values;
            sqlWriter.PacketOpCodes = PacketOpCodeNames.Values;

            sqlWriter.TreasureWielded = wieldedTreasure;
            sqlWriter.TreasureDeath = deathTreasure;

            sqlWriter.Weenies = weenies;

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
