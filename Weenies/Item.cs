﻿using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Item
	{
		public int WCID;
		public int Palette;
		public float Shade;
		public int Destination;
		public int StackSize;
		public bool TryToBond;

		public void Parse(BinaryReader binaryReader)
		{
			WCID = binaryReader.ReadInt32();
			Palette = binaryReader.ReadInt32();
			Shade = binaryReader.ReadSingle();
			Destination = binaryReader.ReadInt32();
			StackSize = binaryReader.ReadInt32();
			// TryToBond
			var c = binaryReader.ReadInt32();

			if (c != 0)
				;//throw new System.Exception();
		}
	}
}
