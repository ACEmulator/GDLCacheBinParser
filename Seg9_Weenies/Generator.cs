using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class Generator
	{
		public float Probability;
		public int Type;
		public double Delay;

		public int InitCreate;

		public int MaxNum;

		public int WhenCreate;
		public int WhereCreate;

		public int StackSize;

		public int PalleteTypeID;
		public float Shade;

		public readonly Position Position = new Position();

		public int Slot;

		public void Parse(BinaryReader binaryReader)
		{
			Probability = binaryReader.ReadSingle();
			Type = binaryReader.ReadInt32();
			Delay = binaryReader.ReadDouble();
			InitCreate = binaryReader.ReadInt32();
			MaxNum = binaryReader.ReadInt32();
			WhenCreate = binaryReader.ReadInt32();
			WhereCreate = binaryReader.ReadInt32();
			StackSize = binaryReader.ReadInt32();

			PalleteTypeID = binaryReader.ReadInt32();
			Shade = binaryReader.ReadSingle();

			Position.Parse(binaryReader);

			Slot = binaryReader.ReadInt32();
		}
	}
}
