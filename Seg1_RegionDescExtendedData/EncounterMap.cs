using System.IO;

namespace PhatACCacheBinParser.Seg1_RegionDescExtendedData
{
    class EncounterMap
	{
		public byte Index;

		public bool Unpack(BinaryReader binaryReader)
		{
			Index = binaryReader.ReadByte();

			return true;
		}
	}
}
