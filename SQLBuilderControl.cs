using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

namespace PhatACCacheBinParser
{
	public partial class SQLBuilderControl : UserControl
	{
		public SQLBuilderControl()
		{
			InitializeComponent();
		}


		private readonly RegionDescExtendedData RegionDescExtendedData = new RegionDescExtendedData();
		private readonly SpellTableExtendedData SpellTableExtendedData = new SpellTableExtendedData();
		private readonly TreasureTable TreasureTable = new TreasureTable();
		private readonly CraftingTable CraftingTable = new CraftingTable();
		private readonly HousingPortalsTable HousingPortalsTable = new HousingPortalsTable();
		private readonly LandBlockData LandBlockData = new LandBlockData();
		// Segment 7
		private readonly QuestDefDB QuestDefDB = new QuestDefDB();
		private readonly WeenieDefaults WeenieDefaults = new WeenieDefaults();
		private readonly MutationFilters UnknownATables = new MutationFilters();
		// Segment B

        private Dictionary<uint, string> weenieNames = new Dictionary<uint, string>();

        private void cmdParseAll_Click(object sender, EventArgs e)
		{
			cmdParseAll.Enabled = false;

			progressParseSources.Style = ProgressBarStyle.Marquee;

            ThreadPool.QueueUserWorkItem(o =>
			{
				// Read all the inputs here
				TryParseSegment((string) Settings.Default["_1SourceBin"], RegionDescExtendedData);
                TryParseSegment((string) Settings.Default["_2SourceBin"], SpellTableExtendedData);
				TryParseSegment((string) Settings.Default["_3SourceBin"], TreasureTable);
				TryParseSegment((string) Settings.Default["_4SourceBin"], CraftingTable);
				TryParseSegment((string) Settings.Default["_5SourceBin"], HousingPortalsTable);
				TryParseSegment((string) Settings.Default["_6SourceBin"], LandBlockData);
				// Segment 7
				TryParseSegment((string) Settings.Default["_8SourceBin"], QuestDefDB);
				TryParseSegment((string) Settings.Default["_9SourceBin"], WeenieDefaults);
				TryParseSegment((string) Settings.Default["_ASourceBin"], UnknownATables);
                // Segment B

                BeginInvoke((Action)(() => CollectWeenieNames()));

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


		private void cmdAction1_Click(object sender, EventArgs e)
		{
			cmdAction1.Enabled = false;

			//progressParseSources.Style = ProgressBarStyle.Continuous;
			//progressParseSources.Value = 0;

			progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
			{
                // Do some output thing here

                // For example
                //foreach (var weenie in WeenieDefaults.Weenies)
                //	;

                WriteWeenieFiles();      
                                
                BeginInvoke((Action)(() =>
				{
					progressBar1.Style = ProgressBarStyle.Continuous;
					progressBar1.Value = 100;

					cmdAction1.Enabled = true;
				}));
			});
		}

        private void CollectWeenieNames()
        {
            weenieNames.Clear();
            foreach (var weenie in WeenieDefaults.Weenies)
            {
                var name = weenie.Value.Description;
                if (name == "")
                {
                    if (Enum.IsDefined(typeof(WeenieClasses), (ushort)weenie.Value.WCID))
                        name = Enum.GetName(typeof(WeenieClasses), weenie.Value.WCID).Substring(2);
                    else
                    {
                        name = "ace" + weenie.Value.WCID.ToString(); //+ "-" + parsed.Label.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();
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
            foreach (var weenie in WeenieDefaults.Weenies)
            {
                //var parsed = WeenieDefaults.Weenies[(uint)i]; //parsedObjects[i] as Weenies.Weenie;
                var parsed = weenie.Value;

                if (parsed.WCID > highestWeenieAllowed && highestWeenieAllowed > 0)
                    break;

                var wtFolder = outputFolder + Enum.GetName(typeof(WeenieType), parsed.WeenieType).Replace("_WeenieType", "") + "\\";
                if (!Directory.Exists(wtFolder))
                    Directory.CreateDirectory(wtFolder);

                var ctFolder = wtFolder;
                if (parsed.WeenieType == (uint)WeenieType.Creature_WeenieType)
                {
                    if (parsed.IntValues.ContainsKey((int)STypeInt.CREATURE_TYPE_INT))
                    {
                        Enum.TryParse(parsed.IntValues[(int)STypeInt.CREATURE_TYPE_INT].ToString(), out CreatureType ct);
                        if (Enum.IsDefined(typeof(CreatureType), ct))
                            ctFolder = wtFolder + Enum.GetName(typeof(CreatureType), parsed.IntValues[(int)STypeInt.CREATURE_TYPE_INT]).Replace("_CreatureType", "").Replace("_", "") + "\\";
                        else
                            ctFolder = wtFolder + "UnknownCT_" + parsed.IntValues[(int)STypeInt.CREATURE_TYPE_INT].ToString() + "\\";
                    }
                    else
                        ctFolder = wtFolder + "Unsorted" + "\\";
                    if (!Directory.Exists(ctFolder))
                        Directory.CreateDirectory(ctFolder);
                }

                var itFolder = ctFolder;
                if (parsed.WeenieType != (uint)WeenieType.Creature_WeenieType)
                {
                    if (parsed.IntValues.ContainsKey((int)STypeInt.ITEM_TYPE_INT))
                        itFolder = ctFolder + Enum.GetName(typeof(ITEM_TYPE), parsed.IntValues[(int)STypeInt.ITEM_TYPE_INT]).Replace("TYPE_", "").Replace("_", "") + "\\";
                    else
                        itFolder = ctFolder + Enum.GetName(typeof(ITEM_TYPE), ITEM_TYPE.TYPE_UNDEF).Replace("TYPE_", "").Replace("_", "") + "\\";
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
                //        if (parsed.IntValues[(int)STypeInt.PK_LEVEL_MODIFIER_INT] == 1)
                //            aceObjectDescriptionFlags = PublicWeenieDesc.BitfieldIndex.BF_PKSWITCH;
                //        else if (parsed.IntValues[(int)STypeInt.PK_LEVEL_MODIFIER_INT] == -1)
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
                    string weenieName = "";
                    if (Enum.IsDefined(typeof(WeenieClasses), (ushort)parsed.WCID))
                    {
                        weenieName = Enum.GetName(typeof(WeenieClasses), parsed.WCID).Substring(2);
                        weenieName = weenieName.Substring(0, weenieName.Length - 6).Replace("_", "-").ToLower();
                    }
                    else
                        weenieName = "ace" + parsed.WCID.ToString(); //+ "-" + parsed.Label.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();

                    string name = parsed.Description;
                    if (name == "")
                    {
                        if (Enum.IsDefined(typeof(WeenieClasses), (ushort)parsed.WCID))
                            name = Enum.GetName(typeof(WeenieClasses), parsed.WCID).Substring(2);
                        else
                            name = "ace" + parsed.WCID.ToString();

                        if (name.Contains("_CLASS"))
                            name = name.Remove(name.LastIndexOf("_CLASS")).Replace("_", "-").ToLower();
                    }

                    string line = $"/* Weenie - {name} ({parsed.WCID}) */" + Environment.NewLine;

                    line += $"DELETE FROM weenie WHERE class_Id = {parsed.WCID};" + Environment.NewLine + Environment.NewLine;
                    line += $"{sqlCommand} INTO weenie (`class_Id`, `class_Name`, `type`)" + Environment.NewLine +
                           $"VALUES ({parsed.WCID}, '{weenieName}', /* {Enum.GetName(typeof(WeenieType), parsed.WeenieType)} */ {parsed.WeenieType});" + Environment.NewLine;
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
                        foreach (var stat in parsed.IntValues)
                        {
                            intsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeInt), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.LongValues != null)
                    {
                        foreach (var stat in parsed.LongValues)
                        {
                            bigintsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeInt64), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.DoubleValues != null)
                    {
                        foreach (var stat in parsed.DoubleValues)
                        {
                            floatsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeFloat), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.BoolValues != null)
                    {
                        foreach (var stat in parsed.BoolValues)
                        {
                            boolsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeBool), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.StringValues != null)
                    {
                        foreach (var stat in parsed.StringValues)
                        {
                            if (stat.Key != (uint)STypeString.NAME_STRING)
                                strsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, '{stat.Value.Replace("'", "''")}') /* {Enum.GetName(typeof(STypeString), stat.Key)} */" + Environment.NewLine;
                            else
                            {
                                if (stat.Value != "")
                                    strsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, '{stat.Value.Replace("'", "''")}') /* {Enum.GetName(typeof(STypeString), stat.Key)} */" + Environment.NewLine;
                                else
                                    strsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, '{name.Replace("'", "''")}') /* {Enum.GetName(typeof(STypeString), stat.Key)} */" + Environment.NewLine;
                            }
                        }
                    }
                    if (parsed.DIDValues != null)
                    {
                        foreach (var stat in parsed.DIDValues)
                        {
                            didsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeDID), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.IIDValues != null)
                    {
                        foreach (var stat in parsed.IIDValues)
                        {
                            // wcid 30732 has -1 for an IID.. i think this was to make it so noone could wield
                            // iidsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {(uint)stat.Value}) /* {Enum.GetName(typeof(STypeIID), stat.Key)} */" + Environment.NewLine;
                            iidsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value}) /* {Enum.GetName(typeof(STypeIID), stat.Key)} */" + Environment.NewLine;
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
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.STRENGTH_ATTRIBUTE}, {(uint)parsed.Attributes.Strength.InitLevel}, {parsed.Attributes.Strength.LevelFromCP}, {parsed.Attributes.Strength.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.STRENGTH_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.ENDURANCE_ATTRIBUTE}, {(uint)parsed.Attributes.Endurance.InitLevel}, {parsed.Attributes.Endurance.LevelFromCP}, {parsed.Attributes.Endurance.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.ENDURANCE_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.COORDINATION_ATTRIBUTE}, {(uint)parsed.Attributes.Coordination.InitLevel}, {parsed.Attributes.Coordination.LevelFromCP}, {parsed.Attributes.Coordination.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.COORDINATION_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.QUICKNESS_ATTRIBUTE}, {(uint)parsed.Attributes.Quickness.InitLevel}, {parsed.Attributes.Quickness.LevelFromCP}, {parsed.Attributes.Quickness.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.QUICKNESS_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.FOCUS_ATTRIBUTE}, {(uint)parsed.Attributes.Focus.InitLevel}, {parsed.Attributes.Focus.LevelFromCP}, {parsed.Attributes.Focus.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.FOCUS_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.SELF_ATTRIBUTE}, {(uint)parsed.Attributes.Self.InitLevel}, {parsed.Attributes.Self.LevelFromCP}, {parsed.Attributes.Self.CPSpent}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.SELF_ATTRIBUTE)} */" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.HEALTH_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._health})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.STAMINA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._stamina})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.MANA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._mana})" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_HEALTH_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Health.InitLevel}, {parsed.Attributes.Health.LevelFromCP}, {parsed.Attributes.Health.CPSpent}, {parsed.Attributes.Health.Current}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_HEALTH_ATTRIBUTE_2ND)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_STAMINA_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Stamina.InitLevel}, {parsed.Attributes.Stamina.LevelFromCP}, {parsed.Attributes.Stamina.CPSpent}, {parsed.Attributes.Stamina.Current}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_STAMINA_ATTRIBUTE_2ND)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_MANA_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Mana.InitLevel}, {parsed.Attributes.Mana.LevelFromCP}, {parsed.Attributes.Mana.CPSpent}, {parsed.Attributes.Mana.Current}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_MANA_ATTRIBUTE_2ND)} */" + Environment.NewLine;
                    }
                    if (parsed.PositionValues != null)
                    {
                        foreach (var stat in parsed.PositionValues)
                        {
                            positionsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {stat.Value.ObjCellID}, {stat.Value.Origin.X}, {stat.Value.Origin.Y}, {stat.Value.Origin.Z}, {stat.Value.Angles.W}, {stat.Value.Angles.X}, {stat.Value.Angles.Y}, {stat.Value.Angles.Z}) /* {Enum.GetName(typeof(STypePosition), stat.Key)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.PagesData != null)
                    {
                        //if (parsed.PagesData.MaxNumPages > 0 && parsed.PagesData.Pages != null)
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.APPRAISAL_PAGES_INT}, {(int)parsed.PagesData.Pages.Count}) /* {Enum.GetName(typeof(STypeInt), STypeInt.APPRAISAL_PAGES_INT)} */" + Environment.NewLine;
                        //if (parsed.PagesData.MaxNumPages > 0)
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.APPRAISAL_MAX_PAGES_INT}, {(int)parsed.PagesData.MaxNumPages}) /* {Enum.GetName(typeof(STypeInt), STypeInt.APPRAISAL_MAX_PAGES_INT)} */" + Environment.NewLine;
                        //if (parsed.PagesData.MaxNumCharsPerPage > 0) // pretty sure this is wrong
                        //    intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.AVAILABLE_CHARACTER_INT}, {(int)parsed.PagesData.MaxNumCharsPerPage}) /* {Enum.GetName(typeof(STypeInt), STypeInt.AVAILABLE_CHARACTER_INT)} */" + Environment.NewLine;
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
                            instancesLine += $"     , ({parsed.WCID}, {(uint)instance.Destination}, {instance.WCID}, {instance.StackSize}, {instance.Palette}, {instance.Shade}, {instance.TryToBond}" +
                                $") /* Create {label} for {Enum.GetName(typeof(DestinationType), instance.Destination)} */" + Environment.NewLine;
                        }
                    }
                    if (parsed.Generators != null)
                    {
                        foreach (var profile in parsed.Generators)
                        {
                            weenieNames.TryGetValue(Convert.ToUInt32(profile.Type), out string label);
                            profilesLine += $"     , ({parsed.WCID}, {profile.Probability}, {profile.Type}, {profile.Delay}, {(uint)profile.InitCreate}, {(uint)profile.MaxNum}" +
                            $", {profile.WhenCreate}, {profile.WhereCreate}, {profile.StackSize}, {profile.PalleteTypeID}, {profile.Shade}" +
                            $", {profile.Position.ObjCellID}, {profile.Position.Origin.X}, {profile.Position.Origin.Y}, {profile.Position.Origin.Z}" +
                            $", {profile.Position.Angles.W}, {profile.Position.Angles.X}, {profile.Position.Angles.Y}, {profile.Position.Angles.Z})" +
                            $"/* Generate {label} (x{profile.InitCreate.ToString("N0")} up to max of {profile.MaxNum.ToString("N0")}) - {Enum.GetName(typeof(RegenerationType), profile.WhenCreate)} - {Enum.GetName(typeof(RegenLocationType), profile.WhereCreate)} */" + Environment.NewLine;
                        }
                    }

                    if (parsed.BodyParts != null)
                    {
                        foreach (var bodypart in parsed.BodyParts)
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
                        foreach (var skill in parsed.Skills)
                        {
                            skillsLine += $"     , ({parsed.WCID}, {skill.Key}, {skill.Value.LevelFromPP}, {skill.Value.Sac}, {skill.Value.PP}, {skill.Value.InitLevel}, {skill.Value.ResistanceAtLastCheck}, {skill.Value.LastUsedTime}) " + $"/* {Enum.GetName(typeof(STypeSkill), skill.Key)} */" + Environment.NewLine;
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
                                    quest = category.Quest.ToString().Replace("'", "''");
                                    quest = quest.Insert(0, "'").Insert(quest.Length + 1, "'");
                                }
                                emoteCategorysLine += $"     , ({parsed.WCID}, {category.Probability}, {category.Category}, {emoteSetId}, {((category.ClassID.HasValue) ? category.ClassID.ToString() : "NULL")}, {((category.Style.HasValue) ? category.Style.ToString() : "NULL")}, {((category.Substyle.HasValue) ? category.Substyle.ToString() : "NULL")}, {((category.Quest != null) ? quest : "NULL")}, {((category.VendorType.HasValue) ? category.VendorType.ToString() : "NULL")}, {((category.MinHealth.HasValue) ? category.MinHealth.ToString() : "NULL")}, {((category.MaxHealth.HasValue) ? category.MaxHealth.ToString() : "NULL")}) " + $"/* {Enum.GetName(typeof(EmoteCategory), category.Category)} */" + Environment.NewLine;
                                if (category.EmoteActions != null)
                                {
                                    var order = 0;
                                    foreach (var action in category.EmoteActions)
                                    {
                                        //(`object_Id`, `emote_Category`, `type`, `order`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
                                        string message = "";
                                        if (action.Message != null)
                                        {
                                            message = action.Message.ToString().Replace("'", "''");
                                            message = message.Insert(0, "'").Insert(message.Length + 1, "'");
                                        }
                                        string testString = "";
                                        if (action.TestString != null)
                                        {
                                            testString = action.TestString.ToString().Replace("'", "''");
                                            testString = testString.Insert(0, "'").Insert(testString.Length + 1, "'");
                                        }
                                        if (action.Position != null)
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category}, {emoteSetId}, {order}, {action.Type}, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? action.Motion.ToString() : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? action.SpellID.ToString() : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? action.PScript.ToString() : "NULL")}, {((action.Sound.HasValue) ? action.Sound.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID.ToString() : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString() : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.X.ToString() : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.Y.ToString() : "NULL")}, {((action.Position.Origin != null) ? action.Position.Origin.Z.ToString() : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.W.ToString() : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.X.ToString() : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.Y.ToString() : "NULL")}, {((action.Position.Angles != null) ? action.Position.Angles.Z.ToString() : "NULL")}) " + $"/* {Enum.GetName(typeof(EmoteType), action.Type)} */" + Environment.NewLine;
                                        else if (action.Frame != null)
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category}, {emoteSetId}, {order}, {action.Type}, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? action.Motion.ToString() : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? action.SpellID.ToString() : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? action.PScript.ToString() : "NULL")}, {((action.Sound.HasValue) ? action.Sound.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID.ToString() : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString() : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.X.ToString() : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.Y.ToString() : "NULL")}, {((action.Frame.Origin != null) ? action.Frame.Origin.Z.ToString() : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.W.ToString() : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.X.ToString() : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.Y.ToString() : "NULL")}, {((action.Frame.Angles != null) ? action.Frame.Angles.Z.ToString() : "NULL")}) " + $"/* {Enum.GetName(typeof(EmoteType), action.Type)} */" + Environment.NewLine;
                                        else
                                            emoteActionsLine += $"     , ({parsed.WCID}, {category.Category}, {emoteSetId}, {order}, {action.Type}, {action.Delay}, {action.Extent}, {((action.Motion.HasValue) ? action.Motion.ToString() : "NULL")}, {((action.Message != null) ? message : "NULL")}, {((action.TestString != null) ? testString : "NULL")}, {((action.Min.HasValue) ? action.Min.ToString() : "NULL")}, {((action.Max.HasValue) ? action.Max.ToString() : "NULL")}, {((action.Min64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.Max64.HasValue) ? action.Min64.ToString() : "NULL")}, {((action.MinDbl.HasValue) ? action.MinDbl.ToString() : "NULL")}, {((action.MaxDbl.HasValue) ? action.MaxDbl.ToString() : "NULL")}, {((action.Stat.HasValue) ? action.Stat.ToString() : "NULL")}, {((action.Display.HasValue) ? action.Display.ToString() : "NULL")}, {((action.Amount.HasValue) ? action.Amount.ToString() : "NULL")}, {((action.Amount64.HasValue) ? action.Amount64.ToString() : "NULL")}, {((action.HeroXP64.HasValue) ? action.HeroXP64.ToString() : "NULL")}, {((action.Percent.HasValue) ? action.Percent.ToString() : "NULL")}, {((action.SpellID.HasValue) ? action.SpellID.ToString() : "NULL")}, {((action.WealthRating.HasValue) ? action.WealthRating.ToString() : "NULL")}, {((action.TreasureClass.HasValue) ? action.TreasureClass.ToString() : "NULL")}, {((action.TreasureType.HasValue) ? action.TreasureType.ToString() : "NULL")}, {((action.PScript.HasValue) ? action.PScript.ToString() : "NULL")}, {((action.Sound.HasValue) ? action.Sound.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Destination.ToString() : "NULL")}, {((action.Item != null) ? action.Item.WCID.ToString() : "NULL")}, {((action.Item != null) ? action.Item.StackSize.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Palette.ToString() : "NULL")}, {((action.Item != null) ? action.Item.Shade.ToString() : "NULL")}, {((action.Item != null) ? action.Item.TryToBond.ToString() : "NULL")}, {((action.Position != null) ? action.Position.ObjCellID.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Origin.X.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Origin.Y.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Origin.Z.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Angles.W.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Angles.X.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Angles.Y.ToString() : "NULL")}, {((action.Position != null) ? action.Position.Angles.Z.ToString() : "NULL")}) " + $"/* {Enum.GetName(typeof(EmoteType), action.Type)} */" + Environment.NewLine;
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
                        BeginInvoke((Action)(() => progressBar1.Value = (int)(((double)counter / WeenieDefaults.Weenies.Count) * 100)));
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
            foreach (var landblock in LandBlockData.Landblocks)
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
                        BeginInvoke((Action)(() => progressBar2.Value = (int)(((double)counter / LandBlockData.Landblocks.Count) * 100)));
                }
                //});
            }

            //parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
            //System.Diagnostics.Debug.WriteLine($"Highest Weenie Exported in WorldSpawn was: {highestWeenieFound}");
        }

        private void cmdAction2_Click(object sender, EventArgs e)
        {
            cmdAction2.Enabled = false;

            //progressParseSources.Style = ProgressBarStyle.Continuous;
            //progressParseSources.Value = 0;

            progressBar2.Style = ProgressBarStyle.Continuous;
            progressBar2.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                // For example
                //foreach (var weenie in WeenieDefaults.Weenies)
                //	;

                WriteLandblockFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBar2.Style = ProgressBarStyle.Continuous;
                    progressBar2.Value = 100;

                    cmdAction2.Enabled = true;
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

            foreach (var spell in SpellTableExtendedData.Spells)
            {
                string FileNameFormatter(Spell obj) => obj.ID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(spell);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    var parsed = spell;

                    string spellLineHdr = "";
                    string spellLine = "";
                    //`spell_Id`, `name`, `description`, `school`, `icon_Id`, `category`, `bitfield`, `mana`, `range_Constant`, `range_Mod`, `power`, `economy_Mod`, `formula_Version`, `component_Loss`, `meta_Spell_Type`, `meta_Spell_Id`, `spell_Formula_Comp_1_Component_Id`, `spell_Formula_Comp_2_Component_Id`, `spell_Formula_Comp_3_Component_Id`, `spell_Formula_Comp_4_Component_Id`, `spell_Formula_Comp_5_Component_Id`, `spell_Formula_Comp_6_Component_Id`, `spell_Formula_Comp_7_Component_Id`, `spell_Formula_Comp_8_Component_Id`, `caster_Effect`, `target_Effect`, `fizzle_Effect`, `recovery_Interval`, `recovery_Amount`, `display_Order`, `non_Component_Target_Type`, `mana_Mod`

                    spellLineHdr = $"{sqlCommand} INTO `spell` (`spell_Id`, `name`, `description`, `school`, `icon_Id`, `category`, `bitfield`, `mana`, `range_Constant`, `range_Mod`, `power`, `economy_Mod`, `formula_Version`, `component_Loss`, `meta_Spell_Type`, `meta_Spell_Id`, `spell_Formula_Comp_1_Component_Id`, `spell_Formula_Comp_2_Component_Id`, `spell_Formula_Comp_3_Component_Id`, `spell_Formula_Comp_4_Component_Id`, `spell_Formula_Comp_5_Component_Id`, `spell_Formula_Comp_6_Component_Id`, `spell_Formula_Comp_7_Component_Id`, `spell_Formula_Comp_8_Component_Id`, `caster_Effect`, `target_Effect`, `fizzle_Effect`, `recovery_Interval`, `recovery_Amount`, `display_Order`, `non_Component_Target_Type`, `mana_Mod`";
                    spellLine = $"({spell.ID}, '{spell.Name.Replace("'", "''")}', '{spell.Description.Replace("'", "''")}', {(int)spell.School} /* {Enum.GetName(typeof(School), spell.School)} */, {spell.IconID}, {spell.Category} , {spell.Bitfield}, {spell.Mana}, {spell.RangeConstant}, {spell.RangeMod}, {spell.Power}, {spell.EconomyMod}, {spell.FormulaVersion}, {spell.ComponentLoss}, {(int)spell.MetaSpellType} /* {Enum.GetName(typeof(Enums.SpellType), spell.MetaSpellType)} */, {spell.MetaSpellId}, {spell.SpellFormula.Comps[0]}, {spell.SpellFormula.Comps[1]}, {spell.SpellFormula.Comps[2]}, {spell.SpellFormula.Comps[3]}, {spell.SpellFormula.Comps[4]}, {spell.SpellFormula.Comps[5]}, {spell.SpellFormula.Comps[6]}, {spell.SpellFormula.Comps[7]}, {spell.CasterEffect}, {spell.TargetEffect}, {spell.FizzleEffect}, {spell.RecoveryInterval}, {spell.RecoveryAmount}, {spell.DisplayOrder}, {spell.NonComponentTargetType}, {spell.ManaMod}";

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
                        spellLine += $", {spell.SpellStatMod.Type} /* {Enum.GetName(typeof(StatType), spell.SpellStatMod.Type)} */, {spell.SpellStatMod.Key}, {spell.SpellStatMod.Val}";
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
                        spellLine += $", {(int)spell.DamageType} /* {Enum.GetName(typeof(DAMAGE_TYPE), spell.DamageType)} */";
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
                        spellLine += $", {(int)spell.Source} /* {Enum.GetName(typeof(STypeAttribute2nd), spell.Source)} */";
                    }

                    if (spell.Destination.HasValue)
                    {
                        spellLineHdr += ", `destination`";
                        spellLine += $", {(int)spell.Destination} /* {Enum.GetName(typeof(STypeAttribute2nd), spell.Destination)} */";
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

                    spellLineHdr += $")" + Environment.NewLine + "VALUES ";
                    spellLine += $");";

                    if (spellLine != "")
                    {
                        writer.WriteLine(spellLineHdr + spellLine);
                    }
                    
                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBar3.Value = (int)(((double)counter / SpellTableExtendedData.Spells.Count) * 100)));
                }
            }
        }

        private void cmdAction3_Click(object sender, EventArgs e)
        {
            cmdAction3.Enabled = false;

            progressBar3.Style = ProgressBarStyle.Continuous;
            progressBar3.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteSpellFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBar3.Style = ProgressBarStyle.Continuous;
                    progressBar3.Value = 100;

                    cmdAction3.Enabled = true;
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

            foreach (var quest in QuestDefDB.QuestDefs)
            {
                string FileNameFormatter(QuestDef obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(quest);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    var parsed = quest;

                    string questLineHdr = "";
                    string questLine = "";
                    // `name`, `min_Delta`, `max_Solves`, `message`

                    questLineHdr = $"{sqlCommand} INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`";
                    questLine = $"('{quest.Name.Replace("'", "''")}', {quest.MinDelta}, {quest.MaxSolves}, '{quest.Message.Replace("'", "''")}'";

                    questLineHdr += $")" + Environment.NewLine + "VALUES ";
                    questLine += $");";

                    if (questLine != "")
                    {
                        writer.WriteLine(questLineHdr + questLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBar4.Value = (int)(((double)counter / QuestDefDB.QuestDefs.Count) * 100)));
                }
            }
        }

        private void cmdAction4_Click(object sender, EventArgs e)
        {
            cmdAction4.Enabled = false;

            progressBar4.Style = ProgressBarStyle.Continuous;
            progressBar4.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteQuestFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBar4.Style = ProgressBarStyle.Continuous;
                    progressBar4.Value = 100;

                    cmdAction4.Enabled = true;
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

            foreach (var house in HousingPortalsTable.HousingPortals)
            {
                //string FileNameFormatter(QuestDef obj) => obj.Name.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Name, "_");
                //string FileNameFormatter(HousingPortal obj) => Util.IllegalInFileName.Replace(obj.Name, "_");
                //string FileNameFormatter(int obj) => obj.ToString("00000") + " " + "HousingPortal";
                string FileNameFormatter(HousingPortal obj) => obj.HouseId.ToString("00000"); //+ " " + Util.IllegalInFileName.Replace(obj.Name, "_");

                string fileNameFormatter = FileNameFormatter(house);

                using (StreamWriter writer = new StreamWriter(outputFolder + fileNameFormatter + ".sql"))
                {
                    var parsed = house;

                    // string houseLineHdr = "";
                    string houseLine = "";
                    // `house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`

                    //houseLineHdr = $"{sqlCommand} INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`";
                    //houseLine = $"('{quest.Name.Replace("'", "''")}', {quest.MinDelta}, {quest.MaxSolves}, '{quest.Message.Replace("'", "''")}'";

                    foreach (var destination in house.Destinations)
                    {
                        //houseLine = $"({house.HouseId}, {destination.ObjCellID}, {destination.Origin.X}, {destination.Origin.Y}, {destination.Origin.Z}, {destination.Angles.W}, {destination.Angles.X}, {destination.Angles.Y}, {destination.Angles.Z}";
                        houseLine += $"     , ({house.HouseId}, {destination.ObjCellID}, {destination.Origin.X}, {destination.Origin.Y}, {destination.Origin.Z}, {destination.Angles.W}, {destination.Angles.X}, {destination.Angles.Y}, {destination.Angles.Z})" + Environment.NewLine;
                    }

                    //houseLineHdr += $")" + Environment.NewLine + "VALUES ";
                    //houseLine += $");";

                    //if (houseLine != "")
                    //{
                    //    writer.WriteLine(houseLineHdr + houseLine);
                    //}

                    if (houseLine != "")
                    {
                        houseLine = $"{sqlCommand} INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)" + Environment.NewLine
                            + "VALUES " + houseLine.TrimStart("     ,".ToCharArray());
                        houseLine = houseLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(houseLine);
                    }

                    var counter = Interlocked.Increment(ref processedCounter);

                    if ((counter % 1000) == 0)
                        BeginInvoke((Action)(() => progressBar5.Value = (int)(((double)counter / HousingPortalsTable.HousingPortals.Count) * 100)));
                }
            }
        }

        private void cmdAction8_Click(object sender, EventArgs e)
        {
            cmdAction8.Enabled = false;

            progressBar8.Style = ProgressBarStyle.Continuous;
            progressBar8.Value = 0;

            ThreadPool.QueueUserWorkItem(o =>
            {
                // Do some output thing here

                WriteHouseFiles();

                BeginInvoke((Action)(() =>
                {
                    progressBar8.Style = ProgressBarStyle.Continuous;
                    progressBar8.Value = 100;

                    cmdAction8.Enabled = true;
                }));
            });
        }
    }
}
