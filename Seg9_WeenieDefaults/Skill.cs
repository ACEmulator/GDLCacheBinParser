using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class Skill
	{
		// What is PP short for?
		// What is Sac short for?

		public ushort LevelFromPP;
		public uint Sac;
		public uint PP;
		public uint InitLevel;
		public uint ResistanceAtLastCheck;
		public double LastUsedTime;

		public bool Unpack(BinaryReader binaryReader)
		{
			LevelFromPP = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // discard.. this contains a flag (0x10000) that doesn't seem used
		    Sac = binaryReader.ReadUInt32();
			PP = binaryReader.ReadUInt32();
			InitLevel = binaryReader.ReadUInt32();
			ResistanceAtLastCheck = binaryReader.ReadUInt32();
			LastUsedTime = binaryReader.ReadDouble();

			return true;
		}
	}
}
