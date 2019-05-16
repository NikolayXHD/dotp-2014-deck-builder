using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using RSN.Tools;

namespace RSN.DotP
{
	// This class will hold our filter settings and manage the logic for them.
	public class Filters
	{
		public bool AllowBasic;
		public bool AllowLegendary;
		public bool AllowSnow;
		public bool AllowWorld;

		// Ignored because the XmlSerializer doesn't understand multiple enum values "Or"d together.
		[XmlIgnore]
		public CardType Type;
		public FilterType TypeFilter;

		public bool DoSubTypeCheck;
		public string SubType;
		public FilterStringComparisonType SubTypeFilter;

		// Ignored because the XmlSerializer doesn't understand multiple enum values "Or"d together.
		[XmlIgnore]
		public ColourFlags Colour;
		public FilterType ColourFilter;

		public bool DoTitleCheck;
		public string Title;
		public FilterStringComparisonType TitleFilter;

		public int Cmc;
		public FilterIntComparisonType CmcFilter;

		public bool DoPowerToughnessCheck;
		public string Power;
		public FilterIntComparisonType PowerFilter;
		public string Toughness;
		public FilterIntComparisonType ToughnessFilter;

		// Ignored because the XmlSerializer doesn't understand multiple enum values "Or"d together.
		[XmlIgnore]
		public CardRarity Rarity;

		public bool DoAbilityCheck;
		public string Ability;
		public bool CheckGrantedAbilities;
		public FilterStringComparisonType AbilityFilter;

		public bool AllowToken;
		public bool AllowCreateTokens;

		public bool AllowUnderscores;

		public bool AllowManaRegular;
		public bool AllowManaHybrid;
		public bool AllowManaPhyrexian;

		public Filters()
		{
			// Sets up our defaults.
			AllowBasic = false;
			AllowLegendary = true;
			AllowSnow = true;
			AllowWorld = true;

			// By default we do not allow Plane, Planeswalker, or Scheme as those really aren't allowed in a regular deck.
			Type = CardType.Artifact | CardType.Creature | CardType.Enchantment | CardType.Instant | CardType.Land | CardType.Phenomenon | CardType.Sorcery | CardType.Tribal;
			TypeFilter = FilterType.Allow;

			DoSubTypeCheck = false;
			SubType = string.Empty;
			SubTypeFilter = FilterStringComparisonType.Contains;

			// By default allow all colours and colourless.
			Colour = ColourFlags.Black | ColourFlags.Blue | ColourFlags.Green | ColourFlags.Red | ColourFlags.White | ColourFlags.Colourless | ColourFlags.MultiColour;
			ColourFilter = FilterType.Allow;

			DoTitleCheck = false;
			Title = string.Empty;
			TitleFilter = FilterStringComparisonType.Contains;

			Cmc = 0;
			CmcFilter = FilterIntComparisonType.GreaterThanOrEqualTo;

			// Doing a power/toughness check will inherently limit to creatures so we don't want to do one by default.
			DoPowerToughnessCheck = false;
			Power = "0";
			PowerFilter = FilterIntComparisonType.GreaterThanOrEqualTo;
			Toughness = "0";
			ToughnessFilter = FilterIntComparisonType.GreaterThanOrEqualTo;

			// A card can really only have one rarity so AllowOnly and Exact Match don't make sense and Exclude is easy enough for people to specify using Allow.
			Rarity = CardRarity.Common | CardRarity.Land | CardRarity.Mythic | CardRarity.Rare | CardRarity.Uncommon | CardRarity.Special;

			// Don't want to do any ability checks at this time.
			DoAbilityCheck = false;
			Ability = string.Empty;
			CheckGrantedAbilities = false;
			AbilityFilter = FilterStringComparisonType.Contains;

			// Don't allow tokens to show since they aren't allowed in a regular deck.
			AllowToken = false;
			// Do allow cards that create tokens though (assuming they aren't also tokens).
			AllowCreateTokens = true;

			//Don't allow cards that begin with an underscore because that aren't allowed in a regular deck.
			AllowUnderscores = false;

			// By default allow all mana types.
			AllowManaRegular = true;
			AllowManaHybrid = true;
			AllowManaPhyrexian = true;
		}

		// These next few properties are for the XmlSerializer so that my "Or"d enums can be output.
		[XmlElement("Type")]
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public int TypeInt
		{
			get { return (int)Type; }
			set { Type = (CardType)value; }
		}

		[XmlElement("Colour")]
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public int ColourInt
		{
			get { return (int)Colour; }
			set { Colour = (ColourFlags)value; }
		}

