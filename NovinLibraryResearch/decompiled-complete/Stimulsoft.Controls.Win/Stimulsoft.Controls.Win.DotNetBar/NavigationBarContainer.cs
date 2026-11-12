using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class NavigationBarContainer : ImageItem, IDesignTimeProvider
{
	private ButtonItem m_ConfigureItem;

	private int m_ItemPaddingTop = 2;

	private int m_ItemPaddingBottom = 2;

	private int m_BottomLineItemStartIndex = -1;

	private bool m_ConfigureItemVisible = true;

	private bool m_ConfigureShowHideVisible = true;

	private bool m_ConfigureNavOptionsVisible = true;

	private bool m_ConfigureAddRemoveVisible = true;

	private bool m_AutoSizeButtonImage = true;

	private Size m_ImageSizeSummaryLine = new Size(16, 16);

	private const int DEFAULT_HEIGHT = 32;

	private bool m_SummaryLineVisible = true;

	[Browsable(false)]
	public int SummaryLineStartItemIndex => m_BottomLineItemStartIndex;

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
			if (m_ConfigureItem != null && m_ConfigureItem.Expanded)
			{
				m_ConfigureItem.Expanded = false;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates Configure Buttons button is visible.")]
	[DefaultValue(true)]
	public bool ConfigureItemVisible
	{
		get
		{
			return m_ConfigureItemVisible;
		}
		set
		{
			if (m_ConfigureItemVisible != value)
			{
				m_ConfigureItemVisible = value;
				NeedRecalcSize = true;
				if (!m_ConfigureItemVisible && m_ConfigureItem != null)
				{
					m_ConfigureItem = null;
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return m_ConfigureShowHideVisible;
		}
		set
		{
			if (m_ConfigureShowHideVisible == value)
			{
				return;
			}
			m_ConfigureShowHideVisible = value;
			if (m_ConfigureItem != null)
			{
				if (m_ConfigureItem.Expanded)
				{
					m_ConfigureItem.Expanded = false;
				}
				m_ConfigureItem = null;
				NeedRecalcSize = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return m_ConfigureNavOptionsVisible;
		}
		set
		{
			if (m_ConfigureNavOptionsVisible == value)
			{
				return;
			}
			m_ConfigureNavOptionsVisible = value;
			if (m_ConfigureItem != null)
			{
				if (m_ConfigureItem.Expanded)
				{
					m_ConfigureItem.Expanded = false;
				}
				m_ConfigureItem = null;
			}
			NeedRecalcSize = true;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return m_ConfigureAddRemoveVisible;
		}
		set
		{
			if (m_ConfigureAddRemoveVisible == value)
			{
				return;
			}
			m_ConfigureAddRemoveVisible = value;
			if (m_ConfigureItem != null)
			{
				if (m_ConfigureItem.Expanded)
				{
					m_ConfigureItem.Expanded = false;
				}
				m_ConfigureItem = null;
				NeedRecalcSize = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(2)]
	public int ItemPaddingTop
	{
		get
		{
			return m_ItemPaddingTop;
		}
		set
		{
			m_ItemPaddingTop = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the bottom of the item.")]
	[DefaultValue(2)]
	public int ItemPaddingBottom
	{
		get
		{
			return m_ItemPaddingBottom;
		}
		set
		{
			m_ItemPaddingBottom = value;
			NeedRecalcSize = true;
			if (DesignMode)
			{
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	[DefaultValue(true)]
	public bool AutoSizeButtonImage
	{
		get
		{
			return m_AutoSizeButtonImage;
		}
		set
		{
			if (m_AutoSizeButtonImage != value)
			{
				m_AutoSizeButtonImage = value;
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return m_ImageSizeSummaryLine;
		}
		set
		{
			m_ImageSizeSummaryLine = value;
			if (m_AutoSizeButtonImage)
			{
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether summary line is visible.")]
	public bool SummaryLineVisible
	{
		get
		{
			return m_SummaryLineVisible;
		}
		set
		{
			if (m_SummaryLineVisible != value)
			{
				m_SummaryLineVisible = value;
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
		CreateConfigureButton(m_Rect.Bottom, 32);
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is ButtonItem)
			{
				((ButtonItem)subItem).ButtonStyle = eButtonStyle.ImageAndText;
				((ButtonItem)subItem).ImageFixedSize = Size.Empty;
			}
			if (m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
			{
				((ButtonItem)subItem).ImageFixedSize = m_ImageSizeSummaryLine;
			}
			subItem.RecalcSize();
			subItem.HeightInternal += m_ItemPaddingTop + m_ItemPaddingBottom;
			if (subItem.Visible)
			{
				baseItem = subItem;
				if (subItem.HeightInternal > num)
				{
					num = subItem.HeightInternal;
				}
			}
			if (m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
			{
				((ButtonItem)subItem).ImageFixedSize = Size.Empty;
				subItem.RecalcSize();
				subItem.HeightInternal += m_ItemPaddingTop + m_ItemPaddingBottom;
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
				if (num4 + subItem2.HeightInternal + ((baseItem != subItem2) ? num2 : 0) <= m_Rect.Bottom - (m_SummaryLineVisible ? num : 0))
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
				else if (m_SummaryLineVisible)
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
						if (m_AutoSizeButtonImage && !m_ImageSizeSummaryLine.IsEmpty)
						{
							((ButtonItem)subItem2).ImageFixedSize = m_ImageSizeSummaryLine;
						}
						subItem2.RecalcSize();
						subItem2.HeightInternal += m_ItemPaddingTop + m_ItemPaddingBottom;
						if (effectiveStyle == eDotNetBarStyle.Office2010)
						{
							subItem2.WidthInternal += 4;
						}
						if (flag2 || num5 + subItem2.WidthInternal > m_Rect.Right - GetConfigureItemWidth())
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
		CreateConfigureButton(num4, num);
		if (!flag && m_SummaryLineVisible)
		{
			num6 += num;
		}
		if (num7 >= 0)
		{
			num5 = m_Rect.Right - num3;
			if (m_ConfigureItem != null)
			{
				num5 -= m_ConfigureItem.WidthInternal;
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
		m_BottomLineItemStartIndex = num7;
		if (num6 == 0)
		{
			num6 = 32;
		}
		m_Rect.Height = num6;
		m_RecalculatingSize = false;
	}

	private void PaintLineBackground(ItemPaintArgs pa, Rectangle r, eDotNetBarStyle style)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		if (BarFunctions.IsOffice2007Style(style) && pa.Renderer != null)
		{
			pa.Renderer.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, r));
		}
		else if (!pa.Colors.ItemBackground.IsEmpty || !pa.Colors.ItemBackground2.IsEmpty)
		{
			DisplayHelp.FillRectangle(pa.Graphics, r, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle);
		}
		else if (!pa.Colors.BarBackground.IsEmpty || !pa.Colors.BarBackground2.IsEmpty)
		{
			DisplayHelp.FillRectangle(pa.Graphics, r, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
		}
		if (style != eDotNetBarStyle.Office2010 && style != eDotNetBarStyle.Metro)
		{
			Pen val = new Pen(pa.Colors.PanelBorder, 1f);
			try
			{
				pa.Graphics.DrawLine(val, r.Left, r.Top - 1, r.Right, r.Top - 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		if (m_Rect.Width == 0 || m_Rect.Height == 0)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if ((effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro) && pa.Renderer != null)
		{
			Region clip = pa.Graphics.Clip;
			pa.Graphics.ResetClip();
			pa.Renderer.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, pa.ContainerControl.ClientRectangle));
			pa.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (m_ConfigureItem != null && m_SummaryLineVisible)
		{
			Rectangle r = new Rectangle(m_Rect.X, m_ConfigureItem.TopInternal, m_Rect.Width, m_ConfigureItem.HeightInternal);
			PaintLineBackground(pa, r, effectiveStyle);
		}
		else if (m_BottomLineItemStartIndex >= 0)
		{
			Rectangle r2 = new Rectangle(m_Rect.X, SubItems[m_BottomLineItemStartIndex].TopInternal, m_Rect.Width, SubItems[m_BottomLineItemStartIndex].HeightInternal);
			PaintLineBackground(pa, r2, effectiveStyle);
		}
		else if (m_SummaryLineVisible)
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
				Rectangle r3 = new Rectangle(m_Rect.X, num, m_Rect.Width, m_Rect.Bottom - num);
				PaintLineBackground(pa, r3, effectiveStyle);
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
				if (BarFunctions.IsOffice2007Style(effectiveStyle) && pa.Renderer != null)
				{
					pa.Renderer.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(pa.Graphics, baseItem2.DisplayRectangle));
				}
				else
				{
					DisplayHelp.FillRectangle(pa.Graphics, baseItem2.DisplayRectangle, pa.Colors.BarBackground, pa.Colors.BarBackground2, pa.Colors.BarBackgroundGradientAngle);
				}
			}
			baseItem2.Paint(pa);
			if (i > 0 && effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Metro)
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
		if (m_ConfigureItem != null)
		{
			m_ConfigureItem.Paint(pa);
		}
	}

	internal void OptionsDialogClosed()
	{
		if (ContainerControl is NavigationBar navigationBar)
		{
			navigationBar.InvokeOptionsDialogClosed();
		}
	}

	private void CreateConfigureButton(int y, int height)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		if (!m_ConfigureItemVisible)
		{
			return;
		}
		if (m_ConfigureItem == null)
		{
			LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			m_ConfigureItem = new ButtonItem();
			m_ConfigureItem.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarConfigure.png");
			m_ConfigureItem.ImageFixedSize = m_ConfigureItem.Image.Size;
			m_ConfigureItem.Style = m_Style;
			m_ConfigureItem.ShowSubItems = false;
			m_ConfigureItem.SetSystemItem(b: true);
			m_ConfigureItem.ContainerState = eContainerState.NavigationPane;
			if (m_ConfigureShowHideVisible)
			{
				ButtonItem buttonItem = new ButtonItem("sysShowMoreButtons");
				buttonItem.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarShowMore.png");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarShowMoreButtons);
				buttonItem.SetSystemItem(b: true);
				buttonItem.Click += OnShowMoreButtonsClick;
				m_ConfigureItem.SubItems.Add(buttonItem);
				buttonItem = new ButtonItem("sysShowFewerButtons");
				buttonItem.Image = (Image)new Bitmap(typeof(DotNetBarManager), "SystemImages.NavBarShowLess.png");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarShowFewerButtons);
				buttonItem.SetSystemItem(b: true);
				buttonItem.Click += OnShowFewerButtonsClick;
				m_ConfigureItem.SubItems.Add(buttonItem);
			}
			if (m_ConfigureNavOptionsVisible)
			{
				ButtonItem buttonItem = new ButtonItem("sysNavPaneOptions");
				buttonItem.Text = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarOptions);
				buttonItem.Click += OnNavPaneOptionsClick;
				buttonItem.SetSystemItem(b: true);
				m_ConfigureItem.SubItems.Add(buttonItem);
			}
			if (m_ConfigureAddRemoveVisible)
			{
				CustomizeItem customizeItem = new CustomizeItem();
				customizeItem.Name = "sysNavPaneAddRemove";
				customizeItem.CustomizeItemVisible = false;
				m_ConfigureItem.SubItems.Add(customizeItem);
			}
			m_ConfigureItem.SetParent(this);
			m_ConfigureItem.Click += ConfigureItemClick;
			m_ConfigureItem.ExpandChange += ConfigureExpandedChanged;
			m_ConfigureItem.PopupShowing += ConfigurePopupShowing;
			m_ConfigureItem.PopupSide = ePopupSide.Right;
			localizationManager.Dispose();
			if (ContainerControl is BarBaseControl barBaseControl && ((Control)barBaseControl).Font != null)
			{
				m_ConfigureItem.PopupFont = new Font(((Control)barBaseControl).Font, (FontStyle)0);
			}
			else
			{
				m_ConfigureItem.PopupFont = SystemFonts.MenuFont;
			}
		}
		m_ConfigureItem.PopupType = ePopupType.Menu;
		m_ConfigureItem.Displayed = true;
		m_ConfigureItem.HeightInternal = height;
		m_ConfigureItem.RecalcSize();
		m_ConfigureItem.LeftInternal = m_Rect.Right - m_ConfigureItem.WidthInternal;
		m_ConfigureItem.TopInternal = y;
		m_ConfigureItem.HeightInternal = height;
	}

	private int GetConfigureItemWidth()
	{
		if (m_ConfigureItem != null && m_ConfigureItemVisible)
		{
			return m_ConfigureItem.WidthInternal;
		}
		return 0;
	}

	private void ConfigureItemClick(object sender, EventArgs e)
	{
		m_ConfigureItem.Expanded = !m_ConfigureItem.Expanded;
	}

	private void ConfigureExpandedChanged(object sender, EventArgs e)
	{
		if (m_ConfigureItem.Expanded)
		{
			if (m_ConfigureShowHideVisible)
			{
				m_ConfigureItem.SubItems["sysShowMoreButtons"].Enabled = m_BottomLineItemStartIndex >= 0;
				m_ConfigureItem.SubItems["sysShowFewerButtons"].Enabled = m_BottomLineItemStartIndex != 0;
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
						baseItem2.Click += ButtonCopyClick;
						flag = false;
					}
					else
					{
						baseItem2.GlobalItem = false;
						baseItem2.BeginGroup = false;
						baseItem2.Click += ButtonCopyClick;
					}
					baseItem2.Tag = subItem;
					if (baseItem2 is ButtonItem)
					{
						((ButtonItem)baseItem2).ImageFixedSize = new Size(16, 16);
					}
					m_ConfigureItem.SubItems.Insert(m_ConfigureItem.SubItems.Count - 1, baseItem2);
				}
			}
			Size popupSize = m_ConfigureItem.PopupSize;
			Point point = new Point(m_ConfigureItem.DisplayRectangle.Right, m_ConfigureItem.DisplayRectangle.Top + m_ConfigureItem.DisplayRectangle.Height / 2 - popupSize.Height);
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Point point2 = val.PointToScreen(point);
				ScreenInformation screenInformation = BarFunctions.ScreenFromControl(val);
				if (point2.Y < screenInformation.WorkingArea.Top)
				{
					point.Y = val.PointToClient(screenInformation.WorkingArea.Location).Y;
				}
			}
			m_ConfigureItem.PopupLocation = point;
			return;
		}
		int heightInternal = m_ConfigureItem.HeightInternal;
		ArrayList arrayList = new ArrayList();
		foreach (BaseItem subItem2 in m_ConfigureItem.SubItems)
		{
			if (!subItem2.SystemItem)
			{
				arrayList.Add(subItem2);
			}
		}
		m_ConfigureItem.SuspendLayout = true;
		foreach (BaseItem item in arrayList)
		{
			m_ConfigureItem.SubItems.Remove(item);
		}
		m_ConfigureItem.SuspendLayout = false;
		m_ConfigureItem.RecalcSize();
		m_ConfigureItem.HeightInternal = heightInternal;
	}

	private void ButtonCopyClick(object sender, EventArgs e)
	{
		if (sender is ButtonItem { Tag: ButtonItem tag } buttonItem)
		{
			tag.Checked = true;
			buttonItem.Checked = true;
		}
	}

	private void OnShowMoreButtonsClick(object sender, EventArgs e)
	{
		ShowMoreButtons();
		BaseItem.CollapseAll(sender as BaseItem);
	}

	public void ShowMoreButtons()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			val.Height += GetChangeSizeHeight(reduceSize: false) + 1;
		}
	}

	internal int GetChangeSizeHeight(bool reduceSize)
	{
		int result = 0;
		BaseItem resizeReferenceItem = GetResizeReferenceItem(reduceSize);
		if (resizeReferenceItem == null)
		{
			return result;
		}
		result = resizeReferenceItem.HeightInternal;
		if (resizeReferenceItem is ButtonItem && AutoSizeButtonImage && !ImageSizeSummaryLine.IsEmpty && !((ButtonItem)resizeReferenceItem).ImageFixedSize.IsEmpty)
		{
			Size imageFixedSize = ((ButtonItem)resizeReferenceItem).ImageFixedSize;
			((ButtonItem)resizeReferenceItem).ImageFixedSize = Size.Empty;
			resizeReferenceItem.RecalcSize();
			result = resizeReferenceItem.HeightInternal;
			result += ItemPaddingBottom + ItemPaddingTop;
			((ButtonItem)resizeReferenceItem).ImageFixedSize = imageFixedSize;
			resizeReferenceItem.RecalcSize();
		}
		return result;
	}

	private void OnShowFewerButtonsClick(object sender, EventArgs e)
	{
		ShowFewerButtons();
		BaseItem.CollapseAll(sender as BaseItem);
	}

	internal BaseItem GetResizeReferenceItem(bool reduceSize)
	{
		if (reduceSize && SummaryLineStartItemIndex > 1)
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

	internal BaseItem GetFirstVisibleItem()
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
			int changeSizeHeight = GetChangeSizeHeight(reduceSize: true);
			if (val.Height - changeSizeHeight > 0)
			{
				val.Height -= changeSizeHeight;
			}
		}
	}

	private void OnNavPaneOptionsClick(object sender, EventArgs e)
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Invalid comparison between Unknown and I4
		BaseItem.CollapseAll(sender as BaseItem);
		NavPaneOptions navPaneOptions = new NavPaneOptions();
		LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		if (localizationManager != null)
		{
			string defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogCancel);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.cmdCancel).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveDown);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.cmdMoveDown).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogMoveUp);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.cmdMoveUp).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogOK);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.cmdOK).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogReset);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.cmdReset).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogTitle);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions).Text = defaultLocalizedString;
			}
			defaultLocalizedString = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavBarDialogListLabel);
			if (defaultLocalizedString != "")
			{
				((Control)navPaneOptions.labelListCaption).Text = defaultLocalizedString;
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

	private void ConfigurePopupShowing(object sender, EventArgs e)
	{
		if (ContainerControl is NavigationBar && BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			((MenuPanel)(object)m_ConfigureItem.PopupControl).ColorScheme = ((NavigationBar)ContainerControl).GetColorScheme();
		}
		else
		{
			((MenuPanel)(object)m_ConfigureItem.PopupControl).ColorScheme = new ColorScheme(EffectiveStyle);
		}
	}

	public override void InternalMouseHover()
	{
		if (m_ConfigureItem != null && m_ConfigureItem.Expanded && m_HotSubItem != m_ConfigureItem && DesignMode)
		{
			m_ConfigureItem.Expanded = false;
		}
		base.InternalMouseHover();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (m_ConfigureItem != null && m_ConfigureItem.Expanded && m_HotSubItem != m_ConfigureItem && !DesignMode)
		{
			m_ConfigureItem.Expanded = false;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (m_ConfigureItem == null || !m_ConfigureItem.DisplayRectangle.Contains(objArg.X, objArg.Y) || DesignMode)
		{
			base.InternalMouseMove(objArg);
			return;
		}
		BaseItem configureItem = m_ConfigureItem;
		if (configureItem != m_HotSubItem)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (configureItem != null && m_HotSubItem.Expanded)
				{
					m_HotSubItem.Expanded = false;
				}
			}
			if (configureItem != null)
			{
				if (m_AutoExpand)
				{
					BaseItem baseItem = ExpandedItem();
					if (baseItem != null && baseItem != configureItem)
					{
						baseItem.Expanded = false;
					}
				}
				configureItem.InternalMouseEnter();
				configureItem.InternalMouseMove(objArg);
				if (m_AutoExpand && configureItem.Enabled && configureItem.ShowSubItems)
				{
					if (configureItem is PopupItem)
					{
						PopupItem popupItem = configureItem as PopupItem;
						ePopupAnimation popupAnimation = popupItem.PopupAnimation;
						popupItem.PopupAnimation = ePopupAnimation.None;
						configureItem.Expanded = true;
						popupItem.PopupAnimation = popupAnimation;
					}
					else
					{
						configureItem.Expanded = true;
					}
				}
				m_HotSubItem = configureItem;
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
		if (m_ImageSizeSummaryLine.Width == 16)
		{
			return m_ImageSizeSummaryLine.Height != 16;
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

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnAfterItemRemoved(item);
		item.ContainerState = eContainerState.NotSet;
		item.DesignMarkerOrientation = eDesignMarkerOrientation.NotSet;
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		base.InternalKeyDown(objArg);
		ContainerKeyboardNavigation.ProcessKeyDown(this, objArg);
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return GetContainerInsertPosition(pScreen, dragItem);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}
}
