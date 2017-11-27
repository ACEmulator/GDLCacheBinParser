using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Weenies
{
	class Frame
	{
		public Origin Origin;

		public Angles Angles;

		public void Parse(BinaryReader binaryReader)
		{
			Origin = new Origin();
			Origin.Parse(binaryReader);

			Angles = new Angles();
			Angles.Parse(binaryReader);
		}
	}
}
