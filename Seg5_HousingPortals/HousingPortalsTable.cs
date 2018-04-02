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
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

			HousingPortals.Unpack(reader);

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
