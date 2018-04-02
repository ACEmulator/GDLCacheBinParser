using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
	class MutationFilters : Segment
	{
	    public readonly Dictionary<uint, List<Mutation>> Mutations = new Dictionary<uint, List<Mutation>>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

		    var count = reader.ReadInt32();

		    for (int i = 0; i < count; i++)
		    {
		        var key = reader.ReadUInt32();

		        var mutations = new List<Mutation>();

		        var dataLength = reader.ReadUInt32();

		        var count2 = reader.ReadInt32();
		        for (int j = 0; j < count2; j++)
		        {
		            var mutation = new Mutation();
		            mutation.Unpack(reader);
		            mutations.Add(mutation);
		        }

		        Mutations.Add(key, mutations);
		    }

		    return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

			Parallel.ForEach(Mutations, mutation =>
			{
				using (StreamWriter sw = new StreamWriter(outputFolder + mutation.Key.ToString("00") + ".json"))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					Serializer.Serialize(writer, mutation.Value);
				}
			});

			return true;
		}
	}
}
