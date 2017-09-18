using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.Tools
{
	public class LanguageEntry
	{
		private string m_strLanguageCode;
		private string m_strShortCode;
		private string m_strText;
		private string m_strTextHeader;
		private int m_nTextIndex;
		private string m_strMasqueradeLangCode;

		public LanguageEntry()
		{ }
		
		public LanguageEntry(string strLangCode, string strShortCode, string strText, string strTextHeader, int nTextIndex, string strMasqueradeLangCode = null)
		{
			m_strLanguageCode = strLangCode;
			m_strShortCode = strShortCode;
			m_strText = strText;
			m_strTextHeader = strTextHeader;
			m_nTextIndex = nTextIndex;
			m_strMasqueradeLangCode = strMasqueradeLangCode;
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

		public string MasqueradeAsLangCode
		{
			get { return m_strMasqueradeLangCode; }
			set { m_strMasqueradeLangCode = value; }
		}
	}
}
