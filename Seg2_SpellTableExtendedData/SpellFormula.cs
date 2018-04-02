using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg2_SpellTableExtendedData
{
    class SpellFormula : IUnpackable
    {
        public List<uint> Comps { get; } = new List<uint>();

        public bool Unpack(BinaryReader reader)
        {
            for (int i = 0; i < 8; i++)
                Comps.Add(reader.ReadUInt32());

            return true;
        }
    }
}
