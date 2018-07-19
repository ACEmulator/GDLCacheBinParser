using System;
using System.Collections.Generic;
using System.IO;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using PhatACCacheBinParser.Enums;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg2_SpellTableExtendedData;

namespace PhatACCacheBinParser.SQLWriters
{
    static class SpellsSQLWriter
    {
        public static void WriteSpellFiles(SpellTableExtendedData spellTableExtendedData, Dictionary<uint, string> weenieNames)
        {
            var outputFolder = Settings.Default["OutputFolder"] + "\\" + "2 SpellTableExtendedData" + "\\" + "\\SQL\\";

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

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
                }
            }
        }
    }
}
