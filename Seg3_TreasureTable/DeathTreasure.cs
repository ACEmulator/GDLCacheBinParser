using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class DeathTreasure : IUnpackable
	{
		public int Tier;
		public float LootQualityMod;
		public int UnknownChances;
		public int ItemChance;
		public int ItemMinAmount;
		public int ItemMaxAmount;
		public int ItemTreasureTypeSelectionChances;
		public int MagicItemChance;
		public int MagicItemMinAmount;
		public int MagicItemMaxAmount;
		public int MagicItemTreasureTypeSelectionChances;
		public int MundaneItemChance;
		public int MundaneItemMinAmount;
		public int MundaneItemMaxAmount;
		public int MundaneItemTypeSelectionChances;

		public bool Unpack(BinaryReader reader)
		{
			Tier = reader.ReadInt32();
			LootQualityMod = reader.ReadSingle();
			UnknownChances = reader.ReadInt32();
			ItemChance = reader.ReadInt32();
			ItemMinAmount = reader.ReadInt32();
			ItemMaxAmount = reader.ReadInt32();
			ItemTreasureTypeSelectionChances = reader.ReadInt32();
			MagicItemChance = reader.ReadInt32();
			MagicItemMinAmount = reader.ReadInt32();
			MagicItemMaxAmount = reader.ReadInt32();
			MagicItemTreasureTypeSelectionChances = reader.ReadInt32();
			MundaneItemChance = reader.ReadInt32();
			MundaneItemMinAmount = reader.ReadInt32();
			MundaneItemMaxAmount = reader.ReadInt32();
			MundaneItemTypeSelectionChances = reader.ReadInt32();

			return true;
		}
	}
}
