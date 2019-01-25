using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Microsoft.EntityFrameworkCore;

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
                if (ACE.Adapter.GDLE.GDLELoader.TryLoadWorldSpawnsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "worldspawns.json"), out Globals.GDLE.Instances, out Globals.GDLE.Links))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Instances.Count} instances and {Globals.GDLE.Links.Count} links found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                txtGDLEJSONParser.Text += "Loading \\weenies\\*.json";
                if (ACE.Adapter.Lifestoned.LifestonedLoader.TryLoadWeeniesConvertedInParallel(Path.Combine(lblGDLEJSONRootFolder.Text, "weenies"), out Globals.GDLE.Weenies))
                    txtGDLEJSONParser.Text += $" completed. {Globals.GDLE.Weenies.Count} entries found." + Environment.NewLine;
                else
                    txtGDLEJSONParser.Text += " failed." + Environment.NewLine;

                Globals.GDLE.IsLoaded = true;


                // Collect some meta data that we'll use to pretty up the SQL files

                Globals.GDLE.AddToWeenieNames();
            }
            catch (Exception ex)
            {
                txtGDLEJSONParser.Text += Environment.NewLine + ex;
            }


            cmdParseGDLEJSONs.Enabled = true;
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
                txtACEDatabaseConnector.Text += "Connection succeeded!" + Environment.NewLine;
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
