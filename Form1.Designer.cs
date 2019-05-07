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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chkCacheDedupe = new System.Windows.Forms.CheckBox();
            this.chkGDLEApplyEnumShift = new System.Windows.Forms.CheckBox();
            this.cmdGDLEBEventsParse = new System.Windows.Forms.Button();
            this.cmdGDLEAMutationParse = new System.Windows.Forms.Button();
            this.cmdGDLE5HousingParse = new System.Windows.Forms.Button();
            this.cmdGDLE4CraftingParse = new System.Windows.Forms.Button();
            this.cmdGDLE1RegionsParse = new System.Windows.Forms.Button();
            this.cmdGDLE3TreasureParse = new System.Windows.Forms.Button();
            this.cmdGDLE8QuestsParse = new System.Windows.Forms.Button();
            this.cmdGDLE9WeeniesParse = new System.Windows.Forms.Button();
            this.cmdGDLE6LandblocksParse = new System.Windows.Forms.Button();
            this.cmdGDLE2SpellsParse = new System.Windows.Forms.Button();
            this.cmdParseGDLEJSONs = new System.Windows.Forms.Button();
            this.txtGDLEJSONParser = new System.Windows.Forms.RichTextBox();
            this.lblGDLESQLOutputFolder = new System.Windows.Forms.Label();
            this.cmdChooseSQLOutputFolder = new System.Windows.Forms.Button();
            this.cmdChooseJSONRootFolder = new System.Windows.Forms.Button();
            this.lblGDLEJSONRootFolder = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmdACEExportWeenieAsJson = new System.Windows.Forms.Button();
            this.chkHidePassword = new System.Windows.Forms.CheckBox();
            this.cmdACEBEventsParse = new System.Windows.Forms.Button();
            this.cmdACEAMutationParse = new System.Windows.Forms.Button();
            this.cmdACE5HousingParse = new System.Windows.Forms.Button();
            this.cmdACE4CraftingParse = new System.Windows.Forms.Button();
            this.cmdACE1RegionsParse = new System.Windows.Forms.Button();
            this.cmdACE3TreasureParse = new System.Windows.Forms.Button();
            this.cmdACE8QuestsParse = new System.Windows.Forms.Button();
            this.cmdACE9WeeniesParse = new System.Windows.Forms.Button();
            this.cmdACE6LandblocksParse = new System.Windows.Forms.Button();
            this.cmdACE2SpellsParse = new System.Windows.Forms.Button();
            this.cmdACEDatabaseCacheAllWeenies = new System.Windows.Forms.Button();
            this.txtACEDatabaseConnector = new System.Windows.Forms.RichTextBox();
            this.txtACEWorldDatabase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtACEWorldPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtACEWorldUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtACEWorldPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtACEWorldServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdTestInitDatabaseConnection = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtOutputTool1 = new System.Windows.Forms.RichTextBox();
            this.cmdOutputTool1 = new System.Windows.Forms.Button();
            this.txtACEExportwcidStart = new System.Windows.Forms.TextBox();
            this.txtACEExportwcidEnd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
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
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1068, 1061);
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
            this.tabPage1.Size = new System.Drawing.Size(1060, 1035);
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkCacheDedupe);
            this.tabPage3.Controls.Add(this.chkGDLEApplyEnumShift);
            this.tabPage3.Controls.Add(this.cmdGDLEBEventsParse);
            this.tabPage3.Controls.Add(this.cmdGDLEAMutationParse);
            this.tabPage3.Controls.Add(this.cmdGDLE5HousingParse);
            this.tabPage3.Controls.Add(this.cmdGDLE4CraftingParse);
            this.tabPage3.Controls.Add(this.cmdGDLE1RegionsParse);
            this.tabPage3.Controls.Add(this.cmdGDLE3TreasureParse);
            this.tabPage3.Controls.Add(this.cmdGDLE8QuestsParse);
            this.tabPage3.Controls.Add(this.cmdGDLE9WeeniesParse);
            this.tabPage3.Controls.Add(this.cmdGDLE6LandblocksParse);
            this.tabPage3.Controls.Add(this.cmdGDLE2SpellsParse);
            this.tabPage3.Controls.Add(this.cmdParseGDLEJSONs);
            this.tabPage3.Controls.Add(this.txtGDLEJSONParser);
            this.tabPage3.Controls.Add(this.lblGDLESQLOutputFolder);
            this.tabPage3.Controls.Add(this.cmdChooseSQLOutputFolder);
            this.tabPage3.Controls.Add(this.cmdChooseJSONRootFolder);
            this.tabPage3.Controls.Add(this.lblGDLEJSONRootFolder);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1060, 1035);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "GDLE .json Parser";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkCacheDedupe
            // 
            this.chkCacheDedupe.AutoSize = true;
            this.chkCacheDedupe.Location = new System.Drawing.Point(840, 82);
            this.chkCacheDedupe.Name = "chkCacheDedupe";
            this.chkCacheDedupe.Size = new System.Drawing.Size(121, 17);
            this.chkCacheDedupe.TabIndex = 50;
            this.chkCacheDedupe.Text = "Dedupe from Cache";
            this.chkCacheDedupe.UseVisualStyleBackColor = true;
            // 
            // chkGDLEApplyEnumShift
            // 
            this.chkGDLEApplyEnumShift.AutoSize = true;
            this.chkGDLEApplyEnumShift.Location = new System.Drawing.Point(186, 124);
            this.chkGDLEApplyEnumShift.Name = "chkGDLEApplyEnumShift";
            this.chkGDLEApplyEnumShift.Size = new System.Drawing.Size(109, 17);
            this.chkGDLEApplyEnumShift.TabIndex = 49;
            this.chkGDLEApplyEnumShift.Text = "Apply ENUM shift";
            this.chkGDLEApplyEnumShift.UseVisualStyleBackColor = true;
            // 
            // cmdGDLEBEventsParse
            // 
            this.cmdGDLEBEventsParse.Enabled = false;
            this.cmdGDLEBEventsParse.Location = new System.Drawing.Point(840, 41);
            this.cmdGDLEBEventsParse.Name = "cmdGDLEBEventsParse";
            this.cmdGDLEBEventsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLEBEventsParse.TabIndex = 48;
            this.cmdGDLEBEventsParse.Text = "B Events";
            this.cmdGDLEBEventsParse.UseVisualStyleBackColor = true;
            this.cmdGDLEBEventsParse.Click += new System.EventHandler(this.cmdGDLEBEventsParse_Click);
            // 
            // cmdGDLEAMutationParse
            // 
            this.cmdGDLEAMutationParse.Enabled = false;
            this.cmdGDLEAMutationParse.Location = new System.Drawing.Point(840, 6);
            this.cmdGDLEAMutationParse.Name = "cmdGDLEAMutationParse";
            this.cmdGDLEAMutationParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLEAMutationParse.TabIndex = 47;
            this.cmdGDLEAMutationParse.Text = "A Mutation";
            this.cmdGDLEAMutationParse.UseVisualStyleBackColor = true;
            this.cmdGDLEAMutationParse.Click += new System.EventHandler(this.cmdGDLEAMutationParse_Click);
            // 
            // cmdGDLE5HousingParse
            // 
            this.cmdGDLE5HousingParse.Enabled = false;
            this.cmdGDLE5HousingParse.Location = new System.Drawing.Point(715, 6);
            this.cmdGDLE5HousingParse.Name = "cmdGDLE5HousingParse";
            this.cmdGDLE5HousingParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE5HousingParse.TabIndex = 46;
            this.cmdGDLE5HousingParse.Text = "5 Housing";
            this.cmdGDLE5HousingParse.UseVisualStyleBackColor = true;
            this.cmdGDLE5HousingParse.Click += new System.EventHandler(this.cmdGDLE5HousingParse_Click);
            // 
            // cmdGDLE4CraftingParse
            // 
            this.cmdGDLE4CraftingParse.Enabled = false;
            this.cmdGDLE4CraftingParse.Location = new System.Drawing.Point(590, 115);
            this.cmdGDLE4CraftingParse.Name = "cmdGDLE4CraftingParse";
            this.cmdGDLE4CraftingParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE4CraftingParse.TabIndex = 45;
            this.cmdGDLE4CraftingParse.Text = "4 Crafting";
            this.cmdGDLE4CraftingParse.UseVisualStyleBackColor = true;
            this.cmdGDLE4CraftingParse.Click += new System.EventHandler(this.cmdGDLE4CraftingParse_Click);
            // 
            // cmdGDLE1RegionsParse
            // 
            this.cmdGDLE1RegionsParse.Enabled = false;
            this.cmdGDLE1RegionsParse.Location = new System.Drawing.Point(590, 6);
            this.cmdGDLE1RegionsParse.Name = "cmdGDLE1RegionsParse";
            this.cmdGDLE1RegionsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE1RegionsParse.TabIndex = 44;
            this.cmdGDLE1RegionsParse.Text = "1 Regions";
            this.cmdGDLE1RegionsParse.UseVisualStyleBackColor = true;
            this.cmdGDLE1RegionsParse.Click += new System.EventHandler(this.cmdGDLE1RegionsParse_Click);
            // 
            // cmdGDLE3TreasureParse
            // 
            this.cmdGDLE3TreasureParse.Enabled = false;
            this.cmdGDLE3TreasureParse.Location = new System.Drawing.Point(590, 78);
            this.cmdGDLE3TreasureParse.Name = "cmdGDLE3TreasureParse";
            this.cmdGDLE3TreasureParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE3TreasureParse.TabIndex = 43;
            this.cmdGDLE3TreasureParse.Text = "3 Treasure";
            this.cmdGDLE3TreasureParse.UseVisualStyleBackColor = true;
            this.cmdGDLE3TreasureParse.Click += new System.EventHandler(this.cmdGDLE3TreasureParse_Click);
            // 
            // cmdGDLE8QuestsParse
            // 
            this.cmdGDLE8QuestsParse.Enabled = false;
            this.cmdGDLE8QuestsParse.Location = new System.Drawing.Point(715, 78);
            this.cmdGDLE8QuestsParse.Name = "cmdGDLE8QuestsParse";
            this.cmdGDLE8QuestsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE8QuestsParse.TabIndex = 42;
            this.cmdGDLE8QuestsParse.Text = "8 Quests";
            this.cmdGDLE8QuestsParse.UseVisualStyleBackColor = true;
            this.cmdGDLE8QuestsParse.Click += new System.EventHandler(this.cmdGDLE8QuestsParse_Click);
            // 
            // cmdGDLE9WeeniesParse
            // 
            this.cmdGDLE9WeeniesParse.Enabled = false;
            this.cmdGDLE9WeeniesParse.Location = new System.Drawing.Point(715, 115);
            this.cmdGDLE9WeeniesParse.Name = "cmdGDLE9WeeniesParse";
            this.cmdGDLE9WeeniesParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE9WeeniesParse.TabIndex = 41;
            this.cmdGDLE9WeeniesParse.Text = "9 Weenies";
            this.cmdGDLE9WeeniesParse.UseVisualStyleBackColor = true;
            this.cmdGDLE9WeeniesParse.Click += new System.EventHandler(this.cmdGDLE9WeeniesParse_Click);
            // 
            // cmdGDLE6LandblocksParse
            // 
            this.cmdGDLE6LandblocksParse.Enabled = false;
            this.cmdGDLE6LandblocksParse.Location = new System.Drawing.Point(715, 41);
            this.cmdGDLE6LandblocksParse.Name = "cmdGDLE6LandblocksParse";
            this.cmdGDLE6LandblocksParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE6LandblocksParse.TabIndex = 40;
            this.cmdGDLE6LandblocksParse.Text = "6 Landblocks";
            this.cmdGDLE6LandblocksParse.UseVisualStyleBackColor = true;
            this.cmdGDLE6LandblocksParse.Click += new System.EventHandler(this.cmdGDLE6LandblocksParse_Click);
            // 
            // cmdGDLE2SpellsParse
            // 
            this.cmdGDLE2SpellsParse.Enabled = false;
            this.cmdGDLE2SpellsParse.Location = new System.Drawing.Point(590, 41);
            this.cmdGDLE2SpellsParse.Name = "cmdGDLE2SpellsParse";
            this.cmdGDLE2SpellsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdGDLE2SpellsParse.TabIndex = 39;
            this.cmdGDLE2SpellsParse.Text = "2 Spells";
            this.cmdGDLE2SpellsParse.UseVisualStyleBackColor = true;
            this.cmdGDLE2SpellsParse.Click += new System.EventHandler(this.cmdGDLE2SpellsParse_Click);
            // 
            // cmdParseGDLEJSONs
            // 
            this.cmdParseGDLEJSONs.Location = new System.Drawing.Point(20, 120);
            this.cmdParseGDLEJSONs.Name = "cmdParseGDLEJSONs";
            this.cmdParseGDLEJSONs.Size = new System.Drawing.Size(160, 23);
            this.cmdParseGDLEJSONs.TabIndex = 29;
            this.cmdParseGDLEJSONs.Text = "Parse GDLE JSON";
            this.cmdParseGDLEJSONs.UseVisualStyleBackColor = true;
            this.cmdParseGDLEJSONs.Click += new System.EventHandler(this.cmdParseGDLEJSONs_Click);
            // 
            // txtGDLEJSONParser
            // 
            this.txtGDLEJSONParser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGDLEJSONParser.Location = new System.Drawing.Point(8, 149);
            this.txtGDLEJSONParser.Name = "txtGDLEJSONParser";
            this.txtGDLEJSONParser.Size = new System.Drawing.Size(1044, 878);
            this.txtGDLEJSONParser.TabIndex = 28;
            this.txtGDLEJSONParser.Text = "";
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
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.txtACEExportwcidEnd);
            this.tabPage4.Controls.Add(this.txtACEExportwcidStart);
            this.tabPage4.Controls.Add(this.cmdACEExportWeenieAsJson);
            this.tabPage4.Controls.Add(this.chkHidePassword);
            this.tabPage4.Controls.Add(this.cmdACEBEventsParse);
            this.tabPage4.Controls.Add(this.cmdACEAMutationParse);
            this.tabPage4.Controls.Add(this.cmdACE5HousingParse);
            this.tabPage4.Controls.Add(this.cmdACE4CraftingParse);
            this.tabPage4.Controls.Add(this.cmdACE1RegionsParse);
            this.tabPage4.Controls.Add(this.cmdACE3TreasureParse);
            this.tabPage4.Controls.Add(this.cmdACE8QuestsParse);
            this.tabPage4.Controls.Add(this.cmdACE9WeeniesParse);
            this.tabPage4.Controls.Add(this.cmdACE6LandblocksParse);
            this.tabPage4.Controls.Add(this.cmdACE2SpellsParse);
            this.tabPage4.Controls.Add(this.cmdACEDatabaseCacheAllWeenies);
            this.tabPage4.Controls.Add(this.txtACEDatabaseConnector);
            this.tabPage4.Controls.Add(this.txtACEWorldDatabase);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.txtACEWorldPassword);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.txtACEWorldUser);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.txtACEWorldPort);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.txtACEWorldServer);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.cmdTestInitDatabaseConnection);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1060, 1035);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "ACE Database Connector";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cmdACEExportWeenieAsJson
            // 
            this.cmdACEExportWeenieAsJson.Enabled = false;
            this.cmdACEExportWeenieAsJson.Location = new System.Drawing.Point(590, 188);
            this.cmdACEExportWeenieAsJson.Name = "cmdACEExportWeenieAsJson";
            this.cmdACEExportWeenieAsJson.Size = new System.Drawing.Size(119, 23);
            this.cmdACEExportWeenieAsJson.TabIndex = 60;
            this.cmdACEExportWeenieAsJson.Text = "9 Weenies";
            this.cmdACEExportWeenieAsJson.UseVisualStyleBackColor = true;
            this.cmdACEExportWeenieAsJson.Click += new System.EventHandler(this.cmdACEExportWeenieAsJson_Click);
            // 
            // chkHidePassword
            // 
            this.chkHidePassword.AutoSize = true;
            this.chkHidePassword.Checked = true;
            this.chkHidePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHidePassword.Location = new System.Drawing.Point(286, 100);
            this.chkHidePassword.Name = "chkHidePassword";
            this.chkHidePassword.Size = new System.Drawing.Size(48, 17);
            this.chkHidePassword.TabIndex = 59;
            this.chkHidePassword.Text = "Hide";
            this.chkHidePassword.UseVisualStyleBackColor = true;
            this.chkHidePassword.CheckedChanged += new System.EventHandler(this.chkHidePassword_CheckedChanged);
            // 
            // cmdACEBEventsParse
            // 
            this.cmdACEBEventsParse.Enabled = false;
            this.cmdACEBEventsParse.Location = new System.Drawing.Point(840, 41);
            this.cmdACEBEventsParse.Name = "cmdACEBEventsParse";
            this.cmdACEBEventsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACEBEventsParse.TabIndex = 58;
            this.cmdACEBEventsParse.Text = "B Events";
            this.cmdACEBEventsParse.UseVisualStyleBackColor = true;
            this.cmdACEBEventsParse.Click += new System.EventHandler(this.cmdACEBEventsParse_Click);
            // 
            // cmdACEAMutationParse
            // 
            this.cmdACEAMutationParse.Enabled = false;
            this.cmdACEAMutationParse.Location = new System.Drawing.Point(840, 6);
            this.cmdACEAMutationParse.Name = "cmdACEAMutationParse";
            this.cmdACEAMutationParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACEAMutationParse.TabIndex = 57;
            this.cmdACEAMutationParse.Text = "A Mutation";
            this.cmdACEAMutationParse.UseVisualStyleBackColor = true;
            this.cmdACEAMutationParse.Click += new System.EventHandler(this.cmdACEAMutationParse_Click);
            // 
            // cmdACE5HousingParse
            // 
            this.cmdACE5HousingParse.Enabled = false;
            this.cmdACE5HousingParse.Location = new System.Drawing.Point(715, 6);
            this.cmdACE5HousingParse.Name = "cmdACE5HousingParse";
            this.cmdACE5HousingParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE5HousingParse.TabIndex = 56;
            this.cmdACE5HousingParse.Text = "5 Housing";
            this.cmdACE5HousingParse.UseVisualStyleBackColor = true;
            this.cmdACE5HousingParse.Click += new System.EventHandler(this.cmdACE5HousingParse_Click);
            // 
            // cmdACE4CraftingParse
            // 
            this.cmdACE4CraftingParse.Enabled = false;
            this.cmdACE4CraftingParse.Location = new System.Drawing.Point(590, 115);
            this.cmdACE4CraftingParse.Name = "cmdACE4CraftingParse";
            this.cmdACE4CraftingParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE4CraftingParse.TabIndex = 55;
            this.cmdACE4CraftingParse.Text = "4 Crafting";
            this.cmdACE4CraftingParse.UseVisualStyleBackColor = true;
            this.cmdACE4CraftingParse.Click += new System.EventHandler(this.cmdACE4CraftingParse_Click);
            // 
            // cmdACE1RegionsParse
            // 
            this.cmdACE1RegionsParse.Enabled = false;
            this.cmdACE1RegionsParse.Location = new System.Drawing.Point(590, 6);
            this.cmdACE1RegionsParse.Name = "cmdACE1RegionsParse";
            this.cmdACE1RegionsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE1RegionsParse.TabIndex = 54;
            this.cmdACE1RegionsParse.Text = "1 Regions";
            this.cmdACE1RegionsParse.UseVisualStyleBackColor = true;
            this.cmdACE1RegionsParse.Click += new System.EventHandler(this.cmdACE1RegionsParse_Click);
            // 
            // cmdACE3TreasureParse
            // 
            this.cmdACE3TreasureParse.Enabled = false;
            this.cmdACE3TreasureParse.Location = new System.Drawing.Point(590, 78);
            this.cmdACE3TreasureParse.Name = "cmdACE3TreasureParse";
            this.cmdACE3TreasureParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE3TreasureParse.TabIndex = 53;
            this.cmdACE3TreasureParse.Text = "3 Treasure";
            this.cmdACE3TreasureParse.UseVisualStyleBackColor = true;
            this.cmdACE3TreasureParse.Click += new System.EventHandler(this.cmdACE3TreasureParse_Click);
            // 
            // cmdACE8QuestsParse
            // 
            this.cmdACE8QuestsParse.Enabled = false;
            this.cmdACE8QuestsParse.Location = new System.Drawing.Point(715, 78);
            this.cmdACE8QuestsParse.Name = "cmdACE8QuestsParse";
            this.cmdACE8QuestsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE8QuestsParse.TabIndex = 52;
            this.cmdACE8QuestsParse.Text = "8 Quests";
            this.cmdACE8QuestsParse.UseVisualStyleBackColor = true;
            this.cmdACE8QuestsParse.Click += new System.EventHandler(this.cmdACE8QuestsParse_Click);
            // 
            // cmdACE9WeeniesParse
            // 
            this.cmdACE9WeeniesParse.Enabled = false;
            this.cmdACE9WeeniesParse.Location = new System.Drawing.Point(715, 115);
            this.cmdACE9WeeniesParse.Name = "cmdACE9WeeniesParse";
            this.cmdACE9WeeniesParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE9WeeniesParse.TabIndex = 51;
            this.cmdACE9WeeniesParse.Text = "9 Weenies";
            this.cmdACE9WeeniesParse.UseVisualStyleBackColor = true;
            this.cmdACE9WeeniesParse.Click += new System.EventHandler(this.cmdACE9WeeniesParse_Click);
            // 
            // cmdACE6LandblocksParse
            // 
            this.cmdACE6LandblocksParse.Enabled = false;
            this.cmdACE6LandblocksParse.Location = new System.Drawing.Point(715, 41);
            this.cmdACE6LandblocksParse.Name = "cmdACE6LandblocksParse";
            this.cmdACE6LandblocksParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE6LandblocksParse.TabIndex = 50;
            this.cmdACE6LandblocksParse.Text = "6 Landblocks";
            this.cmdACE6LandblocksParse.UseVisualStyleBackColor = true;
            this.cmdACE6LandblocksParse.Click += new System.EventHandler(this.cmdACE6LandblocksParse_Click);
            // 
            // cmdACE2SpellsParse
            // 
            this.cmdACE2SpellsParse.Enabled = false;
            this.cmdACE2SpellsParse.Location = new System.Drawing.Point(590, 41);
            this.cmdACE2SpellsParse.Name = "cmdACE2SpellsParse";
            this.cmdACE2SpellsParse.Size = new System.Drawing.Size(119, 23);
            this.cmdACE2SpellsParse.TabIndex = 49;
            this.cmdACE2SpellsParse.Text = "2 Spells";
            this.cmdACE2SpellsParse.UseVisualStyleBackColor = true;
            this.cmdACE2SpellsParse.Click += new System.EventHandler(this.cmdACE2SpellsParse_Click);
            // 
            // cmdACEDatabaseCacheAllWeenies
            // 
            this.cmdACEDatabaseCacheAllWeenies.Location = new System.Drawing.Point(8, 249);
            this.cmdACEDatabaseCacheAllWeenies.Name = "cmdACEDatabaseCacheAllWeenies";
            this.cmdACEDatabaseCacheAllWeenies.Size = new System.Drawing.Size(200, 23);
            this.cmdACEDatabaseCacheAllWeenies.TabIndex = 30;
            this.cmdACEDatabaseCacheAllWeenies.Text = "Cache All Weenies";
            this.cmdACEDatabaseCacheAllWeenies.UseVisualStyleBackColor = true;
            this.cmdACEDatabaseCacheAllWeenies.Click += new System.EventHandler(this.cmdACEDatabaseCacheAllWeenies_Click);
            // 
            // txtACEDatabaseConnector
            // 
            this.txtACEDatabaseConnector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtACEDatabaseConnector.Location = new System.Drawing.Point(8, 278);
            this.txtACEDatabaseConnector.Name = "txtACEDatabaseConnector";
            this.txtACEDatabaseConnector.Size = new System.Drawing.Size(1044, 749);
            this.txtACEDatabaseConnector.TabIndex = 29;
            this.txtACEDatabaseConnector.Text = "";
            // 
            // txtACEWorldDatabase
            // 
            this.txtACEWorldDatabase.Location = new System.Drawing.Point(79, 124);
            this.txtACEWorldDatabase.Name = "txtACEWorldDatabase";
            this.txtACEWorldDatabase.Size = new System.Drawing.Size(200, 20);
            this.txtACEWorldDatabase.TabIndex = 10;
            this.txtACEWorldDatabase.TextChanged += new System.EventHandler(this.txtACEWorldDatabase_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Database";
            // 
            // txtACEWorldPassword
            // 
            this.txtACEWorldPassword.Location = new System.Drawing.Point(79, 98);
            this.txtACEWorldPassword.Name = "txtACEWorldPassword";
            this.txtACEWorldPassword.Size = new System.Drawing.Size(200, 20);
            this.txtACEWorldPassword.TabIndex = 8;
            this.txtACEWorldPassword.UseSystemPasswordChar = true;
            this.txtACEWorldPassword.TextChanged += new System.EventHandler(this.txtACEWorldPassword_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Password";
            // 
            // txtACEWorldUser
            // 
            this.txtACEWorldUser.Location = new System.Drawing.Point(79, 72);
            this.txtACEWorldUser.Name = "txtACEWorldUser";
            this.txtACEWorldUser.Size = new System.Drawing.Size(200, 20);
            this.txtACEWorldUser.TabIndex = 6;
            this.txtACEWorldUser.TextChanged += new System.EventHandler(this.txtACEWorldUser_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "User";
            // 
            // txtACEWorldPort
            // 
            this.txtACEWorldPort.Location = new System.Drawing.Point(79, 46);
            this.txtACEWorldPort.Name = "txtACEWorldPort";
            this.txtACEWorldPort.Size = new System.Drawing.Size(200, 20);
            this.txtACEWorldPort.TabIndex = 4;
            this.txtACEWorldPort.TextChanged += new System.EventHandler(this.txtACEWorldPort_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port";
            // 
            // txtACEWorldServer
            // 
            this.txtACEWorldServer.Location = new System.Drawing.Point(79, 20);
            this.txtACEWorldServer.Name = "txtACEWorldServer";
            this.txtACEWorldServer.Size = new System.Drawing.Size(200, 20);
            this.txtACEWorldServer.TabIndex = 2;
            this.txtACEWorldServer.TextChanged += new System.EventHandler(this.txtACEWorldServer_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server";
            // 
            // cmdTestInitDatabaseConnection
            // 
            this.cmdTestInitDatabaseConnection.Location = new System.Drawing.Point(79, 150);
            this.cmdTestInitDatabaseConnection.Name = "cmdTestInitDatabaseConnection";
            this.cmdTestInitDatabaseConnection.Size = new System.Drawing.Size(200, 23);
            this.cmdTestInitDatabaseConnection.TabIndex = 0;
            this.cmdTestInitDatabaseConnection.Text = "Test/Init Database Connection";
            this.cmdTestInitDatabaseConnection.UseVisualStyleBackColor = true;
            this.cmdTestInitDatabaseConnection.Click += new System.EventHandler(this.cmdTestInitDatabaseConnection_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtOutputTool1);
            this.tabPage2.Controls.Add(this.cmdOutputTool1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1060, 1035);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Output Tools";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtOutputTool1
            // 
            this.txtOutputTool1.Location = new System.Drawing.Point(20, 49);
            this.txtOutputTool1.Name = "txtOutputTool1";
            this.txtOutputTool1.Size = new System.Drawing.Size(394, 516);
            this.txtOutputTool1.TabIndex = 29;
            this.txtOutputTool1.Text = "";
            // 
            // cmdOutputTool1
            // 
            this.cmdOutputTool1.Location = new System.Drawing.Point(20, 20);
            this.cmdOutputTool1.Name = "cmdOutputTool1";
            this.cmdOutputTool1.Size = new System.Drawing.Size(200, 23);
            this.cmdOutputTool1.TabIndex = 0;
            this.cmdOutputTool1.Text = "OutputTool1";
            this.cmdOutputTool1.UseVisualStyleBackColor = true;
            this.cmdOutputTool1.Click += new System.EventHandler(this.cmdOutputTool1_Click);
            // 
            // txtACEExportwcidStart
            // 
            this.txtACEExportwcidStart.Location = new System.Drawing.Point(716, 190);
            this.txtACEExportwcidStart.Name = "txtACEExportwcidStart";
            this.txtACEExportwcidStart.Size = new System.Drawing.Size(100, 20);
            this.txtACEExportwcidStart.TabIndex = 61;
            this.txtACEExportwcidStart.Text = "0";
            this.txtACEExportwcidStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtACEExportwcidEnd
            // 
            this.txtACEExportwcidEnd.Location = new System.Drawing.Point(822, 190);
            this.txtACEExportwcidEnd.Name = "txtACEExportwcidEnd";
            this.txtACEExportwcidEnd.Size = new System.Drawing.Size(100, 20);
            this.txtACEExportwcidEnd.TabIndex = 62;
            this.txtACEExportwcidEnd.Text = "0";
            this.txtACEExportwcidEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(587, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 13);
            this.label7.TabIndex = 63;
            this.label7.Text = "Export From ACE World Database to JSON:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(713, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 64;
            this.label8.Text = "Start WCID";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(864, 213);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "End WCID";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 1061);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACE Adapter (Format Converter)";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
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
        private System.Windows.Forms.Button cmdParseGDLEJSONs;
        private System.Windows.Forms.RichTextBox txtGDLEJSONParser;
        private System.Windows.Forms.Button cmdTestInitDatabaseConnection;
        private System.Windows.Forms.TextBox txtACEWorldServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtACEWorldDatabase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtACEWorldPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtACEWorldUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtACEWorldPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox txtOutputTool1;
        private System.Windows.Forms.Button cmdOutputTool1;
        private System.Windows.Forms.RichTextBox txtACEDatabaseConnector;
        private System.Windows.Forms.Button cmdACEDatabaseCacheAllWeenies;
        private System.Windows.Forms.Button cmdGDLEBEventsParse;
        private System.Windows.Forms.Button cmdGDLEAMutationParse;
        private System.Windows.Forms.Button cmdGDLE5HousingParse;
        private System.Windows.Forms.Button cmdGDLE4CraftingParse;
        private System.Windows.Forms.Button cmdGDLE1RegionsParse;
        private System.Windows.Forms.Button cmdGDLE3TreasureParse;
        private System.Windows.Forms.Button cmdGDLE8QuestsParse;
        private System.Windows.Forms.Button cmdGDLE9WeeniesParse;
        private System.Windows.Forms.Button cmdGDLE2SpellsParse;
        private System.Windows.Forms.Button cmdACEBEventsParse;
        private System.Windows.Forms.Button cmdACEAMutationParse;
        private System.Windows.Forms.Button cmdACE5HousingParse;
        private System.Windows.Forms.Button cmdACE4CraftingParse;
        private System.Windows.Forms.Button cmdACE1RegionsParse;
        private System.Windows.Forms.Button cmdACE3TreasureParse;
        private System.Windows.Forms.Button cmdACE8QuestsParse;
        private System.Windows.Forms.Button cmdACE9WeeniesParse;
        private System.Windows.Forms.Button cmdACE6LandblocksParse;
        private System.Windows.Forms.Button cmdACE2SpellsParse;
        private System.Windows.Forms.CheckBox chkHidePassword;
        private System.Windows.Forms.Button cmdGDLE6LandblocksParse;
        private System.Windows.Forms.CheckBox chkGDLEApplyEnumShift;
        private System.Windows.Forms.CheckBox chkCacheDedupe;
        private System.Windows.Forms.Button cmdACEExportWeenieAsJson;
        private System.Windows.Forms.TextBox txtACEExportwcidEnd;
        private System.Windows.Forms.TextBox txtACEExportwcidStart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}

