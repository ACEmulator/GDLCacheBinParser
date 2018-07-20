using System;
using System.Collections.Generic;
using System.IO;

using ACE.Entity.Enum.Properties;

using PhatACCacheBinParser.Enums;
using PhatACCacheBinParser.Seg4_CraftTable;

namespace PhatACCacheBinParser.SQLWriters
{
    static class CraftingSQLWriter
    {
        public static void WriteFiles(CraftingTable craftingTable, Dictionary<uint, string> weenieNames, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

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
                                componentsLine += $"     , ({recipe.ID}, {component.DestroyChance}, {component.DestroyAmount}, '{component.DestroyMessage.Replace("'", "''")}') /* Target */" + Environment.NewLine;
                                break;
                            case 2:
                            case 4:
                                componentsLine += $"     , ({recipe.ID}, {component.DestroyChance}, {component.DestroyAmount}, '{component.DestroyMessage.Replace("'", "''")}') {(sourceWCID > 0 ? $"/* {weenieNames[sourceWCID]} */" : "/* Source */")}" + Environment.NewLine;
                                break;
                        }
                        compidx++;
                    }

                    string requirementsIntLine = "", requirementsDIDLine = "", requirementsIIDLine = "", requirementsFloatLine = "", requirementsStringLine = "", requirementsBoolLine = "";

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
                            modsLine += $"     , ({recipe.ID}, {modSet}, {mod.Health}, {mod.Unknown2}, {mod.Mana}, {mod.Unknown4}, {mod.Unknown5}, {mod.Unknown6}, {mod.Unknown7}, {mod.DataID}, {mod.Unknown9}, {mod.InstanceID})" + Environment.NewLine;

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
                }
            }
        }
    }
}
