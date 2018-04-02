using System.IO;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class SpellStatMod : IPackable
    {
        public uint Type;
        public uint Key;
        public float Val;

        public bool Unpack(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadUInt32();
            Key = binaryReader.ReadUInt32();
            Val = binaryReader.ReadSingle();

            return true;
        }
    }
}
