using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RSN.Tools
{
	public class LanguageStrings
	{
		private Dictionary<string, string> m_dicLocalizedText;

		public LanguageStrings(string strLangCode)
		{
			m_dicLocalizedText = new Dictionary<string, string>();
			// Load the localized strings from an XML file containing the translations.
			string strFile = Settings.GetProgramDir() + "Lang\\DeckBuilder2014_" + strLangCode + ".xml";
			if (File.Exists(strFile))
			{
				XmlDocument xDoc = new XmlDocument();
				try
				{
					xDoc.Load(strFile);
				}
				catch (Exception e)
				{
					System.Windows.Forms.MessageBox.Show("Unable to load XML language file: Lang\\DeckBuilder2014_" + strLangCode + ".xml\n" +
						e.Message + e.StackTrace, "Unable to load language!", 
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					return;
				}
				XmlNode xmlRoot = xDoc["LOCALIZATION"];
				XmlNodeList xmlStrings = xmlRoot.SelectNodes("STRING");
				foreach (XmlNode xmlString in xmlStrings)
					m_dicLocalizedText.Add(xmlString.Attributes["key"].Value.Trim().ToUpper(), xmlString.InnerText.Trim());
			}
		}

		public string this[string strTextKey]
		{
			get
			{
				if (m_dicLocalizedText.ContainsKey(strTextKey))
					return m_dicLocalizedText[strTextKey];
				else
					return '[' + strTextKey + ']';
			}
		}
	}
}
