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
	public partial class CardFilterAdvancedSettings : Form
	{
		// Set up what are our allowable DataProperties are for each filter group.
		//	Special filters like "SubTypes" will need to be individually configured in
		//	the appropriate CreateX function.
		private string[] BOOLEAN_FIELDS = new string[] { "CreateTokens", "HybridMana", "PhyrexianMana", "Token", "Underscore" };
		private string[] INTEGER_FIELDS = new string[] { "ConvertedManaCost", "MultiverseId" };
		private string[] STRING_FIELDS = new string[] { "LocalizedAbilitiesText", "Artist", "CardXML", "CastingCost", "Expansion", "LocalizedFlavour", "Filename", "ImageFilename", "LocalizedName", "RegisteredTokensText", "SubTypes", "PresentInWad" };
		private string[] ENUM_FIELDS = new string[] { "Colour", "Rarity", "SuperTypes", "Type" };
		// Power/Toughness fields
		private string[] STRINGINT_FIELDS = new string[] { "Power", "Toughness" };

		public CardFilterAdvancedSettings()
		{
			InitializeComponent();

			Rectangle rcPosition = Settings.GetSetting("CardFilterAdvancedSettingsPosition", new Rectangle(-1, -1, -1, -1));
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

			CardFilterSet cfsLoaded = Settings.GetSerializableSetting("CardFilterAdvanced", new CardFilterSet());
			FillTreeFromSet(cfsLoaded);
			tvwFilters.SelectedNode = tvwFilters.Nodes[0];
		}

		private void LoadLocalizedStrings()
		{
			// Window
			this.Text = Settings.UIStrings[(string)this.Tag];

			// Tabs
			foreach (TabPage tpPage in tcFilters.TabPages)
			{
				tpPage.Text = Settings.UIStrings[(string)tpPage.Tag];
			}

			// Filter Set Page
			chkSetNegate.Text = Settings.UIStrings[(string)chkSetNegate.Tag];
			lblSetCompare.Text = Settings.UIStrings[(string)lblSetCompare.Tag];
			AddItemsToCompareDropDown(cboSetCompare);
			cmdSetAdd.Text = Settings.UIStrings[(string)cmdSetAdd.Tag];
			cmdSetReplace.Text = Settings.UIStrings[(string)cmdSetReplace.Tag];

			// Boolean Filter Page
			AddItemsToBoolOperationDropDown(cboBoolOperation);
			AddItemsToDataPropDropDown(cboBoolDataProp, BOOLEAN_FIELDS);
			lblBoolCompare.Text = Settings.UIStrings[(string)lblBoolCompare.Tag];
			AddItemsToCompareDropDown(cboBoolCompare);
			cmdBoolAdd.Text = Settings.UIStrings[(string)cmdBoolAdd.Tag];
			cmdBoolReplace.Text = Settings.UIStrings[(string)cmdBoolReplace.Tag];

			// Integer Filter Page
			AddItemsToDataPropDropDown(cboIntDataProp, INTEGER_FIELDS);
			AddItemsToIntOperationDropDown(cboIntOperation);
			lblIntCompare.Text = Settings.UIStrings[(string)lblIntCompare.Tag];
			AddItemsToCompareDropDown(cboIntCompare);
			cmdIntAdd.Text = Settings.UIStrings[(string)cmdIntAdd.Tag];
			cmdIntReplace.Text = Settings.UIStrings[(string)cmdIntReplace.Tag];

			// String Filter Page
			AddItemsToDataPropDropDown(cboStringDataProp, STRING_FIELDS);
			AddItemsToStringOperationDropDown(cboStringOperation);
			lblStringCompare.Text = Settings.UIStrings[(string)lblStringCompare.Tag];
			AddItemsToCompareDropDown(cboStringCompare);
			cmdStringAdd.Text = Settings.UIStrings[(string)cmdStringAdd.Tag];
			cmdStringReplace.Text = Settings.UIStrings[(string)cmdStringReplace.Tag];

			// Enum Filter Page
			AddItemsToDataPropDropDown(cboEnumDataProp, ENUM_FIELDS);
			AddItemsToEnumOperationDropDown(cboEnumOperation);
			lblEnumCompare.Text = Settings.UIStrings[(string)lblEnumCompare.Tag];
			AddItemsToCompareDropDown(cboEnumCompare);
			cmdEnumAdd.Text = Settings.UIStrings[(string)cmdEnumAdd.Tag];
			cmdEnumReplace.Text = Settings.UIStrings[(string)cmdEnumReplace.Tag];

			// Power/Toughness Page
			AddItemsToDataPropDropDown(cboStringIntDataProp, STRINGINT_FIELDS);
			AddItemsToIntOperationDropDown(cboStringIntOperation);
			lblStringIntCompare.Text = Settings.UIStrings[(string)lblStringIntCompare.Tag];
			AddItemsToCompareDropDown(cboStringIntCompare);
			cmdStringIntAdd.Text = Settings.UIStrings[(string)cmdStringIntAdd.Tag];
			cmdStringIntReplace.Text = Settings.UIStrings[(string)cmdStringIntReplace.Tag];

			// Clear Filters
			cmdClearFilters.Text = Settings.UIStrings[(string)cmdClearFilters.Tag];

			// Apply & Cancel
			cmdApply.Text = Settings.UIStrings[(string)cmdApply.Tag];
			cmdCancel.Text = Settings.UIStrings[(string)cmdCancel.Tag];
		}

		private void AddItemsToCompareDropDown(ComboBox cboCompareDropDown)
		{
			// Make sure there's nothing already in the list.
			cboCompareDropDown.Items.Clear();
			// Set up how we should display our items.
			cboCompareDropDown.DisplayMember = "Value";
			cboCompareDropDown.ValueMember = "Key";
			// Add the items.
			foreach (FilterBooleanCompare fbcValue in Enum.GetValues(typeof(FilterBooleanCompare)))
				cboCompareDropDown.Items.Add(new KeyValuePair<FilterBooleanCompare, string>(fbcValue, Settings.UIStrings["CARD_FILTER_COMPARE_" + fbcValue.ToString().ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboCompareDropDown.SelectedIndex = 0;
		}

		private void AddItemsToDataPropDropDown(ComboBox cboDropDown, string[] astrProps)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			// Add the items.
			foreach (string strItem in astrProps)
				cboDropDown.Items.Add(new KeyValuePair<string, string>(strItem, Settings.UIStrings["CARD_FILTER_PROP_" + strItem.ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboDropDown.SelectedIndex = 0;
		}

		private void AddItemsToBoolOperationDropDown(ComboBox cboDropDown)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			// Add the items.
			foreach (FilterBooleanComparisonType eItem in Enum.GetValues(typeof(FilterBooleanComparisonType)))
				cboDropDown.Items.Add(new KeyValuePair<FilterBooleanComparisonType, string>(eItem, Settings.UIStrings[eItem.ToString().ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboDropDown.SelectedIndex = 0;
		}

		private void AddItemsToIntOperationDropDown(ComboBox cboDropDown)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			// Add the items.
			foreach (FilterIntComparisonType eItem in Enum.GetValues(typeof(FilterIntComparisonType)))
				cboDropDown.Items.Add(new KeyValuePair<FilterIntComparisonType, string>(eItem, Settings.UIStrings["CARD_FILTER_INT_COMPARISON_" + eItem.ToString().ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboDropDown.SelectedIndex = 0;
		}

		private void AddItemsToStringOperationDropDown(ComboBox cboDropDown)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			// Add the items.
			foreach (FilterStringComparisonType eItem in Enum.GetValues(typeof(FilterStringComparisonType)))
				cboDropDown.Items.Add(new KeyValuePair<FilterStringComparisonType, string>(eItem, Settings.UIStrings[eItem.ToString().ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboDropDown.SelectedIndex = 0;
		}

		private void AddItemsToEnumOperationDropDown(ComboBox cboDropDown)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			// Add the items.
			foreach (FilterEnumComparisonType eItem in Enum.GetValues(typeof(FilterEnumComparisonType)))
				cboDropDown.Items.Add(new KeyValuePair<FilterEnumComparisonType, string>(eItem, Settings.UIStrings[eItem.ToString().ToUpper()]));
			// Set the first item as our selected item to start things off.
			cboDropDown.SelectedIndex = 0;
		}

		private void AddItemsToEnumValueDropDown(ComboBox cboDropDown, string strDataProp)
		{
			// Make sure there's nothing already in the list.
			cboDropDown.Items.Clear();
			// Set up how we should display our items.
			cboDropDown.DisplayMember = "Value";
			cboDropDown.ValueMember = "Key";
			//Figure out what items to get.
			if (strDataProp.Equals("Rarity", StringComparison.OrdinalIgnoreCase))
			{
				foreach (CardRarity eItem in Enum.GetValues(typeof(CardRarity)))
				{
					if (eItem != CardRarity.None)
						cboDropDown.Items.Add(new KeyValuePair<CardRarity, string>(eItem, Settings.UIStrings["RARITY_" + eItem.ToString().ToUpper()]));
				}
			}
			else if (strDataProp.Equals("SuperTypes", StringComparison.OrdinalIgnoreCase))
			{
				foreach (CardSuperType eItem in Enum.GetValues(typeof(CardSuperType)))
				{
					if (eItem != CardSuperType.None)
						cboDropDown.Items.Add(new KeyValuePair<CardSuperType, string>(eItem, Settings.UIStrings["CARD_SUPERTYPE_" + eItem.ToString().ToUpper()]));
				}
			}
			else if (strDataProp.Equals("Type", StringComparison.OrdinalIgnoreCase))
			{
				foreach (CardType eItem in Enum.GetValues(typeof(CardType)))
				{
					if (eItem != CardType.None)
						cboDropDown.Items.Add(new KeyValuePair<CardType, string>(eItem, Settings.UIStrings["CARD_TYPE_" + eItem.ToString().ToUpper()]));
				}
			}
			else if (strDataProp.Equals("Colour", StringComparison.OrdinalIgnoreCase))
			{
				foreach (ColourFlags eItem in Enum.GetValues(typeof(ColourFlags)))
				{
					if (eItem != ColourFlags.NotDefined)
						cboDropDown.Items.Add(new KeyValuePair<ColourFlags, string>(eItem, Settings.UIStrings["MANA_" + eItem.ToString().ToUpper()]));
				}
			}
			// Set the first item as our selected item to start things off.
			if (cboDropDown.Items.Count > 0)
				cboDropDown.SelectedIndex = 0;
		}

		private void FillTreeFromSet(CardFilterBase cfbFilter, TreeNode trParent = null)
		{
			TreeNode trNode = new TreeNode(cfbFilter.ToString());
			trNode.Tag = cfbFilter;
			if (trParent == null)
				tvwFilters.Nodes.Add(trNode);
			else
				trParent.Nodes.Add(trNode);

			if (cfbFilter is CardFilterSet)
			{
				CardFilterSet cfsSet = cfbFilter as CardFilterSet;
				foreach (CardFilterBase cfbSub in cfsSet.Filters)
					FillTreeFromSet(cfbSub, trNode);
				// We want the tree fully expanded, but you can really only expand sets.
				trNode.Expand();
			}
		}

		private CardFilterBase GenerateFilterFromTree(TreeNode trNode = null)
		{
			if (trNode == null)
			{
				// We are to start from the top of the tree.
				//	We will always have only a single root node.
				trNode = tvwFilters.Nodes[0];
			}

			CardFilterBase cfbItem = trNode.Tag as CardFilterBase;
			if (cfbItem is CardFilterSet)
			{
				CardFilterSet cfsSet = cfbItem as CardFilterSet;
				cfsSet.Filters.Clear();
				foreach (TreeNode trSub in trNode.Nodes)
					cfsSet.Filters.Add(GenerateFilterFromTree(trSub));
			}

			// Done retrieving so return what we got (should never be null).
			return cfbItem;
		}

		private void CardFilterAdvancedSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.SaveSetting("CardFilterAdvancedSettingsPosition", new Rectangle(this.Location, this.Size));
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			// Since the root item of the tree will always be a CardFilterSet this works:
			Settings.SaveSerializableSetting("CardFilterAdvanced", GenerateFilterFromTree() as CardFilterSet);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdClearFilters_Click(object sender, EventArgs e)
		{
			// First remove all nodes.
			tvwFilters.Nodes.Clear();

			// There must always be a Filter Set as the root node (so create a new one).
			CardFilterSet cfsRoot = new CardFilterSet();
			TreeNode trRoot = new TreeNode(cfsRoot.ToString());
			trRoot.Tag = cfsRoot;
			tvwFilters.Nodes.Add(trRoot);
			// Make sure it is expanded.
			trRoot.Expand();
		}

		private void CheckButtons()
		{
			if (tvwFilters.SelectedNode == null)
			{
				// No selected node.
				cmdSetReplace.Enabled = false;
				cmdBoolReplace.Enabled = false;
				cmdIntReplace.Enabled = false;
				cmdStringReplace.Enabled = false;
				cmdEnumReplace.Enabled = false;
				cmdStringIntReplace.Enabled = false;
			}
			else if (tvwFilters.SelectedNode.Parent == null)
			{
				// Root node, need to disallow replacing selected node.
				//	Root node must always be a CardFilterSet node.
				cmdSetReplace.Enabled = false;
				cmdBoolReplace.Enabled = false;
				cmdIntReplace.Enabled = false;
				cmdStringReplace.Enabled = false;
				cmdEnumReplace.Enabled = false;
				cmdStringIntReplace.Enabled = false;
			}
			else
			{
				// All other cases
				cmdSetReplace.Enabled = true;
				cmdBoolReplace.Enabled = true;
				cmdIntReplace.Enabled = true;
				cmdStringReplace.Enabled = true;
				cmdEnumReplace.Enabled = true;
				cmdStringIntReplace.Enabled = true;
			}
		}

		private void tvwFilters_AfterSelect(object sender, TreeViewEventArgs e)
		{
			CheckButtons();
		}

		private void tvwFilters_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				// User pressed delete, make sure this isn't the root node then remove it.
				if ((tvwFilters.SelectedNode != null) && (tvwFilters.SelectedNode.Parent != null))
				{
					tvwFilters.SelectedNode.Parent.Nodes.Remove(tvwFilters.SelectedNode);
					// Refresh the state of our buttons since we may no longer have a selected node.
					CheckButtons();
				}
			}
		}

		private void tvwFilters_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Item is TreeNode)
			{
				TreeNode trNode = e.Item as TreeNode;
				// We don't allow moving the root node.
				if (trNode.Parent != null)
				{
					// Make sure we have a CardFilter that we are trying to move.
					if (trNode.Tag is CardFilterBase)
					{
						DoDragDrop(e.Item, DragDropEffects.Move);
					}
				}
			}
		}

		private void tvwFilters_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
			{
				TreeNode trNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				if (trNode.Tag is CardFilterBase)
				{
					// Verify it is over a valid node.
					Point ptLoc = tvwFilters.PointToClient(new Point(e.X, e.Y));
					TreeViewHitTestInfo tvhtiTest = tvwFilters.HitTest(ptLoc.X, ptLoc.Y);
					if ((tvhtiTest != null) && (tvhtiTest.Node != null))
					{
						// Remove from previous position.
						trNode.Parent.Nodes.Remove(trNode);

						// Move to new position.
						TreeNode trDest = tvhtiTest.Node;
						if (trDest.Tag is CardFilterSet)
						{
							// Dropped on a set so add the node as the last child.
							trDest.Nodes.Add(trNode);
							trDest.ExpandAll();
						}
						else
						{
							// Dropped on a regular filter so add before that filter.
							trDest.Parent.Nodes.Insert(trDest.Index, trNode);
						}
					}
				}
			}
		}

		private void tvwFilters_DragOver(object sender, DragEventArgs e)
		{
			// Make sure data can't be dropped here until we have verified it.
			e.Effect = DragDropEffects.None;
			// Must be a TreeNode
			if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
			{
				TreeNode trNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				// The TreeNode must contain a CardFilter (it can't be anything else)
				if (trNode.Tag is CardFilterBase)
				{
					// Verify it is over a valid node.
					Point ptLoc = tvwFilters.PointToClient(new Point(e.X, e.Y));
					TreeViewHitTestInfo tvhtiTest = tvwFilters.HitTest(ptLoc.X, ptLoc.Y);
					if ((tvhtiTest != null) && (tvhtiTest.Node != null))
						e.Effect = DragDropEffects.Move;
				}
			}
		}

		private void ReplaceNode(TreeNode trNode, CardFilterBase cfbNewItem)
		{
			// Make sure there are no children of the node we are "replacing"
			trNode.Nodes.Clear();
			// Update with the new information.
			trNode.Text = cfbNewItem.ToString();
			trNode.Tag = cfbNewItem;
		}

		private void AddNode(TreeNode trCurrentSelection, CardFilterBase cfbNewItem)
		{
			TreeNode trNode = new TreeNode(cfbNewItem.ToString());
			trNode.Tag = cfbNewItem;
			if (trCurrentSelection == null)
			{
				// Add to end of root node.
				tvwFilters.Nodes[0].Nodes.Add(trNode);
				tvwFilters.Nodes[0].Expand();
			}
			else
			{
				// If selected node is a set add to the set.
				if (trCurrentSelection.Tag is CardFilterSet)
				{
					trCurrentSelection.Nodes.Add(trNode);
					trCurrentSelection.Expand();
				}
				else
				{
					// Otherwise we add to the set that is the parent of the current node.
					//	Should not need to worry about a null parent, because the only node
					//	that should have a null parent is the root node which is a Set so
					//	it should not fall to this code.
					trCurrentSelection.Parent.Nodes.Add(trNode);
				}
			}
		}

		private CardFilterSet CreateFilterSet()
		{
			if (cboSetCompare.SelectedIndex > -1)
				return new CardFilterSet(chkSetNegate.Checked, ((KeyValuePair<FilterBooleanCompare, string>)cboSetCompare.Items[cboSetCompare.SelectedIndex]).Key);
			else
				return new CardFilterSet(chkSetNegate.Checked, FilterBooleanCompare.And);
		}

		private void cmdSetReplace_Click(object sender, EventArgs e)
		{
			CardFilterSet cfsSet = CreateFilterSet();
			ReplaceNode(tvwFilters.SelectedNode, cfsSet);
		}

		private void cmdSetAdd_Click(object sender, EventArgs e)
		{
			CardFilterSet cfsSet = CreateFilterSet();
			AddNode(tvwFilters.SelectedNode, cfsSet);
		}

		private CardFilterBoolProp CreateBooleanFilter()
		{
			if ((cboBoolOperation.SelectedItem != null) &&
				(cboBoolDataProp.SelectedItem != null) &&
				(cboBoolCompare.SelectedItem != null))
			{
				FilterBooleanComparisonType fbctOperation = ((KeyValuePair<FilterBooleanComparisonType, string>)cboBoolOperation.SelectedItem).Key;
				string strDataProp = ((KeyValuePair<string, string>)cboBoolDataProp.SelectedItem).Key;
				FilterBooleanCompare fbcCompare = ((KeyValuePair<FilterBooleanCompare, string>)cboBoolCompare.SelectedItem).Key;
				// Check for special case(s):
				if (strDataProp.Equals("CreateTokens", StringComparison.OrdinalIgnoreCase))
					return new CardFilterCreateTokens(fbctOperation, fbcCompare);
				else
					return new CardFilterBoolProp(strDataProp, fbctOperation, fbcCompare);
			}
			else
			{
				// User needs to make sure all fields are filled.
				//	This should never happen, because I forcibly fill all fields on load
				//	and since they are drop-down lists the user should not be able to
				//	deselect the items or select an invalid item.
				return null;
			}
		}
		
		private void cmdBoolReplace_Click(object sender, EventArgs e)
		{
			CardFilterBoolProp cfbpFilter = CreateBooleanFilter();
			if (cfbpFilter != null)
				ReplaceNode(tvwFilters.SelectedNode, cfbpFilter);
		}

		private void cmdBoolAdd_Click(object sender, EventArgs e)
		{
			CardFilterBoolProp cfbpFilter = CreateBooleanFilter();
			if (cfbpFilter != null)
				AddNode(tvwFilters.SelectedNode, cfbpFilter);
		}

		private CardFilterIntProp CreateIntFilter()
		{
			if ((cboIntDataProp.SelectedItem != null) &&
				(cboIntOperation.SelectedItem != null) &&
				(cboIntCompare.SelectedItem != null))
			{
				string strDataProp = ((KeyValuePair<string, string>)cboIntDataProp.SelectedItem).Key;
				FilterIntComparisonType fictOperation = ((KeyValuePair<FilterIntComparisonType, string>)cboIntOperation.SelectedItem).Key;
				int nValue = (int)numIntValue.Value;
				FilterBooleanCompare fbcCompare = ((KeyValuePair<FilterBooleanCompare, string>)cboIntCompare.SelectedItem).Key;
				return new CardFilterIntProp(strDataProp, fictOperation, nValue, fbcCompare);
			}
			else
			{
				// User needs to make sure all fields are filled.
				//	This should never happen, because I forcibly fill all fields on load
				//	and since they are drop-down lists the user should not be able to
				//	deselect the items or select an invalid item.
				return null;
			}
		}

		private void cmdIntReplace_Click(object sender, EventArgs e)
		{
			CardFilterIntProp cfipFilter = CreateIntFilter();
			if (cfipFilter != null)
				ReplaceNode(tvwFilters.SelectedNode, cfipFilter);
		}

		private void cmdIntAdd_Click(object sender, EventArgs e)
		{
			CardFilterIntProp cfipFilter = CreateIntFilter();
			if (cfipFilter != null)
				AddNode(tvwFilters.SelectedNode, cfipFilter);
		}

		private CardFilterStringIntProp CreateStringIntFilter()
		{
			if ((cboStringIntDataProp.SelectedItem != null) &&
				(cboStringIntOperation.SelectedItem != null) &&
				(cboStringIntCompare.SelectedItem != null))
			{
				string strDataProp = ((KeyValuePair<string, string>)cboStringIntDataProp.SelectedItem).Key;
				FilterIntComparisonType fictOperation = ((KeyValuePair<FilterIntComparisonType, string>)cboStringIntOperation.SelectedItem).Key;
				string strValue = txtStringIntValue.Text.Trim();
				FilterBooleanCompare fbcCompare = ((KeyValuePair<FilterBooleanCompare, string>)cboStringIntCompare.SelectedItem).Key;
				return new CardFilterStringIntProp(strDataProp, fictOperation, strValue, fbcCompare);
			}
			else
			{
				// User needs to make sure all fields are filled.
				//	This should never happen, because I forcibly fill all fields on load
				//	and since they are drop-down lists the user should not be able to
				//	deselect the items or select an invalid item.
				return null;
			}
		}

		private void cmdStringIntReplace_Click(object sender, EventArgs e)
		{
			CardFilterStringIntProp cfsipFilter = CreateStringIntFilter();
			if (cfsipFilter != null)
				ReplaceNode(tvwFilters.SelectedNode, cfsipFilter);
		}

		private void cmdStringIntAdd_Click(object sender, EventArgs e)
		{
			CardFilterStringIntProp cfsipFilter = CreateStringIntFilter();
			if (cfsipFilter != null)
				AddNode(tvwFilters.SelectedNode, cfsipFilter);
		}

		private void txtStringIntValue_Leave(object sender, EventArgs e)
		{
			// Allow blank.
			if (txtStringIntValue.Text.Length > 0)
			{
				// Allow "*"
				if (!txtStringIntValue.Text.Trim().Equals("*"))
				{
					int nValue;
					if (!Int32.TryParse(txtStringIntValue.Text.Trim(), out nValue))
						txtStringIntValue.Text = "0";
				}
			}
			else
				txtStringIntValue.Text = "0";
		}

		private CardFilterBase CreateEnumFilter()
		{
			if ((cboEnumDataProp.SelectedItem != null) &&
				(cboEnumOperation.SelectedItem != null) &&
				(cboEnumValue.SelectedItem != null) &&
				(cboEnumCompare.SelectedItem != null))
			{
				string strDataProp = ((KeyValuePair<string, string>)cboEnumDataProp.SelectedItem).Key;
				FilterEnumComparisonType fectOperation = ((KeyValuePair<FilterEnumComparisonType, string>)cboEnumOperation.SelectedItem).Key;
				FilterBooleanCompare fbcCompare = ((KeyValuePair<FilterBooleanCompare, string>)cboEnumCompare.SelectedItem).Key;

				CardFilterBase cfbFilter = null;
				if (strDataProp.Equals("Colour", StringComparison.OrdinalIgnoreCase))
				{
					ColourFlags cfColour = ((KeyValuePair<ColourFlags, string>)cboEnumValue.SelectedItem).Key;
					cfbFilter = new CardFilterEnumProp<ColourFlags>(strDataProp, fectOperation, cfColour, fbcCompare);
				}
				else if (strDataProp.Equals("Rarity", StringComparison.OrdinalIgnoreCase))
				{
					CardRarity crRarity = ((KeyValuePair<CardRarity, string>)cboEnumValue.SelectedItem).Key;
					cfbFilter = new CardFilterEnumProp<CardRarity>(strDataProp, fectOperation, crRarity, fbcCompare);
				}
				else if (strDataProp.Equals("SuperTypes", StringComparison.OrdinalIgnoreCase))
				{
					CardSuperType cstType = ((KeyValuePair<CardSuperType, string>)cboEnumValue.SelectedItem).Key;
					cfbFilter = new CardFilterEnumProp<CardSuperType>(strDataProp, fectOperation, cstType, fbcCompare);
				}
				else if (strDataProp.Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					CardType ctType = ((KeyValuePair<CardType, string>)cboEnumValue.SelectedItem).Key;
					cfbFilter = new CardFilterEnumProp<CardType>(strDataProp, fectOperation, ctType, fbcCompare);
				}
				return cfbFilter;
			}
			else
			{
				// User needs to make sure all fields are filled.
				//	This should never happen, because I forcibly fill all fields on load
				//	and since they are drop-down lists the user should not be able to
				//	deselect the items or select an invalid item.
				return null;
			}
		}

		private void cmdEnumReplace_Click(object sender, EventArgs e)
		{
			CardFilterBase cfbFilter = CreateEnumFilter();
			if (cfbFilter != null)
				ReplaceNode(tvwFilters.SelectedNode, cfbFilter);
		}

		private void cmdEnumAdd_Click(object sender, EventArgs e)
		{
			CardFilterBase cfbFilter = CreateEnumFilter();
			if (cfbFilter != null)
				AddNode(tvwFilters.SelectedNode, cfbFilter);
		}

		private void cboEnumDataProp_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboEnumDataProp.SelectedItem != null)
			{
				string strDataProp = ((KeyValuePair<string, string>)cboEnumDataProp.SelectedItem).Key;
				AddItemsToEnumValueDropDown(cboEnumValue, strDataProp);
			}
		}

		private CardFilterStringProp CreateStringFilter()
		{
			if ((cboStringDataProp.SelectedItem != null) &&
				(cboStringOperation.SelectedItem != null) &&
				(cboStringCompare.SelectedItem != null))
			{
				string strDataProp = ((KeyValuePair<string, string>)cboStringDataProp.SelectedItem).Key;
				FilterStringComparisonType fsctOperation = ((KeyValuePair<FilterStringComparisonType, string>)cboStringOperation.SelectedItem).Key;
				string strValue = txtStringValue.Text.Trim();
				FilterBooleanCompare fbcCompare = ((KeyValuePair<FilterBooleanCompare, string>)cboStringCompare.SelectedItem).Key;
				// Handle any special cases.
				if (strDataProp.Equals("SubTypes", StringComparison.OrdinalIgnoreCase))
					return new CardFilterSubTypes(fsctOperation, strValue, fbcCompare);
				else
					return new CardFilterStringProp(strDataProp, fsctOperation, strValue, fbcCompare);
			}
			else
			{
				// User needs to make sure all fields are filled.
				//	This should never happen, because I forcibly fill all fields on load
				//	and since they are drop-down lists the user should not be able to
				//	deselect the items or select an invalid item.
				return null;
			}
		}

		private void cmdStringReplace_Click(object sender, EventArgs e)
		{
			CardFilterStringProp cfspFilter = CreateStringFilter();
			if (cfspFilter != null)
				ReplaceNode(tvwFilters.SelectedNode, cfspFilter);
		}

		private void cmdStringAdd_Click(object sender, EventArgs e)
		{
			CardFilterStringProp cfspFilter = CreateStringFilter();
			if (cfspFilter != null)
				AddNode(tvwFilters.SelectedNode, cfspFilter);
		}
	}
}
