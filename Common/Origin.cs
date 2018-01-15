using System.IO;

namespace PhatACCacheBinParser.Common
{
	class Origin
	{
		public float X;
		public float Y;
		public float Z;

		public bool Unpack(BinaryReader binaryReader)
		{
			X = binaryReader.ReadSingle();
			Y = binaryReader.ReadSingle();
			Z = binaryReader.ReadSingle();

			return true;
		}
	}
}
