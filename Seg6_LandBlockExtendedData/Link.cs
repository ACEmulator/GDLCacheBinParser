using System.IO;

namespace PhatACCacheBinParser.Seg6_LandBlockExtendedData
{
	class Link
	{
		public uint Source;
		public uint Target;

		public bool Unpack(BinaryReader binaryReader)
		{
			Source = binaryReader.ReadUInt32();
			Target = binaryReader.ReadUInt32();

			return true;
		}
	}
}
