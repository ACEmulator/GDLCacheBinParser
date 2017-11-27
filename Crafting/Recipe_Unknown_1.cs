using System.IO;

namespace PhatACCacheBinParser.Crafting
{
	class Recipe_Unknown_1
	{
		public double unknown_1;
		public uint unknown_2;
		public string unknown_3;

		public void Parse(BinaryReader binaryReader)
		{
			unknown_1 = binaryReader.ReadDouble();
			unknown_2 = binaryReader.ReadUInt32();
			unknown_3 = Util.ReadString(binaryReader, true);
		}
	}
}
