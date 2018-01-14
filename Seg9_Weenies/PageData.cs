using System;
using System.IO;

namespace PhatACCacheBinParser.Seg9_Weenies
{
	class PageData
	{
		public uint AuthorID;
		public string AuthorName;
		public string AuthorAccount;
		public bool IgnoreAuthor;

		public string Text;

		public void Parse(BinaryReader binaryReader)
		{
			AuthorID = binaryReader.ReadUInt32();

			AuthorName = Util.ReadString(binaryReader, true);

			AuthorAccount = Util.ReadString(binaryReader, true);

			var a = binaryReader.ReadUInt32(); // 02 00 FF FF Flag count?
			if (a != 0xFFFF0002)
				throw new Exception();
			var b = binaryReader.ReadInt32(); // 01 00 00 00 Key?
			if (b != 0x01)
				throw new Exception();
			var c = binaryReader.ReadInt32(); // 00 00 00 00 Value? ignoreAuthor false
			IgnoreAuthor = Convert.ToBoolean(c);

			Text = Util.ReadString(binaryReader, true);
		}
	}
}
