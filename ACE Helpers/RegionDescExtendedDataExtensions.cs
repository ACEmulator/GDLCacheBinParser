using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class RegionDescExtendedDataExtensions
    {
        public static List<Encounter> ConvertToACE(this Seg1_RegionDescExtendedData.RegionDescExtendedData input, Seg6_LandBlockExtendedData.LandBlockData landBlockData)
        {
            var encounters = new Dictionary<int, List<Encounter>>();

            for (var landblock = 0; landblock < (255 * 255); landblock++)
            {
                var block_x = (landblock & 0xFF00) >> 8;
                var block_y = (landblock & 0x00FF) >> 0;

                //var tbIndex = ((block_x * 255) + block_y) * 9 * 9;
                //var tbIndex = ((block_x * 255) + block_y) * 9;
                var tbIndex = ((block_x * 255) + block_y);

                //if (tlIndex > LandBlockData.TerrainLandblocks.Count)
                //    continue;

                var terrain_base = landBlockData.TerrainLandblocks[tbIndex];

                for (var cell_x = 0; cell_x < 8; cell_x++)
                {
                    for (var cell_y = 0; cell_y < 8; cell_y++)
                    {
                        var terrain = terrain_base.Terrain[(cell_x * 9) + cell_y];

                        int encounterIndex = (terrain >> 7) & 0xF;

                        var encounterMap = input.EncounterMaps[(block_x * 255) + block_y];
                        var encounterTable = input.EncounterTables.FirstOrDefault(t => t.Index == encounterMap.Index);

                        if (encounterTable == null)
                            continue;

                        var wcid = encounterTable.Values[encounterIndex];

                        // System.Diagnostics.Debug.WriteLine($"landblock = {landblock:X4} | terrain = {terrain} | encounterIndex = {encounterIndex} | encounterTable = {encounterMap.Index} | wcid = {wcid}");

                        if (wcid > 0)
                        {
                            //var objCellId = (landblock << 16) | 0;

                            if (!encounters.ContainsKey(landblock))
                                encounters.Add(landblock, new List<Encounter>());

                            encounters[landblock].Add(new Encounter { Landblock = landblock, WeenieClassId = wcid, CellX = cell_x, CellY = cell_y });
                        }
                    }
                }
            }


            var results = new List<Encounter>();

            foreach (var kvp in encounters)
            {
                foreach (var value in kvp.Value)
                {
                    results.Add(new Encounter
                    {
                        Landblock = value.Landblock,
                        WeenieClassId = value.WeenieClassId,
                        CellX = value.CellX,
                        CellY = value.CellY,
                        LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
                    });
                }
            }

            return results;
        }
    }
}
