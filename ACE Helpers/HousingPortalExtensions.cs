using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class HousingPortalExtensions
    {
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

                        AnglesX = destination.Angles.X,
                        AnglesY = destination.Angles.Y,
                        AnglesZ = destination.Angles.Z,
                        AnglesW = destination.Angles.W
                    };

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
