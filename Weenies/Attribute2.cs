using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Attribute2 : Attribute
	{
		public int Current;

		public override void Parse(BinaryReader binaryReader)
		{
			base.Parse(binaryReader);

			Current = binaryReader.ReadInt32();
		}
	}
}
