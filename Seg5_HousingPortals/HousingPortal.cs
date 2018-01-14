using System.Collections.Generic;
using System.IO;

using PhatACCacheBinParser.Common;

namespace PhatACCacheBinParser.Seg5_HousingPortals
{
	class HousingPortal : IParseableObject
	{
		public uint Unknown1;

		public List<Position> Destinations = new List<Position>();

		public void Parse(BinaryReader binaryReader)
		{
			Unknown1 = binaryReader.ReadUInt32();

			var count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				var position = new Position();
				position.Parse(binaryReader);
				Destinations.Add(position);
			}
		}
	}
}
