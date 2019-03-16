using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class TreasureTableExtensions
    {
        public static List<TreasureDeath> ConvertToACE(this Dictionary<uint, Seg3_TreasureTable.DeathTreasure> input)
        {
            var results = new List<TreasureDeath>();

            foreach (var kvp in input)
            {
                var converted = kvp.Value.ConvertToACE(kvp.Key);

                results.Add(converted);
            }

            return results;
        }

        public static TreasureDeath ConvertToACE(this Seg3_TreasureTable.DeathTreasure input, uint treasureType)
        {
            var result = new TreasureDeath
            {
                TreasureType = treasureType,
                Tier = input.Tier,
                LootQualityMod = input.LootQualityMod,
                UnknownChances = input.UnknownChances,
                ItemChance = input.ItemChance,
                ItemMinAmount = input.ItemMinAmount,
                ItemMaxAmount = input.ItemMaxAmount,
                ItemTreasureTypeSelectionChances = input.ItemTreasureTypeSelectionChances,
                MagicItemChance = input.MagicItemChance,
                MagicItemMinAmount = input.MagicItemMinAmount,
                MagicItemMaxAmount = input.MagicItemMaxAmount,
                MagicItemTreasureTypeSelectionChances = input.MagicItemTreasureTypeSelectionChances,
                MundaneItemChance = input.MundaneItemChance,
                MundaneItemMinAmount = input.MundaneItemMinAmount,
                MundaneItemMaxAmount = input.MundaneItemMaxAmount,
                MundaneItemTypeSelectionChances = input.MundaneItemTypeSelectionChances,
                LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
            };

            return result;
        }


        public static List<TreasureWielded> ConvertToACE(this Dictionary<uint, List<Seg3_TreasureTable.WieldedTreasure>> input)
        {
            var results = new List<TreasureWielded>();

            foreach (var kvp in input)
            {
                foreach (var value in kvp.Value)
                {
                    var converted = value.ConvertToACE(kvp.Key);

                    results.Add(converted);
                }
            }

            return results;
        }

        public static TreasureWielded ConvertToACE(this Seg3_TreasureTable.WieldedTreasure input, uint treasureType)
        {
            var result = new TreasureWielded
            {
                TreasureType = treasureType,
                WeenieClassId = input.WeenieClassId,
                PaletteId = input.PaletteId,
                Unknown1 = input.Unknown1,
                Shade = input.Shade,
                StackSize = input.StackSize,
                StackSizeVariance = input.StackSizeVariance,
                Probability = input.Probability,
                Unknown3 = input.Unknown3,
                Unknown4 = input.Unknown4,
                Unknown5 = input.Unknown5,
                SetStart = input.SetStart,
                HasSubSet = input.HasSubSet,
                ContinuesPreviousSet = input.ContinuesPreviousSet,
                Unknown9 = input.Unknown9,
                Unknown10 = input.Unknown10,
                Unknown11 = input.Unknown11,
                Unknown12 = input.Unknown12,
                LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00)
            };

            return result;
        }
    }
}
