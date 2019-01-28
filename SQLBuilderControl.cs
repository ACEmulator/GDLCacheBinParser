using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using PhatACCacheBinParser.ACE_Helpers;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.SQLWriters;

namespace PhatACCacheBinParser
{
    public partial class SQLBuilderControl : UserControl
    {
        public SQLBuilderControl()
        {
            InitializeComponent();
        }


        private async void cmdParseAll_Click(object sender, EventArgs e)
        {
            cmdParseAll.Enabled = false;

            progressParseSources.Style = ProgressBarStyle.Marquee;

            // Read all the inputs here
            await Task.Run(delegate
            {
                // Read all the inputs here
                TryParseSegment((string)Settings.Default["_1SourceBin"], Globals.CacheBin.RegionDescExtendedData);
                TryParseSegment((string)Settings.Default["_2SourceBin"], Globals.CacheBin.SpellTableExtendedData);
                TryParseSegment((string)Settings.Default["_3SourceBin"], Globals.CacheBin.TreasureTable);
                TryParseSegment((string)Settings.Default["_4SourceBin"], Globals.CacheBin.CraftingTable);
                TryParseSegment((string)Settings.Default["_5SourceBin"], Globals.CacheBin.HousingPortalsTable);
                TryParseSegment((string)Settings.Default["_6SourceBin"], Globals.CacheBin.LandBlockData);
                // Segment 7
                TryParseSegment((string)Settings.Default["_8SourceBin"], Globals.CacheBin.QuestDefDB);
                TryParseSegment((string)Settings.Default["_9SourceBin"], Globals.CacheBin.WeenieDefaults);
                TryParseSegment((string)Settings.Default["_ASourceBin"], Globals.CacheBin.MutationFilters);
                TryParseSegment((string)Settings.Default["_BSourceBin"], Globals.CacheBin.GameEventDefDB);

                Globals.CacheBin.IsLoaded = true;

                Globals.CacheBin.AddToWeenieClsNames();
                Globals.CacheBin.AddToWeenieNames();
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

        private async void cmd1RegionsParse_Click(object sender, EventArgs e)
        {
            cmd1RegionsParse.Enabled = false;

            progressBarRegions.Style = ProgressBarStyle.Marquee;
            progressBarRegions.Value = 0;

            await Task.Run(() =>
            {
                var aceEncounters = Globals.CacheBin.RegionDescExtendedData.ConvertToACE(Globals.CacheBin.LandBlockData);
                RegionDescSQLWriter.WriteFiles(aceEncounters, Settings.Default["OutputFolder"] + "\\1 RegionDescExtendedData\\SQL\\", Globals.WeenieNames);
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
                var aceSpells = Globals.CacheBin.SpellTableExtendedData.ConvertToACE();
                SpellsSQLWriter.WriteFiles(aceSpells, Settings.Default["OutputFolder"] + "\\2 SpellTableExtendedData\\SQL\\", Globals.WeenieNames);
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
                var treasureDeath = Globals.CacheBin.TreasureTable.DeathTreasure.ConvertToACE();
                var treasureWielded = Globals.CacheBin.TreasureTable.WieldedTreasure.ConvertToACE();

                TreasureSQLWriter.WriteFiles(treasureDeath, Settings.Default["OutputFolder"] + "\\3 TreasureTable\\SQL\\Death\\");
                TreasureSQLWriter.WriteFiles(treasureWielded, Settings.Default["OutputFolder"] + "\\3 TreasureTable\\SQL\\Wielded\\", Globals.WeenieNames);
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
                var aceCraftingTables = Globals.CacheBin.CraftingTable.ConvertToACE();
                CraftingSQLWriter.WriteFiles(aceCraftingTables, Globals.WeenieNames, Settings.Default["OutputFolder"] + "\\4 CraftTable\\SQL\\");
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
                var aceHouePortals = Globals.CacheBin.HousingPortalsTable.ConvertToACE();
                HouseSQLWriter.WriteFiles(aceHouePortals, Settings.Default["OutputFolder"] + "\\5 HousingPortals\\SQL\\");
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
                var landblockInstances = Globals.CacheBin.LandBlockData.ConvertToACE();
                LandblockSQLWriter.WriteFiles(landblockInstances, Settings.Default["OutputFolder"] + "\\6 LandBlockExtendedData\\SQL\\", Globals.WeenieNames);
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
                var aceQuests = Globals.CacheBin.QuestDefDB.ConvertToACE();
                QuestSQLWriter.WriteFiles(aceQuests, Settings.Default["OutputFolder"] + "\\8 QuestDefDB\\SQL\\");
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
                // ClassId 30732 has -1 for an IID.. i think this was to make it so noone could wield
                var aceWeenies = Globals.CacheBin.WeenieDefaults.ConvertToACE();
                var aceTreasureWielded = Globals.CacheBin.TreasureTable.WieldedTreasure.ConvertToACE();
                var aceTreasureDeath = Globals.CacheBin.TreasureTable.DeathTreasure.ConvertToACE();
                var weenies = new Dictionary<uint, ACE.Database.Models.World.Weenie>();
                foreach (var item in aceWeenies)
                {
                    if (!weenies.ContainsKey(item.ClassId))
                        weenies.Add(item.ClassId, item);
                }
                var treasureWielded = new Dictionary<uint, List<ACE.Database.Models.World.TreasureWielded>>();
                foreach (var item in aceTreasureWielded)
                {
                    if (!treasureWielded.ContainsKey(item.TreasureType))
                        treasureWielded.Add(item.TreasureType, new List<ACE.Database.Models.World.TreasureWielded>());

                    treasureWielded[item.TreasureType].Add(item);
                }
                var treasureDeath = new Dictionary<uint, ACE.Database.Models.World.TreasureDeath>();
                foreach (var item in aceTreasureDeath)
                {
                    if (!treasureDeath.ContainsKey(item.TreasureType))
                        treasureDeath.Add(item.TreasureType, item);
                }
                WeenieSQLWriter.WriteFiles(aceWeenies, Settings.Default["OutputFolder"] + "\\9 WeenieDefaults\\SQL\\", Globals.WeenieNames, treasureWielded, treasureDeath, weenies);
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
                var aceEvents = Globals.CacheBin.GameEventDefDB.ConvertToACE();
                EventSQLWriter.WriteFiles(aceEvents, Settings.Default["OutputFolder"] + "\\B GameEventDefDB\\SQL\\");
            });

            progressBarEvents.Style = ProgressBarStyle.Continuous;
            progressBarEvents.Value = 100;

            cmdBEventsParse.Enabled = true;
        }
    }
}
