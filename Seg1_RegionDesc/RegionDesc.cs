using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg1_RegionDesc
{
	class RegionDesc : IParseableObject
	{
		public readonly List<EncounterTable> EncounterTables = new List<EncounterTable>();

		public byte[] EncounterMap;

		public void Parse(BinaryReader binaryReader)
		{
			var numTableEntries = binaryReader.ReadInt32();
			for (int i = 0; i < numTableEntries; i++)
			{
				var encounterTable = new EncounterTable();
				encounterTable.Parse(binaryReader);
				EncounterTables.Add(encounterTable);
			}

			EncounterMap = binaryReader.ReadBytes(255 * 255);
		}
	}
}
