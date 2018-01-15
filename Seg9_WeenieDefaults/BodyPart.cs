using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class BodyPart
	{
		public int DType;

		public int DVal;

		public float DVar;

		public readonly ArmorValues ArmorValues = new ArmorValues();

		public int BH;

		public readonly BodyPartSD SD = new BodyPartSD();

		public void Parse(BinaryReader binaryReader)
		{
			DType = binaryReader.ReadInt32();

			DVal = binaryReader.ReadInt32();

			DVar = binaryReader.ReadSingle();

			ArmorValues.Parse(binaryReader);

			BH = binaryReader.ReadInt32();

			SD.Parse(binaryReader);
		}
	}
}
