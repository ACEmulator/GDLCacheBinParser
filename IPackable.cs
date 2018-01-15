using System.IO;

namespace PhatACCacheBinParser
{
	interface IPackable
	{
		bool Unpack(BinaryReader binaryReader);
	}
}
