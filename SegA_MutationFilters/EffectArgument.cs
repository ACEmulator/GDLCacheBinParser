using System.IO;

namespace PhatACCacheBinParser.SegA_MutationFilters
{
    class EffectArgument
    {
        public int Type;

        /*union 
	    {
		    BYTE _raw[8];
            double dbl_value;
            int int_value;
            struct quality_value_s
            {
                StatType statType;

                union
			    {
				    int statIndex;
                    STypeInt intStat;
                    STypeFloat floatStat;
                };
            } quality_value;
		    struct range_value_s
            {
                float min;
                float max;
            } range_value;
	    };*/
        public byte[] Raw;

        public bool Unpack(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadInt32();

            Raw = binaryReader.ReadBytes(8);

            return true;
        }
    }
}
