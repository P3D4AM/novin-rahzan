using System;
using System.Collections;
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

namespace Stimulsoft.Controls.Win.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class BarFunctions
{
	public const int ANIMATION_INTERVAL = 100;

	private static string ms_ResourceName;

	private const string DEFAULT_RESOURCE = ".Strings";

	private static bool m_ThemedOS;

	private static bool m_IsVista;

	private static bool _IsWindows7;

	private static bool _IsWindows8;

	private static bool m_SupportsAnimation;

	private static bool _IsWindowsXP;

	private static ArrayList m_Screens;

	public static bool IsWindowsXP => _IsWindowsXP;

	public static bool IsVista => m_IsVista;

	public static bool IsWindows7 => _IsWindows7;

	public static bool IsWindows8 => _IsWindows8;

	public static bool ThemedOS
	{
		get
		{
			return m_ThemedOS;
		}
		set
		{
			m_ThemedOS = value;
		}
	}

	public static bool SupportsAnimation => m_SupportsAnimation;

	internal static eWinXPColorScheme WinXPColorScheme
	{
		get
		{
			eWinXPColorScheme result = eWinXPColorScheme.Undetermined;
			if (ThemedOS && NativeFunctions.ColorDepth >= 16)
			{
				if (m_IsVista)
				{
					result = eWinXPColorScheme.Blue;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(49, 106, 197).ToArgb())
				{
					result = eWinXPColorScheme.Blue;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
				{
					result = eWinXPColorScheme.Silver;
				}
				else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
				{
					result = eWinXPColorScheme.OliveGreen;
				}
			}
			return result;
		}
	}

	static BarFunctions()
	{
		ms_ResourceName = "";
		m_ThemedOS = false;
		m_IsVista = false;
		_IsWindows7 = false;
		_IsWindows8 = false;
		m_SupportsAnimation = true;
		_IsWindowsXP = false;
		m_Screens = new ArrayList(2);
		m_ThemedOS = false;
		NativeFunctions.OSVERSIONINFO o = new NativeFunctions.OSVERSIONINFO
		{
			dwOSVersionInfoSize = Marshal.SizeOf(typeof(NativeFunctions.OSVERSIONINFO))
		};
		NativeFunctions.GetVersionEx(ref o);
		if (o.dwPlatformId == 2 && o.dwMajorVersion == 4)
		{
			m_SupportsAnimation = false;
		}
		if ((o.dwMajorVersion == 5 && o.dwMinorVersion >= 1 && o.dwPlatformId == 2) || (o.dwMajorVersion > 5 && o.dwPlatformId == 2))
		{
			m_ThemedOS = ((FeatureSupport)OSFeature.Feature).IsPresent(OSFeature.Themes);
		}
		Version version = Environment.OSVersion.Version;
		_IsWindowsXP = version.Major <= 5;
		m_IsVista = version.Major >= 6;
		_IsWindows7 = version.Major >= 6 && version.Minor >= 1 && version.Build >= 7000;
		_IsWindows8 = version.Major >= 6 && version.Minor >= 2 && version.Build >= 9200;
		RefreshScreens();
	}

	internal static char GetCharForKeyValue(int keyValue)
	{
		byte[] array = new byte[2];
		try
		{
			byte[] array2 = new byte[256];
			if (NativeFunctions.GetKeyboardState(array2) && NativeFunctions.ToAscii((uint)keyValue, 0u, array2, array, 0u) != 0)
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
		if (method != null)
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
			ShortcutTableEntry shortcutTableEntry = (ShortcutTableEntry)itemsShortcuts[key];
			BaseItem[] array = new BaseItem[shortcutTableEntry.Items.Values.Count];
			shortcutTableEntry.Items.Values.CopyTo(array, 0);
			Hashtable hashtable = new Hashtable(array.Length);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				if (baseItem.CanRaiseClick && (baseItem.Name == "" || !hashtable.Contains(baseItem.Name)) && (!baseItem.GlobalItem || baseItem.GlobalName == "" || !hashtable.Contains(baseItem.GlobalName)))
				{
					result = true;
					baseItem.RaiseClick(eEventSource.Keyboard, key);
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
		bar.ItemsContainer.MinHeight = instance.MinHeight;
		bar.ItemsContainer.MinWidth = instance.ItemsContainer.MinWidth;
		bar.CanDockBottom = instance.CanDockBottom;
		bar.CanDockLeft = instance.CanDockLeft;
		bar.CanDockRight = instance.CanDockRight;
		bar.CanDockTop = instance.CanDockTop;
		bar.CanDockDocument = instance.CanDockDocument;
		bar.CanDockTab = instance.CanDockTab;
		bar.CanUndock = instance.CanUndock;
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
		if (!instance.CaptionBackColor.IsEmpty)
		{
			bar.CaptionBackColor = instance.CaptionBackColor;
		}
		if (!instance.CaptionForeColor.IsEmpty)
		{
			bar.CaptionForeColor = instance.CaptionForeColor;
		}
		if (!instance.ItemsContainer.BackgroundColor.IsEmpty)
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
		bar.SetDockTabStyle(bar.Style);
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
		bar.SetDockTabStyle(bar.Style);
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
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
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
			NativeFunctions.DrawRectangle(graphics, val2, r);
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
		if (NativeFunctions.AlphaBlendingSupported)
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
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
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
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
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
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
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

	internal static BaseItem CreateItemFromXml(XmlElement xmlItem)
	{
		string attribute = xmlItem.GetAttribute("class");
		BaseItem baseItem = null;
		switch (attribute)
		{
		case "Stimulsoft.Controls.Win.DotNetBar.ButtonItem":
			return new ButtonItem();
		case "Stimulsoft.Controls.Win.DotNetBar.TextBoxItem":
			return new TextBoxItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ComboBoxItem":
			return new ComboBoxItem();
		case "Stimulsoft.Controls.Win.DotNetBar.LabelItem":
			return new LabelItem();
		case "Stimulsoft.Controls.Win.DotNetBar.CustomizeItem":
			return new CustomizeItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ControlContainerItem":
			return new ControlContainerItem();
		case "Stimulsoft.Controls.Win.DotNetBar.DockContainerItem":
			return new DockContainerItem();
		case "Stimulsoft.Controls.Win.DotNetBar.MdiWindowListItem":
			return new MdiWindowListItem();
		case "Stimulsoft.Controls.Win.DotNetBar.SideBarContainerItem":
			return new SideBarContainerItem();
		case "Stimulsoft.Controls.Win.DotNetBar.SideBarPanelItem":
			return new SideBarPanelItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ExplorerBarGroupItem":
			return new ExplorerBarGroupItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ExplorerBarContainerItem":
			return new ExplorerBarContainerItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ProgressBarItem":
			return new ProgressBarItem();
		case "Stimulsoft.Controls.Win.DotNetBar.ColorPickerDropDown":
			return new ColorPickerDropDown();
		default:
			try
			{
				Assembly assembly = Assembly.Load(xmlItem.GetAttribute("assembly"));
				if (assembly == null)
				{
					return null;
				}
				return assembly.CreateInstance(xmlItem.GetAttribute("class")) as BaseItem;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Could not create item from XML. Assembly=" + xmlItem.GetAttribute("assembly") + ", Class=" + xmlItem.GetAttribute("class") + ", Inner Exception: " + ex.Message + ", Source=" + ex.Source);
			}
		}
	}

	internal static BaseItem CreateItemFromXml(XmlElement xmlItem, IDesignerHost dh, string name)
	{
		string attribute = xmlItem.GetAttribute("class");
		BaseItem baseItem = null;
		switch (attribute)
		{
		case "Stimulsoft.Controls.Win.DotNetBar.ButtonItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ButtonItem), name) as ButtonItem;
			}
			return dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
		case "Stimulsoft.Controls.Win.DotNetBar.TextBoxItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(TextBoxItem), name) as TextBoxItem;
			}
			return dh.CreateComponent(typeof(TextBoxItem)) as TextBoxItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ComboBoxItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ComboBoxItem), name) as ComboBoxItem;
			}
			return dh.CreateComponent(typeof(ComboBoxItem)) as ComboBoxItem;
		case "Stimulsoft.Controls.Win.DotNetBar.LabelItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(LabelItem), name) as LabelItem;
			}
			return dh.CreateComponent(typeof(LabelItem)) as LabelItem;
		case "Stimulsoft.Controls.Win.DotNetBar.CustomizeItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(CustomizeItem), name) as CustomizeItem;
			}
			return dh.CreateComponent(typeof(CustomizeItem)) as CustomizeItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ControlContainerItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ControlContainerItem), name) as ControlContainerItem;
			}
			return dh.CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
		case "Stimulsoft.Controls.Win.DotNetBar.DockContainerItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(DockContainerItem), name) as DockContainerItem;
			}
			return dh.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
		case "Stimulsoft.Controls.Win.DotNetBar.MdiWindowListItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(MdiWindowListItem), name) as MdiWindowListItem;
			}
			return dh.CreateComponent(typeof(MdiWindowListItem)) as MdiWindowListItem;
		case "Stimulsoft.Controls.Win.DotNetBar.SideBarContainerItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(SideBarContainerItem), name) as SideBarContainerItem;
			}
			return dh.CreateComponent(typeof(SideBarContainerItem)) as SideBarContainerItem;
		case "Stimulsoft.Controls.Win.DotNetBar.SideBarPanelItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(SideBarPanelItem), name) as SideBarPanelItem;
			}
			return dh.CreateComponent(typeof(SideBarPanelItem)) as SideBarPanelItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ExplorerBarGroupItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ExplorerBarGroupItem), name) as ExplorerBarGroupItem;
			}
			return dh.CreateComponent(typeof(ExplorerBarGroupItem)) as ExplorerBarGroupItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ExplorerBarContainerItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ExplorerBarContainerItem), name) as ExplorerBarContainerItem;
			}
			return dh.CreateComponent(typeof(ExplorerBarContainerItem)) as ExplorerBarContainerItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ProgressBarItem":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ProgressBarItem), name) as ProgressBarItem;
			}
			return dh.CreateComponent(typeof(ProgressBarItem)) as ProgressBarItem;
		case "Stimulsoft.Controls.Win.DotNetBar.ColorPickerDropDown":
			if (name != "")
			{
				return dh.CreateComponent(typeof(ColorPickerDropDown), name) as ColorPickerDropDown;
			}
			return dh.CreateComponent(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
		default:
			try
			{
				Assembly assembly = Assembly.Load(xmlItem.GetAttribute("assembly"));
				if (assembly == null)
				{
					return null;
				}
				BaseItem baseItem2 = assembly.CreateInstance(xmlItem.GetAttribute("class")) as BaseItem;
				return dh.CreateComponent(baseItem2.GetType()) as BaseItem;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Could not create item from XML. Assembly=" + xmlItem.GetAttribute("assembly") + ", Class=" + xmlItem.GetAttribute("class") + ", Inner Exception: " + ex.Message + ", Source=" + ex.Source);
			}
		}
	}

	internal static string GetItemErrorInfo(XmlElement xmlItem)
	{
		string text = "";
		if (xmlItem.HasAttribute("assembly"))
		{
			text += xmlItem.GetAttribute("assembly");
		}
		if (xmlItem.HasAttribute("class"))
		{
			text += xmlItem.GetAttribute("class");
		}
		return text;
	}

	internal static void PaintSystemButton(Graphics g, SystemButton btn, Rectangle r, bool MouseDown, bool MouseOver, bool Disabled)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		if (MouseDown)
		{
			g.FillRectangle((Brush)new SolidBrush(ColorFunctions.PressedBackColor(g)), r);
			NativeFunctions.DrawRectangle(g, SystemPens.Highlight, r);
		}
		else if (MouseOver)
		{
			g.FillRectangle((Brush)new SolidBrush(ColorFunctions.HoverBackColor(g)), r);
			NativeFunctions.DrawRectangle(g, SystemPens.Highlight, r);
		}
		Bitmap val = new Bitmap(r.Width, r.Height, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
		rectangle.Inflate(0, -1);
		Rectangle clip = rectangle;
		clip.Inflate(-1, -1);
		SolidBrush val3 = new SolidBrush(SystemColors.Control);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, r.Width, r.Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val2.SetClip(clip);
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)btn, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		val.MakeTransparent(SystemColors.Control);
		if (Disabled)
		{
			ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
			{
				new float[5],
				new float[5],
				new float[5],
				new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
				new float[5]
			});
			ImageAttributes val4 = new ImageAttributes();
			val4.ClearColorKey();
			val4.SetColorMatrix(colorMatrix);
			g.DrawImage((Image)(object)val, r, 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val4);
		}
		else
		{
			if (MouseDown)
			{
				r.Offset(1, 1);
			}
			g.DrawImageUnscaled((Image)(object)val, r);
		}
	}

	internal static void SyncProperty(BaseItem item, string propertyName)
	{
		if (item.GlobalName.Length > 0)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item)[propertyName];
			SetPropertyByGlobalName(GetOwner(item), item.GetType(), item.GlobalName, propertyDescriptor, propertyDescriptor.GetValue(item));
		}
		else if (item.Name.Length > 0)
		{
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(item)[propertyName];
			SetProperty(GetOwner(item), item.GetType(), item.Name, propertyDescriptor2, propertyDescriptor2.GetValue(item));
		}
	}

	private static object GetOwner(BaseItem item)
	{
		object obj = item.GetOwner();
		if (obj is RibbonBar && ((RibbonBar)obj).IsOverflowRibbon)
		{
			obj = ((!((RibbonBar)obj).IsOnQat) ? ((RibbonBar)obj).OverflowParent : ((RibbonBar)obj).QatButtonParent.GetOwner());
		}
		return obj;
	}

	internal static void SetProperty(object owner, Type itemType, string itemName, PropertyDescriptor prop, object value)
	{
		IOwner owner2 = owner as IOwner;
		DotNetBarManager dotNetBarManager = owner as DotNetBarManager;
		if (owner2 == null || itemName == "" || prop == null)
		{
			return;
		}
		ArrayList arrayList = null;
		if (dotNetBarManager != null)
		{
			if (!dotNetBarManager.IsDisposed)
			{
				arrayList = dotNetBarManager.GetItems(itemName, itemType, fullSearch: true);
			}
		}
		else
		{
			arrayList = owner2.GetItems(itemName, itemType);
		}
		if (arrayList == null)
		{
			return;
		}
		foreach (BaseItem item in arrayList)
		{
			object value2 = prop.GetValue(item);
			if (value2 != value && (value2 == null || !value2.Equals(value)))
			{
				prop.SetValue(item, value);
			}
		}
	}

	internal static void SetPropertyByGlobalName(object owner, Type itemType, string itemName, PropertyDescriptor prop, object value)
	{
		IOwner owner2 = owner as IOwner;
		DotNetBarManager dotNetBarManager = owner as DotNetBarManager;
		if (owner2 == null || itemName == "" || prop == null)
		{
			return;
		}
		ArrayList arrayList = null;
		if (dotNetBarManager != null)
		{
			if (!dotNetBarManager.IsDisposed)
			{
				arrayList = dotNetBarManager.GetItems(itemName, itemType, fullSearch: true, useGlobalName: true);
			}
		}
		else
		{
			arrayList = owner2.GetItems(itemName, itemType, useGlobalName: true);
		}
		if (arrayList == null)
		{
			return;
		}
		foreach (BaseItem item in arrayList)
		{
			if (prop.GetValue(item) != value)
			{
				prop.SetValue(item, value);
			}
		}
	}

	internal static ResourceManager GetResourceManager(bool bDefault)
	{
		string text = ".Strings";
		text = ((!(Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute) || !(dotNetBarResourcesAttribute.NamespacePrefix != "")) ? ("Stimulsoft.Controls.Win.DotNetBar" + text) : (dotNetBarResourcesAttribute.NamespacePrefix + text));
		return new ResourceManager(text, Assembly.GetExecutingAssembly());
	}

	internal static ResourceManager GetResourceManager()
	{
		string text = ".Strings";
		text = ((!(Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute) || !(dotNetBarResourcesAttribute.NamespacePrefix != "")) ? ("Stimulsoft.Controls.Win.DotNetBar" + text) : (dotNetBarResourcesAttribute.NamespacePrefix + text));
		if (ms_ResourceName == "")
		{
			CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			int num = 0;
			while (cultureInfo.LCID != 127 && num < 16)
			{
				if (executingAssembly.GetManifestResourceInfo(text + "_" + cultureInfo.Name.ToLower() + ".resources") != null)
				{
					ms_ResourceName = text + "_" + cultureInfo.Name.ToLower();
					break;
				}
				if (executingAssembly.GetManifestResourceInfo(text + "_" + cultureInfo.TwoLetterISOLanguageName.ToLower() + ".resources") != null)
				{
					ms_ResourceName = text + "_" + cultureInfo.TwoLetterISOLanguageName.ToLower();
					break;
				}
				cultureInfo = cultureInfo.Parent;
				num++;
			}
			if (ms_ResourceName == "")
			{
				ms_ResourceName = text;
			}
		}
		return new ResourceManager(ms_ResourceName, Assembly.GetExecutingAssembly());
	}

	internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor)
	{
		DrawBorder(g, bordertype, r, singleLineColor, eBorderSide.All);
	}

	internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side)
	{
		DrawBorder(g, bordertype, r, singleLineColor, side, (DashStyle)0);
	}

	internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side, DashStyle borderDashStyle)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder(g, bordertype, r, singleLineColor, side, borderDashStyle, 1);
	}

	internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side, DashStyle borderDashStyle, int lineWidth)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		if (lineWidth <= 0)
		{
			return;
		}
		Border3DSide val = ((side != eBorderSide.All) ? ((Border3DSide)((((side | eBorderSide.Left) != 0) ? 1 : 0) | (((side | eBorderSide.Right) != 0) ? 4 : 0) | (((side | eBorderSide.Top) != 0) ? 2 : 0) | (((side | eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063));
		switch (bordertype)
		{
		case eBorderType.Bump:
			ControlPaint.DrawBorder3D(g, r, (Border3DStyle)9, val);
			break;
		case eBorderType.Etched:
			ControlPaint.DrawBorder3D(g, r, (Border3DStyle)6, val);
			break;
		case eBorderType.Raised:
			ControlPaint.DrawBorder3D(g, r, (Border3DStyle)4, val);
			break;
		case eBorderType.Sunken:
			ControlPaint.DrawBorder3D(g, r, (Border3DStyle)2, val);
			break;
		case eBorderType.SingleLine:
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)3;
			Pen val3 = new Pen(singleLineColor, (float)lineWidth);
			try
			{
				val3.DashStyle = borderDashStyle;
				int num = lineWidth / 2;
				if ((side & eBorderSide.Left) != 0)
				{
					g.DrawLine(val3, r.X + num, r.Y, r.X + num, r.Bottom - 1);
				}
				if ((side & eBorderSide.Top) != 0)
				{
					g.DrawLine(val3, r.X, r.Y + num, r.Right - 1, r.Y + num);
				}
				if (num == 0)
				{
					num = 1;
				}
				if ((side & eBorderSide.Right) != 0)
				{
					g.DrawLine(val3, r.Right - num, r.Y, r.Right - num, r.Bottom - 1);
				}
				if ((side & eBorderSide.Bottom) != 0)
				{
					g.DrawLine(val3, r.X, r.Bottom - num, r.Right - 1, r.Bottom - num);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
			break;
		}
		case eBorderType.DoubleLine:
		{
			Pen val2 = new Pen(singleLineColor, (float)lineWidth);
			try
			{
				val2.DashStyle = borderDashStyle;
				for (int i = 0; i < lineWidth + 1; i += lineWidth)
				{
					if ((side & eBorderSide.Left) != 0)
					{
						g.DrawLine(val2, r.X, r.Y, r.X, r.Bottom - 1);
					}
					if ((side & eBorderSide.Top) != 0)
					{
						g.DrawLine(val2, r.X, r.Y, r.Right - 1, r.Y);
					}
					if ((side & eBorderSide.Right) != 0)
					{
						g.DrawLine(val2, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
					}
					if ((side & eBorderSide.Bottom) != 0)
					{
						g.DrawLine(val2, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
					}
					r.Inflate(-1, -1);
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

	internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, Border3DStyle style, Border3DSide side)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, x, y, width, height, style, side, SystemColors.Control, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, Border3DStyle style, Color clr)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, x, y, width, height, style, (Border3DSide)2063, clr, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, Border3DStyle style)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, x, y, width, height, style, (Border3DSide)2063, SystemColors.Control, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, Rectangle r, Border3DStyle style, Border3DSide side, Color baseColor)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, side, baseColor, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, Rectangle r, Border3DStyle style, Border3DSide side, Color baseColor, bool bFillInner)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, side, baseColor, bFillInner);
	}

	internal static void DrawBorder3D(Graphics g, Rectangle r, Border3DStyle style)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, (Border3DSide)2063, SystemColors.Control, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, Border3DStyle style, Border3DSide side, Color baseColor)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		DrawBorder3D(g, x, y, width, height, style, side, baseColor, bFillInner: true);
	}

	internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, Border3DStyle style, Border3DSide side, Color baseColor, bool bFillInner)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected I4, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		if (bFillInner)
		{
			g.FillRectangle((Brush)new SolidBrush(baseColor), x, y, width, height);
		}
		Color color = ControlPaint.Light(baseColor);
		Color color2 = ControlPaint.Dark(baseColor);
		Pen val = null;
		Pen val2 = null;
		Pen val3 = new Pen(baseColor);
		height--;
		width--;
		switch (style - 2)
		{
		default:
			if ((int)style == 10)
			{
				val2 = new Pen(ControlPaint.DarkDark(baseColor), 1f);
				val = new Pen(color2, 1f);
				if ((side & 2) != 0)
				{
					g.DrawLine(val2, x, y, x + width, y);
					g.DrawLine(val, x + 1, y + 1, x + width - 2, y + 1);
				}
				if ((side & 1) != 0)
				{
					g.DrawLine(val2, x, y, x, y + height);
					g.DrawLine(val, x + 1, y + 1, x + 1, y + height - 2);
				}
				val2.Dispose();
				val.Dispose();
				val2 = new Pen(color, 1f);
				val = new Pen(ControlPaint.LightLight(baseColor), 1f);
				if ((side & 4) != 0)
				{
					g.DrawLine(val2, x + width, y, x + width, y + height);
					g.DrawLine(val, x + width - 1, y + 1, x + width - 1, y + height - 1);
				}
				if ((side & 8) != 0)
				{
					g.DrawLine(val2, x, y + height, x + width, y + height);
					g.DrawLine(val, x + 1, y + height - 1, x + width - 1, y + height - 1);
				}
			}
			break;
		case 2:
			val = new Pen(color, 1f);
			val2 = new Pen(color2, 1f);
			if ((side & 2) != 0)
			{
				g.DrawLine(val, x, y, x + width, y);
			}
			if ((side & 1) != 0)
			{
				g.DrawLine(val, x, y, x, y + height);
			}
			if ((side & 4) != 0)
			{
				g.DrawLine(val2, x + width, y, x + width, y + height);
			}
			if ((side & 8) != 0)
			{
				g.DrawLine(val2, x, y + height, x + width, y + height);
			}
			break;
		case 0:
			val = new Pen(color, 1f);
			val2 = new Pen(color2, 1f);
			if ((side & 2) != 0)
			{
				g.DrawLine(val2, x, y, x + width, y);
			}
			if ((side & 1) != 0)
			{
				g.DrawLine(val2, x, y, x, y + height);
			}
			if ((side & 4) != 0)
			{
				g.DrawLine(val, x + width, y, x + width, y + height);
			}
			if ((side & 8) != 0)
			{
				g.DrawLine(val, x, y + height, x + width, y + height);
			}
			break;
		case 3:
			if ((side & 2) != 0 && (side & 1) != 0 && (side & 4) != 0 && (side & 8) != 0)
			{
				val = new Pen(Color.White, 1f);
				g.DrawRectangle(val3, x, y, width, height);
				g.DrawLine(val, x + 1, y + 1, x + width - 1, y + 1);
				g.DrawLine(val, x + 1, y + 1, x + 1, y + height - 1);
				break;
			}
			val2 = new Pen(color, 1f);
			val = new Pen(ControlPaint.LightLight(baseColor), 1f);
			if ((side & 2) != 0)
			{
				g.DrawLine(val2, x, y, x + width, y);
				g.DrawLine(val, x + 1, y + 1, x + width - 2, y + 1);
			}
			if ((side & 1) != 0)
			{
				g.DrawLine(val2, x, y, x, y + height);
				g.DrawLine(val, x + 1, y + 1, x + 1, y + height - 2);
			}
			val2.Dispose();
			val.Dispose();
			val2 = new Pen(ControlPaint.DarkDark(baseColor), 1f);
			val = new Pen(color2, 1f);
			if ((side & 4) != 0)
			{
				g.DrawLine(val2, x + width, y, x + width, y + height);
				g.DrawLine(val, x + width - 1, y + 1, x + width - 1, y + height - 1);
			}
			if ((side & 8) != 0)
			{
				g.DrawLine(val2, x, y + height, x + width, y + height);
				g.DrawLine(val, x + 1, y + height - 1, x + width - 1, y + height - 1);
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

	internal static LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle);
	}

	internal static LinearGradientBrush CreateLinearGradientBrush(RectangleF r, Color color1, Color color2, float gradientAngle)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (r.Width <= 0f)
		{
			r.Width = 1f;
		}
		if (r.Height <= 0f)
		{
			r.Height = 1f;
		}
		return new LinearGradientBrush(new RectangleF(r.X, r.Y - 1f, r.Width, r.Height + 1f), color1, color2, gradientAngle);
	}

	internal static LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle, bool isAngleScalable)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle, isAngleScalable);
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
			if (subItem.GetType() == itemType && ((useGlobalName && subItem.GlobalName == ItemName) || (!useGlobalName && subItem.Name == ItemName)))
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

	public static ScreenInformation ScreenFromPoint(Point pScreen)
	{
		if (m_Screens.Count == 0)
		{
			RefreshScreens();
		}
		foreach (ScreenInformation screen in m_Screens)
		{
			if (screen.Bounds.Contains(pScreen))
			{
				return screen;
			}
		}
		Screen val = Screen.FromPoint(pScreen);
		if (val != null)
		{
			return new ScreenInformation(val.Bounds, val.WorkingArea);
		}
		return null;
	}

	internal static ScreenInformation ScreenFromControl(Control control)
	{
		Rectangle rect;
		if (control.Parent != null)
		{
			Point location = control.PointToScreen(control.Location);
			rect = new Rectangle(location, control.Size);
		}
		else
		{
			rect = new Rectangle(control.Location, control.Size);
		}
		if (m_Screens.Count == 0)
		{
			RefreshScreens();
		}
		foreach (ScreenInformation screen in m_Screens)
		{
			if (screen.Bounds.Contains(rect))
			{
				return screen;
			}
		}
		Screen val = Screen.FromControl(control);
		if (val != null)
		{
			return new ScreenInformation(val.Bounds, val.WorkingArea);
		}
		return null;
	}

	public static void RefreshScreens()
	{
		m_Screens.Clear();
		Screen[] allScreens = Screen.AllScreens;
		foreach (Screen val in allScreens)
		{
			m_Screens.Add(new ScreenInformation(val.Bounds, val.WorkingArea));
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

	internal static Icon CreateDisabledIcon(Icon ico)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			ico.Save((Stream)memoryStream);
			byte[] array = memoryStream.ToArray();
			int num = array[4];
			for (int i = 0; i < num; i++)
			{
				int num2 = 6 + i * 16;
				int num3 = array[num2];
				int num4 = array[num2 + 1];
				int dWORD = GetDWORD(ref array, num2 + 12);
				int dWORD2 = GetDWORD(ref array, dWORD);
				int wORD = GetWORD(ref array, dWORD + 12);
				int wORD2 = GetWORD(ref array, dWORD + 14);
				if (wORD != 1)
				{
					continue;
				}
				int dWORD3 = GetDWORD(ref array, dWORD + 20);
				int num5 = 0;
				if (wORD2 != 8)
				{
					if (wORD2 != 24)
					{
						if (wORD2 != 32)
						{
							continue;
						}
						num5 = 4;
						dWORD3 = num3 * num4 * 4;
					}
					else
					{
						num5 = 3;
						dWORD3 = num3 * num4 * 3;
					}
				}
				else
				{
					num5 = 4;
					dWORD3 = 1024;
				}
				int num6 = dWORD + dWORD2;
				for (int j = num6; j < num6 + dWORD3; j += num5)
				{
					byte b = (byte)((array[j] + 255) / 2);
					byte b2 = (byte)((array[j + 1] + 255) / 2);
					byte b3 = (byte)((array[j + 2] + 255) / 2);
					if (b != 127 || b2 != 127 || b3 != 127)
					{
						byte b4 = (array[j + 1] = (array[j] = (byte)((double)(int)b3 * 0.299 + (double)(int)b2 * 0.587 + (double)(int)b * 0.114)));
						array[j + 2] = b4;
					}
				}
			}
			return new Icon((Stream)new MemoryStream(array));
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static int GetDWORD(ref byte[] array, int offset)
	{
		return array[offset] + (array[offset + 1] << 8) + (array[offset + 2] << 16) + (array[offset + 3] << 24);
	}

	private static int GetWORD(ref byte[] array, int offset)
	{
		return array[offset] + (array[offset + 1] << 8);
	}

	internal static void PaintBackgroundImage(Graphics g, Rectangle targetRect, Image backgroundImage, eStyleBackgroundImage backgroundImagePosition, int backgroundImageAlpha)
	{
		PaintBackgroundImage(g, targetRect, backgroundImage, (eBackgroundImagePosition)backgroundImagePosition, backgroundImageAlpha);
	}

	internal static void PaintBackgroundImage(Graphics g, Rectangle targetRect, Image backgroundImage, eBackgroundImagePosition backgroundImagePosition, int backgroundImageAlpha)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		if (backgroundImage == null)
		{
			return;
		}
		Rectangle rectangle = targetRect;
		ImageAttributes val = null;
		if (backgroundImageAlpha != 255)
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
					(float)backgroundImageAlpha / 255f,
					0f
				},
				new float[5] { 0f, 0f, 0f, 0f, 1f }
			};
			ColorMatrix val2 = new ColorMatrix(array);
			val = new ImageAttributes();
			val.SetColorMatrix(val2, (ColorMatrixFlag)0, (ColorAdjustType)1);
		}
		switch (backgroundImagePosition)
		{
		case eBackgroundImagePosition.Stretch:
			if (val != null)
			{
				g.DrawImage(backgroundImage, rectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2, val);
			}
			else
			{
				g.DrawImage(backgroundImage, rectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2);
			}
			break;
		case eBackgroundImagePosition.CenterLeft:
		case eBackgroundImagePosition.CenterRight:
		{
			Rectangle rectangle3 = new Rectangle(rectangle.X, rectangle.Y, backgroundImage.Width, backgroundImage.Height);
			if (rectangle.Width > backgroundImage.Width && backgroundImagePosition == eBackgroundImagePosition.CenterRight)
			{
				rectangle3.X += rectangle.Width - backgroundImage.Width;
			}
			rectangle3.Y += (rectangle.Height - backgroundImage.Height) / 2;
			if (val != null)
			{
				g.DrawImage(backgroundImage, rectangle3, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2, val);
			}
			else
			{
				g.DrawImage(backgroundImage, rectangle3, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.Center:
		{
			Rectangle rectangle5 = new Rectangle(rectangle.X, rectangle.Y, backgroundImage.Width, backgroundImage.Height);
			if (rectangle.Width > backgroundImage.Width)
			{
				rectangle5.X += (rectangle.Width - backgroundImage.Width) / 2;
			}
			if (rectangle.Height > backgroundImage.Height)
			{
				rectangle5.Y += (rectangle.Height - backgroundImage.Height) / 2;
			}
			if (val != null)
			{
				g.DrawImage(backgroundImage, rectangle5, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2, val);
			}
			else
			{
				g.DrawImage(backgroundImage, rectangle5, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.TopLeft:
		case eBackgroundImagePosition.TopRight:
		case eBackgroundImagePosition.BottomLeft:
		case eBackgroundImagePosition.BottomRight:
		{
			Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y, backgroundImage.Width, backgroundImage.Height);
			switch (backgroundImagePosition)
			{
			case eBackgroundImagePosition.TopRight:
				rectangle4.X = rectangle.Right - backgroundImage.Width;
				break;
			case eBackgroundImagePosition.BottomLeft:
				rectangle4.Y = rectangle.Bottom - backgroundImage.Height;
				break;
			case eBackgroundImagePosition.BottomRight:
				rectangle4.Y = rectangle.Bottom - backgroundImage.Height;
				rectangle4.X = rectangle.Right - backgroundImage.Width;
				break;
			}
			if (val != null)
			{
				g.DrawImage(backgroundImage, rectangle4, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2, val);
			}
			else
			{
				g.DrawImage(backgroundImage, rectangle4, 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2);
			}
			break;
		}
		case eBackgroundImagePosition.Tile:
			if (val != null)
			{
				if (rectangle.Width > backgroundImage.Width || rectangle.Height > backgroundImage.Height)
				{
					int i = rectangle.X;
					for (int j = rectangle.Y; j < rectangle.Bottom; j += backgroundImage.Height)
					{
						for (; i < rectangle.Right; i += backgroundImage.Width)
						{
							Rectangle rectangle2 = new Rectangle(i, j, backgroundImage.Width, backgroundImage.Height);
							if (rectangle2.Right > rectangle.Right)
							{
								rectangle2.Width -= rectangle2.Right - rectangle.Right;
							}
							if (rectangle2.Bottom > rectangle.Bottom)
							{
								rectangle2.Height -= rectangle2.Bottom - rectangle.Bottom;
							}
							g.DrawImage(backgroundImage, rectangle2, 0, 0, rectangle2.Width, rectangle2.Height, (GraphicsUnit)2, val);
						}
						i = rectangle.X;
					}
				}
				else
				{
					g.DrawImage(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), 0, 0, backgroundImage.Width, backgroundImage.Height, (GraphicsUnit)2, val);
				}
			}
			else
			{
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)3;
				TextureBrush val3 = new TextureBrush(backgroundImage);
				try
				{
					val3.WrapMode = (WrapMode)0;
					g.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				g.SmoothingMode = smoothingMode;
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
					TimeSpan timeSpan2 = DateTime.Now - now2;
					TimeSpan timeSpan3 = DateTime.Now - now;
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

	internal static Bitmap LoadBitmap(string imageName)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		return new Bitmap(typeof(DotNetBarManager).Assembly.GetManifestResourceStream("Stimulsoft.Controls.Win." + imageName));
	}

	internal static Icon LoadIcon(string imageName)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + imageName));
		}
		return new Icon(typeof(DotNetBarManager), imageName);
	}

	internal static byte[] LoadFont(string fontFileName)
	{
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + fontFileName))
			{
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
				stream.Close();
				return array;
			}
		}
		using Stream stream2 = typeof(DotNetBarManager).Assembly.GetManifestResourceStream("Stimulsoft.Controls.Win." + fontFileName);
		byte[] array2 = new byte[stream2.Length];
		stream2.Read(array2, 0, array2.Length);
		stream2.Close();
		return array2;
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
			return style == eDotNetBarStyle.Metro;
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
			return style == eDotNetBarStyle.Metro;
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

	public static bool IsOffice2013StyleOnly(eDotNetBarStyle style)
	{
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		return style == eDotNetBarStyle.Metro;
	}
}
