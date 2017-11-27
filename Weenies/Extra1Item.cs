using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Extra1Item
	{
		public ushort Component1;
		public byte Component2;
		public byte Component3;

		public void Parse(BinaryReader binaryReader)
		{
			Component1 = binaryReader.ReadUInt16();
			Component2 = binaryReader.ReadByte();
			Component3 = binaryReader.ReadByte();
		}
	}
}
