using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace PhatACCacheBinParser.Seg3_TreasureTable
{
	class TreasureTable : Segment
	{
		public readonly Dictionary<uint, List<WieldedTreasure>> WieldedTreasure = new Dictionary<uint, List<WieldedTreasure>>();
		public readonly Dictionary<uint, DeathTreasure> DeathTreasure = new Dictionary<uint, DeathTreasure>();

        // 4 list entries with the following keys:
        // 1-11 (values are 2-8)
        // 1-12 (values are 2-8)
        // 1-8  (values are 1, 10-14)
        // 1-19 (values are 1-3)
		public readonly List<Dictionary<uint, List<ValueAndChance>>> _treasure3 = new List<Dictionary<uint, List<ValueAndChance>>>();

        // Each tier has values 15, 16, 17, 19, 20, 18, 21
	    public readonly ValuesAndChancesByTier _treasure7_a = new ValuesAndChancesByTier();

        // List list is 46 entries log, each entry has its own Tier:Value:Chance matrix
        // Each list item contains values that represent a set of weenies
        public readonly List<ValuesAndChancesByTier> _treasure7_b = new List<ValuesAndChancesByTier>();

        // Each tier has values 1-6
	    public readonly ValuesAndChancesByTier _treasure7_c = new ValuesAndChancesByTier();

        // Keys 1-6 (Could this be tiers?)
        // Key 1 values: 10 14 28 30 40 42 44 46
        // Key 2 values: 11 17 18 19 25 31 32 37 29 36
        // Key 3 values: 12 15 24 27 35 43 45 48 50
        // Key 4 values: 13 23 33 34 49
        // Key 5 values: 16 22 26 41 47
        // Key 6 values: 20 21 38 39
        // Notice that values are 10-50, and there are no duplicates
        public readonly Dictionary<uint, List<ValueAndChance>> _treasure8 = new Dictionary<uint, List<ValueAndChance>>();

		public readonly ValuesAndChancesByTier HealerChances = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier LockpickChances = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier ConsumableChances = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier PeaChances = new ValuesAndChancesByTier();

		public readonly Dictionary<uint, ChanceByTier> ItemBaneSpells = new Dictionary<uint, ChanceByTier>();
		public readonly Dictionary<uint, ChanceByTier> MeleeWeaponSpells = new Dictionary<uint, ChanceByTier>();
		public readonly Dictionary<uint, ChanceByTier> MissileWeaponSpells = new Dictionary<uint, ChanceByTier>();
		public readonly Dictionary<uint, ChanceByTier> CasterWeaponSpells = new Dictionary<uint, ChanceByTier>();

		public readonly ValuesAndChancesByTier SpellLevelProbabilities = new ValuesAndChancesByTier();
		public readonly List<List<ValueAndChance>> KeyedSpells = new List<List<ValueAndChance>>();

		public readonly ValuesAndChancesByTier OtherBuffCasterSpell = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier OtherWarCasterSpell = new ValuesAndChancesByTier();

		public readonly ValuesAndChancesByTier ScrollChances = new ValuesAndChancesByTier(); 
		public readonly ValuesAndChancesByTier ManaStoneChances = new ValuesAndChancesByTier();

		public readonly Dictionary<uint, ValuesAndChancesByTier> MaterialCodeToBaseMaterialMap = new Dictionary<uint, ValuesAndChancesByTier>();

		public readonly ValuesAndChancesByTier CeramicMaterials = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier ClothMaterials = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier LeatherMaterials = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier MetalMaterials = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier StoneMaterials = new ValuesAndChancesByTier();
		public readonly ValuesAndChancesByTier WoodMaterials = new ValuesAndChancesByTier();

		public readonly ValuesAndChancesByTier Workmanship = new ValuesAndChancesByTier();


        // Each tier has values 1-5
        // Tier 1 chances: 1.0  0.0  0.0  0.0  0.0
        // Tier 2 chances: 1.0  0.0  0.0  0.0  0.0
        // Tier 3 chances: 0.5  0.5  0.0  0.0  0.0
        // Tier 4 chances: 0.25 0.5  0.25 0.0  0.0
        // Tier 5 chances: 0.0  0.25 0.5  0.25 0.0
        // Tier 6 chances: 0.0  0.25 0.35 0.3  0.1
	    // This is likely a percent of chance for something to happen in quantity, ie: something that can happen in quantities of 1-5
        public readonly ValuesAndChancesByTier _treasure18_2 = new ValuesAndChancesByTier();

	    // Keys 11,1,2,3,4,5,6,7,8,9,10
	    // Keys 11-6 , Chances by tier: 0.5, 0.6 , 0.7 , 0.8 , 0.9 , 1.0
	    // Keys  7-10, Chances by tier: 0  , 0.01, 0.05, 0.08, 0.25, 0.4
        public readonly Dictionary<uint, ChanceByTier> _treasure19 = new Dictionary<uint, ChanceByTier>();

        // Keys 1-9, each key has values 0-8 for each tier
		public readonly Dictionary<uint, ValuesAndChancesByTier> _treasure20 = new Dictionary<uint, ValuesAndChancesByTier>();

		public readonly ValuesAndChancesByTier _gemProbabilitiesMaybe = new ValuesAndChancesByTier();
		public readonly Dictionary<uint, List<ValueAndChance>> GemMaterials = new Dictionary<uint, List<ValueAndChance>>();
		public readonly Dictionary<uint, float> _materialValueAddedPossibly = new Dictionary<uint, float>();

        // 3 lists
        // List 1, values 1-18
        // List 2, values 22, 39, 2, 19, 21, 8, 13, 14, 20
	    // List 3, values 1-18
        public readonly List<List<ValueAndChance>> _treasure24 = new List<List<ValueAndChance>>();

		public readonly Dictionary<uint, Dictionary<uint, List<ValueAndChance>>> MaterialColorKeyMap = new Dictionary<uint, Dictionary<uint, List<ValueAndChance>>>();

		public readonly List<ValuesAndChancesByTier> CantripChances = new List<ValuesAndChancesByTier>();

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

		    _treasure7_a.Unpack(reader);

            _treasure7_b.Unpack(reader, 46);

		    _treasure7_c.Unpack(reader);


            _treasure8.Unpack(reader);

			HealerChances.Unpack(reader);
			LockpickChances.Unpack(reader);
			ConsumableChances.Unpack(reader);
			PeaChances.Unpack(reader);

			ItemBaneSpells.Unpack(reader);
			MeleeWeaponSpells.Unpack(reader);
			MissileWeaponSpells.Unpack(reader);
			CasterWeaponSpells.Unpack(reader);

			SpellLevelProbabilities.Unpack(reader);
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

		    Workmanship.Unpack(reader);

		    _treasure18_2.Unpack(reader);
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


            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure3.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure3);


            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure7_a.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure7_a);

		    using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure7_b.json"))
		    using (JsonWriter writer = new JsonTextWriter(sw))
		        Serializer.Serialize(writer, _treasure7_b);

		    using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure7_c.json"))
		    using (JsonWriter writer = new JsonTextWriter(sw))
		        Serializer.Serialize(writer, _treasure7_c);


            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure8.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure8);


            using (StreamWriter sw = new StreamWriter(outputFolder + "HealerChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, HealerChances);

            using (StreamWriter sw = new StreamWriter(outputFolder + "LockpickChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, LockpickChances);

            using (StreamWriter sw = new StreamWriter(outputFolder + "ConsumableChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, ConsumableChances);

            using (StreamWriter sw = new StreamWriter(outputFolder + "PeaChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, PeaChances);


            using (StreamWriter sw = new StreamWriter(outputFolder + "ItemBaneSpells.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, ItemBaneSpells);

            using (StreamWriter sw = new StreamWriter(outputFolder + "MeleeWeaponSpells.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, MeleeWeaponSpells);

            using (StreamWriter sw = new StreamWriter(outputFolder + "MissileWeaponSpells.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, MissileWeaponSpells);

            using (StreamWriter sw = new StreamWriter(outputFolder + "CasterWeaponSpells.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, CasterWeaponSpells);


            using (StreamWriter sw = new StreamWriter(outputFolder + "SpellLevelProbabilities.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, SpellLevelProbabilities);

            using (StreamWriter sw = new StreamWriter(outputFolder + "KeyedSpells.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, KeyedSpells);


            using (StreamWriter sw = new StreamWriter(outputFolder + "OtherBuffCasterSpell.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, OtherBuffCasterSpell);

            using (StreamWriter sw = new StreamWriter(outputFolder + "OtherWarCasterSpell.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, OtherWarCasterSpell);


            using (StreamWriter sw = new StreamWriter(outputFolder + "ScrollChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, ScrollChances);

            using (StreamWriter sw = new StreamWriter(outputFolder + "ManaStoneChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, ManaStoneChances);


            using (StreamWriter sw = new StreamWriter(outputFolder + "MaterialCodeToBaseMaterialMap.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, MaterialCodeToBaseMaterialMap);


            using (StreamWriter sw = new StreamWriter(outputFolder + "CeramicMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, CeramicMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "ClothMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, ClothMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "LeatherMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, LeatherMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "MetalMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, MetalMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "StoneMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, StoneMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "WoodMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, WoodMaterials);


            using (StreamWriter sw = new StreamWriter(outputFolder + "Workmanship.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, Workmanship);


		    using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure18_2.json"))
		    using (JsonWriter writer = new JsonTextWriter(sw))
		        Serializer.Serialize(writer, _treasure18_2);

            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure19.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure19);

            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure20.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure20);

            using (StreamWriter sw = new StreamWriter(outputFolder + "_gemProbabilitiesMaybe.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _gemProbabilitiesMaybe);

            using (StreamWriter sw = new StreamWriter(outputFolder + "GemMaterials.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, GemMaterials);

            using (StreamWriter sw = new StreamWriter(outputFolder + "_materialValueAddedPossibly.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _materialValueAddedPossibly);

            using (StreamWriter sw = new StreamWriter(outputFolder + "_treasure24.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, _treasure24);

            using (StreamWriter sw = new StreamWriter(outputFolder + "MaterialColorKeyMap.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, MaterialColorKeyMap);


            using (StreamWriter sw = new StreamWriter(outputFolder + "CantripChances.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
                Serializer.Serialize(writer, CantripChances);


            return true;
		}
	}
}
