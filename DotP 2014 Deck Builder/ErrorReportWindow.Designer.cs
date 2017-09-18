namespace RSN.DotP
{
	partial class ErrorReportWindow
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
			this.txtErrors = new System.Windows.Forms.TextBox();
			this.gbFiltering = new System.Windows.Forms.GroupBox();
			this.rbNoFiltering = new System.Windows.Forms.RadioButton();
			this.rbTypeFiltering = new System.Windows.Forms.RadioButton();
			this.gbTypeFilters = new System.Windows.Forms.GroupBox();
			this.chkCardProblems = new System.Windows.Forms.CheckBox();
			this.chkDeckProblems = new System.Windows.Forms.CheckBox();
			this.chkOtherProblems = new System.Windows.Forms.CheckBox();
			this.rbSeverityFiltering = new System.Windows.Forms.RadioButton();
			this.gbSeverityFiltering = new System.Windows.Forms.GroupBox();
			this.chkCritical = new System.Windows.Forms.CheckBox();
			this.chkHigh = new System.Windows.Forms.CheckBox();
			this.chkMedium = new System.Windows.Forms.CheckBox();
			this.chkLow = new System.Windows.Forms.CheckBox();
			this.chkZero = new System.Windows.Forms.CheckBox();
			this.gbFiltering.SuspendLayout();
			this.gbTypeFilters.SuspendLayout();
			this.gbSeverityFiltering.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtErrors
			// 
			this.txtErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtErrors.Location = new System.Drawing.Point(158, 12);
			this.txtErrors.Multiline = true;
			this.txtErrors.Name = "txtErrors";
			this.txtErrors.ReadOnly = true;
			this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtErrors.Size = new System.Drawing.Size(377, 323);
			this.txtErrors.TabIndex = 0;
			this.txtErrors.WordWrap = false;
			// 
			// gbFiltering
			// 
			this.gbFiltering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.gbFiltering.Controls.Add(this.gbSeverityFiltering);
			this.gbFiltering.Controls.Add(this.rbSeverityFiltering);
			this.gbFiltering.Controls.Add(this.gbTypeFilters);
			this.gbFiltering.Controls.Add(this.rbTypeFiltering);
			this.gbFiltering.Controls.Add(this.rbNoFiltering);
			this.gbFiltering.Location = new System.Drawing.Point(12, 12);
			this.gbFiltering.Name = "gbFiltering";
			this.gbFiltering.Size = new System.Drawing.Size(140, 323);
			this.gbFiltering.TabIndex = 1;
			this.gbFiltering.TabStop = false;
			this.gbFiltering.Text = "Filtering Options";
			// 
			// rbNoFiltering
			// 
			this.rbNoFiltering.AutoSize = true;
			this.rbNoFiltering.Checked = true;
			this.rbNoFiltering.Location = new System.Drawing.Point(6, 19);
			this.rbNoFiltering.Name = "rbNoFiltering";
			this.rbNoFiltering.Size = new System.Drawing.Size(78, 17);
			this.rbNoFiltering.TabIndex = 0;
			this.rbNoFiltering.TabStop = true;
			this.rbNoFiltering.Text = "No Filtering";
			this.rbNoFiltering.UseVisualStyleBackColor = true;
			this.rbNoFiltering.CheckedChanged += new System.EventHandler(this.ChangeFiltering);
			// 
			// rbTypeFiltering
			// 
			this.rbTypeFiltering.AutoSize = true;
			this.rbTypeFiltering.Location = new System.Drawing.Point(6, 42);
			this.rbTypeFiltering.Name = "rbTypeFiltering";
			this.rbTypeFiltering.Size = new System.Drawing.Size(88, 17);
			this.rbTypeFiltering.TabIndex = 1;
			this.rbTypeFiltering.Text = "Type Filtering";
			this.rbTypeFiltering.UseVisualStyleBackColor = true;
			this.rbTypeFiltering.CheckedChanged += new System.EventHandler(this.ChangeFiltering);
			// 
			// gbTypeFilters
			// 
			this.gbTypeFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbTypeFilters.Controls.Add(this.chkOtherProblems);
			this.gbTypeFilters.Controls.Add(this.chkDeckProblems);
			this.gbTypeFilters.Controls.Add(this.chkCardProblems);
			this.gbTypeFilters.Location = new System.Drawing.Point(6, 65);
			this.gbTypeFilters.Name = "gbTypeFilters";
			this.gbTypeFilters.Size = new System.Drawing.Size(128, 88);
			this.gbTypeFilters.TabIndex = 2;
			this.gbTypeFilters.TabStop = false;
			// 
			// chkCardProblems
			// 
			this.chkCardProblems.AutoSize = true;
			this.chkCardProblems.Checked = true;
			this.chkCardProblems.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCardProblems.Location = new System.Drawing.Point(6, 19);
			this.chkCardProblems.Name = "chkCardProblems";
			this.chkCardProblems.Size = new System.Drawing.Size(94, 17);
			this.chkCardProblems.TabIndex = 0;
			this.chkCardProblems.Text = "Card Problems";
			this.chkCardProblems.UseVisualStyleBackColor = true;
			this.chkCardProblems.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkDeckProblems
			// 
			this.chkDeckProblems.AutoSize = true;
			this.chkDeckProblems.Checked = true;
			this.chkDeckProblems.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDeckProblems.Location = new System.Drawing.Point(6, 42);
			this.chkDeckProblems.Name = "chkDeckProblems";
			this.chkDeckProblems.Size = new System.Drawing.Size(98, 17);
			this.chkDeckProblems.TabIndex = 1;
			this.chkDeckProblems.Text = "Deck Problems";
			this.chkDeckProblems.UseVisualStyleBackColor = true;
			this.chkDeckProblems.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkOtherProblems
			// 
			this.chkOtherProblems.AutoSize = true;
			this.chkOtherProblems.Checked = true;
			this.chkOtherProblems.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOtherProblems.Location = new System.Drawing.Point(6, 65);
			this.chkOtherProblems.Name = "chkOtherProblems";
			this.chkOtherProblems.Size = new System.Drawing.Size(98, 17);
			this.chkOtherProblems.TabIndex = 2;
			this.chkOtherProblems.Text = "Other Problems";
			this.chkOtherProblems.UseVisualStyleBackColor = true;
			this.chkOtherProblems.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// rbSeverityFiltering
			// 
			this.rbSeverityFiltering.AutoSize = true;
			this.rbSeverityFiltering.Location = new System.Drawing.Point(6, 159);
			this.rbSeverityFiltering.Name = "rbSeverityFiltering";
			this.rbSeverityFiltering.Size = new System.Drawing.Size(102, 17);
			this.rbSeverityFiltering.TabIndex = 3;
			this.rbSeverityFiltering.Text = "Severity Filtering";
			this.rbSeverityFiltering.UseVisualStyleBackColor = true;
			this.rbSeverityFiltering.CheckedChanged += new System.EventHandler(this.ChangeFiltering);
			// 
			// gbSeverityFiltering
			// 
			this.gbSeverityFiltering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbSeverityFiltering.Controls.Add(this.chkZero);
			this.gbSeverityFiltering.Controls.Add(this.chkLow);
			this.gbSeverityFiltering.Controls.Add(this.chkMedium);
			this.gbSeverityFiltering.Controls.Add(this.chkHigh);
			this.gbSeverityFiltering.Controls.Add(this.chkCritical);
			this.gbSeverityFiltering.Location = new System.Drawing.Point(6, 182);
			this.gbSeverityFiltering.Name = "gbSeverityFiltering";
			this.gbSeverityFiltering.Size = new System.Drawing.Size(128, 134);
			this.gbSeverityFiltering.TabIndex = 4;
			this.gbSeverityFiltering.TabStop = false;
			// 
			// chkCritical
			// 
			this.chkCritical.AutoSize = true;
			this.chkCritical.Checked = true;
			this.chkCritical.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCritical.Location = new System.Drawing.Point(6, 19);
			this.chkCritical.Name = "chkCritical";
			this.chkCritical.Size = new System.Drawing.Size(57, 17);
			this.chkCritical.TabIndex = 0;
			this.chkCritical.Text = "Critical";
			this.chkCritical.UseVisualStyleBackColor = true;
			this.chkCritical.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkHigh
			// 
			this.chkHigh.AutoSize = true;
			this.chkHigh.Checked = true;
			this.chkHigh.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkHigh.Location = new System.Drawing.Point(6, 42);
			this.chkHigh.Name = "chkHigh";
			this.chkHigh.Size = new System.Drawing.Size(48, 17);
			this.chkHigh.TabIndex = 1;
			this.chkHigh.Text = "High";
			this.chkHigh.UseVisualStyleBackColor = true;
			this.chkHigh.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkMedium
			// 
			this.chkMedium.AutoSize = true;
			this.chkMedium.Checked = true;
			this.chkMedium.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMedium.Location = new System.Drawing.Point(6, 65);
			this.chkMedium.Name = "chkMedium";
			this.chkMedium.Size = new System.Drawing.Size(63, 17);
			this.chkMedium.TabIndex = 2;
			this.chkMedium.Text = "Medium";
			this.chkMedium.UseVisualStyleBackColor = true;
			this.chkMedium.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkLow
			// 
			this.chkLow.AutoSize = true;
			this.chkLow.Checked = true;
			this.chkLow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLow.Location = new System.Drawing.Point(6, 88);
			this.chkLow.Name = "chkLow";
			this.chkLow.Size = new System.Drawing.Size(46, 17);
			this.chkLow.TabIndex = 3;
			this.chkLow.Text = "Low";
			this.chkLow.UseVisualStyleBackColor = true;
			this.chkLow.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// chkZero
			// 
			this.chkZero.AutoSize = true;
			this.chkZero.Location = new System.Drawing.Point(6, 111);
			this.chkZero.Name = "chkZero";
			this.chkZero.Size = new System.Drawing.Size(48, 17);
			this.chkZero.TabIndex = 4;
			this.chkZero.Text = "Zero";
			this.chkZero.UseVisualStyleBackColor = true;
			this.chkZero.CheckedChanged += new System.EventHandler(this.UpdateFilter);
			// 
			// ErrorReportWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(547, 347);
			this.Controls.Add(this.gbFiltering);
			this.Controls.Add(this.txtErrors);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(563, 383);
			this.Name = "ErrorReportWindow";
			this.ShowIcon = false;
			this.Text = "Errors During Load ...";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ErrorReportWindow_FormClosing);
			this.Shown += new System.EventHandler(this.ErrorReportWindow_Shown);
			this.gbFiltering.ResumeLayout(false);
			this.gbFiltering.PerformLayout();
			this.gbTypeFilters.ResumeLayout(false);
			this.gbTypeFilters.PerformLayout();
			this.gbSeverityFiltering.ResumeLayout(false);
			this.gbSeverityFiltering.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtErrors;
		private System.Windows.Forms.GroupBox gbFiltering;
		private System.Windows.Forms.GroupBox gbSeverityFiltering;
		private System.Windows.Forms.CheckBox chkCritical;
		private System.Windows.Forms.RadioButton rbSeverityFiltering;
		private System.Windows.Forms.GroupBox gbTypeFilters;
		private System.Windows.Forms.CheckBox chkOtherProblems;
		private System.Windows.Forms.CheckBox chkDeckProblems;
		private System.Windows.Forms.CheckBox chkCardProblems;
		private System.Windows.Forms.RadioButton rbTypeFiltering;
		private System.Windows.Forms.RadioButton rbNoFiltering;
		private System.Windows.Forms.CheckBox chkZero;
		private System.Windows.Forms.CheckBox chkLow;
		private System.Windows.Forms.CheckBox chkMedium;
		private System.Windows.Forms.CheckBox chkHigh;
	}
}