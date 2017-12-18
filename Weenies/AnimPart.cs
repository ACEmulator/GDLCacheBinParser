using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class AnimPart
	{
		public byte Index;
		public int ID;

		public void Parse(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadByte();
		    ID = Util.ReadPackedKnownType(binaryReader, 0x01000000);
		}
	}
}
