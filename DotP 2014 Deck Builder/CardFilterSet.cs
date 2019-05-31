using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using RSN.Tools;

namespace RSN.DotP
{
	// This class represents a collection of filters and at the same time a filter
	//	itself, this is to allow for nested filters and a unified way to check a
	//	card regardless of how many filters there are.
	public class CardFilterSet : CardFilterBase
	{
		private List<CardFilterBase> m_lstFilters;

		public bool NegateSet;

		public CardFilterSet() : this(false, FilterBooleanCompare.And) { }

		public CardFilterSet(bool bNegateSet, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			NegateSet = bNegateSet;
			m_lstFilters = new List<CardFilterBase>();
		}

		// To serialize and deserialize properly we have to list all possible derived types of CardFilterBase here as XmlArrayItems.
		[XmlArrayItem(Type = typeof(CardFilterSet)),
		XmlArrayItem(Type = typeof(CardFilterBoolProp)),
		XmlArrayItem(Type = typeof(CardFilterCreateTokens)),
		XmlArrayItem(Type = typeof(CardFilterIntProp)),
		XmlArrayItem(Type = typeof(CardFilterStringIntProp)),
		XmlArrayItem(Type = typeof(CardFilterStringProp)),
		XmlArrayItem(Type = typeof(CardFilterSubTypes)),
		XmlArrayItem(Type = typeof(CardFilterEnumProp<CardRarity>)),
		XmlArrayItem(Type = typeof(CardFilterEnumProp<CardSuperType>)),
		XmlArrayItem(Type = typeof(CardFilterEnumProp<CardType>)),
		XmlArrayItem(Type = typeof(CardFilterEnumProp<ColourFlags>)),
        XmlArrayItem(Type = typeof(CardFilterLegalityProp))
		]
		public List<CardFilterBase> Filters
		{
			get { return m_lstFilters; }
			set { m_lstFilters = value; }
		}

		public override string Name
		{
			get { return "Set"; }
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bPass = true;

			if (m_lstFilters.Count > 0)
			{
				bPass = m_lstFilters[0].IsAllowed(ciCard);
				FilterBooleanCompare eCompare = m_lstFilters[0].Compare;
				for (int i = 1; i < m_lstFilters.Count; i++)
				{
					CardFilterBase cfbFilter = m_lstFilters[i];
					switch (eCompare)
					{
						case FilterBooleanCompare.And:
							bPass &= cfbFilter.IsAllowed(ciCard);
							break;
						case FilterBooleanCompare.Or:
							bPass |= cfbFilter.IsAllowed(ciCard);
							break;
						default:
							// Ignore filters with invalid Compare values.
							break;
					}
					eCompare = cfbFilter.Compare;
				}
			}

			if (NegateSet)
				bPass = !bPass;

			return bPass;
		}

		public override string ToString()
		{
			if (NegateSet)
				return Settings.UIStrings["CARD_FILTER_SET_NOT_COMPARE"].Replace("{0}", Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()]);
			else
				return Settings.UIStrings["CARD_FILTER_SET_COMPARE"].Replace("{0}", Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()]);
		}
	}
}
