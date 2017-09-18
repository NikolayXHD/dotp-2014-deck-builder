using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSN.DotP
{
	public class IdScheme
	{
		private IdSchemeType m_eType;
		private int m_nMinId;
		private int m_nMaxId;
		private int m_nMinIdDigits;
		private int m_nDeckIdChange;
		private int m_nLandPoolIdChange;
		private int m_nRegularUnlockIdChange;
		private int m_nPromoUnlockIdChange;
		private bool m_bUseIdBlock;
		private int m_nIdBlock;

		public IdScheme()
		{
			// Set up default scheme values
			m_eType = IdSchemeType.PrefixScheme;
			m_nMinId = 0;
			m_nMaxId = 99;
			m_nMinIdDigits = 2;
			m_nDeckIdChange = 1000;
			m_nLandPoolIdChange = 10001;
			m_nRegularUnlockIdChange = 10002;
			m_nPromoUnlockIdChange = 10003;
			m_bUseIdBlock = true;
			m_nIdBlock = 1000;
		}

		public IdSchemeType Type
		{
			get { return m_eType; }
			set { m_eType = value; }
		}

		public int MinimumId
		{
			get { return m_nMinId; }
			set
			{
				if (value < 0)
					value = 0;
				if (value >= m_nMaxId)
					throw new Exception("Minimum must be less than maximum.");
				m_nMinId = value;
			}
		}

		public int MaximumId
		{
			get { return m_nMaxId; }
			set
			{
				if (value <= m_nMinId)
					throw new Exception("Maximum must be greater than minimum.");
				m_nMaxId = value;
			}
		}

		public int DeckIdChange
		{
			get { return m_nDeckIdChange; }
			set
			{
				if (value < 0)
					value = 0;
				m_nDeckIdChange = value;
			}
		}

		public int LandPoolIdChange
		{
			get { return m_nLandPoolIdChange; }
			set
			{
				if (value < 0)
					value = 0;
				m_nLandPoolIdChange = value;
			}
		}

		public int RegularUnlockIdChange
		{
			get { return m_nRegularUnlockIdChange; }
			set
			{
				if (value < 0)
					value = 0;
				m_nRegularUnlockIdChange = value;
			}
		}

		public int PromoUnlockIdChange
		{
			get { return m_nPromoUnlockIdChange; }
			set
			{
				if (value < 0)
					value = 0;
				m_nPromoUnlockIdChange = value;
			}
		}

		public int MinimumIdDigits
		{
			get { return m_nMinIdDigits; }
			set
			{
				if (value < 0)
					value = 0;
				// I could actually allow 10 digits, but only values of ~2.147 billion or less would be allowed so I'll limit it to 9.
				if (value > 9)
					value = 9;
				m_nMinIdDigits = value;
			}
		}

		public bool UseIdBlock
		{
			get { return m_bUseIdBlock; }
			set
			{
				m_bUseIdBlock = value;
				if (m_bUseIdBlock)
				{
					m_nDeckIdChange = m_nIdBlock;
					m_nLandPoolIdChange = (m_nIdBlock * 10) + 1;
					m_nRegularUnlockIdChange = (m_nIdBlock * 10) + 2;
					m_nPromoUnlockIdChange = (m_nIdBlock * 10) + 3;
				}
			}
		}

		public int IdBlock
		{
			get { return m_nIdBlock; }
			set
			{
				if (value < 0)
					value = 0;
				m_nIdBlock = value;
				if (m_bUseIdBlock)
				{
					m_nDeckIdChange = m_nIdBlock;
					m_nLandPoolIdChange = (m_nIdBlock * 10) + 1;
					m_nRegularUnlockIdChange = (m_nIdBlock * 10) + 2;
					m_nPromoUnlockIdChange = (m_nIdBlock * 10) + 3;
				}
			}
		}

		public int GetRandomId()
		{
			if (m_nMinId < 0)
				throw new Exception("Number Exception: Minimum must be greater than or equal to 0");
			if (m_nMaxId <= m_nMinId)
				throw new Exception("Number Exception: Maximum must be greater than minimum.");

			// Return an integer between min and max inclusive.
			return Tools.Random.Next(m_nMinId - 1, m_nMaxId) + 1;
		}

		// Will return -1 if there are no more valid ids.
		public int GetNextAvailableId(Dictionary<int, string> dicUsedIds)
		{
			int nTestId = m_nMinId;
			while ((nTestId <= MaximumId) &&
				((dicUsedIds.ContainsKey(GetDeckId(nTestId))) ||
				(dicUsedIds.ContainsKey(GetLandPoolId(nTestId))) ||
				(dicUsedIds.ContainsKey(GetRegularUnlockId(nTestId))) ||
				(dicUsedIds.ContainsKey(GetPromoUnlockId(nTestId)))))
				nTestId++;
			if (nTestId > m_nMaxId)
				return -1;

			// If we've gotten this far then we found a good usable id with this scheme.
			return nTestId;
		}

		private string GetMinDigitFormatString()
		{
			return new string('0', m_nMinIdDigits);
		}

		public int GetDeckId(int nId = -1)
		{
			if (nId == -1)
				nId = GetRandomId();
			if ((nId < m_nMinId) || (nId > m_nMaxId))
				throw new Exception("Number Exception: Id not within valid range for this scheme.");

			int nDeckId = nId;

			switch (m_eType)
			{
				case IdSchemeType.PrefixScheme:
					{
						string strId = m_nDeckIdChange.ToString() + nId.ToString(GetMinDigitFormatString());
						try
						{
							nDeckId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.SuffixScheme:
					{
						string strId = nId.ToString() + m_nDeckIdChange.ToString();
						try
						{
							nDeckId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.AdditiveScheme:
					nDeckId = nId + m_nDeckIdChange;
					break;
			}

			return nDeckId;
		}

		public int GetLandPoolId(int nDeckId)
		{
			if ((nDeckId < m_nMinId) || (nDeckId > m_nMaxId))
				throw new Exception("Number Exception: Id not within valid range for this scheme.");

			int nLandPoolId = nDeckId;

			switch (m_eType)
			{
				case IdSchemeType.PrefixScheme:
					{
						string strId = m_nLandPoolIdChange.ToString() + nDeckId.ToString(GetMinDigitFormatString());
						try
						{
							nLandPoolId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.SuffixScheme:
					{
						string strId = nDeckId.ToString() + m_nLandPoolIdChange.ToString();
						try
						{
							nLandPoolId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.AdditiveScheme:
					nLandPoolId = nDeckId + m_nLandPoolIdChange;
					break;
			}

			return nLandPoolId;
		}

		public int GetUnlockId(int nDeckId, bool bPromo)
		{
			if (bPromo)
				return GetPromoUnlockId(nDeckId);
			else
				return GetRegularUnlockId(nDeckId);
		}

		public int GetRegularUnlockId(int nDeckId)
		{
			if ((nDeckId < m_nMinId) || (nDeckId > m_nMaxId))
				throw new Exception("Number Exception: Id not within valid range for this scheme.");

			int nUnlockId = nDeckId;

			switch (m_eType)
			{
				case IdSchemeType.PrefixScheme:
					{
						string strId = m_nRegularUnlockIdChange.ToString() + nDeckId.ToString(GetMinDigitFormatString());
						try
						{
							nUnlockId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.SuffixScheme:
					{
						string strId = nDeckId.ToString() + m_nRegularUnlockIdChange.ToString();
						try
						{
							nUnlockId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.AdditiveScheme:
					nUnlockId = nDeckId + m_nRegularUnlockIdChange;
					break;
			}

			return nUnlockId;
		}

		public int GetPromoUnlockId(int nDeckId)
		{
			if ((nDeckId < m_nMinId) || (nDeckId > m_nMaxId))
				throw new Exception("Number Exception: Id not within valid range for this scheme.");

			int nUnlockId = nDeckId;

			switch (m_eType)
			{
				case IdSchemeType.PrefixScheme:
					{
						string strId = m_nPromoUnlockIdChange.ToString() + nDeckId.ToString(GetMinDigitFormatString());
						try
						{
							nUnlockId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.SuffixScheme:
					{
						string strId = nDeckId.ToString() + m_nPromoUnlockIdChange.ToString();
						try
						{
							nUnlockId = Int32.Parse(strId);
						}
						catch (Exception)
						{
							throw new Exception("Number Exception: Final Id larger than 32-bit signed integer max.  Check your scheme settings.");
						}
					}
					break;

				case IdSchemeType.AdditiveScheme:
					nUnlockId = nDeckId + m_nPromoUnlockIdChange;
					break;
			}

			return nUnlockId;
		}
	}
}
