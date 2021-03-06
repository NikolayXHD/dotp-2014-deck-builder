﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using RSN.Tools;

namespace RSN.DotP
{
	public class AiPersonality : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string m_strFilename;					// Just the name of the file with extension.
		private string m_strNameId;
		private Dictionary<string, string> m_dicName;
		private string m_strLargeAvatarImage;           // This is a high-res full image now (not masked)
        private int m_iLargeAvatarImageLoaded;
        private XmlNode m_xnLargeAvatarImageNode;
        private GameDirectory m_gdLargeAvatarImageGD;
        private Bitmap m_bmpLargeAvatarImage;
		private string m_strSmallAvatarImage;			// Circular Image (Medium & Small are the same)
        private int m_iSmallAvatarImageLoaded;
        private XmlNode m_xnSmallAvatarImageNode;
        private GameDirectory m_gdSmallAvatarImageGD;
		private Bitmap m_bmpSmallAvatarImage;
		private string m_strSmallAvatarLockedImage;		// Circular Locked Image
        private int m_iSmallAvatarLockedImageLoaded;
        private XmlNode m_xnSmallAvatarLockedImageNode;
        private GameDirectory m_gdSmallAvatarLockedImageGD;
		private Bitmap m_bmpSmallAvatarLockedImage;
		private string m_strLobbyImage;					// This is the backplate now.
        private int m_iLobbyImageLoaded;
        private XmlNode m_xnLobbyImageNode;
        private GameDirectory m_gdLobbyImageGD;
		private Bitmap m_bmpLobbyImage;
		private string m_strMusicMix;					// Music to play during battle.

		// Non-persisted data
		private bool m_bBuiltIn;

		public AiPersonality()
		{
			m_strFilename = string.Empty;
			m_dicName = new Dictionary<string, string>();
			m_dicName.Add("en-US", "New Personality");
			m_bmpLargeAvatarImage = null;
			m_strLargeAvatarImage = string.Empty;
            m_iLargeAvatarImageLoaded = 0;
            m_xnLargeAvatarImageNode = null;
            m_gdLargeAvatarImageGD = null;
            m_bmpSmallAvatarImage = null;
			m_strSmallAvatarImage = string.Empty;
            m_iSmallAvatarImageLoaded = 0;
            m_xnSmallAvatarImageNode = null;
            m_gdSmallAvatarImageGD = null;
            m_bmpSmallAvatarLockedImage = null;
			m_strSmallAvatarLockedImage = string.Empty;
            m_iSmallAvatarLockedImageLoaded = 0;
            m_xnSmallAvatarLockedImageNode = null;
            m_gdSmallAvatarLockedImageGD = null;
            m_bmpLobbyImage = null;
			m_strLobbyImage = string.Empty;
            m_iLobbyImageLoaded = 0;
            m_xnLobbyImageNode = null;
            m_gdLobbyImageGD = null;
            m_bBuiltIn = false;
		}

		public AiPersonality(GameDirectory gdData, string strXml, string strFilename)
		{
			try
			{
				XmlDocument xdDoc = new XmlDocument();
				xdDoc.LoadXml(strXml);
				foreach (XmlNode xnItem in xdDoc.ChildNodes)
				{
					if (xnItem.Name.Equals("CONFIG", StringComparison.OrdinalIgnoreCase))
						LoadFromNode(gdData, xnItem);
				}
				m_strFilename = Path.GetFileName(strFilename);
			}
			catch (Exception e)
			{
				Settings.ReportError(e, ErrorPriority.Medium, "Unable to load AI Personality: " + strFilename);
			}
		}

		public AiPersonality(GameDirectory gdData, XmlNode xnNode)
		{
			LoadFromNode(gdData, xnNode);
		}

		public AiPersonality(AiPersonality apCopyFrom)
		{
			// We are creating a new personality from this one so we don't get the filename.
			m_strFilename = string.Empty;
			m_strNameId = apCopyFrom.NameId;
			m_dicName = new Dictionary<string, string>();
			if (apCopyFrom.Name != null)
				foreach (KeyValuePair<string, string> kvName in apCopyFrom.Name)
					m_dicName.Add(kvName.Key, kvName.Value);
			m_strLargeAvatarImage = apCopyFrom.LargeAvatarImageName;
			m_bmpLargeAvatarImage = apCopyFrom.LargeAvatarImage;
			m_strSmallAvatarImage = apCopyFrom.SmallAvatarImageName;
			m_bmpSmallAvatarImage = apCopyFrom.SmallAvatarImage;
			m_strSmallAvatarLockedImage = apCopyFrom.SmallAvatarLockedImageName;
			m_bmpSmallAvatarLockedImage = apCopyFrom.SmallAvatarLockedImage;
			m_strLobbyImage = apCopyFrom.LobbyImageName;
			m_bmpLobbyImage = apCopyFrom.LobbyImage;
			m_strMusicMix = apCopyFrom.MusicMix;
			m_bBuiltIn = false;
		}

