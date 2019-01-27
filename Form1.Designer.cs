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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
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
            this.chkHidePassword = new System.Windows.Forms.CheckBox();
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
            this.tabPage4.Controls.Add(this.chkHidePassword);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.button4);
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Controls.Add(this.button7);
            this.tabPage4.Controls.Add(this.button8);
            this.tabPage4.Controls.Add(this.button9);
            this.tabPage4.Controls.Add(this.button10);
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
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(840, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 58;
            this.button1.Text = "B Events";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(840, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 57;
            this.button2.Text = "A Mutation";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(715, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 23);
            this.button3.TabIndex = 56;
            this.button3.Text = "5 Housing";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(590, 115);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 23);
            this.button4.TabIndex = 55;
            this.button4.Text = "4 Crafting";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(590, 6);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 23);
            this.button5.TabIndex = 54;
            this.button5.Text = "1 Regions";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(590, 78);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(119, 23);
            this.button6.TabIndex = 53;
            this.button6.Text = "3 Treasure";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(715, 78);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(119, 23);
            this.button7.TabIndex = 52;
            this.button7.Text = "8 Quests";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(715, 115);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(119, 23);
            this.button8.TabIndex = 51;
            this.button8.Text = "9 Weenies";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(715, 41);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(119, 23);
            this.button9.TabIndex = 50;
            this.button9.Text = "6 Landblocks";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Enabled = false;
            this.button10.Location = new System.Drawing.Point(590, 41);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(119, 23);
            this.button10.TabIndex = 49;
            this.button10.Text = "2 Spells";
            this.button10.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.Button cmdGDLE6LandblocksParse;
        private System.Windows.Forms.Button cmdGDLE2SpellsParse;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.CheckBox chkHidePassword;
    }
}

