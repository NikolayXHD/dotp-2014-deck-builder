using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using RSN.Tools;

namespace RSN.DotP
{
	public class XmlTools
	{
		private const string NAMESPACE_ID = "urn:schemas-microsoft-com:office:spreadsheet";
		private const string LOCALIZED_TEXT = "LOCALISED_TEXT";	// Note: WotC misspelled this element when coding.
		private const string LANG_CODE = "LanguageCode";
		private const string FALLBACK_LANG_CODE = "en-US";

		public static string GetEnglishLocalizedText(XmlNode xml)
		{
			return GetLocalizedText(xml, "en-US");
		}

		public static string GetLocalizedText(XmlNode xml, string strLanguageCode)
		{
			if (HasLocalizedText(xml))
			{
				string strReturn = string.Empty;
				foreach (XmlNode xnItem in xml.ChildNodes)
				{
					if (xnItem.Name.Equals(LOCALIZED_TEXT, StringComparison.OrdinalIgnoreCase))
					{
						if (GetValueFromAttribute(xnItem, LANG_CODE).Equals(strLanguageCode, StringComparison.OrdinalIgnoreCase))
						{
							strReturn = CleanUpText(xnItem.InnerText);
							break;
						}
					}
				}
				return strReturn;
			}
			else
				return string.Empty;
		}

		public static Dictionary<string, string> GetLocalizedText(XmlNode xml)
		{
			if (HasLocalizedText(xml))
			{
				Dictionary<string, string> dicReturn = new Dictionary<string, string>();
				foreach (XmlNode xnItem in xml.ChildNodes)
				{
					if (xnItem.Name.Equals(LOCALIZED_TEXT, StringComparison.OrdinalIgnoreCase))
					{
						string strLangCode = GetValueFromAttribute(xnItem, LANG_CODE);
						if (strLangCode.Length > 0)
						{
							if (dicReturn.ContainsKey(strLangCode))
							{
								// Ooh, there is an issue in this localized text block.
								Settings.ReportError(null, ErrorPriority.Low, "XML Block has multiple localizations defined for same language code (" + strLangCode + "):\r\n" + xml.OuterXml);
							}
							else
								dicReturn.Add(strLangCode, CleanUpText(xnItem.InnerText));
						}
					}
				}
				return dicReturn;
			}
			else
				return new Dictionary<string, string>();
		}

		public static string GetLocalizedText(Dictionary<string, string> dicLocalized, string strLangCode)
		{
			string strReturn = string.Empty;

			if (dicLocalized.ContainsKey(strLangCode))
				strReturn = dicLocalized[strLangCode];
			else if (dicLocalized.ContainsKey(FALLBACK_LANG_CODE))
				strReturn = dicLocalized[FALLBACK_LANG_CODE];

			return strReturn;
		}

		private static string CleanUpText(string strText)
		{
			string strReturn = strText.Trim();

			// Save our good \r\n instances so we don't accidently double them.
			strReturn = strReturn.Replace("\r\n", "{newline}");
			// Change any single \r or \n to \r\n
			strReturn = strReturn.Replace("\r", "{newline}").Replace("\n", "{newline}");
			// Now restore our good \r\n instances.
			strReturn = strReturn.Replace("{newline}", "\r\n");

			return strReturn;
		}

		public static bool HasLocalizedText(XmlNode xml)
		{
			bool bHasText = false;

			foreach (XmlNode xnItem in xml.ChildNodes)
			{
				if (xnItem.Name.Equals(LOCALIZED_TEXT, StringComparison.OrdinalIgnoreCase))
				{
					bHasText = true;
					break;
				}
			}

			return bHasText;
		}

		public static void SaveLocalizedString(string strLangCode, string strString, XmlDocument xdDoc, XmlNode xnNodeToSaveIn)
		{
			XmlNode xnLocalized = xdDoc.CreateElement(LOCALIZED_TEXT);
			XmlAttribute xaLangCode = xdDoc.CreateAttribute(LANG_CODE);
			xaLangCode.Value = strLangCode.Trim();
			xnLocalized.Attributes.Append(xaLangCode);
			XmlCDataSection xcdData = xdDoc.CreateCDataSection(strString.Trim());
			xnLocalized.AppendChild(xcdData);
			xnNodeToSaveIn.AppendChild(xnLocalized);
		}

