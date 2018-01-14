using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.TreasureTables
{
    class Unknown3_3_3 : IParseableObject
    {
        public List<Unknown3_3_4> Unknown3_3_4 = new List<Unknown3_3_4>();

        public void Parse(BinaryReader binaryReader)
        {
            // _cache_bin_parse_3_3_3
            var count = binaryReader.ReadUInt32();

            for (int i = 0; i < count; i++)
            {
                // _cache_bin_parse_3_3_4
                var unknown3_3_4 = new Unknown3_3_4();
                unknown3_3_4.Parse(binaryReader);
                Unknown3_3_4.Add(unknown3_3_4);
            }
        }
    }
}
