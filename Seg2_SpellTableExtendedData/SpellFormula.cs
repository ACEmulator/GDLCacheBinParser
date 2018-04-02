using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class SpellFormula : IPackable
    {
        public List<uint> Comps { get; } = new List<uint>();

        public bool Unpack(BinaryReader binaryReader)
        {
            for (int i = 0; i < 8; i++)
                Comps.Add(binaryReader.ReadUInt32());

            return true;
        }
    }
}
