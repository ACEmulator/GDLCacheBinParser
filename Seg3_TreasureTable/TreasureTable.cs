using System;
using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureTable : Segment
	{
		public Dictionary<uint, List<TreasureEntry>> _wieldedTreasure; // hashA
		public Dictionary<uint, TreasureEntry2> _treasure2; // hashB
		//public Dictionary<uint, List<TreasureEntry5>> _treasure3[4]; // hashC x 4
		//public TreasureEntry7 _treasure7[48]; // listyA x 48
		public Dictionary<uint, List<TreasureEntry5>> _treasure8; // hash D

		public TreasureEntry7 _healerChances;
		public TreasureEntry7 _lockpickChances;
		public TreasureEntry7 _consumableChances;
		public TreasureEntry7 _peaChances;

		public Dictionary<uint, TreasureEntry6> _itemBaneSpells;
		public Dictionary<uint, TreasureEntry6> _meleeWeaponSpells;
		public Dictionary<uint, TreasureEntry6> _missileWeaponSpells;
		public Dictionary<uint, TreasureEntry6> _casterWeaponSpells;

		public TreasureEntry7 _spellLevelProbabilitiesMaybe; // listyA
		public List<List<TreasureEntry5>> _keyedSpells; // listyBcount + listyB

		public TreasureEntry7 _otherBuffCasterSpell;
		public TreasureEntry7 _otherWarCasterSpell;

		public TreasureEntry7 _scrollChances; // listyA
		public TreasureEntry7 _manaStoneChances; // listyA
		public Dictionary<uint, TreasureEntry7> _materialCodeToBaseMaterialMap; // hash F

		public TreasureEntry7 _ceramicMaterials;
		public TreasureEntry7 _clothMaterials;
		public TreasureEntry7 _leatherMaterials;
		public TreasureEntry7 _metalMaterials;
		public TreasureEntry7 _stoneMaterials;
		public TreasureEntry7 _woodMaterials;

		//public TreasureEntry7 _treasure18[2]; // listyA x 8
		public Dictionary<uint, TreasureEntry6> _treasure19; // hash G
		public Dictionary<uint, TreasureEntry7> _treasure20; // hashF
		public TreasureEntry7 _gemProbabilitiesMaybe; // listyA
		public Dictionary<uint, List<TreasureEntry5>> _gemMaterials; // hashD
		public Dictionary<uint, float> _materialValueAddedPossibly; // hashH
		//public List<TreasureEntry5> _treasure24[3]; // listyB x3
		public Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>> _materialColorKeyMap;
		//public TreasureEntry7 _cantripChances[5];

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Parse(BinaryReader binaryReader)
		{
			base.Parse(binaryReader);



			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);


			return true;
		}
	}
}
