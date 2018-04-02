using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg4_CraftTable
{
	class Recipe : IUnpackable
	{
		// _cache_bin_parse_4_1
		public uint ID;

		// _cache_bin_parse_4_2
		public uint unknown_1;
		public uint Skill;
		public uint Difficulty;
		public uint unknown_4;

		public uint SuccessWCID;
		public uint SuccessAmount;
		public string SuccessMessage;

		public uint FailWCID;
		public uint FailAmount;
		public string FailMessage;

		public List<Recipe_Component> Components = new List<Recipe_Component>();

		public List<Recipe_Requirement> Requirements = new List<Recipe_Requirement>();

	    public List<Recipe_Mod> Mods = new List<Recipe_Mod>();

	    public uint DataID;

        public bool Unpack(BinaryReader reader)
        {
			// _cache_bin_parse_4_1
			ID = reader.ReadUInt32();

			// _cache_bin_parse_4_2
			unknown_1 = reader.ReadUInt32();
			Skill = reader.ReadUInt32();
			Difficulty = reader.ReadUInt32();
			unknown_4 = reader.ReadUInt32();

			SuccessWCID = reader.ReadUInt32();
			SuccessAmount = reader.ReadUInt32();
			SuccessMessage = Util.ReadString(reader, true);

			FailWCID = reader.ReadUInt32();
			FailAmount = reader.ReadUInt32();
			FailMessage = Util.ReadString(reader, true);

			for (int i = 0; i < 4; i++)
			{
				var item = new Recipe_Component();
				item.Unpack(reader);
				Components.Add(item);
			}

			for (int i = 0; i < 3; i++)
			{
				var item = new Recipe_Requirement();
				item.Unpack(reader);
				Requirements.Add(item);
			}

		    for (int i = 0; i < 8; i++)
		    {
		        var item = new Recipe_Mod();
		        item.Unpack(reader);
		        Mods.Add(item);
            }

            DataID = reader.ReadUInt32();

	        return true;
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
