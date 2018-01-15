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

		public bool Unpack(BinaryReader binaryReader)
		{
			DType = binaryReader.ReadInt32();

			DVal = binaryReader.ReadInt32();

			DVar = binaryReader.ReadSingle();

			ArmorValues.Unpack(binaryReader);

			BH = binaryReader.ReadInt32();

			SD.Unpack(binaryReader);

			return true;
		}
	}
}
