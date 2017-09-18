using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using RSN.Tools;

namespace RSN.DotP
{
	public class CardSubType
	{
		public string SubType;
		public int OrderEnglish;		// order_en-US
		public int OrderFrench;			// order_fr-FR
		public int OrderSpanish;		// order_es-ES
		public int OrderGerman;			// order_de-DE
		public int OrderItalian;		// order_it-IT
		public int OrderJapanese;		// order_jp-JA
		public int OrderKorean;			// order_ko-KR
		public int OrderRussian;		// order_ru-RU
		public int OrderPortuguese;		// order_pt-BR
		public int OrderChinese;

		public CardSubType() { }

		public CardSubType(XmlNode xnSubType, int nOrderFound, int nMax)
		{
			SubType = XmlTools.GetValueFromAttribute(xnSubType, "metaname").ToUpper();

			string strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_en-US");
			if (strValue.Length > 0)
				OrderEnglish = Int32.Parse(strValue);
			else
				OrderEnglish = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_fr-FR");
			if (strValue.Length > 0)
				OrderFrench = Int32.Parse(strValue);
			else
				OrderFrench = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_es-ES");
			if (strValue.Length > 0)
				OrderSpanish = Int32.Parse(strValue);
			else
				OrderSpanish = nMax - nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_de-DE");
			if (strValue.Length > 0)
				OrderGerman = Int32.Parse(strValue);
			else
				OrderGerman = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_it-IT");
			if (strValue.Length > 0)
				OrderItalian = Int32.Parse(strValue);
			else
				OrderItalian = nMax - nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_jp-JA");
			if (strValue.Length > 0)
				OrderJapanese = Int32.Parse(strValue);
			else
				OrderJapanese = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_ko-KR");
			if (strValue.Length > 0)
				OrderKorean = Int32.Parse(strValue);
			else
				OrderKorean = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_ru-RU");
			if (strValue.Length > 0)
				OrderRussian = Int32.Parse(strValue);
			else
				OrderRussian = nOrderFound;

			strValue = XmlTools.GetValueFromAttribute(xnSubType, "order_pt-BR");
			if (strValue.Length > 0)
				OrderPortuguese = Int32.Parse(strValue);
			else
				OrderPortuguese = nOrderFound;

			OrderChinese = nOrderFound;
		}

		public int OrderForLangCode(string strLangCode)
		{
			// Order by default uses the English ordering.
			int nOrder = OrderEnglish;

			if (strLangCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderFrench;
			else if (strLangCode.Equals("es-ES", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderSpanish;
			else if (strLangCode.Equals("de-DE", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderGerman;
			else if (strLangCode.Equals("it-IT", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderItalian;
			else if (strLangCode.Equals("jp-JA", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderJapanese;
			else if (strLangCode.Equals("ko-KR", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderKorean;
			else if (strLangCode.Equals("ru-RU", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderRussian;
			else if (strLangCode.Equals("pt-BR", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderPortuguese;
			else if (strLangCode.Equals("zh-CN", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderChinese;
			else if (strLangCode.Equals("zh-HK", StringComparison.OrdinalIgnoreCase))
				nOrder = OrderChinese;

			return nOrder;
		}

		public int LocalizedOrder()
		{
			if (Settings.Language.MasqueradeAsLangCode != null)
				return OrderForLangCode(Settings.Language.MasqueradeAsLangCode);
			else
				return OrderForLangCode(Settings.Language.LanguageCode);
		}
	}
}
