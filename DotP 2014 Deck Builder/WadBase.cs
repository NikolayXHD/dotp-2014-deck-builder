using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

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
		public const string AI_PERSONALITY_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\PLANESWALKERS\";
		public const string AI_PERSONALITY_LOCATION = @"DATA_ALL_PLATFORMS\AI_PERSONALITIES\";
		public const string CARD_DIRECTORY_LOCATION = @"DATA_ALL_PLATFORMS\CARDS\";
		public const string CARD_FRAME_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\CARD_FRAMES\";
		public const string CARD_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\ILLUSTRATIONS\";
		public const string DECK_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\DECKS\";
		public const string DECK_LOCATION = @"DATA_ALL_PLATFORMS\DECKS\";
		public const string FUNCTIONS_LOCATION = @"DATA_ALL_PLATFORMS\FUNCTIONS\";
		public const string MANA_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\MANA\";
		public const string SPECS_LOCATION = @"DATA_ALL_PLATFORMS\SPECS\";
		public const string TEXT_LOCATION = @"DATA_ALL_PLATFORMS\TEXT_PERMANENT\";
		public const string TEXTURE_IMAGES_LOCATION = @"DATA_ALL_PLATFORMS\ART_ASSETS\TEXTURES\";
		public const string UNLOCKS_LOCATION = @"DATA_ALL_PLATFORMS\UNLOCKS\";

		protected string m_strName;
		protected HashSet<KeyValuePair<string, LoadImageType>> m_hsetImages;
		protected Dictionary<string, TdxWrapper> m_dicCachedImages;
		protected Dictionary<string, Dictionary<string, string>> m_dicStringTable;
		protected SortableBindingList<CardInfo> m_lstCards;
		protected SortableBindingList<Deck> m_lstDecks;
		protected SortableBindingList<AiPersonality> m_lstPersonalities;

		// For sub-types and ordering
		protected Dictionary<string, Dictionary<string, int>> m_dicSubTypeOrdering;
		protected Dictionary<CardSubTypeArchetypes, List<string>> m_dicSubTypes;

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

		public HashSet<KeyValuePair<string, LoadImageType>> Images
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

		public Dictionary<string, Dictionary<string, int>> SubTypeOrdering
		{
			get { return m_dicSubTypeOrdering; }
		}

		public Dictionary<CardSubTypeArchetypes, List<string>> SubTypes
		{
			get { return m_dicSubTypes; }
		}

		public List<string> GetSubTypes(CardSubTypeArchetypes eType)
		{
			if (m_dicSubTypes != null)
				return m_dicSubTypes[eType];
			return null;
		}

		public void AddHeader(WadHeaderInfo whiInfo = null)
		{
			AddFile("HEADER.XML", CreateHeader(whiInfo));
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
										else
										{
											foreach (LanguageEntry lang in Settings.Languages.Values)
											{
												if (nIndex == lang.TextIndex)
													dicTable.Add(lang.LanguageCode, xnCell.InnerText.Trim());
											}
										}
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

		protected MemoryStream CreateHeader(WadHeaderInfo whiInfo)
		{
			XmlDocument xdHeader = new XmlDocument();
			XmlDeclaration xdDec = xdHeader.CreateXmlDeclaration("1.0", null, null);
			xdHeader.AppendChild(xdDec);

			XmlNode xnHeader = XmlTools.AddElementToNode(xdHeader, xdHeader, "WAD_HEADER");
			XmlNode xnEntry = XmlTools.AddElementToNode(xdHeader, xnHeader, "ENTRY");
			XmlTools.AddAttributeToNode(xdHeader, xnEntry, "platform", "ALL");

			if ((whiInfo != null) && (whiInfo.ForAppIdLinking))
			{
				XmlTools.AddAttributeToNode(xdHeader, xnEntry, "source", m_strName.ToUpper() + "/DATA_PC/");
			}
			else
			{
				XmlTools.AddAttributeToNode(xdHeader, xnEntry, "source", m_strName.ToUpper() + "/DATA_ALL_PLATFORMS/");
			}

			XmlTools.AddAttributeToNode(xdHeader, xnEntry, "alias", "Content");

			if (whiInfo != null)
			{
				if (whiInfo.OrderPriority > 0)
				{
					XmlTools.AddAttributeToNode(xdHeader, xnEntry, "order", whiInfo.OrderPriority.ToString());
				}
				else if (whiInfo.ForAppIdLinking)
				{
					XmlTools.AddAttributeToNode(xdHeader, xnEntry, "order", "4");
				}
				else
				{
					XmlTools.AddAttributeToNode(xdHeader, xnEntry, "order", "3");
				}
			}
			else
			{
				XmlTools.AddAttributeToNode(xdHeader, xnEntry, "order", "3");
			}

			if ((whiInfo != null) && (whiInfo.ContentPackId > 0))
			{
				XmlNode xnContentPack = XmlTools.AddElementToNode(xdHeader, xnHeader, "CONTENTPACK");
				XmlTools.AddAttributeToNode(xdHeader, xnContentPack, "UID", whiInfo.ContentPackId.ToString());
				if (whiInfo.DeckId > 0)
					XmlTools.AddAttributeToNode(xdHeader, xnContentPack, "DECK_UID", whiInfo.DeckId.ToString());

				XmlNode xnPDSection = XmlTools.AddElementToNode(xdHeader, xnContentPack, "PD_SECTION");
				XmlNode xnAppId = XmlTools.AddElementToNode(xdHeader, xnPDSection, "APP_ID");
				XmlTools.AddAttributeToNode(xdHeader, xnAppId, "ID", whiInfo.ContentAppId.ToString());

				if (whiInfo.ContentFlags != WadHeaderContentFlags.Invalid)
				{
					XmlNode xnFlags = XmlTools.AddElementToNode(xdHeader, xnContentPack, "CONTENTFLAGS");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Avatar) == WadHeaderContentFlags.Avatar)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "AVATAR_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Background) == WadHeaderContentFlags.Background)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "BACKGROUND_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Campaign) == WadHeaderContentFlags.Campaign)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "CAMPAIGN_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Deck) == WadHeaderContentFlags.Deck)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "DECK_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Foil) == WadHeaderContentFlags.Foil)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "FOIL_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Font) == WadHeaderContentFlags.Font)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "FONT_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Glossary) == WadHeaderContentFlags.Glossary)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "GLOSSARY_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.LoadingScreen) == WadHeaderContentFlags.LoadingScreen)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "LOADING_SCREEN_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.PlayField) == WadHeaderContentFlags.PlayField)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "PLAYFIELD_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.ReloadAll) == WadHeaderContentFlags.ReloadAll)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "RELOAD_ALL_CONTENT");
					if ((whiInfo.ContentFlags & WadHeaderContentFlags.Unlock) == WadHeaderContentFlags.Unlock)
						XmlTools.AddElementToNode(xdHeader, xnFlags, "UNLOCK_CONTENT");
				}
			}

			MemoryStream msReturn = new MemoryStream();
			xdHeader.Save(msReturn);
			return msReturn;
		}
	}
}
