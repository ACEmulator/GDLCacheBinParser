using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_5_6 : IParseableObject
    {
        public uint Key;

        public Unknown3_5_8 Unknown3_5_8 = new Unknown3_5_8();

        public void Parse(BinaryReader binaryReader)
        {
            // _cache_bin_parse_3_5_6
            Key = binaryReader.ReadUInt32();

            // _cache_bin_parse_3_5_8
            Unknown3_5_8.Parse(binaryReader);
        }
    }
}
