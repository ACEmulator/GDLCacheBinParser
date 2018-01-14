using System.IO;

namespace PhatACCacheBinParser.Seg2_Spells
{
	class Spell : IParseableObject
	{
		// _cache_bin_parse_2_4
		public uint ID;

		// _cache_bin_parse_2_5
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

		// _cache_bin_parse_2_6
		public SpellType SpellType;

		// _cache_bin_parse_2_7
		public uint unknown_70;
		public double Duration;
		public double unknown_72;
		public uint unknown_73;

		// _cache_bin_parse_2_8
		public uint unknown_80;
		public uint unknown_81;
		public float unknown_82;

		// _cache_bin_parse_2_9
		public uint Formula0;
		public uint Formula1;
		public uint Formula2;
		public uint Formula3;
		public uint Formula4;
		public uint Formula5;
		public uint Formula6;
		public uint Formula7;

		// _cache_bin_parse_2_5
		public uint CasterEffect;
		public uint TargetEffect;
		public uint FizzleEffect;
		public double RecoveryInterval;
		public float RecoveryAmount;
		public uint DisplayOrder;
		public uint NonComponentTargetType; // aka Target Mask
		public uint ManaMod;


		public void Parse(BinaryReader binaryReader)
		{
			// _cache_bin_parse_2_4
			ID = binaryReader.ReadUInt32();

			// _cache_bin_parse_2_5
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

			// _cache_bin_parse_2_6
			SpellType = (SpellType)binaryReader.ReadUInt32();

			// _cache_bin_parse_2_7
			if (SpellType == SpellType.Enchantment)
			{
				unknown_70 = binaryReader.ReadUInt32();
				Duration = binaryReader.ReadDouble();
				unknown_72 = binaryReader.ReadDouble();
				unknown_73 = binaryReader.ReadUInt32();
			}

			// _cache_bin_parse_2_8
			if (SpellType == SpellType.Enchantment || SpellType == SpellType.Boost)
			{
				unknown_80 = binaryReader.ReadUInt32();
				unknown_81 = binaryReader.ReadUInt32();
				unknown_82 = binaryReader.ReadSingle();
			}

			// _cache_bin_parse_2_9
			Formula0 = binaryReader.ReadUInt32();
			Formula1 = binaryReader.ReadUInt32();
			Formula2 = binaryReader.ReadUInt32();
			Formula3 = binaryReader.ReadUInt32();
			Formula4 = binaryReader.ReadUInt32();
			Formula5 = binaryReader.ReadUInt32();
			Formula6 = binaryReader.ReadUInt32();
			Formula7 = binaryReader.ReadUInt32();

			// // _cache_bin_parse_2_5
			CasterEffect = binaryReader.ReadUInt32();
			TargetEffect = binaryReader.ReadUInt32();
			FizzleEffect = binaryReader.ReadUInt32();
			RecoveryInterval = binaryReader.ReadDouble();
			RecoveryAmount = binaryReader.ReadSingle();
			DisplayOrder = binaryReader.ReadUInt32();
			NonComponentTargetType = binaryReader.ReadUInt32();
			ManaMod = binaryReader.ReadUInt32();
		}
	}
}
