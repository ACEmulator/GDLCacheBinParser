using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry6 : IUnpackable
	{
		// Unknown3_5_8
		public float[] Chances = new float[6];

		public bool Unpack(BinaryReader reader)
		{
			// Unknown3_5_8
			Chances[0] = reader.ReadSingle();
			Chances[1] = reader.ReadSingle();
			Chances[2] = reader.ReadSingle();
			Chances[3] = reader.ReadSingle();
			Chances[4] = reader.ReadSingle();
			Chances[5] = reader.ReadSingle();

			return true;
		}
	}
}
