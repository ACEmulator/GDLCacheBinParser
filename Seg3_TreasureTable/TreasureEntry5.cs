using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry5 : IUnpackable
	{
		// Unknown3_3_4
		public uint Slot_WCID_SpellID;
		public double Chance;

		public bool Unpack(BinaryReader reader)
		{
			// Unknown3_3_4
			Slot_WCID_SpellID = reader.ReadUInt32();
			Chance = reader.ReadDouble();

			return true;
		}
	}
}
