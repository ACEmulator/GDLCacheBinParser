using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class CraftingTable : Segment
	{
		public readonly List<Recipe> Recipes = new List<Recipe>();

		// I don't know what this is
		public readonly List<Tuple<ulong, uint>> Precursors = new List<Tuple<ulong, uint>>();

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
				var item = new Recipe();
				item.Parse(binaryReader);
				Recipes.Add(item);
			}

			// The recipe heap seems to have a map after the main recipe heap. Precursor maybe?
			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard

			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt64(); // key       Even though this is 64 bits, it appears to be made up of 2 uint32 values
				var value = binaryReader.ReadUInt32(); // value

				Precursors.Add(new Tuple<ulong, uint>(key, value));
			}

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			if (!Directory.Exists(outputFolder + "Recipes\\"))
				Directory.CreateDirectory(outputFolder + "Recipes\\");

			Parallel.For(0, Recipes.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + "Recipes\\" + Recipes[i].ID.ToString("00000") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, Recipes[i]);
				}
			});

			using (StreamWriter sw = new StreamWriter(outputFolder + "Precursors.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				Serializer.Serialize(writer, Precursors);
			}

			return true;
		}
	}
}
