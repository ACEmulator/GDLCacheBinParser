﻿using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry5
	{
		// Unknown3_3_4
		public uint Slot_WCID_SpellID;
		public double Chance;

		public void Parse(BinaryReader binaryReader)
		{
			// Unknown3_3_4
			Slot_WCID_SpellID = binaryReader.ReadUInt32();
			Chance = binaryReader.ReadDouble();
		}
	}
}
