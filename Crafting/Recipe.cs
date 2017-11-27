using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Crafting
{
	class Recipe : IParseableObject
	{
		// _cache_bin_parse_4_1
		public uint ID;

		// _cache_bin_parse_4_2
		public uint unknown_1;
		public uint unknown_2;
		public uint unknown_3;
		public uint unknown_4;

		public uint unknown_5;
		public uint unknown_6;

		public string SuccessMessage;

		public uint unknown_7;
		public uint unknown_8;

		public string FailMessage;

		public List<Recipe_Unknown_1> Unknowns_1 = new List<Recipe_Unknown_1>();

		public List<Recipe_Unknown_2> Unknowns_2 = new List<Recipe_Unknown_2>();


		public void Parse(BinaryReader binaryReader)
		{
			// _cache_bin_parse_4_1
			ID = binaryReader.ReadUInt32();

			// _cache_bin_parse_4_2
			unknown_1 = binaryReader.ReadUInt32();
			unknown_2 = binaryReader.ReadUInt32();
			unknown_3 = binaryReader.ReadUInt32();
			unknown_4 = binaryReader.ReadUInt32();
			unknown_5 = binaryReader.ReadUInt32();
			unknown_6 = binaryReader.ReadUInt32();

			SuccessMessage = Util.ReadString(binaryReader, true);

			unknown_7 = binaryReader.ReadUInt32();
			unknown_8 = binaryReader.ReadUInt32();

			FailMessage = Util.ReadString(binaryReader, true);

			for (int i = 0; i < 4; i++)
			{
				var unknown = new Recipe_Unknown_1();
				unknown.Parse(binaryReader);
				Unknowns_1.Add(unknown);
			}

			for (int i = 0; i < 3; i++)
			{
				var unknown = new Recipe_Unknown_2();
				unknown.Parse(binaryReader);
				Unknowns_2.Add(unknown);
			}

			// Jump right here

			// hack to find the next recipe
			if (ID == 0x13C2)
				return;

			// Make sure our position is a multiple of 4
			if (binaryReader.BaseStream.Position % 4 != 0)
				binaryReader.BaseStream.Position += 4 - (binaryReader.BaseStream.Position % 4);

			while (true)
			{
				var position = binaryReader.BaseStream.Position;

				var temp = binaryReader.ReadUInt32();

				if (temp == ID + 1)
				{
					binaryReader.BaseStream.Position = position;
					return;
				}
			}
		}


		public string GetFriendlyFileName()
		{
			if (string.IsNullOrEmpty(SuccessMessage))
				return null;

			string result = SuccessMessage.Substring(0, SuccessMessage.Length - 1);

			/*if (SuccessMessage.StartsWith("You ")) result = SuccessMessage.Substring("You ".Length, SuccessMessage.Length - "You ".Length - 1);
			else if (SuccessMessage.StartsWith("You've ")) result = SuccessMessage.Substring("You've ".Length, SuccessMessage.Length - "You've ".Length - 1);
			else if (SuccessMessage.StartsWith("The")) result = SuccessMessage.Substring("The ".Length, SuccessMessage.Length - "The ".Length - 1);
			else if (SuccessMessage.StartsWith("As you ")) result = SuccessMessage.Substring("As you ".Length, SuccessMessage.Length - "As you ".Length - 1);
			else if (SuccessMessage.StartsWith("As the ")) result = SuccessMessage.Substring("As the ".Length, SuccessMessage.Length - "As the ".Length - 1);
			else if (SuccessMessage.StartsWith("With a burst of warmth and heat, ")) result = SuccessMessage.Substring("With a burst of warmth and heat, ".Length, SuccessMessage.Length - "With a burst of warmth and heat, ".Length - 1);
			else if (SuccessMessage.StartsWith("Well, you've certainly ")) result = SuccessMessage.Substring("Well, you've certainly ".Length, SuccessMessage.Length - "Well, you've certainly ".Length - 1);*/

			if (result != null)
			{
				if (result.Length > 50)
					result = result.Substring(0, 50);

				result = Util.IllegalInFileName.Replace(result, "_");
			}

			return result;
		}
	}
}
