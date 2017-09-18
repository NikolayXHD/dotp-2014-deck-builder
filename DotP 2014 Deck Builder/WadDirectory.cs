using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

			// Load Cards & Images
			m_lstCards = LoadCards(gdData);
			m_hsetImages = LoadImageList();

			// Load up all the strings present in TEXT_PERMANENT
			m_dicStringTable = LoadStringTable();
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
								CardInfo ciCard = new CardInfo(strXml, m_strName, gdData);
								if (ciCard.Filename == null)
									Settings.ReportError(null, ErrorPriority.Low, "Card (" + strFile + ") could not be loaded due to missing the CARD_V2 block in wad " + m_strName + ".");
								else if (ciCard.Filename.Length <= 0)
									Settings.ReportError(null, ErrorPriority.Low, "Card (" + strFile + ") could not be properly loaded due to missing or malformed FILENAME tag in wad " + m_strName + ".");
								else
									set.Add(ciCard);
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

		private HashSet<string> LoadImageList()
		{
			HashSet<string> set = new HashSet<string>();

			LoadImageListFrom(set, m_strFullDir + CARD_IMAGES_LOCATION);
			LoadImageListFrom(set, m_strFullDir + DECK_IMAGES_LOCATION);
			LoadImageListFrom(set, m_strFullDir + AI_PERSONALITY_IMAGES_LOCATION);
			LoadImageListFrom(set, m_strFullDir + CARD_FRAME_IMAGES_LOCATION);
			LoadImageListFrom(set, m_strFullDir + TEXTURE_IMAGES_LOCATION);

			return set;
		}

		private void LoadImageListFrom(HashSet<string> set, string strDir)
		{
			if (Directory.Exists(strDir))
			{
				IEnumerable<string> files = Directory.EnumerateFiles(strDir, "*.tdx");
				foreach (string strFile in files)
				{
					string strId = Path.GetFileNameWithoutExtension(strFile);
					set.Add(strId.ToUpper());
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

			if (m_hsetImages.Contains(strId.ToUpper()))
			{
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
			return null;
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
							// This was loaded from the game directory so we consider it built-in because the user already has it.
							ap.BuiltIn = true;
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
							TdxWrapper twDeckBox = gdData.LoadImage(deck.DeckBoxImageName, LoadImageType.Deck);
							if (twDeckBox != null)
								deck.DeckBoxImage = new System.Drawing.Bitmap(twDeckBox.Image);
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
							Deck dkDeck = gdData.GetDeckById(duUnlocks.DeckUid);
							if (dkDeck != null)
							{
								if (duUnlocks.Promo)
									dkDeck.PromoUnlocks = duUnlocks;
								else
									dkDeck.RegularUnlocks = duUnlocks;
							}
							else
								Settings.ReportError(null, ErrorPriority.Low, "Unable to find deck with id " + duUnlocks.DeckUid.ToString() + " to associate unlock file " + strFileName + " with.");
						}
						catch (Exception e)
						{
							Settings.ReportError(e, ErrorPriority.Medium, "Unable to load unlock file: " + strFile);
						}
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to read unlock file: " + strFile);
					}
				}
			}

			return dicUsedIds;
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
