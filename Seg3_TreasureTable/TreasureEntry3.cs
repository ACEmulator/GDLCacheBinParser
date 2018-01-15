using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry3
	{
		public int m_00;
		public int m_04;
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
		public int m_3C;
		public int m_40;
		public int m_44;
		public int m_48;
		public int m_4C;
		public int m_50;
		public int m_54;

		public void Parse(BinaryReader binaryReader)
		{
			m_00 = binaryReader.ReadInt32();
			m_04 = binaryReader.ReadInt32();
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
			m_3C = binaryReader.ReadInt32();
			m_40 = binaryReader.ReadInt32();
			m_44 = binaryReader.ReadInt32();
			m_48 = binaryReader.ReadInt32();
			m_4C = binaryReader.ReadInt32();
			m_50 = binaryReader.ReadInt32();
			m_54 = binaryReader.ReadInt32();
		}
	}
}
