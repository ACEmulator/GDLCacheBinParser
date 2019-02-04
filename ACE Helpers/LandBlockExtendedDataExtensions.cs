using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class LandBlockExtendedDataExtensions
    {
        public static List<LandblockInstance> ConvertToACE(this Seg6_LandBlockExtendedData.LandBlockData input)
        {
            return input.Landblocks.ConvertToACE();
        }

        public static List<LandblockInstance> ConvertToACE(this List<Seg6_LandBlockExtendedData.Landblock> input)
        {
            var results = new List<LandblockInstance>();

            foreach (var value in input)
            {
                if (value.Weenies != null)
                {
                    foreach (var weenie in value.Weenies)
                    {
                        var result = new LandblockInstance
                        {
                            Guid = weenie.ID,

                            WeenieClassId = weenie.WCID,

                            ObjCellId = weenie.Position.ObjCellID,

                            OriginX = weenie.Position.Origin.X,
                            OriginY = weenie.Position.Origin.Y,
                            OriginZ = weenie.Position.Origin.Z,

                            AnglesW = weenie.Position.Angles.W,
                            AnglesX = weenie.Position.Angles.X,
                            AnglesY = weenie.Position.Angles.Y,
                            AnglesZ = weenie.Position.Angles.Z,
                            LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
                        };

                        // Somebody goofed and a guid was used in two places... I'm not sure that it ultimately was a problem on retail worlds but this fixes it for ACE
                        if (result.Guid == 1975799995)
                        {
                            if (result.WeenieClassId == 22775)
                                result.Guid = 1975799994; // Unused guid.
                        }

                        if (value.Links != null)
                        {
                            foreach (var link in value.Links)
                            {
                                if (result.Guid == link.Source)
                                    result.IsLinkChild = true;

                                if (result.Guid == link.Target)
                                {
                                    // This code prevents duplicates. There are a few duplicate entries in the cache.bin
                                    bool found = false;

                                    foreach (var record in result.LandblockInstanceLink)
                                    {
                                        if (record.ChildGuid == link.Source)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (!found)
                                    {
                                        result.LandblockInstanceLink.Add(new LandblockInstanceLink
                                        {
                                            ParentGuid = result.Guid,
                                            ChildGuid = link.Source,
                                            LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
                                        });
                                    }
                                }
                            }
                        }

                        results.Add(result);
                    }
                }
            }

            return results;
        }
    }
}
