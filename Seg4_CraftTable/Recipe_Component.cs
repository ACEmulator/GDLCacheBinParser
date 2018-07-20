using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class Recipe_Component
	{
		public double DestroyChance; // This is the % of failure? It's usually .1
		public uint DestroyAmount;
		public string DestroyMessage; // This is the destroyed message

		public bool Unpack(BinaryReader binaryReader)
		{
			DestroyChance = binaryReader.ReadDouble();
			DestroyAmount = binaryReader.ReadUInt32();
			DestroyMessage = Util.ReadString(binaryReader, true);

			return true;
		}
	}
}
