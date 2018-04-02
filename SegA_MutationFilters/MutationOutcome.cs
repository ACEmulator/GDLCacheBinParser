using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
	class MutationOutcome
	{
		public readonly List<MutationEffectList> EffectList = new List<MutationEffectList>();


		public bool Unpack(BinaryReader binaryReader)
		{
			int count;


			count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var mutationEffectList = new MutationEffectList();
				mutationEffectList.Unpack(binaryReader);
				EffectList.Add(mutationEffectList);
			}

			return true;
		}
	}
}
