using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class EditString : Form
	{
		private Dictionary<string, string> m_dicString;
		private Label[] m_alblLabels;
		private TextBox[] m_atxtTextBoxes;

		public EditString(Dictionary<string,string> dicString, string strTitle)
		{
			InitializeComponent();

			Rectangle rcPosition = Settings.GetSetting("StringEditPosition", new Rectangle(-1, -1, -1, -1));
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

			// Set our variables with the data we need.
			this.Text = strTitle;
			m_dicString = dicString;

			// Now we need to create our controls.
			m_alblLabels = new Label[Settings.Languages.Count];
			m_atxtTextBoxes = new TextBox[Settings.Languages.Count];
			int nIndex = 0;
			foreach (KeyValuePair<string, LanguageEntry> kvLang in Settings.Languages)
			{
				m_alblLabels[nIndex] = new Label();
				m_alblLabels[nIndex].Text = kvLang.Value.Text + ":";
				m_alblLabels[nIndex].Location = new Point(12, 15 + ((m_alblLabels[nIndex].Size.Height + 3) * nIndex));
				// I could set them to autosize, but in this case I think the screen looks better if I don't
				//m_alblLabels[nIndex].AutoSize = true;
				this.Controls.Add(m_alblLabels[nIndex]);

				m_atxtTextBoxes[nIndex] = new TextBox();
				m_atxtTextBoxes[nIndex].AcceptsReturn = false;
				m_atxtTextBoxes[nIndex].AcceptsTab = false;
				m_atxtTextBoxes[nIndex].Location = new Point(m_alblLabels[nIndex].Location.X + m_alblLabels[nIndex].Size.Width, m_alblLabels[nIndex].Location.Y - 3);
				m_atxtTextBoxes[nIndex].Size = new Size(this.ClientSize.Width - (12 + m_atxtTextBoxes[nIndex].Location.X), m_atxtTextBoxes[nIndex].Size.Height);
				m_atxtTextBoxes[nIndex].Multiline = false;
				m_atxtTextBoxes[nIndex].TabIndex = nIndex;
				m_atxtTextBoxes[nIndex].TabStop = true;
				m_atxtTextBoxes[nIndex].Tag = kvLang.Key;	// So I can easily figure out which one is which later.
				m_atxtTextBoxes[nIndex].TextAlign = HorizontalAlignment.Left;
				this.Controls.Add(m_atxtTextBoxes[nIndex]);

				// Might as well go ahead and populate the text boxes with their text while I'm here.
				if (m_dicString.ContainsKey(kvLang.Key))
					m_atxtTextBoxes[nIndex].Text = m_dicString[kvLang.Key];

				nIndex++;
			}
			if (Settings.Languages.Count > 0)
			{
				Point ptLastText = m_atxtTextBoxes[Settings.Languages.Count - 1].Location;
				Size szLastText = m_atxtTextBoxes[Settings.Languages.Count - 1].Size;
				int nHeightAdjust = (ptLastText.Y + szLastText.Height + 6) - cmdApply.Location.Y;
				this.Size = new Size(this.Size.Width, this.Size.Height + nHeightAdjust);
			}

			// Adjust tab indexes for Apply and Cancel
			cmdApply.TabIndex = nIndex;
			cmdCancel.TabIndex = nIndex + 1;

			// I have form sizing issues if I try to set this in the designer when coupled with a manual start position rather than from code.
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			// Save out the edited strings to the dictionary.
			foreach (TextBox txtBox in m_atxtTextBoxes)
			{
				if (m_dicString.ContainsKey((string)txtBox.Tag))
				{
					if (txtBox.Text.Trim().Length == 0)
						m_dicString.Remove((string)txtBox.Tag);
					else
						m_dicString[(string)txtBox.Tag] = txtBox.Text.Trim();
				}
				else
				{
					if (txtBox.Text.Trim().Length != 0)
						m_dicString.Add((string)txtBox.Tag, txtBox.Text.Trim());
				}
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void EditString_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("StringEditPosition", new Rectangle(this.Location, this.Size));
		}

        private void CmdClear_Click(object sender, EventArgs e)
        {
            foreach (TextBox txtBox in m_atxtTextBoxes)
            {
                txtBox.Text = "";
            }
        }
    }
}
