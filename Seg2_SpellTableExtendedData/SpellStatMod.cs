using System.IO;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class SpellStatMod : IUnpackable
    {
        public uint Type;
        public uint Key;
        public float Val;

        public bool Unpack(BinaryReader reader)
        {
            Type = reader.ReadUInt32();
            Key = reader.ReadUInt32();
            Val = reader.ReadSingle();

            return true;
        }
    }
}
