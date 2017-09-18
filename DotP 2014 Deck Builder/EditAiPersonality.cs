using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class EditAiPersonality : Form
	{
		private AiPersonality m_apPersonality;
		private AiPersonality m_apFinal;

		private bool m_bLoadingPersonality;

		// For adjusting numeric up/downs and selected radio buttons (due to canceling)
		private bool m_bProgramaticAdjust;
		private bool m_bChangingToLast;

		// For building Personality images.
		private ImageBuilder m_ibSmallAvatar;
		private ImageBuilder m_ibLargeAvatar;
		private ImageBuilder m_ibFullAvatar;
		private bool m_bBuildSmallAvatar;
		private bool m_bBuildLargeAvatar;
		private bool m_bBuildFullAvatar;
		private RadioButton m_rbSmallAvatarLastChecked;
		private RadioButton m_rbLargeAvatarLastChecked;
		private RadioButton m_rbFullAvatarLastChecked;

		// For context menu support for exporting images.
		private PictureBox m_picContextPicBox;

		public EditAiPersonality(AiPersonality apPersonality, GameDirectory gdData)
		{
			InitializeComponent();

			LoadLocalizationStrings();

			Rectangle rcPosition = Settings.GetSetting("EditPersonalityPosition", new Rectangle(-1, -1, -1, -1));
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

			m_bLoadingPersonality = true;
			cboMusic.DataSource = gdData.Music;
			cboPersonality.DisplayMember = "LocalizedName";
			if ((apPersonality != null) && (!apPersonality.BuiltIn))
			{
				m_apPersonality = new AiPersonality(apPersonality);
				SortableBindingList<AiPersonality> lstPersonalities = new SortableBindingList<AiPersonality>(gdData.Personalities.Values);
				lstPersonalities.Insert(0, m_apPersonality);
				cboPersonality.DataSource = lstPersonalities;
				cboPersonality.SelectedItem = m_apPersonality;
			}
			else
			{
				cboPersonality.DataSource = new SortableBindingList<AiPersonality>(gdData.Personalities.Values);
				if (apPersonality != null)
					cboPersonality.SelectedItem = apPersonality;
				m_apPersonality = apPersonality;
			}
			RefreshPersonality();

			// Set up for building full avatar images.
			m_rbFullAvatarLastChecked = rbUseThisImageFull;
			m_ibFullAvatar = new ImageBuilder(null, null, null, new Rectangle(0, 0, 512, 512));
			rbBuildImageFull_CheckedChanged(null, null);

			// Make sure we can build large avatars for personality.
			m_rbLargeAvatarLastChecked = rbUseThisImageLarge;
			if (!File.Exists(Settings.GetProgramDir() + "Images\\D14_PersonalityBackplateAlpha.png"))
				rbBuildImageLarge.Enabled = false;
			else
			{
				// Attempt to load them now so we don't have to later.
				try
				{
					Bitmap bmpLoadedAlpha = new Bitmap(Settings.GetProgramDir() + "Images\\D14_PersonalityBackplateAlpha.png");
					if (bmpLoadedAlpha != null)
					{
						m_ibLargeAvatar = new ImageBuilder(null, null, bmpLoadedAlpha, new Rectangle(0, 0, bmpLoadedAlpha.Width, bmpLoadedAlpha.Height));
						// Since we are showing the image at half width, half height we should adjust our mouse change by a factor of 2.
						//m_ibLargeAvatar.MouseChangeModifier = 2f;
					}
					else
						rbBuildImageLarge.Enabled = false;
				}
				catch (Exception e)
				{
					// We can't load the box template and mask so we can't build deck box images.
					rbBuildImageLarge.Enabled = false;
					Settings.ReportError(e, ErrorPriority.Low, "Could not load Large Avatar Overlay and Mask, building of large avatar images will be disabled.");
				}
			}
			rbBuildImageLarge_CheckedChanged(null, null);

			// Make sure we can build small avatars for personality.
			m_rbSmallAvatarLastChecked = rbUseThisImageSmall;
			if ((!File.Exists(Settings.GetProgramDir() + "Images\\D14_PersonalityCircularMask.png")) ||
				(!File.Exists(Settings.GetProgramDir() + "Images\\D14_PersonalityCircularAlpha.png")))
				rbBuildImageSmall.Enabled = false;
			else
			{
				// Attempt to load the mask so we don't have to later.
				try
				{
					Bitmap bmpSmallAvatarMask = new Bitmap(Settings.GetProgramDir() + "Images\\D14_PersonalityCircularMask.png");
					Bitmap bmpSmallAvatarAlpha = new Bitmap(Settings.GetProgramDir() + "Images\\D14_PersonalityCircularAlpha.png");
					if ((bmpSmallAvatarMask != null) && (bmpSmallAvatarAlpha != null))
						m_ibSmallAvatar = new ImageBuilder(null, bmpSmallAvatarMask, bmpSmallAvatarAlpha, new Rectangle(38, 38, 180, 180));
					else
						rbBuildImageSmall.Enabled = false;
				}
				catch (Exception e)
				{
					// Couldn't load the mask so we can't build small avatar images.
					rbBuildImageSmall.Enabled = false;
					Settings.ReportError(e, ErrorPriority.Low, "Could not load Circular Personality Mask and Alpha, building of circular personality images will be disabled.");
				}
			}
			rbBuildImageSmall_CheckedChanged(null, null);

			m_bProgramaticAdjust = false;
		}

		public AiPersonality FinalizedPersonality
		{
			get { return m_apFinal; }
		}

		private void RefreshPersonality()
		{
			m_bLoadingPersonality = true;
			lblPersonalityName.Text = m_apPersonality.LocalizedName.Replace("&", "&&");
			if ((m_apPersonality.MusicMix != null) && (m_apPersonality.MusicMix.Length > 0))
				cboMusic.SelectedItem = m_apPersonality.MusicMix;

			if (m_apPersonality.SmallAvatarImage != null)
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
			else
				picPersonalitySmall.Image = null;

			if (m_apPersonality.LobbyImage != null)
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
			else
				picPersonalityLarge.Image = null;

			if (m_apPersonality.LargeAvatarImage != null)
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
			else
				picPersonalityFull.Image = null;

			rbUseThisImageSmall.Checked = true;
			rbUseThisImageLarge.Checked = true;
			rbUseThisImageFull.Checked = true;
			m_bLoadingPersonality = false;
		}

		private void PrepPersonalityForEditing()
		{
			// Only really need to care if this is a built-in personality, if custom it already is marked.
			if (m_apPersonality.BuiltIn)
			{
				// To really do this properly we need to create a new Personality as a copy of the current one
				m_bLoadingPersonality = true;
				m_apPersonality = new AiPersonality(m_apPersonality);
				// Add this to the drop down's data source and select it.
				((SortableBindingList<AiPersonality>)cboPersonality.DataSource).Add(m_apPersonality);
				cboPersonality.SelectedItem = m_apPersonality;
				m_bLoadingPersonality = false;
				// We should now be ready to accept any editing without screwing up the built-in personalities for this run.
			}
		}

		private void LoadLocalizationStrings()
		{
			this.Text = Settings.UIStrings[(string)this.Tag];

			cboPersonality.Refresh();
			cmdPersonalityEditName.Text = Settings.UIStrings[(string)cmdPersonalityEditName.Tag];
			lblMusic.Text = Settings.UIStrings[(string)lblMusic.Tag];
			// Adjust the Music Select combobox to match with new label length.
			Tools.ResizeRelatedControl(cboMusic, lblMusic);

			rbUseThisImageSmall.Text = Settings.UIStrings[(string)rbUseThisImageSmall.Tag];
			rbLoadImageSmall.Text = Settings.UIStrings[(string)rbLoadImageSmall.Tag];
			rbBuildImageSmall.Text = Settings.UIStrings[(string)rbBuildImageSmall.Tag];
			gbSmallLocation.Text = Settings.UIStrings[(string)gbSmallLocation.Tag];
			gbSmallSize.Text = Settings.UIStrings[(string)gbSmallSize.Tag];

			rbUseThisImageLarge.Text = Settings.UIStrings[(string)rbUseThisImageLarge.Tag];
			rbLoadImageLarge.Text = Settings.UIStrings[(string)rbLoadImageLarge.Tag];
			rbBuildImageLarge.Text = Settings.UIStrings[(string)rbBuildImageLarge.Tag];
			gbLargeLocation.Text = Settings.UIStrings[(string)gbLargeLocation.Tag];
			gbLargeSize.Text = Settings.UIStrings[(string)gbLargeSize.Tag];

			rbUseThisImageFull.Text = Settings.UIStrings[(string)rbUseThisImageFull.Tag];
			rbLoadImageFull.Text = Settings.UIStrings[(string)rbLoadImageFull.Tag];
			rbBuildImageFull.Text = Settings.UIStrings[(string)rbBuildImageFull.Tag];
			gbFullLocation.Text = Settings.UIStrings[(string)gbFullLocation.Tag];
			gbFullSize.Text = Settings.UIStrings[(string)gbFullSize.Tag];

			// Apply & Cancel
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];

			// Context Menu for exporting images.
			cmnuiExport.Text = Settings.UIStrings[(string)cmnuiExport.Tag];
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			m_apFinal = m_apPersonality;
			this.DialogResult = DialogResult.OK;
			this.Close();
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

		private void EditAiPersonality_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("EditPersonalityPosition", new Rectangle(this.Location, this.Size));
		}

		private void cboPersonality_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bLoadingPersonality)
			{
				if (cboPersonality.SelectedItem != null)
				{
					if (cboPersonality.SelectedItem != m_apPersonality)
					{
						m_apPersonality = cboPersonality.SelectedItem as AiPersonality;
						RefreshPersonality();
					}
				}
			}
		}

		private void cmdPersonalityEditName_Click(object sender, EventArgs e)
		{
			Dictionary<string, string> dicTemp = new Dictionary<string, string>(m_apPersonality.Name);
			EditString frmEdit = new EditString(dicTemp, cmdPersonalityEditName.Text);
			if (frmEdit.ShowDialog(this) == DialogResult.OK)
			{
				PrepPersonalityForEditing();
				m_apPersonality.Name = dicTemp;
				lblPersonalityName.Text = m_apPersonality.LocalizedName.Replace("&", "&&");
				cboPersonality.Refresh();
			}
			frmEdit.Dispose();
		}

		private void cboMusic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((!m_bLoadingPersonality) && (m_apPersonality != null) && (cboMusic.SelectedIndex > -1))
			{
				PrepPersonalityForEditing();
				m_apPersonality.MusicMix = (string)cboMusic.SelectedItem;
			}
		}

		private void rbUseThisImageSmall_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbUseThisImageSmall.Checked) && (!m_bChangingToLast))
			{
				if (m_apPersonality.SmallAvatarImage != null)
					picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
				else
					picPersonalitySmall.Image = null;
				m_bBuildSmallAvatar = false;
				m_rbSmallAvatarLastChecked = rbUseThisImageSmall;
			}
		}

		private void rbLoadImageSmall_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbLoadImageSmall.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_apPersonality.SmallAvatarImage = bmpChosen;
					picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
					m_rbSmallAvatarLastChecked = rbLoadImageSmall;
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbSmallAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
		}

		private void rbBuildImageSmall_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbBuildImageSmall.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_ibSmallAvatar.LoadedImage = bmpChosen;
					InitialSmallAvatarAdjust();
					m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.BuildImage();
					picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;

					// We've successfully built an initial image so go ahead and enable our controls.
					m_rbSmallAvatarLastChecked = rbBuildImageSmall;
					m_bBuildSmallAvatar = true;
					Tools.ChangeControlEnabled(scSmallLocationSize, true);
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbSmallAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
			else if ((m_bChangingToLast) && (m_rbSmallAvatarLastChecked == rbBuildImageSmall))
			{
				// We were the last checked so we go back to here and re-enable build mode.
				m_bBuildSmallAvatar = true;
				Tools.ChangeControlEnabled(scSmallLocationSize, true);
			}
			else
			{
				// Disable our controls.
				m_bBuildSmallAvatar = false;
				Tools.ChangeControlEnabled(scSmallLocationSize, false);
			}
		}

		private void InitialSmallAvatarAdjust()
		{
			m_ibSmallAvatar.InitialImageAdjust();

			// Update our controls (without screwing this up)
			m_bProgramaticAdjust = true;
			numSmallX.Value = m_ibSmallAvatar.AdjustedRect.X;
			numSmallY.Value = m_ibSmallAvatar.AdjustedRect.Y;
			numSmallWidth.Value = m_ibSmallAvatar.AdjustedRect.Width;
			numSmallHeight.Value = m_ibSmallAvatar.AdjustedRect.Height;
			m_bProgramaticAdjust = false;
		}

		private void numSmallX_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildSmallAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibSmallAvatar.AdjustedRect.X = (int)numSmallX.Value;
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.BuildImage();
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
			}
		}

		private void numSmallY_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildSmallAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibSmallAvatar.AdjustedRect.Y = (int)numSmallY.Value;
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.BuildImage();
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
			}
		}

		private void numSmallWidth_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildSmallAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibSmallAvatar.AdjustedRect.Width = (int)numSmallWidth.Value;
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.BuildImage();
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
			}
		}

		private void numSmallHeight_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildSmallAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibSmallAvatar.AdjustedRect.Height = (int)numSmallHeight.Value;
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.BuildImage();
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
			}
		}

		private void picPersonalitySmall_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bBuildSmallAvatar)
				m_ibSmallAvatar.MouseDownHandler(e);
		}

		private void picPersonalitySmall_MouseEnter(object sender, EventArgs e)
		{
			if (m_bBuildSmallAvatar)
			{
				// Take focus so that we can get the mouse wheel events.
				picPersonalitySmall.Focus();
			}
		}

		private void picPersonalitySmall_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bBuildSmallAvatar)
			{
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.MouseMoveHandler(e);
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
				// And update the location numeric up downs.
				lock (numSmallX)
				{
					m_bProgramaticAdjust = true;
					numSmallX.Value = m_ibSmallAvatar.AdjustedRect.X;
					numSmallY.Value = m_ibSmallAvatar.AdjustedRect.Y;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void picPersonalitySmall_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bBuildSmallAvatar)
				m_ibSmallAvatar.MouseUpHandler(e);

			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
			{
				m_picContextPicBox = picPersonalitySmall;
				if (m_picContextPicBox.Image != null)
					cmnuiExport.Enabled = true;
				else
					cmnuiExport.Enabled = false;
				cmnuPictures.Show(Cursor.Position);
			}
		}

		private void picPersonalitySmall_MouseWheel(object sender, MouseEventArgs e)
		{
			if (m_bBuildSmallAvatar)
			{
				m_apPersonality.SmallAvatarImage = m_ibSmallAvatar.MouseWheelHandler(e);
				picPersonalitySmall.Image = m_apPersonality.SmallAvatarImage;
				lock (numSmallX)
				{
					m_bProgramaticAdjust = true;
					numSmallX.Value = m_ibSmallAvatar.AdjustedRect.X;
					numSmallY.Value = m_ibSmallAvatar.AdjustedRect.Y;
					numSmallWidth.Value = m_ibSmallAvatar.AdjustedRect.Width;
					numSmallHeight.Value = m_ibSmallAvatar.AdjustedRect.Height;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void rbUseThisImageLarge_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbUseThisImageLarge.Checked) && (!m_bChangingToLast))
			{
				if (m_apPersonality.LobbyImage != null)
					picPersonalityLarge.Image = m_apPersonality.LobbyImage;
				else
					picPersonalityLarge.Image = null;
				m_bBuildLargeAvatar = false;
				m_rbLargeAvatarLastChecked = rbUseThisImageLarge;
			}
		}

		private void rbLoadImageLarge_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbLoadImageLarge.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_apPersonality.LobbyImage = bmpChosen;
					picPersonalityLarge.Image = m_apPersonality.LobbyImage;
					m_rbLargeAvatarLastChecked = rbLoadImageLarge;
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbLargeAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
		}

		private void rbBuildImageLarge_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbBuildImageLarge.Checked) && (!m_bChangingToLast))
			{
				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_ibLargeAvatar.LoadedImage = bmpChosen;
					InitialLargeAvatarAdjust();
					m_apPersonality.LobbyImage = m_ibLargeAvatar.BuildImage();
					picPersonalityLarge.Image = m_apPersonality.LobbyImage;

					// We've successfully built an initial image so go ahead and enable our controls.
					m_rbLargeAvatarLastChecked = rbBuildImageLarge;
					m_bBuildLargeAvatar = true;
					Tools.ChangeControlEnabled(scLargeLocationSize, true);
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbLargeAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
			else if ((m_bChangingToLast) && (m_rbLargeAvatarLastChecked == rbBuildImageLarge))
			{
				// We were the last checked so we go back to here and re-enable build mode.
				m_bBuildLargeAvatar = true;
				Tools.ChangeControlEnabled(scLargeLocationSize, true);
			}
			else
			{
				// Disable our controls.
				m_bBuildLargeAvatar = false;
				Tools.ChangeControlEnabled(scLargeLocationSize, false);
			}
		}

		private void InitialLargeAvatarAdjust()
		{
			m_ibLargeAvatar.InitialImageAdjust();

			// Update our controls (without screwing this up)
			m_bProgramaticAdjust = true;
			numLargeX.Value = m_ibLargeAvatar.AdjustedRect.X;
			numLargeY.Value = m_ibLargeAvatar.AdjustedRect.Y;
			numLargeWidth.Value = m_ibLargeAvatar.AdjustedRect.Width;
			numLargeHeight.Value = m_ibLargeAvatar.AdjustedRect.Height;
			m_bProgramaticAdjust = false;
		}

		private void numLargeX_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildLargeAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibLargeAvatar.AdjustedRect.X = (int)numLargeX.Value;
				m_apPersonality.LobbyImage = m_ibLargeAvatar.BuildImage();
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
			}
		}

		private void numLargeY_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildLargeAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibLargeAvatar.AdjustedRect.Y = (int)numLargeY.Value;
				m_apPersonality.LobbyImage = m_ibLargeAvatar.BuildImage();
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
			}
		}

		private void numLargeWidth_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildLargeAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibLargeAvatar.AdjustedRect.Width = (int)numLargeWidth.Value;
				m_apPersonality.LobbyImage = m_ibLargeAvatar.BuildImage();
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
			}
		}

		private void numLargeHeight_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildLargeAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibLargeAvatar.AdjustedRect.Height = (int)numLargeHeight.Value;
				m_apPersonality.LobbyImage = m_ibLargeAvatar.BuildImage();
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
			}
		}

		private void picPersonalityLarge_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bBuildLargeAvatar)
				m_ibLargeAvatar.MouseDownHandler(e);
		}

		private void picPersonalityLarge_MouseEnter(object sender, EventArgs e)
		{
			if (m_bBuildLargeAvatar)
			{
				// Take focus so that we can get the mouse wheel events.
				picPersonalityLarge.Focus();
			}
		}

		private void picPersonalityLarge_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bBuildLargeAvatar)
			{
				m_apPersonality.LobbyImage = m_ibLargeAvatar.MouseMoveHandler(e);
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
				// And update the location numeric up downs.
				lock (numLargeX)
				{
					m_bProgramaticAdjust = true;
					numLargeX.Value = m_ibLargeAvatar.AdjustedRect.X;
					numLargeY.Value = m_ibLargeAvatar.AdjustedRect.Y;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void picPersonalityLarge_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bBuildLargeAvatar)
				m_ibLargeAvatar.MouseUpHandler(e);

			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
			{
				m_picContextPicBox = picPersonalityLarge;
				if (m_picContextPicBox.Image != null)
					cmnuiExport.Enabled = true;
				else
					cmnuiExport.Enabled = false;
				cmnuPictures.Show(Cursor.Position);
			}
		}

		private void picPersonalityLarge_MouseWheel(object sender, MouseEventArgs e)
		{
			if (m_bBuildLargeAvatar)
			{
				m_apPersonality.LobbyImage = m_ibLargeAvatar.MouseWheelHandler(e);
				picPersonalityLarge.Image = m_apPersonality.LobbyImage;
				lock (numLargeX)
				{
					m_bProgramaticAdjust = true;
					numLargeX.Value = m_ibLargeAvatar.AdjustedRect.X;
					numLargeY.Value = m_ibLargeAvatar.AdjustedRect.Y;
					numLargeWidth.Value = m_ibLargeAvatar.AdjustedRect.Width;
					numLargeHeight.Value = m_ibLargeAvatar.AdjustedRect.Height;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void cmnuiExportPng_Click(object sender, EventArgs e)
		{
			if ((m_picContextPicBox != null) && (m_picContextPicBox.Image != null))
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png");
				if (strFilename.Length > 0)
					m_picContextPicBox.Image.Save(strFilename, System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		private void cmnuiExportTdx_Click(object sender, EventArgs e)
		{
			if ((m_picContextPicBox != null) && (m_picContextPicBox.Image != null))
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX");
				if (strFilename.Length > 0)
				{
					TdxWrapper twImage = new TdxWrapper();
					if (m_picContextPicBox == picPersonalityFull)
						twImage.LoadImage(m_picContextPicBox.Image, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT1, Settings.GetSetting("IncludeMipMaps", true));
					else
						twImage.LoadImage(m_picContextPicBox.Image, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT5, Settings.GetSetting("IncludeMipMaps", true));
					twImage.Save(strFilename);
					twImage.Dispose();
				}
			}
		}

		private void rbUseThisImageFull_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbUseThisImageFull.Checked) && (!m_bChangingToLast))
			{
				// Due to putting the controls in a split container I need to manually toggle off the other controls.
				rbLoadImageFull.Checked = false;
				rbBuildImageFull.Checked = false;

				if (m_apPersonality.LargeAvatarImage != null)
					picPersonalityFull.Image = new Bitmap(m_apPersonality.LargeAvatarImage);
				else
					picPersonalityFull.Image = null;
				m_bBuildFullAvatar = false;
				m_rbFullAvatarLastChecked = rbUseThisImageFull;
			}
		}

		private void rbLoadImageFull_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbLoadImageFull.Checked) && (!m_bChangingToLast))
			{
				// Due to putting the controls in a split container I need to manually toggle off the other controls.
				rbUseThisImageFull.Checked = false;
				rbBuildImageFull.Checked = false;

				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_apPersonality.LargeAvatarImage = bmpChosen;
					picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
					m_rbFullAvatarLastChecked = rbLoadImageFull;
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbFullAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
		}

		private void rbBuildImageFull_CheckedChanged(object sender, EventArgs e)
		{
			if ((rbBuildImageFull.Checked) && (!m_bChangingToLast))
			{
				// Due to putting the controls in a split container I need to manually toggle off the other controls.
				rbUseThisImageFull.Checked = false;
				rbLoadImageFull.Checked = false;

				Bitmap bmpChosen = ChooseImage();
				if (bmpChosen != null)
				{
					PrepPersonalityForEditing();
					m_ibFullAvatar.LoadedImage = bmpChosen;
					InitialFullAvatarAdjust();
					m_apPersonality.LargeAvatarImage = m_ibFullAvatar.BuildImage();
					picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;

					// We've successfully built an initial image so go ahead and enable our controls.
					m_rbFullAvatarLastChecked = rbBuildImageFull;
					m_bBuildFullAvatar = true;
					Tools.ChangeControlEnabled(scFullLocationSize, true);
				}
				else
				{
					// They either canceled or picked a bad file, either way we go back to where we were last.
					m_bChangingToLast = true;
					m_rbFullAvatarLastChecked.Checked = true;
					m_bChangingToLast = false;
				}
			}
			else if ((m_bChangingToLast) && (m_rbFullAvatarLastChecked == rbBuildImageFull))
			{
				// We were the last checked so we go back to here and re-enable build mode.
				m_bBuildFullAvatar = true;
				Tools.ChangeControlEnabled(scFullLocationSize, true);
			}
			else
			{
				// Disable our controls.
				m_bBuildFullAvatar = false;
				Tools.ChangeControlEnabled(scFullLocationSize, false);
			}
		}

		private void InitialFullAvatarAdjust()
		{
			m_ibFullAvatar.InitialImageAdjust();

			// Update our controls (without screwing this up)
			m_bProgramaticAdjust = true;
			numFullX.Value = m_ibFullAvatar.AdjustedRect.X;
			numFullY.Value = m_ibFullAvatar.AdjustedRect.Y;
			numFullWidth.Value = m_ibFullAvatar.AdjustedRect.Width;
			numFullHeight.Value = m_ibFullAvatar.AdjustedRect.Height;
			m_bProgramaticAdjust = false;
		}

		private void numFullX_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildFullAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibFullAvatar.AdjustedRect.X = (int)numFullX.Value;
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.BuildImage();
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
			}
		}

		private void numFullY_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildFullAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibFullAvatar.AdjustedRect.Y = (int)numFullY.Value;
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.BuildImage();
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
			}
		}

		private void numFullWidth_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildFullAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibFullAvatar.AdjustedRect.Width = (int)numFullWidth.Value;
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.BuildImage();
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
			}
		}

		private void numFullHeight_ValueChanged(object sender, EventArgs e)
		{
			// We're not going to bother if we aren't building an image or we got here through programatic adjustment.
			if ((m_bBuildFullAvatar) && (!m_bProgramaticAdjust))
			{
				m_ibFullAvatar.AdjustedRect.Height = (int)numFullHeight.Value;
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.BuildImage();
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
			}
		}

		private void picPersonalityFull_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_bBuildFullAvatar)
				m_ibFullAvatar.MouseDownHandler(e);
		}

		private void picPersonalityFull_MouseEnter(object sender, EventArgs e)
		{
			if (m_bBuildFullAvatar)
			{
				// Take focus so that we can get the mouse wheel events.
				picPersonalityFull.Focus();
			}
		}

		private void picPersonalityFull_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bBuildFullAvatar)
			{
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.MouseMoveHandler(e);
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
				// And update the location numeric up downs.
				lock (numFullX)
				{
					m_bProgramaticAdjust = true;
					numFullX.Value = m_ibFullAvatar.AdjustedRect.X;
					numFullY.Value = m_ibFullAvatar.AdjustedRect.Y;
					m_bProgramaticAdjust = false;
				}
			}
		}

		private void picFull_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bBuildFullAvatar)
				m_ibFullAvatar.MouseUpHandler(e);

			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
			{
				m_picContextPicBox = picPersonalityFull;
				if (m_picContextPicBox.Image != null)
					cmnuiExport.Enabled = true;
				else
					cmnuiExport.Enabled = false;
				cmnuPictures.Show(Cursor.Position);
			}
		}

		private void picPersonalityFull_MouseWheel(object sender, MouseEventArgs e)
		{
			if (m_bBuildFullAvatar)
			{
				m_apPersonality.LargeAvatarImage = m_ibFullAvatar.MouseWheelHandler(e);
				picPersonalityFull.Image = m_apPersonality.LargeAvatarImage;
				lock (numFullX)
				{
					m_bProgramaticAdjust = true;
					numFullX.Value = m_ibFullAvatar.AdjustedRect.X;
					numFullY.Value = m_ibFullAvatar.AdjustedRect.Y;
					numFullWidth.Value = m_ibFullAvatar.AdjustedRect.Width;
					numFullHeight.Value = m_ibFullAvatar.AdjustedRect.Height;
					m_bProgramaticAdjust = false;
				}
			}
		}
	}
}
