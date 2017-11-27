using System;
using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Weenies
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

		// I don't know the order of these, they're always 0.
		public float Shade;
		public int PalleteTypeID;

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

			// I don't know the order of these, they're always 0.
			// Shade, PTID
			var a = binaryReader.ReadInt32();
			var b = binaryReader.ReadInt32();

			if (a != b || a != 0)
				throw new NotImplementedException();

			Position.Parse(binaryReader);

			Slot = binaryReader.ReadInt32();
		}
	}
}
