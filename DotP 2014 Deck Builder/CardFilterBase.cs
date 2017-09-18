using System;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	// This class is the base class from which all filter objects will be derived.
	public abstract class CardFilterBase
	{
		public FilterBooleanCompare Compare;

		public CardFilterBase() : this(FilterBooleanCompare.And) { }

		public CardFilterBase(FilterBooleanCompare fbcCompare)
		{
			Compare = fbcCompare;
		}

		public abstract string Name { get; }

		public abstract bool IsAllowed(CardInfo ciCard);

		public override string ToString()
		{
			return Settings.UIStrings["CARD_FILTER_" + Name.ToUpper()];
		}
	}
}
