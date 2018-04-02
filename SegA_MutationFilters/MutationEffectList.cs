using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
	class MutationEffectList
	{
		public double Probability;

		public readonly List<MutationEffect> Effects = new List<MutationEffect>();


		public bool Unpack(BinaryReader binaryReader)
		{
			int count;


			Probability = binaryReader.ReadDouble();

			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var mutationEffect = new MutationEffect();
				mutationEffect.Unpack(binaryReader);
				Effects.Add(mutationEffect);
			}

			return true;
		}
	}
}
