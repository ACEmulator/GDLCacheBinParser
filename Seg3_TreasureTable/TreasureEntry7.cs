using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry7
	{
		public readonly List<TreasureEntry5> Entries = new List<TreasureEntry5>();

		public void Parse(BinaryReader binaryReader)
		{
			// phat inits this to a static size of 6, is it always that?
		}
	}
}
