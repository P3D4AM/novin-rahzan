using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(SuperTooltip), "Ribbon.SuperTooltip.ico")]
[ToolboxItem(false)]
[ProvideProperty("SuperTooltip", typeof(IComponent))]
[ComVisible(false)]
public class SuperTooltip : Component, IExtenderProvider
{
	private const string TREEGX_NODE_CLASS_NAME = "Stimulsoft.Controls.Win.Tree.Node";

	public static string CurrentSuperTooltipHyperlink;

	private Hashtable superTooltipInfo = new Hashtable();

	private SuperTooltipControl Tooltip;

	private Timer ActiveWindowTimer;

	private IntPtr ActiveWindowPtr = IntPtr.Zero;

	private long DurationDisplayed;

	private int HoverCount;

	public Size MinimumTooltipSize = new Size(150, 24);

	public bool ShowTooltipForFocusedControl = true;

	public bool IsCalibri;

	public Font DefaultHeaderFont = new Font("Arial", 8f, (FontStyle)1);

	public Font DefaultFont = new Font("Arial", 8f);

	public Font DefaultFooterFont = new Font("Arial", 8f, (FontStyle)1);

	public SuperTooltipInfo DefaultTooltipSettings = new SuperTooltipInfo();

	private bool m_MouseOverSuperTooltip;

	private bool m_TimerOperation;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Hashtable SuperTooltipInfoTable => superTooltipInfo;

