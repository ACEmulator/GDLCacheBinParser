using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SegA
{
	class UnknownA_1 : IPackable
	{
		// _cache_bin_parse_A_2
		public readonly List<double> unknown_2_1 = new List<double>();

		// _cache_bin_parse_A_3
		public readonly List<UnknownA_3> AUnknown_3 = new List<UnknownA_3>();


		public bool Unpack(BinaryReader binaryReader)
		{
			int count;


			// _cache_bin_parse_A_2
			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
				unknown_2_1.Add(binaryReader.ReadDouble());

			// _cache_bin_parse_A_3
			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var aunknown_3 = new UnknownA_3();
				aunknown_3.Unpack(binaryReader);
				AUnknown_3.Add(aunknown_3);
			}

			return true;
		}
	}
}
