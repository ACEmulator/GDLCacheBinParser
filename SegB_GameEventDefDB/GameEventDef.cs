using System.IO;

using ACE.Entity.Enum;

namespace PhatACCacheBinParser.SegB_GameEventDefDB
{
	class GameEventDef : IUnpackable
	{
        public string Name;

        public int StartTime;
		public int EndTime;

        public GameEventState GameEventState;

		public bool Unpack(BinaryReader reader)
		{
            Name = Util.ReadString(reader, true);

            StartTime = reader.ReadInt32();
            EndTime = reader.ReadInt32();

            GameEventState = (GameEventState)reader.ReadInt32();

			return true;
		}
	}
}
