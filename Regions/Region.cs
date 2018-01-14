using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Regions
{
	class Region : IParseableObject
	{
		public readonly List<EncounterTable> EncounterTables = new List<EncounterTable>();

		public void Parse(BinaryReader binaryReader)
		{
			var numTableEntries = binaryReader.ReadInt32();
			for (int i = 0; i < numTableEntries; i++)
			{
				var encounterTable = new EncounterTable();
				encounterTable.Parse(binaryReader);
				EncounterTables.Add(encounterTable);
			}
		}
	}
}
