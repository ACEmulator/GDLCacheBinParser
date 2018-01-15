using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg6_LandBlockExtendedData
{
	class Landblock : IParseableObject
	{
		public uint Key;

		public List<Weenie> Weenies;

		public List<Link> Links;

		public void Parse(BinaryReader binaryReader)
		{
			int count;


			Key = (binaryReader.ReadUInt32() >> 16);


			count = binaryReader.ReadInt32();

			if (count > 0)
			{
				Weenies = new List<Weenie>();

				for (int i = 0; i < count; i++)
				{
					var weenie = new Weenie();

					weenie.Parse(binaryReader);

					Weenies.Add(weenie);
				}
			}


			count = binaryReader.ReadInt32();

			if (count > 0)
			{
				Links = new List<Link>();

				for (int i = 0; i < count; i++)
				{
					var link = new Link();

					link.Parse(binaryReader);

					Links.Add(link);
				}
			}
		}
	}
}
