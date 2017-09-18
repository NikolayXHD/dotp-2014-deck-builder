using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;

namespace RSN.DotP
{
	public enum LoadImageType
	{
		SearchAll = 0,
		Card,
		Personality,
		Deck,
		Mana,
		Frame,
		Texture,
	}

	public abstract class WadBase
	{
		protected const string AI_PERSONALITY_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\PLANESWALKERS\";
		protected const string AI_PERSONALITY_LOCATION = @"DATA_ALL_PLATFORMS\AI_PERSONALITIES\";
		protected const string CARD_DIRECTORY_LOCATION = @"DATA_ALL_PLATFORMS\CARDS\";
		protected const string CARD_FRAME_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\CARD_FRAMES\";
		protected const string CARD_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\ILLUSTRATIONS\";
		protected const string DECK_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\DECKS\";
		protected const string DECK_LOCATION = @"DATA_ALL_PLATFORMS\DECKS\";
		protected const string FUNCTIONS_LOCATION = @"DATA_ALL_PLATFORMS\FUNCTIONS\";
		protected const string MANA_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\MANA\";
		protected const string TEXT_LOCATION = @"DATA_ALL_PLATFORMS\TEXT_PERMANENT\";
		protected const string TEXTURE_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\";
		protected const string UNLOCKS_LOCATION = @"DATA_ALL_PLATFORMS\UNLOCKS\";

		protected string m_strName;
		protected HashSet<string> m_hsetImages;
		protected Dictionary<string, TdxWrapper> m_dicCachedImages;
		protected Dictionary<string, Dictionary<string, string>> m_dicStringTable;
		protected SortableBindingList<CardInfo> m_lstCards;
		protected SortableBindingList<Deck> m_lstDecks;
		protected SortableBindingList<AiPersonality> m_lstPersonalities;

		// For building Wads
		protected SortedDictionary<string, MemoryStream> m_sdicOutputFiles;

		public WadBase()
		{
			m_sdicOutputFiles = new SortedDictionary<string, MemoryStream>();
			m_dicCachedImages = new Dictionary<string, TdxWrapper>();
		}

		public string Name
		{
			get { return m_strName; }
			// The set here should be used sparingly.
			set { m_strName = value; }
		}

		public SortableBindingList<CardInfo> Cards
		{
			get { return m_lstCards; }
		}

		public HashSet<string> Images
		{
			get { return m_hsetImages; }
		}

		public abstract TdxWrapper LoadImage(string strId, LoadImageType eType = LoadImageType.SearchAll);

		public Dictionary<string, Dictionary<string, string>> StringTable
		{
			get { return m_dicStringTable; }
		}

		public SortableBindingList<Deck> Decks
		{
			get { return m_lstDecks; }
		}

		public SortableBindingList<AiPersonality> AiPersonalities
		{
			get { return m_lstPersonalities; }
		}

		public void AddHeader(bool bForAppIdLinking = false)
		{
			AddFile("HEADER.XML", CreateHeader(bForAppIdLinking));
		}

		public void AddAiPersonality(string strFilename, XmlDocument xdPersonality)
		{
			AddFile(AI_PERSONALITY_LOCATION + strFilename, XmlToMemoryStream(xdPersonality));
		}

		public void AddAiPersonalityImage(string strFilename, TdxWrapper twImage)
		{
			AddFile(AI_PERSONALITY_IMAGES_LOCATION + strFilename, TdxToMemoryStream(twImage));
		}

		public void AddDeck(string strFilename, XmlDocument xdDeck)
		{
			AddFile(DECK_LOCATION + strFilename, XmlToMemoryStream(xdDeck));
		}

		public void AddDeckImage(string strFilename, TdxWrapper twImage)
		{
			AddFile(DECK_IMAGES_LOCATION + strFilename, TdxToMemoryStream(twImage));
		}

		public void AddUnlock(string strFilename, XmlDocument xdUnlock)
		{
			AddFile(UNLOCKS_LOCATION + strFilename, XmlToMemoryStream(xdUnlock));
		}

		public void AddTextPermanent(string strFilename, XmlDocument xdText)
		{
			AddFile(TEXT_LOCATION + strFilename, XmlToMemoryStream(xdText));
		}

		public void AddFile(string strFile, MemoryStream msFile)
		{
			m_sdicOutputFiles.Add(m_strName.ToUpper() + "\\" + strFile, msFile);
		}

		public void AddPlaceholder(string strDir)
		{
			// Make sure we have a trailing back-slash.
			if (!strDir.EndsWith("\\"))
				strDir += "\\";
			AddFile(strDir + "PLACEHOLDER.TXT", null);
		}

		private MemoryStream XmlToMemoryStream(XmlDocument xdDoc)
		{
			MemoryStream msDoc = new MemoryStream();
			xdDoc.Save(msDoc);
			msDoc.Flush();
			// Go ahead and put us at the beginning of the stream.
			msDoc.Seek(0, SeekOrigin.Begin);
			return msDoc;
		}

