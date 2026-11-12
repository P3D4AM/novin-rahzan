using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CompositeImage : IDisposable
{
	private bool m_DisposeImage;

	private Image m_Image;

	private Icon m_Icon;

	private Size m_ImageSizeOverride = Size.Empty;

	private ImageList m_ImageList;

	private int m_ImageIndex = -1;

	public bool IsIcon => m_Icon != null;

	public int Width
	{
		get
		{
			if (!m_ImageSizeOverride.IsEmpty)
			{
				return m_ImageSizeOverride.Width;
			}
			if (m_Image != null)
			{
				return m_Image.Width;
			}
			if (m_Icon != null)
			{
				return m_Icon.Width;
			}
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.ImageSize.Width;
			}
			return 0;
		}
	}

	public int Height
	{
		get
		{
			if (!m_ImageSizeOverride.IsEmpty)
			{
				return m_ImageSizeOverride.Height;
			}
			if (m_Image != null)
			{
				return m_Image.Height;
			}
			if (m_Icon != null)
			{
				return m_Icon.Height;
			}
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.ImageSize.Height;
			}
			return 0;
		}
	}

	public Size Size
	{
		get
		{
			if (!m_ImageSizeOverride.IsEmpty)
			{
				return m_ImageSizeOverride;
			}
			if (m_Image != null)
			{
				return m_Image.Size;
			}
			if (m_Icon != null)
			{
				return m_Icon.Size;
			}
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.ImageSize;
			}
			return Size.Empty;
		}
	}

	public Image Image
	{
		get
		{
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.Images[m_ImageIndex];
			}
			return m_Image;
		}
	}

	public Icon Icon => m_Icon;

	internal int RealHeight
	{
		get
		{
			if (m_Image != null)
			{
				return m_Image.Height;
			}
			if (m_Icon != null)
			{
				return m_Icon.Height;
			}
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.ImageSize.Height;
			}
			return 0;
		}
	}

	internal int RealWidth
	{
		get
		{
			if (m_Image != null)
			{
				return m_Image.Width;
			}
			if (m_Icon != null)
			{
				return m_Icon.Width;
			}
			if (m_ImageIndex >= 0 && m_ImageList != null)
			{
				return m_ImageList.ImageSize.Width;
			}
			return 0;
		}
	}

	public CompositeImage(int imageIndex, ImageList imageList)
	{
		m_ImageIndex = imageIndex;
		m_ImageList = imageList;
	}

	public CompositeImage(Image image, bool dispose)
	{
		m_Image = image;
		m_DisposeImage = dispose;
	}

	public CompositeImage(Icon icon, bool dispose)
	{
		m_Icon = icon;
		m_DisposeImage = dispose;
	}

	public CompositeImage(Image image, bool dispose, Size overrideSize)
	{
		m_Image = image;
		m_DisposeImage = dispose;
		m_ImageSizeOverride = overrideSize;
	}

	public CompositeImage(Icon icon, bool dispose, Size overrideSize)
	{
		m_Icon = icon;
		m_DisposeImage = dispose;
		m_ImageSizeOverride = overrideSize;
	}

	~CompositeImage()
	{
		if (m_DisposeImage && (m_Image != null || m_Icon != null))
		{
			Dispose();
		}
	}

	public void Dispose()
	{
		if (m_DisposeImage)
		{
			if (m_Image != null)
			{
				m_Image.Dispose();
			}
			if (m_Icon != null)
			{
				m_Icon.Dispose();
			}
		}
		m_Image = null;
		m_Icon = null;
	}

	public void DrawImage(Graphics g, Rectangle rect)
	{
		try
		{
			if (m_Image != null)
			{
				g.DrawImage(m_Image, rect);
			}
			else if (m_Icon != null)
			{
				DrawIcon(g, rect);
			}
			else if (m_ImageIndex >= 0 && m_ImageList != null && m_ImageIndex < m_ImageList.Images.Count)
			{
				m_ImageList.Draw(g, rect.X, rect.Y, rect.Width, rect.Height, m_ImageIndex);
			}
		}
		catch
		{
		}
	}

	private void DrawIcon(Graphics g, Rectangle rect)
	{
		if (Environment.Version.Build <= 3705 && Environment.Version.Revision == 288 && Environment.Version.Major == 1 && Environment.Version.Minor == 0)
		{
			if (g.ClipBounds.IntersectsWith(rect) && rect.Width > 0 && rect.Height > 0 && m_Icon.Handle != IntPtr.Zero)
			{
				IntPtr hdc = g.GetHdc();
				try
				{
					NativeFunctions.DrawIconEx(hdc, rect.X, rect.Y, m_Icon.Handle, rect.Width, rect.Height, 0, IntPtr.Zero, 3);
				}
				finally
				{
					g.ReleaseHdc(hdc);
				}
			}
		}
		else if (rect.Width > 0 && rect.Height > 0 && m_Icon.Handle != IntPtr.Zero)
		{
			try
			{
				g.DrawIcon(m_Icon, rect);
			}
			catch
			{
			}
		}
	}

	public void DrawImage(Graphics g, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (m_Image != null)
		{
			g.DrawImage(m_Image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
		}
		else if (m_Icon != null)
		{
			Bitmap val = new Bitmap(Width, Height, (PixelFormat)2498570);
			val.MakeTransparent();
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.DrawIcon(m_Icon, 0, 0);
			val2.Dispose();
			g.DrawImage((Image)(object)val, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
			((Image)val).Dispose();
		}
		else if (m_ImageIndex >= 0 && m_ImageList != null && m_ImageIndex < m_ImageList.Images.Count)
		{
			m_ImageList.Draw(g, srcX, srcY, srcWidth, srcHeight, m_ImageIndex);
		}
	}
}
