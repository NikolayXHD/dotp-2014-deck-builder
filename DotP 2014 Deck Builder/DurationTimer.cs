using System;

namespace RSN.Tools
{
	// This is intended to be used to time sections of code, mainly so that I can find spots that need to be improved if possible.
	class DurationTimer
	{
		private DateTime m_dtDummy;
		private DateTime m_dtStart;
		private DateTime m_dtEnd;

		public DurationTimer()
		{
			m_dtDummy = new DateTime(1, 1, 1, 0, 0, 0);
			m_dtEnd = m_dtDummy;
			m_dtStart = DateTime.UtcNow;
		}

		public void End()
		{
			m_dtEnd = DateTime.UtcNow;
		}

		public long Ticks()
		{
			if (m_dtEnd != m_dtDummy)
				return (m_dtEnd - m_dtStart).Ticks;
			else
				return (DateTime.UtcNow - m_dtStart).Ticks;
		}

		public double Milliseconds()
		{
			if (m_dtEnd != m_dtDummy)
				return (m_dtEnd - m_dtStart).TotalMilliseconds;
			else
				return (DateTime.UtcNow - m_dtStart).TotalMilliseconds;
		}
	}
}
