using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg9_WeenieDefaults
{
	class PagesData
	{
		public int MaxNumPages;
		public int MaxNumCharsPerPage;

		public List<PageData> Pages;

		public void Parse(BinaryReader binaryReader)
		{
			MaxNumPages = binaryReader.ReadInt32();
			MaxNumCharsPerPage = binaryReader.ReadInt32();

			var pagesCount = binaryReader.ReadInt32();

			if (pagesCount > 0)
			{
				Pages = new List<PageData>();

				for (int i = 0; i < pagesCount; i++)
				{
					var page = new PageData();
					page.Parse(binaryReader);
					Pages.Add(page);
				}
			}
		}
	}
}
