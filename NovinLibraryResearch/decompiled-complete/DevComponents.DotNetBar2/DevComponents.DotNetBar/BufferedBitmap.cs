using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar;

public class BufferedBitmap : IDisposable
{
	private Rectangle _1DE8 = Rectangle.Empty;

	private IntPtr _1DE9 = IntPtr.Zero;

	private Graphics _1DEA;

	private IntPtr _1DEB = IntPtr.Zero;

	public Graphics Graphics => _1DEA;

	public Rectangle TargetRect
	{
		get
		{
			return _1DE8;
		}
		set
		{
			_1DE8 = value;
		}
	}

	public BufferedBitmap(Graphics source, Rectangle rect)
	{
		IntPtr hdc = source.GetHdc();
		try
		{
			_1AB8(hdc, rect);
		}
		finally
		{
			source.ReleaseHdc(hdc);
		}
	}

	public BufferedBitmap(IntPtr hdcSource, Rectangle rect)
	{
		_1AB8(hdcSource, rect);
	}

	private void _1AB8(IntPtr _1DEC, Rectangle ल)
	{
		_1DE8 = ल;
		_1DE9 = _0EFE.CreateCompatibleDC(_1DEC);
		_0EFE.BITMAPINFO bITMAPINFO = new _0EFE.BITMAPINFO();
		bITMAPINFO.biWidth = ल.Width;
		bITMAPINFO.biHeight = ल.Height;
		bITMAPINFO.biPlanes = 1;
		bITMAPINFO.biBitCount = 32;
		bITMAPINFO.biSize = Marshal.SizeOf((object)bITMAPINFO);
		_1DEB = _0EFE.CreateDIBSection(_1DEC, bITMAPINFO, 0u, 0, IntPtr.Zero, 0u);
		_0EFE.SelectObject(_1DE9, _1DEB);
		_1DEA = Graphics.FromHdc(_1DE9);
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
			if (excludeArr != null && excludeArr.Length > 0)
			{
				for (int i = 0; i < excludeArr.Length; i++)
				{
					Rectangle rectangle = excludeArr[i];
					if (!rectangle.IsEmpty)
					{
						_0EFE.ExcludeClipRect(hdc, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
					}
				}
			}
			_0EFE.BitBlt(hdc, _1DE8.Left, _1DE8.Top, _1DE8.Width, _1DE8.Height, _1DE9, 0, 0, 13369376u);
		}
		finally
		{
			targetGraphics.ReleaseHdc(hdc);
		}
	}

	public void Dispose()
	{
		if (_1DEA != null)
		{
			_1DEA.Dispose();
			_1DEA = null;
		}
		if (_1DEB != IntPtr.Zero)
		{
			_0EFE.DeleteObject(_1DEB);
			_1DEB = IntPtr.Zero;
		}
		if (_1DE9 != IntPtr.Zero)
		{
			_0EFE.DeleteDC(_1DE9);
			_1DE9 = IntPtr.Zero;
		}
	}
}
