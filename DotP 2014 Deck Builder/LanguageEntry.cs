using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.Tools
{
	public class LanguageEntry
	{
		public string LanguageCode;
		public string ShortCode;
		public string Text;
		public string TextHeader;
		public int TextIndex;

		public LanguageEntry()
		{ }
		
		public LanguageEntry(string strLangCode, string strShortCode, string strText, string strTextHeader, int nTextIndex)
		{
			LanguageCode = strLangCode;
			ShortCode = strShortCode;
			Text = strText;
			TextHeader = strTextHeader;
			TextIndex = nTextIndex;
		}
	}
}
