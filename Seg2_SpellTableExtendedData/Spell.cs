using System;
using System.IO;
using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class Spell : IPackable
    {
        public uint ID;

        public string Name;

        public string Description;

        public School School;
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

        public SpellType MetaSpellType;
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
        public DamageType? DamageType;
        public int? Boost;
        public int? BoostVariance;

        // Transfer
        public Attribute2nd? Source;
        public Attribute2nd? Destination;
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
        public double? PortalLlifetime;
        public int? Link;

        // PortalSending, FellowPortalSending
        public Position Position;

        // Dispel, FellowDispel
        public int? MinPower;
        public int? MaxPower;
        public float? PowerVariance;
        public School? DispelSchool;
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


        public bool Unpack(BinaryReader binaryReader)
        {
            ID = binaryReader.ReadUInt32();

            Name = Util.ReadString(binaryReader, true);

            Description = Util.ReadString(binaryReader, true);

            School = (School)binaryReader.ReadUInt32();
            IconID = binaryReader.ReadUInt32();
            Category = binaryReader.ReadUInt32();
            Bitfield = binaryReader.ReadUInt32();
            Mana = binaryReader.ReadUInt32();
            RangeConstant = binaryReader.ReadSingle();
            RangeMod = binaryReader.ReadSingle();
            Power = binaryReader.ReadUInt32();
            EconomyMod = binaryReader.ReadSingle();
            FormulaVersion = binaryReader.ReadUInt32();
            ComponentLoss = binaryReader.ReadSingle();

            MetaSpellType = (SpellType)binaryReader.ReadUInt32();
            MetaSpellId = binaryReader.ReadUInt32();

            if (MetaSpellType == SpellType.Enchantment || MetaSpellType == SpellType.FellowEnchantment)
            {
                Duration = binaryReader.ReadDouble();
                DegradeModifier = binaryReader.ReadSingle();
                DegradeLimit = binaryReader.ReadSingle();

                var spellCategory = binaryReader.ReadUInt32();
                if (spellCategory != Category)
                    throw new Exception();

                SpellStatMod = new SpellStatMod();
                SpellStatMod.Unpack(binaryReader);
            }
            else if (MetaSpellType == SpellType.Projectile || MetaSpellType == SpellType.LifeProjectile)
            {
                EType = binaryReader.ReadUInt32();
                BaseIntensity = binaryReader.ReadInt32();
                Variance = binaryReader.ReadInt32();
                WCID = binaryReader.ReadUInt32();
                NumProjectiles = binaryReader.ReadInt32();
                NumProjectilesVariance = binaryReader.ReadInt32();
                SpreadAngle = binaryReader.ReadSingle();
                VerticalAngle = binaryReader.ReadSingle();
                DefaultLaunchAngle = binaryReader.ReadSingle();
                NonTracking = (binaryReader.ReadInt32() == 1);

                CreateOffset = new Origin();
                Padding = new Origin();
                Dims = new Origin();
                Peturbation = new Origin();

                CreateOffset.Unpack(binaryReader);
                Padding.Unpack(binaryReader);
                Dims.Unpack(binaryReader);
                Peturbation.Unpack(binaryReader);

                ImbuedEffect = binaryReader.ReadUInt32();
                SlayerCreatureType = binaryReader.ReadInt32();
                SlayerDamageBonus = binaryReader.ReadSingle();
                CritFreq = binaryReader.ReadDouble();
                CritMultiplier = binaryReader.ReadDouble();
                IgnoreMagicResist = binaryReader.ReadInt32();
                ElementalModifier = binaryReader.ReadDouble();

                if (MetaSpellType == SpellType.LifeProjectile)
                {
                    DrainPercentage = binaryReader.ReadSingle();
                    DamageRatio = binaryReader.ReadSingle();
                }
            }
            else if (MetaSpellType == SpellType.Boost || MetaSpellType == SpellType.FellowBoost)
            {
                DamageType = (DamageType)binaryReader.ReadInt32();
                Boost = binaryReader.ReadInt32();
                BoostVariance = binaryReader.ReadInt32();
            }
            else if (MetaSpellType == SpellType.Transfer)
            {
                Source = (Attribute2nd)binaryReader.ReadInt32();
                Destination = (Attribute2nd)binaryReader.ReadInt32();
                Proportion = binaryReader.ReadSingle();
                LossPercent = binaryReader.ReadSingle();
                SourceLoss = binaryReader.ReadInt32();
                TransferCap = binaryReader.ReadInt32();
                MaxBoostAllowed = binaryReader.ReadInt32();
                TransferBitfield = binaryReader.ReadUInt32();
            }
            else if (MetaSpellType == SpellType.PortalLink)
            {
                Index = binaryReader.ReadInt32();
            }
            else if (MetaSpellType == SpellType.PortalRecall)
            {
                Index = binaryReader.ReadInt32();
            }
            else if (MetaSpellType == SpellType.PortalSummon)
            {
                PortalLlifetime = binaryReader.ReadDouble();
                Link = binaryReader.ReadInt32();
            }
            else if (MetaSpellType == SpellType.PortalSending || MetaSpellType == SpellType.FellowPortalSending)
            {
                Position = new Position();
                Position.Unpack(binaryReader);
            }
            else if (MetaSpellType == SpellType.Dispel || MetaSpellType == SpellType.FellowDispel)
            {
                MinPower = binaryReader.ReadInt32();
                MaxPower = binaryReader.ReadInt32();
                PowerVariance = binaryReader.ReadSingle();
                DispelSchool = (School)binaryReader.ReadInt32();
                Align = binaryReader.ReadInt32();
                Number = binaryReader.ReadInt32();
                NumberVariance = binaryReader.ReadSingle();
            }
            else
                throw new NotImplementedException();

            SpellFormula.Unpack(binaryReader);

            CasterEffect = binaryReader.ReadUInt32();
            TargetEffect = binaryReader.ReadUInt32();
            FizzleEffect = binaryReader.ReadUInt32();
            RecoveryInterval = binaryReader.ReadDouble();
            RecoveryAmount = binaryReader.ReadSingle();
            DisplayOrder = binaryReader.ReadUInt32();
            NonComponentTargetType = binaryReader.ReadUInt32();
            ManaMod = binaryReader.ReadUInt32();

            return true;
        }
    }
}
