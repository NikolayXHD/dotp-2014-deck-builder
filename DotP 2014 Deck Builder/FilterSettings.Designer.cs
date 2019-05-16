namespace RSN.DotP
{
	partial class FilterSettings
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
			this.gbSupertype = new System.Windows.Forms.GroupBox();
			this.chkWorld = new System.Windows.Forms.CheckBox();
			this.chkSnow = new System.Windows.Forms.CheckBox();
			this.chkLegendary = new System.Windows.Forms.CheckBox();
			this.chkBasic = new System.Windows.Forms.CheckBox();
			this.gbTypes = new System.Windows.Forms.GroupBox();
			this.cboTypeFilter = new System.Windows.Forms.ComboBox();
			this.scTypes = new System.Windows.Forms.SplitContainer();
			this.chkPhenomenon = new System.Windows.Forms.CheckBox();
			this.chkLand = new System.Windows.Forms.CheckBox();
			this.chkInstant = new System.Windows.Forms.CheckBox();
			this.chkEnchantment = new System.Windows.Forms.CheckBox();
			this.chkCreature = new System.Windows.Forms.CheckBox();
			this.chkArtifact = new System.Windows.Forms.CheckBox();
			this.chkVanguard = new System.Windows.Forms.CheckBox();
			this.chkTribal = new System.Windows.Forms.CheckBox();
			this.chkSorcery = new System.Windows.Forms.CheckBox();
			this.chkScheme = new System.Windows.Forms.CheckBox();
			this.chkPlaneswalker = new System.Windows.Forms.CheckBox();
			this.chkPlane = new System.Windows.Forms.CheckBox();
			this.gbSubTypes = new System.Windows.Forms.GroupBox();
			this.cboSubTypeFilter = new System.Windows.Forms.ComboBox();
			this.txtSubType = new System.Windows.Forms.TextBox();
			this.chkCheckSubTypes = new System.Windows.Forms.CheckBox();
			this.gbColour = new System.Windows.Forms.GroupBox();
			this.cboColourFilter = new System.Windows.Forms.ComboBox();
			this.scColour = new System.Windows.Forms.SplitContainer();
			this.chkRed = new System.Windows.Forms.CheckBox();
			this.chkGreen = new System.Windows.Forms.CheckBox();
			this.chkBlue = new System.Windows.Forms.CheckBox();
			this.chkBlack = new System.Windows.Forms.CheckBox();
			this.chkMultiColoured = new System.Windows.Forms.CheckBox();
			this.chkColourless = new System.Windows.Forms.CheckBox();
			this.chkWhite = new System.Windows.Forms.CheckBox();
			this.gbName = new System.Windows.Forms.GroupBox();
			this.cboNameFilter = new System.Windows.Forms.ComboBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.chkName = new System.Windows.Forms.CheckBox();
			this.gbRarity = new System.Windows.Forms.GroupBox();
			this.chkRaritySpecial = new System.Windows.Forms.CheckBox();
			this.chkRarityMythic = new System.Windows.Forms.CheckBox();
			this.chkRarityRare = new System.Windows.Forms.CheckBox();
			this.chkRarityUncommon = new System.Windows.Forms.CheckBox();
			this.chkRarityCommon = new System.Windows.Forms.CheckBox();
			this.chkRarityLand = new System.Windows.Forms.CheckBox();
			this.chkRarityToken = new System.Windows.Forms.CheckBox();
			this.gbCmc = new System.Windows.Forms.GroupBox();
			this.cboCmcFilter = new System.Windows.Forms.ComboBox();
			this.numCmc = new System.Windows.Forms.NumericUpDown();
			this.gbPowerToughness = new System.Windows.Forms.GroupBox();
			this.txtToughness = new System.Windows.Forms.TextBox();
			this.txtPower = new System.Windows.Forms.TextBox();
			this.cboToughnessFilter = new System.Windows.Forms.ComboBox();
			this.cboPowerFilter = new System.Windows.Forms.ComboBox();
			this.chkCheckPowerToughness = new System.Windows.Forms.CheckBox();
			this.gbAbilities = new System.Windows.Forms.GroupBox();
			this.chkCheckGrantedAbilities = new System.Windows.Forms.CheckBox();
			this.cboAbilitiesFilter = new System.Windows.Forms.ComboBox();
			this.txtAbilities = new System.Windows.Forms.TextBox();
			this.chkCheckAbilities = new System.Windows.Forms.CheckBox();
			this.gbExtra = new System.Windows.Forms.GroupBox();
			this.chkUnderscores = new System.Windows.Forms.CheckBox();
			this.chkPhyrexianMana = new System.Windows.Forms.CheckBox();
			this.chkHybridMana = new System.Windows.Forms.CheckBox();
			this.chkRegularMana = new System.Windows.Forms.CheckBox();
			this.chkCardsThatCreateTokens = new System.Windows.Forms.CheckBox();
			this.chkTokens = new System.Windows.Forms.CheckBox();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdApply = new System.Windows.Forms.Button();
			this.cmdReset = new System.Windows.Forms.Button();
			this.gbSupertype.SuspendLayout();
			this.gbTypes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scTypes)).BeginInit();
			this.scTypes.Panel1.SuspendLayout();
			this.scTypes.Panel2.SuspendLayout();
			this.scTypes.SuspendLayout();
			this.gbSubTypes.SuspendLayout();
			this.gbColour.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scColour)).BeginInit();
			this.scColour.Panel1.SuspendLayout();
			this.scColour.Panel2.SuspendLayout();
			this.scColour.SuspendLayout();
			this.gbName.SuspendLayout();
			this.gbRarity.SuspendLayout();
			this.gbCmc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numCmc)).BeginInit();
			this.gbPowerToughness.SuspendLayout();
			this.gbAbilities.SuspendLayout();
			this.gbExtra.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSupertype
			// 
			this.gbSupertype.Controls.Add(this.chkWorld);
			this.gbSupertype.Controls.Add(this.chkSnow);
			this.gbSupertype.Controls.Add(this.chkLegendary);
			this.gbSupertype.Controls.Add(this.chkBasic);
			this.gbSupertype.Location = new System.Drawing.Point(244, 12);
			this.gbSupertype.Name = "gbSupertype";
			this.gbSupertype.Size = new System.Drawing.Size(190, 111);
			this.gbSupertype.TabIndex = 3;
			this.gbSupertype.TabStop = false;
			this.gbSupertype.Tag = "SUPERTYPE";
			this.gbSupertype.Text = "Super Types";
			// 
			// chkWorld
			// 
			this.chkWorld.AutoSize = true;
			this.chkWorld.Location = new System.Drawing.Point(6, 88);
			this.chkWorld.Name = "chkWorld";
			this.chkWorld.Size = new System.Drawing.Size(54, 17);
			this.chkWorld.TabIndex = 3;
			this.chkWorld.Tag = "CARD_SUPERTYPE_WORLD";
			this.chkWorld.Text = "World";
			this.chkWorld.UseVisualStyleBackColor = true;
			// 
			// chkSnow
			// 
			this.chkSnow.AutoSize = true;
			this.chkSnow.Location = new System.Drawing.Point(6, 65);
			this.chkSnow.Name = "chkSnow";
			this.chkSnow.Size = new System.Drawing.Size(53, 17);
			this.chkSnow.TabIndex = 2;
			this.chkSnow.Tag = "CARD_SUPERTYPE_SNOW";
			this.chkSnow.Text = "Snow";
			this.chkSnow.UseVisualStyleBackColor = true;
			// 
			// chkLegendary
			// 
			this.chkLegendary.AutoSize = true;
			this.chkLegendary.Location = new System.Drawing.Point(6, 42);
			this.chkLegendary.Name = "chkLegendary";
			this.chkLegendary.Size = new System.Drawing.Size(76, 17);
			this.chkLegendary.TabIndex = 1;
			this.chkLegendary.Tag = "CARD_SUPERTYPE_LEGENDARY";
			this.chkLegendary.Text = "Legendary";
			this.chkLegendary.UseVisualStyleBackColor = true;
			// 
			// chkBasic
			// 
			this.chkBasic.AutoSize = true;
			this.chkBasic.Location = new System.Drawing.Point(6, 19);
			this.chkBasic.Name = "chkBasic";
			this.chkBasic.Size = new System.Drawing.Size(52, 17);
			this.chkBasic.TabIndex = 0;
			this.chkBasic.Tag = "CARD_SUPERTYPE_BASIC";
			this.chkBasic.Text = "Basic";
			this.chkBasic.UseVisualStyleBackColor = true;
			// 
			// gbTypes
			// 
			this.gbTypes.Controls.Add(this.cboTypeFilter);
			this.gbTypes.Controls.Add(this.scTypes);
			this.gbTypes.Location = new System.Drawing.Point(12, 263);
			this.gbTypes.Name = "gbTypes";
			this.gbTypes.Size = new System.Drawing.Size(226, 190);
			this.gbTypes.TabIndex = 2;
			this.gbTypes.TabStop = false;
			this.gbTypes.Tag = "TYPES";
			this.gbTypes.Text = "Card Types";
			// 
			// cboTypeFilter
			// 
			this.cboTypeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTypeFilter.FormattingEnabled = true;
			this.cboTypeFilter.Location = new System.Drawing.Point(6, 163);
			this.cboTypeFilter.Name = "cboTypeFilter";
			this.cboTypeFilter.Size = new System.Drawing.Size(214, 21);
			this.cboTypeFilter.TabIndex = 12;
			// 
			// scTypes
			// 
			this.scTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.scTypes.IsSplitterFixed = true;
			this.scTypes.Location = new System.Drawing.Point(6, 19);
			this.scTypes.Name = "scTypes";
			// 
			// scTypes.Panel1
			// 
			this.scTypes.Panel1.Controls.Add(this.chkPhenomenon);
			this.scTypes.Panel1.Controls.Add(this.chkLand);
			this.scTypes.Panel1.Controls.Add(this.chkInstant);
			this.scTypes.Panel1.Controls.Add(this.chkEnchantment);
			this.scTypes.Panel1.Controls.Add(this.chkCreature);
			this.scTypes.Panel1.Controls.Add(this.chkArtifact);
			// 
			// scTypes.Panel2
			// 
			this.scTypes.Panel2.Controls.Add(this.chkVanguard);
			this.scTypes.Panel2.Controls.Add(this.chkTribal);
			this.scTypes.Panel2.Controls.Add(this.chkSorcery);
			this.scTypes.Panel2.Controls.Add(this.chkScheme);
			this.scTypes.Panel2.Controls.Add(this.chkPlaneswalker);
			this.scTypes.Panel2.Controls.Add(this.chkPlane);
			this.scTypes.Size = new System.Drawing.Size(214, 138);
			this.scTypes.SplitterDistance = 104;
			this.scTypes.TabIndex = 0;
			// 
			// chkPhenomenon
			// 
			this.chkPhenomenon.AutoSize = true;
			this.chkPhenomenon.Location = new System.Drawing.Point(3, 118);
			this.chkPhenomenon.Name = "chkPhenomenon";
			this.chkPhenomenon.Size = new System.Drawing.Size(89, 17);
			this.chkPhenomenon.TabIndex = 5;
			this.chkPhenomenon.Tag = "CARD_TYPE_PHENOMENON";
			this.chkPhenomenon.Text = "Phenomenon";
			this.chkPhenomenon.UseVisualStyleBackColor = true;
			// 
			// chkLand
			// 
			this.chkLand.AutoSize = true;
			this.chkLand.Location = new System.Drawing.Point(3, 95);
			this.chkLand.Name = "chkLand";
			this.chkLand.Size = new System.Drawing.Size(50, 17);
			this.chkLand.TabIndex = 4;
			this.chkLand.Tag = "CARD_TYPE_LAND";
			this.chkLand.Text = "Land";
			this.chkLand.UseVisualStyleBackColor = true;
			// 
			// chkInstant
			// 
			this.chkInstant.AutoSize = true;
			this.chkInstant.Location = new System.Drawing.Point(3, 72);
			this.chkInstant.Name = "chkInstant";
			this.chkInstant.Size = new System.Drawing.Size(58, 17);
			this.chkInstant.TabIndex = 3;
			this.chkInstant.Tag = "CARD_TYPE_INSTANT";
			this.chkInstant.Text = "Instant";
			this.chkInstant.UseVisualStyleBackColor = true;
			// 
			// chkEnchantment
			// 
			this.chkEnchantment.AutoSize = true;
			this.chkEnchantment.Location = new System.Drawing.Point(3, 49);
			this.chkEnchantment.Name = "chkEnchantment";
			this.chkEnchantment.Size = new System.Drawing.Size(89, 17);
			this.chkEnchantment.TabIndex = 2;
			this.chkEnchantment.Tag = "CARD_TYPE_ENCHANTMENT";
			this.chkEnchantment.Text = "Enchantment";
			this.chkEnchantment.UseVisualStyleBackColor = true;
			// 
			// chkCreature
			// 
			this.chkCreature.AutoSize = true;
			this.chkCreature.Location = new System.Drawing.Point(3, 26);
			this.chkCreature.Name = "chkCreature";
			this.chkCreature.Size = new System.Drawing.Size(66, 17);
			this.chkCreature.TabIndex = 1;
			this.chkCreature.Tag = "CARD_TYPE_CREATURE";
			this.chkCreature.Text = "Creature";
			this.chkCreature.UseVisualStyleBackColor = true;
			// 
			// chkArtifact
			// 
			this.chkArtifact.AutoSize = true;
			this.chkArtifact.Location = new System.Drawing.Point(3, 3);
			this.chkArtifact.Name = "chkArtifact";
			this.chkArtifact.Size = new System.Drawing.Size(59, 17);
			this.chkArtifact.TabIndex = 0;
			this.chkArtifact.Tag = "CARD_TYPE_ARTIFACT";
			this.chkArtifact.Text = "Artifact";
			this.chkArtifact.UseVisualStyleBackColor = true;
			// 
			// chkVanguard
			// 
			this.chkVanguard.AutoSize = true;
			this.chkVanguard.Location = new System.Drawing.Point(3, 118);
			this.chkVanguard.Name = "chkVanguard";
			this.chkVanguard.Size = new System.Drawing.Size(72, 17);
			this.chkVanguard.TabIndex = 11;
			this.chkVanguard.Tag = "CARD_TYPE_VANGUARD";
			this.chkVanguard.Text = "Vanguard";
			this.chkVanguard.UseVisualStyleBackColor = true;
			// 
			// chkTribal
			// 
			this.chkTribal.AutoSize = true;
			this.chkTribal.Location = new System.Drawing.Point(3, 95);
			this.chkTribal.Name = "chkTribal";
			this.chkTribal.Size = new System.Drawing.Size(52, 17);
			this.chkTribal.TabIndex = 10;
			this.chkTribal.Tag = "CARD_TYPE_TRIBAL";
			this.chkTribal.Text = "Tribal";
			this.chkTribal.UseVisualStyleBackColor = true;
			// 
			// chkSorcery
			// 
			this.chkSorcery.AutoSize = true;
			this.chkSorcery.Location = new System.Drawing.Point(3, 72);
			this.chkSorcery.Name = "chkSorcery";
			this.chkSorcery.Size = new System.Drawing.Size(62, 17);
			this.chkSorcery.TabIndex = 9;
			this.chkSorcery.Tag = "CARD_TYPE_SORCERY";
			this.chkSorcery.Text = "Sorcery";
			this.chkSorcery.UseVisualStyleBackColor = true;
			// 
			// chkScheme
			// 
			this.chkScheme.AutoSize = true;
			this.chkScheme.Location = new System.Drawing.Point(3, 49);
			this.chkScheme.Name = "chkScheme";
			this.chkScheme.Size = new System.Drawing.Size(65, 17);
			this.chkScheme.TabIndex = 8;
			this.chkScheme.Tag = "CARD_TYPE_SCHEME";
			this.chkScheme.Text = "Scheme";
			this.chkScheme.UseVisualStyleBackColor = true;
			// 
			// chkPlaneswalker
			// 
			this.chkPlaneswalker.AutoSize = true;
			this.chkPlaneswalker.Location = new System.Drawing.Point(3, 26);
			this.chkPlaneswalker.Name = "chkPlaneswalker";
			this.chkPlaneswalker.Size = new System.Drawing.Size(89, 17);
			this.chkPlaneswalker.TabIndex = 7;
			this.chkPlaneswalker.Tag = "CARD_TYPE_PLANESWALKER";
			this.chkPlaneswalker.Text = "Planeswalker";
			this.chkPlaneswalker.UseVisualStyleBackColor = true;
			// 
			// chkPlane
			// 
			this.chkPlane.AutoSize = true;
			this.chkPlane.Location = new System.Drawing.Point(3, 3);
			this.chkPlane.Name = "chkPlane";
			this.chkPlane.Size = new System.Drawing.Size(53, 17);
			this.chkPlane.TabIndex = 6;
			this.chkPlane.Tag = "CARD_TYPE_PLANE";
			this.chkPlane.Text = "Plane";
			this.chkPlane.UseVisualStyleBackColor = true;
			// 
			// gbSubTypes
			// 
			this.gbSubTypes.Controls.Add(this.cboSubTypeFilter);
			this.gbSubTypes.Controls.Add(this.txtSubType);
			this.gbSubTypes.Controls.Add(this.chkCheckSubTypes);
			this.gbSubTypes.Location = new System.Drawing.Point(244, 129);
			this.gbSubTypes.Name = "gbSubTypes";
			this.gbSubTypes.Size = new System.Drawing.Size(190, 95);
			this.gbSubTypes.TabIndex = 4;
			this.gbSubTypes.TabStop = false;
			this.gbSubTypes.Tag = "SUB_TYPES";
			this.gbSubTypes.Text = "Sub Types";
			// 
			// cboSubTypeFilter
			// 
			this.cboSubTypeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboSubTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSubTypeFilter.FormattingEnabled = true;
			this.cboSubTypeFilter.Location = new System.Drawing.Point(6, 68);
			this.cboSubTypeFilter.Name = "cboSubTypeFilter";
			this.cboSubTypeFilter.Size = new System.Drawing.Size(178, 21);
			this.cboSubTypeFilter.TabIndex = 2;
			// 
			// txtSubType
			// 
			this.txtSubType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSubType.Location = new System.Drawing.Point(6, 42);
			this.txtSubType.Name = "txtSubType";
			this.txtSubType.Size = new System.Drawing.Size(178, 20);
			this.txtSubType.TabIndex = 1;
			// 
			// chkCheckSubTypes
			// 
			this.chkCheckSubTypes.AutoSize = true;
			this.chkCheckSubTypes.Location = new System.Drawing.Point(6, 19);
			this.chkCheckSubTypes.Name = "chkCheckSubTypes";
			this.chkCheckSubTypes.Size = new System.Drawing.Size(111, 17);
			this.chkCheckSubTypes.TabIndex = 0;
			this.chkCheckSubTypes.Tag = "CHECK_SUB_TYPE";
			this.chkCheckSubTypes.Text = "Check Sub Types";
			this.chkCheckSubTypes.UseVisualStyleBackColor = true;
			this.chkCheckSubTypes.CheckedChanged += new System.EventHandler(this.chkCheckSubTypes_CheckedChanged);
			// 
			// gbColour
			// 
			this.gbColour.Controls.Add(this.cboColourFilter);
			this.gbColour.Controls.Add(this.scColour);
			this.gbColour.Location = new System.Drawing.Point(12, 113);
			this.gbColour.Name = "gbColour";
			this.gbColour.Size = new System.Drawing.Size(226, 144);
			this.gbColour.TabIndex = 1;
			this.gbColour.TabStop = false;
			this.gbColour.Tag = "COLOUR";
			this.gbColour.Text = "Colour";
			// 
			// cboColourFilter
			// 
			this.cboColourFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboColourFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboColourFilter.FormattingEnabled = true;
			this.cboColourFilter.Location = new System.Drawing.Point(6, 117);
			this.cboColourFilter.Name = "cboColourFilter";
			this.cboColourFilter.Size = new System.Drawing.Size(214, 21);
			this.cboColourFilter.TabIndex = 7;
			// 
			// scColour
			// 
			this.scColour.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.scColour.IsSplitterFixed = true;
			this.scColour.Location = new System.Drawing.Point(6, 19);
			this.scColour.Name = "scColour";
			// 
			// scColour.Panel1
			// 
			this.scColour.Panel1.Controls.Add(this.chkRed);
			this.scColour.Panel1.Controls.Add(this.chkGreen);
			this.scColour.Panel1.Controls.Add(this.chkBlue);
			this.scColour.Panel1.Controls.Add(this.chkBlack);
			// 
			// scColour.Panel2
			// 
			this.scColour.Panel2.Controls.Add(this.chkMultiColoured);
			this.scColour.Panel2.Controls.Add(this.chkColourless);
			this.scColour.Panel2.Controls.Add(this.chkWhite);
			this.scColour.Size = new System.Drawing.Size(214, 92);
			this.scColour.SplitterDistance = 104;
			this.scColour.TabIndex = 3;
			// 
			// chkRed
			// 
			this.chkRed.AutoSize = true;
			this.chkRed.Location = new System.Drawing.Point(3, 72);
			this.chkRed.Name = "chkRed";
			this.chkRed.Size = new System.Drawing.Size(46, 17);
			this.chkRed.TabIndex = 3;
			this.chkRed.Tag = "MANA_RED";
			this.chkRed.Text = "Red";
			this.chkRed.UseVisualStyleBackColor = true;
			// 
			// chkGreen
			// 
			this.chkGreen.AutoSize = true;
			this.chkGreen.Location = new System.Drawing.Point(3, 49);
			this.chkGreen.Name = "chkGreen";
			this.chkGreen.Size = new System.Drawing.Size(55, 17);
			this.chkGreen.TabIndex = 2;
			this.chkGreen.Tag = "MANA_GREEN";
			this.chkGreen.Text = "Green";
			this.chkGreen.UseVisualStyleBackColor = true;
			// 
			// chkBlue
			// 
			this.chkBlue.AutoSize = true;
			this.chkBlue.Location = new System.Drawing.Point(3, 26);
			this.chkBlue.Name = "chkBlue";
			this.chkBlue.Size = new System.Drawing.Size(47, 17);
			this.chkBlue.TabIndex = 1;
			this.chkBlue.Tag = "MANA_BLUE";
			this.chkBlue.Text = "Blue";
			this.chkBlue.UseVisualStyleBackColor = true;
			// 
			// chkBlack
			// 
			this.chkBlack.AutoSize = true;
			this.chkBlack.Location = new System.Drawing.Point(3, 3);
			this.chkBlack.Name = "chkBlack";
			this.chkBlack.Size = new System.Drawing.Size(53, 17);
			this.chkBlack.TabIndex = 0;
			this.chkBlack.Tag = "MANA_BLACK";
			this.chkBlack.Text = "Black";
			this.chkBlack.UseVisualStyleBackColor = true;
			// 
			// chkMultiColoured
			// 
			this.chkMultiColoured.AutoSize = true;
			this.chkMultiColoured.Location = new System.Drawing.Point(3, 49);
			this.chkMultiColoured.Name = "chkMultiColoured";
			this.chkMultiColoured.Size = new System.Drawing.Size(93, 17);
			this.chkMultiColoured.TabIndex = 6;
			this.chkMultiColoured.Tag = "MANA_MULTICOLOUR";
			this.chkMultiColoured.Text = "Multi-Coloured";
			this.chkMultiColoured.UseVisualStyleBackColor = true;
			// 
			// chkColourless
			// 
			this.chkColourless.AutoSize = true;
			this.chkColourless.Location = new System.Drawing.Point(3, 26);
			this.chkColourless.Name = "chkColourless";
			this.chkColourless.Size = new System.Drawing.Size(74, 17);
			this.chkColourless.TabIndex = 5;
			this.chkColourless.Tag = "MANA_COLOURLESS";
			this.chkColourless.Text = "Colourless";
			this.chkColourless.UseVisualStyleBackColor = true;
			// 
			// chkWhite
			// 
			this.chkWhite.AutoSize = true;
			this.chkWhite.Location = new System.Drawing.Point(3, 3);
			this.chkWhite.Name = "chkWhite";
			this.chkWhite.Size = new System.Drawing.Size(54, 17);
			this.chkWhite.TabIndex = 4;
			this.chkWhite.Tag = "MANA_WHITE";
			this.chkWhite.Text = "White";
			this.chkWhite.UseVisualStyleBackColor = true;
			// 
			// gbName
			// 
			this.gbName.Controls.Add(this.cboNameFilter);
			this.gbName.Controls.Add(this.txtName);
			this.gbName.Controls.Add(this.chkName);
			this.gbName.Location = new System.Drawing.Point(12, 12);
			this.gbName.Name = "gbName";
			this.gbName.Size = new System.Drawing.Size(226, 95);
			this.gbName.TabIndex = 0;
			this.gbName.TabStop = false;
			this.gbName.Tag = "CARD_NAME";
			this.gbName.Text = "Card Name";
			// 
			// cboNameFilter
			// 
			this.cboNameFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboNameFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboNameFilter.FormattingEnabled = true;
			this.cboNameFilter.Location = new System.Drawing.Point(6, 68);
			this.cboNameFilter.Name = "cboNameFilter";
			this.cboNameFilter.Size = new System.Drawing.Size(214, 21);
			this.cboNameFilter.TabIndex = 2;
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(6, 42);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(214, 20);
			this.txtName.TabIndex = 1;
			// 
			// chkName
			// 
			this.chkName.AutoSize = true;
			this.chkName.Location = new System.Drawing.Point(6, 19);
			this.chkName.Name = "chkName";
			this.chkName.Size = new System.Drawing.Size(113, 17);
			this.chkName.TabIndex = 0;
			this.chkName.Tag = "CHECK_CARD_NAME";
			this.chkName.Text = "Check Card Name";
			this.chkName.UseVisualStyleBackColor = true;
			this.chkName.CheckedChanged += new System.EventHandler(this.chkName_CheckedChanged);
			// 
			// gbRarity
			// 
			this.gbRarity.Controls.Add(this.chkRaritySpecial);
			this.gbRarity.Controls.Add(this.chkRarityMythic);
			this.gbRarity.Controls.Add(this.chkRarityRare);
			this.gbRarity.Controls.Add(this.chkRarityUncommon);
			this.gbRarity.Controls.Add(this.chkRarityCommon);
			this.gbRarity.Controls.Add(this.chkRarityLand);
			this.gbRarity.Controls.Add(this.chkRarityToken);
			this.gbRarity.Location = new System.Drawing.Point(440, 12);
			this.gbRarity.Name = "gbRarity";
			this.gbRarity.Size = new System.Drawing.Size(200, 180);
			this.gbRarity.TabIndex = 7;
			this.gbRarity.TabStop = false;
			this.gbRarity.Tag = "RARITY";
			this.gbRarity.Text = "Rarity";
			// 
			// chkRaritySpecial
			// 
			this.chkRaritySpecial.AutoSize = true;
			this.chkRaritySpecial.Location = new System.Drawing.Point(6, 157);
			this.chkRaritySpecial.Name = "chkRaritySpecial";
			this.chkRaritySpecial.Size = new System.Drawing.Size(61, 17);
			this.chkRaritySpecial.TabIndex = 6;
			this.chkRaritySpecial.Tag = "RARITY_SPECIAL";
			this.chkRaritySpecial.Text = "Special";
			this.chkRaritySpecial.UseVisualStyleBackColor = true;
			// 
			// chkRarityMythic
			// 
			this.chkRarityMythic.AutoSize = true;
			this.chkRarityMythic.Location = new System.Drawing.Point(6, 134);
			this.chkRarityMythic.Name = "chkRarityMythic";
			this.chkRarityMythic.Size = new System.Drawing.Size(83, 17);
			this.chkRarityMythic.TabIndex = 5;
			this.chkRarityMythic.Tag = "RARITY_MYTHIC";
			this.chkRarityMythic.Text = "Mythic Rare";
			this.chkRarityMythic.UseVisualStyleBackColor = true;
			// 
			// chkRarityRare
			// 
			this.chkRarityRare.AutoSize = true;
			this.chkRarityRare.Location = new System.Drawing.Point(6, 111);
			this.chkRarityRare.Name = "chkRarityRare";
			this.chkRarityRare.Size = new System.Drawing.Size(49, 17);
			this.chkRarityRare.TabIndex = 4;
			this.chkRarityRare.Tag = "RARITY_RARE";
			this.chkRarityRare.Text = "Rare";
			this.chkRarityRare.UseVisualStyleBackColor = true;
			// 
			// chkRarityUncommon
			// 
			this.chkRarityUncommon.AutoSize = true;
			this.chkRarityUncommon.Location = new System.Drawing.Point(6, 88);
			this.chkRarityUncommon.Name = "chkRarityUncommon";
			this.chkRarityUncommon.Size = new System.Drawing.Size(80, 17);
			this.chkRarityUncommon.TabIndex = 3;
			this.chkRarityUncommon.Tag = "RARITY_UNCOMMON";
			this.chkRarityUncommon.Text = "Uncommon";
			this.chkRarityUncommon.UseVisualStyleBackColor = true;
			// 
			// chkRarityCommon
			// 
			this.chkRarityCommon.AutoSize = true;
			this.chkRarityCommon.Location = new System.Drawing.Point(6, 65);
			this.chkRarityCommon.Name = "chkRarityCommon";
			this.chkRarityCommon.Size = new System.Drawing.Size(67, 17);
			this.chkRarityCommon.TabIndex = 2;
			this.chkRarityCommon.Tag = "RARITY_COMMON";
			this.chkRarityCommon.Text = "Common";
			this.chkRarityCommon.UseVisualStyleBackColor = true;
			// 
			// chkRarityLand
			// 
			this.chkRarityLand.AutoSize = true;
			this.chkRarityLand.Location = new System.Drawing.Point(6, 42);
			this.chkRarityLand.Name = "chkRarityLand";
			this.chkRarityLand.Size = new System.Drawing.Size(50, 17);
			this.chkRarityLand.TabIndex = 1;
			this.chkRarityLand.Tag = "RARITY_LAND";
			this.chkRarityLand.Text = "Land";
			this.chkRarityLand.UseVisualStyleBackColor = true;
			// 
			// chkRarityToken
			// 
			this.chkRarityToken.AutoSize = true;
			this.chkRarityToken.Location = new System.Drawing.Point(6, 19);
			this.chkRarityToken.Name = "chkRarityToken";
			this.chkRarityToken.Size = new System.Drawing.Size(57, 17);
			this.chkRarityToken.TabIndex = 0;
			this.chkRarityToken.Tag = "RARITY_TOKEN";
			this.chkRarityToken.Text = "Token";
			this.chkRarityToken.UseVisualStyleBackColor = true;
			// 
			// gbCmc
			// 
			this.gbCmc.Controls.Add(this.cboCmcFilter);
			this.gbCmc.Controls.Add(this.numCmc);
			this.gbCmc.Location = new System.Drawing.Point(244, 230);
			this.gbCmc.Name = "gbCmc";
			this.gbCmc.Size = new System.Drawing.Size(190, 72);
			this.gbCmc.TabIndex = 5;
			this.gbCmc.TabStop = false;
			this.gbCmc.Tag = "CONVERTED_MANA_COST";
			this.gbCmc.Text = "Converted Mana Cost";
			// 
			// cboCmcFilter
			// 
			this.cboCmcFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboCmcFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCmcFilter.FormattingEnabled = true;
			this.cboCmcFilter.Location = new System.Drawing.Point(6, 45);
			this.cboCmcFilter.Name = "cboCmcFilter";
			this.cboCmcFilter.Size = new System.Drawing.Size(178, 21);
			this.cboCmcFilter.TabIndex = 1;
			// 
			// numCmc
			// 
			this.numCmc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numCmc.Location = new System.Drawing.Point(6, 19);
			this.numCmc.Name = "numCmc";
			this.numCmc.Size = new System.Drawing.Size(178, 20);
			this.numCmc.TabIndex = 0;
			this.numCmc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// gbPowerToughness
			// 
			this.gbPowerToughness.Controls.Add(this.txtToughness);
			this.gbPowerToughness.Controls.Add(this.txtPower);
			this.gbPowerToughness.Controls.Add(this.cboToughnessFilter);
			this.gbPowerToughness.Controls.Add(this.cboPowerFilter);
			this.gbPowerToughness.Controls.Add(this.chkCheckPowerToughness);
			this.gbPowerToughness.Location = new System.Drawing.Point(244, 305);
			this.gbPowerToughness.Name = "gbPowerToughness";
			this.gbPowerToughness.Size = new System.Drawing.Size(190, 148);
			this.gbPowerToughness.TabIndex = 6;
			this.gbPowerToughness.TabStop = false;
			this.gbPowerToughness.Tag = "POWER_TOUGHNESS";
			this.gbPowerToughness.Text = "Power && Toughness";
			// 
			// txtToughness
			// 
			this.txtToughness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtToughness.Location = new System.Drawing.Point(6, 95);
			this.txtToughness.Name = "txtToughness";
			this.txtToughness.Size = new System.Drawing.Size(178, 20);
			this.txtToughness.TabIndex = 3;
			this.txtToughness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtToughness.Leave += new System.EventHandler(this.txtToughness_Leave);
			// 
			// txtPower
			// 
			this.txtPower.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPower.Location = new System.Drawing.Point(6, 42);
			this.txtPower.Name = "txtPower";
			this.txtPower.Size = new System.Drawing.Size(178, 20);
			this.txtPower.TabIndex = 1;
			this.txtPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPower.Leave += new System.EventHandler(this.txtPower_Leave);
			// 
			// cboToughnessFilter
			// 
			this.cboToughnessFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboToughnessFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboToughnessFilter.FormattingEnabled = true;
			this.cboToughnessFilter.Location = new System.Drawing.Point(6, 121);
			this.cboToughnessFilter.Name = "cboToughnessFilter";
			this.cboToughnessFilter.Size = new System.Drawing.Size(178, 21);
			this.cboToughnessFilter.TabIndex = 4;
			// 
			// cboPowerFilter
			// 
			this.cboPowerFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboPowerFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPowerFilter.FormattingEnabled = true;
			this.cboPowerFilter.Location = new System.Drawing.Point(6, 68);
			this.cboPowerFilter.Name = "cboPowerFilter";
			this.cboPowerFilter.Size = new System.Drawing.Size(178, 21);
			this.cboPowerFilter.TabIndex = 2;
			// 
			// chkCheckPowerToughness
			// 
			this.chkCheckPowerToughness.AutoSize = true;
			this.chkCheckPowerToughness.Location = new System.Drawing.Point(6, 19);
			this.chkCheckPowerToughness.Name = "chkCheckPowerToughness";
			this.chkCheckPowerToughness.Size = new System.Drawing.Size(148, 17);
			this.chkCheckPowerToughness.TabIndex = 0;
			this.chkCheckPowerToughness.Tag = "CHECK_POWER_TOUGHNESS";
			this.chkCheckPowerToughness.Text = "Check Power/Toughness";
			this.chkCheckPowerToughness.UseVisualStyleBackColor = true;
			this.chkCheckPowerToughness.CheckedChanged += new System.EventHandler(this.chkCheckPowerToughness_CheckedChanged);
			// 
			// gbAbilities
			// 
			this.gbAbilities.Controls.Add(this.chkCheckGrantedAbilities);
			this.gbAbilities.Controls.Add(this.cboAbilitiesFilter);
			this.gbAbilities.Controls.Add(this.txtAbilities);
			this.gbAbilities.Controls.Add(this.chkCheckAbilities);
			this.gbAbilities.Location = new System.Drawing.Point(440, 198);
			this.gbAbilities.Name = "gbAbilities";
			this.gbAbilities.Size = new System.Drawing.Size(200, 118);
			this.gbAbilities.TabIndex = 8;
			this.gbAbilities.TabStop = false;
			this.gbAbilities.Tag = "ABILITIES";
			this.gbAbilities.Text = "Abilities";
			// 
			// chkCheckGrantedAbilities
			// 
			this.chkCheckGrantedAbilities.AutoSize = true;
			this.chkCheckGrantedAbilities.Location = new System.Drawing.Point(6, 95);
			this.chkCheckGrantedAbilities.Name = "chkCheckGrantedAbilities";
			this.chkCheckGrantedAbilities.Size = new System.Drawing.Size(136, 17);
			this.chkCheckGrantedAbilities.TabIndex = 3;
			this.chkCheckGrantedAbilities.Tag = "CHECK_GRANTED_ABILITIES";
			this.chkCheckGrantedAbilities.Text = "Check Granted Abilities";
			this.chkCheckGrantedAbilities.UseVisualStyleBackColor = true;
			// 
			// cboAbilitiesFilter
			// 
			this.cboAbilitiesFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboAbilitiesFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAbilitiesFilter.FormattingEnabled = true;
			this.cboAbilitiesFilter.Location = new System.Drawing.Point(6, 68);
			this.cboAbilitiesFilter.Name = "cboAbilitiesFilter";
			this.cboAbilitiesFilter.Size = new System.Drawing.Size(188, 21);
			this.cboAbilitiesFilter.TabIndex = 2;
			// 
			// txtAbilities
			// 
			this.txtAbilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAbilities.Location = new System.Drawing.Point(6, 42);
			this.txtAbilities.Name = "txtAbilities";
			this.txtAbilities.Size = new System.Drawing.Size(188, 20);
			this.txtAbilities.TabIndex = 1;
			// 
			// chkCheckAbilities
			// 
			this.chkCheckAbilities.AutoSize = true;
			this.chkCheckAbilities.Location = new System.Drawing.Point(6, 19);
			this.chkCheckAbilities.Name = "chkCheckAbilities";
			this.chkCheckAbilities.Size = new System.Drawing.Size(95, 17);
			this.chkCheckAbilities.TabIndex = 0;
			this.chkCheckAbilities.Tag = "CHECK_ABILITIES";
			this.chkCheckAbilities.Text = "Check Abilities";
			this.chkCheckAbilities.UseVisualStyleBackColor = true;
			this.chkCheckAbilities.CheckedChanged += new System.EventHandler(this.chkCheckAbilities_CheckedChanged);
			// 
			// gbExtra
			// 
			this.gbExtra.Controls.Add(this.chkUnderscores);
			this.gbExtra.Controls.Add(this.chkPhyrexianMana);
			this.gbExtra.Controls.Add(this.chkHybridMana);
			this.gbExtra.Controls.Add(this.chkRegularMana);
			this.gbExtra.Controls.Add(this.chkCardsThatCreateTokens);
			this.gbExtra.Controls.Add(this.chkTokens);
			this.gbExtra.Location = new System.Drawing.Point(440, 322);
			this.gbExtra.Name = "gbExtra";
			this.gbExtra.Size = new System.Drawing.Size(200, 134);
			this.gbExtra.TabIndex = 9;
			this.gbExtra.TabStop = false;
			this.gbExtra.Tag = "EXTRA_FILTERS";
			this.gbExtra.Text = "Extra Filters";
			// 
			// chkUnderscores
			// 
			this.chkUnderscores.AutoSize = true;
			this.chkUnderscores.Location = new System.Drawing.Point(74, 19);
			this.chkUnderscores.Name = "chkUnderscores";
			this.chkUnderscores.Size = new System.Drawing.Size(86, 17);
			this.chkUnderscores.TabIndex = 13;
			this.chkUnderscores.Tag = "UNDERSCORES";
			this.chkUnderscores.Text = "Underscores";
			this.chkUnderscores.UseVisualStyleBackColor = true;
			// 
			// chkPhyrexianMana
			// 
			this.chkPhyrexianMana.AutoSize = true;
			this.chkPhyrexianMana.Location = new System.Drawing.Point(6, 111);
			this.chkPhyrexianMana.Name = "chkPhyrexianMana";
			this.chkPhyrexianMana.Size = new System.Drawing.Size(173, 17);
			this.chkPhyrexianMana.TabIndex = 4;
			this.chkPhyrexianMana.Tag = "USES_PHYREXIAN_MANA";
			this.chkPhyrexianMana.Text = "Cards that use Phyrexian Mana";
			this.chkPhyrexianMana.UseVisualStyleBackColor = true;
			// 
			// chkHybridMana
			// 
			this.chkHybridMana.AutoSize = true;
			this.chkHybridMana.Location = new System.Drawing.Point(6, 88);
			this.chkHybridMana.Name = "chkHybridMana";
			this.chkHybridMana.Size = new System.Drawing.Size(157, 17);
			this.chkHybridMana.TabIndex = 3;
			this.chkHybridMana.Tag = "USES_HYBRID_MANA";
			this.chkHybridMana.Text = "Cards that use Hybrid Mana";
			this.chkHybridMana.UseVisualStyleBackColor = true;
			// 
			// chkRegularMana
			// 
			this.chkRegularMana.AutoSize = true;
			this.chkRegularMana.Location = new System.Drawing.Point(6, 65);
			this.chkRegularMana.Name = "chkRegularMana";
			this.chkRegularMana.Size = new System.Drawing.Size(164, 17);
			this.chkRegularMana.TabIndex = 2;
			this.chkRegularMana.Tag = "USES_REGULAR_MANA";
			this.chkRegularMana.Text = "Cards that use Regular Mana";
			this.chkRegularMana.UseVisualStyleBackColor = true;
			// 
			// chkCardsThatCreateTokens
			// 
			this.chkCardsThatCreateTokens.AutoSize = true;
			this.chkCardsThatCreateTokens.Location = new System.Drawing.Point(6, 42);
			this.chkCardsThatCreateTokens.Name = "chkCardsThatCreateTokens";
			this.chkCardsThatCreateTokens.Size = new System.Drawing.Size(146, 17);
			this.chkCardsThatCreateTokens.TabIndex = 1;
			this.chkCardsThatCreateTokens.Tag = "CARDS_THAT_CREATE_TOKENS";
			this.chkCardsThatCreateTokens.Text = "Cards that create Tokens";
			this.chkCardsThatCreateTokens.UseVisualStyleBackColor = true;
			// 
			// chkTokens
			// 
			this.chkTokens.AutoSize = true;
			this.chkTokens.Location = new System.Drawing.Point(6, 19);
			this.chkTokens.Name = "chkTokens";
			this.chkTokens.Size = new System.Drawing.Size(62, 17);
			this.chkTokens.TabIndex = 0;
			this.chkTokens.Tag = "TOKENS";
			this.chkTokens.Text = "Tokens";
			this.chkTokens.UseVisualStyleBackColor = true;
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(545, 462);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(95, 21);
			this.cmdCancel.TabIndex = 12;
			this.cmdCancel.Tag = "CANCEL";
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(444, 462);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(95, 21);
			this.cmdApply.TabIndex = 11;
			this.cmdApply.Tag = "APPLY";
			this.cmdApply.Text = "Apply";
			this.cmdApply.UseVisualStyleBackColor = true;
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// cmdReset
			// 
			this.cmdReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdReset.Location = new System.Drawing.Point(343, 462);
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size(95, 21);
			this.cmdReset.TabIndex = 10;
			this.cmdReset.Tag = "RESET";
			this.cmdReset.Text = "Reset";
			this.cmdReset.UseVisualStyleBackColor = true;
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			// 
			// FilterSettings
			// 
			this.AcceptButton = this.cmdApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(652, 495);
			this.ControlBox = false;
			this.Controls.Add(this.cmdReset);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdApply);
			this.Controls.Add(this.gbExtra);
			this.Controls.Add(this.gbAbilities);
			this.Controls.Add(this.gbPowerToughness);
			this.Controls.Add(this.gbCmc);
			this.Controls.Add(this.gbRarity);
			this.Controls.Add(this.gbName);
			this.Controls.Add(this.gbColour);
			this.Controls.Add(this.gbSubTypes);
			this.Controls.Add(this.gbTypes);
			this.Controls.Add(this.gbSupertype);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FilterSettings";
			this.ShowInTaskbar = false;
			this.Tag = "FILTER_SETTINGS";
			this.Text = "FilterSettings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FilterSettings_FormClosing);
			this.gbSupertype.ResumeLayout(false);
			this.gbSupertype.PerformLayout();
			this.gbTypes.ResumeLayout(false);
			this.scTypes.Panel1.ResumeLayout(false);
			this.scTypes.Panel1.PerformLayout();
			this.scTypes.Panel2.ResumeLayout(false);
			this.scTypes.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scTypes)).EndInit();
			this.scTypes.ResumeLayout(false);
			this.gbSubTypes.ResumeLayout(false);
			this.gbSubTypes.PerformLayout();
			this.gbColour.ResumeLayout(false);
			this.scColour.Panel1.ResumeLayout(false);
			this.scColour.Panel1.PerformLayout();
			this.scColour.Panel2.ResumeLayout(false);
			this.scColour.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scColour)).EndInit();
			this.scColour.ResumeLayout(false);
			this.gbName.ResumeLayout(false);
			this.gbName.PerformLayout();
			this.gbRarity.ResumeLayout(false);
			this.gbRarity.PerformLayout();
			this.gbCmc.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numCmc)).EndInit();
			this.gbPowerToughness.ResumeLayout(false);
			this.gbPowerToughness.PerformLayout();
			this.gbAbilities.ResumeLayout(false);
			this.gbAbilities.PerformLayout();
			this.gbExtra.ResumeLayout(false);
			this.gbExtra.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbSupertype;
		private System.Windows.Forms.CheckBox chkBasic;
		private System.Windows.Forms.CheckBox chkWorld;
		private System.Windows.Forms.CheckBox chkSnow;
		private System.Windows.Forms.CheckBox chkLegendary;
		private System.Windows.Forms.GroupBox gbTypes;
		private System.Windows.Forms.SplitContainer scTypes;
		private System.Windows.Forms.CheckBox chkPhenomenon;
		private System.Windows.Forms.CheckBox chkLand;
		private System.Windows.Forms.CheckBox chkInstant;
		private System.Windows.Forms.CheckBox chkEnchantment;
		private System.Windows.Forms.CheckBox chkCreature;
		private System.Windows.Forms.CheckBox chkArtifact;
		private System.Windows.Forms.CheckBox chkVanguard;
		private System.Windows.Forms.CheckBox chkTribal;
		private System.Windows.Forms.CheckBox chkSorcery;
		private System.Windows.Forms.CheckBox chkScheme;
		private System.Windows.Forms.CheckBox chkPlaneswalker;
		private System.Windows.Forms.CheckBox chkPlane;
		private System.Windows.Forms.ComboBox cboTypeFilter;
		private System.Windows.Forms.GroupBox gbSubTypes;
		private System.Windows.Forms.CheckBox chkCheckSubTypes;
		private System.Windows.Forms.ComboBox cboSubTypeFilter;
		private System.Windows.Forms.TextBox txtSubType;
		private System.Windows.Forms.GroupBox gbColour;
		private System.Windows.Forms.ComboBox cboColourFilter;
		private System.Windows.Forms.SplitContainer scColour;
		private System.Windows.Forms.CheckBox chkRed;
		private System.Windows.Forms.CheckBox chkGreen;
		private System.Windows.Forms.CheckBox chkBlue;
		private System.Windows.Forms.CheckBox chkBlack;
		private System.Windows.Forms.CheckBox chkMultiColoured;
		private System.Windows.Forms.CheckBox chkColourless;
		private System.Windows.Forms.CheckBox chkWhite;
		private System.Windows.Forms.GroupBox gbName;
		private System.Windows.Forms.ComboBox cboNameFilter;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.CheckBox chkName;
		private System.Windows.Forms.GroupBox gbRarity;
		private System.Windows.Forms.CheckBox chkRarityToken;
		private System.Windows.Forms.CheckBox chkRarityMythic;
		private System.Windows.Forms.CheckBox chkRarityRare;
		private System.Windows.Forms.CheckBox chkRarityUncommon;
		private System.Windows.Forms.CheckBox chkRarityCommon;
		private System.Windows.Forms.CheckBox chkRarityLand;
		private System.Windows.Forms.GroupBox gbCmc;
		private System.Windows.Forms.ComboBox cboCmcFilter;
		private System.Windows.Forms.NumericUpDown numCmc;
		private System.Windows.Forms.GroupBox gbPowerToughness;
		private System.Windows.Forms.CheckBox chkCheckPowerToughness;
		private System.Windows.Forms.ComboBox cboToughnessFilter;
		private System.Windows.Forms.ComboBox cboPowerFilter;
		private System.Windows.Forms.GroupBox gbAbilities;
		private System.Windows.Forms.CheckBox chkCheckGrantedAbilities;
		private System.Windows.Forms.ComboBox cboAbilitiesFilter;
		private System.Windows.Forms.TextBox txtAbilities;
		private System.Windows.Forms.CheckBox chkCheckAbilities;
		private System.Windows.Forms.GroupBox gbExtra;
		private System.Windows.Forms.CheckBox chkTokens;
		private System.Windows.Forms.CheckBox chkPhyrexianMana;
		private System.Windows.Forms.CheckBox chkHybridMana;
		private System.Windows.Forms.CheckBox chkRegularMana;
		private System.Windows.Forms.CheckBox chkCardsThatCreateTokens;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.TextBox txtToughness;
		private System.Windows.Forms.TextBox txtPower;
		private System.Windows.Forms.CheckBox chkRaritySpecial;
		private System.Windows.Forms.Button cmdReset;
		private System.Windows.Forms.CheckBox chkUnderscores;
	}
}