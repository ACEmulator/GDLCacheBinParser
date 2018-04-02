using System.IO;

namespace PhatACCacheBinParser
{
	interface IUnpackable
	{
		bool Unpack(BinaryReader reader);
	}
}
