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
	public partial class IdBlockInput : Form
	{
		public int IdBlock;

		public IdBlockInput(int nDefBlock)
		{
			InitializeComponent();

			IdBlock = nDefBlock;
			numIdBlock.Value = IdBlock;

			LoadLocalizedStrings();
		}

		private void LoadLocalizedStrings()
		{
			this.Text = Settings.UIStrings[(string)this.Tag];
			lblIdBlockToGenerate.Text = Settings.UIStrings[(string)lblIdBlockToGenerate.Tag];
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			IdBlock = (int)numIdBlock.Value;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
