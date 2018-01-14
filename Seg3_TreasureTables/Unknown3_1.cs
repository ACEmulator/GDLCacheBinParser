using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_1 : IParseableObject
    {
        public uint Key;

        public readonly List<Unknown3_1_4> Unknown3_1_4 = new List<Unknown3_1_4>();

        public void Parse(BinaryReader binaryReader)
        {
            // _cache_bin_parse_3_1_1
            Key = binaryReader.ReadUInt32();


            // _cache_bin_parse_3_1_3
            var count = binaryReader.ReadUInt32();

            for (int i = 0; i < count; i++)
            {
                // _cache_bin_parse_3_1_4
                var unknown3_1_4 = new Unknown3_1_4();
                unknown3_1_4.Parse(binaryReader);
                Unknown3_1_4.Add(unknown3_1_4);
            }
        }
    }
}
