namespace RSN.DotP
{
	partial class ViewCard
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
			this.components = new System.ComponentModel.Container();
			this.picCard = new System.Windows.Forms.PictureBox();
			this.txtCardXml = new System.Windows.Forms.TextBox();
			this.tvwXml = new System.Windows.Forms.TreeView();
			this.cmnuPictures = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuiExport = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuiExportPng = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuiExportTdx = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.picCard)).BeginInit();
			this.cmnuPictures.SuspendLayout();
			this.SuspendLayout();
			// 
			// picCard
			// 
			this.picCard.Location = new System.Drawing.Point(12, 12);
			this.picCard.Name = "picCard";
			this.picCard.Size = new System.Drawing.Size(356, 512);
			this.picCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picCard.TabIndex = 7;
			this.picCard.TabStop = false;
			this.picCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCard_MouseUp);
			// 
			// txtCardXml
			// 
			this.txtCardXml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCardXml.Location = new System.Drawing.Point(374, 12);
			this.txtCardXml.MaxLength = 400000;
			this.txtCardXml.Multiline = true;
			this.txtCardXml.Name = "txtCardXml";
			this.txtCardXml.ReadOnly = true;
			this.txtCardXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtCardXml.Size = new System.Drawing.Size(765, 655);
			this.txtCardXml.TabIndex = 0;
			this.txtCardXml.WordWrap = false;
			// 
			// tvwXml
			// 
			this.tvwXml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tvwXml.Location = new System.Drawing.Point(12, 530);
			this.tvwXml.Name = "tvwXml";
			this.tvwXml.Size = new System.Drawing.Size(356, 137);
			this.tvwXml.TabIndex = 8;
			this.tvwXml.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwXml_NodeMouseClick);
			// 
			// cmnuPictures
			// 
			this.cmnuPictures.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExport});
			this.cmnuPictures.Name = "cmnuPictures";
			this.cmnuPictures.Size = new System.Drawing.Size(153, 48);
			// 
			// cmnuiExport
			// 
			this.cmnuiExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExportPng,
            this.cmnuiExportTdx});
			this.cmnuiExport.Name = "cmnuiExport";
			this.cmnuiExport.Size = new System.Drawing.Size(152, 22);
			this.cmnuiExport.Tag = "MENU_EXPORT_IMAGE";
			this.cmnuiExport.Text = "&Export Image";
			// 
			// cmnuiExportPng
			// 
			this.cmnuiExportPng.Name = "cmnuiExportPng";
			this.cmnuiExportPng.Size = new System.Drawing.Size(110, 22);
			this.cmnuiExportPng.Text = "&PNG ...";
			this.cmnuiExportPng.Click += new System.EventHandler(this.cmnuiExportPng_Click);
			// 
			// cmnuiExportTdx
			// 
			this.cmnuiExportTdx.Name = "cmnuiExportTdx";
			this.cmnuiExportTdx.Size = new System.Drawing.Size(110, 22);
			this.cmnuiExportTdx.Text = "&TDX ...";
			this.cmnuiExportTdx.Click += new System.EventHandler(this.cmnuiExportTdx_Click);
			// 
			// ViewCard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1151, 679);
			this.Controls.Add(this.tvwXml);
			this.Controls.Add(this.txtCardXml);
			this.Controls.Add(this.picCard);
			this.Name = "ViewCard";
			this.Text = "ViewCard";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewCard_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.picCard)).EndInit();
			this.cmnuPictures.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picCard;
		private System.Windows.Forms.TextBox txtCardXml;
		private System.Windows.Forms.TreeView tvwXml;
		private System.Windows.Forms.ContextMenuStrip cmnuPictures;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExport;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportPng;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportTdx;
	}
}