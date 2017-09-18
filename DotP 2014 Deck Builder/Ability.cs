using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using RSN.Tools;

namespace RSN.DotP
{
	public class Ability
	{
		private string m_strXml;

		private Dictionary<string, string> m_dicText;
		private AbilityType m_eType;
		private bool m_bCommaSpace;						// commaspace
		private int m_nResourceId;						// resource_id

		public Ability(string strXml)
		{
			m_strXml = strXml;
			XmlDocument xdDoc = new XmlDocument();
			xdDoc.LoadXml(strXml);
			XmlNode xnAbility = xdDoc.FirstChild;
			if (!Enum.TryParse(xnAbility.Name, out m_eType))
			{
				m_eType = (AbilityType)(-1);
				Settings.ReportError(null, ErrorPriority.Low, "Invalid ability detected: " + xnAbility.Name);
			}
			m_bCommaSpace = XmlTools.GetValueFromAttribute(xnAbility, "commaspace").Equals("1");
			m_nResourceId = -1;		// -1 used to indicate no resource_id.
			m_nResourceId = Int32.Parse(XmlTools.GetValueFromAttribute(xnAbility, "resource_id", "-1"));
			if (XmlTools.HasLocalizedText(xnAbility))
				m_dicText = XmlTools.GetLocalizedText(xnAbility);
			else
				m_dicText = null;
		}

		public Dictionary<string, string> Text
		{
			get { return m_dicText; }
		}

		public string LocalizedText
		{
			get
			{
				string strReturn = string.Empty;
				if (m_dicText != null)
				{
					if (m_dicText.ContainsKey(Settings.Language.LanguageCode))
						strReturn = m_dicText[Settings.Language.LanguageCode];
					else if ((Settings.Language.MasqueradeAsLangCode != null) && (m_dicText.ContainsKey(Settings.Language.MasqueradeAsLangCode)))
						strReturn = m_dicText[Settings.Language.MasqueradeAsLangCode];
					else if (m_dicText.ContainsKey("en-US"))
						strReturn = m_dicText["en-US"];
				}
				return strReturn;
			}
		}

		public AbilityType Type
		{
			get { return m_eType; }
		}

		public bool CommaSpace
		{
			get { return m_bCommaSpace; }
		}

		public int ResourceId
		{
			get { return m_nResourceId; }
		}
	}
}
