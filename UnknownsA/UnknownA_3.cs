﻿using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.UnknownsA
{
	class UnknownA_3
	{
		// _cache_bin_parse_A_4
		public readonly List<UnknownA_4> AUnknown_4 = new List<UnknownA_4>();


		public void Parse(BinaryReader binaryReader)
		{
			int count;


			// _cache_bin_parse_A_4
			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var aunknown_4 = new UnknownA_4();
				aunknown_4.Parse(binaryReader);
				AUnknown_4.Add(aunknown_4);
			}
		}
	}
}
