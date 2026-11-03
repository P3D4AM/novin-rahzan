using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class Symbols
{
	private static Dictionary<float, Font> __1A58 = new Dictionary<float, Font>(10);

	private static Dictionary<float, Font> __1A59 = new Dictionary<float, Font>(10);

	private static Font __1A5A = null;

	private static Font __1A5B = null;

	private static PrivateFontCollection __1A5C;

	private static PrivateFontCollection __1A5D;

	private static GCHandle __1A5E;

	private static IntPtr __1A5F;

	private static GCHandle __1A60;

	private static IntPtr __1A61;

	private static float __1A62 = 18f;

	private static float __1A63 = 18f;

	public static Font FontAwesome
	{
		get
		{
			_1A66();
			return __1A5A;
		}
	}

	public static FontFamily FontAwesomeFamily
	{
		get
		{
			_1A66();
			return __1A5A.FontFamily;
		}
	}

	public static Font FontMaterial
	{
		get
		{
			_1A67();
			return __1A5B;
		}
	}

	public static FontFamily FontMaterialFamily
	{
		get
		{
			_1A67();
			return __1A5B.FontFamily;
		}
	}

	public static float FontMaterialDefaultSize => __1A62;

	public static float FontAwesomeDefaultSize => __1A63;

	[DllImport("gdi32")]
	private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

	[DllImport("gdi32")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RemoveFontMemResourceEx(IntPtr fh);

	public static string GetSymbol(string symbol)
	{
		if (string.IsNullOrEmpty(symbol))
		{
			return string.Empty;
		}
		if (symbol.Length == 1)
		{
			return symbol;
		}
		int result = 0;
		if (int.TryParse(symbol, out result))
		{
			return char.ConvertFromUtf32(result);
		}
		return symbol;
	}

	public static Font GetFont(float fontSize, eSymbolSet symbolSet)
	{
		if (symbolSet == eSymbolSet.Awesome)
		{
			return _1A64(fontSize);
		}
		return GetFontMaterial(fontSize);
	}

	private static Font _1A64(float _1A65)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		Font value = null;
		_1A66();
		if (_1A65 <= 0f)
		{
			return __1A5A;
		}
		if (__1A58.TryGetValue(_1A65, out value))
		{
			return value;
		}
		value = new Font(__1A5A.FontFamily, _1A65, (FontStyle)0, (GraphicsUnit)3);
		__1A58.Add(_1A65, value);
		return value;
	}

	public static Font GetFontMaterial(float fontSize)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		Font value = null;
		_1A67();
		if (fontSize <= 0f)
		{
			return __1A5B;
		}
		if (__1A59.TryGetValue(fontSize, out value))
		{
			return value;
		}
		value = new Font(__1A5B.FontFamily, fontSize, (FontStyle)0, (GraphicsUnit)3);
		__1A59.Add(fontSize, value);
		return value;
	}

	private static void _1A66()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (__1A5A != null)
		{
			return;
		}
		__1A5C = new PrivateFontCollection();
		byte[] array = _1A68("SystemImages.FontAwesome.ttf");
		__1A5E = GCHandle.Alloc(array, GCHandleType.Pinned);
		__1A5C.AddMemoryFont(__1A5E.AddrOfPinnedObject(), array.Length);
		uint pcFonts = 1u;
		__1A5F = AddFontMemResourceEx(__1A5E.AddrOfPinnedObject(), (uint)array.Length, IntPtr.Zero, ref pcFonts);
		FontFamily val = ((FontCollection)__1A5C).Families[0];
		try
		{
			if (val.IsStyleAvailable((FontStyle)0))
			{
				__1A5A = new Font(val, __1A63, (FontStyle)0, (GraphicsUnit)3);
				__1A58.Add(__1A63, __1A5A);
			}
			else
			{
				__1A5A = SystemInformation.MenuFont;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void _1A67()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (__1A5B != null)
		{
			return;
		}
		__1A5D = new PrivateFontCollection();
		byte[] array = _1A68("SystemImages.MaterialIcons-Regular.ttf");
		__1A60 = GCHandle.Alloc(array, GCHandleType.Pinned);
		__1A5D.AddMemoryFont(__1A60.AddrOfPinnedObject(), array.Length);
		uint pcFonts = 1u;
		__1A61 = AddFontMemResourceEx(__1A60.AddrOfPinnedObject(), (uint)array.Length, IntPtr.Zero, ref pcFonts);
		FontFamily val = ((FontCollection)__1A5D).Families[0];
		try
		{
			if (val.IsStyleAvailable((FontStyle)0))
			{
				__1A5B = new Font(val, __1A62, (FontStyle)0, (GraphicsUnit)3);
				__1A59.Add(__1A62, __1A5B);
			}
			else
			{
				__1A5B = SystemInformation.MenuFont;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static byte[] _1A68(string _1A69)
	{
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + _1A69))
			{
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
				stream.Close();
				return array;
			}
		}
		using Stream stream2 = typeof(DotNetBarManager).Module.Assembly.GetManifestResourceStream(typeof(DotNetBarManager), _1A69);
		byte[] array2 = new byte[stream2.Length];
		stream2.Read(array2, 0, array2.Length);
		stream2.Close();
		return array2;
	}
}
