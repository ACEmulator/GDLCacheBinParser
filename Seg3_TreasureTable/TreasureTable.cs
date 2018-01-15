using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureTable : Segment
	{
		public readonly Dictionary<uint, List<TreasureEntry>> WieldedTreasure =new Dictionary<uint, List<TreasureEntry>>(); // hashA
		public readonly Dictionary<uint, TreasureEntry2> _treasure2 = new Dictionary<uint, TreasureEntry2>(); // hashB
		public readonly List<Dictionary<uint, List<TreasureEntry5>>> _treasure3 = new List<Dictionary<uint, List<TreasureEntry5>>>();
		public readonly List<TreasureEntry7> _treasure7 = new List<TreasureEntry7>();
		public readonly Dictionary<uint, List<TreasureEntry5>> _treasure8 = new Dictionary<uint, List<TreasureEntry5>>(); // hash D

		public readonly TreasureEntry7 HealerChances = new TreasureEntry7();
		public readonly TreasureEntry7 LockpickChances = new TreasureEntry7();
		public readonly TreasureEntry7 ConsumableChances = new TreasureEntry7();
		public readonly TreasureEntry7 PeaChances = new TreasureEntry7();

		public readonly Dictionary<uint, TreasureEntry6> ItemBaneSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> MeleeWeaponSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> MissileWeaponSpells = new Dictionary<uint, TreasureEntry6>();
		public readonly Dictionary<uint, TreasureEntry6> CasterWeaponSpells = new Dictionary<uint, TreasureEntry6>();

		public readonly TreasureEntry7 _spellLevelProbabilitiesMaybe = new TreasureEntry7(); // listyA
		public readonly List<List<TreasureEntry5>> KeyedSpells = new List<List<TreasureEntry5>>(); // listyBcount + listyB

		public readonly TreasureEntry7 OtherBuffCasterSpell = new TreasureEntry7();
		public readonly TreasureEntry7 OtherWarCasterSpell = new TreasureEntry7();

		public readonly TreasureEntry7 ScrollChances = new TreasureEntry7(); // listyA
		public readonly TreasureEntry7 ManaStoneChances = new TreasureEntry7(); // listyA
		public readonly Dictionary<uint, TreasureEntry7> MaterialCodeToBaseMaterialMap = new Dictionary<uint, TreasureEntry7>(); // hash F

		public readonly TreasureEntry7 CeramicMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 ClothMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 LeatherMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 MetalMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 StoneMaterials = new TreasureEntry7();
		public readonly TreasureEntry7 WoodMaterials = new TreasureEntry7();

		public readonly List<TreasureEntry7> _treasure18 = new List<TreasureEntry7>(); // listyA x 8
		public readonly Dictionary<uint, TreasureEntry6> _treasure19 = new Dictionary<uint, TreasureEntry6>(); // hash G
		public readonly Dictionary<uint, TreasureEntry7> _treasure20 = new Dictionary<uint, TreasureEntry7>(); // hashF
		public readonly TreasureEntry7 _gemProbabilitiesMaybe = new TreasureEntry7(); // listyA
		public readonly Dictionary<uint, List<TreasureEntry5>> GemMaterials = new Dictionary<uint, List<TreasureEntry5>>(); // hashD
		public readonly Dictionary<uint, float> _materialValueAddedPossibly = new Dictionary<uint, float>(); // hashH
		public readonly List<List<TreasureEntry5>> _treasure24 = new List<List<TreasureEntry5>>(); // listyB x3
		public readonly Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>> MaterialColorKeyMap = new Dictionary<uint, Dictionary<uint, List<TreasureEntry5>>>();
		public readonly List<TreasureEntry7> CantripChances = new List<TreasureEntry7>();

		/// <summary>
		/// You can only call Parse() once on an instantiated object.
		/// </summary>
		public override bool Unpack(BinaryReader binaryReader)
		{
			base.Unpack(binaryReader);

			int totalObjects;


			WieldedTreasure.Unpack(binaryReader);

			_treasure2.Unpack(binaryReader);
			_treasure3.Unpack(binaryReader, 4);
			_treasure7.Unpack(binaryReader, 48);
			_treasure8.Unpack(binaryReader);


			HealerChances.Unpack(binaryReader);
			LockpickChances.Unpack(binaryReader);
			ConsumableChances.Unpack(binaryReader);
			PeaChances.Unpack(binaryReader);


			ItemBaneSpells.Unpack(binaryReader);
			MeleeWeaponSpells.Unpack(binaryReader);
			MissileWeaponSpells.Unpack(binaryReader);
			CasterWeaponSpells.Unpack(binaryReader);


			_spellLevelProbabilitiesMaybe.Unpack(binaryReader);
			KeyedSpells.Unpack(binaryReader);


			OtherBuffCasterSpell.Unpack(binaryReader);
			OtherWarCasterSpell.Unpack(binaryReader);


			ScrollChances.Unpack(binaryReader);
			ManaStoneChances.Unpack(binaryReader);
			MaterialCodeToBaseMaterialMap.Unpack(binaryReader);


			CeramicMaterials.Unpack(binaryReader);
			ClothMaterials.Unpack(binaryReader);
			LeatherMaterials.Unpack(binaryReader);
			MetalMaterials.Unpack(binaryReader);
			StoneMaterials.Unpack(binaryReader);
			WoodMaterials.Unpack(binaryReader);


			_treasure18.Unpack(binaryReader, 2);
			_treasure19.Unpack(binaryReader);
			_treasure20.Unpack(binaryReader);
			_gemProbabilitiesMaybe.Unpack(binaryReader);
			GemMaterials.Unpack(binaryReader);

			totalObjects = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = binaryReader.ReadUInt32();
				var value = binaryReader.ReadSingle();
				_materialValueAddedPossibly.Add(key, value);
			}

			_treasure24.Unpack(binaryReader, 3);
			MaterialColorKeyMap.Unpack(binaryReader);
			CantripChances.Unpack(binaryReader, 5);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);


			return false;
		}
	}
}
