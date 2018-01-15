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
using PhatACCacheBinParser.SegA;

namespace PhatACCacheBinParser
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			lblOutputFolder.Text = (string)Settings.Default["OutputFolder"];

			parserControl1.ProperyName = "_1";
			parserControl1.Label = "1 RegionDescExtendedData";
			parserControl1.DoParse += ParserControl1_DoParse;

			parserControl2.ProperyName = "_2";
			parserControl2.Label = "2 SpellTableExtendedData";
			parserControl2.DoParse += ParserControl2_DoParse;

			parserControl3.ProperyName = "_3";
			parserControl3.Label = "3 TreasureTable";
			parserControl3.DoParse += ParserControl3_DoParse;

			parserControl4.ProperyName = "_4";
			parserControl4.Label = "4 CraftTable";
			parserControl4.DoParse += ParserControl4_DoParse;

			parserControl5.ProperyName = "_5";
			parserControl5.Label = "5 Housing Portals";
			parserControl5.DoParse += ParserControl5_DoParse;

			parserControl6.ProperyName = "_6";
			parserControl6.Label = "6 LandBlockExtendedData";
			parserControl6.DoParse += ParserControl6_DoParse;

			parserControl7.ProperyName = "_7";
			parserControl7.Label = "7 Jumpsuits";
			parserControl7.DoParse += ParserControl7_DoParse;

			parserControl8.ProperyName = "_8";
			parserControl8.Label = "8 QuestDefDB";
			parserControl8.DoParse += ParserControl8_DoParse;

			parserControl9.ProperyName = "_9";
			parserControl9.Label = "9 WeenieDefaults";
			parserControl9.DoParse += ParserControl9_DoParse;

			parserControlA.ProperyName = "_A";
			parserControlA.Label = "A Unknown";
			parserControlA.DoParse += ParserControlA_DoParse;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.Save();

			base.OnClosing(e);
		}


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


		private void ParserControl1_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<RegionDescExtendedData>(parserControl);
		}

		private void ParserControl2_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<SpellTableExtendedData>(parserControl);
		}

		private void ParserControl3_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<TreasureTable>(parserControl);
		}

		private void ParserControl4_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<CraftingTable>(parserControl);
		}

		private void ParserControl5_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<HousingPortalsTable>(parserControl);
		}

		private void ParserControl6_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<LandBlockData>(parserControl);
		}

		private void ParserControl7_DoParse(ParserControl parserControl)
		{
			MessageBox.Show("Not implemented.");
		}

		private void ParserControl8_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<QuestDefDB>(parserControl);
		}

		private void ParserControl9_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<WeenieDefaults>(parserControl);
		}

		private void ParserControlA_DoParse(ParserControl parserControl)
		{
			ParserControl_DoParse<UnknownATables>(parserControl);
		}

		private void ParserControl_DoParse<T>(ParserControl parserControl) where T : Segment, new()
		{
			parserControl.Enabled = false;

			parserControl.ParseInputProgress = 0;
			parserControl.WriteJSONOutputProgress = 0;

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

					parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = 1));

					if (segment.Parse(binaryReader))
					{
						parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = 100));

						// Write out the parsed data
						if (parserControl.WriteJSON)
						{
							parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = 1));

							segment.WriteJSONOutput(outputFolder);

							parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = 100));
						}
					}
				}

				parserControl.BeginInvoke((Action)(() => parserControl.Enabled = true));
			});
		}
	}
}
