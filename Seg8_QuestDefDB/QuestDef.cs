using System.IO;

namespace PhatACCacheBinParser.Seg8_QuestDefDB
{
	class QuestDef : IPackable
	{
		public string Name;

		public uint MinDelta;
		public int MaxSolves;

		public string Message;

		public bool Unpack(BinaryReader binaryReader)
		{
			Name = Util.ReadString(binaryReader, true);

			MinDelta = binaryReader.ReadUInt32();
			MaxSolves = binaryReader.ReadInt32();

			Message = Util.ReadEncryptedString1(binaryReader, true);

			return true;
		}
	}
}
