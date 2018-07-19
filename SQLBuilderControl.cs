using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;

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
using PhatACCacheBinParser.SQLWriters;

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

        private async void cmdParseAll_Click(object sender, EventArgs e)
        {
            cmdParseAll.Enabled = false;

            progressParseSources.Style = ProgressBarStyle.Marquee;

            // Read all the inputs here
            await Task.Run(delegate
            {
                // Read all the inputs here
                TryParseSegment((string)Settings.Default["_1SourceBin"], regionDescExtendedData);
                TryParseSegment((string)Settings.Default["_2SourceBin"], spellTableExtendedData);
                TryParseSegment((string)Settings.Default["_3SourceBin"], treasureTable);
                TryParseSegment((string)Settings.Default["_4SourceBin"], craftingTable);
                TryParseSegment((string)Settings.Default["_5SourceBin"], housingPortalsTable);
                TryParseSegment((string)Settings.Default["_6SourceBin"], landBlockData);
                // Segment 7
                TryParseSegment((string)Settings.Default["_8SourceBin"], questDefDB);
                TryParseSegment((string)Settings.Default["_9SourceBin"], weenieDefaults);
                TryParseSegment((string)Settings.Default["_ASourceBin"], mutationFilters);
                TryParseSegment((string)Settings.Default["_BSourceBin"], gameEventDefDB);

                CollectWeenieNames();
            });

            progressParseSources.Style = ProgressBarStyle.Continuous;
            progressParseSources.Value = 100;

            // Now that we've parsed all of our input segments, we can enable outputs
            foreach (Control control in Controls)
            {
                if (control is Button && control != sender)
                    control.Enabled = true;
            }
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
            catch
            {
                // ignored
            }

            return false;
        }

        private void CollectWeenieNames()
        {
            weenieNames.Clear();

            foreach (var weenie in weenieDefaults.Weenies)
            {
                var name = weenie.Value.Description;

                if (String.IsNullOrEmpty(name))
                {
                    if (Enum.IsDefined(typeof(WeenieClasses), (ushort)weenie.Value.WCID))
                        name = Enum.GetName(typeof(WeenieClasses), weenie.Value.WCID).Substring(2);
                    else
                    {
                        name = "ace" + weenie.Value.WCID; //+ "-" + parsed.Label.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();
                    }

                    if (name.StartsWith("W_"))
                        name = name.Remove(0, 2);

                    if (name.EndsWith("_CLASS"))
                        name = name.Remove(name.LastIndexOf("_CLASS", StringComparison.Ordinal));

                    name = name.Replace("_", "-");

                    name = name.ToLower();
                }

                weenieNames.Add(weenie.Value.WCID, name);
            }
        }

        private async void cmd1RegionsParse_Click(object sender, EventArgs e)
        {
            cmd1RegionsParse.Enabled = false;

            progressBarRegions.Style = ProgressBarStyle.Marquee;
            progressBarRegions.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                //WriteRegionFiles();
                RegionDescSQLWriter.WriteEnounterLandblockInstances(regionDescExtendedData, landBlockData, weenieNames);
            });

            progressBarRegions.Style = ProgressBarStyle.Continuous;
            progressBarRegions.Value = 100;

            cmd1RegionsParse.Enabled = true;
        }

        private async void cmd2SpellsParse_Click(object sender, EventArgs e)
        {
            cmd2SpellsParse.Enabled = false;

            progressBarSpells.Style = ProgressBarStyle.Marquee;
            progressBarSpells.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                SpellsSQLWriter.WriteSpellFiles(spellTableExtendedData, weenieNames);
            });

            progressBarSpells.Style = ProgressBarStyle.Continuous;
            progressBarSpells.Value = 100;

            cmd2SpellsParse.Enabled = true;
        }

        private async void cmd3TreasureParse_Click(object sender, EventArgs e)
        {
            cmd3TreasureParse.Enabled = false;

            progressBarTreasure.Style = ProgressBarStyle.Marquee;
            progressBarTreasure.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                TreasureSQLWriter.WriteTreasureFiles(treasureTable, weenieNames);
            });

            progressBarTreasure.Style = ProgressBarStyle.Continuous;
            progressBarTreasure.Value = 100;

            cmd3TreasureParse.Enabled = true;
        }

        private async void cmd4CraftingParse_Click(object sender, EventArgs e)
        {
            cmd4CraftingParse.Enabled = false;

            progressBarCrafting.Style = ProgressBarStyle.Marquee;
            progressBarCrafting.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                CraftingSQLWriter.WriteCraftingFiles(craftingTable, weenieNames);
            });

            progressBarCrafting.Style = ProgressBarStyle.Continuous;
            progressBarCrafting.Value = 100;

            cmd4CraftingParse.Enabled = true;
        }

        private async void cmd5HousingParse_Click(object sender, EventArgs e)
        {
            cmd5HousingParse.Enabled = false;

            progressBarHousing.Style = ProgressBarStyle.Marquee;
            progressBarHousing.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                HouseSQLWriter.WriteHouseFiles(housingPortalsTable);
            });

            progressBarHousing.Style = ProgressBarStyle.Continuous;
            progressBarHousing.Value = 100;

            cmd5HousingParse.Enabled = true;
        }

        private async void cmd6LandblocksParse_Click(object sender, EventArgs e)
        {
            cmd6LandblocksParse.Enabled = false;

            progressBarLandblocks.Style = ProgressBarStyle.Marquee;
            progressBarLandblocks.Value = 0;

            await Task.Run(() =>
            {
                // Old method
                LandblockSQLWriter.WriteLandblockFiles(landBlockData, weenieNames);
            });

            progressBarLandblocks.Style = ProgressBarStyle.Continuous;
            progressBarLandblocks.Value = 100;

            cmd6LandblocksParse.Enabled = true;
        }

        private async void cmd8QuestsParse_Click(object sender, EventArgs e)
        {
            cmd8QuestsParse.Enabled = false;

            progressBarQuests.Style = ProgressBarStyle.Marquee;
            progressBarQuests.Value = 0;

            await Task.Run(() =>
            {
                var efQuests = new List<ACE.Database.Models.World.Quest>();

                foreach (var questDefs in questDefDB.QuestDefs)
                {
                    efQuests.Add(new ACE.Database.Models.World.Quest
                    {
                        Name = questDefs.Name,

                        MinDelta = questDefs.MinDelta,
                        MaxSolves = questDefs.MaxSolves,

                        Message = questDefs.Message
                    });
                }

                // todo do something with efQuests

                // Old method
                QuestSQLWriter.WriteQuestFiles(questDefDB);
            });

            progressBarQuests.Style = ProgressBarStyle.Continuous;
            progressBarQuests.Value = 100;

            cmd8QuestsParse.Enabled = true;
        }

        private async void cmd9WeeniesParse_Click(object sender, EventArgs e)
        {
            cmd9WeeniesParse.Enabled = false;

            progressBarWeenies.Style = ProgressBarStyle.Marquee;
            progressBarWeenies.Value = 0;

            await Task.Run(() =>
            {
                var efWeenies = new List<ACE.Database.Models.World.Weenie>();

                foreach (var weenie in weenieDefaults.Weenies)
                {
                    var efWeenie = new ACE.Database.Models.World.Weenie();

                    efWeenie.ClassId = weenie.Key;
                    efWeenie.ClassName = ((WeenieClasses)weenie.Value.WCID).GetNameFormattedForDatabase();
                    efWeenie.Type = weenie.Value.WeenieType;

                    if (weenie.Value.IntValues != null)
                    {
                        foreach (var value in weenie.Value.IntValues)
                            efWeenie.WeeniePropertiesInt.Add(new WeeniePropertiesInt { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.LongValues != null)
                    {
                        foreach (var value in weenie.Value.LongValues)
                            efWeenie.WeeniePropertiesInt64.Add(new WeeniePropertiesInt64 { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.BoolValues != null)
                    {
                        foreach (var value in weenie.Value.BoolValues)
                            efWeenie.WeeniePropertiesBool.Add(new WeeniePropertiesBool { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.DoubleValues != null)
                    {
                        foreach (var value in weenie.Value.DoubleValues)
                            efWeenie.WeeniePropertiesFloat.Add(new WeeniePropertiesFloat { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.StringValues != null)
                    {
                        foreach (var value in weenie.Value.StringValues)
                            efWeenie.WeeniePropertiesString.Add(new WeeniePropertiesString { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.DIDValues != null)
                    {
                        foreach (var value in weenie.Value.DIDValues)
                            efWeenie.WeeniePropertiesDID.Add(new WeeniePropertiesDID { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.PositionValues != null)
                    {
                        foreach (var value in weenie.Value.PositionValues)
                        {
                            efWeenie.WeeniePropertiesPosition.Add(new WeeniePropertiesPosition()
                            {
                                ObjCellId = value.Value.ObjCellID,
                                OriginX = value.Value.Origin.X,
                                OriginY = value.Value.Origin.Y,
                                OriginZ = value.Value.Origin.Z,
                                AnglesX = value.Value.Angles.X,
                                AnglesY = value.Value.Angles.Y,
                                AnglesZ = value.Value.Angles.Z,
                                AnglesW = value.Value.Angles.W,
                            });
                        }
                    }

                    if (weenie.Value.IIDValues != null)
                    {
                        foreach (var value in weenie.Value.IIDValues)
                            efWeenie.WeeniePropertiesIID.Add(new WeeniePropertiesIID { Type = (ushort)value.Key, Value = value.Value });
                    }

                    if (weenie.Value.Attributes != null)
                    {
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Strength, InitLevel = (uint)weenie.Value.Attributes.Strength.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Strength.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Strength.CPSpent });
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Endurance, InitLevel = (uint)weenie.Value.Attributes.Endurance.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Endurance.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Endurance.CPSpent });
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Quickness, InitLevel = (uint)weenie.Value.Attributes.Quickness.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Quickness.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Quickness.CPSpent });
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Coordination, InitLevel = (uint)weenie.Value.Attributes.Coordination.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Coordination.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Coordination.CPSpent });
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Focus, InitLevel = (uint)weenie.Value.Attributes.Focus.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Focus.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Focus.CPSpent });
                        efWeenie.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Self, InitLevel = (uint)weenie.Value.Attributes.Self.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Self.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Self.CPSpent });

                        efWeenie.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxHealth, InitLevel = (uint)weenie.Value.Attributes.Health.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Health.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Health.CPSpent });
                        efWeenie.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxStamina, InitLevel = (uint)weenie.Value.Attributes.Stamina.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Stamina.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Stamina.CPSpent });
                        efWeenie.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxMana, InitLevel = (uint)weenie.Value.Attributes.Mana.InitLevel, LevelFromCP = (uint)weenie.Value.Attributes.Mana.LevelFromCP, CPSpent = (uint)weenie.Value.Attributes.Mana.CPSpent });
                    }

                    if (weenie.Value.Skills != null)
                    {
                        foreach (var value in weenie.Value.Skills)
                            efWeenie.WeeniePropertiesSkill.Add(new WeeniePropertiesSkill { Type = (ushort)value.Key, LevelFromPP = value.Value.LevelFromPP, SAC = (uint)value.Value.Sac, PP = (uint)value.Value.PP, InitLevel = (uint)value.Value.InitLevel, ResistanceAtLastCheck = (uint)value.Value.ResistanceAtLastCheck, LastUsedTime = value.Value.LastUsedTime });
                    }

                    if (weenie.Value.BodyParts != null)
                    {
                        foreach (var value in weenie.Value.BodyParts)
                        {
                            efWeenie.WeeniePropertiesBodyPart.Add(new WeeniePropertiesBodyPart
                            {
                                Key = (ushort)value.Key,

                                DType = value.Value.DType,
                                DVal = value.Value.DVal,
                                DVar = value.Value.DVar,

                                BaseArmor = value.Value.ArmorValues.BaseArmor,
                                ArmorVsSlash = value.Value.ArmorValues.ArmorVsSlash,
                                ArmorVsPierce = value.Value.ArmorValues.ArmorVsPierce,
                                ArmorVsBludgeon = value.Value.ArmorValues.ArmorVsBludgeon,
                                ArmorVsCold = value.Value.ArmorValues.ArmorVsCold,
                                ArmorVsFire = value.Value.ArmorValues.ArmorVsFire,
                                ArmorVsAcid = value.Value.ArmorValues.ArmorVsAcid,
                                ArmorVsElectric = value.Value.ArmorValues.ArmorVsElectric,
                                ArmorVsNether = value.Value.ArmorValues.ArmorVsNether,

                                BH = value.Value.BH,

                                HLF = value.Value.SD.HLF,
                                MLF = value.Value.SD.MLF,
                                LLF = value.Value.SD.LLF,

                                HRF = value.Value.SD.HRF,
                                MRF = value.Value.SD.MRF,
                                LRF = value.Value.SD.LRF,

                                HLB = value.Value.SD.HLB,
                                MLB = value.Value.SD.MLB,
                                LLB = value.Value.SD.LLB,

                                HRB = value.Value.SD.HRB,
                                MRB = value.Value.SD.MRB,
                                LRB = value.Value.SD.LRB,
                            });
                        }
                    }

                    if (weenie.Value.SpellCastingProbability != null)
                    {
                        foreach (var value in weenie.Value.SpellCastingProbability)
                            efWeenie.WeeniePropertiesSpellBook.Add(new WeeniePropertiesSpellBook { Spell = value.Key, Probability = value.Value });
                    }

                    if (weenie.Value.EventFilters != null)
                    {
                        foreach (var value in weenie.Value.EventFilters)
                            efWeenie.WeeniePropertiesEventFilter.Add(new WeeniePropertiesEventFilter { Event = value });
                    }

                    if (weenie.Value.Emotes != null)
                    {
                        foreach (var kvp in weenie.Value.Emotes)
                        {
                            // kvp.key not used

                            foreach (var value in kvp.Value)
                            {
                                var efEmote = new WeeniePropertiesEmote
                                {
                                    Category = (uint)value.Category,
                                    Probability = value.Probability,

                                    WeenieClassId = value.ClassID,

                                    Style = value.Style,
                                    Substyle = value.Substyle,

                                    Quest = value.Quest,

                                    VendorType = value.VendorType,

                                    MinHealth = value.MinHealth,
                                    MaxHealth = value.MaxHealth
                                };

                                foreach (var action in value.EmoteActions)
                                {
                                    var efAction = new WeeniePropertiesEmoteAction
                                    {
                                        Type = (uint)action.Type,
                                        Delay = action.Delay,
                                        Extent = action.Extent,

                                        Motion = action.Motion,

                                        Message = action.Message,
                                        TestString = action.TestString,
                                        Min = action.Min,
                                        Max = action.Max,
                                        Min64 = action.Min64,
                                        Max64 = action.Max64,
                                        MinDbl = action.MinDbl,
                                        MaxDbl = action.MaxDbl,
                                        Stat = action.Stat,
                                        Display = action.Display,

                                        Amount = action.Amount,
                                        Amount64 = action.Amount64,
                                        HeroXP64 = action.HeroXP64,

                                        Percent = action.Percent,

                                        SpellId = action.SpellID,

                                        WealthRating = action.WealthRating,
                                        TreasureClass = action.TreasureClass,
                                        TreasureType = action.TreasureType,

                                        PScript = action.PScript,

                                        Sound = action.Sound
                                    };

                                    if (action.Item != null)
                                    {
                                        efAction.WeenieClassId = action.Item.WCID;
                                        efAction.Palette = action.Item.Palette;
                                        efAction.Shade = action.Item.Shade;
                                        efAction.DestinationType = (sbyte)action.Item.Destination;
                                        efAction.StackSize = action.Item.StackSize;
                                        efAction.TryToBond = action.Item.TryToBond;
                                    }

                                    if (action.Frame != null)
                                    {
                                        efAction.OriginX = action.Frame.Origin.X;
                                        efAction.OriginY = action.Frame.Origin.Y;
                                        efAction.OriginZ = action.Frame.Origin.Z;

                                        efAction.AnglesX = action.Frame.Angles.X;
                                        efAction.AnglesY = action.Frame.Angles.Y;
                                        efAction.AnglesZ = action.Frame.Angles.Z;
                                        efAction.AnglesW = action.Frame.Angles.W;
                                    }

                                    if (action.Position != null)
                                    {
                                        efAction.ObjCellId = action.Position.ObjCellID;

                                        efAction.OriginX = action.Position.Origin.X;
                                        efAction.OriginY = action.Position.Origin.Y;
                                        efAction.OriginZ = action.Position.Origin.Z;

                                        efAction.AnglesX = action.Position.Angles.X;
                                        efAction.AnglesY = action.Position.Angles.Y;
                                        efAction.AnglesZ = action.Position.Angles.Z;
                                        efAction.AnglesW = action.Position.Angles.W;
                                    }

                                    efEmote.WeeniePropertiesEmoteAction.Add(efAction);
                                }

                                efWeenie.WeeniePropertiesEmote.Add(efEmote);
                            }
                        }
                    }

                    if (weenie.Value.CreateList != null)
                    {
                        foreach (var value in weenie.Value.CreateList)
                            efWeenie.WeeniePropertiesCreateList.Add(new WeeniePropertiesCreateList { WeenieClassId = (uint)value.WCID, Palette = (sbyte)value.Palette, Shade = value.Shade, DestinationType = (sbyte)value.Destination, StackSize = value.StackSize, TryToBond = value.TryToBond });
                    }

                    if (weenie.Value.PagesData != null)
                    {
                        efWeenie.WeeniePropertiesBook = new WeeniePropertiesBook { MaxNumPages = weenie.Value.PagesData.MaxNumPages, MaxNumCharsPerPage = weenie.Value.PagesData.MaxNumCharsPerPage };

                        if (weenie.Value.PagesData.Pages != null)
                        {
                            foreach (var value in weenie.Value.PagesData.Pages)
                                efWeenie.WeeniePropertiesBookPageData.Add(new WeeniePropertiesBookPageData { AuthorId = value.AuthorID, AuthorName = value.AuthorName, AuthorAccount = value.AuthorAccount, IgnoreAuthor = value.IgnoreAuthor, PageText = value.Text });
                        }
                    }

                    if (weenie.Value.Generators != null)
                    {
                        foreach (var value in weenie.Value.Generators)
                        {
                            efWeenie.WeeniePropertiesGenerator.Add(new WeeniePropertiesGenerator
                            {
                                Probability = value.Probability,
                                WeenieClassId = (uint)value.Type,
                                Delay = (uint)value.Delay, // Can be null. Is there a default null value in the cache.bin?

                                InitCreate = (uint)value.InitCreate,
                                MaxCreate = (uint)value.MaxNum,

                                WhenCreate = (uint)value.WhenCreate,
                                WhereCreate = (uint)value.WhereCreate,

                                StackSize = value.StackSize, // Can be null. Is there a default null value in the cache.bin?

                                PaletteId = (uint)value.PalleteTypeID, // Can be null. Is there a default null value in the cache.bin?
                                Shade = value.Shade, // Can be null. Is there a default null value in the cache.bin?

                                // Can be null. Is there a default null value in the cache.bin?
                                ObjCellId = value.Position.ObjCellID,
                                OriginX = value.Position.Origin.X,
                                OriginY = value.Position.Origin.Y,
                                OriginZ = value.Position.Origin.Z,
                                AnglesX = value.Position.Angles.X,
                                AnglesY = value.Position.Angles.Y,
                                AnglesZ = value.Position.Angles.Z,
                                AnglesW = value.Position.Angles.W

                                // Slot
                            });
                        }
                    }


                    if (weenie.Value.Palette != null)
                    {
                        foreach (var value in weenie.Value.Palette.SubPalettes)
                            efWeenie.WeeniePropertiesPalette.Add(new WeeniePropertiesPalette { SubPaletteId = (uint)value.ID, Offset = value.Offset, Length = value.NumberOfColors });
                    }

                    if (weenie.Value.TextureMaps != null)
                    {
                        foreach (var value in weenie.Value.TextureMaps)
                            efWeenie.WeeniePropertiesTextureMap.Add(new WeeniePropertiesTextureMap { Index = value.Index, OldId = (uint)value.OldTextureID, NewId = (uint)value.NewTextureID });
                    }

                    if (weenie.Value.AnimParts != null)
                    {
                        foreach (var value in weenie.Value.AnimParts)
                            efWeenie.WeeniePropertiesAnimPart.Add(new WeeniePropertiesAnimPart { Index = value.Index, Id = (uint)value.ID });
                    }


                    efWeenies.Add(efWeenie);
                }

                // todo do something with efWeenies

                // Old method
                WeenieSQLWriter.WriteWeenieFiles(weenieDefaults, treasureTable, weenieNames);
            });

            progressBarWeenies.Style = ProgressBarStyle.Continuous;
            progressBarWeenies.Value = 100;

            cmd9WeeniesParse.Enabled = true;
        }

        private async void cmdBEventsParse_Click(object sender, EventArgs e)
        {
            cmdBEventsParse.Enabled = false;

            progressBarEvents.Style = ProgressBarStyle.Marquee;
            progressBarEvents.Value = 0;

            await Task.Run(() =>
            {
                var efEvents = new List<ACE.Database.Models.World.Event>();

                foreach (var gameEventDef in gameEventDefDB.GameEventDefs)
                {
                    efEvents.Add(new ACE.Database.Models.World.Event
                    {
                        Name = gameEventDef.Name,

                        StartTime = gameEventDef.StartTime,
                        EndTime = gameEventDef.EndTime,

                        State =  (int)gameEventDef.GameEventState
                    });
                }

                // todo do something with efEvents

                // Old method
                EventSQLWriter.WriteEventFiles(gameEventDefDB);
            });

            progressBarEvents.Style = ProgressBarStyle.Continuous;
            progressBarEvents.Value = 100;

            cmdBEventsParse.Enabled = true;
        }
    }
}
