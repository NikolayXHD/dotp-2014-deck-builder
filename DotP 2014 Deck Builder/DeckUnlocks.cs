using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public class DeckUnlocks
	{
		private int m_nUid;
		private int m_nDeckUid;
		private bool m_bPromo;
		private SortableBindingList<DeckCard> m_lstCards;

		public DeckUnlocks()
		{
			m_lstCards = new SortableBindingList<DeckCard>();
		}

		public DeckUnlocks(GameDirectory gdData, XmlNode xnUnlocks, bool bPromo)
		{
			m_lstCards = LoadCards(gdData, xnUnlocks);
			m_bPromo = bPromo;
		}

		public DeckUnlocks(GameDirectory gdData, string strXml, string strFilename)
		{
			m_lstCards = new SortableBindingList<DeckCard>();
			try
			{
				XmlDocument xdDoc = new XmlDocument();
				xdDoc.LoadXml(strXml);
				XmlNode xnUnlocks = XmlTools.GetChild(xdDoc, "UNLOCKS");
				if (xnUnlocks != null)
				{
					m_nUid = Int32.Parse(XmlTools.GetValueFromAttribute(xnUnlocks, "uid", "-1"));
					m_nDeckUid = Int32.Parse(XmlTools.GetValueFromAttribute(xnUnlocks, "deck_uid", "-1"));
					m_bPromo = XmlTools.GetValueFromAttribute(xnUnlocks, "game_mode").Equals("2");

					// Lastly load the cards.
					m_lstCards = LoadCards(gdData, xnUnlocks);
				}
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Low, "Unable to load unlock file: " + strFilename);
			}
		}

		public DeckUnlocks(DeckUnlocks duToCopy)
		{
			m_nUid = duToCopy.Uid;
			m_nDeckUid = duToCopy.DeckUid;
			m_bPromo = duToCopy.Promo;

			m_lstCards = new SortableBindingList<DeckCard>();
			foreach (DeckCard dcCard in duToCopy.Cards)
				m_lstCards.Add(new DeckCard(dcCard.Card, dcCard.Quantity, dcCard.Bias, dcCard.Promo));
		}

		private SortableBindingList<DeckCard> LoadCards(GameDirectory gdData, XmlNode xnUnlocks)
		{
			SortableBindingList<DeckCard> lstCards = new SortableBindingList<DeckCard>();
			foreach (XmlNode xnItem in xnUnlocks.ChildNodes)
			{
				if (xnItem.Name.Equals("CARD", StringComparison.OrdinalIgnoreCase))
				{
					string strCardName = XmlTools.GetValueFromAttribute(xnItem, "name");
					int nBias = 1;
					bool bPromo = false;
					// Apparently you can mix biased shuffling and promo status like this: <CardFileName>#@<BiasLevel>
					// Check for biased shuffling.
					if (strCardName.IndexOf('@') > -1)
					{
						// Biased shuffling found
						string[] astrParts = strCardName.Split('@');
						strCardName = astrParts[0].Trim();
						nBias = Int32.Parse(astrParts[1].Trim());
					}
					// Check for Promo additive
					if (strCardName.IndexOf('#') > -1)
					{
						strCardName = strCardName.Substring(0, strCardName.IndexOf('#')).Trim();
						bPromo = true;
					}
					int nOrderId = -1;
					int.TryParse(XmlTools.GetValueFromAttribute(xnItem, "deckOrderId"), out nOrderId);
					DeckCard dcCard = null;
					CardInfo ciCard = gdData.GetCardByFileName(strCardName);
					if (ciCard != null)
						dcCard = new DeckCard(ciCard, 1, nBias, bPromo, nOrderId);
					if (dcCard != null)
						lstCards.Add(dcCard);
					else
					{
						// Report that we couldn't load card.
						Settings.ReportError(null, ErrorPriority.Low, "Can't find referenced card: " + strCardName + " for " + (m_bPromo ? "promo" : "regular") + " unlocks (" + m_nUid.ToString() + ") for deck with Id: " + m_nDeckUid.ToString());
					}
				}
			}
			if (lstCards.Count > 0)
			{
				PropertyDescriptorCollection pdcProps = System.ComponentModel.TypeDescriptor.GetProperties(lstCards[0]);
				PropertyDescriptor pdOrder = pdcProps["OrderId"];
				ListSortDescription lsdSort = new ListSortDescription(pdOrder, ListSortDirection.Ascending);
				ListSortDescriptionCollection lsdcSort = new ListSortDescriptionCollection(new ListSortDescription[] { lsdSort });
				lstCards.ApplySort(lsdcSort);
			}

			return lstCards;
		}

		public int Uid
		{
			get { return m_nUid; }
			set { m_nUid = value; }
		}

		public int DeckUid
		{
			get { return m_nDeckUid; }
			set { m_nDeckUid = value; }
		}

		public bool Promo
		{
			get { return m_bPromo; }
			set { m_bPromo = value; }
		}

		public SortableBindingList<DeckCard> Cards
		{
			get { return m_lstCards; }
		}

		public KeyValuePair<int, XmlDocument> Export(int nStartOrderId)
		{
			XmlDocument xdUnlocks = new XmlDocument();

			// The base game uses unlock XMLs that are saved in ANSI and have no XML Declaration so we will do the same.
			//	Uncomment the block below to add a UTF-8 XML Declaration to the file.
			/*XmlDeclaration xdDeclaration = xdDeck.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
			xdDeck.AppendChild(xdDeclaration);//*/

			XmlNode xnUnlocks = xdUnlocks.CreateElement("UNLOCKS");
			XmlAttribute xaAttr = xdUnlocks.CreateAttribute("uid");
			xaAttr.Value = m_nUid.ToString();
			xnUnlocks.Attributes.Append(xaAttr);
			xaAttr = xdUnlocks.CreateAttribute("deck_uid");
			xaAttr.Value = m_nDeckUid.ToString();
			xnUnlocks.Attributes.Append(xaAttr);
			xaAttr = xdUnlocks.CreateAttribute("content_pack");
			xaAttr.Value = "0";
			xnUnlocks.Attributes.Append(xaAttr);
			if (m_bPromo)
			{
				xaAttr = xdUnlocks.CreateAttribute("game_mode");
				xaAttr.Value = "2";
				xnUnlocks.Attributes.Append(xaAttr);
			}
			else
			{
				xaAttr = xdUnlocks.CreateAttribute("game_mode");
				xaAttr.Value = "0";
				xnUnlocks.Attributes.Append(xaAttr);
			}
			xdUnlocks.AppendChild(xnUnlocks);

			int nNextId = CreateCardTags(xdUnlocks, xnUnlocks, nStartOrderId);

			return new KeyValuePair<int, XmlDocument>(nNextId, xdUnlocks);
		}

		public int Export(XmlDocument xdDeck, XmlNode xnUnlocks, int nStartOrderId)
		{
			return CreateCardTags(xdDeck, xnUnlocks, nStartOrderId);
		}

		private int CreateCardTags(XmlDocument xdUnlocks, XmlNode xnUnlocks, int nStartOrderId)
		{
			int nDeckOrderId = nStartOrderId;
			int nUnlockOrderId = 0;
			foreach (DeckCard dcCard in m_lstCards)
			{
				XmlNode xnCard = xdUnlocks.CreateElement("CARD");
				XmlAttribute xaName = xdUnlocks.CreateAttribute("name");
				xaName.Value = dcCard.CardDeckName();
				xnCard.Attributes.Append(xaName);
				XmlAttribute xaDeckOrderId = xdUnlocks.CreateAttribute("deckOrderId");
				xaDeckOrderId.Value = nDeckOrderId.ToString();
				xnCard.Attributes.Append(xaDeckOrderId);
				XmlAttribute xaUnlockOrderId = xdUnlocks.CreateAttribute("unlockOrderId");
				xaUnlockOrderId.Value = nUnlockOrderId.ToString();
				xnCard.Attributes.Append(xaUnlockOrderId);
				// Even though we have a quantity attribute it only causes problems if set to greater than 1.
				XmlAttribute xaQuantity = xdUnlocks.CreateAttribute("quantity");
				xaQuantity.Value = "1";
				xnCard.Attributes.Append(xaQuantity);
				xnUnlocks.AppendChild(xnCard);
				nDeckOrderId++;
				nUnlockOrderId++;
			}

			// Return our next available deckOrderId (used for unlocks)
			return nDeckOrderId;
		}

		private bool HasOrderId(int nOrderId)
		{
			foreach (DeckCard dcCard in m_lstCards)
			{
				if (dcCard.OrderId == nOrderId)
					return true;
			}
			return false;
		}

		public void MergeUnlocks(DeckUnlocks duUnlocks)
		{
			// We don't want to merge with something that has the same UID.
			if (duUnlocks.Uid != Uid)
			{
				foreach (DeckCard dcCard in duUnlocks.Cards)
				{
					// Since we are merging we want to check for conflicting deckOrderIds.
					if (!HasOrderId(dcCard.OrderId))
					{
						// Not already in list so go ahead and add it.
						m_lstCards.Add(dcCard);
					}
					else
					{
						// This OrderId is already used by the Unlocks which is potentially a problem for the end-user.
						//	However, I'm not quite sure how to report this yet as there could be other reasons which aren't really errors.
					}
				}
				if (m_lstCards.Count > 0)
				{
					PropertyDescriptorCollection pdcProps = System.ComponentModel.TypeDescriptor.GetProperties(m_lstCards[0]);
					PropertyDescriptor pdOrder = pdcProps["OrderId"];
					ListSortDescription lsdSort = new ListSortDescription(pdOrder, ListSortDirection.Ascending);
					ListSortDescriptionCollection lsdcSort = new ListSortDescriptionCollection(new ListSortDescription[] { lsdSort });
					m_lstCards.ApplySort(lsdcSort);
				}
			}
		}
	}
}
