namespace PhatACCacheBinParser
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdOutputFolder = new System.Windows.Forms.Button();
			this.lblOutputFolder = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.sqlBuilderControl1 = new PhatACCacheBinParser.SQLBuilderControl();
			this.parserControlB = new PhatACCacheBinParser.ParserControl();
			this.parserControl8 = new PhatACCacheBinParser.ParserControl();
			this.parserControl6 = new PhatACCacheBinParser.ParserControl();
			this.parserControl9 = new PhatACCacheBinParser.ParserControl();
			this.parserControl4 = new PhatACCacheBinParser.ParserControl();
			this.parserControl7 = new PhatACCacheBinParser.ParserControl();
			this.parserControl2 = new PhatACCacheBinParser.ParserControl();
			this.parserControl5 = new PhatACCacheBinParser.ParserControl();
			this.parserControlA = new PhatACCacheBinParser.ParserControl();
			this.parserControl3 = new PhatACCacheBinParser.ParserControl();
			this.parserControl1 = new PhatACCacheBinParser.ParserControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.lblGDLESQLOutputFolder = new System.Windows.Forms.Label();
			this.cmdChooseSQLOutputFolder = new System.Windows.Forms.Button();
			this.cmdChooseJSONRootFolder = new System.Windows.Forms.Button();
			this.lblGDLEJSONRootFolder = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.txtGDLEParser = new System.Windows.Forms.RichTextBox();
			this.cmdConvertGDLEJSONToACESQL = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdOutputFolder
			// 
			this.cmdOutputFolder.Location = new System.Drawing.Point(20, 20);
			this.cmdOutputFolder.Name = "cmdOutputFolder";
			this.cmdOutputFolder.Size = new System.Drawing.Size(119, 23);
			this.cmdOutputFolder.TabIndex = 22;
			this.cmdOutputFolder.Text = "Choose Output Folder";
			this.cmdOutputFolder.UseVisualStyleBackColor = true;
			this.cmdOutputFolder.Click += new System.EventHandler(this.cmdOutputFolder_Click);
			// 
			// lblOutputFolder
			// 
			this.lblOutputFolder.AutoSize = true;
			this.lblOutputFolder.Location = new System.Drawing.Point(145, 25);
			this.lblOutputFolder.Name = "lblOutputFolder";
			this.lblOutputFolder.Size = new System.Drawing.Size(35, 13);
			this.lblOutputFolder.TabIndex = 23;
			this.lblOutputFolder.Text = "label1";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1068, 1083);
			this.tabControl1.TabIndex = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.sqlBuilderControl1);
			this.tabPage1.Controls.Add(this.cmdOutputFolder);
			this.tabPage1.Controls.Add(this.parserControlB);
			this.tabPage1.Controls.Add(this.parserControl8);
			this.tabPage1.Controls.Add(this.parserControl6);
			this.tabPage1.Controls.Add(this.parserControl9);
			this.tabPage1.Controls.Add(this.lblOutputFolder);
			this.tabPage1.Controls.Add(this.parserControl4);
			this.tabPage1.Controls.Add(this.parserControl7);
			this.tabPage1.Controls.Add(this.parserControl2);
			this.tabPage1.Controls.Add(this.parserControl5);
			this.tabPage1.Controls.Add(this.parserControlA);
			this.tabPage1.Controls.Add(this.parserControl3);
			this.tabPage1.Controls.Add(this.parserControl1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1060, 1057);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Cache.bin Exporter";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(682, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(187, 13);
			this.label1.TabIndex = 27;
			this.label1.Text = "Export ACE formatted .SQL files";
			// 
			// sqlBuilderControl1
			// 
			this.sqlBuilderControl1.Location = new System.Drawing.Point(676, 49);
			this.sqlBuilderControl1.Name = "sqlBuilderControl1";
			this.sqlBuilderControl1.Size = new System.Drawing.Size(353, 430);
			this.sqlBuilderControl1.TabIndex = 26;
			// 
			// parserControlB
			// 
			this.parserControlB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControlB.ExportJSONProgress = 0;
			this.parserControlB.Label = "label1";
			this.parserControlB.Location = new System.Drawing.Point(20, 949);
			this.parserControlB.Name = "parserControlB";
			this.parserControlB.Size = new System.Drawing.Size(650, 84);
			this.parserControlB.SourceBin = "label1";
			this.parserControlB.TabIndex = 25;
			// 
			// parserControl8
			// 
			this.parserControl8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl8.ExportJSONProgress = 0;
			this.parserControl8.Label = "label1";
			this.parserControl8.Location = new System.Drawing.Point(20, 679);
			this.parserControl8.Name = "parserControl8";
			this.parserControl8.Size = new System.Drawing.Size(650, 84);
			this.parserControl8.SourceBin = "label1";
			this.parserControl8.TabIndex = 12;
			// 
			// parserControl6
			// 
			this.parserControl6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl6.ExportJSONProgress = 0;
			this.parserControl6.Label = "label1";
			this.parserControl6.Location = new System.Drawing.Point(20, 499);
			this.parserControl6.Name = "parserControl6";
			this.parserControl6.Size = new System.Drawing.Size(650, 84);
			this.parserControl6.SourceBin = "label1";
			this.parserControl6.TabIndex = 13;
			// 
			// parserControl9
			// 
			this.parserControl9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl9.ExportJSONProgress = 0;
			this.parserControl9.Label = "label1";
			this.parserControl9.Location = new System.Drawing.Point(20, 769);
			this.parserControl9.Name = "parserControl9";
			this.parserControl9.Size = new System.Drawing.Size(650, 84);
			this.parserControl9.SourceBin = "label1";
			this.parserControl9.TabIndex = 14;
			// 
			// parserControl4
			// 
			this.parserControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl4.ExportJSONProgress = 0;
			this.parserControl4.Label = "label1";
			this.parserControl4.Location = new System.Drawing.Point(20, 319);
			this.parserControl4.Name = "parserControl4";
			this.parserControl4.Size = new System.Drawing.Size(650, 84);
			this.parserControl4.SourceBin = "label1";
			this.parserControl4.TabIndex = 15;
			// 
			// parserControl7
			// 
			this.parserControl7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl7.ExportJSONProgress = 0;
			this.parserControl7.Label = "label1";
			this.parserControl7.Location = new System.Drawing.Point(20, 589);
			this.parserControl7.Name = "parserControl7";
			this.parserControl7.Size = new System.Drawing.Size(650, 84);
			this.parserControl7.SourceBin = "label1";
			this.parserControl7.TabIndex = 21;
			// 
			// parserControl2
			// 
			this.parserControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl2.ExportJSONProgress = 0;
			this.parserControl2.Label = "label1";
			this.parserControl2.Location = new System.Drawing.Point(20, 139);
			this.parserControl2.Name = "parserControl2";
			this.parserControl2.Size = new System.Drawing.Size(650, 84);
			this.parserControl2.SourceBin = "label1";
			this.parserControl2.TabIndex = 16;
			// 
			// parserControl5
			// 
			this.parserControl5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl5.ExportJSONProgress = 0;
			this.parserControl5.Label = "label1";
			this.parserControl5.Location = new System.Drawing.Point(20, 409);
			this.parserControl5.Name = "parserControl5";
			this.parserControl5.Size = new System.Drawing.Size(650, 84);
			this.parserControl5.SourceBin = "label1";
			this.parserControl5.TabIndex = 20;
			// 
			// parserControlA
			// 
			this.parserControlA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControlA.ExportJSONProgress = 0;
			this.parserControlA.Label = "label1";
			this.parserControlA.Location = new System.Drawing.Point(20, 859);
			this.parserControlA.Name = "parserControlA";
			this.parserControlA.Size = new System.Drawing.Size(650, 84);
			this.parserControlA.SourceBin = "label1";
			this.parserControlA.TabIndex = 17;
			// 
			// parserControl3
			// 
			this.parserControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl3.ExportJSONProgress = 0;
			this.parserControl3.Label = "label1";
			this.parserControl3.Location = new System.Drawing.Point(20, 229);
			this.parserControl3.Name = "parserControl3";
			this.parserControl3.Size = new System.Drawing.Size(650, 84);
			this.parserControl3.SourceBin = "label1";
			this.parserControl3.TabIndex = 19;
			// 
			// parserControl1
			// 
			this.parserControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parserControl1.ExportJSONProgress = 0;
			this.parserControl1.Label = "label1";
			this.parserControl1.Location = new System.Drawing.Point(20, 49);
			this.parserControl1.Name = "parserControl1";
			this.parserControl1.Size = new System.Drawing.Size(650, 84);
			this.parserControl1.SourceBin = "label1";
			this.parserControl1.TabIndex = 18;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.cmdConvertGDLEJSONToACESQL);
			this.tabPage3.Controls.Add(this.txtGDLEParser);
			this.tabPage3.Controls.Add(this.lblGDLESQLOutputFolder);
			this.tabPage3.Controls.Add(this.cmdChooseSQLOutputFolder);
			this.tabPage3.Controls.Add(this.cmdChooseJSONRootFolder);
			this.tabPage3.Controls.Add(this.lblGDLEJSONRootFolder);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(1060, 1057);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "GDLE .json -> ACE .sql";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// lblGDLESQLOutputFolder
			// 
			this.lblGDLESQLOutputFolder.AutoSize = true;
			this.lblGDLESQLOutputFolder.Location = new System.Drawing.Point(186, 54);
			this.lblGDLESQLOutputFolder.Name = "lblGDLESQLOutputFolder";
			this.lblGDLESQLOutputFolder.Size = new System.Drawing.Size(35, 13);
			this.lblGDLESQLOutputFolder.TabIndex = 27;
			this.lblGDLESQLOutputFolder.Text = "label1";
			// 
			// cmdChooseSQLOutputFolder
			// 
			this.cmdChooseSQLOutputFolder.Location = new System.Drawing.Point(20, 49);
			this.cmdChooseSQLOutputFolder.Name = "cmdChooseSQLOutputFolder";
			this.cmdChooseSQLOutputFolder.Size = new System.Drawing.Size(160, 23);
			this.cmdChooseSQLOutputFolder.TabIndex = 26;
			this.cmdChooseSQLOutputFolder.Text = "Choose SQL Output Folder";
			this.cmdChooseSQLOutputFolder.UseVisualStyleBackColor = true;
			this.cmdChooseSQLOutputFolder.Click += new System.EventHandler(this.cmdChooseSQLOutputFolder_Click);
			// 
			// cmdChooseJSONRootFolder
			// 
			this.cmdChooseJSONRootFolder.Location = new System.Drawing.Point(20, 20);
			this.cmdChooseJSONRootFolder.Name = "cmdChooseJSONRootFolder";
			this.cmdChooseJSONRootFolder.Size = new System.Drawing.Size(160, 23);
			this.cmdChooseJSONRootFolder.TabIndex = 24;
			this.cmdChooseJSONRootFolder.Text = "Choose JSON Root Folder";
			this.cmdChooseJSONRootFolder.UseVisualStyleBackColor = true;
			this.cmdChooseJSONRootFolder.Click += new System.EventHandler(this.cmdChooseJSONRootFolder_Click);
			// 
			// lblGDLEJSONRootFolder
			// 
			this.lblGDLEJSONRootFolder.AutoSize = true;
			this.lblGDLEJSONRootFolder.Location = new System.Drawing.Point(186, 25);
			this.lblGDLEJSONRootFolder.Name = "lblGDLEJSONRootFolder";
			this.lblGDLEJSONRootFolder.Size = new System.Drawing.Size(35, 13);
			this.lblGDLEJSONRootFolder.TabIndex = 25;
			this.lblGDLEJSONRootFolder.Text = "label1";
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(1060, 1057);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "ACE Database -> ACE .sql";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// txtGDLEParser
			// 
			this.txtGDLEParser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGDLEParser.Location = new System.Drawing.Point(8, 149);
			this.txtGDLEParser.Name = "txtGDLEParser";
			this.txtGDLEParser.Size = new System.Drawing.Size(1044, 900);
			this.txtGDLEParser.TabIndex = 28;
			this.txtGDLEParser.Text = "";
			// 
			// cmdConvertGDLEJSONToACESQL
			// 
			this.cmdConvertGDLEJSONToACESQL.Location = new System.Drawing.Point(20, 120);
			this.cmdConvertGDLEJSONToACESQL.Name = "cmdConvertGDLEJSONToACESQL";
			this.cmdConvertGDLEJSONToACESQL.Size = new System.Drawing.Size(235, 23);
			this.cmdConvertGDLEJSONToACESQL.TabIndex = 29;
			this.cmdConvertGDLEJSONToACESQL.Text = "Convert GDLE JSON to ACE .SQL";
			this.cmdConvertGDLEJSONToACESQL.UseVisualStyleBackColor = true;
			this.cmdConvertGDLEJSONToACESQL.Click += new System.EventHandler(this.cmdConvertGDLEJSONToACESQL_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1068, 1083);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ACE Adapter (Format Converter)";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private ParserControl parserControl8;
		private ParserControl parserControl6;
		private ParserControl parserControl9;
		private ParserControl parserControl4;
		private ParserControl parserControl2;
		private ParserControl parserControlA;
		private ParserControl parserControl1;
		private ParserControl parserControl3;
		private ParserControl parserControl5;
		private ParserControl parserControl7;
		private System.Windows.Forms.Button cmdOutputFolder;
		private System.Windows.Forms.Label lblOutputFolder;
        private ParserControl parserControlB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label1;
        private SQLBuilderControl sqlBuilderControl1;
        private System.Windows.Forms.Label lblGDLESQLOutputFolder;
        private System.Windows.Forms.Button cmdChooseSQLOutputFolder;
        private System.Windows.Forms.Button cmdChooseJSONRootFolder;
        private System.Windows.Forms.Label lblGDLEJSONRootFolder;
        private System.Windows.Forms.Button cmdConvertGDLEJSONToACESQL;
        private System.Windows.Forms.RichTextBox txtGDLEParser;
    }
}

