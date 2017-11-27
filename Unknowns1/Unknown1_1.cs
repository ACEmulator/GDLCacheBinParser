using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Unknowns1
{
	class Unknown1_1
	{
		public uint Unknown;

		public readonly List<uint> Values = new List<uint>();

		public void Parse(BinaryReader binaryReader)
		{
			Unknown = binaryReader.ReadUInt32();

			for (int i = 0; i < 16; i++)
				Values.Add(binaryReader.ReadUInt32());
		}
	}
}
