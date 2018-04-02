using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class WeenieDefaults : Segment
	{
		public readonly Dictionary<uint, Weenie> Weenies = new Dictionary<uint, Weenie>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

			Weenies.Unpack(reader);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.ForEach(Weenies, weenie =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + weenie.Value.WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(weenie.Value.Description, "_") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, weenie.Value);
				}
			});

			return true;
		}
	}
}
