using Gibbed.Duels.FileFormats;
using Tdx = Gibbed.Duels.FileFormats.Tdx;
using Gibbed.Squish;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RSN.DotP
{
	public class TdxWrapper
	{
		private string m_strName;
		private TdxFile m_tdxImage;
		private Bitmap m_bmpImage;

		public void LoadTdx(string strFile)
		{
			if (File.Exists(strFile))
			{
				m_strName = Path.GetFileNameWithoutExtension(strFile);
				Stream input = File.OpenRead(strFile);
				m_tdxImage = new TdxFile();
				m_tdxImage.Deserialize(input);
				input.Close();
				m_bmpImage = ConvertTdxToBitmap();
			}
		}

		// Caller is responsible for closing the stream.
		public void LoadTdx(Stream strmInput)
		{
			m_tdxImage = new TdxFile();
			m_tdxImage.Deserialize(strmInput);
			m_bmpImage = ConvertTdxToBitmap();
		}

		public void LoadImage(string strFile, Tdx.D3DFormat eFormat = Tdx.D3DFormat.A8R8G8B8, bool bGenerateMipMaps = true)
		{
			if (File.Exists(strFile))
			{
				m_strName = Path.GetFileNameWithoutExtension(strFile);
				m_bmpImage = new Bitmap(strFile);
				m_tdxImage = ConvertBitmapToTdx(eFormat, bGenerateMipMaps);
			}
		}

		public void LoadImage(Image imgImage, Tdx.D3DFormat eFormat = Tdx.D3DFormat.A8R8G8B8, bool bGenerateMipMaps = true)
		{
			m_bmpImage = new Bitmap(imgImage);
			m_tdxImage = ConvertBitmapToTdx(eFormat, bGenerateMipMaps);
		}

		public string Name
		{
			get { return m_strName; }
		}

		public TdxFile TdxImage
		{
			get { return m_tdxImage; }
		}

		public Bitmap Image
		{
			get { return m_bmpImage; }
		}

		public void Save(string strFile)
		{
			FileStream fsOutput = File.Create(strFile);
			if (fsOutput != null)
			{
				m_tdxImage.Serialize(fsOutput);
				fsOutput.Close();
			}
		}

		private Bitmap ConvertTdxToBitmap()
		{
            TdxFile tdx = m_tdxImage;
            Bitmap bitmap;

            if (tdx.Format == Tdx.D3DFormat.DXT1 ||
                tdx.Format == Tdx.D3DFormat.DXT3 ||
                tdx.Format == Tdx.D3DFormat.DXT5)
            {
                Native.Flags flags = 0;

                if (tdx.Format == Tdx.D3DFormat.DXT1)
                {
                    flags |= Native.Flags.DXT1;
                }
                else if (tdx.Format == Tdx.D3DFormat.DXT3)
                {
                    flags |= Native.Flags.DXT3;
                }
                else if (tdx.Format == Tdx.D3DFormat.DXT5)
                {
                    flags |= Native.Flags.DXT5;
                }

                byte[] decompressed = Native.DecompressImage(
                    tdx.Mipmaps[0].Data,
                    tdx.Mipmaps[0].Width,
                    tdx.Mipmaps[0].Height,
                    flags);

                bitmap = MakeBitmapFromDXT(
                    tdx.Mipmaps[0].Width,
                    tdx.Mipmaps[0].Height,
                    decompressed,
                    true);
            }
            else if (tdx.Format == Tdx.D3DFormat.A8R8G8B8)
            {
                bitmap = MakeBitmapFromA8R8G8B8(
                    tdx.Mipmaps[0].Width,
                    tdx.Mipmaps[0].Height,
                    tdx.Mipmaps[0].Data);
            }
            else if (tdx.Format == Tdx.D3DFormat.A4R4G4B4)
            {
                bitmap = MakeBitmapFromA4R4G4B4(
                    tdx.Mipmaps[0].Width,
                    tdx.Mipmaps[0].Height,
                    tdx.Mipmaps[0].Data);
            }
            else if (tdx.Format == Tdx.D3DFormat.X8R8G8B8)
            {
                bitmap = MakeBitmapFromX8R8G8B8(
                    tdx.Mipmaps[0].Width,
                    tdx.Mipmaps[0].Height,
                    tdx.Mipmaps[0].Data);
            }
            else
            {
                throw new NotSupportedException("unsupported format " + tdx.Format.ToString());
            }

            return bitmap;
		}

		private TdxFile ConvertBitmapToTdx(Tdx.D3DFormat eFormat, bool bGenerateMipMaps)
		{
			// We want nice colour conversion.
			Native.Flags eFlags = Native.Flags.ColourIterativeClusterFit;
			// No need to colour flip if not compressing.
			bool bCompressing = false;

			// we still need to limit to formats we support.
			if ((eFormat != Tdx.D3DFormat.A8R8G8B8) &&
				(eFormat != Tdx.D3DFormat.DXT1) &&
				(eFormat != Tdx.D3DFormat.DXT3) &&
				(eFormat != Tdx.D3DFormat.DXT5))
			{
				// Default to uncompressed if invalid format.
				eFormat = Tdx.D3DFormat.A8R8G8B8;
			}
			else if (eFormat == Tdx.D3DFormat.DXT1)
			{
				eFlags |= Native.Flags.DXT1;
				bCompressing = true;
			}
			else if (eFormat == Tdx.D3DFormat.DXT3)
			{
				eFlags |= Native.Flags.DXT3;
				bCompressing = true;
			}
			else if (eFormat == Tdx.D3DFormat.DXT5)
			{
				eFlags |= Native.Flags.DXT5;
				bCompressing = true;
			}

			if (bCompressing)
			{
				// If we are compressing then we must have a multiple of 4 size.
				if (((m_bmpImage.Width % 4) != 0) || ((m_bmpImage.Height % 4) != 0))
				{
					// Not a MoF size so we need to redraw at a MoF size.
					Size szNewSize = Tools.FindClosestMultipleOf4Size(m_bmpImage.Size);
                    Bitmap bmpNew = new Bitmap(szNewSize.Width, szNewSize.Height);
                    using (Graphics grfx = Graphics.FromImage(bmpNew))
                    {
                        grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        grfx.DrawImage(m_bmpImage, 0, 0, szNewSize.Width, szNewSize.Height);
                        // Set our main image to the new MoF sized image.
                        m_bmpImage.Dispose();
                        m_bmpImage = bmpNew;
                    }
				}
			}
			else if ((m_bmpImage.Width >= 2000) || (m_bmpImage.Height > 2000))
			{
				// Image is too large for TDX serialization and needs to be cut down to size.
				Size szNewSize = new Size(m_bmpImage.Width / 2, m_bmpImage.Height / 2);
				while ((szNewSize.Width >= 2000) || (szNewSize.Height >= 2000))
				{
					szNewSize.Width /= 2;
					szNewSize.Height /= 2;
				}
                Bitmap bmpNew = new Bitmap(szNewSize.Width, szNewSize.Height);
                using (Graphics grfx = Graphics.FromImage(bmpNew))
                {
                    grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grfx.DrawImage(m_bmpImage, 0, 0, szNewSize.Width, szNewSize.Height);
                    // Set our main image to the new MoF sized image.
                    m_bmpImage.Dispose();
                    m_bmpImage = bmpNew;
                }
			}

            Bitmap bitmap = m_bmpImage;
            TdxFile tdx = new TdxFile();
            tdx.Width = (ushort)bitmap.Width;
            tdx.Height = (ushort)bitmap.Height;
            tdx.Flags = 0;
            tdx.Format = eFormat;

            // Add our main image (top level).
            AddMipMap(tdx, bitmap, tdx.Width, tdx.Height, eFlags, bCompressing);

            if ((bCompressing) && (bGenerateMipMaps))
            {
                // Add the lower levels.
                ushort usLevelWidth = tdx.Width;
                ushort usLevelHeight = tdx.Height;
                while ((usLevelWidth > 1) || (usLevelHeight > 1))
                {
                    // Reduce size and keep to MoF sizing.
                    //  Even mipmap sizing removed as it has proven problematic in game for sizes that do not have even decay.
                    usLevelWidth /= 2;
                    if (usLevelWidth < 1)
                        usLevelWidth = 1;
                    usLevelHeight /= 2;
                    if (usLevelHeight < 1)
                        usLevelHeight = 1;
                    AddMipMap(tdx, bitmap, usLevelWidth, usLevelHeight, eFlags, bCompressing);
                }
            }

            return tdx;
		}

		private void AddMipMap(TdxFile tdx, Bitmap bmpImage, ushort usWidth, ushort usHeight, Native.Flags eFlags, bool bCompress)
		{
			Tdx.Mipmap mip = new Tdx.Mipmap();
			mip.Width = usWidth;
			mip.Height = usHeight;

			int stride = mip.Width * 4;

			if ((bmpImage.Width != usWidth) || (bmpImage.Height != usHeight))
			{
				Size szNewSize = new Size(usWidth, usHeight);
                Bitmap bmpNew = new Bitmap(szNewSize.Width, szNewSize.Height);
                using (Graphics grfx = Graphics.FromImage(bmpNew))
                {
                    grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grfx.DrawImage(bmpImage, 0, 0, szNewSize.Width, szNewSize.Height);
                    bmpImage.Dispose();
                    bmpImage = bmpNew;
                }
			}

			Rectangle area = new Rectangle(0, 0, usWidth, usHeight);
			byte[] buffer = new byte[mip.Height * stride];
            
            BitmapData bitmapData = bmpImage.LockBits(area, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr scan = bitmapData.Scan0;
			for (int y = 0, o = 0; y < mip.Height; y++, o += stride)
			{
				Marshal.Copy(scan, buffer, o, stride);
				scan += bitmapData.Stride;
			}
			bmpImage.UnlockBits(bitmapData);

			if (bCompress)
			{
				for (uint i = 0; i < mip.Width * mip.Height * 4; i += 4)
				{
					// flip red and blue
					byte r = buffer[i + 0];
					buffer[i + 0] = buffer[i + 2];
					buffer[i + 2] = r;
				}
				int nNewImageSize = tdx.GetMipSize(tdx.Format, usWidth, usHeight);
				mip.Data = Native.CompressImage(buffer, usWidth, usHeight, eFlags, nNewImageSize);
			}
			else
				mip.Data = buffer;

			tdx.Mipmaps.Add(mip);
		}

		private Bitmap MakeBitmapFromDXT(uint width, uint height, byte[] buffer, bool keepAlpha)
		{
            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
            
            for (uint i = 0; i < width * height * 4; i += 4)
            {
                // flip red and blue
                byte r = buffer[i + 0];
                buffer[i + 0] = buffer[i + 2];
                buffer[i + 2] = r;
            }

            Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
            bitmap.UnlockBits(data);
            return bitmap;
		}

		private Bitmap MakeBitmapFromA8R8G8B8(uint width, uint height, byte[] buffer)
		{
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private Bitmap MakeBitmapFromA4R4G4B4(uint width, uint height, byte[] buffer)
		{
            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

            byte[] newbuffer = new byte[width * height * 4];

            for (uint i = 0, j = 0; i < width * height * 2; i += 2, j += 4)
            {
                newbuffer[j + 0] = (byte)(((buffer[i + 0] >> 0) & 0x0F) * 0x11); // A
                newbuffer[j + 1] = (byte)(((buffer[i + 0] >> 4) & 0x0F) * 0x11); // R
                newbuffer[j + 2] = (byte)(((buffer[i + 1] >> 0) & 0x0F) * 0x11); // G
                newbuffer[j + 3] = (byte)(((buffer[i + 1] >> 4) & 0x0F) * 0x11); // B
            }

            Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(newbuffer, 0, data.Scan0, (int)(width * height * 4));
            bitmap.UnlockBits(data);
            return bitmap;
		}

		private Bitmap MakeBitmapFromX8R8G8B8(uint width, uint height, byte[] buffer)
		{
            Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppRgb);
            Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
            bitmap.UnlockBits(data);
            return bitmap;
		}

		public void Dispose()
		{
            if (m_bmpImage != null)
            {
                m_bmpImage.Dispose();
                m_bmpImage = null;
            }
        }
	}
}
