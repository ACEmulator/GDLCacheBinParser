using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Extra2Item
	{
		public byte Component1;
		public ushort Component2;
		public ushort Component3;

		public void Parse(BinaryReader binaryReader)
		{
			Component1 = binaryReader.ReadByte();
			Component2 = binaryReader.ReadUInt16();
			Component3 = binaryReader.ReadUInt16();
		}
	}
}
