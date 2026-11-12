using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class BufferedBitmap : IDisposable
{
	private Rectangle m_TargetRect = Rectangle.Empty;

	private IntPtr m_MemDC = IntPtr.Zero;

	private Graphics m_BitmapGraphics;

	private IntPtr m_MemDib = IntPtr.Zero;

	public Graphics Graphics => m_BitmapGraphics;

	public Rectangle TargetRect
	{
		get
		{
			return m_TargetRect;
		}
		set
		{
			m_TargetRect = value;
		}
	}

	public BufferedBitmap(Graphics source, Rectangle rect)
	{
		IntPtr hdc = source.GetHdc();
		try
		{
			Initialize(hdc, rect);
		}
		finally
		{
			source.ReleaseHdc(hdc);
		}
	}

	public BufferedBitmap(IntPtr hdcSource, Rectangle rect)
	{
		Initialize(hdcSource, rect);
	}

	private void Initialize(IntPtr hdcSource, Rectangle r)
	{
		m_TargetRect = r;
		m_MemDC = WinApi.CreateCompatibleDC(hdcSource);
		WinApi.BITMAPINFO bITMAPINFO = new WinApi.BITMAPINFO();
		bITMAPINFO.biWidth = r.Width;
		bITMAPINFO.biHeight = r.Height;
		bITMAPINFO.biPlanes = 1;
		bITMAPINFO.biBitCount = 32;
		bITMAPINFO.biSize = Marshal.SizeOf(bITMAPINFO);
		m_MemDib = WinApi.CreateDIBSection(hdcSource, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
		WinApi.SelectObject(m_MemDC, m_MemDib);
		m_BitmapGraphics = Graphics.FromHdc(m_MemDC);
	}

	public void Render(Graphics targetGraphics)
	{
		Render(targetGraphics, new Rectangle[0]);
	}

	public void Render(Graphics targetGraphics, Rectangle exclude)
	{
		Render(targetGraphics, new Rectangle[1] { exclude });
	}

	public void Render(Graphics targetGraphics, Rectangle[] excludeArr)
	{
		IntPtr hdc = targetGraphics.GetHdc();
		try
		{
			if (excludeArr != null && excludeArr.Length != 0)
			{
				for (int i = 0; i < excludeArr.Length; i++)
				{
					Rectangle rectangle = excludeArr[i];
					if (!rectangle.IsEmpty)
					{
						WinApi.ExcludeClipRect(hdc, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
					}
				}
			}
			WinApi.BitBlt(hdc, m_TargetRect.Left, m_TargetRect.Top, m_TargetRect.Width, m_TargetRect.Height, m_MemDC, 0, 0, 13369376u);
		}
		finally
		{
			targetGraphics.ReleaseHdc(hdc);
		}
	}

	public void Dispose()
	{
		if (m_BitmapGraphics != null)
		{
			m_BitmapGraphics.Dispose();
			m_BitmapGraphics = null;
		}
		if (m_MemDib != IntPtr.Zero)
		{
			WinApi.DeleteObject(m_MemDib);
			m_MemDib = IntPtr.Zero;
		}
		if (m_MemDC != IntPtr.Zero)
		{
			WinApi.DeleteDC(m_MemDC);
			m_MemDC = IntPtr.Zero;
		}
	}
}
