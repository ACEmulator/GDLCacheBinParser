using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
	class Mutation : IUnpackable
	{
		public readonly List<double> Chances = new List<double>();

		public readonly List<MutationOutcome> Outcomes = new List<MutationOutcome>();


		public bool Unpack(BinaryReader reader)
		{
			int count;


			count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				Chances.Add(reader.ReadDouble());

			count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var mutationOutcome = new MutationOutcome();
				mutationOutcome.Unpack(reader);
				Outcomes.Add(mutationOutcome);
			}

			return true;
		}
	}
}
