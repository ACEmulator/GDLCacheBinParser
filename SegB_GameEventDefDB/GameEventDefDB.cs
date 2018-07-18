using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.SegB_GameEventDefDB
{
	class GameEventDefDB : Segment
	{
		public readonly List<GameEventDef> GameEventDefs = new List<GameEventDef>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

            GameEventDefs.Unpack(reader);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

            Parallel.For(0, GameEventDefs.Count, i =>
            {
                using (StreamWriter sw = new StreamWriter(outputFolder + Util.IllegalInFileName.Replace(GameEventDefs[i].Name, "_") + ".json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    Serializer.Serialize(writer, GameEventDefs[i]);
                }
            });

            return true;
		}
	}
}
