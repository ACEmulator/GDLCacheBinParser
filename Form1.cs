using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Microsoft.EntityFrameworkCore;
using PhatACCacheBinParser.ACE_Helpers;
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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();


            // Cache.bin

			lblOutputFolder.Text = (string)Settings.Default["OutputFolder"];

			parserControl1.ProperyName = "_1";
			parserControl1.Label = "1 RegionDescExtendedData";
			parserControl1.DoExportJSON += ParserControl1_ExportJSON;

            parserControl2.ProperyName = "_2";
			parserControl2.Label = "2 SpellTableExtendedData";
			parserControl2.DoExportJSON += ParserControl2_ExportJSON;

            parserControl3.ProperyName = "_3";
			parserControl3.Label = "3 TreasureTable";
			parserControl3.DoExportJSON += ParserControl3_ExportJSON;

            parserControl4.ProperyName = "_4";
			parserControl4.Label = "4 CraftTable";
			parserControl4.DoExportJSON += ParserControl4_ExportJSON;

            parserControl5.ProperyName = "_5";
			parserControl5.Label = "5 HousingPortals";
			parserControl5.DoExportJSON += ParserControl5_ExportJSON;

            parserControl6.ProperyName = "_6";
			parserControl6.Label = "6 LandBlockExtendedData";
			parserControl6.DoExportJSON += ParserControl6_ExportJSON;

            parserControl7.ProperyName = "_7";
			parserControl7.Label = "7 Jumpsuits";
			parserControl7.DoExportJSON += ParserControl7_ExportJSON;

            parserControl8.ProperyName = "_8";
			parserControl8.Label = "8 QuestDefDB";
			parserControl8.DoExportJSON += ParserControl8_ExportJSON;

            parserControl9.ProperyName = "_9";
			parserControl9.Label = "9 WeenieDefaults";
			parserControl9.DoExportJSON += ParserControl9_ExportJSON;

            parserControlA.ProperyName = "_A";
			parserControlA.Label = "A MutationFilters";
			parserControlA.DoExportJSON += ParserControlA_ExportJSON;

            parserControlB.ProperyName = "_B";
            parserControlB.Label = "B GameEventDefDB";
            parserControlB.DoExportJSON += ParserControlB_ExportJSON;


            // GDLE

		    lblGDLEJSONRootFolder.Text = Settings.Default.GDLEJSONRootFolder;
		    lblGDLESQLOutputFolder.Text = Settings.Default.GDLESQLOutputFolder;


            // ACE

		    txtACEWorldServer.Text = Settings.Default.ACEWorldServer;
		    txtACEWorldPort.Text = Settings.Default.ACEWorldPort.ToString();
            txtACEWorldUser.Text = Settings.Default.ACEWorldUser;
		    txtACEWorldPassword.Text = Settings.Default.ACEWorldPassword;
		    txtACEWorldDatabase.Text = Settings.Default.ACEWorldDatabase;
            chkHidePassword.Checked = Settings.Default.HideACEWorldPassword;
		}

        protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.Save();

			base.OnClosing(e);
		}


        // ====================================================================================
        // ==================================== Cache.bin =====================================
        // ====================================================================================

        private void cmdOutputFolder_Click(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.SelectedPath = lblOutputFolder.Text;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					lblOutputFolder.Text = dialog.SelectedPath;
					Settings.Default["OutputFolder"] = lblOutputFolder.Text;
					Settings.Default.Save();
				}
			}
		}

		private void ParserControl1_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<RegionDescExtendedData>(parserControl);
		}

		private void ParserControl2_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<SpellTableExtendedData>(parserControl);
		}

		private void ParserControl3_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<TreasureTable>(parserControl);
		}

		private void ParserControl4_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<CraftingTable>(parserControl);
		}

		private void ParserControl5_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<HousingPortalsTable>(parserControl);
		}

		private void ParserControl6_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<LandBlockData>(parserControl);
		}

		private void ParserControl7_ExportJSON(ParserControl parserControl)
		{
			MessageBox.Show("Not implemented.");
		}

		private void ParserControl8_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<QuestDefDB>(parserControl);
		}

		private void ParserControl9_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<WeenieDefaults>(parserControl);
		}

		private void ParserControlA_ExportJSON(ParserControl parserControl)
		{
			ParserControl_ExportJSON<MutationFilters>(parserControl);
		}

        private void ParserControlB_ExportJSON(ParserControl parserControl)
        {
            ParserControl_ExportJSON<GameEventDefDB>(parserControl);
        }

        private void ParserControl_ExportJSON<T>(ParserControl parserControl) where T : Segment, new()
		{
			parserControl.Enabled = false;

			parserControl.ExportJSONProgress = 0;

			if (!File.Exists(parserControl.SourceBin))
			{
				MessageBox.Show("Source bin path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				parserControl.Enabled = true;
				return;
			}

			if (!Directory.Exists(lblOutputFolder.Text))
			{
				MessageBox.Show("Output folder does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				parserControl.Enabled = true;
				return;
			}

			ThreadPool.QueueUserWorkItem(o =>
			{
				var data = File.ReadAllBytes(parserControl.SourceBin);

				// Parse the data
				using (var memoryStream = new MemoryStream(data))
				using (var binaryReader = new BinaryReader(memoryStream))
				{
					var outputFolder = lblOutputFolder.Text + "\\" + parserControl.Label + "\\" + "\\JSON\\";

					var segment = new T();

					parserControl.BeginInvoke((Action)(() => parserControl.ExportJSONProgress = 1));

					if (segment.Unpack(binaryReader))
					{
						parserControl.BeginInvoke((Action)(() => parserControl.ExportJSONProgress = 50));

                        // Export the parsed data
                        segment.WriteJSONOutput(outputFolder);
                        parserControl.BeginInvoke((Action)(() => parserControl.ExportJSONProgress = 100));
                    }
                }

                parserControl.BeginInvoke((Action)(() => parserControl.Enabled = true));
			});
		}


        // ====================================================================================
        // ======================================= GDLE =======================================
        // ====================================================================================

        private void cmdChooseJSONRootFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = lblGDLEJSONRootFolder.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    lblGDLEJSONRootFolder.Text = dialog.SelectedPath;
                    Settings.Default.GDLEJSONRootFolder = lblGDLEJSONRootFolder.Text;
                    Settings.Default.Save();
                }
            }
        }

        private void cmdChooseSQLOutputFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = lblGDLESQLOutputFolder.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    lblGDLESQLOutputFolder.Text = dialog.SelectedPath;
                    Settings.Default.GDLESQLOutputFolder = lblGDLESQLOutputFolder.Text;
                    Settings.Default.Save();
                }
            }
        }

        private void cmdParseGDLEJSONs_Click(object sender, EventArgs e)
        {
            cmdParseGDLEJSONs.Enabled = false;

            txtGDLEJSONParser.Text = null;

            try
            { 
                // Read in the GDLE jsons

                txtGDLEJSONParser.Text += "Loading events.json...";
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadEventsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "events.json"), out Globals.GDLE.Events))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Events.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                txtGDLEJSONParser.Text += "Loading quests.json...";
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadQuestsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "quests.json"), out Globals.GDLE.Quests))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Quests.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                // recipeprecursors.json

                txtGDLEJSONParser.Text += "Loading recipes.json...";
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadRecipesConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "recipes.json"), out Globals.GDLE.Recipes))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Recipes.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                // restrictedlandblocks.json

                txtGDLEJSONParser.Text += "Loading spells.json...";
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadSpellsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "spells.json"), out Globals.GDLE.Spells))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Spells.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                // treasureProfile.json

                txtGDLEJSONParser.Text += "Loading worldspawns.json...";
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadWorldSpawnsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "worldspawns.json"), out Globals.GDLE.Instances, out Globals.GDLE.Links, 1000))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Instances.Count} instances and {Globals.GDLE.Links.Count} links found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                txtGDLEJSONParser.Text += "Loading \\weenies\\*.json";
                if (ACE.Adapter.Lifestoned.LifestonedLoader.TryLoadWeeniesConvertedInParallel(Path.Combine(lblGDLEJSONRootFolder.Text, "weenies"), out Globals.GDLE.Weenies, chkGDLEApplyEnumShift.Checked))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Weenies.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                Globals.GDLE.IsLoaded = true;


                // Collect some meta data that we'll use to pretty up the SQL files

                Globals.GDLE.AddToWeenieNames();

                if (Globals.GDLE.Spells != null && Globals.GDLE.Spells.Count > 0)
                    cmdGDLE2SpellsParse.Enabled = true;
                if (Globals.GDLE.Quests != null && Globals.GDLE.Quests.Count > 0)
                    cmdGDLE8QuestsParse.Enabled = true;
                if (Globals.GDLE.Weenies != null && Globals.GDLE.Weenies.Count > 0)
                    cmdGDLE9WeeniesParse.Enabled = true;
                if (Globals.GDLE.Events != null && Globals.GDLE.Events.Count > 0)
                    cmdGDLEBEventsParse.Enabled = true;
                if (Globals.GDLE.Instances != null && Globals.GDLE.Instances.Count > 0)
                    cmdGDLE6LandblocksParse.Enabled = true;
            }
            catch (Exception ex)
            {
                txtGDLEJSONParser.Text += Environment.NewLine + ex;
            }


            cmdParseGDLEJSONs.Enabled = true;
        }

        private void cmdGDLE1RegionsParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdGDLE2SpellsParse_Click(object sender, EventArgs e)
        {
            cmdGDLE2SpellsParse.Enabled = false;

            foreach (var x in Globals.GDLE.Spells)
                x.LastModified = DateTime.UtcNow;

            SpellsSQLWriter.WriteFiles(Globals.GDLE.Spells, Settings.Default["GDLESQLOutputFolder"] + "\\2 SpellTableExtendedData\\SQL\\", Globals.WeenieNames);

            cmdGDLE2SpellsParse.Enabled = true;
        }

        private void cmdGDLE3TreasureParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdGDLE4CraftingParse_Click(object sender, EventArgs e)
        {
            cmdGDLE4CraftingParse.Enabled = false;

            //var aceCraftingTables = Globals.CacheBin.CraftingTable.ConvertToACE();
            //var aceCraftingTables = Globals.GDLE.Recipes;
            //CraftingSQLWriter.WriteFiles(aceCraftingTables, Globals.WeenieNames, Settings.Default["OutputFolder"] + "\\4 CraftTable\\SQL\\");

            cmdGDLE4CraftingParse.Enabled = true;
        }

        private void cmdGDLE5HousingParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdGDLE6LandblocksParse_Click(object sender, EventArgs e)
        {
            cmdGDLE6LandblocksParse.Enabled = false;

            var trimmedInstances = new List<ACE.Database.Models.World.LandblockInstance>();

            trimmedInstances = Globals.GDLE.Instances.ToList();

            var margin = 4f;

            foreach (var x in Globals.GDLE.Instances)
            {
                x.LastModified = DateTime.UtcNow;

                foreach (var y in x.LandblockInstanceLink)
                    y.LastModified = DateTime.UtcNow;

                var lbid = (x.ObjCellId & 0xFFFF0000) >> 16;

                var cachedLandblock = Globals.CacheBin.LandBlockData.Landblocks.Where(y => y.Key == lbid).FirstOrDefault();

                if (cachedLandblock != null)
                {
                    if (x.WeenieClassId == 27547 && lbid == 52885)
                        Console.WriteLine("found bindstone");

                    var test = cachedLandblock.Weenies
                        .Where(z => z.WCID == 27547)
                        .FirstOrDefault();

                    var foundInCache = cachedLandblock.Weenies
                        .Where(z => z.WCID == x.WeenieClassId
                                && Math.Abs(z.Position.Origin.X - x.OriginX) < margin
                                && Math.Abs(z.Position.Origin.Y - x.OriginY) < margin
                                && Math.Abs(z.Position.Origin.Z - x.OriginZ) < margin
                              )
                        .FirstOrDefault();

                    if (foundInCache != null)
                        trimmedInstances.Remove(x);
                }
            }

            LandblockSQLWriter.WriteFiles(trimmedInstances, Settings.Default["GDLESQLOutputFolder"] + "\\6 LandBlockExtendedData\\", Globals.WeenieNames, false);

            cmdGDLE6LandblocksParse.Enabled = true;
        }

        private void cmdGDLE8QuestsParse_Click(object sender, EventArgs e)
        {
            cmdGDLE8QuestsParse.Enabled = false;

            foreach (var x in Globals.GDLE.Quests)
                x.LastModified = DateTime.UtcNow;

            QuestSQLWriter.WriteFiles(Globals.GDLE.Quests, Settings.Default["GDLESQLOutputFolder"] + "\\8 QuestDefDB\\SQL\\", true);

            cmdGDLE8QuestsParse.Enabled = true;
        }

        private void cmdGDLE9WeeniesParse_Click(object sender, EventArgs e)
        {
            cmdGDLE9WeeniesParse.Enabled = false;

            foreach (var x in Globals.GDLE.Weenies)
                x.LastModified = DateTime.UtcNow;

            var aceTreasureWielded = Globals.CacheBin.TreasureTable.WieldedTreasure.ConvertToACE();
            var aceTreasureDeath = Globals.CacheBin.TreasureTable.DeathTreasure.ConvertToACE();

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

            WeenieSQLWriter.WriteFiles(Globals.GDLE.Weenies, Settings.Default["GDLESQLOutputFolder"] + "\\9 WeenieDefaults\\SQL\\", Globals.WeenieNames, treasureWielded, treasureDeath, Globals.GDLE.Weenies.ToDictionary(x => x.ClassId, x => x), true);

            cmdGDLE9WeeniesParse.Enabled = true;
        }

        private void cmdGDLEAMutationParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdGDLEBEventsParse_Click(object sender, EventArgs e)
        {
            cmdGDLEBEventsParse.Enabled = false;

            foreach (var x in Globals.GDLE.Events)
                x.LastModified = DateTime.UtcNow;

            EventSQLWriter.WriteFiles(Globals.GDLE.Events, Settings.Default["GDLESQLOutputFolder"] + "\\B GameEventDefDB\\SQL\\");

            cmdGDLEBEventsParse.Enabled = true;
        }


        // ====================================================================================
        // =================================== ACE Database ===================================
        // ====================================================================================

        private void txtACEWorldServer_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ACEWorldServer = txtACEWorldServer.Text;
            Settings.Default.Save();
        }

        private void txtACEWorldPort_TextChanged(object sender, EventArgs e)
        {
            if (ushort.TryParse(txtACEWorldPort.Text, out var port))
            {
                Settings.Default.ACEWorldPort = port;
                Settings.Default.Save();
            }
        }

        private void txtACEWorldUser_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ACEWorldUser = txtACEWorldUser.Text;
            Settings.Default.Save();
        }

        private void txtACEWorldPassword_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ACEWorldPassword = txtACEWorldPassword.Text;
            Settings.Default.Save();
        }

        private void txtACEWorldDatabase_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ACEWorldDatabase = txtACEWorldDatabase.Text;
            Settings.Default.Save();
        }

        private void cmdTestInitDatabaseConnection_Click(object sender, EventArgs e)
        {
            if (Globals.ACEDatabase.TryInitWorldDatabaseContext())
            {
                txtACEDatabaseConnector.Text += "Connection succeeded!" + Environment.NewLine;

                cmdACE1RegionsParse.Enabled = true;
                cmdACE2SpellsParse.Enabled = true;
                cmdACE3TreasureParse.Enabled = true;
                cmdACE4CraftingParse.Enabled = true;
                cmdACE5HousingParse.Enabled = true;
                cmdACE6LandblocksParse.Enabled = true;
                cmdACE8QuestsParse.Enabled = true;
                cmdACE9WeeniesParse.Enabled = true;
                cmdACEAMutationParse.Enabled = true;
                cmdACEBEventsParse.Enabled = true;
            }
            else
                txtACEDatabaseConnector.Text += "Connection failed." + Environment.NewLine;
        }

	    private void cmdACEDatabaseCacheAllWeenies_Click(object sender, EventArgs e)
	    {
	        if (Globals.ACEDatabase.WorldDbContext == null)
	        {
	            txtACEDatabaseConnector.Text += "You must Test/Init Database Connection first." + Environment.NewLine;
                return;
	        }

	        cmdACEDatabaseCacheAllWeenies.Enabled = false;

            txtACEDatabaseConnector.Text += "Caching all weenies in parallel. This may take several minutes and consume lots of CPU...";
	        txtACEDatabaseConnector.Refresh();
            Globals.ACEDatabase.ReCacheAllWeeniesInParallel();
	        txtACEDatabaseConnector.Text += $" completed. {Globals.ACEDatabase.WorldDatabase.GetWeenieCacheCount():N0} weenies cached." + Environment.NewLine;

            cmdACEDatabaseCacheAllWeenies.Enabled = true;
        }

        private void chkHidePassword_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.HideACEWorldPassword = chkHidePassword.Checked;
            Settings.Default.Save();
            txtACEWorldPassword.UseSystemPasswordChar = chkHidePassword.Checked;
        }

        private void cmdACE1RegionsParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE2SpellsParse_Click(object sender, EventArgs e)
        {
            cmdACE2SpellsParse.Enabled = false;

            var results = Globals.ACEDatabase.WorldDbContext.Spell
                    .AsNoTracking()
                    .ToList();

            SpellsSQLWriter.WriteFiles(results, Settings.Default["GDLESQLOutputFolder"] + "\\2 SpellTableExtendedData\\SQL\\", Globals.WeenieNames, true);

            cmdACE2SpellsParse.Enabled = true;
        }

        private void cmdACE3TreasureParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE4CraftingParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE5HousingParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE6LandblocksParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE8QuestsParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACE9WeeniesParse_Click(object sender, EventArgs e)
        {
            cmdACE9WeeniesParse.Enabled = false;

            Globals.ACEDatabase.ReCacheAllWeeniesInParallel();

            WeenieSQLWriter.WriteFiles(Globals.ACEDatabase.Weenies, Settings.Default["GDLESQLOutputFolder"] + "\\9 WeenieDefaults\\SQL\\", Globals.WeenieNames, null, null, Globals.ACEDatabase.Weenies.ToDictionary(x => x.ClassId, x => x), true);

            cmdACE9WeeniesParse.Enabled = true;
        }

        private void cmdACEAMutationParse_Click(object sender, EventArgs e)
        {

        }

        private void cmdACEBEventsParse_Click(object sender, EventArgs e)
        {

        }


        // ====================================================================================
        // =================================== Output Tools ===================================
        // ====================================================================================

        private void cmdOutputTool1_Click(object sender, EventArgs e)
        {
            cmdOutputTool1.Enabled = false;

            txtOutputTool1.Text = null;

            try
            {
                // Get all landblock guids currently in use in the database
                var landblockGuidsUsed = new List<uint>();

                if (Globals.ACEDatabase.WorldDbContext != null)
                {
                    var results = Globals.ACEDatabase.WorldDbContext.LandblockInstance
                        //.Include(r => r.LandblockInstanceLink) // UNCOMMENT THIS IF YOU WANT TO INCLUDE LINKS
                        .AsNoTracking()
                        .ToList();

                    foreach (var result in results)
                        landblockGuidsUsed.Add(result.Guid);
                }


                // Write out the SQL files

                //SpellsSQLWriter.WriteFiles(spells, lblGDLESQLOutputFolder.Text + "\\Database\\Spells\\", weenieNames);

                WeenieSQLWriter.WriteFiles(Globals.GDLE.Weenies, lblGDLESQLOutputFolder.Text + "\\Database\\Weenies\\", Globals.WeenieNames);

                // What about links??
                LandblockSQLWriter.WriteFiles(Globals.GDLE.Instances, lblGDLESQLOutputFolder.Text + "\\Database\\Landblock Instances\\", Globals.WeenieNames);

                // TODO
            }
            catch (Exception ex)
            {
                txtOutputTool1.Text += Environment.NewLine + ex;
            }


            cmdOutputTool1.Enabled = true;
        }
    }
}
