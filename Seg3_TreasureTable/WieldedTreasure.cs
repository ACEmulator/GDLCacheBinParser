using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class WieldedTreasure : IUnpackable
	{
		// Unknown3_1_4
		public uint WeenieClassId;
		public uint PaletteId;
		public uint Unknown1; // always zero
		public float Shade;
		public int StackSize;
		public float Unknown2;
		public float Probability;
		public uint Unknown3; // always zero
		public uint Unknown4; // always zero
		public uint Unknown5; // always zero
		public bool Unknown6;
		public bool Unknown7;
		public bool Unknown8;
		public uint Unknown9; // always zero
		public uint Unknown10; // always zero
		public uint Unknown11; // always zero
		public uint Unknown12; // always zero

		public bool Unpack(BinaryReader reader)
		{
			// Unknown3_1_4
			WeenieClassId = reader.ReadUInt32();
			PaletteId = reader.ReadUInt32();
			Unknown1 = reader.ReadUInt32();
			Shade = reader.ReadSingle();
			StackSize = reader.ReadInt32();
			Unknown2 = reader.ReadSingle();
			Probability = reader.ReadSingle();
			Unknown3 = reader.ReadUInt32();
			Unknown4 = reader.ReadUInt32();
			Unknown5 = reader.ReadUInt32();
			Unknown6 = (reader.ReadUInt32() == 1);
			Unknown7 = (reader.ReadUInt32() == 1);
			Unknown8 = (reader.ReadUInt32() == 1);
			Unknown9 = reader.ReadUInt32();
			Unknown10 = reader.ReadUInt32();
			Unknown11 = reader.ReadUInt32();
			Unknown12 = reader.ReadUInt32();

			return true;
		}
	}
}
