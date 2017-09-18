using System;
using System.Reflection;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	public class CardFilterStringProp : CardFilterBase
	{
		public string DataProperty;
		public FilterStringComparisonType Operation;
		public string Value;

		public CardFilterStringProp() : this(string.Empty, FilterStringComparisonType.Contains, string.Empty, FilterBooleanCompare.And) { }

		public CardFilterStringProp(string strDataProperty, FilterStringComparisonType fsctOperation, string strValue, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			DataProperty = strDataProperty;
			Operation = fsctOperation;
			Value = strValue;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				string strValue = GetCardValue(ciCard);

				switch (Operation)
				{
					case FilterStringComparisonType.DoesNotContain:
						bReturn = !(strValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) > -1);
						break;
					case FilterStringComparisonType.Equal:
						bReturn = strValue.Equals(Value, StringComparison.CurrentCultureIgnoreCase);
						break;
					case FilterStringComparisonType.NotEqual:
						bReturn = !strValue.Equals(Value, StringComparison.CurrentCultureIgnoreCase);
						break;
					case FilterStringComparisonType.Contains:
					default:
						bReturn = (strValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) > -1);
						break;
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed string test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
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
			string strDataProp = Settings.UIStrings["CARD_FILTER_PROP_" + DataProperty.ToUpper()];
			string strMessage = Settings.UIStrings["CARD_FILTER_STRING_COMPARISON_" + Operation.ToString().ToUpper()];
			string strCompare = Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()];
			return strMessage.Replace("{0}", strDataProp).Replace("{1}", Value).Replace("{2}", strCompare);
		}
	}

	public class CardFilterSubTypes : CardFilterStringProp
	{
		public CardFilterSubTypes() : this(FilterStringComparisonType.Contains, string.Empty, FilterBooleanCompare.And) { }
		public CardFilterSubTypes(FilterStringComparisonType fsctOperation, string strValue, FilterBooleanCompare fbcCompare) : base("SubTypes", fsctOperation, strValue, fbcCompare) { }

		protected override string GetCardValue(CardInfo ciCard)
		{
			string strSubTypes = string.Empty;

			if (ciCard.LocalizedTypeLine.IndexOf("— ") > -1)
			{
				int nStartIndex = ciCard.LocalizedTypeLine.IndexOf("— ") + 2;
				strSubTypes = ciCard.LocalizedTypeLine.Substring(nStartIndex);
			}

			return strSubTypes;
		}
	}
}