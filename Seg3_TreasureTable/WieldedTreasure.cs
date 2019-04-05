using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class WieldedTreasure : IUnpackable
	{
		public uint WeenieClassId;
		public uint PaletteId;
		public uint Unknown1; // always zero
		public float Shade;
		public int StackSize;
		public float StackSizeVariance;
		public float Probability;
		public uint Unknown3; // always zero
		public uint Unknown4; // always zero
		public uint Unknown5; // always zero
		public bool SetStart;
		public bool HasSubSet;
		public bool ContinuesPreviousSet;
		public uint Unknown9; // always zero
		public uint Unknown10; // always zero
		public uint Unknown11; // always zero
		public uint Unknown12; // always zero

		public bool Unpack(BinaryReader reader)
		{
			WeenieClassId = reader.ReadUInt32();
			PaletteId = reader.ReadUInt32();
			Unknown1 = reader.ReadUInt32();
			Shade = reader.ReadSingle();
			StackSize = reader.ReadInt32();
			StackSizeVariance = reader.ReadSingle();
			Probability = reader.ReadSingle();
			Unknown3 = reader.ReadUInt32();
			Unknown4 = reader.ReadUInt32();
			Unknown5 = reader.ReadUInt32();
			SetStart = (reader.ReadUInt32() == 1);
			HasSubSet = (reader.ReadUInt32() == 1);
			ContinuesPreviousSet = (reader.ReadUInt32() == 1);
			Unknown9 = reader.ReadUInt32();
			Unknown10 = reader.ReadUInt32();
			Unknown11 = reader.ReadUInt32();
			Unknown12 = reader.ReadUInt32();

			return true;
		}
	}
}
