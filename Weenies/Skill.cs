using System;
using System.IO;

namespace PhatACCacheBinParser.Weenies
{
	class Skill
	{
		// 3 of these I don't know what order they're in... They're always 0.

		// What is PP short for?
		// What is Sac short for?

		public ushort LevelFromPP; // This order may be incorrect
		public int Sac;
		public int PP; // This order may be incorrect.
		public int InitLevel;
		public int ResistanceAtLastCheck; // This order may be incorrect
		public double LastUsedTime;

		public void Parse(BinaryReader binaryReader)
		{
			var a = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // discard
			Sac = binaryReader.ReadInt32();
			var b = binaryReader.ReadInt32();
			InitLevel = binaryReader.ReadInt32();
			var c = binaryReader.ReadInt32();
			LastUsedTime = binaryReader.ReadDouble();

			if (a != 0 || b != 0 || c != 0)
				throw new NotImplementedException();
		}
	}
}
