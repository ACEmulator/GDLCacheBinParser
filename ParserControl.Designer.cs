namespace PhatACCacheBinParser
{
	partial class ParserControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdChooseSource = new System.Windows.Forms.Button();
			this.lblSourceBin = new System.Windows.Forms.Label();
			this.cmdDoParse = new System.Windows.Forms.Button();
			this.chkWriteJSON = new System.Windows.Forms.CheckBox();
			this.progressParseSource = new System.Windows.Forms.ProgressBar();
			this.progressWriteJSONOutput = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdChooseSource
			// 
			this.cmdChooseSource.Location = new System.Drawing.Point(128, 3);
			this.cmdChooseSource.Name = "cmdChooseSource";
			this.cmdChooseSource.Size = new System.Drawing.Size(119, 23);
			this.cmdChooseSource.TabIndex = 12;
			this.cmdChooseSource.Text = "Choose Source .bin";
			this.cmdChooseSource.UseVisualStyleBackColor = true;
			this.cmdChooseSource.Click += new System.EventHandler(this.cmdChooseSource_Click);
			// 
			// lblSourceBin
			// 
			this.lblSourceBin.AutoSize = true;
			this.lblSourceBin.Location = new System.Drawing.Point(253, 8);
			this.lblSourceBin.Name = "lblSourceBin";
			this.lblSourceBin.Size = new System.Drawing.Size(35, 13);
			this.lblSourceBin.TabIndex = 13;
			this.lblSourceBin.Text = "label1";
			// 
			// cmdDoParse
			// 
			this.cmdDoParse.Location = new System.Drawing.Point(6, 55);
			this.cmdDoParse.Name = "cmdDoParse";
			this.cmdDoParse.Size = new System.Drawing.Size(119, 23);
			this.cmdDoParse.TabIndex = 14;
			this.cmdDoParse.Text = "Do Parse";
			this.cmdDoParse.UseVisualStyleBackColor = true;
			this.cmdDoParse.Click += new System.EventHandler(this.cmdDoParse_Click);
			// 
			// chkWriteJSON
			// 
			this.chkWriteJSON.AutoSize = true;
			this.chkWriteJSON.Checked = true;
			this.chkWriteJSON.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWriteJSON.Location = new System.Drawing.Point(358, 32);
			this.chkWriteJSON.Name = "chkWriteJSON";
			this.chkWriteJSON.Size = new System.Drawing.Size(117, 17);
			this.chkWriteJSON.TabIndex = 17;
			this.chkWriteJSON.Text = "Write JSON Output";
			this.chkWriteJSON.UseVisualStyleBackColor = true;
			// 
			// progressParseSource
			// 
			this.progressParseSource.Location = new System.Drawing.Point(128, 55);
			this.progressParseSource.Name = "progressParseSource";
			this.progressParseSource.Size = new System.Drawing.Size(224, 23);
			this.progressParseSource.Step = 1;
			this.progressParseSource.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressParseSource.TabIndex = 15;
			// 
			// progressWriteJSONOutput
			// 
			this.progressWriteJSONOutput.Location = new System.Drawing.Point(358, 55);
			this.progressWriteJSONOutput.Name = "progressWriteJSONOutput";
			this.progressWriteJSONOutput.Size = new System.Drawing.Size(224, 23);
			this.progressWriteJSONOutput.Step = 1;
			this.progressWriteJSONOutput.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressWriteJSONOutput.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "label1";
			// 
			// ParserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdChooseSource);
			this.Controls.Add(this.lblSourceBin);
			this.Controls.Add(this.cmdDoParse);
			this.Controls.Add(this.chkWriteJSON);
			this.Controls.Add(this.progressParseSource);
			this.Controls.Add(this.progressWriteJSONOutput);
			this.Name = "ParserControl";
			this.Size = new System.Drawing.Size(588, 84);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button cmdChooseSource;
		private System.Windows.Forms.Label lblSourceBin;
		private System.Windows.Forms.Button cmdDoParse;
		private System.Windows.Forms.CheckBox chkWriteJSON;
		private System.Windows.Forms.ProgressBar progressParseSource;
		private System.Windows.Forms.ProgressBar progressWriteJSONOutput;
		private System.Windows.Forms.Label label1;
	}
}
