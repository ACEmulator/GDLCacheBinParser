using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Unknowns1
{
	class Unknown1 : IParseableObject
	{
		public readonly List<Unknown1_1> Unknown1_1 = new List<Unknown1_1>();

		public readonly List<byte[]> DataChunks = new List<byte[]>();
		public void Parse(BinaryReader binaryReader)
		{
			int count;


			// _cache_bin_parse_1
			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var unknown1_1 = new Unknown1_1();
				unknown1_1.Parse(binaryReader);
				Unknown1_1.Add(unknown1_1);
			}

			for (int i = 0; i < 0x1FC; i++)
			{
				var data = binaryReader.ReadBytes(128);

				DataChunks.Add(data);
			}
		}
	}
}
