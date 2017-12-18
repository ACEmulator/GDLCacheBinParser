using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Palette
	{
		public int ID;

		public List<SubPalette> SubPalettes = new List<SubPalette>();

		public void Parse(BinaryReader binaryReader, int count)
		{
			ID = Util.ReadPackedKnownType(binaryReader, 0x04000000);

            for (int i = 0; i < count; i++)
			{
				var item = new SubPalette();
				item.Parse(binaryReader);
				SubPalettes.Add(item);
			}
		}
	}
}
