namespace SnapAvi
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.axCVdisplay1 = new AxCVDISPLAYLib.AxCVdisplay();
			this.axCVimage1 = new AxCVIMAGELib.AxCVimage();
			this.btnLoad = new System.Windows.Forms.Button();
			this.chkLive = new System.Windows.Forms.CheckBox();
			this.chkSaveAvi = new System.Windows.Forms.CheckBox();
			this.btnSnap = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.axCVdisplay1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axCVimage1)).BeginInit();
			this.SuspendLayout();
			// 
			// axCVdisplay1
			// 
			this.axCVdisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.axCVdisplay1.Location = new System.Drawing.Point(89, 12);
			this.axCVdisplay1.Name = "axCVdisplay1";
			this.axCVdisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCVdisplay1.OcxState")));
			this.axCVdisplay1.Size = new System.Drawing.Size(183, 238);
			this.axCVdisplay1.TabIndex = 0;
			// 
			// axCVimage1
			// 
			this.axCVimage1.Enabled = true;
			this.axCVimage1.Location = new System.Drawing.Point(220, 186);
			this.axCVimage1.Name = "axCVimage1";
			this.axCVimage1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCVimage1.OcxState")));
			this.axCVimage1.Size = new System.Drawing.Size(32, 32);
			this.axCVimage1.TabIndex = 1;
			this.axCVimage1.ImageSnaped += new System.EventHandler(this.axCVimage1_ImageSnaped);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(12, 12);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(71, 23);
			this.btnLoad.TabIndex = 2;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// chkLive
			// 
			this.chkLive.AutoSize = true;
			this.chkLive.Location = new System.Drawing.Point(12, 70);
			this.chkLive.Name = "chkLive";
			this.chkLive.Size = new System.Drawing.Size(46, 17);
			this.chkLive.TabIndex = 3;
			this.chkLive.Text = "Live";
			this.chkLive.UseVisualStyleBackColor = true;
			this.chkLive.CheckedChanged += new System.EventHandler(this.chkLive_CheckedChanged);
			// 
			// chkSaveAvi
			// 
			this.chkSaveAvi.AutoSize = true;
			this.chkSaveAvi.Location = new System.Drawing.Point(12, 93);
			this.chkSaveAvi.Name = "chkSaveAvi";
			this.chkSaveAvi.Size = new System.Drawing.Size(51, 17);
			this.chkSaveAvi.TabIndex = 4;
			this.chkSaveAvi.Text = "Save";
			this.chkSaveAvi.UseVisualStyleBackColor = true;
			this.chkSaveAvi.CheckedChanged += new System.EventHandler(this.chkSaveAvi_CheckedChanged);
			// 
			// btnSnap
			// 
			this.btnSnap.Location = new System.Drawing.Point(12, 41);
			this.btnSnap.Name = "btnSnap";
			this.btnSnap.Size = new System.Drawing.Size(71, 23);
			this.btnSnap.TabIndex = 5;
			this.btnSnap.Text = "Snap";
			this.btnSnap.UseVisualStyleBackColor = true;
			this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.btnSnap);
			this.Controls.Add(this.chkSaveAvi);
			this.Controls.Add(this.chkLive);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.axCVimage1);
			this.Controls.Add(this.axCVdisplay1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.axCVdisplay1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axCVimage1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private AxCVDISPLAYLib.AxCVdisplay axCVdisplay1;
		private AxCVIMAGELib.AxCVimage axCVimage1;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.CheckBox chkLive;
		private System.Windows.Forms.CheckBox chkSaveAvi;
		private System.Windows.Forms.Button btnSnap;
	}
}

