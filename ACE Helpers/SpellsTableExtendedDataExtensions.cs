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

            result.Id = input.ID;

            result.Name = input.Name;

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
                result.CreateOffsetOriginZ = input.CreateOffset.Z;
            }
            if (input.Padding != null)
            {
                result.PaddingOriginX = input.Padding.X;
                result.PaddingOriginY = input.Padding.Y;
                result.PaddingOriginZ = input.Padding.Z;
            }
            if (input.Dims != null)
            {
                result.DimsOriginX = input.Dims.X;
                result.DimsOriginY = input.Dims.Y;
                result.DimsOriginZ = input.Dims.Z;
            }
            if (input.Peturbation != null)
            {
                result.PeturbationOriginX = input.Peturbation.X;
                result.PeturbationOriginY = input.Peturbation.Y;
                result.PeturbationOriginZ = input.Peturbation.Z;
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
            result.Link = input.Link;

            // PortalSending, FellowPortalSending
            if (input.Position != null)
            {
                result.PositionObjCellId = input.Position.ObjCellID;

                result.PositionOriginX = input.Position.Origin.X;
                result.PositionOriginY = input.Position.Origin.Y;
                result.PositionOriginZ = input.Position.Origin.Z;

                result.PositionAnglesW = input.Position.Angles.W;
                result.PositionAnglesX = input.Position.Angles.X;
                result.PositionAnglesY = input.Position.Angles.Y;
                result.PositionAnglesZ = input.Position.Angles.Z;
            }

            // Dispel, FellowDispel
            result.MinPower = input.MinPower;
            result.MaxPower = input.MaxPower;
            result.PowerVariance = input.PowerVariance;
            result.DispelSchool = input.DispelSchool;
            result.Align = input.Align;
            result.Number = input.Number;
            result.NumberVariance = input.NumberVariance;

            return result;
        }
    }
}
