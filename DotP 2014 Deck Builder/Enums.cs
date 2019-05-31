using System;
using System.Text;

namespace RSN.DotP
{
	public enum ColourFlags
	{
		NotDefined = 0,
		Colourless = 0x01,
		Black = 0x02,
		Blue = 0x04,
		Green = 0x08,
		Red = 0x10,
		White = 0x20,
		MultiColour = 0x40,
	}

	// This is set up as flags for filtering purposes.
	public enum CardRarity
	{
		None = 0,
		Token = 0x01,
		Land = 0x02,
		Common = 0x04,
		Uncommon = 0x08,
		Rare = 0x10,
		Mythic = 0x20,
		Special = 0x40,
	}

	public enum CardType
	{
		None = 0,
		Artifact = 0x0001,
		Creature = 0x0002,
		Enchantment = 0x0004,
		Instant = 0x0008,
		Land = 0x0010,
		Phenomenon = 0x0020,
		Plane = 0x0040,
		Planeswalker = 0x0080,
		Scheme = 0x0100,
		Sorcery = 0x0200,
		Tribal = 0x0400,
		Vanguard = 0x0800,
	}

	// These are specifically for loading of sub-types from the SPECS.
	public enum CardSubTypeArchetypes
	{
		Artifact = 0,
		Creature,
		Enchantment,
		Land,
		Plane,
		Planeswalker,
		Scheme,
		Spell,
	}

	public enum CardSuperType
	{
		None = 0,
		Basic = 0x01,
		Legendary = 0x02,
		OnGoing = 0x04,
		Snow = 0x08,
		World = 0x10,
	}

	public enum DeckAvailability
	{
		Locked = 0,
		NeverAvailable,
		AlwaysAvailable,
	}

	public enum AbilityType
	{
		ACTIVATED_ABILITY = 0,
		MANA_ABILITY,
		STATIC_ABILITY,
		SPELL_ABILITY,
		TRIGGERED_ABILITY,
		UTILITY_ABILITY,
	}

	public enum IdSchemeType
	{
		PrefixScheme = 0,
		SuffixScheme,
		AdditiveScheme,
	}

	public enum FilterIntComparisonType
	{
		Equal = 0,
		GreaterThan,
		GreaterThanOrEqualTo,
		LessThan,
		LessThanOrEqualTo,
		NotEqual,
	}

	public enum FilterStringComparisonType
	{
		Contains = 0,
		DoesNotContain,
		Equal,
		NotEqual,
	}

	public enum FilterType
	{
		Allow = 0,
		AllowAny,
		ExactMatch,
		Exclude,
	}

	public enum FilterBooleanCompare
	{
		And = 0,
		Or,
	}

	public enum FilterBooleanComparisonType
	{
		BoolIs = 0,
		BoolIsNot,
	}

    public enum FilterEnumComparisonType
    {
        EnumIs = 0,
        EnumIsNot,
    }

    public enum FilterLegalityComparisonType
    {
        Equal = 0,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        NotEqual,
    }

    public enum LegalityValue
    {
        Unknown = 0,
        Banned,
        Restricted,
        Legal
    }
}
