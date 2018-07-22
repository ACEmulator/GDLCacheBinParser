using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using PhatACCacheBinParser.Enums;

namespace PhatACCacheBinParser.SQLWriters
{
    static class WeenieSQLWriter
    {
        public static void WriteFiles(ICollection<ACE.Database.Models.World.Weenie> input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            Parallel.ForEach(input, value =>
            //foreach (var value in input)
            {
                // Highest Weenie Exported in WorldSpawns was: 30937
                // Highest Weenie found in WeenieClasses was: 31034

                uint highestWeenieAllowed = 31034;

                if (value.ClassId > highestWeenieAllowed && highestWeenieAllowed > 0)
                    return;


                // Adjust the output folder based on the weenie type, creature type and item type

                var subFolder = outputFolder + Enum.GetName(typeof(WeenieType), value.Type) + "\\";

                if (value.Type == (uint)WeenieType.Creature)
                {
                    var property = value.WeeniePropertiesInt.FirstOrDefault(r => r.Type == (int)PropertyInt.CreatureType);

                    if (property != null)
                    {
                        Enum.TryParse(property.Value.ToString(), out CreatureType ct);

                        if (Enum.IsDefined(typeof(CreatureType), ct))
                            subFolder += Enum.GetName(typeof(CreatureType), property.Value) + "\\";
                        else
                            subFolder += "UnknownCT_" + property.Value + "\\";
                    }
                    else
                        subFolder += "Unsorted" + "\\";
                }

                if (value.Type != (uint)WeenieType.Creature)
                {
                    var property = value.WeeniePropertiesInt.FirstOrDefault(r => r.Type == (int)PropertyInt.ItemType);

                    if (property != null)
                        subFolder += Enum.GetName(typeof(ItemType), property.Value) + "\\";
                    else
                        subFolder += Enum.GetName(typeof(ItemType), ItemType.None) + "\\";
                }

                WriteFile(value, subFolder, weenieNames, includeDELETEStatementBeforeInsert);
            });
        }

