using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class AnimPart
	{
		public byte Index;
		public int ID;

		public bool Unpack(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadByte();
		    ID = Util.ReadPackedKnownType(binaryReader, 0x01000000);

			return true;
		}
	}
}
