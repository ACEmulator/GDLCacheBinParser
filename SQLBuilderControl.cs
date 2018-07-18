using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using PhatACCacheBinParser.Common;
using PhatACCacheBinParser.Enums;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg1_RegionDescExtendedData;
using PhatACCacheBinParser.Seg2_SpellTableExtendedData;
using PhatACCacheBinParser.Seg3_TreasureTable;
using PhatACCacheBinParser.Seg4_CraftTable;
using PhatACCacheBinParser.Seg5_HousingPortals;
using PhatACCacheBinParser.Seg6_LandBlockExtendedData;
using PhatACCacheBinParser.Seg8_QuestDefDB;
using PhatACCacheBinParser.Seg9_WeenieDefaults;
using PhatACCacheBinParser.SegA_MutationFilters;
using PhatACCacheBinParser.SegB_GameEventDefDB;

namespace PhatACCacheBinParser
{
	public partial class SQLBuilderControl : UserControl
	{
		public SQLBuilderControl()
		{
			InitializeComponent();
		}


		private readonly RegionDescExtendedData regionDescExtendedData = new RegionDescExtendedData();
		private readonly SpellTableExtendedData spellTableExtendedData = new SpellTableExtendedData();
		private readonly TreasureTable treasureTable = new TreasureTable();
		private readonly CraftingTable craftingTable = new CraftingTable();
		private readonly HousingPortalsTable housingPortalsTable = new HousingPortalsTable();
		private readonly LandBlockData landBlockData = new LandBlockData();
		// Segment 7
		private readonly QuestDefDB questDefDB = new QuestDefDB();
		private readonly WeenieDefaults weenieDefaults = new WeenieDefaults();
		private readonly MutationFilters mutationFilters = new MutationFilters();
        private readonly GameEventDefDB gameEventDefDB = new GameEventDefDB();

        private readonly Dictionary<uint, string> weenieNames = new Dictionary<uint, string>();

        private void cmdParseAll_Click(object sender, EventArgs e)
		{
			cmdParseAll.Enabled = false;

			progressParseSources.Style = ProgressBarStyle.Marquee;

            ThreadPool.QueueUserWorkItem(o =>
			{
				// Read all the inputs here
				TryParseSegment((string) Settings.Default["_1SourceBin"], regionDescExtendedData);
                TryParseSegment((string) Settings.Default["_2SourceBin"], spellTableExtendedData);
				TryParseSegment((string) Settings.Default["_3SourceBin"], treasureTable);
				TryParseSegment((string) Settings.Default["_4SourceBin"], craftingTable);
				TryParseSegment((string) Settings.Default["_5SourceBin"], housingPortalsTable);
				TryParseSegment((string) Settings.Default["_6SourceBin"], landBlockData);
				// Segment 7
				TryParseSegment((string) Settings.Default["_8SourceBin"], questDefDB);
				TryParseSegment((string) Settings.Default["_9SourceBin"], weenieDefaults);
				TryParseSegment((string) Settings.Default["_ASourceBin"], mutationFilters);
                TryParseSegment((string) Settings.Default["_BSourceBin"], gameEventDefDB);

                BeginInvoke((Action)CollectWeenieNames);

                BeginInvoke((Action)(() =>
				{                    
                    progressParseSources.Style = ProgressBarStyle.Continuous;
					progressParseSources.Value = 100;

					// Now that we've parsed all of our input segments, we can enable outputs
					foreach (Control control in Controls)
					{
						if (control is Button && control != sender)
							control.Enabled = true;
					}
				}));
			});
		}

		private static bool TryParseSegment<T>(string sourceBin, T segment) where T : Segment
		{
			try
			{
				if (!File.Exists(sourceBin))
					return false;

				var data = File.ReadAllBytes(sourceBin);

				// Parse the data
				using (var memoryStream = new MemoryStream(data))
				using (var binaryReader = new BinaryReader(memoryStream))
				{
					if (segment.Unpack(binaryReader))
						return true;
				}
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch { }

			return false;
		}


		private void cmd9WeeniesParse_Click(object sender, EventArgs e)
		{
			cmd9WeeniesParse.Enabled = false;

			progressBarWeenies.Style = ProgressBarStyle.Continuous;
            progressBarWeenies.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
			{
                // Do some output thing here

                // For example
                //foreach (var weenie in WeenieDefaults.Weenies)
                //	;

                WriteWeenieFiles();      
                                
                BeginInvoke((Action)(() =>
				{
					progressBarWeenies.Style = ProgressBarStyle.Continuous;
					progressBarWeenies.Value = 100;

					cmd9WeeniesParse.Enabled = true;
				}));
			});
		}

        private void CollectWeenieNames()
        {
            weenieNames.Clear();
            foreach (var weenie in weenieDefaults.Weenies)
            {
                var name = weenie.Value.Description;
                if (name == "")
                {
                    if (Enum.IsDefined(typeof(WeenieClasses), (ushort)weenie.Value.WCID))
                        name = Enum.GetName(typeof(WeenieClasses), weenie.Value.WCID).Substring(2);
                    else
                    {
                        name = "ace" + weenie.Value.WCID; //+ "-" + parsed.Label.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();
                    }
                    //name = name.Substring(2); // W_
                    if (name.Contains("_CLASS"))
                        name = name.Remove(name.LastIndexOf("_CLASS")).Replace("_", "-").ToLower();
                }
                weenieNames.Add(weenie.Value.WCID, name);
            }
        }

        private void WriteWeenieFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "9 WeenieDefaults" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            // Highest Weenie Exported in WorldSpawns was: 30937
            // Highest Weenie found in WeenieClasses was: 31034

            uint highestWeenieAllowed = 31034;

            string sqlCommand = "INSERT";

