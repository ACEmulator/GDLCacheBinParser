using System.IO;

namespace PhatACCacheBinParser
{
	interface IParseableObject
	{
		void Parse(BinaryReader binaryReader);
	}
}