        public static void WriteFile(ACE.Database.Models.World.Weenie input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var description = input.WeeniePropertiesString.FirstOrDefault(r => r.Type == (int)PropertyString.Name);

            var fileName = input.ClassId.ToString("00000") + " " + (description != null ? description.Value : "");
            fileName = Util.IllegalInFileName.Replace(fileName, "_");

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName + ".sql"))
            {
                if (includeDELETEStatementBeforeInsert)
                {
                    CreateSQLDELETEStatement(input, writer);
                    writer.WriteLine();
                }

                CreateSQLINSERTStatement(input, writer, weenieNames);
            }
        }

        public static void CreateSQLDELETEStatement(ACE.Database.Models.World.Weenie input, StreamWriter writer)
        {
            throw new NotImplementedException();

            // We need to delete all the foreign rows as well)" + Environment.NewLine

            //writer.WriteLine($"DELETE FROM weenie WHERE class_Id = {input.ClassId};");
        }

        public static void CreateSQLINSERTStatement(ACE.Database.Models.World.Weenie input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            string className = ((WeenieClasses)input.ClassId).GetNameFormattedForDatabase();

            writer.WriteLine("INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)");
            writer.WriteLine($"VALUES ('{input.ClassId}', '{className}', {input.Type}) /* {Enum.GetName(typeof(WeenieType), input.Type)} */;");

            if (input.WeeniePropertiesInt != null && input.WeeniePropertiesInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt.OrderBy(r => r.Type).ToList(), writer, weenieNames);
            }
            if (input.WeeniePropertiesInt64 != null && input.WeeniePropertiesInt64.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt64.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesBool != null && input.WeeniePropertiesBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBool.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesFloat != null && input.WeeniePropertiesFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesFloat.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesString != null && input.WeeniePropertiesString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesString.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesDID != null && input.WeeniePropertiesDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesDID.OrderBy(r => r.Type).ToList(), writer, weenieNames);
            }

            if (input.WeeniePropertiesPosition != null && input.WeeniePropertiesPosition.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPosition.OrderBy(r => r.PositionType).ToList(), writer);
            }

            if (input.WeeniePropertiesIID != null && input.WeeniePropertiesIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesIID.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesAttribute != null && input.WeeniePropertiesAttribute.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesAttribute2nd != null && input.WeeniePropertiesAttribute2nd.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute2nd.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesSkill != null && input.WeeniePropertiesSkill.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSkill.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesBodyPart != null && input.WeeniePropertiesBodyPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBodyPart.OrderBy(r => r.Key).ToList(), writer);
            }

            if (input.WeeniePropertiesSpellBook != null && input.WeeniePropertiesSpellBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSpellBook.OrderBy(r => r.Spell).ToList(), writer);
            }

            if (input.WeeniePropertiesEventFilter != null && input.WeeniePropertiesEventFilter.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEventFilter.OrderBy(r => r.Event).ToList(), writer);
            }

            if (input.WeeniePropertiesEmote != null && input.WeeniePropertiesEmote.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEmote.OrderBy(r => r.EmoteSetId).ThenBy(r => r.Category).ToList(), writer);
            }
            if (input.WeeniePropertiesEmoteAction != null && input.WeeniePropertiesEmoteAction.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEmoteAction.OrderBy(r => r.EmoteSetId).ThenBy(r => r.EmoteCategory).ThenBy(r => r.Order).ToList(), writer);
            }

            if (input.WeeniePropertiesCreateList != null && input.WeeniePropertiesCreateList.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesCreateList.OrderBy(r => r.WeenieClassId).ToList(), writer, weenieNames);
            }

            if (input.WeeniePropertiesBook != null)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBook, writer);
            }
            if (input.WeeniePropertiesBookPageData != null && input.WeeniePropertiesBookPageData.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBookPageData.OrderBy(r => r.PageId).ToList(), writer);
            }

            if (input.WeeniePropertiesGenerator != null && input.WeeniePropertiesGenerator.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesGenerator.OrderBy(r => r.Probability).ToList(), writer);
            }

            if (input.WeeniePropertiesPalette != null && input.WeeniePropertiesPalette.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPalette.OrderBy(r => r.SubPaletteId).ToList(), writer);
            }
            if (input.WeeniePropertiesTextureMap != null && input.WeeniePropertiesTextureMap.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesTextureMap.OrderBy(r => r.Index).ToList(), writer);
            }
            if (input.WeeniePropertiesAnimPart != null && input.WeeniePropertiesAnimPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAnimPart.OrderBy(r => r.Index).ToList(), writer);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesInt> input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                string propertyValueDescription = null;

                switch ((PropertyInt)input[i].Type)
                {
                    case PropertyInt.AmmoType:
                        propertyValueDescription = Enum.GetName(typeof(AmmoType), input[i].Value);
                        break;
                    case PropertyInt.BoosterEnum:
                        propertyValueDescription = Enum.GetName(typeof(PropertyAttribute2nd), input[i].Value);
                        break;
                    case PropertyInt.ClothingPriority:
                        propertyValueDescription = ((CoverageMask)input[i].Value).ToString();
                        break;
                    case PropertyInt.CombatMode:
                        propertyValueDescription = Enum.GetName(typeof(CombatMode), input[i].Value);
                        break;
                    case PropertyInt.CombatUse:
                        propertyValueDescription = Enum.GetName(typeof(COMBAT_USE), input[i].Value);
                        break;
                    case PropertyInt.CreatureType:
                    case PropertyInt.SlayerCreatureType:
                    case PropertyInt.FoeType:
                    case PropertyInt.FriendType:
                        propertyValueDescription = Enum.GetName(typeof(CreatureType), input[i].Value);
                        break;
                    case PropertyInt.CurrentWieldedLocation:
                    case PropertyInt.ValidLocations:
                        propertyValueDescription = ((EquipMask)input[i].Value).ToString();
                        break;
                    case PropertyInt.DamageType:
                        propertyValueDescription = ((DamageType)input[i].Value).ToString();
                        break;
                    case PropertyInt.DefaultCombatStyle:
                    case PropertyInt.AiAllowedCombatStyle:
                        propertyValueDescription = Enum.GetName(typeof(CombatStyle), input[i].Value);
                        break;
                    case PropertyInt.Gender:
                        propertyValueDescription = Enum.GetName(typeof(Gender), input[i].Value);
                        break;
                    case PropertyInt.GeneratorDestructionType:
                    case PropertyInt.GeneratorEndDestructionType:
                        propertyValueDescription = Enum.GetName(typeof(GeneratorDestruct), input[i].Value);
                        break;
                    case PropertyInt.GeneratorTimeType:
                        propertyValueDescription = Enum.GetName(typeof(GeneratorTimeType), input[i].Value);
                        break;
                    case PropertyInt.GeneratorType:
                        propertyValueDescription = Enum.GetName(typeof(GeneratorType), input[i].Value);
                        break;
                    case PropertyInt.HeritageGroup:
                        propertyValueDescription = Enum.GetName(typeof(HeritageGroup), input[i].Value);
                        break;
                    case PropertyInt.HookItemType:
                    case PropertyInt.ItemType:
                    case PropertyInt.MerchandiseItemTypes:
                    case PropertyInt.TargetType:
                        propertyValueDescription = Enum.GetName(typeof(ItemType), input[i].Value);
                        break;
                    case PropertyInt.HookPlacement:
                    case PropertyInt.Placement:
                        propertyValueDescription = Enum.GetName(typeof(Placement), input[i].Value);
                        break;
                    case PropertyInt.HookType:
                        propertyValueDescription = ((HookTypeEnum)input[i].Value).ToString();
                        break;
                    case PropertyInt.HouseType:
                        propertyValueDescription = Enum.GetName(typeof(HouseType), input[i].Value);
                        break;
                    case PropertyInt.ItemUseable:
                        propertyValueDescription = ((ITEM_USEABLE)input[i].Value).ToString();
                        break;
                    case PropertyInt.ItemXpStyle:
                        propertyValueDescription = Enum.GetName(typeof(ItemXpStyle), input[i].Value);
                        break;
                    case PropertyInt.MaterialType:
                        propertyValueDescription = Enum.GetName(typeof(Material), input[i].Value);
                        break;
                    case PropertyInt.PaletteTemplate:
                        propertyValueDescription = Enum.GetName(typeof(PALETTE_TEMPLATE), input[i].Value);
                        break;
                    case PropertyInt.PhysicsState:
                        propertyValueDescription = ((PhysicsState)input[i].Value).ToString();
                        break;
                    case PropertyInt.PlayerKillerStatus:
                        propertyValueDescription = Enum.GetName(typeof(PlayerKillerStatus), input[i].Value);
                        break;
                    case PropertyInt.PortalBitmask:
                        propertyValueDescription = Enum.GetName(typeof(PortalBitmask), input[i].Value);
                        break;
                    case PropertyInt.RadarBlipColor:
                        propertyValueDescription = Enum.GetName(typeof(RadarColor), input[i].Value);
                        break;
                    case PropertyInt.ShowableOnRadar:
                        propertyValueDescription = Enum.GetName(typeof(RadarBehavior), input[i].Value);
                        break;
                    case PropertyInt.SummoningMastery:
                        propertyValueDescription = Enum.GetName(typeof(SummoningMastery), input[i].Value);
                        break;
                    case PropertyInt.UiEffects:
                        propertyValueDescription = Enum.GetName(typeof(UiEffects), input[i].Value);
                        break;
                    case PropertyInt.WeaponSkill:
                    case PropertyInt.WieldSkilltype2:
                    case PropertyInt.WieldSkilltype3:
                    case PropertyInt.WieldSkilltype4:
                    case PropertyInt.WieldSkilltype:
                        propertyValueDescription = Enum.GetName(typeof(Skill), input[i].Value);
                        break;
                    case PropertyInt.WeaponType:
                        propertyValueDescription = Enum.GetName(typeof(WeaponType), input[i].Value);
                        break;
                    case PropertyInt.ActivationCreateClass:
                        propertyValueDescription = weenieNames[(uint)input[i].Value];
                        break;
                    case PropertyInt.ActivationResponse:
                        propertyValueDescription = Enum.GetName(typeof(ActivationResponseEnum), input[i].Value);
                        break;
                    case PropertyInt.Attuned:
                        propertyValueDescription = Enum.GetName(typeof(AttunedStatusEnum), input[i].Value);
                        break;
                    case PropertyInt.AttackHeight:
                        propertyValueDescription = Enum.GetName(typeof(AttackHeight), input[i].Value);
                        break;
                    case PropertyInt.AttackType:
                        propertyValueDescription = ((AttackType)input[i].Value).ToString();
                        break;
                    case PropertyInt.Bonded:
                        propertyValueDescription = Enum.GetName(typeof(BondedStatusEnum), input[i].Value);
                        break;
                    case PropertyInt.ChannelsActive:
                    case PropertyInt.ChannelsAllowed:
                        propertyValueDescription = ((Channel)input[i].Value).ToString();
                        break;
                    case PropertyInt.AccountRequirements:
                        propertyValueDescription = Enum.GetName(typeof(SubscriptionStatus__guessedname), input[i].Value);
                        break;
                    case PropertyInt.AetheriaBitfield:
                        propertyValueDescription = ((AetheriaBitfield)input[i].Value).ToString();
                        break;
                    case PropertyInt.EquipmentSetId:
                        propertyValueDescription = Enum.GetName(typeof(EquipmentSet), input[i].Value);
                        break;
                    case PropertyInt.WieldRequirements2:
                    case PropertyInt.WieldRequirements3:
                    case PropertyInt.WieldRequirements4:
                    case PropertyInt.WieldRequirements:
                        propertyValueDescription = ((WieldRequirement)input[i].Value).ToString();
                        break;
                    case PropertyInt.GeneratorStartTime:
                    case PropertyInt.GeneratorEndTime:
                        propertyValueDescription = DateTimeOffset.FromUnixTimeSeconds(input[i].Value).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
                        break;
                    case PropertyInt.ImbuedEffect2:
                    case PropertyInt.ImbuedEffect3:
                    case PropertyInt.ImbuedEffect4:
                    case PropertyInt.ImbuedEffect5:
                    case PropertyInt.ImbuedEffect:
                        propertyValueDescription = ((ImbuedEffectType)input[i].Value).ToString();
                        break;
                }

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Type);
                if (propertyValueDescription != null)
                    comment = (comment != null ? comment.PadRight(40) : "") + " " + propertyValueDescription;

                // ReSharper disable once PossibleNullReferenceException
                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesInt64> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInt64), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadRight(5)}) /* {Enum.GetName(typeof(PropertyBool), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString("0.00").PadLeft(7)}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, '{input[i].Value.Replace("'", "''")}') /* {Enum.GetName(typeof(PropertyString), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesDID> input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                string propertyValueDescription = null;

                switch ((PropertyDataId)input[i].Type)
                {
                    case PropertyDataId.ActivationAnimation:
                    case PropertyDataId.InitMotion:
                    case PropertyDataId.UseTargetAnimation:
                    case PropertyDataId.UseTargetFailureAnimation:
                    case PropertyDataId.UseTargetSuccessAnimation:
                    case PropertyDataId.UseUserAnimation:
                        propertyValueDescription = ((MotionCommand)input[i].Value).ToString();
                        break;
                    case PropertyDataId.ActivationSound:
                    case PropertyDataId.UseSound:
                        propertyValueDescription = ((Sound)input[i].Value).ToString();
                        break;
                    case PropertyDataId.AlternateCurrency:
                    case PropertyDataId.AugmentationCreateItem:
                    case PropertyDataId.LastPortal:
                    case PropertyDataId.LinkedPortalOne:
                    case PropertyDataId.LinkedPortalTwo:
                    case PropertyDataId.OriginalPortal:
                    case PropertyDataId.UseCreateItem:
                    case PropertyDataId.VendorsClassId:
                        weenieNames.TryGetValue(input[i].Value, out propertyValueDescription);
                        break;
                    case PropertyDataId.BlueSurgeSpell:
                    case PropertyDataId.DeathSpell:
                    case PropertyDataId.ProcSpell:
                    case PropertyDataId.RedSurgeSpell:
                    case PropertyDataId.Spell:
                    case PropertyDataId.YellowSurgeSpell:
                        propertyValueDescription = ((SpellID)input[i].Value).ToString();
                        break;
                    case PropertyDataId.PhysicsScript:
                    case PropertyDataId.RestrictionEffect:
                        propertyValueDescription = ((PlayScript)input[i].Value).ToString();
                        break;
                    case PropertyDataId.WieldedTreasureType:
                    case PropertyDataId.DeathTreasureType:
                        // todo
                        break;
                }

                var comment = Enum.GetName(typeof(PropertyDataId), input[i].Type);
                if (propertyValueDescription != null)
                    comment = (comment != null ? comment.PadRight(25) : "") + " " + propertyValueDescription;

                // ReSharper disable once PossibleNullReferenceException
                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesPosition> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_X`, `angles_Y`, `angles_Z`, `angles_W`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {(uint)input[i].PositionType}, {input[i].ObjCellId}, {input[i].OriginX}, {input[i].OriginY}, {input[i].OriginZ}, {input[i].AnglesX}, {input[i].AnglesY}, {input[i].AnglesZ}, {input[i].AnglesW}) /* {Enum.GetName(typeof(PositionType), input[i].PositionType)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInstanceId), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesAttribute> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(3)}, {input[i].LevelFromCP}, {input[i].CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesAttribute2nd> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(5)}, {input[i].LevelFromCP}, {input[i].CPSpent}, {input[i].CurrentLevel}) /* {Enum.GetName(typeof(PropertyAttribute2nd), input[i].Type)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesSkill> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                // ReSharper disable once PossibleNullReferenceException
                output += $"{weenieClassID}, " +
                          $"{input[i].Type.ToString().PadLeft(2)}, " +
                          $"{input[i].LevelFromPP}, " +
                          $"{input[i].SAC}, " +
                          $"{input[i].PP}, " +
                          $"{input[i].InitLevel.ToString().PadLeft(3)}, " +
                          $"{input[i].ResistanceAtLastCheck}, " +
                          $"{input[i].LastUsedTime}) " +
                          $"/* {Enum.GetName(typeof(Skill), input[i].Type).PadRight(19)} {((SkillStatus)input[i].SAC).ToString()} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesBodyPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, " +
                             "`b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, " +
                          $"{input[i].Key.ToString().PadLeft(2)}, " +
                          $"{input[i].DType.ToString().PadLeft(2)}, " +
                          $"{input[i].DVal.ToString().PadLeft(2)}, " +
                          $"{input[i].DVar.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].BaseArmor.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsSlash.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsPierce.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsBludgeon.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsCold.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsFire.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsAcid.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsElectric.ToString().PadLeft(4)}, " +
                          $"{input[i].ArmorVsNether.ToString().PadLeft(4)}, " +
                          $"{input[i].BH}, " +
                          $"{input[i].HLF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].MLF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].LLF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].HRF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].MRF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].LRF.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].HLB.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].MLB.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].LLB.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].HRB.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].MRB.ToString("0.00").PadLeft(4)}, " +
                          $"{input[i].LRB.ToString("0.00").PadLeft(4)}) " +
                          $"/* {Enum.GetName(typeof(BodyPart), input[i].Key)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesSpellBook> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Spell.ToString().PadLeft(5)}, {input[i].Probability.ToString("0.000").PadLeft(6)})  /* {Enum.GetName(typeof(SpellID), input[i].Spell)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesEventFilter> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_event_filter` (`object_Id`, `event`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Event.ToString().PadLeft(3)}) " + $"/* {Enum.GetName(typeof(PacketOpcode), input[i].Event)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesEmote> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_emote` (`object_Id`, `emote_Set_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, " +
                          $"{input[i].EmoteSetId}, " +
                          $"{input[i].Category}, " +
                          $"{input[i].Probability.ToString("0.0000").PadLeft(6)}, " +
                          $"{input[i].WeenieClassId}, " +
                          $"{input[i].Style}, " +
                          $"{input[i].Substyle}, " +
                          $"'{(input[i].Quest ?? "").Replace("'", "''")}', " +
                          $"{input[i].VendorType}, " +
                          $"{input[i].MinHealth}, {input[i].MaxHealth})";

                output = FixNullFields(output);

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesEmoteAction> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_emote_action` (`object_Id`, `emote_Set_Id`, `emote_Category`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, " +
                             "`stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, " +
                             "`obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_X`, `angles_Y`, `angles_Z`, `angles_W`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, " +
                          $"{input[i].EmoteSetId}, " +
                          $"{input[i].EmoteCategory}, " +
                          $"{input[i].Order}, " +
                          $"{input[i].Type.ToString().PadLeft(2)}, " +
                          $"{input[i].Delay}, " +
                          $"{input[i].Extent}, " +
                          $"{input[i].Motion}, " +
                          $"'{(input[i].Message ?? "").Replace("'", "''")}', " +
                          $"'{(input[i].TestString ?? "").Replace("'", "''")}', " +
                          $"{input[i].Min}, " +
                          $"{input[i].Max}, " +
                          $"{input[i].Min64}, " +
                          $"{input[i].Max64}, " +
                          $"{input[i].MinDbl}, " +
                          $"{input[i].MaxDbl}, " +
                          $"{input[i].StackSize}, " +
                          $"{input[i].Display}, " +
                          $"{input[i].Amount}, " +
                          $"{input[i].Amount64}, " +
                          $"{input[i].Percent}, " +
                          $"{input[i].SpellId}, " +
                          $"{input[i].WealthRating}, " +
                          $"{input[i].TreasureClass}, " +
                          $"{input[i].TreasureType}, " +
                          $"{input[i].PScript}, " +
                          $"{input[i].Sound}, " +
                          $"{input[i].DestinationType}, " +
                          $"{input[i].WeenieClassId}, " +
                          $"{input[i].StackSize}, " +
                          $"{input[i].Palette}, " +
                          $"{input[i].Shade}, " +
                          $"{input[i].TryToBond}, " +
                          $"{input[i].ObjCellId}, " +
                          $"{input[i].OriginX}, " +
                          $"{input[i].OriginY}, " +
                          $"{input[i].OriginZ}, " +
                          $"{input[i].AnglesX}, " +
                          $"{input[i].AnglesY}, " +
                          $"{input[i].AnglesZ}, " +
                          $"{input[i].AnglesW})";

                output = FixNullFields(output);

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesCreateList> input, StreamWriter writer, Dictionary<uint, string> weenieNames)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                weenieNames.TryGetValue(input[i].WeenieClassId, out var weenieName);

                output += $"{weenieClassID}, {input[i].DestinationType}, {input[i].WeenieClassId.ToString().PadLeft(5)}, {input[i].StackSize.ToString().PadLeft(2)}, {input[i].Palette}, {input[i].Shade}, {input[i].TryToBond}) /* Create {weenieName ?? "Unknown"} for {Enum.GetName(typeof(DestinationType), input[i].DestinationType)} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, ACE.Database.Models.World.WeeniePropertiesBook input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)");

           writer.WriteLine($"VALUES ({weenieClassID}, {input.MaxNumPages}, {input.MaxNumCharsPerPage});");
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesBookPageData> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].PageId}, {input[i].AuthorId}, '{input[i].AuthorName.Replace("'", "''")}', '{input[i].AuthorAccount.Replace("'", "''")}', {input[i].IgnoreAuthor}, '{input[i].PageText.Replace("'", "''")}')";

                output = FixNullFields(output);

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesGenerator> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_X`, `angles_Y`, `angles_Z`, `angles_W`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, " +
                          $"{input[i].Probability}, " +
                          $"{input[i].WeenieClassId}," +
                          $" {input[i].Delay}, " +
                          $"{input[i].InitCreate}, " +
                          $"{input[i].MaxCreate}, " +
                          $"{input[i].WhenCreate}, " +
                          $"{input[i].WhereCreate}, " +
                          $"{input[i].StackSize}, " +
                          $"{input[i].PaletteId}, " +
                          $"{input[i].Shade}, " +
                          $"{input[i].ObjCellId}, " +
                          $"{input[i].OriginX}, " +
                          $"{input[i].OriginY}, " +
                          $"{input[i].OriginZ}, " +
                          $"{input[i].AnglesX}, " +
                          $"{input[i].AnglesY}, " +
                          $"{input[i].AnglesZ}, " +
                          $"{input[i].AnglesW})";

                output = FixNullFields(output);

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesPalette> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_palette` (`object_Id`, `sub_Palette_Id`, `offset`, `length`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].SubPaletteId}, {input[i].Offset}, {input[i].Length})";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesTextureMap> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_texture_map` (`object_Id`, `index`, `old_Id`, `new_Id`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Index}, {input[i].OldId}, {input[i].NewId})";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesAnimPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_anim_part` (`object_Id`, `index`, `animation_Id`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Index}, {input[i].AnimationId})";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        private static void ValuesWriter(int count, Func<int, string> lineGenerator, StreamWriter writer)
        {
            for (int i = 0; i < count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += lineGenerator(i);

                if (i == count - 1)
                    output += ";";

                FixNullFields(input);

                writer.WriteLine(output);
            }
        }

        private static string FixNullFields(string input)
        {
            // We must do this twice for each to account for adjacent matches since we search inclusive of a start and end comma
            input = input.Replace(", '',", ", NULL,");
            input = input.Replace(", '',", ", NULL,");

            input = input.Replace(", ,", ", NULL,");
            input = input.Replace(", ,", ", NULL,");

            // Fix cases where the last field might be null
            input = input.Replace(", )", ", NULL)");
            input = input.Replace(", '')", ", NULL)");

            return input;
        }
    }
}
