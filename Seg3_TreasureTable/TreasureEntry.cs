using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry : IUnpackable
	{
		// Unknown3_1_4
		public uint WCID;
		public uint PTID;
		public uint m_08_AlwaysZero; // always zero
		public float Shade;
		public uint Amount;
		public float m_f14;
		public float Chance;
		public uint m_1C_AlwaysZero; // always zero
		public uint m_20_AlwaysZero; // always zero
		public uint m_24_AlwaysZero; // always zero
		public bool m_b28;
		public bool m_b2C;
		public bool m_b30;
		public uint m_34_AlwaysZero; // always zero
		public uint m_38_AlwaysZero; // always zero
		public uint m_3C_AlwaysZero; // always zero
		public uint m_40_AlwaysZero; // always zero

		public bool Unpack(BinaryReader reader)
		{
			// Unknown3_1_4
			WCID = reader.ReadUInt32();
			PTID = reader.ReadUInt32();
			m_08_AlwaysZero = reader.ReadUInt32();
			Shade = reader.ReadSingle();
			Amount = reader.ReadUInt32();
			m_f14 = reader.ReadSingle();
			Chance = reader.ReadSingle();
			m_1C_AlwaysZero = reader.ReadUInt32();
			m_20_AlwaysZero = reader.ReadUInt32();
			m_24_AlwaysZero = reader.ReadUInt32();
			m_b28 = (reader.ReadUInt32() == 1);
			m_b2C = (reader.ReadUInt32() == 1);
			m_b30 = (reader.ReadUInt32() == 1);
			m_34_AlwaysZero = reader.ReadUInt32();
			m_38_AlwaysZero = reader.ReadUInt32();
			m_3C_AlwaysZero = reader.ReadUInt32();
			m_40_AlwaysZero = reader.ReadUInt32();

			return true;
		}
	}
}
