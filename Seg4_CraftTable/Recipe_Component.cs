using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class Recipe_Component
	{
		public double unknown_1; // This is the % of failure? It's usually .1
		public uint unknown_2;
		public string unknown_3; // This is the destroyed message

		public void Parse(BinaryReader binaryReader)
		{
			unknown_1 = binaryReader.ReadDouble();
			unknown_2 = binaryReader.ReadUInt32();
			unknown_3 = Util.ReadString(binaryReader, true);
		}
	}
}
