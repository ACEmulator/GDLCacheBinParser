using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry7 : IUnpackable
	{
		public readonly List<List<TreasureEntry5>> Entries = new List<List<TreasureEntry5>>();

		public bool Unpack(BinaryReader reader)
		{
			Entries.Unpack(reader, 6);

			return true;
		}
	}
}
