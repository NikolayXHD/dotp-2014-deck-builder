namespace RSN.DotP
{
	partial class DeckBuilder
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
            this.dgvCards = new System.Windows.Forms.DataGridView();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileCreateFromExisting = new System.Windows.Forms.ToolStripMenuItem();
            this.editExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiFileExportTo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileExportToDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileExportToWad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiToolsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiToolsSetupCustomData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiToolsCreateCoreWad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiToolsGenerateCPE = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiToolsCombineSpecData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.picFrame = new System.Windows.Forms.PictureBox();
            this.gbCardMoveOptions = new System.Windows.Forms.GroupBox();
            this.rbDoubleClickPromoUnlock = new System.Windows.Forms.RadioButton();
            this.rbDoubleClickRegularUnlock = new System.Windows.Forms.RadioButton();
            this.rbDoubleClickDeck = new System.Windows.Forms.RadioButton();
            this.scFilterNormalAdvanced = new System.Windows.Forms.SplitContainer();
            this.scFilterOptions = new System.Windows.Forms.SplitContainer();
            this.cmdClearFilters = new System.Windows.Forms.Button();
            this.cmdSetFilter = new System.Windows.Forms.Button();
            this.cmdFilterAdvanced = new System.Windows.Forms.Button();
            this.scDeckUnlocks = new System.Windows.Forms.SplitContainer();
            this.gbDeck = new System.Windows.Forms.GroupBox();
            this.scCounts = new System.Windows.Forms.SplitContainer();
            this.scTotalCount = new System.Windows.Forms.SplitContainer();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblTotalCardCount = new System.Windows.Forms.Label();
            this.scBasicLand = new System.Windows.Forms.SplitContainer();
            this.lblBasicLand = new System.Windows.Forms.Label();
            this.lblBasicLandCount = new System.Windows.Forms.Label();
            this.cmdDeckInformation = new System.Windows.Forms.Button();
            this.cmdEditName = new System.Windows.Forms.Button();
            this.lblDeckName = new System.Windows.Forms.Label();
            this.dgvDeckCards = new System.Windows.Forms.DataGridView();
            this.scRegularPromo = new System.Windows.Forms.SplitContainer();
            this.gbRegularUnlocks = new System.Windows.Forms.GroupBox();
            this.scRegularUnlockCount = new System.Windows.Forms.SplitContainer();
            this.lblRegUnlockCount = new System.Windows.Forms.Label();
            this.lblRegUnlockCardCount = new System.Windows.Forms.Label();
            this.scRegularButtons = new System.Windows.Forms.SplitContainer();
            this.cmdRegularMoveUp = new System.Windows.Forms.Button();
            this.cmdRegularMoveDown = new System.Windows.Forms.Button();
            this.dgvUnlocksRegular = new System.Windows.Forms.DataGridView();
            this.gbPromoUnlocks = new System.Windows.Forms.GroupBox();
            this.scPromoCount = new System.Windows.Forms.SplitContainer();
            this.lblPromoUnlockCount = new System.Windows.Forms.Label();
            this.lblPromoUnlockCardCount = new System.Windows.Forms.Label();
            this.scPromoButtons = new System.Windows.Forms.SplitContainer();
            this.cmdPromoMoveUp = new System.Windows.Forms.Button();
            this.cmdPromoMoveDown = new System.Windows.Forms.Button();
            this.dgvUnlocksPromo = new System.Windows.Forms.DataGridView();
            this.cmnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuiColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuiRemoveCard = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiViewCard = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiDecksUsedIn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportCard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuiExportPreviews = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportPreviewsPng = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportPreviewsTdx = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportImageCrop = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportImageCropPng = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportImageCropTdx = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuiMoveTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiMoveToMainDeck = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiMoveToRegularUnlocks = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiMoveToPromoUnlocks = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuPictures = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuiExport = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportPng = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuiExportTdx = new System.Windows.Forms.ToolStripMenuItem();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.sslblSpacing = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslblLoadedCards = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslblLoadedCardsNum = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslblCardsInList = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslblCardsInListNum = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).BeginInit();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).BeginInit();
            this.gbCardMoveOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scFilterNormalAdvanced)).BeginInit();
            this.scFilterNormalAdvanced.Panel1.SuspendLayout();
            this.scFilterNormalAdvanced.Panel2.SuspendLayout();
            this.scFilterNormalAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scFilterOptions)).BeginInit();
            this.scFilterOptions.Panel1.SuspendLayout();
            this.scFilterOptions.Panel2.SuspendLayout();
            this.scFilterOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDeckUnlocks)).BeginInit();
            this.scDeckUnlocks.Panel1.SuspendLayout();
            this.scDeckUnlocks.Panel2.SuspendLayout();
            this.scDeckUnlocks.SuspendLayout();
            this.gbDeck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scCounts)).BeginInit();
            this.scCounts.Panel1.SuspendLayout();
            this.scCounts.Panel2.SuspendLayout();
            this.scCounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTotalCount)).BeginInit();
            this.scTotalCount.Panel1.SuspendLayout();
            this.scTotalCount.Panel2.SuspendLayout();
            this.scTotalCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scBasicLand)).BeginInit();
            this.scBasicLand.Panel1.SuspendLayout();
            this.scBasicLand.Panel2.SuspendLayout();
            this.scBasicLand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeckCards)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scRegularPromo)).BeginInit();
            this.scRegularPromo.Panel1.SuspendLayout();
            this.scRegularPromo.Panel2.SuspendLayout();
            this.scRegularPromo.SuspendLayout();
            this.gbRegularUnlocks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scRegularUnlockCount)).BeginInit();
            this.scRegularUnlockCount.Panel1.SuspendLayout();
            this.scRegularUnlockCount.Panel2.SuspendLayout();
            this.scRegularUnlockCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scRegularButtons)).BeginInit();
            this.scRegularButtons.Panel1.SuspendLayout();
            this.scRegularButtons.Panel2.SuspendLayout();
            this.scRegularButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnlocksRegular)).BeginInit();
            this.gbPromoUnlocks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scPromoCount)).BeginInit();
            this.scPromoCount.Panel1.SuspendLayout();
            this.scPromoCount.Panel2.SuspendLayout();
            this.scPromoCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scPromoButtons)).BeginInit();
            this.scPromoButtons.Panel1.SuspendLayout();
            this.scPromoButtons.Panel2.SuspendLayout();
            this.scPromoButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnlocksPromo)).BeginInit();
            this.cmnuContext.SuspendLayout();
            this.cmnuPictures.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCards
            // 
            this.dgvCards.AllowUserToAddRows = false;
            this.dgvCards.AllowUserToDeleteRows = false;
            this.dgvCards.AllowUserToOrderColumns = true;
            this.dgvCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCards.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCards.Location = new System.Drawing.Point(12, 545);
            this.dgvCards.MultiSelect = false;
            this.dgvCards.Name = "dgvCards";
            this.dgvCards.RowHeadersVisible = false;
            this.dgvCards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCards.Size = new System.Drawing.Size(1086, 82);
            this.dgvCards.TabIndex = 0;
            this.dgvCards.VirtualMode = true;
            this.dgvCards.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCards_CellBeginEdit);
            this.dgvCards.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCards_CellDoubleClick);
            this.dgvCards.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCards_CellMouseEnter);
            this.dgvCards.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvCards_CellValueNeeded);
            this.dgvCards.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCards_ColumnHeaderMouseClick);
            this.dgvCards.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvCards_Scroll);
            this.dgvCards.SelectionChanged += new System.EventHandler(this.dgvCards_SelectionChanged);
            this.dgvCards.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCards_KeyDown);
            this.dgvCards.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvCards_MouseClick);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiFile,
            this.mnuiTools,
            this.mnuiAbout});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1110, 24);
            this.mnuMain.TabIndex = 5;
            this.mnuMain.Text = "Main Menu";
            // 
            // mnuiFile
            // 
            this.mnuiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiFileNew,
            this.mnuiFileCreateFromExisting,
            this.editExistingToolStripMenuItem,
            this.mnuiFileOpen,
            this.toolStripSeparator1,
            this.mnuiFileSave,
            this.mnuiFileSaveAs,
            this.toolStripSeparator2,
            this.mnuiFileExportTo,
            this.toolStripSeparator3,
            this.mnuiFileExit});
            this.mnuiFile.Name = "mnuiFile";
            this.mnuiFile.Size = new System.Drawing.Size(37, 20);
            this.mnuiFile.Tag = "MENU_FILE";
            this.mnuiFile.Text = "&File";
            // 
            // mnuiFileNew
            // 
            this.mnuiFileNew.Name = "mnuiFileNew";
            this.mnuiFileNew.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileNew.Tag = "MENU_FILE_NEW";
            this.mnuiFileNew.Text = "&New Deck";
            this.mnuiFileNew.Click += new System.EventHandler(this.mnuiFileNew_Click);
            // 
            // mnuiFileCreateFromExisting
            // 
            this.mnuiFileCreateFromExisting.Name = "mnuiFileCreateFromExisting";
            this.mnuiFileCreateFromExisting.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileCreateFromExisting.Tag = "MENU_FILE_CREATE_FROM_EXISTING";
            this.mnuiFileCreateFromExisting.Text = "&Create From Existing Deck ...";
            this.mnuiFileCreateFromExisting.Click += new System.EventHandler(this.mnuiFileCreateFromExisting_Click);
            // 
            // editExistingToolStripMenuItem
            // 
            this.editExistingToolStripMenuItem.Name = "editExistingToolStripMenuItem";
            this.editExistingToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.editExistingToolStripMenuItem.Tag = "MENU_FILE_EDIT_EXISTING";
            this.editExistingToolStripMenuItem.Text = "&Edit Existing Deck ...";
            this.editExistingToolStripMenuItem.Click += new System.EventHandler(this.editExistingToolStripMenuItem_Click);
            // 
            // mnuiFileOpen
            // 
            this.mnuiFileOpen.Name = "mnuiFileOpen";
            this.mnuiFileOpen.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileOpen.Tag = "MENU_FILE_OPEN";
            this.mnuiFileOpen.Text = "&Open Deck ...";
            this.mnuiFileOpen.Click += new System.EventHandler(this.mnuiFileOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuiFileSave
            // 
            this.mnuiFileSave.Name = "mnuiFileSave";
            this.mnuiFileSave.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileSave.Tag = "MENU_FILE_SAVE";
            this.mnuiFileSave.Text = "&Save Deck";
            this.mnuiFileSave.Click += new System.EventHandler(this.mnuiFileSave_Click);
            // 
            // mnuiFileSaveAs
            // 
            this.mnuiFileSaveAs.Name = "mnuiFileSaveAs";
            this.mnuiFileSaveAs.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileSaveAs.Tag = "MENU_FILE_SAVE_AS";
            this.mnuiFileSaveAs.Text = "Save Deck &As ...";
            this.mnuiFileSaveAs.Click += new System.EventHandler(this.mnuiFileSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuiFileExportTo
            // 
            this.mnuiFileExportTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiFileExportToDirectory,
            this.mnuiFileExportToWad});
            this.mnuiFileExportTo.Name = "mnuiFileExportTo";
            this.mnuiFileExportTo.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileExportTo.Tag = "MENU_FILE_EXPORT";
            this.mnuiFileExportTo.Text = "&Export to";
            // 
            // mnuiFileExportToDirectory
            // 
            this.mnuiFileExportToDirectory.Name = "mnuiFileExportToDirectory";
            this.mnuiFileExportToDirectory.Size = new System.Drawing.Size(122, 22);
            this.mnuiFileExportToDirectory.Tag = "MENU_FILE_EXPORT_DIRECTORY";
            this.mnuiFileExportToDirectory.Text = "&Directory";
            this.mnuiFileExportToDirectory.Click += new System.EventHandler(this.mnuiFileExportToDirectory_Click);
            // 
            // mnuiFileExportToWad
            // 
            this.mnuiFileExportToWad.Name = "mnuiFileExportToWad";
            this.mnuiFileExportToWad.Size = new System.Drawing.Size(122, 22);
            this.mnuiFileExportToWad.Tag = "MENU_FILE_EXPORT_WAD";
            this.mnuiFileExportToWad.Text = "&Wad";
            this.mnuiFileExportToWad.Click += new System.EventHandler(this.mnuiFileExportToWad_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // mnuiFileExit
            // 
            this.mnuiFileExit.Name = "mnuiFileExit";
            this.mnuiFileExit.Size = new System.Drawing.Size(223, 22);
            this.mnuiFileExit.Tag = "MENU_FILE_EXIT";
            this.mnuiFileExit.Text = "E&xit";
            this.mnuiFileExit.Click += new System.EventHandler(this.mnuiFileExit_Click);
            // 
            // mnuiTools
            // 
            this.mnuiTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiToolsRefresh,
            this.toolStripSeparator4,
            this.mnuiToolsSetupCustomData,
            this.mnuiToolsCreateCoreWad,
            this.toolStripSeparator8,
            this.mnuiToolsGenerateCPE,
            this.mnuiToolsCombineSpecData,
            this.toolStripSeparator6,
            this.mnuiToolsOptions});
            this.mnuiTools.Name = "mnuiTools";
            this.mnuiTools.Size = new System.Drawing.Size(47, 20);
            this.mnuiTools.Tag = "MENU_TOOLS";
            this.mnuiTools.Text = "&Tools";
            // 
            // mnuiToolsRefresh
            // 
            this.mnuiToolsRefresh.BackColor = System.Drawing.SystemColors.Control;
            this.mnuiToolsRefresh.Name = "mnuiToolsRefresh";
            this.mnuiToolsRefresh.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsRefresh.Tag = "MENU_TOOLS_REFRESH";
            this.mnuiToolsRefresh.Text = "&Refresh Data";
            this.mnuiToolsRefresh.Click += new System.EventHandler(this.mnuiToolsRefresh_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuiToolsSetupCustomData
            // 
            this.mnuiToolsSetupCustomData.Name = "mnuiToolsSetupCustomData";
            this.mnuiToolsSetupCustomData.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsSetupCustomData.Tag = "MENU_TOOLS_SETUP_CUSTOM_DATA_FOLDER";
            this.mnuiToolsSetupCustomData.Text = "&Setup Custom Data Folder";
            this.mnuiToolsSetupCustomData.Click += new System.EventHandler(this.mnuiToolsSetupCustomData_Click);
            // 
            // mnuiToolsCreateCoreWad
            // 
            this.mnuiToolsCreateCoreWad.Name = "mnuiToolsCreateCoreWad";
            this.mnuiToolsCreateCoreWad.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsCreateCoreWad.Tag = "MENU_TOOLS_CREATE_CORE_WAD";
            this.mnuiToolsCreateCoreWad.Text = "Cr&eate Core Wad from Custom Data";
            this.mnuiToolsCreateCoreWad.Click += new System.EventHandler(this.mnuiToolsCreateCoreWad_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuiToolsGenerateCPE
            // 
            this.mnuiToolsGenerateCPE.Name = "mnuiToolsGenerateCPE";
            this.mnuiToolsGenerateCPE.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsGenerateCPE.Tag = "MENU_TOOLS_GENERATE_CPE";
            this.mnuiToolsGenerateCPE.Text = "&Generate Content Pack Enabler";
            this.mnuiToolsGenerateCPE.Click += new System.EventHandler(this.mnuiToolsGenerateCPE_Click);
            // 
            // mnuiToolsCombineSpecData
            // 
            this.mnuiToolsCombineSpecData.Name = "mnuiToolsCombineSpecData";
            this.mnuiToolsCombineSpecData.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsCombineSpecData.Tag = "MENU_TOOLS_COMBINE_SPEC_DATA";
            this.mnuiToolsCombineSpecData.Text = "&Combine SPEC Data";
            this.mnuiToolsCombineSpecData.Click += new System.EventHandler(this.mnuiToolsCombineSpecData_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuiToolsOptions
            // 
            this.mnuiToolsOptions.Name = "mnuiToolsOptions";
            this.mnuiToolsOptions.Size = new System.Drawing.Size(264, 22);
            this.mnuiToolsOptions.Tag = "MENU_TOOLS_OPTIONS";
            this.mnuiToolsOptions.Text = "&Options ...";
            this.mnuiToolsOptions.Click += new System.EventHandler(this.mnuiToolsOptions_Click);
            // 
            // mnuiAbout
            // 
            this.mnuiAbout.Name = "mnuiAbout";
            this.mnuiAbout.Size = new System.Drawing.Size(52, 20);
            this.mnuiAbout.Tag = "MENU_ABOUT";
            this.mnuiAbout.Text = "&About";
            this.mnuiAbout.Click += new System.EventHandler(this.mnuiAbout_Click);
            // 
            // picFrame
            // 
            this.picFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picFrame.Location = new System.Drawing.Point(742, 27);
            this.picFrame.Name = "picFrame";
            this.picFrame.Size = new System.Drawing.Size(356, 512);
            this.picFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFrame.TabIndex = 6;
            this.picFrame.TabStop = false;
            this.picFrame.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picFrame_MouseUp);
            // 
            // gbCardMoveOptions
            // 
            this.gbCardMoveOptions.Controls.Add(this.rbDoubleClickPromoUnlock);
            this.gbCardMoveOptions.Controls.Add(this.rbDoubleClickRegularUnlock);
            this.gbCardMoveOptions.Controls.Add(this.rbDoubleClickDeck);
            this.gbCardMoveOptions.Location = new System.Drawing.Point(12, 497);
            this.gbCardMoveOptions.Name = "gbCardMoveOptions";
            this.gbCardMoveOptions.Size = new System.Drawing.Size(378, 42);
            this.gbCardMoveOptions.TabIndex = 4;
            this.gbCardMoveOptions.TabStop = false;
            this.gbCardMoveOptions.Tag = "DOUBLE_CLICK_MOVE";
            this.gbCardMoveOptions.Text = "Double-Click adds card to:";
            // 
            // rbDoubleClickPromoUnlock
            // 
            this.rbDoubleClickPromoUnlock.AutoSize = true;
            this.rbDoubleClickPromoUnlock.Location = new System.Drawing.Point(173, 19);
            this.rbDoubleClickPromoUnlock.Name = "rbDoubleClickPromoUnlock";
            this.rbDoubleClickPromoUnlock.Size = new System.Drawing.Size(97, 17);
            this.rbDoubleClickPromoUnlock.TabIndex = 2;
            this.rbDoubleClickPromoUnlock.Tag = "PROMO_UNLOCKS";
            this.rbDoubleClickPromoUnlock.Text = "Promo Unlocks";
            this.rbDoubleClickPromoUnlock.UseVisualStyleBackColor = true;
            // 
            // rbDoubleClickRegularUnlock
            // 
            this.rbDoubleClickRegularUnlock.AutoSize = true;
            this.rbDoubleClickRegularUnlock.Location = new System.Drawing.Point(63, 19);
            this.rbDoubleClickRegularUnlock.Name = "rbDoubleClickRegularUnlock";
            this.rbDoubleClickRegularUnlock.Size = new System.Drawing.Size(104, 17);
            this.rbDoubleClickRegularUnlock.TabIndex = 1;
            this.rbDoubleClickRegularUnlock.Tag = "REGULAR_UNLOCKS";
            this.rbDoubleClickRegularUnlock.Text = "Regular Unlocks";
            this.rbDoubleClickRegularUnlock.UseVisualStyleBackColor = true;
            // 
            // rbDoubleClickDeck
            // 
            this.rbDoubleClickDeck.AutoSize = true;
            this.rbDoubleClickDeck.Checked = true;
            this.rbDoubleClickDeck.Location = new System.Drawing.Point(6, 19);
            this.rbDoubleClickDeck.Name = "rbDoubleClickDeck";
            this.rbDoubleClickDeck.Size = new System.Drawing.Size(51, 17);
            this.rbDoubleClickDeck.TabIndex = 0;
            this.rbDoubleClickDeck.TabStop = true;
            this.rbDoubleClickDeck.Tag = "DECK";
            this.rbDoubleClickDeck.Text = "Deck";
            this.rbDoubleClickDeck.UseVisualStyleBackColor = true;
            // 
            // scFilterNormalAdvanced
            // 
            this.scFilterNormalAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scFilterNormalAdvanced.IsSplitterFixed = true;
            this.scFilterNormalAdvanced.Location = new System.Drawing.Point(396, 497);
            this.scFilterNormalAdvanced.Name = "scFilterNormalAdvanced";
            // 
            // scFilterNormalAdvanced.Panel1
            // 
            this.scFilterNormalAdvanced.Panel1.Controls.Add(this.scFilterOptions);
            // 
            // scFilterNormalAdvanced.Panel2
            // 
            this.scFilterNormalAdvanced.Panel2.Controls.Add(this.cmdFilterAdvanced);
            this.scFilterNormalAdvanced.Size = new System.Drawing.Size(340, 42);
            this.scFilterNormalAdvanced.SplitterDistance = 221;
            this.scFilterNormalAdvanced.TabIndex = 8;
            // 
            // scFilterOptions
            // 
            this.scFilterOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scFilterOptions.IsSplitterFixed = true;
            this.scFilterOptions.Location = new System.Drawing.Point(0, 0);
            this.scFilterOptions.Name = "scFilterOptions";
            // 
            // scFilterOptions.Panel1
            // 
            this.scFilterOptions.Panel1.Controls.Add(this.cmdClearFilters);
            // 
            // scFilterOptions.Panel2
            // 
            this.scFilterOptions.Panel2.Controls.Add(this.cmdSetFilter);
            this.scFilterOptions.Size = new System.Drawing.Size(221, 42);
            this.scFilterOptions.SplitterDistance = 109;
            this.scFilterOptions.TabIndex = 8;
            // 
            // cmdClearFilters
            // 
            this.cmdClearFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdClearFilters.Location = new System.Drawing.Point(0, 0);
            this.cmdClearFilters.Name = "cmdClearFilters";
            this.cmdClearFilters.Size = new System.Drawing.Size(109, 42);
            this.cmdClearFilters.TabIndex = 5;
            this.cmdClearFilters.Tag = "CLEAR_FILTERS";
            this.cmdClearFilters.Text = "Clear All Filters";
            this.cmdClearFilters.UseVisualStyleBackColor = true;
            this.cmdClearFilters.Click += new System.EventHandler(this.cmdClearFilters_Click);
            // 
            // cmdSetFilter
            // 
            this.cmdSetFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdSetFilter.Location = new System.Drawing.Point(0, 0);
            this.cmdSetFilter.Name = "cmdSetFilter";
            this.cmdSetFilter.Size = new System.Drawing.Size(108, 42);
            this.cmdSetFilter.TabIndex = 6;
            this.cmdSetFilter.Tag = "SET_FILTERS";
            this.cmdSetFilter.Text = "Set Filters ...";
            this.cmdSetFilter.UseVisualStyleBackColor = true;
            this.cmdSetFilter.Click += new System.EventHandler(this.cmdSetFilter_Click);
            // 
            // cmdFilterAdvanced
            // 
            this.cmdFilterAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdFilterAdvanced.Location = new System.Drawing.Point(0, 0);
            this.cmdFilterAdvanced.Name = "cmdFilterAdvanced";
            this.cmdFilterAdvanced.Size = new System.Drawing.Size(115, 42);
            this.cmdFilterAdvanced.TabIndex = 7;
            this.cmdFilterAdvanced.Tag = "SET_FILTERS_ADVANCED";
            this.cmdFilterAdvanced.Text = "Set Advanced Filter ...";
            this.cmdFilterAdvanced.UseVisualStyleBackColor = true;
            this.cmdFilterAdvanced.Click += new System.EventHandler(this.cmdFilterAdvanced_Click);
            // 
            // scDeckUnlocks
            // 
            this.scDeckUnlocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scDeckUnlocks.Location = new System.Drawing.Point(12, 27);
            this.scDeckUnlocks.Name = "scDeckUnlocks";
            // 
            // scDeckUnlocks.Panel1
            // 
            this.scDeckUnlocks.Panel1.Controls.Add(this.gbDeck);
            // 
            // scDeckUnlocks.Panel2
            // 
            this.scDeckUnlocks.Panel2.Controls.Add(this.scRegularPromo);
            this.scDeckUnlocks.Size = new System.Drawing.Size(724, 464);
            this.scDeckUnlocks.SplitterDistance = 241;
            this.scDeckUnlocks.TabIndex = 9;
            // 
            // gbDeck
            // 
            this.gbDeck.Controls.Add(this.scCounts);
            this.gbDeck.Controls.Add(this.cmdDeckInformation);
            this.gbDeck.Controls.Add(this.cmdEditName);
            this.gbDeck.Controls.Add(this.lblDeckName);
            this.gbDeck.Controls.Add(this.dgvDeckCards);
            this.gbDeck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDeck.Location = new System.Drawing.Point(0, 0);
            this.gbDeck.Name = "gbDeck";
            this.gbDeck.Size = new System.Drawing.Size(241, 464);
            this.gbDeck.TabIndex = 1;
            this.gbDeck.TabStop = false;
            this.gbDeck.Tag = "DECK";
            this.gbDeck.Text = "Deck";
            // 
            // scCounts
            // 
            this.scCounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scCounts.IsSplitterFixed = true;
            this.scCounts.Location = new System.Drawing.Point(7, 46);
            this.scCounts.Name = "scCounts";
            // 
            // scCounts.Panel1
            // 
            this.scCounts.Panel1.Controls.Add(this.scTotalCount);
            // 
            // scCounts.Panel2
            // 
            this.scCounts.Panel2.Controls.Add(this.scBasicLand);
            this.scCounts.Size = new System.Drawing.Size(228, 15);
            this.scCounts.SplitterDistance = 111;
            this.scCounts.TabIndex = 4;
            // 
            // scTotalCount
            // 
            this.scTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTotalCount.IsSplitterFixed = true;
            this.scTotalCount.Location = new System.Drawing.Point(0, 0);
            this.scTotalCount.Name = "scTotalCount";
            // 
            // scTotalCount.Panel1
            // 
            this.scTotalCount.Panel1.Controls.Add(this.lblTotalCount);
            // 
            // scTotalCount.Panel2
            // 
            this.scTotalCount.Panel2.Controls.Add(this.lblTotalCardCount);
            this.scTotalCount.Size = new System.Drawing.Size(111, 15);
            this.scTotalCount.SplitterDistance = 73;
            this.scTotalCount.TabIndex = 1;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalCount.Location = new System.Drawing.Point(0, 0);
            this.lblTotalCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(60, 13);
            this.lblTotalCount.TabIndex = 3;
            this.lblTotalCount.Tag = "COLUMN_TEXT_CARD_COUNT";
            this.lblTotalCount.Text = "Card Count";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalCardCount
            // 
            this.lblTotalCardCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTotalCardCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalCardCount.Location = new System.Drawing.Point(0, 0);
            this.lblTotalCardCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblTotalCardCount.Name = "lblTotalCardCount";
            this.lblTotalCardCount.Size = new System.Drawing.Size(34, 15);
            this.lblTotalCardCount.TabIndex = 4;
            this.lblTotalCardCount.Tag = "";
            this.lblTotalCardCount.Text = "0";
            this.lblTotalCardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scBasicLand
            // 
            this.scBasicLand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scBasicLand.IsSplitterFixed = true;
            this.scBasicLand.Location = new System.Drawing.Point(0, 0);
            this.scBasicLand.Name = "scBasicLand";
            // 
            // scBasicLand.Panel1
            // 
            this.scBasicLand.Panel1.Controls.Add(this.lblBasicLand);
            // 
            // scBasicLand.Panel2
            // 
            this.scBasicLand.Panel2.Controls.Add(this.lblBasicLandCount);
            this.scBasicLand.Size = new System.Drawing.Size(113, 15);
            this.scBasicLand.SplitterDistance = 74;
            this.scBasicLand.TabIndex = 0;
            // 
            // lblBasicLand
            // 
            this.lblBasicLand.AutoSize = true;
            this.lblBasicLand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBasicLand.Location = new System.Drawing.Point(0, 0);
            this.lblBasicLand.Margin = new System.Windows.Forms.Padding(3);
            this.lblBasicLand.Name = "lblBasicLand";
            this.lblBasicLand.Size = new System.Drawing.Size(60, 13);
            this.lblBasicLand.TabIndex = 3;
            this.lblBasicLand.Tag = "BASIC_LAND";
            this.lblBasicLand.Text = "Basic Land";
            this.lblBasicLand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBasicLandCount
            // 
            this.lblBasicLandCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBasicLandCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBasicLandCount.Location = new System.Drawing.Point(0, 0);
            this.lblBasicLandCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblBasicLandCount.Name = "lblBasicLandCount";
            this.lblBasicLandCount.Size = new System.Drawing.Size(35, 15);
            this.lblBasicLandCount.TabIndex = 4;
            this.lblBasicLandCount.Tag = "";
            this.lblBasicLandCount.Text = "60";
            this.lblBasicLandCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdDeckInformation
            // 
            this.cmdDeckInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDeckInformation.Location = new System.Drawing.Point(6, 437);
            this.cmdDeckInformation.Name = "cmdDeckInformation";
            this.cmdDeckInformation.Size = new System.Drawing.Size(229, 21);
            this.cmdDeckInformation.TabIndex = 2;
            this.cmdDeckInformation.Tag = "DECK_INFORMATION";
            this.cmdDeckInformation.Text = "Deck Information ...";
            this.cmdDeckInformation.UseVisualStyleBackColor = true;
            this.cmdDeckInformation.Click += new System.EventHandler(this.cmdDeckInformation_Click);
            // 
            // cmdEditName
            // 
            this.cmdEditName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEditName.Location = new System.Drawing.Point(147, 19);
            this.cmdEditName.Name = "cmdEditName";
            this.cmdEditName.Size = new System.Drawing.Size(88, 21);
            this.cmdEditName.TabIndex = 0;
            this.cmdEditName.Tag = "EDIT_NAME";
            this.cmdEditName.Text = "Edit Name ...";
            this.cmdEditName.UseVisualStyleBackColor = true;
            this.cmdEditName.Click += new System.EventHandler(this.cmdEditName_Click);
            // 
            // lblDeckName
            // 
            this.lblDeckName.AutoSize = true;
            this.lblDeckName.Location = new System.Drawing.Point(6, 23);
            this.lblDeckName.Margin = new System.Windows.Forms.Padding(3);
            this.lblDeckName.Name = "lblDeckName";
            this.lblDeckName.Size = new System.Drawing.Size(58, 13);
            this.lblDeckName.TabIndex = 1;
            this.lblDeckName.Text = "New Deck";
            // 
            // dgvDeckCards
            // 
            this.dgvDeckCards.AllowUserToAddRows = false;
            this.dgvDeckCards.AllowUserToOrderColumns = true;
            this.dgvDeckCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeckCards.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDeckCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeckCards.Location = new System.Drawing.Point(6, 67);
            this.dgvDeckCards.MultiSelect = false;
            this.dgvDeckCards.Name = "dgvDeckCards";
            this.dgvDeckCards.RowHeadersVisible = false;
            this.dgvDeckCards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeckCards.Size = new System.Drawing.Size(229, 364);
            this.dgvDeckCards.TabIndex = 1;
            this.dgvDeckCards.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvDeckCards_CellBeginEdit);
            this.dgvDeckCards.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeckCards_CellEndEdit);
            this.dgvDeckCards.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeckCards_CellMouseEnter);
            this.dgvDeckCards.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DeckCards_CellValidating);
            this.dgvDeckCards.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDeckCards_ColumnHeaderMouseClick);
            this.dgvDeckCards.SelectionChanged += new System.EventHandler(this.dgvDeckCards_SelectionChanged);
            this.dgvDeckCards.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDeckCards_MouseClick);
            // 
            // scRegularPromo
            // 
            this.scRegularPromo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scRegularPromo.Location = new System.Drawing.Point(0, 0);
            this.scRegularPromo.Name = "scRegularPromo";
            // 
            // scRegularPromo.Panel1
            // 
            this.scRegularPromo.Panel1.Controls.Add(this.gbRegularUnlocks);
            // 
            // scRegularPromo.Panel2
            // 
            this.scRegularPromo.Panel2.Controls.Add(this.gbPromoUnlocks);
            this.scRegularPromo.Size = new System.Drawing.Size(479, 464);
            this.scRegularPromo.SplitterDistance = 238;
            this.scRegularPromo.TabIndex = 0;
            // 
            // gbRegularUnlocks
            // 
            this.gbRegularUnlocks.Controls.Add(this.scRegularUnlockCount);
            this.gbRegularUnlocks.Controls.Add(this.scRegularButtons);
            this.gbRegularUnlocks.Controls.Add(this.dgvUnlocksRegular);
            this.gbRegularUnlocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRegularUnlocks.Location = new System.Drawing.Point(0, 0);
            this.gbRegularUnlocks.Name = "gbRegularUnlocks";
            this.gbRegularUnlocks.Size = new System.Drawing.Size(238, 464);
            this.gbRegularUnlocks.TabIndex = 2;
            this.gbRegularUnlocks.TabStop = false;
            this.gbRegularUnlocks.Tag = "REGULAR_UNLOCKS";
            this.gbRegularUnlocks.Text = "Regular Unlocks";
            // 
            // scRegularUnlockCount
            // 
            this.scRegularUnlockCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scRegularUnlockCount.IsSplitterFixed = true;
            this.scRegularUnlockCount.Location = new System.Drawing.Point(6, 19);
            this.scRegularUnlockCount.Name = "scRegularUnlockCount";
            // 
            // scRegularUnlockCount.Panel1
            // 
            this.scRegularUnlockCount.Panel1.Controls.Add(this.lblRegUnlockCount);
            // 
            // scRegularUnlockCount.Panel2
            // 
            this.scRegularUnlockCount.Panel2.Controls.Add(this.lblRegUnlockCardCount);
            this.scRegularUnlockCount.Size = new System.Drawing.Size(226, 15);
            this.scRegularUnlockCount.SplitterDistance = 148;
            this.scRegularUnlockCount.TabIndex = 3;
            // 
            // lblRegUnlockCount
            // 
            this.lblRegUnlockCount.AutoSize = true;
            this.lblRegUnlockCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegUnlockCount.Location = new System.Drawing.Point(0, 0);
            this.lblRegUnlockCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblRegUnlockCount.Name = "lblRegUnlockCount";
            this.lblRegUnlockCount.Size = new System.Drawing.Size(60, 13);
            this.lblRegUnlockCount.TabIndex = 3;
            this.lblRegUnlockCount.Tag = "COLUMN_TEXT_CARD_COUNT";
            this.lblRegUnlockCount.Text = "Card Count";
            this.lblRegUnlockCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRegUnlockCardCount
            // 
            this.lblRegUnlockCardCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRegUnlockCardCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegUnlockCardCount.Location = new System.Drawing.Point(0, 0);
            this.lblRegUnlockCardCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblRegUnlockCardCount.Name = "lblRegUnlockCardCount";
            this.lblRegUnlockCardCount.Size = new System.Drawing.Size(74, 15);
            this.lblRegUnlockCardCount.TabIndex = 4;
            this.lblRegUnlockCardCount.Tag = "";
            this.lblRegUnlockCardCount.Text = "0";
            this.lblRegUnlockCardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scRegularButtons
            // 
            this.scRegularButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scRegularButtons.IsSplitterFixed = true;
            this.scRegularButtons.Location = new System.Drawing.Point(6, 437);
            this.scRegularButtons.Name = "scRegularButtons";
            // 
            // scRegularButtons.Panel1
            // 
            this.scRegularButtons.Panel1.Controls.Add(this.cmdRegularMoveUp);
            // 
            // scRegularButtons.Panel2
            // 
            this.scRegularButtons.Panel2.Controls.Add(this.cmdRegularMoveDown);
            this.scRegularButtons.Size = new System.Drawing.Size(226, 21);
            this.scRegularButtons.SplitterDistance = 111;
            this.scRegularButtons.TabIndex = 2;
            // 
            // cmdRegularMoveUp
            // 
            this.cmdRegularMoveUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdRegularMoveUp.Location = new System.Drawing.Point(0, 0);
            this.cmdRegularMoveUp.Name = "cmdRegularMoveUp";
            this.cmdRegularMoveUp.Size = new System.Drawing.Size(111, 21);
            this.cmdRegularMoveUp.TabIndex = 1;
            this.cmdRegularMoveUp.Tag = "MOVE_UP";
            this.cmdRegularMoveUp.Text = "Move Up";
            this.cmdRegularMoveUp.UseVisualStyleBackColor = true;
            this.cmdRegularMoveUp.Click += new System.EventHandler(this.cmdRegularMoveUp_Click);
            // 
            // cmdRegularMoveDown
            // 
            this.cmdRegularMoveDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdRegularMoveDown.Location = new System.Drawing.Point(0, 0);
            this.cmdRegularMoveDown.Name = "cmdRegularMoveDown";
            this.cmdRegularMoveDown.Size = new System.Drawing.Size(111, 21);
            this.cmdRegularMoveDown.TabIndex = 2;
            this.cmdRegularMoveDown.Tag = "MOVE_DOWN";
            this.cmdRegularMoveDown.Text = "Move Down";
            this.cmdRegularMoveDown.UseVisualStyleBackColor = true;
            this.cmdRegularMoveDown.Click += new System.EventHandler(this.cmdRegularMoveDown_Click);
            // 
            // dgvUnlocksRegular
            // 
            this.dgvUnlocksRegular.AllowUserToAddRows = false;
            this.dgvUnlocksRegular.AllowUserToOrderColumns = true;
            this.dgvUnlocksRegular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUnlocksRegular.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvUnlocksRegular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnlocksRegular.Location = new System.Drawing.Point(6, 40);
            this.dgvUnlocksRegular.MultiSelect = false;
            this.dgvUnlocksRegular.Name = "dgvUnlocksRegular";
            this.dgvUnlocksRegular.RowHeadersVisible = false;
            this.dgvUnlocksRegular.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnlocksRegular.Size = new System.Drawing.Size(226, 391);
            this.dgvUnlocksRegular.TabIndex = 0;
            this.dgvUnlocksRegular.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvUnlocksRegular_CellBeginEdit);
            this.dgvUnlocksRegular.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnlocksRegular_CellEndEdit);
            this.dgvUnlocksRegular.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnlocksRegular_CellMouseEnter);
            this.dgvUnlocksRegular.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DeckCards_CellValidating);
            this.dgvUnlocksRegular.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvUnlocksRegular_RowsRemoved);
            this.dgvUnlocksRegular.SelectionChanged += new System.EventHandler(this.dgvUnlocksRegular_SelectionChanged);
            this.dgvUnlocksRegular.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvUnlocksRegular_MouseClick);
            // 
            // gbPromoUnlocks
            // 
            this.gbPromoUnlocks.Controls.Add(this.scPromoCount);
            this.gbPromoUnlocks.Controls.Add(this.scPromoButtons);
            this.gbPromoUnlocks.Controls.Add(this.dgvUnlocksPromo);
            this.gbPromoUnlocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPromoUnlocks.Location = new System.Drawing.Point(0, 0);
            this.gbPromoUnlocks.Name = "gbPromoUnlocks";
            this.gbPromoUnlocks.Size = new System.Drawing.Size(237, 464);
            this.gbPromoUnlocks.TabIndex = 3;
            this.gbPromoUnlocks.TabStop = false;
            this.gbPromoUnlocks.Tag = "PROMO_UNLOCKS";
            this.gbPromoUnlocks.Text = "Promo Unlocks";
            // 
            // scPromoCount
            // 
            this.scPromoCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scPromoCount.IsSplitterFixed = true;
            this.scPromoCount.Location = new System.Drawing.Point(6, 19);
            this.scPromoCount.Name = "scPromoCount";
            // 
            // scPromoCount.Panel1
            // 
            this.scPromoCount.Panel1.Controls.Add(this.lblPromoUnlockCount);
            // 
            // scPromoCount.Panel2
            // 
            this.scPromoCount.Panel2.Controls.Add(this.lblPromoUnlockCardCount);
            this.scPromoCount.Size = new System.Drawing.Size(225, 15);
            this.scPromoCount.SplitterDistance = 147;
            this.scPromoCount.TabIndex = 4;
            // 
            // lblPromoUnlockCount
            // 
            this.lblPromoUnlockCount.AutoSize = true;
            this.lblPromoUnlockCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPromoUnlockCount.Location = new System.Drawing.Point(0, 0);
            this.lblPromoUnlockCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblPromoUnlockCount.Name = "lblPromoUnlockCount";
            this.lblPromoUnlockCount.Size = new System.Drawing.Size(60, 13);
            this.lblPromoUnlockCount.TabIndex = 3;
            this.lblPromoUnlockCount.Tag = "COLUMN_TEXT_CARD_COUNT";
            this.lblPromoUnlockCount.Text = "Card Count";
            this.lblPromoUnlockCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPromoUnlockCardCount
            // 
            this.lblPromoUnlockCardCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPromoUnlockCardCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPromoUnlockCardCount.Location = new System.Drawing.Point(0, 0);
            this.lblPromoUnlockCardCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblPromoUnlockCardCount.Name = "lblPromoUnlockCardCount";
            this.lblPromoUnlockCardCount.Size = new System.Drawing.Size(74, 15);
            this.lblPromoUnlockCardCount.TabIndex = 4;
            this.lblPromoUnlockCardCount.Tag = "";
            this.lblPromoUnlockCardCount.Text = "0";
            this.lblPromoUnlockCardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scPromoButtons
            // 
            this.scPromoButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scPromoButtons.IsSplitterFixed = true;
            this.scPromoButtons.Location = new System.Drawing.Point(6, 437);
            this.scPromoButtons.Name = "scPromoButtons";
            // 
            // scPromoButtons.Panel1
            // 
            this.scPromoButtons.Panel1.Controls.Add(this.cmdPromoMoveUp);
            // 
            // scPromoButtons.Panel2
            // 
            this.scPromoButtons.Panel2.Controls.Add(this.cmdPromoMoveDown);
            this.scPromoButtons.Size = new System.Drawing.Size(225, 21);
            this.scPromoButtons.SplitterDistance = 110;
            this.scPromoButtons.TabIndex = 3;
            // 
            // cmdPromoMoveUp
            // 
            this.cmdPromoMoveUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdPromoMoveUp.Location = new System.Drawing.Point(0, 0);
            this.cmdPromoMoveUp.Name = "cmdPromoMoveUp";
            this.cmdPromoMoveUp.Size = new System.Drawing.Size(110, 21);
            this.cmdPromoMoveUp.TabIndex = 1;
            this.cmdPromoMoveUp.Tag = "MOVE_UP";
            this.cmdPromoMoveUp.Text = "Move Up";
            this.cmdPromoMoveUp.UseVisualStyleBackColor = true;
            this.cmdPromoMoveUp.Click += new System.EventHandler(this.cmdPromoMoveUp_Click);
            // 
            // cmdPromoMoveDown
            // 
            this.cmdPromoMoveDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdPromoMoveDown.Location = new System.Drawing.Point(0, 0);
            this.cmdPromoMoveDown.Name = "cmdPromoMoveDown";
            this.cmdPromoMoveDown.Size = new System.Drawing.Size(111, 21);
            this.cmdPromoMoveDown.TabIndex = 2;
            this.cmdPromoMoveDown.Tag = "MOVE_DOWN";
            this.cmdPromoMoveDown.Text = "Move Down";
            this.cmdPromoMoveDown.UseVisualStyleBackColor = true;
            this.cmdPromoMoveDown.Click += new System.EventHandler(this.cmdPromoMoveDown_Click);
            // 
            // dgvUnlocksPromo
            // 
            this.dgvUnlocksPromo.AllowUserToAddRows = false;
            this.dgvUnlocksPromo.AllowUserToOrderColumns = true;
            this.dgvUnlocksPromo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUnlocksPromo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvUnlocksPromo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnlocksPromo.Location = new System.Drawing.Point(6, 40);
            this.dgvUnlocksPromo.MultiSelect = false;
            this.dgvUnlocksPromo.Name = "dgvUnlocksPromo";
            this.dgvUnlocksPromo.RowHeadersVisible = false;
            this.dgvUnlocksPromo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnlocksPromo.Size = new System.Drawing.Size(225, 391);
            this.dgvUnlocksPromo.TabIndex = 0;
            this.dgvUnlocksPromo.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvUnlocksPromo_CellBeginEdit);
            this.dgvUnlocksPromo.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnlocksPromo_CellEndEdit);
            this.dgvUnlocksPromo.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnlocksPromo_CellMouseEnter);
            this.dgvUnlocksPromo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DeckCards_CellValidating);
            this.dgvUnlocksPromo.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvUnlocksPromo_RowsRemoved);
            this.dgvUnlocksPromo.SelectionChanged += new System.EventHandler(this.dgvUnlocksPromo_SelectionChanged);
            this.dgvUnlocksPromo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvUnlocksPromo_MouseClick);
            // 
            // cmnuContext
            // 
            this.cmnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiColumns,
            this.toolStripSeparator5,
            this.cmnuiRemoveCard,
            this.cmnuiViewCard,
            this.cmnuiDecksUsedIn,
            this.cmnuiExportCard,
            this.toolStripSeparator7,
            this.cmnuiExportPreviews,
            this.cmnuiExportImageCrop,
            this.toolStripSeparator9,
            this.cmnuiMoveTo});
            this.cmnuContext.Name = "cmnuContext";
            this.cmnuContext.Size = new System.Drawing.Size(185, 198);
            // 
            // cmnuiColumns
            // 
            this.cmnuiColumns.Name = "cmnuiColumns";
            this.cmnuiColumns.Size = new System.Drawing.Size(184, 22);
            this.cmnuiColumns.Tag = "COLUMNS";
            this.cmnuiColumns.Text = "&Columns";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(181, 6);
            // 
            // cmnuiRemoveCard
            // 
            this.cmnuiRemoveCard.Name = "cmnuiRemoveCard";
            this.cmnuiRemoveCard.Size = new System.Drawing.Size(184, 22);
            this.cmnuiRemoveCard.Tag = "REMOVE_CARD";
            this.cmnuiRemoveCard.Text = "&Remove Card";
            this.cmnuiRemoveCard.Click += new System.EventHandler(this.cmnuiRemoveCard_Click);
            // 
            // cmnuiViewCard
            // 
            this.cmnuiViewCard.Name = "cmnuiViewCard";
            this.cmnuiViewCard.Size = new System.Drawing.Size(184, 22);
            this.cmnuiViewCard.Tag = "VIEW_CARD";
            this.cmnuiViewCard.Text = "&View Card ...";
            this.cmnuiViewCard.Click += new System.EventHandler(this.cmnuiViewCard_Click);
            // 
            // cmnuiDecksUsedIn
            // 
            this.cmnuiDecksUsedIn.Name = "cmnuiDecksUsedIn";
            this.cmnuiDecksUsedIn.Size = new System.Drawing.Size(184, 22);
            this.cmnuiDecksUsedIn.Tag = "DECKS_USED_IN";
            this.cmnuiDecksUsedIn.Text = "&Decks Used In ...";
            this.cmnuiDecksUsedIn.Click += new System.EventHandler(this.cmnuiDecksUsedIn_Click);
            // 
            // cmnuiExportCard
            // 
            this.cmnuiExportCard.Name = "cmnuiExportCard";
            this.cmnuiExportCard.Size = new System.Drawing.Size(184, 22);
            this.cmnuiExportCard.Tag = "MENU_EXPORT_CARD_XML";
            this.cmnuiExportCard.Text = "E&xport Card XML";
            this.cmnuiExportCard.Click += new System.EventHandler(this.cmnuiExportCard_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(181, 6);
            // 
            // cmnuiExportPreviews
            // 
            this.cmnuiExportPreviews.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExportPreviewsPng,
            this.cmnuiExportPreviewsTdx});
            this.cmnuiExportPreviews.Name = "cmnuiExportPreviews";
            this.cmnuiExportPreviews.Size = new System.Drawing.Size(184, 22);
            this.cmnuiExportPreviews.Tag = "MENU_EXPORT_CARD_PREVIEWS";
            this.cmnuiExportPreviews.Text = "&Export Card Previews";
            // 
            // cmnuiExportPreviewsPng
            // 
            this.cmnuiExportPreviewsPng.Name = "cmnuiExportPreviewsPng";
            this.cmnuiExportPreviewsPng.Size = new System.Drawing.Size(110, 22);
            this.cmnuiExportPreviewsPng.Text = "&PNG ...";
            this.cmnuiExportPreviewsPng.Click += new System.EventHandler(this.cmnuiExportPreviewsPng_Click);
            // 
            // cmnuiExportPreviewsTdx
            // 
            this.cmnuiExportPreviewsTdx.Name = "cmnuiExportPreviewsTdx";
            this.cmnuiExportPreviewsTdx.Size = new System.Drawing.Size(110, 22);
            this.cmnuiExportPreviewsTdx.Text = "&TDX ...";
            this.cmnuiExportPreviewsTdx.Click += new System.EventHandler(this.cmnuiExportPreviewsTdx_Click);
            // 
            // cmnuiExportImageCrop
            // 
            this.cmnuiExportImageCrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExportImageCropPng,
            this.cmnuiExportImageCropTdx});
            this.cmnuiExportImageCrop.Name = "cmnuiExportImageCrop";
            this.cmnuiExportImageCrop.Size = new System.Drawing.Size(184, 22);
            this.cmnuiExportImageCrop.Tag = "MENU_EXPORT_IMAGE_CROP";
            this.cmnuiExportImageCrop.Text = "Ex&port Image Crop";
            // 
            // cmnuiExportImageCropPng
            // 
            this.cmnuiExportImageCropPng.Name = "cmnuiExportImageCropPng";
            this.cmnuiExportImageCropPng.Size = new System.Drawing.Size(98, 22);
            this.cmnuiExportImageCropPng.Text = "&PNG";
            this.cmnuiExportImageCropPng.Click += new System.EventHandler(this.cmnuiExportImageCropPng_Click);
            // 
            // cmnuiExportImageCropTdx
            // 
            this.cmnuiExportImageCropTdx.Name = "cmnuiExportImageCropTdx";
            this.cmnuiExportImageCropTdx.Size = new System.Drawing.Size(98, 22);
            this.cmnuiExportImageCropTdx.Text = "&TDX";
            this.cmnuiExportImageCropTdx.Click += new System.EventHandler(this.cmnuiExportImageCropTdx_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(181, 6);
            // 
            // cmnuiMoveTo
            // 
            this.cmnuiMoveTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiMoveToMainDeck,
            this.cmnuiMoveToRegularUnlocks,
            this.cmnuiMoveToPromoUnlocks});
            this.cmnuiMoveTo.Name = "cmnuiMoveTo";
            this.cmnuiMoveTo.Size = new System.Drawing.Size(184, 22);
            this.cmnuiMoveTo.Tag = "MENU_MOVE_CARD_TO";
            this.cmnuiMoveTo.Text = "&Move Card to";
            // 
            // cmnuiMoveToMainDeck
            // 
            this.cmnuiMoveToMainDeck.Name = "cmnuiMoveToMainDeck";
            this.cmnuiMoveToMainDeck.Size = new System.Drawing.Size(159, 22);
            this.cmnuiMoveToMainDeck.Tag = "MENU_MOVE_CARD_TO_MAIN_DECK";
            this.cmnuiMoveToMainDeck.Text = "M&ain Deck";
            this.cmnuiMoveToMainDeck.Click += new System.EventHandler(this.MoveCardTo);
            // 
            // cmnuiMoveToRegularUnlocks
            // 
            this.cmnuiMoveToRegularUnlocks.Name = "cmnuiMoveToRegularUnlocks";
            this.cmnuiMoveToRegularUnlocks.Size = new System.Drawing.Size(159, 22);
            this.cmnuiMoveToRegularUnlocks.Tag = "MENU_MOVE_CARD_TO_REGULAR_UNLOCKS";
            this.cmnuiMoveToRegularUnlocks.Text = "Re&gular Unlocks";
            this.cmnuiMoveToRegularUnlocks.Click += new System.EventHandler(this.MoveCardTo);
            // 
            // cmnuiMoveToPromoUnlocks
            // 
            this.cmnuiMoveToPromoUnlocks.Name = "cmnuiMoveToPromoUnlocks";
            this.cmnuiMoveToPromoUnlocks.Size = new System.Drawing.Size(159, 22);
            this.cmnuiMoveToPromoUnlocks.Tag = "MENU_MOVE_CARD_TO_PROMO_UNLOCKS";
            this.cmnuiMoveToPromoUnlocks.Text = "Pr&omo Unlocks";
            this.cmnuiMoveToPromoUnlocks.Click += new System.EventHandler(this.MoveCardTo);
            // 
            // cmnuPictures
            // 
            this.cmnuPictures.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExport});
            this.cmnuPictures.Name = "cmnuPictures";
            this.cmnuPictures.Size = new System.Drawing.Size(144, 26);
            // 
            // cmnuiExport
            // 
            this.cmnuiExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuiExportPng,
            this.cmnuiExportTdx});
            this.cmnuiExport.Name = "cmnuiExport";
            this.cmnuiExport.Size = new System.Drawing.Size(143, 22);
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
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslblSpacing,
            this.sslblLoadedCards,
            this.sslblLoadedCardsNum,
            this.sslblCardsInList,
            this.sslblCardsInListNum});
            this.ssStatus.Location = new System.Drawing.Point(0, 630);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(1110, 24);
            this.ssStatus.TabIndex = 10;
            // 
            // sslblSpacing
            // 
            this.sslblSpacing.Name = "sslblSpacing";
            this.sslblSpacing.Size = new System.Drawing.Size(897, 19);
            this.sslblSpacing.Spring = true;
            // 
            // sslblLoadedCards
            // 
            this.sslblLoadedCards.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sslblLoadedCards.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sslblLoadedCards.Name = "sslblLoadedCards";
            this.sslblLoadedCards.Size = new System.Drawing.Size(86, 19);
            this.sslblLoadedCards.Tag = "LOADED_CARDS";
            this.sslblLoadedCards.Text = "Loaded Cards:";
            // 
            // sslblLoadedCardsNum
            // 
            this.sslblLoadedCardsNum.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sslblLoadedCardsNum.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sslblLoadedCardsNum.Name = "sslblLoadedCardsNum";
            this.sslblLoadedCardsNum.Size = new System.Drawing.Size(17, 19);
            this.sslblLoadedCardsNum.Text = "0";
            // 
            // sslblCardsInList
            // 
            this.sslblCardsInList.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sslblCardsInList.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sslblCardsInList.Name = "sslblCardsInList";
            this.sslblCardsInList.Size = new System.Drawing.Size(78, 19);
            this.sslblCardsInList.Tag = "CARDS_IN_LIST";
            this.sslblCardsInList.Text = "Cards in List:";
            // 
            // sslblCardsInListNum
            // 
            this.sslblCardsInListNum.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sslblCardsInListNum.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sslblCardsInListNum.Name = "sslblCardsInListNum";
            this.sslblCardsInListNum.Size = new System.Drawing.Size(17, 19);
            this.sslblCardsInListNum.Text = "0";
            // 
            // DeckBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 654);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.scDeckUnlocks);
            this.Controls.Add(this.scFilterNormalAdvanced);
            this.Controls.Add(this.gbCardMoveOptions);
            this.Controls.Add(this.picFrame);
            this.Controls.Add(this.dgvCards);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "DeckBuilder";
            this.Text = "DotP 2014 Deck Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeckBuilder_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeckBuilder_FormClosed);
            this.Shown += new System.EventHandler(this.DeckBuilder_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).EndInit();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).EndInit();
            this.gbCardMoveOptions.ResumeLayout(false);
            this.gbCardMoveOptions.PerformLayout();
            this.scFilterNormalAdvanced.Panel1.ResumeLayout(false);
            this.scFilterNormalAdvanced.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scFilterNormalAdvanced)).EndInit();
            this.scFilterNormalAdvanced.ResumeLayout(false);
            this.scFilterOptions.Panel1.ResumeLayout(false);
            this.scFilterOptions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scFilterOptions)).EndInit();
            this.scFilterOptions.ResumeLayout(false);
            this.scDeckUnlocks.Panel1.ResumeLayout(false);
            this.scDeckUnlocks.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scDeckUnlocks)).EndInit();
            this.scDeckUnlocks.ResumeLayout(false);
            this.gbDeck.ResumeLayout(false);
            this.gbDeck.PerformLayout();
            this.scCounts.Panel1.ResumeLayout(false);
            this.scCounts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scCounts)).EndInit();
            this.scCounts.ResumeLayout(false);
            this.scTotalCount.Panel1.ResumeLayout(false);
            this.scTotalCount.Panel1.PerformLayout();
            this.scTotalCount.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTotalCount)).EndInit();
            this.scTotalCount.ResumeLayout(false);
            this.scBasicLand.Panel1.ResumeLayout(false);
            this.scBasicLand.Panel1.PerformLayout();
            this.scBasicLand.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scBasicLand)).EndInit();
            this.scBasicLand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeckCards)).EndInit();
            this.scRegularPromo.Panel1.ResumeLayout(false);
            this.scRegularPromo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scRegularPromo)).EndInit();
            this.scRegularPromo.ResumeLayout(false);
            this.gbRegularUnlocks.ResumeLayout(false);
            this.scRegularUnlockCount.Panel1.ResumeLayout(false);
            this.scRegularUnlockCount.Panel1.PerformLayout();
            this.scRegularUnlockCount.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scRegularUnlockCount)).EndInit();
            this.scRegularUnlockCount.ResumeLayout(false);
            this.scRegularButtons.Panel1.ResumeLayout(false);
            this.scRegularButtons.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scRegularButtons)).EndInit();
            this.scRegularButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnlocksRegular)).EndInit();
            this.gbPromoUnlocks.ResumeLayout(false);
            this.scPromoCount.Panel1.ResumeLayout(false);
            this.scPromoCount.Panel1.PerformLayout();
            this.scPromoCount.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scPromoCount)).EndInit();
            this.scPromoCount.ResumeLayout(false);
            this.scPromoButtons.Panel1.ResumeLayout(false);
            this.scPromoButtons.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scPromoButtons)).EndInit();
            this.scPromoButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnlocksPromo)).EndInit();
            this.cmnuContext.ResumeLayout(false);
            this.cmnuPictures.ResumeLayout(false);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvCards;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuiFile;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileNew;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileCreateFromExisting;
		private System.Windows.Forms.ToolStripMenuItem editExistingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileSave;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileSaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileExportTo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileExit;
		private System.Windows.Forms.ToolStripMenuItem mnuiTools;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuiAbout;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsRefresh;
		private System.Windows.Forms.PictureBox picFrame;
		private System.Windows.Forms.GroupBox gbCardMoveOptions;
		private System.Windows.Forms.RadioButton rbDoubleClickPromoUnlock;
		private System.Windows.Forms.RadioButton rbDoubleClickRegularUnlock;
		private System.Windows.Forms.RadioButton rbDoubleClickDeck;
		private System.Windows.Forms.SplitContainer scFilterOptions;
		private System.Windows.Forms.SplitContainer scFilterNormalAdvanced;
		private System.Windows.Forms.Button cmdClearFilters;
		private System.Windows.Forms.Button cmdSetFilter;
		private System.Windows.Forms.SplitContainer scDeckUnlocks;
		private System.Windows.Forms.GroupBox gbDeck;
		private System.Windows.Forms.SplitContainer scRegularPromo;
		private System.Windows.Forms.GroupBox gbRegularUnlocks;
		private System.Windows.Forms.GroupBox gbPromoUnlocks;
		private System.Windows.Forms.Label lblDeckName;
		private System.Windows.Forms.DataGridView dgvDeckCards;
		private System.Windows.Forms.Button cmdEditName;
		private System.Windows.Forms.DataGridView dgvUnlocksRegular;
		private System.Windows.Forms.DataGridView dgvUnlocksPromo;
		private System.Windows.Forms.Button cmdDeckInformation;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileExportToDirectory;
		private System.Windows.Forms.ToolStripMenuItem mnuiFileExportToWad;
		private System.Windows.Forms.SplitContainer scRegularButtons;
		private System.Windows.Forms.Button cmdRegularMoveUp;
		private System.Windows.Forms.Button cmdRegularMoveDown;
		private System.Windows.Forms.SplitContainer scPromoButtons;
		private System.Windows.Forms.Button cmdPromoMoveUp;
		private System.Windows.Forms.Button cmdPromoMoveDown;
		private System.Windows.Forms.ContextMenuStrip cmnuContext;
		private System.Windows.Forms.ToolStripMenuItem cmnuiColumns;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem cmnuiViewCard;
		private System.Windows.Forms.ToolStripMenuItem cmnuiDecksUsedIn;
		private System.Windows.Forms.ToolStripMenuItem cmnuiRemoveCard;
		private System.Windows.Forms.ContextMenuStrip cmnuPictures;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExport;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportPng;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportTdx;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportPreviews;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportPreviewsPng;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportPreviewsTdx;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsSetupCustomData;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsCreateCoreWad;
		private System.Windows.Forms.Button cmdFilterAdvanced;
		private System.Windows.Forms.StatusStrip ssStatus;
		private System.Windows.Forms.ToolStripStatusLabel sslblSpacing;
		private System.Windows.Forms.ToolStripStatusLabel sslblLoadedCards;
		private System.Windows.Forms.ToolStripStatusLabel sslblLoadedCardsNum;
		private System.Windows.Forms.ToolStripStatusLabel sslblCardsInList;
		private System.Windows.Forms.ToolStripStatusLabel sslblCardsInListNum;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportCard;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportImageCrop;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportImageCropPng;
		private System.Windows.Forms.ToolStripMenuItem cmnuiExportImageCropTdx;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsGenerateCPE;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem mnuiToolsCombineSpecData;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem cmnuiMoveTo;
		private System.Windows.Forms.ToolStripMenuItem cmnuiMoveToMainDeck;
		private System.Windows.Forms.ToolStripMenuItem cmnuiMoveToRegularUnlocks;
		private System.Windows.Forms.ToolStripMenuItem cmnuiMoveToPromoUnlocks;
		private System.Windows.Forms.SplitContainer scCounts;
		private System.Windows.Forms.SplitContainer scTotalCount;
		private System.Windows.Forms.Label lblTotalCount;
		private System.Windows.Forms.Label lblTotalCardCount;
		private System.Windows.Forms.SplitContainer scBasicLand;
		private System.Windows.Forms.Label lblBasicLand;
		private System.Windows.Forms.Label lblBasicLandCount;
		private System.Windows.Forms.SplitContainer scRegularUnlockCount;
		private System.Windows.Forms.Label lblRegUnlockCount;
		private System.Windows.Forms.Label lblRegUnlockCardCount;
		private System.Windows.Forms.SplitContainer scPromoCount;
		private System.Windows.Forms.Label lblPromoUnlockCount;
		private System.Windows.Forms.Label lblPromoUnlockCardCount;
	}
}

