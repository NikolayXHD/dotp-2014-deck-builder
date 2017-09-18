namespace RSN.DotP
{
	partial class Options
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
			this.gbGeneral = new System.Windows.Forms.GroupBox();
			this.chkIncludeMipMaps = new System.Windows.Forms.CheckBox();
			this.chkPreShuffleOnExport = new System.Windows.Forms.CheckBox();
			this.chkBasicScreenChecks = new System.Windows.Forms.CheckBox();
			this.cboLanguage = new System.Windows.Forms.ComboBox();
			this.lblLanguage = new System.Windows.Forms.Label();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.txtGameDir = new System.Windows.Forms.TextBox();
			this.lblGameDir = new System.Windows.Forms.Label();
			this.cmdApply = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lblSchemeType = new System.Windows.Forms.Label();
			this.cboSchemeType = new System.Windows.Forms.ComboBox();
			this.lblMinDigits = new System.Windows.Forms.Label();
			this.numMinDigits = new System.Windows.Forms.NumericUpDown();
			this.lblMinId = new System.Windows.Forms.Label();
			this.numMinId = new System.Windows.Forms.NumericUpDown();
			this.lblMaxId = new System.Windows.Forms.Label();
			this.numMaxId = new System.Windows.Forms.NumericUpDown();
			this.lblDeckChange = new System.Windows.Forms.Label();
			this.numDeckChange = new System.Windows.Forms.NumericUpDown();
			this.lblLandPoolChange = new System.Windows.Forms.Label();
			this.gbIdScheme = new System.Windows.Forms.GroupBox();
			this.chkIdBlock = new System.Windows.Forms.CheckBox();
			this.numIdBlock = new System.Windows.Forms.NumericUpDown();
			this.numChosenId = new System.Windows.Forms.NumericUpDown();
			this.lblPromoUnlockId = new System.Windows.Forms.Label();
			this.lblRegularUnlockId = new System.Windows.Forms.Label();
			this.lblLandPoolId = new System.Windows.Forms.Label();
			this.lblDeckId = new System.Windows.Forms.Label();
			this.lblIdExample = new System.Windows.Forms.Label();
			this.numPromoUnlockChange = new System.Windows.Forms.NumericUpDown();
			this.lblPromoUnlockChange = new System.Windows.Forms.Label();
			this.numRegularUnlockChange = new System.Windows.Forms.NumericUpDown();
			this.lblRegularUnlockChange = new System.Windows.Forms.Label();
			this.numLandPoolChange = new System.Windows.Forms.NumericUpDown();
			this.chkAlwaysExportPersonalities = new System.Windows.Forms.CheckBox();
			this.gbGeneral.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMinDigits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinId)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMaxId)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDeckChange)).BeginInit();
			this.gbIdScheme.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numIdBlock)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numChosenId)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPromoUnlockChange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numRegularUnlockChange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numLandPoolChange)).BeginInit();
			this.SuspendLayout();
			// 
			// gbGeneral
			// 
			this.gbGeneral.Controls.Add(this.chkAlwaysExportPersonalities);
			this.gbGeneral.Controls.Add(this.chkIncludeMipMaps);
			this.gbGeneral.Controls.Add(this.chkPreShuffleOnExport);
			this.gbGeneral.Controls.Add(this.chkBasicScreenChecks);
			this.gbGeneral.Controls.Add(this.cboLanguage);
			this.gbGeneral.Controls.Add(this.lblLanguage);
			this.gbGeneral.Controls.Add(this.cmdBrowse);
			this.gbGeneral.Controls.Add(this.txtGameDir);
			this.gbGeneral.Controls.Add(this.lblGameDir);
			this.gbGeneral.Location = new System.Drawing.Point(12, 12);
			this.gbGeneral.Name = "gbGeneral";
			this.gbGeneral.Size = new System.Drawing.Size(398, 177);
			this.gbGeneral.TabIndex = 0;
			this.gbGeneral.TabStop = false;
			this.gbGeneral.Tag = "GENERAL_OPTIONS";
			this.gbGeneral.Text = "General Options";
			// 
			// chkIncludeMipMaps
			// 
			this.chkIncludeMipMaps.AutoSize = true;
			this.chkIncludeMipMaps.Location = new System.Drawing.Point(6, 131);
			this.chkIncludeMipMaps.Name = "chkIncludeMipMaps";
			this.chkIncludeMipMaps.Size = new System.Drawing.Size(241, 17);
			this.chkIncludeMipMaps.TabIndex = 5;
			this.chkIncludeMipMaps.Tag = "INCLUDE_MIPMAPS_TDX";
			this.chkIncludeMipMaps.Text = "Include MipMaps in Compressed TDX Images";
			this.chkIncludeMipMaps.UseVisualStyleBackColor = true;
			// 
			// chkPreShuffleOnExport
			// 
			this.chkPreShuffleOnExport.AutoSize = true;
			this.chkPreShuffleOnExport.Location = new System.Drawing.Point(6, 108);
			this.chkPreShuffleOnExport.Name = "chkPreShuffleOnExport";
			this.chkPreShuffleOnExport.Size = new System.Drawing.Size(155, 17);
			this.chkPreShuffleOnExport.TabIndex = 4;
			this.chkPreShuffleOnExport.Tag = "PRE_SHUFFLE_ON_EXPORT";
			this.chkPreShuffleOnExport.Text = "Pre-Shuffle Deck on Export";
			this.chkPreShuffleOnExport.UseVisualStyleBackColor = true;
			// 
			// chkBasicScreenChecks
			// 
			this.chkBasicScreenChecks.AutoSize = true;
			this.chkBasicScreenChecks.Location = new System.Drawing.Point(6, 85);
			this.chkBasicScreenChecks.Name = "chkBasicScreenChecks";
			this.chkBasicScreenChecks.Size = new System.Drawing.Size(279, 17);
			this.chkBasicScreenChecks.TabIndex = 3;
			this.chkBasicScreenChecks.Tag = "PERFORM_BASIC_SCREEN_CHECKS";
			this.chkBasicScreenChecks.Text = "Perform basic screen checks when restoring windows";
			this.chkBasicScreenChecks.UseVisualStyleBackColor = true;
			// 
			// cboLanguage
			// 
			this.cboLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLanguage.FormattingEnabled = true;
			this.cboLanguage.Location = new System.Drawing.Point(70, 58);
			this.cboLanguage.Name = "cboLanguage";
			this.cboLanguage.Size = new System.Drawing.Size(322, 21);
			this.cboLanguage.TabIndex = 2;
			this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
			// 
			// lblLanguage
			// 
			this.lblLanguage.AutoSize = true;
			this.lblLanguage.Location = new System.Drawing.Point(6, 61);
			this.lblLanguage.Name = "lblLanguage";
			this.lblLanguage.Size = new System.Drawing.Size(58, 13);
			this.lblLanguage.TabIndex = 3;
			this.lblLanguage.Tag = "LANGAUGE";
			this.lblLanguage.Text = "Language:";
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdBrowse.Location = new System.Drawing.Point(297, 32);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(95, 20);
			this.cmdBrowse.TabIndex = 1;
			this.cmdBrowse.Tag = "BROWSE";
			this.cmdBrowse.Text = "Browse ...";
			this.cmdBrowse.UseVisualStyleBackColor = true;
			this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
			// 
			// txtGameDir
			// 
			this.txtGameDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGameDir.Location = new System.Drawing.Point(6, 32);
			this.txtGameDir.Name = "txtGameDir";
			this.txtGameDir.Size = new System.Drawing.Size(285, 20);
			this.txtGameDir.TabIndex = 0;
			// 
			// lblGameDir
			// 
			this.lblGameDir.AutoSize = true;
			this.lblGameDir.Location = new System.Drawing.Point(6, 16);
			this.lblGameDir.Name = "lblGameDir";
			this.lblGameDir.Size = new System.Drawing.Size(210, 13);
			this.lblGameDir.TabIndex = 0;
			this.lblGameDir.Tag = "DOTP_2014_DIRECTORY";
			this.lblGameDir.Text = "Duels of the Planeswalkers 2014 Directory:";
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(214, 517);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(95, 21);
			this.cmdApply.TabIndex = 2;
			this.cmdApply.Tag = "APPLY";
			this.cmdApply.Text = "Apply";
			this.cmdApply.UseVisualStyleBackColor = true;
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(315, 517);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(95, 21);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Tag = "CANCEL";
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// lblSchemeType
			// 
			this.lblSchemeType.AutoSize = true;
			this.lblSchemeType.Location = new System.Drawing.Point(6, 16);
			this.lblSchemeType.Name = "lblSchemeType";
			this.lblSchemeType.Size = new System.Drawing.Size(76, 13);
			this.lblSchemeType.TabIndex = 0;
			this.lblSchemeType.Tag = "SCHEME_TYPE";
			this.lblSchemeType.Text = "Scheme Type:";
			// 
			// cboSchemeType
			// 
			this.cboSchemeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSchemeType.FormattingEnabled = true;
			this.cboSchemeType.Location = new System.Drawing.Point(9, 32);
			this.cboSchemeType.Name = "cboSchemeType";
			this.cboSchemeType.Size = new System.Drawing.Size(383, 21);
			this.cboSchemeType.TabIndex = 0;
			this.cboSchemeType.SelectedIndexChanged += new System.EventHandler(this.cboSchemeType_SelectedIndexChanged);
			// 
			// lblMinDigits
			// 
			this.lblMinDigits.AutoSize = true;
			this.lblMinDigits.Location = new System.Drawing.Point(6, 61);
			this.lblMinDigits.Name = "lblMinDigits";
			this.lblMinDigits.Size = new System.Drawing.Size(92, 13);
			this.lblMinDigits.TabIndex = 2;
			this.lblMinDigits.Tag = "SCHEME_ID_MINIMUM_DIGITS";
			this.lblMinDigits.Text = "Id Minimum Digits:";
			// 
			// numMinDigits
			// 
			this.numMinDigits.Location = new System.Drawing.Point(104, 59);
			this.numMinDigits.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numMinDigits.Name = "numMinDigits";
			this.numMinDigits.Size = new System.Drawing.Size(288, 20);
			this.numMinDigits.TabIndex = 1;
			this.numMinDigits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numMinDigits.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numMinDigits.ValueChanged += new System.EventHandler(this.numMinDigits_ValueChanged);
			// 
			// lblMinId
			// 
			this.lblMinId.AutoSize = true;
			this.lblMinId.Location = new System.Drawing.Point(6, 87);
			this.lblMinId.Name = "lblMinId";
			this.lblMinId.Size = new System.Drawing.Size(120, 13);
			this.lblMinId.TabIndex = 4;
			this.lblMinId.Tag = "MINIMUM_ID";
			this.lblMinId.Text = "Minimum Acceptable Id:";
			// 
			// numMinId
			// 
			this.numMinId.Location = new System.Drawing.Point(132, 85);
			this.numMinId.Maximum = new decimal(new int[] {
            999999998,
            0,
            0,
            0});
			this.numMinId.Name = "numMinId";
			this.numMinId.Size = new System.Drawing.Size(260, 20);
			this.numMinId.TabIndex = 2;
			this.numMinId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numMinId.ValueChanged += new System.EventHandler(this.numMinId_ValueChanged);
			// 
			// lblMaxId
			// 
			this.lblMaxId.AutoSize = true;
			this.lblMaxId.Location = new System.Drawing.Point(6, 113);
			this.lblMaxId.Name = "lblMaxId";
			this.lblMaxId.Size = new System.Drawing.Size(123, 13);
			this.lblMaxId.TabIndex = 6;
			this.lblMaxId.Tag = "MAXIMUM_ID";
			this.lblMaxId.Text = "Maximum Acceptable Id:";
			// 
			// numMaxId
			// 
			this.numMaxId.Location = new System.Drawing.Point(135, 111);
			this.numMaxId.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
			this.numMaxId.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numMaxId.Name = "numMaxId";
			this.numMaxId.Size = new System.Drawing.Size(257, 20);
			this.numMaxId.TabIndex = 3;
			this.numMaxId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numMaxId.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numMaxId.ValueChanged += new System.EventHandler(this.numMaxId_ValueChanged);
			// 
			// lblDeckChange
			// 
			this.lblDeckChange.AutoSize = true;
			this.lblDeckChange.Location = new System.Drawing.Point(6, 165);
			this.lblDeckChange.Name = "lblDeckChange";
			this.lblDeckChange.Size = new System.Drawing.Size(141, 13);
			this.lblDeckChange.TabIndex = 8;
			this.lblDeckChange.Tag = "DECK_ID_CHANGE";
			this.lblDeckChange.Text = "Change to make to Deck Id:";
			// 
			// numDeckChange
			// 
			this.numDeckChange.Location = new System.Drawing.Point(153, 163);
			this.numDeckChange.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numDeckChange.Name = "numDeckChange";
			this.numDeckChange.Size = new System.Drawing.Size(239, 20);
			this.numDeckChange.TabIndex = 6;
			this.numDeckChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numDeckChange.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numDeckChange.ValueChanged += new System.EventHandler(this.numDeckChange_ValueChanged);
			// 
			// lblLandPoolChange
			// 
			this.lblLandPoolChange.AutoSize = true;
			this.lblLandPoolChange.Location = new System.Drawing.Point(6, 191);
			this.lblLandPoolChange.Name = "lblLandPoolChange";
			this.lblLandPoolChange.Size = new System.Drawing.Size(163, 13);
			this.lblLandPoolChange.TabIndex = 10;
			this.lblLandPoolChange.Tag = "LAND_POOL_ID_CHANGE";
			this.lblLandPoolChange.Text = "Change to make to Land Pool Id:";
			// 
			// gbIdScheme
			// 
			this.gbIdScheme.Controls.Add(this.chkIdBlock);
			this.gbIdScheme.Controls.Add(this.numIdBlock);
			this.gbIdScheme.Controls.Add(this.numChosenId);
			this.gbIdScheme.Controls.Add(this.lblPromoUnlockId);
			this.gbIdScheme.Controls.Add(this.lblRegularUnlockId);
			this.gbIdScheme.Controls.Add(this.lblLandPoolId);
			this.gbIdScheme.Controls.Add(this.lblDeckId);
			this.gbIdScheme.Controls.Add(this.lblIdExample);
			this.gbIdScheme.Controls.Add(this.numPromoUnlockChange);
			this.gbIdScheme.Controls.Add(this.lblPromoUnlockChange);
			this.gbIdScheme.Controls.Add(this.numRegularUnlockChange);
			this.gbIdScheme.Controls.Add(this.lblRegularUnlockChange);
			this.gbIdScheme.Controls.Add(this.numLandPoolChange);
			this.gbIdScheme.Controls.Add(this.lblLandPoolChange);
			this.gbIdScheme.Controls.Add(this.numDeckChange);
			this.gbIdScheme.Controls.Add(this.lblDeckChange);
			this.gbIdScheme.Controls.Add(this.numMaxId);
			this.gbIdScheme.Controls.Add(this.lblMaxId);
			this.gbIdScheme.Controls.Add(this.numMinId);
			this.gbIdScheme.Controls.Add(this.lblMinId);
			this.gbIdScheme.Controls.Add(this.numMinDigits);
			this.gbIdScheme.Controls.Add(this.lblMinDigits);
			this.gbIdScheme.Controls.Add(this.cboSchemeType);
			this.gbIdScheme.Controls.Add(this.lblSchemeType);
			this.gbIdScheme.Location = new System.Drawing.Point(12, 195);
			this.gbIdScheme.Name = "gbIdScheme";
			this.gbIdScheme.Size = new System.Drawing.Size(398, 316);
			this.gbIdScheme.TabIndex = 1;
			this.gbIdScheme.TabStop = false;
			this.gbIdScheme.Tag = "ID_SCHEME";
			this.gbIdScheme.Text = "Id Numbering Scheme";
			// 
			// chkIdBlock
			// 
			this.chkIdBlock.AutoSize = true;
			this.chkIdBlock.Location = new System.Drawing.Point(9, 138);
			this.chkIdBlock.Name = "chkIdBlock";
			this.chkIdBlock.Size = new System.Drawing.Size(90, 17);
			this.chkIdBlock.TabIndex = 4;
			this.chkIdBlock.Tag = "USE_ID_BLOCK";
			this.chkIdBlock.Text = "Use Id Block:";
			this.chkIdBlock.UseVisualStyleBackColor = true;
			this.chkIdBlock.CheckedChanged += new System.EventHandler(this.chkIdBlock_CheckedChanged);
			// 
			// numIdBlock
			// 
			this.numIdBlock.Location = new System.Drawing.Point(105, 137);
			this.numIdBlock.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numIdBlock.Name = "numIdBlock";
			this.numIdBlock.Size = new System.Drawing.Size(287, 20);
			this.numIdBlock.TabIndex = 5;
			this.numIdBlock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numIdBlock.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numIdBlock.ValueChanged += new System.EventHandler(this.numIdBlock_ValueChanged);
			// 
			// numChosenId
			// 
			this.numChosenId.Location = new System.Drawing.Point(146, 264);
			this.numChosenId.Maximum = new decimal(new int[] {
            1999999999,
            0,
            0,
            0});
			this.numChosenId.Name = "numChosenId";
			this.numChosenId.Size = new System.Drawing.Size(82, 20);
			this.numChosenId.TabIndex = 10;
			this.numChosenId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numChosenId.ValueChanged += new System.EventHandler(this.numChosenId_ValueChanged);
			// 
			// lblPromoUnlockId
			// 
			this.lblPromoUnlockId.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblPromoUnlockId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblPromoUnlockId.Location = new System.Drawing.Point(316, 289);
			this.lblPromoUnlockId.Name = "lblPromoUnlockId";
			this.lblPromoUnlockId.Size = new System.Drawing.Size(76, 20);
			this.lblPromoUnlockId.TabIndex = 21;
			this.lblPromoUnlockId.Text = "9999999999";
			this.lblPromoUnlockId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblRegularUnlockId
			// 
			this.lblRegularUnlockId.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblRegularUnlockId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRegularUnlockId.Location = new System.Drawing.Point(234, 289);
			this.lblRegularUnlockId.Name = "lblRegularUnlockId";
			this.lblRegularUnlockId.Size = new System.Drawing.Size(76, 20);
			this.lblRegularUnlockId.TabIndex = 20;
			this.lblRegularUnlockId.Text = "9999999999";
			this.lblRegularUnlockId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblLandPoolId
			// 
			this.lblLandPoolId.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblLandPoolId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblLandPoolId.Location = new System.Drawing.Point(316, 264);
			this.lblLandPoolId.Name = "lblLandPoolId";
			this.lblLandPoolId.Size = new System.Drawing.Size(76, 20);
			this.lblLandPoolId.TabIndex = 19;
			this.lblLandPoolId.Text = "9999999999";
			this.lblLandPoolId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDeckId
			// 
			this.lblDeckId.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblDeckId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDeckId.Location = new System.Drawing.Point(234, 264);
			this.lblDeckId.Name = "lblDeckId";
			this.lblDeckId.Size = new System.Drawing.Size(76, 20);
			this.lblDeckId.TabIndex = 18;
			this.lblDeckId.Text = "9999999999";
			this.lblDeckId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblIdExample
			// 
			this.lblIdExample.AutoSize = true;
			this.lblIdExample.Location = new System.Drawing.Point(6, 268);
			this.lblIdExample.Name = "lblIdExample";
			this.lblIdExample.Size = new System.Drawing.Size(104, 13);
			this.lblIdExample.TabIndex = 16;
			this.lblIdExample.Tag = "ID_SCHEME_EXAMPLE";
			this.lblIdExample.Text = "Id Scheme Example:";
			// 
			// numPromoUnlockChange
			// 
			this.numPromoUnlockChange.Location = new System.Drawing.Point(199, 241);
			this.numPromoUnlockChange.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numPromoUnlockChange.Name = "numPromoUnlockChange";
			this.numPromoUnlockChange.Size = new System.Drawing.Size(193, 20);
			this.numPromoUnlockChange.TabIndex = 9;
			this.numPromoUnlockChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPromoUnlockChange.Value = new decimal(new int[] {
            10003,
            0,
            0,
            0});
			this.numPromoUnlockChange.ValueChanged += new System.EventHandler(this.numPromoUnlockChange_ValueChanged);
			// 
			// lblPromoUnlockChange
			// 
			this.lblPromoUnlockChange.AutoSize = true;
			this.lblPromoUnlockChange.Location = new System.Drawing.Point(6, 243);
			this.lblPromoUnlockChange.Name = "lblPromoUnlockChange";
			this.lblPromoUnlockChange.Size = new System.Drawing.Size(187, 13);
			this.lblPromoUnlockChange.TabIndex = 14;
			this.lblPromoUnlockChange.Tag = "PROMO_UNLOCK_ID_CHANGE";
			this.lblPromoUnlockChange.Text = "Change to make to Promo Unlocks Id:";
			// 
			// numRegularUnlockChange
			// 
			this.numRegularUnlockChange.Location = new System.Drawing.Point(206, 215);
			this.numRegularUnlockChange.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numRegularUnlockChange.Name = "numRegularUnlockChange";
			this.numRegularUnlockChange.Size = new System.Drawing.Size(186, 20);
			this.numRegularUnlockChange.TabIndex = 8;
			this.numRegularUnlockChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numRegularUnlockChange.Value = new decimal(new int[] {
            10002,
            0,
            0,
            0});
			this.numRegularUnlockChange.ValueChanged += new System.EventHandler(this.numRegularUnlockChange_ValueChanged);
			// 
			// lblRegularUnlockChange
			// 
			this.lblRegularUnlockChange.AutoSize = true;
			this.lblRegularUnlockChange.Location = new System.Drawing.Point(6, 217);
			this.lblRegularUnlockChange.Name = "lblRegularUnlockChange";
			this.lblRegularUnlockChange.Size = new System.Drawing.Size(194, 13);
			this.lblRegularUnlockChange.TabIndex = 12;
			this.lblRegularUnlockChange.Tag = "REGULAR_UNLOCK_ID_CHANGE";
			this.lblRegularUnlockChange.Text = "Change to make to Regular Unlocks Id:";
			// 
			// numLandPoolChange
			// 
			this.numLandPoolChange.Location = new System.Drawing.Point(175, 189);
			this.numLandPoolChange.Maximum = new decimal(new int[] {
            999999900,
            0,
            0,
            0});
			this.numLandPoolChange.Name = "numLandPoolChange";
			this.numLandPoolChange.Size = new System.Drawing.Size(217, 20);
			this.numLandPoolChange.TabIndex = 7;
			this.numLandPoolChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numLandPoolChange.Value = new decimal(new int[] {
            10001,
            0,
            0,
            0});
			this.numLandPoolChange.ValueChanged += new System.EventHandler(this.numLandPoolChange_ValueChanged);
			// 
			// chkAlwaysExportPersonalities
			// 
			this.chkAlwaysExportPersonalities.AutoSize = true;
			this.chkAlwaysExportPersonalities.Location = new System.Drawing.Point(6, 154);
			this.chkAlwaysExportPersonalities.Name = "chkAlwaysExportPersonalities";
			this.chkAlwaysExportPersonalities.Size = new System.Drawing.Size(167, 17);
			this.chkAlwaysExportPersonalities.TabIndex = 6;
			this.chkAlwaysExportPersonalities.Tag = "ALWAYS_EXPORT_PERSONALITIES";
			this.chkAlwaysExportPersonalities.Text = "Always Export AI Personalities";
			this.chkAlwaysExportPersonalities.UseVisualStyleBackColor = true;
			// 
			// Options
			// 
			this.AcceptButton = this.cmdApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(422, 550);
			this.ControlBox = false;
			this.Controls.Add(this.gbIdScheme);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdApply);
			this.Controls.Add(this.gbGeneral);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.Tag = "FORM_CAPTION_OPTIONS";
			this.Text = "Options";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
			this.gbGeneral.ResumeLayout(false);
			this.gbGeneral.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMinDigits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinId)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMaxId)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDeckChange)).EndInit();
			this.gbIdScheme.ResumeLayout(false);
			this.gbIdScheme.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numIdBlock)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numChosenId)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPromoUnlockChange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numRegularUnlockChange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numLandPoolChange)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbGeneral;
		private System.Windows.Forms.ComboBox cboLanguage;
		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.Button cmdBrowse;
		private System.Windows.Forms.TextBox txtGameDir;
		private System.Windows.Forms.Label lblGameDir;
		private System.Windows.Forms.CheckBox chkBasicScreenChecks;
		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblSchemeType;
		private System.Windows.Forms.ComboBox cboSchemeType;
		private System.Windows.Forms.Label lblMinDigits;
		private System.Windows.Forms.NumericUpDown numMinDigits;
		private System.Windows.Forms.Label lblMinId;
		private System.Windows.Forms.NumericUpDown numMinId;
		private System.Windows.Forms.Label lblMaxId;
		private System.Windows.Forms.NumericUpDown numMaxId;
		private System.Windows.Forms.Label lblDeckChange;
		private System.Windows.Forms.NumericUpDown numDeckChange;
		private System.Windows.Forms.Label lblLandPoolChange;
		private System.Windows.Forms.GroupBox gbIdScheme;
		private System.Windows.Forms.NumericUpDown numChosenId;
		private System.Windows.Forms.Label lblPromoUnlockId;
		private System.Windows.Forms.Label lblRegularUnlockId;
		private System.Windows.Forms.Label lblLandPoolId;
		private System.Windows.Forms.Label lblDeckId;
		private System.Windows.Forms.Label lblIdExample;
		private System.Windows.Forms.NumericUpDown numPromoUnlockChange;
		private System.Windows.Forms.Label lblPromoUnlockChange;
		private System.Windows.Forms.NumericUpDown numRegularUnlockChange;
		private System.Windows.Forms.Label lblRegularUnlockChange;
		private System.Windows.Forms.NumericUpDown numLandPoolChange;
		private System.Windows.Forms.CheckBox chkPreShuffleOnExport;
		private System.Windows.Forms.CheckBox chkIdBlock;
		private System.Windows.Forms.NumericUpDown numIdBlock;
		private System.Windows.Forms.CheckBox chkIncludeMipMaps;
		private System.Windows.Forms.CheckBox chkAlwaysExportPersonalities;
	}
}