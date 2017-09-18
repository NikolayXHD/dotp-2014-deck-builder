using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RSN.Tools
{
	public class MessageLog
	{
		private string m_strLogFile;
		private StreamWriter m_swLog;

		public MessageLog(string strLogFile)
		{
			m_strLogFile = strLogFile;
			m_swLog = null;
		}

		public void WriteMessage(string strMessage)
		{
			if (m_swLog == null)
			{
				// We haven't opened this log before this run so create new.
				FileStream fsLog = new FileStream(m_strLogFile, FileMode.Create, FileAccess.Write, FileShare.Read);
				m_swLog = new StreamWriter(fsLog, new UTF8Encoding(Settings.IncludeBOM));
				m_swLog.WriteLine(DateStamp() + "Log Opened.");
			}
			m_swLog.WriteLine(DateStamp() + strMessage);
			m_swLog.Flush();
		}

		public void Close()
		{
			// We don't need to do anything if we haven't opened the log.
			if (m_swLog != null)
			{
				WriteMessage("Log Closed.");
				m_swLog.Close();
			}
		}

		private string DateStamp()
		{
			return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": ";
		}
	}
}
