using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Attribute
	{
		// cp_spent, level_from_cp
		public int a;
		public int InitLevel;
		public int b;

		public virtual bool Unpack(BinaryReader binaryReader)
		{
			a = binaryReader.ReadInt32();
			InitLevel = binaryReader.ReadInt32();
			b = binaryReader.ReadInt32();

			return true;
		}
	}
}
