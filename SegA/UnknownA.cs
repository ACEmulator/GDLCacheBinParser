using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SegA
{
	class UnknownA : IUnpackable
	{
		// _cache_bin_func_123458A
		public uint Index;

		public uint unknown_0_2;

		public readonly List<UnknownA_1> AUnknown_1 = new List<UnknownA_1>();


		public bool Unpack(BinaryReader reader)
		{
			int count;


			// _cache_bin_func_123458A
			Index = reader.ReadUInt32();

			unknown_0_2 = reader.ReadUInt32();

			// _cache_bin_parse_A_1
			count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var aunknown_1 = new UnknownA_1();
				aunknown_1.Unpack(reader);
				AUnknown_1.Add(aunknown_1);
			}

			return true;
		}
	}
}