		public static void SaveLocalizedText(Dictionary<string, string> dicLocalized, XmlDocument xdDoc, XmlNode xnNodeToSaveIn)
		{
			// By doing things this way we only write out the text we have rather than creating bogus entries.
			foreach (KeyValuePair<string, string> kvPair in dicLocalized)
				SaveLocalizedString(kvPair.Key, kvPair.Value, xdDoc, xnNodeToSaveIn);
		}

		public static void SaveFullLocalizedText(Dictionary<string, string> dicLocalized, XmlDocument xdDoc, XmlNode xnNodeToSaveIn)
		{
			// Here we save off a full localized text block with all of our supported lang codes.
			//	If text for a specific lang code does not exist then we will try to use en-US.
			//	If text does not exist for en-US we will duplicate whatever the first lang code we run across is.

			// First we look for our fallback localization (English if possible).
			string strFallBack = null;
			if (dicLocalized.ContainsKey(FALLBACK_LANG_CODE))
				strFallBack = dicLocalized[FALLBACK_LANG_CODE];
			else
			{
				// We could not get an English localization for the fallback so get the first localization we come across.
				if (dicLocalized.Count > 0)
					strFallBack = dicLocalized.Values.GetEnumerator().Current;
				else
					strFallBack = string.Empty;
			}

			// Now that we have our fallback we start outputting.
			//	For simplicity we will use our supported languages from the Settings object.
			foreach (KeyValuePair<string, LanguageEntry> kvLang in Settings.Languages)
			{
				if (dicLocalized.ContainsKey(kvLang.Key))
					SaveLocalizedString(kvLang.Key, dicLocalized[kvLang.Key], xdDoc, xnNodeToSaveIn);
				else
					SaveLocalizedString(kvLang.Key, strFallBack, xdDoc, xnNodeToSaveIn);
			}
		}

		public static Bitmap ImageFromNode(XmlNode xnNode)
		{
			string strImage = xnNode.InnerText;
			MemoryStream msImage = new MemoryStream(Convert.FromBase64String(strImage));
			// This looks odd, but if we want to close the memory stream we need to first
			//	create a bitmap from the stream then create a bitmap from the created bitmap.
			Bitmap bmpTemp = new Bitmap(msImage);
			Bitmap bmpReturn = new Bitmap(bmpTemp);
			// Now we can get rid of the temp image and close the memory stream.
			bmpTemp.Dispose();
			msImage.Close();

			return bmpReturn;
		}

		public static string ImageToBase64String(Image imgPic)
		{
			MemoryStream msImage = new MemoryStream();
			imgPic.Save(msImage, System.Drawing.Imaging.ImageFormat.Png);
			string strReturn = Convert.ToBase64String(msImage.ToArray());
			msImage.Close();

			return strReturn;
		}

		public static XmlDocument CreateStringTable(params KeyValuePair<string, Dictionary<string, string>>[] akvpParams)
		{
			XmlDocument xdDoc = PrepOutputXml();
			XmlNode xnWorkbook = CreateOuptutWorkbook(xdDoc);
			// WotC uses Sheet4 for the name, I don't think it is important, but I'll leave it for the moment.
			XmlNode xnWorksheet = CreateOutputWorksheet(xdDoc, xnWorkbook, "Sheet4");

			CreateHeaderRow(xdDoc, xnWorksheet);

			foreach (KeyValuePair<string, Dictionary<string, string>> kvpString in akvpParams)
			{
				if ((kvpString.Value != null) && (kvpString.Value.Count > 0))
					CreateStringRow(xdDoc, xnWorksheet, kvpString.Key, kvpString.Value);
			}

			return xdDoc;
		}

		private static XmlDocument PrepOutputXml()
		{
			XmlDocument xdDoc = new XmlDocument();
			XmlDeclaration xdDec = xdDoc.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, null);
			xdDoc.AppendChild(xdDec);

			XmlProcessingInstruction xpi = xdDoc.CreateProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
			xdDoc.AppendChild(xpi);

			return xdDoc;
		}

