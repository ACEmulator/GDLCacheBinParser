using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class DeathTreasure : IUnpackable
	{
		// Unknown3_2_2
		public int Tier;
		public float Unknown1;
		public int Unknown2;
		public int Unknown3;
		public int Unknown4;
		public int Unknown5;
		public int Unknown6;
		public int Unknown7;
		public int Unknown8;
		public int Unknown9;
		public int Unknown10;
		public int Unknown11;
		public int Unknown12;
		public int Unknown13;
		public int Unknown14;

		public bool Unpack(BinaryReader reader)
		{
			Tier = reader.ReadInt32();
			Unknown1 = reader.ReadSingle();
			Unknown2 = reader.ReadInt32();
			Unknown3 = reader.ReadInt32();
			Unknown4 = reader.ReadInt32();
			Unknown5 = reader.ReadInt32();
			Unknown6 = reader.ReadInt32();
			Unknown7 = reader.ReadInt32();
			Unknown8 = reader.ReadInt32();
			Unknown9 = reader.ReadInt32();
			Unknown10 = reader.ReadInt32();
			Unknown11 = reader.ReadInt32();
			Unknown12 = reader.ReadInt32();
			Unknown13 = reader.ReadInt32();
			Unknown14 = reader.ReadInt32();

			return true;
		}
	}
}
