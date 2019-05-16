using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Be.Timvw.Framework.ComponentModel;
using RSN.Tools;

namespace RSN.DotP
{
	public class Deck
	{
		private static string[] FOREST_POOL = null;
		private static string[] ISLAND_POOL = null;
		private static string[] MOUNTAIN_POOL = null;
		private static string[] PLAINS_POOL = null;
		private static string[] SWAMP_POOL = null;

		private static string ANDROID_UNLOCK_DEFAULT_ID = null;
		private static string ANDROID_FOIL_DEFAULT_ID = null;
		private static string IOS_UNLOCK_DEFAULT_ID = null;
		private static string IOS_FOIL_DEFAULT_ID = null;
		private static string STEAM_UNLOCK_DEFAULT_ID = null;
		private static string STEAM_FOIL_DEFAULT_ID = null;

		// Defines text permanent entries and whether it will conflict with what is already in other wads.
		//	Note: For constructed decks this should always be the full path and filename.  We don't really
		//		care about whether this is a full path or just a filename with or without extension.
		private string m_strFileName;

		// For DECK tag
		private int m_nUid;						// uid
		private string m_strPersonality;		// personality
		private AiPersonality m_apPersonality;
		private string m_strDeckBoxImage;		// deck_box_image
		private Bitmap m_bmpDeckBoxImage;
		private string m_strDeckBoxImageLocked;	// deck_box_image_locked
		private int m_nContentPack;				// content_pack
		private DeckAvailability m_eAvailability;	// never_available, always_available
		private ColourFlags m_eColour;			// is_black, is_blue, is_green, is_red, is_white
		private string m_strNameTag;			// name_tag
		private string m_strDescriptionTag;		// description_tag

		// For DECKSTATISTICS tag
		private string m_strCreatureSize;		// Size
		private string m_strDeckSpeed;			// Speed
		private string m_strFlexiblity;			// Flex
		private string m_strSynergy;			// Syn

		// For LandConfig tag
		private int m_nIgnoreCmcOver;			// ignoreCmcOver
		private int m_nMinForests;				// minForest
		private int m_nMinIslands;				// minIsland
		private int m_nMinMountains;			// minMountain
		private int m_nMinPlains;				// minPlains
		private int m_nMinSwamps;				// minSwamp
		private int m_nNumberOfSpellsThatCountAsLand;	// numSpellsThatCountAsLand

		// Cards (the heart of a deck)
		private SortableBindingList<DeckCard> m_lstCards;

		// Land Pool
		private Deck m_dkLandPool;

		// Unlocks
		private DeckUnlocks m_duUnlocksRegular;
		private DeckUnlocks m_duUnlocksPromo;		// Promo Unlocks are limited to 10 cards.

		// Other useful variables
		private Dictionary<string, string> m_dicDeckName;
		private Dictionary<string, string> m_dicDeckDescription;
		private bool m_bIsLandPool;
		private bool m_bEditedDeck;
		private string m_strWad;
		private string m_strExportFilename;		// This is the filename given at last export.

		// To allow for overriding deck colours.
		private bool m_bOverrideColours;		// Are we overriding or not?
		private ColourFlags m_eOverrideColour;	// This is the colour(s) to override with.

		public Deck(GameDirectory gdData, bool bLandPool = false)
		{
			InitDefaultIds();
			InitLandPools();
			m_strFileName = string.Empty;							// If a filename is not present it will be generated from the English deck name.
			m_nUid = 0;
			m_apPersonality = new AiPersonality();
			m_strPersonality = string.Empty;
			m_bmpDeckBoxImage = null;
			m_strDeckBoxImage = string.Empty;
			m_strDeckBoxImageLocked = "locked";						// Most if not all decks use the same locked image.
			m_nContentPack = 0;										// By default should be available to all game versions.
			m_eAvailability = DeckAvailability.AlwaysAvailable;		// Deck should always be available by default.
			m_eColour = ColourFlags.NotDefined;						// Do not give it any colour by default (even colourless).
			m_strNameTag = string.Empty;
			m_strDescriptionTag = string.Empty;
			m_strCreatureSize = "?";
			m_strDeckSpeed = "?";
			m_strFlexiblity = "?";
			m_strSynergy = "?";
			m_nIgnoreCmcOver = -1;
			m_nMinForests = 0;
			m_nMinIslands = 0;
			m_nMinMountains = 0;
			m_nMinPlains = 0;
			m_nMinSwamps = 0;
			m_nNumberOfSpellsThatCountAsLand = 0;
			m_lstCards = new SortableBindingList<DeckCard>();
			m_duUnlocksRegular = new DeckUnlocks();
			m_duUnlocksPromo = new DeckUnlocks();
			m_dicDeckName = new Dictionary<string, string>();
			m_dicDeckName.Add("en-US", "New Deck");
			m_dicDeckDescription = new Dictionary<string, string>();
			//m_dicDeckDescription.Add("en-US", string.Empty);
			m_bIsLandPool = bLandPool;
			m_bEditedDeck = false;
			m_strWad = string.Empty;								// For a new deck Wad name will be generated when exported.
			m_bOverrideColours = false;
			m_eOverrideColour = ColourFlags.NotDefined;

			if (!m_bIsLandPool)
			{
				m_dkLandPool = new Deck(gdData, true);
				m_dkLandPool.Personality = null;
				m_dkLandPool.Availability = DeckAvailability.NeverAvailable;
			}
		}

