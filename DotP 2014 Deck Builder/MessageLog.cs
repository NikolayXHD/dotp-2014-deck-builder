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
		private FileStream m_fsLog;
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
				m_fsLog = new FileStream(m_strLogFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
				m_swLog = new StreamWriter(m_fsLog, new UTF8Encoding(Settings.IncludeBOM));
				m_swLog.WriteLine(DateStamp() + "Log Opened.\r\n");
			}
			m_fsLog.Seek(0, SeekOrigin.End);
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

		public bool Opened
		{
			get { return m_swLog != null; }
		}

		// If the log has been opened then read the entire log to date and return it else return null.
		public string ReadToCurrent()
		{
			string strRead = null;

			if (Opened)
			{
				StreamReader srLog = new StreamReader(m_fsLog, new UTF8Encoding(Settings.IncludeBOM));
				m_fsLog.Seek(0, SeekOrigin.Begin);
				strRead = srLog.ReadToEnd();
			}

			return strRead;
		}

		private string DateStamp()
		{
			return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": ";
		}
	}
}
