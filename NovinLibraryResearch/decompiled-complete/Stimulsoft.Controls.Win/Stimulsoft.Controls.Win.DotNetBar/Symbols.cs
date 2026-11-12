using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class Symbols
{
	private static Dictionary<float, Font> _FontAwesomeCache = new Dictionary<float, Font>(10);

	private static Font _FontAwesome = null;

	private static PrivateFontCollection _PrivateFontCollection;

	private static GCHandle _FontAwesomeHandle;

	private static IntPtr _FontAwesomePointer;

	private static float _FontAwesomeDefaultSize = 18f;

	public static Font FontAwesome
	{
		get
		{
			EnsureFontLoaded();
			return _FontAwesome;
		}
	}

	public static FontFamily FontAwesomeFamily
	{
		get
		{
			EnsureFontLoaded();
			return _FontAwesome.FontFamily;
		}
	}

	public static float FontAwesomeDefaultSize => _FontAwesomeDefaultSize;

	[DllImport("gdi32")]
	private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

	[DllImport("gdi32")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RemoveFontMemResourceEx(IntPtr fh);

	public static Font GetFontAwesome(float fontSize)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		Font value = null;
		EnsureFontLoaded();
		if (fontSize <= 0f)
		{
			return _FontAwesome;
		}
		if (_FontAwesomeCache.TryGetValue(fontSize, out value))
		{
			return value;
		}
		value = new Font(_FontAwesome.FontFamily, fontSize, (FontStyle)0, (GraphicsUnit)3);
		_FontAwesomeCache.Add(fontSize, value);
		return value;
	}

	private static void EnsureFontLoaded()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (_FontAwesome != null)
		{
			return;
		}
		_PrivateFontCollection = new PrivateFontCollection();
		byte[] array = BarFunctions.LoadFont("SystemImages.FontAwesome.ttf");
		_FontAwesomeHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		_PrivateFontCollection.AddMemoryFont(_FontAwesomeHandle.AddrOfPinnedObject(), array.Length);
		uint pcFonts = 1u;
		_FontAwesomePointer = AddFontMemResourceEx(_FontAwesomeHandle.AddrOfPinnedObject(), (uint)array.Length, IntPtr.Zero, ref pcFonts);
		FontFamily val = ((FontCollection)_PrivateFontCollection).Families[0];
		try
		{
			if (val.IsStyleAvailable((FontStyle)0))
			{
				_FontAwesome = new Font(val, _FontAwesomeDefaultSize, (FontStyle)0, (GraphicsUnit)3);
				_FontAwesomeCache.Add(_FontAwesomeDefaultSize, _FontAwesome);
			}
			else
			{
				_FontAwesome = SystemInformation.MenuFont;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
