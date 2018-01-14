using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Seg1_RegionDesc;
using PhatACCacheBinParser.Seg2_Spells;
using PhatACCacheBinParser.Seg3_TreasureTables;
using PhatACCacheBinParser.Seg4_Crafting;
using PhatACCacheBinParser.Seg5_HousingPortals;
using PhatACCacheBinParser.Seg6_WorldSpawns;
using PhatACCacheBinParser.Seg8_TriggersActionsEvents;
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
			parserControl1.Label = "1 Region Desc";
			parserControl1.DoParse += ParserControl1_DoParse;

			parserControl2.ProperyName = "_2";
			parserControl2.Label = "2 Spells";
			parserControl2.DoParse += ParserControl2_DoParse;

			parserControl3.ProperyName = "_3";
			parserControl3.Label = "3 Treasure Tables";
			parserControl3.DoParse += ParserControl3_DoParse;

			parserControl4.ProperyName = "_4";
			parserControl4.Label = "4 Crafting";
			parserControl4.DoParse += ParserControl4_DoParse;

			parserControl5.ProperyName = "_5";
			parserControl5.Label = "5 Housing Portals";
			parserControl5.DoParse += ParserControl5_DoParse;

			parserControl6.ProperyName = "_6";
			parserControl6.Label = "6 World Spawns";
			parserControl6.DoParse += ParserControl6_DoParse;

			parserControl7.ProperyName = "_7";
			parserControl7.Label = "7 Jumpsuits";
			parserControl7.DoParse += ParserControl7_DoParse;

			parserControl8.ProperyName = "_8";
			parserControl8.Label = "8 Triggers-Actions-Events";
			parserControl8.DoParse += ParserControl8_DoParse;

			parserControl9.ProperyName = "_9";
			parserControl9.Label = "9 Weenies";
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
			string FileNameFormatter(RegionDesc obj) => "Data";

			ParserControl_DoParse(parserControl, (Func<RegionDesc, string>)FileNameFormatter);
		}

		private void ParserControl2_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Spell obj) => obj.ID.ToString("0000") + " " + obj.Name;

			ParserControl_DoParse(parserControl, (Func<Spell, string>)FileNameFormatter);
		}

		private void ParserControl3_DoParse(ParserControl parserControl)
		{
		    string FileNameFormatter(Unknown3_1 obj) => obj.Key.ToString("000") + " Type 3_1_4";

		    ParserControl_DoParse(parserControl, (Func<Unknown3_1, string>)FileNameFormatter);
        }

		private void ParserControl4_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Recipe obj) => obj.ID.ToString("00000") + " " + obj.GetFriendlyFileName();

			ParserControl_DoParse(parserControl, (Func<Recipe, string>)FileNameFormatter);
		}

		private void ParserControl5_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(HousingPortal obj) => obj.Unknown1.ToString("X4");

			ParserControl_DoParse(parserControl, (Func<HousingPortal, string>)FileNameFormatter);
		}

		private void ParserControl6_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Landblock obj) => obj.Key.ToString("X4");

			ParserControl_DoParse(parserControl, (Func<Landblock, string>)FileNameFormatter);
		}

		private void ParserControl7_DoParse(ParserControl parserControl)
		{
			MessageBox.Show("Not implemented.");
		}

		private void ParserControl8_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(TriggerActionEvent obj) => Util.IllegalInFileName.Replace(obj.Name, "_");

			ParserControl_DoParse(parserControl, (Func<TriggerActionEvent, string>)FileNameFormatter);
		}

		private void ParserControl9_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Seg9_Weenies.Weenie obj) => obj.WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Label, "_");

			ParserControl_DoParse(parserControl, (Func<Seg9_Weenies.Weenie, string>)FileNameFormatter);
		}

		private void ParserControlA_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(UnknownA obj) => obj.Index.ToString("00") + " " + obj.unknown_0_2.ToString("X4");

			ParserControl_DoParse(parserControl, (Func<UnknownA, string>)FileNameFormatter);
		}

		private void ParserControl_DoParse<T>(ParserControl parserControl, Func<T, string> fileNameFormatter) where T : IParseableObject, new()
		{
			parserControl.Enabled = false;

			parserControl.ParseInputProgress = 0;
			parserControl.WriteJSONOutputProgress = 0;

			if (!File.Exists(parserControl.SourceBin))
			{
				MessageBox.Show("Source bin path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Directory.Exists(lblOutputFolder.Text))
			{
				MessageBox.Show("Output folder does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    // ==============================================================================
                    // Normal Cases Below.. This handles the first (or only) data type of the segment
                    // ==============================================================================
                    {
                        var parsedObjects = Util.GetParsedObjects<T>(parserControl, binaryReader);

                        // Write out the parsed data
                        if (parserControl.WriteJSON)
	                        Util.WriteJSONOutput(parserControl, parsedObjects, outputFolder, fileNameFormatter);
                    }


                    // =====================================================================================
                    // Special Cases Below.. these segments have additional data chunks in different formats
                    // =====================================================================================

                    if (typeof(T).IsAssignableFrom(typeof(RegionDesc)))
				    {
				        // There's extra data here to parse
				    }

                    if (typeof(T).IsAssignableFrom(typeof(Unknown3_1)))
				    {
				        {
				            var parsedObjects = Util.GetParsedObjects<Unknown3_2>(parserControl, binaryReader);

				            string FileNameFormatter2(Unknown3_2 obj) => obj.Key.ToString("000") + " Type 3_2_2";

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
					            Util.WriteJSONOutput(parserControl, parsedObjects, outputFolder, FileNameFormatter2);
				        }

				        {
				            //var parsedObjects = new List<List<Unknown3_3>>();

				            for (int i = 0; i < 4; i++)
				            {
				                var parsedObject = Util.GetParsedObjects<Unknown3_3>(parserControl, binaryReader);
                                //parsedObjects.Add(parsedObject);

				                var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\01\\i=" + i.ToString("00") + "\\";

				                string FileNameFormatter3(Unknown3_3 obj) => obj.Key.ToString();

				                // Write out the parsed data
				                if (parserControl.WriteJSON)
					                Util.WriteJSONOutput(parserControl, parsedObject, outputFolderJSON2, FileNameFormatter3);
                            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 03\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_3> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_3>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
				        }

				        {
				            //var parsedObjects = new List<List<Unknown3_3_3>>();

				            for (int i = 0; i < 48; i++)
				            {
				                //var items = new List<Unknown3_3_3>();

				                for (int j = 0; j < 6; j++)
				                {
				                    var item = new Unknown3_3_3();
                                    item.Parse(binaryReader);
				                    //items.Add(item);

				                    var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\02\\i=" + i.ToString("00") + "\\j=" + j.ToString("00") + "\\";

                                    string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString("00000");

				                    // Write out the parsed data
				                    if (parserControl.WriteJSON)
					                    Util.WriteJSONOutput(parserControl, item.Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);
                                }

				                //parsedObjects.Add(items);
				            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 04\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_3_3> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_3_3>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
                        }

				        {
				            var parsedObjects = Util.GetParsedObjects<Unknown3_3>(parserControl, binaryReader);

                            for (int i = 0 ; i < parsedObjects.Count ; i++)
                            {
				                var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\03\\Key=" + parsedObjects[i].Key + "\\";

                                string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString("00000");

                                // Write out the parsed data
                                if (parserControl.WriteJSON)
	                                Util.WriteJSONOutput(parserControl, parsedObjects[i].Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);
                            }
                        }

                        {
				            //var parsedObjects = new List<List<Unknown3_3_3>>();

				            for (int i = 0; i < 4; i++)
				            {
				                //var items = new List<Unknown3_3_3>();

				                for (int j = 0; j < 6; j++)
				                {
				                    var item = new Unknown3_3_3();
				                    item.Parse(binaryReader);
                                    //items.Add(item);

				                    var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\04\\i=" + i.ToString("00") + "\\j=" + j.ToString("00") + "\\";

				                    string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString();

				                    // Write out the parsed data
				                    if (parserControl.WriteJSON)
					                    Util.WriteJSONOutput(parserControl, item.Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);}

				                //parsedObjects.Add(items);
				            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 06\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_3_3> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_3_3>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
                        }

				        {
				            //var parsedObjects = new List<List<Unknown3_5_6>>();

				            for (int i = 0; i < 4; i++)
				            {
				                var parsedObject = Util.GetParsedObjects<Unknown3_5_6>(parserControl, binaryReader);
                                //parsedObjects.Add(parsedObject);

				                var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\05\\i=" + i.ToString("00") + "\\";

                                string FileNameFormatter3(Unknown3_5_6 obj) => obj.Key.ToString();

				                // Write out the parsed data
				                if (parserControl.WriteJSON)
					                Util.WriteJSONOutput(parserControl, parsedObject, outputFolderJSON2, FileNameFormatter3);
                            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 07\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_5_6> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_5_6>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
				        }

				        {
				            //var parsedObjects = new List<List<Unknown3_3_3>>();

				            for (int i = 0; i < 1; i++)
				            {
				                //var items = new List<Unknown3_3_3>();

				                for (int j = 0; j < 6; j++)
				                {
				                    var item = new Unknown3_3_3();
				                    item.Parse(binaryReader);
                                    //items.Add(item);

				                    var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\06\\i=" + i.ToString("00") + "\\j=" + j.ToString("00") + "\\";

				                    string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString();

				                    // Write out the parsed data
				                    if (parserControl.WriteJSON)
					                    Util.WriteJSONOutput(parserControl, item.Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);
                                }

				                //parsedObjects.Add(items);
				            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 08\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_3_3> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_3_3>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
                        }

				        {
				            var parsedObjects = Util.GetParsedObjects<Unknown3_3_3>(parserControl, binaryReader);

				            for (int i = 0; i < parsedObjects.Count; i++)
                            {
                                var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\07\\i=" + i.ToString("00") + "\\";

                                string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString("00000");

                                // Write out the parsed data
                                if (parserControl.WriteJSON)
	                                Util.WriteJSONOutput(parserControl, parsedObjects[i].Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);
                            }

                            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 09\\" + "\\JSON\\";

				            string FileNameFormatter3(Unknown3_3_3 obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<Unknown3_3_3>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
                        }

                        {
				            //var parsedObjects = new List<List<Unknown3_3_3>>();

				            for (int i = 0; i < 4; i++)
				            {
				                //var items = new List<Unknown3_3_3>();

				                for (int j = 0; j < 6; j++)
				                {
				                    var item = new Unknown3_3_3();
				                    item.Parse(binaryReader);
                                    //items.Add(item);

				                    var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " extra\\" + "\\JSON\\08\\i=" + i.ToString("00") + "\\j=" + j.ToString("00") + "\\";

				                    string FileNameFormatter3(Unknown3_3_4 obj) => obj.WCID_SometimesNotAlways.ToString();

				                    // Write out the parsed data
				                    if (parserControl.WriteJSON)
					                    Util.WriteJSONOutput(parserControl, item.Unknown3_3_4, outputFolderJSON2, FileNameFormatter3);
                                }

				                //parsedObjects.Add(items);
				            }

				            /*var outputFolderJSON2 = lblOutputFolder.Text + "\\" + parserControl.Label + " 10\\" + "\\JSON\\";

				            string FileNameFormatter3(List<Unknown3_3_3> obj) => parsedObjects.IndexOf(obj).ToString();

				            // Write out the parsed data
				            if (parserControl.WriteJSON)
				                WriteJSONOutput<List<Unknown3_3_3>>(parserControl, parsedObjects, outputFolderJSON2, FileNameFormatter3);*/
				        }




                        var pos = binaryReader.BaseStream.Position;
				        binaryReader.ReadUInt32();
				    }

					if (typeof(T).IsAssignableFrom(typeof(Recipe)))
				    {
                        // The recipe heap seems to have a map after the main recipe heap. Precursor maybe?
				        var totalObjects = binaryReader.ReadUInt16();
				        binaryReader.ReadUInt16(); // Discard

                        for (int i = 0; i < totalObjects; i++)
				        {
				            binaryReader.ReadUInt64(); // key       Even though this is 64 bits, it appears to be made up of 2 uint32 values
				            binaryReader.ReadUInt32(); // value
				        }
				    }

                    if (typeof(T).IsAssignableFrom(typeof(Landblock)))
				    {
                        // This data structure stores a dword for every vertex (point) in each landcell. Are vertexes that border landcells duplicated?
                        // There are 255 x 255 landblocks.
				        // Each landblock has a 9 * 9 group of WORDS (2 byte values). It's 9x9 because the landcell is 8 cells by 8 cells (takes 9 points in each direction to define that, hence 9 vertices)
                        // Format is x by y I think
						// This data is referenced by segment 1
                        var terrainData = binaryReader.ReadBytes((255 * 255) * (9 * 9) * 2);
				    }
				}


				parserControl.BeginInvoke((Action)(() => parserControl.Enabled = true));
			});
		}
    }
}
