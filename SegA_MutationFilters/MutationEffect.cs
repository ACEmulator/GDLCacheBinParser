using System.IO;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
	class MutationEffect
	{
	    public EffectArgument ArgQuality = new EffectArgument();

		public uint EffectType;

	    public EffectArgument Arg1 = new EffectArgument();
	    public EffectArgument Arg2 = new EffectArgument();


		public bool Unpack(BinaryReader binaryReader)
		{
		    ArgQuality.Unpack(binaryReader);

			EffectType = binaryReader.ReadUInt32();

		    Arg1.Unpack(binaryReader);
		    Arg2.Unpack(binaryReader);
            
			return true;
		}
	}
}
