using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry3 : IUnpackable
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

		public bool Unpack(BinaryReader reader)
		{
			m_00 = reader.ReadInt32();
			m_04 = reader.ReadInt32();
			m_08 = reader.ReadInt32();
			m_0C = reader.ReadInt32();
			m_10 = reader.ReadInt32();
			m_14 = reader.ReadInt32();
			m_18 = reader.ReadInt32();
			m_1C = reader.ReadInt32();
			m_20 = reader.ReadInt32();
			m_24 = reader.ReadInt32();
			m_28 = reader.ReadInt32();
			m_2C = reader.ReadInt32();
			m_30 = reader.ReadInt32();
			m_34 = reader.ReadInt32();
			m_38 = reader.ReadInt32();
			m_3C = reader.ReadInt32();
			m_40 = reader.ReadInt32();
			m_44 = reader.ReadInt32();
			m_48 = reader.ReadInt32();
			m_4C = reader.ReadInt32();
			m_50 = reader.ReadInt32();
			m_54 = reader.ReadInt32();

			return true;
		}
	}
}
