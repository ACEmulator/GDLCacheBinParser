using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Generator
	{
		public float Probability;
		public uint Type;
		public double Delay;

		public uint InitCreate;

		public uint MaxNum;

		public uint WhenCreate;
		public uint WhereCreate;

		public int StackSize;

		public uint PalleteTypeID;
		public float Shade;

		public readonly Position Position = new Position();

		public int Slot;

		public bool Unpack(BinaryReader binaryReader)
		{
			Probability = binaryReader.ReadSingle();
			Type = binaryReader.ReadUInt32();
			Delay = binaryReader.ReadDouble();
			InitCreate = binaryReader.ReadUInt32();
			MaxNum = binaryReader.ReadUInt32();
			WhenCreate = binaryReader.ReadUInt32();
			WhereCreate = binaryReader.ReadUInt32();
			StackSize = binaryReader.ReadInt32();

			PalleteTypeID = binaryReader.ReadUInt32();
			Shade = binaryReader.ReadSingle();

			Position.Unpack(binaryReader);

			Slot = binaryReader.ReadInt32();

			return true;
		}
	}
}
