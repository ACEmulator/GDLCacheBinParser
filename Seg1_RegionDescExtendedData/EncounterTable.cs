﻿using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg1_RegionDescExtendedData
{
	class EncounterTable
	{
		public uint Index;

		public readonly List<uint> Values = new List<uint>();

		public bool Unpack(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadUInt32();

			for (int i = 0; i < 16; i++)
				Values.Add(binaryReader.ReadUInt32());

			return true;
		}
	}
}
