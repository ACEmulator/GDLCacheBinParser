using System;
using System.IO;

using PhatACCacheBinParser.Seg5_HousingPortals;

namespace PhatACCacheBinParser.SQLWriters
{
    static class HouseSQLWriter
    {
        public static void WriteHouseFiles(HousingPortalsTable housingPortalsTable, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string sqlCommand = "INSERT";

            foreach (var house in housingPortalsTable.HousingPortals)
            {
                string FileNameFormatter(HousingPortal obj) => obj.HouseId.ToString("00000");

                string fileNameFormatter = FileNameFormatter(house);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string houseLine = "";

                    foreach (var destination in house.Destinations)
                    {
                        houseLine += $"     , ({house.HouseId}, {destination.ObjCellID}, {destination.Origin.X}, {destination.Origin.Y}, {destination.Origin.Z}, {destination.Angles.W}, {destination.Angles.X}, {destination.Angles.Y}, {destination.Angles.Z})" + Environment.NewLine;
                    }

                    if (houseLine != "")
                    {
                        houseLine = $"{sqlCommand} INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + houseLine.TrimStart("     ,".ToCharArray());
                        houseLine = houseLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(houseLine);
                    }
                }
            }
        }
    }
}
