using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_2 : IParseableObject
    {
        public uint Key;

        public Unknown3_2_2 Unknown3_2_2 = new Unknown3_2_2();

        public void Parse(BinaryReader binaryReader)
        {
            // _cache_bin_parse_3_2_1
            Key = binaryReader.ReadUInt32();

            // _cache_bin_parse_3_2_2
            Unknown3_2_2.Parse(binaryReader);
        }
    }
}
