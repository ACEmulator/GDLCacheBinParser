using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using PhatACCacheBinParser.Properties;

namespace PhatACCacheBinParser
{
	public partial class ParserControl : UserControl
	{
	    public event Action<ParserControl> DoExportJSON;


        public ParserControl()
		{
			InitializeComponent();
		}


        private string propertyName;

		public string ProperyName
		{
			private get
			{
				return propertyName;
			}
			set
			{
				if (propertyName == value)
					return;

				propertyName = value;

				if (!String.IsNullOrEmpty(ProperyName))
				{
					var property = new SettingsProperty(Settings.Default.Properties["SourceBin"]);
					property.Name = ProperyName + "SourceBin";
					property.PropertyType = typeof(string);
					Settings.Default.Properties.Add(property);

					SourceBin = (string)Settings.Default[ProperyName + "SourceBin"];
				}
				else
				{
					SourceBin = null;
				}
			}
		}

		public string Label
		{
			get => label1.Text;
			set => label1.Text = value;
		}


		public string SourceBin
		{
			get => lblSourceBin.Text;
			set => lblSourceBin.Text = value;
		}


		public int ExportJSONProgress
		{
			get => progressExportJSON.Value;
			set
			{
				if (value == 0 || value == 100)
					progressExportJSON.Style = ProgressBarStyle.Continuous;
				else
					progressExportJSON.Style = ProgressBarStyle.Marquee;

				progressExportJSON.Value = value;
			}
		}


		private void cmdChooseSource_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				if (!String.IsNullOrEmpty(SourceBin) && File.Exists(SourceBin))
					dialog.InitialDirectory = Path.GetDirectoryName(SourceBin);

				SourceBin = null;

				dialog.Filter = "Raw Chunk Files (raw)|*.raw";

				if (dialog.ShowDialog() == DialogResult.OK)
					SourceBin = dialog.FileName;
			}

			Settings.Default[ProperyName + "SourceBin"] = SourceBin;
			Settings.Default.Save();
		}

		private void cmdExportJSON_Click(object sender, EventArgs e)
		{
			if (DoExportJSON != null)
				DoExportJSON(this);
		}
    }
}
