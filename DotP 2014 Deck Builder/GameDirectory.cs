using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public class GameDirectory
	{
		private string m_strGameDirectory;
		private SortableBindingList<WadBase> m_lstWads;
		private SortableBindingList<CardInfo> m_lstCards;
		private SortableBindingList<Deck> m_lstDecks;
		private Dictionary<KeyValuePair<string, LoadImageType>, WadBase> m_dicImages;
		private Dictionary<string, TdxWrapper> m_dicCachedImages;
		private Dictionary<string, Dictionary<string, string>> m_dicStringTable;
		private Dictionary<int, string> m_dicUsedIds;
		// This is a dictionary rather than a list so we can search for personalities by name (uppercase).
		private Dictionary<string, AiPersonality> m_dicPersonalities;
		private SortableBindingList<string> m_lstMusic;

		// For sub-types and ordering
		private Dictionary<string, Dictionary<string, int>> m_dicSubTypeOrdering;
		private Dictionary<CardSubTypeArchetypes, List<string>> m_dicSubTypes;

		public GameDirectory(string strDir)
		{
			m_strGameDirectory = strDir;
		}

		public string GameDir
		{
			get { return m_strGameDirectory; }
		}

		public SortableBindingList<CardInfo> Cards
		{
			get { return m_lstCards; }
		}

		public SortableBindingList<Deck> Decks
		{
			get { return m_lstDecks; }
		}

		public Dictionary<string, Dictionary<string, string>> StringTable
		{
			get { return m_dicStringTable; }
		}

		public Dictionary<int, string> UsedIds
		{
			get { return m_dicUsedIds; }
		}

		public Dictionary<string, AiPersonality> Personalities
		{
			get { return m_dicPersonalities; }
		}

		public SortableBindingList<string> Music
		{
			get { return m_lstMusic; }
		}

		public SortableBindingList<WadBase> LoadedWads
		{
			get { return m_lstWads; }
		}

		public CardInfo GetCardByFileName(string strFileName)
		{
			CardInfo ciCard = null;

			foreach (CardInfo ciTest in m_lstCards)
			{
				if (ciTest.Filename.Equals(strFileName, StringComparison.OrdinalIgnoreCase))
				{
					ciCard = ciTest;
					break;
				}
			}

			return ciCard;
		}

		public Deck GetDeckById(int nUid, string strWad = null)
		{
			Deck dkDeck = null;

			foreach (Deck dkTest in m_lstDecks)
			{
				if ((dkTest.Uid == nUid) && 
					((strWad == null) || 
					((strWad != null) && (dkTest.Wad.Equals(strWad, StringComparison.OrdinalIgnoreCase)))))
				{
					dkDeck = dkTest;
					break;
				}
			}

			return dkDeck;
		}

		public TdxWrapper LoadImage(string strImage, LoadImageType eType = LoadImageType.SearchAll)
		{
			if (strImage != null)
			{
				strImage = strImage.ToUpper();

				if (m_dicCachedImages.ContainsKey(eType.ToString() + ":" + strImage))
					return m_dicCachedImages[eType.ToString() + ":" + strImage];

				KeyValuePair<string, LoadImageType> kvpFound = new KeyValuePair<string,LoadImageType>(string.Empty, LoadImageType.SearchAll);
				if (eType != LoadImageType.SearchAll)
				{
					KeyValuePair<string, LoadImageType> kvpTest = new KeyValuePair<string, LoadImageType>(strImage, eType);
					if (m_dicImages.ContainsKey(kvpTest))
						kvpFound = kvpTest;
				}
				else
				{
					foreach (KeyValuePair<string, LoadImageType> kvpTest in m_dicImages.Keys)
					{
						if (kvpTest.Key.Equals(strImage, StringComparison.OrdinalIgnoreCase))
						{
							kvpFound = kvpTest;
							break;
						}
					}
				}

				if (kvpFound.Key.Length > 0)
				{
					TdxWrapper tdx = m_dicImages[kvpFound].LoadImage(strImage, eType);
					if (Settings.GetSetting("MaintainImageCache", Settings.MAINTAIN_IMAGE_CACHE_DEFAULT))
					{
						m_dicCachedImages.Add(eType.ToString() + ":" + strImage, tdx);
					}
					return tdx;
				}
				else
					return null;
			}
			else
				return null;
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

		private void MergeInStringTable(Dictionary<string, Dictionary<string, string>> dicToMerge)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> kvString in dicToMerge)
			{
				if (!m_dicStringTable.ContainsKey(kvString.Key))
					m_dicStringTable.Add(kvString.Key, kvString.Value);
			}
		}

		private void MergeInSubTypeOrdering(Dictionary<string, Dictionary<string, int>> dicToMerge)
		{
			if (dicToMerge != null)
			{
				foreach (string strLang in dicToMerge.Keys)
				{
					Dictionary<string, int> dicLangToMerge = dicToMerge[strLang];
					Dictionary<string, int> dicLang;
					if (!m_dicSubTypeOrdering.ContainsKey(strLang))
					{
						dicLang = new Dictionary<string, int>();
						m_dicSubTypeOrdering.Add(strLang, dicLang);
					}
					else
						dicLang = m_dicSubTypeOrdering[strLang];

					foreach (string strSubType in dicLangToMerge.Keys)
					{
						if (!dicLang.ContainsKey(strSubType))
							dicLang.Add(strSubType, dicLangToMerge[strSubType]);
					}
				}
			}
		}

		private void MergeInSubTypes(Dictionary<CardSubTypeArchetypes, List<string>> dicToMerge)
		{
			if (dicToMerge != null)
			{
				foreach (CardSubTypeArchetypes eType in dicToMerge.Keys)
				{
					List<string> lstTypesToMerge = dicToMerge[eType];
					List<string> lstTypes;
					if (!m_dicSubTypes.ContainsKey(eType))
					{
						lstTypes = new List<string>();
						m_dicSubTypes.Add(eType, lstTypes);
					}
					else
						lstTypes = m_dicSubTypes[eType];

					foreach (string strSubType in lstTypesToMerge)
					{
						if (!lstTypes.Contains(strSubType))
							lstTypes.Add(strSubType);
					}
				}
			}
		}

		public string GetLocalizedString(string strId)
		{
			strId = strId.Trim().ToUpper();
			if (m_dicStringTable != null)
			{
				if (m_dicStringTable.ContainsKey(strId))
				{
					Dictionary<string, string> dicText = m_dicStringTable[strId];
					if (dicText.ContainsKey(Settings.Language.LanguageCode))
						return dicText[Settings.Language.LanguageCode];
					else if ((Settings.Language.MasqueradeAsLangCode != null) && (dicText.ContainsKey(Settings.Language.MasqueradeAsLangCode)))
						return dicText[Settings.Language.MasqueradeAsLangCode];
					else if (dicText.ContainsKey("en-US"))
						return dicText["en-US"];
					else
						return "[" + strId + "]";
				}
				else
					return "[" + strId + "]";
			}
			else
				return "[" + strId + "]";
		}

		public void LoadMusic()
		{
			string strDir = m_strGameDirectory;
			if (!strDir.EndsWith("\\"))
				strDir += "\\";

			m_lstMusic = new SortableBindingList<string>();
			if (Directory.Exists(strDir + "Audio\\Music"))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(strDir + "Audio\\Music", "*.mp3");
				foreach (string strFile in files)
					m_lstMusic.Add(Path.GetFileNameWithoutExtension(strFile));
			}
		}

		// This is to make setting the loadable Wad names easier.
		private bool IsAcceptableWadName(string strName)
		{
			// It seems the game loads those that start with DATA_DLC_ or DATA_DECKS_ (case insensitive).
			//	The special case is DATA_CORE.WAD which is always loaded.
			if ((Path.GetFileName(strName).StartsWith("data_core", StringComparison.OrdinalIgnoreCase)) ||
				(Path.GetFileName(strName).StartsWith("data_dlc_", StringComparison.OrdinalIgnoreCase)) ||
				(Path.GetFileName(strName).StartsWith("data_decks_", StringComparison.OrdinalIgnoreCase)))
			{
				// Check for special WADs that are generated by the Deck Builder that should not be loaded.
				if (Path.GetFileNameWithoutExtension(strName).Equals("data_dlc_combined_spec_data", StringComparison.OrdinalIgnoreCase))
					return false;
				else
					return true;
			}
			else
				return false;
		}

		public void LoadWads()
		{
			m_dicStringTable = new Dictionary<string, Dictionary<string, string>>();
			m_dicCachedImages = new Dictionary<string, TdxWrapper>();
			m_dicUsedIds = new Dictionary<int, string>();
			m_dicSubTypeOrdering = new Dictionary<string, Dictionary<string, int>>();
			m_dicSubTypes = new Dictionary<CardSubTypeArchetypes, List<string>>();

			if (Directory.Exists(m_strGameDirectory))
			{
				// Since the game tends to load packed WADs before unpacked ones we will do the same.

				m_lstWads = new SortableBindingList<WadBase>();
				m_lstCards = new SortableBindingList<CardInfo>();
				m_dicImages = new Dictionary<KeyValuePair<string, LoadImageType>, WadBase>();

				// Check for packed WADs (actual WAD files) that the game might actually load.
				IEnumerable<string> files = Directory.EnumerateFiles(m_strGameDirectory, "*.wad");
				foreach (string strFile in files)
				{
					if (IsAcceptableWadName(strFile))
					{
						try
						{
							WadWrapper wad = new WadWrapper(strFile, this);
							m_lstWads.Add(wad);
							foreach (CardInfo ciCard in wad.Cards)
								m_lstCards.Add(ciCard);
                            wad.Cards.Clear();
							foreach (KeyValuePair<string, LoadImageType> kvpImage in wad.Images)
							{
								if (!m_dicImages.ContainsKey(kvpImage))
									m_dicImages.Add(kvpImage, wad);
							}
							MergeInStringTable(wad.StringTable);

							// Merge in SPECS (if any)
							MergeInSubTypeOrdering(wad.SubTypeOrdering);
							MergeInSubTypes(wad.SubTypes);
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to load packed wad:" + strFile);
						}
					}
				}

				// Check for unpack WADs (directories with Header XMLs).
				IEnumerable<string> dirs = Directory.EnumerateDirectories(m_strGameDirectory);
				foreach (string strDir in dirs)
				{
					if (IsAcceptableWadName(strDir))
					{
						if (File.Exists(strDir + @"\header.xml"))
						{
							try
							{
								WadDirectory wad = new WadDirectory(strDir, this);
								m_lstWads.Add(wad);
								foreach (CardInfo ciCard in wad.Cards)
									m_lstCards.Add(ciCard);
                                wad.Cards.Clear();
                                foreach (KeyValuePair<string, LoadImageType> kvpImage in wad.Images)
								{
									if (m_dicImages.ContainsKey(kvpImage))
										m_dicImages[kvpImage] = wad;
									else
										m_dicImages.Add(kvpImage, wad);
								}
								MergeInStringTable(wad.StringTable);
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to load wad directory:" + strDir);
							}
						}
					}
				}

				// We should load our personalities before loading the decks so that the decks can get associated personalities.
				m_dicPersonalities = new Dictionary<string, AiPersonality>();
				foreach (WadBase wad in m_lstWads)
				{
					try
					{
						wad.LoadPersonalities(this);
						foreach (AiPersonality ap in wad.AiPersonalities)
						{
							if (!m_dicPersonalities.ContainsKey(ap.Filename.ToUpper()))
								m_dicPersonalities.Add(ap.Filename.ToUpper(), ap);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to load personalities for wad:" + wad.Name);
					}
				}

				// Now that we have loaded all the "Wads" now we load the decks from those wads.
				m_lstDecks = new SortableBindingList<Deck>();
				foreach (WadBase wad in m_lstWads)
				{
					try
					{
						wad.LoadDecks(this);
						foreach (Deck deck in wad.Decks)
						{
							m_lstDecks.Add(deck);
							if (m_dicUsedIds.ContainsKey(deck.Uid))
							{
								// Only report if the filenames aren't the same, if they are the same it is due to one overriding another.
								if (!deck.FileName.Equals(m_dicUsedIds[deck.Uid], StringComparison.OrdinalIgnoreCase))
								{
									// If the two UIDs that are conflicting are both land pools then we
									//	don't particularly care since the game doesn't seem to have a
									//	problem with that, but if it is a land pool and something else
									//	then we have a problem.
									if ((m_dicUsedIds[deck.Uid].EndsWith("_LAND_POOL", StringComparison.OrdinalIgnoreCase)) && (deck.IsLandPool))
										Settings.ReportError(null, ErrorPriority.Zero, deck.FileName + " uses a UID that conflicts with " + m_dicUsedIds[deck.Uid] + ".");
									else
										Settings.ReportError(null, ErrorPriority.Medium, deck.FileName + " uses a UID that conflicts with " + m_dicUsedIds[deck.Uid] + ".");
								}
							}
							else
								m_dicUsedIds.Add(deck.Uid, deck.FileName);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to load decks for wad:" + wad.Name);
					}
				}

				try
				{
					// All decks are loaded so try and associate all of their land pools.
					foreach (Deck deck in m_lstDecks)
					{
						if (deck.IsLandPool)
						{
							// This is a land pool so try and find the deck this goes with.
							string strDeckName = deck.FileName.ToUpper().Replace("_LAND_POOL", string.Empty);
							foreach (Deck dkTest in m_lstDecks)
							{
								if (dkTest.FileName.Equals(strDeckName, StringComparison.OrdinalIgnoreCase))
								{
									dkTest.LandPool = deck;
									break;
								}
							}
						}
					}
				}
				catch (Exception e)
				{
					Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to associate decks.");
				}

				// Now that all the decks have been loaded now we need to load and associate their unlocks.
				//	Additionally update the used ids.
				foreach (WadBase wad in m_lstWads)
				{
					try
					{
						Dictionary<int, string> dicIds = wad.LoadUnlocks(this);
						foreach (KeyValuePair<int, string> kvId in dicIds)
						{
							if (m_dicUsedIds.ContainsKey(kvId.Key))
							{
								// Only report if the filenames aren't the same, if they are the same it is due to one overriding another.
								if (!kvId.Value.Equals(m_dicUsedIds[kvId.Key], StringComparison.OrdinalIgnoreCase))
									Settings.ReportError(null, ErrorPriority.Low, kvId.Value + " uses a UID that conflicts with " + m_dicUsedIds[kvId.Key] + ".");
							}
							else
								m_dicUsedIds.Add(kvId.Key, kvId.Value);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Critical, "Unhandled exception when trying to load unlocks for wad:" + wad.Name);
					}
				}
			}
		}

        public void Dispose()
        {
            if (m_dicPersonalities != null && m_dicPersonalities.Count > 0)
            {
                foreach (var P in m_dicPersonalities.Values)
                {
                    P.Dispose();
                }
                m_dicPersonalities.Clear();
                m_dicPersonalities = null;
            }
            if (m_lstDecks != null && m_lstDecks.Count > 0)
            {
                foreach (var D in m_lstDecks)
                {
                    D.Dispose();
                }
                m_lstDecks.Clear();
                m_lstDecks = null;
            }
            if (m_dicImages != null && m_dicImages.Count > 0)
            {
                foreach (var W in m_dicImages.Values)
                {
                    W.Dispose();
                }
                m_dicImages.Clear();
                m_dicImages = null;
            }
            if (m_dicCachedImages != null && m_dicCachedImages.Count > 0)
            {
                foreach (var T in m_dicCachedImages.Values)
                {
                    T.Dispose();
                }
                m_dicCachedImages.Clear();
                m_dicCachedImages = null;
            }
            if (m_lstCards != null && m_lstCards.Count > 0)
            {
                foreach (var C in m_lstCards)
                {
                    C.Dispose();
                }
                m_lstCards.Clear();
                m_lstCards = null;
            }
            if (m_lstWads != null && m_lstWads.Count > 0)
            {
                foreach (var W in m_lstWads)
                {
                    W.Dispose();
                }
                m_lstWads.Clear();
                m_lstWads = null;
            }
        }

        public void ResetImageCache()
        {
            if (m_dicCachedImages != null && m_dicCachedImages.Count > 0)
            {
                foreach (var T in m_dicCachedImages.Values)
                {
                    T.Dispose();
                }
                m_dicCachedImages.Clear();
                m_dicCachedImages = null;
            }
            m_dicCachedImages = new Dictionary<string, TdxWrapper>();
        }
	}
}
