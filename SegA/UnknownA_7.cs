using System.IO;

namespace PhatACCacheBinParser.SegA
{
	class UnknownA_7
	{
		// _cache_bin_parse_A_7
		public uint Unknown_7_01;
		public ulong Unknown_7_02;

		// _cache_bin_parse_A_8
		public uint Unknown_8_0;

		// _cache_bin_parse_A_7
		public uint Unknown_7_11;
		public ulong Unknown_7_12;

		// _cache_bin_parse_A_7
		public uint Unknown_7_21;
		public ulong Unknown_7_22;


		public bool Unpack(BinaryReader binaryReader)
		{
			// _cache_bin_parse_A_7
			Unknown_7_01 = binaryReader.ReadUInt32();
			Unknown_7_02 = binaryReader.ReadUInt64();

			// _cache_bin_parse_A_8
			Unknown_8_0 = binaryReader.ReadUInt32();

			// _cache_bin_parse_A_7
			Unknown_7_11 = binaryReader.ReadUInt32();
			Unknown_7_12 = binaryReader.ReadUInt64();

			// _cache_bin_parse_A_7
			Unknown_7_21 = binaryReader.ReadUInt32();
			Unknown_7_22 = binaryReader.ReadUInt64();

			return true;
		}
	}
}
