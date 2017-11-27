using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Attribute
	{
		// cp_spent, level_from_cp
		public int a;
		public int InitLevel;
		public int b;

		public virtual void Parse(BinaryReader binaryReader)
		{
			a = binaryReader.ReadInt32();
			InitLevel = binaryReader.ReadInt32();
			b = binaryReader.ReadInt32();
		}
	}
}
