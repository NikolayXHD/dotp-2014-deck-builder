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
			dgvDecks.DataSource = new SortableBindingList<Deck>(lstDecks.Where(x => !x.IsLandPool));
			// Restore previous sort.
			string strSortColumnName = Settings.GetSetting("DeckViewSortColumn", "Uid");
			ListSortDirection eSortDir = (ListSortDirection)Enum.Parse(typeof(ListSortDirection), Settings.GetSetting("DeckViewSortDirection", "Ascending"));
			dgvDecks.Sort(dgvDecks.Columns[strSortColumnName], eSortDir);
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

				DataGridViewColumn dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "Uid";
				dgvcColumn.Name = "Uid";
				dgvcColumn.Tag = "COLUMN_TEXT_UID";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 60;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "FileName";
				dgvcColumn.Name = "Filename";
				dgvcColumn.Tag = "COLUMN_TEXT_FILE_NAME";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 150;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "LocalizedName";
				dgvcColumn.Name = "Name";
				dgvcColumn.Tag = "COLUMN_TEXT_NAME";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 150;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "ColourText";
				dgvcColumn.Name = "Colour";
				dgvcColumn.Tag = "COLUMN_TEXT_COLOUR";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 150;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "CardCount";
				dgvcColumn.Name = "CardCount";
				dgvcColumn.Tag = "COLUMN_TEXT_CARD_COUNT";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 60;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "BasicLandAmount";
				dgvcColumn.Name = "BasicLands";
				dgvcColumn.Tag = "COLUMN_TEXT_BASIC_LANDS";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 60;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "RegularUnlockCardCount";
				dgvcColumn.Name = "RegularUnlocks";
				dgvcColumn.Tag = "COLUMN_TEXT_REGULAR_UNLOCKS";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 60;
				dgvDecks.Columns.Add(dgvcColumn);

				dgvcColumn = new DataGridViewTextBoxColumn();
				dgvcColumn.DataPropertyName = "PromoUnlockCardCount";
				dgvcColumn.Name = "PromoUnlocks";
				dgvcColumn.Tag = "COLUMN_TEXT_PROMO_UNLOCKS";
				dgvcColumn.HeaderText = Settings.UIStrings[(string)dgvcColumn.Tag];
				dgvcColumn.SortMode = DataGridViewColumnSortMode.Automatic;
				dgvcColumn.Width = 60;
				dgvDecks.Columns.Add(dgvcColumn);

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

		private void dgvDecks_Sorted(object sender, EventArgs e)
		{
			Settings.SaveSetting("DeckViewSortColumn", dgvDecks.SortedColumn.Name);
			Settings.SaveSetting("DeckViewSortDirection", dgvDecks.SortOrder.ToString());
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
	}
}
