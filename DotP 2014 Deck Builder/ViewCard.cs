using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using RSN.Tools;

namespace RSN.DotP
{
	public partial class ViewCard : Form
	{
		public ViewCard(CardInfo ciCard)
		{
			InitializeComponent();

			Rectangle rcPosition = Settings.GetSetting("ViewCardPosition", new Rectangle(-1, -1, -1, -1));
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
			if (Enum.TryParse(Settings.GetSetting("ViewCardWindowState", FormWindowState.Normal.ToString()), out fwsState))
				this.WindowState = fwsState;

			// Load simple stuff.
			this.Text = ciCard.LocalizedName + " - " + ciCard.Filename;
			picCard.Image = ciCard.GetLocalizedPreviewImage();

			// Create a heirarchical view of the XML in a treeview.
			XmlDocument xdCard = new XmlDocument();
			xdCard.LoadXml(ciCard.CardXML);
			CreateXmlTree(xdCard, null, ciCard.Filename);

			// All cards should have both a document root and a CARD_V2 element so expand both of those by default.
			//	Document Root
			tvwXml.Nodes[0].Expand();
			//	CARD_V2 node
			tvwXml.Nodes[0].Nodes[0].Expand();

			// Show the full xml document.
			tvwXml_NodeMouseClick(tvwXml, new TreeNodeMouseClickEventArgs(tvwXml.Nodes[0], MouseButtons.None, 0, 0, 0));
		}

		private void ViewCard_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
				Settings.SaveSetting("ViewCardPosition", new Rectangle(this.Location, this.Size));
			if (this.WindowState != FormWindowState.Minimized)
				Settings.SaveSetting("ViewCardWindowState", this.WindowState.ToString());
		}

		// Create a full tree structure of the provided XmlNode within tnParent.
		//	If tnParent is null will put the root node in the treeview directly.
		private void CreateXmlTree(XmlNode xnNode, TreeNode tnParent, string strFilename)
		{
			TreeNode tnNode = null;

			if (xnNode is XmlDocument)
				tnNode = new TreeNode(strFilename);
			else if (xnNode is XmlElement)
				tnNode = new TreeNode(xnNode.Name);

			if (tnNode != null)
			{
				tnNode.Tag = xnNode;

				if (xnNode.HasChildNodes)
				{
					foreach (XmlNode xnChild in xnNode.ChildNodes)
						CreateXmlTree(xnChild, tnNode, strFilename);
				}

				if (tnParent != null)
					tnParent.Nodes.Add(tnNode);
				else
					tvwXml.Nodes.Add(tnNode);
			}
		}

		// This lets me get a semi-pretty XML string from a XmlNode object.
		private string WriteNodeXml(XmlNode xnNode)
		{
			string strReturn = string.Empty;

			StringWriter swText = new StringWriter();
			XmlTextWriter xtwWrite = new XmlTextWriter(swText);
			xtwWrite.Formatting = Formatting.Indented;
			xtwWrite.IndentChar = '\t';
			xtwWrite.Indentation = 1;
			xnNode.WriteTo(xtwWrite);
			strReturn = swText.ToString();
			xtwWrite.Close();
			swText.Close();

			return strReturn;
		}

		// This will show a formatted xml string in the textbox of just the selected XmlNode in the tree.
		private void tvwXml_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e != null)
			{
				if (e.Node != null)
				{
					XmlNode xnNode = e.Node.Tag as XmlNode;
					if (xnNode != null)
						txtCardXml.Text = WriteNodeXml(xnNode);
				}
			}
		}

		private void picCard_MouseUp(object sender, MouseEventArgs e)
		{
			// Check for if we need to display the context menu
			if (e.Button == MouseButtons.Right)
			{
				if (picCard.Image != null)
					cmnuiExport.Enabled = true;
				else
					cmnuiExport.Enabled = false;
				cmnuPictures.Show(Cursor.Position);
			}
		}

		private void cmnuiExportPng_Click(object sender, EventArgs e)
		{
			if (picCard.Image != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["PNG_FILTER"], ".png");
				if (strFilename.Length > 0)
					picCard.Image.Save(strFilename, System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		private void cmnuiExportTdx_Click(object sender, EventArgs e)
		{
			if (picCard.Image != null)
			{
				string strFilename = Tools.SaveDialog(Settings.UIStrings["IMAGE_SAVE_TITLE"], Settings.UIStrings["TDX_FILTER"], ".TDX");
				if (strFilename.Length > 0)
				{
					// The card preview we show here does not have any transparency so we have no problems saving as DXT1
					TdxWrapper twImage = new TdxWrapper();
					twImage.LoadImage(picCard.Image, Gibbed.Duels.FileFormats.Tdx.D3DFormat.DXT1, Settings.GetSetting("IncludeMipMaps", true));
					twImage.Save(strFilename);
					twImage.Dispose();
				}
			}
		}
	}
}
