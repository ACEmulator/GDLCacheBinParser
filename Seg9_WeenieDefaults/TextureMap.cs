using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class TextureMap
	{
		public byte Index;
		public int OldTextureID;
		public int NewTextureID;

		public bool Unpack(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadByte();
			OldTextureID = Util.ReadPackedKnownType(binaryReader, 0x05000000);
            NewTextureID = Util.ReadPackedKnownType(binaryReader, 0x05000000);

			return true;
		}
	}
}
