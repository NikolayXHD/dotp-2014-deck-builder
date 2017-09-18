using Gibbed.Duels.FileFormats;
using Wad = Gibbed.Duels.FileFormats.Wad;
using Gibbed.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public class WadWrapper : WadBase
	{
		private class MyFileEntry : Wad.FileEntry
		{
			public MyFileEntry(Wad.DirectoryEntry directory)
				: base(directory)
			{ }

			public MemoryStream FileData;
		}

		private const ushort WAD_VERSION = 0x202;

		private string m_strWadPath;
		private WadFile m_wadArchive;
		private bool m_bHasCompressedFiles;

		// This constructor is mainly for writing out a Wad, as such it does no loading.
		public WadWrapper(string strPath)
			: base()
		{
			// WAD filename is used both to identify the wad that a card/image is in as well as providing a link to the actual file.
			m_strWadPath = strPath;
			m_strName = Path.GetFileNameWithoutExtension(strPath);
		}

		public WadWrapper(string strPath, GameDirectory gdData)
			: base()
		{
			// WAD filename is used both to identify the wad that a card/image is in as well as providing a link to the actual file.
			m_strWadPath = strPath;
			m_strName = Path.GetFileNameWithoutExtension(strPath);

			// Load Cards
			using (FileStream fsInput = OpenWadStream())
			{
				m_lstCards = LoadCards(fsInput, gdData);

				// Load up all the strings present in TEXT_PERMANENT
				m_dicStringTable = LoadStringTable(fsInput);
			}

			// Now we no longer need the wad open as for loading image names all we need is the directory which we already have.
			m_hsetImages = LoadImageList();
		}

		private Dictionary<string, Dictionary<string, string>> LoadStringTable(FileStream fsInput)
		{
			Dictionary<string, Dictionary<string, string>> dicStringTable = new Dictionary<string, Dictionary<string, string>>();

			if (fsInput != null)
			{
				Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, TEXT_LOCATION);
				if (dir != null)
				{
					foreach (Wad.FileEntry feFile in dir.Files)
					{
						if (Path.GetExtension(feFile.Name).Equals(".xml", StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								string strXml = RetrieveTextFile(fsInput, feFile);
								LoadIndividualStringTable(dicStringTable, strXml);
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to read string table: " + feFile.Name + " in " + m_strName);
							}
						}
					}
				}
			}

			return dicStringTable;
		}

		private FileStream OpenWadStream()
		{
			if (File.Exists(m_strWadPath))
			{
				FileStream fsInput = File.OpenRead(m_strWadPath);

				ushort checkMagic, checkVersion;
				string checkReason;

				if (WadFile.IsBadHeader(fsInput, out checkMagic, out checkVersion, out checkReason) == true)
				{
					// We failed to open this Wad due to a bad header
					fsInput.Close();
					Settings.ReportError(null, ErrorPriority.Medium, "Failed to open Wad due to bad header: " + m_strWadPath);
					return null;
				}

				fsInput.Seek(0, SeekOrigin.Begin);
				m_wadArchive = new WadFile();
				m_wadArchive.Deserialize(fsInput);
				m_bHasCompressedFiles = ((m_wadArchive.Flags & Wad.ArchiveFlags.HasCompressedFiles) == Wad.ArchiveFlags.HasCompressedFiles);
				return fsInput;
			}
			else
				return null;
		}

		private SortableBindingList<CardInfo> LoadCards(FileStream fsInput, GameDirectory gdData)
		{
			SortableBindingList<CardInfo> set = new SortableBindingList<CardInfo>();

			if (fsInput != null)
			{
				Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, CARD_DIRECTORY_LOCATION);
				if (dir != null)
				{
					foreach (Wad.FileEntry feFile in dir.Files)
					{
						if (Path.GetExtension(feFile.Name).Equals(".xml", StringComparison.OrdinalIgnoreCase))
						{
							try
							{
								using (MemoryStream msFile = RetrieveFile(fsInput, feFile))
								{
									string strCard = msFile.ReadEncodedString(msFile.Length);
									if (strCard.Length > 0)
									{
										try
										{
											CardInfo ciCard = new CardInfo(strCard, m_strName, gdData);
											if (ciCard.Filename == null)
												Settings.ReportError(null, ErrorPriority.Low, "Card (" + feFile.Name + ") could not be loaded due to missing the CARD_V2 block in wad " + m_strName + ".");
											else if (ciCard.Filename.Length <= 0)
												Settings.ReportError(null, ErrorPriority.Low, "Card (" + feFile.Name + ") could not be properly loaded due to missing or malformed FILENAME tag in wad " + m_strName + ".");
											else
												set.Add(ciCard);
										}
										catch (Exception e)
										{
											Settings.ReportError(e, ErrorPriority.Low, "Unable to load card: " + feFile.Name + " in " + m_strName);
										}
									}
									else
										Settings.ReportError(null, ErrorPriority.Low, "Unable to load card because there is nothing to load: " + feFile.Name + " in " + m_strName);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to read card: " + feFile.Name + " in " + m_strName);
							}
						}
					}
				}
			}

			return set;
		}

		private HashSet<KeyValuePair<string, LoadImageType>> LoadImageList()
		{
			HashSet<KeyValuePair<string, LoadImageType>> set = new HashSet<KeyValuePair<string, LoadImageType>>();

			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, CARD_IMAGES_LOCATION), LoadImageType.Card);
			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, DECK_IMAGES_LOCATION), LoadImageType.Deck);
			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, AI_PERSONALITY_IMAGES_LOCATION), LoadImageType.Personality);
			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, CARD_FRAME_IMAGES_LOCATION), LoadImageType.Frame);
			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, TEXTURE_IMAGES_LOCATION), LoadImageType.Texture);
			LoadImageListFrom(set, FindDir(m_wadArchive.Directories, MANA_IMAGES_LOCATION), LoadImageType.Mana);

			return set;
		}

		private void LoadImageListFrom(HashSet<KeyValuePair<string, LoadImageType>> set, Wad.DirectoryEntry dir, LoadImageType eType)
		{
			if (dir != null)
			{
				foreach (Wad.FileEntry feFile in dir.Files)
				{
					if (Path.GetExtension(feFile.Name).Equals(".tdx", StringComparison.OrdinalIgnoreCase))
						set.Add(new KeyValuePair<string, LoadImageType>(Path.GetFileNameWithoutExtension(feFile.Name).ToUpper(), eType));
				}
			}
		}

		private Wad.DirectoryEntry FindDir(List<Wad.DirectoryEntry> dirList, string strDir)
		{
			// Remove any trailing backslashes as the WAD directory does not have them.
			if (strDir[strDir.Length - 1].Equals('\\'))
				strDir = strDir.Substring(0, strDir.Length - 1);

			Wad.DirectoryEntry dirFound = null;

			foreach (Wad.DirectoryEntry dir in dirList)
			{
				if (dir.Name.Equals(Path.GetFileNameWithoutExtension(strDir), StringComparison.OrdinalIgnoreCase))
				{
					// We may have found the directory, but now we need to check the path.
					string[] astrParts = strDir.Split('\\');

					// We are currently at the last segment.
					bool bFound = true;
					Wad.DirectoryEntry dirParent = dir.ParentDirectory;
					for (int i = astrParts.Length - 2; i >= 0; i--)
					{
						if (!dirParent.Name.Equals(astrParts[i], StringComparison.OrdinalIgnoreCase))
						{
							bFound = false;
							break;
						}
						dirParent = dirParent.ParentDirectory;
					}
					if (bFound)
					{
						dirFound = dir;
						break;
					}
				}
				dirFound = FindDir(dir.Directories, strDir);
				if (dirFound != null)
					break;
			}

			return dirFound;
		}

		public override string FindTextFile(string strName)
		{
			FileStream fsInput = OpenWadStream();
			string strReturn = FindTextFile(fsInput, strName);
			fsInput.Close();
			fsInput.Dispose();
			return strReturn;
		}

		private string FindTextFile(FileStream fsInput, string strName)
		{
			string strReturn = null;
			Wad.FileEntry feFile = GetFileEntryIfExists(m_wadArchive.AllFiles, strName);
			if (feFile != null)
			{
				using (MemoryStream msFile = RetrieveFile(fsInput, feFile))
				{
					strReturn = msFile.ReadEncodedString(msFile.Length);
				}
			}
			return strReturn;
		}

		public override byte[] FindBinaryFile(string strName)
		{
			FileStream fsInput = OpenWadStream();
			byte[] abytReturn = FindBinaryFile(fsInput, strName);
			fsInput.Close();
			fsInput.Dispose();
			return abytReturn;
		}

		private byte[] FindBinaryFile(FileStream fsInput, string strName)
		{
			byte[] abytReturn = null;
			Wad.FileEntry feFile = GetFileEntryIfExists(m_wadArchive.AllFiles, strName);
			if (feFile != null)
			{
				using (MemoryStream msFile = RetrieveFile(fsInput, feFile))
				{
					abytReturn = msFile.ReadBytes((int)msFile.Length);
				}
			}
			return abytReturn;
		}

		private Wad.FileEntry GetFileEntryIfExists(IEnumerable<Wad.FileEntry> files, string strFileName)
		{
			Wad.FileEntry feFile = null;

			foreach (Wad.FileEntry feTest in files)
			{
				if (feTest.Name.Equals(strFileName, StringComparison.OrdinalIgnoreCase))
				{
					feFile = feTest;
					break;
				}
			}

			return feFile;
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

			TdxWrapper tdx = null;

			if (m_hsetImages.Contains(new KeyValuePair<string,LoadImageType>(strId, eType)))
			{
				// This also refreshes the directory in case the WAD changed.
				using (FileStream fsInput = OpenWadStream())
				{
					Wad.DirectoryEntry dir = null;
					Wad.FileEntry feFile = null;
					switch (eType)
					{
						case LoadImageType.Card:
							dir = FindDir(m_wadArchive.Directories, CARD_IMAGES_LOCATION);
							break;
						case LoadImageType.Deck:
							dir = FindDir(m_wadArchive.Directories, DECK_IMAGES_LOCATION);
							break;
						case LoadImageType.Frame:
							dir = FindDir(m_wadArchive.Directories, CARD_FRAME_IMAGES_LOCATION);
							break;
						case LoadImageType.Mana:
							dir = FindDir(m_wadArchive.Directories, MANA_IMAGES_LOCATION);
							break;
						case LoadImageType.Personality:
							dir = FindDir(m_wadArchive.Directories, AI_PERSONALITY_IMAGES_LOCATION);
							break;
						case LoadImageType.Texture:
							dir = FindDir(m_wadArchive.Directories, TEXTURE_IMAGES_LOCATION);
							break;
					}
					feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");

					if (feFile != null)
					{
						tdx = new TdxWrapper();

						using (MemoryStream msImage = RetrieveFile(fsInput, feFile))
						{
							try
							{
								tdx.LoadTdx(msImage);
								// Cache the image to avoid having to load it multiple times.
								if (Settings.GetSetting("MaintainImageCache", Settings.MAINTAIN_IMAGE_CACHE_DEFAULT))
									m_dicCachedImages.Add(eType.ToString() + ":" + strId.ToUpper(), tdx);
							}
							catch (Exception e)
							{
								tdx = null;
								Settings.ReportError(e, ErrorPriority.Low, "Could not load: " + feFile.Name + " in " + m_strName);
							}
						}

						fsInput.Close();
						fsInput.Dispose();
					}
				}
			}

			return tdx;
		}

		private TdxWrapper LoadImageSearch(string strId)
		{
			// Check for image in cache.
			if (m_dicCachedImages.ContainsKey(strId.ToUpper()))
				return m_dicCachedImages[strId.ToUpper()];

			TdxWrapper tdx = null;

			bool bFound = false;
			foreach (KeyValuePair<string, LoadImageType> kvp in m_hsetImages)
			{
				if (kvp.Key.Equals(strId, StringComparison.OrdinalIgnoreCase))
				{
					bFound = true;
					break;
				}
			}

			if (bFound)
			{
				// This also refreshes the directory in case the WAD changed.
				using (FileStream fsInput = OpenWadStream())
				{
					Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, CARD_IMAGES_LOCATION);
					Wad.FileEntry feFile = null;
					if ((dir != null) && (dir.Files.Count > 0))
						feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
					if (feFile == null)
					{
						dir = FindDir(m_wadArchive.Directories, DECK_IMAGES_LOCATION);
						if ((dir != null) && (dir.Files.Count > 0))
							feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
						if (feFile == null)
						{
							dir = FindDir(m_wadArchive.Directories, AI_PERSONALITY_IMAGES_LOCATION);
							if ((dir != null) && (dir.Files.Count > 0))
								feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
							if (feFile == null)
							{
								dir = FindDir(m_wadArchive.Directories, CARD_FRAME_IMAGES_LOCATION);
								if ((dir != null) && (dir.Files.Count > 0))
									feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
								if (feFile == null)
								{
									dir = FindDir(m_wadArchive.Directories, TEXTURE_IMAGES_LOCATION);
									if ((dir != null) && (dir.Files.Count > 0))
										feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
									if (feFile == null)
									{
										dir = FindDir(m_wadArchive.Directories, MANA_IMAGES_LOCATION);
										if ((dir != null) && (dir.Files.Count > 0))
											feFile = GetFileEntryIfExists(dir.Files, strId + ".TDX");
									}
								}
							}
						}
					}

					if (feFile != null)
					{
						tdx = new TdxWrapper();

						using (MemoryStream msImage = RetrieveFile(fsInput, feFile))
						{
							try
							{
								tdx.LoadTdx(msImage);
								// Cache the image to avoid having to load it multiple times.
								if (Settings.GetSetting("MaintainImageCache", Settings.MAINTAIN_IMAGE_CACHE_DEFAULT))
									m_dicCachedImages.Add(strId.ToUpper(), tdx);
							}
							catch (Exception e)
							{
								tdx = null;
								Settings.ReportError(e, ErrorPriority.Low, "Could not load: " + feFile.Name + " in " + m_strName);
							}
						}

						fsInput.Close();
						fsInput.Dispose();
					}
				}
			}

			return tdx;
		}

		private string RetrieveTextFile(FileStream fsInput, Wad.FileEntry feFile)
		{
			string strReturn = null;

			using (MemoryStream msFile = RetrieveFile(fsInput, feFile))
			{
				strReturn = msFile.ReadEncodedString(msFile.Length);
			}

			return strReturn;
		}

		private MemoryStream RetrieveFile(FileStream fsInput, Wad.FileEntry feFile)
		{
			MemoryStream msFile = null;

			if (feFile != null)
			{
				fsInput.Seek(m_wadArchive.DataOffsets[feFile.OffsetIndex], SeekOrigin.Begin);
				if (m_bHasCompressedFiles)
				{
					// Possibly Compressed.
					int nLength = fsInput.ReadValueS32(m_wadArchive.Endian);
					if (nLength == -1)
						msFile = fsInput.ReadToMemoryStream(feFile.Size - 4);
					else
					{
						MemoryStream ms = fsInput.ReadToMemoryStream(feFile.Size - 4);
						InflaterInputStream iis = new InflaterInputStream(ms);
						msFile = iis.ReadToMemoryStream(nLength);
						iis.Close();
						ms.Close();
					}
				}
				else
				{
					// Not Compressed.
					msFile = fsInput.ReadToMemoryStream(feFile.Size);
				}
			}

			return msFile;
		}

		public override void LoadPersonalities(GameDirectory gdData)
		{
			m_lstPersonalities = new SortableBindingList<AiPersonality>();
			using (FileStream fsInput = OpenWadStream())
			{
				Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, AI_PERSONALITY_LOCATION);
				if (dir != null)
				{
					foreach (Wad.FileEntry feFile in dir.Files)
					{
						if (Path.GetExtension(feFile.Name).Equals(".xml", StringComparison.OrdinalIgnoreCase))
						{
							string strFileName = Path.GetFileNameWithoutExtension(feFile.Name).ToUpper();
							try
							{
								string strXml = RetrieveTextFile(fsInput, feFile);
								try
								{
									AiPersonality ap = new AiPersonality(gdData, strXml, feFile.Name);
									// This was loaded from the game directory so we consider it built-in because the user already has it.
									ap.BuiltIn = true;
									m_lstPersonalities.Add(ap);
								}
								catch (Exception e)
								{
									Settings.ReportError(e, ErrorPriority.Medium, "Unable to load deck: " + feFile.Name + " in " + m_strName);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to read deck: " + feFile.Name + " in " + m_strName);
							}
						}
					}
				}
			}
		}

		public override void LoadDecks(GameDirectory gdData)
		{
			m_lstDecks = new SortableBindingList<Deck>();
			using (FileStream fsInput = OpenWadStream())
			{
				Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, DECK_LOCATION);
				if (dir != null)
				{
					foreach (Wad.FileEntry feFile in dir.Files)
					{
						if (Path.GetExtension(feFile.Name).Equals(".xml", StringComparison.OrdinalIgnoreCase))
						{
							string strFileName = Path.GetFileNameWithoutExtension(feFile.Name).ToUpper();
							try
							{
								string strXml = RetrieveTextFile(fsInput, feFile);
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
									Settings.ReportError(e, ErrorPriority.Medium, "Unable to load deck: " + feFile.Name + " in " + m_strName);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to read deck: " + feFile.Name + " in " + m_strName);
							}
						}
					}
				}
			}
		}

		public override Dictionary<int, string> LoadUnlocks(GameDirectory gdData)
		{
			Dictionary<int, string> dicUsedIds = new Dictionary<int, string>();

			using (FileStream fsInput = OpenWadStream())
			{
				Wad.DirectoryEntry dir = FindDir(m_wadArchive.Directories, UNLOCKS_LOCATION);
				if (dir != null)
				{
					foreach (Wad.FileEntry feFile in dir.Files)
					{
						if (Path.GetExtension(feFile.Name).Equals(".xml", StringComparison.OrdinalIgnoreCase))
						{
							string strFileName = Path.GetFileNameWithoutExtension(feFile.Name).ToUpper();
							try
							{
								string strXml = RetrieveTextFile(fsInput, feFile);
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
									Settings.ReportError(e, ErrorPriority.Medium, "Unable to load unlock file: " + feFile.Name + " in " + m_strName);
								}
							}
							catch (Exception e)
							{
								Settings.ReportError(e, ErrorPriority.Medium, "Unable to read unlock file: " + feFile.Name + " in " + m_strName);
							}
						}
					}
				}
			}

			return dicUsedIds;
		}

		public override void WriteWad(string strLocation)
		{
			if (Directory.Exists(strLocation))
			{
				// First figure out whether we have a trailing slash or not.
				//	Here we want one.
				if (!strLocation.EndsWith("\\"))
					strLocation += "\\";

				// Remove the file if it already exists.
				if (File.Exists(strLocation + m_strName + ".wad"))
					File.Delete(strLocation + m_strName + ".wad");

				Wad.ArchiveFlags eFlags = Wad.ArchiveFlags.Unknown6Observed | Wad.ArchiveFlags.HasDataTypes | Wad.ArchiveFlags.HasCompressedFiles;

				WadFile wfWad = new WadFile();
				wfWad.Version = WAD_VERSION;
				wfWad.Flags = eFlags;

				// Create header if not already created.
				if (!m_sdicOutputFiles.ContainsKey(m_strName.ToUpper() + "\\HEADER.XML"))
					AddHeader();

				// Add header to wad.
				wfWad.HeaderXml = m_sdicOutputFiles[m_strName.ToUpper() + "\\HEADER.XML"].ToArray();

				List<MyFileEntry> lstFiles = new List<MyFileEntry>();
				foreach (KeyValuePair<string, MemoryStream> kvpFile in m_sdicOutputFiles)
				{
					string strFilename = Path.GetFileName(kvpFile.Key);
					string strDir = Path.GetDirectoryName(kvpFile.Key);

					Wad.DirectoryEntry deDir = GetOrCreateDirectory(wfWad, strDir);

					// If our memory stream is null, it means we just want to create the directory.
					//	Null memory stream is used by placeholders.
					if (kvpFile.Value != null)
					{
						MyFileEntry mfeFile = new MyFileEntry(deDir);
						mfeFile.FileData = kvpFile.Value;
						mfeFile.Name = strFilename;
						mfeFile.Size = 0;
						mfeFile.Unknown0C = 0;

						mfeFile.OffsetIndex = wfWad.DataOffsets.Count;
						mfeFile.OffsetCount = 1;
						wfWad.DataOffsets.Add(0);

						deDir.Files.Add(mfeFile);
						lstFiles.Add(mfeFile);
					}
				}

				using (FileStream fsOutput = File.Create(strLocation + m_strName + ".wad"))
				{
					// Write the stub header.
					wfWad.Serialize(fsOutput);

					// Write file data.
					foreach (MyFileEntry mfeFile in lstFiles)
					{
						wfWad.DataOffsets[mfeFile.OffsetIndex] = (uint)fsOutput.Position;

						// Make sure data position is at 0
						mfeFile.FileData.Seek(0, SeekOrigin.Begin);

						using (MemoryStream msTemp = new MemoryStream())
						{
							// Compress the stream
							DeflaterOutputStream dosZLib = new DeflaterOutputStream(msTemp, new Deflater(Deflater.BEST_COMPRESSION));
							dosZLib.WriteFromStream(mfeFile.FileData, mfeFile.FileData.Length);
							dosZLib.Finish();
							msTemp.Flush();
							msTemp.Position = 0;

							if (msTemp.Length < mfeFile.FileData.Length)
							{
								mfeFile.Size = (uint)(4 + msTemp.Length);
								fsOutput.WriteValueU32((uint)mfeFile.FileData.Length);
								fsOutput.WriteFromStream(msTemp, msTemp.Length);
							}
							else
							{
								mfeFile.FileData.Seek(0, SeekOrigin.Begin);
								mfeFile.Size = (uint)(4 + mfeFile.FileData.Length);
								fsOutput.WriteValueU32(0xFFFFFFFFu);
								fsOutput.WriteFromStream(mfeFile.FileData, mfeFile.FileData.Length);
							}
						}
					}

					// Write real header now.
					fsOutput.Seek(0, SeekOrigin.Begin);
					wfWad.Serialize(fsOutput);
				}
			}
		}

		private Wad.DirectoryEntry GetOrCreateDirectory(WadFile wfWad, string strPath)
		{
			string[] astrParts = strPath.Split(Path.DirectorySeparatorChar);

			Wad.DirectoryEntry deRoot;

			deRoot = wfWad.Directories.SingleOrDefault(d => d.Name == astrParts[0]);
			if (deRoot == null)
			{
				deRoot = new Wad.DirectoryEntry(null);
				deRoot.Name = astrParts[0];
				wfWad.Directories.Add(deRoot);
			}

			Wad.DirectoryEntry deCurrent = deRoot;
			foreach (string strPart in astrParts.Skip(1))
			{
				Wad.DirectoryEntry deChild = deCurrent.Directories.SingleOrDefault(d => d.Name == strPart);

				if (deChild == null)
				{
					deChild = new Wad.DirectoryEntry(deCurrent);
					deChild.Name = strPart;
					deCurrent.Directories.Add(deChild);
				}

				deCurrent = deChild;
			}

			return deCurrent;
		}

		public override void WriteToWad(WadBase wTarget)
		{
			// We need to open the wad again (this also gets us the latest version).
			using (FileStream fsInput = OpenWadStream())
			{
				string strBaseDir = null;
				// Get the base directory for this wad, this involves reading the header xml and pulling out the base dir.
				using (MemoryStream msHeader = new MemoryStream(m_wadArchive.HeaderXml))
				{
					string strXml = msHeader.ReadEncodedString(msHeader.Length);
					strBaseDir = XmlTools.GetHeaderBaseDir(strXml, "DATA_ALL_PLATFORMS");
				}

				// Iterate through all files in the wad adding them to the new wad.
				foreach (Wad.FileEntry feFile in m_wadArchive.AllFiles)
				{
					// Well, for all files except the header.
					if (!feFile.Name.Equals("HEADER.XML", StringComparison.OrdinalIgnoreCase))
					{
						string strFile = (feFile.ToString()).Replace(strBaseDir, string.Empty);
						wTarget.AddFile(strFile, RetrieveFile(fsInput, feFile));
					}
				}
			}
		}
	}
}
