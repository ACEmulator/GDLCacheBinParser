using System;
using System.IO;

using ACE.Entity.Enum;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class Spell : IUnpackable
    {
        public uint ID;

        public string Name;

        public string Description;

        public int School;
        public uint IconID;
        public uint Category; // aka Family
        public uint Bitfield;
        public uint Mana;
        public float RangeConstant;
        public float RangeMod;
        public uint Power; // aka Difficulty
        public float EconomyMod;
        public uint FormulaVersion;
        public float ComponentLoss;

        public int MetaSpellType;
        public uint MetaSpellId; // Just the spell id again

        // EnchantmentSpell/FellowshipEnchantmentSpells
        public double? Duration;
        public float? DegradeModifier;
        public float? DegradeLimit;
        public SpellStatMod SpellStatMod;

        // Projectile, LifeProjectile
        public uint? EType;
        public int? BaseIntensity;
        public int? Variance;
        public uint? WCID;
        public int? NumProjectiles;
        public int? NumProjectilesVariance;
        public float? SpreadAngle;
        public float? VerticalAngle;
        public float? DefaultLaunchAngle;
        public bool? NonTracking;

        public Origin CreateOffset;
        public Origin Padding;
        public Origin Dims;
        public Origin Peturbation;

        public uint? ImbuedEffect;
        public int? SlayerCreatureType;
        public float? SlayerDamageBonus;
        public double? CritFreq;
        public double? CritMultiplier;
        public int? IgnoreMagicResist;
        public double? ElementalModifier;

        // LifeProjectile
        public float? DrainPercentage;
        public float? DamageRatio;

        // Boost, FellowBoost
        public int? DamageType;
        public int? Boost;
        public int? BoostVariance;

        // Transfer
        public int? Source;
        public int? Destination;
        public float? Proportion;
        public float? LossPercent;
        public int? SourceLoss;
        public int? TransferCap;
        public int? MaxBoostAllowed;
        public uint? TransferBitfield;

        // PortalLink
        public int? Index;

        // PortalRecall
        //public int? Index; Reusd PortalLink index var

        // PortalSummon
        public double? PortalLifetime;
        public int? Link;

        // PortalSending, FellowPortalSending
        public Position Position;

        // Dispel, FellowDispel
        public int? MinPower;
        public int? MaxPower;
        public float? PowerVariance;
        public int? DispelSchool;
        public int? Align;
        public int? Number;
        public float? NumberVariance;

        public SpellFormula SpellFormula = new SpellFormula();

        public uint CasterEffect;
        public uint TargetEffect;
        public uint FizzleEffect;
        public double RecoveryInterval;
        public float RecoveryAmount;
        public uint DisplayOrder;
        public uint NonComponentTargetType; // aka Target Mask
        public uint ManaMod;


        public bool Unpack(BinaryReader reader)
        {
            ID = reader.ReadUInt32();

            Name = Util.ReadString(reader, true);

            Description = Util.ReadString(reader, true);

            School = reader.ReadInt32();
            IconID = reader.ReadUInt32();
            Category = reader.ReadUInt32();
            Bitfield = reader.ReadUInt32();
            Mana = reader.ReadUInt32();
            RangeConstant = reader.ReadSingle();
            RangeMod = reader.ReadSingle();
            Power = reader.ReadUInt32();
            EconomyMod = reader.ReadSingle();
            FormulaVersion = reader.ReadUInt32();
            ComponentLoss = reader.ReadSingle();

            MetaSpellType = reader.ReadInt32();
            MetaSpellId = reader.ReadUInt32();

            if (MetaSpellType == (uint)SpellType.Enchantment || MetaSpellType == (uint)SpellType.FellowEnchantment)
            {
                Duration = reader.ReadDouble();
                DegradeModifier = reader.ReadSingle();
                DegradeLimit = reader.ReadSingle();

                var spellCategory = reader.ReadUInt32();
                if (spellCategory != Category)
                    throw new Exception();

                SpellStatMod = new SpellStatMod();
                SpellStatMod.Unpack(reader);
            }
            else if (MetaSpellType == (uint)SpellType.Projectile || MetaSpellType == (uint)SpellType.LifeProjectile)
            {
                EType = reader.ReadUInt32();
                BaseIntensity = reader.ReadInt32();
                Variance = reader.ReadInt32();
                WCID = reader.ReadUInt32();
                NumProjectiles = reader.ReadInt32();
                NumProjectilesVariance = reader.ReadInt32();
                SpreadAngle = reader.ReadSingle();
                VerticalAngle = reader.ReadSingle();
                DefaultLaunchAngle = reader.ReadSingle();
                NonTracking = (reader.ReadInt32() == 1);

                CreateOffset = new Origin();
                Padding = new Origin();
                Dims = new Origin();
                Peturbation = new Origin();

                CreateOffset.Unpack(reader);
                Padding.Unpack(reader);
                Dims.Unpack(reader);
                Peturbation.Unpack(reader);

                ImbuedEffect = reader.ReadUInt32();
                SlayerCreatureType = reader.ReadInt32();
                SlayerDamageBonus = reader.ReadSingle();
                CritFreq = reader.ReadDouble();
                CritMultiplier = reader.ReadDouble();
                IgnoreMagicResist = reader.ReadInt32();
                ElementalModifier = reader.ReadDouble();

                if (MetaSpellType == (uint)SpellType.LifeProjectile)
                {
                    DrainPercentage = reader.ReadSingle();
                    DamageRatio = reader.ReadSingle();
                }
            }
            else if (MetaSpellType == (uint)SpellType.Boost || MetaSpellType == (uint)SpellType.FellowBoost)
            {
                DamageType = reader.ReadInt32();
                Boost = reader.ReadInt32();
                BoostVariance = reader.ReadInt32();
            }
            else if (MetaSpellType == (uint)SpellType.Transfer)
            {
                Source = reader.ReadInt32();
                Destination = reader.ReadInt32();
                Proportion = reader.ReadSingle();
                LossPercent = reader.ReadSingle();
                SourceLoss = reader.ReadInt32();
                TransferCap = reader.ReadInt32();
                MaxBoostAllowed = reader.ReadInt32();
                TransferBitfield = reader.ReadUInt32();
            }
            else if (MetaSpellType == (uint)SpellType.PortalLink)
            {
                Index = reader.ReadInt32();
            }
            else if (MetaSpellType == (uint)SpellType.PortalRecall)
            {
                Index = reader.ReadInt32();
            }
            else if (MetaSpellType == (uint)SpellType.PortalSummon)
            {
                PortalLifetime = reader.ReadDouble();
                Link = reader.ReadInt32();
            }
            else if (MetaSpellType == (uint)SpellType.PortalSending || MetaSpellType == (uint)SpellType.FellowPortalSending)
            {
                Position = new Position();
                Position.Unpack(reader);
            }
            else if (MetaSpellType == (uint)SpellType.Dispel || MetaSpellType == (uint)SpellType.FellowDispel)
            {
                MinPower = reader.ReadInt32();
                MaxPower = reader.ReadInt32();
                PowerVariance = reader.ReadSingle();
                DispelSchool = reader.ReadInt32();
                Align = reader.ReadInt32();
                Number = reader.ReadInt32();
                NumberVariance = reader.ReadSingle();
            }
            else
                throw new NotImplementedException();

            SpellFormula.Unpack(reader);

            CasterEffect = reader.ReadUInt32();
            TargetEffect = reader.ReadUInt32();
            FizzleEffect = reader.ReadUInt32();
            RecoveryInterval = reader.ReadDouble();
            RecoveryAmount = reader.ReadSingle();
            DisplayOrder = reader.ReadUInt32();
            NonComponentTargetType = reader.ReadUInt32();
            ManaMod = reader.ReadUInt32();

            return true;
        }
    }
}
