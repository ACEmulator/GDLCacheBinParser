using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_1_4
    {
        public uint Unknown01;
        public uint Unknown02;
        public uint Unknown03;
        public float Unknown04;
        public uint Unknown05;
        public float Unknown06;
        public float Unknown07;
        public uint Unknown08;
        public uint Unknown09;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public uint Unknown16;
        public uint Unknown17;

        public void Parse(BinaryReader binaryReader)
        {
            Unknown01 = binaryReader.ReadUInt32();
            Unknown02 = binaryReader.ReadUInt32();
            Unknown03 = binaryReader.ReadUInt32();
            Unknown04 = binaryReader.ReadSingle();
            Unknown05 = binaryReader.ReadUInt32();
            Unknown06 = binaryReader.ReadSingle();
            Unknown07 = binaryReader.ReadSingle();
            Unknown08 = binaryReader.ReadUInt32();
            Unknown09 = binaryReader.ReadUInt32();
            Unknown10 = binaryReader.ReadUInt32();
            Unknown11 = binaryReader.ReadUInt32();
            Unknown12 = binaryReader.ReadUInt32();
            Unknown13 = binaryReader.ReadUInt32();
            Unknown14 = binaryReader.ReadUInt32();
            Unknown15 = binaryReader.ReadUInt32();
            Unknown16 = binaryReader.ReadUInt32();
            Unknown17 = binaryReader.ReadUInt32();
        }
    }
}
