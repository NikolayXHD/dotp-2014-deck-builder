using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class CreateFromExistingDeck : Form
	{
		private Deck m_dkNew;
		private EventHandler m_ehColumnMenuItemClick;

		// For multiple column sorting.
		private BindingSource m_bsDecks;
		private List<ColumnSort> m_lstDeckSort;

		public CreateFromExistingDeck(SortableBindingList<Deck> lstDecks)
		{
			InitializeComponent();

			cmdApply.Enabled = false;

			Rectangle rcPosition = Settings.GetSetting("ExistingDeckPosition", new Rectangle(-1, -1, -1, -1));
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

			LoadLocalizedStrings();

			// Set up columns.
			SetupDeckList();

			// Set Data.
			m_bsDecks = new BindingSource();
			m_bsDecks.DataSource = new SortableBindingList<Deck>(lstDecks.Where(x => !x.IsLandPool));
			dgvDecks.DataSource = m_bsDecks;
			// Restore previous sort.
			RestoreDeckSort();
		}

		private void RestoreDeckSort()
		{
			// Load the list
			List<ColumnSort> lstSort = Settings.GetSerializableSetting("DeckViewSort", new List<ColumnSort>());
			if (lstSort.Count <= 0)
				lstSort.Add(new ColumnSort("Uid", "Uid", SortOrder.Ascending));

			// Actually sort.
			m_lstDeckSort = lstSort;
			Tools.SortFromList(dgvDecks, m_bsDecks, lstSort);
		}

		private void SaveDeckSort()
		{
			Settings.SaveSerializableSetting("DeckViewSort", m_lstDeckSort);
		}

		public Deck CreatedDeck
		{
			get { return m_dkNew; }
		}

		private void LoadLocalizedStrings()
		{
			this.Text = Settings.UIStrings[(string)this.Tag];
			dgvDecks.Refresh();
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];
		}

		private void SetupDeckList()
		{
			if (dgvDecks.Columns.Count == 0)
			{
				dgvDecks.AutoGenerateColumns = false;
				dgvDecks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				dgvDecks.AllowUserToAddRows = false;
				dgvDecks.AllowUserToDeleteRows = false;

				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "Uid", "Uid", "COLUMN_TEXT_UID", DataGridViewColumnSortMode.Programmatic, 60);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "Filename", "FileName", "COLUMN_TEXT_FILE_NAME", DataGridViewColumnSortMode.Programmatic, 150);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "Name", "LocalizedName", "COLUMN_TEXT_NAME", DataGridViewColumnSortMode.Programmatic, 150);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "Colour", "ColourText", "COLUMN_TEXT_COLOUR", DataGridViewColumnSortMode.Programmatic, 150);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "CardCount", "CardCount", "COLUMN_TEXT_CARD_COUNT", DataGridViewColumnSortMode.Programmatic, 60);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "BasicLands", "BasicLandAmount", "COLUMN_TEXT_BASIC_LANDS", DataGridViewColumnSortMode.Programmatic, 60);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "RegularUnlocks", "RegularUnlockCardCount", "COLUMN_TEXT_REGULAR_UNLOCKS", DataGridViewColumnSortMode.Programmatic, 60);
				Tools.AddViewColumn(dgvDecks, new DataGridViewTextBoxColumn(), "PromoUnlocks", "PromoUnlockCardCount", "COLUMN_TEXT_PROMO_UNLOCKS", DataGridViewColumnSortMode.Programmatic, 60);

				Settings.GetSetting("DeckViewColumns", dgvDecks.Columns);

				// All columns here need to be read only.
				foreach (DataGridViewColumn dgvc in dgvDecks.Columns)
					dgvc.ReadOnly = true;
			}
		}

		private void dgvDecks_SelectionChanged(object sender, EventArgs e)
		{
			if ((dgvDecks.SelectedRows != null) && (dgvDecks.SelectedRows.Count > 0))
				cmdApply.Enabled = true;
			else
				cmdApply.Enabled = false;
		}

		private void CreateFromExistingDeck_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("DeckViewColumns", dgvDecks.Columns);
			Settings.SaveSetting("ExistingDeckPosition", new Rectangle(this.Location, this.Size));
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			// First I need to get the deck from the selected row.
			if ((dgvDecks.SelectedRows != null) && (dgvDecks.SelectedRows.Count > 0))
			{
				// We really should only have a single deck here so...
				Deck dkSelected = dgvDecks.SelectedRows[0].DataBoundItem as Deck;
				if (dkSelected != null)
					m_dkNew = new Deck(dkSelected);
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void dgvDecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if ((e.ColumnIndex >= 0) && (e.ColumnIndex < dgvDecks.Columns.Count) &&
				(e.RowIndex >= 0) && (e.RowIndex < dgvDecks.Rows.Count))
			{
				cmdApply_Click(null, null);
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

		private void dgvDecks_MouseClick(object sender, MouseEventArgs e)
		{
			// This is to bring up the context menu when the user right-clicks.
			if (e.Button == MouseButtons.Right)
			{
				BuildColumnsMenu(cmnuiColumns, dgvDecks.Columns);
				// Now show the context menu
				cmnuContext.Show(Cursor.Position);
			}
		}

		private void dgvDecks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if ((e.ColumnIndex >= 0) && (e.ColumnIndex < dgvDecks.Columns.Count))
			{
				DataGridViewColumn dgvcColumn = dgvDecks.Columns[e.ColumnIndex];
				if (dgvcColumn.SortMode == DataGridViewColumnSortMode.Programmatic)
				{
					if (Control.ModifierKeys == Keys.Shift)
					{
						// Add or Modify an existing sort.
						Tools.AdjustSort(dgvDecks, m_bsDecks, m_lstDeckSort, dgvcColumn.DataPropertyName, false);
					}
					else if (Control.ModifierKeys == Keys.Control)
					{
						Tools.AdjustSort(dgvDecks, m_bsDecks, m_lstDeckSort, dgvcColumn.DataPropertyName, true);
					}
					else
					{
						// Regular single sort (or reverse).
						string strProp = dgvcColumn.DataPropertyName;
						SortOrder soDirection = (dgvcColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);
						m_lstDeckSort = new List<ColumnSort>();
						m_lstDeckSort.Add(new ColumnSort(dgvcColumn.Name, strProp, soDirection));
						Tools.SortFromList(dgvDecks, m_bsDecks, m_lstDeckSort);
					}
					SaveDeckSort();
				}
			}
		}
	}
}
