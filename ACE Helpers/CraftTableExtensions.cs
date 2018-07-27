using System.Collections.Generic;

using ACE.Database.Models.World;

namespace PhatACCacheBinParser.ACE_Helpers
{
    static class CraftTableExtensions
    {
        public class CraftTableExtensionsConversionResult
        {
            public List<Recipe> Recipies = new List<Recipe>();

            public List<CookBook> CookBooks = new List<CookBook>();
        }

        public static CraftTableExtensionsConversionResult ConvertToACE(this Seg4_CraftTable.CraftingTable input)
        {
            var results = new CraftTableExtensionsConversionResult();

            foreach (var value in input.Recipes)
            {
                var converted = value.ConvertToACE();

                results.Recipies.Add(converted);
            }

            foreach (var value in input.Precursors)
            {
                foreach (var value2 in value.Value)
                {
                    var converted = value2.ConvertToACE();

                    results.CookBooks.Add(converted);
                }
            }

            return results;
        }

        public static Recipe ConvertToACE(this Seg4_CraftTable.Recipe input)
        {
            var result = new Recipe();

            result.RecipeId = input.ID;

            result.Unknown1 = input.unknown_1;
            result.Skill = input.Skill;
            result.Difficulty = input.Difficulty;
            result.SalvageType = input.SalvageType;

            result.SuccessWCID = input.SuccessWCID;
            result.SuccessAmount = input.SuccessAmount;
            result.SuccessMessage = result.SuccessMessage ?? ""; // todo: Fix these strings in the db context so they can be null

            result.FailWCID = input.FailWCID;
            result.FailAmount = input.FailAmount;
            result.FailMessage = input.FailMessage ?? ""; // todo: Fix these strings in the db context so they can be null

            foreach (var value in input.Components)
            {
                result.RecipeComponent.Add(new RecipeComponent
                {
                    DestroyChance = value.DestroyChance,
                    DestroyAmount = value.DestroyAmount,
                    DestroyMessage = value.DestroyMessage ?? "", // todo: Fix these strings in the db context so they can be null
                });
            }

            foreach (var value in input.Requirements)
            {
                if (value.IntRequirements != null)
                {
                    foreach (var requirement in value.IntRequirements)
                    {
                        result.RecipeRequirementsInt.Add(new RecipeRequirementsInt
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value,
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }

                if (value.DIDRequirements != null)
                {
                    foreach (var requirement in value.DIDRequirements)
                    {
                        result.RecipeRequirementsDID.Add(new RecipeRequirementsDID
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value,
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }

                if (value.IIDRequirements != null)
                {
                    foreach (var requirement in value.IIDRequirements)
                    {
                        result.RecipeRequirementsIID.Add(new RecipeRequirementsIID
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value,
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }

                if (value.FloatRequirements != null)
                {
                    foreach (var requirement in value.FloatRequirements)
                    {
                        result.RecipeRequirementsFloat.Add(new RecipeRequirementsFloat
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value,
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }

                if (value.StringRequirements != null)
                {
                    foreach (var requirement in value.StringRequirements)
                    {
                        result.RecipeRequirementsString.Add(new RecipeRequirementsString
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value ?? "", // todo: Fix these strings in the db context so they can be null
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }

                if (value.BoolRequirements != null)
                {
                    foreach (var requirement in value.BoolRequirements)
                    {
                        result.RecipeRequirementsBool.Add(new RecipeRequirementsBool
                        {
                            Stat = requirement.Stat,
                            Value = requirement.Value,
                            Enum = requirement.Enum,
                            Message = requirement.Message
                        });
                    }
                }
            }

            int modSet = 1;
            foreach (var value in input.Mods)
            {
                var recipeMod = new RecipeMod();

                if (value.IntMods != null)
                {
                    foreach (var mod in value.IntMods)
                    {
                        result.RecipeModsInt.Add(new RecipeModsInt
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                if (value.DIDMods != null)
                {
                    foreach (var mod in value.DIDMods)
                    {
                        result.RecipeModsDID.Add(new RecipeModsDID
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                if (value.IIDMods != null)
                {
                    foreach (var mod in value.IIDMods)
                    {
                        result.RecipeModsIID.Add(new RecipeModsIID
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                if (value.FloatMods != null)
                {
                    foreach (var mod in value.FloatMods)
                    {
                        result.RecipeModsFloat.Add(new RecipeModsFloat
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                if (value.StringMods != null)
                {
                    foreach (var mod in value.StringMods)
                    {
                        result.RecipeModsString.Add(new RecipeModsString
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value ?? "", // todo: Fix these strings in the db context so they can be null
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                if (value.BoolMods != null)
                {
                    foreach (var mod in value.BoolMods)
                    {
                        result.RecipeModsBool.Add(new RecipeModsBool
                        {
                            RecipeId = result.RecipeId,

                            ModSetId = modSet,

                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Unknown1 = mod.Unknown1
                        });
                    }
                }

                recipeMod.RecipeId = result.RecipeId;

                recipeMod.ModSetId = modSet;

                recipeMod.Health = value.Health;
                recipeMod.Unknown2 = value.Unknown2;
                recipeMod.Mana = value.Mana;
                recipeMod.Unknown4 = value.Unknown4;
                recipeMod.Unknown5 = value.Unknown5;
                recipeMod.Unknown6 = value.Unknown6;

                recipeMod.Unknown7 = value.Unknown7;
                recipeMod.DataId = value.DataID;

                recipeMod.Unknown9 = value.Unknown9;
                recipeMod.InstanceId = value.InstanceID;

                result.RecipeMod.Add(recipeMod);

                modSet++;
            }

            result.DataId = input.DataID;

            return result;
        }

        public static CookBook ConvertToACE(this Seg4_CraftTable.Precursor input)
        {
            var result = new CookBook();

            result.RecipeId = input.RecipeID;
            result.TargetWCID = input.Target;
            result.SourceWCID = input.Source;

            return result;
        }
    }
}
