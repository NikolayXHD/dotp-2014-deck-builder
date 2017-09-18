using System;
using System.Reflection;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	public class CardFilterBoolProp : CardFilterBase
	{
		public string DataProperty;
		public FilterBooleanComparisonType Operation;

		public CardFilterBoolProp() : this(string.Empty, FilterBooleanComparisonType.BoolIs, FilterBooleanCompare.And) { }

		public CardFilterBoolProp(string strDataProperty, FilterBooleanComparisonType fbctOperation, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			DataProperty = strDataProperty;
			Operation = fbctOperation;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				bool bValue = GetCardValue(ciCard);

				switch (Operation)
				{
					case FilterBooleanComparisonType.BoolIsNot:
						bReturn = !bValue;
						break;
					case FilterBooleanComparisonType.BoolIs:
					default:
						bReturn = bValue;
						break;
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed boolean test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
			}

			return bReturn;
		}

		protected virtual bool GetCardValue(CardInfo ciCard)
		{
			if (DataProperty.Length == 0)
				throw new InvalidOperationException("DataProperty must be defined.");

			PropertyInfo piProp = ciCard.GetType().GetProperty(DataProperty);
			if (piProp == null)
				throw new InvalidOperationException("DataProperty must refer to an existing property.");

			return (bool)piProp.GetValue(ciCard, null);
		}

		public override string Name { get { return DataProperty; } }

		public override string ToString()
		{
			string strDataProp = Settings.UIStrings["CARD_FILTER_PROP_" + DataProperty.ToUpper()];
			string strMessage = Settings.UIStrings["CARD_FILTER_BOOL_COMPARISON_" + Operation.ToString().ToUpper()];
			string strCompare = Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()];
			return strMessage.Replace("{0}", strDataProp).Replace("{1}", strCompare);
		}
	}

	public class CardFilterCreateTokens : CardFilterBoolProp
	{
		public CardFilterCreateTokens() : this(FilterBooleanComparisonType.BoolIs, FilterBooleanCompare.And) { }
		public CardFilterCreateTokens(FilterBooleanComparisonType fbctOperation, FilterBooleanCompare fbcCompare) : base("CreateTokens", fbctOperation, fbcCompare) { }

		protected override bool GetCardValue(CardInfo ciCard)
		{
			return ((ciCard.RegisteredTokens != null) && (ciCard.RegisteredTokens.Count > 0));
		}
	}
}
