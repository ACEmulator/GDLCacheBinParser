using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Attribute2 : Attribute
	{
		public uint Current;

		public override bool Unpack(BinaryReader binaryReader)
		{
			base.Unpack(binaryReader);

			Current = binaryReader.ReadUInt32();

			return true;
		}
	}
}
