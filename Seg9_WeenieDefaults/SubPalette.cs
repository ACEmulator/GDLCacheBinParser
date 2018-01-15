using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class SubPalette
	{
		public int ID;
		public byte Offset;
		public byte NumberOfColors;

		public void Parse(BinaryReader binaryReader)
		{
			ID = Util.ReadPackedKnownType(binaryReader, 0x04000000);

            Offset = binaryReader.ReadByte();
		    //Offset *= 8;

            NumberOfColors = binaryReader.ReadByte();
            //if (NumberOfColors == 0)
            //    NumberOfColors = 256;
        }
	}
}
