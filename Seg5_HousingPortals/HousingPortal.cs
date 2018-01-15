using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg5_HousingPortals
{
	class HousingPortal : IPackable
	{
		public uint Unknown1;

		public List<Position> Destinations = new List<Position>();

		public bool Unpack(BinaryReader binaryReader)
		{
			Unknown1 = binaryReader.ReadUInt32();

			var count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var position = new Position();
				position.Unpack(binaryReader);
				Destinations.Add(position);
			}

			return true;
		}
	}
}