		private void LoadFromNode(GameDirectory gdData, XmlNode xnNode)
		{
			// Basic information that should always be there.
			m_strNameId = XmlTools.GetValueFromChildsAttribute(xnNode, "PLANESWALKER_NAME_TAG", "string").ToUpper();
			m_strLargeAvatarImage = XmlTools.GetValueFromChildsAttribute(xnNode, "LARGE_AVATAR_IMAGE", "string");
			m_strSmallAvatarImage = XmlTools.GetValueFromChildsAttribute(xnNode, "SMALL_AVATAR_IMAGE", "string");
			m_strSmallAvatarLockedImage = XmlTools.GetValueFromChildsAttribute(xnNode, "SMALL_AVATAR_IMAGE_LOCKED", "string");
			m_strLobbyImage = XmlTools.GetValueFromChildsAttribute(xnNode, "LOBBY_IMAGE", "string");
			m_strMusicMix = XmlTools.GetValueFromChildsAttribute(xnNode, "MUSIC", "string");

			// These are only present if created by this program so I shouldn't have to worry about case here.
			if (xnNode["LocalizedNames"] != null)
				m_dicName = XmlTools.GetLocalizedText(xnNode["LocalizedNames"]);
			else if (gdData.StringTable.ContainsKey(m_strNameId))
				m_dicName = gdData.StringTable[m_strNameId];
			else
				m_dicName = new Dictionary<string, string>();

            if (xnNode["LargeAvatar"] != null)
            {
                m_xnLargeAvatarImageNode = xnNode;
                m_iLargeAvatarImageLoaded = 2;
            }
            else
            {
                m_gdLargeAvatarImageGD = gdData;
                m_iLargeAvatarImageLoaded = 3;
            }

            if (xnNode["SmallAvatar"] != null)
            {
                m_xnSmallAvatarImageNode = xnNode;
                m_iSmallAvatarImageLoaded = 2;
            }
            else
            {
                m_gdSmallAvatarImageGD = gdData;
                m_iSmallAvatarImageLoaded = 3;
            }

            if (xnNode["SmallAvatarLocked"] != null)
            {
                m_xnSmallAvatarLockedImageNode = xnNode;
                m_iSmallAvatarLockedImageLoaded = 2;
            }
            else
            {
                m_gdSmallAvatarLockedImageGD = gdData;
                m_iSmallAvatarLockedImageLoaded = 3;
            }

            if (xnNode["LobbyImage"] != null)
            {
                m_xnLobbyImageNode = xnNode;
                m_iLobbyImageLoaded = 2;
            }
            else
            {
                m_gdLobbyImageGD = gdData;
                m_iLobbyImageLoaded = 3;
            }
        }

		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}

		public string NameId
		{
			get { return m_strNameId; }
			set { m_strNameId = value; }
		}

		public Dictionary<string, string> Name
		{
			get { return m_dicName; }
			set
			{
				m_dicName = value;
				TriggerPropChange("LocalizedName");
			}
		}

		public string LocalizedName
		{
			get
			{
				if (m_dicName.ContainsKey(Settings.Language.LanguageCode))
					return m_dicName[Settings.Language.LanguageCode];
				else if ((Settings.Language.MasqueradeAsLangCode != null) && (m_dicName.ContainsKey(Settings.Language.MasqueradeAsLangCode)))
					return m_dicName[Settings.Language.MasqueradeAsLangCode];
				else if (m_dicName.ContainsKey("en-US"))
					return m_dicName["en-US"];
				else
					return "New Personality";
			}
		}

		public string LargeAvatarImageName
		{
            get { return m_strLargeAvatarImage; }
            set { m_strLargeAvatarImage = value; }
		}

		public Bitmap LargeAvatarImage
		{
            get
            {
                m_bmpLargeAvatarImage = Tools.LoadDeferredImage(m_strLargeAvatarImage, m_iLargeAvatarImageLoaded, m_xnLargeAvatarImageNode, m_gdLargeAvatarImageGD, m_bmpLargeAvatarImage);
                return m_bmpLargeAvatarImage;
            }
            set { m_bmpLargeAvatarImage = value; }
		}

		public string SmallAvatarImageName
		{
			get
            { return m_strSmallAvatarImage; }
			set { m_strSmallAvatarImage = value; }
		}

		public Bitmap SmallAvatarImage
		{
			get
            {
                m_bmpSmallAvatarImage = Tools.LoadDeferredImage(m_strSmallAvatarImage, m_iSmallAvatarImageLoaded, m_xnSmallAvatarImageNode, m_gdSmallAvatarImageGD, m_bmpSmallAvatarImage);
                return m_bmpSmallAvatarImage;
            }
            set { m_bmpSmallAvatarImage = value; }
		}

		public string SmallAvatarLockedImageName
		{
			get { return m_strSmallAvatarLockedImage; }
			set { m_strSmallAvatarLockedImage = value; }
		}

		public Bitmap SmallAvatarLockedImage
		{
            get
            {
                m_bmpSmallAvatarLockedImage = Tools.LoadDeferredImage(m_strSmallAvatarLockedImage, m_iSmallAvatarLockedImageLoaded, m_xnSmallAvatarLockedImageNode, m_gdSmallAvatarLockedImageGD, m_bmpSmallAvatarLockedImage);
                return m_bmpSmallAvatarLockedImage;
            }
            set { m_bmpSmallAvatarLockedImage = value; }
		}

		public string LobbyImageName
		{
			get { return m_strLobbyImage; }
			set { m_strLobbyImage = value; }
		}

		public Bitmap LobbyImage
		{
            get
            {
                m_bmpLobbyImage = Tools.LoadDeferredImage(m_strLobbyImage, m_iLobbyImageLoaded, m_xnLobbyImageNode, m_gdLobbyImageGD, m_bmpLobbyImage);
                return m_bmpLobbyImage;
            }
            set
            {
                if (m_bmpLobbyImage != null)
                    m_bmpLobbyImage.Dispose();
                m_bmpLobbyImage = value;
            }
		}

		public string MusicMix
		{
			get { return m_strMusicMix; }
			set { m_strMusicMix = value; }
		}

		public bool BuiltIn
		{
			get { return m_bBuiltIn; }
			set { m_bBuiltIn = value; }
		}

		public KeyValuePair<string, XmlDocument> Export()
		{
			if (!m_dicName.ContainsKey("en-US"))
				throw new Exception("No English localized name specified, unable to export.");

			XmlDocument xdDoc = new XmlDocument();

			// The base game uses personality XMLs that are saved in ANSI and have no XML Declaration so we will do the same.
			//	If you want the declaration on all personalities then uncomment the section below.
			/*XmlDeclaration xdDeclaration = xdDeck.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
			xdDeck.AppendChild(xdDeclaration);//*/

			XmlNode xnRoot = xdDoc.CreateElement("CONFIG");
			xdDoc.AppendChild(xnRoot);

			// If the user is using the Block Id setting then we can have our files include that Id to
			//	make things more unique with less likelyhood of conflicts.
			IdScheme isScheme = Settings.GetSerializableSetting("CurrentIdScheme", new IdScheme());
			string strBlockPrefix = string.Empty;
			if (isScheme.UseIdBlock)
				strBlockPrefix = isScheme.IdBlock.ToString() + "_";

			// Now we should generate a file name for this personality.
			//	I don't particularly care if this overrides an existing one so I don't do any checking.
			string strCodified = strBlockPrefix + Tools.CodifyName(m_dicName["en-US"]);
			string strFilename = "D14_PERSONALITY_" + strCodified + ".XML";
			Filename = strFilename;
			NameId = "PLAYER_NAME_" + strCodified;

			// Now give our images names.
			if (m_bmpLargeAvatarImage != null)
				LargeAvatarImageName = "D14_PW_" + strCodified + "_FULL";
			if (m_bmpSmallAvatarImage != null)
				SmallAvatarImageName = "D14_PW_" + strCodified;
			if ((m_bmpSmallAvatarLockedImage != null) || (m_bmpSmallAvatarImage != null))
				SmallAvatarLockedImageName = "D14_PW_" + strCodified + "_LOCKED";
			if ((m_bmpSmallAvatarLockedImage == null) && (m_bmpSmallAvatarImage != null))
				m_bmpSmallAvatarLockedImage = Tools.BluescaleImage(m_bmpSmallAvatarImage);
			if (m_bmpLobbyImage != null)
				m_strLobbyImage = "D14_PW_" + strCodified + "_BACKPLATE";

			// Call our other export since it does just about everthing else we need.
			Export(xdDoc, xnRoot);

			// Now we return what we needed to export (images and string table will be saved out separately).
			return new KeyValuePair<string, XmlDocument>(strFilename, xdDoc);
		}

		public void Export(XmlDocument xdDoc, XmlNode xnNode, bool bSaveAll = false)
		{
			// Basic info
			AddStringTag(xdDoc, xnNode, "PLANESWALKER_NAME_TAG", m_strNameId);
			AddStringTag(xdDoc, xnNode, "LARGE_AVATAR_IMAGE", m_strLargeAvatarImage);
			AddStringTag(xdDoc, xnNode, "MEDIUM_AVATAR_IMAGE", m_strSmallAvatarImage);
			AddStringTag(xdDoc, xnNode, "SMALL_AVATAR_IMAGE", m_strSmallAvatarImage);
			AddStringTag(xdDoc, xnNode, "SMALL_AVATAR_IMAGE_LOCKED", m_strSmallAvatarLockedImage);
			AddStringTag(xdDoc, xnNode, "LOBBY_IMAGE", m_strLobbyImage);
			AddStringTag(xdDoc, xnNode, "MUSIC", m_strMusicMix);

			if (bSaveAll)
			{
				// Save off our localized names.
				if (m_dicName != null)
				{
					XmlNode xnLocalized = xdDoc.CreateElement("LocalizedNames");
					xnNode.AppendChild(xnLocalized);
					XmlTools.SaveLocalizedText(m_dicName, xdDoc, xnLocalized);
				}
				// Now we save off the images to the xml.
				if (m_bmpLargeAvatarImage != null)
					AddImageNode(xdDoc, xnNode, "LargeAvatar", m_bmpLargeAvatarImage);
				if (m_bmpSmallAvatarImage != null)
					AddImageNode(xdDoc, xnNode, "SmallAvatar", m_bmpSmallAvatarImage);
				if (m_bmpSmallAvatarLockedImage != null)
					AddImageNode(xdDoc, xnNode, "SmallAvatarLocked", m_bmpSmallAvatarLockedImage);
				if (m_bmpLobbyImage != null)
					AddImageNode(xdDoc, xnNode, "LobbyImage", m_bmpLobbyImage);
			}
		}

		private void AddImageNode(XmlDocument xdDoc, XmlNode xnNode, string strTagName, Bitmap bmpImage)
		{
			XmlNode xnTag = xdDoc.CreateElement(strTagName.Trim());
			xnTag.InnerText = XmlTools.ImageToBase64String(bmpImage);
			xnNode.AppendChild(xnTag);
		}

		private void AddStringTag(XmlDocument xdDoc, XmlNode xnNode, string strTagName, string strValue)
		{
			XmlNode xnTag = xdDoc.CreateElement(strTagName.Trim());
			XmlAttribute xaAttr = xdDoc.CreateAttribute("string");
			if (strValue != null)
				xaAttr.Value = strValue.Trim();
			xnTag.Attributes.Append(xaAttr);
			xnNode.AppendChild(xnTag);
		}

		private void TriggerPropChange(string strProp)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(strProp));
		}

        public void Dispose()
        {
            //public event PropertyChangedEventHandler PropertyChanged;
            if (m_bmpLargeAvatarImage != null)
            {
                m_bmpLargeAvatarImage.Dispose();
                m_bmpLargeAvatarImage = null;
            }
            if (m_bmpSmallAvatarImage != null)
            {
                m_bmpSmallAvatarImage.Dispose();
                m_bmpSmallAvatarImage = null;
            }
            if (m_bmpSmallAvatarLockedImage != null)
            {
                m_bmpSmallAvatarLockedImage.Dispose();
                m_bmpSmallAvatarLockedImage = null;
            }
            if (m_bmpLobbyImage != null)
            {
                m_bmpLobbyImage.Dispose();
                m_bmpLobbyImage = null;
            }
        }
	}
}
