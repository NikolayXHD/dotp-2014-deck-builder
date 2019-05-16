using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	/**
	 * This class is intended to encapsulate a directory that is set up to be treated like a WAD by the game.
	 * For example this should support unpacked WADs that have been setup so that the original wad is not needed by the game and the game uses this directory instead.
	 */
	public class WadDirectory : WadBase
	{
		private string m_strFullDir;

		// This constructor is mainly for writing out a "Wad Directory", as such it does no loading.
		public WadDirectory(string strDir)
			: base()
		{
			// Directory name doubles as the "WAD" name, but first we need to cut out the rest of the path.
			// If we have a trailing backslash then remove it.
			if (strDir.EndsWith("\\"))
				strDir = strDir.Substring(0, strDir.Length - 1);

			// Now that we know the state of trailing backslashes.
			m_strFullDir = strDir + "\\";
			m_strName = Path.GetFileNameWithoutExtension(strDir);
		}

		public WadDirectory(string strDir, GameDirectory gdData)
			: base()
		{
			// Directory name doubles as the "WAD" name, but first we need to cut out the rest of the path.
			// If we have a trailing backslash then remove it.
			if (strDir[strDir.Length - 1].Equals('\\'))
				strDir = strDir.Substring(0, strDir.Length - 1);

			// Now that we know the state of trailing backslashes.
			m_strFullDir = strDir + "\\";
			m_strName = Path.GetFileNameWithoutExtension(strDir);

			// Load Header
			m_xdHeader = LoadHeader();
			ReadHeaderXml(m_xdHeader);

			// Load Cards & Images
			m_lstCards = LoadCards(gdData);
			m_hsetImages = LoadImageList();

			// Load up all the strings present in TEXT_PERMANENT
			m_dicStringTable = LoadStringTable();

			// Load up the SPECS present (if any)
			LoadSpecs();
		}

		private XmlDocument LoadHeader()
		{
			XmlDocument xdHeader = null;

			string strHeader = FindTextFile("HEADER.XML");
			if ((strHeader != null) && (strHeader.Length > 0))
			{
				try
				{
					xdHeader = new XmlDocument();
					xdHeader.LoadXml(strHeader);
				}
				catch (Exception e)
				{
					xdHeader = null;
					Settings.ReportError(e, ErrorPriority.Low, "Wad " + m_strName + " has a header that could not be loaded, header text follows.\n" + strHeader);
				}
			}

			return xdHeader;
		}

		private Dictionary<string, Dictionary<string, string>> LoadStringTable()
		{
			Dictionary<string, Dictionary<string, string>> dicStringTable = new Dictionary<string, Dictionary<string, string>>();

			if (Directory.Exists(m_strFullDir + TEXT_LOCATION))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + TEXT_LOCATION, "*.xml");
				foreach (string strFile in files)
				{
					try
					{
						string strXml = FileTools.ReadFileString(strFile);
						// Load string table from this file.
						LoadIndividualStringTable(dicStringTable, strXml);
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read string table: " + strFile);
					}
				}
			}

			return dicStringTable;
		}

		private SortableBindingList<CardInfo> LoadCards(GameDirectory gdData)
		{
			SortableBindingList<CardInfo> set = new SortableBindingList<CardInfo>();

			if (Directory.Exists(m_strFullDir + CARD_DIRECTORY_LOCATION))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + CARD_DIRECTORY_LOCATION, "*.xml");
				foreach (string strFile in files)
				{
					try
					{
						string strXml = FileTools.ReadFileString(strFile).Trim();
						if (strXml.Length > 0)
						{
							try
							{
								CardInfo ciCard = new CardInfo(strFile, strXml, m_strName, gdData);
								if (ciCard.Filename == null)
									Settings.ReportError(null, ErrorPriority.Low, "Card (" + strFile + ") could not be loaded due to missing the CARD_V2 block in wad " + m_strName + ".");
								else if (ciCard.Filename.Length <= 0)
									Settings.ReportError(null, ErrorPriority.Low, "Card (" + strFile + ") could not be properly loaded due to missing or malformed FILENAME tag in wad " + m_strName + ".");
								else
								{
									set.Add(ciCard);
									if (!ciCard.Filename.Equals(ciCard.ActualFilename, StringComparison.OrdinalIgnoreCase))
										Settings.ReportError(null, ErrorPriority.Low, "Card (" + ciCard.ActualFilename + ") in wad " + m_strName + " has a FILENAME tag that does not match which will cause problems in-game: " + ciCard.Filename);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Low, "Unable to load card: " + strFile);
							}
						}
						else
							Settings.ReportError(null, ErrorPriority.Low, "Unable to load card because there is nothing to load: " + strFile);
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read card: " + strFile);
					}
				}
			}

			return set;
		}

		private HashSet<KeyValuePair<string, LoadImageType>> LoadImageList()
		{
			HashSet<KeyValuePair<string, LoadImageType>> set = new HashSet<KeyValuePair<string, LoadImageType>>();

			LoadImageListFrom(set, m_strFullDir + CARD_IMAGES_LOCATION, LoadImageType.Card);
			LoadImageListFrom(set, m_strFullDir + DECK_IMAGES_LOCATION, LoadImageType.Deck);
			LoadImageListFrom(set, m_strFullDir + AI_PERSONALITY_IMAGES_LOCATION, LoadImageType.Personality);
			LoadImageListFrom(set, m_strFullDir + CARD_FRAME_IMAGES_LOCATION, LoadImageType.Frame);
			LoadImageListFrom(set, m_strFullDir + TEXTURE_IMAGES_LOCATION, LoadImageType.Texture);
			LoadImageListFrom(set, m_strFullDir + MANA_IMAGES_LOCATION, LoadImageType.Mana);
			LoadImageListFrom(set, m_strFullDir + FRONT_END_IMAGES_LOCATION, LoadImageType.FrontEnd);

			return set;
		}

		private void LoadImageListFrom(HashSet<KeyValuePair<string, LoadImageType>> set, string strDir, LoadImageType eType)
		{
			if (Directory.Exists(strDir))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(strDir, "*.tdx");
				foreach (string strFile in files)
				{
					string strId = Path.GetFileNameWithoutExtension(strFile);
					set.Add(new KeyValuePair<string, LoadImageType>(strId.ToUpper(), eType));
				}
			}
		}

		public override string FindTextFile(string strName)
		{
			string strReturn = null;

			using (FileStream fs = CheckDirForFile(m_strFullDir.Substring(0, m_strFullDir.Length - 1), strName))
			{
				strReturn = (new StreamReader(fs)).ReadToEnd();
			}

			return strReturn;
		}

		private FileStream CheckDirForFile(string strDir, string strName)
		{
			string strFullName = strDir + "\\" + strName;
			if (File.Exists(strFullName))
			{
				return new FileStream(strFullName, FileMode.Open, FileAccess.Read);
			}
			else
			{
				IEnumerable<string> dirs = Directory.EnumerateDirectories(strDir);
				foreach (string strSubDir in dirs)
				{
					FileStream fs = CheckDirForFile(strSubDir, strName);
					if (fs != null)
						return fs;
				}
			}
			return null;
		}

		public override byte[] FindBinaryFile(string strName)
		{
			byte[] abytReturn = null;

			using (FileStream fs = CheckDirForFile(m_strFullDir.Substring(0, m_strFullDir.Length - 1), strName))
			{
				abytReturn = (new BinaryReader(fs)).ReadBytes((int)fs.Length);
			}

			return abytReturn;
		}

		public override TdxWrapper LoadImage(string strId, LoadImageType eType = LoadImageType.SearchAll)
		{
			if (eType == LoadImageType.SearchAll)
				return LoadImageSearch(strId);
			else
				return LoadImageSpecific(strId, eType);
		}

		private TdxWrapper LoadImageSpecific(string strId, LoadImageType eType)
		{
			// Check for image in cache.
			if (m_dicCachedImages.ContainsKey(eType.ToString() + ":" + strId.ToUpper()))
				return m_dicCachedImages[eType.ToString() + ":" + strId.ToUpper()];

			string strFile = string.Empty;
			switch (eType)
			{
				case LoadImageType.Card:
					strFile = m_strFullDir + CARD_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.Deck:
					strFile = m_strFullDir + DECK_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.Frame:
					strFile = m_strFullDir + CARD_FRAME_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.FrontEnd:
					strFile = m_strFullDir + FRONT_END_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.Mana:
					strFile = m_strFullDir + MANA_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.Personality:
					strFile = m_strFullDir + AI_PERSONALITY_IMAGES_LOCATION + strId + ".tdx";
					break;
				case LoadImageType.Texture:
					strFile = m_strFullDir + TEXTURE_IMAGES_LOCATION + strId + ".tdx";
					break;
			}

			TdxWrapper tdx = null;

			if (File.Exists(strFile))
			{
				tdx = new TdxWrapper();
				try
				{
					tdx.LoadTdx(strFile);
					// Cache the image to avoid having to load it multiple times.
					if (Settings.GetSetting("MaintainImageCache", Settings.MAINTAIN_IMAGE_CACHE_DEFAULT))
						m_dicCachedImages.Add(eType.ToString() + ":" + strId.ToUpper(), tdx);
				}
				catch (Exception e)
				{
					tdx = null;
					Settings.ReportError(e, ErrorPriority.Low, "Could not load: " + strFile);
				}
			}

			return tdx;
		}

		private TdxWrapper LoadImageSearch(string strId)
		{
			// Check for image in cache.
			if (m_dicCachedImages.ContainsKey(strId.ToUpper()))
				return m_dicCachedImages[strId.ToUpper()];

			string strFile = string.Empty;
			if (File.Exists(m_strFullDir + CARD_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + CARD_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + DECK_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + DECK_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + AI_PERSONALITY_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + AI_PERSONALITY_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + CARD_FRAME_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + CARD_FRAME_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + TEXTURE_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + TEXTURE_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + MANA_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + MANA_IMAGES_LOCATION + strId + ".tdx";
			else if (File.Exists(m_strFullDir + FRONT_END_IMAGES_LOCATION + strId + ".tdx"))
				strFile = m_strFullDir + FRONT_END_IMAGES_LOCATION + strId + ".tdx";

			TdxWrapper tdx = null;

			if (strFile.Length > 0)
			{
				tdx = new TdxWrapper();
				try
				{
					tdx.LoadTdx(strFile);
					// Cache the image to avoid having to load it multiple times.
					if (Settings.GetSetting("MaintainImageCache", Settings.MAINTAIN_IMAGE_CACHE_DEFAULT)) 
						m_dicCachedImages.Add(strId.ToUpper(), tdx);
				}
				catch (Exception e)
				{
					tdx = null;
					Settings.ReportError(e, ErrorPriority.Low, "Could not load: " + strFile);
				}
			}

			return tdx;
		}

		public override void LoadPersonalities(GameDirectory gdData)
		{
			m_lstPersonalities = new SortableBindingList<AiPersonality>();
			if (Directory.Exists(m_strFullDir + AI_PERSONALITY_LOCATION))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + AI_PERSONALITY_LOCATION, "*.xml");
				foreach (string strFile in files)
				{
					string strFileName = Path.GetFileName(strFile).ToUpper();
					try
					{
						string strXml = FileTools.ReadFileString(strFile);
						try
						{
							AiPersonality ap = new AiPersonality(gdData, strXml, strFileName);
							//Check if the personality was in a deck wad. If it was, don't consider it built-in.
                            if (
                                //Don't include all deck wads. Only the official deck wads.
                                m_strFullDir.IndexOf("\\DATA_DECKS_", StringComparison.OrdinalIgnoreCase) < 0 ||
                                m_strFullDir.IndexOf("\\DATA_DECKS_D14\\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m_strFullDir.IndexOf("\\DATA_DECKS_E14\\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m_strFullDir.IndexOf("\\DATA_DECKS_F14_PACK1\\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m_strFullDir.IndexOf("\\DATA_DECKS_F14_PACK2\\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m_strFullDir.IndexOf("\\DATA_DECKS_F14_PACK3\\", StringComparison.OrdinalIgnoreCase) >= 0
                                )
                            {
								// This was loaded from the game directory and not a custom deck so we consider it built-in because the user already has it.
								ap.BuiltIn = true;
                            }
							m_lstPersonalities.Add(ap);
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Medium, "Unable to load AI Personality: " + strFile);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read AI Personality: " + strFile);
					}
				}
			}
		}

		public override void LoadDecks(GameDirectory gdData)
		{
			m_lstDecks = new SortableBindingList<Deck>();
			if (Directory.Exists(m_strFullDir + DECK_LOCATION))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + DECK_LOCATION, "*.xml");
				foreach (string strFile in files)
				{
					string strFileName = Path.GetFileNameWithoutExtension(strFile).ToUpper();
					try
					{
						string strXml = FileTools.ReadFileString(strFile);
						try
						{
							Deck deck = new Deck(gdData, strFileName, strXml, m_strName);
							// See if we can load the deck image (can make things easier for people who create from existing deck).
							/*TdxWrapper twDeckBox = gdData.LoadImage(deck.DeckBoxImageName, LoadImageType.Deck);
							if (twDeckBox != null)
								deck.DeckBoxImage = twDeckBox.Image;//*/
								//deck.DeckBoxImage = new System.Drawing.Bitmap(twDeckBox.Image);
							m_lstDecks.Add(deck);
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Medium, "Unable to load deck: " + strFile);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read deck: " + strFile);
					}
				}
			}
		}

		public override Dictionary<int, string> LoadUnlocks(GameDirectory gdData)
		{
			Dictionary<int, string> dicUsedIds = new Dictionary<int, string>();
			if (Directory.Exists(m_strFullDir + UNLOCKS_LOCATION))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + UNLOCKS_LOCATION, "*.xml");
				foreach (string strFile in files)
				{
					string strFileName = Path.GetFileNameWithoutExtension(strFile).ToUpper();
					try
					{
						string strXml = FileTools.ReadFileString(strFile);
						try
						{
							DeckUnlocks duUnlocks = new DeckUnlocks(gdData, strXml, strFileName);
							if (!dicUsedIds.ContainsKey(duUnlocks.Uid))
								dicUsedIds.Add(duUnlocks.Uid, strFileName);
							Deck dkDeck = gdData.GetDeckById(duUnlocks.DeckUid, m_strName);
							if (dkDeck == null)
								dkDeck = gdData.GetDeckById(duUnlocks.DeckUid);
							if (dkDeck != null)
							{
								if (duUnlocks.Promo)
								{
									if (dkDeck.PromoUnlockCardCount == 0)
										dkDeck.PromoUnlocks = duUnlocks;
									else
										dkDeck.PromoUnlocks.MergeUnlocks(duUnlocks);
								}
								else
								{
									if (dkDeck.RegularUnlockCardCount == 0)
										dkDeck.RegularUnlocks = duUnlocks;
									else
										dkDeck.RegularUnlocks.MergeUnlocks(duUnlocks);
								}
							}
							else
								Settings.ReportError(null, ErrorPriority.Low, "Unable to find deck with id " + duUnlocks.DeckUid.ToString() + " to associate unlock file " + strFileName + " with.");
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Medium, "Unable to load unlock file: " + strFile + " in " + m_strName);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read unlock file: " + strFile + " in " + m_strName);
					}
				}
			}

			return dicUsedIds;
		}

		private void LoadSpecs()
		{
			if (Directory.Exists(m_strFullDir + SPECS_LOCATION))
			{
				m_dicSubTypeOrdering = new Dictionary<string, Dictionary<string, int>>();
				m_dicSubTypes = new Dictionary<CardSubTypeArchetypes, List<string>>();

				// Loop through the SUBTYPEORDERINGDATA_* files for sub-type ordering data.
				IEnumerable<string> files = Directory.EnumerateFiles(m_strFullDir + SPECS_LOCATION, "SUBTYPEORDERINGDATA_*");
				foreach (string strFile in files)
				{
					if (Path.GetExtension(strFile).Equals(".txt", StringComparison.OrdinalIgnoreCase))
					{
						string strLangCode = Path.GetFileNameWithoutExtension(strFile).ToUpper().Replace("SUBTYPEORDERINGDATA_", "");
						try
						{
							string strContents = FileTools.ReadFileString(strFile);
							try
							{
								// Now we need to make sure the contents are clean enough to properly process.
								//	Mainly this unifies all line endings to a common line ending so I can split on it.
								strContents = strContents.Replace("\r\n", "{{RN}}").Replace("\r", "{{RN}}").Replace("\n", "{{RN}}").Replace("{{RN}}", "\r\n");

								// Now we create a dictionary for the ordering data and add it.
								Dictionary<string, int> dicOrdering = new Dictionary<string, int>();
								m_dicSubTypeOrdering.Add(strLangCode, dicOrdering);

								// And Load in the data.
								string[] astrEntries = strContents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
								foreach (string strEntry in astrEntries)
								{
									if (!strEntry.StartsWith("//"))
									{
										string[] astrEntryParts = strEntry.Split(new char[] { ',' });
										if (astrEntryParts.Length >= 2)
										{
											string strSubType = astrEntryParts[0].Trim().ToUpper();
											// Verify that we have something other than a blank line (which should be the case)
											//	and that it isn't already in the list.
											if ((strSubType.Length > 0) && (!dicOrdering.ContainsKey(strSubType)))
											{
												int nOrder = 0;
												if (int.TryParse(astrEntryParts[1].Trim(), out nOrder))
												{
													dicOrdering.Add(strSubType, nOrder);
												}
											}
										}
									}
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to load SPEC file: " + Path.GetFileName(strFile) + " in " + m_strName);
							}
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Low, "Unable to read SPEC file: " + Path.GetFileName(strFile) + " in " + m_strName);
						}
					}
				}

				// Loop through the CardSubTypeArchetypes to load the sub-types for each archetype.
				foreach (CardSubTypeArchetypes eType in Enum.GetValues(typeof(CardSubTypeArchetypes)))
				{
					string strFile = m_strFullDir + SPECS_LOCATION + eType.ToString().ToUpper() + "_TYPES.TXT";
					if (File.Exists(strFile))
					{
						// Great, we have a SPEC file to attempt to read in and process.
						try
						{
							string strContents = FileTools.ReadFileString(strFile);
							try
							{
								// Now we need to make sure the contents are clean enough to properly process.
								//	Mainly this unifies all line endings to a common line ending so I can split on it.
								strContents = strContents.Replace("\r\n", "{{RN}}").Replace("\r", "{{RN}}").Replace("\n", "{{RN}}").Replace("{{RN}}", "\r\n");

								// Now we create our list and add it to the Sub-Types dictionary.
								List<string> lstSubTypes = new List<string>();
								m_dicSubTypes.Add(eType, lstSubTypes);

								// Now we parse our entries and add them to the Sub-Types list.
								string[] astrEntries = strContents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
								foreach (string strEntry in astrEntries)
								{
									string strKey = strEntry.Trim().ToUpper();
									// Verify that we have something other than a blank line (which should be the case)
									//	and that it isn't already in the list.
									if ((strKey.Length > 0) && (!lstSubTypes.Contains(strKey)))
										lstSubTypes.Add(strKey);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to load SPEC file: " + Path.GetFileName(strFile) + " in " + m_strName);
							}
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Medium, "Unable to read SPEC file: " + Path.GetFileName(strFile) + " in " + m_strName);
						}
					}
				}
			}
		}

		// For directories this is pretty easy.
		public override void WriteWad(string strLocation)
		{
			if (Directory.Exists(strLocation))
			{
				// First figure out whether we have a trailing slash or not.
				//	Here we want one.
				if (!strLocation.EndsWith("\\"))
					strLocation += "\\";

				// Delete the directory if it already exists.
				//	We do this to get rid of any old files.
				try
				{
					if (Directory.Exists(strLocation + m_strName.ToUpper()))
						Directory.Delete(strLocation + m_strName.ToUpper(), true);
				}
				catch (Exception e)
				{
					Settings.ReportError(e, ErrorPriority.Low, "Unable to fully remove directory (" + strLocation + m_strName.ToUpper() + ") and it's components before re-creating it, some old elements may be left behind.");
				}

				// Create our directory.
				Directory.CreateDirectory(strLocation + m_strName.ToUpper());

				foreach (KeyValuePair<string, MemoryStream> kvFile in m_sdicOutputFiles)
				{
					string strDir = Path.GetDirectoryName(kvFile.Key);
					string strFilename = strLocation + kvFile.Key;

					CreateDirectoryIfNecessary(strLocation + strDir);

					// We only want to create a file if we actually have one.
					//	Null memory stream is used by placeholders.
					if (kvFile.Value != null)
						File.WriteAllBytes(strFilename, kvFile.Value.ToArray());
				}
			}
			else
				throw new Exception("Output directory not found.");
		}

		private void CreateDirectoryIfNecessary(string strFullPath)
		{
			if (!Directory.Exists(strFullPath))
				Directory.CreateDirectory(strFullPath);
		}

		public void WritePlaceholderWad(string strLocation)
		{
			// Here we setup and write out a placeholder Wad Directory to the specified location.
			if (Directory.Exists(strLocation))
			{
				AddHeader();
				// Add placeholders for AI Personalities, Cards, Functions, and Text
				//	If they want Decks, and Unlocks they can add them, the users can add those themselves.
				AddPlaceholder(AI_PERSONALITY_LOCATION);
				AddPlaceholder(AI_PERSONALITY_IMAGES_LOCATION);
				AddPlaceholder(CARD_DIRECTORY_LOCATION);
				AddPlaceholder(CARD_IMAGES_LOCATION);
				AddPlaceholder(FUNCTIONS_LOCATION);
				AddPlaceholder(TEXT_LOCATION);
				// Now write it out.
				WriteWad(strLocation);
			}
			else
				throw new Exception("Output directory not found.");
		}

		public override void WriteToWad(WadBase wTarget)
		{
			// The only file in our root directory should be HEADER.XML (which we will re-create so don't worry about copying it).
			IEnumerable<string> dirs = Directory.EnumerateDirectories(m_strFullDir);
			foreach (string strDir in dirs)
				WriteDirectoryToWad(wTarget, strDir);
		}

		private void WriteDirectoryToWad(WadBase wTarget, string strDir)
		{
			IEnumerable<string> files = Directory.EnumerateFiles(strDir);
			foreach (string strFile in files)
			{
				// We don't want to copy over Header.xml as we are creating a new one.
				if (!Path.GetFileName(strFile).Equals("HEADER.XML", StringComparison.OrdinalIgnoreCase))
				{
					string strTruncFile = strFile.Replace(m_strFullDir, string.Empty);
					wTarget.AddFile(strTruncFile, FileTools.ReadFileToMemoryStream(strFile));
				}
			}

			IEnumerable<string> dirs = Directory.EnumerateDirectories(strDir);
			foreach (string strSubDir in dirs)
				WriteDirectoryToWad(wTarget, strSubDir);
		}
	}
}
