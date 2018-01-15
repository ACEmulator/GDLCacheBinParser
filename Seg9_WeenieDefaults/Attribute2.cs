using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Attribute2 : Attribute
	{
		public int Current;

		public override bool Unpack(BinaryReader binaryReader)
		{
			base.Unpack(binaryReader);

			Current = binaryReader.ReadInt32();

			return true;
		}
	}
}
