using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
[ComVisible(false)]
public class MetroTabStrip : ItemControl
{
	private MetroStripContainerItem _StripContainer;

	private int _CaptionHeight;

	private bool _CaptionVisible;

	private Rectangle _CaptionBounds = Rectangle.Empty;

	private Rectangle _QuickToolbarBounds = Rectangle.Empty;

	private Rectangle _SystemCaptionItemBounds = Rectangle.Empty;

	private Font _CaptionFont;

	private bool _CanCustomize = true;

	private ElementStyle _DefaultBackgroundStyle = new ElementStyle();

	private bool _KeyTipsEnabled = true;

	private string _TitleText = "";

	private BodyElement _TitleTextMarkup;

	internal Rectangle TitleTextMarkupLastArrangeBounds = Rectangle.Empty;

	private bool _AltFocus;

	private IKeyTipsControl _AltFocusedControl;

	private bool _MouseDownOnCaption;

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return _TitleText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_TitleText = value;
			_TitleTextMarkup = null;
			if (MarkupParser.IsMarkup(ref _TitleText))
			{
				_TitleTextMarkup = MarkupParser.Parse(_TitleText);
				if (_TitleTextMarkup != null)
				{
					_TitleTextMarkup.HyperLinkClick += InternalTitleTextMarkupLinkClick;
				}
				TitleTextMarkupLastArrangeBounds = Rectangle.Empty;
				((Control)this).Invalidate();
			}
		}
	}

	internal BodyElement TitleTextMarkupBody => _TitleTextMarkup;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return _KeyTipsEnabled;
		}
		set
		{
			_KeyTipsEnabled = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Customization")]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	public bool CanCustomize
	{
		get
		{
			return _CanCustomize;
		}
		set
		{
			_CanCustomize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates explicit height of the caption provided by control")]
	public int CaptionHeight
	{
		get
		{
			return _CaptionHeight;
		}
		set
		{
			_CaptionHeight = value;
			_StripContainer.NeedRecalcSize = true;
		}
	}

	internal bool HasVisibleTabs
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal Rectangle CaptionBounds
	{
		get
		{
			return _CaptionBounds;
		}
		set
		{
			_CaptionBounds = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle QuickToolbarBounds
	{
		get
		{
			return _QuickToolbarBounds;
		}
		set
		{
			_QuickToolbarBounds = value;
		}
	}

	internal Rectangle SystemCaptionItemBounds
	{
		get
		{
			return _SystemCaptionItemBounds;
		}
		set
		{
			_SystemCaptionItemBounds = value;
		}
	}

	internal SystemCaptionItem SystemCaptionItem => _StripContainer.SystemCaptionItem;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether custom caption line provided by the control is visible.")]
	public bool CaptionVisible
	{
		get
		{
			return _CaptionVisible;
		}
		set
		{
			_CaptionVisible = value;
			OnCaptionVisibleChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("")]
	public Font CaptionFont
	{
		get
		{
			return _CaptionFont;
		}
		set
		{
			_CaptionFont = value;
			_StripContainer.NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	internal eDotNetBarStyle EffectiveStyle => _StripContainer.EffectiveStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Metro)]
	public eDotNetBarStyle Style
	{
		get
		{
			return _StripContainer.Style;
		}
		set
		{
			base.ColorScheme.Style = value;
			_StripContainer.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _StripContainer.RibbonStripContainer.SubItems;

	[Browsable(false)]
	public MetroTabItem SelectedTab
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is MetroTabItem && ((MetroTabItem)item).Checked)
				{
					return (MetroTabItem)item;
				}
			}
			return null;
		}
	}

	internal bool IsMaximized
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return (int)val.WindowState == 2;
			}
			return false;
		}
	}

	internal override bool IsGlassEnabled => false;

	[Browsable(false)]
	public bool IsInKeyTipsMode => _AltFocus | ShowKeyTips;

	internal bool MouseDownOnCaption => _MouseDownOnCaption;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => _StripContainer.CaptionContainer.SubItems;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public GenericItemContainer CaptionContainerItem => _StripContainer.CaptionContainer;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public GenericItemContainer StripContainerItem => _StripContainer.RibbonStripContainer;

	internal MetroStripContainerItem MetroStripContainer => _StripContainer;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;

	public MetroTabStrip()
	{
		((Control)this).SetStyle((ControlStyles)4096, true);
		_StripContainer = new MetroStripContainerItem(this);
		_StripContainer.GlobalItem = false;
		_StripContainer.ContainerControl = this;
		_StripContainer.Displayed = true;
		_StripContainer.SetOwner(this);
		_StripContainer.Style = eDotNetBarStyle.Metro;
		SetBaseItemContainer(_StripContainer);
		base.ColorScheme.Style = eDotNetBarStyle.Office2003;
		((Control)this).AutoSize = true;
		_StripContainer.SystemCaptionItem.Click += SystemCaptionClick;
	}

	private void InternalTitleTextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink && this.TitleTextMarkupLinkClick != null)
		{
			this.TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	private void OnCaptionVisibleChanged()
	{
		_StripContainer.NeedRecalcSize = true;
		RecalcLayout();
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (IsGlassEnabled && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - SystemInformation.CaptionButtonSize.Width * 3, 0, SystemInformation.CaptionButtonSize.Width * 3, SystemInformation.CaptionButtonSize.Height + 6);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (_CaptionBounds.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (CaptionVisible && !IsMaximized && new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (BarFunctions.IsWindows7 && IsMaximized && CaptionBounds.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}

	protected override ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return base.GetColorScheme();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		base.PaintControlBackground(pa);
		_QuickToolbarBounds = Rectangle.Empty;
		_CaptionBounds = Rectangle.Empty;
		_SystemCaptionItemBounds = Rectangle.Empty;
		MetroRender.Paint(this, pa);
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return _DefaultBackgroundStyle;
	}

	internal void InitDefaultStyles()
	{
		_DefaultBackgroundStyle = MetroRender.GetColorTable().MetroTab.TabStrip.BackgroundStyle;
	}

	internal ElementStyle InternalGetBackgroundStyle()
	{
		return GetBackgroundStyle();
	}

	internal Rectangle GetItemContainerBounds()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (_CaptionVisible)
		{
			itemContainerRectangle.Y += GetAbsoluteCaptionHeight();
		}
		return itemContainerRectangle;
	}

	internal Rectangle GetCaptionContainerBounds()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (IsGlassEnabled)
		{
			itemContainerRectangle.Y += 3;
		}
		return new Rectangle(itemContainerRectangle.X, itemContainerRectangle.Y, itemContainerRectangle.Width, GetAbsoluteCaptionHeight());
	}

	public int GetCaptionHeight()
	{
		if (_CaptionHeight == 0)
		{
			return 25;
		}
		return _CaptionHeight;
	}

	internal int GetAbsoluteCaptionHeight()
	{
		return GetCaptionHeight();
	}

	internal int GetTotalCaptionHeight()
	{
		return GetCaptionHeight();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_StripContainer.NeedRecalcSize)
		{
			RecalcSize();
		}
		InitDefaultStyles();
		base.OnPaint(e);
	}

	protected override void RecalcSize()
	{
		_CaptionBounds = Rectangle.Empty;
		_SystemCaptionItemBounds = Rectangle.Empty;
		_QuickToolbarBounds = Rectangle.Empty;
		InitDefaultStyles();
		base.RecalcSize();
	}

	public override int GetAutoSizeHeight()
	{
		return base.GetAutoSizeHeight();
	}

	protected override bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Location = ((Control)this).PointToScreen(displayRectangle.Location);
		MetroShell metroShell = ((Control)this).Parent as MetroShell;
		if (((Control)this).Parent is MetroShell)
		{
			displayRectangle = ((Control)this).Parent.DisplayRectangle;
			displayRectangle.Location = ((Control)this).Parent.PointToScreen(displayRectangle.Location);
			if (metroShell.SelectedTab != null && metroShell.SelectedTab.Panel != null && ((IKeyTipsControl)metroShell.SelectedTab.Panel).ShowKeyTips)
			{
				return false;
			}
		}
		if (metroShell != null && !metroShell.MouseWheelTabScrollEnabled)
		{
			return false;
		}
		Point mousePosition = Control.MousePosition;
		bool flag = true;
		Form val = ((Control)this).FindForm();
		if (val != null && !BarFunctions.IsFormActive(val))
		{
			flag = false;
		}
		if (flag && displayRectangle.Contains(mousePosition) && !ShowKeyTips && Items.Count > 0)
		{
			IntPtr intPtr = NativeFunctions.WindowFromPoint(new NativeFunctions.POINT(mousePosition));
			Control val2 = Control.FromChildHandle(intPtr);
			if (val2 == null)
			{
				val2 = Control.FromHandle(intPtr);
			}
			if (val2 is MetroTabStrip || val2 is MetroShell || val2 is MetroTabPanel)
			{
				MetroTabItem selectedTab = SelectedTab;
				int num = 0;
				int num2 = Items.Count - 1;
				int num3 = 1;
				if (wParam.ToInt64() > 0)
				{
					num3 = -1;
					num2 = 0;
				}
				if (selectedTab != null)
				{
					num = Items.IndexOf(selectedTab) + num3;
					if (num3 < 0 && num < 0)
					{
						return false;
					}
					if (num == Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = Items.Count - 1;
					}
				}
				int num4 = num - num3;
				do
				{
					num4 += num3;
					if (num4 < 0 || num4 > Items.Count - 1)
					{
						break;
					}
					if (Items[num4] is MetroTabItem && Items[num4].Visible)
					{
						((MetroTabItem)Items[num4]).Checked = true;
						return true;
					}
				}
				while (num4 != num2);
			}
		}
		return false;
	}

	public override ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, arrayList);
		MetroShell metroTab = GetMetroTab();
		if (metroTab != null && metroTab.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByName(metroTab.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType);
		MetroShell metroTab = GetMetroTab();
		if (metroTab != null && metroTab.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(metroTab.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		MetroShell metroTab = GetMetroTab();
		if (metroTab != null && metroTab.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(metroTab.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public override BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		MetroShell metroTab = GetMetroTab();
		if (metroTab != null && metroTab.GlobalContextMenuBar != null)
		{
			return BarFunctions.GetSubItemByName(metroTab.GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	internal void OnRibbonBarShowKeyTips(RibbonBar bar)
	{
		if (!_AltFocus)
		{
			_AltFocus = true;
		}
		if (_AltFocusedControl != ((Control)bar).Parent && ((Control)bar).Parent is RibbonPanel)
		{
			if (_AltFocusedControl != null)
			{
				_AltFocusedControl.ShowKeyTips = false;
			}
			_AltFocusedControl = ((Control)bar).Parent as IKeyTipsControl;
			if (_AltFocusedControl != null)
			{
				_AltFocusedControl.ShowKeyTips = true;
			}
		}
	}

	protected override void OnShowKeyTipsChanged()
	{
		MetroShell metroTab = GetMetroTab();
		base.OnShowKeyTipsChanged();
		if (ShowKeyTips && !_AltFocus)
		{
			GiveKeyTipsAltFocus();
		}
	}

	internal void BackstageTabClosed(SuperTabControl tabControl)
	{
		if (_AltFocusedControl == tabControl || _AltFocusedControl == tabControl.TabStrip)
		{
			_AltFocusedControl = null;
		}
	}

	protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected I4, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Invalid comparison between Unknown and I4
		if (_AltFocus)
		{
			if (base.HasRegisteredPopups)
			{
				bool flag = base.OnKeyDown(hWnd, wParam, lParam);
				if (!base.HasRegisteredPopups)
				{
					ReleaseAltFocus();
				}
			}
			Keys val = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
			if ((int)val == 0)
			{
				int num = WinApi.ToInt(wParam);
				val = (Keys)num;
			}
			if ((int)val == 27)
			{
				bool flag2 = false;
				if (!ShowKeyTips && _AltFocusedControl != null && _AltFocusedControl is RibbonPanel)
				{
					flag2 = true;
				}
				ReleaseAltFocus();
				if (flag2)
				{
					GiveKeyTipsAltFocus();
				}
				return base.OnKeyDown(hWnd, wParam, lParam);
			}
			char c = CharFromInt((int)val);
			if (c == '\0')
			{
				ReleaseAltFocus();
				return true;
			}
			if ((int)val == 32 || (int)val == 40 || (int)val == 39 || (int)val == 9)
			{
				ReleaseAltFocus();
				return false;
			}
			((Control)this).ProcessMnemonic(c);
			return true;
		}
		return base.OnKeyDown(hWnd, wParam, lParam);
	}

	private char CharFromInt(int key)
	{
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(key);
			Encoding.Default.GetDecoder().GetChars(array2, 0, 1, array, 0);
		}
		catch (Exception)
		{
			return '\0';
		}
		return array[0];
	}

	protected override bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		int num = WinApi.ToInt(wParam);
		if (num == 18 || num == 121)
		{
			bool flag = true;
			if (_AltFocus)
			{
				if (num == 18 || num == 121)
				{
					flag = false;
				}
				ReleaseAltFocus();
			}
			else
			{
				Form val = ((Control)this).FindForm();
				if (val == Form.ActiveForm)
				{
					flag = false;
					GiveKeyTipsAltFocus();
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return base.OnSysKeyUp(hWnd, wParam, lParam);
	}

	private void GiveKeyTipsAltFocus()
	{
		if (_KeyTipsEnabled)
		{
			_AltFocus = true;
			ShowKeyTips = true;
			SetupActiveWindowTimer();
		}
	}

	private void ReleaseAltFocus()
	{
		if (_AltFocus)
		{
			ReleaseActiveWindowTimer();
			if (!ShowKeyTips && _AltFocusedControl != null)
			{
				_AltFocusedControl.ShowKeyTips = false;
				_AltFocusedControl = null;
			}
			ShowKeyTips = false;
		}
		_AltFocus = false;
	}

	public void ExitKeyTipsMode()
	{
		ReleaseAltFocus();
	}

	internal void OnChildItemClick(BaseItem item)
	{
		if (ShowKeyTips || (_AltFocusedControl != null && _AltFocusedControl.ShowKeyTips))
		{
			ShowKeyTips = false;
			ReleaseAltFocus();
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ShowKeyTips || (_AltFocusedControl != null && _AltFocusedControl.ShowKeyTips))
		{
			ShowKeyTips = false;
			ReleaseAltFocus();
		}
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (_AltFocusedControl != null)
		{
			if (_AltFocusedControl.ProcessMnemonicEx(charCode))
			{
				ReleaseAltFocus();
				return true;
			}
			return false;
		}
		if (!_AltFocus && (Control.ModifierKeys & 0x40000) != 262144)
		{
			return false;
		}
		return base.ProcessMnemonic(charCode);
	}

	public override bool ProcessMnemonicEx(char charCode)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		BaseItem baseItem = null;
		if (_CaptionVisible)
		{
			baseItem = GetItemForMnemonic(_StripContainer.CaptionContainer, charCode, deepScan: false, stackKeys: true);
			if (baseItem != null && ProcessItemMnemonicKey(baseItem))
			{
				ReleaseAltFocus();
				return true;
			}
		}
		MetroShell metroTab = GetMetroTab();
		if (baseItem == null)
		{
			baseItem = GetItemForMnemonic(GetBaseItemContainer(), charCode, deepScan: true, stackKeys: true);
		}
		if (baseItem is MetroTabItem && baseItem.Visible)
		{
			if (!_AltFocus)
			{
				GiveKeyTipsAltFocus();
			}
			ShowKeyTips = false;
			if (baseItem != SelectedTab)
			{
				baseItem.RaiseClick();
			}
			if (SelectedTab != null && _KeyTipsEnabled)
			{
				if (_AltFocusedControl != null)
				{
					_AltFocusedControl.ShowKeyTips = false;
				}
				_AltFocusedControl = SelectedTab.Panel;
				if (_AltFocusedControl != null)
				{
					_AltFocusedControl.ShowKeyTips = true;
				}
			}
			return true;
		}
		if (baseItem != null && ProcessItemMnemonicKey(baseItem))
		{
			ShowKeyTips = false;
			if (baseItem is MetroAppButton)
			{
				MetroAppButton metroAppButton = (MetroAppButton)baseItem;
				if (metroAppButton.BackstageTab != null)
				{
					if (_AltFocusedControl != null)
					{
						_AltFocusedControl.ShowKeyTips = false;
					}
					_AltFocusedControl = metroAppButton.BackstageTab.TabStrip;
					if (!_AltFocusedControl.ShowKeyTips)
					{
						_AltFocusedControl.ShowKeyTips = true;
					}
				}
			}
			return true;
		}
		if (_AltFocus && SelectedTab != null && SelectedTab.Panel != null)
		{
			Control panel = (Control)(object)SelectedTab.Panel;
			foreach (Control item in (ArrangedElementCollection)panel.Controls)
			{
				Control val = item;
				if (val is RibbonBar && val.Visible && Control.IsMnemonic(charCode, val.Text))
				{
					ShowKeyTips = false;
					((RibbonBar)(object)val).ShowKeyTips = true;
					if (_AltFocusedControl != null)
					{
						_AltFocusedControl.ShowKeyTips = false;
					}
					_AltFocusedControl = (IKeyTipsControl)val;
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnActiveWindowChanged()
	{
		ReleaseAltFocus();
		base.OnActiveWindowChanged();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return ((ContainerControl)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return base.ProcessDialogKey(keyData);
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		GetMetroTab()?.ShowCustomizeContextMenu(item, isTabStrip: false);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_TitleTextMarkup != null)
		{
			_TitleTextMarkup.MouseLeave((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	private MetroShell GetMetroTab()
	{
		return ((Control)this).Parent as MetroShell;
	}

	internal void ShowSystemMenu(Point p)
	{
		Form val = ((Control)this).FindForm();
		if (val is MetroAppForm)
		{
			((MetroAppForm)(object)val).ShowSystemWindowMenu(p);
			return;
		}
		byte[] bytes = BitConverter.GetBytes(p.X);
		byte[] bytes2 = BitConverter.GetBytes(p.Y);
		byte[] value = new byte[4]
		{
			bytes[0],
			bytes[1],
			bytes2[0],
			bytes2[1]
		};
		int lParam = BitConverter.ToInt32(value, 0);
		((Control)this).Capture = false;
		NativeFunctions.SendMessage(((Control)val).Handle, 274, NativeFunctions.TrackPopupMenu(NativeFunctions.GetSystemMenu(((Control)val).Handle, bRevert: false), 256u, p.X, p.Y, 0, ((Control)val).Handle, IntPtr.Zero), lParam);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		_MouseDownOnCaption = false;
		if (_CaptionVisible)
		{
			_MouseDownOnCaption = HitTestCaption(new Point(e.X, e.Y));
			if ((int)e.Button == 2097152 && _MouseDownOnCaption)
			{
				ShowSystemMenu(Control.MousePosition);
				return;
			}
			if (_TitleTextMarkup != null)
			{
				_TitleTextMarkup.MouseDown((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		if ((int)e.Button == 2097152 && QuickToolbarBounds.Contains(e.X, e.Y))
		{
			GetMetroTab()?.OnTabStripRightClick(this, e.X, e.Y);
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_CaptionVisible)
		{
			if (_TitleTextMarkup != null)
			{
				_TitleTextMarkup.MouseUp((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		_MouseDownOnCaption = false;
		base.OnMouseUp(e);
	}

	internal BaseItem GetApplicationButton()
	{
		if (!CaptionVisible)
		{
			return null;
		}
		BaseItem captionContainerItem = CaptionContainerItem;
		if (EffectiveStyle == eDotNetBarStyle.Office2010 && Items.Count > 0 && Items[0] is MetroAppButton)
		{
			return Items[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is MetroAppButton)
		{
			return captionContainerItem.SubItems[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ButtonItem && ((ButtonItem)captionContainerItem.SubItems[0]).HotTrackingStyle == eHotTrackingStyle.Image)
		{
			return captionContainerItem.SubItems[0];
		}
		return null;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (_CaptionVisible)
		{
			if ((int)e.Button == 1048576 && _MouseDownOnCaption && HitTestCaption(new Point(e.X, e.Y)))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && (int)val.WindowState == 0)
				{
					if (GetApplicationButton() is PopupItem { Expanded: not false } popupItem)
					{
						popupItem.Expanded = false;
					}
					Point mousePosition = Control.MousePosition;
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					NativeFunctions.SendMessage(((Control)val).Handle, 274, 61458, lParam);
					_MouseDownOnCaption = false;
					return;
				}
			}
			if (_TitleTextMarkup != null)
			{
				_TitleTextMarkup.MouseMove((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		base.OnMouseMove(e);
	}

	protected override void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (_CaptionVisible)
		{
			MouseEventArgs e = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
			e = TranslateMouseEventArgs(e);
			mousePos = new Point(e.X, e.Y);
		}
		base.InternalOnClick(mb, mousePos);
	}

	private MouseEventArgs TranslateMouseEventArgs(MouseEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		if (e.Y <= 6)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 2 && val is RibbonForm)
			{
				if (e.X > 4)
				{
					e = ((e.X < ((Control)this).Width - 6) ? new MouseEventArgs(e.Button, e.Clicks, e.X, CaptionContainerItem.TopInternal + 5, e.Delta) : new MouseEventArgs(e.Button, e.Clicks, SystemCaptionItem.DisplayRectangle.Right - 4, SystemCaptionItem.DisplayRectangle.Top + 4, e.Delta));
				}
				else
				{
					BaseItem applicationButton = GetApplicationButton();
					if (applicationButton != null)
					{
						e = new MouseEventArgs(e.Button, e.Clicks, applicationButton.LeftInternal + 1, applicationButton.TopInternal + 1, e.Delta);
					}
				}
			}
		}
		return e;
	}

	protected override void OnClick(EventArgs e)
	{
		if (_CaptionVisible && _TitleTextMarkup != null)
		{
			_TitleTextMarkup.Click((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		if (_CaptionVisible)
		{
			Point p = ((Control)this).PointToClient(Control.MousePosition);
			if (HitTestCaption(p))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.MaximizeBox && ((int)val.FormBorderStyle == 4 || (int)val.FormBorderStyle == 6))
				{
					if ((int)val.WindowState == 0)
					{
						NativeFunctions.PostMessage(((Control)val).Handle, 274, 61488, 0);
					}
					else if ((int)val.WindowState == 2)
					{
						NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}

	protected bool HitTestCaption(Point p)
	{
		return _CaptionBounds.Contains(p);
	}

	private void SystemCaptionClick(object sender, EventArgs e)
	{
		SystemCaptionItem systemCaptionItem = sender as SystemCaptionItem;
		Form val = ((Control)this).FindForm();
		if (val != null && systemCaptionItem.LastButtonClick == systemCaptionItem.MouseDownButton)
		{
			if (systemCaptionItem.LastButtonClick == SystemButton.Minimize)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61472, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Maximize)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61488, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Restore)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Close)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61536, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Help)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61824, 0);
			}
		}
	}

	internal void CloseParentForm()
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		base.OnResize(e);
		if (!_CaptionVisible)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if ((int)val.WindowState == 2 || (int)val.WindowState == 1)
			{
				_StripContainer.SystemCaptionItem.RestoreEnabled = true;
			}
			else
			{
				_StripContainer.SystemCaptionItem.RestoreEnabled = false;
			}
		}
	}

	protected override void PaintKeyTips(Graphics g)
	{
		if (ShowKeyTips)
		{
			KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			PaintContainerKeyTips(_StripContainer.RibbonStripContainer, renderer, e);
			if (_CaptionVisible)
			{
				PaintContainerKeyTips(_StripContainer.CaptionContainer, renderer, e);
			}
		}
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Rectangle keyTipRectangle = base.GetKeyTipRectangle(g, item, font, keyTip);
		if (QuickToolbarItems.Contains(item))
		{
			keyTipRectangle.Y += 4;
		}
		return keyTipRectangle;
	}

	internal void ClearMDIChildSystemItems(bool bRecalcLayout)
	{
		if (_StripContainer.RibbonStripContainer == null)
		{
			return;
		}
		bool flag = false;
		try
		{
			if (Items.Contains("dotnetbarsysiconitem"))
			{
				Items.Remove("dotnetbarsysiconitem");
				flag = true;
			}
			if (Items.Contains("dotnetbarsysmenuitem"))
			{
				Items.Remove("dotnetbarsysmenuitem");
				flag = true;
			}
			if (bRecalcLayout && flag)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ShowMDIChildSystemItems(Form objMdiChild, bool bRecalcLayout)
	{
		ClearMDIChildSystemItems(bRecalcLayout);
		if (objMdiChild != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!objMdiChild.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!objMdiChild.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			if (!objMdiChild.MaximizeBox)
			{
				mDISystemItem.RestoreEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += MDISysItemClick;
			Items.Add(mDISystemItem);
			if (bRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}

	private void MDISysItemClick(object sender, EventArgs e)
	{
		MDISystemItem mDISystemItem = sender as MDISystemItem;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val = val.ActiveMdiChild;
		}
		if (val == null)
		{
			ClearMDIChildSystemItems(bRecalcLayout: true);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Minimize)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61472, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Restore)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Close)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.NextWindow)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61504, 0);
		}
	}

	internal void InvokeMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
	}

	internal void InvokeMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	internal void InvokeMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	internal void InvokeClick(EventArgs e)
	{
		((Control)this).OnClick(e);
	}
}
