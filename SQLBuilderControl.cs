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

                var aceObjectDescriptionFlags = 0;
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

                    line += $"DELETE FROM ace_weenie_class WHERE weenieClassId = {parsed.WCID};" + Environment.NewLine + Environment.NewLine;
                    line += $"{sqlCommand} INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)" + Environment.NewLine +
                           $"VALUES ({parsed.WCID}, '{weenieName}');" + Environment.NewLine;
                    writer.WriteLine(line);
                    string intsLine = "", bigintsLine = "", floatsLine = "", boolsLine = "", strsLine = "", didsLine = "", iidsLine = "";
                    string skillsLine = "", attributesLine = "", attribute2ndsLine = "", bodyDamageValuesLine = "", bodyDamageVariancesLine = "", bodyArmorValuesLine = "", numsLine = "";
                    string spellsLine = "", positionsLine = "", pagesLine = "", instancesLine = "", profilesLine = "";
                    line = $"{sqlCommand} INTO `ace_object` (`" +
                        "aceObjectId`, `aceObjectDescriptionFlags`, " +
                        "`weenieClassId`" +
                        ")" + Environment.NewLine + "VALUES (" +
                    $"{parsed.WCID}, {(uint)aceObjectDescriptionFlags}, " +
                    $"{parsed.WCID}"; //+
                    line += ");" + Environment.NewLine;
                    writer.WriteLine(line);
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
                            iidsLine += $"     , ({parsed.WCID}, {(uint)stat.Key}, {(uint)stat.Value}) /* {Enum.GetName(typeof(STypeIID), stat.Key)} */" + Environment.NewLine;
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
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.STRENGTH_ATTRIBUTE}, {(uint)parsed.Attributes.Strength.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.STRENGTH_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.ENDURANCE_ATTRIBUTE}, {(uint)parsed.Attributes.Endurance.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.ENDURANCE_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.COORDINATION_ATTRIBUTE}, {(uint)parsed.Attributes.Coordination.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.COORDINATION_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.QUICKNESS_ATTRIBUTE}, {(uint)parsed.Attributes.Quickness.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.QUICKNESS_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.FOCUS_ATTRIBUTE}, {(uint)parsed.Attributes.Focus.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.FOCUS_ATTRIBUTE)} */" + Environment.NewLine;
                        attributesLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute.SELF_ATTRIBUTE}, {(uint)parsed.Attributes.Self.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute), STypeAttribute.SELF_ATTRIBUTE)} */" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.HEALTH_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._health})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.STAMINA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._stamina})" + Environment.NewLine;
                        ////attribute2ndsLine += $"     , ({parsed.i_objid}, {(uint)STypeAttribute2nd.MANA_ATTRIBUTE_2ND}, {(uint)parsed.i_prof._creatureProfileTable._mana})" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_HEALTH_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Health.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_HEALTH_ATTRIBUTE_2ND)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_STAMINA_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Stamina.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_STAMINA_ATTRIBUTE_2ND)} */" + Environment.NewLine;
                        attribute2ndsLine += $"     , ({parsed.WCID}, {(uint)STypeAttribute2nd.MAX_MANA_ATTRIBUTE_2ND}, {(uint)parsed.Attributes.Mana.InitLevel}) /* {Enum.GetName(typeof(STypeAttribute2nd), STypeAttribute2nd.MAX_MANA_ATTRIBUTE_2ND)} */" + Environment.NewLine;
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
                        if (parsed.PagesData.MaxNumPages > 0 && parsed.PagesData.Pages != null)
                            intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.APPRAISAL_PAGES_INT}, {(int)parsed.PagesData.Pages.Count}) /* {Enum.GetName(typeof(STypeInt), STypeInt.APPRAISAL_PAGES_INT)} */" + Environment.NewLine;
                        if (parsed.PagesData.MaxNumPages > 0)
                            intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.APPRAISAL_MAX_PAGES_INT}, {(int)parsed.PagesData.MaxNumPages}) /* {Enum.GetName(typeof(STypeInt), STypeInt.APPRAISAL_MAX_PAGES_INT)} */" + Environment.NewLine;
                        if (parsed.PagesData.MaxNumCharsPerPage > 0) // pretty sure this is wrong
                            intsLine += $"     , ({parsed.WCID}, {(uint)STypeInt.AVAILABLE_CHARACTER_INT}, {(int)parsed.PagesData.MaxNumCharsPerPage}) /* {Enum.GetName(typeof(STypeInt), STypeInt.AVAILABLE_CHARACTER_INT)} */" + Environment.NewLine;
                        if (parsed.PagesData.Pages != null)
                        {
                            foreach (var page in parsed.PagesData.Pages)
                            {
                                pagesLine += $"     , ({parsed.WCID}, {parsed.PagesData.Pages.IndexOf(page)}, '{page.AuthorName.Replace("'", "''")}', '{page.AuthorAccount.Replace("'", "''")}', {page.AuthorID}, {page.IgnoreAuthor}, '{page.Text.Replace("'", "''")}')" + Environment.NewLine;
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
                    intsLine += $"     , ({parsed.WCID}, {(uint)9007}, {parsed.WeenieType}) /* {Enum.GetName(typeof(WeenieType), parsed.WeenieType)} */" + Environment.NewLine;
                    if (strsLine != "")
                    {
                        strsLine = $"{sqlCommand} INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + strsLine.TrimStart("     ,".ToCharArray());
                        strsLine = strsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(strsLine);
                    }
                    if (didsLine != "")
                    {
                        didsLine = $"{sqlCommand} INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + didsLine.TrimStart("     ,".ToCharArray());
                        didsLine = didsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(didsLine);
                    }
                    if (iidsLine != "")
                    {
                        iidsLine = $"{sqlCommand} INTO `ace_object_properties_iid` (`aceObjectId`, `iidPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + iidsLine.TrimStart("     ,".ToCharArray());
                        iidsLine = iidsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(iidsLine);
                    }
                    if (intsLine != "")
                    {
                        intsLine = $"{sqlCommand} INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + intsLine.TrimStart("     ,".ToCharArray());
                        intsLine = intsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(intsLine);
                    }
                    if (bigintsLine != "")
                    {
                        bigintsLine = $"{sqlCommand} INTO `ace_object_properties_bigint` (`aceObjectId`, `bigIntPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + bigintsLine.TrimStart("     ,".ToCharArray());
                        bigintsLine = bigintsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(bigintsLine);
                    }
                    if (floatsLine != "")
                    {
                        floatsLine = $"{sqlCommand} INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + floatsLine.TrimStart("     ,".ToCharArray());
                        floatsLine = floatsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(floatsLine);
                    }
                    if (boolsLine != "")
                    {
                        boolsLine = $"{sqlCommand} INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)" + Environment.NewLine
                            + "VALUES " + boolsLine.TrimStart("     ,".ToCharArray());
                        boolsLine = boolsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(boolsLine);
                    }
                    if (spellsLine != "")
                    {
                        spellsLine = $"{sqlCommand} INTO `ace_object_properties_spell` (`aceObjectId`, `spellId`, `probability`)" + Environment.NewLine
                            + "VALUES " + spellsLine.TrimStart("     ,".ToCharArray());
                        spellsLine = spellsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(spellsLine);
                    }
                    if (attributesLine != "")
                    {
                        attributesLine = $"{sqlCommand} INTO `ace_object_properties_attribute` (`aceObjectId`, `attributeId`, `attributeBase`)" + Environment.NewLine
                            + "VALUES " + attributesLine.TrimStart("     ,".ToCharArray());
                        attributesLine = attributesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(attributesLine);
                    }
                    if (attribute2ndsLine != "")
                    {
                        attribute2ndsLine = $"{sqlCommand} INTO `ace_object_properties_attribute2nd` (`aceObjectId`, `attribute2ndId`, `attribute2ndValue`)" + Environment.NewLine
                            + "VALUES " + attribute2ndsLine.TrimStart("     ,".ToCharArray());
                        attribute2ndsLine = attribute2ndsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(attribute2ndsLine);
                    }
                    if (positionsLine != "")
                    {
                        positionsLine = $"{sqlCommand} INTO `ace_position` (`aceObjectId`, `positionType`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`)" + Environment.NewLine
                            + "VALUES " + positionsLine.TrimStart("     ,".ToCharArray());
                        positionsLine = positionsLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(positionsLine);
                    }
                    if (pagesLine != "")
                    {
                        pagesLine = $"{sqlCommand} INTO `ace_object_properties_book` (`aceObjectId`, `page`, `authorName`, `authorAccount`, `authorId`, `ignoreAuthor`, `pageText`)" + Environment.NewLine
                            + "VALUES " + pagesLine.TrimStart("     ,".ToCharArray());
                        pagesLine = pagesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(pagesLine);
                    }
                    if (instancesLine != "")
                    {
                        instancesLine = $"{sqlCommand} INTO `ace_object_inventory` (`aceObjectId`, `destinationType`, `weenieClassId`, `stackSize`, `palette`, `shade`, `tryToBond`)" + Environment.NewLine
                            + "VALUES " + instancesLine.TrimStart("     ,".ToCharArray());
                        instancesLine = instancesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(instancesLine);
                    }
                    if (profilesLine != "")
                    {
                        profilesLine = $"{sqlCommand} INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`)" + Environment.NewLine
                            + "VALUES " + profilesLine.TrimStart("     ,".ToCharArray());
                        profilesLine = profilesLine.TrimEnd(Environment.NewLine.ToCharArray()) + ";" + Environment.NewLine;
                        writer.WriteLine(profilesLine);
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
                        targetsLine += $"UPDATE `ace_landblock` SET `linkSlot`='{slotId}', `linkSource`='1' WHERE `preassignedGuid`='{link.Key}'; /* {instanceNames[link.Key]} */" + Environment.NewLine; //+

                        foreach (var source in link.Value)
                        {
                            sourcesLine += $"UPDATE `ace_landblock` SET `linkSlot`='{slotId}' WHERE `preassignedGuid`='{source}'; /* {instanceNames[link.Key]} <- {instanceNames[source]} */" + Environment.NewLine;
                        }

                        slotId++;
                    }

                    if (instanceLine != "")
                    {
                        instanceLine = $"{sqlCommand} INTO `ace_landblock` (`weenieClassId`, `preassignedGuid`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`)" + Environment.NewLine
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
    }
}
