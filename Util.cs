using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PhatACCacheBinParser
{
	static class Util
	{
		public static readonly Regex IllegalInFileName = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Compiled);


		public static string ReadString(BinaryReader binaryReader, bool alignTo4Bytes)
		{
			var len = binaryReader.ReadUInt16();

			var bytes = binaryReader.ReadBytes(len);

			if (alignTo4Bytes)
			{
				// Make sure our position is a multiple of 4
				if (binaryReader.BaseStream.Position % 4 != 0)
					binaryReader.BaseStream.Position += 4 - (binaryReader.BaseStream.Position % 4);
			}

			return Encoding.ASCII.GetString(bytes);
		}

		/// <summary>
		/// Each byte of the string has it's upper and lower nibble swapped.
		/// </summary>
		public static string ReadEncryptedString1(BinaryReader binaryReader, bool alignTo4Bytes)
		{
			var len = binaryReader.ReadUInt16();

			var bytes = binaryReader.ReadBytes(len);

			for (int i = 0; i < bytes.Length; i++)
			{
				var b = bytes[i];
				bytes[i] = (byte)((b >> 4) | (b << 4));
			}

			if (alignTo4Bytes)
			{
				// Make sure our position is a multiple of 4
				if (binaryReader.BaseStream.Position % 4 != 0)
					binaryReader.BaseStream.Position += 4 - (binaryReader.BaseStream.Position % 4);
			}

			return Encoding.ASCII.GetString(bytes);
		}
	}
}
