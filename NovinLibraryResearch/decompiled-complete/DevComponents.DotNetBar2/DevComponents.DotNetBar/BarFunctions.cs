using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class BarFunctions
{
	private class _23D3 : Form
	{
		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 132)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
			}
			else
			{
				((Form)this).WndProc(ref m);
			}
		}
	}

	public const int ANIMATION_INTERVAL = 100;

	private const string _0946 = ".Strings";

	private static string _0947;

	private static bool _0948;

	private static bool _0949;

	private static bool __094A;

	private static bool __094B;

	private static bool __094C;

	private static bool _094D;

	private static bool __094E;

	private static Size __094F;

	private static List<ঊ> ॐ;

	public static Size CaptionButtonSize
	{
		get
		{
			if (__094C)
			{
				return Dpi.Size(__094F);
			}
			return SystemInformation.CaptionButtonSize;
		}
	}

	public static Size Windows10CaptionButtonSize
	{
		get
		{
			return __094F;
		}
		set
		{
			__094F = value;
		}
	}

	public static bool IsWindowsXP => __094E;

	public static bool IsVista => _0949;

	public static bool IsWindows7 => __094A;

	public static bool IsWindows8 => __094B;

	public static bool IsWindows10 => __094C;

	public static bool ThemedOS
	{
		get
		{
			return _0948;
		}
		set
		{
			_0948 = value;
		}
	}

	public static bool SupportsAnimation => _094D;

	internal static ঊ _0944
	{
		get
		{
			if (ॐ.Count == 0)
			{
				RefreshScreens();
			}
			foreach (ঊ item in ॐ)
			{
				if (item._098D)
				{
					return item;
				}
			}
			if (ॐ.Count > 0)
			{
				return ॐ[0];
			}
			return new ঊ(new Rectangle(0, 0, 1024, 768), new Rectangle(0, 0, 1024, 768), primary: true);
		}
	}

	internal static _0883 _0945
	{
		get
		{
			_0883 result = _0883._0884;
			if (ThemedOS && _089F.न >= 16)
			{
				if (_0949)
				{
					result = _0883._0885;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(49, 106, 197).ToArgb())
				{
					result = _0883._0885;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
				{
					result = _0883._0887;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
				{
					result = _0883._0886;
				}
			}
			return result;
		}
	}

	static BarFunctions()
	{
		_0947 = "";
		_0948 = false;
		_0949 = false;
		__094A = false;
		__094B = false;
		__094C = false;
		_094D = true;
		__094E = false;
		__094F = new Size(46, 28);
		ॐ = new List<ঊ>(2);
		_0948 = false;
		_089F.OSVERSIONINFO o = new _089F.OSVERSIONINFO
		{
			dwOSVersionInfoSize = Marshal.SizeOf(typeof(_089F.OSVERSIONINFO))
		};
		_089F.GetVersionEx(ref o);
		if (o.dwPlatformId == 2 && o.dwMajorVersion == 4)
		{
			_094D = false;
		}
		if ((o.dwMajorVersion == 5 && o.dwMinorVersion >= 1 && o.dwPlatformId == 2) || (o.dwMajorVersion > 5 && o.dwPlatformId == 2))
		{
			_0948 = ((FeatureSupport)OSFeature.Feature).IsPresent(OSFeature.Themes);
		}
		Version version = Environment.OSVersion.Version;
		__094E = version.Major <= 5;
		_0949 = version.Major >= 6;
		__094A = ((version.Major == 6 && version.Minor >= 1) || version.Major > 6) && version.Build >= 7000;
		__094B = ((version.Major == 6 && version.Minor >= 2) || version.Major > 6) && version.Build >= 9200;
		__094C = ((version.Major == 6 && version.Minor >= 3) || version.Major > 6) && version.Build >= 9600;
		RefreshScreens();
	}

	internal static char _0951(int _0952)
	{
		byte[] array = new byte[2];
		try
		{
			byte[] array2 = new byte[256];
			if (_089F.GetKeyboardState(array2) && _089F.ToAscii((uint)_0952, 0u, array2, array, 0u) != 0)
			{
				return (char)array[0];
			}
		}
		catch (Exception)
		{
			return '\0';
		}
		return '\0';
	}

	public static Color Darken(Color color, int percent)
	{
		ColorFunctions.HLSColor clr = ColorFunctions.RGBToHSL(color.R, color.G, color.B);
		clr.Lightness *= (double)(100 - percent) / 100.0;
		return ColorFunctions.HLSToRGB(clr);
	}

	public static Color Ligten(Color color, int percent)
	{
		ColorFunctions.HLSColor clr = ColorFunctions.RGBToHSL(color.R, color.G, color.B);
		clr.Lightness *= 1.0 + (double)percent / 100.0;
		return ColorFunctions.HLSToRGB(clr);
	}

	public static bool InvokeRecalcLayout(Control c, bool invalidate)
	{
		if (c is ItemControl)
		{
			((ItemControl)(object)c).RecalcLayout();
			return true;
		}
		if (c is Bar)
		{
			((Bar)(object)c).RecalcLayout();
			return true;
		}
		if (c is ExplorerBar)
		{
			((ExplorerBar)(object)c).RecalcLayout();
			return true;
		}
		if (c is BaseItemControl)
		{
			((BaseItemControl)(object)c).RecalcLayout();
			return true;
		}
		if (c is BarBaseControl)
		{
			((BarBaseControl)(object)c).RecalcLayout();
			return true;
		}
		if (c is PopupItemControl)
		{
			((PopupItemControl)(object)c).RecalcLayout();
			return true;
		}
		MethodInfo method = ((object)c).GetType().GetMethod("RecalcLayout");
		if ((object)method != null)
		{
			method.Invoke(c, null);
			return true;
		}
		if (invalidate)
		{
			c.Invalidate(true);
			c.Update();
		}
		return false;
	}

	public static StringFormat CreateStringFormat()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		StringFormat val = new StringFormat();
		val.Alignment = (StringAlignment)0;
		val.LineAlignment = (StringAlignment)0;
		val.HotkeyPrefix = (HotkeyPrefix)0;
		val.Trimming = (StringTrimming)1;
		return val;
	}

	public static void SetControlVisible(Control c, bool visible)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (visible)
		{
			int num = -1;
			if (c.Parent != null && (int)c.Dock != 0)
			{
				num = c.Parent.Controls.IndexOf(c);
			}
			c.Visible = true;
			if (num != -1)
			{
				c.Parent.Controls.SetChildIndex(c, num);
			}
		}
		else
		{
			c.Visible = false;
		}
	}

	public static bool ProcessItemsShortcuts(eShortcut key, Hashtable itemsShortcuts)
	{
		bool result = false;
		if (itemsShortcuts.Contains(key))
		{
			ᯣ ᯣ2 = (ᯣ)itemsShortcuts[key];
			BaseItem[] array = new BaseItem[ᯣ2.ᯥ.Values.Count];
			ᯣ2.ᯥ.Values.CopyTo(array, 0);
			Hashtable hashtable = new Hashtable(array.Length);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				if (baseItem.CanRaiseClick && (baseItem.Name == "" || !hashtable.Contains(baseItem.Name)) && (!baseItem.GlobalItem || baseItem.GlobalName == "" || !hashtable.Contains(baseItem.GlobalName)))
				{
					result = true;
					baseItem.RaiseClick(eEventSource.Keyboard);
					if (baseItem.Name != "")
					{
						hashtable.Add(baseItem.Name, "");
					}
					if (baseItem.GlobalItem && baseItem.GlobalName != "" && baseItem.GlobalName != baseItem.Name && !hashtable.Contains(baseItem.GlobalName))
					{
						hashtable.Add(baseItem.GlobalName, "");
					}
				}
			}
		}
		return result;
	}

	public static Bar CreateDuplicateDockBar(Bar instance)
	{
		Bar bar = new Bar(((Control)instance).Text);
		return CreateDuplicateDockBar(instance, bar);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Bar CreateDuplicateDockBar(Bar instance, Bar bar)
	{
		((Control)bar).Text = ((Control)instance).Text;
		bar.ItemsContainer._0B03 = instance._0B03;
		bar.ItemsContainer.ර = instance.ItemsContainer.ර;
		bar.CanDockBottom = instance.CanDockBottom;
		bar.CanDockLeft = instance.CanDockLeft;
		bar.CanDockRight = instance.CanDockRight;
		bar.CanDockTop = instance.CanDockTop;
		bar.CanDockDocument = instance.CanDockDocument;
		bar.CanDockTab = instance.CanDockTab;
		bar.CanUndock = instance.CanUndock;
		bar.CanMaximizeFloating = instance.CanMaximizeFloating;
		bar.CanAutoHide = instance.CanAutoHide;
		bar.DockTabAlignment = instance.DockTabAlignment;
		bar.CanCustomize = instance.CanCustomize;
		bar.AutoHideAnimationTime = instance.AutoHideAnimationTime;
		bar.AlwaysDisplayDockTab = instance.AlwaysDisplayDockTab;
		bar.AutoCreateCaptionMenu = instance.AutoCreateCaptionMenu;
		bar.AutoSyncBarCaption = instance.AutoSyncBarCaption;
		bar.HideFloatingInactive = instance.HideFloatingInactive;
		bar.CloseSingleTab = instance.CloseSingleTab;
		bar.DockTabCloseButtonVisible = instance.DockTabCloseButtonVisible;
		bar.CaptionHeight = instance.CaptionHeight;
		bar.TabNavigation = instance.TabNavigation;
		if (!instance.CaptionBackColor.IsEmpty)
		{
			bar.CaptionBackColor = instance.CaptionBackColor;
		}
		if (!instance.CaptionForeColor.IsEmpty)
		{
			bar.CaptionForeColor = instance.CaptionForeColor;
		}
		if (!instance.ItemsContainer.m_BackgroundColor.IsEmpty)
		{
			bar.ItemsContainer.BackColor = instance.ItemsContainer.m_BackgroundColor;
		}
		if (instance.DockedBorderStyle != 0)
		{
			bar.DockedBorderStyle = instance.DockedBorderStyle;
		}
		bar.Style = instance.Style;
		if (instance.ColorScheme.SchemeChanged)
		{
			bar.ColorScheme = instance.ColorScheme;
		}
		bar.LayoutType = instance.LayoutType;
		bar.GrabHandleStyle = instance.GrabHandleStyle;
		bar.Stretch = instance.Stretch;
		bar.CanHide = instance.CanHide;
		bar.ThemeAware = instance.ThemeAware;
		bar.DockedBorderStyle = instance.DockedBorderStyle;
		return bar;
	}

	public static void ApplyAutoDocumentBarStyle(Bar bar)
	{
		bar._0C54(bar.Style);
		bar.TabNavigation = true;
		if (!bar.AlwaysDisplayDockTab)
		{
			bar.AlwaysDisplayDockTab = true;
		}
		if (bar.DockTabAlignment != eTabStripAlignment.Top)
		{
			bar.DockTabAlignment = eTabStripAlignment.Top;
		}
		if (bar.GrabHandleStyle != 0)
		{
			bar.GrabHandleStyle = eGrabHandleStyle.None;
		}
	}

	public static void RestoreAutoDocumentBarStyle(Bar bar)
	{
		bar._0C54(bar.Style);
		if (bar.AlwaysDisplayDockTab)
		{
			bar.AlwaysDisplayDockTab = false;
		}
		if (bar.DockTabAlignment != eTabStripAlignment.Bottom)
		{
			bar.DockTabAlignment = eTabStripAlignment.Bottom;
		}
		if (bar.GrabHandleStyle != eGrabHandleStyle.Caption)
		{
			bar.GrabHandleStyle = eGrabHandleStyle.Caption;
		}
	}

	public static bool IsHandleValid(Control objCtrl)
	{
		if (objCtrl != null && !objCtrl.Disposing && !objCtrl.IsDisposed)
		{
			return objCtrl.IsHandleCreated;
		}
		return false;
	}

	public static void DrawMenuCheckBox(ItemPaintArgs pa, Rectangle r, eDotNetBarStyle style, bool MouseOver)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (style != eDotNetBarStyle.Office2000)
		{
			Color itemCheckedBackground;
			if (!MouseOver)
			{
				itemCheckedBackground = pa.Colors.ItemCheckedBackground;
				SolidBrush val = new SolidBrush(itemCheckedBackground);
				graphics.FillRectangle((Brush)(object)val, r);
				((Brush)val).Dispose();
			}
			itemCheckedBackground = pa.Colors.ItemCheckedBorder;
			Pen val2 = new Pen(itemCheckedBackground, 1f);
			_089F.र(graphics, val2, r);
			val2.Dispose();
			Point[] array = new Point[3];
			array[0].X = r.Left + (r.Width - 5) / 2 - 1;
			array[0].Y = r.Top + (r.Height - 6) / 2 + 3;
			array[1].X = array[0].X + 2;
			array[1].Y = array[0].Y + 2;
			array[2].X = array[1].X + 4;
			array[2].Y = array[1].Y - 4;
			val2 = new Pen(pa.Colors.ItemCheckedText);
			graphics.DrawLines(val2, array);
			array[0].X++;
			array[1].X++;
			array[2].X++;
			graphics.DrawLines(val2, array);
			val2.Dispose();
		}
		else if (style == eDotNetBarStyle.Office2000)
		{
			ControlPaint.DrawBorder3D(graphics, r, (Border3DStyle)2, (Border3DSide)2063);
			if (!MouseOver)
			{
				r.Inflate(-1, -1);
				graphics.FillRectangle((Brush)(object)ColorFunctions.GetPushedBrush(), r);
			}
			Point[] array2 = new Point[3];
			array2[0].X = r.Left + (r.Width - 6) / 2;
			array2[0].Y = r.Top + (r.Height - 6) / 2 + 3;
			array2[1].X = array2[0].X + 2;
			array2[1].Y = array2[0].Y + 2;
			array2[2].X = array2[1].X + 4;
			array2[2].Y = array2[1].Y - 4;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X++;
			array2[1].X++;
			array2[2].X++;
			graphics.DrawLines(SystemPens.ControlText, array2);
		}
	}

	public static void SerializeImage(Image image, XmlElement xml)
	{
		if (image != null)
		{
			MemoryStream memoryStream = new MemoryStream(1024);
			image.Save((Stream)memoryStream, ImageFormat.Png);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter w = new StringWriter(stringBuilder);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xmlTextWriter.WriteBase64(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			xml.InnerText = stringBuilder.ToString();
		}
	}

	public static void SerializeIcon(Icon icon, XmlElement xml)
	{
		if (icon != null)
		{
			MemoryStream memoryStream = new MemoryStream(1024);
			icon.Save((Stream)memoryStream);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter w = new StringWriter(stringBuilder);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xml.SetAttribute("encoding", "binhex");
			xmlTextWriter.WriteBinHex(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			xml.InnerText = stringBuilder.ToString();
		}
	}

	public static Form CreateOutlineForm()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Form val = new Form();
		try
		{
			val.Size = new Size(0, 0);
		}
		catch
		{
			val = new Form();
		}
		((Control)val).BackColor = SystemColors.Highlight;
		val.MinimizeBox = false;
		val.MaximizeBox = false;
		val.FormBorderStyle = (FormBorderStyle)0;
		if (_089F.ࢤ)
		{
			val.Opacity = 0.5;
		}
		else
		{
			((Control)val).BackColor = ControlPaint.LightLight(SystemColors.Highlight);
		}
		val.ShowInTaskbar = false;
		((Control)val).Text = "";
		((Control)val).CreateControl();
		return val;
	}

	public static Form CreateTransparentOutlineForm()
	{
		Form val = (Form)(object)new _23D3();
		try
		{
			val.Size = new Size(0, 0);
		}
		catch
		{
			val = (Form)(object)new _23D3();
		}
		((Control)val).BackColor = SystemColors.Highlight;
		val.MinimizeBox = false;
		val.MaximizeBox = false;
		val.FormBorderStyle = (FormBorderStyle)0;
		if (_089F.ࢤ)
		{
			val.Opacity = 0.5;
		}
		else
		{
			((Control)val).BackColor = ControlPaint.LightLight(SystemColors.Highlight);
		}
		val.ShowInTaskbar = false;
		((Control)val).Text = "";
		((Control)val).CreateControl();
		return val;
	}

	public static Image DeserializeImage(XmlElement xml)
	{
		Image val = null;
		if (xml == null || xml.InnerText == "")
		{
			return null;
		}
		StringReader input = new StringReader(xml.OuterXml);
		XmlTextReader xmlTextReader = new XmlTextReader(input);
		MemoryStream memoryStream = new MemoryStream(1024);
		xmlTextReader.Read();
		byte[] array = new byte[1024];
		int num = 0;
		do
		{
			num = xmlTextReader.ReadBase64(array, 0, 1024);
			if (num > 0)
			{
				memoryStream.Write(array, 0, num);
			}
		}
		while (num != 0);
		return Image.FromStream((Stream)memoryStream);
	}

	public static Icon DeserializeIcon(XmlElement xml)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		Icon val = null;
		if (xml == null || xml.InnerText == "")
		{
			return null;
		}
		bool flag = false;
		if (xml.HasAttribute("encoding") && xml.GetAttribute("encoding") == "binhex")
		{
			flag = true;
		}
		StringReader input = new StringReader(xml.OuterXml);
		XmlTextReader xmlTextReader = new XmlTextReader(input);
		MemoryStream memoryStream = new MemoryStream(1024);
		xmlTextReader.Read();
		byte[] array = new byte[1024];
		int num = 0;
		if (flag)
		{
			do
			{
				num = xmlTextReader.ReadBinHex(array, 0, 1024);
				if (num > 0)
				{
					memoryStream.Write(array, 0, num);
				}
			}
			while (num != 0);
		}
		else
		{
			do
			{
				num = xmlTextReader.ReadBase64(array, 0, 1024);
				if (num > 0)
				{
					memoryStream.Write(array, 0, num);
				}
			}
			while (num != 0);
		}
		memoryStream.Position = 0L;
		return new Icon((Stream)memoryStream);
	}

	internal static BaseItem _0953(XmlElement _0954)
	{
		string attribute = _0954.GetAttribute("class");
		BaseItem baseItem = null;
		switch (attribute)
		{
		case "DevComponents.DotNetBar.ButtonItem":
			return new ButtonItem();
		case "DevComponents.DotNetBar.TextBoxItem":
			return new TextBoxItem();
		case "DevComponents.DotNetBar.ComboBoxItem":
			return new ComboBoxItem();
		case "DevComponents.DotNetBar.LabelItem":
			return new LabelItem();
		case "DevComponents.DotNetBar.CustomizeItem":
			return new CustomizeItem();
		case "DevComponents.DotNetBar.ControlContainerItem":
			return new ControlContainerItem();
		case "DevComponents.DotNetBar.DockContainerItem":
			return new DockContainerItem();
		case "DevComponents.DotNetBar.MdiWindowListItem":
			return new MdiWindowListItem();
		case "DevComponents.DotNetBar.SideBarContainerItem":
			return new SideBarContainerItem();
		case "DevComponents.DotNetBar.SideBarPanelItem":
			return new SideBarPanelItem();
		case "DevComponents.DotNetBar.ExplorerBarGroupItem":
			return new ExplorerBarGroupItem();
		case "DevComponents.DotNetBar.ExplorerBarContainerItem":
			return new ExplorerBarContainerItem();
		case "DevComponents.DotNetBar.ProgressBarItem":
			return new ProgressBarItem();
		case "DevComponents.DotNetBar.ColorPickerDropDown":
			return new ColorPickerDropDown();
		default:
			try
			{
				Assembly assembly = Assembly.Load(_0954.GetAttribute("assembly"));
				if ((object)assembly == null)
				{
					return null;
				}
				return assembly.CreateInstance(_0954.GetAttribute("class")) as BaseItem;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Could not create item from XML. Assembly=" + _0954.GetAttribute("assembly") + ", Class=" + _0954.GetAttribute("class") + ", Inner Exception: " + ex.Message + ", Source=" + ex.Source);
			}
		}
	}

	internal static BaseItem _0953(XmlElement _0954, IDesignerHost _0955, string _0956)
	{
		string attribute = _0954.GetAttribute("class");
		BaseItem baseItem = null;
		switch (attribute)
		{
		case "DevComponents.DotNetBar.ButtonItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ButtonItem), _0956) as ButtonItem;
			}
			return _0955.CreateComponent(typeof(ButtonItem)) as ButtonItem;
		case "DevComponents.DotNetBar.TextBoxItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(TextBoxItem), _0956) as TextBoxItem;
			}
			return _0955.CreateComponent(typeof(TextBoxItem)) as TextBoxItem;
		case "DevComponents.DotNetBar.ComboBoxItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ComboBoxItem), _0956) as ComboBoxItem;
			}
			return _0955.CreateComponent(typeof(ComboBoxItem)) as ComboBoxItem;
		case "DevComponents.DotNetBar.LabelItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(LabelItem), _0956) as LabelItem;
			}
			return _0955.CreateComponent(typeof(LabelItem)) as LabelItem;
		case "DevComponents.DotNetBar.CustomizeItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(CustomizeItem), _0956) as CustomizeItem;
			}
			return _0955.CreateComponent(typeof(CustomizeItem)) as CustomizeItem;
		case "DevComponents.DotNetBar.ControlContainerItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ControlContainerItem), _0956) as ControlContainerItem;
			}
			return _0955.CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
		case "DevComponents.DotNetBar.DockContainerItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(DockContainerItem), _0956) as DockContainerItem;
			}
			return _0955.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
		case "DevComponents.DotNetBar.MdiWindowListItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(MdiWindowListItem), _0956) as MdiWindowListItem;
			}
			return _0955.CreateComponent(typeof(MdiWindowListItem)) as MdiWindowListItem;
		case "DevComponents.DotNetBar.SideBarContainerItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(SideBarContainerItem), _0956) as SideBarContainerItem;
			}
			return _0955.CreateComponent(typeof(SideBarContainerItem)) as SideBarContainerItem;
		case "DevComponents.DotNetBar.SideBarPanelItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(SideBarPanelItem), _0956) as SideBarPanelItem;
			}
			return _0955.CreateComponent(typeof(SideBarPanelItem)) as SideBarPanelItem;
		case "DevComponents.DotNetBar.ExplorerBarGroupItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ExplorerBarGroupItem), _0956) as ExplorerBarGroupItem;
			}
			return _0955.CreateComponent(typeof(ExplorerBarGroupItem)) as ExplorerBarGroupItem;
		case "DevComponents.DotNetBar.ExplorerBarContainerItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ExplorerBarContainerItem), _0956) as ExplorerBarContainerItem;
			}
			return _0955.CreateComponent(typeof(ExplorerBarContainerItem)) as ExplorerBarContainerItem;
		case "DevComponents.DotNetBar.ProgressBarItem":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ProgressBarItem), _0956) as ProgressBarItem;
			}
			return _0955.CreateComponent(typeof(ProgressBarItem)) as ProgressBarItem;
		case "DevComponents.DotNetBar.ColorPickerDropDown":
			if (_0956 != "")
			{
				return _0955.CreateComponent(typeof(ColorPickerDropDown), _0956) as ColorPickerDropDown;
			}
			return _0955.CreateComponent(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
		default:
			try
			{
				Assembly assembly = Assembly.Load(_0954.GetAttribute("assembly"));
				if ((object)assembly == null)
				{
					return null;
				}
				BaseItem baseItem2 = assembly.CreateInstance(_0954.GetAttribute("class")) as BaseItem;
				return _0955.CreateComponent(baseItem2.GetType()) as BaseItem;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Could not create item from XML. Assembly=" + _0954.GetAttribute("assembly") + ", Class=" + _0954.GetAttribute("class") + ", Inner Exception: " + ex.Message + ", Source=" + ex.Source);
			}
		}
	}

	internal static string _0957(XmlElement _0954)
	{
		string text = "";
		if (_0954.HasAttribute("assembly"))
		{
			text += _0954.GetAttribute("assembly");
		}
		if (_0954.HasAttribute("class"))
		{
			text += _0954.GetAttribute("class");
		}
		return text;
	}

	internal static void क़(Graphics _٥, _1886 ख़, Rectangle ल, bool ग़, bool ज़, bool ड़)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		if (ग़)
		{
			_٥.FillRectangle((Brush)new SolidBrush(ColorFunctions.PressedBackColor(_٥)), ल);
			_089F.र(_٥, SystemPens.Highlight, ल);
		}
		else if (ज़)
		{
			_٥.FillRectangle((Brush)new SolidBrush(ColorFunctions.HoverBackColor(_٥)), ल);
			_089F.र(_٥, SystemPens.Highlight, ल);
		}
		Bitmap val = new Bitmap(ल.Width, ल.Height, _٥);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		Rectangle rectangle = new Rectangle(0, 0, ल.Width, ल.Height);
		rectangle.Inflate(0, -1);
		Rectangle clip = rectangle;
		clip.Inflate(-1, -1);
		SolidBrush val3 = new SolidBrush(SystemColors.Control);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, ल.Width, ल.Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val2.SetClip(clip);
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)ख़, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		val.MakeTransparent(SystemColors.Control);
		if (ड़)
		{
			float[][] array = new float[5][];
			float[] array2 = new float[5];
			array[0] = array2;
			float[] array3 = new float[5];
			array[1] = array3;
			float[] array4 = new float[5];
			array[2] = array4;
			array[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
			float[] array5 = new float[5];
			array[4] = array5;
			ColorMatrix colorMatrix = new ColorMatrix(array);
			ImageAttributes val4 = new ImageAttributes();
			val4.ClearColorKey();
			val4.SetColorMatrix(colorMatrix);
			_٥.DrawImage((Image)(object)val, ल, 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val4);
		}
		else
		{
			if (ग़)
			{
				ल.Offset(1, 1);
			}
			_٥.DrawImageUnscaled((Image)(object)val, ल);
		}
	}

	internal static void ढ़(BaseItem _06EC, string फ़)
	{
		if (_06EC.GlobalName.Length > 0)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(_06EC)[फ़];
			_0965(य़(_06EC), _06EC.GetType(), _06EC.GlobalName, propertyDescriptor, propertyDescriptor.GetValue(_06EC));
		}
		else if (_06EC.Name.Length > 0)
		{
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(_06EC)[फ़];
			ॠ(य़(_06EC), _06EC.GetType(), _06EC.Name, propertyDescriptor2, propertyDescriptor2.GetValue(_06EC));
		}
	}

	private static object य़(BaseItem _06EC)
	{
		object obj = _06EC.GetOwner();
		if (obj is RibbonBar && ((RibbonBar)obj)._10FB)
		{
			obj = ((!((RibbonBar)obj).ჵ) ? ((RibbonBar)obj).ჼ : ((RibbonBar)obj).ჴ.GetOwner());
		}
		return obj;
	}

	internal static void ॠ(object ॡ, Type _0962, string _0963, PropertyDescriptor _0964, object ԝ)
	{
		IOwner owner = ॡ as IOwner;
		DotNetBarManager dotNetBarManager = ॡ as DotNetBarManager;
		if (owner == null || _0963 == "" || _0964 == null)
		{
			return;
		}
		ArrayList arrayList = null;
		if (dotNetBarManager != null)
		{
			if (!dotNetBarManager.IsDisposed)
			{
				arrayList = dotNetBarManager.GetItems(_0963, _0962, fullSearch: true);
			}
		}
		else
		{
			arrayList = owner.GetItems(_0963, _0962);
		}
		if (arrayList == null)
		{
			return;
		}
		foreach (BaseItem item in arrayList)
		{
			object value = _0964.GetValue(item);
			if (value != ԝ && (value == null || !value.Equals(ԝ)))
			{
				_0964.SetValue(item, ԝ);
			}
		}
	}

	internal static void _0965(object ॡ, Type _0962, string _0963, PropertyDescriptor _0964, object ԝ)
	{
		IOwner owner = ॡ as IOwner;
		DotNetBarManager dotNetBarManager = ॡ as DotNetBarManager;
		if (owner == null || _0963 == "" || _0964 == null)
		{
			return;
		}
		ArrayList arrayList = null;
		if (dotNetBarManager != null)
		{
			if (!dotNetBarManager.IsDisposed)
			{
				arrayList = dotNetBarManager.GetItems(_0963, _0962, fullSearch: true, useGlobalName: true);
			}
		}
		else
		{
			arrayList = owner.GetItems(_0963, _0962, useGlobalName: true);
		}
		if (arrayList == null)
		{
			return;
		}
		foreach (BaseItem item in arrayList)
		{
			if (_0964.GetValue(item) != ԝ)
			{
				_0964.SetValue(item, ԝ);
			}
		}
	}

	internal static ResourceManager _०(bool _१)
	{
		string text = ".Strings";
		text = ((!(Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute) || !(dotNetBarResourcesAttribute.NamespacePrefix != "")) ? ("DevComponents.DotNetBar" + text) : (dotNetBarResourcesAttribute.NamespacePrefix + text));
		return new ResourceManager(text, Assembly.GetExecutingAssembly());
	}

	internal static ResourceManager _०()
	{
		string text = ".Strings";
		text = ((!(Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute) || !(dotNetBarResourcesAttribute.NamespacePrefix != "")) ? ("DevComponents.DotNetBar" + text) : (dotNetBarResourcesAttribute.NamespacePrefix + text));
		if (_0947 == "")
		{
			CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			executingAssembly.GetManifestResourceNames();
			int num = 0;
			while (cultureInfo.LCID != 127 && num < 16)
			{
				if (executingAssembly.GetManifestResourceInfo(text + "_" + cultureInfo.Name.ToLower() + ".resources") != null)
				{
					_0947 = text + "_" + cultureInfo.Name.ToLower();
					break;
				}
				if (executingAssembly.GetManifestResourceInfo(text + "_" + cultureInfo.TwoLetterISOLanguageName.ToLower() + ".resources") != null)
				{
					_0947 = text + "_" + cultureInfo.TwoLetterISOLanguageName.ToLower();
					break;
				}
				cultureInfo = cultureInfo.Parent;
				num++;
			}
			if (_0947 == "")
			{
				_0947 = text;
			}
		}
		return new ResourceManager(_0947, Assembly.GetExecutingAssembly());
	}

	internal static void _२(Graphics _٥, eBorderType _३, Rectangle ल, Color _४)
	{
		_२(_٥, _३, ल, _४, eBorderSide.All);
	}

	internal static void _२(Graphics _٥, eBorderType _३, Rectangle ल, Color _४, eBorderSide _५)
	{
		_२(_٥, _३, ल, _४, _५, (DashStyle)0);
	}

	internal static void _२(Graphics _٥, eBorderType _३, Rectangle ल, Color _४, eBorderSide _५, DashStyle _६)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_२(_٥, _३, ल, _४, _५, _६, 1);
	}

	internal static void _२(Graphics _٥, eBorderType _३, Rectangle ल, Color _४, eBorderSide _५, DashStyle _६, int _७)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		if (_७ <= 0)
		{
			return;
		}
		Border3DSide val = ((_५ != eBorderSide.All) ? ((Border3DSide)((((_५ | eBorderSide.Left) != 0) ? 1 : 0) | (((_५ | eBorderSide.Right) != 0) ? 4 : 0) | (((_५ | eBorderSide.Top) != 0) ? 2 : 0) | (((_५ | eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063));
		switch (_३)
		{
		case eBorderType.Bump:
			ControlPaint.DrawBorder3D(_٥, ल, (Border3DStyle)9, val);
			break;
		case eBorderType.Etched:
			ControlPaint.DrawBorder3D(_٥, ल, (Border3DStyle)6, val);
			break;
		case eBorderType.Raised:
			ControlPaint.DrawBorder3D(_٥, ल, (Border3DStyle)4, val);
			break;
		case eBorderType.Sunken:
			ControlPaint.DrawBorder3D(_٥, ल, (Border3DStyle)2, val);
			break;
		case eBorderType.SingleLine:
		{
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)3;
			Pen val3 = new Pen(_४, (float)_७);
			try
			{
				val3.DashStyle = _६;
				int num = _७ / 2;
				if ((_५ & eBorderSide.Left) != 0)
				{
					_٥.DrawLine(val3, ल.X + num, ल.Y, ल.X + num, ल.Bottom - ((_७ <= 1) ? 1 : 0));
				}
				if ((_५ & eBorderSide.Top) != 0)
				{
					_٥.DrawLine(val3, ल.X, ल.Y + num, ल.Right - 1, ल.Y + num);
				}
				if (num == 0)
				{
					num = 1;
				}
				if ((_५ & eBorderSide.Right) != 0)
				{
					_٥.DrawLine(val3, ल.Right - num, ल.Y, ल.Right - num, ल.Bottom - ((_७ <= 1) ? 1 : 0));
				}
				if ((_५ & eBorderSide.Bottom) != 0)
				{
					_٥.DrawLine(val3, ल.X, ल.Bottom - num, ल.Right - 1, ल.Bottom - num);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode;
			break;
		}
		case eBorderType.DoubleLine:
		{
			Pen val2 = new Pen(_४, (float)_७);
			try
			{
				val2.DashStyle = _६;
				for (int i = 0; i < _७ + 1; i += _७)
				{
					if ((_५ & eBorderSide.Left) != 0)
					{
						_٥.DrawLine(val2, ल.X, ल.Y, ल.X, ल.Bottom - 1);
					}
					if ((_५ & eBorderSide.Top) != 0)
					{
						_٥.DrawLine(val2, ल.X, ल.Y, ल.Right - 1, ल.Y);
					}
					if ((_५ & eBorderSide.Right) != 0)
					{
						_٥.DrawLine(val2, ल.Right - 1, ल.Y, ल.Right - 1, ल.Bottom - 1);
					}
					if ((_५ & eBorderSide.Bottom) != 0)
					{
						_٥.DrawLine(val2, ल.X, ल.Bottom - 1, ल.Right - 1, ल.Bottom - 1);
					}
					ल.Inflate(-1, -1);
				}
				break;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		}
	}

	internal static void _८(Graphics _٥, int و, int ى, int _९, int _0970, Border3DStyle _0657, Border3DSide _५)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, و, ى, _९, _0970, _0657, _५, SystemColors.Control, ॳ: true);
	}

	internal static void _८(Graphics _٥, int و, int ى, int _९, int _0970, Border3DStyle _0657, Color ॱ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, و, ى, _९, _0970, _0657, (Border3DSide)2063, ॱ, ॳ: true);
	}

	internal static void _८(Graphics _٥, int و, int ى, int _९, int _0970, Border3DStyle _0657)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, و, ى, _९, _0970, _0657, (Border3DSide)2063, SystemColors.Control, ॳ: true);
	}

	internal static void _८(Graphics _٥, Rectangle ल, Border3DStyle _0657, Border3DSide _५, Color ॲ)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, ल.X, ल.Y, ल.Width, ल.Height, _0657, _५, ॲ, ॳ: true);
	}

	internal static void _८(Graphics _٥, Rectangle ल, Border3DStyle _0657, Border3DSide _५, Color ॲ, bool ॳ)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, ल.X, ल.Y, ल.Width, ल.Height, _0657, _५, ॲ, ॳ);
	}

	internal static void _८(Graphics _٥, Rectangle ल, Border3DStyle _0657)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, ल.X, ल.Y, ल.Width, ल.Height, _0657, (Border3DSide)2063, SystemColors.Control, ॳ: true);
	}

	internal static void _८(Graphics _٥, int و, int ى, int _९, int _0970, Border3DStyle _0657, Border3DSide _५, Color ॲ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_८(_٥, و, ى, _९, _0970, _0657, _५, ॲ, ॳ: true);
	}

	internal static void _८(Graphics _٥, int و, int ى, int _९, int _0970, Border3DStyle _0657, Border3DSide _५, Color ॲ, bool ॳ)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected I4, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		if (ॳ)
		{
			_٥.FillRectangle((Brush)new SolidBrush(ॲ), و, ى, _९, _0970);
		}
		Color color = ControlPaint.Light(ॲ);
		Color color2 = ControlPaint.Dark(ॲ);
		Pen val = null;
		Pen val2 = null;
		Pen val3 = new Pen(ॲ);
		_0970--;
		_९--;
		switch (_0657 - 2)
		{
		default:
			if ((int)_0657 == 10)
			{
				val2 = new Pen(ControlPaint.DarkDark(ॲ), 1f);
				val = new Pen(color2, 1f);
				if ((_५ & 2) != 0)
				{
					_٥.DrawLine(val2, و, ى, و + _९, ى);
					_٥.DrawLine(val, و + 1, ى + 1, و + _९ - 2, ى + 1);
				}
				if ((_५ & 1) != 0)
				{
					_٥.DrawLine(val2, و, ى, و, ى + _0970);
					_٥.DrawLine(val, و + 1, ى + 1, و + 1, ى + _0970 - 2);
				}
				val2.Dispose();
				val.Dispose();
				val2 = new Pen(color, 1f);
				val = new Pen(ControlPaint.LightLight(ॲ), 1f);
				if ((_५ & 4) != 0)
				{
					_٥.DrawLine(val2, و + _९, ى, و + _९, ى + _0970);
					_٥.DrawLine(val, و + _९ - 1, ى + 1, و + _९ - 1, ى + _0970 - 1);
				}
				if ((_५ & 8) != 0)
				{
					_٥.DrawLine(val2, و, ى + _0970, و + _९, ى + _0970);
					_٥.DrawLine(val, و + 1, ى + _0970 - 1, و + _९ - 1, ى + _0970 - 1);
				}
			}
			break;
		case 2:
			val = new Pen(color, 1f);
			val2 = new Pen(color2, 1f);
			if ((_५ & 2) != 0)
			{
				_٥.DrawLine(val, و, ى, و + _९, ى);
			}
			if ((_५ & 1) != 0)
			{
				_٥.DrawLine(val, و, ى, و, ى + _0970);
			}
			if ((_५ & 4) != 0)
			{
				_٥.DrawLine(val2, و + _९, ى, و + _९, ى + _0970);
			}
			if ((_५ & 8) != 0)
			{
				_٥.DrawLine(val2, و, ى + _0970, و + _९, ى + _0970);
			}
			break;
		case 0:
			val = new Pen(color, 1f);
			val2 = new Pen(color2, 1f);
			if ((_५ & 2) != 0)
			{
				_٥.DrawLine(val2, و, ى, و + _९, ى);
			}
			if ((_५ & 1) != 0)
			{
				_٥.DrawLine(val2, و, ى, و, ى + _0970);
			}
			if ((_५ & 4) != 0)
			{
				_٥.DrawLine(val, و + _९, ى, و + _९, ى + _0970);
			}
			if ((_५ & 8) != 0)
			{
				_٥.DrawLine(val, و, ى + _0970, و + _९, ى + _0970);
			}
			break;
		case 3:
			if ((_५ & 2) != 0 && (_५ & 1) != 0 && (_५ & 4) != 0 && (_५ & 8) != 0)
			{
				val = new Pen(Color.White, 1f);
				_٥.DrawRectangle(val3, و, ى, _९, _0970);
				_٥.DrawLine(val, و + 1, ى + 1, و + _९ - 1, ى + 1);
				_٥.DrawLine(val, و + 1, ى + 1, و + 1, ى + _0970 - 1);
				break;
			}
			val2 = new Pen(color, 1f);
			val = new Pen(ControlPaint.LightLight(ॲ), 1f);
			if ((_५ & 2) != 0)
			{
				_٥.DrawLine(val2, و, ى, و + _९, ى);
				_٥.DrawLine(val, و + 1, ى + 1, و + _९ - 2, ى + 1);
			}
			if ((_५ & 1) != 0)
			{
				_٥.DrawLine(val2, و, ى, و, ى + _0970);
				_٥.DrawLine(val, و + 1, ى + 1, و + 1, ى + _0970 - 2);
			}
			val2.Dispose();
			val.Dispose();
			val2 = new Pen(ControlPaint.DarkDark(ॲ), 1f);
			val = new Pen(color2, 1f);
			if ((_५ & 4) != 0)
			{
				_٥.DrawLine(val2, و + _९, ى, و + _९, ى + _0970);
				_٥.DrawLine(val, و + _९ - 1, ى + 1, و + _९ - 1, ى + _0970 - 1);
			}
			if ((_५ & 8) != 0)
			{
				_٥.DrawLine(val2, و, ى + _0970, و + _९, ى + _0970);
				_٥.DrawLine(val, و + 1, ى + _0970 - 1, و + _९ - 1, ى + _0970 - 1);
			}
			break;
		case 1:
			break;
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
		val3.Dispose();
	}

	internal static LinearGradientBrush ॴ(Rectangle ल, Color ॵ, Color ॶ, float ॷ)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		if (ल.Width <= 0)
		{
			ल.Width = 1;
		}
		if (ल.Height <= 0)
		{
			ल.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(ल.X, ल.Y - 1, ल.Width, ल.Height + 1), ॵ, ॶ, ॷ);
	}

	internal static LinearGradientBrush ॴ(RectangleF ल, Color ॵ, Color ॶ, float ॷ)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (ल.Width <= 0f)
		{
			ल.Width = 1f;
		}
		if (ल.Height <= 0f)
		{
			ल.Height = 1f;
		}
		return new LinearGradientBrush(new RectangleF(ल.X, ल.Y - 1f, ल.Width, ल.Height + 1f), ॵ, ॶ, ॷ);
	}

	internal static LinearGradientBrush ॴ(Rectangle ल, Color ॵ, Color ॶ, float ॷ, bool ॸ)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (ल.Width <= 0)
		{
			ल.Width = 1;
		}
		if (ल.Height <= 0)
		{
			ल.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(ल.X, ल.Y - 1, ल.Width, ल.Height + 1), ॵ, ॶ, ॷ, ॸ);
	}

	public static void DrawDesignTimeSelection(Graphics g, Rectangle r, Color backColor, Color border, int penWidth)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		if (!backColor.IsEmpty && backColor != Color.Transparent)
		{
			border = ((!((double)backColor.GetBrightness() < 0.5)) ? ControlPaint.Dark(backColor) : ControlPaint.Light(backColor));
		}
		Pen val = new Pen(border, (float)penWidth);
		try
		{
			val.DashStyle = (DashStyle)1;
			r.Width--;
			r.Height--;
			g.DrawRectangle(val, r);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static BaseItem GetSubItemByName(BaseItem objParent, string ItemName)
	{
		return GetSubItemByName(objParent, ItemName, useGlobalName: false);
	}

	public static BaseItem GetSubItemByName(BaseItem objParent, string ItemName, bool useGlobalName)
	{
		if (objParent == null)
		{
			return null;
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if ((useGlobalName && subItem.GlobalName == ItemName) || (!useGlobalName && subItem.Name == ItemName))
			{
				return subItem;
			}
			if (subItem.SubItems.Count > 0)
			{
				BaseItem subItemByName = GetSubItemByName(subItem, ItemName, useGlobalName);
				if (subItemByName != null)
				{
					return subItemByName;
				}
			}
		}
		if (objParent is GalleryContainer)
		{
			return GetSubItemByName(((GalleryContainer)objParent).PopupGalleryItem, ItemName, useGlobalName);
		}
		return null;
	}

	public static void GetSubItemsByName(BaseItem objParent, string ItemName, ArrayList list)
	{
		GetSubItemsByName(objParent, ItemName, list, useGlobalName: false);
	}

	public static void GetSubItemsByName(BaseItem objParent, string ItemName, ArrayList list, bool useGlobalName)
	{
		if (objParent == null)
		{
			return;
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if ((useGlobalName && subItem.GlobalName == ItemName) || (!useGlobalName && subItem.Name == ItemName))
			{
				list.Add(subItem);
			}
			else if (subItem is ControlContainerItem)
			{
				ControlContainerItem controlContainerItem = subItem as ControlContainerItem;
				if (controlContainerItem.Control is RibbonBar)
				{
					RibbonBar ribbonBar = controlContainerItem.Control as RibbonBar;
					ArrayList items = ribbonBar.GetItems(ItemName);
					list.AddRange(items);
				}
			}
			if (subItem.SubItems.Count > 0)
			{
				GetSubItemsByName(subItem, ItemName, list, useGlobalName);
			}
		}
		if (objParent is GalleryContainer)
		{
			GetSubItemsByName(((GalleryContainer)objParent).PopupGalleryItem, ItemName, list, useGlobalName);
		}
	}

	public static void GetSubItemsByNameAndType(BaseItem objParent, string ItemName, ArrayList list, Type itemType)
	{
		GetSubItemsByNameAndType(objParent, ItemName, list, itemType, useGlobalName: false);
	}

	public static void GetSubItemsByNameAndType(BaseItem objParent, string ItemName, ArrayList list, Type itemType, bool useGlobalName)
	{
		if (objParent == null)
		{
			return;
		}
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if ((object)subItem.GetType() == itemType && ((useGlobalName && subItem.GlobalName == ItemName) || (!useGlobalName && subItem.Name == ItemName)))
			{
				list.Add(subItem);
			}
			else if (subItem is ControlContainerItem)
			{
				ControlContainerItem controlContainerItem = subItem as ControlContainerItem;
				if (controlContainerItem.Control is RibbonBar)
				{
					RibbonBar ribbonBar = controlContainerItem.Control as RibbonBar;
					ArrayList items = ribbonBar.GetItems(ItemName, itemType, useGlobalName);
					list.AddRange(items);
				}
			}
			if (subItem.SubItems.Count > 0)
			{
				GetSubItemsByNameAndType(subItem, ItemName, list, itemType, useGlobalName);
			}
		}
	}

	public static string ColorToString(Color clr)
	{
		if (clr.IsSystemColor)
		{
			return "." + clr.Name;
		}
		return clr.ToArgb().ToString();
	}

	public static Color ColorFromString(string sclr)
	{
		if (sclr == "")
		{
			return Color.Empty;
		}
		if (sclr[0] == '.')
		{
			return Color.FromName(sclr.Substring(1));
		}
		return Color.FromArgb(XmlConvert.ToInt32(sclr));
	}

	internal static ঊ ॹ(Point ॺ)
	{
		if (ॐ.Count == 0)
		{
			RefreshScreens();
		}
		foreach (ঊ item in ॐ)
		{
			if (item.ঋ.Contains(ॺ))
			{
				return item;
			}
		}
		Screen val = Screen.FromPoint(ॺ);
		if (val != null && val.Bounds.Contains(ॺ))
		{
			return new ঊ(val.Bounds, val.WorkingArea);
		}
		return null;
	}

	internal static ঊ ॻ(Control ॼ)
	{
		Rectangle rect;
		if (ॼ.Parent != null)
		{
			Point location = ॼ.PointToScreen(Point.Empty);
			rect = new Rectangle(location, ॼ.Size);
		}
		else
		{
			rect = new Rectangle(ॼ.Location, ॼ.Size);
		}
		if (ॐ.Count == 0)
		{
			RefreshScreens();
		}
		foreach (ঊ item in ॐ)
		{
			if (item.ঋ.Contains(rect))
			{
				return item;
			}
		}
		Screen val = Screen.FromControl(ॼ);
		if (val != null)
		{
			return new ঊ(val.Bounds, val.WorkingArea);
		}
		return null;
	}

	public static void RefreshScreens()
	{
		ॐ.Clear();
		Screen[] allScreens = Screen.AllScreens;
		foreach (Screen val in allScreens)
		{
			ॐ.Add(new ঊ(val.Bounds, val.WorkingArea, val.Primary));
		}
	}

	public static void SetExplorerBarStyle(ExplorerBar bar, eExplorerBarStockStyle stockStyle)
	{
		switch (stockStyle)
		{
		case eExplorerBarStockStyle.SystemColors:
			bar.BackStyle.Reset();
			bar.BackStyle.BackColorSchemePart = eColorSchemePart.ExplorerBarBackground;
			bar.BackStyle.BackColor2SchemePart = eColorSchemePart.ExplorerBarBackground2;
			bar.BackStyle.BackColorGradientAngle = bar.ColorScheme.ExplorerBarBackgroundGradientAngle;
			return;
		case eExplorerBarStockStyle.Custom:
			return;
		}
		ePredefinedColorScheme predefinedColorScheme = ePredefinedColorScheme.Blue2003;
		switch (stockStyle)
		{
		case eExplorerBarStockStyle.Silver:
		case eExplorerBarStockStyle.SilverSpecial:
			predefinedColorScheme = ePredefinedColorScheme.Silver2003;
			break;
		case eExplorerBarStockStyle.OliveGreen:
		case eExplorerBarStockStyle.OliveGreenSpecial:
			predefinedColorScheme = ePredefinedColorScheme.OliveGreen2003;
			break;
		}
		ColorScheme colorScheme = new ColorScheme(eDotNetBarStyle.Office2003);
		colorScheme.PredefinedColorScheme = predefinedColorScheme;
		bar.BackStyle.Reset();
		bar.BackStyle.BackColor = colorScheme.ExplorerBarBackground;
		bar.BackStyle.BackColor2 = colorScheme.ExplorerBarBackground2;
		bar.BackStyle.BackColorGradientAngle = colorScheme.ExplorerBarBackgroundGradientAngle;
	}

	public static void SetExplorerBarStyle(ExplorerBarGroupItem group, eExplorerBarStockStyle stockStyle)
	{
		if (stockStyle == eExplorerBarStockStyle.SystemColors)
		{
			eExplorerBarStockStyle eExplorerBarStockStyle2 = eExplorerBarStockStyle.Blue;
			eExplorerBarStockStyle eExplorerBarStockStyle3 = eExplorerBarStockStyle.BlueSpecial;
			if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
			{
				eExplorerBarStockStyle2 = eExplorerBarStockStyle.Silver;
				eExplorerBarStockStyle3 = eExplorerBarStockStyle.SilverSpecial;
			}
			else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
			{
				eExplorerBarStockStyle2 = eExplorerBarStockStyle.OliveGreen;
				eExplorerBarStockStyle3 = eExplorerBarStockStyle.OliveGreenSpecial;
			}
			stockStyle = ((!group.XPSpecialGroup) ? eExplorerBarStockStyle2 : eExplorerBarStockStyle3);
		}
		if (stockStyle != eExplorerBarStockStyle.Custom)
		{
			group.TitleStyle.Reset();
			group.TitleHotStyle.Reset();
			group.BackStyle.Reset();
			group.TitleStyle.CornerTypeTopLeft = eCornerType.Rounded;
			group.TitleStyle.CornerTypeTopRight = eCornerType.Rounded;
			group.TitleStyle.CornerDiameter = 3;
			group.TitleHotStyle.CornerTypeTopLeft = eCornerType.Rounded;
			group.TitleHotStyle.CornerTypeTopRight = eCornerType.Rounded;
			group.TitleHotStyle.CornerDiameter = 3;
		}
		switch (stockStyle)
		{
		case eExplorerBarStockStyle.Blue:
			group.TitleStyle.BackColor = Color.White;
			group.TitleStyle.BackColor2 = Color.FromArgb(199, 211, 247);
			group.TitleStyle.TextColor = Color.FromArgb(33, 93, 198);
			group.TitleHotStyle.TextColor = Color.FromArgb(66, 142, 255);
			group.TitleHotStyle.BackColor = Color.White;
			group.TitleHotStyle.BackColor2 = Color.FromArgb(199, 211, 247);
			group.BackStyle.BackColor = Color.FromArgb(214, 223, 247);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.White;
			group.ExpandBorderColor = Color.FromArgb(174, 182, 216);
			group.ExpandForeColor = Color.FromArgb(0, 60, 165);
			group.ExpandHotBackColor = Color.White;
			group.ExpandHotBorderColor = Color.FromArgb(174, 182, 216);
			group.ExpandHotForeColor = Color.FromArgb(66, 142, 255);
			break;
		case eExplorerBarStockStyle.BlueSpecial:
			group.TitleStyle.BackColor = Color.FromArgb(0, 73, 181);
			group.TitleStyle.BackColor2 = Color.FromArgb(41, 93, 206);
			group.TitleStyle.TextColor = Color.White;
			group.TitleHotStyle.TextColor = Color.FromArgb(66, 142, 255);
			group.TitleHotStyle.BackColor = Color.FromArgb(0, 73, 181);
			group.TitleHotStyle.BackColor2 = Color.FromArgb(41, 93, 206);
			group.BackStyle.Reset();
			group.BackStyle.BackColor = Color.FromArgb(239, 243, 255);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.FromArgb(48, 97, 196);
			group.ExpandBorderColor = Color.FromArgb(123, 168, 229);
			group.ExpandForeColor = Color.White;
			group.ExpandHotBackColor = Color.FromArgb(48, 97, 196);
			group.ExpandHotBorderColor = Color.FromArgb(123, 168, 229);
			group.ExpandHotForeColor = Color.FromArgb(172, 205, 255);
			break;
		case eExplorerBarStockStyle.OliveGreen:
			group.TitleStyle.BackColor = Color.FromArgb(255, 252, 236);
			group.TitleStyle.BackColor2 = Color.FromArgb(224, 231, 184);
			group.TitleStyle.TextColor = Color.FromArgb(86, 102, 45);
			group.TitleHotStyle.TextColor = Color.FromArgb(114, 146, 29);
			group.TitleHotStyle.BackColor = Color.FromArgb(255, 252, 236);
			group.TitleHotStyle.BackColor2 = Color.FromArgb(224, 231, 184);
			group.BackStyle.Reset();
			group.BackStyle.BackColor = Color.FromArgb(246, 246, 236);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.FromArgb(254, 254, 253);
			group.ExpandBorderColor = Color.FromArgb(194, 206, 185);
			group.ExpandForeColor = Color.FromArgb(75, 103, 28);
			group.ExpandHotBackColor = Color.FromArgb(254, 254, 253);
			group.ExpandHotBorderColor = Color.FromArgb(194, 206, 185);
			group.ExpandHotForeColor = Color.FromArgb(114, 146, 29);
			break;
		case eExplorerBarStockStyle.OliveGreenSpecial:
			group.TitleStyle.BackColor = Color.FromArgb(119, 140, 64);
			group.TitleStyle.BackColor2 = Color.FromArgb(150, 168, 103);
			group.TitleStyle.TextColor = Color.White;
			group.TitleHotStyle.TextColor = Color.FromArgb(224, 231, 184);
			group.TitleHotStyle.BackColor = Color.FromArgb(119, 140, 64);
			group.TitleHotStyle.BackColor2 = Color.FromArgb(150, 168, 103);
			group.BackStyle.Reset();
			group.BackStyle.BackColor = Color.FromArgb(246, 246, 236);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.FromArgb(129, 163, 79);
			group.ExpandBorderColor = Color.FromArgb(191, 205, 156);
			group.ExpandForeColor = Color.White;
			group.ExpandHotBackColor = Color.FromArgb(130, 164, 80);
			group.ExpandHotBorderColor = Color.FromArgb(182, 202, 139);
			group.ExpandHotForeColor = Color.FromArgb(221, 237, 190);
			break;
		case eExplorerBarStockStyle.Silver:
			group.TitleStyle.BackColor = Color.White;
			group.TitleStyle.BackColor2 = Color.FromArgb(214, 215, 224);
			group.TitleStyle.TextColor = Color.FromArgb(63, 61, 61);
			group.TitleHotStyle.TextColor = Color.FromArgb(126, 124, 124);
			group.TitleHotStyle.BackColor = Color.White;
			group.TitleHotStyle.BackColor2 = Color.FromArgb(214, 215, 224);
			group.BackStyle.Reset();
			group.BackStyle.BackColor = Color.FromArgb(240, 241, 245);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.White;
			group.ExpandBorderColor = Color.FromArgb(188, 189, 203);
			group.ExpandForeColor = Color.FromArgb(49, 68, 115);
			group.ExpandHotBackColor = Color.White;
			group.ExpandHotBorderColor = Color.FromArgb(194, 195, 208);
			group.ExpandHotForeColor = Color.FromArgb(126, 124, 124);
			break;
		case eExplorerBarStockStyle.SilverSpecial:
			group.TitleStyle.BackColor = Color.FromArgb(119, 119, 146);
			group.TitleStyle.BackColor2 = Color.FromArgb(180, 182, 199);
			group.TitleStyle.TextColor = Color.White;
			group.TitleHotStyle.BackColor = Color.FromArgb(119, 119, 146);
			group.TitleHotStyle.BackColor2 = Color.FromArgb(180, 182, 199);
			group.TitleHotStyle.TextColor = Color.FromArgb(230, 230, 230);
			group.BackStyle.Reset();
			group.BackStyle.BackColor = Color.FromArgb(240, 241, 245);
			group.BackStyle.BorderBottom = eStyleBorderType.Solid;
			group.BackStyle.BorderTop = eStyleBorderType.None;
			group.BackStyle.BorderLeft = eStyleBorderType.Solid;
			group.BackStyle.BorderRight = eStyleBorderType.Solid;
			group.BackStyle.BorderBottomWidth = 1;
			group.BackStyle.BorderTopWidth = 0;
			group.BackStyle.BorderLeftWidth = 1;
			group.BackStyle.BorderRightWidth = 1;
			group.BackStyle.BorderColor = Color.White;
			group.ExpandBackColor = Color.FromArgb(111, 117, 151);
			group.ExpandBorderColor = Color.FromArgb(196, 203, 224);
			group.ExpandForeColor = Color.White;
			group.ExpandHotBackColor = Color.FromArgb(111, 117, 151);
			group.ExpandHotBorderColor = Color.FromArgb(196, 203, 224);
			group.ExpandHotForeColor = Color.White;
			break;
		}
	}

	public static void SetExplorerBarStyle(ButtonItem item, eExplorerBarStockStyle stockStyle)
	{
		if (stockStyle == eExplorerBarStockStyle.SystemColors)
		{
			eExplorerBarStockStyle eExplorerBarStockStyle2 = eExplorerBarStockStyle.Blue;
			if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
			{
				eExplorerBarStockStyle2 = eExplorerBarStockStyle.Silver;
			}
			else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
			{
				eExplorerBarStockStyle2 = eExplorerBarStockStyle.OliveGreen;
			}
			stockStyle = eExplorerBarStockStyle2;
		}
		switch (stockStyle)
		{
		case eExplorerBarStockStyle.Blue:
		case eExplorerBarStockStyle.BlueSpecial:
			item.ForeColor = Color.FromArgb(33, 93, 198);
			item.HotForeColor = Color.FromArgb(66, 142, 255);
			break;
		case eExplorerBarStockStyle.OliveGreen:
		case eExplorerBarStockStyle.OliveGreenSpecial:
			item.ForeColor = Color.FromArgb(86, 102, 45);
			item.HotForeColor = Color.FromArgb(114, 146, 29);
			break;
		case eExplorerBarStockStyle.Silver:
		case eExplorerBarStockStyle.SilverSpecial:
			item.ForeColor = Color.FromArgb(63, 61, 61);
			item.HotForeColor = Color.FromArgb(126, 124, 124);
			break;
		default:
			item.ForeColor = SystemColors.ControlText;
			item.HotForeColor = SystemColors.ControlDark;
			break;
		}
	}

	public static MdiClient GetMdiClient(Form MdiForm)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (!MdiForm.IsMdiContainer)
		{
			return null;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)MdiForm).Controls)
		{
			Control val = item;
			if (val is MdiClient)
			{
				return (MdiClient)(object)((val is MdiClient) ? val : null);
			}
		}
		return null;
	}

	internal static Icon ॽ(Icon ॾ)
	{
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			ॾ.Save((Stream)memoryStream);
			byte[] ঀ = memoryStream.ToArray();
			int num = ঀ[4];
			for (int i = 0; i < num; i++)
			{
				int num2 = 6 + i * 16;
				int num3 = ঀ[num2];
				int num4 = ঀ[num2 + 1];
				int num5 = ॿ(ref ঀ, num2 + 12);
				int num6 = ॿ(ref ঀ, num5);
				int num7 = _0981(ref ঀ, num5 + 12);
				int num8 = _0981(ref ঀ, num5 + 14);
				if (num7 != 1)
				{
					continue;
				}
				int num9 = ॿ(ref ঀ, num5 + 20);
				int num10 = 0;
				int num11 = num8;
				if (num11 != 8)
				{
					if (num11 != 24)
					{
						if (num11 != 32)
						{
							continue;
						}
						num10 = 4;
						num9 = num3 * num4 * 4;
					}
					else
					{
						num10 = 3;
						num9 = num3 * num4 * 3;
					}
				}
				else
				{
					num10 = 4;
					num9 = 1024;
				}
				int num12 = num5 + num6;
				for (int j = num12; j < num12 + num9; j += num10)
				{
					byte b = (byte)((ঀ[j] + 255) / 2);
					byte b2 = (byte)((ঀ[j + 1] + 255) / 2);
					byte b3 = (byte)((ঀ[j + 2] + 255) / 2);
					if (b != 127 || b2 != 127 || b3 != 127)
					{
						byte b4 = (ঀ[j + 1] = (ঀ[j] = (byte)((double)(int)b3 * 0.299 + (double)(int)b2 * 0.587 + (double)(int)b * 0.114)));
						ঀ[j + 2] = b4;
					}
				}
			}
			return new Icon((Stream)new MemoryStream(ঀ));
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static int ॿ(ref byte[] ঀ, int _066A)
	{
		return ঀ[_066A] + (ঀ[_066A + 1] << 8) + (ঀ[_066A + 2] << 16) + (ঀ[_066A + 3] << 24);
	}

	private static int _0981(ref byte[] ঀ, int _066A)
	{
		return ঀ[_066A] + (ঀ[_066A + 1] << 8);
	}

	internal static void _0982(Graphics _٥, Rectangle _0983, Image _0984, eStyleBackgroundImage অ, int আ)
	{
		_0982(_٥, _0983, _0984, (eBackgroundImagePosition)অ, আ);
	}

	internal static void _0982(Graphics _٥, Rectangle _0983, Image _0984, eBackgroundImagePosition অ, int আ)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		if (_0984 == null)
		{
			return;
		}
		Rectangle rectangle = _0983;
		ImageAttributes val = null;
		if (আ != 255)
		{
			float[][] array = new float[5][]
			{
				new float[5] { 1f, 0f, 0f, 0f, 0f },
				new float[5] { 0f, 1f, 0f, 0f, 0f },
				new float[5] { 0f, 0f, 1f, 0f, 0f },
				new float[5]
				{
					0f,
					0f,
					0f,
					(float)আ / 255f,
					0f
				},
				new float[5] { 0f, 0f, 0f, 0f, 1f }
			};
			ColorMatrix val2 = new ColorMatrix(array);
			val = new ImageAttributes();
			val.SetColorMatrix(val2, (ColorMatrixFlag)0, (ColorAdjustType)1);
		}
		switch (অ)
		{
		case eBackgroundImagePosition.Stretch:
			if (val != null)
			{
				_٥.DrawImage(_0984, rectangle, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2, val);
			}
			else
			{
				_٥.DrawImage(_0984, rectangle, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2);
			}
			break;
		case eBackgroundImagePosition.CenterLeft:
		case eBackgroundImagePosition.CenterRight:
		{
			Rectangle rectangle3 = new Rectangle(rectangle.X, rectangle.Y, Dpi.Width(_0984.Width), Dpi.Height(_0984.Height));
			if (rectangle.Width > rectangle3.Width && অ == eBackgroundImagePosition.CenterRight)
			{
				rectangle3.X += rectangle.Width - rectangle3.Width;
			}
			rectangle3.Y += (rectangle.Height - rectangle3.Height) / 2;
			if (val != null)
			{
				_٥.DrawImage(_0984, rectangle3, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2, val);
			}
			else
			{
				_٥.DrawImage(_0984, rectangle3, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.Center:
		{
			Rectangle rectangle5 = new Rectangle(rectangle.X, rectangle.Y, Dpi.Width(_0984.Width), Dpi.Height(_0984.Height));
			if (rectangle.Width > rectangle5.Width)
			{
				rectangle5.X += (rectangle.Width - rectangle5.Width) / 2;
			}
			if (rectangle.Height > rectangle5.Height)
			{
				rectangle5.Y += (rectangle.Height - rectangle5.Height) / 2;
			}
			if (val != null)
			{
				_٥.DrawImage(_0984, rectangle5, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2, val);
			}
			else
			{
				_٥.DrawImage(_0984, rectangle5, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.TopLeft:
		case eBackgroundImagePosition.TopRight:
		case eBackgroundImagePosition.BottomLeft:
		case eBackgroundImagePosition.BottomRight:
		{
			Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y, Dpi.Width(_0984.Width), Dpi.Height(_0984.Height));
			switch (অ)
			{
			case eBackgroundImagePosition.TopRight:
				rectangle4.X = rectangle.Right - rectangle4.Width;
				break;
			case eBackgroundImagePosition.BottomLeft:
				rectangle4.Y = rectangle.Bottom - rectangle4.Height;
				break;
			case eBackgroundImagePosition.BottomRight:
				rectangle4.Y = rectangle.Bottom - rectangle4.Height;
				rectangle4.X = rectangle.Right - rectangle4.Width;
				break;
			}
			if (val != null)
			{
				_٥.DrawImage(_0984, rectangle4, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2, val);
			}
			else
			{
				_٥.DrawImage(_0984, rectangle4, 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.Tile:
			if (val != null)
			{
				int num = Dpi.Width(_0984.Width);
				int num2 = Dpi.Height(_0984.Height);
				if (rectangle.Width > num || rectangle.Height > num2)
				{
					int i = rectangle.X;
					for (int j = rectangle.Y; j < rectangle.Bottom; j += num2)
					{
						for (; i < rectangle.Right; i += num)
						{
							Rectangle rectangle2 = new Rectangle(i, j, num, num2);
							if (rectangle2.Right > rectangle.Right)
							{
								rectangle2.Width -= rectangle2.Right - rectangle.Right;
							}
							if (rectangle2.Bottom > rectangle.Bottom)
							{
								rectangle2.Height -= rectangle2.Bottom - rectangle.Bottom;
							}
							_٥.DrawImage(_0984, rectangle2, 0, 0, num, num2, (GraphicsUnit)2, val);
						}
						i = rectangle.X;
					}
				}
				else
				{
					_٥.DrawImage(_0984, new Rectangle(0, 0, num, num2), 0, 0, _0984.Width, _0984.Height, (GraphicsUnit)2, val);
				}
			}
			else
			{
				SmoothingMode smoothingMode = _٥.SmoothingMode;
				_٥.SmoothingMode = (SmoothingMode)3;
				TextureBrush val3 = new TextureBrush(_0984);
				try
				{
					val3.WrapMode = (WrapMode)0;
					_٥.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				_٥.SmoothingMode = smoothingMode;
			}
			break;
		}
	}

	public static bool IsSystemKey(Keys key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if ((int)key == 107 || (int)key == 262144 || (int)key == 93 || (int)key == 246 || (int)key == 8 || (int)key == 27 || (int)key == 13 || ((int)key >= 112 && (int)key <= 130) || (int)key == 9)
		{
			return true;
		}
		return false;
	}

	public static bool IsFormActive(Form f)
	{
		if (f == null)
		{
			return false;
		}
		if (Form.ActiveForm == f)
		{
			if (f.IsMdiChild && f.MdiParent != null)
			{
				if (f.MdiParent.ActiveMdiChild == f)
				{
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public static void AnimateControl(Control control, bool show, int animationTime, Rectangle rectStart, Rectangle rectEnd)
	{
		control.Bounds = rectStart;
		if (!control.Visible)
		{
			control.Visible = true;
		}
		bool flag = false;
		if (animationTime <= 0)
		{
			flag = true;
		}
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, animationTime);
		int num3;
		int num2;
		int num;
		int num4 = (num3 = (num2 = (num = 0)));
		if (rectStart.Left == rectEnd.Left && rectStart.Top == rectEnd.Top && rectStart.Right == rectEnd.Right && rectStart.Height != rectEnd.Height)
		{
			num = ((rectEnd.Height > rectStart.Height) ? 1 : (-1));
		}
		else if (rectStart.Left == rectEnd.Left && rectStart.Top == rectEnd.Top && rectStart.Bottom == rectEnd.Bottom && rectStart.Width != rectEnd.Width)
		{
			num2 = ((rectEnd.Width > rectStart.Width) ? 1 : (-1));
		}
		else if (rectStart.Right == rectEnd.Right && rectStart.Top == rectEnd.Top && rectStart.Bottom == rectEnd.Bottom && rectStart.Width != rectEnd.Width)
		{
			num4 = ((rectEnd.Width <= rectStart.Width) ? 1 : (-1));
			num2 = ((rectEnd.Width > rectStart.Width) ? 1 : (-1));
		}
		else if (rectStart.Right == rectEnd.Right && rectStart.Left == rectEnd.Left && rectStart.Bottom == rectEnd.Bottom && rectStart.Height != rectEnd.Height)
		{
			num3 = ((rectEnd.Height <= rectStart.Height) ? 1 : (-1));
			num = ((rectEnd.Height > rectStart.Height) ? 1 : (-1));
		}
		else if (rectEnd.X != rectStart.X && rectEnd.Y == rectStart.Y && rectStart.Height == rectEnd.Height && rectEnd.Width == rectStart.Width)
		{
			num4 = ((rectEnd.X > rectStart.X) ? 1 : (-1));
		}
		else if (rectEnd.Y != rectStart.Y && rectEnd.X == rectStart.X && rectStart.Height == rectEnd.Height && rectEnd.Width == rectStart.Width)
		{
			num4 = ((rectEnd.Y > rectStart.Y) ? 1 : (-1));
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			control.Bounds = rectEnd;
		}
		else
		{
			int num5 = 1;
			int num6 = ((rectStart.Width != rectEnd.Width) ? Math.Abs(rectStart.Width - rectEnd.Width) : Math.Abs(rectStart.Height - rectEnd.Height));
			if (num6 == 0 && rectStart.Width == rectEnd.Width && rectStart.Height == rectEnd.Height)
			{
				if (rectEnd.X - rectStart.X != 0)
				{
					num6 = Math.Abs(rectStart.X - rectEnd.X);
				}
				else if (rectEnd.Y - rectStart.Y != 0)
				{
					num6 = Math.Abs(rectStart.Y - rectEnd.Y);
				}
			}
			int num7 = num6;
			DateTime now = DateTime.Now;
			Rectangle rectangle = rectStart;
			while (rectangle != rectEnd)
			{
				DateTime now2 = DateTime.Now;
				rectangle.X += num4 * num5;
				rectangle.Y += num3 * num5;
				rectangle.Width += num2 * num5;
				rectangle.Height += num * num5;
				if (Math.Sign(rectEnd.X - rectangle.X) != Math.Sign(num4))
				{
					rectangle.X = rectEnd.X;
				}
				if (Math.Sign(rectEnd.Y - rectangle.Y) != Math.Sign(num3))
				{
					rectangle.Y = rectEnd.Y;
				}
				if (Math.Sign(rectEnd.Width - rectangle.Width) != Math.Sign(num2))
				{
					rectangle.Width = rectEnd.Width;
				}
				if (Math.Sign(rectEnd.Height - rectangle.Height) != Math.Sign(num))
				{
					rectangle.Height = rectEnd.Height;
				}
				control.Bounds = rectangle;
				if (control.Parent != null)
				{
					control.Parent.Update();
				}
				else
				{
					control.Update();
				}
				num7 -= num5;
				do
				{
					DateTime now3 = DateTime.Now;
					TimeSpan timeSpan2 = now3 - now2;
					TimeSpan timeSpan3 = now3 - now;
					if ((timeSpan - timeSpan3).TotalMilliseconds <= 0.0)
					{
						num5 = num7;
						break;
					}
					if ((int)(timeSpan - timeSpan3).TotalMilliseconds == 0)
					{
						num5 = 1;
						continue;
					}
					try
					{
						num5 = num7 * (int)timeSpan2.TotalMilliseconds / (int)(timeSpan - timeSpan3).TotalMilliseconds;
					}
					catch
					{
					}
				}
				while (num5 < 1);
			}
		}
		if (!show)
		{
			control.Visible = false;
			control.Bounds = rectStart;
		}
	}

	internal static Bitmap ই(string ঈ)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + ঈ));
		}
		return new Bitmap(typeof(DotNetBarManager), ঈ);
	}

	internal static Icon উ(string ঈ)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + ঈ));
		}
		return new Icon(typeof(DotNetBarManager), ঈ);
	}

	public static Graphics CreateGraphics(Control objCtrl)
	{
		if (objCtrl is ItemControl)
		{
			return ((ItemControl)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is Bar)
		{
			return ((Bar)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is ExplorerBar)
		{
			return ((ExplorerBar)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is BaseItemControl)
		{
			return ((BaseItemControl)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is BarBaseControl)
		{
			return ((BarBaseControl)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is PanelControl)
		{
			return ((PanelControl)(object)objCtrl).CreateGraphics();
		}
		if (objCtrl is PopupItemControl)
		{
			return ((PopupItemControl)(object)objCtrl).CreateGraphics();
		}
		return objCtrl.CreateGraphics();
	}

	public static bool IsOffice2007Style(eDotNetBarStyle style)
	{
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		if (style != eDotNetBarStyle.Office2007 && style != eDotNetBarStyle.Office2010 && style != eDotNetBarStyle.Windows7)
		{
			return StyleManager.IsMetro(style);
		}
		return true;
	}

	public static bool IsOffice2010Style(eDotNetBarStyle style)
	{
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		if (style != eDotNetBarStyle.Office2010 && style != eDotNetBarStyle.Windows7)
		{
			return StyleManager.IsMetro(style);
		}
		return true;
	}

	public static bool IsOffice2007StyleOnly(eDotNetBarStyle style)
	{
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		return style == eDotNetBarStyle.Office2007;
	}
}
