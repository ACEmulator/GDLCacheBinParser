using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Extra3Item
	{
		public byte Component1;
		public ushort Component2;

		public void Parse(BinaryReader binaryReader)
		{
			Component1 = binaryReader.ReadByte();
			Component2 = binaryReader.ReadUInt16();
		}
	}
}
