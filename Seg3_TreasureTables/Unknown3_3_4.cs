using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTables
{
    class Unknown3_3_4 : IParseableObject
    {
        public uint WCID_SometimesNotAlways;
        public double Unknown02;

        public void Parse(BinaryReader binaryReader)
        {
            WCID_SometimesNotAlways = binaryReader.ReadUInt32();
            Unknown02 = binaryReader.ReadDouble();
        }
    }
}
