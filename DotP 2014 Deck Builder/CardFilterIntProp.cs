using System;
using System.Reflection;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	public class CardFilterIntProp : CardFilterBase
	{
		public string DataProperty;
		public FilterIntComparisonType Operation;
		public int Value;

		public CardFilterIntProp() : this(string.Empty, FilterIntComparisonType.GreaterThanOrEqualTo, 0, FilterBooleanCompare.And) { }

		public CardFilterIntProp(string strDataProperty, FilterIntComparisonType fictOperation, int nValue, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			DataProperty = strDataProperty;
			Operation = fictOperation;
			Value = nValue;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				int nValue = GetCardValue(ciCard);

				switch (Operation)
				{
					case FilterIntComparisonType.Equal:
						bReturn = (nValue == Value);
						break;
					case FilterIntComparisonType.GreaterThan:
						bReturn = (nValue > Value);
						break;
					case FilterIntComparisonType.LessThan:
						bReturn = (nValue < Value);
						break;
					case FilterIntComparisonType.LessThanOrEqualTo:
						bReturn = (nValue <= Value);
						break;
					case FilterIntComparisonType.NotEqual:
						bReturn = (nValue != Value);
						break;
					case FilterIntComparisonType.GreaterThanOrEqualTo:
					default:
						bReturn = (nValue >= Value);
						break;
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed integer test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
			}

			return bReturn;
		}

		protected virtual int GetCardValue(CardInfo ciCard)
		{
			if (DataProperty.Length == 0)
				throw new InvalidOperationException("DataProperty must be defined.");

			PropertyInfo piProp = ciCard.GetType().GetProperty(DataProperty);
			if (piProp == null)
				throw new InvalidOperationException("DataProperty must refer to an existing property.");

			return (int)piProp.GetValue(ciCard, null);
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
