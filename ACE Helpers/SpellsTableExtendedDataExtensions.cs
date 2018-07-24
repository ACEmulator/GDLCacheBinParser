using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class SpellsTableExtendedDataExtensions
    {
        public static List<Spell> ConvertToACE(this Seg2_SpellTableExtendedData.SpellTableExtendedData input)
        {
            var results = new List<Spell>();

            foreach (var value in input.Spells)
            {
                var converted = value.ConvertToACE();

                results.Add(converted);
            }

            return results;
        }

        public static Spell ConvertToACE(this Seg2_SpellTableExtendedData.Spell input)
        {
            var result = new Spell();

            result.SpellId = input.ID;

            result.Name = input.Name;

            result.Description = input.Description;

            result.School = input.School;
            result.IconId = input.IconID;
            result.Category = input.Category; // aka Family
            result.Bitfield = input.Bitfield;
            result.Mana = input.Mana;
            result.RangeConstant = input.RangeConstant;
            result.RangeMod = input.RangeMod;
            result.Power = input.Power; // aka Difficulty
            result.EconomyMod = input.EconomyMod;
            result.FormulaVersion = input.FormulaVersion;
            result.ComponentLoss = input.ComponentLoss;

            result.MetaSpellType = input.MetaSpellType;
            result.MetaSpellId = input.MetaSpellId; // Just the spell id again

            // EnchantmentSpell/FellowshipEnchantmentSpells
            result.Duration = input.Duration;
            result.DegradeModifier = input.DegradeModifier;
            result.DegradeLimit = input.DegradeLimit;
            if (input.SpellStatMod != null)
            {
                result.StatModType = input.SpellStatMod.Type;
                result.StatModKey = input.SpellStatMod.Key;
                result.StatModVal = input.SpellStatMod.Val;
            }

            // Projectile, LifeProjectile
            result.EType = input.EType;
            result.BaseIntensity = input.BaseIntensity;
            result.Variance = input.Variance;
            result.Wcid = input.WCID;
            result.NumProjectiles = input.NumProjectiles;
            result.NumProjectilesVariance = input.NumProjectilesVariance;
            result.SpreadAngle = input.SpreadAngle;
            result.VerticalAngle = input.VerticalAngle;
            result.DefaultLaunchAngle = input.DefaultLaunchAngle;
            result.NonTracking = input.NonTracking;

            if (input.CreateOffset != null)
            {
                result.CreateOffsetOriginX = input.CreateOffset.X;
                result.CreateOffsetOriginY = input.CreateOffset.Y;
                result.CreateOffsetOriginZ = input.CreateOffset.X;
            }
            if (input.Padding != null)
            {
                result.PaddingOriginX = input.Padding.X;
                result.PaddingOriginY = input.Padding.Y;
                result.PaddingOriginZ = input.Padding.X;
            }
            if (input.Dims != null)
            {
                result.DimsOriginX = input.Dims.X;
                result.DimsOriginY = input.Dims.Y;
                result.DimsOriginZ = input.Dims.X;
            }
            if (input.Peturbation != null)
            {
                result.PeturbationOriginX = input.Peturbation.X;
                result.PeturbationOriginY = input.Peturbation.Y;
                result.PeturbationOriginZ = input.Peturbation.X;
            }

            result.ImbuedEffect = input.ImbuedEffect;
            result.SlayerCreatureType = input.SlayerCreatureType;
            result.SlayerDamageBonus = input.SlayerDamageBonus;
            result.CritFreq = input.CritFreq;
            result.CritMultiplier = input.CritMultiplier;
            result.IgnoreMagicResist = input.IgnoreMagicResist;
            result.ElementalModifier = input.ElementalModifier;

            // LifeProjectile
            result.DrainPercentage = input.DrainPercentage;
            result.DamageRatio = input.DamageRatio;

            // Boost, FellowBoost
            result.DamageType = input.DamageType;
            result.Boost = input.Boost;
            result.BoostVariance = input.BoostVariance;

            // Transfer
            result.Source = input.Source;
            result.Destination = input.Destination;
            result.Proportion = input.Proportion;
            result.LossPercent = input.LossPercent;
            result.SourceLoss = input.SourceLoss;
            result.TransferCap = input.TransferCap;
            result.MaxBoostAllowed = input.MaxBoostAllowed;
            result.TransferBitfield = input.TransferBitfield;

            // PortalLink
            result.Index = input.Index;

            // PortalRecall
            //public int? Index; Reusd PortalLink index var

            // PortalSummon
            result.PortalLifetime = input.PortalLifetime;
            result.Link = input.Link;

            // PortalSending, FellowPortalSending
            if (input.Position != null)
            {
                result.PositionObjCellId = input.Position.ObjCellID;

                result.PositionOriginX = input.Position.Origin.X;
                result.PositionOriginY = input.Position.Origin.Y;
                result.PositionOriginZ = input.Position.Origin.Z;

                result.PositionAnglesX = input.Position.Angles.X;
                result.PositionAnglesY = input.Position.Angles.Y;
                result.PositionAnglesZ = input.Position.Angles.Z;
                result.PositionAnglesW = input.Position.Angles.W;
            }

            // Dispel, FellowDispel
            result.MinPower = input.MinPower;
            result.MaxPower = input.MaxPower;
            result.PowerVariance = input.PowerVariance;
            result.DispelSchool = input.DispelSchool;
            result.Align = input.Align;
            result.Number = input.Number;
            result.NumberVariance = input.NumberVariance;

            if (input.SpellFormula.Comps.Count >= 1) result.SpellFormulaComp1ComponentId = input.SpellFormula.Comps[0];
            if (input.SpellFormula.Comps.Count >= 2) result.SpellFormulaComp2ComponentId = input.SpellFormula.Comps[1];
            if (input.SpellFormula.Comps.Count >= 3) result.SpellFormulaComp3ComponentId = input.SpellFormula.Comps[2];
            if (input.SpellFormula.Comps.Count >= 4) result.SpellFormulaComp4ComponentId = input.SpellFormula.Comps[3];
            if (input.SpellFormula.Comps.Count >= 5) result.SpellFormulaComp5ComponentId = input.SpellFormula.Comps[4];
            if (input.SpellFormula.Comps.Count >= 6) result.SpellFormulaComp6ComponentId = input.SpellFormula.Comps[5];
            if (input.SpellFormula.Comps.Count >= 7) result.SpellFormulaComp7ComponentId = input.SpellFormula.Comps[6];
            if (input.SpellFormula.Comps.Count >= 8) result.SpellFormulaComp8ComponentId = input.SpellFormula.Comps[7];

            result.CasterEffect = input.CasterEffect;
            result.TargetEffect = input.TargetEffect;
            result.FizzleEffect = input.FizzleEffect;
            result.RecoveryInterval = input.RecoveryInterval;
            result.RecoveryAmount = input.RecoveryAmount;
            result.DisplayOrder = input.DisplayOrder;
            result.NonComponentTargetType = input.NonComponentTargetType; // aka Target Mask
            result.ManaMod = input.ManaMod;

            return result;
        }
    }
}
