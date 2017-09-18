using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class DeckInformation : Form
	{
		private GameDirectory m_gdData;
		private Deck m_dkInfo;
		private Dictionary<int, string> m_dicUsedIds;
		private IdScheme m_isScheme;

		private AiPersonality m_apPersonality;

		// For adjusting numeric up/downs and selected radio buttons (due to canceling)
		private bool m_bProgramaticAdjust;
		private bool m_bChangingToLast;

		// For building deck box images.
		private ImageBuilder m_ibDeckBox;
		private bool m_bBuildDeckImage;
		private RadioButton m_rbDeckLastChecked;

		public DeckInformation(Deck dkInfo, GameDirectory gdData)
		{
			InitializeComponent();

			m_gdData = gdData;
			m_dkInfo = dkInfo;
			m_dicUsedIds = gdData.UsedIds;

			// Load our current Id Scheme.
			m_isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			numUid.Minimum = m_isScheme.MinimumId;
			numUid.Maximum = m_isScheme.MaximumId;

			LoadLocalizationStrings();

			Rectangle rcPosition = Settings.GetSetting("DeckInfoPosition", new Rectangle(-1, -1, -1, -1));
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

			// Now load our values.
			//	Basic
			cboAvailability.SelectedItem = new KeyValuePair<DeckAvailability, string>(m_dkInfo.Availability, Settings.UIStrings[m_dkInfo.Availability.ToString().ToUpper()]);
			// Uid
			try
			{
				numUid.Value = m_dkInfo.Uid;
			}
			catch (Exception)
			{ }
			numUid_ValueChanged(null, null);

			// Deck Statistics
			if (!m_dkInfo.CreatureSize.Equals("?"))
				((RadioButton)pnlSize.Controls["rbSize" + m_dkInfo.CreatureSize]).Checked = true;
			if (!m_dkInfo.DeckSpeed.Equals("?"))
				((RadioButton)pnlSpeed.Controls["rbSpeed" + m_dkInfo.DeckSpeed]).Checked = true;
			if (!m_dkInfo.Flexibility.Equals("?"))
				((RadioButton)pnlFlexibility.Controls["rbFlex" + m_dkInfo.Flexibility]).Checked = true;
			if (!m_dkInfo.Synergy.Equals("?"))
				((RadioButton)pnlSynergy.Controls["rbSyn" + m_dkInfo.Synergy]).Checked = true;

			// Land Config
			numIgnoreCmcOver.Value = m_dkInfo.IgnoreCmcOver;
			numMinForests.Value = m_dkInfo.MinForests;
			numMinIslands.Value = m_dkInfo.MinIslands;
			numMinMountains.Value = m_dkInfo.MinMountains;
			numMinPlains.Value = m_dkInfo.MinPlains;
			numMinSwamps.Value = m_dkInfo.MinSwamps;
			numSpellsThatCountAsLand.Value = m_dkInfo.NumberOfSpellsThatCountAsLands;

			// Deck Box
			if (m_dkInfo.DeckBoxImage != null)
				picDeckBox.Image = new Bitmap(m_dkInfo.DeckBoxImage);
			rbUseDeckImage.Checked = true;
			rbUseDeckImage_CheckedChanged(null, null);
			rbBuildImage_CheckedChanged(null, null);

			// Personality
			m_apPersonality = m_dkInfo.Personality;
			lblAiPersonality.Text = m_apPersonality.LocalizedName.Replace("&", "&&");

			// Make sure we can build deck box images.
			if ((!File.Exists(Settings.GetProgramDir() + "Images\\D14_DeckBoxOverlay.png")) ||
				(!File.Exists(Settings.GetProgramDir() + "Images\\D14_DeckBoxMask.png")) ||
				(!File.Exists(Settings.GetProgramDir() + "Images\\D14_DeckBoxAlpha.png")))
				rbBuildImage.Enabled = false;
			else
			{
				// Attempt to load them now so we don't have to later.
				try
				{
					Bitmap bmpLoadedOverlay = new Bitmap(Settings.GetProgramDir() + "Images\\D14_DeckBoxOverlay.png");
					Bitmap bmpLoadedMask = new Bitmap(Settings.GetProgramDir() + "Images\\D14_DeckBoxMask.png");
					Bitmap bmpLoadedAlpha = new Bitmap(Settings.GetProgramDir() + "Images\\D14_DeckBoxAlpha.png");
					if ((bmpLoadedOverlay != null) && (bmpLoadedMask != null))
						m_ibDeckBox = new ImageBuilder(bmpLoadedOverlay, bmpLoadedMask, bmpLoadedAlpha, new Rectangle(150, 93, 216, 257));
					else
						rbBuildImage.Enabled = false;
				}
				catch (Exception e)
				{
					// We can't load the box template and mask so we can't build deck box images.
					rbBuildImage.Enabled = false;
					Settings.ReportError(e, ErrorPriority.Low, "Could not load Deck Box Overlay, Mask, and Alpha, building of deck box images will be disabled.");
				}
			}

			m_bProgramaticAdjust = false;
		}

		private void LoadLocalizationStrings()
		{
			this.Text = Settings.UIStrings[(string)this.Tag];

			// Basic Info
			gbBasicInfo.Text = Settings.UIStrings[(string)gbBasicInfo.Tag];
			lblAvailability.Text = Settings.UIStrings[(string)lblAvailability.Tag];
			cboAvailability.DisplayMember = "Value";
			cboAvailability.ValueMember = "Key";
			cboAvailability.Items.Add(new KeyValuePair<DeckAvailability, string>(DeckAvailability.AlwaysAvailable, Settings.UIStrings[DeckAvailability.AlwaysAvailable.ToString().ToUpper()]));
			cboAvailability.Items.Add(new KeyValuePair<DeckAvailability, string>(DeckAvailability.Locked, Settings.UIStrings[DeckAvailability.Locked.ToString().ToUpper()]));
			cboAvailability.Items.Add(new KeyValuePair<DeckAvailability, string>(DeckAvailability.NeverAvailable, Settings.UIStrings[DeckAvailability.NeverAvailable.ToString().ToUpper()]));
			cmdEditPersonality.Text = Settings.UIStrings[(string)cmdEditPersonality.Tag];
			lblUid.Text = Settings.UIStrings[(string)lblUid.Tag];
			cmdGetFirstAvailableUid.Text = Settings.UIStrings[(string)cmdGetFirstAvailableUid.Tag];
			lblFinalUidText.Text = Settings.UIStrings[(string)lblFinalUidText.Tag];

			// Deck Statistics
			gbDeckStatistics.Text = Settings.UIStrings[(string)gbDeckStatistics.Tag];
			lblCreatureSize.Text = Settings.UIStrings[(string)lblCreatureSize.Tag];
			lblSpeed.Text = Settings.UIStrings[(string)lblSpeed.Tag];
			lblFlexibility.Text = Settings.UIStrings[(string)lblFlexibility.Tag];
			lblSynergy.Text = Settings.UIStrings[(string)lblSynergy.Tag];

			// Land Config
			gbLandConfig.Text = Settings.UIStrings[(string)gbLandConfig.Tag];
			lblIgnoreCmcOver.Text = Settings.UIStrings[(string)lblIgnoreCmcOver.Tag];
			lblMinForests.Text = Settings.UIStrings[(string)lblMinForests.Tag];
			lblMinIslands.Text = Settings.UIStrings[(string)lblMinIslands.Tag];
			lblMinMountains.Text = Settings.UIStrings[(string)lblMinMountains.Tag];
			lblMinPlains.Text = Settings.UIStrings[(string)lblMinPlains.Tag];
			lblMinSwamps.Text = Settings.UIStrings[(string)lblMinSwamps.Tag];
			lblNumSpellsThatCountAsLand.Text = Settings.UIStrings[(string)lblNumSpellsThatCountAsLand.Tag];

			// Deck Box
			gbDeckBox.Text = Settings.UIStrings[(string)gbDeckBox.Tag];
			rbUseDeckImage.Text = Settings.UIStrings[(string)rbUseDeckImage.Tag];
			rbLoadImage.Text = Settings.UIStrings[(string)rbLoadImage.Tag];
			rbBuildImage.Text = Settings.UIStrings[(string)rbBuildImage.Tag];
			gbDeckLocation.Text = Settings.UIStrings[(string)gbDeckLocation.Tag];
			gbDeckSize.Text = Settings.UIStrings[(string)gbDeckSize.Tag];

			// Apply & Cancel
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];

			// Context Menu for exporting images.
			cmnuiExport.Text = Settings.UIStrings[(string)cmnuiExport.Tag];
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			// Save off our values.
			m_dkInfo.Availability = ((KeyValuePair<DeckAvailability, string>)cboAvailability.SelectedItem).Key;
			m_dkInfo.Uid = (int)numUid.Value;
			m_dkInfo.CreatureSize = ValueFromRadioButtonPanel(pnlSize);
			m_dkInfo.DeckSpeed = ValueFromRadioButtonPanel(pnlSpeed);
			m_dkInfo.Flexibility = ValueFromRadioButtonPanel(pnlFlexibility);
			m_dkInfo.Synergy = ValueFromRadioButtonPanel(pnlSynergy);
			m_dkInfo.IgnoreCmcOver = (int)numIgnoreCmcOver.Value;
			m_dkInfo.MinForests = (int)numMinForests.Value;
			m_dkInfo.MinIslands = (int)numMinIslands.Value;
			m_dkInfo.MinMountains = (int)numMinMountains.Value;
			m_dkInfo.MinPlains = (int)numMinPlains.Value;
			m_dkInfo.MinSwamps = (int)numMinSwamps.Value;
			m_dkInfo.NumberOfSpellsThatCountAsLands = (int)numSpellsThatCountAsLand.Value;
			// If they apply with a null image then trying to make a new bitmap from it will error out so check for that.
			if (m_dkInfo.DeckBoxImage != null)
			{
				m_dkInfo.DeckBoxImage.Dispose();
				m_dkInfo.DeckBoxImage = null;
			}
			if (picDeckBox.Image != null)
				m_dkInfo.DeckBoxImage = new Bitmap(picDeckBox.Image);
			// Set our personality.
			m_dkInfo.Personality = m_apPersonality;
			m_dkInfo.Edited = true;

			this.Close();
		}

		private string ValueFromRadioButtonPanel(Panel pnlButtons)
		{
			string strReturn = "?";	// In case we can't figure it out which shouldn't be the case.

			foreach (Control ctrl in pnlButtons.Controls)
			{
				if (ctrl is RadioButton)
				{
					RadioButton rbButton = ctrl as RadioButton;
					if (rbButton.Checked)
					{
						strReturn = (string)rbButton.Tag;
					}
				}
			}

			return strReturn;
		}

		private void cmdGetFirstAvailableUid_Click(object sender, EventArgs e)
		{
			try
			{
				int nId = m_isScheme.GetNextAvailableId(m_dicUsedIds);
				if (nId > -1)
					numUid.Value = nId;
				else
				{
					// No available ids given the current scheme settings.
					MessageBox.Show(Settings.UIStrings["NO_AVAILABLE_IDS_MESSAGE"], Settings.UIStrings["NO_AVAILABLE_IDS_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception)
			{
				// If we catch an exception then there is probably something wrong with the scheme settings.
				MessageBox.Show(Settings.UIStrings["PROBLEM_WITH_SCHEME_SETTINGS_MESSAGE"], Settings.UIStrings["PROBLEM_WITH_SCHEME_SETTINGS_CAPTION"], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void numUid_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				lblFinalUid.Text = m_isScheme.GetDeckId((int)numUid.Value).ToString();
			}
			catch (Exception)
			{
				lblFinalUid.Text = Settings.UIStrings["INVALID"];
			}
		}

		private void rbUseDeckImage_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbUseDeckImage.Checked) && (!m_bChangingToLast))
			{
				if (picDeckBox.Image != null)
					picDeckBox.Image.Dispose();
				if (m_dkInfo.DeckBoxImage != null)
					picDeckBox.Image = new Bitmap(m_dkInfo.DeckBoxImage);
				else
					picDeckBox.Image = null;
				m_bBuildDeckImage = false;
				m_rbDeckLastChecked = rbUseDeckImage;
			}
		}

		private void rbLoadImage_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbLoadImage.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					if (picDeckBox.Image != null)
						picDeckBox.Image.Dispose();
					picDeckBox.Image = bmpChosen;
					m_rbDeckLastChecked = rbLoadImage;
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbDeckLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
		}

		private void rbBuildImage_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbBuildImage.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					m_ibDeckBox.LoadedImage = bmpChosen;
					InitialDeckImageAdjust();
					if (picDeckBox.Image != null)
						picDeckBox.Image.Dispose();
					picDeckBox.Image = m_ibDeckBox.BuildImage();

					// We've successfully built an initial image so go ahead and enable our controls.
					m_rbDeckLastChecked = rbBuildImage;
					m_bBuildDeckImage = true;
					Tools.ChangeControlEnabled(scDeckLocationSize, true);
				}
				else
				{
					// We've successfully built an initial image so go ahead and enable our controls.
					m_rbDeckLastChecked = rbBuildImage;
					m_bBuildDeckImage = true;
					Tools.ChangeControlEnabled(scDeckLocationSize, true);
				}
			}
			else if ((m_bChangingToLast) && (m_rbDeckLastChecked == rbBuildImage))
			{
				// We were the last checked so we go back to here and re-enable build mode.
				m_bBuildDeckImage = true;
				Tools.ChangeControlEnabled(scDeckLocationSize, true);
			}
			else
			{
				// Disable our controls.
				m_bBuildDeckImage = false;
				Tools.ChangeControlEnabled(scDeckLocationSize, false);
			}
		}

		private void InitialDeckImageAdjust()
		{
			m_ibDeckBox.InitialImageAdjust();

			// Update our controls (without screwing this up)
			m_bProgramaticAdjust = true;
			numDeckX.Value = m_ibDeckBox.AdjustedRect.X;
			numDeckY.Value = m_ibDeckBox.AdjustedRect.Y;
			numDeckWidth.Value = m_ibDeckBox.AdjustedRect.Width;
			numDeckHeight.Value = m_ibDeckBox.AdjustedRect.Height;
			m_bProgramaticAdjust = false;
		}

		private Bitmap ChooseImage()
		{
			Bitmap bmpReturn = null;

			OpenFileDialog ofdLoad = new OpenFileDialog();
			ofdLoad.AddExtension = false;
			ofdLoad.AutoUpgradeEnabled = true;
			ofdLoad.CheckFileExists = true;
			ofdLoad.CheckPathExists = true;
			ofdLoad.DereferenceLinks = true;
			ofdLoad.Filter = Settings.UIStrings["LOAD_IMAGE_FILTER"];
			ofdLoad.FilterIndex = 1;
			ofdLoad.Multiselect = false;
			ofdLoad.Title = Settings.UIStrings["LOAD_IMAGE_CAPTION"];
			DialogResult drResult = ofdLoad.ShowDialog();
			if (drResult == DialogResult.OK)
			{
				// We should have a valid filename and extension so time to load the image.
				try
				{
					if (Path.GetExtension(ofdLoad.FileName).Equals(".tdx", StringComparison.OrdinalIgnoreCase))
					{
						TdxWrapper twLoaded = new TdxWrapper();
						twLoaded.LoadTdx(ofdLoad.FileName);
						bmpReturn = new Bitmap(twLoaded.Image);
						twLoaded.Dispose();
					}
					else
					{
						Bitmap bmpLoading = new Bitmap(ofdLoad.FileName);
						bmpReturn = bmpLoading;
					}
				}
				catch (Exception e)
				{
					// We don't really care what happens here, we'll just treat it as though the user canceled.
					//	But we will report the error anyway.
					Settings.ReportError(e, ErrorPriority.Low, "Failed to load image: " + ofdLoad.FileName);
				}
			}

			return bmpReturn;
		}

		private void numDeckX_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildDeckImage) && (!m_bProgramaticAdjust))
			{
				m_ibDeckBox.AdjustedRect.X = (int)numDeckX.Value;
				picDeckBox.Image = m_ibDeckBox.BuildImage();
			}
		}

		private void numDeckY_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildDeckImage) && (!m_bProgramaticAdjust))
			{
				m_ibDeckBox.AdjustedRect.Y = (int)numDeckY.Value;
				picDeckBox.Image = m_ibDeckBox.BuildImage();
			}
		}

		private void numDeckWidth_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildDeckImage) && (!m_bProgramaticAdjust))
			{
				m_ibDeckBox.AdjustedRect.Width = (int)numDeckWidth.Value;
				picDeckBox.Image = m_ibDeckBox.BuildImage();
			}
		}

		private void numDeckHeight_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildDeckImage) && (!m_bProgramaticAdjust))
			{
				m_ibDeckBox.AdjustedRect.Height = (int)numDeckHeight.Value;
				picDeckBox.Image = m_ibDeckBox.BuildImage();
			}
		}

		private void picDeckBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bBuildDeckImage)
				m_ibDeckBox.MouseDownHandler(e);
		}

		private void picDeckBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bBuildDeckImage)
			{
				picDeckBox.Image = m_ibDeckBox.MouseMoveHandler(e);
				// And update the location numeric up downs.
				lock (numDeckX)
				{
					m_bProgramaticAdjust = true;
					numDeckX.Value = m_ibDeckBox.AdjustedRect.X;
					numDeckY.Value = m_ibDeckBox.AdjustedRect.Y;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void picDeckBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bBuildDeckImage)
				m_ibDeckBox.MouseUpHandler(e);

			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
				cmnuPictures.Show(Cursor.Position);
		}

		private void picDeckBox_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (m_bBuildDeckImage)
			{
				picDeckBox.Image = m_ibDeckBox.MouseWheelHandler(e);
				lock (numDeckX)
				{
					m_bProgramaticAdjust = true;
					numDeckX.Value = m_ibDeckBox.AdjustedRect.X;
					numDeckY.Value = m_ibDeckBox.AdjustedRect.Y;
					numDeckWidth.Value = m_ibDeckBox.AdjustedRect.Width;
					numDeckHeight.Value = m_ibDeckBox.AdjustedRect.Height;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void picDeckBox_MouseEnter(object sender, EventArgs e)
		{
			if (m_bBuildDeckImage)
			{
				// Take focus so that we can get the mouse wheel events.
				picDeckBox.Focus();
			}
		}

		private void DeckInformation_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("DeckInfoPosition", new Rectangle(this.Location, this.Size));
		}

		private void cmnuiExportPng_Click(object sender, EventArgs e)
		{
			string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png");
			if (strFilename.Length > 0)
				picDeckBox.Image.Save(strFilename, System.Drawing.Imaging.ImageFormat.Png);
		}

		private void cmnuiExportTdx_Click(object sender, EventArgs e)
		{
			string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX");
			if (strFilename.Length > 0)
			{
				TdxWrapper twImage = new TdxWrapper();
				twImage.LoadImage(picDeckBox.Image, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT5, Settings.GetSetting("IncludeMipMaps", true));
				twImage.Save(strFilename);
				twImage.Dispose();
			}
		}

		private void cmdEditPersonality_Click(object sender, EventArgs e)
		{
			EditAiPersonality frmPersonality = new EditAiPersonality(m_apPersonality, m_gdData);
			DialogResult drResult = frmPersonality.ShowDialog(this);
			if (drResult == DialogResult.OK)
			{
				m_apPersonality = frmPersonality.FinalizedPersonality;
				lblAiPersonality.Text = m_apPersonality.LocalizedName.Replace("&", "&&");
			}
			frmPersonality.Dispose();
		}
	}
}