	private bool IsMouseOverSuperTooltip
	{
		get
		{
			if (m_MouseOverSuperTooltip)
			{
				return true;
			}
			if (Tooltip != null && ((Control)Tooltip).Visible && ((Control)Tooltip).Bounds.Contains(Control.MousePosition))
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsTooltipVisible
	{
		get
		{
			if (Tooltip != null && ((Control)Tooltip).Visible)
			{
				return true;
			}
			return false;
		}
	}

	public event SuperTooltipEventHandler BeforeTooltipDisplay;

	protected override void Dispose(bool disposing)
	{
		HideTooltip();
		DestroyActiveWindowTimer();
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinimumTooltipSize()
	{
		if (MinimumTooltipSize.Width == 150)
		{
			return MinimumTooltipSize.Height != 24;
		}
		return true;
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SuperTooltipInfoEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Localizable(true)]
	public SuperTooltipInfo GetSuperTooltip(IComponent c)
	{
		if (superTooltipInfo.Contains(c) && superTooltipInfo[c] is SuperTooltipInfo result)
		{
			return result;
		}
		return null;
	}

	public void SetSuperTooltip(IComponent c, SuperTooltipInfo info)
	{
		if (info != null)
		{
			if (info.BodyText == null)
			{
				info.BodyText = string.Empty;
			}
			if (info.FooterText == null)
			{
				info.FooterText = string.Empty;
			}
		}
		if (superTooltipInfo.Contains(c))
		{
			if (info == null)
			{
				RemoveSuperTooltipInfo(c);
			}
			else
			{
				superTooltipInfo[c] = info;
			}
		}
		else if (info != null)
		{
			AddSuperTooltipInfo(c, info);
		}
		if (c is MaskedTextBoxAdv maskedTextBoxAdv && !base.DesignMode)
		{
			SetSuperTooltip((IComponent)maskedTextBoxAdv.MaskedTextBox, info);
		}
		else if (c is TextBoxDropDown)
		{
			SetSuperTooltip((IComponent)((TextBoxDropDown)c).TextBox, info);
		}
		else if (c is ExpandablePanel)
		{
			SetSuperTooltip((IComponent)((ExpandablePanel)c).TitlePanel, info);
		}
	}

	public SizeF MeasureHeaderText(Graphics g, string headerText)
	{
		return g.MeasureString(headerText, DefaultHeaderFont);
	}

	public SizeF MeasureBodyText(Graphics g, string bodyText, int maxWidth = 260)
	{
		return g.MeasureString(bodyText, DefaultFont, maxWidth);
	}

	public SizeF MeasureFooterText(Graphics g, string footerText)
	{
		return g.MeasureString(footerText, DefaultFooterFont);
	}

	public void RemoveAllSuperTooltip()
	{
		int count = superTooltipInfo.Keys.Count;
		IComponent[] array = new IComponent[count];
		int num = 0;
		foreach (IComponent key in superTooltipInfo.Keys)
		{
			array[num] = key;
			num++;
		}
		for (num = 0; num < count; num++)
		{
			IComponent c = array[num];
			RemoveSuperTooltipInfo(c);
		}
	}

	private void RemoveSuperTooltipInfo(IComponent c)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		superTooltipInfo.Remove(c);
		if (!base.DesignMode)
		{
			if (c is MaskedTextBoxAdv)
			{
				RemoveSuperTooltipInfo((IComponent)((MaskedTextBoxAdv)c).MaskedTextBox);
			}
			else if (c is TextBoxDropDown)
			{
				RemoveSuperTooltipInfo((IComponent)((TextBoxDropDown)c).TextBox);
			}
			else if (c is ExpandablePanel)
			{
				RemoveSuperTooltipInfo((IComponent)((ExpandablePanel)c).TitlePanel);
			}
			if (c is ComboBoxItem comboBoxItem)
			{
				ComboBoxItem comboBoxItem2 = comboBoxItem;
				comboBoxItem2.MouseHover -= ComponentMouseHover;
				comboBoxItem2.MouseLeave -= ComponentMouseLeave;
				comboBoxItem2.MouseEnter -= ComponentMouseEnter;
				comboBoxItem2.MouseDown -= new MouseEventHandler(ComponentMouseDown);
				((Control)comboBoxItem2.ComboBoxEx).MouseHover -= ComponentMouseHover;
				((Control)comboBoxItem2.ComboBoxEx).MouseLeave -= ComponentMouseLeave;
				((Control)comboBoxItem2.ComboBoxEx).MouseEnter -= ComponentMouseEnter;
				((Control)comboBoxItem2.ComboBoxEx).MouseDown -= new MouseEventHandler(ComponentMouseDown);
			}
			else if (c is TextBoxItem)
			{
				TextBoxItem textBoxItem = (TextBoxItem)c;
				textBoxItem.MouseHover -= ComponentMouseHover;
				textBoxItem.MouseLeave -= ComponentMouseLeave;
				textBoxItem.MouseEnter -= ComponentMouseEnter;
				textBoxItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
				((Control)textBoxItem.TextBox).MouseHover -= ComponentMouseHover;
				((Control)textBoxItem.TextBox).MouseLeave -= ComponentMouseLeave;
				((Control)textBoxItem.TextBox).MouseEnter -= ComponentMouseEnter;
				((Control)textBoxItem.TextBox).MouseDown -= new MouseEventHandler(ComponentMouseDown);
			}
			else if (c is BaseItem)
			{
				BaseItem baseItem = (BaseItem)c;
				baseItem.MouseHover -= ComponentMouseHover;
				baseItem.MouseLeave -= ComponentMouseLeave;
				baseItem.MouseEnter -= ComponentMouseEnter;
				baseItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
			}
			else if (c is RibbonBar)
			{
				RibbonBar ribbonBar = (RibbonBar)c;
				ribbonBar.DialogLauncherMouseHover -= ComponentMouseHover;
				ribbonBar.DialogLauncherMouseLeave -= ComponentMouseLeave;
				ribbonBar.DialogLauncherMouseDown -= new MouseEventHandler(ComponentMouseDown);
				ribbonBar.DialogLauncherMouseEnter -= ComponentMouseEnter;
			}
			else if (c is Control)
			{
				Control val = (Control)c;
				val.MouseHover -= ComponentMouseHover;
				val.MouseLeave -= ComponentMouseLeave;
				val.MouseDown -= new MouseEventHandler(ComponentMouseDown);
				val.MouseEnter -= ComponentMouseEnter;
			}
			else if (c is ISuperTooltipInfoProvider)
			{
				ISuperTooltipInfoProvider superTooltipInfoProvider = (ISuperTooltipInfoProvider)c;
				superTooltipInfoProvider.DisplayTooltip -= ComponentMouseHover;
				superTooltipInfoProvider.HideTooltip -= ComponentHideTooltip;
			}
			else if (c is ToolStripButton)
			{
				ToolStripButton val2 = (ToolStripButton)c;
				((ToolStripItem)val2).MouseHover -= ComponentMouseHover;
				((ToolStripItem)val2).MouseLeave -= ComponentMouseLeave;
				((ToolStripItem)val2).MouseEnter -= ComponentMouseEnter;
				((ToolStripItem)val2).MouseDown -= new MouseEventHandler(ComponentMouseDown);
			}
			else if (c is TabItem)
			{
				TabItem tabItem = (TabItem)c;
				tabItem.MouseHover -= ComponentMouseHover;
				tabItem.MouseLeave -= ComponentMouseLeave;
				tabItem.MouseEnter -= ComponentMouseEnter;
				tabItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
			}
		}
	}

	private void AddSuperTooltipInfo(IComponent c, SuperTooltipInfo info)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		superTooltipInfo[c] = info;
		if (c is ISuperTooltipInfoProvider superTooltipInfoProvider)
		{
			ISuperTooltipInfoProvider superTooltipInfoProvider2 = superTooltipInfoProvider;
			superTooltipInfoProvider2.DisplayTooltip += ComponentMouseHover;
			superTooltipInfoProvider2.HideTooltip += ComponentHideTooltip;
		}
		else if (c is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = (ComboBoxItem)c;
			comboBoxItem.MouseHover += ComponentMouseHover;
			comboBoxItem.MouseLeave += ComponentMouseLeave;
			comboBoxItem.MouseEnter += ComponentMouseEnter;
			comboBoxItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
			((Control)comboBoxItem.ComboBoxEx).MouseHover += ComponentMouseHover;
			((Control)comboBoxItem.ComboBoxEx).MouseLeave += ComponentMouseLeave;
			((Control)comboBoxItem.ComboBoxEx).MouseEnter += ComponentMouseEnter;
			((Control)comboBoxItem.ComboBoxEx).MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is TextBoxItem)
		{
			TextBoxItem textBoxItem = (TextBoxItem)c;
			textBoxItem.MouseHover += ComponentMouseHover;
			textBoxItem.MouseLeave += ComponentMouseLeave;
			textBoxItem.MouseEnter += ComponentMouseEnter;
			textBoxItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
			((Control)textBoxItem.TextBox).MouseHover += ComponentMouseHover;
			((Control)textBoxItem.TextBox).MouseLeave += ComponentMouseLeave;
			((Control)textBoxItem.TextBox).MouseEnter += ComponentMouseEnter;
			((Control)textBoxItem.TextBox).MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is BaseItem)
		{
			BaseItem baseItem = (BaseItem)c;
			baseItem.MouseHover += ComponentMouseHover;
			baseItem.MouseLeave += ComponentMouseLeave;
			baseItem.MouseEnter += ComponentMouseEnter;
			baseItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is RibbonBar)
		{
			RibbonBar ribbonBar = (RibbonBar)c;
			ribbonBar.DialogLauncherMouseHover += ComponentMouseHover;
			ribbonBar.DialogLauncherMouseEnter += ComponentMouseEnter;
			ribbonBar.DialogLauncherMouseLeave += ComponentMouseLeave;
			ribbonBar.DialogLauncherMouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is Control)
		{
			Control val = (Control)c;
			val.MouseHover += ComponentMouseHover;
			val.MouseLeave += ComponentMouseLeave;
			val.MouseDown += new MouseEventHandler(ComponentMouseDown);
			val.MouseEnter += ComponentMouseEnter;
		}
		else if (c is ToolStripButton)
		{
			ToolStripButton val2 = (ToolStripButton)c;
			((ToolStripItem)val2).MouseHover += ComponentMouseHover;
			((ToolStripItem)val2).MouseLeave += ComponentMouseLeave;
			((ToolStripItem)val2).MouseEnter += ComponentMouseEnter;
			((ToolStripItem)val2).MouseDown += new MouseEventHandler(ComponentMouseDown);
			val2.AutoToolTip = false;
			((ToolStripItem)val2).ToolTipText = "";
		}
		else if (c is TabItem)
		{
			TabItem tabItem = (TabItem)c;
			tabItem.MouseHover += ComponentMouseHover;
			tabItem.MouseLeave += ComponentMouseLeave;
			tabItem.MouseEnter += ComponentMouseEnter;
			tabItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
	}

	private void ComponentHideTooltip(object sender, EventArgs e)
	{
		HideTooltip();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseDown(object sender, MouseEventArgs e)
	{
		HideTooltip();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseEnter(object sender, EventArgs e)
	{
		if (sender is Control)
		{
			ResetHover((Control)((sender is Control) ? sender : null));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseLeave(object sender, EventArgs e)
	{
		if (!IsMouseOverSuperTooltip)
		{
			HideTooltip();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseHover(object sender, EventArgs e)
	{
		HoverCount++;
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null && HoverCount < 2)
		{
			ResetHover(val);
		}
		else if (sender is BaseItem item && HoverCount < 2)
		{
			Control containerControl = GetContainerControl(item);
			if (containerControl != null)
			{
				ResetHover(containerControl);
			}
		}
		else if (sender is TabItem tabItem && HoverCount < 2)
		{
			ResetHover((Control)(object)tabItem.Parent);
		}
		else
		{
			ShowTooltip(sender);
		}
	}

	private static Control GetContainerControl(BaseItem item)
	{
		object containerControl = item.ContainerControl;
		if (containerControl is BaseItem)
		{
			containerControl = ((BaseItem)containerControl).ContainerControl;
		}
		return (Control)((containerControl is Control) ? containerControl : null);
	}

	private ComboBoxItem GetComboBoxItemFromComboBoxEx(ComboBoxEx c)
	{
		foreach (object key in superTooltipInfo.Keys)
		{
			if (key is ComboBoxItem comboBoxItem && comboBoxItem.ComboBoxEx == c)
			{
				return comboBoxItem;
			}
		}
		return null;
	}

	public void ShowTooltip(object sender, Point screenPosition)
	{
		ShowTooltip(sender, screenPosition, useScreenPosition: true);
	}

	public void ShowTooltip(object sender)
	{
		ShowTooltip(sender, Point.Empty, useScreenPosition: false);
	}

	public void ShowTooltip(object sender, SuperTooltipInfo info)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		if (info == null)
		{
			return;
		}
		Control val = (Control)((sender is Control) ? sender : null);
		if ((val != null && val.Focused && !ShowTooltipForFocusedControl) || !IsFormActive(sender))
		{
			return;
		}
		if (sender is ComboBoxEx)
		{
			BaseItem comboBoxItemFromComboBoxEx = GetComboBoxItemFromComboBoxEx((ComboBoxEx)sender);
			if (comboBoxItemFromComboBoxEx != null)
			{
				sender = comboBoxItemFromComboBoxEx;
			}
		}
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = Rectangle.Empty;
		bool flag = false;
		if (sender is Control)
		{
			Control val2 = (Control)sender;
			rectangle = new Rectangle(val2.PointToScreen(Point.Empty), val2.Size);
			rect = rectangle;
			flag = (int)val2.RightToLeft == 1;
		}
		if (rectangle.IsEmpty)
		{
			return;
		}
		HideTooltip();
		Point point = new Point(Control.MousePosition.X, Control.MousePosition.Y + (int)((float)SystemInformation.CursorSize.Height * 0.6f));
		point.Y = Math.Max(rectangle.Bottom + 1, point.Y);
		Tooltip = new SuperTooltipControl();
		Tooltip.ShowTooltipDescription = true;
		Tooltip.MaximumWidth = 0;
		Tooltip.MouseActivateEnabled = false;
		if (DefaultFont != null)
		{
			((Control)Tooltip).Font = DefaultFont;
		}
		if (flag)
		{
			((Control)Tooltip).RightToLeft = (RightToLeft)1;
		}
		Tooltip.MinimumTooltipSize = MinimumTooltipSize;
		Tooltip.AntiAlias = true;
		Size empty = Size.Empty;
		if (!info.CustomSize.IsEmpty && info.CustomSize.Width > 0 && info.CustomSize.Height > 0)
		{
			empty = info.CustomSize;
		}
		else
		{
			Tooltip.UpdateWithSuperTooltipInfo(info);
			Tooltip.UpdateSuperTooltipSize(info);
			empty = ((Control)Tooltip).Size;
		}
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(point);
		if (screenInformation != null)
		{
			Rectangle rectangle2 = new Rectangle(point, empty);
			if (rectangle2.Right > screenInformation.WorkingArea.Right)
			{
				rectangle2.X -= rectangle2.Right - screenInformation.WorkingArea.Right;
			}
			if (rectangle2.Bottom > screenInformation.Bounds.Bottom)
			{
				rectangle2.Y = screenInformation.Bounds.Bottom - rectangle2.Height;
			}
			point = rectangle2.Location;
		}
		if (!rect.IsEmpty)
		{
			Rectangle rectangle3 = new Rectangle(point, empty);
			if (rectangle3.IntersectsWith(rect))
			{
				if (rect.Y - rectangle3.Height >= 0)
				{
					rectangle3.Y = rect.Y - rectangle3.Height;
				}
				else if (rect.X - rectangle3.Width >= 0)
				{
					rectangle3.Width = rect.X - rectangle3.Width;
				}
				point = rectangle3.Location;
			}
		}
		if (this.BeforeTooltipDisplay != null)
		{
			SuperTooltipEventArgs superTooltipEventArgs = new SuperTooltipEventArgs(sender, info, point);
			this.BeforeTooltipDisplay(this, superTooltipEventArgs);
			if (superTooltipEventArgs.Cancel)
			{
				((Component)(object)Tooltip).Dispose();
				Tooltip = null;
				return;
			}
			point = superTooltipEventArgs.Location;
		}
		CurrentSuperTooltipHyperlink = info.Hyperlink;
		Tooltip.ShowTooltip(info, point.X, point.Y, enforceScreenPosition: false);
		DurationDisplayed = 0L;
		CreateActiveWindowTimer();
	}

	private void ShowTooltip(object sender, Point screenPosition, bool useScreenPosition)
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Invalid comparison between Unknown and I4
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Invalid comparison between Unknown and I4
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Invalid comparison between Unknown and I4
		Control val = (Control)((sender is Control) ? sender : null);
		if ((val != null && val.Focused && !ShowTooltipForFocusedControl) || !IsFormActive(sender))
		{
			return;
		}
		if (sender is ComboBoxEx)
		{
			BaseItem comboBoxItemFromComboBoxEx = GetComboBoxItemFromComboBoxEx((ComboBoxEx)sender);
			if (comboBoxItemFromComboBoxEx != null)
			{
				sender = comboBoxItemFromComboBoxEx;
			}
		}
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = Rectangle.Empty;
		bool flag = false;
		if (sender is BaseItem)
		{
			BaseItem baseItem = (BaseItem)sender;
			Control containerControl = GetContainerControl(baseItem);
			if (baseItem is PopupItem && baseItem.Expanded && (containerControl is RibbonBar || containerControl is RibbonStrip))
			{
				return;
			}
			Control val2 = containerControl;
			if (val2 != null)
			{
				rectangle = (((val2 is RibbonStrip && ((RibbonStrip)(object)val2).QuickToolbarItems.Contains(baseItem)) || !(val2 is RibbonBar)) ? new Rectangle(val2.PointToScreen(baseItem.DisplayRectangle.Location), baseItem.DisplayRectangle.Size) : new Rectangle(val2.PointToScreen(Point.Empty), val2.Size));
				rect = new Rectangle(val2.PointToScreen(baseItem.DisplayRectangle.Location), baseItem.DisplayRectangle.Size);
				flag = (int)containerControl.RightToLeft == 1;
			}
		}
		else if (sender is Control)
		{
			Control val3 = (Control)sender;
			rectangle = new Rectangle(val3.PointToScreen(Point.Empty), val3.Size);
			rect = rectangle;
			flag = (int)val3.RightToLeft == 1;
		}
		else if (sender is TabItem)
		{
			TabItem tabItem = (TabItem)sender;
			TabStrip parent = tabItem.Parent;
			rectangle = new Rectangle(((Control)parent).PointToScreen(Point.Empty), ((Control)parent).Size);
			rect = rectangle;
			flag = (int)((Control)parent).RightToLeft == 1;
		}
		else if (sender is ISuperTooltipInfoProvider)
		{
			ISuperTooltipInfoProvider superTooltipInfoProvider = (ISuperTooltipInfoProvider)sender;
			rectangle = superTooltipInfoProvider.ComponentRectangle;
			rect = rectangle;
		}
		else if (sender is ToolStripButton)
		{
			ToolStripButton val4 = (ToolStripButton)sender;
			rectangle = ((ToolStripItem)val4).Bounds;
			rect = ((ToolStripItem)val4).Bounds;
		}
		if (rectangle.IsEmpty)
		{
			return;
		}
		SuperTooltipInfo superTooltipInfo = GetSuperTooltipInfo(sender);
		if (superTooltipInfo == null && sender is BaseItem)
		{
			foreach (object key in this.superTooltipInfo.Keys)
			{
				if (key is BaseItem baseItem2 && baseItem2.Name == ((BaseItem)sender).Name)
				{
					superTooltipInfo = this.superTooltipInfo[key] as SuperTooltipInfo;
					break;
				}
			}
		}
		if (superTooltipInfo == null)
		{
			return;
		}
		HideTooltip();
		Point point = new Point(Control.MousePosition.X, Control.MousePosition.Y + (int)((float)SystemInformation.CursorSize.Height * 0.6f));
		point.Y = Math.Max(rectangle.Bottom + 1, point.Y);
		if (useScreenPosition)
		{
			point = screenPosition;
		}
		Tooltip = new SuperTooltipControl();
		Tooltip.ShowTooltipDescription = true;
		Tooltip.MaximumWidth = 0;
		Tooltip.MouseActivateEnabled = false;
		((Control)Tooltip).MouseEnter += SuperTooltip_MouseEnter;
		((Control)Tooltip).MouseLeave += SuperTooltip_MouseLeave;
		if (DefaultFont != null)
		{
			((Control)Tooltip).Font = DefaultFont;
		}
		if (flag)
		{
			((Control)Tooltip).RightToLeft = (RightToLeft)1;
		}
		Tooltip.MinimumTooltipSize = MinimumTooltipSize;
		Tooltip.AntiAlias = true;
		Size empty = Size.Empty;
		if (!superTooltipInfo.CustomSize.IsEmpty && superTooltipInfo.CustomSize.Width > 0 && superTooltipInfo.CustomSize.Height > 0)
		{
			empty = superTooltipInfo.CustomSize;
		}
		else
		{
			Tooltip.UpdateWithSuperTooltipInfo(superTooltipInfo);
			Tooltip.UpdateSuperTooltipSize(superTooltipInfo);
			empty = ((Control)Tooltip).Size;
		}
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(point);
		if (screenInformation != null)
		{
			Rectangle rectangle2 = new Rectangle(point, empty);
			if (rectangle2.Right > screenInformation.WorkingArea.Right)
			{
				rectangle2.X -= rectangle2.Right - screenInformation.WorkingArea.Right;
			}
			if (rectangle2.Bottom > screenInformation.Bounds.Bottom)
			{
				rectangle2.Y = screenInformation.Bounds.Bottom - rectangle2.Height;
			}
			point = rectangle2.Location;
		}
		if (!rect.IsEmpty)
		{
			Rectangle rectangle3 = new Rectangle(point, empty);
			if (rectangle3.IntersectsWith(rect))
			{
				if (rect.Y - rectangle3.Height >= 0)
				{
					rectangle3.Y = rect.Y - rectangle3.Height;
				}
				else if (rect.X - rectangle3.Width >= 0)
				{
					rectangle3.Width = rect.X - rectangle3.Width;
				}
				point = rectangle3.Location;
			}
		}
		if (this.BeforeTooltipDisplay != null)
		{
			SuperTooltipEventArgs superTooltipEventArgs = new SuperTooltipEventArgs(sender, superTooltipInfo, point);
			this.BeforeTooltipDisplay(this, superTooltipEventArgs);
			if (superTooltipEventArgs.Cancel)
			{
				((Component)(object)Tooltip).Dispose();
				Tooltip = null;
				return;
			}
			point = superTooltipEventArgs.Location;
		}
		CurrentSuperTooltipHyperlink = superTooltipInfo.Hyperlink;
		Tooltip.ShowTooltip(superTooltipInfo, point.X, point.Y, enforceScreenPosition: false);
		DurationDisplayed = 0L;
		CreateActiveWindowTimer();
	}

	private SuperTooltipInfo GetSuperTooltipInfo(object sender)
	{
		if (sender is TextBoxX && !superTooltipInfo.Contains(sender))
		{
			foreach (object key in superTooltipInfo.Keys)
			{
				if (key is TextBoxItem && ((TextBoxItem)key).TextBox == sender)
				{
					return superTooltipInfo[key] as SuperTooltipInfo;
				}
			}
		}
		else if (sender is ComboBoxEx && !superTooltipInfo.Contains(sender))
		{
			foreach (object key2 in superTooltipInfo.Keys)
			{
				if (key2 is ComboBoxItem && ((ComboBoxItem)key2).ComboBoxEx == sender)
				{
					return superTooltipInfo[key2] as SuperTooltipInfo;
				}
			}
		}
		return superTooltipInfo[sender] as SuperTooltipInfo;
	}

	private bool IsFormActive(object sender)
	{
		Control val = null;
		if (sender is Control)
		{
			val = (Control)((sender is Control) ? sender : null);
		}
		else if (sender is BaseItem)
		{
			val = GetContainerControl((BaseItem)sender);
		}
		if (val != null)
		{
			Form val2 = val.FindForm();
			if (val2 != null && val2.IsMdiChild && val2.MdiParent.ActiveMdiChild == val2)
			{
				return true;
			}
			if (val2 != null && Form.ActiveForm != val2)
			{
				return false;
			}
		}
		return true;
	}

	private void SuperTooltip_MouseLeave(object sender, EventArgs e)
	{
		m_MouseOverSuperTooltip = false;
		HideTooltip();
	}

	private void SuperTooltip_MouseEnter(object sender, EventArgs e)
	{
		m_MouseOverSuperTooltip = true;
	}

	public void HideTooltip()
	{
		HoverCount = 0;
		DestroyActiveWindowTimer();
		if (Tooltip != null)
		{
			CurrentSuperTooltipHyperlink = null;
			((Control)Tooltip).Hide();
			((Component)(object)Tooltip).Dispose();
			Tooltip = null;
		}
		m_MouseOverSuperTooltip = false;
	}

	private void CreateActiveWindowTimer()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (ActiveWindowTimer != null || m_TimerOperation)
		{
			return;
		}
		m_TimerOperation = true;
		try
		{
			ActiveWindowPtr = NativeFunctions.GetActiveWindow();
			ActiveWindowTimer = new Timer();
			ActiveWindowTimer.Interval = 300;
			ActiveWindowTimer.Tick += ActiveWindowTimerTick;
			ActiveWindowTimer.Start();
		}
		finally
		{
			m_TimerOperation = false;
		}
	}

	private void ActiveWindowTimerTick(object sender, EventArgs e)
	{
		DurationDisplayed += ActiveWindowTimer.Interval;
		if (ActiveWindowPtr != IntPtr.Zero)
		{
			IntPtr activeWindow = NativeFunctions.GetActiveWindow();
			if (activeWindow != ActiveWindowPtr)
			{
				ActiveWindowTimer.Stop();
				HideTooltip();
				return;
			}
		}
		if (DurationDisplayed > 20000 && !m_MouseOverSuperTooltip)
		{
			ActiveWindowTimer.Stop();
			HideTooltip();
		}
	}

	private void DestroyActiveWindowTimer()
	{
		if (m_TimerOperation)
		{
			return;
		}
		m_TimerOperation = true;
		try
		{
			if (ActiveWindowTimer != null)
			{
				ActiveWindowTimer.Enabled = false;
				ActiveWindowTimer.Stop();
				ActiveWindowTimer.Tick -= ActiveWindowTimerTick;
				((Component)(object)ActiveWindowTimer).Dispose();
				ActiveWindowTimer = null;
			}
		}
		finally
		{
			m_TimerOperation = false;
		}
	}

	private void ResetHover(Control c)
	{
		if (c != null && BarFunctions.IsHandleValid(c))
		{
			NativeFunctions.TRACKMOUSEEVENT tme = default(NativeFunctions.TRACKMOUSEEVENT);
			tme.dwFlags = 1073741824u;
			tme.hwndTrack = c.Handle;
			tme.cbSize = Marshal.SizeOf(tme);
			NativeFunctions.TrackMouseEvent(ref tme);
			tme.dwFlags |= 1u;
			NativeFunctions.TrackMouseEvent(ref tme);
		}
	}

	private bool IsTreeGXType(Type type)
	{
		if (!(type.BaseType.FullName == "Stimulsoft.Controls.Win.Tree.Node"))
		{
			return type.FullName == "Stimulsoft.Controls.Win.Tree.Node";
		}
		return true;
	}

	public bool CanExtend(object extendee)
	{
		if (extendee is Control || extendee is BaseItem || extendee is ISuperTooltipInfoProvider || IsTreeGXType(extendee.GetType()) || extendee is TabItem)
		{
			return true;
		}
		if (extendee is ToolStripButton)
		{
			return true;
		}
		return false;
	}
}
