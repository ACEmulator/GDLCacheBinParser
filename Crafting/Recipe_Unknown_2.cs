using System.IO;

namespace PhatACCacheBinParser.Crafting
{
	class Recipe_Unknown_2
	{
		public uint unknown_1;
		public uint unknown_2;
		public uint unknown_3;
		public uint unknown_4;
		public uint unknown_5;
		public uint unknown_6;

		public void Parse(BinaryReader binaryReader)
		{
			unknown_1 = binaryReader.ReadUInt32();
			unknown_2 = binaryReader.ReadUInt32();
			unknown_3 = binaryReader.ReadUInt32();
			unknown_4 = binaryReader.ReadUInt32();
			unknown_5 = binaryReader.ReadUInt32();
			unknown_6 = binaryReader.ReadUInt32();
		}
	}
}
