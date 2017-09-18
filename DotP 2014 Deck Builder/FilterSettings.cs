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
	public partial class FilterSettings : Form
	{
		private Filters m_fltSettings;

		public FilterSettings()
		{
			InitializeComponent();

			LoadLocalizationStrings();

			Rectangle rcPosition = Settings.GetSetting("FilterSettingsPosition", new Rectangle(-1, -1, -1, -1));
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

			m_fltSettings = Settings.GetSerializableSetting("CardFilters", new Filters());

			// Now that we have restored our filter settings we need to populate the controls with those settings.
			//	Name
			chkName.Checked = m_fltSettings.DoTitleCheck;
			txtName.Text = m_fltSettings.Title;
			cboNameFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(m_fltSettings.TitleFilter, Settings.UIStrings[m_fltSettings.TitleFilter.ToString().ToUpper()]);

			// Colour
			chkBlack.Checked = ((m_fltSettings.Colour & ColourFlags.Black) == ColourFlags.Black);
			chkBlue.Checked = ((m_fltSettings.Colour & ColourFlags.Blue) == ColourFlags.Blue);
			chkGreen.Checked = ((m_fltSettings.Colour & ColourFlags.Green) == ColourFlags.Green);
			chkRed.Checked = ((m_fltSettings.Colour & ColourFlags.Red) == ColourFlags.Red);
			chkWhite.Checked = ((m_fltSettings.Colour & ColourFlags.White) == ColourFlags.White);
			chkColourless.Checked = ((m_fltSettings.Colour & ColourFlags.Colourless) == ColourFlags.Colourless);
			chkMultiColoured.Checked = ((m_fltSettings.Colour & ColourFlags.MultiColour) == ColourFlags.MultiColour);
			cboColourFilter.SelectedItem = new KeyValuePair<FilterType, string>(m_fltSettings.ColourFilter, Settings.UIStrings[m_fltSettings.ColourFilter.ToString().ToUpper()]);

			// Card Types
			chkArtifact.Checked = ((m_fltSettings.Type & CardType.Artifact) == CardType.Artifact);
			chkCreature.Checked = ((m_fltSettings.Type & CardType.Creature) == CardType.Creature);
			chkEnchantment.Checked = ((m_fltSettings.Type & CardType.Enchantment) == CardType.Enchantment);
			chkInstant.Checked = ((m_fltSettings.Type & CardType.Instant) == CardType.Instant);
			chkLand.Checked = ((m_fltSettings.Type & CardType.Land) == CardType.Land);
			chkPhenomenon.Checked = ((m_fltSettings.Type & CardType.Phenomenon) == CardType.Phenomenon);
			chkPlane.Checked = ((m_fltSettings.Type & CardType.Plane) == CardType.Plane);
			chkPlaneswalker.Checked = ((m_fltSettings.Type & CardType.Planeswalker) == CardType.Planeswalker);
			chkScheme.Checked = ((m_fltSettings.Type & CardType.Scheme) == CardType.Scheme);
			chkSorcery.Checked = ((m_fltSettings.Type & CardType.Sorcery) == CardType.Sorcery);
			chkTribal.Checked = ((m_fltSettings.Type & CardType.Tribal) == CardType.Tribal);
			cboTypeFilter.SelectedItem = new KeyValuePair<FilterType, string>(m_fltSettings.TypeFilter, Settings.UIStrings[m_fltSettings.TypeFilter.ToString().ToUpper()]);

			// Super Types
			chkBasic.Checked = m_fltSettings.AllowBasic;
			chkLegendary.Checked = m_fltSettings.AllowLegendary;
			chkSnow.Checked = m_fltSettings.AllowSnow;
			chkWorld.Checked = m_fltSettings.AllowWorld;

			// Sub Types
			chkCheckSubTypes.Checked = m_fltSettings.DoSubTypeCheck;
			txtSubType.Text = m_fltSettings.SubType;
			cboSubTypeFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(m_fltSettings.SubTypeFilter, Settings.UIStrings[m_fltSettings.SubTypeFilter.ToString().ToUpper()]);

			// Converted Mana Cost
			numCmc.Value = m_fltSettings.Cmc;
			cboCmcFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(m_fltSettings.CmcFilter, Settings.UIStrings[m_fltSettings.CmcFilter.ToString().ToUpper()]);

			// Power/Toughness
			chkCheckPowerToughness.Checked = m_fltSettings.DoPowerToughnessCheck;
			txtPower.Text = m_fltSettings.Power;
			cboPowerFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(m_fltSettings.PowerFilter, Settings.UIStrings[m_fltSettings.PowerFilter.ToString().ToUpper()]);
			txtToughness.Text = m_fltSettings.Toughness;
			cboToughnessFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(m_fltSettings.ToughnessFilter, Settings.UIStrings[m_fltSettings.ToughnessFilter.ToString().ToUpper()]);

			// Rarity
			chkRarityToken.Checked = ((m_fltSettings.Rarity & CardRarity.Token) == CardRarity.Token);
			chkRarityLand.Checked = ((m_fltSettings.Rarity & CardRarity.Land) == CardRarity.Land);
			chkRarityCommon.Checked = ((m_fltSettings.Rarity & CardRarity.Common) == CardRarity.Common);
			chkRarityUncommon.Checked = ((m_fltSettings.Rarity & CardRarity.Uncommon) == CardRarity.Uncommon);
			chkRarityRare.Checked = ((m_fltSettings.Rarity & CardRarity.Rare) == CardRarity.Rare);
			chkRarityMythic.Checked = ((m_fltSettings.Rarity & CardRarity.Mythic) == CardRarity.Mythic);
			chkRaritySpecial.Checked = ((m_fltSettings.Rarity & CardRarity.Special) == CardRarity.Special);

			// Abilities
			chkCheckAbilities.Checked = m_fltSettings.DoAbilityCheck;
			txtAbilities.Text = m_fltSettings.Ability;
			cboAbilitiesFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(m_fltSettings.AbilityFilter, Settings.UIStrings[m_fltSettings.AbilityFilter.ToString().ToUpper()]);
			chkCheckGrantedAbilities.Checked = m_fltSettings.CheckGrantedAbilities;

			// Extras
			chkTokens.Checked = m_fltSettings.AllowToken;
			chkCardsThatCreateTokens.Checked = m_fltSettings.AllowCreateTokens;
			chkRegularMana.Checked = m_fltSettings.AllowManaRegular;
			chkHybridMana.Checked = m_fltSettings.AllowManaHybrid;
			chkPhyrexianMana.Checked = m_fltSettings.AllowManaPhyrexian;

			chkName_CheckedChanged(null, null);
			chkCheckSubTypes_CheckedChanged(null, null);
			chkCheckPowerToughness_CheckedChanged(null, null);
			chkCheckAbilities_CheckedChanged(null, null);
		}

		private void LoadLocalizationStrings()
		{
			// Window
			this.Text = Settings.UIStrings[(string)this.Tag];

			// Name
			gbName.Text = Settings.UIStrings[(string)gbName.Tag];
			chkName.Text = Settings.UIStrings[(string)chkName.Tag];
			LoadStringComparisonTypes(cboNameFilter);

			// Colour
			gbColour.Text = Settings.UIStrings[(string)gbColour.Tag];
			chkBlack.Text = Settings.UIStrings[(string)chkBlack.Tag];
			chkBlue.Text = Settings.UIStrings[(string)chkBlue.Tag];
			chkGreen.Text = Settings.UIStrings[(string)chkGreen.Tag];
			chkRed.Text = Settings.UIStrings[(string)chkRed.Tag];
			chkWhite.Text = Settings.UIStrings[(string)chkWhite.Tag];
			chkColourless.Text = Settings.UIStrings[(string)chkColourless.Tag];
			chkMultiColoured.Text = Settings.UIStrings[(string)chkMultiColoured.Tag];
			LoadFilterTypeStrings(cboColourFilter);

			// Card Types
			gbTypes.Text = Settings.UIStrings[(string)gbTypes.Tag];
			chkArtifact.Text = Settings.UIStrings[(string)chkArtifact.Tag];
			chkCreature.Text = Settings.UIStrings[(string)chkCreature.Tag];
			chkEnchantment.Text = Settings.UIStrings[(string)chkEnchantment.Tag];
			chkInstant.Text = Settings.UIStrings[(string)chkInstant.Tag];
			chkLand.Text = Settings.UIStrings[(string)chkLand.Tag];
			chkPhenomenon.Text = Settings.UIStrings[(string)chkPhenomenon.Tag];
			chkPlane.Text = Settings.UIStrings[(string)chkPlane.Tag];
			chkPlaneswalker.Text = Settings.UIStrings[(string)chkPlaneswalker.Tag];
			chkScheme.Text = Settings.UIStrings[(string)chkScheme.Tag];
			chkSorcery.Text = Settings.UIStrings[(string)chkSorcery.Tag];
			chkTribal.Text = Settings.UIStrings[(string)chkTribal.Tag];
			chkVanguard.Text = Settings.UIStrings[(string)chkVanguard.Tag];
			LoadFilterTypeStrings(cboTypeFilter);

			// Super Types
			gbSupertype.Text = Settings.UIStrings[(string)gbSupertype.Tag];
			chkBasic.Text = Settings.UIStrings[(string)chkBasic.Tag];
			chkLegendary.Text = Settings.UIStrings[(string)chkLegendary.Tag];
			chkSnow.Text = Settings.UIStrings[(string)chkSnow.Tag];
			chkWorld.Text = Settings.UIStrings[(string)chkWorld.Tag];

			// Sub Types
			gbSubTypes.Text = Settings.UIStrings[(string)gbSubTypes.Tag];
			chkCheckSubTypes.Text = Settings.UIStrings[(string)chkCheckSubTypes.Tag];
			LoadStringComparisonTypes(cboSubTypeFilter);

			// Converted Mana Cost
			gbCmc.Text = Settings.UIStrings[(string)gbCmc.Tag];
			LoadIntComparisonTypes(cboCmcFilter);

			// Power/Toughness
			gbPowerToughness.Text = Settings.UIStrings[(string)gbPowerToughness.Tag];
			chkCheckPowerToughness.Text = Settings.UIStrings[(string)chkCheckPowerToughness.Tag];
			LoadIntComparisonTypes(cboPowerFilter);
			LoadIntComparisonTypes(cboToughnessFilter);

			// Rarity
			gbRarity.Text = Settings.UIStrings[(string)gbRarity.Tag];
			chkRarityToken.Text = Settings.UIStrings[(string)chkRarityToken.Tag];
			chkRarityLand.Text = Settings.UIStrings[(string)chkRarityLand.Tag];
			chkRarityCommon.Text = Settings.UIStrings[(string)chkRarityCommon.Tag];
			chkRarityUncommon.Text = Settings.UIStrings[(string)chkRarityUncommon.Tag];
			chkRarityRare.Text = Settings.UIStrings[(string)chkRarityRare.Tag];
			chkRarityMythic.Text = Settings.UIStrings[(string)chkRarityMythic.Tag];
			chkRaritySpecial.Text = Settings.UIStrings[(string)chkRaritySpecial.Tag];

			// Abilities
			gbAbilities.Text = Settings.UIStrings[(string)gbAbilities.Tag];
			chkCheckAbilities.Text = Settings.UIStrings[(string)chkCheckAbilities.Tag];
			chkCheckGrantedAbilities.Text = Settings.UIStrings[(string)chkCheckGrantedAbilities.Tag];
			LoadStringComparisonTypes(cboAbilitiesFilter);

			// Extra Filters
			gbExtra.Text = Settings.UIStrings[(string)gbExtra.Tag];
			chkTokens.Text = Settings.UIStrings[(string)chkTokens.Tag];
			chkCardsThatCreateTokens.Text = Settings.UIStrings[(string)chkCardsThatCreateTokens.Tag];
			chkRegularMana.Text = Settings.UIStrings[(string)chkRegularMana.Tag];
			chkHybridMana.Text = Settings.UIStrings[(string)chkHybridMana.Tag];
			chkPhyrexianMana.Text = Settings.UIStrings[(string)chkPhyrexianMana.Tag];

			// Apply & Cancel
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];
		}

		private void LoadStringComparisonTypes(ComboBox cboList)
		{
			cboList.Items.Clear();
			cboList.DisplayMember = "Value";
			cboList.ValueMember = "Key";
			cboList.Items.Add(new KeyValuePair<FilterStringComparisonType, string>(FilterStringComparisonType.Contains, Settings.UIStrings[(string)FilterStringComparisonType.Contains.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterStringComparisonType, string>(FilterStringComparisonType.DoesNotContain, Settings.UIStrings[(string)FilterStringComparisonType.DoesNotContain.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterStringComparisonType, string>(FilterStringComparisonType.Equal, Settings.UIStrings[(string)FilterStringComparisonType.Equal.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterStringComparisonType, string>(FilterStringComparisonType.NotEqual, Settings.UIStrings[(string)FilterStringComparisonType.NotEqual.ToString().ToUpper()]));
		}

		private void LoadIntComparisonTypes(ComboBox cboList)
		{
			cboList.Items.Clear();
			cboList.DisplayMember = "Value";
			cboList.ValueMember = "Key";
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.Equal, Settings.UIStrings[(string)FilterIntComparisonType.Equal.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.GreaterThan, Settings.UIStrings[(string)FilterIntComparisonType.GreaterThan.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.GreaterThanOrEqualTo, Settings.UIStrings[(string)FilterIntComparisonType.GreaterThanOrEqualTo.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.LessThan, Settings.UIStrings[(string)FilterIntComparisonType.LessThan.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.LessThanOrEqualTo, Settings.UIStrings[(string)FilterIntComparisonType.LessThanOrEqualTo.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(FilterIntComparisonType.NotEqual, Settings.UIStrings[(string)FilterIntComparisonType.NotEqual.ToString().ToUpper()]));
		}

		private void LoadFilterTypeStrings(ComboBox cboList)
		{
			cboList.Items.Clear();
			cboList.DisplayMember = "Value";
			cboList.ValueMember = "Key";
			cboList.Items.Add(new KeyValuePair<FilterType, string>(FilterType.Allow, Settings.UIStrings[(string)FilterType.Allow.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterType, string>(FilterType.AllowAny, Settings.UIStrings[(string)FilterType.AllowAny.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterType, string>(FilterType.ExactMatch, Settings.UIStrings[(string)FilterType.ExactMatch.ToString().ToUpper()]));
			cboList.Items.Add(new KeyValuePair<FilterType, string>(FilterType.Exclude, Settings.UIStrings[(string)FilterType.Exclude.ToString().ToUpper()]));
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			// Save the selected setting to the Filters object.
			//	Name
			m_fltSettings.DoTitleCheck = chkName.Checked;
			m_fltSettings.Title = txtName.Text.Trim();
			if (cboNameFilter.SelectedIndex > -1)
				m_fltSettings.TitleFilter = ((KeyValuePair<FilterStringComparisonType, string>)cboNameFilter.SelectedItem).Key;

			// Colour
			m_fltSettings.Colour = (chkBlack.Checked ? ColourFlags.Black : 0);
			m_fltSettings.Colour |= (chkBlue.Checked ? ColourFlags.Blue : 0);
			m_fltSettings.Colour |= (chkGreen.Checked ? ColourFlags.Green : 0);
			m_fltSettings.Colour |= (chkRed.Checked ? ColourFlags.Red : 0);
			m_fltSettings.Colour |= (chkWhite.Checked ? ColourFlags.White : 0);
			m_fltSettings.Colour |= (chkColourless.Checked ? ColourFlags.Colourless : 0);
			m_fltSettings.Colour |= (chkMultiColoured.Checked ? ColourFlags.MultiColour : 0);
			if (cboColourFilter.SelectedIndex > -1)
				m_fltSettings.ColourFilter = ((KeyValuePair<FilterType, string>)cboColourFilter.SelectedItem).Key;

			// Types
			m_fltSettings.Type = (chkArtifact.Checked ? CardType.Artifact : 0);
			m_fltSettings.Type |= (chkCreature.Checked ? CardType.Creature : 0);
			m_fltSettings.Type |= (chkEnchantment.Checked ? CardType.Enchantment : 0);
			m_fltSettings.Type |= (chkInstant.Checked ? CardType.Instant : 0);
			m_fltSettings.Type |= (chkLand.Checked ? CardType.Land : 0);
			m_fltSettings.Type |= (chkPhenomenon.Checked ? CardType.Phenomenon : 0);
			m_fltSettings.Type |= (chkPlane.Checked ? CardType.Plane : 0);
			m_fltSettings.Type |= (chkPlaneswalker.Checked ? CardType.Planeswalker : 0);
			m_fltSettings.Type |= (chkScheme.Checked ? CardType.Scheme : 0);
			m_fltSettings.Type |= (chkSorcery.Checked ? CardType.Sorcery : 0);
			m_fltSettings.Type |= (chkTribal.Checked ? CardType.Tribal : 0);
			if (cboTypeFilter.SelectedIndex > -1)
				m_fltSettings.TypeFilter = ((KeyValuePair<FilterType, string>)cboTypeFilter.SelectedItem).Key;

			// Super Types
			m_fltSettings.AllowBasic = chkBasic.Checked;
			m_fltSettings.AllowLegendary = chkLegendary.Checked;
			m_fltSettings.AllowSnow = chkSnow.Checked;
			m_fltSettings.AllowWorld = chkWorld.Checked;

			// Sub Types
			m_fltSettings.DoSubTypeCheck = chkCheckSubTypes.Checked;
			m_fltSettings.SubType = txtSubType.Text.Trim();
			if (cboSubTypeFilter.SelectedIndex > -1)
				m_fltSettings.SubTypeFilter = ((KeyValuePair<FilterStringComparisonType, string>)cboSubTypeFilter.SelectedItem).Key;

			// Converted Mana Cost
			m_fltSettings.Cmc = (int)numCmc.Value;
			if (cboCmcFilter.SelectedIndex > -1)
				m_fltSettings.CmcFilter = ((KeyValuePair<FilterIntComparisonType, string>)cboCmcFilter.SelectedItem).Key;

			// Power/Toughness
			m_fltSettings.DoPowerToughnessCheck = chkCheckPowerToughness.Checked;
			m_fltSettings.Power = txtPower.Text.Trim();
			if (cboPowerFilter.SelectedIndex > -1)
				m_fltSettings.PowerFilter = ((KeyValuePair<FilterIntComparisonType, string>)cboPowerFilter.SelectedItem).Key;
			m_fltSettings.Toughness = txtToughness.Text.Trim();
			if (cboToughnessFilter.SelectedIndex > -1)
				m_fltSettings.ToughnessFilter = ((KeyValuePair<FilterIntComparisonType, string>)cboToughnessFilter.SelectedItem).Key;

			// Rarity
			m_fltSettings.Rarity = (chkRarityToken.Checked ? CardRarity.Token : 0);
			m_fltSettings.Rarity |= (chkRarityLand.Checked ? CardRarity.Land : 0);
			m_fltSettings.Rarity |= (chkRarityCommon.Checked ? CardRarity.Common : 0);
			m_fltSettings.Rarity |= (chkRarityUncommon.Checked ? CardRarity.Uncommon : 0);
			m_fltSettings.Rarity |= (chkRarityRare.Checked ? CardRarity.Rare : 0);
			m_fltSettings.Rarity |= (chkRarityMythic.Checked ? CardRarity.Mythic : 0);
			m_fltSettings.Rarity |= (chkRaritySpecial.Checked ? CardRarity.Special : 0);

			// Abilities
			m_fltSettings.DoAbilityCheck = chkCheckAbilities.Checked;
			m_fltSettings.Ability = txtAbilities.Text.Trim();
			if (cboAbilitiesFilter.SelectedIndex > -1)
				m_fltSettings.AbilityFilter = ((KeyValuePair<FilterStringComparisonType, string>)cboAbilitiesFilter.SelectedItem).Key;
			m_fltSettings.CheckGrantedAbilities = chkCheckGrantedAbilities.Checked;

			// Extras
			m_fltSettings.AllowToken = chkTokens.Checked;
			m_fltSettings.AllowCreateTokens = chkCardsThatCreateTokens.Checked;
			m_fltSettings.AllowManaRegular = chkRegularMana.Checked;
			m_fltSettings.AllowManaHybrid = chkHybridMana.Checked;
			m_fltSettings.AllowManaPhyrexian = chkPhyrexianMana.Checked;

			// Now that we have all of the filters back in the object save it.
			Settings.SaveSerializableSetting("CardFilters", m_fltSettings);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void FilterSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("FilterSettingsPosition", new Rectangle(this.Location, this.Size));
		}

		private void chkName_CheckedChanged(object sender, EventArgs e)
		{
			txtName.Enabled = chkName.Checked;
			cboNameFilter.Enabled = chkName.Checked;
		}

		private void chkCheckSubTypes_CheckedChanged(object sender, EventArgs e)
		{
			txtSubType.Enabled = chkCheckSubTypes.Checked;
			cboSubTypeFilter.Enabled = chkCheckSubTypes.Checked;
		}

		private void chkCheckPowerToughness_CheckedChanged(object sender, EventArgs e)
		{
			txtPower.Enabled = chkCheckPowerToughness.Checked;
			cboPowerFilter.Enabled = chkCheckPowerToughness.Checked;
			txtToughness.Enabled = chkCheckPowerToughness.Checked;
			cboToughnessFilter.Enabled = chkCheckPowerToughness.Checked;
		}

		private void chkCheckAbilities_CheckedChanged(object sender, EventArgs e)
		{
			txtAbilities.Enabled = chkCheckAbilities.Checked;
			cboAbilitiesFilter.Enabled = chkCheckAbilities.Checked;
			chkCheckGrantedAbilities.Enabled = chkCheckAbilities.Checked;
		}

		// Special processing for Power/Toughness to allow only numbers and "*"
		private void txtPower_Leave(object sender, EventArgs e)
		{
			// Allow blank.
			if (txtPower.Text.Length > 0)
			{
				// Allow "*"
				if (!txtPower.Text.Trim().Equals("*"))
				{
					int nValue;
					if (!Int32.TryParse(txtPower.Text.Trim(), out nValue))
						txtPower.Text = "0";
				}
			}
			else
				txtPower.Text = "0";
		}

		private void txtToughness_Leave(object sender, EventArgs e)
		{
			// Allow blank.
			if (txtToughness.Text.Length > 0)
			{
				// Allow "*"
				if (!txtToughness.Text.Trim().Equals("*"))
				{
					int nValue;
					if (!Int32.TryParse(txtToughness.Text.Trim(), out nValue))
						txtToughness.Text = "0";
				}
			}
			else
				txtToughness.Text = "0";
		}
	}
}
