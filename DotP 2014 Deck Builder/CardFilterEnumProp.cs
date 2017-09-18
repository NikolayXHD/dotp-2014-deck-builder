using System;
using System.Reflection;
using System.Text;
using RSN.Tools;

namespace RSN.DotP
{
	public class CardFilterEnumProp<T> : CardFilterBase where T: struct, IConvertible, IComparable, IFormattable
	{
		public string DataProperty;
		public FilterEnumComparisonType Operation;
		public T Value;

		public CardFilterEnumProp() : this(string.Empty, FilterEnumComparisonType.EnumIs, default(T), FilterBooleanCompare.And) { }

		public CardFilterEnumProp(string strDataProperty, FilterEnumComparisonType fectOperation, T oValue, FilterBooleanCompare fbcCompare)
			: base(fbcCompare)
		{
			DataProperty = strDataProperty;
			Operation = fectOperation;
			Value = oValue;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				T oValue = GetCardValue(ciCard);

				switch (Operation)
				{
					case FilterEnumComparisonType.EnumIsNot:
						bReturn = !((Convert.ToInt32(oValue) & Convert.ToInt32(Value)) == Convert.ToInt32(Value));
						break;
					case FilterEnumComparisonType.EnumIs:
					default:
						bReturn = ((Convert.ToInt32(oValue) & Convert.ToInt32(Value)) == Convert.ToInt32(Value));
						break;
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed enum test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
			}

			return bReturn;
		}

		protected virtual T GetCardValue(CardInfo ciCard)
		{
			if (DataProperty.Length == 0)
				throw new InvalidOperationException("DataProperty must be defined.");

			PropertyInfo piProp = ciCard.GetType().GetProperty(DataProperty);
			if (piProp == null)
				throw new InvalidOperationException("DataProperty must refer to an existing property.");

			return (T)piProp.GetValue(ciCard, null);
		}

		public override string Name { get { return DataProperty; } }

		public override string ToString()
		{
			string strDataProp = Settings.UIStrings["CARD_FILTER_PROP_" + DataProperty.ToUpper()];
			string strMessage = Settings.UIStrings["CARD_FILTER_ENUM_COMPARISON_" + Operation.ToString().ToUpper()];
			string strCompare = Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()];
			string strValue = null;
			// Unfortunately due to how the colour and type strings are already named I need to choose between the valid specializations to get the value prefix string.
			if (typeof(T) == typeof(CardRarity))
				strValue = Settings.UIStrings["RARITY_" + Value.ToString().ToUpper()];
			else if (typeof(T) == typeof(CardSuperType))
				strValue = Settings.UIStrings["CARD_SUPERTYPE_" + Value.ToString().ToUpper()];
			else if (typeof(T) == typeof(CardType))
				strValue = Settings.UIStrings["CARD_TYPE_" + Value.ToString().ToUpper()];
			else if (typeof(T) == typeof(ColourFlags))
				strValue = Settings.UIStrings["MANA_" + Value.ToString().ToUpper()];
			else
			{
				// Unknown specialization, attempt to just use the type name as the prefix.
				strValue = Settings.UIStrings[typeof(T).Name + "_" + Value.ToString().ToUpper()];
			}
			return strMessage.Replace("{0}", strDataProp).Replace("{1}", strValue).Replace("{2}", strCompare);
		}
	}
}
