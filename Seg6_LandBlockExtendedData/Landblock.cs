using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg6_LandBlockExtendedData
{
	class Landblock : IUnpackable
	{
		public uint Key;

		public List<Weenie> Weenies;

		public List<Link> Links;

		public bool Unpack(BinaryReader reader)
		{
			int count;


			Key = (reader.ReadUInt32() >> 16);


			count = reader.ReadInt32();

			if (count > 0)
			{
				Weenies = new List<Weenie>();

				for (int i = 0; i < count; i++)
				{
					var weenie = new Weenie();

					weenie.Unpack(reader);

					Weenies.Add(weenie);
				}
			}


			count = reader.ReadInt32();

			if (count > 0)
			{
				Links = new List<Link>();

				for (int i = 0; i < count; i++)
				{
					var link = new Link();

					link.Unpack(reader);

					Links.Add(link);
				}
			}

			return true;
		}
	}
}
