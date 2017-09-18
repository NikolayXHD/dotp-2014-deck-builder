using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class Options : Form
	{
		private DeckBuilder m_frmMain = null;
		private IdScheme m_isScheme;
		private bool m_bLoading;

		public Options(DeckBuilder frmMain)
		{
			InitializeComponent();

			m_bLoading = true;

			m_frmMain = frmMain;

			// Id Scheme options need to be done before loading of strings to make sure we load the right strings into the scheme type drop down.
			m_isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			cboSchemeType.DisplayMember = "Value";
			cboSchemeType.ValueMember = "Key";
			numMinDigits.Value = m_isScheme.MinimumIdDigits;
			numMinId.Value = m_isScheme.MinimumId;
			numMaxId.Value = m_isScheme.MaximumId;
			chkIdBlock.Checked = m_isScheme.UseIdBlock;
			numIdBlock.Value = m_isScheme.IdBlock;
			numDeckChange.Value = m_isScheme.DeckIdChange;
			numLandPoolChange.Value = m_isScheme.LandPoolIdChange;
			numRegularUnlockChange.Value = m_isScheme.RegularUnlockIdChange;
			numPromoUnlockChange.Value = m_isScheme.PromoUnlockIdChange;

			LoadLocalizedStrings();

			Rectangle rcPosition = Settings.GetSetting("OptionsPosition", new Rectangle(-1, -1, -1, -1));
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
				// This is a non-resizable form.
				//this.Size = rcPosition.Size;
			}

			// I have form sizing issues if I try to set this in the designer when coupled with a manual start position rather than from code.
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			txtGameDir.Text = Settings.GetSetting("DotP2014Directory", string.Empty);

			// Fill the languages list.
			foreach (KeyValuePair<string, string> lang in Settings.Languages)
				cboLanguage.Items.Add(lang);
			cboLanguage.DisplayMember = "Value";
			cboLanguage.ValueMember = "Key";
			if (Settings.Languages.ContainsKey(Settings.LanguageCode))
				cboLanguage.SelectedItem = new KeyValuePair<string, string>(Settings.LanguageCode, Settings.Languages[Settings.LanguageCode]);

			chkBasicScreenChecks.Checked = Settings.GetSetting("PerformBasicScreenChecks", true);
			chkPreShuffleOnExport.Checked = Settings.GetSetting("PreShuffleOnExport", false);

			if (txtGameDir.Text.Length == 0)
				cmdCancel.Enabled = false;
			else
				cmdCancel.Enabled = true;

			m_bLoading = false;

			// Initialize the Id Scheme Example
			numChosenId_ValueChanged(null, null);
			chkIdBlock_CheckedChanged(null, null);
		}

		private void LoadLocalizedStrings(string strLangCode = "")
		{
			LanguageStrings lsStrings = Settings.UIStrings;
			if (strLangCode.Length > 0)
			{
				try
				{
					LanguageStrings lsLoad = new LanguageStrings(strLangCode);
					lsStrings = lsLoad;
				}
				catch (Exception e)
				{
					Settings.ReportError(e, ErrorPriority.Medium, "Unable to load Language Strings for language code: " + strLangCode);
				}
			}

			// I could simply iterate over all of the controls in this form and replace
			//	their text with localized strings using the tags as a key, but then I
			//	might forget to resize certain other controls when their label counterpart
			//	changes.
			this.Text = lsStrings[(string)this.Tag];
			gbGeneral.Text = lsStrings[(string)gbGeneral.Tag];
			lblGameDir.Text = lsStrings[(string)lblGameDir.Tag];
			cmdBrowse.Text = lsStrings[(string)cmdBrowse.Tag];
			lblLanguage.Text = lsStrings[(string)lblLanguage.Tag];
			Tools.ResizeRelatedControl(cboLanguage, lblLanguage);
			chkBasicScreenChecks.Text = lsStrings[(string)chkBasicScreenChecks.Tag];
			chkPreShuffleOnExport.Text = lsStrings[(string)chkPreShuffleOnExport.Tag];
			cmdApply.Text = lsStrings[(string)cmdApply.Tag];
			cmdCancel.Text = lsStrings[(string)cmdCancel.Tag];

			// For Id Scheme stuff
			gbIdScheme.Text = lsStrings[(string)gbIdScheme.Tag];
			lblSchemeType.Text = lsStrings[(string)lblSchemeType.Tag];
			lblMinDigits.Text = lsStrings[(string)lblMinDigits.Tag];
			Tools.ResizeRelatedControl(numMinDigits, lblMinDigits);
			lblMinId.Text = lsStrings[(string)lblMinId.Tag];
			Tools.ResizeRelatedControl(numMinId, lblMinId);
			lblMaxId.Text = lsStrings[(string)lblMaxId.Tag];
			Tools.ResizeRelatedControl(numMaxId, lblMaxId);
			chkIdBlock.Text = lsStrings[(string)chkIdBlock.Tag];
			Tools.ResizeRelatedControl(numIdBlock, chkIdBlock);
			lblDeckChange.Text = lsStrings[(string)lblDeckChange.Tag];
			Tools.ResizeRelatedControl(numDeckChange, lblDeckChange);
			lblLandPoolChange.Text = lsStrings[(string)lblLandPoolChange.Tag];
			Tools.ResizeRelatedControl(numLandPoolChange, lblLandPoolChange);
			lblRegularUnlockChange.Text = lsStrings[(string)lblRegularUnlockChange.Tag];
			Tools.ResizeRelatedControl(numRegularUnlockChange, lblRegularUnlockChange);
			lblPromoUnlockChange.Text = lsStrings[(string)lblPromoUnlockChange.Tag];
			Tools.ResizeRelatedControl(numPromoUnlockChange, lblPromoUnlockChange);
			lblIdExample.Text = lsStrings[(string)lblIdExample.Tag];

			// To load the Scheme Types I have to reload the combo box
			cboSchemeType.Items.Clear();
			cboSchemeType.Items.Add(new KeyValuePair<IdSchemeType, string>(IdSchemeType.AdditiveScheme, lsStrings[IdSchemeType.AdditiveScheme.ToString().ToUpper()]));
			cboSchemeType.Items.Add(new KeyValuePair<IdSchemeType, string>(IdSchemeType.PrefixScheme, lsStrings[IdSchemeType.PrefixScheme.ToString().ToUpper()]));
			cboSchemeType.Items.Add(new KeyValuePair<IdSchemeType, string>(IdSchemeType.SuffixScheme, lsStrings[IdSchemeType.SuffixScheme.ToString().ToUpper()]));

			// For combo boxes that need to be re-selected after string change.
			if (m_bLoading)
				cboSchemeType.SelectedItem = new KeyValuePair<IdSchemeType, string>(m_isScheme.Type, lsStrings[m_isScheme.Type.ToString().ToUpper()]);
			else
			{
				m_bLoading = true;
				cboSchemeType.SelectedItem = new KeyValuePair<IdSchemeType, string>(m_isScheme.Type, lsStrings[m_isScheme.Type.ToString().ToUpper()]);
				m_bLoading = false;
			}
		}

		private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				if (cboLanguage.SelectedIndex > -1)
				{
					LoadLocalizedStrings(((KeyValuePair<string, string>)cboLanguage.SelectedItem).Key);
				}
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			if ((txtGameDir.Text.Trim().Length == 0) || (!Directory.Exists(txtGameDir.Text.Trim())))
			{
				// We have a problem: we don't have a valid directory.
				MessageBox.Show(Settings.UIStrings["NEED_VALID_GAME_DRIECTORY_MESSAGE"], Settings.UIStrings["NEED_VALID_GAME_DRIECTORY_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Settings.SaveSetting("DotP2014Directory", txtGameDir.Text.Trim());
			Settings.ChangeLanguage(((KeyValuePair<string, string>)cboLanguage.SelectedItem).Key);
			m_frmMain.LoadLocalizedStrings();
			Settings.SaveSetting("PerformBasicScreenChecks", chkBasicScreenChecks.Checked);
			Settings.SaveSetting("PreShuffleOnExport", chkPreShuffleOnExport.Checked);

			// Id Scheme Options
			Settings.SaveSerializableSetting("CurrentIdScheme", m_isScheme);

			this.Close();
		}

		private void cmdBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofdChoose = new OpenFileDialog();
			ofdChoose.AutoUpgradeEnabled = true;
			ofdChoose.CheckFileExists = true;
			ofdChoose.CheckPathExists = true;
			ofdChoose.DereferenceLinks = true;
			ofdChoose.Filter = Settings.UIStrings["CHOOSE_DIRECTORY_FILTER"];
			ofdChoose.FilterIndex = 1;
			ofdChoose.Title = Settings.UIStrings["CHOOSE_DIRECTORY_TITLE"];
			if (txtGameDir.Text.Trim().Length > 0)
				ofdChoose.InitialDirectory = txtGameDir.Text.Trim();
			ofdChoose.FileName = "DotP_D14.exe";
			DialogResult drResult = ofdChoose.ShowDialog();
			if (drResult == DialogResult.OK)
			{
				txtGameDir.Text = Path.GetDirectoryName(ofdChoose.FileName);
			}
		}

		private void Options_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("OptionsPosition", new Rectangle(this.Location, this.Size));
		}

		private void numChosenId_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				lblDeckId.Text = m_isScheme.GetDeckId((int)numChosenId.Value).ToString();
			}
			catch(Exception)
			{
				lblDeckId.Text = Settings.UIStrings["INVALID"];
			}
			try
			{
				lblLandPoolId.Text = m_isScheme.GetLandPoolId((int)numChosenId.Value).ToString();
			}
			catch (Exception)
			{
				lblLandPoolId.Text = Settings.UIStrings["INVALID"];
			}
			try
			{
				lblRegularUnlockId.Text = m_isScheme.GetRegularUnlockId((int)numChosenId.Value).ToString();
			}
			catch (Exception)
			{
				lblRegularUnlockId.Text = Settings.UIStrings["INVALID"];
			}
			try
			{
				lblPromoUnlockId.Text = m_isScheme.GetPromoUnlockId((int)numChosenId.Value).ToString();
			}
			catch (Exception)
			{
				lblPromoUnlockId.Text = Settings.UIStrings["INVALID"];
			}
		}

		private void cboSchemeType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.Type = ((KeyValuePair<IdSchemeType, string>)cboSchemeType.SelectedItem).Key;
				numChosenId_ValueChanged(null, null);
			}
			switch (m_isScheme.Type)
			{
				case IdSchemeType.AdditiveScheme:
					lblMinDigits.Enabled = false;
					numMinDigits.Enabled = false;
					break;
				case IdSchemeType.PrefixScheme:
					lblMinDigits.Enabled = true;
					numMinDigits.Enabled = true;
					break;
				case IdSchemeType.SuffixScheme:
					lblMinDigits.Enabled = false;
					numMinDigits.Enabled = false;
					break;
			}
			ColourizeFields();
		}

		private void numMinDigits_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.MinimumIdDigits = (int)numMinDigits.Value;
				numChosenId_ValueChanged(null, null);
			}
		}

		private void numMinId_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				try
				{
					m_isScheme.MinimumId = (int)numMinId.Value;
					numChosenId.Minimum = m_isScheme.MinimumId;
					numChosenId_ValueChanged(null, null);
				}
				catch (Exception)
				{
					numMinId.Value = m_isScheme.MinimumId;
				}
			}
		}

		private void numMaxId_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				try
				{
					m_isScheme.MaximumId = (int)numMaxId.Value;
					numChosenId.Maximum = m_isScheme.MaximumId;
					numChosenId_ValueChanged(null, null);
				}
				catch (Exception)
				{
					numMaxId.Value = m_isScheme.MaximumId;
				}
			}
		}

		private void numDeckChange_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.DeckIdChange = (int)numDeckChange.Value;
				numChosenId_ValueChanged(null, null);
				ColourizeFields();
			}
		}

		private void numLandPoolChange_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.LandPoolIdChange = (int)numLandPoolChange.Value;
				numChosenId_ValueChanged(null, null);
				ColourizeFields();
			}
		}

		private void numRegularUnlockChange_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.RegularUnlockIdChange = (int)numRegularUnlockChange.Value;
				numChosenId_ValueChanged(null, null);
				ColourizeFields();
			}
		}

		private void numPromoUnlockChange_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.PromoUnlockIdChange = (int)numPromoUnlockChange.Value;
				numChosenId_ValueChanged(null, null);
				ColourizeFields();
			}
		}

		// This function is intended to colourize the Id Scheme UI
		//	fields to emphasize to users they should change them.
		private void ColourizeFields()
		{
			IdScheme isDefault = new IdScheme();

			if (m_isScheme.UseIdBlock)
			{
				if (m_isScheme.IdBlock == isDefault.IdBlock)
				{
					numIdBlock.BackColor = Color.LightPink;
					numDeckChange.BackColor = Color.LightPink;
					numLandPoolChange.BackColor = Color.LightPink;
					numRegularUnlockChange.BackColor = Color.LightPink;
					numPromoUnlockChange.BackColor = Color.LightPink;
				}
				else
				{
					numIdBlock.BackColor = Color.LightGreen;
					numDeckChange.BackColor = Color.LightGreen;
					numLandPoolChange.BackColor = Color.LightGreen;
					numRegularUnlockChange.BackColor = Color.LightGreen;
					numPromoUnlockChange.BackColor = Color.LightGreen;
				}
			}
			else
			{
				numIdBlock.BackColor = SystemColors.Window;
				if (m_isScheme.Type == isDefault.Type)
				{
					// Deck Prefix (should not equal any of the defaults or equal to any of the other changes).
					if ((m_isScheme.DeckIdChange == isDefault.DeckIdChange) ||
						(m_isScheme.DeckIdChange == isDefault.LandPoolIdChange) ||
						(m_isScheme.DeckIdChange == isDefault.RegularUnlockIdChange) ||
						(m_isScheme.DeckIdChange == isDefault.PromoUnlockIdChange) ||
						(m_isScheme.DeckIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.DeckIdChange == m_isScheme.RegularUnlockIdChange) ||
						(m_isScheme.DeckIdChange == m_isScheme.PromoUnlockIdChange))
						numDeckChange.BackColor = Color.LightPink;
					else
						numDeckChange.BackColor = Color.LightGreen;

					// Land Pool Prefix
					if ((m_isScheme.LandPoolIdChange == isDefault.DeckIdChange) ||
						(m_isScheme.LandPoolIdChange == isDefault.LandPoolIdChange) ||
						(m_isScheme.LandPoolIdChange == isDefault.RegularUnlockIdChange) ||
						(m_isScheme.LandPoolIdChange == isDefault.PromoUnlockIdChange) ||
						(m_isScheme.LandPoolIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.LandPoolIdChange == m_isScheme.RegularUnlockIdChange) ||
						(m_isScheme.LandPoolIdChange == m_isScheme.PromoUnlockIdChange))
						numLandPoolChange.BackColor = Color.LightPink;
					else
						numLandPoolChange.BackColor = Color.LightGreen;

					// Regular Unlock Prefix
					if ((m_isScheme.RegularUnlockIdChange == isDefault.DeckIdChange) ||
						(m_isScheme.RegularUnlockIdChange == isDefault.LandPoolIdChange) ||
						(m_isScheme.RegularUnlockIdChange == isDefault.RegularUnlockIdChange) ||
						(m_isScheme.RegularUnlockIdChange == isDefault.PromoUnlockIdChange) ||
						(m_isScheme.RegularUnlockIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.RegularUnlockIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.RegularUnlockIdChange == m_isScheme.PromoUnlockIdChange))
						numRegularUnlockChange.BackColor = Color.LightPink;
					else
						numRegularUnlockChange.BackColor = Color.LightGreen;

					// Promo Unlock Prefix
					if ((m_isScheme.PromoUnlockIdChange == isDefault.DeckIdChange) ||
						(m_isScheme.PromoUnlockIdChange == isDefault.LandPoolIdChange) ||
						(m_isScheme.PromoUnlockIdChange == isDefault.RegularUnlockIdChange) ||
						(m_isScheme.PromoUnlockIdChange == isDefault.PromoUnlockIdChange) ||
						(m_isScheme.PromoUnlockIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.PromoUnlockIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.PromoUnlockIdChange == m_isScheme.RegularUnlockIdChange))
						numPromoUnlockChange.BackColor = Color.LightPink;
					else
						numPromoUnlockChange.BackColor = Color.LightGreen;
				}
				else
				{
					// The type is different so we don't really care about whether they match the defaults or not.
					//	We do, however, care if they match each other.
					if ((m_isScheme.DeckIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.DeckIdChange == m_isScheme.RegularUnlockIdChange) ||
						(m_isScheme.DeckIdChange == m_isScheme.PromoUnlockIdChange))
						numDeckChange.BackColor = Color.LightPink;
					else
						numDeckChange.BackColor = SystemColors.Window;

					if ((m_isScheme.LandPoolIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.LandPoolIdChange == m_isScheme.RegularUnlockIdChange) ||
						(m_isScheme.LandPoolIdChange == m_isScheme.PromoUnlockIdChange))
						numLandPoolChange.BackColor = Color.LightPink;
					else
						numLandPoolChange.BackColor = SystemColors.Window;

					if ((m_isScheme.RegularUnlockIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.RegularUnlockIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.RegularUnlockIdChange == m_isScheme.PromoUnlockIdChange))
						numRegularUnlockChange.BackColor = Color.LightPink;
					else
						numRegularUnlockChange.BackColor = SystemColors.Window;

					if ((m_isScheme.PromoUnlockIdChange == m_isScheme.DeckIdChange) ||
						(m_isScheme.PromoUnlockIdChange == m_isScheme.LandPoolIdChange) ||
						(m_isScheme.PromoUnlockIdChange == m_isScheme.RegularUnlockIdChange))
						numPromoUnlockChange.BackColor = Color.LightPink;
					else
						numPromoUnlockChange.BackColor = SystemColors.Window;
				}
			}
		}

		private void CheckIdSchemeFields()
		{
			if (chkIdBlock.Checked)
			{
				numIdBlock.Enabled = true;
				numDeckChange.Enabled = false;
				numLandPoolChange.Enabled = false;
				numRegularUnlockChange.Enabled = false;
				numPromoUnlockChange.Enabled = false;
			}
			else
			{
				numIdBlock.Enabled = false;
				numDeckChange.Enabled = true;
				numLandPoolChange.Enabled = true;
				numRegularUnlockChange.Enabled = true;
				numPromoUnlockChange.Enabled = true;
			}
		}

		private void chkIdBlock_CheckedChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.UseIdBlock = chkIdBlock.Checked;
				if (chkIdBlock.Checked)
				{
					m_bLoading = true;
					numDeckChange.Value = m_isScheme.DeckIdChange;
					numLandPoolChange.Value = m_isScheme.LandPoolIdChange;
					numRegularUnlockChange.Value = m_isScheme.RegularUnlockIdChange;
					numPromoUnlockChange.Value = m_isScheme.PromoUnlockIdChange;
					m_bLoading = false;
				}
				CheckIdSchemeFields();
				ColourizeFields();
				numChosenId_ValueChanged(null, null);
			}
		}

		private void numIdBlock_ValueChanged(object sender, EventArgs e)
		{
			if (!m_bLoading)
			{
				m_isScheme.IdBlock = (int)numIdBlock.Value;
				if (chkIdBlock.Checked)
				{
					m_bLoading = true;
					numDeckChange.Value = m_isScheme.DeckIdChange;
					numLandPoolChange.Value = m_isScheme.LandPoolIdChange;
					numRegularUnlockChange.Value = m_isScheme.RegularUnlockIdChange;
					numPromoUnlockChange.Value = m_isScheme.PromoUnlockIdChange;
					m_bLoading = false;
				}
				numChosenId_ValueChanged(null, null);
				ColourizeFields();
			}
		}
	}
}
