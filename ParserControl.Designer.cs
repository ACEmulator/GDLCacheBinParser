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
			this.cmdExportJSON = new System.Windows.Forms.Button();
			this.progressExportJSON = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdChooseSource
			// 
			this.cmdChooseSource.Location = new System.Drawing.Point(185, 3);
			this.cmdChooseSource.Name = "cmdChooseSource";
			this.cmdChooseSource.Size = new System.Drawing.Size(119, 23);
			this.cmdChooseSource.TabIndex = 12;
			this.cmdChooseSource.Text = "Choose Source .raw";
			this.cmdChooseSource.UseVisualStyleBackColor = true;
			this.cmdChooseSource.Click += new System.EventHandler(this.cmdChooseSource_Click);
			// 
			// lblSourceBin
			// 
			this.lblSourceBin.AutoSize = true;
			this.lblSourceBin.Location = new System.Drawing.Point(3, 32);
			this.lblSourceBin.Name = "lblSourceBin";
			this.lblSourceBin.Size = new System.Drawing.Size(35, 13);
			this.lblSourceBin.TabIndex = 13;
			this.lblSourceBin.Text = "label1";
			// 
			// cmdExportJSON
			// 
			this.cmdExportJSON.Location = new System.Drawing.Point(6, 55);
			this.cmdExportJSON.Name = "cmdExportJSON";
			this.cmdExportJSON.Size = new System.Drawing.Size(100, 23);
			this.cmdExportJSON.TabIndex = 14;
			this.cmdExportJSON.Text = "Export JSON";
			this.cmdExportJSON.UseVisualStyleBackColor = true;
			this.cmdExportJSON.Click += new System.EventHandler(this.cmdExportJSON_Click);
			// 
			// progressExportJSON
			// 
			this.progressExportJSON.Location = new System.Drawing.Point(112, 55);
			this.progressExportJSON.Name = "progressExportJSON";
			this.progressExportJSON.Size = new System.Drawing.Size(150, 23);
			this.progressExportJSON.Step = 1;
			this.progressExportJSON.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressExportJSON.TabIndex = 16;
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
			this.Controls.Add(this.cmdExportJSON);
			this.Controls.Add(this.progressExportJSON);
			this.Name = "ParserControl";
			this.Size = new System.Drawing.Size(588, 84);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button cmdChooseSource;
		private System.Windows.Forms.Label lblSourceBin;
		private System.Windows.Forms.Button cmdExportJSON;
		private System.Windows.Forms.ProgressBar progressExportJSON;
		private System.Windows.Forms.Label label1;
    }
}
