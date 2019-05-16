using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class DeckBuilder : Form
	{
		private const string DECK_BUILDER_CUSTOM_DATA_WAD_NAME = "DATA_DLC_DECK_BUILDER_CUSTOM";

		private GameDirectory m_gdWads;
		private Filters m_fltCardFilter;
		private CardFilterSet m_cfsCardFilterAdvanced;
		private Timer m_tmrScroll;
		private CardInfo m_ciCurrentViewingCard;
		private string m_strGameDirectory;
		private Deck m_dkWorking;
		private ListChangedEventHandler m_lcehListHandler;
		private EventHandler m_ehColumnMenuItemClick;
		private CardInfo m_ciContextCard;
		private DeckCard m_dcContextCard;
		private int m_nContextRow;
		private DeckLocation m_eLocation;

		private bool m_bScrolling;

		private string m_strDeckCellValue;
		private string m_strUnlockRegularCellValue;
		private string m_strUnlockPromoCellValue;

		// For multi-column sorting.
		private BindingSource m_bsCards;
		private BindingSource m_bsDeckCards;
		private List<ColumnSort> m_lstCardSort;
		private List<ColumnSort> m_lstDeckCardSort;

		// To minimize reflection calls (columns aren't created/destroyed after start-up).
		private Dictionary<DataGridViewColumn, PropertyInfo> m_dicColumnMap;
		private MethodInfo m_miCustomTagValue;

		private enum DeckLocation
		{
			MainDeck = 0,
			RegularUnlocks,
			PromoUnlocks
		}

		public DeckBuilder()
		{
			InitializeComponent();

			// This will set the double buffering for the Data Grids well before we start loading data.
			SetDoubleBufferingForDataGrids();

			Settings.InitDefaults();
			Settings.LoadSettings();

			this.Text = Settings.GetAssemblyTitle();

			// Now we need to properly localized strings for this form.
			LoadLocalizedStrings();

			// Initialize necessary internal variables.
			m_ciCurrentViewingCard = null;
			m_miCustomTagValue = null;

			// Initialize the column map.
			m_dicColumnMap = new Dictionary<DataGridViewColumn, PropertyInfo>();

			Rectangle rcPosition = Settings.GetSetting("MainPosition", new Rectangle(-1, -1, -1, -1));
			if (rcPosition.X != -1)
			{
				bool bDoChecks = Settings.GetSetting("PerformBasicScreenChecks", true);
				if ((rcPosition.X >= 0) && (rcPosition.Y >= 0) &&
					(rcPosition.X < (Screen.PrimaryScreen.WorkingArea.X + Screen.PrimaryScreen.WorkingArea.Width - 50)) &&
					(rcPosition.Y < (Screen.PrimaryScreen.WorkingArea.Y + Screen.PrimaryScreen.WorkingArea.Height - 50)) || !bDoChecks)
				{
					this.StartPosition = FormStartPosition.Manual;
					this.Location = rcPosition.Location;
				}
				this.Size = rcPosition.Size;
			}
			FormWindowState fwsState = FormWindowState.Normal;
			if (Enum.TryParse(Settings.GetSetting("MainWindowState", FormWindowState.Normal.ToString()), out fwsState))
				this.WindowState = fwsState;

			m_strGameDirectory = Settings.GetSetting("DotP2014Directory", string.Empty);

			// Get our saved filter if any otherwise init the default.
			m_fltCardFilter = Settings.GetSerializableSetting("CardFilters", new Filters());
			m_cfsCardFilterAdvanced = Settings.GetSerializableSetting("CardFilterAdvanced", new CardFilterSet());
		}

		public void LoadLocalizedStrings()
		{
			// I could simply iterate over all of the controls in this form and replace
			//	their text with localized strings using the tags as a key, but then I
			//	might forget to resize certain other controls when their label counterpart
			//	changes.

			// Load strings for the Menus:
			foreach (ToolStripItem mnuiItem in mnuMain.Items)
			{
				if (mnuiItem.GetType() == typeof(ToolStripMenuItem))
					LoadMenuItemStrings((ToolStripMenuItem)mnuiItem);
			}

			// Load strings for the context menus:
			foreach (ToolStripItem mnuiItem in cmnuContext.Items)
			{
				if (mnuiItem.GetType() == typeof(ToolStripMenuItem))
					LoadMenuItemStrings((ToolStripMenuItem)mnuiItem);
			}

			RefreshColumnNames(dgvCards);
			dgvCards.Refresh();
			RefreshColumnNames(dgvDeckCards);
			dgvDeckCards.Refresh();
			RefreshColumnNames(dgvUnlocksRegular);
			dgvUnlocksRegular.Refresh();
			RefreshColumnNames(dgvUnlocksPromo);
			dgvUnlocksPromo.Refresh();
			ShowCardInfo(m_ciCurrentViewingCard);

			// Double-Click move to box and radio buttons.
			gbCardMoveOptions.Text = Settings.UIStrings[(string)gbCardMoveOptions.Tag];
			rbDoubleClickDeck.Text = Settings.UIStrings[(string)rbDoubleClickDeck.Tag];
			rbDoubleClickRegularUnlock.Location = new Point(rbDoubleClickDeck.Location.X + rbDoubleClickDeck.Size.Width + 6, rbDoubleClickRegularUnlock.Location.Y);
			rbDoubleClickRegularUnlock.Text = Settings.UIStrings[(string)rbDoubleClickRegularUnlock.Tag];
			rbDoubleClickPromoUnlock.Location = new Point(rbDoubleClickRegularUnlock.Location.X + rbDoubleClickRegularUnlock.Size.Width + 6, rbDoubleClickPromoUnlock.Location.Y);
			rbDoubleClickPromoUnlock.Text = Settings.UIStrings[(string)rbDoubleClickPromoUnlock.Tag];

			// Filter Option Buttons
			cmdClearFilters.Text = Settings.UIStrings[(string)cmdClearFilters.Tag];
			cmdSetFilter.Text = Settings.UIStrings[(string)cmdSetFilter.Tag];
			cmdFilterAdvanced.Text = Settings.UIStrings[(string)cmdFilterAdvanced.Tag];

			// Deck Group
			gbDeck.Text = Settings.UIStrings[(string)gbDeck.Tag];
			if (m_dkWorking != null)
				lblDeckName.Text = m_dkWorking.LocalizedName;
			lblBasicLand.Text = Settings.UIStrings[(string)lblBasicLand.Tag];
			lblTotalCount.Text = Settings.UIStrings[(string)lblTotalCount.Tag];
			cmdEditName.Text = Settings.UIStrings[(string)cmdEditName.Tag];
			cmdDeckInformation.Text = Settings.UIStrings[(string)cmdDeckInformation.Tag];

			// Regular Unlock Group
			gbRegularUnlocks.Text = Settings.UIStrings[(string)gbRegularUnlocks.Tag];
			lblRegUnlockCount.Text = Settings.UIStrings[(string)lblRegUnlockCount.Tag];

			// Promo Unlock Group
			gbPromoUnlocks.Text = Settings.UIStrings[(string)gbPromoUnlocks.Tag];
			lblPromoUnlockCount.Text = Settings.UIStrings[(string)lblPromoUnlockCount.Tag];

			// Context Menu
			cmnuiColumns.Text = Settings.UIStrings[(string)cmnuiColumns.Tag];
			cmnuiRemoveCard.Text = Settings.UIStrings[(string)cmnuiRemoveCard.Tag];
			cmnuiViewCard.Text = Settings.UIStrings[(string)cmnuiViewCard.Tag];
			cmnuiDecksUsedIn.Text = Settings.UIStrings[(string)cmnuiDecksUsedIn.Tag];
			cmnuiExport.Text = Settings.UIStrings[(string)cmnuiExport.Tag];
			cmnuiExportCard.Text = Settings.UIStrings[(string)cmnuiExportCard.Tag];
			cmnuiExportImageCrop.Text = Settings.UIStrings[(string)cmnuiExportImageCrop.Tag];
			cmnuiExportPreviews.Text = Settings.UIStrings[(string)cmnuiExportPreviews.Tag];

			// Status Bar
			sslblLoadedCards.Text = Settings.UIStrings[(string)sslblLoadedCards.Tag];
			sslblCardsInList.Text = Settings.UIStrings[(string)sslblCardsInList.Tag];
		}

		private void LoadMenuItemStrings(ToolStripMenuItem mnuiItem)
		{
			if ((mnuiItem.Tag != null) && (((string)mnuiItem.Tag).Length > 0))
				mnuiItem.Text = Settings.UIStrings[(string)mnuiItem.Tag];
			if (mnuiItem.DropDownItems.Count > 0)
			{
				foreach (ToolStripItem mnuiSubItem in mnuiItem.DropDownItems)
				{
					if (mnuiSubItem.GetType() == typeof(ToolStripMenuItem))
						LoadMenuItemStrings((ToolStripMenuItem)mnuiSubItem);
				}
			}
		}

		public void RefreshGameData()
		{
			this.Cursor = Cursors.WaitCursor;
			m_strGameDirectory = Settings.GetSetting("DotP2014Directory", m_strGameDirectory);
			m_gdWads = new GameDirectory(m_strGameDirectory);
			m_gdWads.LoadMusic();
			m_gdWads.LoadWads();
			bool bFirstSetup = SetupCardList();
			m_bsCards = new BindingSource();
			if ((Settings.GetSetting("Filtering", true)) && (Settings.GetSetting("AdvancedFiltering", false)) && (m_cfsCardFilterAdvanced != null))
				m_bsCards.DataSource = new SortableBindingList<CardInfo>(m_gdWads.Cards.Where(x => m_cfsCardFilterAdvanced.IsAllowed(x)));
			else if ((Settings.GetSetting("Filtering", true)) && (m_fltCardFilter != null))
				m_bsCards.DataSource = new SortableBindingList<CardInfo>(m_gdWads.Cards.Where(x => m_fltCardFilter.CheckAgainstFilter(x)));
			else
				m_bsCards.DataSource = m_gdWads.Cards;
			//dgvCards.DataSource = m_bsCards;
			dgvCards.RowCount = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count;
			// Update status numbers.
			sslblLoadedCardsNum.Text = m_gdWads.Cards.Count.ToString();
			sslblCardsInListNum.Text = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count.ToString();
			// Restore the previous sort.
			RestoreCardSort();
			if (bFirstSetup)
			{
				foreach (DataGridViewColumn dgvc in dgvCards.Columns)
					dgvc.Width = dgvc.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
			}
			// Refresh preview card (if any)
			if (m_ciCurrentViewingCard != null)
			{
				string strCard = m_ciCurrentViewingCard.Filename;
				m_ciCurrentViewingCard = m_gdWads.GetCardByFileName(strCard);
				ShowCardInfo(m_ciCurrentViewingCard);
			}

			// Check to see if we have read/write access to directory.
			bool bHasWriteAccess = false;	// We assume no access by default.
			try
			{
				DirectorySecurity dsDir = Directory.GetAccessControl(m_gdWads.GameDir);
				if (dsDir != null)
				{
					AuthorizationRuleCollection arcRules = dsDir.GetAccessRules(true, true, typeof(SecurityIdentifier));
					bool bHasCreateDirs = false;
					bool bHasCreateFiles = false;
					bool bHasDelete = false;
					bool bHasWrite = false;
					foreach (FileSystemAccessRule fsarRule in arcRules)
					{
						// Create Directories
						if ((fsarRule.FileSystemRights & FileSystemRights.CreateDirectories) == FileSystemRights.CreateDirectories)
						{
							if (fsarRule.AccessControlType == AccessControlType.Allow)
							{
								// We will allow access if there is an Allow.
								bHasCreateDirs = true;
							}
							else if (fsarRule.AccessControlType == AccessControlType.Deny)
							{
								// If there is a Deny then it doesn't matter if we have previously come across any Allows.
								bHasCreateDirs = false;
								break;
							}
						}
						// Create Files
						if ((fsarRule.FileSystemRights & FileSystemRights.CreateFiles) == FileSystemRights.CreateFiles)
						{
							if (fsarRule.AccessControlType == AccessControlType.Allow)
							{
								// We will allow access if there is an Allow.
								bHasCreateFiles = true;
							}
							else if (fsarRule.AccessControlType == AccessControlType.Deny)
							{
								// If there is a Deny then it doesn't matter if we have previously come across any Allows.
								bHasCreateFiles = false;
								break;
							}
						}
						// Delete
						if ((fsarRule.FileSystemRights & FileSystemRights.DeleteSubdirectoriesAndFiles) == FileSystemRights.DeleteSubdirectoriesAndFiles)
						{
							if (fsarRule.AccessControlType == AccessControlType.Allow)
							{
								// We will allow access if there is an Allow.
								bHasDelete = true;
							}
							else if (fsarRule.AccessControlType == AccessControlType.Deny)
							{
								// If there is a Deny then it doesn't matter if we have previously come across any Allows.
								bHasDelete = false;
								break;
							}
						}
						// Write
						if ((fsarRule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
						{
							if (fsarRule.AccessControlType == AccessControlType.Allow)
							{
								// We will allow access if there is an Allow.
								bHasWrite = true;
							}
							else if (fsarRule.AccessControlType == AccessControlType.Deny)
							{
								// If there is a Deny then it doesn't matter if we have previously come across any Allows.
								bHasWrite = false;
								break;
							}
						}
					}
					bHasWriteAccess = bHasCreateDirs && bHasCreateFiles && bHasDelete && bHasWrite;
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.High, "Builder does not have Write access to game directory, exporting and some other features will be disabled.");
			}
			if (bHasWriteAccess)
			{
				mnuiFileExportTo.Enabled = true;
			}
			else
			{
				mnuiFileExportTo.Enabled = false;
			}

			// Check to see if our custom data exists.
			bool bFound = false;
			foreach (WadBase wad in m_gdWads.LoadedWads)
			{
				if (wad.Name.Equals(DECK_BUILDER_CUSTOM_DATA_WAD_NAME, StringComparison.OrdinalIgnoreCase))
				{
					bFound = true;
					break;
				}
			}
			if (bHasWriteAccess)
				mnuiToolsSetupCustomData.Enabled = !bFound;
			else
				mnuiToolsSetupCustomData.Enabled = false;
			mnuiToolsCreateCoreWad.Enabled = bFound;

			this.Cursor = Cursors.Default;
		}

		private void RestoreCardSort()
		{
			// Load the list
			List<ColumnSort> lstSort = Settings.GetSerializableSetting("CardViewSort", new List<ColumnSort>());
			if (lstSort.Count <= 0)
				lstSort.Add(new ColumnSort("Name", "LocalizedName", SortOrder.Ascending));

			// Actually sort.
			m_lstCardSort = lstSort;
			Tools.SortFromList(dgvCards, m_bsCards, lstSort);
		}

		private void RestoreDeckCardSort()
		{
			/*
			// Load the list
			List<ColumnSort> lstSort = Settings.GetSerializableSetting("DeckCardViewSort", new List<ColumnSort>());
			if (lstSort.Count <= 0)
				lstSort.Add(new ColumnSort("Name", "LocalizedCardName", SortOrder.Ascending));

			// Actually sort.
			m_lstDeckCardSort = lstSort;
			Tools.SortFromList(dgvDeckCards, m_bsDeckCards, lstSort);
			//*/
		}

		private void SaveCardSort()
		{
			Settings.SaveSerializableSetting("CardViewSort", m_lstCardSort);
		}

		private void SaveDeckCardSort()
		{
			//Settings.SaveSerializableSetting("DeckCardViewSort", m_lstDeckCardSort);
		}

		public void RefreshDeck()
		{
			lblDeckName.Text = m_dkWorking.LocalizedName;
			lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
			lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
			SetupDeckCardList();
			m_bsDeckCards = new BindingSource();
			m_bsDeckCards.DataSource = m_dkWorking.Cards;
			dgvDeckCards.DataSource = m_bsDeckCards;
			dgvDeckCards.Refresh();
			RestoreDeckCardSort();
			SetupUnlockCardList(dgvUnlocksRegular, "RegularUnlocksViewColumns");
			dgvUnlocksRegular.DataSource = m_dkWorking.RegularUnlocks.Cards;
			dgvUnlocksRegular.Refresh();
			lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
			CheckRegularButtons();
			SetupUnlockCardList(dgvUnlocksPromo, "PromoUnlocksViewColumns");
			dgvUnlocksPromo.DataSource = m_dkWorking.PromoUnlocks.Cards;
			dgvUnlocksPromo.Refresh();
			lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
			CheckPromoButtons();
		}

		// This is merely to setup the columns for the card list.
		private bool SetupCardList()
		{
			bool bSetup = false;
			if (dgvCards.Columns.Count == 0)
			{
				// We will change this to false again if we actually have saved column data to load.
				bSetup = true;

				dgvCards.AutoGenerateColumns = false;
				dgvCards.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				dgvCards.AllowUserToAddRows = false;
				dgvCards.AllowUserToDeleteRows = false;

				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Name", "LocalizedName", "COLUMN_TEXT_NAME", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Filename", "Filename", "COLUMN_TEXT_FILE_NAME", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Colour", "ColourText", "COLUMN_TEXT_COLOUR", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewImageColumn(), "Cost", "CastingCostImage", "COLUMN_TEXT_CASTING_COST", DataGridViewColumnSortMode.NotSortable);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "CostText", "CastingCost", "COLUMN_TEXT_CASTING_COST_TEXT", DataGridViewColumnSortMode.Programmatic, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "CMC", "ConvertedManaCost", "COLUMN_TEXT_CMC", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Type", "LocalizedTypeLine", "COLUMN_TEXT_TYPE", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Power", "Power", "COLUMN_TEXT_POWER", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Toughness", "Toughness", "COLUMN_TEXT_TOUGHNESS", DataGridViewColumnSortMode.Programmatic);
				Tools.AddViewColumn(dgvCards, new DataGridViewCheckBoxColumn(), "Token", "Token", "COLUMN_TEXT_TOKEN", DataGridViewColumnSortMode.Programmatic, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Rarity", "Rarity", "COLUMN_TEXT_RARITY", DataGridViewColumnSortMode.Programmatic, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Artist", "Artist", "COLUMN_TEXT_ARTIST", DataGridViewColumnSortMode.Programmatic, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Expansion", "Expansion", "COLUMN_TEXT_EXPANSION", DataGridViewColumnSortMode.Programmatic, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "Wad", "PresentInWad", "COLUMN_TEXT_WAD", DataGridViewColumnSortMode.Programmatic);

				// Let's add some custom tag columns related to the Community Wad (custom tags are not sortable unfortunately):
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "CustomTag:AUTHOR", "CustomTag:AUTHOR", "CustomTag:Author", DataGridViewColumnSortMode.NotSortable, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "CustomTag:EDITORS", "CustomTag:EDITORS", "CustomTag:Editors", DataGridViewColumnSortMode.NotSortable, -1, false);
				Tools.AddViewColumn(dgvCards, new DataGridViewTextBoxColumn(), "CustomTag:DATE", "CustomTag:DATE", "CustomTag:Date", DataGridViewColumnSortMode.NotSortable, -1, false);

				bSetup = !Settings.GetSetting("CardViewColumns", dgvCards.Columns);
			}
			return bSetup;
		}

		private void SetupDeckCardList()
		{
			if (dgvDeckCards.Columns.Count == 0)
			{
				dgvDeckCards.AutoGenerateColumns = false;
				dgvDeckCards.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				dgvDeckCards.AllowUserToAddRows = false;
				dgvDeckCards.AllowUserToDeleteRows = true;

				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Name", "LocalizedCardName", "COLUMN_TEXT_NAME", DataGridViewColumnSortMode.Automatic, 130);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Quantity", "Quantity", "COLUMN_TEXT_QUANTITY", DataGridViewColumnSortMode.Automatic, 54);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Bias", "Bias", "COLUMN_TEXT_BIAS", DataGridViewColumnSortMode.Automatic, 35);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewCheckBoxColumn(), "Promo", "Promo", "COLUMN_TEXT_PROMO", DataGridViewColumnSortMode.Automatic, 44);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Filename", "Filename", "COLUMN_TEXT_FILE_NAME", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Colour", "ColourText", "COLUMN_TEXT_COLOUR", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewImageColumn(), "Cost", "CastingCostImage", "COLUMN_TEXT_CASTING_COST", DataGridViewColumnSortMode.NotSortable, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "CostText", "CastingCost", "COLUMN_TEXT_CASTING_COST_TEXT", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "CMC", "ConvertedManaCost", "COLUMN_TEXT_CMC", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Type", "LocalizedTypeLine", "COLUMN_TEXT_TYPE", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Power", "Power", "COLUMN_TEXT_POWER", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Toughness", "Toughness", "COLUMN_TEXT_TOUGHNESS", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewCheckBoxColumn(), "Token", "Token", "COLUMN_TEXT_TOKEN", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Rarity", "Rarity", "COLUMN_TEXT_RARITY", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Artist", "Artist", "COLUMN_TEXT_ARTIST", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Expansion", "Expansion", "COLUMN_TEXT_EXPANSION", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvDeckCards, new DataGridViewTextBoxColumn(), "Wad", "PresentInWad", "COLUMN_TEXT_WAD", DataGridViewColumnSortMode.Automatic, 130);

				Settings.GetSetting("DeckCardViewColumns", dgvDeckCards.Columns);
			}
		}

		private void SetupUnlockCardList(DataGridView dgvList, string strSetting)
		{
			if (dgvList.Columns.Count == 0)
			{
				dgvList.AutoGenerateColumns = false;
				dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				dgvList.AllowUserToAddRows = false;
				dgvList.AllowUserToDeleteRows = true;

				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Name", "LocalizedCardName", "COLUMN_TEXT_NAME", DataGridViewColumnSortMode.Automatic, 130);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Bias", "Bias", "COLUMN_TEXT_BIAS", DataGridViewColumnSortMode.Automatic, 35);
				Tools.AddViewColumn(dgvList, new DataGridViewCheckBoxColumn(), "Promo", "Promo", "COLUMN_TEXT_PROMO", DataGridViewColumnSortMode.Automatic, 44);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Filename", "Filename", "COLUMN_TEXT_FILE_NAME", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Colour", "ColourText", "COLUMN_TEXT_COLOUR", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewImageColumn(), "Cost", "CastingCostImage", "COLUMN_TEXT_CASTING_COST", DataGridViewColumnSortMode.NotSortable, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "CostText", "CastingCost", "COLUMN_TEXT_CASTING_COST_TEXT", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "CMC", "ConvertedManaCost", "COLUMN_TEXT_CMC", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Type", "LocalizedTypeLine", "COLUMN_TEXT_TYPE", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Power", "Power", "COLUMN_TEXT_POWER", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Toughness", "Toughness", "COLUMN_TEXT_TOUGHNESS", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewCheckBoxColumn(), "Token", "Token", "COLUMN_TEXT_TOKEN", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Rarity", "Rarity", "COLUMN_TEXT_RARITY", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Artist", "Artist", "COLUMN_TEXT_ARTIST", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Expansion", "Expansion", "COLUMN_TEXT_EXPANSION", DataGridViewColumnSortMode.Automatic, -1, false);
				Tools.AddViewColumn(dgvList, new DataGridViewTextBoxColumn(), "Wad", "PresentInWad", "COLUMN_TEXT_WAD", DataGridViewColumnSortMode.Automatic, 130);

				Settings.GetSetting(strSetting, dgvList.Columns);

				// Unfortunately due to the nature of unlocks and order being important we need to make sure that all the columns are non-sortable.
				//	This prevents people from modifying the settings.xml file to put in sorting.
				foreach (DataGridViewColumn dgvc in dgvList.Columns)
					dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
			}
		}

		private void RefreshColumnNames(DataGridView dgvList)
		{
			foreach (DataGridViewColumn dgvcColumn in dgvList.Columns)
			{
				if (((string)dgvcColumn.Tag).StartsWith("CustomTag:", StringComparison.OrdinalIgnoreCase))
				{
					// Custom Tag title processing.
					string strTitle = ((string)dgvcColumn.Tag).Substring(10);
					dgvcColumn.HeaderText = strTitle;
				}
				else
					dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
			}
		}

		private void dgvCards_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if ((e.ColumnIndex >= 0) && (e.ColumnIndex < dgvCards.Columns.Count) && ((e.Button & System.Windows.Forms.MouseButtons.Right) == 0))
			{
				DataGridViewColumn dgvcColumn = dgvCards.Columns[e.ColumnIndex];
				if (dgvcColumn.SortMode == DataGridViewColumnSortMode.Programmatic)
				{
					if (Control.ModifierKeys == Keys.Shift)
					{
						// Add or Modify an existing sort.
						Tools.AdjustSort(dgvCards, m_bsCards, m_lstCardSort, dgvcColumn.DataPropertyName, false);
					}
					else if (Control.ModifierKeys == Keys.Control)
					{
						Tools.AdjustSort(dgvCards, m_bsCards, m_lstCardSort, dgvcColumn.DataPropertyName, true);
					}
					else
					{
						// Regular single sort (or reverse).
						string strProp = dgvcColumn.DataPropertyName;
						SortOrder soDirection = (dgvcColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);
						m_lstCardSort = new List<ColumnSort>();
						m_lstCardSort.Add(new ColumnSort(dgvcColumn.Name, strProp, soDirection));
						Tools.SortFromList(dgvCards, m_bsCards, m_lstCardSort);
					}
					SaveCardSort();
				}
			}
		}

		private void dgvCards_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (!m_bScrolling)
			{
				if ((e.RowIndex > -1) && (e.RowIndex < dgvCards.RowCount) &&
					(e.ColumnIndex > -1) && (e.ColumnIndex < dgvCards.Columns.Count))
				{
					CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[e.RowIndex];
					ShowCardInfo(ciCard);
				}
			}
		}

		private void dgvCards_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// Here I add cards to either the deck or one of the unlocks (which is still in the deck object).
			if ((e.RowIndex > -1) && (e.RowIndex < dgvCards.RowCount) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvCards.Columns.Count))
			{
				CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[e.RowIndex];
				m_dkWorking.Edited = true;
				if (rbDoubleClickDeck.Checked)
				{
					DeckCard dcCard = m_dkWorking.AddCard(ciCard);
					int nIndex = m_dkWorking.Cards.IndexOf(dcCard);
					lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
					lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
					dgvDeckCards.CurrentCell = dgvDeckCards.Rows[nIndex].Cells[0];
					dgvDeckCards.Refresh();
				}
				else if (rbDoubleClickRegularUnlock.Checked)
				{
					m_dkWorking.RegularUnlocks.Cards.Add(new DeckCard(ciCard));
					lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
					dgvUnlocksRegular.CurrentCell = dgvUnlocksRegular.Rows[dgvUnlocksRegular.RowCount - 1].Cells[0];
					CheckRegularButtons();
				}
				else
				{
					// Only thing left is the Promo Unlocks.
					m_dkWorking.PromoUnlocks.Cards.Add(new DeckCard(ciCard));
					lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
					dgvUnlocksPromo.CurrentCell = dgvUnlocksPromo.Rows[dgvUnlocksPromo.RowCount - 1].Cells[0];
					CheckPromoButtons();
				}
			}
		}

		private void ShowCardInfo(CardInfo ciCard)
		{
			if (ciCard != null)
			{
				if (m_ciCurrentViewingCard != ciCard)
					m_ciCurrentViewingCard = ciCard;
				picFrame.Image = ciCard.GetLocalizedPreviewImage();
			}
			else
				picFrame.Image = null;
		}

		private void dgvCards_Scroll(object sender, ScrollEventArgs e)
		{
			m_bScrolling = true;
			if (m_tmrScroll == null)
			{
				m_tmrScroll = new Timer();
				m_tmrScroll.Tick += new EventHandler(m_tmrScroll_Tick);
			}
			m_tmrScroll.Stop();
			m_tmrScroll.Interval = 100;
			m_tmrScroll.Start();
		}

		void m_tmrScroll_Tick(object sender, EventArgs e)
		{
			m_bScrolling = false;
		}

		private void DeckBuilder_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((m_dkWorking != null) && (m_dkWorking.Edited))
			{
				// They have unsaved work so we should probably ask if they want to save first.
				DialogResult drChoice = MessageBox.Show(Settings.UIStrings["UNSAVED_WORK_MESSAGE"], Settings.UIStrings["UNSAVED_WORK_CAPTION"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (drChoice == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
				else if (drChoice == DialogResult.Yes)
				{
					// We only continue if saving completes successfully.
					if (!SaveDeck())
					{
						e.Cancel = true;
						return;
					}
				}
			}

			// We know we are about to close this form and exit the program so we should take this time to save any unsaved settings (like form position and size).
			if (this.WindowState == FormWindowState.Normal)
				Settings.SaveSetting("MainPosition", new Rectangle(this.Location, this.Size));
			if (this.WindowState != FormWindowState.Minimized)
				Settings.SaveSetting("MainWindowState", this.WindowState.ToString());
			Settings.SaveSetting("CardViewColumns", dgvCards.Columns);
			Settings.SaveSetting("DeckCardViewColumns", dgvDeckCards.Columns);
			Settings.SaveSetting("RegularUnlocksViewColumns", dgvUnlocksRegular.Columns);
			Settings.SaveSetting("PromoUnlocksViewColumns", dgvUnlocksPromo.Columns);
			Settings.SaveSerializableSetting("CardFilters", m_fltCardFilter);
			Settings.SaveSerializableSetting("CardFilterAdvanced", m_cfsCardFilterAdvanced);
		}

		private void DeckBuilder_FormClosed(object sender, FormClosedEventArgs e)
		{
			// Save our settings to file so that we can get them back once we re-open the program.
			Settings.SaveSettings();
			// Nothing left to do as we are exiting the program at this point except clean up.
			Settings.Cleanup();
		}

		private void mnuiFileExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuiAbout_Click(object sender, EventArgs e)
		{
			AboutBox frmAbout = new AboutBox();
			frmAbout.ShowDialog(this);
			frmAbout.Dispose();
		}

		private void mnuiToolsRefresh_Click(object sender, EventArgs e)
		{
			// If we haven't edited the file then we can refresh without any warning.
			if (m_dkWorking.Edited)
			{
				// We could refresh from memory, but if a card was present and then
				//	suddenly it can't be loaded due to one reason or another (broke
				//	on an update, attempt to change something and messed up) then
				//	this should assure at least a good attempt to make sure they saved.
				DialogResult drResult = MessageBox.Show(Settings.UIStrings["REFRESH_DATA_WARNING_TEXT"], Settings.UIStrings["REFRESH_DATA_WARNING_CAPTION"], MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (drResult == DialogResult.Yes)
				{
                    Settings.ResetErrorLog();
					RefreshGameData();
					if (m_dkWorking != null)
					{
						string strFilename = m_dkWorking.FileName;
						if ((strFilename != null) && (strFilename.Length > 0) && (File.Exists(strFilename)))
							AttemptToLoadDeck(strFilename);
						else
							mnuiFileNew_Click(null, null);
					}

                    // Check to see if there were any errors during load.
                    if (Settings.ErrorLog.Opened)
                    {
                        // Error log was opened so we have errors, now we need to show them to user.
                        ErrorReportWindow frmErrors = new ErrorReportWindow(Settings.ErrorLog);
                        frmErrors.Show(this);
					}
				}
			}
			else
			{
                Settings.ResetErrorLog();
				RefreshGameData();
				if (m_dkWorking != null)
				{
					string strFilename = m_dkWorking.FileName;
					if ((strFilename != null) && (strFilename.Length > 0) && (File.Exists(strFilename)))
						AttemptToLoadDeck(strFilename);
					else
						mnuiFileNew_Click(null, null);
				}

                // Check to see if there were any errors during load.
                if (Settings.ErrorLog.Opened)
                {
                    // Error log was opened so we have errors, now we need to show them to user.
                    ErrorReportWindow frmErrors = new ErrorReportWindow(Settings.ErrorLog);
                    frmErrors.Show(this);
				}
			}
		}

		private void mnuiToolsOptions_Click(object sender, EventArgs e)
		{
			Options frmOptions = new Options(this);
			frmOptions.ShowDialog(this);
			// If they decided to change the game directory while working on a deck then tough
			//	they don't get a message about needing to reload everything.  If they haven't
			//	saved then maybe they will learn their lesson.  Changing the game directory
			//	should be very rare because for most people it doesn't move around.
			if (m_strGameDirectory != Settings.GetSetting("DotP2014Directory", m_strGameDirectory))
			{
                Settings.ResetErrorLog();
				RefreshGameData();
				// Also need to reload our current working deck.
				if (m_dkWorking != null)
				{
					string strFilename = m_dkWorking.FileName;
					if ((strFilename != null) && (strFilename.Length > 0) && (File.Exists(strFilename)))
						AttemptToLoadDeck(strFilename);
					else
						mnuiFileNew_Click(null, null);
				}

                // Check to see if there were any errors during load.
                if (Settings.ErrorLog.Opened)
                {
                    // Error log was opened so we have errors, now we need to show them to user.
                    ErrorReportWindow frmErrors = new ErrorReportWindow(Settings.ErrorLog);
                    frmErrors.Show(this);
				}
			}
			frmOptions.Dispose();
		}

		private void DeckBuilder_Shown(object sender, EventArgs e)
		{
			if ((m_strGameDirectory.Length == 0) || (!Directory.Exists(m_strGameDirectory)))
			{
				// This is probably our first run since we don't have a valid directory.
				MessageBox.Show(Settings.UIStrings["MISSING_DOTP_DIRECTORY_MESSAGE"], Settings.UIStrings["MISSING_DOTP_DIRECTORY_TITLE"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Options frmOptions = new Options(this);
				frmOptions.ShowDialog(this);
			}
			RefreshGameData();
			// We just started the program so give us a new deck to work with.
			mnuiFileNew_Click(null, null);

			// Check to see if there were any errors during load.
			if (Settings.ErrorLog.Opened)
			{
				// Error log was opened so we have errors, now we need to show them to user.
				ErrorReportWindow frmErrors = new ErrorReportWindow(Settings.ErrorLog);
				frmErrors.Show(this);
			}
		}

		private void cmdClearFilters_Click(object sender, EventArgs e)
		{
			if (m_gdWads != null)
			{
				if (m_bsCards.DataSource != m_gdWads.Cards)
				{
					Settings.SaveSetting("Filtering", false);
					m_bsCards.DataSource = m_gdWads.Cards;
					dgvCards.RowCount = m_gdWads.Cards.Count;
					sslblCardsInListNum.Text = m_gdWads.Cards.Count.ToString();
					// Restore the previous sort.
					RestoreCardSort();
				}
			}
			dgvCards.Focus();
		}

		private void cmdSetFilter_Click(object sender, EventArgs e)
		{
			if (m_gdWads != null)
			{
				FilterSettings frmFilters = new FilterSettings();
				DialogResult drResult = frmFilters.ShowDialog(this);
				frmFilters.Dispose();
				if (drResult == DialogResult.OK)
				{
					m_fltCardFilter = Settings.GetSerializableSetting("CardFilters", new Filters());
					Settings.SaveSetting("Filtering", true);
					Settings.SaveSetting("AdvancedFiltering", false);
					m_bsCards.DataSource = new SortableBindingList<CardInfo>(m_gdWads.Cards.Where(x => m_fltCardFilter.CheckAgainstFilter(x)));
					dgvCards.RowCount = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count;
					sslblCardsInListNum.Text = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count.ToString();
					// Restore the previous sort.
					RestoreCardSort();
				}
			}
			dgvCards.Focus();
		}

		private void cmdFilterAdvanced_Click(object sender, EventArgs e)
		{
			if (m_gdWads != null)
			{
				CardFilterAdvancedSettings frmFilters = new CardFilterAdvancedSettings();
				DialogResult drResult = frmFilters.ShowDialog(this);
				frmFilters.Dispose();
				if (drResult == DialogResult.OK)
				{
					m_cfsCardFilterAdvanced = Settings.GetSerializableSetting("CardFilterAdvanced", new CardFilterSet());
					Settings.SaveSetting("Filtering", true);
					Settings.SaveSetting("AdvancedFiltering", true);
					m_bsCards.DataSource = new SortableBindingList<CardInfo>(m_gdWads.Cards.Where(x => m_cfsCardFilterAdvanced.IsAllowed(x)));
					dgvCards.RowCount = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count;
					sslblCardsInListNum.Text = ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count.ToString();
					// Restore the previous sort.
					RestoreCardSort();
				}
			}
			dgvCards.Focus();
		}

		private void cmdEditName_Click(object sender, EventArgs e)
		{
			EditString frmEdit = new EditString(m_dkWorking.Name, cmdEditName.Text);
			if (frmEdit.ShowDialog(this) == DialogResult.OK)
			{
				lblDeckName.Text = m_dkWorking.LocalizedName;
				m_dkWorking.Edited = true;
			}
			frmEdit.Dispose();
		}

		private void mnuiFileNew_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) && (m_dkWorking.Edited))
			{
				// They have unsaved work so we should probably ask if they want to save first.
				DialogResult drChoice = MessageBox.Show(Settings.UIStrings["UNSAVED_WORK_MESSAGE"], Settings.UIStrings["UNSAVED_WORK_CAPTION"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (drChoice == DialogResult.Cancel)
					return;
				else if (drChoice == DialogResult.Yes)
				{
					// We only continue if saving completes successfully.
					if (!SaveDeck())
						return;
				}
			}

			if (m_dkWorking != null)
				m_dkWorking.Cards.ListChanged -= m_lcehListHandler;
			m_dkWorking = new Deck(m_gdWads);
			// Get the next available Id if possible.
			IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			int nUid = isScheme.GetNextAvailableId(m_gdWads.UsedIds);
			if (nUid > -1)
				m_dkWorking.Uid = nUid;
			m_lcehListHandler = new ListChangedEventHandler(DeckCards_ListChanged);
			m_dkWorking.Cards.ListChanged += m_lcehListHandler;
			m_dkWorking.Edited = false;
			RefreshDeck();
		}

		private void dgvDeckCards_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvDeckCards.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvDeckCards.Columns.Count))
			{
				CardInfo ciCard = ((DeckCard)dgvDeckCards.Rows[e.RowIndex].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void dgvDeckCards_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if ((e.ColumnIndex >= 0) && (e.ColumnIndex < dgvDeckCards.Columns.Count))
			{
				DataGridViewColumn dgvcColumn = dgvDeckCards.Columns[e.ColumnIndex];
				if (dgvcColumn.SortMode == DataGridViewColumnSortMode.Programmatic)
				{
					if (Control.ModifierKeys == Keys.Shift)
					{
						// Add or Modify an existing sort.
						Tools.AdjustSort(dgvDeckCards, m_bsDeckCards, m_lstDeckCardSort, dgvcColumn.DataPropertyName, false);
					}
					else if (Control.ModifierKeys == Keys.Control)
					{
						Tools.AdjustSort(dgvDeckCards, m_bsDeckCards, m_lstDeckCardSort, dgvcColumn.DataPropertyName, true);
					}
					else
					{
						// Regular single sort (or reverse).
						string strProp = dgvcColumn.DataPropertyName;
						SortOrder soDirection = (dgvcColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);
						m_lstDeckCardSort = new List<ColumnSort>();
						m_lstDeckCardSort.Add(new ColumnSort(dgvcColumn.Name, strProp, soDirection));
						Tools.SortFromList(dgvDeckCards, m_bsDeckCards, m_lstDeckCardSort);
					}
					SaveDeckCardSort();
				}
			}
		}

		private void dgvUnlocksPromo_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (m_dkWorking != null)
			{
				m_dkWorking.Edited = true;
				lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
				CheckPromoButtons();
			}
		}

		private void dgvUnlocksRegular_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (m_dkWorking != null)
			{
				m_dkWorking.Edited = true;
				lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
				CheckRegularButtons();
			}
		}

		private void dgvUnlocksRegular_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksRegular.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksRegular.Columns.Count))
			{
				CardInfo ciCard = ((DeckCard)dgvUnlocksRegular.Rows[e.RowIndex].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void dgvUnlocksPromo_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksPromo.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksPromo.Columns.Count))
			{
				CardInfo ciCard = ((DeckCard)dgvUnlocksPromo.Rows[e.RowIndex].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void mnuiFileSave_Click(object sender, EventArgs e)
		{
			SaveDeck();
		}

		private void mnuiFileSaveAs_Click(object sender, EventArgs e)
		{
			SaveDeckAs();
		}

		private bool SaveDeck()
		{
			if ((m_dkWorking.FileName != null) && (m_dkWorking.FileName.Length > 0))
			{
				// Determine whether we are forcing to always export AI Personalities.
				bool bForceExport = Settings.GetSetting("AlwaysExportPersonalities", false);

				// Since we are saving as "unfinished" we should only get back one file.
				Dictionary<string, XmlDocument> dicDeckFiles = m_dkWorking.Export(null, bForceExport);
				if (dicDeckFiles.Count != 1)
					Settings.ReportError(null, ErrorPriority.High, "We have a different number of files than we should have given the code.");
				// Also since the filename for the deck should be a full path for any created/loaded decks
				//	we should simply be able to save to this filename.
				foreach (KeyValuePair<string, XmlDocument> kvFile in dicDeckFiles)
				{
					kvFile.Value.Save(m_dkWorking.FileName);
					break;
				}
				m_dkWorking.Edited = false;
				return true;
			}
			else
			{
				// We don't already have a filename so we need to essentially do a "Save As ..."
				return SaveDeckAs();
			}
		}
	
		private bool SaveDeckAs()
		{
			SaveFileDialog sfdSave = new SaveFileDialog();
			sfdSave.AddExtension = true;
			sfdSave.AutoUpgradeEnabled = true;
			sfdSave.CheckPathExists = true;
			sfdSave.DefaultExt = ".xml";
			sfdSave.DereferenceLinks = true;
			sfdSave.Filter = Settings.UIStrings["DECK_FILTER"];
			sfdSave.FilterIndex = 1;
			sfdSave.OverwritePrompt = true;
			sfdSave.Title = Settings.UIStrings["DECK_SAVE_TITLE"];
			sfdSave.FileName = "DECK_" + Tools.CodifyName(m_dkWorking.LocalizedName);
			DialogResult drResult = sfdSave.ShowDialog();
			if (drResult == DialogResult.OK)
			{
				// Now that we have a valid filename it should be exactly the same as "saving"
				m_dkWorking.FileName = sfdSave.FileName;
				return SaveDeck();
			}
			return false;
		}

		private void mnuiFileOpen_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) && (m_dkWorking.Edited))
			{
				// They have unsaved work so we should probably ask if they want to save first.
				DialogResult drChoice = MessageBox.Show(Settings.UIStrings["UNSAVED_WORK_MESSAGE"], Settings.UIStrings["UNSAVED_WORK_CAPTION"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (drChoice == DialogResult.Cancel)
					return;
				else if (drChoice == DialogResult.Yes)
				{
					// We only continue if saving completes successfully.
					if (!SaveDeck())
						return;
				}
			}

			OpenFileDialog ofdLoad = new OpenFileDialog();
			ofdLoad.AutoUpgradeEnabled = true;
			ofdLoad.CheckFileExists = true;
			ofdLoad.CheckPathExists = true;		// Yes, I know setting CheckFileExists to true also sets this.
			ofdLoad.DereferenceLinks = true;
			ofdLoad.Filter = Settings.UIStrings["DECK_FILTER"];
			ofdLoad.FilterIndex = 1;
			ofdLoad.Multiselect = false;
			ofdLoad.SupportMultiDottedExtensions = true;
			ofdLoad.Title = Settings.UIStrings["DECK_OPEN_TITLE"];
			DialogResult drResult = ofdLoad.ShowDialog();
			if (drResult == DialogResult.OK)
			{
				AttemptToLoadDeck(ofdLoad.FileName);
			}
		}

		private void AttemptToLoadDeck(string strFilename)
		{
			try
			{
				Deck dkOld = m_dkWorking;
				string strXml = FileTools.ReadFileString(strFilename);
				Deck dkLoaded = new Deck(m_gdWads, strFilename, strXml);
				m_dkWorking = dkLoaded;
				// Now we need to remove the old event bindings and bind the new ones.
				if (dkOld != null)
					dkOld.Cards.ListChanged -= m_lcehListHandler;
				m_lcehListHandler = new ListChangedEventHandler(DeckCards_ListChanged);
				m_dkWorking.Cards.ListChanged += m_lcehListHandler;
				RefreshDeck();
			}
			catch (Exception err)
			{
				Settings.ReportError(err, ErrorPriority.High, "Unable to fully load deck: " + strFilename);
				MessageBox.Show(Settings.UIStrings["UNABLE_TO_LOAD_DECK_MESSAGE"], Settings.UIStrings["UNABLE_TO_LOAD_DECK_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DeckCards_ListChanged(object sender, ListChangedEventArgs e)
		{
			// Since the list changed I should update the current basic land count and mark the deck as edited.
			lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
			lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
			m_dkWorking.Edited = true;
		}

		private void dgvDeckCards_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvDeckCards.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvDeckCards.Columns.Count))
			{
				// Save off the original value for comparison later.
				m_strDeckCellValue = dgvDeckCards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			}
		}

		private void dgvDeckCards_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvDeckCards.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvDeckCards.Columns.Count))
			{
				// Check the current value with the original value.
				if (!m_strDeckCellValue.Equals(dgvDeckCards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), StringComparison.OrdinalIgnoreCase))
					m_dkWorking.Edited = true;
			}
		}

		private void dgvUnlocksRegular_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksRegular.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksRegular.Columns.Count))
			{
				// Save off the original value for comparison later.
				m_strUnlockRegularCellValue = dgvUnlocksRegular.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			}
		}

		private void dgvUnlocksRegular_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksRegular.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksRegular.Columns.Count))
			{
				// Check the current value with the original value.
				if (!m_strUnlockRegularCellValue.Equals(dgvUnlocksRegular.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), StringComparison.OrdinalIgnoreCase))
					m_dkWorking.Edited = true;
			}
		}

		private void dgvUnlocksPromo_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksPromo.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksPromo.Columns.Count))
			{
				// Save off the original value for comparison later.
				m_strUnlockPromoCellValue = dgvUnlocksPromo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			}
		}

		private void dgvUnlocksPromo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.RowIndex > -1) && (e.RowIndex < dgvUnlocksPromo.Rows.Count) &&
				(e.ColumnIndex > -1) && (e.ColumnIndex < dgvUnlocksPromo.Columns.Count))
			{
				// Check the current value with the original value.
				if (!m_strUnlockPromoCellValue.Equals(dgvUnlocksPromo.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), StringComparison.OrdinalIgnoreCase))
					m_dkWorking.Edited = true;
			}
		}

		private void cmdDeckInformation_Click(object sender, EventArgs e)
		{
			DeckInformation diInfo = new DeckInformation(m_dkWorking, m_gdWads);
			diInfo.ShowDialog(this);
			diInfo.Dispose();
		}

		private void mnuiFileCreateFromExisting_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) && (m_dkWorking.Edited))
			{
				// They have unsaved work so we should probably ask if they want to save first.
				DialogResult drChoice = MessageBox.Show(Settings.UIStrings["UNSAVED_WORK_MESSAGE"], Settings.UIStrings["UNSAVED_WORK_CAPTION"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (drChoice == DialogResult.Cancel)
					return;
				else if (drChoice == DialogResult.Yes)
				{
					// We only continue if saving completes successfully.
					if (!SaveDeck())
						return;
				}
			}

			CreateFromExistingDeck frmCreate = new CreateFromExistingDeck(m_gdWads.Decks);
			DialogResult drResult = frmCreate.ShowDialog(this);
			if (drResult == DialogResult.OK)
			{
				if (m_dkWorking != null)
					m_dkWorking.Cards.ListChanged -= m_lcehListHandler;
				m_dkWorking = frmCreate.CreatedDeck;
				TdxWrapper twDeckBox = m_gdWads.LoadImage(m_dkWorking.DeckBoxImageName, LoadImageType.Deck);
				if (twDeckBox != null)
					m_dkWorking.DeckBoxImage = twDeckBox.Image;
				// Changed from clamping to get next available id to be more new-user friendly.
				IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
				int nUid = isScheme.GetNextAvailableId(m_gdWads.UsedIds);
				if (nUid > -1)
					m_dkWorking.Uid = nUid;
				else
				{
					// Getting next id failed due to no more ids in the block.
					// Now we fall back to clamping.
					if (m_dkWorking.Uid < isScheme.MinimumId)
						m_dkWorking.Uid = isScheme.MinimumId;
					else if (m_dkWorking.Uid > isScheme.MaximumId)
						m_dkWorking.Uid = isScheme.MaximumId;
				}
				// Refresh our events and deck view on the window.
				m_lcehListHandler = new ListChangedEventHandler(DeckCards_ListChanged);
				m_dkWorking.Cards.ListChanged += m_lcehListHandler;
				RefreshDeck();
			}
			frmCreate.Dispose();
		}

        private void editExistingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((m_dkWorking != null) && (m_dkWorking.Edited))
            {
                // They have unsaved work so we should probably ask if they want to save first.
                DialogResult drChoice = MessageBox.Show(Settings.UIStrings["UNSAVED_WORK_MESSAGE"], Settings.UIStrings["UNSAVED_WORK_CAPTION"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (drChoice == DialogResult.Cancel)
                    return;
                else if (drChoice == DialogResult.Yes)
                {
                    // We only continue if saving completes successfully.
                    if (!SaveDeck())
                        return;
                }
            }

            // Changed from clamping to get next available id to be more new-user friendly.
            IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());

            CreateFromExistingDeck frmCreate = new CreateFromExistingDeck(m_gdWads.Decks, isScheme.IdBlock);
            DialogResult drResult = frmCreate.ShowDialog(this);
            if (drResult == DialogResult.OK)
            {
                if (m_dkWorking != null)
                    m_dkWorking.Cards.ListChanged -= m_lcehListHandler;
                m_dkWorking = frmCreate.CreatedDeck;
                TdxWrapper twDeckBox = m_gdWads.LoadImage(m_dkWorking.DeckBoxImageName, LoadImageType.Deck);
                if (twDeckBox != null)
                    m_dkWorking.DeckBoxImage = twDeckBox.Image;
                
                int nUid = -1;
                if (m_dkWorking.TryMatchScheme != -1)
                {
                    nUid = m_dkWorking.Uid;
                    if (nUid.ToString().Length >= m_dkWorking.TryMatchScheme.ToString().Length && 
                        nUid.ToString().Substring(0, m_dkWorking.TryMatchScheme.ToString().Length) == m_dkWorking.TryMatchScheme.ToString())
                    {
                        nUid = Int32.Parse(nUid.ToString().Substring(m_dkWorking.TryMatchScheme.ToString().Length));
                    } else
                    {
                        nUid = -1;
                    }
                }
                if (nUid == -1)
                    nUid = isScheme.GetNextAvailableId(m_gdWads.UsedIds);
                if (nUid > -1)
                    m_dkWorking.Uid = nUid;
                else
                {
                    // Getting next id failed due to no more ids in the block.
                    // Now we fall back to clamping.
                    if (m_dkWorking.Uid < isScheme.MinimumId)
                        m_dkWorking.Uid = isScheme.MinimumId;
                    else if (m_dkWorking.Uid > isScheme.MaximumId)
                        m_dkWorking.Uid = isScheme.MaximumId;
                }
                // Refresh our events and deck view on the window.
                m_lcehListHandler = new ListChangedEventHandler(DeckCards_ListChanged);
                m_dkWorking.Cards.ListChanged += m_lcehListHandler;
                RefreshDeck();
            }
            frmCreate.Dispose();
        }

		private void ExportWad(WadBase wad, string strGameDirectory)
		{
			// Create a WadHeaderInfo to fill for our header.
			WadHeaderInfo whiInfo = new WadHeaderInfo();
			whiInfo.ContentAppId = Settings.GetSetting("SteamDefaultUnlockId", 213850);
			whiInfo.ContentFlags = WadHeaderContentFlags.Deck | WadHeaderContentFlags.Glossary | WadHeaderContentFlags.Unlock | WadHeaderContentFlags.Avatar;

			// Create an IdScheme to pass to the deck.
			IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			whiInfo.ContentPackId = (isScheme.UseIdBlock ? isScheme.IdBlock : isScheme.DeckIdChange);

			// Check our ids to make sure none of them will conflict.
			try
			{
				bool bConflict = false;
				bConflict |= m_gdWads.UsedIds.ContainsKey(isScheme.GetDeckId(m_dkWorking.Uid));
				bConflict |= m_gdWads.UsedIds.ContainsKey(isScheme.GetLandPoolId(m_dkWorking.Uid));
				bConflict |= m_gdWads.UsedIds.ContainsKey(isScheme.GetRegularUnlockId(m_dkWorking.Uid));
				bConflict |= m_gdWads.UsedIds.ContainsKey(isScheme.GetPromoUnlockId(m_dkWorking.Uid));
				if (bConflict)
				{
					DialogResult drResult = MessageBox.Show(Settings.UIStrings["UID_CONFLICT_DETECTED_MESSAGE"], Settings.UIStrings["UID_CONFLICT_DETECTED_CAPTION"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (drResult == DialogResult.No)
					{
						// User chose to abort the operation to fix the conflict so we will exit the function here.
						return;
					}
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Low, "IdScheme could not generate valid ids, settings are probably wrong.");
				MessageBox.Show(Settings.UIStrings["SCHEME_ERROR_MESSAGE"], Settings.UIStrings["SCHEME_ERROR_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Error);
				// Exit without completing.
				return;
			}

			this.Cursor = Cursors.WaitCursor;

			// Now before we can export we need to make sure we have a good land pool that contains only
			//	basic lands for the colours used for the deck.  This is new in DotP 2014.
			m_dkWorking.CreateLandPool(m_gdWads);

			// Determine whether we are forcing to always export AI Personalities.
			bool bForceExport = Settings.GetSetting("AlwaysExportPersonalities", false);

			// Collect our files.
			m_dkWorking.ContentPack = whiInfo.ContentPackId;
			Dictionary<string, XmlDocument> dicFiles = m_dkWorking.Export(isScheme, bForceExport);

			// We cut off the D14_ from the export filename here since we want a different prefix.
			wad.Name = "Data_Decks_" + m_dkWorking.ExportFileName.Substring(4);

			wad.AddHeader();

			// This should cover most of our XML files except for the String Table.
			foreach (KeyValuePair<string, XmlDocument> kvFile in dicFiles)
			{
				if ((kvFile.Key.Equals(m_dkWorking.ExportFileName)) ||
					(kvFile.Key.EndsWith("_LAND_POOL", StringComparison.OrdinalIgnoreCase)))
					wad.AddDeck(kvFile.Key + ".XML", kvFile.Value);
				else if ((kvFile.Key.EndsWith("_UNLOCK", StringComparison.OrdinalIgnoreCase)) ||
					(kvFile.Key.EndsWith("_PROMO", StringComparison.OrdinalIgnoreCase)))
					wad.AddUnlock(kvFile.Key + ".XML", kvFile.Value);
				else if (kvFile.Key.StartsWith("D14_PERSONALITY_", StringComparison.OrdinalIgnoreCase))
					wad.AddAiPersonality(kvFile.Key, kvFile.Value);
			}

			// Create our string table (we should have at most 2 entries).
			//	1 - Deck Name
			//	2 - Personality Name (if any)
			KeyValuePair<string, Dictionary<string, string>> kvpDeckName = new KeyValuePair<string, Dictionary<string, string>>(m_dkWorking.NameTag, m_dkWorking.Name);
			KeyValuePair<string, Dictionary<string, string>> kvpDeckDescription = new KeyValuePair<string,Dictionary<string,string>>();
			KeyValuePair<string, Dictionary<string, string>> kvpPersonalityName = new KeyValuePair<string,Dictionary<string,string>>();
			if ((m_dkWorking.DescriptionTag.Length > 0) && (m_dkWorking.Description != null))
				kvpDeckDescription = new KeyValuePair<string, Dictionary<string, string>>(m_dkWorking.DescriptionTag, m_dkWorking.Description);
			if (!m_dkWorking.Personality.BuiltIn || bForceExport)
				kvpPersonalityName = new KeyValuePair<string, Dictionary<string, string>>(m_dkWorking.Personality.NameId, m_dkWorking.Personality.Name);
			XmlDocument xdStringTable = XmlTools.CreateStringTable(kvpDeckName, kvpDeckDescription, kvpPersonalityName);
			wad.AddTextPermanent(m_dkWorking.ExportFileName + "_TEXT.XML", xdStringTable);

			// Now we need to add our deck box image.
			if (m_dkWorking.DeckBoxImage != null)
			{
				TdxWrapper twDeckBox = GetTdxImage(m_dkWorking.DeckBoxImage);
				wad.AddDeckImage(m_dkWorking.DeckBoxImageName + ".TDX", twDeckBox);
				twDeckBox.Dispose();
			}

			// Now for personality images (if any)
			if ((!m_dkWorking.Personality.BuiltIn) || (bForceExport))
			{
				if (m_dkWorking.Personality.LargeAvatarImageName.Length > 0)
				{
					TdxWrapper twImage = GetTdxImage(m_dkWorking.Personality.LargeAvatarImage, false);
					wad.AddAiPersonalityImage(m_dkWorking.Personality.LargeAvatarImageName + ".TDX", twImage);
					twImage.Dispose();
				}
				if (m_dkWorking.Personality.SmallAvatarImageName.Length > 0)
				{
					TdxWrapper twImage = GetTdxImage(m_dkWorking.Personality.SmallAvatarImage);
					wad.AddAiPersonalityImage(m_dkWorking.Personality.SmallAvatarImageName + ".TDX", twImage);
					twImage.Dispose();
				}
				if (m_dkWorking.Personality.SmallAvatarLockedImageName.Length > 0)
				{
					TdxWrapper twImage = GetTdxImage(m_dkWorking.Personality.SmallAvatarLockedImage);
					wad.AddAiPersonalityImage(m_dkWorking.Personality.SmallAvatarLockedImageName + ".TDX", twImage);
					twImage.Dispose();
				}
				if (m_dkWorking.Personality.LobbyImageName.Length > 0)
				{
					TdxWrapper twImage = GetTdxImage(m_dkWorking.Personality.LobbyImage);
					wad.AddAiPersonalityImage(m_dkWorking.Personality.LobbyImageName + ".TDX", twImage);
					twImage.Dispose();
				}
			}

			try
			{
				// Create a ContentPack Enabler Wad.
				WadWrapper wwContentPack = new WadWrapper("Data_DLC_" + whiInfo.ContentPackId.ToString() + "_Content_Pack_Enabler");
				wwContentPack.AddHeader(whiInfo);
				wwContentPack.WriteWad(strGameDirectory);

				// Now we should have collected all the files so now we finalize.
				wad.WriteWad(strGameDirectory);

				// Give the user a simple readme.
				GenerateWadReadme(wad, m_dkWorking, strGameDirectory);

				// Now that generation is complete we should add the ids to the used id list so that they won't be used accidently if making more than one deck in a session.
				//	We don't really care if it says the same filename is using all 4 ids, because this one deck (and its related files) did really use those ids.
				try
				{
					m_gdWads.UsedIds.Add(isScheme.GetDeckId(m_dkWorking.Uid), m_dkWorking.ExportFileName);
					m_gdWads.UsedIds.Add(isScheme.GetLandPoolId(m_dkWorking.Uid), m_dkWorking.ExportFileName);
					m_gdWads.UsedIds.Add(isScheme.GetRegularUnlockId(m_dkWorking.Uid), m_dkWorking.ExportFileName);
					m_gdWads.UsedIds.Add(isScheme.GetPromoUnlockId(m_dkWorking.Uid), m_dkWorking.ExportFileName);
				}
				catch (Exception e)
				{
					// Don't really care if there is an error in adding them to the used ids because it is possible the user decided to write out a file using a previously used id.
					//	The catch however is still necessary so that the program doesn't crash.
					Settings.ReportError(e, ErrorPriority.Zero, "Deck exported using an already used id: " + m_dkWorking.ExportFileName);
				}

				this.Cursor = Cursors.Default;

				// Report to user that export completed successfully.
				MessageBox.Show(Settings.UIStrings["EXPORT_COMPLETE_MESSAGE"], Settings.UIStrings["EXPORT_COMPLETE_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Information);

				// We can mark as not edited so that we don't thoroughly annoy the user.
				m_dkWorking.Edited = false;
			}
			catch (Exception e)
			{
				// This is probably due to a permissions problem (read-only directory).
				this.Cursor = Cursors.Default;
				Settings.ReportError(e, ErrorPriority.Medium, "Unable to Export Wad: " + wad.Name);
				MessageBox.Show(Settings.UIStrings["EXPORT_UNSUCCESSFUL"], this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// The purpose of this function is to generate a simple readme for the generated wad that details things like what other wads this one is dependent on.
		private void GenerateWadReadme(WadBase wad, Deck dkDeck, string strGameDirectory)
		{
			if (!strGameDirectory.EndsWith("\\"))
				strGameDirectory += "\\";
			FileStream fsOutput = File.Create(strGameDirectory + wad.Name + "_README.TXT");
			StreamWriter swOutput = new StreamWriter(fsOutput, Encoding.UTF8);
			swOutput.WriteLine(wad.Name);

			//bool bHasRegularUnlocks = false;

			// Compile a set of wads this one is dependent on.
			SortedSet<string> setDependent = new SortedSet<string>();
			foreach (DeckCard dcCard in dkDeck.Cards)
				setDependent.Add(dcCard.Card.PresentInWad);
			if ((dkDeck.RegularUnlocks != null) && (dkDeck.RegularUnlocks.Cards != null) && (dkDeck.RegularUnlocks.Cards.Count > 0))
			{
				//bHasRegularUnlocks = true;
				foreach (DeckCard dcCard in dkDeck.RegularUnlocks.Cards)
					setDependent.Add(dcCard.Card.PresentInWad);
			}
			if ((dkDeck.PromoUnlocks != null) && (dkDeck.PromoUnlocks.Cards != null) && (dkDeck.PromoUnlocks.Cards.Count > 0))
			{
				foreach (DeckCard dcCard in dkDeck.PromoUnlocks.Cards)
					setDependent.Add(dcCard.Card.PresentInWad);
			}

			// We should generate for each of the supported languages.
			foreach (KeyValuePair<string, LanguageEntry> kvpLang in Settings.Languages)
			{
				LanguageStrings lsStrings = new LanguageStrings(kvpLang.Key);

				// Export language and preamble.
				swOutput.WriteLine();
				swOutput.WriteLine("---===" + kvpLang.Value.Text + "===---");
				swOutput.WriteLine(lsStrings["README_PREAMBLE"]);
				swOutput.WriteLine();

				// Removed because DotP 2014 no longer uses APPID_LINKING.TXT for AppId linking.
				/*if (bHasRegularUnlocks)
				{
					// Inform user that this deck has regular unlocks and what they will need to do to properly unlock them.
					swOutput.WriteLine(lsStrings["README_APPID_LINKING_NOTICE"]);
					swOutput.WriteLine("[" + dkDeck.ExportFileName + "]");
					string[] astrParts = dkDeck.ExportFileName.Split('_');
					swOutput.WriteLine(astrParts[1]);
					swOutput.WriteLine("97330");
					swOutput.WriteLine("97330");
					swOutput.WriteLine();
				}//*/

				// Export list of cards in deck.
				swOutput.WriteLine(lsStrings["README_CARDS_IN_DECK"]);
				foreach (DeckCard dcCard in dkDeck.Cards)
					swOutput.WriteLine("\t" + dcCard.Quantity.ToString() + "x - " + dcCard.Card.GetLocalizedName(kvpLang.Key) + " (" + dcCard.Card.Filename + ")");
				swOutput.WriteLine();

				// Export list of Regular Unlocks (if any)
				if ((dkDeck.RegularUnlocks != null) && (dkDeck.RegularUnlocks.Cards != null) && (dkDeck.RegularUnlocks.Cards.Count > 0))
				{
					swOutput.WriteLine(lsStrings["README_CARDS_IN_REGULAR_UNLOCKS"]);
					foreach (DeckCard dcCard in dkDeck.RegularUnlocks.Cards)
						swOutput.WriteLine("\t" + dcCard.Quantity.ToString() + "x - " + dcCard.Card.GetLocalizedName(kvpLang.Key) + " (" + dcCard.Card.Filename + ")");
					swOutput.WriteLine();
				}

				// Export list of Promo Unlocks (if any)
				if ((dkDeck.PromoUnlocks != null) && (dkDeck.PromoUnlocks.Cards != null) && (dkDeck.PromoUnlocks.Cards.Count > 0))
				{
					swOutput.WriteLine(lsStrings["README_CARDS_IN_PROMO_UNLOCKS"]);
					foreach (DeckCard dcCard in dkDeck.PromoUnlocks.Cards)
						swOutput.WriteLine("\t" + dcCard.Quantity.ToString() + "x - " + dcCard.Card.GetLocalizedName(kvpLang.Key) + " (" + dcCard.Card.Filename + ")");
					swOutput.WriteLine();
				}

				// Export our list of wads we depend on.
				swOutput.WriteLine(lsStrings["README_DEPENDS_ON_WADS"]);
				foreach (string strWad in setDependent)
					swOutput.WriteLine("\t" + strWad);
			}

			swOutput.Close();
		}

		private TdxWrapper GetTdxImage(Bitmap bmpImage, bool bHasTransparency = true)
		{
			TdxWrapper twImage = null;

			if (bmpImage != null)
			{
				// The TdxWrapper handles resizing images to MoF sizes if compressing so
				//	no need to worry about that.  Deck Box and AI Personality images need
				//	transparency so we'll compress to DXT5.
				twImage = new TdxWrapper();
				if (bHasTransparency)
					twImage.LoadImage(bmpImage, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT5, Settings.GetSetting("IncludeMipMaps", true));
				else
					twImage.LoadImage(bmpImage, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT1, Settings.GetSetting("IncludeMipMaps", true));
			}

			return twImage;
		}

		private TdxWrapper GetTdxPreview(CardInfo ciCard, string strLangCode)
		{
			TdxWrapper twPreview = null;

			if (ciCard != null)
			{
				Bitmap bmpPreview = Tools.AddCardBorder(ciCard.GetPreviewImage(strLangCode));
				// The TdxWrapper handles resizing images to MoF sizes if compressing so
				//	no need to worry about that.  Since our Card Border will add
				//	transparency to have the nice rounded look we need to use DXT5.
				twPreview = new TdxWrapper();
				twPreview.LoadImage(bmpPreview, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT5, Settings.GetSetting("IncludeMipMaps", true));
			}

			return twPreview;
		}

		private void mnuiFileExportToDirectory_Click(object sender, EventArgs e)
		{
			ExportWad(new WadDirectory(string.Empty), m_gdWads.GameDir);
		}

		private void mnuiFileExportToWad_Click(object sender, EventArgs e)
		{
			ExportWad(new WadWrapper(string.Empty), m_gdWads.GameDir);
		}

		private void CheckRegularButtons()
		{
			if ((m_dkWorking != null) && (m_dkWorking.RegularUnlocks != null) && (m_dkWorking.RegularUnlocks.Cards != null) && (m_dkWorking.RegularUnlocks.Cards.Count > 1))
			{
				if ((dgvUnlocksRegular.SelectedRows != null) && (dgvUnlocksRegular.SelectedRows.Count > 0) && (dgvUnlocksRegular.SelectedRows[0].Index > 0))
					cmdRegularMoveUp.Enabled = true;
				else
					cmdRegularMoveUp.Enabled = false;
				if ((dgvUnlocksRegular.SelectedRows != null) && (dgvUnlocksRegular.SelectedRows.Count > 0) && (dgvUnlocksRegular.SelectedRows[0].Index < (dgvUnlocksRegular.RowCount - 1)))
					cmdRegularMoveDown.Enabled = true;
				else
					cmdRegularMoveDown.Enabled = false;
			}
			else
			{
				cmdRegularMoveUp.Enabled = false;
				cmdRegularMoveDown.Enabled = false;
			}
		}

		private void CheckPromoButtons()
		{
			if ((m_dkWorking != null) && (m_dkWorking.PromoUnlocks != null) && (m_dkWorking.PromoUnlocks.Cards != null) && (m_dkWorking.PromoUnlocks.Cards.Count > 1))
			{
				if ((dgvUnlocksPromo.SelectedRows != null) && (dgvUnlocksPromo.SelectedRows.Count > 0) && (dgvUnlocksPromo.SelectedRows[0].Index > 0))
					cmdPromoMoveUp.Enabled = true;
				else
					cmdPromoMoveUp.Enabled = false;
				if ((dgvUnlocksPromo.SelectedRows != null) && (dgvUnlocksPromo.SelectedRows.Count > 0) && (dgvUnlocksPromo.SelectedRows[0].Index < (dgvUnlocksPromo.RowCount - 1)))
					cmdPromoMoveDown.Enabled = true;
				else
					cmdPromoMoveDown.Enabled = false;
			}
			else
			{
				cmdPromoMoveUp.Enabled = false;
				cmdPromoMoveDown.Enabled = false;
			}

			// This is added to centralize promo status checking for all controls.
			if (m_dkWorking.PromoUnlocks.Cards.Count >= 10)
			{
				// Disable anything that could put a card into the promo unlocks.
				cmnuiMoveToPromoUnlocks.Enabled = false;
				if (rbDoubleClickPromoUnlock.Checked)
					rbDoubleClickRegularUnlock.Checked = true;
				rbDoubleClickPromoUnlock.Enabled = false;
			}
			else
			{
				// Enable controls that can put card(s) into promo unlocks.
				cmnuiMoveToPromoUnlocks.Enabled = true;
				rbDoubleClickPromoUnlock.Enabled = true;
			}
		}

		private void SwapListIndexes<T>(SortableBindingList<T> lstItems, int nIndex1, int nIndex2)
		{
			T oItem = lstItems[nIndex1];
			lstItems[nIndex1] = lstItems[nIndex2];
			lstItems[nIndex2] = oItem;
		}

		private void cmdRegularMoveUp_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) &&
				(m_dkWorking.RegularUnlocks != null) &&
				(m_dkWorking.RegularUnlocks.Cards != null) &&
				(m_dkWorking.RegularUnlocks.Cards.Count > 1) &&
				(dgvUnlocksRegular.SelectedRows != null) &&
				(dgvUnlocksRegular.SelectedRows.Count > 0) &&
				(dgvUnlocksRegular.SelectedRows[0].Index > 0))
			{
				DeckCard dcCard = dgvUnlocksRegular.SelectedRows[0].DataBoundItem as DeckCard;
				if (dcCard != null)
				{
					int nIndex = m_dkWorking.RegularUnlocks.Cards.IndexOf(dcCard);
					SwapListIndexes(m_dkWorking.RegularUnlocks.Cards, nIndex, nIndex - 1);
					dgvUnlocksRegular.Rows[nIndex].Selected = false;
                    dgvUnlocksRegular.Rows[nIndex - 1].Selected = true;

				}
			}
		}

		private void cmdRegularMoveDown_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) &&
				(m_dkWorking.RegularUnlocks != null) &&
				(m_dkWorking.RegularUnlocks.Cards != null) &&
				(m_dkWorking.RegularUnlocks.Cards.Count > 1) &&
				(dgvUnlocksRegular.SelectedRows != null) &&
				(dgvUnlocksRegular.SelectedRows.Count > 0) &&
				(dgvUnlocksRegular.SelectedRows[0].Index < (dgvUnlocksRegular.RowCount - 1)))
			{
				DeckCard dcCard = dgvUnlocksRegular.SelectedRows[0].DataBoundItem as DeckCard;
				if (dcCard != null)
				{
					int nIndex = m_dkWorking.RegularUnlocks.Cards.IndexOf(dcCard);
					SwapListIndexes(m_dkWorking.RegularUnlocks.Cards, nIndex, nIndex + 1);
					dgvUnlocksRegular.Rows[nIndex].Selected = false;
                    dgvUnlocksRegular.Rows[nIndex + 1].Selected = true;
                }
			}
		}

		private void cmdPromoMoveUp_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) &&
				(m_dkWorking.PromoUnlocks != null) &&
				(m_dkWorking.PromoUnlocks.Cards != null) &&
				(m_dkWorking.PromoUnlocks.Cards.Count > 1) &&
				(dgvUnlocksPromo.SelectedRows != null) &&
				(dgvUnlocksPromo.SelectedRows.Count > 0) &&
				(dgvUnlocksPromo.SelectedRows[0].Index > 0))
			{
				DeckCard dcCard = dgvUnlocksPromo.SelectedRows[0].DataBoundItem as DeckCard;
				if (dcCard != null)
				{
					int nIndex = m_dkWorking.PromoUnlocks.Cards.IndexOf(dcCard);
					SwapListIndexes(m_dkWorking.PromoUnlocks.Cards, nIndex, nIndex - 1);
					dgvUnlocksPromo.Rows[nIndex].Selected = false;
                    dgvUnlocksPromo.Rows[nIndex - 1].Selected = true;
				}
			}
		}

		private void cmdPromoMoveDown_Click(object sender, EventArgs e)
		{
			if ((m_dkWorking != null) &&
				(m_dkWorking.PromoUnlocks != null) &&
				(m_dkWorking.PromoUnlocks.Cards != null) &&
				(m_dkWorking.PromoUnlocks.Cards.Count > 1) &&
				(dgvUnlocksPromo.SelectedRows != null) &&
				(dgvUnlocksPromo.SelectedRows.Count > 0) &&
				(dgvUnlocksPromo.SelectedRows[0].Index < (dgvUnlocksPromo.RowCount - 1)))
			{
				DeckCard dcCard = dgvUnlocksPromo.SelectedRows[0].DataBoundItem as DeckCard;
				if (dcCard != null)
				{
					int nIndex = m_dkWorking.PromoUnlocks.Cards.IndexOf(dcCard);
					SwapListIndexes(m_dkWorking.PromoUnlocks.Cards, nIndex, nIndex + 1);
					dgvUnlocksPromo.Rows[nIndex].Selected = false;
                    dgvUnlocksPromo.Rows[nIndex + 1].Selected = true;
				}
			}
		}

		private void dgvUnlocksRegular_SelectionChanged(object sender, EventArgs e)
		{
			// Make sure our buttons are properly enabled (or disabled)
			CheckRegularButtons();

			// When the selection changes we should update which image is being viewed.
			if ((dgvUnlocksRegular.SelectedRows != null) && (dgvUnlocksRegular.SelectedRows.Count > 0))
			{
				CardInfo ciCard = ((DeckCard)dgvUnlocksRegular.SelectedRows[0].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void dgvUnlocksPromo_SelectionChanged(object sender, EventArgs e)
		{
			// Make sure our buttons are properly enabled (or disabled)
			CheckPromoButtons();

			// When the selection changes we should update which image is being viewed.
			if ((dgvUnlocksPromo.SelectedRows != null) && (dgvUnlocksPromo.SelectedRows.Count > 0))
			{
				CardInfo ciCard = ((DeckCard)dgvUnlocksPromo.SelectedRows[0].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void BuildColumnsMenu(ToolStripMenuItem mnuiParent, DataGridViewColumnCollection dgvccColl)
		{
			if (m_ehColumnMenuItemClick == null)
				m_ehColumnMenuItemClick = new EventHandler(ColumnMenu_Click);

			if (mnuiParent.DropDownItems.Count > 0)
				mnuiParent.DropDownItems.Clear();

			foreach (DataGridViewColumn dgvcColumn in dgvccColl)
			{
				ToolStripMenuItem mnuiColumn = new ToolStripMenuItem(dgvcColumn.HeaderText, null, m_ehColumnMenuItemClick);
				if (dgvcColumn.Visible)
					mnuiColumn.Checked = true;
				else
					mnuiColumn.Checked = false;
				mnuiColumn.Tag = dgvcColumn;
				mnuiParent.DropDownItems.Add(mnuiColumn);
			}
		}

		private void ColumnMenu_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mnuiColumn = sender as ToolStripMenuItem;
			if (mnuiColumn != null)
			{
				DataGridViewColumn dgvcColumn = mnuiColumn.Tag as DataGridViewColumn;
				if (dgvcColumn != null)
					dgvcColumn.Visible = !mnuiColumn.Checked;
			}
		}

		private void cmnuiViewCard_Click(object sender, EventArgs e)
		{
			// Open up the view card window.
			ViewCard frmView = new ViewCard(m_ciContextCard);
			frmView.Show(this);
		}

		private void dgvCards_MouseClick(object sender, MouseEventArgs e)
		{
			// This is to bring up the context menu when the user right-clicks.
			if (e.Button == MouseButtons.Right)
			{
				BuildColumnsMenu(cmnuiColumns, dgvCards.Columns);
				// check to see if we have a selected card.
				DataGridView.HitTestInfo htiInfo = dgvCards.HitTest(e.X, e.Y);
				if ((htiInfo.RowIndex > -1) && (htiInfo.RowIndex < dgvCards.RowCount))
				{
					//m_ciContextCard = (CardInfo)dgvCards.Rows[htiInfo.RowIndex].DataBoundItem;
					m_ciContextCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[htiInfo.RowIndex];
					cmnuiViewCard.Enabled = true;
					cmnuiDecksUsedIn.Enabled = true;
					cmnuiExportPreviews.Enabled = true;
					cmnuiExportCard.Enabled = true;
					TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
					cmnuiExportImageCrop.Enabled = (twImage != null);
				}
				else
				{
					cmnuiViewCard.Enabled = false;
					cmnuiDecksUsedIn.Enabled = false;
					cmnuiExportPreviews.Enabled = false;
					cmnuiExportCard.Enabled = false;
					cmnuiExportImageCrop.Enabled = false;
				}

				// Always disable remove card for the master card list.
				cmnuiRemoveCard.Enabled = false;

				// Always disable move to menu.
				SetMenuEnabled(cmnuiMoveTo, false);

				// Now show the context menu
				cmnuContext.Show(Cursor.Position);
			}
		}

		private void dgvDeckCards_MouseClick(object sender, MouseEventArgs e)
		{
			// This is to bring up the context menu when the user right-clicks.
			if (e.Button == MouseButtons.Right)
			{
				BuildColumnsMenu(cmnuiColumns, dgvDeckCards.Columns);
				// check to see if we have a selected card.
				DataGridView.HitTestInfo htiInfo = dgvDeckCards.HitTest(e.X, e.Y);
				if ((htiInfo.RowIndex > -1) && (htiInfo.RowIndex < dgvDeckCards.RowCount))
				{
					m_dcContextCard = (DeckCard)dgvDeckCards.Rows[htiInfo.RowIndex].DataBoundItem;
					m_ciContextCard = m_dcContextCard.Card;
					cmnuiViewCard.Enabled = true;
					cmnuiDecksUsedIn.Enabled = true;
					cmnuiRemoveCard.Enabled = true;
					cmnuiExportPreviews.Enabled = true;
					cmnuiExportCard.Enabled = true;
					TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
					cmnuiExportImageCrop.Enabled = (twImage != null);
					SetMenuEnabled(cmnuiMoveTo, (m_dcContextCard != null));
					cmnuiMoveToMainDeck.Enabled = false;
					cmnuiMoveToPromoUnlocks.Enabled = (m_dkWorking.PromoUnlockCardCount < 10);
				}
				else
				{
					cmnuiViewCard.Enabled = false;
					cmnuiDecksUsedIn.Enabled = false;
					cmnuiRemoveCard.Enabled = false;
					cmnuiExportPreviews.Enabled = false;
					cmnuiExportCard.Enabled = false;
					cmnuiExportImageCrop.Enabled = false;
					SetMenuEnabled(cmnuiMoveTo, false);
				}

				// Information for remove card.
				m_nContextRow = htiInfo.RowIndex;
				m_eLocation = DeckLocation.MainDeck;

				// Now show the context menu
				cmnuContext.Show(Cursor.Position);
			}
		}

		private void dgvUnlocksRegular_MouseClick(object sender, MouseEventArgs e)
		{
			// This is to bring up the context menu when the user right-clicks.
			if (e.Button == MouseButtons.Right)
			{
				BuildColumnsMenu(cmnuiColumns, dgvUnlocksRegular.Columns);
				// check to see if we have a selected card.
				DataGridView.HitTestInfo htiInfo = dgvUnlocksRegular.HitTest(e.X, e.Y);
				if ((htiInfo.RowIndex > -1) && (htiInfo.RowIndex < dgvUnlocksRegular.RowCount))
				{
					m_dcContextCard = (DeckCard)dgvUnlocksRegular.Rows[htiInfo.RowIndex].DataBoundItem;
					m_ciContextCard = m_dcContextCard.Card;
					cmnuiViewCard.Enabled = true;
					cmnuiDecksUsedIn.Enabled = true;
					cmnuiRemoveCard.Enabled = true;
					cmnuiExportPreviews.Enabled = true;
					cmnuiExportCard.Enabled = true;
					TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
					cmnuiExportImageCrop.Enabled = (twImage != null);
					SetMenuEnabled(cmnuiMoveTo, (m_dcContextCard != null));
					cmnuiMoveToRegularUnlocks.Enabled = false;
					cmnuiMoveToPromoUnlocks.Enabled = (m_dkWorking.PromoUnlockCardCount < 10);
				}
				else
				{
					cmnuiViewCard.Enabled = false;
					cmnuiDecksUsedIn.Enabled = false;
					cmnuiRemoveCard.Enabled = false;
					cmnuiExportPreviews.Enabled = false;
					cmnuiExportCard.Enabled = false;
					cmnuiExportImageCrop.Enabled = false;
					SetMenuEnabled(cmnuiMoveTo, false);
				}

				// Information for remove card.
				m_nContextRow = htiInfo.RowIndex;
				m_eLocation = DeckLocation.RegularUnlocks;

				// Now show the context menu
				cmnuContext.Show(Cursor.Position);
			}
		}

		private void dgvUnlocksPromo_MouseClick(object sender, MouseEventArgs e)
		{
			// This is to bring up the context menu when the user right-clicks.
			if (e.Button == MouseButtons.Right)
			{
				BuildColumnsMenu(cmnuiColumns, dgvUnlocksPromo.Columns);
				// check to see if we have a selected card.
				DataGridView.HitTestInfo htiInfo = dgvUnlocksPromo.HitTest(e.X, e.Y);
				if ((htiInfo.RowIndex > -1) && (htiInfo.RowIndex < dgvUnlocksPromo.RowCount))
				{
					m_dcContextCard = (DeckCard)dgvUnlocksPromo.Rows[htiInfo.RowIndex].DataBoundItem;
					m_ciContextCard = m_dcContextCard.Card;
					cmnuiViewCard.Enabled = true;
					cmnuiDecksUsedIn.Enabled = true;
					cmnuiRemoveCard.Enabled = true;
					cmnuiExportPreviews.Enabled = true;
					cmnuiExportCard.Enabled = true;
					TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
					cmnuiExportImageCrop.Enabled = (twImage != null);
					SetMenuEnabled(cmnuiMoveTo, (m_dcContextCard != null));
					cmnuiMoveToPromoUnlocks.Enabled = false;
				}
				else
				{
					cmnuiViewCard.Enabled = false;
					cmnuiDecksUsedIn.Enabled = false;
					cmnuiRemoveCard.Enabled = false;
					cmnuiExportPreviews.Enabled = false;
					cmnuiExportCard.Enabled = false;
					cmnuiExportImageCrop.Enabled = false;
					SetMenuEnabled(cmnuiMoveTo, false);
				}

				// Information for remove card.
				m_nContextRow = htiInfo.RowIndex;
				m_eLocation = DeckLocation.PromoUnlocks;

				// Now show the context menu
				cmnuContext.Show(Cursor.Position);
			}
		}

		private void cmnuiDecksUsedIn_Click(object sender, EventArgs e)
		{
			CardInfo ciCard = m_ciContextCard;
			string strDecksUsingCard = string.Empty;

			string strFoundDecks = GetDeckListCardUsedIn(ciCard);
			if (strFoundDecks.Length > 0)
			{
				// Well this card is used in decks directly.
				strDecksUsingCard = Settings.UIStrings["DECKS_USING_CARD"] + "\r\n" + strFoundDecks;
			}
			else
			{
				// This card isn't used in any decks (at least not directly).
				strDecksUsingCard = Settings.UIStrings["NO_DECKS_USING_CARD"];
			}

			if (ciCard.Token)
			{
				// This card is a token, so lets look for all the cards that can create this token and see what decks they are used in.
				List<string> lstCardNames = new List<string>();
				foreach (CardInfo ciTest in m_gdWads.Cards.OrderBy(x => x.Filename))
				{
					if ((ciTest.RegisteredTokens != null) &&
						(ciTest.RegisteredTokens.Count > 0) &&
						(!lstCardNames.Contains(ciTest.Filename)))
					{
						foreach (string strToken in ciTest.RegisteredTokens)
						{
							if (strToken.Equals(ciCard.Filename, StringComparison.OrdinalIgnoreCase))
							{
								// This card can create the card we're researching so now we need to see
								//	where this card is used.
								string strDecks = GetDeckListCardUsedIn(ciTest);
								if (strDecks.Length > 0)
								{
									// Card is used.
									strDecksUsingCard += "\r\n\r\n" + Settings.UIStrings["TOKEN_CREATED_BY_AND_USED_IN"].Replace("{0}", ciTest.LocalizedName + " (" + ciTest.Filename + ")") + "\r\n" + strDecks;
								}
								else
								{
									// Card not used.
									strDecksUsingCard += "\r\n\r\n" + Settings.UIStrings["TOKEN_CREATED_BY_AND_NOT_USED"].Replace("{0}", ciTest.LocalizedName + " (" + ciTest.Filename + ")");
								}
								lstCardNames.Add(ciTest.Filename);
							}
						}
					}
				}
			}

			// Display an appropriate message on which decks (if any) were using the card.
			MessageBox.Show(strDecksUsingCard, ciCard.Filename, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private string GetDeckListCardUsedIn(CardInfo ciCard)
		{
			string strReturn = string.Empty;

			// Now look through all the decks (and all landpools and unlocks) to see where it's used.
			//	m_gdWads.Decks includes the land pools so we don't necessarily have to check those separately.
			foreach (Deck deck in m_gdWads.Decks)
			{
				// If we found the card in this deck then we need to put it in the list.
				if (CheckCardUsedInDeck(ciCard, deck))
				{
					if (strReturn.Length > 0)
						strReturn += "\r\n";
					strReturn += "\t\"" + deck.LocalizedName + "\" - " + deck.FileName;
				}
			}

			return strReturn;
		}

		private bool CheckCardUsedInDeck(CardInfo ciCard, Deck dkDeck)
		{
			bool bFound = false;

			if ((dkDeck.Cards != null) && (dkDeck.Cards.Count > 0))
			{
				// This deck has cards so check them
				foreach (DeckCard dcCard in dkDeck.Cards)
				{
					if (dcCard.Card.Filename.Equals(ciCard.Filename, StringComparison.OrdinalIgnoreCase))
					{
						bFound = true;
						break;
					}
				}
				if (!bFound)
				{
					// If we haven't found it in this deck yet then check our unlocks if any.
					if (dkDeck.RegularUnlockCardCount > 0)
					{
						foreach (DeckCard dcCard in dkDeck.RegularUnlocks.Cards)
						{
							if (dcCard.Card.Filename.Equals(ciCard.Filename, StringComparison.OrdinalIgnoreCase))
							{
								bFound = true;
								break;
							}
						}
					}
					if (dkDeck.PromoUnlockCardCount > 0)
					{
						foreach (DeckCard dcCard in dkDeck.PromoUnlocks.Cards)
						{
							if (dcCard.Card.Filename.Equals(ciCard.Filename, StringComparison.OrdinalIgnoreCase))
							{
								bFound = true;
								break;
							}
						}
					}
				}
			}

			return bFound;
		}

		private void cmnuiRemoveCard_Click(object sender, EventArgs e)
		{
			if (m_nContextRow > -1)
			{
				switch (m_eLocation)
				{
					case DeckLocation.MainDeck:
						dgvDeckCards.Rows.RemoveAt(m_nContextRow);
						break;
					case DeckLocation.RegularUnlocks:
						dgvUnlocksRegular.Rows.RemoveAt(m_nContextRow);
						break;
					case DeckLocation.PromoUnlocks:
						dgvUnlocksPromo.Rows.RemoveAt(m_nContextRow);
						break;
				}
			}
		}

		private void dgvDeckCards_SelectionChanged(object sender, EventArgs e)
		{
			// When the selection changes we should update which image is being viewed.
			if ((dgvDeckCards.SelectedRows != null) && (dgvDeckCards.SelectedRows.Count > 0))
			{
				CardInfo ciCard = ((DeckCard)dgvDeckCards.SelectedRows[0].DataBoundItem).Card;
				ShowCardInfo(ciCard);
			}
		}

		private void dgvCards_SelectionChanged(object sender, EventArgs e)
		{
			// When the selection changes we should update which image is being viewed.
			if ((dgvCards.SelectedRows != null) && (dgvCards.SelectedRows.Count > 0) && (dgvCards.SelectedRows[0].Index < ((SortableBindingList<CardInfo>)m_bsCards.DataSource).Count))
			{
				//CardInfo ciCard = ((CardInfo)dgvCards.SelectedRows[0].DataBoundItem);
				CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[dgvCards.SelectedRows[0].Index];
				ShowCardInfo(ciCard);
			}
		}

		private void picFrame_MouseUp(object sender, MouseEventArgs e)
		{
			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
			{
				if (picFrame.Image != null)
					cmnuiExport.Enabled = true;
				else
					cmnuiExport.Enabled = false;
				cmnuPictures.Show(Cursor.Position);
			}
		}

		private void cmnuiExportPng_Click(object sender, EventArgs e)
		{
			if (picFrame.Image != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png");
				if (strFilename.Length > 0)
					picFrame.Image.Save(strFilename, ImageFormat.Png);
			}
		}

		private void cmnuiExportTdx_Click(object sender, EventArgs e)
		{
			if (picFrame.Image != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX");
				if (strFilename.Length > 0)
				{
					// The card preview we show here does not have any transparency so we have no problems saving as DXT1
					TdxWrapper twImage = new TdxWrapper();
					twImage.LoadImage(picFrame.Image, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT1, Settings.GetSetting("IncludeMipMaps", true));
					twImage.Save(strFilename);
					twImage.Dispose();
				}
			}
		}

		private void cmnuiExportPreviewsPng_Click(object sender, EventArgs e)
		{
			CardInfo ciCard = m_ciContextCard;
			if (ciCard != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png", "EN_" + ciCard.Filename + ".png");
				if (strFilename.Length > 0)
				{
					// Get our chosen directory and base filename.
					string strDir = Path.GetDirectoryName(strFilename);
					if (!strDir.EndsWith("\\"))
						strDir += "\\";
					string strFileBase = Path.GetFileName(strFilename);
					if (strFileBase.StartsWith("EN_", StringComparison.OrdinalIgnoreCase))
						strFileBase = strFileBase.Substring(3);
					// Save for each language entry.
					foreach (LanguageEntry lang in Settings.Languages.Values)
					{
						if (lang.MasqueradeAsLangCode == null)
						{
							Bitmap bmpPreview = Tools.AddCardBorder(ciCard.GetPreviewImage(lang.LanguageCode));
							if (bmpPreview != null)
								bmpPreview.Save(strDir + lang.ShortCode + "_" + strFileBase, ImageFormat.Png);
						}
					}
				}
			}
		}

		private void cmnuiExportPreviewsTdx_Click(object sender, EventArgs e)
		{
			CardInfo ciCard = m_ciContextCard;
			if (ciCard != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX", "EN_" + ciCard.Filename + ".TDX");
				if (strFilename.Length > 0)
				{
					// Get our chosen directory and base filename.
					string strDir = Path.GetDirectoryName(strFilename);
					if (!strDir.EndsWith("\\"))
						strDir += "\\";
					string strFileBase = Path.GetFileName(strFilename);
					if (strFileBase.StartsWith("EN_", StringComparison.OrdinalIgnoreCase))
						strFileBase = strFileBase.Substring(3);
					// Save for each language entry.
					foreach (LanguageEntry lang in Settings.Languages.Values)
					{
						if (lang.MasqueradeAsLangCode == null)
						{
							TdxWrapper twPreview = GetTdxPreview(ciCard, lang.LanguageCode);
							if (twPreview != null)
							{
								twPreview.Save(strDir + lang.ShortCode + "_" + strFileBase);
								twPreview.Dispose();
							}
						}
					}
				}
			}
		}

		private void mnuiToolsSetupCustomData_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			// Basically we just create a placeholder Wad Directory in the game directory.
			WadDirectory wdPlaceholder = new WadDirectory(DECK_BUILDER_CUSTOM_DATA_WAD_NAME);
			wdPlaceholder.WritePlaceholderWad(m_gdWads.GameDir);

			// Disable ourselves since the directory now exists.
			mnuiToolsSetupCustomData.Enabled = false;
			// Enable creating a core wad from our custom data.
			mnuiToolsCreateCoreWad.Enabled = true;

			this.Cursor = Cursors.Default;

			// Report back to the user that the folder was successfully setup.
			MessageBox.Show(Settings.UIStrings["CUSTOM_DATA_FOLDER_CREATED"], this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuiToolsCreateCoreWad_Click(object sender, EventArgs e)
		{
			IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			string strInitFilename = string.Empty;
			if (isScheme.UseIdBlock)
				strInitFilename = "Data_DLC_" + isScheme.IdBlock.ToString() + "_Core.wad";
			else
				strInitFilename = "Data_DLC_Deck_Builder_Core.wad";

			string strFilename = Tools.SaveDialog(Settings.UIStrings["SAVE_CORE_WAD_AS"], Settings.UIStrings["WAD_FILTER"], "wad", strInitFilename);
			if (strFilename.Length > 0)
			{
				this.Cursor = Cursors.WaitCursor;

				// Retrieve our custom data wad.
				WadBase wbCustom = null;
				foreach (WadBase wad in m_gdWads.LoadedWads)
				{
					if (wad.Name.Equals(DECK_BUILDER_CUSTOM_DATA_WAD_NAME, StringComparison.OrdinalIgnoreCase))
					{
						wbCustom = wad;
						break;
					}
				}
				// Make sure we found one.
				if (wbCustom != null)
				{
					string strDir = Path.GetDirectoryName(strFilename);
					WadWrapper wwCore = new WadWrapper(strFilename);
					wwCore.AddCustomHeader(wbCustom.Header);
					// Copy all our data from the custom data directory to the new core wad.
					//	Note: This will automatically remove any empty directories from the new core so no need to worry about that before hand.
					wbCustom.WriteToWad(wwCore);
					// Finalize the new wad by writing it out.
					wwCore.WriteWad(strDir);

					this.Cursor = Cursors.Default;

					MessageBox.Show(Settings.UIStrings["CREATE_CORE_WAD_SUCCESS"], this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					this.Cursor = Cursors.Default;

					// I'm not going to create a message box for this since it should never happen and I don't really want to create unnecessary strings.
					Settings.ReportError(null, ErrorPriority.High, "This should never have happened: Tried to create a core wad without the custom data wad being present.");
				}
			}
		}

		private void dgvCards_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if ((dgvCards.SelectedRows != null) && (dgvCards.SelectedRows.Count > 0))
				{
					CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[dgvCards.SelectedRows[0].Index];
					m_dkWorking.Edited = true;
					if (rbDoubleClickDeck.Checked)
					{
						DeckCard dcCard = m_dkWorking.AddCard(ciCard);
						int nIndex = m_dkWorking.Cards.IndexOf(dcCard);
						lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
						lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
						dgvDeckCards.CurrentCell = dgvDeckCards.Rows[nIndex].Cells[0];
						dgvDeckCards.Refresh();
					}
					else if (rbDoubleClickRegularUnlock.Checked)
					{
						m_dkWorking.RegularUnlocks.Cards.Add(new DeckCard(ciCard));
						lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
						dgvUnlocksRegular.CurrentCell = dgvUnlocksRegular.Rows[dgvUnlocksRegular.RowCount - 1].Cells[0];
						CheckRegularButtons();
					}
					else
					{
						// Only thing left is the Promo Unlocks.
						m_dkWorking.PromoUnlocks.Cards.Add(new DeckCard(ciCard));
						lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
						dgvUnlocksPromo.CurrentCell = dgvUnlocksPromo.Rows[dgvUnlocksPromo.RowCount - 1].Cells[0];
						CheckPromoButtons();
					}
				}
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Back)
			{
				if ((dgvCards.SelectedRows != null) && (dgvCards.SelectedRows.Count > 0))
				{
					CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[dgvCards.SelectedRows[0].Index];
					if (rbDoubleClickDeck.Checked)
					{
						DeckCard dcCard = m_dkWorking.RemoveCard(ciCard);
						if (dcCard != null)
						{
							m_dkWorking.Edited = true;
							if (m_dkWorking.Cards.Contains(dcCard))
							{
								// Still in list so lets move to it.
								int nIndex = m_dkWorking.Cards.IndexOf(dcCard);
								dgvDeckCards.CurrentCell = dgvDeckCards.Rows[nIndex].Cells[0];
							}
						}
					}
					else if (rbDoubleClickRegularUnlock.Checked)
					{
						if (m_dkWorking.RegularUnlocks.RemoveCard(ciCard))
							m_dkWorking.Edited = true;
					}
					else
					{
						if (m_dkWorking.PromoUnlocks.RemoveCard(ciCard))
							m_dkWorking.Edited = true;
					}
				}
			}
			else if (e.KeyCode == Keys.Delete)
			{
				if ((dgvCards.SelectedRows != null) && (dgvCards.SelectedRows.Count > 0))
				{
					CardInfo ciCard = ((SortableBindingList<CardInfo>)m_bsCards.DataSource)[dgvCards.SelectedRows[0].Index];
					if (rbDoubleClickDeck.Checked)
					{
						DeckCard dcCard = m_dkWorking.RemoveCard(ciCard, -1);
						if (dcCard != null)
							m_dkWorking.Edited = true;
					}
					else if (rbDoubleClickRegularUnlock.Checked)
					{
						if (m_dkWorking.RegularUnlocks.RemoveCard(ciCard, -1))
							m_dkWorking.Edited = true;
					}
					else
					{
						if (m_dkWorking.PromoUnlocks.RemoveCard(ciCard, -1))
							m_dkWorking.Edited = true;
					}
				}
			}
		}

		private void cmnuiExportCard_Click(object sender, EventArgs e)
		{
			if (m_ciContextCard != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["CARD_SAVE_TITLE"], Settings.UIStrings["CARD_FILTER"], ".XML", m_ciContextCard.Filename + ".XML");
				if (strFilename.Length > 0)
				{
					// Save to file (in UTF-8 with BOM)
					using (FileStream fs = new FileStream(strFilename, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(Settings.IncludeBOM));
						sw.Write(m_ciContextCard.CardXML);
						sw.Close();
					}
				}
			}
		}

		private void cmnuiExportImageCropPng_Click(object sender, EventArgs e)
		{
			if (m_ciContextCard != null)
			{
				TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
				if ((twImage != null) && (twImage.Image != null))
				{
					string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png", m_ciContextCard.ImageFilename + ".png");
					if (strFilename.Length > 0)
						twImage.Image.Save(strFilename, ImageFormat.Png);
				}
			}
		}

		private void cmnuiExportImageCropTdx_Click(object sender, EventArgs e)
		{
			if (m_ciContextCard != null)
			{
				TdxWrapper twImage = m_gdWads.LoadImage(m_ciContextCard.ImageFilename, LoadImageType.Card);
				if (twImage != null)
				{
					string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX", m_ciContextCard.ImageFilename + ".TDX");
					if (strFilename.Length > 0)
						twImage.Save(strFilename);
				}
			}
		}

		private void SetDoubleBufferingForDataGrids()
		{
			// Set all the lists at once to make things a bit more efficient.
			PropertyInfo piDoubleBuffered = dgvCards.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			piDoubleBuffered.SetValue(dgvCards, true, null);
			piDoubleBuffered.SetValue(dgvDeckCards, true, null);
			piDoubleBuffered.SetValue(dgvUnlocksPromo, true, null);
			piDoubleBuffered.SetValue(dgvUnlocksRegular, true, null);
		}

		private void DeckCards_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			// Here I need to check to see that valid values were entered.
			DataGridView dgvList = sender as DataGridView;
			DataGridViewColumn dgvcColumn = dgvList.Columns[e.ColumnIndex];
			if (dgvcColumn.DataPropertyName.Equals("Quantity", StringComparison.OrdinalIgnoreCase) ||
				dgvcColumn.DataPropertyName.Equals("Bias", StringComparison.OrdinalIgnoreCase))
			{
				// Must be a positive integer (I don't allow 0).
				int nTest = 0;
				if (int.TryParse(e.FormattedValue.ToString(), out nTest))
				{
					if (nTest <= 0)
					{
						// Not greater than 0 so not valid.
						e.Cancel = true;
					}
				}
				else
				{
					// Not a valid integer so prevent the push.
					e.Cancel = true;
				}
			}
		}

		private void dgvCards_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			DataGridViewColumn dgvcColumn = dgvCards.Columns[e.ColumnIndex];
			SortableBindingList<CardInfo> lstCards = ((SortableBindingList<CardInfo>)m_bsCards.DataSource);
			PropertyInfo piProp = null;

			if (e.RowIndex < lstCards.Count)
			{
				// First see if this is a Custom Tag column
				if (dgvcColumn.DataPropertyName.StartsWith("CustomTag:", StringComparison.OrdinalIgnoreCase))
				{
					string[] astrTagName = new string[1];
					astrTagName[0] = dgvcColumn.DataPropertyName.Substring(10);
					if (m_miCustomTagValue == null)
					{
						// We need to initialize the MethodInfo before we can use it.
						try
						{
							m_miCustomTagValue = lstCards[e.RowIndex].GetType().GetMethod("GetFirstCustomTagValue");
						}
						catch (Exception ex)
						{
							Settings.ReportError(ex, ErrorPriority.Zero, "Problem reflecting custom tag method.");
							e.Value = null;
							return;
						}
					}
					if (m_miCustomTagValue != null)
					{
						e.Value = m_miCustomTagValue.Invoke(lstCards[e.RowIndex], astrTagName);
					}
				}
				else
				{
					// Look up this column and get the property without a reflection look up.
					if (m_dicColumnMap.ContainsKey(dgvcColumn))
						piProp = m_dicColumnMap[dgvcColumn];
					else
					{
						try
						{
							piProp = lstCards[e.RowIndex].GetType().GetProperty(dgvcColumn.DataPropertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
							// Make sure we don't need to do any more expensive reflection look ups.
							m_dicColumnMap.Add(dgvcColumn, piProp);
						}
						catch (Exception ex)
						{
							Settings.ReportError(ex, ErrorPriority.Zero, "Problem reflecting property: " + dgvcColumn.DataPropertyName);
							// This next bit prevents lots and lots of repeated failed reflection look ups.
							m_dicColumnMap.Add(dgvcColumn, null);
						}
					}
					if (piProp != null)
						e.Value = piProp.GetValue(lstCards[e.RowIndex], null);
				}
			}
		}

		private void dgvCards_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			// The Cards view should always be read only.
			e.Cancel = true;
		}

		private void mnuiToolsCombineSpecData_Click(object sender, EventArgs e)
		{
			// This will create a custom WAD file that only contains a header and merged SPEC data from all loaded WADs.
			//	Currently this will only handle SubTypeOrderingData and SubTypes as those are the only ones we can edit
			//	properly without the source code of the game.

			// Create Header
			WadHeaderInfo whiInfo = new WadHeaderInfo();
			whiInfo.OrderPriority = 99;	// By default I will use Order 99 to ensure overriding of files.

			// Start creating our WAD
			WadWrapper wwMerged = new WadWrapper("Data_DLC_Combined_Spec_Data");
			wwMerged.AddHeader(whiInfo);

			// Now to create and add our merged sub-types data.
			//	The hard part of reading in and merging all the data is already done by GameDirectory.
			MemoryStream msMergedLol = new MemoryStream();
			StreamWriter swMergedLol = new StreamWriter(msMergedLol);
			using (AboutBox frmAbout = new AboutBox())
			{
				swMergedLol.WriteLine("-- This file was autogenerated by " + frmAbout.AssemblyProduct + " on " + DateTime.Now);
			}

			foreach (CardSubTypeArchetypes eType in m_gdWads.SubTypes.Keys)
			{
				MemoryStream msSubTypes = new MemoryStream();
				StreamWriter swSubTypes = new StreamWriter(msSubTypes);
				List<string> lstSubTypes = m_gdWads.SubTypes[eType];
				string strSubTypeConstantPrefix = "";
				int nSubTypeIndexStart = 0;
				switch (eType)
				{
					case CardSubTypeArchetypes.Artifact:
						strSubTypeConstantPrefix = "ARTIFACT_";
						nSubTypeIndexStart = 0;
						break;
					case CardSubTypeArchetypes.Creature:
						strSubTypeConstantPrefix = "CREATURE_";
						nSubTypeIndexStart = 1000;
						break;
					case CardSubTypeArchetypes.Enchantment:
						strSubTypeConstantPrefix = "ENCHANTMENT_";
						nSubTypeIndexStart = 2000;
						break;
					case CardSubTypeArchetypes.Land:
						strSubTypeConstantPrefix = "LAND_";
						nSubTypeIndexStart = 4000;
						break;
					case CardSubTypeArchetypes.Plane:
						strSubTypeConstantPrefix = "PLANE_";
						nSubTypeIndexStart = 9000;
						break;
					case CardSubTypeArchetypes.Planeswalker:
						strSubTypeConstantPrefix = "PLANESWALKER_";
						nSubTypeIndexStart = 5000;
						break;
					case CardSubTypeArchetypes.Scheme:
						strSubTypeConstantPrefix = "SCHEME_";
						nSubTypeIndexStart = 8000;
						break;
					case CardSubTypeArchetypes.Spell:
						strSubTypeConstantPrefix = "SPELL_";
						nSubTypeIndexStart = 3000;
						break;
				}
				int nSubTypeIndex = nSubTypeIndexStart;

				swMergedLol.WriteLine();
				swMergedLol.WriteLine("_" + strSubTypeConstantPrefix + "TYPE_FIRST = " + nSubTypeIndex.ToString());

				if (lstSubTypes.Count > 0)
				{
					foreach (string strSubType in lstSubTypes)
					{
						swSubTypes.WriteLine(strSubType);
						swMergedLol.WriteLine(strSubTypeConstantPrefix + "TYPE_" + strSubType + " = " + nSubTypeIndex.ToString());
						nSubTypeIndex++;
					}
					nSubTypeIndex--;
				}

				swMergedLol.WriteLine("_" + strSubTypeConstantPrefix + "TYPE_LAST = " + nSubTypeIndex.ToString());
				swMergedLol.WriteLine("_" + strSubTypeConstantPrefix + "COUNT = " + lstSubTypes.Count.ToString());

				if (lstSubTypes.Count > 0)
				{
					// Now to add the SPEC file to the WAD.
					swSubTypes.Flush();
					wwMerged.AddFile(WadBase.SPECS_LOCATION + eType.ToString().ToUpper() + "_TYPES.TXT", msSubTypes);
				}
			}

			// Now to add the LOL file to the WAD.
			swMergedLol.Flush();
			wwMerged.AddFile(WadBase.FUNCTIONS_LOCATION + "DECK_BUILDER_MERGED_SPEC_CONSTANTS.LOL", msMergedLol);

			// Now loop through the SubTypeOrdering Data and output it.
			foreach (string strLangCode in m_gdWads.SubTypeOrdering.Keys)
			{
				Dictionary<string, int> dicSubTypeOrdering = m_gdWads.SubTypeOrdering[strLangCode];
				if (dicSubTypeOrdering.Keys.Count > 0)
				{
					MemoryStream msSubTypeOrdering = new MemoryStream();
					StreamWriter swSubTypeOrdering = new StreamWriter(msSubTypeOrdering);

					// Write our file.
					foreach (string strKey in dicSubTypeOrdering.Keys)
						swSubTypeOrdering.WriteLine(strKey + "," + dicSubTypeOrdering[strKey].ToString());

					// Add it to the WAD.
					swSubTypeOrdering.Flush();
					wwMerged.AddFile(WadBase.SPECS_LOCATION + "SUBTYPEORDERINGDATA_" + strLangCode + ".TXT", msSubTypeOrdering);
				}
			}

			// Write out the finalized WAD file.
			try
			{
				wwMerged.WriteWad(m_gdWads.GameDir);
				MessageBox.Show(Settings.UIStrings["EXPORT_COMPLETE_CAPTION"], this.Text);
			}
			catch (Exception ex)
			{
				// This is probably due to a permissions problem (read-only directory).
				Settings.ReportError(ex, ErrorPriority.Medium, "Unable to Export Wad: Data_DLC_Combined_Spec_Data");
				MessageBox.Show(Settings.UIStrings["EXPORT_UNSUCCESSFUL"], Settings.UIStrings["EXPORT_UNSUCCESSFUL"], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void MoveCardTo(object sender, EventArgs e)
		{
			if (m_dcContextCard != null)
			{
				DeckCard dcCardToAdd = null;

				if (m_dcContextCard.Quantity > 1)
					dcCardToAdd = new DeckCard(m_dcContextCard.Card, 1, m_dcContextCard.Bias, m_dcContextCard.Promo);
				else
					dcCardToAdd = m_dcContextCard;

				// Add to new location.
				if (sender == cmnuiMoveToMainDeck)
				{
					DeckCard dcAdded = m_dkWorking.AddCard(dcCardToAdd.Card, dcCardToAdd.Bias, dcCardToAdd.Promo);
					int nIndex = m_dkWorking.Cards.IndexOf(dcAdded);
					lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
					lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
					dgvDeckCards.CurrentCell = dgvDeckCards.Rows[nIndex].Cells[0];
					dgvDeckCards.Refresh();
				}
				else if (sender == cmnuiMoveToRegularUnlocks)
				{
					m_dkWorking.RegularUnlocks.Cards.Add(dcCardToAdd);
					int nIndex = m_dkWorking.RegularUnlocks.Cards.IndexOf(dcCardToAdd);
					lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
					dgvUnlocksRegular.CurrentCell = dgvUnlocksRegular.Rows[nIndex].Cells[0];
					CheckRegularButtons();
				}
				else if (sender == cmnuiMoveToPromoUnlocks)
				{
					m_dkWorking.PromoUnlocks.Cards.Add(dcCardToAdd);
					int nIndex = m_dkWorking.PromoUnlocks.Cards.IndexOf(dcCardToAdd);
					lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
					dgvUnlocksPromo.CurrentCell = dgvUnlocksPromo.Rows[nIndex].Cells[0];
					CheckPromoButtons();
				}

				// Remove from previous location.
				switch (m_eLocation)
				{
					case DeckLocation.MainDeck:
						if (m_dcContextCard.Quantity > 1)
							m_dcContextCard.Quantity--;
						else
							m_dkWorking.Cards.Remove(m_dcContextCard);
						lblBasicLandCount.Text = m_dkWorking.BasicLandAmount.ToString();
						lblTotalCardCount.Text = m_dkWorking.CardCount.ToString();
						dgvDeckCards.Refresh();
						break;
					case DeckLocation.RegularUnlocks:
						m_dkWorking.RegularUnlocks.Cards.Remove(m_dcContextCard);
						lblRegUnlockCardCount.Text = m_dkWorking.RegularUnlockCardCount.ToString();
						CheckRegularButtons();
						break;
					case DeckLocation.PromoUnlocks:
						m_dkWorking.PromoUnlocks.Cards.Remove(m_dcContextCard);
						lblPromoUnlockCardCount.Text = m_dkWorking.PromoUnlockCardCount.ToString();
						CheckPromoButtons();
						break;
				}
			}
		}

		private void SetMenuEnabled(ToolStripMenuItem mnuiMenu, bool bEnabled)
		{
			if (mnuiMenu.DropDownItems.Count > 0)
			{
				foreach (ToolStripItem mnuiItem in mnuiMenu.DropDownItems)
					if (mnuiItem.GetType() == typeof(ToolStripMenuItem))
						SetMenuEnabled((ToolStripMenuItem)mnuiItem, bEnabled);
			}
			mnuiMenu.Enabled = bEnabled;
		}

		private void mnuiToolsGenerateCPE_Click(object sender, EventArgs e)
		{
			IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			IdBlockInput frmInput = new IdBlockInput(isScheme.IdBlock);
			DialogResult drResult = frmInput.ShowDialog(this);
			if (drResult == DialogResult.OK)
			{
				try
				{
					Tools.CreateContentPackEnabler(m_gdWads.GameDir, frmInput.IdBlock);
					MessageBox.Show(Settings.UIStrings["EXPORT_COMPLETE_CAPTION"], this.Text);
				}
				catch (Exception ex)
				{
					// This is probably due to a permissions problem (read-only directory).
					Settings.ReportError(ex, ErrorPriority.Medium, "Unable to Export Wad: Data_DLC_" + frmInput.IdBlock.ToString() + "Content_Pack_Enabler.wad");
					MessageBox.Show(Settings.UIStrings["EXPORT_UNSUCCESSFUL"], Settings.UIStrings["EXPORT_UNSUCCESSFUL"], MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
