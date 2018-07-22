using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Attribute
	{
		public uint LevelFromCP;
		public uint InitLevel;
		public uint CPSpent;

		public virtual bool Unpack(BinaryReader binaryReader)
		{
			LevelFromCP = binaryReader.ReadUInt32();
			InitLevel = binaryReader.ReadUInt32();
			CPSpent = binaryReader.ReadUInt32();

			return true;
		}
	}
}
