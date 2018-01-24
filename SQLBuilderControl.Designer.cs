namespace PhatACCacheBinParser
{
	partial class SQLBuilderControl
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
            this.cmdParseAll = new System.Windows.Forms.Button();
            this.progressParseSources = new System.Windows.Forms.ProgressBar();
            this.cmdAction1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cmdAction2 = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // cmdParseAll
            // 
            this.cmdParseAll.Location = new System.Drawing.Point(3, 3);
            this.cmdParseAll.Name = "cmdParseAll";
            this.cmdParseAll.Size = new System.Drawing.Size(119, 23);
            this.cmdParseAll.TabIndex = 16;
            this.cmdParseAll.Text = "Parse All";
            this.cmdParseAll.UseVisualStyleBackColor = true;
            this.cmdParseAll.Click += new System.EventHandler(this.cmdParseAll_Click);
            // 
            // progressParseSources
            // 
            this.progressParseSources.Location = new System.Drawing.Point(125, 3);
            this.progressParseSources.Name = "progressParseSources";
            this.progressParseSources.Size = new System.Drawing.Size(224, 23);
            this.progressParseSources.Step = 1;
            this.progressParseSources.TabIndex = 17;
            // 
            // cmdAction1
            // 
            this.cmdAction1.Enabled = false;
            this.cmdAction1.Location = new System.Drawing.Point(3, 41);
            this.cmdAction1.Name = "cmdAction1";
            this.cmdAction1.Size = new System.Drawing.Size(119, 23);
            this.cmdAction1.TabIndex = 18;
            this.cmdAction1.Text = "9 Weenies";
            this.cmdAction1.UseVisualStyleBackColor = true;
            this.cmdAction1.Click += new System.EventHandler(this.cmdAction1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(125, 41);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(224, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 19;
            // 
            // cmdAction2
            // 
            this.cmdAction2.Enabled = false;
            this.cmdAction2.Location = new System.Drawing.Point(3, 80);
            this.cmdAction2.Name = "cmdAction2";
            this.cmdAction2.Size = new System.Drawing.Size(119, 23);
            this.cmdAction2.TabIndex = 20;
            this.cmdAction2.Text = "6 Landblocks";
            this.cmdAction2.UseVisualStyleBackColor = true;
            this.cmdAction2.Click += new System.EventHandler(this.cmdAction2_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(125, 80);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(224, 23);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 21;
            // 
            // SQLBuilderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdAction2);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.cmdAction1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cmdParseAll);
            this.Controls.Add(this.progressParseSources);
            this.Name = "SQLBuilderControl";
            this.Size = new System.Drawing.Size(353, 397);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdParseAll;
		private System.Windows.Forms.ProgressBar progressParseSources;
		private System.Windows.Forms.Button cmdAction1;
		private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button cmdAction2;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}
