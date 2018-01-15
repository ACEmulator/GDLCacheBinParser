using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg8_QuestDefDB
{
	class QuestDefDB : Segment
	{
		public readonly List<QuestDef> QuestDefs = new List<QuestDef>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader binaryReader)
		{
			base.Unpack(binaryReader);

			QuestDefs.Unpack(binaryReader);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.For(0, QuestDefs.Count, i =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + Util.IllegalInFileName.Replace(QuestDefs[i].Name, "_") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, QuestDefs[i]);
				}
			});

			return true;
		}
	}
}
