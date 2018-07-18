using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Attribute
	{
		public int LevelFromCP;
		public int InitLevel;
		public int CPSpent;

		public virtual bool Unpack(BinaryReader binaryReader)
		{
			LevelFromCP = binaryReader.ReadInt32();
			InitLevel = binaryReader.ReadInt32();
			CPSpent = binaryReader.ReadInt32();

			return true;
		}
	}
}
