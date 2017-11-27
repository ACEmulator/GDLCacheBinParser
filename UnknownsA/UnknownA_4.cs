using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.UnknownsA
{
	class UnknownA_4
	{
		// _cache_bin_parse_A_5
		public double Unknown_5_0;

		// _cache_bin_parse_A_6
		public readonly List<UnknownA_7> AUnknown_7 = new List<UnknownA_7>();


		public void Parse(BinaryReader binaryReader)
		{
			int count;


			// _cache_bin_parse_A_5
			Unknown_5_0 = binaryReader.ReadDouble();

			// _cache_bin_parse_A_6
			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var aunknown_7 = new UnknownA_7();
				aunknown_7.Parse(binaryReader);
				AUnknown_7.Add(aunknown_7);
			}
		}
	}
}
