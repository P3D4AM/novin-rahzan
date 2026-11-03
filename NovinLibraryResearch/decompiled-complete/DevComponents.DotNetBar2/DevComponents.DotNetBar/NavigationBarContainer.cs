using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class NavigationBarContainer : ImageItem, IDesignTimeProvider
{
	private const int _2162 = 32;

	private ButtonItem _2163;

	private int _2164 = 2;

	private int _2165 = 2;

	private int _2166 = -1;

	private bool _2167 = true;

	private bool _2168 = true;

	private bool _2169 = true;

	private bool _216A = true;

	private bool _216B = true;

	private Size _216C = new Size(16, 16);

	private bool _216D = true;

	[Browsable(false)]
	public int SummaryLineStartItemIndex => _2166;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
	[Browsable(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.Expanded)
				{
					subItem.Expanded = false;
				}
			}
			if (_2163 != null && _2163.Expanded)
			{
				_2163.Expanded = false;
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates Configure Buttons button is visible.")]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public bool ConfigureItemVisible
	{
		get
		{
			return _2167;
		}
		set
		{
			if (_2167 != value)
			{
				_2167 = value;
				NeedRecalcSize = true;
				if (!_2167 && _2163 != null)
				{
					_2163 = null;
				}
			}
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DevCoBrowsable(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return _2168;
		}
		set
		{
			if (_2168 == value)
			{
				return;
			}
			_2168 = value;
			if (_2163 != null)
			{
				if (_2163.Expanded)
				{
					_2163.Expanded = false;
				}
				_2163 = null;
				NeedRecalcSize = true;
			}
		}
	}

	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return _2169;
		}
		set
		{
			if (_2169 == value)
			{
				return;
			}
			_2169 = value;
			if (_2163 != null)
			{
				if (_2163.Expanded)
				{
					_2163.Expanded = false;
				}
				_2163 = null;
			}
			NeedRecalcSize = true;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[DevCoBrowsable(true)]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return _216A;
		}
		set
		{
			if (_216A == value)
			{
				return;
			}
			_216A = value;
			if (_2163 != null)
			{
				if (_2163.Expanded)
				{
					_2163.Expanded = false;
				}
				_2163 = null;
				NeedRecalcSize = true;
			}
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(2)]
	[DevCoBrowsable(true)]
	public int ItemPaddingTop
	{
		get
		{
			return _2164;
		}
		set
		{
			_2164 = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(2)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the bottom of the item.")]
	[Browsable(true)]
	public int ItemPaddingBottom
	{
		get
		{
			return _2165;
		}
		set
		{
			_2165 = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	[Browsable(true)]
	[Category("Appearance")]
	public bool AutoSizeButtonImage
	{
		get
		{
			return _216B;
		}
		set
		{
			if (_216B != value)
			{
				_216B = value;
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return _216C;
		}
		set
		{
			_216C = value;
			if (_216B)
			{
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether summary line is visible.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public bool SummaryLineVisible
	{
		get
		{
			return _216D;
		}
		set
		{
			if (_216D != value)
			{
				_216D = value;
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	public NavigationBarContainer()
		: this("", "")
	{
	}

	public NavigationBarContainer(string sItemName)
		: this(sItemName, "")
	{
	}

	public NavigationBarContainer(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = true;
		SubItemsImageSize = Size.Empty;
	}

	public override BaseItem Copy()
	{
		NavigationBarContainer navigationBarContainer = new NavigationBarContainer();
		CopyToItem(navigationBarContainer);
		return navigationBarContainer;
	}

	public override void RecalcSize()
	{
		m_RecalculatingSize = true;
		int num = 0;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		int num2 = 1;
		int num3 = 0;
		BaseItem baseItem = null;
		_216F(m_Rect.Bottom, 32);
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is ButtonItem)
			{
				((ButtonItem)subItem).ButtonStyle = eButtonStyle.ImageAndText;
				((ButtonItem)subItem).ImageFixedSize = Size.Empty;
			}
			if (_216B && !_216C.IsEmpty)
			{
				((ButtonItem)subItem).ImageFixedSize = _216C;
			}
			subItem.RecalcSize();
			subItem.HeightInternal += _2164 + _2165;
			if (subItem.Visible)
			{
				baseItem = subItem;
				if (subItem.HeightInternal > num)
				{
					num = subItem.HeightInternal;
				}
			}
			if (_216B && !_216C.IsEmpty)
			{
				((ButtonItem)subItem).ImageFixedSize = Size.Empty;
				subItem.RecalcSize();
				subItem.HeightInternal += _2164 + _2165;
			}
		}
		int num4 = m_Rect.Top;
		int num5 = m_Rect.Left + num3;
		int num6 = 0;
		int widthInternal = WidthInternal - num3 * 2;
		bool flag = false;
		bool flag2 = false;
		int num7 = -1;
		foreach (BaseItem subItem2 in SubItems)
		{
			if (subItem2.Visible)
			{
				if (num4 + subItem2.HeightInternal + ((baseItem != subItem2) ? num2 : 0) <= m_Rect.Bottom - (_216D ? num : 0))
				{
					subItem2.TopInternal = num4;
					subItem2.LeftInternal = num5;
					subItem2.WidthInternal = widthInternal;
					subItem2.Displayed = true;
					num4 += subItem2.HeightInternal + num2;
					num6 += subItem2.HeightInternal + num2;
					if (baseItem == subItem2)
					{
						num6 -= num2;
					}
					subItem2.DesignMarkerOrientation = eDesignMarkerOrientation.Vertical;
				}
				else if (_216D)
				{
					subItem2.DesignMarkerOrientation = eDesignMarkerOrientation.Horizontal;
					if (!flag)
					{
						num6 += num;
						flag = true;
						num7 = SubItems.IndexOf(subItem2);
					}
					if (subItem2 is ButtonItem)
					{
						((ButtonItem)subItem2).ButtonStyle = eButtonStyle.Default;
						if (_216B && !_216C.IsEmpty)
						{
							((ButtonItem)subItem2).ImageFixedSize = _216C;
						}
						subItem2.RecalcSize();
						subItem2.HeightInternal += _2164 + _2165;
						if (effectiveStyle == eDotNetBarStyle.Office2010)
						{
							subItem2.WidthInternal += 4;
						}
						if (flag2 || num5 + subItem2.WidthInternal > m_Rect.Right - _2170())
						{
							flag2 = true;
							subItem2.Displayed = false;
							continue;
						}
						subItem2.TopInternal = num4;
						subItem2.LeftInternal = num5;
						subItem2.HeightInternal = num;
						num5 += subItem2.WidthInternal;
						subItem2.Displayed = true;
					}
				}
				else
				{
					subItem2.Displayed = false;
				}
			}
			else
			{
				subItem2.Displayed = false;
			}
		}
		if (num == 0)
		{
			num = 32;
		}
		_216F(num4, num);
		if (!flag && _216D)
		{
			num6 += num;
		}
		if (num7 >= 0)
		{
			num5 = m_Rect.Right - num3;
			if (_2163 != null)
			{
				num5 -= _2163.WidthInternal;
			}
			for (int num8 = SubItems.Count - 1; num8 >= num7; num8--)
			{
				if (SubItems[num8].Displayed)
				{
					num5 -= SubItems[num8].WidthInternal;
					SubItems[num8].LeftInternal = num5;
				}
			}
		}
		_2166 = num7;
		if (num6 == 0)
		{
			num6 = 32;
		}
		m_Rect.Height = num6;
		m_RecalculatingSize = false;
	}

	private void _216E(ItemPaintArgs _0BD2, Rectangle ल, eDotNetBarStyle _0657)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		if (BarFunctions.IsOffice2007Style(_0657) && _0BD2.ໝ != null)
		{
			_0BD2.ໝ.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(_0BD2.Graphics, ल));
		}
		else if (!_0BD2.Colors.ItemBackground.IsEmpty || !_0BD2.Colors.ItemBackground2.IsEmpty)
		{
			DisplayHelp.FillRectangle(_0BD2.Graphics, ल, _0BD2.Colors.ItemBackground, _0BD2.Colors.ItemBackground2, _0BD2.Colors.ItemBackgroundGradientAngle);
		}
		else if (!_0BD2.Colors.BarBackground.IsEmpty || !_0BD2.Colors.BarBackground2.IsEmpty)
		{
			DisplayHelp.FillRectangle(_0BD2.Graphics, ल, _0BD2.Colors.BarBackground, _0BD2.Colors.BarBackground2, _0BD2.Colors.BarBackgroundGradientAngle);
		}
		if (_0657 != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(_0657))
		{
			Pen val = new Pen(_0BD2.Colors.PanelBorder, 1f);
			try
			{
				_0BD2.Graphics.DrawLine(val, ल.Left, ल.Top - 1, ल.Right, ल.Top - 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		if (m_Rect.Width == 0 || m_Rect.Height == 0)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if ((effectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(effectiveStyle)) && pa.ໝ != null)
		{
			Region clip = pa.Graphics.Clip;
			pa.Graphics.ResetClip();
			pa.ໝ.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, pa.ContainerControl.ClientRectangle));
			pa.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (_2163 != null && _216D)
		{
			Rectangle ल = new Rectangle(m_Rect.X, _2163.TopInternal, m_Rect.Width, _2163.HeightInternal);
			_216E(pa, ल, effectiveStyle);
		}
		else if (_2166 >= 0)
		{
			Rectangle ल2 = new Rectangle(m_Rect.X, SubItems[_2166].TopInternal, m_Rect.Width, SubItems[_2166].HeightInternal);
			_216E(pa, ल2, effectiveStyle);
		}
		else if (_216D)
		{
			int num = -1;
			for (int num2 = SubItems.Count - 1; num2 >= 0; num2--)
			{
				BaseItem baseItem = SubItems[num2];
				if (baseItem.Visible && baseItem.Displayed)
				{
					num = baseItem.DisplayRectangle.Bottom;
					break;
				}
			}
			if (num >= 0)
			{
				Rectangle ल3 = new Rectangle(m_Rect.X, num, m_Rect.Width, m_Rect.Bottom - num);
				_216E(pa, ल3, effectiveStyle);
				DisplayHelp.DrawLine(pa.Graphics, m_Rect.X, num, m_Rect.Right, num, pa.Colors.PanelBorder, 1);
			}
		}
		bool flag = pa.Colors.ItemBackground.IsEmpty && pa.Colors.ItemBackground2.IsEmpty && effectiveStyle != eDotNetBarStyle.Office2010;
		for (int i = 0; i < SubItems.Count; i++)
		{
			BaseItem baseItem2 = SubItems[i];
			if (!baseItem2.Visible || !baseItem2.Displayed)
			{
				continue;
			}
			if (flag)
			{
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && pa.ໝ != null)
				{
					pa.ໝ.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, baseItem2.DisplayRectangle));
				}
				else
				{
					DisplayHelp.FillRectangle(pa.Graphics, baseItem2.DisplayRectangle, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
				}
			}
			baseItem2.Paint(pa);
			if (i > 0 && effectiveStyle != eDotNetBarStyle.Office2010 && !StyleManager.IsMetro(effectiveStyle))
			{
				Pen val = new Pen(pa.Colors.PanelBorder, 1f);
				try
				{
					pa.Graphics.DrawLine(val, m_Rect.Left, baseItem2.TopInternal - 1, m_Rect.Right, baseItem2.TopInternal - 1);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		if (_2163 != null)
		{
			_2163.Paint(pa);
		}
	}

	internal void ᯞ()
	{
		if (ContainerControl is NavigationBar navigationBar)
		{
			navigationBar.ᯠ();
		}
	}

	private void _216F(int ى, int _0970)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		if (!_2167)
		{
			return;
		}
		if (_2163 == null)
		{
			LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			_2163 = new ButtonItem();
			_2163.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarConfigure.png");
			_2163.ImageFixedSize = _2163.Image.Size;
			_2163.Style = m_Style;
			_2163.ShowSubItems = false;
			_2163.SetSystemItem(b: true);
			_2163.ContainerState = eContainerState.NavigationPane;
			if (_2168)
			{
				ButtonItem buttonItem = new ButtonItem("sysShowMoreButtons");
				buttonItem.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarShowMore.png");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarShowMoreButtons);
				buttonItem.SetSystemItem(b: true);
				buttonItem.Click += _2174;
				_2163.SubItems.Add(buttonItem);
				buttonItem = new ButtonItem("sysShowFewerButtons");
				buttonItem.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarShowLess.png");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarShowFewerButtons);
				buttonItem.SetSystemItem(b: true);
				buttonItem.Click += _2177;
				_2163.SubItems.Add(buttonItem);
			}
			if (_2169)
			{
				ButtonItem buttonItem = new ButtonItem("sysNavPaneOptions");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarOptions);
				buttonItem.Click += _2179;
				buttonItem.SetSystemItem(b: true);
				_2163.SubItems.Add(buttonItem);
			}
			if (_216A)
			{
				CustomizeItem customizeItem = new CustomizeItem();
				customizeItem.Name = "sysNavPaneAddRemove";
				customizeItem.CustomizeItemVisible = false;
				_2163.SubItems.Add(customizeItem);
			}
			_2163.SetParent(this);
			_2163.Click += _2171;
			_2163.ExpandChange += _2172;
			_2163.PopupShowing += _217A;
			_2163.PopupSide = ePopupSide.Right;
			localizationManager.Dispose();
			if (ContainerControl is BarBaseControl barBaseControl && ((Control)barBaseControl).Font != null)
			{
				_2163.PopupFont = new Font(((Control)barBaseControl).Font, (FontStyle)0);
			}
			else
			{
				_2163.PopupFont = SystemFonts.MenuFont;
			}
		}
		_2163.PopupType = ePopupType.Menu;
		_2163.Displayed = true;
		_2163.HeightInternal = _0970;
		_2163.RecalcSize();
		_2163.LeftInternal = m_Rect.Right - _2163.WidthInternal;
		_2163.TopInternal = ى;
		_2163.HeightInternal = _0970;
	}

	private int _2170()
	{
		if (_2163 != null && _2167)
		{
			return _2163.WidthInternal;
		}
		return 0;
	}

	private void _2171(object _٠, EventArgs ؿ)
	{
		_2163.Expanded = !_2163.Expanded;
	}

	private void _2172(object _٠, EventArgs ؿ)
	{
		if (_2163.Expanded)
		{
			if (_2168)
			{
				_2163.SubItems["sysShowMoreButtons"].Enabled = _2166 >= 0;
				_2163.SubItems["sysShowFewerButtons"].Enabled = _2166 != 0;
			}
			bool flag = true;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem.Visible && !subItem.Displayed)
				{
					BaseItem baseItem2 = subItem.Clone() as BaseItem;
					if (flag)
					{
						baseItem2.GlobalItem = false;
						baseItem2.BeginGroup = true;
						baseItem2.Click += _2173;
						flag = false;
					}
					else
					{
						baseItem2.GlobalItem = false;
						baseItem2.BeginGroup = false;
						baseItem2.Click += _2173;
					}
					baseItem2.Tag = subItem;
					if (baseItem2 is ButtonItem)
					{
						((ButtonItem)baseItem2).ImageFixedSize = new Size(16, 16);
					}
					_2163.SubItems.Insert(_2163.SubItems.Count - 1, baseItem2);
				}
			}
			Size popupSize = _2163.PopupSize;
			Point point = new Point(_2163.DisplayRectangle.Right, _2163.DisplayRectangle.Top + _2163.DisplayRectangle.Height / 2 - popupSize.Height);
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Point point2 = val.PointToScreen(point);
				ঊ ঊ2 = BarFunctions.ॻ(val);
				if (point2.Y < ঊ2.ঌ.Top)
				{
					point.Y = val.PointToClient(ঊ2.ঌ.Location).Y;
				}
			}
			_2163.PopupLocation = point;
			return;
		}
		int heightInternal = _2163.HeightInternal;
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem subItem2 in _2163.SubItems)
		{
			if (!subItem2.SystemItem)
			{
				arrayList.Add(subItem2);
			}
		}
		_2163.SuspendLayout = true;
		foreach (BaseItem item in arrayList)
		{
			_2163.SubItems.Remove(item);
		}
		_2163.SuspendLayout = false;
		_2163.RecalcSize();
		_2163.HeightInternal = heightInternal;
	}

	private void _2173(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem { Tag: ButtonItem tag } buttonItem)
		{
			tag.Checked = true;
			buttonItem.Checked = true;
		}
	}

	private void _2174(object _٠, EventArgs ؿ)
	{
		ShowMoreButtons();
		BaseItem.CollapseAll(_٠ as BaseItem);
	}

	public void ShowMoreButtons()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			val.Height += _2175(_2176: false) + ((!DesignMode) ? 1 : 10);
		}
	}

	internal int _2175(bool _2176)
	{
		int result = 0;
		BaseItem baseItem = _2178(_2176);
		if (baseItem == null)
		{
			return result;
		}
		result = baseItem.HeightInternal;
		if (baseItem is ButtonItem && AutoSizeButtonImage && !ImageSizeSummaryLine.IsEmpty && !((ButtonItem)baseItem).ImageFixedSize.IsEmpty)
		{
			ButtonItem buttonItem = (ButtonItem)baseItem;
			Size imageFixedSize = buttonItem.ImageFixedSize;
			buttonItem.ImageFixedSize = Size.Empty;
			eButtonStyle buttonStyle = buttonItem.ButtonStyle;
			buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
			baseItem.RecalcSize();
			result = Math.Max(result, baseItem.HeightInternal + ItemPaddingBottom + ItemPaddingTop);
			buttonItem.ImageFixedSize = imageFixedSize;
			buttonItem.ButtonStyle = buttonStyle;
			buttonItem.RecalcSize();
		}
		return result;
	}

	private void _2177(object _٠, EventArgs ؿ)
	{
		ShowFewerButtons();
		BaseItem.CollapseAll(_٠ as BaseItem);
	}

	internal BaseItem _2178(bool _2176)
	{
		if (_2176 && SummaryLineStartItemIndex > 1)
		{
			return SubItems[SummaryLineStartItemIndex - 1];
		}
		if (SummaryLineStartItemIndex >= 0)
		{
			return SubItems[SummaryLineStartItemIndex];
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.Displayed)
			{
				return subItem;
			}
		}
		return null;
	}

	internal BaseItem ఽ()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.Displayed)
			{
				return subItem;
			}
		}
		return null;
	}

	public void ShowFewerButtons()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			int num = _2175(_2176: true);
			if (val.Height - num > 0)
			{
				val.Height -= num;
			}
		}
	}

	private void _2179(object _٠, EventArgs ؿ)
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Invalid comparison between Unknown and I4
		BaseItem.CollapseAll(_٠ as BaseItem);
		NavPaneOptions navPaneOptions = new NavPaneOptions();
		LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		if (localizationManager != null)
		{
			string defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogCancel);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._2182).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveDown);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._217F).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveUp);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._217E).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogOK);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._2181).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogReset);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._2180).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogTitle);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogListLabel);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions._217B).Text = defaultLocalizedString;
			}
		}
		((Form)navPaneOptions).StartPosition = (FormStartPosition)1;
		navPaneOptions.NavBarContainer = this;
		DialogResult val = ((Form)navPaneOptions).ShowDialog();
		((Component)(object)navPaneOptions).Dispose();
		if ((int)val == 1 && ContainerControl is BarBaseControl)
		{
			((BarBaseControl)ContainerControl).RecalcLayout();
		}
	}

	private void _217A(object _٠, EventArgs ؿ)
	{
		if (ContainerControl is NavigationBar && BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			((MenuPanel)(object)_2163.PopupControl).ColorScheme = ((NavigationBar)ContainerControl).GetColorScheme();
		}
		else
		{
			((MenuPanel)(object)_2163.PopupControl).ColorScheme = new ColorScheme(EffectiveStyle);
		}
	}

	public override void InternalMouseHover()
	{
		if (_2163 != null && _2163.Expanded && m_HotSubItem != _2163 && DesignMode)
		{
			_2163.Expanded = false;
		}
		base.InternalMouseHover();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (_2163 != null && _2163.Expanded && m_HotSubItem != _2163 && !DesignMode)
		{
			_2163.Expanded = false;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (_2163 == null || !_2163.DisplayRectangle.Contains(objArg.X, objArg.Y) || DesignMode)
		{
			base.InternalMouseMove(objArg);
			return;
		}
		BaseItem baseItem = _2163;
		if (baseItem != m_HotSubItem)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (baseItem != null && m_HotSubItem.Expanded)
				{
					m_HotSubItem.Expanded = false;
				}
			}
			if (baseItem != null)
			{
				if (m_AutoExpand)
				{
					BaseItem baseItem2 = ExpandedItem();
					if (baseItem2 != null && baseItem2 != baseItem)
					{
						baseItem2.Expanded = false;
					}
				}
				baseItem.InternalMouseEnter();
				baseItem.InternalMouseMove(objArg);
				if (m_AutoExpand && baseItem.Enabled && baseItem.ShowSubItems)
				{
					if (baseItem is PopupItem)
					{
						PopupItem popupItem = baseItem as PopupItem;
						ePopupAnimation popupAnimation = popupItem.PopupAnimation;
						popupItem.PopupAnimation = ePopupAnimation.None;
						baseItem.Expanded = true;
						popupItem.PopupAnimation = popupAnimation;
					}
					else
					{
						baseItem.Expanded = true;
					}
				}
				m_HotSubItem = baseItem;
			}
			else
			{
				m_HotSubItem = null;
			}
		}
		else if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseMove(objArg);
		}
	}

	protected internal override void OnSubItemExpandChange(BaseItem objItem)
	{
		base.OnSubItemExpandChange(objItem);
		if (!objItem.Expanded)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem != objItem && subItem is PopupItem && subItem.Expanded)
			{
				subItem.Expanded = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeSummaryLine()
	{
		if (_216C.Width == 16)
		{
			return _216C.Height != 16;
		}
		return true;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void RefreshImageSize()
	{
	}

	public override void OnSubItemImageSizeChanged(BaseItem objItem)
	{
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		GetIOwnerItemEvents()?.InvokeItemAdded(item, new EventArgs());
		item.ContainerState = eContainerState.NavigationPane;
		item.DesignMarkerOrientation = eDesignMarkerOrientation.Horizontal;
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		item.ContainerState = eContainerState.NotSet;
		item.DesignMarkerOrientation = eDesignMarkerOrientation.NotSet;
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		base.InternalKeyDown(objArg);
		_2189.ᡟ(this, objArg);
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}
}
