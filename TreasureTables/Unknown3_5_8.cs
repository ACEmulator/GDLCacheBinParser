using System.IO;

namespace PhatACCacheBinParser.TreasureTables
{
    class Unknown3_5_8
    {
        public float Unknown01;
        public float Unknown02;
        public float Unknown03;
        public float Unknown04;
        public float Unknown05;
        public float Unknown06;

        public void Parse(BinaryReader binaryReader)
        {
            Unknown01 = binaryReader.ReadSingle();
            Unknown02 = binaryReader.ReadSingle();
            Unknown03 = binaryReader.ReadSingle();
            Unknown04 = binaryReader.ReadSingle();
            Unknown05 = binaryReader.ReadSingle();
            Unknown06 = binaryReader.ReadSingle();
        }
    }
}
