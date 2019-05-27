using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RSN.DotP
{
	class ImageBuilder
	{
		private Bitmap m_bmpOverlay;
		private Bitmap m_bmpMask;
		private Bitmap m_bmpAlpha;
		private Bitmap m_bmpLastBuilt;
		private Rectangle m_rcFullImageRect;
		private Rectangle m_rcMaskOptimizedRect;
		private Size m_szFullImageSize;
		private Point m_ptMouseReferenceLocation;
		private Point m_ptMouseStartLocation;
		private float m_fMouseChangeMultiplier;

		public Bitmap LoadedImage;
		public Rectangle AdjustedRect;

		public ImageBuilder(Bitmap bmpOverlay, Bitmap bmpMask, Bitmap bmpAlpha, Rectangle rcMaskOptimal)
		{
			m_bmpOverlay = bmpOverlay;
			m_bmpMask = bmpMask;
			m_bmpAlpha = bmpAlpha;
			if (m_bmpOverlay != null)
				m_szFullImageSize = m_bmpOverlay.Size;
			else if (m_bmpMask != null)
				m_szFullImageSize = m_bmpMask.Size;
			else if (m_bmpAlpha != null)
				m_szFullImageSize = m_bmpAlpha.Size;
			else
				m_szFullImageSize = rcMaskOptimal.Size;
			m_rcFullImageRect = new Rectangle(new Point(0, 0), m_szFullImageSize);
			m_rcMaskOptimizedRect = rcMaskOptimal;
			m_bmpLastBuilt = null;

			// Initialize the points related to mouse manipulation just in case.
			m_ptMouseReferenceLocation = new Point(-1, -1);
			m_ptMouseStartLocation = new Point(-1, -1);
			m_fMouseChangeMultiplier = 1f;
		}

		public void InitialImageAdjust()
		{
			Size szLoaded = LoadedImage.Size;
			Size szMaskOptimal = m_rcMaskOptimizedRect.Size;
			Size szAdjusted = new Size(0, 0);
			Point ptAdjust = new Point();

			double dRatio = 0;

			// See if we can shrink this height-wise first.
			dRatio = ((double)szMaskOptimal.Height) / ((double)szLoaded.Height);
			if (szMaskOptimal.Width <= (int)(dRatio * szLoaded.Width))
			{
				// This will work.
				szAdjusted = new Size((int)(dRatio * szLoaded.Width), szMaskOptimal.Height);
				// Center horizontally
				ptAdjust = new Point((szMaskOptimal.Width - szAdjusted.Width) / 2, 0);
			}

			// Now we see if we can shrink it width-wise.
			dRatio = ((double)szMaskOptimal.Width) / ((double)szLoaded.Width);
			if (szMaskOptimal.Height <= (int)(dRatio * szLoaded.Height))
			{
				// This will work.
				if (szAdjusted.Width != 0)
				{
					// In most cases the image can only be squished one way so this block of code will probably go completely unused.
					Size szTest = new Size(szMaskOptimal.Width, (int)(dRatio * szLoaded.Height));
					ptAdjust = new Point((szMaskOptimal.Width - szAdjusted.Width) / 2, 0);
					double dHeightWasteRatio = ((double)szMaskOptimal.Height) / ((double)szTest.Height);
					double dWidthWasteRatio = ((double)szMaskOptimal.Width) / ((double)szAdjusted.Width);
					if (dHeightWasteRatio < dWidthWasteRatio)
					{
						szAdjusted = szTest;
						// Center vertically
						ptAdjust = new Point(0, (szMaskOptimal.Height - szAdjusted.Height) / 2);
					}
				}
				else
				{
					szAdjusted = new Size(szMaskOptimal.Width, (int)(dRatio * szLoaded.Height));
					// Center vertically
					ptAdjust = new Point(0, (szMaskOptimal.Height - szAdjusted.Height) / 2);
				}
			}

			// Now we figure out our actual upper left corner.
			ptAdjust = new Point(m_rcMaskOptimizedRect.Location.X + ptAdjust.X, m_rcMaskOptimizedRect.Location.Y + ptAdjust.Y);

			// Now we construct our mask targetting rect
			AdjustedRect = new Rectangle(ptAdjust, szAdjusted);
		}

		public float MouseChangeModifier
		{
			get { return m_fMouseChangeMultiplier; }
			set { m_fMouseChangeMultiplier = value; }
		}

		public Bitmap BuildImage()
		{
			// Apply the mask first.
			Bitmap bmpMasked = null;
			if (m_bmpMask != null)
				bmpMasked = Tools.MaskImage(LoadedImage, m_bmpMask, AdjustedRect);
			else
				bmpMasked = Tools.AdjustImage(LoadedImage, m_szFullImageSize, AdjustedRect);

			// If we have an overlay then apply it.
			Bitmap bmpOverlaid = null;
			if (m_bmpOverlay != null)
			{
                bmpOverlaid.Dispose();
                bmpOverlaid = new Bitmap(m_szFullImageSize.Width, m_szFullImageSize.Height);
                using (Graphics grfx = Graphics.FromImage(bmpOverlaid))
                {
                    grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grfx.DrawImage(bmpMasked, m_rcFullImageRect);
                    grfx.DrawImage(m_bmpOverlay, m_rcFullImageRect);
                }

				// Now we no longer need the masked image.
				bmpMasked.Dispose();
			}
			else
				bmpOverlaid = bmpMasked;

			// Now we apply the Alpha layer if we have one.
			Bitmap bmpAlphaApplied = null;
			if (m_bmpAlpha != null)
			{
				bmpAlphaApplied = Tools.ApplyAlphaToImage(bmpOverlaid, m_bmpAlpha);

				// Now we no longer need the overlaid image.
				bmpOverlaid.Dispose();
			}
			else
				bmpAlphaApplied = bmpOverlaid;

			// This allows us to quickly send back the last image we built rather than building it again.
			if (m_bmpLastBuilt != null)
				m_bmpLastBuilt.Dispose();
			m_bmpLastBuilt = bmpAlphaApplied;

			return m_bmpLastBuilt;
		}

		public void MouseDownHandler(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_ptMouseReferenceLocation = AdjustedRect.Location;
				m_ptMouseStartLocation = e.Location;
			}
		}

		public Bitmap MouseMoveHandler(MouseEventArgs e)
		{
			if ((e.Button == MouseButtons.Left) && (m_ptMouseStartLocation.X != -1))
			{
				// We need to figure out how far to change the location
				Point ptMove = new Point(m_ptMouseStartLocation.X - e.X, m_ptMouseStartLocation.Y - e.Y);
				// Now we adjust the location based on our reference.
				AdjustedRect.Location = new Point(m_ptMouseReferenceLocation.X - (int)Math.Round(ptMove.X * m_fMouseChangeMultiplier),
													m_ptMouseReferenceLocation.Y - (int)Math.Round(ptMove.Y * m_fMouseChangeMultiplier));
				return BuildImage();
			}
			return m_bmpLastBuilt;
		}

		public void MouseUpHandler(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_ptMouseStartLocation = new Point(-1, -1);
		}

		public Bitmap MouseWheelHandler(MouseEventArgs e)
		{
			if (e.Delta != 0)
			{
				// Adjusting the entire rect
				double dRatio = ((double)AdjustedRect.Width) / ((double)AdjustedRect.Height);
				float fChange = (e.Delta / 120) * m_fMouseChangeMultiplier;
				Rectangle rcNew = new Rectangle(AdjustedRect.X - (int)Math.Round(fChange * dRatio),
												AdjustedRect.Y - (int)Math.Round(fChange),
												AdjustedRect.Width + (int)Math.Round(fChange * dRatio * 2),
												AdjustedRect.Height + (int)Math.Round(fChange * 2));
				if ((rcNew.Width > 0) && (rcNew.Height > 0))
				{
					AdjustedRect = rcNew;
					return BuildImage();
				}
			}
			return m_bmpLastBuilt;
		}
	}
}
