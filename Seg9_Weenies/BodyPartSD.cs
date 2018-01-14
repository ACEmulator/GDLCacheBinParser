using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class BodyPartSD
	{
		// I'm not sure what SD is an abbreviation for. Possibly Scaled Defense?

		// I believe these are High.. Middle.. Low..          .Left. .Right.     ..Back ..Front
		/*
		_cache_bin_parse_9_body_body_part_table_value_bpsd
		00000000 00000000 AE47613E    ?HLF?   ?HRF    ?MLF?   ?MRF LLB?LLF?LRB?LRF
		00000000 00000000 AE47613E    ?HLF?   ?HRF    ?MLF?   ?MRF LLB?LLF?LRB?LRF
		00000000 00000000 AE47613E HLB?   ?HRB?    MLB?   ?MRB?    LLB?LLF?LRB?LRF
		00000000 00000000 AE47613E HLB?   ?HRB?    MLB?   ?MRB?    LLB?LLF?LRB?LRF
		*/

		public float HLF;
		public float MLF;
		public float LLF;

		public float HRF;
		public float MRF;
		public float LRF;

		public float HLB;
		public float MLB;
		public float LLB;

		public float HRB;
		public float MRB;
		public float LRB;

		public void Parse(BinaryReader binaryReader)
		{
			HLF = binaryReader.ReadSingle();
			MLF = binaryReader.ReadSingle();
			LLF = binaryReader.ReadSingle(); 

			HRF = binaryReader.ReadSingle();
			MRF = binaryReader.ReadSingle();
			LRF = binaryReader.ReadSingle();

			HLB = binaryReader.ReadSingle();
			MLB = binaryReader.ReadSingle();
			LLB = binaryReader.ReadSingle();

			HRB = binaryReader.ReadSingle();
			MRB = binaryReader.ReadSingle();
			LRB = binaryReader.ReadSingle();
		}
	}
}