		[XmlElement("Rarity")]
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public int RarityInt
		{
			get { return (int)Rarity; }
			set { Rarity = (CardRarity)value; }
		}

		public bool CheckAgainstFilter(CardInfo ciCard)
		{
			bool bAllowed = true;

			bAllowed &= CheckAgainstSuperTypes(ciCard);
			bAllowed &= CheckAgainstTypes(ciCard);
			bAllowed &= CheckAgainstSubType(ciCard);
			bAllowed &= CheckAgainstColours(ciCard);
			bAllowed &= CheckAgainstTitle(ciCard);
			bAllowed &= CheckAgainstCmc(ciCard);
			bAllowed &= CheckAgainstPowerToughness(ciCard);
			bAllowed &= CheckAgainstRarity(ciCard);
			bAllowed &= CheckAgainstAbility(ciCard);
			bAllowed &= CheckAgainstTokens(ciCard);
			bAllowed &= CheckAgainstUnderscores(ciCard);
			bAllowed &= CheckAgainstManaType(ciCard);

			return bAllowed;
		}

		private bool CheckAgainstSuperTypes(CardInfo ciCard)
		{
			bool bAllowed = true;

			if (((!AllowBasic) && ((ciCard.SuperTypes & CardSuperType.Basic) == CardSuperType.Basic)) ||
				((!AllowLegendary) && ((ciCard.SuperTypes & CardSuperType.Legendary) == CardSuperType.Legendary)) ||
				((!AllowSnow) && ((ciCard.SuperTypes & CardSuperType.Snow) == CardSuperType.Snow)) ||
				((!AllowWorld) && ((ciCard.SuperTypes & CardSuperType.World) == CardSuperType.World)))
				bAllowed = false;

			return bAllowed;
		}

