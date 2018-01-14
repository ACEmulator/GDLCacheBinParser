using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
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
