using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RSN.Tools;

namespace RSN.DotP
{
	public static partial class StreamHelpers
	{
		public static string ReadEncodedString(this Stream stream,
														Encoding encoding,
														long size,
														bool trailingNull = false)
		{
			byte[] bom = encoding.GetPreamble();
			bool bBomPresent = true;
			byte[] data = new byte[size];
			stream.Read(data, 0, data.Length);

			for (int i = 0; i < bom.Length; i++)
				if (bom[i] != data[i])
				{
					bBomPresent = false;
					break;
				}

			string value = string.Empty;
			if (bBomPresent)
				value = encoding.GetString(data, bom.Length, data.Length - bom.Length);
			else
				value = encoding.GetString(data, 0, data.Length);

			if (trailingNull == true)
			{
				int position = value.IndexOf('\0');
				if (position >= 0)
				{
					value = value.Substring(0, position);
				}
			}

			return value;
		}
		public static string ReadEncodedString(this Stream stream,
														long size,
														bool trailingNull = false)
		{
			string strValue = string.Empty;

			// Check for Unicode Encoding
			if (stream.Length >= 2)
			{
				byte[] bom = Encoding.Unicode.GetPreamble();
				bool bUnicodeBomPresent = true;
				byte[] data = new byte[2];
				stream.Read(data, 0, 2);
				stream.Seek(-2, SeekOrigin.Current);

				for (int i = 0; i < bom.Length; i++)
					if (bom[i] != data[i])
					{
						bUnicodeBomPresent = false;
						break;
					}

				if (bUnicodeBomPresent)
					strValue = ReadEncodedString(stream, Encoding.Unicode, size, trailingNull);
				else
					strValue = ReadEncodedString(stream, Encoding.UTF8, size, trailingNull);
			}

			return strValue;
		}
	}

	public class Tools
	{
		private const int MOF_MAX_DISTANCE = 50;
		private static Size MOF_FALLBACK_SIZE = new Size(512, 512);

		private static bool m_bAttemptedLoad;
		private static Bitmap m_bmpCardBorder;
		private static Random m_rRand = new Random();

		public static string ColourText(ColourFlags eColour)
		{
			if ((eColour & ColourFlags.MultiColour) == ColourFlags.MultiColour)
			{
				string strColours = string.Empty;
				if ((eColour & ColourFlags.Black) == ColourFlags.Black)
					strColours += Settings.UIStrings["MANA_" + ColourFlags.Black.ToString().ToUpper()];
				if ((eColour & ColourFlags.Blue) == ColourFlags.Blue)
				{
					if (strColours.Length > 0)
						strColours += ", ";
					strColours += Settings.UIStrings["MANA_" + ColourFlags.Blue.ToString().ToUpper()];
				}
				if ((eColour & ColourFlags.Green) == ColourFlags.Green)
				{
					if (strColours.Length > 0)
						strColours += ", ";
					strColours += Settings.UIStrings["MANA_" + ColourFlags.Green.ToString().ToUpper()];
				}
				if ((eColour & ColourFlags.Red) == ColourFlags.Red)
				{
					if (strColours.Length > 0)
						strColours += ", ";
					strColours += Settings.UIStrings["MANA_" + ColourFlags.Red.ToString().ToUpper()];
				}
				if ((eColour & ColourFlags.White) == ColourFlags.White)
				{
					if (strColours.Length > 0)
						strColours += ", ";
					strColours += Settings.UIStrings["MANA_" + ColourFlags.White.ToString().ToUpper()];
				}

				return strColours;
			}
			else if (eColour == 0)
				return Settings.UIStrings["MANA_NOTDEFINED"];
			else
				return Settings.UIStrings["MANA_" + eColour.ToString().ToUpper()];
		}

