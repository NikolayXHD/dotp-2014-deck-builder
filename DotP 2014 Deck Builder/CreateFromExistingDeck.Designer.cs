namespace RSN.DotP
{
	partial class CreateFromExistingDeck
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
			this.dgvDecks = new System.Windows.Forms.DataGridView();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdApply = new System.Windows.Forms.Button();
			this.cmnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuiColumns = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.dgvDecks)).BeginInit();
			this.cmnuContext.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvDecks
			// 
			this.dgvDecks.AllowUserToAddRows = false;
			this.dgvDecks.AllowUserToDeleteRows = false;
			this.dgvDecks.AllowUserToOrderColumns = true;
			this.dgvDecks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvDecks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDecks.Location = new System.Drawing.Point(12, 12);
			this.dgvDecks.MultiSelect = false;
			this.dgvDecks.Name = "dgvDecks";
			this.dgvDecks.RowHeadersVisible = false;
			this.dgvDecks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvDecks.Size = new System.Drawing.Size(398, 176);
			this.dgvDecks.TabIndex = 0;
			this.dgvDecks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDecks_CellDoubleClick);
			this.dgvDecks.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDecks_ColumnHeaderMouseClick);
			this.dgvDecks.SelectionChanged += new System.EventHandler(this.dgvDecks_SelectionChanged);
			this.dgvDecks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDecks_MouseClick);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(315, 194);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(95, 21);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Tag = "CANCEL";
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(214, 194);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(95, 21);
			this.cmdApply.TabIndex = 1;
			this.cmdApply.Tag = "APPLY";
			this.cmdApply.Text = "Apply";
			this.cmdApply.UseVisualStyleBackColor = true;
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// cmnuContext
			// 
			this.cmnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiColumns});
			this.cmnuContext.Name = "cmnuContext";
			this.cmnuContext.Size = new System.Drawing.Size(123, 26);
			// 
			// cmnuiColumns
			// 
			this.cmnuiColumns.Name = "cmnuiColumns";
			this.cmnuiColumns.Size = new System.Drawing.Size(152, 22);
			this.cmnuiColumns.Tag = "COLUMNS";
			this.cmnuiColumns.Text = "Columns";
			// 
			// CreateFromExistingDeck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(422, 227);
			this.ControlBox = false;
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdApply);
			this.Controls.Add(this.dgvDecks);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateFromExistingDeck";
			this.ShowInTaskbar = false;
			this.Tag = "CHOOSE_DECK";
			this.Text = "Choose Deck to Create from ...";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateFromExistingDeck_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.dgvDecks)).EndInit();
			this.cmnuContext.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvDecks;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.ContextMenuStrip cmnuContext;
		private System.Windows.Forms.ToolStripMenuItem cmnuiColumns;
	}
}