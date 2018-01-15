using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry2
	{
		// Unknown3_2_2
		public int Tier;
		public float m_f04;
		public int m_08;
		public int m_0C;
		public int m_10;
		public int m_14;
		public int m_18;
		public int m_1C;
		public int m_20;
		public int m_24;
		public int m_28;
		public int m_2C;
		public int m_30;
		public int m_34;
		public int m_38;

		public void Parse(BinaryReader binaryReader)
		{
			Tier = binaryReader.ReadInt32();
			m_f04 = binaryReader.ReadSingle();
			m_08 = binaryReader.ReadInt32();
			m_0C = binaryReader.ReadInt32();
			m_10 = binaryReader.ReadInt32();
			m_14 = binaryReader.ReadInt32();
			m_18 = binaryReader.ReadInt32();
			m_1C = binaryReader.ReadInt32();
			m_20 = binaryReader.ReadInt32();
			m_24 = binaryReader.ReadInt32();
			m_28 = binaryReader.ReadInt32();
			m_2C = binaryReader.ReadInt32();
			m_30 = binaryReader.ReadInt32();
			m_34 = binaryReader.ReadInt32();
			m_38 = binaryReader.ReadInt32();
		}
	}
}