		private bool CheckAgainstTypes(CardInfo ciCard)
		{
			bool bAllowed = true;

			switch (TypeFilter)
			{
				case FilterType.AllowAny:
					// Allow any combination of ...
					//	Anything outside the possible combinations should be denied.
					bAllowed = ((ciCard.Type & (~Type)) == 0);
					break;
				case FilterType.ExactMatch:
					bAllowed = (ciCard.Type == Type);
					break;
				case FilterType.Allow:
					bAllowed = CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Artifact, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Creature, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Enchantment, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Instant, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Land, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Phenomenon, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Plane, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Planeswalker, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Scheme, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Sorcery, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Tribal, TypeFilter);
					bAllowed |= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Vanguard, TypeFilter);
					break;
				case FilterType.Exclude:
					bAllowed = CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Artifact, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Creature, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Enchantment, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Instant, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Land, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Phenomenon, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Plane, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Planeswalker, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Scheme, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Sorcery, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Tribal, TypeFilter);
					bAllowed &= CheckFlag((int)ciCard.Type, (int)Type, (int)CardType.Vanguard, TypeFilter);
					break;
			}

			return bAllowed;
		}

		private bool CheckAgainstSubType(CardInfo ciCard)
		{
			bool bAllowed = true;

			if (DoSubTypeCheck)
			{
				if (ciCard.LocalizedTypeLine.IndexOf("— ") > -1)
				{
					int nStartIndex = ciCard.LocalizedTypeLine.IndexOf("— ") + 2;
					string strSubTypes = ciCard.LocalizedTypeLine.Substring(nStartIndex);
					bAllowed = CheckString(strSubTypes, SubType, SubTypeFilter);
				}
				else
				{
					if (((SubTypeFilter == FilterStringComparisonType.Equal) && (SubType.Length != 0)) ||
						((SubTypeFilter == FilterStringComparisonType.Contains) && (SubType.Length != 0)))
						bAllowed = false;
				}
			}

			return bAllowed;
		}

		private bool CheckAgainstColours(CardInfo ciCard)
		{
			bool bAllowed = true;

			switch (ColourFilter)
			{
				case FilterType.AllowAny:
					// Allow any combination of ...
					//	Anything outside the possible combinations should be denied.
					bAllowed = ((ciCard.Colour & (~Colour)) == 0);
					break;
				case FilterType.ExactMatch:
					bAllowed = (ciCard.Colour == Colour);
					break;
				case FilterType.Allow:
					bAllowed = CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Black, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Blue, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Green, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Red, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.White, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Colourless, ColourFilter);
					bAllowed |= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.MultiColour, ColourFilter);
					break;
				case FilterType.Exclude:
					bAllowed = CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Black, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Blue, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Green, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Red, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.White, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.Colourless, ColourFilter);
					bAllowed &= CheckFlag((int)ciCard.Colour, (int)Colour, (int)ColourFlags.MultiColour, ColourFilter);
					break;
			}

			return bAllowed;
		}

		private bool CheckAgainstTitle(CardInfo ciCard)
		{
			if (DoTitleCheck)
				return CheckString(ciCard.LocalizedName, Title, TitleFilter);
			else
				return true;
		}

		private bool CheckAgainstCmc(CardInfo ciCard)
		{
			return CheckInt(ciCard.ConvertedManaCost, Cmc, CmcFilter);
		}

		private bool CheckAgainstPowerToughness(CardInfo ciCard)
		{
			bool bAllowed = true;

			if (DoPowerToughnessCheck)
			{
				// Since we are doing the check set to false for all cards initially.
				bAllowed = false;
				if ((ciCard.Type & CardType.Creature) == CardType.Creature)
				{
					bAllowed = CheckStringInt(ciCard.Power, Power, PowerFilter);
					bAllowed &= CheckStringInt(ciCard.Toughness, Toughness, ToughnessFilter);
				}
			}

			return bAllowed;
		}

		private bool CheckAgainstRarity(CardInfo ciCard)
		{
			// Simple check since we only check whether a specific rarity is allowed or not.
			//	If "and"ing the two together gives us a value greater than 0 then at least one flag matched and it is allowed.
			return ((ciCard.Rarity & Rarity) > 0);
		}

		private bool CheckAgainstAbility(CardInfo ciCard)
		{
			bool bAllowed = true;

			if (DoAbilityCheck)
			{
				if ((ciCard.Abilities != null) && (ciCard.Abilities.Count > 0))
				{
					bool bFoundNotEqual = false;
					if (AbilityFilter != FilterStringComparisonType.DoesNotContain)
						bAllowed = false;
					foreach (Ability ab in ciCard.Abilities)
					{
						// Since we have abilities we only want to compare on abilities that have text.
						if ((ab.Text != null) && (ab.LocalizedText.Length > 0))
						{
							// We only check granted abilities if the user wants us to.
							if ((ab.ResourceId < 0) || (CheckGrantedAbilities))
							{
								switch (AbilityFilter)
								{
									case FilterStringComparisonType.Contains:
										{
											if (ab.LocalizedText.IndexOf(Ability, StringComparison.CurrentCultureIgnoreCase) > -1)
												bAllowed = true;
										}
										break;
									case FilterStringComparisonType.DoesNotContain:
										{
											if (ab.LocalizedText.IndexOf(Ability, StringComparison.CurrentCultureIgnoreCase) > -1)
												bAllowed = false;
										}
										break;
									case FilterStringComparisonType.Equal:
										{
											if (ab.LocalizedText.Equals(Ability, StringComparison.CurrentCultureIgnoreCase))
												bAllowed = true;
											else
												bFoundNotEqual = true;
										}
										break;
									case FilterStringComparisonType.NotEqual:
										{
											if (!ab.LocalizedText.Equals(Ability, StringComparison.CurrentCultureIgnoreCase))
												bAllowed = true;
										}
										break;
								}
							}
						}
					}
					if ((AbilityFilter == FilterStringComparisonType.Equal) && (bFoundNotEqual))
						bAllowed = false;
				}
				else
				{
					// No abilities on card so unless we are checking DoesNotContain or NotEqual then our comparison is false (except if they are searching for Equal "").
					bAllowed = false;
					if (((AbilityFilter == FilterStringComparisonType.Equal) && (Ability.Equals(string.Empty))) ||
						((AbilityFilter == FilterStringComparisonType.NotEqual) && (!Ability.Equals(string.Empty))) ||
						((AbilityFilter == FilterStringComparisonType.DoesNotContain) && (Ability.Length > 0)) ||
						((AbilityFilter == FilterStringComparisonType.Contains) && (Ability.Length == 0)))
						bAllowed = true;
				}
			}

			return bAllowed;
		}

		private bool CheckAgainstTokens(CardInfo ciCard)
		{
			bool bAllowed = true;

			if (((!AllowToken) && (ciCard.Token)) ||
				((!AllowCreateTokens) && (ciCard.RegisteredTokens != null) && (ciCard.RegisteredTokens.Count > 0)))
				bAllowed = false;

			return bAllowed;
		}

		private bool CheckAgainstUnderscores(CardInfo ciCard)
		{
			bool bAllowed = true;

			if ((!AllowUnderscores) && (ciCard.Underscore))
				bAllowed = false;

			return bAllowed;
		}

		private bool CheckAgainstManaType(CardInfo ciCard)
		{
			bool bAllowed = false;

			if (((AllowManaRegular) && (!ciCard.HybridMana) && (!ciCard.PhyrexianMana)) ||
				((AllowManaHybrid) && (ciCard.HybridMana)) ||
				((AllowManaPhyrexian) && (ciCard.PhyrexianMana)))
				bAllowed = true;

			return bAllowed;
		}

		private bool CheckFlag(int nValue, int nCheck, int nFlag, FilterType eType)
		{
			bool bAllow = true;

			switch (eType)
			{
				case FilterType.Allow:
					bAllow = ((nValue & nCheck & nFlag) == nFlag);
					break;
				case FilterType.Exclude:
					bAllow = !((nValue & nCheck & nFlag) == nFlag);
					break;
			}

			return bAllow;
		}

		private bool CheckInt(int nValue, int nCheck, FilterIntComparisonType eType)
		{
			bool bAllow = true;

			// Pretty straight-forward.
			switch (eType)
			{
				case FilterIntComparisonType.Equal:
					bAllow = (nValue == nCheck);
					break;
				case FilterIntComparisonType.GreaterThan:
					bAllow = (nValue > nCheck);
					break;
				case FilterIntComparisonType.GreaterThanOrEqualTo:
					bAllow = (nValue >= nCheck);
					break;
				case FilterIntComparisonType.LessThan:
					bAllow = (nValue < nCheck);
					break;
				case FilterIntComparisonType.LessThanOrEqualTo:
					bAllow = (nValue <= nCheck);
					break;
				case FilterIntComparisonType.NotEqual:
					bAllow = (nValue != nCheck);
					break;
			}

			return bAllow;
		}

		private bool CheckString(string strValue, string strCheck, FilterStringComparisonType eType)
		{
			bool bAllow = true;

			switch (eType)
			{
				case FilterStringComparisonType.Contains:
					bAllow = (strValue.IndexOf(strCheck, StringComparison.CurrentCultureIgnoreCase) > -1);
					break;
				case FilterStringComparisonType.DoesNotContain:
					bAllow = (strValue.IndexOf(strCheck, StringComparison.CurrentCultureIgnoreCase) < 0);
					break;
				case FilterStringComparisonType.Equal:
					bAllow = strValue.Equals(strCheck, StringComparison.CurrentCultureIgnoreCase);
					break;
				case FilterStringComparisonType.NotEqual:
					bAllow = !strValue.Equals(strCheck, StringComparison.CurrentCultureIgnoreCase);
					break;
			}

			return bAllow;
		}

		// This is a special check intended for Power & Toughness where they could be "*", but are otherwise integers.
		private bool CheckStringInt(string strValue, string strCheck, FilterIntComparisonType eType)
		{
			// Double check to make sure we have either a * or a number
			strValue = CardFilterStringIntProp.CleanValue(strValue.Trim());
			strCheck = CardFilterStringIntProp.CleanValue(strCheck.Trim());
			int nValue = -1;
			int nCheck = -1;
			// If we aren't going to use a string check then we need to make sure we can get an integer from the strings.
			if ((!strValue.Contains("*")) && (!strCheck.Contains("*")))
			{
				try
				{
					nValue = Int32.Parse(strValue);
					nCheck = Int32.Parse(strCheck);
				}
				catch (Exception e)
				{
					Settings.ReportError(e, ErrorPriority.Medium, "Error when trying to get integer values to check: Value='" + strValue + "' CheckValue='" + strCheck + "'\r\nThis check will be ignored due to an error and everything will pass this check while the error persists.");
					return true;
				}
			}

			bool bAllow = false;

			// Check for the special case first.
			if ((strValue.Contains("*")) || (strCheck.Contains("*")))
			{
				// We have our special case they are only true for Equal or Not Equal.
				if ((eType == FilterIntComparisonType.Equal) ||
					(eType == FilterIntComparisonType.GreaterThanOrEqualTo) ||
					(eType == FilterIntComparisonType.LessThanOrEqualTo))
					bAllow = strValue.Equals(strCheck);
				else if (eType == FilterIntComparisonType.NotEqual)
					bAllow = !strValue.Equals(strCheck);
			}
			else
			{
				bAllow = CheckInt(nValue, nCheck, eType);
			}

			return bAllow;
		}
	}
}
