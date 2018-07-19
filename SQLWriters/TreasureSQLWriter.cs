using System;
using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg3_TreasureTable;

namespace PhatACCacheBinParser.SQLWriters
{
    static class TreasureSQLWriter
    {
        public static void WriteTreasureFiles(TreasureTable treasureTable, Dictionary<uint, string> weenieNames)
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "3 TreasureTable" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            var subFolder = "\\Wielded\\";
            if (!Directory.Exists(outputFolder + subFolder))
                Directory.CreateDirectory(outputFolder + subFolder);

            foreach (var entry in treasureTable.WieldedTreasure)
            {
                //string FileNameFormatter(TreasureEntry obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                //string fileNameFormatter = FileNameFormatter(entry.Value);

                string fileNameFormatter = entry.Key.ToString("00000");

                using (StreamWriter writer = new StreamWriter(outputFolder + subFolder + fileNameFormatter + ".sql"))
                {
                    string entryLine = "";

                    foreach (var treasure in entry.Value)
                    {
                        //(`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)
                        entryLine += $"     , ({entry.Key}, {treasure.WCID} /* {weenieNames[treasure.WCID]} */, {treasure.PTID}, {treasure.m_08_AlwaysZero}, {treasure.Shade}, {treasure.Amount}, {treasure.m_f14}, {treasure.Chance}, {treasure.m_1C_AlwaysZero}, {treasure.m_20_AlwaysZero}, {treasure.m_24_AlwaysZero}, {treasure.m_b28}, {treasure.m_b2C}, {treasure.m_b30}, {treasure.m_34_AlwaysZero}, {treasure.m_38_AlwaysZero}, {treasure.m_3C_AlwaysZero}, {treasure.m_40_AlwaysZero})" + Environment.NewLine;
                    }

                    if (entryLine != "")
                    {
                        //(`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)
                        entryLine = $"{sqlCommand} INTO `treasure_wielded` (`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)" + Environment.NewLine
                            + "VALUES " + entryLine.TrimStart("     ,".ToCharArray());
                        entryLine = entryLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(entryLine);
                    }
                }
            }

            subFolder = "\\Death\\";
            if (!Directory.Exists(outputFolder + subFolder))
                Directory.CreateDirectory(outputFolder + subFolder);

            foreach (var entry in treasureTable.DeathTreasure)
            {
                //string FileNameFormatter(TreasureEntry obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                //string fileNameFormatter = FileNameFormatter(entry.Value);

                string fileNameFormatter = entry.Key.ToString("00000");

                using (StreamWriter writer = new StreamWriter(outputFolder + subFolder + fileNameFormatter + ".sql"))
                {
                    string entryLine = "";

                    //foreach (var treasure in entry.Value)
                    //{
                    //(`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`)
                    entryLine += $"     , ({entry.Key}, {entry.Value.Tier}, {entry.Value.m_f04}, {entry.Value.m_08}, {entry.Value.m_0C}, {entry.Value.m_10}, {entry.Value.m_14}, {entry.Value.m_18}, {entry.Value.m_1C}, {entry.Value.m_20}, {entry.Value.m_24}, {entry.Value.m_28}, {entry.Value.m_2C}, {entry.Value.m_30}, {entry.Value.m_34}, {entry.Value.m_38})" + Environment.NewLine;
                    //}

                    if (entryLine != "")
                    {
                        //(`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`)
                        entryLine = $"{sqlCommand} INTO `treasure_death` (`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`, `unknown_14`)" + Environment.NewLine
                            + "VALUES " + entryLine.TrimStart("     ,".ToCharArray());
                        entryLine = entryLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(entryLine);
                    }
                }
            }
        }
    }
}
