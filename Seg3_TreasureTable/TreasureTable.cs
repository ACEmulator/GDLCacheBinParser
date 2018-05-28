using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureTable : Segment
	{
		public readonly Dictionary<uint, List<TreasureEntry>> WieldedTreasure =new Dictionary<uint, List<TreasureEntry>>(); // hashA
		public readonly Dictionary<uint, TreasureEntry2> DeathTreasure = new Dictionary<uint, TreasureEntry2>(); // hashB
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
		public override bool Unpack(BinaryReader reader)
		{
			base.Unpack(reader);

			int totalObjects;


			WieldedTreasure.Unpack(reader);

			DeathTreasure.Unpack(reader);
			_treasure3.Unpack(reader, 4);
			_treasure7.Unpack(reader, 48);
			_treasure8.Unpack(reader);


			HealerChances.Unpack(reader);
			LockpickChances.Unpack(reader);
			ConsumableChances.Unpack(reader);
			PeaChances.Unpack(reader);


			ItemBaneSpells.Unpack(reader);
			MeleeWeaponSpells.Unpack(reader);
			MissileWeaponSpells.Unpack(reader);
			CasterWeaponSpells.Unpack(reader);


			_spellLevelProbabilitiesMaybe.Unpack(reader);
			KeyedSpells.Unpack(reader);


			OtherBuffCasterSpell.Unpack(reader);
			OtherWarCasterSpell.Unpack(reader);


			ScrollChances.Unpack(reader);
			ManaStoneChances.Unpack(reader);
			MaterialCodeToBaseMaterialMap.Unpack(reader);


			CeramicMaterials.Unpack(reader);
			ClothMaterials.Unpack(reader);
			LeatherMaterials.Unpack(reader);
			MetalMaterials.Unpack(reader);
			StoneMaterials.Unpack(reader);
			WoodMaterials.Unpack(reader);


			_treasure18.Unpack(reader, 2);
			_treasure19.Unpack(reader);
			_treasure20.Unpack(reader);
			_gemProbabilitiesMaybe.Unpack(reader);
			GemMaterials.Unpack(reader);

			totalObjects = reader.ReadUInt16();
			reader.ReadUInt16(); // Discard
			for (int i = 0; i < totalObjects; i++)
			{
				var key = reader.ReadUInt32();
				var value = reader.ReadSingle();
				_materialValueAddedPossibly.Add(key, value);
			}

			_treasure24.Unpack(reader, 3);
			MaterialColorKeyMap.Unpack(reader);
			CantripChances.Unpack(reader, 5);

			return true;
		}

		public override bool WriteJSONOutput(string outputFolder)
		{
			base.WriteJSONOutput(outputFolder);

            using (StreamWriter sw = new StreamWriter(outputFolder + "WieldedTreasure.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                Serializer.Serialize(writer, WieldedTreasure);
            }

            using (StreamWriter sw = new StreamWriter(outputFolder + "DeathTreasure.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                Serializer.Serialize(writer, DeathTreasure);
            }

            return true;
		}
	}
}
