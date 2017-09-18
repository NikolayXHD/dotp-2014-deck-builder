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
			Value = CleanValue(strValue);
		}

		// Public and Static so that I don't have to rewrite it for the regular filters.
		public static string CleanValue(string strValue)
		{
			string strCleaned = strValue;
			bool bArithFound = false;
			bool bStarFound = false;

			// First we need to loop through and check for invalid characters:
			for (int i = 0; i < strValue.Length; i++)
			{
				char cExamine = strCleaned[i];
				if ((cExamine >= '0') && (cExamine <= '9'))
					continue;
				else if ((cExamine == '+') || (cExamine == '-'))
				{
					if (bArithFound)
					{
						// We now have at least 2 arithmetic characters in the string making it invalid.
						strCleaned = string.Empty;
						break;
					}

					bArithFound = true;
				}
				else if (cExamine == '*')
				{
					if (bStarFound)
					{
						// We now have at least 2 * characters in the string making it invalid.
						strCleaned = string.Empty;
						break;
					}

					bStarFound = true;
				}
				else if ((cExamine == 'x') || (cExamine == 'X'))
				{
					if (bStarFound)
					{
						// We now have at least 2 * characters in the string making it invalid.
						strCleaned = string.Empty;
						break;
					}

					// X Detected (or x) so we need to change it to *.
					strCleaned = strCleaned.Replace('x', '*');
					strCleaned = strCleaned.Replace('X', '*');
					bStarFound = true;
				}
				else if (cExamine == ' ')
				{
					// We have a space so lets clear out all spaces and go back one to start this char again.
					strCleaned = strCleaned.Replace(" ", string.Empty);
					i--;
				}
				else
				{
					// We have an invalid character which invalidates our string.
					strCleaned = string.Empty;
					break;
				}
			}

			return strCleaned;
		}

		public override bool IsAllowed(CardInfo ciCard)
		{
			bool bReturn = true;

			try
			{
				string strValue = CleanValue(GetCardValue(ciCard));

				if ((strValue == null) || (strValue.Length == 0) || (Value == null) || (Value.Length == 0))
					bReturn = false;
				else
				{
					if ((strValue.Contains("*")) || (Value.Contains("*")))
					{
						switch (Operation)
						{
							case FilterIntComparisonType.Equal:
							case FilterIntComparisonType.GreaterThanOrEqualTo:
							case FilterIntComparisonType.LessThanOrEqualTo:
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
