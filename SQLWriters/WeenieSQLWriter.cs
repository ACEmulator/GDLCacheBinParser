using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using PhatACCacheBinParser.Enums;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg3_TreasureTable;
using PhatACCacheBinParser.Seg9_WeenieDefaults;

namespace PhatACCacheBinParser.SQLWriters
{
    static class WeenieSQLWriter
    {
        public static void WriteWeenieFiles(WeenieDefaults weenieDefaults, TreasureTable treasureTable, Dictionary<uint, string> weenieNames)
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "9 WeenieDefaults" + "\\" + "\\SQL Old Method\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Highest Weenie Exported in WorldSpawns was: 30937
            // Highest Weenie found in WeenieClasses was: 31034

            uint highestWeenieAllowed = 31034;

            string sqlCommand = "INSERT";

            //Parallel.ForEach(weenieDefaults.Weenies, weenie =>
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

                string FileNameFormatter(Weenie obj) => obj.WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Description, "_");

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
                }
                //});
            }

            // parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
        }

    }
}
