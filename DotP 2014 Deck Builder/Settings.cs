using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace RSN.Tools
{
	public enum ErrorPriority
	{
		Critical = 4,
		High = 3,
		Medium = 2,
		Low = 1,
		Zero = 0
	}

	public static class Settings
	{
		public const bool MAINTAIN_IMAGE_CACHE_DEFAULT = false;

		private static XmlDocument m_xdSettings = new XmlDocument();
		private static MessageLog m_mlErrorLog = new MessageLog("Errors.log");
		private static Dictionary<Type, XmlSerializer> m_dicSerializers = new Dictionary<Type,XmlSerializer>();

		public static bool LogAllErrors = false;
		public static bool IncludeBOM = true;
		public static string LanguageCode = "en-US";
		public static Dictionary<string, LanguageEntry> Languages;
		public static LanguageStrings UIStrings;

		public static void InitDefaults()
		{
			Languages = new Dictionary<string, LanguageEntry>();
			// Fill Languages list.
			Languages.Add("en-US", new LanguageEntry("en-US", "EN", "English", "Master Text", 3));
			Languages.Add("fr-FR", new LanguageEntry("fr-FR", "FR", "français", "French", 4));
			Languages.Add("es-ES", new LanguageEntry("es-ES", "ES", "español", "Spanish", 5));
			Languages.Add("de-DE", new LanguageEntry("de-DE", "DE", "Deutsch", "German", 6));
			Languages.Add("it-IT", new LanguageEntry("it-IT", "IT", "italiano", "Italian", 7));
			Languages.Add("jp-JA", new LanguageEntry("jp-JA", "JP", "日本語", "Japanese", 9));
			Languages.Add("ko-KR", new LanguageEntry("ko-KR", "KO", "한국의", "Korean", 10));
			Languages.Add("ru-RU", new LanguageEntry("ru-RU", "RU", "русский", "Russian", 11));
			Languages.Add("pt-BR", new LanguageEntry("pt-BR", "PT", "português", "Portuguese (Brazil)", 12));
			Languages.Add("zh-CN", new LanguageEntry("zh-CN", "CN", "中文（简体）", "Chinese Simplified", 15));
			Languages.Add("zh-HK", new LanguageEntry("zh-HK", "HK", "中文（繁体）", "Chinese Traditional", 16));
		}

		public static void LoadSettings()
		{
			m_xdSettings = new XmlDocument();
			if (File.Exists(GetProgramDir() + "Settings.xml"))
			{
				// Settings file found attempt to load it.
				try
				{
					m_xdSettings.Load(GetProgramDir() + "Settings.xml");

					// Here is where we recover the global settings
					IncludeBOM = GetSetting("IncludeBOM", IncludeBOM);
					LogAllErrors = GetSetting("LogAllErrors", LogAllErrors);
					LanguageCode = GetSetting("LanguageCode", LanguageCode);
					List<LanguageEntry> lstLangs = GetSerializableSetting("LanguageEntries", new List<LanguageEntry>());
					if (lstLangs.Count > 0)
					{
						Languages = new Dictionary<string,LanguageEntry>();
						foreach (LanguageEntry lang in lstLangs)
							Languages.Add(lang.LanguageCode, lang);
					}
				}
				catch (Exception e)
				{
					// Medium priority error since the file exists we should be able to load it.
					//	GetSetting() should never throw an error if I have coded it right.
					ReportError(e, ErrorPriority.Medium);
				}
			}
			else
			{
				// No settings file found so setup our XML declaration for saving a new one later.
				XmlDeclaration xdDeclaration = m_xdSettings.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
				m_xdSettings.AppendChild(xdDeclaration);
			}
			UIStrings = new LanguageStrings(LanguageCode);
		}

		public static void SaveSettings()
		{
			try
			{
				// Save out our global settings.
				SaveSetting("IncludeBOM", IncludeBOM);
				SaveSetting("LogAllErrors", LogAllErrors);
				SaveSetting("LanguageCode", LanguageCode);
				SaveSerializableSetting("LanguageEntries", new List<LanguageEntry>(Languages.Values));

				// Save out the settings file.
				m_xdSettings.Save(GetProgramDir() + "Settings.xml");
			}
			catch (Exception e)
			{
				ReportError(e, ErrorPriority.Critical);
			}
		}

		public static void Cleanup()
		{
			// Close any open logs and do any other necessary cleanup.
			m_mlErrorLog.Close();
		}

		public static void ChangeLanguage(string strLangCode)
		{
			LanguageCode = strLangCode;
			UIStrings = new LanguageStrings(LanguageCode);
		}

		public static void ReportError(Exception eError, ErrorPriority epPriority, string strOptionalInfo = "")
		{
			string strExtraInfo = string.Empty;
			if (strOptionalInfo.Length > 0)
				strExtraInfo = "\r\nExtra Information:\r\n" + strOptionalInfo;
			if ((epPriority != ErrorPriority.Zero) || (LogAllErrors))
			{
				if (eError != null)
					m_mlErrorLog.WriteMessage(epPriority.ToString() + ": " + eError.Source + " - " + eError.Message + ":\r\n" + eError.StackTrace + strExtraInfo + "\r\n");
				else
					m_mlErrorLog.WriteMessage(epPriority.ToString() + ": " + strOptionalInfo + "\r\n");
			}
		}

		public static string GetSetting(string strSettingName, string strDefault)
		{
			string strReturn = strDefault;
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				XmlNode xnSetting = xnlSettings[0];
				strReturn = xnSetting.InnerText.Trim();
			}
			return strReturn;
		}

		public static void SaveSetting(string strSettingName, string strValue)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
				xnlSettings[0].InnerText = strValue;
			else
			{
				XmlNode xnRoot = GetRootNode();
				XmlNode xnSetting = m_xdSettings.CreateElement(strSettingName);
				XmlCDataSection xcdData = m_xdSettings.CreateCDataSection(strValue);
				xnSetting.AppendChild(xcdData);
				xnRoot.AppendChild(xnSetting);
			}
		}

		public static bool GetSetting(string strSettingName, bool bDefault)
		{
			bool bReturn = bDefault;
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				XmlNode xnSetting = xnlSettings[0];
				Boolean.TryParse(xnSetting.InnerText.Trim(), out bReturn);
			}
			return bReturn;
		}

		public static void SaveSetting(string strSettingName, bool bValue)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
				xnlSettings[0].InnerText = bValue.ToString();
			else
			{
				XmlNode xnRoot = GetRootNode();
				XmlNode xnSetting = m_xdSettings.CreateElement(strSettingName);
				xnSetting.InnerText = bValue.ToString();
				xnRoot.AppendChild(xnSetting);
			}
		}

		public static int GetSetting(string strSettingName, int nDefault)
		{
			int nReturn = nDefault;
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				XmlNode xnSetting = xnlSettings[0];
				Int32.TryParse(xnSetting.InnerText.Trim(), out nReturn);
			}
			return nReturn;
		}

		public static void SaveSetting(string strSettingName, int nValue)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
				xnlSettings[0].InnerText = nValue.ToString();
			else
			{
				XmlNode xnRoot = GetRootNode();
				XmlNode xnSetting = m_xdSettings.CreateElement(strSettingName);
				xnSetting.InnerText = nValue.ToString();
				xnRoot.AppendChild(xnSetting);
			}
		}

		public static Rectangle GetSetting(string strSettingName, Rectangle rcDefault)
		{
			Rectangle rcReturn = rcDefault;
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				XmlNode xnSetting = xnlSettings[0];
				try
				{
					Rectangle rcGet = new Rectangle();
					rcGet.X = Int32.Parse(xnSetting.Attributes["x"].Value.Trim());
					rcGet.Y = Int32.Parse(xnSetting.Attributes["y"].Value.Trim());
					rcGet.Width = Int32.Parse(xnSetting.Attributes["width"].Value.Trim());
					rcGet.Height = Int32.Parse(xnSetting.Attributes["height"].Value.Trim());//*/
					rcReturn = rcGet;
				}
				catch (Exception e)
				{
					// We don't particularly care if it fails to parse the value for the setting because we'll just use the default.
					ReportError(e, ErrorPriority.Zero);
				}
			}
			return rcReturn;
		}

		public static void SaveSetting(string strSettingName, Rectangle rcValue)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				xnlSettings[0].Attributes["x"].Value = rcValue.X.ToString();
				xnlSettings[0].Attributes["y"].Value = rcValue.Y.ToString();
				xnlSettings[0].Attributes["width"].Value = rcValue.Width.ToString();
				xnlSettings[0].Attributes["height"].Value = rcValue.Height.ToString();
			}
			else
			{
				XmlNode xnRoot = GetRootNode();
				XmlNode xnSetting = m_xdSettings.CreateElement(strSettingName);
				XmlAttribute xaAttribute = m_xdSettings.CreateAttribute("x");
				xaAttribute.Value = rcValue.X.ToString();
				xnSetting.Attributes.Append(xaAttribute);
				xaAttribute = m_xdSettings.CreateAttribute("y");
				xaAttribute.Value = rcValue.Y.ToString();
				xnSetting.Attributes.Append(xaAttribute);
				xaAttribute = m_xdSettings.CreateAttribute("width");
				xaAttribute.Value = rcValue.Width.ToString();
				xnSetting.Attributes.Append(xaAttribute);
				xaAttribute = m_xdSettings.CreateAttribute("height");
				xaAttribute.Value = rcValue.Height.ToString();
				xnSetting.Attributes.Append(xaAttribute);
				xnRoot.AppendChild(xnSetting);
			}
		}

		public static Dictionary<string, string> GetSetting(string strSettingName, string strItemName, string strKeyAttribute, string strDefaultKey, string strDefaultValue)
		{
			Dictionary<string, string> dicReturn = new Dictionary<string, string>();
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				XmlNodeList xnlEntries = xnlSettings[0].SelectNodes(strItemName);
				foreach (XmlNode xnEntry in xnlEntries)
				{
					if (xnEntry.Attributes[strKeyAttribute] != null)
						dicReturn.Add(xnEntry.Attributes[strKeyAttribute].Value.Trim(), xnEntry.InnerText.Trim());
				}
			}
			if ((dicReturn.Count == 0) && (strDefaultKey != null))
				dicReturn.Add(strDefaultKey, strDefaultValue);
			return dicReturn;
		}

		public static void SaveSetting(string strSettingName, string strItemName, string strKeyAttribute, Dictionary<string, string> dicValue)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			XmlNode xnRoot = GetRootNode();
			XmlNode xnSetting = null;
			if (xnlSettings.Count > 0)
			{
				// Due to this being a list we are going to need to destroy the previous contents before we re-create them.
				xnlSettings[0].InnerText = string.Empty;
				xnSetting = xnlSettings[0];
			}
			else
			{
				xnSetting = m_xdSettings.CreateElement(strSettingName);
				xnRoot.AppendChild(xnSetting);
			}
			// We now have a valid empty element to store our dictionary.
			foreach (KeyValuePair<string, string> kvItem in dicValue)
			{
				XmlNode xnItem = m_xdSettings.CreateElement(strItemName);
				XmlAttribute xaAttribute = m_xdSettings.CreateAttribute(strKeyAttribute);
				xaAttribute.Value = kvItem.Key;
				xnItem.Attributes.Append(xaAttribute);
				XmlCDataSection xcdData = m_xdSettings.CreateCDataSection(kvItem.Value);
				xnItem.AppendChild(xcdData);
				xnSetting.AppendChild(xnItem);
			}
		}

		public static bool GetSetting(string strSettingName, DataGridViewColumnCollection dgvccColl)
		{
			bool bFound = false;

			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				bFound = true;
				XmlNodeList xnlEntries = xnlSettings[0].SelectNodes("Column");
				foreach (XmlNode xnEntry in xnlEntries)
				{
					if (xnEntry.Attributes["Name"] != null)
					{
						DataGridViewColumn dgvcColumn = null;
						string strName = xnEntry.Attributes["Name"].Value.Trim();
						if (dgvccColl.Contains(strName))
						{
							// Column exists so we just need to retrieve it before we move on.
							dgvcColumn = dgvccColl[strName];
						}
						else
						{
							// Column does not exist in the collection so we're going to have to create it.
							if (xnEntry.Attributes["Type"] != null)
							{
								string strType = xnEntry.Attributes["Type"].Value.Trim();
								try
								{
									Type typInfo = Type.GetType(strType);
									dgvcColumn = Activator.CreateInstance(typInfo) as DataGridViewColumn;
									dgvcColumn.Name = strName;
									dgvccColl.Add(dgvcColumn);
								}
								catch (Exception e)
								{
									dgvcColumn = null;
									ReportError(e, ErrorPriority.Medium, string.Empty);
								}
							}
						}
						if (dgvcColumn != null)
						{
							// Ok, we have a valid column to work with, the hard part is over.
							if (xnEntry.Attributes["DataProperty"] != null)
								dgvcColumn.DataPropertyName = xnEntry.Attributes["DataProperty"].Value.Trim();
							if (xnEntry.Attributes["HeaderText"] != null)
							{
								dgvcColumn.Tag = xnEntry.Attributes["HeaderText"].Value.Trim();
								dgvcColumn.HeaderText = UIStrings[(string)dgvcColumn.Tag];
							}
							if (xnEntry.Attributes["SortMode"] != null)
							{
								DataGridViewColumnSortMode eSort;
								if (Enum.TryParse(xnEntry.Attributes["SortMode"].Value.Trim(), out eSort))
									dgvcColumn.SortMode = eSort;
								else
									dgvcColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
							}
							if (xnEntry.Attributes["Width"] != null)
								dgvcColumn.Width = Int32.Parse(xnEntry.Attributes["Width"].Value.Trim());
							if (xnEntry.Attributes["Visible"] != null)
								dgvcColumn.Visible = Boolean.Parse(xnEntry.Attributes["Visible"].Value.Trim());
							if (xnEntry.Attributes["DisplayIndex"] != null)
								dgvcColumn.DisplayIndex = Int32.Parse(xnEntry.Attributes["DisplayIndex"].Value.Trim());
						}
					}
				}
			}

			return bFound;
		}

		public static void SaveSetting(string strSettingName, DataGridViewColumnCollection dgvccColl)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			XmlNode xnRoot = GetRootNode();
			XmlNode xnSetting = null;
			if (xnlSettings.Count > 0)
			{
				// Due to this being a list we are going to need to destroy the previous contents before we re-create them.
				xnlSettings[0].InnerText = string.Empty;
				xnSetting = xnlSettings[0];
			}
			else
			{
				xnSetting = m_xdSettings.CreateElement(strSettingName);
				xnRoot.AppendChild(xnSetting);
			}
			// We now have a valid empty element to store our collection.
			DataGridViewColumn[] dgvcArrary = new DataGridViewColumn[dgvccColl.Count];
			dgvccColl.CopyTo(dgvcArrary, 0);
			foreach (DataGridViewColumn dgvcColumn in dgvcArrary.OrderBy(x => x.DisplayIndex))
			{
				XmlNode xnColumn = m_xdSettings.CreateElement("Column");

				XmlAttribute xaAttr = m_xdSettings.CreateAttribute("Name");
				xaAttr.Value = dgvcColumn.Name;
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("Type");
				xaAttr.Value = dgvcColumn.GetType().AssemblyQualifiedName;
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("DataProperty");
				xaAttr.Value = dgvcColumn.DataPropertyName;
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("HeaderText");
				xaAttr.Value = (string)dgvcColumn.Tag;
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("SortMode");
				xaAttr.Value = dgvcColumn.SortMode.ToString();
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("Width");
				xaAttr.Value = dgvcColumn.Width.ToString();
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("Visible");
				xaAttr.Value = dgvcColumn.Visible.ToString();
				xnColumn.Attributes.Append(xaAttr);

				xaAttr = m_xdSettings.CreateAttribute("DisplayIndex");
				xaAttr.Value = dgvcColumn.DisplayIndex.ToString();
				xnColumn.Attributes.Append(xaAttr);

				xnSetting.AppendChild(xnColumn);
			}
		}

		public static T GetSerializableSetting<T>(string strSettingName, T oDefault)
		{
			T savedObj = oDefault;

			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			if (xnlSettings.Count > 0)
			{
				MemoryStream msXml = new MemoryStream(Encoding.UTF8.GetBytes(xnlSettings[0].InnerXml));
				XmlSerializer xsLoad = null;
				if (m_dicSerializers.ContainsKey(typeof(T)))
					xsLoad = m_dicSerializers[typeof(T)];
				else
				{
					Type[] atypKnown = Assembly.GetExecutingAssembly().GetTypes().Where(t => (typeof(T).IsAssignableFrom(t) && (t != oDefault.GetType()) && (t.Name.IndexOf('`') == -1))).ToArray();
					if (atypKnown.Length > 0)
						xsLoad = new XmlSerializer(oDefault.GetType(), atypKnown);
					else
						xsLoad = new XmlSerializer(typeof(T));
					m_dicSerializers.Add(typeof(T), xsLoad);
				}
				savedObj = (T)xsLoad.Deserialize(msXml);
			}

			return savedObj;
		}

		public static void SaveSerializableSetting<T>(string strSettingName, T oSave)
		{
			XmlNodeList xnlSettings = m_xdSettings.GetElementsByTagName(strSettingName);
			XmlNode xnRoot = GetRootNode();
			XmlNode xnSetting = null;
			if (xnlSettings.Count > 0)
			{
				// Due to this being something that has sub-items we will need to destroy the contents first.
				xnlSettings[0].InnerXml = string.Empty;
				xnSetting = xnlSettings[0];
			}
			else
			{
				xnSetting = m_xdSettings.CreateElement(strSettingName);
				xnRoot.AppendChild(xnSetting);
			}
			// Now save out the serializable object.
			StringWriter swSave = new StringWriter();
			XmlSerializer xsSave = null;
			if (m_dicSerializers.ContainsKey(typeof(T)))
				xsSave = m_dicSerializers[typeof(T)];
			else
			{
				Type[] atypKnown = Assembly.GetExecutingAssembly().GetTypes().Where(t => (typeof(T).IsAssignableFrom(t) && (t != oSave.GetType()) && (t.Name.IndexOf('`') == -1))).ToArray();
				if (atypKnown.Length > 0)
					xsSave = new XmlSerializer(oSave.GetType(), atypKnown);
				else
					xsSave = new XmlSerializer(typeof(T));
				m_dicSerializers.Add(typeof(T), xsSave);
			}
			xsSave.Serialize(swSave, oSave);
			// Because it wants to save out with it's own declaration we need to strip it.
			XmlDocument xdSave = new XmlDocument();
			xdSave.LoadXml(swSave.ToString());
			foreach (XmlNode xnNode in xdSave)
			{
				if (xnNode is XmlDeclaration)
					xdSave.RemoveChild(xnNode);
			}
			// Now we should have only 1 node under the document.
			if ((xdSave.HasChildNodes) && (xdSave.ChildNodes.Count == 1))
			{
				TransplantNode(m_xdSettings, xnSetting, xdSave.ChildNodes[0]);
			}
		}

		private static void TransplantNode(XmlDocument xdNewDoc, XmlNode xnTargetParent, XmlNode xnNodeToTransplant)
		{
			// Since we can't actually transplant nodes we actually do a deep copy.
			//	Though since in this program we don't really use namespaces and
			//	attributes for much I should only really have to copy Elements,
			//	CData sections, and Text (without having to worry about attributes).
			if (xnNodeToTransplant is XmlElement)
			{
				XmlNode xnNewNode = xdNewDoc.CreateElement(xnNodeToTransplant.Name);
				xnTargetParent.AppendChild(xnNewNode);
				if (xnNodeToTransplant.HasChildNodes)
				{
					foreach (XmlNode xnChild in xnNodeToTransplant.ChildNodes)
						TransplantNode(xdNewDoc, xnNewNode, xnChild);
				}
				else if (xnNodeToTransplant.InnerText.Length > 0)
					xnNewNode.InnerText = xnNodeToTransplant.InnerText;
			}
			else if (xnNodeToTransplant is XmlCDataSection)
				xnTargetParent.AppendChild(xdNewDoc.CreateCDataSection(xnNodeToTransplant.InnerText));
			else if (xnNodeToTransplant is XmlText)
				xnTargetParent.AppendChild(xdNewDoc.CreateTextNode(xnNodeToTransplant.InnerText));
		}

		// Will return with trailing back-slash.
		public static string GetProgramDir()
		{
			string strAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			string strPath = Path.GetDirectoryName(strAssemblyPath);
			if (strPath.LastIndexOf('\\') < (strPath.Length - 1))
				strPath = strPath + '\\';
			return strPath;
		}

		public static string GetAssemblyTitle()
		{
			object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			if (attributes.Length > 0)
			{
				AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
				if (titleAttribute.Title.Length > 0)
				{
					return titleAttribute.Title;
				}
			}
			return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
		}

		private static XmlNode GetRootNode()
		{
			XmlNode xnReturn = null;
			if (m_xdSettings["Settings"] != null)
				xnReturn = m_xdSettings["Settings"];
			else
			{
				xnReturn = m_xdSettings.CreateElement("Settings");
				m_xdSettings.AppendChild(xnReturn);
			}

			return xnReturn;
		}
	}
}
