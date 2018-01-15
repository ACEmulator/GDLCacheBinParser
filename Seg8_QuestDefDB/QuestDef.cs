﻿using System.IO;

namespace PhatACCacheBinParser.Seg8_QuestDefDB
{
	class QuestDef : IParseableObject
	{
		public string Name;

		public uint MinDelta;
		public int MaxSolves;

		public string Message;

		public void Parse(BinaryReader binaryReader)
		{
			Name = Util.ReadString(binaryReader, true);

			MinDelta = binaryReader.ReadUInt32();
			MaxSolves = binaryReader.ReadInt32();

			Message = Util.ReadEncryptedString1(binaryReader, true);
		}
	}
}