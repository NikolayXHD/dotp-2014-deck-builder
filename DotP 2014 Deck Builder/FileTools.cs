using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RSN.DotP
{
	public class FileTools
	{
		public static string ReadFileString(string strPath)
		{
			// Use StreamReader to consume the entire text file.
			using (StreamReader srReader = new StreamReader(strPath))
			{
				return srReader.ReadToEnd();
			}
		}

		public static MemoryStream ReadFileToMemoryStream(string strPath)
		{
			MemoryStream msFile = null;
			using (FileStream fsFile = new FileStream(strPath, FileMode.Open, FileAccess.Read))
			{
				msFile = new MemoryStream();
				msFile.SetLength(fsFile.Length);
				fsFile.Read(msFile.GetBuffer(), 0, (int)fsFile.Length);
			}
			return msFile;
		}

		// WARNING: Very time consuming try to avoid using this.
		// Search all fixed disks for a directories containing a specified file.
		//	Returns a list of directories without trailing slashes.
		public static List<string> FindLocalFileDirectory(string strFilename)
		{
			List<string> lstLocation = new List<string>();
			DateTime dtStart = DateTime.Now;

			DriveInfo[] adiDrives = DriveInfo.GetDrives();
			foreach (DriveInfo diDrive in adiDrives)
			{
				// We only want to check fixed drives while trying to find the right place.
				if (diDrive.DriveType == DriveType.Fixed)
				{
					try
					{
						if (File.Exists(diDrive.RootDirectory.FullName + strFilename))
							lstLocation.Add(diDrive.RootDirectory.FullName.Substring(0, diDrive.RootDirectory.FullName.Length - 1));

						IEnumerable<string> ieDirs = Directory.EnumerateDirectories(diDrive.RootDirectory.FullName);
						foreach (string strDir in ieDirs)
							LookInDirectoryForFile(lstLocation, strDir, strFilename);
					}
					catch (Exception)
					{
						// More than likely permission was denied to access the directory.
					}
				}
			}
			DateTime dtEnd = DateTime.Now;

			return lstLocation;
		}

		private static void LookInDirectoryForFile(List<string> lstFound, string strDirectory, string strFilename)
		{
			try
			{
				if (File.Exists(strDirectory + "\\" + strFilename))
					lstFound.Add(strDirectory);

				IEnumerable<string> ieDirs = Directory.EnumerateDirectories(strDirectory);
				foreach (string strDir in ieDirs)
					LookInDirectoryForFile(lstFound, strDir, strFilename);
			}
			catch (Exception)
			{
				// More than likely permission was denied to access the directory.
			}
		}
	}
}
