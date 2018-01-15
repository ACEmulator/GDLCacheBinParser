using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.SegA
{
	class UnknownATables : Segment
	{
		public readonly List<UnknownA> UnknownAs = new List<UnknownA>();

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
				var item = new UnknownA();
				item.Parse(binaryReader);
				UnknownAs.Add(item);
			}

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.For(0, UnknownAs.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + UnknownAs[i].Index.ToString("00") + " " + UnknownAs[i].unknown_0_2.ToString("X4") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, UnknownAs[i]);
				}
			});

			return true;
		}
	}
}
