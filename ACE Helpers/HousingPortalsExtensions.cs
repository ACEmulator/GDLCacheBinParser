using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class HousingPortalsExtensions
    {
        public static List<HousePortal> ConvertToACE(this Seg5_HousingPortals.HousingPortalsTable input)
        {
            return input.HousingPortals.ConvertToACE();
        }

        public static List<HousePortal> ConvertToACE(this List<Seg5_HousingPortals.HousingPortal> input)
        {
            var results = new List<HousePortal>();

            foreach (var value in input)
            {
                foreach (var destination in value.Destinations)
                {
                    var result = new HousePortal
                    {
                        HouseId = value.HouseId,

                        ObjCellId = destination.ObjCellID,

                        OriginX = destination.Origin.X,
                        OriginY = destination.Origin.Y,
                        OriginZ = destination.Origin.Z,

                        AnglesW = destination.Angles.W,
                        AnglesX = destination.Angles.X,
                        AnglesY = destination.Angles.Y,
                        AnglesZ = destination.Angles.Z,
                        LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
                    };

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