		private static XmlNode CreateOuptutWorkbook(XmlDocument xdDoc)
		{
			// Create the workbook itself.
			XmlNode xnWorkbook = xdDoc.CreateElement("Workbook");
			XmlAttribute xaAttr = xdDoc.CreateAttribute("xmlns");
			xaAttr.Value = "urn:schemas-microsoft-com:office:spreadsheet";
			xnWorkbook.Attributes.Append(xaAttr);
			xaAttr = xdDoc.CreateAttribute("xmlns", "ss", "http://www.w3.org/2000/xmlns/");
			xaAttr.Value = NAMESPACE_ID;
			xnWorkbook.Attributes.Append(xaAttr);
			xdDoc.AppendChild(xnWorkbook);

			// Now create the document properties for it.
			XmlNode xnDocProp = xdDoc.CreateElement("DocumentProperties");
			xaAttr = xdDoc.CreateAttribute("xmlns");
			xaAttr.Value = "urn:schemas-microsoft-com:office:office";
			xnDocProp.Attributes.Append(xaAttr);
			xnWorkbook.AppendChild(xnDocProp);

			XmlNode xnProp = xdDoc.CreateElement("Author");
			xnProp.InnerText = Settings.GetAssemblyTitle();
			xnDocProp.AppendChild(xnProp);

			xnProp = xdDoc.CreateElement("Created");
			xnProp.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
			xnDocProp.AppendChild(xnProp);

			xnProp = xdDoc.CreateElement("Company");
			xnProp.InnerText = string.Empty;
			xnDocProp.AppendChild(xnProp);

			return xnWorkbook;
		}

		private static XmlNode CreateOutputWorksheet(XmlDocument xdDoc, XmlNode xnWorkbook, string strName)
		{
			XmlNode xnWorksheet = xdDoc.CreateElement("Worksheet");
			XmlAttribute xaAttr = xdDoc.CreateAttribute("ss", "Name", NAMESPACE_ID);
			xaAttr.Value = strName;
			xnWorksheet.Attributes.Append(xaAttr);
			xnWorkbook.AppendChild(xnWorksheet);

			XmlNode xnTable = xdDoc.CreateElement("Table");
			xnWorksheet.AppendChild(xnTable);

			return xnTable;
		}

		private static void CreateHeaderRow(XmlDocument xdDoc, XmlNode xnTable)
		{
			XmlNode xnRow = xdDoc.CreateElement("Row");
			xnTable.AppendChild(xnRow);

			CreateCellWithData(xdDoc, xnRow, "Ident");
			CreateCellWithData(xdDoc, xnRow, "Comment");
			int nIndex = 3;
			foreach (LanguageEntry lang in Settings.Languages.Values.OrderBy(t => t.TextIndex))
			{
				if (nIndex == lang.TextIndex)
					CreateCellWithData(xdDoc, xnRow, lang.TextHeader);
				else
				{
					nIndex = lang.TextIndex;
					CreateCellWithData(xdDoc, xnRow, lang.TextHeader, nIndex);
				}
				nIndex++;
			}
		}

		private static void CreateStringRow(XmlDocument xdDoc, XmlNode xnTable, string strId, Dictionary<string, string> dicLocalized)
		{
			if ((strId != null) && (dicLocalized != null))
			{
				XmlNode xnRow = xdDoc.CreateElement("Row");
				xnTable.AppendChild(xnRow);

				CreateCellWithData(xdDoc, xnRow, strId);
				int nIndex = 2;
				foreach (LanguageEntry lang in Settings.Languages.Values.OrderBy(t => t.TextIndex))
				{
					if (nIndex == lang.TextIndex)
						CreateCellWithData(xdDoc, xnRow, GetLocalizedText(dicLocalized, lang.LanguageCode));
					else
					{
						nIndex = lang.TextIndex;
						CreateCellWithData(xdDoc, xnRow, GetLocalizedText(dicLocalized, lang.LanguageCode), nIndex);
					}
					nIndex++;
				}
			}
		}