            //Parallel.For(0, WeenieDefaults.Weenies.Count, i =>
            foreach (var weenie in weenieDefaults.Weenies)
            {
                //var parsed = WeenieDefaults.Weenies[(uint)i]; //parsedObjects[i] as Weenies.Weenie;
                var parsed = weenie.Value;

                if (parsed.WCID > highestWeenieAllowed && highestWeenieAllowed > 0)
                    break;

                var wtFolder = outputFolder + Enum.GetName(typeof(WeenieType), parsed.WeenieType) + "\\";
                if (!Directory.Exists(wtFolder))
                    Directory.CreateDirectory(wtFolder);

                var ctFolder = wtFolder;
                if (parsed.WeenieType == (uint)WeenieType.Creature)
                {
                    if (parsed.IntValues.ContainsKey((int)PropertyInt.CreatureType))
                    {
                        Enum.TryParse(parsed.IntValues[(int)PropertyInt.CreatureType].ToString(), out CreatureType ct);
                        if (Enum.IsDefined(typeof(CreatureType), ct))
                            ctFolder = wtFolder + Enum.GetName(typeof(CreatureType), parsed.IntValues[(int)PropertyInt.CreatureType]) + "\\";
                        else
                            ctFolder = wtFolder + "UnknownCT_" + parsed.IntValues[(int)PropertyInt.CreatureType] + "\\";
                    }
                    else
                        ctFolder = wtFolder + "Unsorted" + "\\";
                    if (!Directory.Exists(ctFolder))
                        Directory.CreateDirectory(ctFolder);
                }

                var itFolder = ctFolder;
                if (parsed.WeenieType != (uint)WeenieType.Creature)
                {
                    if (parsed.IntValues.ContainsKey((int)PropertyInt.ItemType))
                        itFolder = ctFolder + Enum.GetName(typeof(ItemType), (uint)parsed.IntValues[(int)PropertyInt.ItemType]) + "\\";
                    else
                        itFolder = ctFolder + Enum.GetName(typeof(ItemType), ItemType.None) + "\\";
                    if (!Directory.Exists(itFolder))
                        Directory.CreateDirectory(itFolder);
                }

                var outFolder = itFolder;

                //var aceObjectDescriptionFlags = 0;
                //// ACE currently relies on aceObjectDescriptionFlags in ace_object to set the base.. In the future that column should be ignored entirely
                //// the WeenieType classes should set their default base aceObjectDescriptionFlags similar to below
                //var aceObjectDescriptionFlags = new PublicWeenieDesc.BitfieldIndex();
                //// Research on ACE-World db tells me that the WorldObject base flags are Attackable and Stuck
                //aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_ATTACKABLE | PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //var wt = (WeenieType)parsed.WeenieType;
                //switch (wt)
                //{
                //    case WeenieType.Admin_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_ADMIN;
                //        break;
                //    case WeenieType.AllegianceBindstone_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_BINDSTONE;
                //        break;
                //    case WeenieType.Allegiance_WeenieType:
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_ATTACKABLE;
                //        break;
                //    case WeenieType.Book_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_BOOK;
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    case WeenieType.Chest_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_OPENABLE;
                //        break;
                //    case WeenieType.Creature_WeenieType:
                //        if (parsed.WCID == 1)
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_PLAYER;                                    
                //        break;
                //    case WeenieType.Cow_WeenieType:
                //        break;
                //    case WeenieType.Container_WeenieType:
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_OPENABLE;
                //        break;
                //    case WeenieType.Corpse_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_CORPSE | PublicWeenieDesc.BitfieldIndex.BF_OPENABLE;
                //        break;
                //    case WeenieType.Door_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_DOOR;
                //        break;
                //    case WeenieType.Food_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_FOOD;
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    case WeenieType.Healer_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_HEALER;
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    case WeenieType.LifeStone_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_LIFESTONE;
                //        break;
                //    case WeenieType.Lockpick_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_LOCKPICK;
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    case WeenieType.PKModifier_WeenieType:
                //        if (parsed.IntValues[(int)PropertyInt.PK_LEVEL_MODIFIER_INT] == 1)
                //            aceObjectDescriptionFlags = PublicWeenieDesc.BitfieldIndex.BF_PKSWITCH;
                //        else if (parsed.IntValues[(int)PropertyInt.PK_LEVEL_MODIFIER_INT] == -1)
                //            aceObjectDescriptionFlags = PublicWeenieDesc.BitfieldIndex.BF_NPKSWITCH;
                //        break;
                //    case WeenieType.Portal_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_PORTAL;
                //        break;                                
                //    case WeenieType.Sentinel_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_ADMIN;
                //        break;
                //    case WeenieType.Stackable_WeenieType:
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    case WeenieType.Storage_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_OPENABLE;
                //        break;
                //    case WeenieType.Vendor_WeenieType:
                //        aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_VENDOR;
                //        break;
                //    case WeenieType.AdvocateItem_WeenieType:
                //    case WeenieType.Ammunition_WeenieType:
                //    case WeenieType.AttributeTransferDevice_WeenieType:
                //    case WeenieType.AugmentationDevice_WeenieType:
                //    case WeenieType.Caster_WeenieType:
                //    case WeenieType.Clothing_WeenieType:
                //    case WeenieType.Coin_WeenieType:
                //    case WeenieType.CraftTool_WeenieType:
                //    case WeenieType.Deed_WeenieType:
                //    case WeenieType.Gem_WeenieType:
                //    case WeenieType.Generic_WeenieType:
                //    case WeenieType.Hooker_WeenieType:
                //    case WeenieType.Key_WeenieType:
                //    case WeenieType.LightSource_WeenieType:
                //    case WeenieType.ManaStone_WeenieType:
                //    case WeenieType.MeleeWeapon_WeenieType:
                //    case WeenieType.MissileLauncher_WeenieType:
                //    case WeenieType.Missile_WeenieType:
                //    case WeenieType.PetDevice_WeenieType:
                //    case WeenieType.Scroll_WeenieType:
                //    case WeenieType.SkillAlterationDevice_WeenieType:
                //    case WeenieType.SpellComponent_WeenieType:
                //        aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        break;
                //    default:
                //        break;
                //}
                //if (parsed.BoolValues != null)
                //{
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.ATTACKABLE_BOOL))
                //    {
                //        if (parsed.BoolValues[(int)STypeBool.ATTACKABLE_BOOL] != true)
                //            aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_ATTACKABLE;
                //        else
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_ATTACKABLE;
                //    }
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.STUCK_BOOL))
                //    {
                //        if (parsed.BoolValues[(int)STypeBool.STUCK_BOOL] != true)
                //            aceObjectDescriptionFlags &= ~PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //        else
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_STUCK;
                //    }
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.INSCRIBABLE_BOOL))
                //        if (parsed.BoolValues[(int)STypeBool.INSCRIBABLE_BOOL] == true)
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_INSCRIBABLE;
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.REQUIRES_BACKPACK_SLOT_BOOL))
                //        if (parsed.BoolValues[(int)STypeBool.REQUIRES_BACKPACK_SLOT_BOOL] == true)
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_REQUIRES_PACKSLOT;
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.RETAINED_BOOL))
                //        if (parsed.BoolValues[(int)STypeBool.RETAINED_BOOL] == true)
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_RETAINED;
                //    if (parsed.BoolValues.ContainsKey((int)STypeBool.UI_HIDDEN_BOOL))
                //        if (parsed.BoolValues[(int)STypeBool.UI_HIDDEN_BOOL] == true)
                //            aceObjectDescriptionFlags |= PublicWeenieDesc.BitfieldIndex.BF_UI_HIDDEN;
                //}

                string FileNameFormatter(Seg9_WeenieDefaults.Weenie obj) => obj.WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Description, "_");

                string fileNameFormatter = FileNameFormatter(weenie.Value);

                using (StreamWriter writer = new StreamWriter(outFolder + fileNameFormatter + ".sql"))
                {
                    string weenieName;
                    if (Enum.IsDefined(typeof(WeenieClasses), (ushort)parsed.WCID))
                    {
                        weenieName = Enum.GetName(typeof(WeenieClasses), parsed.WCID).Substring(2);
                        weenieName = weenieName.Substring(0, weenieName.Length - 6).Replace("_", "-").ToLower();
                    }
                    else
                        weenieName = "ace" + parsed.WCID; //+ "-" + parsed.Label.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();

                    string name = parsed.Description;
                    if (name == "")
                    {
                        if (Enum.IsDefined(typeof(WeenieClasses), (ushort)parsed.WCID))
                            name = Enum.GetName(typeof(WeenieClasses), parsed.WCID).Substring(2);
                        else
                            name = "ace" + parsed.WCID;

                        if (name.Contains("_CLASS"))
                            name = name.Remove(name.LastIndexOf("_CLASS")).Replace("_", "-").ToLower();
                    }

                    string line = $"/* Weenie - {name} ({parsed.WCID}) */" + Environment.NewLine;

                    line += $"DELETE FROM weenie WHERE class_Id = {parsed.WCID};" + Environment.NewLine + Environment.NewLine;
                    line += $"{sqlCommand} INTO weenie (`class_Id`, `class_Name`, `type`)" + Environment.NewLine +
                           $"VALUES ({parsed.WCID}, '{weenieName}', {parsed.WeenieType} /* {Enum.GetName(typeof(WeenieType), parsed.WeenieType)} */);" + Environment.NewLine;
                    writer.WriteLine(line);
                    string intsLine = "", bigintsLine = "", floatsLine = "", boolsLine = "", strsLine = "", didsLine = "", iidsLine = "";
                    string skillsLine = "", attributesLine = "", attribute2ndsLine = ""; //, bodyDamageValuesLine = "", bodyDamageVariancesLine = "", bodyArmorValuesLine = "", numsLine = "";
                    string spellsLine = "", positionsLine = "", pagesLine = "", instancesLine = "", profilesLine = "", booksLine = "";
                    string bodyPartsLine = "", eventsLine = "", emoteCategorysLine = "", emoteActionsLine = "";
                    //line = $"{sqlCommand} INTO `ace_object` (`" +
                    //    "aceObjectId`, `aceObjectDescriptionFlags`, " +
                    //    "`weenieClassId`" +
                    //    ")" + Environment.NewLine + "VALUES (" +
                    //$"{parsed.WCID}, {(uint)aceObjectDescriptionFlags}, " +
                    //$"{parsed.WCID}"; //+
                    //line += ");" + Environment.NewLine;
                    //writer.WriteLine(line);
                    if (parsed.IntValues != null)
                    {
                        foreach (var stat in parsed.IntValues.OrderBy(x => x.Key))
                        {
                            // intsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */" + Environment.NewLine;
                            switch ((PropertyInt)stat.Key)
                            {
                                case PropertyInt.AmmoType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(AmmoType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.BoosterEnum:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(PropertyAttribute2nd), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ClothingPriority:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((CoverageMask)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.CombatMode:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CombatMode), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.CombatUse:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(COMBAT_USE), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.CreatureType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CreatureType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.CurrentWieldedLocation:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((EquipMask)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.DamageType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((DamageType)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.DefaultCombatStyle:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CombatStyle), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.Gender:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(Gender), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.GeneratorDestructionType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(GeneratorDestruct), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.GeneratorEndDestructionType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(GeneratorDestruct), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.GeneratorTimeType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(GeneratorTimeType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.GeneratorType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(GeneratorType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.HeritageGroup:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(HeritageGroup), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                //case PropertyInt.HOOK_GROUP_INT:
                                //    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((HookTypeEnum)stat.Value).ToString()} */)" + Environment.NewLine;
                                //    break;
                                case PropertyInt.HookItemType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ItemType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.HookPlacement:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(Placement), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.HookType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((HookTypeEnum)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.HouseType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(HouseType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ItemType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ItemType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ItemUseable:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((ITEM_USEABLE)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ItemXpStyle:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ItemXpStyle), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ValidLocations:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((EquipMask)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.MaterialType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(Material), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.MerchandiseItemTypes:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((ItemType)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.PaletteTemplate:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(PALETTE_TEMPLATE), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.PhysicsState:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((PhysicsState)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.Placement:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(Placement), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.PlayerKillerStatus:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(PlayerKillerStatus), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.PortalBitmask:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(PortalBitmask), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.RadarBlipColor:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(RadarColor), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ShowableOnRadar:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(RadarBehavior), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.SlayerCreatureType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CreatureType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.SummoningMastery:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(SummoningMastery), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.TargetType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((ItemType)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.UiEffects:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(UiEffects), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.WeaponSkill:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ACE.Entity.Enum.Skill), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.WeaponType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(WeaponType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ActivationCreateClass:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {weenieNames[(uint)stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ActivationResponse:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ActivationResponseEnum), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.Attuned:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(AttunedStatusEnum), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.AttackHeight:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(AttackHeight), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.AttackType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((AttackType)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.Bonded:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(BondedStatusEnum), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ChannelsActive:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((Channel)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ChannelsAllowed:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((Channel)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.AccountRequirements:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(SubscriptionStatus__guessedname), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.AetheriaBitfield:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((AetheriaBitfield)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.AiAllowedCombatStyle:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((CombatStyle)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.EquipmentSetId:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(EquipmentSet), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.FoeType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CreatureType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.FriendType:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(CreatureType), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.WieldRequirements2:
                                case PropertyInt.WieldRequirements3:
                                case PropertyInt.WieldRequirements4:
                                case PropertyInt.WieldRequirements:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((WieldRequirement)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.WieldSkilltype2:
                                case PropertyInt.WieldSkilltype3:
                                case PropertyInt.WieldSkilltype4:
                                case PropertyInt.WieldSkilltype:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {Enum.GetName(typeof(ACE.Entity.Enum.Skill), stat.Value)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.GeneratorStartTime:
                                case PropertyInt.GeneratorEndTime:
                                    //intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(stat.Value)).ToString()} */)" + Environment.NewLine;
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {DateTimeOffset.FromUnixTimeSeconds(stat.Value).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */)" + Environment.NewLine;
                                    break;
                                case PropertyInt.ImbuedEffect2:
                                case PropertyInt.ImbuedEffect3:
                                case PropertyInt.ImbuedEffect4:
                                case PropertyInt.ImbuedEffect5:
                                case PropertyInt.ImbuedEffect:
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value} /* {((ImbuedEffectType)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                default:                                    
                                    intsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                                    break;
                            }
                        }
                    }
                    if (parsed.LongValues != null)
                    {
                        foreach (var stat in parsed.LongValues.OrderBy(x => x.Key))
                        {
                            bigintsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInt64), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                        }
                    }
                    if (parsed.DoubleValues != null)
                    {
                        foreach (var stat in parsed.DoubleValues.OrderBy(x => x.Key))
                        {
                            floatsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyFloat), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                        }
                    }
                    if (parsed.BoolValues != null)
                    {
                        foreach (var stat in parsed.BoolValues.OrderBy(x => x.Key))
                        {
                            boolsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyBool), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                        }
                    }
                    if (parsed.StringValues != null)
                    {
                        foreach (var stat in parsed.StringValues.OrderBy(x => x.Key))
                        {
                            if (stat.Key != (uint)PropertyString.Name)
                                strsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyString), stat.Key)} */, '{stat.Value.Replace("'", "''")}')" + Environment.NewLine;
                            else
                            {
                                if (stat.Value != "")
                                    strsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyString), stat.Key)} */, '{stat.Value.Replace("'", "''")}')" + Environment.NewLine;
                                else
                                    strsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyString), stat.Key)} */, '{name.Replace("'", "''")}')" + Environment.NewLine;
                            }
                        }
                    }
                    if (parsed.DIDValues != null)
                    {
                        foreach (var stat in parsed.DIDValues.OrderBy(x => x.Key))
                        {
                            // didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(STypeDID), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                            switch ((PropertyDataId)stat.Key)
                            {
                                case PropertyDataId.ActivationAnimation:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.ActivationSound:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((Sound)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.AlternateCurrency:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.AugmentationCreateItem:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.BlueSurgeSpell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.DeathSpell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.InitMotion:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.LastPortal:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.LinkedPortalOne:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.LinkedPortalTwo:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.OriginalPortal:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.PhysicsScript:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((PlayScript)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.ProcSpell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.RedSurgeSpell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.RestrictionEffect:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((PlayScript)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                //case STypeDID.SPELL_COMPONENT_DID:
                                //    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(STypeDID), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                //    break;
                                case PropertyDataId.Spell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseCreateItem:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseSound:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((Sound)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseTargetAnimation:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseTargetFailureAnimation:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseTargetSuccessAnimation:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.UseUserAnimation:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((MotionCommand)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.VendorsClassId:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {weenieNames[stat.Value]} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.YellowSurgeSpell:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* {((SpellID)stat.Value).ToString()} */)" + Environment.NewLine;
                                    break;
                                case PropertyDataId.WieldedTreasureType:                                    
                                    //didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(STypeDID), stat.Key)} */, {stat.Value} /* Loot Tier: {TreasureTable.DeathTreasure[(uint)stat.Key].Tier} */)" + Environment.NewLine;
                                    if (treasureTable.WieldedTreasure.ContainsKey(stat.Value))
                                    {
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value})" + Environment.NewLine;

                                        foreach (var item in treasureTable.WieldedTreasure[stat.Value])
                                        {
                                            didsLine += $"     /* Wield {(item.Amount > 1 ? $"{item.Amount}x" : "")} {weenieNames[item.WCID]} ({item.WCID}) {(item.PTID > 0 ? $"| Palette: {Enum.GetName(typeof(PALETTE_TEMPLATE), item.PTID)} ({item.PTID})" : "")} {(item.Shade > 0 ? $"| Shade: {item.Shade}" : "")} | Chance: {item.Chance * 100}% */" + Environment.NewLine;
                                        }
                                    }
                                    else if (treasureTable.DeathTreasure.ContainsKey(stat.Value))
                                    {
                                        //foreach (var item in TreasureTable.DeathTreasure[stat.Value])
                                        //{
                                        //    didsLine += $"     /* Wield {(item.Amount > 1 ? $"{item.Amount}x" : "")} {weenieNames[item.WCID]} ({item.WCID}) {(item.PTID > 0 ? $"Palette: {Enum.GetName(typeof(PALETTE_TEMPLATE), item.PTID)} ({item.PTID})" : "")} {(item.Shade > 0 ? $"Shade: {item.Shade})" : "")} Chance: {item.Chance / 1}% */" + Environment.NewLine;
                                        //}
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* Loot Tier: {treasureTable.DeathTreasure[stat.Value].Tier} */)" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                                    }
                                    break;
                                case PropertyDataId.DeathTreasureType:
                                    //didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(STypeDID), stat.Key)} */, {stat.Value} /* Loot Tier: {TreasureTable.DeathTreasure[stat.Value].Tier} */)" + Environment.NewLine;
                                    if (treasureTable.WieldedTreasure.ContainsKey(stat.Value))
                                    {
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value})" + Environment.NewLine;

                                        foreach (var item in treasureTable.WieldedTreasure[stat.Value])
                                        {
                                            didsLine += $"     /* Contain {(item.Amount > 1 ? $"{item.Amount}x" : "")} {weenieNames[item.WCID]} ({item.WCID}) {(item.PTID > 0 ? $"| Palette: {Enum.GetName(typeof(PALETTE_TEMPLATE), item.PTID)} ({item.PTID})" : "")} {(item.Shade > 0 ? $"| Shade: {item.Shade}" : "")} | Chance: {item.Chance * 100}% */" + Environment.NewLine;
                                        }
                                    }
                                    else if (treasureTable.DeathTreasure.ContainsKey(stat.Value))
                                    {
                                        //foreach (var item in TreasureTable.DeathTreasure[stat.Value])
                                        //{
                                        //    didsLine += $"     /* Wield {(item.Amount > 1 ? $"{item.Amount}x" : "")} {weenieNames[item.WCID]} ({item.WCID}) {(item.PTID > 0 ? $"Palette: {Enum.GetName(typeof(PALETTE_TEMPLATE), item.PTID)} ({item.PTID})" : "")} {(item.Shade > 0 ? $"Shade: {item.Shade})" : "")} Chance: {item.Chance / 1}% */" + Environment.NewLine;
                                        //}
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value} /* Loot Tier: {treasureTable.DeathTreasure[stat.Value].Tier} */)" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                                    }
                                    break;
                                default:
                                    didsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyDataId), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                                    break;
                            }
                        }
                    }
                    if (parsed.IIDValues != null)
                    {
                        foreach (var stat in parsed.IIDValues.OrderBy(x => x.Key))
                        {
                            // wcid 30732 has -1 for an IID.. i think this was to make it so noone could wield
                            // iidsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {(uint)stat.Value}) /* {Enum.GetName(typeof(STypeIID), stat.Key)} */" + Environment.NewLine;
                            iidsLine += $"     , ({parsed.WCID}, {((uint)stat.Key):000} /* {Enum.GetName(typeof(PropertyInstanceId), stat.Key)} */, {stat.Value})" + Environment.NewLine;
                        }
                    }
                    if (parsed.SpellCastingProbability != null)
                    {
                        foreach (var stat in parsed.SpellCastingProbability)
                        {
                            if (Enum.IsDefined(typeof(SpellID), stat.Key))
                                spellsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(SpellID), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.Attributes != null)
                    {
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Strength}, {(uint)parsed.Attributes.Strength.InitLevel}, {parsed.Attributes.Strength.LevelFromCP}, {parsed.Attributes.Strength.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Strength)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Endurance}, {(uint)parsed.Attributes.Endurance.InitLevel}, {parsed.Attributes.Endurance.LevelFromCP}, {parsed.Attributes.Endurance.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Endurance)} */" + Environment.NewLine;                        
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Quickness}, {(uint)parsed.Attributes.Quickness.InitLevel}, {parsed.Attributes.Quickness.LevelFromCP}, {parsed.Attributes.Quickness.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Quickness)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Coordination}, {(uint)parsed.Attributes.Coordination.InitLevel}, {parsed.Attributes.Coordination.LevelFromCP}, {parsed.Attributes.Coordination.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Coordination)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Focus}, {(uint)parsed.Attributes.Focus.InitLevel}, {parsed.Attributes.Focus.LevelFromCP}, {parsed.Attributes.Focus.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Focus)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute.Self}, {(uint)parsed.Attributes.Self.InitLevel}, {parsed.Attributes.Self.LevelFromCP}, {parsed.Attributes.Self.CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), PropertyAttribute.Self)} */" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.HEALTH_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._health})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.STAMINA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._stamina})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.MANA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._mana})" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute2nd.MaxHealth}, {(uint)parsed.Attributes.Health.InitLevel}, {parsed.Attributes.Health.LevelFromCP}, {parsed.Attributes.Health.CPSpent}, {parsed.Attributes.Health.Current}) /* {Enum.GetName(typeof(PropertyAttribute2nd), PropertyAttribute2nd.MaxHealth)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute2nd.MaxStamina}, {(uint)parsed.Attributes.Stamina.InitLevel}, {parsed.Attributes.Stamina.LevelFromCP}, {parsed.Attributes.Stamina.CPSpent}, {parsed.Attributes.Stamina.Current}) /* {Enum.GetName(typeof(PropertyAttribute2nd), PropertyAttribute2nd.MaxStamina)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)PropertyAttribute2nd.MaxMana}, {(uint)parsed.Attributes.Mana.InitLevel}, {parsed.Attributes.Mana.LevelFromCP}, {parsed.Attributes.Mana.CPSpent}, {parsed.Attributes.Mana.Current}) /* {Enum.GetName(typeof(PropertyAttribute2nd), PropertyAttribute2nd.MaxMana)} */" + Environment.NewLine;
                    }
                    if (parsed.PositionValues != null)
                    {
                        foreach (var stat in parsed.PositionValues.OrderBy(x => x.Key))
                        {
                            positionsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value.ObjCellID}, {stat.Value.Origin.X}, {stat.Value.Origin.Y}, {stat.Value.Origin.Z}, {stat.Value.Angles.W}, {stat.Value.Angles.X}, {stat.Value.Angles.Y}, {stat.Value.Angles.Z}) /* {Enum.GetName(typeof(PositionType), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.PagesData != null)
                    {
                        //if (parsed.PagesData.MaxNumPages > 0 && parsed.PagesData.Pages != null)
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)PropertyInt.APPRAISAL_PAGES_INT}, {(int)parsed.PagesData.Pages.Count}) /* {Enum.GetName(typeof(PropertyInt), PropertyInt.APPRAISAL_PAGES_INT)} */" + Environment.NewLine;
                        //if (parsed.PagesData.MaxNumPages > 0)
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)PropertyInt.APPRAISAL_MAX_PAGES_INT}, {(int)parsed.PagesData.MaxNumPages}) /* {Enum.GetName(typeof(PropertyInt), PropertyInt.APPRAISAL_MAX_PAGES_INT)} */" + Environment.NewLine;
                        //if (parsed.PagesData.MaxNumCharsPerPage > 0) // pretty sure this is wrong
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)PropertyInt.AVAILABLE_CHARACTER_INT}, {(int)parsed.PagesData.MaxNumCharsPerPage}) /* {Enum.GetName(typeof(PropertyInt), PropertyInt.AVAILABLE_CHARACTER_INT)} */" + Environment.NewLine;
                        //if (parsed.PagesData.MaxNumPages > 0 && parsed.PagesData.Pages != null && parsed.PagesData.MaxNumCharsPerPage > 0)
                        //{
                        booksLine += $"     , ({parsed.WCID}, {parsed.PagesData.MaxNumPages}, {parsed.PagesData.MaxNumCharsPerPage}) /* Book Data */" + Environment.NewLine;
                        //}
                        if (parsed.PagesData.Pages != null)
                        {
                            foreach (var page in parsed.PagesData.Pages)
                            {
                                pagesLine += $"     , ({parsed.WCID}, {parsed.PagesData.Pages.IndexOf(page)}, {page.AuthorID}, '{page.AuthorName.Replace("'", "''")}', '{page.AuthorAccount.Replace("'", "''")}', {page.IgnoreAuthor}, '{page.Text.Replace("'", "''")}')" + Environment.NewLine;
                            }
                        }
                    }
                    if (parsed.CreateList != null)
                    {
                        foreach (var instance in parsed.CreateList)
                        {
                            weenieNames.TryGetValue(Convert.ToUInt32(instance.WCID), out string label);
                            if (instance.WCID == 0)
                            {
                                if (parsed.DIDValues.ContainsKey((int)PropertyDataId.DeathTreasureType))
                                {
                                    if (treasureTable.DeathTreasure.ContainsKey(parsed.DIDValues[(int)PropertyDataId.DeathTreasureType]))
                                    {
                                        label = $"RANDOMLY GENERATED TREASURE from Loot Tier {treasureTable.DeathTreasure[parsed.DIDValues[(int)PropertyDataId.DeathTreasureType]].Tier}";
                                    }
                                    else if (treasureTable.WieldedTreasure.ContainsKey(parsed.DIDValues[(int)PropertyDataId.DeathTreasureType]))
                                    {
                                        label = "";
                                        foreach (var item in treasureTable.WieldedTreasure[parsed.DIDValues[(int)PropertyDataId.DeathTreasureType]])
                                        {
                                            label += $"{(item.Amount > 0 ? $"{item.Amount}" : "1")}x {weenieNames[item.WCID]} ({item.WCID}), ";
                                        }
                                        label = label.Substring(0, label.Length - 2) + " from Wielded Treasure Table";
                                    }
                                    else
                                    {
                                        label = "RANDOMLY GENERATED TREASURE";
                                    }
                                }
                                else
                                {
                                    label = "RANDOMLY GENERATED TREASURE";
                                }
                            }
                            //else if ((instance.Destination & (int)DestinationType.Treasure_DestinationType) != 0)
                            //else if (instance.Destination == (int)DestinationType.ContainTreasure_DestinationType)
                            //{
                            //    //if (TreasureTable.DeathTreasure.ContainsKey((uint)instance.WCID))
                            //    //{
                            //    //    label = $"RANDOM TREASURE from Loot Tier {TreasureTable.DeathTreasure[(uint)instance.WCID].Tier}";
                            //    //}
                            //    //else if (TreasureTable.WieldedTreasure.ContainsKey((uint)instance.WCID))
                            //    //{
                            //    //    label = "";
                            //    //    foreach (var item in TreasureTable.WieldedTreasure[(uint)instance.WCID])
                            //    //    {
                            //    //        label += $"{(item.Amount > 0 ? $"{item.Amount}" : "1")}x {weenieNames[item.WCID]} ({item.WCID}) {item.Chance * 100}% of the time, ";
                            //    //    }
                            //    //    label = label.Substring(0, label.Length - 2) + " from Wielded Treasure Table";
                            //    //}
                            //    //if (TreasureTable.DeathTreasure.ContainsKey((uint)instance.WCID))
                            //    //{
                            //    //    label = $"RANDOM TREASURE from Loot Tier {TreasureTable.DeathTreasure[(uint)instance.WCID].Tier}";
                            //    //}
                            //    //else
                            //    if (TreasureTable.WieldedTreasure.ContainsKey((uint)instance.WCID))
                            //    {
                            //        label = "";
                            //        foreach (var item in TreasureTable.WieldedTreasure[(uint)instance.WCID])
                            //        {
                            //            label += $"{(item.Amount > 0 ? $"{item.Amount}" : "1")}x {weenieNames[item.WCID]} ({item.WCID}) {item.Chance * 100}% of the time, ";
                            //        }
                            //        label = label.Substring(0, label.Length - 2) + " from Wielded Treasure Table";
                            //    }
                            //}
                            instancesLine += $"     , ({parsed.WCID}, {(uint)instance.Destination}, {instance.WCID}, {instance.StackSize}, {instance.Palette}, {instance.Shade}, {instance.TryToBond}" +
                                $") /* Create {label} for {Enum.GetName(typeof(DestinationType), instance.Destination)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.Generators != null)
                    {
                        foreach (var profile in parsed.Generators)
                        {
                            weenieNames.TryGetValue(Convert.ToUInt32(profile.Type), out string label);
                            if ((profile.WhereCreate & (int)RegenLocationType.Treasure) != 0)
                            {
                                //label = "";
                                if (treasureTable.DeathTreasure.ContainsKey((uint)profile.Type))
                                {
                                    label = $"RANDOM TREASURE from Loot Tier {treasureTable.DeathTreasure[(uint)profile.Type].Tier}";
                                }
                                else if (treasureTable.WieldedTreasure.ContainsKey((uint)profile.Type))
                                {
                                    label = "";
                                    foreach (var item in treasureTable.WieldedTreasure[(uint)profile.Type])
                                    {
                                        label += $"{(item.Amount > 0 ? $"{item.Amount}" : "1")}x {weenieNames[item.WCID]} ({item.WCID}) {item.Chance * 100}% of the time, ";
                                    }
                                    label = label.Substring(0, label.Length - 2) + " from Wielded Treasure Table";
                                }
                                else
                                {

                                }
                            }
                            profilesLine += $"     , ({parsed.WCID}, {profile.Probability}, {profile.Type}, {profile.Delay}, {(uint)profile.InitCreate}, {(uint)profile.MaxNum}" +
                            $", {profile.WhenCreate}, {profile.WhereCreate}, {profile.StackSize}, {profile.PalleteTypeID}, {profile.Shade}" +
                            $", {profile.Position.ObjCellID}, {profile.Position.Origin.X}, {profile.Position.Origin.Y}, {profile.Position.Origin.Z}" +
                            $", {profile.Position.Angles.W}, {profile.Position.Angles.X}, {profile.Position.Angles.Y}, {profile.Position.Angles.Z})" +
                            $"/* Generate {label} (x{profile.InitCreate:N0} up to max of {profile.MaxNum:N0}) - {Enum.GetName(typeof(RegenerationType), profile.WhenCreate)} - {Enum.GetName(typeof(RegenLocationType), profile.WhereCreate)} */" + Environment.NewLine;
                        }
                    }

                    if (parsed.BodyParts != null)
                    {
                        foreach (var bodypart in parsed.BodyParts.OrderBy(x => x.Key))
                        {
                            //(`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
                            bodyPartsLine += $"     , ({parsed.WCID}, {bodypart.Key}, {bodypart.Value.DType}, {bodypart.Value.DVal}, {bodypart.Value.DVar}, {bodypart.Value.ArmorValues.BaseArmor}, {bodypart.Value.ArmorValues.ArmorVsSlash}, {bodypart.Value.ArmorValues.ArmorVsPierce}, {bodypart.Value.ArmorValues.ArmorVsBludgeon}, {bodypart.Value.ArmorValues.ArmorVsCold}, {bodypart.Value.ArmorValues.ArmorVsFire}, {bodypart.Value.ArmorValues.ArmorVsAcid}, {bodypart.Value.ArmorValues.ArmorVsElectric}, {bodypart.Value.ArmorValues.ArmorVsNether}, {bodypart.Value.BH}, {bodypart.Value.SD.HLF}, {bodypart.Value.SD.MLF}, {bodypart.Value.SD.LLF}, {bodypart.Value.SD.HRF}, {bodypart.Value.SD.MRF}, {bodypart.Value.SD.LRF}, {bodypart.Value.SD.HLB}, {bodypart.Value.SD.MLB}, {bodypart.Value.SD.LLB}, {bodypart.Value.SD.HRB}, {bodypart.Value.SD.MRB}, {bodypart.Value.SD.LRB}) " + $"/* {Enum.GetName(typeof(Enums.BodyPart), bodypart.Key)} */" + Environment.NewLine;
                        }
                    }

                    if (parsed.EventFilters != null)
                    {
                        foreach (var filter in parsed.EventFilters)
                        {
                            eventsLine += $"     , ({parsed.WCID}, {filter}) " + $"/* {Enum.GetName(typeof(PacketOpcode), filter)} */" + Environment.NewLine;
                        }
                    }

                    if (parsed.Skills != null)
                    {
                        //(`object_Id`, `type`, `level_From_P_P`, `adjust_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
                        foreach (var skill in parsed.Skills.OrderBy(x => x.Key))
                        {
                            skillsLine += $"     , ({parsed.WCID}, {skill.Key}, {skill.Value.LevelFromPP}, {skill.Value.Sac} /* {((SkillStatus)skill.Value.Sac).ToString()} */, {skill.Value.PP}, {skill.Value.InitLevel}, {skill.Value.ResistanceAtLastCheck}, {skill.Value.LastUsedTime}) " + $"/* {Enum.GetName(typeof(ACE.Entity.Enum.Skill), skill.Key)} */" + Environment.NewLine;
                        }
                    }

                    if (parsed.Emotes != null)
                    {
                        foreach (var emote in parsed.Emotes)
                        {
                            var emoteSetId = 0;
                            foreach (var category in emote.Value)
                            {
                                //(`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
                                string quest = "";
                                if (category.Quest != null)
                                {
                                    quest = category.Quest.Replace("'", "''");
                                    quest = quest.Insert(0, "'").Insert(quest.Length + 1, "'");
                                }
                                emoteCategorysLine += $"     , ({parsed.WCID}, {category.Probability}, {category.Category} /* {Enum.GetName(typeof(EmoteCategory), category.Category)} */, {emoteSetId}, {((category.ClassID.HasValue) ? category.ClassID + $" /* {weenieNames[(uint)category.ClassID]} */" : "NULL")}, {((category.Style.HasValue) ? (category.Style + $" /* {((MotionStance)category.Style).ToString()} */") : "NULL")}, {((category.Substyle.HasValue) ? (category.Substyle + $" /* {((MotionCommand)category.Substyle).ToString()} */") : "NULL")}, {((category.Quest != null) ? quest : "NULL")}, {((category.VendorType.HasValue) ? (category.VendorType + $" /* {((VendorType)category.VendorType).ToString()} */") : "NULL")}, {((category.MinHealth.HasValue) ? category.MinHealth.ToString() : "NULL")}, {((category.MaxHealth.HasValue) ? category.MaxHealth.ToString() : "NULL")})" + Environment.NewLine;
                                if (category.EmoteActions != null)
                                {
                                    var order = 0;
                                    foreach (var action in category.EmoteActions)
                                    {
                                        //(`object_Id`, `emote_Category`, `type`, `order`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
                                        string message = "";
                                        if (action.Message != null)
                                        {
                                            message = action.Message.Replace("'", "''");
                                            message = message.Insert(0, "'").Insert(message.Length + 1, "'");
                                        }
                                        string testString = "";
                                        if (action.TestString != null)
                                        {
                                            testString = action.TestString.Replace("'", "''");
                                            testString = testString.Insert(0, "'").Insert(testString.Length + 1, "'");
                                        }
                                        if (action.Position != null)
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category} /* {Enum.GetName(typeof(EmoteCategory), category.Category)} */, {emoteSetId}, {order}, {action.Type} /* {Enum.GetName(typeof(EmoteType), action.Type)} */, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? (action.Motion + $" /* {((MotionCommand)action.Motion).ToString()} */") : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? (action.SpellID + $" /* {((SpellID)action.SpellID).ToString()} */") : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? (action.PScript + $" /* {((PlayScript)action.PScript).ToString()} */") : "NULL")}, {((action.Sound.HasValue) ? (action.Sound + $" /* {((Sound)action.Sound).ToString()} */") : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID + $" /* {weenieNames[(uint)action.Item.WCID]} */" : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.Z.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.W.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.Z.ToString(CultureInfo.InvariantCulture) : "NULL")})" + Environment.NewLine;
                                        else if (action.Frame != null)
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category} /* {Enum.GetName(typeof(EmoteCategory), category.Category)} */, {emoteSetId}, {order}, {action.Type} /* {Enum.GetName(typeof(EmoteType), action.Type)} */, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? (action.Motion + $" /* {((MotionCommand)action.Motion).ToString()} */") : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? (action.SpellID + $" /* {((SpellID)action.SpellID).ToString()} */") : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? (action.PScript + $" /* {((PlayScript)action.PScript).ToString()} */") : "NULL")}, {((action.Sound.HasValue) ? (action.Sound + $" /* {((Sound)action.Sound).ToString()} */") : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID + $" /* {weenieNames[(uint)action.Item.WCID]} */" : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.Z.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.W.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.Z.ToString(CultureInfo.InvariantCulture) : "NULL")})" + Environment.NewLine;
                                        else
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category} /* {Enum.GetName(typeof(EmoteCategory), category.Category)} */, {emoteSetId}, {order}, {action.Type} /* {Enum.GetName(typeof(EmoteType), action.Type)} */, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? (action.Motion + $" /* {((MotionCommand)action.Motion).ToString()} */") : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? (action.SpellID + $" /* {((SpellID)action.SpellID).ToString()} */") : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? (action.PScript + $" /* {((PlayScript)action.PScript).ToString()} */") : "NULL")}, {((action.Sound.HasValue) ? (action.Sound + $" /* {((Sound)action.Sound).ToString()} */") : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID + $" /* {weenieNames[(uint)action.Item.WCID]} */" : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Origin.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Origin.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Origin.Z.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Angles.W.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Angles.X.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Angles.Y.ToString(CultureInfo.InvariantCulture) : "NULL")}, {((action.Position != null) ? action.Position.Angles.Z.ToString(CultureInfo.InvariantCulture) : "NULL")})" + Environment.NewLine;
                                        order++;
                                    }
                                }
                                emoteSetId++;
                            }
                        }
                    }

                    // intsLine += $"     , ({parsed.WCID}, {(uint)9007}, {parsed.WeenieType}) /* {Enum.GetName(typeof(WeenieType), parsed.WeenieType)} */" + Environment.NewLine;

                    if (strsLine != "")
                    {
                        strsLine = $"{sqlCommand} INTO `weenie_properties_string` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + strsLine.TrimStart("     ,".ToCharArray());
                        strsLine = strsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(strsLine);
                    }
                    if (didsLine != "")
                    {
                        didsLine = $"{sqlCommand} INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + didsLine.TrimStart("     ,".ToCharArray());
                        didsLine = didsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(didsLine);
                    }
                    if (iidsLine != "")
                    {
                        iidsLine = $"{sqlCommand} INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + iidsLine.TrimStart("     ,".ToCharArray());
                        iidsLine = iidsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(iidsLine);
                    }
                    if (intsLine != "")
                    {
                        intsLine = $"{sqlCommand} INTO `weenie_properties_int` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + intsLine.TrimStart("     ,".ToCharArray());
                        intsLine = intsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(intsLine);
                    }
                    if (bigintsLine != "")
                    {
                        bigintsLine = $"{sqlCommand} INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + bigintsLine.TrimStart("     ,".ToCharArray());
                        bigintsLine = bigintsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(bigintsLine);
                    }
                    if (floatsLine != "")
                    {
                        floatsLine = $"{sqlCommand} INTO `weenie_properties_float` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + floatsLine.TrimStart("     ,".ToCharArray());
                        floatsLine = floatsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(floatsLine);
                    }
                    if (boolsLine != "")
                    {
                        boolsLine = $"{sqlCommand} INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)" + Environment.NewLine
                            + "VALUES " + boolsLine.TrimStart("     ,".ToCharArray());
                        boolsLine = boolsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(boolsLine);
                    }
                    if (spellsLine != "")
                    {
                        spellsLine = $"{sqlCommand} INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)" + Environment.NewLine
                            + "VALUES " + spellsLine.TrimStart("     ,".ToCharArray());
                        spellsLine = spellsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(spellsLine);
                    }
                    if (attributesLine != "")
                    {
                        attributesLine = $"{sqlCommand} INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)" + Environment.NewLine
                            + "VALUES " + attributesLine.TrimStart("     ,".ToCharArray());
                        attributesLine = attributesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(attributesLine);
                    }
                    if (attribute2ndsLine != "")
                    {
                        attribute2ndsLine = $"{sqlCommand} INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)" + Environment.NewLine
                            + "VALUES " + attribute2ndsLine.TrimStart("     ,".ToCharArray());
                        attribute2ndsLine = attribute2ndsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(attribute2ndsLine);
                    }
                    if (positionsLine != "")
                    {
                        positionsLine = $"{sqlCommand} INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + positionsLine.TrimStart("     ,".ToCharArray());
                        positionsLine = positionsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(positionsLine);
                    }
                    if (booksLine != "")
                    {
                        booksLine = $"{sqlCommand} INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)" + Environment.NewLine
                            + "VALUES " + booksLine.TrimStart("     ,".ToCharArray());
                        booksLine = booksLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(booksLine);
                    }
                    if (pagesLine != "")
                    {
                        pagesLine = $"{sqlCommand} INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)" + Environment.NewLine
                            + "VALUES " + pagesLine.TrimStart("     ,".ToCharArray());
                        pagesLine = pagesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(pagesLine);
                    }
                    if (instancesLine != "")
                    {
                        instancesLine = $"{sqlCommand} INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)" + Environment.NewLine
                            + "VALUES " + instancesLine.TrimStart("     ,".ToCharArray());
                        instancesLine = instancesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(instancesLine);
                    }
                    if (profilesLine != "")
                    {
                        profilesLine = $"{sqlCommand} INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + profilesLine.TrimStart("     ,".ToCharArray());
                        profilesLine = profilesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(profilesLine);
                    }
                    if (bodyPartsLine != "")
                    {
                        bodyPartsLine = $"{sqlCommand} INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)" + Environment.NewLine
                            + "VALUES " + bodyPartsLine.TrimStart("     ,".ToCharArray());
                        bodyPartsLine = bodyPartsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(bodyPartsLine);
                    }
                    if (eventsLine != "")
                    {
                        eventsLine = $"{sqlCommand} INTO `weenie_properties_event_filter` (`object_Id`, `event`)" + Environment.NewLine
                            + "VALUES " + eventsLine.TrimStart("     ,".ToCharArray());
                        eventsLine = eventsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(eventsLine);
                    }
                    if (skillsLine != "")
                    {
                        //skillsLine = $"{sqlCommand} INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `adjust_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)" + Environment.NewLine
                        skillsLine = $"{sqlCommand} INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)" + Environment.NewLine
                            + "VALUES " + skillsLine.TrimStart("     ,".ToCharArray());
                        skillsLine = skillsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(skillsLine);
                    }
                    if (emoteCategorysLine != "")
                    {
                        emoteCategorysLine = $"{sqlCommand} INTO `weenie_properties_emote` (`object_Id`, `probability`, `category`, `emote_Set_Id`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)" + Environment.NewLine
                            + "VALUES " + emoteCategorysLine.TrimStart("     ,".ToCharArray());
                        emoteCategorysLine = emoteCategorysLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(emoteCategorysLine);
                    }
                    if (emoteActionsLine != "")
                    {
                        emoteActionsLine = $"{sqlCommand} INTO `weenie_properties_emote_action` (`object_Id`, `emote_Category`, `emote_Set_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + emoteActionsLine.TrimStart("     ,".ToCharArray());
                        emoteActionsLine = emoteActionsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(emoteActionsLine);
                    }
                    var counter = Interlocked.Increment(ref processedCounter);
                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarWeenies.Value = (int)(((double)counter / weenieDefaults.Weenies.Count) * 100)));
                }
                //});
            }

            // parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
        }

        private void WriteLandblockFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "6 LandBlockExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            uint highestWeenieFound = 0;

            //Parallel.For(0, parsedObjects.Count, i =>
            foreach (var landblock in landBlockData.Landblocks)
            {
                string FileNameFormatter(Landblock obj) => obj.Key.ToString("X4");

                string fileNameFormatter = FileNameFormatter(landblock);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    // var parsed = parsedObjects[i] as Landblock;
                    var parsed = landblock;

                    string instanceLine = "", sourcesLine = "", targetsLine = "";

                    Dictionary<uint, List<uint>> targets = new Dictionary<uint, List<uint>>();

                    Dictionary<uint, string> instanceNames = new Dictionary<uint, string>();

                    if (parsed.Weenies != null)
                    {
                        foreach (var instance in parsed.Weenies)
                        {
                            if (instance.WCID > highestWeenieFound)
                                highestWeenieFound = instance.WCID;

                            // Somebody goofed and a guid was used in two places... I'm not sure that it ultimately was a problem on retail worlds but this fixes it for ACE
                            if (instance.ID == 1975799995)
                            {
                                if (instance.WCID == 22775)
                                    instance.ID = 1975799994; // Unused guid.
                            }

                            //// ACE has a problem currently dealing with objects placed at xxxx0000 of a landblock, this moves any object to xxxx0001 for now
                            //string landblockHex = instance.Position.ObjCellID.ToString("X8");
                            //if (landblockHex.EndsWith("0000"))
                            //{
                            //    landblockHex = landblockHex.Substring(0, 4) + "0001";
                            //    instance.Position.ObjCellID = Convert.ToUInt32(landblockHex, 16);
                            //}

                            instanceLine += $"     , ({instance.WCID}, {instance.ID}, " +
                                $"{instance.Position.ObjCellID}, " +
                                $"{instance.Position.Origin.X}, {instance.Position.Origin.Y}, {instance.Position.Origin.Z}, " +
                                $"{instance.Position.Angles.W}, {instance.Position.Angles.X}, {instance.Position.Angles.Y}, {instance.Position.Angles.Z}" +
                            $") /* {weenieNames[instance.WCID]} */" + Environment.NewLine;

                            if (!instanceNames.ContainsKey(instance.ID))
                                instanceNames.Add(instance.ID, weenieNames[instance.WCID]);
                        }
                    }

                    if (parsed.Links != null)
                    {
                        foreach (var link in parsed.Links)
                        {
                            if (!targets.ContainsKey(link.Target))
                                targets.Add(link.Target, new List<uint>());

                            targets[link.Target].Add(link.Source);
                        }
                    }

                    int slotId = 1;
                    foreach (var link in targets)
                    {
                        targetsLine += $"UPDATE `landblock_instances` SET `link_Slot`='{slotId}', `link_Controller`={true} WHERE `guid`='{link.Key}'; /* {instanceNames[link.Key]} */" + Environment.NewLine; //+

                        foreach (var source in link.Value)
                        {
                            sourcesLine += $"UPDATE `landblock_instances` SET `link_Slot`='{slotId}' WHERE `guid`='{source}'; /* {instanceNames[link.Key]} <- {instanceNames[source]} */" + Environment.NewLine;
                        }

                        slotId++;
                    }

                    if (instanceLine != "")
                    {
                        instanceLine = $"{sqlCommand} INTO `landblock_instances` (`weenie_Class_Id`, `guid`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + instanceLine.TrimStart("     ,".ToCharArray());
                        instanceLine = instanceLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(instanceLine);
                    }

                    if (targetsLine != "")
                    {
                        targetsLine = targetsLine.TrimEnd(Environment.NewLine.ToCharArray()) + "" + Environment.NewLine;
                        writer.WriteLine(targetsLine);
                    }

                    if (sourcesLine != "")
                    {
                        sourcesLine = sourcesLine.TrimEnd(Environment.NewLine.ToCharArray()) + "" + Environment.NewLine;
                        writer.WriteLine(sourcesLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarLandblocks.Value = (int)(((double)counter / landBlockData.Landblocks.Count) * 100)));
                }
                //});               
                }

            //string fileName = "TerrainData";

            //using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            //{
            //    foreach (var terrainLandblock in LandBlockData.TerrainLandblocks)
            //    {
            //        var parsed = terrainLandblock;

            //        string encounterLine = "";

            //        //        //foreach (var generator in encounter.Values)
            //        //        //{
            //        //        //    weenieNames.TryGetValue(generator, out string label);
            //        //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //        //}

            //        //encounterLine += $"     , ({LandBlockData.TerrainLandblocks.IndexOf(terrain)}, {map.Index})" + $" /* {LandBlockData.TerrainLandblocks.IndexOf(terrain).ToString("X4")} */" + Environment.NewLine;
            //        foreach (var terrain in terrainLandblock.Terrain)
            //        {
            //            encounterLine += $"     , ({LandBlockData.TerrainLandblocks.IndexOf(terrainLandblock)}, {terrain})" + $" /* {LandBlockData.TerrainLandblocks.IndexOf(terrainLandblock).ToString("X4")} */" + Environment.NewLine;
            //        }

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `terrain` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        //        var counter = Interlocked.Increment(ref processedCounter);

            //        //        if ((counter % 1000) == 0)
            //        //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }

            //}

            //parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
            //System.Diagnostics.Debug.WriteLine($"Highest Weenie Exported in WorldSpawn was: {highestWeenieFound}");
        }

        private void cmd6LandblocksParse_Click(object sender, EventArgs e)
        {
            cmd6LandblocksParse.Enabled = false;

            progressBarLandblocks.Style = ProgressBarStyle.Continuous;
            progressBarLandblocks.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                // For example
                //foreach (var weenie in WeenieDefaults.Weenies)
                //	;

                WriteLandblockFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarLandblocks.Style = ProgressBarStyle.Continuous;
                    progressBarLandblocks.Value = 100;

                    cmd6LandblocksParse.Enabled = true;
                }));
            });
        }

        private void WriteSpellFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "2 SpellTableExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var spell in spellTableExtendedData.Spells)
            {
                string FileNameFormatter(Spell obj) => obj.ID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(spell);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    //`spell_Id`, `name`, `description`, `school`, `icon_Id`, `category`, `bitfield`, `mana`, `range_Constant`, `range_Mod`, `power`, `economy_Mod`, `formula_Version`, `component_Loss`, `meta_Spell_Type`, `meta_Spell_Id`, `spell_Formula_Comp_1_Component_Id`, `spell_Formula_Comp_2_Component_Id`, `spell_Formula_Comp_3_Component_Id`, `spell_Formula_Comp_4_Component_Id`, `spell_Formula_Comp_5_Component_Id`, `spell_Formula_Comp_6_Component_Id`, `spell_Formula_Comp_7_Component_Id`, `spell_Formula_Comp_8_Component_Id`, `caster_Effect`, `target_Effect`, `fizzle_Effect`, `recovery_Interval`, `recovery_Amount`, `display_Order`, `non_Component_Target_Type`, `mana_Mod`

                    var spellLineHdr = $"{sqlCommand} INTO `spell` (`spell_Id`, `name`, `description`, `school`, `icon_Id`, `category`, `bitfield`, `mana`, `range_Constant`, `range_Mod`, `power`, `economy_Mod`, `formula_Version`, `component_Loss`, `meta_Spell_Type`, `meta_Spell_Id`, `spell_Formula_Comp_1_Component_Id`, `spell_Formula_Comp_2_Component_Id`, `spell_Formula_Comp_3_Component_Id`, `spell_Formula_Comp_4_Component_Id`, `spell_Formula_Comp_5_Component_Id`, `spell_Formula_Comp_6_Component_Id`, `spell_Formula_Comp_7_Component_Id`, `spell_Formula_Comp_8_Component_Id`, `caster_Effect`, `target_Effect`, `fizzle_Effect`, `recovery_Interval`, `recovery_Amount`, `display_Order`, `non_Component_Target_Type`, `mana_Mod`";
                    var spellLine = $"({spell.ID}, '{spell.Name.Replace("'", "''")}', '{spell.Description.Replace("'", "''")}', {(int)spell.School} /* {Enum.GetName(typeof(School), spell.School)} */, {spell.IconID}, {spell.Category}, {spell.Bitfield} /* {((SpellBitfield)spell.Bitfield).ToString()} */, {spell.Mana}, {spell.RangeConstant}, {spell.RangeMod}, {spell.Power}, {spell.EconomyMod}, {spell.FormulaVersion}, {spell.ComponentLoss}, {(int)spell.MetaSpellType} /* {Enum.GetName(typeof(ACE.Entity.Enum.SpellType), spell.MetaSpellType)} */, {spell.MetaSpellId}, {spell.SpellFormula.Comps[0]}, {spell.SpellFormula.Comps[1]}, {spell.SpellFormula.Comps[2]}, {spell.SpellFormula.Comps[3]}, {spell.SpellFormula.Comps[4]}, {spell.SpellFormula.Comps[5]}, {spell.SpellFormula.Comps[6]}, {spell.SpellFormula.Comps[7]}, {spell.CasterEffect}, {spell.TargetEffect}, {spell.FizzleEffect}, {spell.RecoveryInterval}, {spell.RecoveryAmount}, {spell.DisplayOrder}, {spell.NonComponentTargetType}, {spell.ManaMod}";

                    //, `duration`, `degrade_Modifier`, `degrade_Limit`, `stat_Mod_Type`, `stat_Mod_Key`, `stat_Mod_Val`, `e_Type`, `base_Intensity`, `variance`, `wcid`, `num_Projectiles` 
                    //, `num_Projectiles_Variance`, `spread_Angle`, `vertical_Angle`, `default_Launch_Angle`, `non_Tracking`, `create_Offset_Origin_X`, `create_Offset_Origin_Y`
                    //, `create_Offset_Origin_Z`, `padding_Origin_X`, `padding_Origin_Y`, `padding_Origin_Z`, `dims_Origin_X`, `dims_Origin_Y`, `dims_Origin_Z`, `peturbation_Origin_X`
                    //, `peturbation_Origin_Y`, `peturbation_Origin_Z`, `imbued_Effect`, `slayer_Creature_Type`, `slayer_Damage_Bonus`, `crit_Freq`, `crit_Multiplier`, `ignore_Magic_Resist`
                    //, `elemental_Modifier`, `drain_Percentage`, `damage_Ratio`, `damage_Type`, `boost`, `boost_Variance`, `source`, `destination`, `proportion`, `loss_Percent`, `source_Loss`
                    //, `transfer_Cap`, `max_Boost_Allowed`, `transfer_Bitfield`, `index`, `portal_Lifetime`, `link`, `position_Obj_Cell_ID`, `position_Origin_X`, `position_Origin_Y`
                    //, `position_Origin_Z`, `position_Angles_W`, `position_Angles_X`, `position_Angles_Y`, `position_Angles_Z`, `min_Power`, `max_Power`, `power_Variance`, `dispel_School`
                    //, `align`, `number`, `number_Variance

                    if (spell.Duration.HasValue)
                    {
                        spellLineHdr += ", `duration`";
                        spellLine += $", {spell.Duration}";
                    }

                    if (spell.DegradeModifier.HasValue)
                    {
                        spellLineHdr += ", `degrade_Modifier`";
                        spellLine += $", {spell.DegradeModifier}";
                    }

                    if (spell.DegradeLimit.HasValue)
                    {
                        spellLineHdr += ", `degrade_Limit`";
                        spellLine += $", {spell.DegradeLimit}";
                    }

                    if (spell.SpellStatMod != null)
                    {
                        spellLineHdr += ", `stat_Mod_Type`, `stat_Mod_Key`, `stat_Mod_Val`";
                        spellLine += $", {spell.SpellStatMod.Type} /* {((EnchantmentTypeFlags)spell.SpellStatMod.Type).ToString()} */, {spell.SpellStatMod.Key}, {spell.SpellStatMod.Val}";
                    }

                    if (spell.EType.HasValue)
                    {
                        spellLineHdr += ", `e_Type`";
                        spellLine += $", {spell.EType}";
                    }

                    if (spell.BaseIntensity.HasValue)
                    {
                        spellLineHdr += ", `base_Intensity`";
                        spellLine += $", {spell.BaseIntensity}";
                    }
                    
                    if (spell.Variance.HasValue)
                    {
                        spellLineHdr += ", `variance`";
                        spellLine += $", {spell.Variance}";
                    }

                    if (spell.WCID.HasValue)
                    {
                        spellLineHdr += ", `wcid`";
                        spellLine += $", {spell.WCID} /* {weenieNames[spell.WCID.Value]} */";
                    }

                    if (spell.NumProjectiles.HasValue)
                    {
                        spellLineHdr += ", `num_Projectiles`";
                        spellLine += $", {spell.NumProjectiles}";
                    }

                    if (spell.NumProjectilesVariance.HasValue)
                    {
                        spellLineHdr += ", `num_Projectiles_Variance`";
                        spellLine += $", {spell.NumProjectilesVariance}";
                    }

                    if (spell.SpreadAngle.HasValue)
                    {
                        spellLineHdr += ", `spread_Angle`";
                        spellLine += $", {spell.SpreadAngle}";
                    }

                    if (spell.VerticalAngle.HasValue)
                    {
                        spellLineHdr += ", `vertical_Angle`";
                        spellLine += $", {spell.VerticalAngle}";
                    }

                    if (spell.DefaultLaunchAngle.HasValue)
                    {
                        spellLineHdr += ", `default_Launch_Angle`";
                        spellLine += $", {spell.DefaultLaunchAngle}";
                    }

                    if (spell.NonTracking.HasValue)
                    {
                        spellLineHdr += ", `non_Tracking`";
                        spellLine += $", {spell.NonTracking}";
                    }

                    if (spell.CreateOffset != null)
                    {
                        spellLineHdr += ", `create_Offset_Origin_X`, `create_Offset_Origin_Y`, `create_Offset_Origin_Z`";
                        spellLine += $", {spell.CreateOffset.X}, {spell.CreateOffset.Y}, {spell.CreateOffset.Z}";
                    }

                    if (spell.Padding != null)
                    {
                        spellLineHdr += ", `padding_Origin_X`, `padding_Origin_Y`, `padding_Origin_Z`";
                        spellLine += $", {spell.Padding.X}, {spell.Padding.Y}, {spell.Padding.Z}";
                    }

                    if (spell.Dims != null)
                    {
                        spellLineHdr += ", `dims_Origin_X`, `dims_Origin_Y`, `dims_Origin_Z`";
                        spellLine += $", {spell.Dims.X}, {spell.Dims.Y}, {spell.Dims.Z}";
                    }

                    if (spell.Peturbation != null)
                    {
                        spellLineHdr += ", `peturbation_Origin_X`, `peturbation_Origin_Y`, `peturbation_Origin_Z`";
                        spellLine += $", {spell.Peturbation.X}, {spell.Peturbation.Y}, {spell.Peturbation.Z}";
                    }

                    if (spell.ImbuedEffect.HasValue)
                    {
                        spellLineHdr += ", `imbued_Effect`";
                        spellLine += $", {spell.ImbuedEffect} /* {Enum.GetName(typeof(ImbuedEffectType), spell.ImbuedEffect)} */";
                    }

                    if (spell.SlayerCreatureType.HasValue)
                    {
                        spellLineHdr += ", `slayer_Creature_Type`";
                        spellLine += $", {spell.SlayerCreatureType} /* {Enum.GetName(typeof(CreatureType), spell.SlayerCreatureType)} */";
                    }

                    if (spell.SlayerDamageBonus.HasValue)
                    {
                        spellLineHdr += ", `slayer_Damage_Bonus`";
                        spellLine += $", {spell.SlayerDamageBonus}";
                    }

                    if (spell.CritFreq.HasValue)
                    {
                        spellLineHdr += ", `crit_Freq`";
                        spellLine += $", {spell.CritFreq}";
                    }

                    if (spell.CritMultiplier.HasValue)
                    {
                        spellLineHdr += ", `crit_Multiplier`";
                        spellLine += $", {spell.CritMultiplier}";
                    }

                    if (spell.IgnoreMagicResist.HasValue)
                    {
                        spellLineHdr += ", `ignore_Magic_Resist`";
                        spellLine += $", {spell.IgnoreMagicResist}";
                    }

                    if (spell.ElementalModifier.HasValue)
                    {
                        spellLineHdr += ", `elemental_Modifier`";
                        spellLine += $", {spell.ElementalModifier}";
                    }

                    if (spell.DrainPercentage.HasValue)
                    {
                        spellLineHdr += ", `drain_Percentage`";
                        spellLine += $", {spell.DrainPercentage}";
                    }

                    if (spell.DamageRatio.HasValue)
                    {
                        spellLineHdr += ", `damage_Ratio`";
                        spellLine += $", {spell.DamageRatio}";
                    }

                    if (spell.DamageType.HasValue)
                    {
                        spellLineHdr += ", `damage_Type`";
                        spellLine += $", {(int)spell.DamageType} /* {Enum.GetName(typeof(DamageType), spell.DamageType)} */";
                    }

                    if (spell.Boost.HasValue)
                    {
                        spellLineHdr += ", `boost`";
                        spellLine += $", {spell.Boost}";
                    }

                    if (spell.BoostVariance.HasValue)
                    {
                        spellLineHdr += ", `boost_Variance`";
                        spellLine += $", {spell.BoostVariance}";
                    }

                    if (spell.Source.HasValue)
                    {
                        spellLineHdr += ", `source`";
                        spellLine += $", {(int)spell.Source} /* {Enum.GetName(typeof(PropertyAttribute2nd), spell.Source)} */";
                    }

                    if (spell.Destination.HasValue)
                    {
                        spellLineHdr += ", `destination`";
                        spellLine += $", {(int)spell.Destination} /* {Enum.GetName(typeof(PropertyAttribute2nd), spell.Destination)} */";
                    }

                    if (spell.Proportion.HasValue)
                    {
                        spellLineHdr += ", `proportion`";
                        spellLine += $", {spell.Proportion}";
                    }

                    if (spell.LossPercent.HasValue)
                    {
                        spellLineHdr += ", `loss_Percent`";
                        spellLine += $", {spell.LossPercent}";
                    }

                    if (spell.SourceLoss.HasValue)
                    {
                        spellLineHdr += ", `source_Loss`";
                        spellLine += $", {spell.SourceLoss}";
                    }

                    if (spell.TransferCap.HasValue)
                    {
                        spellLineHdr += ", `transfer_Cap`";
                        spellLine += $", {spell.TransferCap}";
                    }

                    if (spell.MaxBoostAllowed.HasValue)
                    {
                        spellLineHdr += ", `max_Boost_Allowed`";
                        spellLine += $", {spell.MaxBoostAllowed}";
                    }

                    if (spell.TransferBitfield.HasValue)
                    {
                        spellLineHdr += ", `transfer_Bitfield`";
                        spellLine += $", {spell.TransferBitfield}";
                    }

                    if (spell.Index.HasValue)
                    {
                        spellLineHdr += ", `index`";
                        spellLine += $", {spell.Index}";
                    }

                    if (spell.PortalLifetime.HasValue)
                    {
                        spellLineHdr += ", `portal_Lifetime`";
                        spellLine += $", {spell.PortalLifetime}";
                    }

                    if (spell.Link.HasValue)
                    {
                        spellLineHdr += ", `link`";
                        spellLine += $", {spell.Link}";
                    }

                    if (spell.Position != null)
                    {
                        spellLineHdr += ", `position_Obj_Cell_ID`, `position_Origin_X`, `position_Origin_Y`, `position_Origin_Z`, `position_Angles_W`, `position_Angles_X`, `position_Angles_Y`, `position_Angles_Z`";
                        spellLine += $", {spell.Position.ObjCellID}, {spell.Position.Origin.X}, {spell.Position.Origin.Y}, {spell.Position.Origin.Z}, {spell.Position.Angles.W}, {spell.Position.Angles.X}, {spell.Position.Angles.Y}, {spell.Position.Angles.Z}";
                    }

                    if (spell.MinPower.HasValue)
                    {
                        spellLineHdr += ", `min_Power`";
                        spellLine += $", {spell.MinPower}";
                    }

                    if (spell.MaxPower.HasValue)
                    {
                        spellLineHdr += ", `max_Power`";
                        spellLine += $", {spell.MaxPower}";
                    }

                    if (spell.PowerVariance.HasValue)
                    {
                        spellLineHdr += ", `power_Variance`";
                        spellLine += $", {spell.PowerVariance}";
                    }

                    if (spell.DispelSchool.HasValue)
                    {
                        spellLineHdr += ", `dispel_School`";
                        spellLine += $", {(int)spell.DispelSchool} /* {Enum.GetName(typeof(School), spell.DispelSchool)} */";
                    }

                    if (spell.Align.HasValue)
                    {
                        spellLineHdr += ", `align`";
                        spellLine += $", {spell.Align}";
                    }

                    if (spell.Number.HasValue)
                    {
                        spellLineHdr += ", `number`";
                        spellLine += $", {spell.Number}";
                    }

                    if (spell.NumberVariance.HasValue)
                    {
                        spellLineHdr += ", `number_Variance`";
                        spellLine += $", {spell.NumberVariance}";
                    }

                    spellLineHdr += ")" + Environment.NewLine + "VALUES ";
                    spellLine += ");";

                    if (spellLine != "")
                    {
                        writer.WriteLine(spellLineHdr + spellLine);
                    }
                    
                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarSpells.Value = (int)(((double)counter / spellTableExtendedData.Spells.Count) * 100)));
                }
            }
        }

        private void cmd2SpellsParse_Click(object sender, EventArgs e)
        {
            cmd2SpellsParse.Enabled = false;

            progressBarSpells.Style = ProgressBarStyle.Continuous;
            progressBarSpells.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteSpellFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarSpells.Style = ProgressBarStyle.Continuous;
                    progressBarSpells.Value = 100;

                    cmd2SpellsParse.Enabled = true;
                }));
            });
        }

        private void WriteQuestFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "8 QuestDefDB" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var quest in questDefDB.QuestDefs)
            {
                string FileNameFormatter(QuestDef obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(quest);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    // `name`, `min_Delta`, `max_Solves`, `message`

                    var questLineHdr = $"{sqlCommand} INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`";
                    var questLine = $"('{quest.Name.Replace("'", "''")}', {quest.MinDelta}, {quest.MaxSolves}, '{quest.Message.Replace("'", "''")}'";

                    questLineHdr += ")" + Environment.NewLine + "VALUES ";
                    questLine += ");";

                    if (questLine != "")
                    {
                        writer.WriteLine(questLineHdr + questLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarQuests.Value = (int)(((double)counter / questDefDB.QuestDefs.Count) * 100)));
                }
            }
        }

        private void cmd8QuestsParse_Click(object sender, EventArgs e)
        {
            cmd8QuestsParse.Enabled = false;

            progressBarQuests.Style = ProgressBarStyle.Continuous;
            progressBarQuests.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteQuestFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarQuests.Style = ProgressBarStyle.Continuous;
                    progressBarQuests.Value = 100;

                    cmd8QuestsParse.Enabled = true;
                }));
            });
        }

        private void WriteHouseFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "5 HousingPortals" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var house in housingPortalsTable.HousingPortals)
            {
                string FileNameFormatter(HousingPortal obj) => obj.HouseId.ToString("00000");

                string fileNameFormatter = FileNameFormatter(house);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string houseLine = "";

                    foreach (var destination in house.Destinations)
                    {
                        houseLine += $"     , ({house.HouseId}, {destination.ObjCellID}, {destination.Origin.X}, {destination.Origin.Y}, {destination.Origin.Z}, {destination.Angles.W}, {destination.Angles.X}, {destination.Angles.Y}, {destination.Angles.Z})" + Environment.NewLine;
                    }

                    if (houseLine != "")
                    {
                        houseLine = $"{sqlCommand} INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + houseLine.TrimStart("     ,".ToCharArray());
                        houseLine = houseLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(houseLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarHousing.Value = (int)(((double)counter / housingPortalsTable.HousingPortals.Count) * 100)));
                }
            }
        }

        private void cmd5HousingParse_Click(object sender, EventArgs e)
        {
            cmd5HousingParse.Enabled = false;

            progressBarHousing.Style = ProgressBarStyle.Continuous;
            progressBarHousing.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteHouseFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarHousing.Style = ProgressBarStyle.Continuous;
                    progressBarHousing.Value = 100;

                    cmd5HousingParse.Enabled = true;
                }));
            });
        }

        private void WriteEventFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "B GameEventDefDB" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var gameEvent in gameEventDefDB.GameEventDefs)
            {
                string FileNameFormatter(GameEventDef obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(gameEvent);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string eventLine = "";

                    eventLine += $"     , ('{gameEvent.Name.Replace("'", "''")}', {(gameEvent.StartTime == -1 ? $"{gameEvent.StartTime}" : $"{gameEvent.StartTime} /* {DateTimeOffset.FromUnixTimeSeconds(gameEvent.StartTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, {(gameEvent.EndTime == -1 ? $"{gameEvent.EndTime}" : $"{gameEvent.EndTime} /* {DateTimeOffset.FromUnixTimeSeconds(gameEvent.EndTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, {(int)gameEvent.GameEventState})" + Environment.NewLine;

                    if (eventLine != "")
                    {
                        eventLine = $"{sqlCommand} INTO `event` (`name`, `start_Time`, `end_Time`, `state`)" + Environment.NewLine
                            + "VALUES " + eventLine.TrimStart("     ,".ToCharArray());
                        eventLine = eventLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(eventLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarEvents.Value = (int)(((double)counter / gameEventDefDB.GameEventDefs.Count) * 100)));
                }
            }
        }

        private void cmdBEventsParse_Click(object sender, EventArgs e)
        {
            cmdBEventsParse.Enabled = false;

            progressBarEvents.Style = ProgressBarStyle.Continuous;
            progressBarEvents.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteEventFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarEvents.Style = ProgressBarStyle.Continuous;
                    progressBarEvents.Value = 100;

                    cmdBEventsParse.Enabled = true;
                }));
            });
        }

        private void WriteRegionFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "1 RegionDescExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var encounter in regionDescExtendedData.EncounterTables)
            {
                string FileNameFormatter(EncounterTable obj) => obj.Index.ToString("00000");

                string fileNameFormatter = FileNameFormatter(encounter);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string encounterLine = "";

                    foreach (var generator in encounter.Values)
                    {
                        weenieNames.TryGetValue(generator, out string label);
                        encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
                    }

                    if (encounterLine != "")
                    {
                        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`)" + Environment.NewLine
                            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
                        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(encounterLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarRegions.Value = (int)(((double)counter / regionDescExtendedData.EncounterTables.Count) * 100)));
                }
            }

            //string fileName = "EncounterMaps";

            //using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            //{
            //    foreach (var map in RegionDescExtendedData.EncounterMaps)
            //    {
            //        var parsed = map;

            //        string encounterLine = "";

            //        //        //foreach (var generator in encounter.Values)
            //        //        //{
            //        //        //    weenieNames.TryGetValue(generator, out string label);
            //        //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //        //}

            //        encounterLine += $"     , ({RegionDescExtendedData.EncounterMaps.IndexOf(map)}, {map.Index})" + $" /* {RegionDescExtendedData.EncounterMaps.IndexOf(map).ToString("X4")} */" + Environment.NewLine;

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `encounter_map` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        //        var counter = Interlocked.Increment(ref processedCounter);

            //        //        if ((counter % 1000) == 0)
            //        //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }
            //}

            //foreach (var map in RegionDescExtendedData.EncounterMaps)
            //{
            //    //string FileNameFormatter(EncounterMap obj) => obj.Index.ToString("00000");

            //    //string fileNameFormatter = FileNameFormatter(encounter);

            //    string fileNameFormatter = RegionDescExtendedData.EncounterMaps.IndexOf(map).ToString("X4");

            //    using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
            //    {
            //        var parsed = map;

            //        string encounterLine = "";

            //        //foreach (var generator in encounter.Values)
            //        //{
            //        //    weenieNames.TryGetValue(generator, out string label);
            //        //    encounterLine += $"     , ({encounter.Index}, {generator})" + $" /* {label} */" + Environment.NewLine;
            //        //}

            //        encounterLine += $"     , ({RegionDescExtendedData.EncounterMaps.IndexOf(map)}, {map.Index})" + Environment.NewLine;

            //        if (encounterLine != "")
            //        {
            //            encounterLine = $"{sqlCommand} INTO `encounter_map` (`landblock`, `index`)" + Environment.NewLine
            //                + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //            encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //            writer.WriteLine(encounterLine);
            //        }

            //        var counter = Interlocked.Increment(ref processedCounter);

            //        if ((counter % 1000) == 0)
            //            BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / RegionDescExtendedData.EncounterMaps.Count) * 100)));
            //    }
            //}

            //using (StreamWriter writer = new StreamWriter(outputFolder + "00000" + ".sql"))
            //{
            //    string encounterLine = "";

            //    encounterLine += $"     , (0, 0, '{Convert.ToBase64String(RegionDescExtendedData.EncounterMap)}')" + Environment.NewLine;

            //    if (encounterLine != "")
            //    {
            //        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //        writer.WriteLine(encounterLine);
            //    }
            //}

            //using (StreamWriter writer = new StreamWriter(outputFolder + "00001" + ".sql"))
            //{
            //    //string encounterLine = "";

            //    //encounterLine += $"     , (1, 0, '{Convert.ToBase64String(LandBlockData.TerrainData)}')" + Environment.NewLine;

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //string encounterLine = "";

            //    //encounterLine += $"     , (1, 0)" + Environment.NewLine;

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //var terrainDataStrings = WholeChunks(Convert.ToBase64String(LandBlockData.TerrainData), Convert.ToBase64String(RegionDescExtendedData.EncounterMap).Length);

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine = "";
            //    //    //UPDATE Table SET Field=CONCAT(IFNULL(Field, ''), 'Your extra HTML')
            //    //    //encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //    //    + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    //encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    encounterLine = $"UPDATE `encounter` SET `encounter_Map` = CONCAT(IFNULL(encounter_Map, ''), '{line}') WHERE `index` = 1;" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine += $"     , (1, 0, '{line}')" + Environment.NewLine;
            //    //}

            //    //if (encounterLine != "")
            //    //{
            //    //    encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //        + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //    encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //    writer.WriteLine(encounterLine);
            //    //}

            //    //foreach (string line in terrainDataStrings)
            //    //{
            //    //    encounterLine = "";
            //    //    encounterLine += $"     , (1, 0, '{line}')" + Environment.NewLine;
            //    //    if (encounterLine != "")
            //    //    {
            //    //        encounterLine = $"{sqlCommand} INTO `encounter` (`index`, `weenie_Class_Id`, `encounter_Map`)" + Environment.NewLine
            //    //            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
            //    //        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
            //    //        writer.WriteLine(encounterLine);
            //    //    }
            //    //}
            //}
        }

        static IEnumerable<string> WholeChunks(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }

        private void cmd1RegionsParse_Click(object sender, EventArgs e)
        {
            cmd1RegionsParse.Enabled = false;

            progressBarRegions.Style = ProgressBarStyle.Continuous;
            progressBarRegions.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                //WriteRegionFiles();

                WriteEnounterLandblockInstances();

                BeginInvoke((Action)(() =>
                {
                    progressBarRegions.Style = ProgressBarStyle.Continuous;
                    progressBarRegions.Value = 100;

                    cmd1RegionsParse.Enabled = true;
                }));
            });
        }

        private void WriteEnounterLandblockInstances()
        {
            var encounters = new Dictionary<int, List<Encounter>>();
            
            for (var landblock = 0; landblock < (255 * 255); landblock++)
            {
                var block_x = (landblock & 0xFF00) >> 8;
                var block_y = (landblock & 0x00FF) >> 0;

                //var tbIndex = ((block_x * 255) + block_y) * 9 * 9;
                //var tbIndex = ((block_x * 255) + block_y) * 9;
                var tbIndex = ((block_x * 255) + block_y);

                //if (tlIndex > LandBlockData.TerrainLandblocks.Count)
                //    continue;

                var terrain_base = landBlockData.TerrainLandblocks[tbIndex];

                for (var cell_x = 0; cell_x < 8; cell_x++)
                {
                    for (var cell_y = 0; cell_y < 8; cell_y++)
                    {
                        var terrain = terrain_base.Terrain[(cell_x * 9) + cell_y];

                        int encounterIndex = (terrain >> 7) & 0xF;

                        var encounterMap = regionDescExtendedData.EncounterMaps[(block_x * 255) + block_y];
                        var encounterTable = regionDescExtendedData.EncounterTables.FirstOrDefault(t => t.Index == encounterMap.Index);

                        if (encounterTable == null)
                            continue;

                        var wcid = encounterTable.Values[encounterIndex];

                        // System.Diagnostics.Debug.WriteLine($"landblock = {landblock:X4} | terrain = {terrain} | encounterIndex = {encounterIndex} | encounterTable = {encounterMap.Index} | wcid = {wcid}");

                        if (wcid > 0)
                        {
                            var objCellId = (landblock << 16) | 0;

                            if (!encounters.ContainsKey(landblock))
                                encounters.Add(landblock, new List<Encounter>());

                            encounters[landblock].Add(new Encounter { Landblock = landblock, WeenieClassId = wcid, CellX = cell_x, CellY = cell_y });
                        }
                    }
                }
            }

            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "1 RegionDescExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var landblock in encounters)
            {
                string fileNameFormatter = landblock.Key.ToString("X4");

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    var parsed = landblock.Value;

                    string encounterLine = "";

                    foreach (var encounter in parsed)
                    {
                        weenieNames.TryGetValue(encounter.WeenieClassId, out string label);
                        encounterLine += $"     , ({encounter.Landblock}, {encounter.WeenieClassId}, {encounter.CellX}, {encounter.CellY})" + $" /* {label} */" + Environment.NewLine;
                    }

                    if (encounterLine != "")
                    {
                        encounterLine = $"{sqlCommand} INTO `encounter` (`landblock`, `weenie_Class_Id`, `cell_X`, `cell_Y`)" + Environment.NewLine
                            + "VALUES " + encounterLine.TrimStart("     ,".ToCharArray());
                        encounterLine = encounterLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(encounterLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarRegions.Value = (int)(((double)counter / encounters.Count) * 100)));
                }
            }
        }

        private void cmd4CraftingParse_Click(object sender, EventArgs e)
        {
            cmd4CraftingParse.Enabled = false;

            progressBarCrafting.Style = ProgressBarStyle.Continuous;
            progressBarCrafting.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteRecipeFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarCrafting.Style = ProgressBarStyle.Continuous;
                    progressBarCrafting.Value = 100;

                    cmd4CraftingParse.Enabled = true;
                }));
            });
        }

        private void WriteRecipeFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "4 CraftTable" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

            foreach (var recipe in craftingTable.Recipes)
            {
                string FileNameFormatter(Recipe obj) => obj.ID.ToString("00000");

                string fileNameFormatter = FileNameFormatter(recipe);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    string recipeLine = "";

                    //`recipe_Id`, `unknown_1`, `skill`, `difficulty`, `unknown_4`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, `data_Id`
                    recipeLine += $"     , ({recipe.ID}, {recipe.unknown_1}, {recipe.Skill} /* {Enum.GetName(typeof(ACE.Entity.Enum.Skill), recipe.Skill)} */, {recipe.Difficulty}, {recipe.unknown_4}, {(recipe.SuccessWCID > 0 ? $"{recipe.SuccessWCID} /* {weenieNames[recipe.SuccessWCID]} */" : $"{recipe.SuccessWCID}")}, {recipe.SuccessAmount}, '{recipe.SuccessMessage.Replace("'", "''")}', {(recipe.FailWCID > 0 ? $"{recipe.FailWCID} /* {weenieNames[recipe.FailWCID]} */" : $"{recipe.FailWCID}")}, {recipe.FailAmount}, '{recipe.FailMessage.Replace("'", "''")}', {recipe.DataID})" + Environment.NewLine;

                    string cookbookLine = "";

                    uint sourceWCID = 0;

                    foreach (var entry in craftingTable.Precursors[recipe.ID])
                    {
                        cookbookLine += $"     , ({recipe.ID}, {(entry.Target > 0 ? $"{entry.Target} /* {weenieNames[entry.Target]} */" : $"{entry.Target}")}, {(entry.Source > 0 ? $"{entry.Source} /* {weenieNames[entry.Source]} */" : $"{entry.Source}")})" + Environment.NewLine;
                        if (entry.Source > 0)
                            sourceWCID = entry.Source;
                    }

                    string componentsLine = "";

                    //`recipe_Id`, `percent`, `unknown_2`, `message`
                    int compidx = 1;
                    foreach (var component in recipe.Components)
                    {
                        switch (compidx)
                        {
                            case 1:
                            case 3:
                                componentsLine += $"     , ({recipe.ID}, {component.unknown_1}, {component.unknown_2}, '{component.unknown_3.Replace("'", "''")}') /* Target */" + Environment.NewLine;
                                break;
                            case 2:
                            case 4:
                                componentsLine += $"     , ({recipe.ID}, {component.unknown_1}, {component.unknown_2}, '{component.unknown_3.Replace("'", "''")}') {(sourceWCID > 0 ? $"/* {weenieNames[sourceWCID]} */" : "/* Source */")}" + Environment.NewLine;
                                break;
                        }
                        compidx++;
                    }

                    string requirementsIntLine = "", requirementsDIDLine = "", requirementsIIDLine = "", requirementsFloatLine = "",requirementsStringLine = "", requirementsBoolLine = "";

                    //`recipe_Id`, `stat`, `value`, `enum`, `message`
                    if (recipe.Requirements != null)
                    {
                        foreach (var requirement in recipe.Requirements)
                        {
                            //`recipe_Id`, `stat`, `value`, `enum`, `message`
                            if (requirement.IntRequirements != null)
                            {
                                foreach (var req in requirement.IntRequirements)
                                {
                                    switch ((PropertyInt)req.Stat)
                                    {
                                        case PropertyInt.ImbuedEffect2:
                                        case PropertyInt.ImbuedEffect3:
                                        case PropertyInt.ImbuedEffect4:
                                        case PropertyInt.ImbuedEffect5:
                                        case PropertyInt.ImbuedEffect:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {((ImbuedEffectType)req.Value).ToString()} */, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                        case PropertyInt.AttackType:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {((AttackType)req.Value).ToString()} */, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                        case PropertyInt.Attuned:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(AttunedStatusEnum), req.Value)} */, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                        case PropertyInt.Bonded:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(BondedStatusEnum), req.Value)} */, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                        case PropertyInt.PaletteTemplate:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(PALETTE_TEMPLATE), req.Value)} */, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                        default:
                                            requirementsIntLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value}, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                            break;
                                    }
                                }
                            }

                            if (requirement.DIDRequirements != null)
                            {
                                foreach (var req in requirement.DIDRequirements)
                                {
                                    requirementsDIDLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyDataId), req.Stat)} */, {req.Value}, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                }
                            }

                            if (requirement.IIDRequirements != null)
                            {
                                foreach (var req in requirement.IIDRequirements)
                                {
                                    requirementsIIDLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInstanceId), req.Stat)} */, {req.Value}, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                }
                            }

                            if (requirement.StringRequirements != null)
                            {
                                foreach (var req in requirement.StringRequirements)
                                {
                                    requirementsStringLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyString), req.Stat)} */, '{req.Value.Replace("'", "''")}', {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                }
                            }

                            if (requirement.FloatRequirements != null)
                            {
                                foreach (var req in requirement.FloatRequirements)
                                {
                                    requirementsFloatLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyFloat), req.Stat)} */, {req.Value}, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                }
                            }

                            if (requirement.BoolRequirements != null)
                            {
                                foreach (var req in requirement.BoolRequirements)
                                {
                                    requirementsBoolLine += $"     , ({recipe.ID}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyBool), req.Stat)} */, {req.Value}, {req.Enum}, '{req.Message.Replace("'", "''")}')" + Environment.NewLine;
                                }
                            }
                        }
                    }

                    string modsLine = "";
                    string modsIntLine = "", modsDIDLine = "", modsIIDLine = "", modsFloatLine = "", modsStringLine = "", modsBoolLine = "";

                    if (recipe.Mods != null)
                    {
                        int modSet = 1;
                        foreach (var mod in recipe.Mods)
                        {
                            //`recipe_Id`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`
                            modsLine += $"     , ({recipe.ID}, {modSet}, {mod.Unknown1}, {mod.Unknown2}, {mod.Unknown3}, {mod.Unknown4}, {mod.Unknown5}, {mod.Unknown6}, {mod.Unknown7}, {mod.DataID}, {mod.Unknown9}, {mod.InstanceID})" + Environment.NewLine;

                            if (mod.IntMods != null)
                            {
                                //`recipe_Id`, `stat`, `value`, `enum`, `unknown_1`
                                foreach (var req in mod.IntMods)
                                {
                                    switch (req.Enum)
                                    {
                                        case 7:
                                            modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat} /* {Enum.GetName(typeof(SpellID), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                            break;
                                        default:
                                            switch ((PropertyInt)req.Stat)
                                            {
                                                case PropertyInt.ImbuedEffect2:
                                                case PropertyInt.ImbuedEffect3:
                                                case PropertyInt.ImbuedEffect4:
                                                case PropertyInt.ImbuedEffect5:
                                                case PropertyInt.ImbuedEffect:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {((ImbuedEffectType)req.Value).ToString()} */, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                                case PropertyInt.AttackType:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {((AttackType)req.Value).ToString()} */, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                                case PropertyInt.Attuned:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(AttunedStatusEnum), req.Value)} */, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                                case PropertyInt.Bonded:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(BondedStatusEnum), req.Value)} */, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                                case PropertyInt.PaletteTemplate:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value} /* {Enum.GetName(typeof(PALETTE_TEMPLATE), req.Value)} */, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                                default:
                                                    modsIntLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInt), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                                    break;
                                            }
                                            break;
                                    }
                                }                                
                            }

                            if (mod.DIDMods != null)
                            {
                                foreach (var req in mod.DIDMods)
                                {
                                    modsDIDLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyDataId), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                }
                            }

                            if (mod.IIDMods != null)
                            {
                                foreach (var req in mod.IIDMods)
                                {
                                    modsIIDLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyInstanceId), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                }
                            }

                            if (mod.StringMods != null)
                            {
                                foreach (var req in mod.StringMods)
                                {
                                    modsStringLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyString), req.Stat)} */, '{req.Value.Replace("'", "''")}', {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                }
                            }

                            if (mod.FloatMods != null)
                            {
                                foreach (var req in mod.FloatMods)
                                {
                                    modsFloatLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyFloat), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                }
                            }

                            if (mod.BoolMods != null)
                            {
                                foreach (var req in mod.BoolMods)
                                {
                                    modsBoolLine += $"     , ({recipe.ID}, {modSet}, {req.Stat:000} /* {Enum.GetName(typeof(PropertyBool), req.Stat)} */, {req.Value}, {req.Enum}, {req.Unknown1})" + Environment.NewLine;
                                }
                            }

                            modSet++;
                        }
                    }

                    if (recipeLine != "")
                    {
                        recipeLine = $"{sqlCommand} INTO `recipe` (`recipe_Id`, `unknown_1`, `skill`, `difficulty`, `salvage_Type`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, `data_Id`)" + Environment.NewLine
                            + "VALUES " + recipeLine.TrimStart("     ,".ToCharArray());
                        recipeLine = recipeLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(recipeLine);
                    }

                    if (cookbookLine != "")
                    {
                        cookbookLine = $"{sqlCommand} INTO `cook_book` (`recipe_Id`, `target_W_C_I_D`, `source_W_C_I_D`)" + Environment.NewLine
                            + "VALUES " + cookbookLine.TrimStart("     ,".ToCharArray());
                        cookbookLine = cookbookLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(cookbookLine);
                    }

                    if (componentsLine != "")
                    {
                        componentsLine = $"{sqlCommand} INTO `recipe_component` (`recipe_Id`, `destroy_Chance`, `destroy_Amount`, `destroy_Message`)" + Environment.NewLine
                            + "VALUES " + componentsLine.TrimStart("     ,".ToCharArray());
                        componentsLine = componentsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(componentsLine);
                    }

                    if (requirementsIntLine != "")
                    {
                        requirementsIntLine = $"{sqlCommand} INTO `recipe_requirements_int` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsIntLine.TrimStart("     ,".ToCharArray());
                        requirementsIntLine = requirementsIntLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsIntLine);
                    }

                    if (requirementsDIDLine != "")
                    {
                        requirementsDIDLine = $"{sqlCommand} INTO `recipe_requirements_d_i_d` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsDIDLine.TrimStart("     ,".ToCharArray());
                        requirementsDIDLine = requirementsDIDLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsDIDLine);
                    }

                    if (requirementsIIDLine != "")
                    {
                        requirementsIIDLine = $"{sqlCommand} INTO `recipe_requirements_i_i_d` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsIIDLine.TrimStart("     ,".ToCharArray());
                        requirementsIIDLine = requirementsIIDLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsIIDLine);
                    }

                    if (requirementsStringLine != "")
                    {
                        requirementsStringLine = $"{sqlCommand} INTO `recipe_requirements_string` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsStringLine.TrimStart("     ,".ToCharArray());
                        requirementsStringLine = requirementsStringLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsStringLine);
                    }

                    if (requirementsFloatLine != "")
                    {
                        requirementsFloatLine = $"{sqlCommand} INTO `recipe_requirements_float` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsFloatLine.TrimStart("     ,".ToCharArray());
                        requirementsFloatLine = requirementsFloatLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsFloatLine);
                    }

                    if (requirementsBoolLine != "")
                    {
                        requirementsBoolLine = $"{sqlCommand} INTO `recipe_requirements_bool` (`recipe_Id`, `stat`, `value`, `enum`, `message`)" + Environment.NewLine
                            + "VALUES " + requirementsBoolLine.TrimStart("     ,".ToCharArray());
                        requirementsBoolLine = requirementsBoolLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(requirementsBoolLine);
                    }

                    if (modsLine != "")
                    {
                        modsLine = $"{sqlCommand} INTO `recipe_mod` (`recipe_Id`, `mod_Set_Id`, `health`, `unknown_2`, `mana`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)" + Environment.NewLine
                            + "VALUES " + modsLine.TrimStart("     ,".ToCharArray());
                        modsLine = modsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsLine);
                    }

                    if (modsIntLine != "")
                    {
                        modsIntLine = $"{sqlCommand} INTO `recipe_mods_int` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsIntLine.TrimStart("     ,".ToCharArray());
                        modsIntLine = modsIntLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsIntLine);
                    }

                    if (modsDIDLine != "")
                    {
                        modsDIDLine = $"{sqlCommand} INTO `recipe_mods_d_i_d` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsDIDLine.TrimStart("     ,".ToCharArray());
                        modsDIDLine = modsDIDLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsDIDLine);
                    }

                    if (modsIIDLine != "")
                    {
                        modsIIDLine = $"{sqlCommand} INTO `recipe_mods_i_i_d` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsIIDLine.TrimStart("     ,".ToCharArray());
                        modsIIDLine = modsIIDLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsIIDLine);
                    }

                    if (modsStringLine != "")
                    {
                        modsStringLine = $"{sqlCommand} INTO `recipe_mods_string` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsStringLine.TrimStart("     ,".ToCharArray());
                        modsStringLine = modsStringLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsStringLine);
                    }

                    if (modsFloatLine != "")
                    {
                        modsFloatLine = $"{sqlCommand} INTO `recipe_mods_float` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsFloatLine.TrimStart("     ,".ToCharArray());
                        modsFloatLine = modsFloatLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsFloatLine);
                    }

                    if (modsBoolLine != "")
                    {
                        modsBoolLine = $"{sqlCommand} INTO `recipe_mods_bool` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)" + Environment.NewLine
                            + "VALUES " + modsBoolLine.TrimStart("     ,".ToCharArray());
                        modsBoolLine = modsBoolLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(modsBoolLine);
                    }


                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarCrafting.Value = (int)(((double)counter / craftingTable.Recipes.Count) * 100)));
                }
            }
        }

        private void cmd3TreasureParse_Click(object sender, EventArgs e)
        {
            cmd3TreasureParse.Enabled = false;

            progressBarTreasure.Style = ProgressBarStyle.Continuous;
            progressBarTreasure.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteTreasureFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBarTreasure.Style = ProgressBarStyle.Continuous;
                    progressBarTreasure.Value = 100;

                    cmd3TreasureParse.Enabled = true;
                }));
            });
        }

        private void WriteTreasureFiles()
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "3 TreasureTable" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCounter = 0;

            string sqlCommand = "INSERT";

           var subFolder = "\\Wielded\\";
            if (!Directory.Exists(outputFolder + subFolder))
                Directory.CreateDirectory(outputFolder + subFolder);

            foreach (var entry in treasureTable.WieldedTreasure)
            {
                //string FileNameFormatter(TreasureEntry obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                //string fileNameFormatter = FileNameFormatter(entry.Value);

                string fileNameFormatter = entry.Key.ToString("00000");

                using (StreamWriter writer = new StreamWriter(outputFolder + subFolder + fileNameFormatter + ".sql"))
                {
                    string entryLine = "";

                    foreach (var treasure in entry.Value)
                    {
                        //(`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)
                        entryLine += $"     , ({entry.Key}, {treasure.WCID} /* {weenieNames[treasure.WCID]} */, {treasure.PTID}, {treasure.m_08_AlwaysZero}, {treasure.Shade}, {treasure.Amount}, {treasure.m_f14}, {treasure.Chance}, {treasure.m_1C_AlwaysZero}, {treasure.m_20_AlwaysZero}, {treasure.m_24_AlwaysZero}, {treasure.m_b28}, {treasure.m_b2C}, {treasure.m_b30}, {treasure.m_34_AlwaysZero}, {treasure.m_38_AlwaysZero}, {treasure.m_3C_AlwaysZero}, {treasure.m_40_AlwaysZero})" + Environment.NewLine;
                    }

                    if (entryLine != "")
                    {
                        //(`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)
                        entryLine = $"{sqlCommand} INTO `treasure_wielded` (`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `unknown_2`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`)" + Environment.NewLine
                            + "VALUES " + entryLine.TrimStart("     ,".ToCharArray());
                        entryLine = entryLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(entryLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarTreasure.Value = (int)(((double)counter / treasureTable.WieldedTreasure.Count) * 100)));
                }
            }

            subFolder = "\\Death\\";
            if (!Directory.Exists(outputFolder + subFolder))
                Directory.CreateDirectory(outputFolder + subFolder);

            foreach (var entry in treasureTable.DeathTreasure)
            {
                //string FileNameFormatter(TreasureEntry obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                //string fileNameFormatter = FileNameFormatter(entry.Value);

                string fileNameFormatter = entry.Key.ToString("00000");

                using (StreamWriter writer = new StreamWriter(outputFolder + subFolder + fileNameFormatter + ".sql"))
                {
                    string entryLine = "";

                    //foreach (var treasure in entry.Value)
                    //{
                        //(`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`)
                        entryLine += $"     , ({entry.Key}, {entry.Value.Tier}, {entry.Value.m_f04}, {entry.Value.m_08}, {entry.Value.m_0C}, {entry.Value.m_10}, {entry.Value.m_14}, {entry.Value.m_18}, {entry.Value.m_1C}, {entry.Value.m_20}, {entry.Value.m_24}, {entry.Value.m_28}, {entry.Value.m_2C}, {entry.Value.m_30}, {entry.Value.m_34}, {entry.Value.m_38})" + Environment.NewLine;
                    //}

                    if (entryLine != "")
                    {
                        //(`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`)
                        entryLine = $"{sqlCommand} INTO `treasure_death` (`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`, `unknown_14`)" + Environment.NewLine
                            + "VALUES " + entryLine.TrimStart("     ,".ToCharArray());
                        entryLine = entryLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(entryLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBarTreasure.Value = (int)(((double)counter / treasureTable.DeathTreasure.Count) * 100)));
                }
            }
        }
    }
}
