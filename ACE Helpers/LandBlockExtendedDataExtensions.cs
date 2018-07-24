using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class LandBlockExtendedDataExtensions
    {
        public static List<LandblockInstances> ConvertToACE(this Seg6_LandBlockExtendedData.LandBlockData input)
        {
            return input.Landblocks.ConvertToACE();
        }

        public static List<LandblockInstances> ConvertToACE(this List<Seg6_LandBlockExtendedData.Landblock> input)
        {
            var results = new List<LandblockInstances>();

            foreach (var value in input)
            {
                foreach (var weenie in value.Weenies)
                {
                    var result = new LandblockInstances
                    {
                        WeenieClassId = weenie.WCID,

                        Guid = weenie.ID,

                        ObjCellId = weenie.Position.ObjCellID,

                        OriginX = weenie.Position.Origin.X,
                        OriginY = weenie.Position.Origin.Y,
                        OriginZ = weenie.Position.Origin.Z,

                        AnglesX = weenie.Position.Angles.X,
                        AnglesY = weenie.Position.Angles.Y,
                        AnglesZ = weenie.Position.Angles.Z,
                        AnglesW = weenie.Position.Angles.W
                    };

                    results.Add(result);
                }

                // todo links
            }

            return results;
        }
    }
}
