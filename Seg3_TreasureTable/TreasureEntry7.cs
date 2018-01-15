using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureEntry7
	{
		public readonly List<List<TreasureEntry5>> Entries = new List<List<TreasureEntry5>>();

		public void Parse(BinaryReader binaryReader)
		{
			for (int i = 0; i < 6; i++)
			{
				var items = new List<TreasureEntry5>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry5();
					item.Parse(binaryReader);
					items.Add(item);
				}
				Entries.Add(items);
			}
		}
	}
}
