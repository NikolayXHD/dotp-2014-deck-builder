using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.DotP
{
    public class LegalityTools
    {
        public static List<Legality> AllLegalities;

        public static bool ListHasLegality(List<Legality> legList, Legality leg)
        {
            return legList.Where(L => L.Format == leg.Format && L.Status == leg.Status).Count() > 0;
        }

        public class Legality : IEquatable<Legality>
        {
            public string Format { get; set; }
            public LegalityValue Status { get; set; }

            public bool Equals(Legality other)
            {

                //Check whether the compared object is null. 
                if (Object.ReferenceEquals(other, null)) return false;

                //Check whether the compared object references the same data. 
                if (Object.ReferenceEquals(this, other)) return true;

                //Check whether the products' properties are equal. 
                return Format.Equals(other.Format) && Status.Equals(other.Status);
            }

            // If Equals() returns true for a pair of objects  
            // then GetHashCode() must return the same value for these objects. 

            public override int GetHashCode()
            {
                return (Format == null ? 0 : Format.GetHashCode()) ^ Status.GetHashCode();
            }
        }

        public static bool IsDeckFormatLegal(Deck dkDeck, string strFormat)
        {
            foreach (var ciCard in dkDeck.Cards)
            {
                if (CardFormatLegality(ciCard.Card, strFormat) < ciCard.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public static int CardFormatLegality(CardInfo ciCard, string strFormat)
        {
            List<Legality> legLegalities = ciCard.Legalities;
            if (legLegalities != null)
            {
                foreach (Legality L in legLegalities)
                {
                    if (L.Format == strFormat)
                    {
                        if (L.Status == LegalityValue.Legal)
                            return 4;
                        else if (L.Status == LegalityValue.Restricted)
                            return 1;
                        else
                            return 0;
                    }
                }
            }
            return 0;
        }
    }
}
