using System;
using System.Reflection;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	// This covers the special cases of Power & Toughness in that they are either integers or * for variable/unknown.
	public class CardFilterStringIntProp : CardFilterBase
	{
		public string DataProperty;
		public FilterIntComparisonType Operation;
		public string Value;

		public CardFilterStringIntProp() : this(string.Empty, FilterIntComparisonType.GreaterThanOrEqualTo, "0", FilterBooleanCompare.And) { }

		public CardFilterStringIntProp(string strDataProperty, FilterIntComparisonType fictOperation, string strValue, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			DataProperty = strDataProperty;
			Operation = fictOperation;
			Value = strValue;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				string strValue = GetCardValue(ciCard);

				if ((strValue == null) || (strValue.Length == 0) || (Value == null) || (Value.Length == 0))
					bReturn = false;
				else
				{
					if ((strValue.Equals("*")) || (Value.Equals("*")))
					{
						switch (Operation)
						{
							case FilterIntComparisonType.Equal:
								bReturn = (strValue.Equals(Value));
								break;
							case FilterIntComparisonType.NotEqual:
								bReturn = !(strValue.Equals(Value));
								break;
							default:
								bReturn = false;
								break;
						}
					}
					else
					{
						int nCardValue = Int32.Parse(strValue);
						int nValue = Int32.Parse(Value);

						switch (Operation)
						{
							case FilterIntComparisonType.Equal:
								bReturn = (nCardValue == nValue);
								break;
							case FilterIntComparisonType.GreaterThan:
								bReturn = (nCardValue > nValue);
								break;
							case FilterIntComparisonType.LessThan:
								bReturn = (nCardValue < nValue);
								break;
							case FilterIntComparisonType.LessThanOrEqualTo:
								bReturn = (nCardValue <= nValue);
								break;
							case FilterIntComparisonType.NotEqual:
								bReturn = (nCardValue != nValue);
								break;
							case FilterIntComparisonType.GreaterThanOrEqualTo:
							default:
								bReturn = (nCardValue >= nValue);
								break;
						}
					}
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed string integer test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
			}

			return bReturn;
		}

		protected virtual string GetCardValue(CardInfo ciCard)
		{
			if (DataProperty.Length == 0)
				throw new InvalidOperationException("DataProperty must be defined.");

			PropertyInfo piProp = ciCard.GetType().GetProperty(DataProperty);
			if (piProp == null)
				throw new InvalidOperationException("DataProperty must refer to an existing property.");

			return piProp.GetValue(ciCard, null) as string;
		}

		public override string Name { get { return DataProperty; } }

		public override string ToString()
		{
			return Settings.UIStrings["CARD_FILTER_PROP_" + DataProperty.ToUpper()] + " " +
				Settings.UIStrings["CARD_FILTER_INT_COMPARISON_" + Operation.ToString().ToUpper()] + " " +
				Value.ToString() + " " +
				Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()];
		}
	}
}
