using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.Tools
{
	public class LanguageEntry
	{
		public string m_strLanguageCode;
		public string m_strShortCode;
		public string m_strText;
		public string m_strTextHeader;
		public int m_nTextIndex;

		public LanguageEntry()
		{ }
		
		public LanguageEntry(string strLangCode, string strShortCode, string strText, string strTextHeader, int nTextIndex)
		{
			m_strLanguageCode = strLangCode;
			m_strShortCode = strShortCode;
			m_strText = strText;
			m_strTextHeader = strTextHeader;
			m_nTextIndex = nTextIndex;
		}

		public string LanguageCode
		{
			get { return m_strLanguageCode; }
			set { m_strLanguageCode = value; }
		}

		public string ShortCode
		{
			get { return m_strShortCode; }
			set { m_strShortCode = value; }
		}

		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}

		public string TextHeader
		{
			get { return m_strTextHeader; }
			set { m_strTextHeader = value; }
		}

		public int TextIndex
		{
			get { return m_nTextIndex; }
			set { m_nTextIndex = value; }
		}
	}
}
