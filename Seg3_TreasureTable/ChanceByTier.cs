using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class ChanceByTier : IUnpackable
	{
		public float[] Chances = new float[6];

		public bool Unpack(BinaryReader reader)
		{
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
