namespace RSN.DotP
{
	partial class IdBlockInput
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
			this.lblIdBlockToGenerate = new System.Windows.Forms.Label();
			this.numIdBlock = new System.Windows.Forms.NumericUpDown();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdApply = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numIdBlock)).BeginInit();
			this.SuspendLayout();
			// 
			// lblIdBlockToGenerate
			// 
			this.lblIdBlockToGenerate.AutoSize = true;
			this.lblIdBlockToGenerate.Location = new System.Drawing.Point(12, 14);
			this.lblIdBlockToGenerate.Name = "lblIdBlockToGenerate";
			this.lblIdBlockToGenerate.Size = new System.Drawing.Size(108, 13);
			this.lblIdBlockToGenerate.TabIndex = 0;
			this.lblIdBlockToGenerate.Tag = "ID_BLOCK_TO_GENERATE";
			this.lblIdBlockToGenerate.Text = "ID Block to generate:";
			// 
			// numIdBlock
			// 
			this.numIdBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numIdBlock.Location = new System.Drawing.Point(153, 12);
			this.numIdBlock.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numIdBlock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numIdBlock.Name = "numIdBlock";
			this.numIdBlock.Size = new System.Drawing.Size(81, 20);
			this.numIdBlock.TabIndex = 1;
			this.numIdBlock.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(139, 38);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(95, 21);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Tag = "CANCEL";
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(38, 38);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(95, 21);
			this.cmdApply.TabIndex = 2;
			this.cmdApply.Tag = "APPLY";
			this.cmdApply.Text = "Apply";
			this.cmdApply.UseVisualStyleBackColor = true;
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// IdBlockInput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 71);
			this.ControlBox = false;
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdApply);
			this.Controls.Add(this.numIdBlock);
			this.Controls.Add(this.lblIdBlockToGenerate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "IdBlockInput";
			this.Tag = "ID_BLOCK_INPUT";
			this.Text = "ID Block Input";
			((System.ComponentModel.ISupportInitialize)(this.numIdBlock)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblIdBlockToGenerate;
		private System.Windows.Forms.NumericUpDown numIdBlock;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdApply;
	}
}