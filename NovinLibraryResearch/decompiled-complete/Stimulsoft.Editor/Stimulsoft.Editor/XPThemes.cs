using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class XPThemes
{
	private abstract class XPImages : IDisposable
	{
		internal Bitmap collapse;

		internal Bitmap collapseHot;

		internal Bitmap expand;

		internal Bitmap expandHot;

		internal Bitmap groupHeader;

		public Bitmap Collapse => collapse;

		public Bitmap CollapseHot => collapseHot;

		public Bitmap Expand => expand;

		public Bitmap ExpandHot => expandHot;

		public Bitmap GroupHeader => groupHeader;

		internal abstract string GetImagesSubPath();

		public void Dispose()
		{
			((Image)collapse).Dispose();
			((Image)collapseHot).Dispose();
			((Image)expand).Dispose();
			((Image)expandHot).Dispose();
			((Image)GroupHeader).Dispose();
		}

		public void LoadImages()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			Assembly assembly = typeof(XPImages).Assembly;
			string name = assembly.GetName().Name;
			string imagesSubPath = GetImagesSubPath();
			Stream manifestResourceStream = assembly.GetManifestResourceStream(name + imagesSubPath + ".Collapse.png");
			collapse = new Bitmap(manifestResourceStream);
			manifestResourceStream = assembly.GetManifestResourceStream(name + imagesSubPath + ".CollapseHot.png");
			collapseHot = new Bitmap(manifestResourceStream);
			manifestResourceStream = assembly.GetManifestResourceStream(name + imagesSubPath + ".Expand.png");
			expand = new Bitmap(manifestResourceStream);
			manifestResourceStream = assembly.GetManifestResourceStream(name + imagesSubPath + ".ExpandHot.png");
			expandHot = new Bitmap(manifestResourceStream);
			manifestResourceStream = assembly.GetManifestResourceStream(name + imagesSubPath + ".GroupHeader.png");
			groupHeader = new Bitmap(manifestResourceStream);
			groupHeader.MakeTransparent(Color.Fuchsia);
		}
	}

	private class BlueImages : XPImages
	{
		internal override string GetImagesSubPath()
		{
			return ".Images.Blue";
		}
	}

	private class HomeSteadImages : XPImages
	{
		internal override string GetImagesSubPath()
		{
			return ".Images.HomeStead";
		}
	}

	private class MetallicImages : XPImages
	{
		internal override string GetImagesSubPath()
		{
			return ".Images.Metallic";
		}
	}

	private class XPTheme
	{
		private string explorerBarTheme = "explorerbar";

		private string editTheme = "edit";

		private string currentThemeName = string.Empty;

		private const int EBP_BACKGROUND = 0;

		private const int EBP_NORMALGROUPBACKGROUND = 5;

		private const int EBP_NORMALGROUPCOLLAPSE = 6;

		private const int EBP_NORMALGROUPEXPAND = 7;

		private const int EBP_NORMALGROUP = 8;

		private const int EBNG_NORMAL = 1;

		private const int EBNG_HOT = 2;

		private const int TS_DRAW = 2;

		private const int TMT_TEXTCOLOR = 3803;

		private const int EP_EDITTEXT = 1;

		private const int ETS_NORMAL = 1;

		private IntPtr handle = IntPtr.Zero;

		internal bool themesAvailable;

		private Hashtable themeNames;

		private static XPImages images;

		public string GetCurrentThemeName
		{
			get
			{
				int num = 260;
				IntPtr intPtr = Marshal.AllocHGlobal(num + 1);
				IntPtr intPtr2 = Marshal.AllocHGlobal(num + 1);
				try
				{
					Win32.GetCurrentThemeName(intPtr, num, intPtr2, num, IntPtr.Zero, IntPtr.Zero);
					return Marshal.PtrToStringAuto(intPtr2);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
			}
		}

		public XPImages Images
		{
			get
			{
				InitImages();
				return images;
			}
		}

		public XPTheme()
		{
			themeNames = new Hashtable();
		}

		public void Dispose()
		{
			DoneImages();
		}

		public void DoneImages()
		{
			images.Dispose();
			images = null;
		}

		private void InitImages()
		{
			if (images == null)
			{
				switch (currentTheme)
				{
				case XPThemeName.None:
					images = new BlueImages();
					break;
				case XPThemeName.Blue:
					images = new BlueImages();
					break;
				case XPThemeName.HomeStead:
					images = new HomeSteadImages();
					break;
				case XPThemeName.Metallic:
					images = new MetallicImages();
					break;
				}
				if (images != null)
				{
					images.LoadImages();
				}
			}
		}

		private void CheckThemes()
		{
			themesAvailable = ((FeatureSupport)OSFeature.Feature).GetVersionPresent(OSFeature.Themes) != null && Win32.IsAppThemed() && Win32.IsThemeActive() && (Win32.GetThemeAppProperties() & 2) == 2;
		}

		public void Close()
		{
			currentThemeName = string.Empty;
			themeNames.Clear();
			if (handle != IntPtr.Zero)
			{
				Win32.CloseThemeData(handle);
			}
			DoneImages();
		}

		public void DrawGroupBackground(IPainter painter, Rectangle rect)
		{
			OpenTheme(explorerBarTheme);
			DrawThemeBackground(painter, 5, 0, rect);
		}

		public void DrawGroupHeaderSymbol(IPainter painter, Rectangle rect, bool collapse, bool hot)
		{
			int partID = ((!collapse) ? 7 : 6);
			int stateID = ((!hot) ? 1 : 2);
			OpenTheme(explorerBarTheme);
			DrawThemeBackground(painter, partID, stateID, rect);
		}

		public void DrawHeaderBackground(IPainter painter, Rectangle rect)
		{
			OpenTheme(explorerBarTheme);
			DrawThemeBackground(painter, 8, 0, rect);
		}

		public void DrawOutbarBackground(IPainter painter, Rectangle rect)
		{
			OpenTheme(explorerBarTheme);
			DrawThemeBackground(painter, 0, 0, rect);
		}

		public void DrawThemeBackground(IPainter painter, int partID, int stateID, Rectangle rect)
		{
			painter.DrawThemeBackground(handle, partID, stateID, rect);
		}

		public void DrawThemeBackground(IntPtr dc, int partID, int stateID, Rectangle rect)
		{
			Win32.GdiRect rect2 = new Win32.GdiRect(rect);
			Win32.DrawThemeBackground(handle, dc, partID, stateID, ref rect2, IntPtr.Zero);
		}

		public void OpenTheme(string name)
		{
			if (!(currentThemeName == name))
			{
				object obj = themeNames[name];
				if (obj != null)
				{
					handle = (IntPtr)obj;
				}
				else
				{
					handle = Win32.OpenThemeData(IntPtr.Zero, name);
					themeNames.Add(name, handle);
				}
				currentThemeName = name;
			}
		}

		public void DrawEditBorder(IPainter painter, Rectangle rect)
		{
			if (themesAvailable)
			{
				OpenTheme(editTheme);
				DrawThemeBackground(painter, 1, 1, rect);
			}
		}

		public void DrawEditBorder(IntPtr dc, Rectangle rect)
		{
			if (themesAvailable)
			{
				OpenTheme(editTheme);
				DrawThemeBackground(dc, 1, 1, rect);
			}
		}

		public Color GetGroupCaptionColor(bool hot)
		{
			Win32.GetThemeColor(handle, 8, 0, 3803, out var colorRef);
			return colorRef.Color;
		}

		public void Open(string name)
		{
			CheckThemes();
			if (themesAvailable && name != string.Empty)
			{
				handle = Win32.OpenThemeData(IntPtr.Zero, name);
			}
		}
	}

	private static XPThemeName currentTheme;

	private static XPTheme theme;

	private static Point offset;

	public static Point Offset
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
		}
	}

	public static XPThemeName CurrentTheme => currentTheme;

	static XPThemes()
	{
		currentTheme = XPThemeName.None;
		theme = new XPTheme();
		InitTheme();
	}

	private static void DetectCurrentTheme()
	{
		string getCurrentThemeName = theme.GetCurrentThemeName;
		if (getCurrentThemeName == null || getCurrentThemeName.Length == 0)
		{
			currentTheme = XPThemeName.Blue;
			return;
		}
		switch (getCurrentThemeName)
		{
		case "NormalColor":
			currentTheme = XPThemeName.Blue;
			break;
		case "HomeStead":
			currentTheme = XPThemeName.HomeStead;
			break;
		case "Metallic":
			currentTheme = XPThemeName.Metallic;
			break;
		default:
			currentTheme = XPThemeName.Custom;
			break;
		}
	}

	public static Color GetGroupCaptionColor(bool hot)
	{
		switch (currentTheme)
		{
		case XPThemeName.None:
		case XPThemeName.Blue:
			if (hot)
			{
				return Consts.DefaultXPItemsForeColor;
			}
			return Consts.DefaultXPHeaderForeColor;
		case XPThemeName.HomeStead:
			if (hot)
			{
				return Color.FromArgb(114, 146, 29);
			}
			return Color.FromArgb(86, 102, 45);
		case XPThemeName.Metallic:
			if (hot)
			{
				return Color.FromArgb(126, 124, 124);
			}
			return Color.FromArgb(63, 61, 61);
		default:
			return theme.GetGroupCaptionColor(hot);
		}
	}

	public static Color GetGroupItemCaptionColor(bool hot)
	{
		return GetGroupCaptionColor(hot);
	}

	public static void DrawHeaderButton(IPainter painter, Rectangle rect, bool collapse, bool hot)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		if (painter == null)
		{
			return;
		}
		if (currentTheme != 0)
		{
			theme.DrawGroupHeaderSymbol(painter, DrawRect(rect), collapse, hot);
			return;
		}
		Bitmap val = (collapse ? ((!hot) ? theme.Images.Collapse : theme.Images.CollapseHot) : ((!hot) ? theme.Images.Expand : theme.Images.ExpandHot));
		ImageList val2 = new ImageList();
		val2.Images.Add((Image)(object)val);
		try
		{
			painter.DrawImage(val2, 0, rect);
		}
		finally
		{
			((Component)(object)val2).Dispose();
		}
	}

	public static void DrawHeaderBackground(IPainter painter, Rectangle rect)
	{
		if (painter != null)
		{
			if (currentTheme != 0)
			{
				theme.DrawHeaderBackground(painter, rect);
				return;
			}
			Rectangle imageRect = new Rectangle(new Point(0, 0), ((Image)theme.Images.GroupHeader).Size);
			Rectangle stretchRect = new Rectangle(imageRect.Location, imageRect.Size);
			stretchRect.Inflate(-2, 0);
			painter.StretchDrawImage(rect, stretchRect, imageRect, theme.Images.GroupHeader);
		}
	}

	public static void DrawGroupBackground(IPainter painter, Rectangle rect)
	{
		if (painter != null)
		{
			switch (currentTheme)
			{
			case XPThemeName.None:
				painter.FillRectangle(Consts.BlueItemsAreaColor, rect);
				break;
			case XPThemeName.HomeStead:
				painter.FillRectangle(Consts.HomeSteadItemsAreaColor, rect);
				break;
			case XPThemeName.Metallic:
				painter.FillRectangle(Consts.MetallicItemsAreaColor, rect);
				break;
			default:
				theme.DrawGroupBackground(painter, DrawRect(rect));
				break;
			}
		}
	}

	public static void DrawBackground(IPainter painter, Rectangle rect, Color startColor, Color endColor)
	{
		if (painter != null)
		{
			if (currentTheme == XPThemeName.None)
			{
				painter.FillGradient(rect, startColor, endColor, rect.Location, new Point(0, rect.Height));
			}
			else
			{
				theme.DrawOutbarBackground(painter, DrawRect(rect));
			}
		}
	}

	public static Rectangle DrawRect(Rectangle sourceRect)
	{
		return new Rectangle(sourceRect.X + offset.X, sourceRect.Y + offset.Y, sourceRect.Width, sourceRect.Height);
	}

	public static void DoneTheme()
	{
		theme.Close();
	}

	public static void InitTheme()
	{
		theme.Open(string.Empty);
		if (theme.themesAvailable)
		{
			DetectCurrentTheme();
		}
		else
		{
			currentTheme = XPThemeName.None;
		}
	}

	public static void DrawEditBorder(IPainter painter, Rectangle rect)
	{
		theme.DrawEditBorder(painter, rect);
	}

	public static void DrawEditBorder(IntPtr dc, Rectangle rect)
	{
		theme.DrawEditBorder(dc, rect);
	}
}
