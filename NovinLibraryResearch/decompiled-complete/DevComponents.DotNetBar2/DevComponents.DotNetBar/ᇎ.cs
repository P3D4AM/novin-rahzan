using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

internal class ᇎ : IDisposable
{
	private bool ᇖ;

	private Image _0E66;

	private Icon ຈ;

	private Size ຆ = Size.Empty;

	private ImageList գ;

	private int դ = -1;

	public bool ᇏ => ຈ != null;

	public int ܐ
	{
		get
		{
			if (!ຆ.IsEmpty)
			{
				return ᇠ(ຆ.Width);
			}
			if (_0E66 != null)
			{
				return ᇠ(_0E66.Width);
			}
			if (ຈ != null)
			{
				return ᇠ(ຈ.Width);
			}
			if (դ >= 0 && գ != null)
			{
				return ᇠ(գ.ImageSize.Width);
			}
			return 0;
		}
	}

	public int _0711
	{
		get
		{
			if (!ຆ.IsEmpty)
			{
				return ᇠ(ຆ.Height);
			}
			if (_0E66 != null)
			{
				return ᇠ(_0E66.Height);
			}
			if (ຈ != null)
			{
				return ᇠ(ຈ.Height);
			}
			if (դ >= 0 && գ != null)
			{
				return ᇠ(գ.ImageSize.Height);
			}
			return 0;
		}
	}

	public Size ᇐ
	{
		get
		{
			if (!ຆ.IsEmpty)
			{
				return ᇠ(ຆ);
			}
			if (_0E66 != null)
			{
				return ᇠ(_0E66.Size);
			}
			if (ຈ != null)
			{
				return ᇠ(ຈ.Size);
			}
			if (դ >= 0 && գ != null)
			{
				return ᇠ(գ.ImageSize);
			}
			return Size.Empty;
		}
	}

	public Image ᇑ
	{
		get
		{
			if (դ >= 0 && գ != null)
			{
				return գ.Images[դ];
			}
			return _0E66;
		}
	}

	public Icon ᆻ => ຈ;

	internal int ᇒ
	{
		get
		{
			if (_0E66 != null)
			{
				return ᇠ(_0E66.Height);
			}
			if (ຈ != null)
			{
				return ᇠ(ຈ.Height);
			}
			if (դ >= 0 && գ != null)
			{
				return ᇠ(գ.ImageSize.Height);
			}
			return 0;
		}
	}

	internal int ᇓ
	{
		get
		{
			if (_0E66 != null)
			{
				return ᇠ(_0E66.Width);
			}
			if (ຈ != null)
			{
				return ᇠ(ຈ.Width);
			}
			if (դ >= 0 && գ != null)
			{
				return ᇠ(գ.ImageSize.Width);
			}
			return 0;
		}
	}

	internal int ᇔ
	{
		get
		{
			if (_0E66 != null)
			{
				return _0E66.Height;
			}
			if (ຈ != null)
			{
				return ຈ.Height;
			}
			if (դ >= 0 && գ != null)
			{
				return գ.ImageSize.Height;
			}
			return 0;
		}
	}

	internal int ᇕ
	{
		get
		{
			if (_0E66 != null)
			{
				return _0E66.Width;
			}
			if (ຈ != null)
			{
				return ຈ.Width;
			}
			if (դ >= 0 && գ != null)
			{
				return գ.ImageSize.Width;
			}
			return 0;
		}
	}

	public ᇎ(int imageIndex, ImageList imageList)
	{
		դ = imageIndex;
		գ = imageList;
	}

	public ᇎ(Image image, bool dispose)
	{
		_0E66 = image;
		ᇖ = dispose;
	}

	public ᇎ(Icon icon, bool dispose)
	{
		ຈ = icon;
		ᇖ = dispose;
	}

	public ᇎ(Image image, bool dispose, Size overrideSize)
	{
		_0E66 = image;
		ᇖ = dispose;
		ຆ = overrideSize;
	}

	public ᇎ(Icon icon, bool dispose, Size overrideSize)
	{
		ຈ = icon;
		ᇖ = dispose;
		ຆ = overrideSize;
	}

	~ᇎ()
	{
		if (ᇖ && (_0E66 != null || ຈ != null))
		{
			Dispose();
		}
	}

	public void Dispose()
	{
		if (ᇖ)
		{
			if (_0E66 != null)
			{
				_0E66.Dispose();
			}
			if (ຈ != null)
			{
				ຈ.Dispose();
			}
		}
		_0E66 = null;
		ຈ = null;
	}

	public void ᇗ(Graphics _٥, Rectangle भ)
	{
		if (_0E66 != null)
		{
			_٥.DrawImage(_0E66, भ);
		}
		else if (ຈ != null)
		{
			ᇘ(_٥, भ);
		}
		else if (դ >= 0 && գ != null && դ < գ.Images.Count)
		{
			գ.Draw(_٥, भ.X, भ.Y, भ.Width, भ.Height, դ);
		}
	}

	private void ᇘ(Graphics _٥, Rectangle भ)
	{
		if (Environment.Version.Build <= 3705 && Environment.Version.Revision == 288 && Environment.Version.Major == 1 && Environment.Version.Minor == 0)
		{
			if (_٥.ClipBounds.IntersectsWith(भ) && भ.Width > 0 && भ.Height > 0 && ຈ.Handle != IntPtr.Zero)
			{
				IntPtr hdc = _٥.GetHdc();
				try
				{
					_089F.DrawIconEx(hdc, भ.X, भ.Y, ຈ.Handle, भ.Width, भ.Height, 0, IntPtr.Zero, 3);
				}
				finally
				{
					_٥.ReleaseHdc(hdc);
				}
			}
		}
		else if (भ.Width > 0 && भ.Height > 0 && ຈ.Handle != IntPtr.Zero)
		{
			try
			{
				_٥.DrawIcon(ຈ, भ);
			}
			catch
			{
			}
		}
	}

	public void ᇗ(Graphics _٥, Rectangle ᇙ, int ᇚ, int ᇛ, int ᇜ, int ᇝ, GraphicsUnit ᇞ, ImageAttributes ᇟ)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (_0E66 != null)
		{
			_٥.DrawImage(_0E66, ᇙ, ᇚ, ᇛ, ᇜ, ᇝ, ᇞ, ᇟ);
		}
		else if (ຈ != null)
		{
			Bitmap val = new Bitmap(ܐ, _0711, (PixelFormat)2498570);
			val.MakeTransparent();
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.DrawIcon(ຈ, 0, 0);
			val2.Dispose();
			_٥.DrawImage((Image)(object)val, ᇙ, ᇚ, ᇛ, ᇜ, ᇝ, ᇞ, ᇟ);
			((Image)val).Dispose();
		}
		else if (դ >= 0 && գ != null && դ < գ.Images.Count)
		{
			գ.Draw(_٥, ᇚ, ᇛ, ᇜ, ᇝ, դ);
		}
	}

	private int ᇠ(int _0970)
	{
		if (!Dpi.AutoScaleImages)
		{
			return _0970;
		}
		return Dpi.Height(_0970);
	}

	private Size ᇠ(Size ߒ)
	{
		if (!Dpi.AutoScaleImages)
		{
			return ߒ;
		}
		return Dpi.Size(ߒ);
	}
}
