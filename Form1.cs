using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using PhatACCacheBinParser.UnknownsA;
using PhatACCacheBinParser.Crafting;
using PhatACCacheBinParser.Properties;
using PhatACCacheBinParser.Spells;
using PhatACCacheBinParser.TriggersActionsEvents;
using PhatACCacheBinParser.Unknowns1;
using PhatACCacheBinParser.WorldSpawns;

namespace PhatACCacheBinParser
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			lblOutputFolder.Text = (string)Settings.Default["OutputFolder"];

			parserControl1.ProperyName = "_1";
			parserControl1.Label = "1 Unknown";
			parserControl1.DoParse += ParserControl1_DoParse;

			parserControl2.ProperyName = "_2";
			parserControl2.Label = "2 Spells";
			parserControl2.DoParse += ParserControl2_DoParse;

			parserControl3.ProperyName = "_3";
			parserControl3.Label = "3 Unknown";
			parserControl3.DoParse += ParserControl3_DoParse;

			parserControl4.ProperyName = "_4";
			parserControl4.Label = "4 Crafting";
			parserControl4.DoParse += ParserControl4_DoParse;

			parserControl5.ProperyName = "_5";
			parserControl5.Label = "5 Unknown";
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
			string FileNameFormatter(Unknown1 obj) => "Data";

			ParserControl_DoParse(parserControl, (Func<Unknown1, string>)FileNameFormatter);
		}

		private void ParserControl2_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Spell obj) => obj.ID.ToString("0000") + " " + obj.Name;

			ParserControl_DoParse(parserControl, (Func<Spell, string>)FileNameFormatter);
		}

		private void ParserControl3_DoParse(ParserControl parserControl)
		{
			MessageBox.Show("Not implemented.");
		}

		private void ParserControl4_DoParse(ParserControl parserControl)
		{
			string FileNameFormatter(Recipe obj) => obj.ID.ToString("00000") + " " + obj.GetFriendlyFileName();

			ParserControl_DoParse(parserControl, (Func<Recipe, string>)FileNameFormatter);
		}

		private void ParserControl5_DoParse(ParserControl parserControl)
		{
			MessageBox.Show("Not implemented.");
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
			string FileNameFormatter(Weenies.Weenie obj) => obj.WCID.ToString("00000") + " " + Util.IllegalInFileName.Replace(obj.Label, "_");

			ParserControl_DoParse(parserControl, (Func<Weenies.Weenie, string>)FileNameFormatter);
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

				var parsedObjects = new List<T>();


				// Parse the data
				using (var memoryStream = new MemoryStream(data))
				using (var binaryReader = new BinaryReader(memoryStream))
				{
					var totalObjects = binaryReader.ReadUInt16();
					binaryReader.ReadUInt16(); // Discard

					// For Segment 1, the first dword appears to simply be an is present flag
					if (typeof(T).IsAssignableFrom(typeof(Unknown1)) && totalObjects > 0)
						totalObjects = 1;

					// ReSharper disable once NotAccessedVariable
					T lastParsed; // Used for debugging

					//while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
					while (parsedObjects.Count < totalObjects)
					{
						var parsedObject = new T();

						parsedObject.Parse(binaryReader);

						parsedObjects.Add(parsedObject);

						// ReSharper disable once RedundantAssignment
						lastParsed = parsedObject; // Used for debugging

						if ((parsedObjects.Count % 100) == 0)
							parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = (int)(((double)parsedObjects.Count / totalObjects) * 100)));
					}

					parserControl.BeginInvoke((Action)(() => parserControl.ParseInputProgress = (int)(((double)parsedObjects.Count / totalObjects) * 100)));
				}


				// Write out the parsed data
				if (parserControl.WriteJSON)
				{
					var outputFolder = lblOutputFolder.Text + "\\" + parserControl.Label + "\\" + "\\JSON\\";

					if (!Directory.Exists(outputFolder))
						Directory.CreateDirectory(outputFolder);

					JsonSerializer serializer = new JsonSerializer();
					serializer.Converters.Add(new JavaScriptDateTimeConverter());
					serializer.NullValueHandling = NullValueHandling.Ignore;

					int processedCounter = 0;

					Parallel.For(0, parsedObjects.Count, i =>
					{
						using (StreamWriter sw = new StreamWriter(outputFolder + fileNameFormatter(parsedObjects[i]) + ".json"))
						using (JsonWriter writer = new JsonTextWriter(sw))
						{
							serializer.Serialize(writer, parsedObjects[i]);

							var counter = Interlocked.Increment(ref processedCounter);

							if ((counter % 1000) == 0)
								parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = (int)(((double)counter / parsedObjects.Count) * 100)));
						}
					});

					parserControl.BeginInvoke((Action)(() => parserControl.WriteJSONOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
				}

				if (parserControl.WriteSQL)
				{
					//var outputFolder = lblOutputFolder.Text + "\\" + parserControl.Label + "\\" + "\\SQL\\";

					//if (!Directory.Exists(outputFolder))
					//	Directory.CreateDirectory(outputFolder);

					// Do stuff here
					// Probably take a func as an argument to an SQL writer, or, add the SQL writer as a method of T

					//int processedCounter = 0;

					// more stuff here

					//parserControl.BeginInvoke((Action)(() => parserControl.WriteSQLOutputProgress = (int)(((double)processedCounter / parsedObjects.Count) * 100)));
				}


				parserControl.BeginInvoke((Action)(() => parserControl.Enabled = true));
			});
		}
	}
}
