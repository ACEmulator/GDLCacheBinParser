using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class ValuesAndChancesByTier : IUnpackable
	{
	    public readonly List<ValueAndChance> Tier1 = new List<ValueAndChance>();
	    public readonly List<ValueAndChance> Tier2 = new List<ValueAndChance>();
	    public readonly List<ValueAndChance> Tier3 = new List<ValueAndChance>();
	    public readonly List<ValueAndChance> Tier4 = new List<ValueAndChance>();
	    public readonly List<ValueAndChance> Tier5 = new List<ValueAndChance>();
	    public readonly List<ValueAndChance> Tier6 = new List<ValueAndChance>();

		public bool Unpack(BinaryReader reader)
		{
		    Tier1.Unpack(reader);
		    Tier2.Unpack(reader);
		    Tier3.Unpack(reader);
		    Tier4.Unpack(reader);
		    Tier5.Unpack(reader);
		    Tier6.Unpack(reader);

			return true;
		}
	}
}
