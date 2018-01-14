using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class Skill
	{
		// What is PP short for?
		// What is Sac short for?

		public ushort LevelFromPP;
		public int Sac;
		public int PP;
		public int InitLevel;
		public int ResistanceAtLastCheck;
		public double LastUsedTime;

		public void Parse(BinaryReader binaryReader)
		{
			LevelFromPP = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // discard.. this contains a flag (0x10000) that doesn't seem used
			Sac = binaryReader.ReadInt32();
			PP = binaryReader.ReadInt32();
			InitLevel = binaryReader.ReadInt32();
			ResistanceAtLastCheck = binaryReader.ReadInt32();
			LastUsedTime = binaryReader.ReadDouble();
		}
	}
}
