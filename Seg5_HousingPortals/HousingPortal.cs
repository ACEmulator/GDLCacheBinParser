using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg5_HousingPortals
{
	class HousingPortal : IUnpackable
	{
		public uint Unknown1;

		public List<Position> Destinations = new List<Position>();

		public bool Unpack(BinaryReader reader)
		{
			Unknown1 = reader.ReadUInt32();

			var count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var position = new Position();
				position.Unpack(reader);
				Destinations.Add(position);
			}

			return true;
		}
	}
}
