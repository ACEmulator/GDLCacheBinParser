using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry2 : IUnpackable
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

		public bool Unpack(BinaryReader reader)
		{
			Tier = reader.ReadInt32();
			m_f04 = reader.ReadSingle();
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

			return true;
		}
	}
}
