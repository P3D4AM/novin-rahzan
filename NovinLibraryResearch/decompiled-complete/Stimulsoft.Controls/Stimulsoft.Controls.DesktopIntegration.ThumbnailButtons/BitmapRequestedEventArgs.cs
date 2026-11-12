using System;
using System.Drawing;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

public class BitmapRequestedEventArgs : EventArgs
{
	private int _width;

	private int _height;

	private bool useWindowScreenshot;

	private bool doNotMirrorBitmap;

	private bool displayFrameAroundBitmap;

	private Bitmap bitmap;

	public int Width => _width;

	public int Height => _height;

	public bool UseWindowScreenshot
	{
		internal get
		{
			return useWindowScreenshot;
		}
		set
		{
			useWindowScreenshot = value;
		}
	}

	public bool DoNotMirrorBitmap
	{
		internal get
		{
			return doNotMirrorBitmap;
		}
		set
		{
			doNotMirrorBitmap = value;
		}
	}

	public bool DisplayFrameAroundBitmap
	{
		internal get
		{
			return displayFrameAroundBitmap;
		}
		set
		{
			displayFrameAroundBitmap = value;
		}
	}

	public Bitmap Bitmap
	{
		internal get
		{
			return bitmap;
		}
		set
		{
			bitmap = value;
		}
	}

	internal BitmapRequestedEventArgs(int width, int height, bool defaultDisplayFrame)
	{
		_width = width;
		_height = height;
		DisplayFrameAroundBitmap = defaultDisplayFrame;
	}
}
