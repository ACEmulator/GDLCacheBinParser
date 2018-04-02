using System;
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

namespace PhatACCacheBinParser
{
	public partial class SQLBuilderControl : UserControl
	{
		public SQLBuilderControl()
		{
			InitializeComponent();
		}


		private readonly RegionDescExtendedData RegionDescExtendedData = new RegionDescExtendedData();
		private readonly SpellTableExtendedData SpellTableExtendedData = new SpellTableExtendedData();
		private readonly TreasureTable TreasureTable = new TreasureTable();
		private readonly CraftingTable CraftingTable = new CraftingTable();
		private readonly HousingPortalsTable HousingPortalsTable = new HousingPortalsTable();
		private readonly LandBlockData LandBlockData = new LandBlockData();
		// Segment 7
		private readonly QuestDefDB QuestDefDB = new QuestDefDB();
		private readonly WeenieDefaults WeenieDefaults = new WeenieDefaults();
		private readonly MutationFilters UnknownATables = new MutationFilters();
		// Segment B


		private void cmdParseAll_Click(object sender, EventArgs e)
		{
			cmdParseAll.Enabled = false;

			progressParseSources.Style = ProgressBarStyle.Marquee;

			ThreadPool.QueueUserWorkItem(o =>
			{
				// Read all the inputs here
				TryParseSegment((string) Settings.Default["_1SourceBin"], RegionDescExtendedData);
				TryParseSegment((string) Settings.Default["_3SourceBin"], SpellTableExtendedData);
				TryParseSegment((string) Settings.Default["_4SourceBin"], TreasureTable);
				TryParseSegment((string) Settings.Default["_5SourceBin"], CraftingTable);
				TryParseSegment((string) Settings.Default["_6SourceBin"], HousingPortalsTable);
				TryParseSegment((string) Settings.Default["_7SourceBin"], LandBlockData);
				// Segment 7
				TryParseSegment((string) Settings.Default["_8SourceBin"], QuestDefDB);
				TryParseSegment((string) Settings.Default["_9SourceBin"], WeenieDefaults);
				TryParseSegment((string) Settings.Default["_ASourceBin"], UnknownATables);
				// Segment B

				BeginInvoke((Action)(() =>
				{
					progressParseSources.Style = ProgressBarStyle.Continuous;
					progressParseSources.Value = 100;

					// Now that we've parsed all of our input segments, we can enable outputs
					foreach (Control control in Controls)
					{
						if (control is Button && control != sender)
							control.Enabled = true;
					}
				}));
			});
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
			// ReSharper disable once EmptyGeneralCatchClause
			catch { }

			return false;
		}


		private void cmdAction1_Click(object sender, EventArgs e)
		{
			cmdAction1.Enabled = false;

			progressParseSources.Style = ProgressBarStyle.Continuous;
			progressParseSources.Value = 0;

			progressBar1.Style = ProgressBarStyle.Marquee;

			ThreadPool.QueueUserWorkItem(o =>
			{
				// Do some output thing here

				// For example
				//foreach (var weenie in WeenieDefaults.Weenies)
				//	;

				BeginInvoke((Action)(() =>
				{
					progressBar1.Style = ProgressBarStyle.Continuous;
					progressBar1.Value = 100;

					cmdAction1.Enabled = true;
				}));
			});
		}
	}
}
