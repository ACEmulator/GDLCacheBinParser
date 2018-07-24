using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Item
	{
		public uint WCID;
		public sbyte Palette;
		public float Shade;
		public sbyte Destination;
		public int StackSize;
		public bool TryToBond;

		public bool Unpack(BinaryReader binaryReader)
		{
			WCID = binaryReader.ReadUInt32();

			var tempPalette = binaryReader.ReadInt32();
		    if (tempPalette < sbyte.MinValue || tempPalette > sbyte.MaxValue)
		        throw new System.ArgumentOutOfRangeException();
            Palette = (sbyte)tempPalette;

            Shade = binaryReader.ReadSingle();

			var tempDestination = binaryReader.ReadInt32();
		    if (tempDestination < sbyte.MinValue || tempDestination > sbyte.MaxValue)
		        throw new System.ArgumentOutOfRangeException();
            Destination = (sbyte)tempDestination;

			StackSize = binaryReader.ReadInt32();
			TryToBond = (binaryReader.ReadInt32() == 1);

			return true;
		}
	}
}
