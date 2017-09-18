using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class ErrorReportWindow : Form
	{
		private MessageLog m_mlLog;
		private string m_strLog;

		public ErrorReportWindow(MessageLog mlLog)
		{
			InitializeComponent();

			Rectangle rcPosition = Settings.GetSetting("ErrorReportPosition", new Rectangle(-1, -1, -1, -1));
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

			m_mlLog = mlLog;
		}

		private void ErrorReportWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("ErrorReportPosition", new Rectangle(this.Location, this.Size));
		}

		private void ErrorReportWindow_Shown(object sender, EventArgs e)
		{
			m_strLog = m_mlLog.ReadToCurrent();
			ChangeFiltering(null, null);
		}

		private string MatchingErrors()
		{
			string strMatched = "";
			Regex rExp = new Regex(@"\b(.*?)(?:(\r\n){2,}|\r{2,}|\n{2,}|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
			MatchCollection mcCaught = rExp.Matches(m_strLog);

			if (mcCaught.Count > 0)
			{
				foreach (Match mCaught in mcCaught)
				{
					string strError = mCaught.Captures[0].Value;

					bool bShow = false;
					if (rbTypeFiltering.Checked)
					{
						bool bCardProblem = false;
						bool bDeckProblem = false;

						bCardProblem = ((strError.IndexOf("Invalid ability detected:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("has a multiverse id that is problematic:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("has a FILENAME tag that does not match", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to load card", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to read card", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("could not be loaded due to missing CARD_V2 block", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("could not be properly loaded due to missing or malformed FILENAME tag", StringComparison.OrdinalIgnoreCase) > -1));

						bDeckProblem = ((strError.IndexOf("File is missing DECK tag:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to load deck:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Can't find referenced card:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to load unlock file:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("uses a UID that conflicts with", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to load deck:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to read deck:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to find deck with id", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to load unlock file:", StringComparison.OrdinalIgnoreCase) > -1) ||
										(strError.IndexOf("Unable to read unlock file:", StringComparison.OrdinalIgnoreCase) > -1));

						bShow = ((chkCardProblems.Checked && bCardProblem) ||
								(chkDeckProblems.Checked && bDeckProblem) ||
								(chkOtherProblems.Checked && !(bCardProblem || bDeckProblem)));

						if (strError.IndexOf("Log Opened.", StringComparison.OrdinalIgnoreCase) > -1)
							bShow = false;
					}
					else if (rbSeverityFiltering.Checked)
					{
						bShow = (((chkCritical.Checked) && (strError.IndexOf(ErrorPriority.Critical.ToString() + ": ", StringComparison.OrdinalIgnoreCase) > -1)) ||
								((chkHigh.Checked) && (strError.IndexOf(ErrorPriority.High.ToString() + ": ", StringComparison.OrdinalIgnoreCase) > -1)) ||
								((chkMedium.Checked) && (strError.IndexOf(ErrorPriority.Medium.ToString() + ": ", StringComparison.OrdinalIgnoreCase) > -1)) ||
								((chkLow.Checked) && (strError.IndexOf(ErrorPriority.Low.ToString() + ": ", StringComparison.OrdinalIgnoreCase) > -1)) ||
								((chkZero.Checked) && (strError.IndexOf(ErrorPriority.Zero.ToString() + ": ", StringComparison.OrdinalIgnoreCase) > -1)));
					}
					else
						bShow = true;

					if (bShow)
						strMatched += strError;
				}
			}

			return strMatched;
		}

		private void ChangeFiltering(object sender, EventArgs e)
		{
			gbTypeFilters.Enabled = rbTypeFiltering.Checked;
			gbSeverityFiltering.Enabled = rbSeverityFiltering.Checked;

			UpdateFilter(sender, e);
		}

		private void UpdateFilter(object sender, EventArgs e)
		{
			txtErrors.Text = MatchingErrors();
		}
	}
}
