using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry : IPackable
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

		public bool Unpack(BinaryReader binaryReader)
		{
			// Unknown3_1_4
			WCID = binaryReader.ReadUInt32();
			PTID = binaryReader.ReadUInt32();
			m_08_AlwaysZero = binaryReader.ReadUInt32();
			Shade = binaryReader.ReadSingle();
			Amount = binaryReader.ReadUInt32();
			m_f14 = binaryReader.ReadSingle();
			Chance = binaryReader.ReadSingle();
			m_1C_AlwaysZero = binaryReader.ReadUInt32();
			m_20_AlwaysZero = binaryReader.ReadUInt32();
			m_24_AlwaysZero = binaryReader.ReadUInt32();
			m_b28 = (binaryReader.ReadUInt32() == 1);
			m_b2C = (binaryReader.ReadUInt32() == 1);
			m_b30 = (binaryReader.ReadUInt32() == 1);
			m_34_AlwaysZero = binaryReader.ReadUInt32();
			m_38_AlwaysZero = binaryReader.ReadUInt32();
			m_3C_AlwaysZero = binaryReader.ReadUInt32();
			m_40_AlwaysZero = binaryReader.ReadUInt32();

			return true;
		}
	}
}
