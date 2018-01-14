using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
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
