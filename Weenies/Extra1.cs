using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Extra1
	{
		public ushort unknown;

		public List<Extra1Item> Items = new List<Extra1Item>();

		public void Parse(BinaryReader binaryReader, int count)
		{
			unknown = binaryReader.ReadUInt16();

			for (int i = 0; i < count; i++)
			{
				var item = new Extra1Item();
				item.Parse(binaryReader);
				Items.Add(item);
			}
		}
	}
}
