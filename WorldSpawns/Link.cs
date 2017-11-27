using System.IO;

namespace PhatACCacheBinParser.WorldSpawns
{
	class Link
	{
		public uint Source;
		public uint Target;

		public void Parse(BinaryReader binaryReader)
		{
			Source = binaryReader.ReadUInt32();
			Target = binaryReader.ReadUInt32();
		}
	}
}
