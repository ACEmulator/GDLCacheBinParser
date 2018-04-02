using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg1_RegionDescExtendedData
{
	class RegionDescExtendedData : Segment
	{
		public readonly List<EncounterTable> EncounterTables = new List<EncounterTable>();

		public byte[] EncounterMap;

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

			// For Segment 1, the first dword appears to simply be an is present flag
			// The value is 256, which is probably tied to the number of landblocks or landblock width or something.
			/*var totalObjects = */reader.ReadUInt16();
			reader.ReadUInt16(); // Discard

			var numTableEntries = reader.ReadInt32();
			for (int i = 0; i < numTableEntries; i++)
			{
				var item = new EncounterTable();
				item.Unpack(reader);
				EncounterTables.Add(item);
			}

			EncounterMap = reader.ReadBytes(255 * 255);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			using (StreamWriter sw = new StreamWriter(outputFolder + "EncounterTables.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				Serializer.Serialize(writer, EncounterTables);
			}

			using (StreamWriter sw = new StreamWriter(outputFolder + "EncounterMap.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				Serializer.Serialize(writer, EncounterMap);

			}
			return true;
		}
	}
}
