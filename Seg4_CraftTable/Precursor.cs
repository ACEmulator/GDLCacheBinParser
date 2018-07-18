using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
    class Precursor
    {
        public uint RecipeID;
        public uint Target;
        public uint Source;

        public bool Unpack(BinaryReader binaryReader)
        {
            Target = binaryReader.ReadUInt32();
            Source = binaryReader.ReadUInt32();

            RecipeID = binaryReader.ReadUInt32();

            return true;
        }
    }
}