		private static void CreateCellWithData(XmlDocument xdDoc, XmlNode xnRow, string strData, int nIndex = 0)
		{
			XmlNode xnCell = xdDoc.CreateElement("Cell");
			if (nIndex > 0)
			{
				XmlAttribute xaIndex = xdDoc.CreateAttribute("ss", "Index", NAMESPACE_ID);
				xaIndex.Value = nIndex.ToString();
				xnCell.Attributes.Append(xaIndex);
			}
			xnRow.AppendChild(xnCell);
			XmlNode xnData = xdDoc.CreateElement("Data");
			XmlAttribute xaAttr = xdDoc.CreateAttribute("ss", "Type", NAMESPACE_ID);
			xaAttr.Value = "String";
			xnData.Attributes.Append(xaAttr);
			xnData.InnerText = strData;
			xnCell.AppendChild(xnData);
		}

		public static XmlAttribute FindCaseInsensitiveAttribute(XmlNode xnNode, string strAttr)
		{
			XmlAttribute xaFound = null;

			foreach (XmlAttribute xaAttr in xnNode.Attributes)
			{
				if (xaAttr.Name.Equals(strAttr, StringComparison.OrdinalIgnoreCase))
				{
					xaFound = xaAttr;
					break;
				}
			}

			return xaFound;
		}

		public static string GetValueFromAttribute(XmlNode xnNode, string strAttribute, string strDefault = "")
		{
			string strReturn = strDefault;

			XmlAttribute xaAttr = FindCaseInsensitiveAttribute(xnNode, strAttribute);
			if (xaAttr != null)
				strReturn = xaAttr.Value.Trim();

			return strReturn;
		}

		public static string GetValueFromChildsAttribute(XmlNode xnParent, string strChildTag, string strAttribute, string strDefault = "")
		{
			string strReturn = strDefault;

			foreach (XmlNode xnChild in xnParent.ChildNodes)
			{
				if (xnChild.Name.Equals(strChildTag, StringComparison.OrdinalIgnoreCase))
					strReturn = GetValueFromAttribute(xnChild, strAttribute, strDefault);
			}

			return strReturn;
		}

		public static XmlNode GetChild(XmlNode xnParent, string strChildTag)
		{
			XmlNode xnFound = null;

			foreach (XmlNode xnChild in xnParent.ChildNodes)
			{
				if (xnChild.Name.Equals(strChildTag, StringComparison.OrdinalIgnoreCase))
				{
					xnFound = xnChild;
					break;
				}
			}

			return xnFound;
		}

		public static int CountTags(XmlNode xnParent, string strChildTag)
		{
			int nCount = 0;

			foreach (XmlNode xnItem in xnParent.ChildNodes)
			{
				if (xnItem.Name.Equals(strChildTag, StringComparison.OrdinalIgnoreCase))
					nCount++;
			}

			return nCount;
		}

		public static string GetHeaderBaseDir(string strXml, string strBaseContains)
		{
			string strReturn = null;

			XmlDocument xdHeader = new XmlDocument();
			xdHeader.LoadXml(strXml);
			XmlNode xnRoot = GetChild(xdHeader, "WAD_HEADER");

			foreach (XmlNode xnChild in xnRoot.ChildNodes)
			{
				if (xnChild.Name.Equals("ENTRY", StringComparison.OrdinalIgnoreCase))
				{
					string strSource = GetValueFromAttribute(xnChild, "source");
					if ((strSource.Length > 0) && (strSource.IndexOf(strBaseContains, StringComparison.OrdinalIgnoreCase) > -1))
					{
						int nIndex = strSource.IndexOf(strBaseContains, StringComparison.OrdinalIgnoreCase);
						strReturn = strSource.Substring(0, nIndex).Replace("/", "\\");
						break;
					}
				}
			}

			return strReturn;
		}

		public static XmlElement AddElementToNode(XmlDocument xdDoc, XmlNode xnParent, string strElement)
		{
			XmlElement xeElement = xdDoc.CreateElement(strElement);
			xnParent.AppendChild(xeElement);
			return xeElement;
		}

		public static XmlAttribute AddAttributeToNode(XmlDocument xdDoc, XmlNode xnNode, string strAttribute, string strValue)
		{
			XmlAttribute xaAttr = xdDoc.CreateAttribute(strAttribute);
			xaAttr.Value = strValue;
			xnNode.Attributes.Append(xaAttr);
			return xaAttr;
		}
	}
}
