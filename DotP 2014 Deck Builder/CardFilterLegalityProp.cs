using RSN.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.DotP
{
    public class CardFilterLegalityProp : CardFilterBase
    {
        public string DataProperty;
        public FilterLegalityComparisonType Operation;
        public LegalityValue Value;

        public CardFilterLegalityProp() : this(string.Empty, FilterLegalityComparisonType.GreaterThanOrEqualTo, LegalityValue.Unknown, FilterBooleanCompare.And) { }

        public CardFilterLegalityProp(string strDataProperty, FilterLegalityComparisonType flctOperation, LegalityValue lvValue, FilterBooleanCompare fbcCompare)
            : base(fbcCompare)
        {
            DataProperty = strDataProperty;
            Operation = flctOperation;
            Value = lvValue;
        }

        public override bool IsAllowed(CardInfo ciCard)
        {
            bool bReturn = true;

            try
            {
                LegalityValue lvValue = GetCardValue(ciCard);

                if ((lvValue == LegalityValue.Unknown) && (Value != LegalityValue.Unknown))
                    bReturn = false;
                else
                {
                    switch (Operation)
                    {
                        case FilterLegalityComparisonType.Equal:
                            bReturn = (lvValue == Value);
                            break;
                        case FilterLegalityComparisonType.GreaterThan:
                            bReturn = (lvValue > Value);
                            break;
                        case FilterLegalityComparisonType.LessThan:
                            bReturn = (lvValue < Value);
                            break;
                        case FilterLegalityComparisonType.LessThanOrEqualTo:
                            bReturn = (lvValue <= Value);
                            break;
                        case FilterLegalityComparisonType.NotEqual:
                            bReturn = (lvValue != Value);
                            break;
                        case FilterLegalityComparisonType.GreaterThanOrEqualTo:
                        default:
                            bReturn = (lvValue >= Value);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Settings.ReportError(e, ErrorPriority.Medium, "IsAllowed string integer test encountered an error processing " + DataProperty + " on card " + ciCard.Filename + " in wad " + ciCard.PresentInWad + ".");
            }

            return bReturn;
        }

        protected virtual LegalityValue GetCardValue(CardInfo ciCard)
        {
            if (DataProperty.Length == 0)
                throw new InvalidOperationException("DataProperty must be defined.");
            if (ciCard.Legalities == null)
                return LegalityValue.Unknown;
            try
            {
                var Legality = ciCard.Legalities.Where(L => L.Format == DataProperty).First();
                if (Legality != null)
                    return Legality.Status;
                else
                    return LegalityValue.Banned;
            }
            catch
            {
                return LegalityValue.Banned;
            }
        }

        public override string Name { get { return DataProperty; } }

        public override string ToString()
        {
            return "Format: " + DataProperty + " " +
                Settings.UIStrings["CARD_FILTER_INT_COMPARISON_" + Operation.ToString().ToUpper()] + " " +
                Value.ToString() + " " +
                Settings.UIStrings["CARD_FILTER_COMPARE_" + Compare.ToString().ToUpper()];
        }
    }
}
