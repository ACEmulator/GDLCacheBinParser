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

            result.Id = input.ID;

            result.Unknown1 = input.unknown_1;
            result.Skill = input.Skill;
            result.Difficulty = input.Difficulty;
            result.SalvageType = input.SalvageType;

            result.SuccessWCID = input.SuccessWCID;
            result.SuccessAmount = input.SuccessAmount;
            result.SuccessMessage = input.SuccessMessage;

            result.FailWCID = input.FailWCID;
            result.FailAmount = input.FailAmount;
            result.FailMessage = input.FailMessage;

            result.SuccessDestroyTargetChance = input.Components[0].DestroyChance;
            result.SuccessDestroyTargetAmount = input.Components[0].DestroyAmount;
            result.SuccessDestroyTargetMessage = input.Components[0].DestroyMessage;

            result.SuccessDestroySourceChance = input.Components[1].DestroyChance;
            result.SuccessDestroySourceAmount = input.Components[1].DestroyAmount;
            result.SuccessDestroySourceMessage = input.Components[1].DestroyMessage;

            result.FailDestroyTargetChance = input.Components[2].DestroyChance;
            result.FailDestroyTargetAmount = input.Components[2].DestroyAmount;
            result.FailDestroyTargetMessage = input.Components[2].DestroyMessage;

            result.FailDestroySourceChance = input.Components[3].DestroyChance;
            result.FailDestroySourceAmount = input.Components[3].DestroyAmount;
            result.FailDestroySourceMessage = input.Components[3].DestroyMessage;

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
                            Value = requirement.Value, 
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

            for (int i = 0 ; i < 8 ; i++) // Must be 8
            {
                var recipeMod = new RecipeMod();

                var value = input.Mods[i];

                if (value.IntMods != null)
                {
                    foreach (var mod in value.IntMods)
                    {
                        recipeMod.RecipeModsInt.Add(new RecipeModsInt
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                if (value.DIDMods != null)
                {
                    foreach (var mod in value.DIDMods)
                    {
                        recipeMod.RecipeModsDID.Add(new RecipeModsDID
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                if (value.IIDMods != null)
                {
                    foreach (var mod in value.IIDMods)
                    {
                        recipeMod.RecipeModsIID.Add(new RecipeModsIID
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                if (value.FloatMods != null)
                {
                    foreach (var mod in value.FloatMods)
                    {
                        recipeMod.RecipeModsFloat.Add(new RecipeModsFloat
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                if (value.StringMods != null)
                {
                    foreach (var mod in value.StringMods)
                    {
                        recipeMod.RecipeModsString.Add(new RecipeModsString
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                if (value.BoolMods != null)
                {
                    foreach (var mod in value.BoolMods)
                    {
                        recipeMod.RecipeModsBool.Add(new RecipeModsBool
                        {
                            Stat = mod.Stat,
                            Value = mod.Value,
                            Enum = mod.Enum,
                            Source = mod.Source
                        });
                    }
                }

                recipeMod.RecipeId = result.Id;

                recipeMod.ExecutesOnSuccess = (i <= 3); // The first 4 are "act on success", the second 4 are "act on failure"

                recipeMod.Health = value.Health;
                recipeMod.Stamina = value.Stamina;
                recipeMod.Mana = value.Mana;
                // In the cache.bin, the below flags are always set if their associated vital is != 0
                //recipeMod.Unknown4 = value.DoHealthMod;
                //recipeMod.Unknown5 = value.DoStaminaMod;
                //recipeMod.Unknown6 = value.DoManaMod;

                recipeMod.Unknown7 = value.Unknown7;
                recipeMod.DataId = value.DataID;

                recipeMod.Unknown9 = value.Unknown9;
                recipeMod.InstanceId = value.InstanceID;

                bool add = (recipeMod.Health > 0 || recipeMod.Stamina > 0 || recipeMod.Mana > 0);
                add = (add || recipeMod.Unknown7 || recipeMod.DataId > 0 || recipeMod.Unknown9 > 0 || recipeMod.InstanceId > 0);
                add = (add || recipeMod.RecipeModsBool.Count > 0 || recipeMod.RecipeModsDID.Count > 0 || recipeMod.RecipeModsFloat.Count > 0 || recipeMod.RecipeModsIID.Count > 0 || recipeMod.RecipeModsInt.Count > 0 || recipeMod.RecipeModsString.Count > 0);

                if (add)
                    result.RecipeMod.Add(recipeMod);
            }

            result.DataId = input.DataID;

            result.LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00);

            return result;
        }

        public static CookBook ConvertToACE(this Seg4_CraftTable.Precursor input)
        {
            var result = new CookBook();

            result.RecipeId = input.RecipeID;
            result.TargetWCID = input.Target;
            result.SourceWCID = input.Source;

            result.LastModified = new System.DateTime(2005, 2, 9, 10, 00, 00);

            return result;
        }
    }
}
