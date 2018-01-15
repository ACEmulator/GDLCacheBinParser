using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class WeenieDefaults : Segment
	{
		public readonly List<Weenie> Weenies = new List<Weenie>();

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
				// Skip the next 4 bytes. It is the wcid, but the code doesn't pull it from here.
				binaryReader.ReadInt32();

				var item = new Weenie();
				item.Parse(binaryReader);
				Weenies.Add(item);
			}

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.For(0, Weenies.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + Weenies[i].WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(Weenies[i].Description, "_") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, Weenies[i]);
				}
			});

			return true;
		}
	}
}
