using System;
using System.Collections.Generic;
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
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt.ToList(), writer);
            }
            if (input.WeeniePropertiesInt64 != null && input.WeeniePropertiesInt64.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt64.ToList(), writer);
            }
            if (input.WeeniePropertiesBool != null && input.WeeniePropertiesBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBool.ToList(), writer);
            }
            if (input.WeeniePropertiesFloat != null && input.WeeniePropertiesFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesFloat.ToList(), writer);
            }
            if (input.WeeniePropertiesString != null && input.WeeniePropertiesString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesString.ToList(), writer);
            }
            if (input.WeeniePropertiesDID != null && input.WeeniePropertiesDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesDID.ToList(), writer);
            }

            if (input.WeeniePropertiesPosition != null && input.WeeniePropertiesPosition.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPosition.ToList(), writer);
            }

            if (input.WeeniePropertiesIID != null && input.WeeniePropertiesIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesIID.ToList(), writer);
            }

            if (input.WeeniePropertiesAttribute != null && input.WeeniePropertiesAttribute.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute.ToList(), writer);
            }
            if (input.WeeniePropertiesAttribute2nd != null && input.WeeniePropertiesAttribute2nd.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute2nd.ToList(), writer);
            }

            if (input.WeeniePropertiesSkill != null && input.WeeniePropertiesSkill.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSkill.ToList(), writer);
            }

            if (input.WeeniePropertiesBodyPart != null && input.WeeniePropertiesBodyPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBodyPart.ToList(), writer);
            }

            if (input.WeeniePropertiesSpellBook != null && input.WeeniePropertiesSpellBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSpellBook.ToList(), writer);
            }

            if (input.WeeniePropertiesEventFilter != null && input.WeeniePropertiesEventFilter.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEventFilter.ToList(), writer);
            }

            if (input.WeeniePropertiesEmote != null && input.WeeniePropertiesEmote.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEmote.ToList(), writer);
            }

            if (input.WeeniePropertiesCreateList != null && input.WeeniePropertiesCreateList.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesCreateList.ToList(), writer);
            }

            if (input.WeeniePropertiesBook != null)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBook, writer);
            }
            if (input.WeeniePropertiesBookPageData != null && input.WeeniePropertiesBookPageData.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBookPageData.ToList(), writer);
            }

            if (input.WeeniePropertiesGenerator != null && input.WeeniePropertiesGenerator.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesGenerator.ToList(), writer);
            }

            if (input.WeeniePropertiesPalette != null && input.WeeniePropertiesPalette.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPalette.ToList(), writer);
            }
            if (input.WeeniePropertiesTextureMap != null && input.WeeniePropertiesTextureMap.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesTextureMap.ToList(), writer);
            }
            if (input.WeeniePropertiesAnimPart != null && input.WeeniePropertiesAnimPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAnimPart.ToList(), writer);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInt), input[i].Type)} */";

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

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString("0.00").PadLeft(5)}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Type)} */";

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

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyDataId), input[i].Type)} */";

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

                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel}, {input[i].LevelFromCP}, {input[i].CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), input[i].Type)} */";

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
                output += $"{weenieClassID}, {input[i].Type.ToString().PadLeft(2)}, {input[i].LevelFromPP}, {input[i].SAC}, {input[i].PP}, {input[i].InitLevel.ToString().PadLeft(3)}, {input[i].ResistanceAtLastCheck}, {input[i].LastUsedTime}) " + $"/* {Enum.GetName(typeof(Skill), input[i].Type).PadRight(19)} {((SkillStatus)input[i].SAC).ToString()} */";

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesBodyPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)");

            for (int i = 0; i < input.Count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += $"{weenieClassID}, {input[i].Key}, {input[i].DType}, {input[i].DVal}, {input[i].DVar}, {input[i].BaseArmor}, {input[i].ArmorVsSlash}, {input[i].ArmorVsPierce}, {input[i].ArmorVsBludgeon}, {input[i].ArmorVsCold}, {input[i].ArmorVsFire}, {input[i].ArmorVsAcid}, {input[i].ArmorVsElectric}, {input[i].ArmorVsNether}, {input[i].BH}, {input[i].HLF}, {input[i].MLF}, {input[i].LLF}, {input[i].HRF}, {input[i].MRF}, {input[i].LRF}, {input[i].HLB}, {input[i].MLB}, {input[i].LLB}, {input[i].HRB}, {input[i].MRB}, {input[i].LRB}) " + $"/* {Enum.GetName(typeof(BodyPart), input[i].Key)} */";

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
            writer.WriteLine("INSERT INTO `weenie_properties_emote` (`object_Id`, `probability`, `category`, `emote_Set_Id`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)");

            // todo call WeeniePropertiesEmoteAction
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesEmoteAction> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_emote` (`object_Id`, `probability`, `category`, `emote_Set_Id`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)");

            // todo
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesCreateList> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)");

            // todo
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

                if (i == input.Count - 1)
                    output += ";";

                writer.WriteLine(output);
            }
        }

        public static void CreateSQLINSERTStatement(uint weenieClassID, IList<ACE.Database.Models.World.WeeniePropertiesGenerator> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            // todo
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
    }
}
