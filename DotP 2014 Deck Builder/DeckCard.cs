using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace RSN.DotP
{
	public class DeckCard : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private CardInfo m_ciCard;
		private int m_nQuantity;
		private int m_nBias;		// A bias level of 1 is no bias.
		private bool m_bPromo;
		private int m_nDeckOrderId;

		public DeckCard(CardInfo ciCard, int nQuantity = 1, int nBias = 1, bool bPromo = false, int nDeckOrderId = -1)
		{
			m_ciCard = ciCard;
			m_nQuantity = nQuantity;
			m_nBias = nBias;
			m_bPromo = bPromo;
			m_nDeckOrderId = nDeckOrderId;
		}

		public CardInfo Card
		{
			get { return m_ciCard; }
		}

		public int Quantity
		{
			get { return m_nQuantity; }
			set
			{
				if (value < 1)
					value = 1;
				if (m_nQuantity != value)
				{
					m_nQuantity = value;
					TriggerPropChange("Quantity");
				}
			}
		}

		public int Bias
		{
			get { return m_nBias; }
			set { m_nBias = value; }
		}

		public bool Promo
		{
			get { return m_bPromo; }
			set { m_bPromo = value; }
		}

		public int OrderId
		{
			get { return m_nDeckOrderId; }
			set { m_nDeckOrderId = value; }
		}

		public string CardDeckName()
		{
			string strName = m_ciCard.Filename;
			if (m_bPromo)
				strName += "#";
			if (m_nBias > 1)
				strName += "@" + m_nBias.ToString();
			return strName;
		}

		private void TriggerPropChange(string strProp)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(strProp));
		}

		// These are card properties that I've exposed from the DeckCard object so
		//	that they can be used for columns in DataGridView since you can't use
		//	"Card.X" for the DataPropertyName.
		public string LocalizedCardName
		{
			get { return m_ciCard.LocalizedName; }
		}

		public string FileName
		{
			get { return m_ciCard.Filename; }
		}

		public string PresentInWad
		{
			get { return m_ciCard.PresentInWad; }
		}

		public string ColourText
		{
			get { return m_ciCard.ColourText; }
		}

		public string LocalizedTypeLine
		{
			get { return m_ciCard.LocalizedTypeLine; }
		}

		public Image CastingCostImage
		{
			get { return m_ciCard.CastingCostImage; }
		}

		public string CastingCost
		{
			get { return m_ciCard.CastingCost; }
		}

		public int MultiverseId
		{
			get { return m_ciCard.MultiverseId; }
		}

		public int ConvertedManaCost
		{
			get { return m_ciCard.ConvertedManaCost; }
		}

		public CardRarity Rarity
		{
			get { return m_ciCard.Rarity; }
		}

		public string Power
		{
			get { return m_ciCard.Power; }
		}

		public string Toughness
		{
			get { return m_ciCard.Toughness; }
		}

		public string Artist
		{
			get { return m_ciCard.Artist; }
		}

		public string Expansion
		{
			get { return m_ciCard.Expansion; }
		}

		public bool Token
		{
			get { return m_ciCard.Token; }
		}
	}
}
