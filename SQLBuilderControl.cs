using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using PhatACCacheBinParser.ACE_Helpers;
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
                    name = ((WeenieClasses)weenie.Value.WCID).GetNameFormattedForDatabase();

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
                var aceEncounters = regionDescExtendedData.ConvertToACE(landBlockData);
                RegionDescSQLWriter.WriteFiles(aceEncounters, Settings.Default["OutputFolder"] + "\\" + "1 RegionDescExtendedData" + "\\" + "\\SQL\\", weenieNames);
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
                var aceSpells = spellTableExtendedData.ConvertToACE();
                SpellsSQLWriter.WriteFiles(aceSpells, Settings.Default["OutputFolder"] + "\\" + "2 SpellTableExtendedData" + "\\" + "\\SQL\\", weenieNames);
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
                // New method
                // todo
                // todo

                // Old method
                TreasureSQLWriter.WriteFiles(treasureTable, weenieNames, Settings.Default["OutputFolder"] + "\\" + "3 TreasureTable" + "\\" + "\\SQL Old Method\\");
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
                // New method
                var aceRecipies = craftingTable.ConvertToACE();
                // todo

                // Old method
                CraftingSQLWriter.WriteFiles(craftingTable, weenieNames, Settings.Default["OutputFolder"] + "\\" + "4 CraftTable" + "\\" + "\\SQL Old Method\\");
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
                var aceHouePortals = housingPortalsTable.ConvertToACE();
                HouseSQLWriter.WriteFiles(aceHouePortals, Settings.Default["OutputFolder"] + "\\" + "5 HousingPortals" + "\\" + "\\SQL\\");
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
                // New method
                // todo
                // todo

                // Old method
                LandblockSQLWriter.WriteFiles(landBlockData, weenieNames, Settings.Default["OutputFolder"] + "\\" + "6 LandBlockExtendedData" + "\\" + "\\SQL Old Method\\");
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
                var aceQuests = questDefDB.ConvertToACE();
                QuestSQLWriter.WriteFiles(aceQuests, Settings.Default["OutputFolder"] + "\\" + "8 QuestDefDB" + "\\" + "\\SQL\\");
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
                // New method
                var aceWeenies = weenieDefaults.ConvertToACE();
                // todo

                // Old method
                WeenieSQLWriter.WriteFiles(weenieDefaults, treasureTable, weenieNames, Settings.Default["OutputFolder"] + "\\" + "9 WeenieDefaults" + "\\" + "\\SQL Old Method\\");
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
                var aceEvents = gameEventDefDB.ConvertToACE();
                EventSQLWriter.WriteFiles(aceEvents, Settings.Default["OutputFolder"] + "\\" + "B GameEventDefDB" + "\\" + "\\SQL\\");
            });

            progressBarEvents.Style = ProgressBarStyle.Continuous;
            progressBarEvents.Value = 100;

            cmdBEventsParse.Enabled = true;
        }
    }
}
