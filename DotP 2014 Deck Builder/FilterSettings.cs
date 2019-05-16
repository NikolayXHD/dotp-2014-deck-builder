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
			LoadSettingsFromFilters(m_fltSettings);
		}

		private void LoadSettingsFromFilters(Filters filters)
		{
			//	Name
			chkName.Checked = filters.DoTitleCheck;
			txtName.Text = filters.Title;
			cboNameFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(filters.TitleFilter, Settings.UIStrings[filters.TitleFilter.ToString().ToUpper()]);

			// Colour
			chkBlack.Checked = ((filters.Colour & ColourFlags.Black) == ColourFlags.Black);
			chkBlue.Checked = ((filters.Colour & ColourFlags.Blue) == ColourFlags.Blue);
			chkGreen.Checked = ((filters.Colour & ColourFlags.Green) == ColourFlags.Green);
			chkRed.Checked = ((filters.Colour & ColourFlags.Red) == ColourFlags.Red);
			chkWhite.Checked = ((filters.Colour & ColourFlags.White) == ColourFlags.White);
			chkColourless.Checked = ((filters.Colour & ColourFlags.Colourless) == ColourFlags.Colourless);
			chkMultiColoured.Checked = ((filters.Colour & ColourFlags.MultiColour) == ColourFlags.MultiColour);
			cboColourFilter.SelectedItem = new KeyValuePair<FilterType, string>(filters.ColourFilter, Settings.UIStrings[filters.ColourFilter.ToString().ToUpper()]);

			// Card Types
			chkArtifact.Checked = ((filters.Type & CardType.Artifact) == CardType.Artifact);
			chkCreature.Checked = ((filters.Type & CardType.Creature) == CardType.Creature);
			chkEnchantment.Checked = ((filters.Type & CardType.Enchantment) == CardType.Enchantment);
			chkInstant.Checked = ((filters.Type & CardType.Instant) == CardType.Instant);
			chkLand.Checked = ((filters.Type & CardType.Land) == CardType.Land);
			chkPhenomenon.Checked = ((filters.Type & CardType.Phenomenon) == CardType.Phenomenon);
			chkPlane.Checked = ((filters.Type & CardType.Plane) == CardType.Plane);
			chkPlaneswalker.Checked = ((filters.Type & CardType.Planeswalker) == CardType.Planeswalker);
			chkScheme.Checked = ((filters.Type & CardType.Scheme) == CardType.Scheme);
			chkSorcery.Checked = ((filters.Type & CardType.Sorcery) == CardType.Sorcery);
			chkTribal.Checked = ((filters.Type & CardType.Tribal) == CardType.Tribal);
			chkVanguard.Checked = ((filters.Type & CardType.Vanguard) == CardType.Vanguard);
			cboTypeFilter.SelectedItem = new KeyValuePair<FilterType, string>(filters.TypeFilter, Settings.UIStrings[filters.TypeFilter.ToString().ToUpper()]);

			// Super Types
			chkBasic.Checked = filters.AllowBasic;
			chkLegendary.Checked = filters.AllowLegendary;
			chkSnow.Checked = filters.AllowSnow;
			chkWorld.Checked = filters.AllowWorld;

			// Sub Types
			chkCheckSubTypes.Checked = filters.DoSubTypeCheck;
			txtSubType.Text = filters.SubType;
			cboSubTypeFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(filters.SubTypeFilter, Settings.UIStrings[filters.SubTypeFilter.ToString().ToUpper()]);

			// Converted Mana Cost
			numCmc.Value = filters.Cmc;
			cboCmcFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(filters.CmcFilter, Settings.UIStrings[filters.CmcFilter.ToString().ToUpper()]);

			// Power/Toughness
			chkCheckPowerToughness.Checked = filters.DoPowerToughnessCheck;
			txtPower.Text = filters.Power;
			cboPowerFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(filters.PowerFilter, Settings.UIStrings[filters.PowerFilter.ToString().ToUpper()]);
			txtToughness.Text = filters.Toughness;
			cboToughnessFilter.SelectedItem = new KeyValuePair<FilterIntComparisonType, string>(filters.ToughnessFilter, Settings.UIStrings[filters.ToughnessFilter.ToString().ToUpper()]);

			// Rarity
			chkRarityToken.Checked = ((filters.Rarity & CardRarity.Token) == CardRarity.Token);
			chkRarityLand.Checked = ((filters.Rarity & CardRarity.Land) == CardRarity.Land);
			chkRarityCommon.Checked = ((filters.Rarity & CardRarity.Common) == CardRarity.Common);
			chkRarityUncommon.Checked = ((filters.Rarity & CardRarity.Uncommon) == CardRarity.Uncommon);
			chkRarityRare.Checked = ((filters.Rarity & CardRarity.Rare) == CardRarity.Rare);
			chkRarityMythic.Checked = ((filters.Rarity & CardRarity.Mythic) == CardRarity.Mythic);
			chkRaritySpecial.Checked = ((filters.Rarity & CardRarity.Special) == CardRarity.Special);

			// Abilities
			chkCheckAbilities.Checked = filters.DoAbilityCheck;
			txtAbilities.Text = filters.Ability;
			cboAbilitiesFilter.SelectedItem = new KeyValuePair<FilterStringComparisonType, string>(filters.AbilityFilter, Settings.UIStrings[filters.AbilityFilter.ToString().ToUpper()]);
			chkCheckGrantedAbilities.Checked = filters.CheckGrantedAbilities;

			// Extras
			chkTokens.Checked = filters.AllowToken;
			chkUnderscores.Checked = filters.AllowUnderscores;
			chkCardsThatCreateTokens.Checked = filters.AllowCreateTokens;
			chkRegularMana.Checked = filters.AllowManaRegular;
			chkHybridMana.Checked = filters.AllowManaHybrid;
			chkPhyrexianMana.Checked = filters.AllowManaPhyrexian;

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
			chkUnderscores.Text = Settings.UIStrings[(string)chkUnderscores.Tag];
			chkCardsThatCreateTokens.Text = Settings.UIStrings[(string)chkCardsThatCreateTokens.Tag];
			chkRegularMana.Text = Settings.UIStrings[(string)chkRegularMana.Tag];
			chkHybridMana.Text = Settings.UIStrings[(string)chkHybridMana.Tag];
			chkPhyrexianMana.Text = Settings.UIStrings[(string)chkPhyrexianMana.Tag];

			// Apply & Cancel
			cmdReset.Text = Settings.UIStrings[(string)cmdReset.Tag];
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
			m_fltSettings.Type |= (chkVanguard.Checked ? CardType.Vanguard : 0);
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
			m_fltSettings.AllowUnderscores = chkUnderscores.Checked;
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

		private void cmdReset_Click(object sender, EventArgs e)
		{
			// Reset our settings in case the user has made multiple changes they want to undo simultaneously.
			m_fltSettings = new Filters();
			LoadSettingsFromFilters(m_fltSettings);
		}
	}
}
