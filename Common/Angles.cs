using System.IO;

namespace PhatACCacheBinParser.Common
{
	class Angles
	{
		public float W;
		public float X;
		public float Y;
		public float Z;

		public bool Unpack(BinaryReader binaryReader)
		{
			W = binaryReader.ReadSingle();
			X = binaryReader.ReadSingle();
			Y = binaryReader.ReadSingle();
			Z = binaryReader.ReadSingle();

			return true;
		}
	}
}
