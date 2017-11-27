using System.IO;

namespace PhatACCacheBinParser.Common
{
	class Angles
	{
		public float W;
		public float X;
		public float Y;
		public float Z;

		public void Parse(BinaryReader binaryReader)
		{
			W = binaryReader.ReadSingle();
			X = binaryReader.ReadSingle();
			Y = binaryReader.ReadSingle();
			Z = binaryReader.ReadSingle();
		}
	}
}
