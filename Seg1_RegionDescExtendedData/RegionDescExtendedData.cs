using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg1_RegionDescExtendedData
{
	class RegionDescExtendedData : Segment
	{
		public readonly List<EncounterTable> EncounterTables = new List<EncounterTable>();

        //public byte[] EncounterMap;

        public List<EncounterMap> EncounterMaps = new List<EncounterMap>();

        /// <summary>
        /// You can only call Parse() once on an instantiated object.
        /// </summary>
        public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

            var totalObjects = reader.ReadUInt32();

            var numTableEntries = reader.ReadUInt32();
			for (int i = 0; i < numTableEntries; i++)
			{
				var item = new EncounterTable();
				item.Unpack(reader);
				EncounterTables.Add(item);
			}

            //EncounterMap = reader.ReadBytes(255 * 255);

            for (var i = 0; i < (255 * 255); i++)
            {
                var item = new EncounterMap();
                item.Unpack(reader);
                EncounterMaps.Add(item);
            }

            reader.ReadBytes(3); // discard

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

			using (StreamWriter sw = new StreamWriter(outputFolder + "EncounterMaps.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
                Serializer.Serialize(writer, EncounterMaps);

            }
			return true;
		}
	}
}
