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
                if (value.Weenies != null)
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

                        // Somebody goofed and a guid was used in two places... I'm not sure that it ultimately was a problem on retail worlds but this fixes it for ACE
                        if (result.Guid == 1975799995)
                        {
                            if (result.WeenieClassId == 22775)
                                result.Guid = 1975799994; // Unused guid.
                        }

                        results.Add(result);
                    }
                }

                if (value.Links != null)
                {
                   var targets = new Dictionary<uint, HashSet<uint>>();

                    foreach (var link in value.Links)
                    {
                        if (!targets.ContainsKey(link.Target))
                            targets.Add(link.Target, new HashSet<uint>());

                        targets[link.Target].Add(link.Source);
                    }
                    
                    // This is done twice so it matches the previous method used by Ripley.
                    // It's also VERY slow, but, who cares. It works.
                    // TODO This code needs to be reworked when teh parent link is added.

                    int slotId = 1;
                    foreach (var kvp in targets)
                    {
                        foreach (var landblockInstance in results)
                        {
                            if (landblockInstance.Guid == kvp.Key)
                            {
                                landblockInstance.LinkSlot = slotId;
                                landblockInstance.LinkController = true;
                            }
                        }

                        slotId++;
                    }

                    slotId = 1;
                    foreach (var kvp in targets)
                    {
                        foreach (var landblockInstance in results)
                        {
                            if (kvp.Value.Contains(landblockInstance.Guid))
                                landblockInstance.LinkSlot = slotId;
                        }

                        slotId++;
                    }
                }
            }

            return results;
        }
    }
}
