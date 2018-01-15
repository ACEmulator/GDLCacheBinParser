using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry6
	{
		// Unknown3_5_8
		public float[] Chances = new float[6];

		public void Parse(BinaryReader binaryReader)
		{
			// Unknown3_5_8
			Chances[0] = binaryReader.ReadSingle();
			Chances[1] = binaryReader.ReadSingle();
			Chances[2] = binaryReader.ReadSingle();
			Chances[3] = binaryReader.ReadSingle();
			Chances[4] = binaryReader.ReadSingle();
			Chances[5] = binaryReader.ReadSingle();
		}
	}
}
