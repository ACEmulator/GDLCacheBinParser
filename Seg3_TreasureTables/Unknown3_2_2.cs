using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_2_2
    {
        public uint Unknown01;
        public float Unknown02;
        public uint Unknown03;
        public uint Unknown04;
        public uint Unknown05;
        public uint Unknown06;
        public uint Unknown07;
        public uint Unknown08;
        public uint Unknown09;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;

        public void Parse(BinaryReader binaryReader)
        {
            Unknown01 = binaryReader.ReadUInt32();
            Unknown02 = binaryReader.ReadSingle();
            Unknown03 = binaryReader.ReadUInt32();
            Unknown04 = binaryReader.ReadUInt32();
            Unknown05 = binaryReader.ReadUInt32();
            Unknown06 = binaryReader.ReadUInt32();
            Unknown07 = binaryReader.ReadUInt32();
            Unknown08 = binaryReader.ReadUInt32();
            Unknown09 = binaryReader.ReadUInt32();
            Unknown10 = binaryReader.ReadUInt32();
            Unknown11 = binaryReader.ReadUInt32();
            Unknown12 = binaryReader.ReadUInt32();
            Unknown13 = binaryReader.ReadUInt32();
            Unknown14 = binaryReader.ReadUInt32();
            Unknown15 = binaryReader.ReadUInt32();
        }
    }
}
