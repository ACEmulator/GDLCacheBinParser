using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg6_LandBlockExtendedData
{
	class LandBlockData : Segment
	{
		public readonly List<Landblock> Landblocks = new List<Landblock>();

		public byte[] TerrainData;

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader binaryReader)
		{
			base.Unpack(binaryReader);

			Landblocks.Unpack(binaryReader);

			TerrainData = binaryReader.ReadBytes((255 * 255) * (9 * 9) * 2);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			if (!Directory.Exists(outputFolder + "LandBlocks\\"))
				Directory.CreateDirectory(outputFolder + "LandBlocks\\");

			Parallel.For(0, Landblocks.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + "LandBlocks\\" + Landblocks[i].Key.ToString("X4") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, Landblocks[i]);
				}
			});

			using (StreamWriter sw = new StreamWriter(outputFolder + "TerrainData.json"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				Serializer.Serialize(writer, TerrainData);
			}

			return true;
		}
	}
}