		public static string TypeText(CardType eType)
		{
			string strReturn = string.Empty;

			if ((eType & CardType.Tribal) == CardType.Tribal)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Tribal;
			}
			if ((eType & CardType.Artifact) == CardType.Artifact)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Artifact;
			}
			if ((eType & CardType.Creature) == CardType.Creature)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Creature;
			}
			if ((eType & CardType.Enchantment) == CardType.Enchantment)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Enchantment;
			}
			if ((eType & CardType.Instant) == CardType.Instant)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Instant;
			}
			if ((eType & CardType.Land) == CardType.Land)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Land;
			}
			if ((eType & CardType.Phenomenon) == CardType.Phenomenon)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Phenomenon;
			}
			if ((eType & CardType.Plane) == CardType.Plane)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Plane;
			}
			if ((eType & CardType.Planeswalker) == CardType.Planeswalker)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Planeswalker;
			}
			if ((eType & CardType.Scheme) == CardType.Scheme)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Scheme;
			}
			if ((eType & CardType.Sorcery) == CardType.Sorcery)
			{
				if (strReturn.Length > 0)
					strReturn += " ";
				strReturn += CardType.Sorcery;
			}

			return strReturn;
		}

		public static string ListText(IList<string> set, string strDelim = " ")
		{
			string strReturn = string.Empty;

			foreach (string str in set)
			{
				if (strReturn.Length > 0)
					strReturn += strDelim;
				strReturn += str;
			}

			return strReturn;
		}

		public static int CastingCostToCMC(string strCost)
		{
			int nCMC = 0;

			if (strCost != null)
			{
				// First things first, we need to get groups of items used in the casting cost.
				MatchCollection mcGroups = Regex.Matches(strCost, "{([^}]+)}");

				foreach (Match m in mcGroups)
				{
					if (m.Value.Length > 2)
					{
						// Extract from curly brackets.
						string strItem = m.Value.Substring(1, m.Value.Length - 2);

						// Now we need to check for dual value costs (items with "/" in them, B/W, R/P, 2/U, etc...)
						if (strItem.IndexOf('/') > -1)
						{
							// We have dual values so to convert to CMC if it also has a 2 in it then it is worth 2 otherwise 1.
							if (strItem.IndexOf('2') > -1)
								nCMC += 2;
							else
								nCMC++;
						}
						// No longer dealing with dual costs so we check to see if it is a standard colour
						else if ((strItem.IndexOf('B') > -1) ||
							(strItem.IndexOf('U') > -1) ||
							(strItem.IndexOf('G') > -1) ||
							(strItem.IndexOf('R') > -1) ||
							(strItem.IndexOf('W') > -1))
							nCMC++;
						// Check for X costs (they count as 0 towards CMC)
						else if (strItem.IndexOf('X') > -1)
						{ }
						// We've gotten this far so the only thing left is numeric costs.
						else
							nCMC += Int32.Parse(strItem);
					}
				}
			}

			return nCMC;
		}

		public static List<string> CostToTokenList(string strCost)
		{
			List<string> lstTokens = new List<string>();

			if (strCost != null)
			{
				// First things first, we need to get groups of items used in the casting cost.
				MatchCollection mcGroups = Regex.Matches(strCost, "{([^}]+)}");

				foreach (Match m in mcGroups)
				{
					if (m.Value.Length > 2)
						lstTokens.Add(m.Value);
				}
			}

			return lstTokens;
		}

		public static string CostTokenToImageName(string strToken)
		{
			string strImg = string.Empty;

			switch (strToken)
			{
				case "{0}": strImg = "MANA_0"; break;
				case "{1}": strImg = "MANA_1"; break;
				case "{2}": strImg = "MANA_2"; break;
				case "{3}": strImg = "MANA_3"; break;
				case "{4}": strImg = "MANA_4"; break;
				case "{5}": strImg = "MANA_5"; break;
				case "{6}": strImg = "MANA_6"; break;
				case "{7}": strImg = "MANA_7"; break;
				case "{8}": strImg = "MANA_8"; break;
				case "{9}": strImg = "MANA_9"; break;
				case "{10}": strImg = "MANA_10"; break;
				case "{11}": strImg = "MANA_11"; break;
				case "{12}": strImg = "MANA_12"; break;
				case "{13}": strImg = "MANA_13"; break;
				case "{14}": strImg = "MANA_14"; break;
				case "{15}": strImg = "MANA_15"; break;
				case "{16}": strImg = "MANA_16"; break;
				case "{X}": strImg = "MANA_X"; break;
				case "{B}": strImg = "MANA_B"; break;
				case "{U}": strImg = "MANA_U"; break;
				case "{G}": strImg = "MANA_G"; break;
				case "{R}": strImg = "MANA_R"; break;
				case "{W}": strImg = "MANA_W"; break;
				case "{B/G}": strImg = "MANA_BG"; break;
				case "{B/R}": strImg = "MANA_BR"; break;
				case "{B/U}": strImg = "MANA_UB"; break;
				case "{B/W}": strImg = "MANA_WB"; break;
				case "{G/B}": strImg = "MANA_BG"; break;
				case "{G/R}": strImg = "MANA_RG"; break;
				case "{G/U}": strImg = "MANA_GU"; break;
				case "{G/W}": strImg = "MANA_GW"; break;
				case "{R/B}": strImg = "MANA_BR"; break;
				case "{R/G}": strImg = "MANA_RG"; break;
				case "{R/U}": strImg = "MANA_UR"; break;
				case "{R/W}": strImg = "MANA_RW"; break;
				case "{U/B}": strImg = "MANA_UB"; break;
				case "{U/G}": strImg = "MANA_GU"; break;
				case "{U/R}": strImg = "MANA_UR"; break;
				case "{U/W}": strImg = "MANA_WU"; break;
				case "{W/B}": strImg = "MANA_WB"; break;
				case "{W/G}": strImg = "MANA_GW"; break;
				case "{W/R}": strImg = "MANA_RW"; break;
				case "{W/U}": strImg = "MANA_WU"; break;
				case "{B/P}": strImg = "PHYREXIAN_BLACK_MANA"; break;
				case "{U/P}": strImg = "PHYREXIAN_BLUE_MANA"; break;
				case "{G/P}": strImg = "PHYREXIAN_GREEN_MANA"; break;
				case "{R/P}": strImg = "PHYREXIAN_RED_MANA"; break;
				case "{W/P}": strImg = "PHYREXIAN_WHITE_MANA"; break;
			}

			return strImg;
		}

		public static string TextTokenToImageName(string strToken)
		{
			string strImg = string.Empty;

			switch (strToken)
			{
				case "{0}": strImg = "MANA_0"; break;
				case "{1}": strImg = "MANA_1"; break;
				case "{2}": strImg = "MANA_2"; break;
				case "{3}": strImg = "MANA_3"; break;
				case "{4}": strImg = "MANA_4"; break;
				case "{5}": strImg = "MANA_5"; break;
				case "{6}": strImg = "MANA_6"; break;
				case "{7}": strImg = "MANA_7"; break;
				case "{8}": strImg = "MANA_8"; break;
				case "{9}": strImg = "MANA_9"; break;
				case "{10}": strImg = "MANA_10"; break;
				case "{11}": strImg = "MANA_11"; break;
				case "{12}": strImg = "MANA_12"; break;
				case "{13}": strImg = "MANA_13"; break;
				case "{14}": strImg = "MANA_14"; break;
				case "{15}": strImg = "MANA_15"; break;
				case "{16}": strImg = "MANA_16"; break;
				case "{X}": strImg = "MANA_X"; break;
				case "{B}": strImg = "MANA_B"; break;
				case "{U}": strImg = "MANA_U"; break;
				case "{G}": strImg = "MANA_G"; break;
				case "{R}": strImg = "MANA_R"; break;
				case "{W}": strImg = "MANA_W"; break;
				case "{T}": strImg = "MANA_T"; break;
				case "{q}": strImg = "MANA_Q"; break;
				case "{a}": strImg = "MANA_BG"; break;
				case "{b}": strImg = "MANA_BR"; break;
				case "{c}": strImg = "MANA_GU"; break;
				case "{d}": strImg = "MANA_GW"; break;
				case "{e}": strImg = "MANA_RG"; break;
				case "{f}": strImg = "MANA_RW"; break;
				case "{g}": strImg = "MANA_UB"; break;
				case "{h}": strImg = "MANA_UR"; break;
				case "{i}": strImg = "MANA_WB"; break;
				case "{j}": strImg = "MANA_WU"; break;
				case "{J}": strImg = "PHYREXIAN_BLACK_MANA"; break;
				case "{K}": strImg = "PHYREXIAN_BLUE_MANA"; break;
				case "{L}": strImg = "PHYREXIAN_GREEN_MANA"; break;
				case "{I}": strImg = "PHYREXIAN_RED_MANA"; break;
				case "{O}": strImg = "PHYREXIAN_WHITE_MANA"; break;
				case "{Q}": strImg = "CHAOS_UNLOCK_SYMBOL"; break;
				// Planeswalk image (for planes)
				//case "{P}": strImg = ""; break;
			}

			return strImg;
		}

		public static string CardRarityToImageName(CardRarity eRarity)
		{
			string strImage = null;

			switch (eRarity)
			{
				case CardRarity.Common: strImage = "EXPANSION_COMMON"; break;
				case CardRarity.Uncommon: strImage = "EXPANSION_UNCOMMON"; break;
				case CardRarity.Rare: strImage = "EXPANSION_RARE"; break;
				case CardRarity.Mythic: strImage = "EXPANSION_MYTHIC"; break;
			}

			return strImage;
		}

		public static string CodifyName(string strName)
		{
			string strReturn = string.Empty;
			// Make it uppercase and remove any padding at the beginning or end.
			strName = strName.Trim().ToUpper();
			// Change spaces to underscores.
			strName = strName.Replace(' ', '_');
			// Change certain special characters to similar English characters.
			strName = strName.Replace("Æ","AE");
			// Move English characters over to a new string removing all others.
			//	This is easier than trying to check for every other character.
			foreach (char c in strName)
			{
				if (((c >= 'a') && (c <= 'z')) ||
					((c >= 'A') && (c <= 'Z')) ||
					((c >= '0') && (c <= '9')) ||
					(c == '_'))
				{
					strReturn += c;
				}
			}

			return strReturn;
		}

		// This is a specialized function for how I mask images in the context of this application.
		//	This has an unfortunate side effect of making any pixel in the source image transparent if it is the color of the mask.
		//	I use color 255, 0, 255 (a bright purple/hot pink color) as I rarely see that color in most images.
		//	Another unfortunate side effect is that I require the top left most pixel to be transparent and the mask does not support variable transparency.
		//TODO: Replace this with a high speed variable transparency masking function (if possible).
		public static Bitmap MaskImage(Bitmap bmpSource, Bitmap bmpMask, Rectangle rcSourceDestination)
		{
			// I use the mask dimensions as those are what I designed the mask for.
			Bitmap bmpMasked = new Bitmap(bmpMask.Width, bmpMask.Height);
			Graphics grfx = Graphics.FromImage(bmpMasked);
			// Since we may not be drawing at 100% size we want to use a high quality interpolation so that the source image still looks good.
			grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			// Draw the source image onto the new bitmap.
			grfx.DrawImage(bmpSource, rcSourceDestination);
			// Draw the mask on top of it.
			grfx.DrawImage(bmpMask, new Rectangle(0, 0, bmpMask.Width, bmpMask.Height));
			// Make the mask color transparent.
			bmpMasked.MakeTransparent(bmpMask.GetPixel(0, 0));

			return bmpMasked;
		}

		public static Bitmap AdjustImage(Bitmap bmpSource, Size szSize, Rectangle rcSourceDestination)
		{
			// I use the mask dimensions as those are what I designed the mask for.
			Bitmap bmpAdjusted = new Bitmap(szSize.Width, szSize.Height);
			Graphics grfx = Graphics.FromImage(bmpAdjusted);
			// Since we may not be drawing at 100% size we want to use a high quality interpolation so that the source image still looks good.
			grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			// Draw the source image onto the new bitmap.
			grfx.DrawImage(bmpSource, rcSourceDestination);

			return bmpAdjusted;
		}

		public static Bitmap GrayscaleImage(Bitmap bmpImage)
		{
			// Basic setup here.
			Bitmap bmpGrayscale = new Bitmap(bmpImage.Width, bmpImage.Height);
			Graphics grfx = Graphics.FromImage(bmpGrayscale);
			// This color matrix does a matrix multiply on a RGBAW vector to get a grayscale vector of the input.
			//	I'm not going to go into matrix math because I remember just enough to get by.
			ColorMatrix cmGrayscaleAdjust = new ColorMatrix(
				new float[][]
				{
					new float[] {0.33f, 0.33f, 0.33f, 0, 0},
					new float[] {0.59f, 0.59f, 0.59f, 0, 0},
					new float[] {0.11f, 0.11f, 0.11f, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0, 0, 0, 0, 1},
				});
			// We need an ImageAttributes object so that we can use the ColorMatrix that we created.
			ImageAttributes iaAdjust = new ImageAttributes();
			iaAdjust.SetColorMatrix(cmGrayscaleAdjust);

			// Now we re-draw the image in grayscale.
			grfx.DrawImage(bmpImage, new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), 0, 0, bmpImage.Width, bmpImage.Height, GraphicsUnit.Pixel, iaAdjust);

			// And return it.
			return bmpGrayscale;
		}

		public static Bitmap BluescaleImage(Bitmap bmpImage)
		{
			// Basic setup here.
			Bitmap bmpBluescale = new Bitmap(bmpImage.Width, bmpImage.Height);
			Graphics grfx = Graphics.FromImage(bmpBluescale);
			// This color matrix does a matrix multiply on a RGBAW vector to get a grayscale vector of the input.
			//	I'm not going to go into matrix math because I remember just enough to get by.
			ColorMatrix cmGrayscaleAdjust = new ColorMatrix(
				new float[][]
				{
					new float[] {0.1f, 0.3f, 0.3f, 0, 0},
					new float[] {0.59f, 0.39f, 0.59f, 0, 0},
					new float[] {0.11f, 0.11f, 0.31f, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0, 0, 0, 0, 1},
				});
			// We need an ImageAttributes object so that we can use the ColorMatrix that we created.
			ImageAttributes iaAdjust = new ImageAttributes();
			iaAdjust.SetColorMatrix(cmGrayscaleAdjust);

			// Now we re-draw the image in grayscale.
			grfx.DrawImage(bmpImage, new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), 0, 0, bmpImage.Width, bmpImage.Height, GraphicsUnit.Pixel, iaAdjust);

			// And return it.
			return bmpBluescale;
		}

		public static Bitmap ApplyAlphaToImage(Bitmap bmpImage, Bitmap bmpAlpha)
		{
			Bitmap bmpApplied = new Bitmap(bmpAlpha.Width, bmpAlpha.Height, PixelFormat.Format32bppArgb);
			Rectangle rcArea = new Rectangle(0, 0, bmpAlpha.Width, bmpAlpha.Height);
			BitmapData bdImage = bmpImage.LockBits(rcArea, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			BitmapData bdAlpha = bmpAlpha.LockBits(rcArea, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			BitmapData bdOutput = bmpApplied.LockBits(rcArea, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			unsafe
			{
				for (int y = 0; y < bmpImage.Height; y++)
				{
					byte* ptrImage = (byte*)bdImage.Scan0 + (y * bdImage.Stride);
					byte* ptrAlpha = (byte*)bdAlpha.Scan0 + (y * bdAlpha.Stride);
					byte* ptrOutput = (byte*)bdOutput.Scan0 + (y * bdOutput.Stride);
					for (int x = 0; x < bmpImage.Width; x++)
					{
						ptrOutput[(4 * x)] = ptrImage[(4 * x)];				// blue
						ptrOutput[(4 * x) + 1] = ptrImage[(4 * x) + 1];		// green
						ptrOutput[(4 * x) + 2] = ptrImage[(4 * x) + 2];		// red
						ptrOutput[(4 * x) + 3] = ptrAlpha[(4 * x) + 3];		// alpha
					}
				}
			}
			bmpImage.UnlockBits(bdImage);
			bmpAlpha.UnlockBits(bdAlpha);
			bmpApplied.UnlockBits(bdOutput);
			return bmpApplied;
		}

		public static Size FindClosestMultipleOf4Size(Size szInput)
		{
			double dRatio = ((double)szInput.Height) / ((double)szInput.Width);
			int nDistance = 0;

			// Try going to the closest larger Multiple of Four and see if that works
			int nWidthMof = szInput.Width + (4 - (szInput.Width % 4));
			Size szReturn = new Size(nWidthMof, (int)(nWidthMof * dRatio));

			// While we have a non-multiple of 4 size keep looking for one.
			while ((((szReturn.Width % 4) != 0) || ((szReturn.Height % 4) != 0)) && (nDistance < MOF_MAX_DISTANCE))
			{
				nDistance++;
				// Try enlarging by our distance to see if we can get a Mof size.
				szReturn.Width = nWidthMof + (4 * nDistance);
				szReturn.Height = (int)(szReturn.Width * dRatio);
				if (((szReturn.Width % 4) == 0) && ((szReturn.Height % 4) == 0))
					break;
				// No luck enlarging so lets try reducing.
				szReturn.Width = nWidthMof - (4 * nDistance);
				szReturn.Height = (int)(szReturn.Width * dRatio);
				// loop will do our checking for us so no need for an extra if.
			}

			if (((szReturn.Width % 4) != 0) || ((szReturn.Height % 4) != 0))
			{
				// We must have hit our maximum distance without finding a Mof match now we need to go to our fallback.
				szReturn = MOF_FALLBACK_SIZE;
			}

			return szReturn;
		}

		public static Bitmap AddCardBorder(Bitmap bmpPreview)
		{
			if ((m_bmpCardBorder == null) && (!m_bAttemptedLoad))
			{
				if (File.Exists(Settings.GetProgramDir() + "Images\\D14_CardBorder.png"))
				{
					// Attempt to load the card border (we will store it statically so we don't have to keep reloading it)
					try
					{
						m_bmpCardBorder = new Bitmap(Settings.GetProgramDir() + "Images\\D14_CardBorder.png");
					}
					catch (Exception e)
					{
						Settings.ReportError(e, ErrorPriority.Medium, "Unable to load card border due to error, exported preview images will be borderless.");
					}
				}
				else
					Settings.ReportError(null, ErrorPriority.Low, "Unable to load card border, so exported preview images will be borderless.");

				// Prevent us from trying again and again and filling the log with errors.
				m_bAttemptedLoad = true;
			}

			if (m_bmpCardBorder != null)
			{
				Bitmap bmpWithBorder = new Bitmap(m_bmpCardBorder);
				Graphics grfx = Graphics.FromImage(bmpWithBorder);
				grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				// Now we want 6 pixels on either side of the image to be the border so we will draw our preview image inside that rect.
				grfx.DrawImage(bmpPreview, 20, 23, 324, 466);

				return bmpWithBorder;
			}
			else
			{
				// We were unable to load the card border image so
				//	we can't add it so return what we were given.
				return bmpPreview;
			}
		}

		public static void ResizeRelatedControl(Control ctrlToResize, Control ctrlRelated)
		{
			int nOrigX = ctrlToResize.Location.X;
			ctrlToResize.Location = new Point(ctrlRelated.Location.X + ctrlRelated.Size.Width + 6, ctrlToResize.Location.Y);
			ctrlToResize.Size = new Size(ctrlToResize.Size.Width - (ctrlToResize.Location.X - nOrigX), ctrlToResize.Size.Height);
		}

		// Will return an empty string if cancelled.
		public static string SaveDialog(string strTitle, string strFilter, string strExtension, string strDefFilename = "")
		{
			string strReturn = string.Empty;

			SaveFileDialog sfdSave = new SaveFileDialog();
			sfdSave.AddExtension = true;
			sfdSave.AutoUpgradeEnabled = true;
			sfdSave.CheckFileExists = false;
			sfdSave.CheckPathExists = true;
			sfdSave.DefaultExt = strExtension;
			sfdSave.DereferenceLinks = true;
			sfdSave.FileName = strDefFilename;
			sfdSave.Filter = strFilter;
			sfdSave.FilterIndex = 1;
			sfdSave.OverwritePrompt = true;
			sfdSave.Title = strTitle;
			DialogResult drResult = sfdSave.ShowDialog();
			if (drResult == DialogResult.OK)
				strReturn = sfdSave.FileName;

			return strReturn;
		}

		public static Random Random
		{
			get { return m_rRand; }
		}

		public static void ChangeControlEnabled(Control ctrlControl, bool bEnabled)
		{
			ctrlControl.Enabled = bEnabled;
			if ((ctrlControl.Controls != null) && (ctrlControl.Controls.Count > 0))
			{
				foreach (Control ctrl in ctrlControl.Controls)
					ChangeControlEnabled(ctrl, bEnabled);
			}
		}

		public static Control GetFocusedControl(Control ctrl)
		{
			Control ctrlReturn = null;

			if (ctrl.Focused)
				ctrlReturn = ctrl;
			else if ((ctrl.Controls != null) && (ctrl.Controls.Count > 0))
			{
				foreach (Control ctrlSub in ctrl.Controls)
				{
					ctrlReturn = GetFocusedControl(ctrlSub);
					if (ctrlReturn != null)
						break;
				}
			}

			return ctrlReturn;
		}
	}
}
