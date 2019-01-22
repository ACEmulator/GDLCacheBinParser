using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

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

        private void cmdConvertGDLEJSONToACESQL_Click(object sender, EventArgs e)
        {
            cmdConvertGDLEJSONToACESQL.Enabled = false;

            txtGDLEParser.Text = null;

            // Read in the GDLE jsons

            txtGDLEParser.Text += "Loading events.json...";
            if (ACE.Adapter.GDLE.GDLELoader.TryLoadEventsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "events.json"), out var events))
                txtGDLEParser.Text += $" completed. {events.Count} entries found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            txtGDLEParser.Text += "Loading quests.json...";
            if (ACE.Adapter.GDLE.GDLELoader.TryLoadQuestsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "quests.json"), out var quests))
                txtGDLEParser.Text += $" completed. {quests.Count} entries found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            // recipeprecursors.json

            txtGDLEParser.Text += "Loading recipes.json...";
            if (ACE.Adapter.GDLE.GDLELoader.TryLoadRecipesConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "recipes.json"), out var recipes))
                txtGDLEParser.Text += $" completed. {recipes.Count} entries found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            // restrictedlandblocks.json

            txtGDLEParser.Text += "Loading spells.json...";
            if (ACE.Adapter.GDLE.GDLELoader.TryLoadSpellsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "spells.json"), out var spells))
                txtGDLEParser.Text += $" completed. {spells.Count} entries found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            // treasureProfile.json

            txtGDLEParser.Text += "Loading worldspawns.json...";
            if (ACE.Adapter.GDLE.GDLELoader.TryLoadWorldSpawnsConverted(Path.Combine(lblGDLEJSONRootFolder.Text, "worldspawns.json"), out var instances, out var links))
                txtGDLEParser.Text += $" completed. {instances.Count} instances and {links.Count} links found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            txtGDLEParser.Text += "Loading \\weenies\\*.json";
            if (ACE.Adapter.Lifestoned.LifestonedLoader.TryLoadWeeniesConvertedInParallel(Path.Combine(lblGDLEJSONRootFolder.Text, "weenies"), out var weenies))
                txtGDLEParser.Text += $" completed. {weenies.Count} entries found." + Environment.NewLine;
            else
                txtGDLEParser.Text += " failed." + Environment.NewLine;

            // Write out the SQL files

            // TODO

            cmdConvertGDLEJSONToACESQL.Enabled = true;
        }
    }
}
