using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureTable : Segment
	{
		public readonly Dictionary<uint, List<TreasureEntry>> _wieldedTreasure =new Dictionary<uint, List<TreasureEntry>>(); // hashA
		public readonly Dictionary<uint, TreasureEntry2> _treasure2 = new Dictionary<uint, TreasureEntry2>(); // hashB
		public readonly List<Dictionary<uint, List<TreasureEntry5>>> _treasure3 = new List<Dictionary<uint, List<TreasureEntry5>>>();
		public readonly List<TreasureEntry7> _treasure7 = new List<TreasureEntry7>();
		public readonly Dictionary<uint, List<TreasureEntry5>> _treasure8 = new Dictionary<uint, List<TreasureEntry5>>(); // hash D

		public readonly TreasureEntry7 _healerChances = new TreasureEntry7();
		public readonly TreasureEntry7 _lockpickChances = new TreasureEntry7();
		public readonly TreasureEntry7 _consumableChances = new TreasureEntry7();
		public readonly TreasureEntry7 _peaChances = new TreasureEntry7();

		public readonly Dictionary<uint, TreasureEntry6> _itemBaneSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> _meleeWeaponSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> _missileWeaponSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> _casterWeaponSpells = new Dictionary<uint, TreasureEntry6>();

		public readonly TreasureEntry7 _spellLevelProbabilitiesMaybe = new TreasureEntry7(); // listyA
		public readonly List<List<TreasureEntry5>> _keyedSpells = new List<List<TreasureEntry5>>(); // listyBcount + listyB

		public readonly TreasureEntry7 _otherBuffCasterSpell = new TreasureEntry7();
		public readonly TreasureEntry7 _otherWarCasterSpell = new TreasureEntry7();

		public readonly TreasureEntry7 _scrollChances = new TreasureEntry7(); // listyA
		public readonly TreasureEntry7 _manaStoneChances = new TreasureEntry7(); // listyA
		public readonly Dictionary<uint, TreasureEntry7> _materialCodeToBaseMaterialMap = new Dictionary<uint, TreasureEntry7>(); // hash F

		public readonly TreasureEntry7 _ceramicMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 _clothMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 _leatherMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 _metalMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 _stoneMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 _woodMaterials = new TreasureEntry7();

		public readonly List<TreasureEntry7> _treasure18 = new List<TreasureEntry7>(); // listyA x 8
		public readonly Dictionary<uint, TreasureEntry6> _treasure19 = new Dictionary<uint, TreasureEntry6>(); // hash G
		public readonly Dictionary<uint, TreasureEntry7> _treasure20 = new Dictionary<uint, TreasureEntry7>(); // hashF
		public readonly TreasureEntry7 _gemProbabilitiesMaybe = new TreasureEntry7(); // listyA
		public readonly Dictionary<uint, List<TreasureEntry5>> _gemMaterials = new Dictionary<uint, List<TreasureEntry5>>(); // hashD
		public readonly Dictionary<uint, float> _materialValueAddedPossibly = new Dictionary<uint, float>(); // hashH
		public readonly List<List<TreasureEntry5>> _treasure24 = new List<List<TreasureEntry5>>(); // listyB x3
		public readonly Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>> _materialColorKeyMap = new Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>>();
		public readonly List<TreasureEntry7> _cantripChances = new List<TreasureEntry7>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Parse(BinaryReader binaryReader)
		{
			base.Parse(binaryReader);

			var totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var items = new List<TreasureEntry>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry();
					item.Parse(binaryReader);
					items.Add(item);
				}
				_wieldedTreasure.Add(key, items);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry2();
				item.Parse(binaryReader);
				_treasure2.Add(key, item);

			}

			for (int i = 0; i < 4; i++)
			{
				var tempTreasure3 = new Dictionary<uint, List<TreasureEntry5>>();

				totalObjects = binaryReader.ReadUInt16();
				binaryReader.ReadUInt16(); // Discard
				for (int j = 0; j < totalObjects; j++)
				{
					var key = binaryReader.ReadUInt32();
					var items = new List<TreasureEntry5>();
					var count = binaryReader.ReadUInt32();
					for (int k = 0; k < count; k++)
					{
						var item = new TreasureEntry5();
						item.Parse(binaryReader);
						items.Add(item);
					}
					tempTreasure3.Add(key, items);
				}

				_treasure3.Add(tempTreasure3);
			}

			for (int i = 0; i < 48; i++)
			{
				var item = new TreasureEntry7();
				item.Parse(binaryReader);
				_treasure7.Add(item);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var items = new List<TreasureEntry5>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry5();
					item.Parse(binaryReader);
					items.Add(item);
				}
				_treasure8.Add(key, items);
			}


			_healerChances.Parse(binaryReader);
			_lockpickChances.Parse(binaryReader);
			_consumableChances.Parse(binaryReader);
			_peaChances.Parse(binaryReader);


			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry6();
				item.Parse(binaryReader);
				_itemBaneSpells.Add(key, item);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry6();
				item.Parse(binaryReader);
				_meleeWeaponSpells.Add(key, item);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry6();
				item.Parse(binaryReader);
				_missileWeaponSpells.Add(key, item);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry6();
				item.Parse(binaryReader);
				_casterWeaponSpells.Add(key, item);
			}


			_spellLevelProbabilitiesMaybe.Parse(binaryReader);

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var items = new List<TreasureEntry5>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry5();
					item.Parse(binaryReader);
					items.Add(item);
				}
				_keyedSpells.Add(items);
			}


			_otherBuffCasterSpell.Parse(binaryReader);
			_otherWarCasterSpell.Parse(binaryReader);


			_scrollChances.Parse(binaryReader);
			_manaStoneChances.Parse(binaryReader);

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry7();
				item.Parse(binaryReader);
				_materialCodeToBaseMaterialMap.Add(key, item);
			}


			_ceramicMaterials.Parse(binaryReader);
			_clothMaterials.Parse(binaryReader);
			_leatherMaterials.Parse(binaryReader);
			_metalMaterials.Parse(binaryReader);
			_stoneMaterials.Parse(binaryReader);
			_woodMaterials.Parse(binaryReader);


			for (int i = 0; i < 2; i++)
			{
				var item = new TreasureEntry7();
				item.Parse(binaryReader);
				_treasure18.Add(item);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry6();
				item.Parse(binaryReader);
				_treasure19.Add(key, item);

			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var item = new TreasureEntry7();
				item.Parse(binaryReader);
				_treasure20.Add(key, item);

			}

			_gemProbabilitiesMaybe.Parse(binaryReader);

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var items = new List<TreasureEntry5>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry5();
					item.Parse(binaryReader);
					items.Add(item);
				}
				_gemMaterials.Add(key, items);
			}

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var value = binaryReader.ReadSingle();
				_materialValueAddedPossibly.Add(key, value);
			}

			for (int i = 0; i < 3; i++)
			{
				var items = new List<TreasureEntry5>();
				var count = binaryReader.ReadUInt32();
				for (int j = 0; j < count; j++)
				{
					var item = new TreasureEntry5();
					item.Parse(binaryReader);
					items.Add(item);
				}
				_treasure24.Add(items);
			}

			// public readonly Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>> _materialColorKeyMap;

			// public readonly TreasureEntry7 _cantripChances[5];

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);


			return true;
		}
	}
}
