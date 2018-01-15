using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
	class SpellTableExtendedData : Segment
	{
		public readonly List<Spell> Spells = new List<Spell>();

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
				var item = new Spell();
				item.Parse(binaryReader);
				Spells.Add(item);
			}

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.For(0, Spells.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + Spells[i].ID.ToString("0000") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, Spells[i]);
				}
			});

			return true;
		}
	}
}