		private MemoryStream TdxToMemoryStream(TdxWrapper twImage)
		{
			MemoryStream msImage = new MemoryStream();
			twImage.TdxImage.Serialize(msImage);
			msImage.Flush();
			// Go ahead and put us at the beginning of the stream.
			msImage.Seek(0, SeekOrigin.Begin);
			return msImage;
		}

		public abstract string FindTextFile(string strName);

		public abstract Byte[] FindBinaryFile(string strName);

		public abstract void LoadPersonalities(GameDirectory gdData);

		public abstract void LoadDecks(GameDirectory gdData);

		public abstract Dictionary<int, string> LoadUnlocks(GameDirectory gdData);

		// Should overwrite if the Wad/Directory already exists (silently).
		public abstract void WriteWad(string strLocation);

		// Add all files in Wad to target Wad (used when changing between Wad types, creating a Wad from WadDirectory).
		public abstract void WriteToWad(WadBase wTarget);

		protected void LoadIndividualStringTable(Dictionary<string, Dictionary<string, string>> dicStrings, string strXml)
		{
			XmlDocument xdDoc = new XmlDocument();
			xdDoc.LoadXml(strXml);
			foreach (XmlNode xnWorksheet in xdDoc["Workbook"].ChildNodes)
			{
				if (xnWorksheet.Name.Equals("Worksheet", StringComparison.OrdinalIgnoreCase))
				{
					if (xnWorksheet["Table"] != null)
					{
						foreach (XmlNode xnRow in xnWorksheet["Table"].ChildNodes)
						{
							if (xnRow.Name.Equals("Row", StringComparison.OrdinalIgnoreCase))
							{
								int nIndex = 1;		// Cells are 1 indexed
								string strName = string.Empty;
								Dictionary<string, string> dicTable = new Dictionary<string, string>();
								foreach (XmlNode xnCell in xnRow.ChildNodes)
								{
									if (xnCell is XmlElement)
									{
										if (xnCell.Attributes["ss:Index"] != null)
											nIndex = Int32.Parse(xnCell.Attributes["ss:Index"].Value.Trim());
										if (nIndex == 1)
											strName = xnCell.InnerText.Trim().ToUpper();
										else if (nIndex == 3)
											dicTable.Add("en-US", xnCell.InnerText.Trim());
										else if (nIndex == 4)
											dicTable.Add("fr-FR", xnCell.InnerText.Trim());
										else if (nIndex == 5)
											dicTable.Add("es-ES", xnCell.InnerText.Trim());
										else if (nIndex == 6)
											dicTable.Add("de-DE", xnCell.InnerText.Trim());
										else if (nIndex == 7)
											dicTable.Add("it-IT", xnCell.InnerText.Trim());
										else if (nIndex == 9)
											dicTable.Add("jp-JA", xnCell.InnerText.Trim());
										else if (nIndex == 10)
											dicTable.Add("ko-KR", xnCell.InnerText.Trim());
										else if (nIndex == 11)
											dicTable.Add("ru-RU", xnCell.InnerText.Trim());
										else if (nIndex == 12)
											dicTable.Add("pt-BR", xnCell.InnerText.Trim());
										nIndex++;
									}
								}
								if (!dicStrings.ContainsKey(strName))
									dicStrings.Add(strName, dicTable);
							}
						}
					}
				}
			}
		}

		protected MemoryStream CreateHeader(bool bForAppIdLinking)
		{
			XmlDocument xdHeader = new XmlDocument();
			XmlDeclaration xdDec = xdHeader.CreateXmlDeclaration("1.0", null, null);
			xdHeader.AppendChild(xdDec);

			XmlNode xnHeader = xdHeader.CreateElement("WAD_HEADER");
			xdHeader.AppendChild(xnHeader);

			XmlNode xnEntry = xdHeader.CreateElement("ENTRY");
			xnHeader.AppendChild(xnEntry);

			XmlAttribute xaAttr = xdHeader.CreateAttribute("platform");
			xaAttr.Value = "ALL";
			xnEntry.Attributes.Append(xaAttr);

			xaAttr = xdHeader.CreateAttribute("source");
			if (bForAppIdLinking)
				xaAttr.Value = m_strName.ToUpper() + "/DATA_PC/";
			else
				xaAttr.Value = m_strName.ToUpper() + "/DATA_ALL_PLATFORMS/";
			xnEntry.Attributes.Append(xaAttr);

			xaAttr = xdHeader.CreateAttribute("alias");
			xaAttr.Value = "Content";
			xnEntry.Attributes.Append(xaAttr);

			xaAttr = xdHeader.CreateAttribute("order");
			if (bForAppIdLinking)
				xaAttr.Value = "4";
			else
				xaAttr.Value = "3";
			xnEntry.Attributes.Append(xaAttr);

			MemoryStream msReturn = new MemoryStream();
			xdHeader.Save(msReturn);
			return msReturn;
		}
	}
}