		public Deck(GameDirectory gdData, string strFileName, string strXml, string strWad = "", bool bLandPool = false)
		{
			InitDefaultIds();
			InitLandPools();
			m_bOverrideColours = false;
			m_eOverrideColour = ColourFlags.NotDefined;
			m_strFileName = strFileName;
			m_strWad = strWad;
			m_bIsLandPool = (strFileName.IndexOf("_LAND_POOL", StringComparison.OrdinalIgnoreCase) > -1) || bLandPool;
			m_duUnlocksRegular = new DeckUnlocks();
			m_duUnlocksPromo = new DeckUnlocks();
			m_apPersonality = new AiPersonality();
			try
			{
				XmlDocument xdDoc = new XmlDocument();
				xdDoc.LoadXml(strXml);
				XmlNode xnDeck = XmlTools.GetChild(xdDoc, "DECK");
				if (xnDeck != null)
				{
					m_nUid = Int32.Parse(XmlTools.GetValueFromAttribute(xnDeck, "uid", "-1"));
					m_strPersonality = XmlTools.GetValueFromAttribute(xnDeck, "personality");
					m_strDeckBoxImage = XmlTools.GetValueFromAttribute(xnDeck, "deck_box_image");
					m_strDeckBoxImageLocked = XmlTools.GetValueFromAttribute(xnDeck, "deck_box_image_locked");
					m_nContentPack = Int32.Parse(XmlTools.GetValueFromAttribute(xnDeck, "content_pack", "0"));
					string strAvailabilityNever = XmlTools.GetValueFromAttribute(xnDeck, "never_available");
					string strAvailabilityAlways = XmlTools.GetValueFromAttribute(xnDeck, "always_available");
					if (strAvailabilityNever.Equals("True", StringComparison.OrdinalIgnoreCase))
						m_eAvailability = DeckAvailability.NeverAvailable;
					else if (strAvailabilityAlways.Equals("True", StringComparison.OrdinalIgnoreCase))
						m_eAvailability = DeckAvailability.AlwaysAvailable;
					else
						m_eAvailability = DeckAvailability.Locked;
					m_eColour = GetDeckColour(xnDeck);
					m_strNameTag = XmlTools.GetValueFromAttribute(xnDeck, "name_tag");
					m_strDescriptionTag = XmlTools.GetValueFromAttribute(xnDeck, "description_tag");
					XmlNode xnStatistics = XmlTools.GetChild(xnDeck, "DECKSTATISTICS");
					if (xnStatistics != null)
					{
						m_strCreatureSize = XmlTools.GetValueFromAttribute(xnStatistics, "Size");
						m_strDeckSpeed = XmlTools.GetValueFromAttribute(xnStatistics, "Speed");
						m_strFlexiblity = XmlTools.GetValueFromAttribute(xnStatistics, "Flex");
						m_strSynergy = XmlTools.GetValueFromAttribute(xnStatistics, "Syn");
					}
					else
					{
						m_strCreatureSize = "?";
						m_strDeckSpeed = "?";
						m_strFlexiblity = "?";
						m_strSynergy = "?";
					}
					m_nIgnoreCmcOver = -1;
					m_nMinForests = 0;
					m_nMinIslands = 0;
					m_nMinMountains = 0;
					m_nMinPlains = 0;
					m_nMinSwamps = 0;
					m_nNumberOfSpellsThatCountAsLand = 0;
					XmlNode xnLandConfig = XmlTools.GetChild(xnDeck, "LandConfig");
					if (xnLandConfig != null)
					{
						m_nIgnoreCmcOver = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "ignoreCmcOver", "-1"));
						m_nMinForests = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "minForest", "0"));
						m_nMinIslands = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "minIsland", "0"));
						m_nMinMountains = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "minMountain", "0"));
						m_nMinPlains = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "minPlains", "0"));
						m_nMinSwamps = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "minSwamp", "0"));
						m_nNumberOfSpellsThatCountAsLand = Int32.Parse(XmlTools.GetValueFromAttribute(xnLandConfig, "numSpellsThatCountAsLand", "0"));
					}
					LoadCards(gdData, xnDeck);

					// The rest of the stuff here is only generated by this tool so we shouldn't need to worry about case.
					m_dicDeckName = new Dictionary<string, string>();
					if (xnDeck["LocalizedDeckNames"] != null)
					{
						// This deck was saved as an unfinished deck so instead of creating a
						//	TEXT_PERMANENT (since it might change) we just stored the localized
						//	names in with the deck.
						m_dicDeckName = XmlTools.GetLocalizedText(xnDeck["LocalizedDeckNames"]);
					}
					else if ((!m_bIsLandPool) && (m_strNameTag.Length > 0))
					{
						// Not a Land Pool so try to load the Names for this deck.
						string strDeckStringName = m_strNameTag.ToUpper();
						if (gdData.StringTable.ContainsKey(strDeckStringName))
							m_dicDeckName = gdData.StringTable[strDeckStringName];
					}

					// Look and see if we have a description set.
					m_dicDeckDescription = new Dictionary<string, string>();
					if (xnDeck["LocalizedDescriptions"] != null)
					{
						// This deck was saved as an unfinished deck so instead of creating a
						//	TEXT_PERMANENT (since it might change) we just stored the localized
						//	descriptions in with the deck.
						m_dicDeckDescription = XmlTools.GetLocalizedText(xnDeck["LocalizedDescriptions"]);
					}
					else if ((!m_bIsLandPool) && (m_strDescriptionTag.Length > 0))
					{
						// Not a Land Pool so try to load Descriptions for this deck.
						string strDeckDescriptionName = m_strDescriptionTag.ToUpper();
						if (gdData.StringTable.ContainsKey(strDeckDescriptionName))
							m_dicDeckDescription = gdData.StringTable[strDeckDescriptionName];
					}

					// Look to see if we have an embedded personality tag.
					if (xnDeck["AiPersonality"] != null)
						m_apPersonality = new AiPersonality(gdData, xnDeck["AiPersonality"]);
					else if (m_strPersonality.Length > 0)
					{
						// We may be using a built-in personality so try to find it.
						if (gdData.Personalities.ContainsKey(m_strPersonality.ToUpper()))
							m_apPersonality = gdData.Personalities[m_strPersonality.ToUpper()];
						else
							m_apPersonality = new AiPersonality();
					}

					// The file may also have another deck (the land pool) buried inside if this is an "unfinished" deck.
					if (xnDeck["LandPool"] != null)
						m_dkLandPool = new Deck(gdData, string.Empty, xnDeck["LandPool"].InnerXml.Trim(), m_strWad, true);

					if (xnDeck["RegularUnlocks"] != null)
						m_duUnlocksRegular = new DeckUnlocks(gdData, xnDeck["RegularUnlocks"], false);

					if (xnDeck["PromoUnlocks"] != null)
						m_duUnlocksPromo = new DeckUnlocks(gdData, xnDeck["PromoUnlocks"], true);

					if (xnDeck["DeckBoxImage"] != null)
						m_bmpDeckBoxImage = XmlTools.ImageFromNode(xnDeck["DeckBoxImage"]);

					if (xnDeck["ColourOverride"] != null)
					{
						m_bOverrideColours = true;
						m_eOverrideColour = (ColourFlags)Int32.Parse(XmlTools.GetValueFromAttribute(xnDeck["ColourOverride"], "Value", "0"));
					}
				}
				else
					Settings.ReportError(null, ErrorPriority.Low, "File is missing DECK tag: " + Path.GetFileName(m_strFileName) + " in " + m_strWad);
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Low, "Unable to load deck: " + Path.GetFileName(m_strFileName) + " in " + m_strWad);
			}
			m_bEditedDeck = false;
		}

		public Deck(Deck dkToCopy)
		{
			// Basic Information
			// Since this is a new deck rather than a carbon copy don't bring over the filename.
			m_strFileName = string.Empty;
			m_nUid = dkToCopy.Uid;
			// We're not creating a new personality yet, so don't create a new one as a copy.
			m_apPersonality = dkToCopy.Personality;
			m_strPersonality = dkToCopy.PersonalityName;
			m_bmpDeckBoxImage = dkToCopy.DeckBoxImage;
			m_strDeckBoxImage = dkToCopy.DeckBoxImageName;
			m_strDeckBoxImageLocked = dkToCopy.DeckBoxImageLocked;
			m_nContentPack = dkToCopy.ContentPack;
			m_eAvailability = dkToCopy.Availability;
			m_bOverrideColours = dkToCopy.OverrideColours;
			m_eOverrideColour = dkToCopy.OverrideColour;
			Colour = dkToCopy.Colour;

			// Deck Statistics
			m_strCreatureSize = dkToCopy.CreatureSize;
			m_strDeckSpeed = dkToCopy.DeckSpeed;
			m_strFlexiblity = dkToCopy.Flexibility;
			m_strSynergy = dkToCopy.Synergy;

			// Land Config
			m_nIgnoreCmcOver = dkToCopy.IgnoreCmcOver;
			m_nMinForests = dkToCopy.MinForests;
			m_nMinIslands = dkToCopy.MinIslands;
			m_nMinMountains = dkToCopy.MinMountains;
			m_nMinPlains = dkToCopy.MinPlains;
			m_nMinSwamps = dkToCopy.MinSwamps;
			m_nNumberOfSpellsThatCountAsLand = dkToCopy.NumberOfSpellsThatCountAsLands;

			// Cards
			//	We actually want to create new deck cards as copies of those in the deck we're
			//	copying rather than simply copying the references because if we change something
			//	here we don't want to change the original deck.
			m_lstCards = new SortableBindingList<DeckCard>();
			foreach (DeckCard dcCard in dkToCopy.Cards)
				m_lstCards.Add(new DeckCard(dcCard.Card, dcCard.Quantity, dcCard.Bias, dcCard.Promo));

			// Land Pool
			m_bIsLandPool = dkToCopy.IsLandPool;
			if ((!m_bIsLandPool) && (dkToCopy.LandPool != null))
				m_dkLandPool = new Deck(dkToCopy.LandPool);

			// Unlocks
			m_duUnlocksRegular = new DeckUnlocks(dkToCopy.RegularUnlocks);
			m_duUnlocksPromo = new DeckUnlocks(dkToCopy.PromoUnlocks);

			// Other useful stuff to copy
			m_dicDeckName = new Dictionary<string, string>();
			if (dkToCopy.Name != null)
				foreach (KeyValuePair<string, string> kvName in dkToCopy.Name)
					m_dicDeckName.Add(kvName.Key, kvName.Value);

			m_dicDeckDescription = new Dictionary<string, string>();
			if (dkToCopy.Description != null)
				foreach (KeyValuePair<string, string> kvDesc in dkToCopy.Description)
					m_dicDeckDescription.Add(kvDesc.Key, kvDesc.Value);

			// Things that should be initialized
			m_strWad = string.Empty;
			m_bEditedDeck = false;
		}

		private void LoadCards(GameDirectory gdData, XmlNode xnDeck)
		{
			m_lstCards = new SortableBindingList<DeckCard>();
			foreach (XmlNode xnItem in xnDeck.ChildNodes)
			{
				if (xnItem.Name.Equals("CARD", StringComparison.OrdinalIgnoreCase))
				{
					XmlAttribute xaAttr = XmlTools.FindCaseInsensitiveAttribute(xnItem, "name");
					if (xaAttr != null)
					{
						string strCardName = xaAttr.Value.Trim();
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
						DeckCard dcCard = AddCard(gdData, strCardName, nBias, bPromo);
					}
				}
			}
		}

		private ColourFlags GetDeckColour(XmlNode xnDeck)
		{
			ColourFlags eColour = 0;
			if (XmlTools.GetValueFromAttribute(xnDeck, "is_black").Equals("True", StringComparison.OrdinalIgnoreCase))
			{
				if (eColour != 0)
					eColour |= ColourFlags.MultiColour;
				eColour |= ColourFlags.Black;
			}
			if (XmlTools.GetValueFromAttribute(xnDeck, "is_blue").Equals("True", StringComparison.OrdinalIgnoreCase))
			{
				if (eColour != 0)
					eColour |= ColourFlags.MultiColour;
				eColour |= ColourFlags.Blue;
			}
			if (XmlTools.GetValueFromAttribute(xnDeck, "is_green").Equals("True", StringComparison.OrdinalIgnoreCase))
			{
				if (eColour != 0)
					eColour |= ColourFlags.MultiColour;
				eColour |= ColourFlags.Green;
			}
			if (XmlTools.GetValueFromAttribute(xnDeck, "is_red").Equals("True", StringComparison.OrdinalIgnoreCase))
			{
				if (eColour != 0)
					eColour |= ColourFlags.MultiColour;
				eColour |= ColourFlags.Red;
			}
			if (XmlTools.GetValueFromAttribute(xnDeck, "is_white").Equals("True", StringComparison.OrdinalIgnoreCase))
			{
				if (eColour != 0)
					eColour |= ColourFlags.MultiColour;
				eColour |= ColourFlags.White;
			}

			return eColour;
		}

		private string InitDefaultId(string strSetting, string strDefault)
		{
			string strReturn = string.Empty;

			strReturn = Settings.GetSetting(strSetting, string.Empty);
			if (strReturn.Length == 0)
			{
				strReturn = strDefault;
				Settings.SaveSetting(strSetting, strDefault);
			}

			return strReturn;
		}

		private void InitDefaultIds()
		{
			lock (this)
			{
				if (STEAM_UNLOCK_DEFAULT_ID == null)
					STEAM_UNLOCK_DEFAULT_ID = InitDefaultId("SteamDefaultUnlockId", "213850");
				if (STEAM_FOIL_DEFAULT_ID == null)
					STEAM_FOIL_DEFAULT_ID = InitDefaultId("SteamDefaultFoilId", "213850");
				if (IOS_UNLOCK_DEFAULT_ID == null)
					IOS_UNLOCK_DEFAULT_ID = InitDefaultId("iOsDefaultUnlockId", "D14_DECK_UNLOCK_1");
				if (IOS_FOIL_DEFAULT_ID == null)
					IOS_FOIL_DEFAULT_ID = InitDefaultId("iOsDefaultFoilId", "D14_DECK_FOIL_1");
				if (ANDROID_UNLOCK_DEFAULT_ID == null)
					ANDROID_UNLOCK_DEFAULT_ID = InitDefaultId("AndroidDefaultUnlockId", "d14_deck_unlock_01");
				if (ANDROID_FOIL_DEFAULT_ID == null)
					ANDROID_FOIL_DEFAULT_ID = InitDefaultId("AndroidDefaultFoilId", "d14_deck_foil_01");
			}
		}

		private void InitLandPools()
		{
			lock (this)
			{
				if (FOREST_POOL == null)
				{
					FOREST_POOL = Settings.GetSerializableSetting("ForestPool", new string[0]);
					if (FOREST_POOL.Length <= 0)
					{
						FOREST_POOL = new string[] { "FOREST_357833", "FOREST_357834", "FOREST_357835", "FOREST_357836", "FOREST_357837",
													"FOREST_357838", "FOREST_357839", "FOREST_357841", "FOREST_357842", "FOREST_357843",
													"FOREST_357844", "FOREST_357964", "FOREST_357965", "FOREST_357966", "FOREST_357968",
													"FOREST_357970", "FOREST_357971", "FOREST_357972", "FOREST_357973", "FOREST_357979",
													"FOREST_369819", "FOREST_369820" };
						Settings.SaveSerializableSetting("ForestPool", FOREST_POOL);
					}
				}
				if (ISLAND_POOL == null)
				{
					ISLAND_POOL = Settings.GetSerializableSetting("Island", new string[0]);
					if (ISLAND_POOL.Length <= 0)
					{
						ISLAND_POOL = new string[] { "ISLAND_357809", "ISLAND_357810", "ISLAND_357811", "ISLAND_357812", "ISLAND_357813",
													"ISLAND_357814", "ISLAND_357815", "ISLAND_357816", "ISLAND_357819", "ISLAND_357935",
													"ISLAND_357939", "ISLAND_357943", "ISLAND_369824", "ISLAND_369825" };
						Settings.SaveSerializableSetting("Island", ISLAND_POOL);
					}
				}
				if (MOUNTAIN_POOL == null)
				{
					MOUNTAIN_POOL = Settings.GetSerializableSetting("MountainPool", new string[0]);
					if (MOUNTAIN_POOL.Length <= 0)
					{
						MOUNTAIN_POOL = new string[] { "MOUNTAIN_357797", "MOUNTAIN_357798", "MOUNTAIN_357799", "MOUNTAIN_357800", "MOUNTAIN_357801",
													"MOUNTAIN_357802", "MOUNTAIN_357803", "MOUNTAIN_357804", "MOUNTAIN_357805", "MOUNTAIN_357806",
													"MOUNTAIN_357807", "MOUNTAIN_357808", "MOUNTAIN_357947", "MOUNTAIN_357954", "MOUNTAIN_357955",
													"MOUNTAIN_357956", "MOUNTAIN_357957", "MOUNTAIN_357958", "MOUNTAIN_357959", "MOUNTAIN_357960",
													"MOUNTAIN_357961", "MOUNTAIN_357962", "MOUNTAIN_357963", "MOUNTAIN_369827", "MOUNTAIN_369830" };
						Settings.SaveSerializableSetting("MountainPool", MOUNTAIN_POOL);
					}
				}
				if (PLAINS_POOL == null)
				{
					PLAINS_POOL = Settings.GetSerializableSetting("PlainsPool", new string[0]);
					if (PLAINS_POOL.Length <= 0)
					{
						PLAINS_POOL = new string[] { "PLAINS_357846", "PLAINS_357847", "PLAINS_357849", "PLAINS_357850", "PLAINS_357851",
													"PLAINS_357852", "PLAINS_357853", "PLAINS_357855", "PLAINS_357856", "PLAINS_357924",
													"PLAINS_357925", "PLAINS_357926", "PLAINS_357927", "PLAINS_357928", "PLAINS_357930",
													"PLAINS_357931", "PLAINS_357932", "PLAINS_357933", "PLAINS_357981", "PLAINS_357982",
													"PLAINS_357983", "PLAINS_369832", "PLAINS_369833" };
						Settings.SaveSerializableSetting("PlainsPool", PLAINS_POOL);
					}
				}
				if (SWAMP_POOL == null)
				{
					SWAMP_POOL = Settings.GetSerializableSetting("SwampPool", new string[0]);
					if (SWAMP_POOL.Length <= 0)
					{
						SWAMP_POOL = new string[] { "SWAMP_357821", "SWAMP_357822", "SWAMP_357823", "SWAMP_357824", "SWAMP_357825",
													"SWAMP_357826", "SWAMP_357827", "SWAMP_357828", "SWAMP_357829", "SWAMP_357830",
													"SWAMP_357831", "SWAMP_357832", "SWAMP_357944", "SWAMP_357946", "SWAMP_357948",
													"SWAMP_357951", "SWAMP_357952", "SWAMP_357953", "SWAMP_369837", "SWAMP_369838" };
						Settings.SaveSerializableSetting("SwampPool", SWAMP_POOL);
					}
				}
			}
		}

		public string FileName
		{
			get { return m_strFileName; }
			set { m_strFileName=value; }
		}

		public string ExportFileName
		{
			get { return m_strExportFilename; }
		}

		public int Uid
		{
			get { return m_nUid; }
			set { m_nUid = value; }
		}

		public AiPersonality Personality
		{
			get { return m_apPersonality; }
			set { m_apPersonality = value; }
		}

		public string PersonalityName
		{
			get { return m_strPersonality; }
			set { m_strPersonality = value; }
		}

		public Bitmap DeckBoxImage
		{
			get { return m_bmpDeckBoxImage; }
			set { m_bmpDeckBoxImage = value; }
		}

		public string DeckBoxImageName
		{
			get { return m_strDeckBoxImage; }
			set { m_strDeckBoxImage = value; }
		}

		public string DeckBoxImageLocked
		{
			get { return m_strDeckBoxImageLocked; }
			set { m_strDeckBoxImageLocked = value; }
		}

		public int ContentPack
		{
			get { return m_nContentPack; }
			set { m_nContentPack = value; }
		}

		public DeckAvailability Availability
		{
			get { return m_eAvailability; }
			set { m_eAvailability = value; }
		}

		public ColourFlags Colour
		{
			get { return m_eColour; }
			set
			{
				m_eColour = value;
				if (!m_bOverrideColours)
					m_eOverrideColour = value;
			}
		}

		public bool OverrideColours
		{
			get { return m_bOverrideColours; }
			set { m_bOverrideColours = value; }
		}

		public ColourFlags OverrideColour
		{
			get { return m_eOverrideColour; }
			set { m_eOverrideColour = value; }
		}

		public string NameTag
		{
			get { return m_strNameTag; }
			set { m_strNameTag = value; }
		}

		public string DescriptionTag
		{
			get { return m_strDescriptionTag; }
			set { m_strDescriptionTag = value; }
		}

		public string ColourText
		{
			get { return Tools.ColourText(m_eColour); }
		}

		public string CreatureSize
		{
			get { return m_strCreatureSize; }
			set { m_strCreatureSize = value; }
		}

		public string DeckSpeed
		{
			get { return m_strDeckSpeed; }
			set { m_strDeckSpeed = value; }
		}

		public string Flexibility
		{
			get { return m_strFlexiblity; }
			set { m_strFlexiblity = value; }
		}

		public string Synergy
		{
			get { return m_strSynergy; }
			set { m_strSynergy = value; }
		}

		public int IgnoreCmcOver
		{
			get { return m_nIgnoreCmcOver; }
			set { m_nIgnoreCmcOver = value; }
		}

		public int MinForests
		{
			get { return m_nMinForests; }
			set { m_nMinForests = value; }
		}

		public int MinIslands
		{
			get { return m_nMinIslands; }
			set { m_nMinIslands = value; }
		}

		public int MinMountains
		{
			get { return m_nMinMountains; }
			set { m_nMinMountains = value; }
		}

		public int MinPlains
		{
			get { return m_nMinPlains; }
			set { m_nMinPlains = value; }
		}

		public int MinSwamps
		{
			get { return m_nMinSwamps; }
			set { m_nMinSwamps = value; }
		}

		public int NumberOfSpellsThatCountAsLands
		{
			get { return m_nNumberOfSpellsThatCountAsLand; }
			set { m_nNumberOfSpellsThatCountAsLand = value; }
		}

		public SortableBindingList<DeckCard> Cards
		{
			get { return m_lstCards; }
		}

		public Dictionary<string, string> Name
		{
			get { return m_dicDeckName; }
			set { m_dicDeckName = value; }
		}

		public Dictionary<string, string> Description
		{
			get { return m_dicDeckDescription; }
			set { m_dicDeckDescription = value; }
		}

		public bool IsLandPool
		{
			get { return m_bIsLandPool; }
			set { m_bIsLandPool = value; }
		}

		public bool Edited
		{
			get { return m_bEditedDeck; }
			set { m_bEditedDeck = value; }
		}

		public string Wad
		{
			get { return m_strWad; }
			set { m_strWad = value; }
		}

		public Deck LandPool
		{
			get { return m_dkLandPool; }
			set { m_dkLandPool = value; }
		}

		public DeckUnlocks RegularUnlocks
		{
			get { return m_duUnlocksRegular; }
			set { m_duUnlocksRegular = value; }
		}

		public DeckUnlocks PromoUnlocks
		{
			get { return m_duUnlocksPromo; }
			set { m_duUnlocksPromo = value; }
		}

		public string LocalizedName
		{
			get
			{
				if (Settings.Language.MasqueradeAsLangCode != null)
					return GetLocalizedName(Settings.Language.MasqueradeAsLangCode);
				else
					return GetLocalizedName(Settings.Language.LanguageCode);
			}
		}

		public string GetLocalizedName(string strLangCode)
		{
			string strReturn = string.Empty;

			if (m_dicDeckName == null)
			{ }
			else if (m_dicDeckName.ContainsKey(strLangCode))
				strReturn = m_dicDeckName[strLangCode];
			else if (m_dicDeckName.ContainsKey("en-US"))
				strReturn = m_dicDeckName["en-US"];

			return strReturn;
		}

		public int CardCount
		{
			get
			{
				int nCount = 0;
				foreach (DeckCard dcCard in m_lstCards)
					nCount += dcCard.Quantity;
				return nCount;
			}
		}

		public ColourFlags CalculateDeckColour()
		{
			ColourFlags eColour = 0;
			foreach (DeckCard dcCard in m_lstCards)
			{
				if (dcCard.Card.Colour != ColourFlags.Colourless)
					eColour |= dcCard.Card.Colour;
			}
			if ((m_duUnlocksRegular != null) && (m_duUnlocksRegular.Cards != null) && (m_duUnlocksRegular.Cards.Count > 0))
			{
				foreach (DeckCard dcCard in m_duUnlocksRegular.Cards)
					if (dcCard.Card.Colour != ColourFlags.Colourless)
						eColour |= dcCard.Card.Colour;
			}
			if ((m_duUnlocksPromo != null) && (m_duUnlocksPromo.Cards != null) && (m_duUnlocksPromo.Cards.Count > 0))
			{
				foreach (DeckCard dcCard in m_duUnlocksPromo.Cards)
					if (dcCard.Card.Colour != ColourFlags.Colourless)
						eColour |= dcCard.Card.Colour;
			}
			if (eColour == 0)
				eColour = ColourFlags.Colourless;
			Colour = eColour;
			return eColour;
		}

		public int BasicLandAmount
		{
			get
			{
				// There are two factors that determine the amount of basic land in the deck
				//	First is 60 - the number of cards in the deck.
				int nBasicLand = 60 - CardCount;

				// Second is the actual number of basic lands that have been manually added to the deck.
				foreach (DeckCard dcCard in m_lstCards)
				{
					if (((dcCard.Card.SuperTypes & CardSuperType.Basic) == CardSuperType.Basic) &&
						((dcCard.Card.Type & CardType.Land) == CardType.Land))
						nBasicLand += dcCard.Quantity;
				}

				return Math.Max(nBasicLand, 0);
			}
		}

		public int RegularUnlockCardCount
		{
			get
			{
				if ((m_duUnlocksRegular != null) && (m_duUnlocksRegular.Cards != null))
					return m_duUnlocksRegular.Cards.Count;
				else
					return 0;
			}
		}

		public int PromoUnlockCardCount
		{
			get
			{
				if ((m_duUnlocksPromo != null) && (m_duUnlocksPromo.Cards != null))
					return m_duUnlocksPromo.Cards.Count;
				else
					return 0;
			}
		}

		private DeckCard AddCard(GameDirectory gdData, string strCardName, int nBias = 1, bool bPromo = false)
		{
			DeckCard dcCard = null;
			// Check to see if card is already in deck.
			foreach (DeckCard dcTest in m_lstCards)
			{
				if ((dcTest.Card.Filename.Equals(strCardName, StringComparison.OrdinalIgnoreCase)) &&
					(dcTest.Bias == nBias) && (dcTest.Promo == bPromo))
				{
					// Already in deck so just increment quantity.
					dcCard = dcTest;
					dcCard.Quantity++;
					break;
				}
			}
			if (dcCard == null)
			{
				// Not in deck so we need to add.
				CardInfo ciCard = gdData.GetCardByFileName(strCardName);
				if (ciCard != null)
				{
					dcCard = new DeckCard(ciCard, 1, nBias, bPromo);
					m_lstCards.Add(dcCard);
				}
				else
					Settings.ReportError(null, ErrorPriority.Low, "Can't find referenced card: " + strCardName + " for deck " + Path.GetFileNameWithoutExtension(m_strFileName) + " in " + m_strWad);
			}
			return dcCard;
		}

		public DeckCard AddCard(CardInfo ciCard, int nBias = 1, bool bPromo = false)
		{
			DeckCard dcCard = null;
			if (ciCard != null)
			{
				// Check to see if card is already in deck.
				foreach (DeckCard dcTest in m_lstCards)
				{
					if ((dcTest.Card.Filename.Equals(ciCard.Filename, StringComparison.OrdinalIgnoreCase)) &&
						(dcTest.Bias == nBias) && (dcTest.Promo == bPromo))
					{
						// Already in deck so just increment quantity.
						dcCard = dcTest;
						dcCard.Quantity++;
						break;
					}
				}
				if (dcCard == null)
				{
					// Not in deck so we add it.
					dcCard = new DeckCard(ciCard, 1, nBias, bPromo);
					m_lstCards.Add(dcCard);

					// Since we are adding a new card (or simply one with different bias or promo status)
					//	then we should recalculate what colour the deck is based on the cards now in it.
					CalculateDeckColour();
				}
			}
			return dcCard;
		}

		public bool ContainsCard(string strCardName)
		{
			bool bFound = false;

			foreach (DeckCard dcCard in m_lstCards)
			{
				if (dcCard.Card.Filename.Equals(strCardName, StringComparison.OrdinalIgnoreCase))
				{
					bFound = true;
					break;
				}
			}

			return bFound;
		}

		// Remove a card from the deck in X quantity.
		//	If nQuantity is -1 it will completely remove the card from the deck.
		//	Will return the dcCard instance that was found if one was found (though it may or may not remain in the list depending on the quantity to remove).
		public DeckCard RemoveCard(CardInfo ciCard, int nQuantity = 1)
		{
			// Quick return conditions.
			if ((ciCard == null) || (nQuantity == 0))
				return null;

			// We may not find the card we want to remove at all as it may not be in the list so we should initialize as null.
			DeckCard dcFound = null;

			for (int i = 0; i < m_lstCards.Count; i++)
			{
				DeckCard dcCard = m_lstCards[i];
				if (dcCard.Card == ciCard)
				{
					dcFound = dcCard;
					if ((nQuantity == -1) || (dcCard.Quantity <= nQuantity))
					{
						// Requested to remove all or quantity is less than or equal to what we want to remove so simply remove.
						m_lstCards.Remove(dcCard);
					}
					else
					{
						// More than we are removing so we need to return what's left.
						dcCard.Quantity -= nQuantity;
					}

					// We should only find one in the list so we can break out now.
					break;
				}
			}

			return dcFound;
		}

		// This will create an appropriate Land Pool for the deck based on the colours of the deck.
		//	It will add 4 land for each used colour randomly from the appropriate land pool.
		public void CreateLandPool(GameDirectory gdData)
		{
			// Calculate the colour of the deck first to prevent problems.
			CalculateDeckColour();

			// Forcibly create a new land pool (the old one will get garbage collected eventually).
			m_dkLandPool = new Deck(gdData, true);
			m_dkLandPool.Personality = null;
			m_dkLandPool.Availability = DeckAvailability.NeverAvailable;

			bool bAddedLand = false;

			if ((m_eColour & ColourFlags.Green) == ColourFlags.Green)
			{
				AddLandFromPool(gdData, m_dkLandPool, FOREST_POOL);
				bAddedLand = true;
			}

			if ((m_eColour & ColourFlags.Blue) == ColourFlags.Blue)
			{
				AddLandFromPool(gdData, m_dkLandPool, ISLAND_POOL);
				bAddedLand = true;
			}

			if ((m_eColour & ColourFlags.Red) == ColourFlags.Red)
			{
				AddLandFromPool(gdData, m_dkLandPool, MOUNTAIN_POOL);
				bAddedLand = true;
			}

			if ((m_eColour & ColourFlags.White) == ColourFlags.White)
			{
				AddLandFromPool(gdData, m_dkLandPool, PLAINS_POOL);
				bAddedLand = true;
			}

			if ((m_eColour & ColourFlags.Black) == ColourFlags.Black)
			{
				AddLandFromPool(gdData, m_dkLandPool, SWAMP_POOL);
				bAddedLand = true;
			}

			if (!bAddedLand)
			{
				AddLandFromPool(gdData, m_dkLandPool, FOREST_POOL);
				AddLandFromPool(gdData, m_dkLandPool, ISLAND_POOL);
				AddLandFromPool(gdData, m_dkLandPool, MOUNTAIN_POOL);
				AddLandFromPool(gdData, m_dkLandPool, PLAINS_POOL);
				AddLandFromPool(gdData, m_dkLandPool, SWAMP_POOL);
			}
		}

		private void AddLandFromPool(GameDirectory gdData, Deck dkLandPool, string[] astrPool)
		{
			if (astrPool.Length > 4)
			{
				int i = 0;
				while (i < 4)
				{
					string strCard = astrPool[Tools.Random.Next(0, astrPool.Length)];
					if (!dkLandPool.ContainsCard(strCard))
					{
						dkLandPool.AddCard(gdData, strCard);
						i++;
					}
				}
			}
			else
			{
				for (int i = 0; i < astrPool.Length; i++)
					dkLandPool.AddCard(gdData, astrPool[i]);
			}
		}

		// This method can send back (currently) up to 5 XmlDocuments containing (Dictionary is unordered so don't count on any order):
		//	Deck
		//	Land Pool (if finished)
		//  Unlocks (regular & promo, if any of either and finished)
		//	AI Personality (if not built-in and finished)
		public Dictionary<string, XmlDocument> Export(IdScheme isScheme = null, bool bForceExport = false)
		{
			// If we have not been passed in an id scheme then we know that we are not finished with this deck.
			bool bUnfinished = (isScheme == null);

			Dictionary<string, XmlDocument> dicRelated = new Dictionary<string, XmlDocument>();
			XmlDocument xdDeck = new XmlDocument();

			// The base game uses deck XMLs that are saved in ANSI and have no XML Declaration so we will do the same.
			//	If you want the declaration on all decks then comment out the if statement below (but leave the block uncommented).
			if (bUnfinished)
			{
				// Since this is an "unfinished" deck and we store extra information like localized deck names we need the declaration so we can save foreign characters.
				XmlDeclaration xdDeclaration = xdDeck.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
				xdDeck.AppendChild(xdDeclaration);
			}

			// Before we do anything else we should recalculate the deck colour just to make sure that the last card of colour was not removed just before.
			CalculateDeckColour();

			// Filename without extension.
			string strFilename = Path.GetFileNameWithoutExtension(m_strFileName);
			if ((strFilename == null) || (strFilename.Length == 0) || (!bUnfinished))
			{
				string strDeckName = string.Empty;
				if (m_dicDeckName.ContainsKey("en-US"))
					strDeckName = Tools.CodifyName(m_dicDeckName["en-US"]);
				strFilename = "D14_" + isScheme.GetDeckId(m_nUid).ToString() + "_" + strDeckName;
				m_strExportFilename = strFilename;
				m_strNameTag = strFilename;
				m_strDescriptionTag = strFilename + "_DESCRIPTION";
			}

			// If we have a deck box image (actual image) and we are finished then we need to give it a name
			if ((!bUnfinished) && (m_bmpDeckBoxImage != null))
			{
				m_strDeckBoxImage = strFilename + "_DECK_IMAGE";
			}

			// Before we make our deck tag we need to export the AI Personality if any so we can get the finalized filename.
			if (!bUnfinished)
			{
				// We may or may not have a custom personality to export.
				//	NOTE: We may also be forcing a personality export because of changing a deck's IdBlock.
				if ((m_apPersonality != null) && ((!m_apPersonality.BuiltIn) || (bForceExport)))
				{
					KeyValuePair<string, XmlDocument> kvPersonality = m_apPersonality.Export();
					dicRelated.Add(kvPersonality.Key, kvPersonality.Value);
					m_strPersonality = m_apPersonality.Filename;
				}
				else if (m_apPersonality != null)
					m_strPersonality = m_apPersonality.Filename;
			}
			else
			{
				if ((m_apPersonality != null) && ((!m_apPersonality.BuiltIn) || (bForceExport)))
					m_strPersonality = m_apPersonality.Filename;
			}

			XmlNode xnDeckTag = null;
			if (isScheme != null)
				xnDeckTag = CreateDeckTag(xdDeck, isScheme.GetDeckId(m_nUid));
			else
				xnDeckTag = CreateDeckTag(xdDeck, m_nUid);

			CreateDeckStatisticsTag(xdDeck, xnDeckTag);
			CreateLandConfigTag(xdDeck, xnDeckTag);

			int nNextDeckOrderId = CreateCardTags(xdDeck, xnDeckTag, m_lstCards.OrderBy(x => x.CardDeckName()));

			dicRelated.Add(strFilename, xdDeck);

			// If this deck is "unfinished" then we also want to save out some extra information.
			if (bUnfinished)
			{
				XmlNode xnLocalizedNames = xdDeck.CreateElement("LocalizedDeckNames");
				xnDeckTag.AppendChild(xnLocalizedNames);
				XmlTools.SaveLocalizedText(m_dicDeckName, xdDeck, xnLocalizedNames);

				// Don't need to save description if we don't have one.
				if ((m_dicDeckDescription != null) && (m_dicDeckDescription.Count > 0))
				{
					XmlNode xnLocalizedDescription = xdDeck.CreateElement("LocalizedDescriptions");
					xnDeckTag.AppendChild(xnLocalizedDescription);
					XmlTools.SaveLocalizedText(m_dicDeckDescription, xdDeck, xnLocalizedDescription);
				}

				// Put in any custom personality (if any)
				//	NOTE: We may also be forcing a personality export because of changing a deck's IdBlock.
				if ((m_apPersonality != null) && ((!m_apPersonality.BuiltIn) || (bForceExport)))
				{
					XmlNode xnPersonality = xdDeck.CreateElement("AiPersonality");
					xnDeckTag.AppendChild(xnPersonality);
					m_apPersonality.Export(xdDeck, xnPersonality, true);
				}

				// Rather than create and manage an extra land pool in tandem files at this point
				//	we're just going to go ahead and add it in an extra node.
				XmlNode xnLandPool = xdDeck.CreateElement("LandPool");
				xnDeckTag.AppendChild(xnLandPool);
				// We don't really care about the land pool id for an unfinished deck.
				m_dkLandPool.Uid = m_nUid + 1;
				m_dkLandPool.Availability = DeckAvailability.NeverAvailable;
				m_dkLandPool.Export(xdDeck, xnLandPool);

				// Now we need to export any regular unlocks.
				if (m_duUnlocksRegular.Cards.Count > 0)
				{
					XmlNode xnRegularUnlocks = xdDeck.CreateElement("RegularUnlocks");
					xnDeckTag.AppendChild(xnRegularUnlocks);
					nNextDeckOrderId = m_duUnlocksRegular.Export(xdDeck, xnRegularUnlocks, nNextDeckOrderId);
				}

				// Now we need to export any promo unlocks.
				if (m_duUnlocksPromo.Cards.Count > 0)
				{
					XmlNode xnPromoUnlocks = xdDeck.CreateElement("PromoUnlocks");
					xnDeckTag.AppendChild(xnPromoUnlocks);
					nNextDeckOrderId = m_duUnlocksPromo.Export(xdDeck, xnPromoUnlocks, nNextDeckOrderId);
				}

				// Save out our deck box image if any.
				if (m_bmpDeckBoxImage != null)
				{
					XmlNode xnDeckBoxImage = xdDeck.CreateElement("DeckBoxImage");
					xnDeckBoxImage.InnerText = XmlTools.ImageToBase64String(m_bmpDeckBoxImage);
					xnDeckTag.AppendChild(xnDeckBoxImage);
				}

				// Since this is unfinished we need to force out the deck colour overrides separately so that it isn't forgotten on load.
				if (m_bOverrideColours)
				{
					XmlNode xnColourOverride = xdDeck.CreateElement("ColourOverride");
					XmlAttribute xaColour = xdDeck.CreateAttribute("Value");
					xaColour.Value = ((int)m_eOverrideColour).ToString();
					xnColourOverride.Attributes.Append(xaColour);
					xnDeckTag.AppendChild(xnColourOverride);
				}
			}
			else
			{
				// If we're in here then we know that we have a valid id scheme.
				m_dkLandPool.Uid = isScheme.GetLandPoolId(m_nUid);
				m_dkLandPool.Availability = DeckAvailability.NeverAvailable;
				KeyValuePair<string, XmlDocument> kvLandPool = m_dkLandPool.Export(strFilename);
				dicRelated.Add(kvLandPool.Key, kvLandPool.Value);

				// We may or may not have regular and/or promo unlocks.
				if (m_duUnlocksRegular.Cards.Count > 0)
				{
					m_duUnlocksRegular.DeckUid = isScheme.GetDeckId(m_nUid);
					m_duUnlocksRegular.Uid = isScheme.GetRegularUnlockId(m_nUid);
					m_duUnlocksRegular.Promo = false;
					KeyValuePair<int, XmlDocument> kvRegUnlocks = m_duUnlocksRegular.Export(nNextDeckOrderId);
					nNextDeckOrderId = kvRegUnlocks.Key;
					dicRelated.Add(strFilename + "_UNLOCK", kvRegUnlocks.Value);
				}
				if (m_duUnlocksPromo.Cards.Count > 0)
				{
					m_duUnlocksPromo.DeckUid = isScheme.GetDeckId(m_nUid);
					m_duUnlocksPromo.Uid = isScheme.GetPromoUnlockId(m_nUid);
					m_duUnlocksPromo.Promo = true;
					KeyValuePair<int, XmlDocument> kvPromoUnlocks = m_duUnlocksPromo.Export(nNextDeckOrderId);
					nNextDeckOrderId = kvPromoUnlocks.Key;
					dicRelated.Add(strFilename + "_PROMO", kvPromoUnlocks.Value);
				}
			}

			return dicRelated;
		}

		public void Export(XmlDocument xdMainDeck, XmlNode xnLocation)
		{
			// This method is only called on a land pool deck by an unfinished main deck.
			XmlNode xnDeckTag = CreateDeckTag(xdMainDeck, m_nUid, xnLocation);
			CreateCardTags(xdMainDeck, xnDeckTag, m_lstCards.OrderBy(x => x.CardDeckName()));
		}

		public KeyValuePair<string, XmlDocument> Export(string strDeckFileName)
		{
			// This method is only called by a (finished) main deck on a land pool.
			XmlDocument xdDeck = new XmlDocument();

			// The base game uses deck XMLs that are saved in ANSI and have no XML Declaration so we will do the same.
			//	Uncomment the block below to add a UTF-8 XML Declaration to the file.
			/*XmlDeclaration xdDeclaration = xdDeck.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
			xdDeck.AppendChild(xdDeclaration);//*/

			XmlNode xnDeckTag = CreateDeckTag(xdDeck, m_nUid);

			CreateCardTags(xdDeck, xnDeckTag, m_lstCards.OrderBy(x => x.CardDeckName()));

			return new KeyValuePair<string, XmlDocument>(strDeckFileName + "_LAND_POOL", xdDeck);
		}

		private XmlNode CreateDeckTag(XmlDocument xdDeck, int nDeckId, XmlNode xnAlternate = null)
		{
			XmlNode xnDeckTag = xdDeck.CreateElement("DECK");
			if (xnAlternate != null)
				xnAlternate.AppendChild(xnDeckTag);
			else
				xdDeck.AppendChild(xnDeckTag);

			XmlAttribute xaAttr = xdDeck.CreateAttribute("uid");
			xaAttr.Value = nDeckId.ToString();
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("personality");
			xaAttr.Value = m_strPersonality;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("deck_box_image");
			xaAttr.Value = m_strDeckBoxImage;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("deck_box_image_locked");
			xaAttr.Value = m_strDeckBoxImageLocked;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("content_pack");
			xaAttr.Value = m_nContentPack.ToString();
			xnDeckTag.Attributes.Append(xaAttr);

			if (m_eAvailability != DeckAvailability.Locked)
			{
				if (m_eAvailability == DeckAvailability.NeverAvailable)
					xaAttr = xdDeck.CreateAttribute("never_available");
				else
					xaAttr = xdDeck.CreateAttribute("always_available");
				xaAttr.Value = "true";
				xnDeckTag.Attributes.Append(xaAttr);
			}

			AddColoursToDeckTag(xdDeck, xnDeckTag);

			// These are the new attributes added in DotP 2014
			xaAttr = xdDeck.CreateAttribute("cheat_menu_filter_deck_type");
			xaAttr.Value = (m_bIsLandPool ? "Utility" : "Standard");
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("tus_save_data_id");
			xaAttr.Value = nDeckId.ToString();
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("ios_id_1");
			xaAttr.Value = IOS_UNLOCK_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("ios_id_2");
			xaAttr.Value = IOS_FOIL_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("steam_id_1");
			xaAttr.Value = STEAM_UNLOCK_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("steam_id_2");
			xaAttr.Value = STEAM_FOIL_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("android_id_1");
			xaAttr.Value = ANDROID_UNLOCK_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("android_id_2");
			xaAttr.Value = ANDROID_FOIL_DEFAULT_ID;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("cheat_menu_filter_datapool");
			xaAttr.Value = "D14";
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("name_tag");
			xaAttr.Value = m_strNameTag;
			xnDeckTag.Attributes.Append(xaAttr);

			xaAttr = xdDeck.CreateAttribute("description_tag");
			xaAttr.Value = m_strDescriptionTag;
			xnDeckTag.Attributes.Append(xaAttr);

			return xnDeckTag;
		}

		private void AddColoursToDeckTag(XmlDocument xdDeck, XmlNode xnDeckBlock)
		{
			ColourFlags eColour = m_eColour;

			// Allow for colour overriding.
			if (m_bOverrideColours)
				eColour = m_eOverrideColour;

			if ((eColour & ColourFlags.Black) == ColourFlags.Black)
			{
				XmlAttribute xaAttr = xdDeck.CreateAttribute("is_black");
				xaAttr.Value = "true";
				xnDeckBlock.Attributes.Append(xaAttr);
			}
			if ((eColour & ColourFlags.Blue) == ColourFlags.Blue)
			{
				XmlAttribute xaAttr = xdDeck.CreateAttribute("is_blue");
				xaAttr.Value = "true";
				xnDeckBlock.Attributes.Append(xaAttr);
			}
			if ((eColour & ColourFlags.Green) == ColourFlags.Green)
			{
				XmlAttribute xaAttr = xdDeck.CreateAttribute("is_green");
				xaAttr.Value = "true";
				xnDeckBlock.Attributes.Append(xaAttr);
			}
			if ((eColour & ColourFlags.Red) == ColourFlags.Red)
			{
				XmlAttribute xaAttr = xdDeck.CreateAttribute("is_red");
				xaAttr.Value = "true";
				xnDeckBlock.Attributes.Append(xaAttr);
			}
			if ((eColour & ColourFlags.White) == ColourFlags.White)
			{
				XmlAttribute xaAttr = xdDeck.CreateAttribute("is_white");
				xaAttr.Value = "true";
				xnDeckBlock.Attributes.Append(xaAttr);
			}
		}

		private void CreateDeckStatisticsTag(XmlDocument xdDeck, XmlNode xnDeckBlock)
		{
			if (!m_bIsLandPool)
			{
				XmlNode xnDeckStats = xdDeck.CreateElement("DECKSTATISTICS");
				xnDeckBlock.AppendChild(xnDeckStats);

				XmlAttribute xaAttr = xdDeck.CreateAttribute("Size");
				xaAttr.Value = m_strCreatureSize;
				xnDeckStats.Attributes.Append(xaAttr);

				xaAttr = xdDeck.CreateAttribute("Speed");
				xaAttr.Value = m_strDeckSpeed;
				xnDeckStats.Attributes.Append(xaAttr);

				xaAttr = xdDeck.CreateAttribute("Flex");
				xaAttr.Value = m_strFlexiblity;
				xnDeckStats.Attributes.Append(xaAttr);

				xaAttr = xdDeck.CreateAttribute("Syn");
				xaAttr.Value = m_strSynergy;
				xnDeckStats.Attributes.Append(xaAttr);
			}
		}

		private void CreateLandConfigTag(XmlDocument xdDeck, XmlNode xnDeckBlock)
		{
			// We only write this tag if we actually want to configure the land.
			if ((m_nIgnoreCmcOver > -1) ||
				(m_nMinForests > 0) ||
				(m_nMinIslands > 0) ||
				(m_nMinMountains > 0) ||
				(m_nMinPlains > 0) ||
				(m_nMinSwamps > 0) ||
				(m_nNumberOfSpellsThatCountAsLand > 0))
			{
				XmlNode xnDeckStats = xdDeck.CreateElement("LandConfig");
				xnDeckBlock.AppendChild(xnDeckStats);

				XmlAttribute xaAttr = null;
				if (m_nIgnoreCmcOver > -1)
				{
					xaAttr = xdDeck.CreateAttribute("ignoreCmcOver");
					xaAttr.Value = m_nIgnoreCmcOver.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nMinForests > 0)
				{
					xaAttr = xdDeck.CreateAttribute("minForest");
					xaAttr.Value = m_nMinForests.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nMinIslands > 0)
				{
					xaAttr = xdDeck.CreateAttribute("minIsland");
					xaAttr.Value = m_nMinIslands.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nMinMountains > 0)
				{
					xaAttr = xdDeck.CreateAttribute("minMountain");
					xaAttr.Value = m_nMinMountains.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nMinPlains > 0)
				{
					xaAttr = xdDeck.CreateAttribute("minPlains");
					xaAttr.Value = m_nMinPlains.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nMinSwamps > 0)
				{
					xaAttr = xdDeck.CreateAttribute("minSwamp");
					xaAttr.Value = m_nMinSwamps.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
				if (m_nNumberOfSpellsThatCountAsLand > 0)
				{
					xaAttr = xdDeck.CreateAttribute("numSpellsThatCountAsLand");
					xaAttr.Value = m_nNumberOfSpellsThatCountAsLand.ToString();
					xnDeckStats.Attributes.Append(xaAttr);
				}
			}
		}

		private int CreateCardTags(XmlDocument xdDeck, XmlNode xnDeckBlock, IEnumerable<DeckCard> cards)
		{
			// Check to see if we are pre-shuffling.
			if ((!m_bIsLandPool) && (Settings.GetSetting("PreShuffleOnExport", false)))
			{
				// We need to pre-shuffle (the object being to try and prevent consecutive deckOrderIds).
				cards = PreShuffleCards(cards);
			}

			int nDeckOrderId = 0;
			foreach (DeckCard dcCard in cards)
			{
				for (int i = 0; i < dcCard.Quantity; i++)
				{
					XmlNode xnCard = xdDeck.CreateElement("CARD");
					XmlAttribute xaName = xdDeck.CreateAttribute("name");
					xaName.Value = dcCard.CardDeckName();
					xnCard.Attributes.Append(xaName);
					XmlAttribute xaDeckOrderId = xdDeck.CreateAttribute("deckOrderId");
					xaDeckOrderId.Value = nDeckOrderId.ToString();
					xnCard.Attributes.Append(xaDeckOrderId);
					xnDeckBlock.AppendChild(xnCard);
					nDeckOrderId++;
				}
			}

			// Return our next available deckOrderId (used for unlocks)
			return nDeckOrderId;
		}

		private IEnumerable<DeckCard> PreShuffleCards(IEnumerable<DeckCard> cards)
		{
			// First thing to do is expand the list so that no card has a quantity greater than 1.
			List<DeckCard> lstCards = new List<DeckCard>();
			foreach (DeckCard dcCard in cards)
			{
				for (int i = 0; i < dcCard.Quantity; i++)
					lstCards.Add(new DeckCard(dcCard.Card, 1, dcCard.Bias, dcCard.Promo));
			}

			// Now that we have a simple list of cards, so now shuffle.
			//	We will do up to 7 rounds trying to get one that has no consecutive identical cards.
			for (int nRound = 0; nRound < 7; nRound++)
			{
				// Do the shuffle
				for (int i = 0; i < (lstCards.Count - 1); i++)
				{
					int nSwap = Tools.Random.Next(i + 1, lstCards.Count);
					DeckCard dcTemp = lstCards[i];
					lstCards[i] = lstCards[nSwap];
					lstCards[nSwap] = dcTemp;
				}

				// Check for consecutive cards.
				bool bFoundIdentical = false;
				DeckCard dcLast = null;
				foreach (DeckCard dcCard in lstCards)
				{
					if (dcLast != null)
					{
						if (dcLast.Card.Filename.Equals(dcCard.Card.Filename, StringComparison.OrdinalIgnoreCase))
						{
							bFoundIdentical = true;
							break;
						}
					}
					dcLast = dcCard;
				}
				// Do we have a good shuffle?
				if (!bFoundIdentical)
					break;
			}

			return lstCards;
		}
	}
}
