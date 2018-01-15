using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Frame
	{
		public Origin Origin;

		public Angles Angles;

		public bool Unpack(BinaryReader binaryReader)
		{
			Origin = new Origin();
			Origin.Unpack(binaryReader);

			Angles = new Angles();
			Angles.Unpack(binaryReader);

			return true;
		}
	}
}
