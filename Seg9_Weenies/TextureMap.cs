using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class TextureMap
	{
		public byte Index;
		public int OldTextureID;
		public int NewTextureID;

		public void Parse(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadByte();
			OldTextureID = Util.ReadPackedKnownType(binaryReader, 0x05000000);
            NewTextureID = Util.ReadPackedKnownType(binaryReader, 0x05000000);
        }
	}
}
