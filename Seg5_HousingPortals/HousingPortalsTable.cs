using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg5_HousingPortals
{
	class HousingPortalsTable : Segment
	{
		public readonly List<HousingPortal> HousingPortals = new List<HousingPortal>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Parse(BinaryReader binaryReader)
		{
			base.Parse(binaryReader);

			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var item = new HousingPortal();
				item.Parse(binaryReader);
				HousingPortals.Add(item);
			}

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.For(0, HousingPortals.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + HousingPortals[i].Unknown1.ToString("X4") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, HousingPortals[i]);
				}
			});

			return true;
		}
	}
}
