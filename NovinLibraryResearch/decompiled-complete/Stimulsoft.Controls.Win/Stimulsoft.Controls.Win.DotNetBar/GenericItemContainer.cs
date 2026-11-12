using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class GenericItemContainer : ImageItem, IDesignTimeProvider
{
	protected int m_PaddingLeft;

	protected int m_PaddingTop;

	protected int m_PaddingBottom;

	protected int m_PaddingRight;

	protected int m_ItemSpacing;

	private int m_FirstItemSpacing;

	protected eBorderType m_BorderType;

	protected internal Color m_BackgroundColor;

	protected bool m_WrapItems;

	protected DisplayMoreItem m_MoreItems;

	private bool m_MoreItemsOnMenu;

	private bool m_EqualButtonSize;

	private int m_DisplayedItems;

	private bool m_SystemContainer;

	private bool m_HaveCustomizeItem;

	private eLayoutType m_LayoutType;

	private bool m_Scroll;

	private int m_ScrollTopPosition;

	private bool m_TopScroll;

	private bool m_BottomScroll;

	private ScrollButton m_TopScrollButton;

	private ScrollButton m_BottomScrollButton;

	private bool m_IgnoreDuplicateMouseMove;

	private MouseEventArgs m_LastMouseMoveEvent;

	private int m_MinWidth;

	private int m_MinHeight;

	private bool m_Painting;

	private bool m_EventHeight = true;

	private bool m_UseMoreItemsButton = true;

	private eContainerVerticalAlignment m_ToolbarItemsAlign = eContainerVerticalAlignment.Bottom;

	private bool m_TrackSubItemsImageSize = true;

	internal EventHandler ItemAdded;

	private bool _FillsContainerControl;

	internal Color BackgroundColor
	{
		get
		{
			return m_BackgroundColor;
		}
		set
		{
			m_BackgroundColor = value;
		}
	}

	internal int MinHeight
	{
		get
		{
			return m_MinHeight;
		}
		set
		{
			m_MinHeight = value;
		}
	}

	internal int MinWidth
	{
		get
		{
			return m_MinWidth;
		}
		set
		{
			m_MinWidth = value;
		}
	}

	public eBorderType BorderType
	{
		get
		{
			return m_BorderType;
		}
		set
		{
			m_BorderType = value;
		}
	}

	public bool WrapItems
	{
		get
		{
			return m_WrapItems;
		}
		set
		{
			m_WrapItems = value;
		}
	}

	public int ItemSpacing
	{
		get
		{
			return m_ItemSpacing;
		}
		set
		{
			if (m_ItemSpacing != value)
			{
				m_ItemSpacing = value;
				NeedRecalcSize = true;
			}
		}
	}

	protected virtual int FirstItemSpacing
	{
		get
		{
			return m_FirstItemSpacing;
		}
		set
		{
			m_FirstItemSpacing = value;
		}
	}

	public bool MoreItemsOnMenu
	{
		get
		{
			return m_MoreItemsOnMenu;
		}
		set
		{
			m_MoreItemsOnMenu = value;
		}
	}

	internal DisplayMoreItem MoreItems => m_MoreItems;

	private int GroupLineSize
	{
		get
		{
			if (EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return 6;
			}
			return 3;
		}
	}

	public bool EqualButtonSize
	{
		get
		{
			return m_EqualButtonSize;
		}
		set
		{
			if (m_EqualButtonSize != value)
			{
				m_EqualButtonSize = value;
				NeedRecalcSize = true;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			base.Expanded = value;
			if (!value && ContainerControl is ItemControl)
			{
				BaseItem.CollapseSubItems(this);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool SystemContainer
	{
		get
		{
			return m_SystemContainer;
		}
		set
		{
			m_SystemContainer = value;
		}
	}

	public bool HaveCustomizeItem => m_HaveCustomizeItem;

	public override bool AutoExpand
	{
		get
		{
			return base.AutoExpand;
		}
		set
		{
			base.AutoExpand = value;
			if (SystemContainer && ContainerControl is Bar bar)
			{
				if (AutoExpand && !bar.MenuFocus)
				{
					bar.MenuFocus = true;
				}
				else if (!AutoExpand && bar.MenuFocus)
				{
					bar.MenuFocus = false;
				}
			}
		}
	}

	internal bool EventHeight
	{
		get
		{
			return m_EventHeight;
		}
		set
		{
			m_EventHeight = value;
		}
	}

	internal bool UseMoreItemsButton
	{
		get
		{
			return m_UseMoreItemsButton;
		}
		set
		{
			m_UseMoreItemsButton = value;
		}
	}

	internal eContainerVerticalAlignment ToolbarItemsAlign
	{
		get
		{
			return m_ToolbarItemsAlign;
		}
		set
		{
			m_ToolbarItemsAlign = value;
		}
	}

	public virtual eLayoutType LayoutType
	{
		get
		{
			return m_LayoutType;
		}
		set
		{
			if (m_LayoutType == value)
			{
				return;
			}
			m_LayoutType = value;
			NeedRecalcSize = true;
			if (m_LayoutType == eLayoutType.TaskList)
			{
				base.Orientation = eOrientation.Horizontal;
			}
			else if (ContainerControl != null && ContainerControl is Bar)
			{
				Bar bar = ContainerControl as Bar;
				if (bar.DockedSite != null && bar.DockedSite is DockSite)
				{
					base.Orientation = ((DockSite)(object)bar.DockedSite).DockOrientation;
				}
			}
		}
	}

	[Browsable(false)]
	public override eOrientation Orientation
	{
		get
		{
			if (m_LayoutType == eLayoutType.TaskList)
			{
				return eOrientation.Vertical;
			}
			return base.Orientation;
		}
		set
		{
			if (m_LayoutType != eLayoutType.TaskList)
			{
				base.Orientation = value;
			}
		}
	}

	public Color BackColor
	{
		get
		{
			if (!m_BackgroundColor.IsEmpty)
			{
				return m_BackgroundColor;
			}
			if (EffectiveStyle == eDotNetBarStyle.Office2000 || IsOnMenuBar)
			{
				return SystemColors.Control;
			}
			return ColorFunctions.ToolMenuFocusBackColor();
		}
		set
		{
			if (m_BackgroundColor != value)
			{
				m_BackgroundColor = value;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Size SubItemsImageSize
	{
		get
		{
			return base.SubItemsImageSize;
		}
		set
		{
			if (m_TrackSubItemsImageSize)
			{
				base.SubItemsImageSize = value;
			}
		}
	}

	internal bool TrackSubItemsImageSize
	{
		get
		{
			return m_TrackSubItemsImageSize;
		}
		set
		{
			m_TrackSubItemsImageSize = value;
			if (!m_TrackSubItemsImageSize)
			{
				base.SubItemsImageSize = new Size(16, 16);
			}
		}
	}

	public int PaddingLeft
	{
		get
		{
			return m_PaddingLeft;
		}
		set
		{
			m_PaddingLeft = value;
		}
	}

	public int PaddingRight
	{
		get
		{
			return m_PaddingRight;
		}
		set
		{
			m_PaddingRight = value;
		}
	}

	public int PaddingTop
	{
		get
		{
			return m_PaddingTop;
		}
		set
		{
			m_PaddingTop = value;
		}
	}

	public int PaddingBottom
	{
		get
		{
			return m_PaddingBottom;
		}
		set
		{
			m_PaddingBottom = value;
		}
	}

	public bool FillsContainerControl
	{
		get
		{
			return _FillsContainerControl;
		}
		set
		{
			_FillsContainerControl = value;
		}
	}

	public GenericItemContainer()
	{
		m_IsContainer = true;
		m_PaddingLeft = 1;
		m_PaddingTop = 1;
		m_PaddingBottom = 1;
		m_PaddingRight = 1;
		m_ItemSpacing = 0;
		m_BackgroundColor = Color.Empty;
		m_BorderType = eBorderType.None;
		m_WrapItems = false;
		m_MoreItems = null;
		m_EqualButtonSize = false;
		m_MoreItemsOnMenu = false;
		m_SystemContainer = false;
		AccessibleRole = (AccessibleRole)20;
	}

	protected override void Dispose(bool disposing)
	{
		if (m_TopScrollButton != null)
		{
			if (((Control)m_TopScrollButton).Parent != null)
			{
				((Control)m_TopScrollButton).Parent.Controls.Remove((Control)(object)m_TopScrollButton);
			}
			((Component)(object)m_TopScrollButton).Dispose();
			m_TopScrollButton = null;
		}
		if (m_BottomScrollButton != null)
		{
			if (((Control)m_BottomScrollButton).Parent != null)
			{
				((Control)m_BottomScrollButton).Parent.Controls.Remove((Control)(object)m_BottomScrollButton);
			}
			((Component)(object)m_BottomScrollButton).Dispose();
			m_BottomScrollButton = null;
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		GenericItemContainer genericItemContainer = new GenericItemContainer();
		CopyToItem(genericItemContainer);
		return genericItemContainer;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		GenericItemContainer genericItemContainer = copy as GenericItemContainer;
		base.CopyToItem(genericItemContainer);
		genericItemContainer.WrapItems = m_WrapItems;
		genericItemContainer.EqualButtonSize = m_EqualButtonSize;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		if (SuspendLayout || m_Painting)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		m_Painting = true;
		try
		{
			Graphics graphics = pa.Graphics;
			if (m_NeedRecalcSize)
			{
				RecalcSize();
			}
			int left = m_Rect.Left;
			int top = m_Rect.Top;
			Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width, m_Rect.Height);
			switch (m_BorderType)
			{
			case eBorderType.Bump:
				ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)9, (Border3DSide)2063);
				rectangle.Inflate(-2, -2);
				break;
			case eBorderType.Etched:
				ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)6, (Border3DSide)2063);
				rectangle.Inflate(-2, -2);
				break;
			case eBorderType.Raised:
				ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4, (Border3DSide)2063);
				rectangle.Inflate(-2, -2);
				break;
			case eBorderType.Sunken:
				ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2, (Border3DSide)2063);
				rectangle.Inflate(-2, -2);
				break;
			case eBorderType.SingleLine:
			{
				Pen val = new Pen(SystemBrushes.ControlDark, 1f);
				NativeFunctions.DrawRectangle(graphics, val, m_Rect);
				val.Dispose();
				rectangle.Inflate(-1, -1);
				break;
			}
			}
			if (!m_BackgroundColor.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(m_BackgroundColor);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				((Brush)val2).Dispose();
			}
			graphics.SetClip(rectangle);
			if (m_SubItems != null)
			{
				BaseItem baseItem = null;
				int num = 0;
				ThemeToolbar themeToolbar = null;
				ThemeToolbarParts separator = ThemeToolbarParts.Separator;
				ThemeToolbarStates normal = ThemeToolbarStates.Normal;
				if (base.IsThemed)
				{
					themeToolbar = pa.ThemeToolbar;
				}
				for (int i = 0; i < m_SubItems.Count; i++)
				{
					baseItem = m_SubItems[i];
					if (baseItem.Visible && baseItem.Displayed)
					{
						if (baseItem.BeginGroup && num > 0 && m_LayoutType != eLayoutType.DockContainer && (!(baseItem is ItemContainer) || !BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle)))
						{
							if (m_Orientation == eOrientation.Vertical || m_LayoutType == eLayoutType.TaskList)
							{
								if (baseItem.TopInternal == m_PaddingTop + m_Rect.Top || baseItem.LeftInternal > m_PaddingLeft + m_Rect.Left)
								{
									if (effectiveStyle == eDotNetBarStyle.Office2000)
									{
										ControlPaint.DrawBorder3D(graphics, baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 2, m_Rect.Height - m_PaddingTop - m_PaddingBottom - 4, (Border3DStyle)6, (Border3DSide)1);
									}
									else if (themeToolbar != null)
									{
										separator = ThemeToolbarParts.Separator;
										themeToolbar.DrawBackground(graphics, separator, normal, new Rectangle(baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 6, m_Rect.Height - m_PaddingBottom - 2));
									}
									else
									{
										Pen val3 = new Pen(pa.Colors.ItemSeparator);
										try
										{
											if (effectiveStyle == eDotNetBarStyle.OfficeXP)
											{
												graphics.DrawLine(val3, baseItem.LeftInternal - 2, baseItem.TopInternal + 1, baseItem.LeftInternal - 2, m_Rect.Bottom - m_PaddingBottom - 2);
											}
											else
											{
												graphics.DrawLine(val3, baseItem.LeftInternal - 2, baseItem.TopInternal + 2, baseItem.LeftInternal - 2, m_Rect.Bottom - m_PaddingBottom - 4);
												Pen val4 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
												graphics.DrawLine(val4, baseItem.LeftInternal - 1, baseItem.TopInternal + 3, baseItem.LeftInternal - 1, m_Rect.Bottom - m_PaddingBottom - 5);
												val4.Dispose();
											}
										}
										finally
										{
											((IDisposable)val3)?.Dispose();
										}
									}
								}
								else
								{
									Point point = new Point(baseItem.LeftInternal + m_PaddingLeft, baseItem.TopInternal - 2);
									Point point2 = new Point(baseItem.DisplayRectangle.Right - m_PaddingRight, baseItem.TopInternal - 2);
									if (LayoutType == eLayoutType.TaskList)
									{
										point.X = LeftInternal + 1;
										point2.X = DisplayRectangle.Right - 2;
									}
									if (effectiveStyle == eDotNetBarStyle.Office2000)
									{
										ControlPaint.DrawBorder3D(graphics, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 4, baseItem.WidthInternal - 2, 2, (Border3DStyle)6, (Border3DSide)2);
									}
									else if (themeToolbar != null)
									{
										separator = ThemeToolbarParts.SeparatorVert;
										themeToolbar.DrawBackground(graphics, separator, normal, new Rectangle(point.X, point.Y, point2.X - point.X, 6));
									}
									else
									{
										Pen val5 = new Pen(pa.Colors.ItemSeparator);
										try
										{
											if (effectiveStyle == eDotNetBarStyle.OfficeXP)
											{
												graphics.DrawLine(val5, point, point2);
											}
											else
											{
												graphics.DrawLine(val5, point, point2);
												point.Offset(0, 1);
												point2.Offset(0, 1);
												Pen val6 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
												try
												{
													graphics.DrawLine(val6, point, point2);
												}
												finally
												{
													((IDisposable)val6)?.Dispose();
												}
											}
										}
										finally
										{
											((IDisposable)val5)?.Dispose();
										}
									}
								}
							}
							else if (baseItem.TopInternal == m_PaddingTop + m_Rect.Top || baseItem.LeftInternal > m_PaddingLeft + m_Rect.Left)
							{
								if (effectiveStyle == eDotNetBarStyle.Office2000)
								{
									ControlPaint.DrawBorder3D(graphics, baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 2, baseItem.HeightInternal - 2, (Border3DStyle)6, (Border3DSide)1);
								}
								else if (themeToolbar != null)
								{
									separator = ThemeToolbarParts.Separator;
									themeToolbar.DrawBackground(graphics, separator, normal, new Rectangle(baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 6, baseItem.HeightInternal - 3));
								}
								else
								{
									Pen val7 = new Pen(pa.Colors.ItemSeparator);
									try
									{
										if (effectiveStyle == eDotNetBarStyle.OfficeXP)
										{
											graphics.DrawLine(val7, baseItem.LeftInternal - 2, baseItem.TopInternal + 1, baseItem.LeftInternal - 2, baseItem.TopInternal + 1 + baseItem.HeightInternal - 3);
										}
										else if (m_Style == eDotNetBarStyle.Office2007)
										{
											graphics.DrawLine(val7, baseItem.LeftInternal - 2, baseItem.TopInternal + 2, baseItem.LeftInternal - 2, baseItem.TopInternal + 4 + baseItem.HeightInternal - 9);
											Pen val8 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
											graphics.DrawLine(val8, baseItem.LeftInternal - 1, baseItem.TopInternal + 3, baseItem.LeftInternal - 1, baseItem.TopInternal + 5 + baseItem.HeightInternal - 9);
											val8.Dispose();
										}
										else
										{
											graphics.DrawLine(val7, baseItem.LeftInternal - 2, baseItem.TopInternal + 4, baseItem.LeftInternal - 2, baseItem.TopInternal + 4 + baseItem.HeightInternal - 8);
											Pen val9 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
											graphics.DrawLine(val9, baseItem.LeftInternal - 1, baseItem.TopInternal + 5, baseItem.LeftInternal - 1, baseItem.TopInternal + 5 + baseItem.HeightInternal - 8);
											val9.Dispose();
										}
									}
									finally
									{
										((IDisposable)val7)?.Dispose();
									}
								}
							}
							else if (effectiveStyle == eDotNetBarStyle.Office2000)
							{
								ControlPaint.DrawBorder3D(graphics, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 4, m_Rect.Width - m_PaddingLeft - m_PaddingRight - 4, 2, (Border3DStyle)6, (Border3DSide)2);
							}
							else if (themeToolbar != null)
							{
								separator = ThemeToolbarParts.SeparatorVert;
								themeToolbar.DrawBackground(graphics, separator, normal, new Rectangle(m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 4, m_Rect.Width - m_PaddingRight - 2, 6));
							}
							else
							{
								Pen val10 = new Pen(pa.Colors.ItemSeparator);
								try
								{
									if (effectiveStyle == eDotNetBarStyle.OfficeXP)
									{
										graphics.DrawLine(val10, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, baseItem.TopInternal - 2);
									}
									else
									{
										graphics.DrawLine(val10, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, baseItem.TopInternal - 2);
										Pen val11 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
										graphics.DrawLine(val11, m_Rect.Left + m_PaddingLeft + 6, baseItem.TopInternal - 1, m_Rect.Right - m_PaddingRight - 4, baseItem.TopInternal - 1);
										val11.Dispose();
									}
								}
								finally
								{
									((IDisposable)val10)?.Dispose();
								}
							}
						}
						if (graphics.ClipBounds.IntersectsWith(baseItem.DisplayRectangle))
						{
							baseItem.Paint(pa);
						}
						num++;
					}
					if (m_MoreItems != null)
					{
						m_MoreItems.Paint(pa);
					}
				}
			}
			graphics.ResetClip();
		}
		finally
		{
			m_Painting = false;
		}
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			m_RecalculatingSize = true;
			switch (m_LayoutType)
			{
			case eLayoutType.Toolbar:
				RecalcSizeToolbar();
				break;
			case eLayoutType.TaskList:
				RecalcSizeTaskList();
				break;
			case eLayoutType.DockContainer:
				RecalcSizeDockContainer(bFitContainer: false);
				break;
			}
			base.RecalcSize();
			m_RecalculatingSize = false;
		}
	}

	private void RecalcSizeDockContainer(bool bFitContainer)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		bool flag = false;
		if (SuspendLayout)
		{
			return;
		}
		m_DisplayedItems = 0;
		num = m_PaddingLeft + m_Rect.Left;
		num2 = m_PaddingTop + m_Rect.Top;
		if (m_SubItems != null)
		{
			for (int i = 0; i < SubItems.Count; i++)
			{
				BaseItem baseItem = SubItems[i];
				baseItem.RecalcSize();
				if (baseItem.WidthInternal > num3)
				{
					num3 = baseItem.WidthInternal;
				}
				if (baseItem.HeightInternal > num4)
				{
					num4 = baseItem.HeightInternal;
				}
				baseItem.LeftInternal = num;
				baseItem.TopInternal = num2;
				if (!baseItem.Visible)
				{
					baseItem.Displayed = false;
					continue;
				}
				m_DisplayedItems++;
				if (!flag && (baseItem.Displayed || i == SubItems.Count - 1))
				{
					baseItem.Displayed = true;
					flag = true;
				}
				else
				{
					baseItem.Displayed = false;
				}
			}
			if (!flag)
			{
				foreach (BaseItem subItem in m_SubItems)
				{
					if (subItem.Visible)
					{
						subItem.Displayed = true;
						break;
					}
				}
			}
		}
		if (bFitContainer || Stretch)
		{
			if (num3 + m_PaddingLeft + m_PaddingRight < m_Rect.Width || num4 + m_PaddingTop + m_PaddingBottom < m_Rect.Height)
			{
				if (num3 + m_PaddingLeft + m_PaddingRight < m_Rect.Width)
				{
					num3 = m_Rect.Width - m_PaddingLeft - m_PaddingRight;
				}
				if (num4 + m_PaddingTop + m_PaddingBottom < m_Rect.Height)
				{
					num4 = m_Rect.Height - m_PaddingTop - m_PaddingBottom;
				}
			}
			if (ContainerControl is Bar bar && (bar.BarState == eBarState.Docked || bar.BarState == eBarState.AutoHide))
			{
				if (MinWidth > 0 && num3 + m_PaddingLeft + m_PaddingRight < MinWidth && Orientation == eOrientation.Vertical)
				{
					num3 = MinWidth - (m_PaddingLeft + m_PaddingRight);
				}
				else if (MinHeight > 0 && num4 + m_PaddingTop + m_PaddingBottom < MinHeight && Orientation == eOrientation.Horizontal)
				{
					num4 = MinHeight - (m_PaddingTop + m_PaddingBottom);
				}
			}
		}
		Size size = new Size(num3, num4);
		foreach (BaseItem subItem2 in m_SubItems)
		{
			subItem2.Size = size;
		}
		if (Stretch)
		{
			if (m_Orientation == eOrientation.Horizontal)
			{
				m_Rect.Height = m_PaddingTop + num4 + m_PaddingBottom;
				if (m_Rect.Width < num3 + m_PaddingLeft + m_PaddingRight)
				{
					m_Rect.Width = m_PaddingLeft + num3 + m_PaddingRight;
				}
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = m_PaddingLeft + num3 + m_PaddingRight;
				if (m_Rect.Height < num4 + m_PaddingBottom + m_PaddingTop)
				{
					m_Rect.Height = m_PaddingTop + num4 + m_PaddingBottom;
				}
			}
		}
		else
		{
			m_Rect.Width = m_PaddingLeft + num3 + m_PaddingRight;
			m_Rect.Height = m_PaddingTop + num4 + m_PaddingBottom;
		}
	}

	private void RecalcSizeToolbar()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		bool flag = false;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			flag = (int)val.RightToLeft == 1;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 3;
		int num9 = 0;
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = false;
		int num10 = 0;
		BaseItem baseItem = null;
		ArrayList arrayList = null;
		Stack stack = null;
		ArrayList arrayList2 = null;
		ArrayList arrayList3 = null;
		ArrayList arrayList4 = null;
		if (m_MoreItems != null && m_MoreItems.Expanded)
		{
			m_MoreItems.Expanded = false;
		}
		if (EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			num8 = 6;
		}
		num = m_PaddingLeft + m_Rect.Left;
		num2 = m_PaddingTop + m_Rect.Top;
		int visibleSubItems = base.VisibleSubItems;
		int num11 = 0;
		if (m_SubItems != null)
		{
			bool flag5 = false;
			int num12 = 0;
			if (m_WrapItems || Stretch)
			{
				arrayList = new ArrayList(3);
			}
			if (Stretch)
			{
				arrayList2 = new ArrayList(3);
				arrayList3 = new ArrayList(3);
			}
			bool flag6 = OnBeforeLayout();
			do
			{
				num10 = 0;
				for (int i = 0; i < m_SubItems.Count; i++)
				{
					baseItem = m_SubItems[i];
					if (baseItem != null && baseItem.Visible)
					{
						num11++;
						if ((baseItem.SupportedOrientation == eSupportedOrientation.Horizontal && m_Orientation == eOrientation.Vertical) || (baseItem.SupportedOrientation == eSupportedOrientation.Vertical && m_Orientation == eOrientation.Horizontal))
						{
							baseItem.Displayed = false;
							continue;
						}
						int num13 = ((num10 > 1 || m_FirstItemSpacing == 0) ? m_ItemSpacing : m_FirstItemSpacing);
						if (flag6)
						{
							baseItem.RecalcSize();
						}
						if (flag4)
						{
							if (m_Orientation == eOrientation.Vertical)
							{
								baseItem.WidthInternal = num4;
							}
							else
							{
								baseItem.HeightInternal = num5;
							}
						}
						switch (m_Orientation)
						{
						case eOrientation.Horizontal:
							if (m_WrapItems)
							{
								if (baseItem.BeginGroup && num10 > 0)
								{
									num += num8;
								}
								if (num + baseItem.WidthInternal + num13 > m_Rect.Right && num > m_PaddingLeft + m_Rect.Left)
								{
									num2 += num3 + num13;
									if (baseItem.BeginGroup)
									{
										num2 += num8;
									}
									num = m_PaddingLeft + m_Rect.Left;
									num3 = 0;
									num9++;
									if (stack != null)
									{
										stack = null;
									}
									if (arrayList4 != null)
									{
										arrayList4 = null;
									}
								}
							}
							else if (flag2)
							{
								if (baseItem.BeginGroup && num10 > 0)
								{
									num += num8;
								}
								if ((num + baseItem.WidthInternal + num13 > m_Rect.Right - m_PaddingRight && num > m_PaddingLeft + m_Rect.Left) || (num + baseItem.WidthInternal + num13 + DisplayMoreItem.FixedSize > m_Rect.Right && i + 1 < m_SubItems.Count && num11 < visibleSubItems))
								{
									if (i == 0)
									{
										num3 = baseItem.HeightInternal;
									}
									num += num13 + DisplayMoreItem.FixedSize;
									flag2 = false;
								}
							}
							if (flag2)
							{
								if (num10 > 0)
								{
									num += num13;
								}
								baseItem.LeftInternal = GetItemLayoutX(baseItem, num);
								baseItem.TopInternal = GetItemLayoutY(baseItem, num2);
								num += GetItemLayoutWidth(baseItem);
								if (baseItem.HeightInternal > num3)
								{
									num3 = baseItem.HeightInternal;
								}
							}
							break;
						case eOrientation.Vertical:
							if (m_WrapItems)
							{
								if (baseItem.BeginGroup && num10 > 0)
								{
									num2 += num8;
								}
								if (num2 + baseItem.HeightInternal + num13 > m_Rect.Bottom && num2 > m_PaddingTop + m_Rect.Top)
								{
									num += num3 + num13;
									if (baseItem.BeginGroup)
									{
										num += num8;
									}
									num2 = m_PaddingTop + m_Rect.Top;
									num3 = 0;
									num9++;
									if (stack != null)
									{
										stack = null;
									}
									if (arrayList4 != null)
									{
										arrayList4 = null;
									}
								}
							}
							else if (flag2)
							{
								if (baseItem.BeginGroup && num10 > 0)
								{
									num2 += num8;
								}
								if ((num2 + baseItem.HeightInternal + num13 > m_Rect.Bottom && num2 > m_PaddingTop + m_Rect.Top) || (num2 + baseItem.HeightInternal + num13 + DisplayMoreItem.FixedSize > m_Rect.Bottom && i + 1 < m_SubItems.Count && num11 < visibleSubItems))
								{
									if (i == 0)
									{
										num3 = baseItem.WidthInternal;
									}
									num2 += num13 + DisplayMoreItem.FixedSize;
									flag2 = false;
								}
							}
							if (flag2)
							{
								if (num10 > 0)
								{
									num2 += num13;
								}
								baseItem.LeftInternal = num;
								baseItem.TopInternal = num2;
								num2 += GetItemLayoutHeight(baseItem);
								if (baseItem.WidthInternal > num3)
								{
									num3 = baseItem.WidthInternal;
								}
							}
							break;
						}
						if (flag2)
						{
							if (baseItem.WidthInternal != num4)
							{
								if (m_Orientation == eOrientation.Vertical && num4 != 0 && !flag4)
								{
									flag3 = true;
								}
								if (baseItem.WidthInternal > num4)
								{
									num4 = baseItem.WidthInternal;
								}
							}
							if (baseItem.HeightInternal != num5)
							{
								if (m_Orientation == eOrientation.Horizontal && num5 != 0 && !flag4)
								{
									flag3 = true;
								}
								if (baseItem.HeightInternal > num5)
								{
									num5 = baseItem.HeightInternal;
								}
							}
							num10++;
						}
						baseItem.Displayed = flag2;
						if ((flag2 && arrayList != null && baseItem.ItemAlignment == eItemAlignment.Far) || stack != null)
						{
							if (stack == null)
							{
								stack = new Stack();
								arrayList.Add(stack);
							}
							stack.Push(baseItem);
							arrayList2 = null;
						}
						if ((flag2 && baseItem.Stretch && arrayList2 != null) || arrayList4 != null)
						{
							if (arrayList4 == null)
							{
								arrayList4 = new ArrayList(5);
								arrayList2.Add(arrayList4);
								arrayList3.Add(0);
							}
							arrayList4.Add(baseItem);
							if (baseItem.Stretch)
							{
								arrayList3[arrayList3.Count - 1] = (int)arrayList3[arrayList3.Count - 1] + 1;
							}
						}
						if (num > num6)
						{
							num6 = num;
						}
						if (num2 > num7)
						{
							num7 = num2;
						}
					}
					else if (baseItem != null)
					{
						baseItem.Displayed = false;
					}
				}
				if (m_WrapItems && num9 > 0 && num12 < 1)
				{
					if (m_Orientation == eOrientation.Horizontal && (num6 > m_Rect.Width || flag3))
					{
						flag5 = true;
						m_Rect.Width = num6 + m_PaddingLeft + m_PaddingRight;
						num = m_PaddingLeft + m_Rect.Left;
						num2 = m_PaddingTop + m_Rect.Top;
						num6 = 0;
						num7 = 0;
					}
					else if (m_Orientation == eOrientation.Vertical && (num7 > m_Rect.Height || flag3))
					{
						flag5 = true;
						m_Rect.Height = num7 + m_PaddingTop + m_PaddingBottom;
						num = m_PaddingLeft + m_Rect.Left;
						num2 = m_PaddingTop + m_Rect.Top;
						num6 = 0;
						num7 = 0;
					}
					if (flag3)
					{
						if (m_EventHeight)
						{
							flag4 = true;
						}
						flag3 = false;
					}
					if (flag5 && num12 + 1 < 2)
					{
						if (arrayList != null && arrayList.Count > 0)
						{
							arrayList = new ArrayList(3);
						}
						if (arrayList2 != null && arrayList2.Count > 0)
						{
							arrayList2 = new ArrayList(3);
							arrayList3 = new ArrayList(3);
						}
					}
				}
				num12++;
			}
			while (flag5 && num12 < 2);
		}
		if (m_EqualButtonSize)
		{
			num = m_PaddingLeft + m_Rect.Left;
			num2 = m_PaddingTop + m_Rect.Top;
			num6 = 0;
			num7 = 0;
			flag2 = true;
			num9 = 0;
			flag3 = false;
			num10 = 0;
			if (arrayList != null && arrayList.Count > 0)
			{
				arrayList = new ArrayList(3);
			}
			if (arrayList2 != null && arrayList2.Count > 0)
			{
				arrayList2 = new ArrayList(3);
				arrayList3 = new ArrayList(3);
			}
			for (int j = 0; j < m_SubItems.Count; j++)
			{
				baseItem = m_SubItems[j];
				if (baseItem == null || !baseItem.Visible)
				{
					continue;
				}
				if ((baseItem.SupportedOrientation == eSupportedOrientation.Horizontal && m_Orientation == eOrientation.Vertical) || (baseItem.SupportedOrientation == eSupportedOrientation.Vertical && m_Orientation == eOrientation.Horizontal))
				{
					baseItem.Displayed = false;
					continue;
				}
				int num14 = ((num10 > 1 || m_FirstItemSpacing == 0) ? m_ItemSpacing : m_FirstItemSpacing);
				if (baseItem.SystemItem)
				{
					if (m_Orientation == eOrientation.Vertical)
					{
						baseItem.WidthInternal = num4;
					}
					else
					{
						baseItem.HeightInternal = num5;
					}
				}
				else
				{
					baseItem.WidthInternal = num4;
					baseItem.HeightInternal = num5;
				}
				switch (m_Orientation)
				{
				case eOrientation.Horizontal:
					if (m_WrapItems)
					{
						if (baseItem.BeginGroup)
						{
							num += num8;
						}
						if (num + baseItem.WidthInternal + num14 > m_Rect.Right && num > m_PaddingLeft + m_PaddingRight)
						{
							num2 += num3 + num14;
							if (baseItem.BeginGroup)
							{
								num2 += num8;
							}
							num = m_PaddingLeft + m_Rect.Left;
							num3 = 0;
							num9++;
							if (stack != null)
							{
								stack = null;
							}
							if (arrayList4 != null)
							{
								arrayList4 = null;
							}
						}
					}
					else if (flag2)
					{
						if (baseItem.BeginGroup)
						{
							num += num8;
						}
						if ((num + baseItem.WidthInternal + num14 > m_Rect.Right && num > m_PaddingLeft + m_Rect.Left) || (num + baseItem.WidthInternal + num14 + DisplayMoreItem.FixedSize > m_Rect.Right && j + 1 < m_SubItems.Count && num11 < visibleSubItems))
						{
							num += num14 + DisplayMoreItem.FixedSize;
							flag2 = false;
						}
					}
					if (flag2)
					{
						baseItem.LeftInternal = num;
						baseItem.TopInternal = num2;
						num += baseItem.WidthInternal + num14;
						if (baseItem.HeightInternal > num3)
						{
							num3 = baseItem.HeightInternal;
						}
					}
					break;
				case eOrientation.Vertical:
					if (m_WrapItems)
					{
						if (num2 + baseItem.HeightInternal + num14 > m_Rect.Bottom && num2 > m_PaddingTop + m_PaddingBottom)
						{
							num += num3 + num14;
							num2 = m_PaddingTop + m_Rect.Top;
							num3 = 0;
							num9++;
							if (stack != null)
							{
								stack = null;
							}
							if (arrayList4 != null)
							{
								arrayList4 = null;
							}
						}
					}
					else if (flag2 && ((num2 + baseItem.HeightInternal + num14 > m_Rect.Bottom && num2 > m_PaddingTop + m_Rect.Top) || (num2 + baseItem.HeightInternal + num14 + DisplayMoreItem.FixedSize > m_Rect.Bottom && j + 1 < m_SubItems.Count && num11 < visibleSubItems)))
					{
						num2 += num14 + DisplayMoreItem.FixedSize;
						flag2 = false;
					}
					if (flag2)
					{
						baseItem.LeftInternal = num;
						baseItem.TopInternal = num2;
						num2 += baseItem.HeightInternal + num14;
						if (baseItem.WidthInternal > num3)
						{
							num3 = baseItem.WidthInternal;
						}
					}
					break;
				}
				baseItem.Displayed = flag2;
				if (flag2)
				{
					num10++;
				}
				if ((flag2 && arrayList != null && baseItem.ItemAlignment == eItemAlignment.Far) || stack != null)
				{
					if (stack == null)
					{
						stack = new Stack();
						arrayList.Add(stack);
					}
					stack.Push(baseItem);
					arrayList2 = null;
				}
				if ((flag2 && baseItem.Stretch && arrayList2 != null) || arrayList4 != null)
				{
					if (arrayList4 == null)
					{
						arrayList4 = new ArrayList(5);
						arrayList2.Add(arrayList4);
						arrayList3.Add(0);
					}
					arrayList4.Add(baseItem);
					if (baseItem.Stretch)
					{
						arrayList3[arrayList3.Count - 1] = (int)arrayList3[arrayList3.Count - 1] + 1;
					}
				}
				if (num > num6)
				{
					num6 = num;
				}
				if (num2 > num7)
				{
					num7 = num2;
				}
			}
		}
		m_DisplayedItems = num10;
		if (_FillsContainerControl)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				num4 = m_Rect.Width - m_PaddingLeft;
			}
			else
			{
				num5 = m_Rect.Height - m_PaddingTop;
			}
			flag3 = true;
		}
		if (flag3)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				foreach (BaseItem subItem in SubItems)
				{
					if (m_EventHeight)
					{
						subItem.WidthInternal = num4;
					}
					else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Bottom)
					{
						subItem.LeftInternal += num4 - subItem.WidthInternal;
					}
					else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Middle)
					{
						subItem.LeftInternal += (num4 - subItem.WidthInternal) / 2;
					}
				}
			}
			else
			{
				foreach (BaseItem subItem2 in SubItems)
				{
					if (m_EventHeight)
					{
						subItem2.HeightInternal = num5;
					}
					else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Bottom)
					{
						subItem2.TopInternal += num5 - subItem2.HeightInternal;
					}
					else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Middle && num5 != subItem2.HeightInternal)
					{
						subItem2.TopInternal += (int)Math.Ceiling((float)(num5 - subItem2.HeightInternal) / 2f);
					}
				}
			}
		}
		switch (m_Orientation)
		{
		case eOrientation.Horizontal:
			num7 += num3;
			break;
		case eOrientation.Vertical:
			num6 += num3;
			break;
		}
		if (Stretch)
		{
			if (m_Orientation == eOrientation.Horizontal)
			{
				m_Rect.Height = num7 + m_PaddingBottom - m_Rect.Top;
				if (arrayList2 != null && arrayList2.Count > 0)
				{
					StretchItems(arrayList2, arrayList3, num6 + m_PaddingRight - m_Rect.Left);
				}
				if (m_Rect.Width < num6 + m_PaddingRight - m_Rect.Left)
				{
					m_Rect.Width = num6 + m_PaddingRight - m_Rect.Left;
				}
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = num6 + m_PaddingRight - m_Rect.Left;
				if (arrayList2 != null && arrayList2.Count > 0)
				{
					StretchItems(arrayList2, arrayList3, num7 + m_PaddingBottom - m_Rect.Top);
				}
				if (m_Rect.Height < num7 + m_PaddingBottom - m_Rect.Top)
				{
					m_Rect.Height = num7 + m_PaddingBottom - m_Rect.Top;
				}
			}
			else
			{
				m_Rect.Width = num6 + m_PaddingRight - m_Rect.Left;
				m_Rect.Height = num7 + m_PaddingBottom - m_Rect.Top;
			}
		}
		else if (!m_WrapItems && !flag2)
		{
			if (m_Orientation == eOrientation.Horizontal)
			{
				m_Rect.Height = num7 + m_PaddingBottom - m_Rect.Top;
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = num6 + m_PaddingRight - m_Rect.Left;
			}
		}
		else if (m_DisplayedItems > 0)
		{
			m_Rect.Width = num6 + m_PaddingRight - m_Rect.Left;
			m_Rect.Height = num7 + m_PaddingBottom - m_Rect.Top;
		}
		if (!m_WrapItems && !flag2 && m_UseMoreItemsButton)
		{
			CreateMoreItemsButton(flag);
		}
		else if (m_MoreItems != null)
		{
			m_MoreItems.Dispose();
			m_MoreItems = null;
		}
		if (arrayList != null && arrayList.Count > 0)
		{
			AlignItemsFar(arrayList);
		}
		if (flag && m_Orientation == eOrientation.Horizontal)
		{
			MirrorPositionItems();
		}
		m_NeedRecalcSize = false;
		if (m_Parent != null)
		{
			m_Parent.SubItemSizeChanged(this);
		}
	}

	protected virtual int GetItemLayoutX(BaseItem objItem, int iX)
	{
		return iX;
	}

	protected virtual int GetItemLayoutY(BaseItem objItem, int iY)
	{
		return iY;
	}

	protected virtual int GetItemLayoutHeight(BaseItem objItem)
	{
		return objItem.HeightInternal;
	}

	protected virtual int GetItemLayoutWidth(BaseItem objItem)
	{
		return objItem.WidthInternal;
	}

	protected virtual bool OnBeforeLayout()
	{
		return true;
	}

	protected void MirrorPositionItems()
	{
		Rectangle displayRectangle = DisplayRectangle;
		foreach (BaseItem subItem in SubItems)
		{
			subItem.LeftInternal = displayRectangle.Right - (subItem.DisplayRectangle.X - displayRectangle.X + subItem.DisplayRectangle.Width);
		}
	}

	private void RecalcSizeTaskList()
	{
		int leftInternal = m_Rect.Left + m_PaddingLeft;
		int num = m_Rect.Top + m_PaddingTop;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 3;
		bool flag = true;
		int num7 = 0;
		Stack stack = null;
		if (EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			num6 = 6;
		}
		if (m_EqualButtonSize)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.Orientation = eOrientation.Horizontal;
				subItem.RecalcSize();
				if (subItem.WidthInternal > num2)
				{
					num2 = subItem.WidthInternal;
				}
				if (subItem.HeightInternal > num3)
				{
					num3 = subItem.HeightInternal;
				}
			}
		}
		for (int i = 0; i < m_SubItems.Count; i++)
		{
			BaseItem baseItem2 = m_SubItems[i];
			baseItem2.Orientation = eOrientation.Horizontal;
			if (baseItem2 == null || !baseItem2.Visible)
			{
				continue;
			}
			if (!m_EqualButtonSize)
			{
				baseItem2.RecalcSize();
			}
			if (baseItem2.BeginGroup && num7 > 0)
			{
				num += num6;
			}
			if (!m_EqualButtonSize)
			{
				if (baseItem2.WidthInternal > num2)
				{
					num2 = baseItem2.WidthInternal;
				}
				if (baseItem2.HeightInternal > num3)
				{
					num3 = baseItem2.HeightInternal;
				}
			}
			else
			{
				baseItem2.WidthInternal = num2;
				baseItem2.HeightInternal = num3;
			}
			baseItem2.TopInternal = num;
			baseItem2.LeftInternal = leftInternal;
			num += baseItem2.HeightInternal;
			baseItem2.Displayed = flag;
			if (!flag)
			{
				continue;
			}
			if (num > m_Rect.Bottom)
			{
				flag = false;
				stack = null;
				baseItem2.Displayed = false;
				continue;
			}
			if (stack != null && stack.Count > 0)
			{
				stack.Push(baseItem2);
			}
			num7++;
			if (baseItem2.ItemAlignment == eItemAlignment.Far)
			{
				if (stack == null)
				{
					stack = new Stack(10);
				}
				stack.Push(baseItem2);
			}
		}
		num4 = num2 + m_PaddingLeft + m_PaddingRight;
		num5 = num + m_PaddingBottom - m_Rect.Top;
		m_DisplayedItems = num7;
		if (Stretch)
		{
			if (flag && stack != null && stack.Count > 0)
			{
				AlignItemsFar(stack, eOrientation.Vertical);
			}
			if (!(ContainerControl is Bar bar) || (bar != null && bar.DockSide != eDockSide.Top && bar.DockSide != eDockSide.Bottom))
			{
				m_Rect.Width = num4;
			}
			if (m_Rect.Height < num5)
			{
				if (m_Rect.Height < 36)
				{
					m_Rect.Height = 36;
				}
				m_Scroll = true;
				RepositionItems();
			}
			else
			{
				m_Scroll = false;
				m_ScrollTopPosition = 0;
			}
		}
		else
		{
			m_Rect.Width = num4;
			if (m_Rect.Height > num5 && flag)
			{
				m_Rect.Height = num5;
			}
			if (!flag)
			{
				if (m_Rect.Height < 36)
				{
					m_Rect.Height = 36;
				}
				m_Scroll = true;
				RepositionItems();
			}
			else
			{
				m_Scroll = false;
				m_ScrollTopPosition = 0;
			}
		}
		if (m_MoreItems != null)
		{
			m_MoreItems.Dispose();
			m_MoreItems = null;
		}
		SetupScrollButtons();
	}

	private void RepositionItems()
	{
		int num = 0;
		int num2 = 0;
		int groupLineSize = GroupLineSize;
		bool flag = true;
		m_TopScroll = false;
		m_BottomScroll = false;
		for (int num3 = m_ScrollTopPosition - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = m_SubItems[num3];
			if (baseItem.Visible)
			{
				num = (baseItem.TopInternal = num - baseItem.HeightInternal);
				baseItem.Displayed = false;
				if (baseItem.BeginGroup && num3 > 0)
				{
					num -= groupLineSize;
				}
			}
		}
		num = m_Rect.Top;
		for (int i = m_ScrollTopPosition; i < m_SubItems.Count; i++)
		{
			BaseItem baseItem2 = m_SubItems[i];
			if (!baseItem2.Visible)
			{
				continue;
			}
			baseItem2.Displayed = flag;
			if (flag)
			{
				if (baseItem2.BeginGroup && num2 > 0)
				{
					num += groupLineSize;
				}
				baseItem2.TopInternal = num;
				num += baseItem2.HeightInternal;
				if (num > m_Rect.Bottom)
				{
					baseItem2.Displayed = false;
					flag = false;
				}
				else
				{
					num2++;
				}
			}
		}
		m_DisplayedItems = num2;
		if (!flag)
		{
			m_BottomScroll = true;
		}
		if (m_ScrollTopPosition > 0)
		{
			m_TopScroll = true;
		}
	}

	private void SetupScrollButtons()
	{
		bool flag = false;
		bool flag2 = false;
		if (!m_Scroll)
		{
			flag = true;
			flag2 = true;
		}
		else
		{
			if (!m_TopScroll)
			{
				flag = true;
			}
			if (!m_BottomScroll)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			if (m_TopScrollButton != null)
			{
				if (((Control)m_TopScrollButton).Parent != null)
				{
					((Control)m_TopScrollButton).Parent.Controls.Remove((Control)(object)m_TopScrollButton);
				}
				((Component)(object)m_TopScrollButton).Dispose();
				m_TopScrollButton = null;
			}
		}
		else
		{
			if (m_TopScrollButton == null)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					m_TopScrollButton = new ScrollButton();
					m_TopScrollButton.Orientation = eOrientation.Vertical;
					m_TopScrollButton.ButtonAlignment = eItemAlignment.Near;
					val.Controls.Add((Control)(object)m_TopScrollButton);
					((Control)m_TopScrollButton).Click += ScrollClick;
				}
			}
			if (m_TopScrollButton != null)
			{
				((Control)m_TopScrollButton).Location = m_Rect.Location;
				((Control)m_TopScrollButton).Size = new Size(m_Rect.Width, 10);
				((Control)m_TopScrollButton).BringToFront();
			}
		}
		if (flag2)
		{
			if (m_BottomScrollButton != null)
			{
				if (((Control)m_BottomScrollButton).Parent != null)
				{
					((Control)m_BottomScrollButton).Parent.Controls.Remove((Control)(object)m_BottomScrollButton);
				}
				((Component)(object)m_BottomScrollButton).Dispose();
				m_BottomScrollButton = null;
			}
			return;
		}
		if (m_BottomScrollButton == null)
		{
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val2 != null)
			{
				m_BottomScrollButton = new ScrollButton();
				m_BottomScrollButton.Orientation = eOrientation.Vertical;
				m_BottomScrollButton.ButtonAlignment = eItemAlignment.Far;
				val2.Controls.Add((Control)(object)m_BottomScrollButton);
				((Control)m_BottomScrollButton).Click += ScrollClick;
			}
		}
		if (m_BottomScrollButton != null)
		{
			((Control)m_BottomScrollButton).Location = new Point(m_Rect.X, m_Rect.Bottom - 10);
			((Control)m_BottomScrollButton).Size = new Size(m_Rect.Width, 10);
			((Control)m_BottomScrollButton).BringToFront();
		}
	}

	private void ScrollClick(object sender, EventArgs e)
	{
		if (sender == m_TopScrollButton)
		{
			m_ScrollTopPosition--;
		}
		else
		{
			m_ScrollTopPosition++;
		}
		RepositionItems();
		SetupScrollButtons();
		Refresh();
	}

	private void AlignItemsFar(Stack FarAlignItems, eOrientation orientation)
	{
		int num = 0;
		int groupLineSize = GroupLineSize;
		if (orientation == eOrientation.Horizontal)
		{
			num = m_Rect.Right - m_PaddingRight;
			if (m_MoreItems != null)
			{
				num -= m_MoreItems.WidthInternal;
			}
			while (FarAlignItems.Count > 0)
			{
				BaseItem baseItem = FarAlignItems.Pop() as BaseItem;
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.LeftInternal = num - GetItemLayoutWidth(baseItem);
					num = baseItem.LeftInternal - m_ItemSpacing;
					if (baseItem.BeginGroup)
					{
						num -= groupLineSize;
					}
				}
			}
			return;
		}
		num = m_Rect.Bottom - m_PaddingBottom;
		if (m_MoreItems != null)
		{
			num -= m_MoreItems.HeightInternal;
		}
		while (FarAlignItems.Count > 0)
		{
			BaseItem baseItem2 = FarAlignItems.Pop() as BaseItem;
			if (baseItem2.Displayed && baseItem2.Visible)
			{
				baseItem2.TopInternal = num - GetItemLayoutHeight(baseItem2);
				num = baseItem2.TopInternal - m_ItemSpacing;
				if (baseItem2.BeginGroup)
				{
					num -= groupLineSize;
				}
			}
		}
	}

	private void AlignItemsFar(ArrayList FarAlignLines, eOrientation orientation)
	{
		if (orientation == eOrientation.Horizontal)
		{
			foreach (Stack FarAlignLine in FarAlignLines)
			{
				AlignItemsFar(FarAlignLine, orientation);
			}
			return;
		}
		foreach (Stack FarAlignLine2 in FarAlignLines)
		{
			AlignItemsFar(FarAlignLine2, orientation);
		}
	}

	private void AlignItemsFar(ArrayList FarAlignLines)
	{
		AlignItemsFar(FarAlignLines, Orientation);
	}

	private void StretchItems(ArrayList StretchLines, ArrayList StretchLinesISCount, int CalculatedSize)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < StretchLines.Count; i++)
		{
			ArrayList arrayList = StretchLines[i] as ArrayList;
			num2 = ((m_Orientation != 0) ? ((HeightInternal - CalculatedSize) / (int)StretchLinesISCount[i]) : ((WidthInternal - CalculatedSize) / (int)StretchLinesISCount[i]));
			foreach (BaseItem item in arrayList)
			{
				if (item.Stretch)
				{
					if (m_Orientation == eOrientation.Horizontal)
					{
						item.LeftInternal += num;
						item.WidthInternal += num2;
					}
					else
					{
						item.TopInternal += num;
						item.HeightInternal += num2;
					}
					num += num2;
				}
				else if (m_Orientation == eOrientation.Horizontal)
				{
					item.LeftInternal += num;
				}
				else
				{
					item.TopInternal += num;
				}
			}
		}
	}

	protected virtual void CreateMoreItemsButton(bool isRightToLeft)
	{
		if (m_MoreItems == null)
		{
			m_MoreItems = new DisplayMoreItem();
			m_MoreItems.Style = m_Style;
			m_MoreItems.SetParent(this);
			m_MoreItems.ThemeAware = ThemeAware;
		}
		if (m_MoreItemsOnMenu)
		{
			m_MoreItems.PopupType = ePopupType.Menu;
		}
		else
		{
			m_MoreItems.PopupType = ePopupType.ToolBar;
		}
		m_MoreItems.Orientation = m_Orientation;
		m_MoreItems.Displayed = true;
		if (m_Orientation == eOrientation.Vertical)
		{
			m_MoreItems.WidthInternal = m_Rect.Width - (m_PaddingLeft + m_PaddingRight);
			m_MoreItems.RecalcSize();
		}
		else
		{
			m_MoreItems.HeightInternal = m_Rect.Height - (m_PaddingTop + m_PaddingBottom);
			m_MoreItems.RecalcSize();
		}
		Point moreItemsLocation = GetMoreItemsLocation(isRightToLeft);
		m_MoreItems.LeftInternal = moreItemsLocation.X;
		m_MoreItems.TopInternal = moreItemsLocation.Y;
	}

	protected virtual Point GetMoreItemsLocation(bool isRightToLeft)
	{
		if (m_MoreItems == null)
		{
			return Point.Empty;
		}
		Point empty = Point.Empty;
		if (m_Orientation == eOrientation.Vertical)
		{
			empty.X = m_Rect.Left + m_PaddingLeft;
			empty.Y = m_Rect.Bottom - m_MoreItems.HeightInternal - m_PaddingBottom;
		}
		else
		{
			if (isRightToLeft)
			{
				empty.X = m_Rect.X + m_PaddingLeft;
			}
			else
			{
				empty.X = m_Rect.Right - m_MoreItems.WidthInternal - m_PaddingRight;
			}
			empty.Y = m_Rect.Top + m_PaddingTop;
		}
		return empty;
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		NeedRecalcSize = true;
		if (Displayed)
		{
			if (m_Parent == null)
			{
				if (ContainerControl is Bar)
				{
					((Bar)ContainerControl).RecalcLayout();
				}
				else if (ContainerControl is MenuPanel)
				{
					((MenuPanel)ContainerControl).RecalcSize();
				}
			}
			else
			{
				m_Parent.SubItemSizeChanged(this);
			}
		}
		else if (m_Parent == null)
		{
			RecalcSize();
		}
		else
		{
			m_Parent.SubItemSizeChanged(this);
		}
	}

	public override void InternalMouseHover()
	{
		if (m_MoreItems != null && m_MoreItems.Expanded && m_HotSubItem != m_MoreItems && DesignMode)
		{
			m_MoreItems.Expanded = false;
		}
		base.InternalMouseHover();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (m_MoreItems != null && m_MoreItems.Expanded && m_HotSubItem != m_MoreItems && !DesignMode)
		{
			m_MoreItems.Expanded = false;
		}
		base.InternalMouseDown(objArg);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (m_HotSubItem == null && (IsOnMenuBar || LayoutType == eLayoutType.Toolbar))
		{
			BaseItem baseItem = ExpandedItem();
			if (baseItem != null && baseItem is PopupItem)
			{
				baseItem.InternalMouseUp(objArg);
			}
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		if (m_IgnoreDuplicateMouseMove)
		{
			if (m_LastMouseMoveEvent == null)
			{
				m_LastMouseMoveEvent = new MouseEventArgs(objArg.Button, objArg.Clicks, objArg.X, objArg.Y, objArg.Delta);
				return;
			}
			if (m_LastMouseMoveEvent.X == objArg.X && m_LastMouseMoveEvent.Y == objArg.Y && m_LastMouseMoveEvent.Button == objArg.Button)
			{
				return;
			}
			m_IgnoreDuplicateMouseMove = false;
			m_LastMouseMoveEvent = null;
		}
		if (m_MoreItems == null || !m_MoreItems.DisplayRectangle.Contains(objArg.X, objArg.Y) || DesignMode)
		{
			bool isOnMenuBar = IsOnMenuBar;
			if (isOnMenuBar || LayoutType == eLayoutType.Toolbar)
			{
				ExpandedItem()?.InternalMouseMove(objArg);
				m_CheckMouseMovePressed = true;
			}
			else
			{
				m_CheckMouseMovePressed = false;
			}
			if ((int)objArg.Button == 0 || DesignMode || isOnMenuBar || LayoutType == eLayoutType.Toolbar)
			{
				base.InternalMouseMove(objArg);
			}
			return;
		}
		BaseItem moreItems = m_MoreItems;
		if (moreItems != m_HotSubItem)
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (moreItems != null && m_HotSubItem.Expanded)
				{
					m_HotSubItem.Expanded = false;
				}
			}
			if (moreItems != null)
			{
				if (m_AutoExpand)
				{
					BaseItem baseItem = ExpandedItem();
					if (baseItem != null && baseItem != moreItems)
					{
						baseItem.Expanded = false;
					}
				}
				moreItems.InternalMouseEnter();
				moreItems.InternalMouseMove(objArg);
				if (m_AutoExpand && moreItems.GetEnabled() && moreItems.ShowSubItems)
				{
					if (moreItems is PopupItem)
					{
						PopupItem popupItem = moreItems as PopupItem;
						ePopupAnimation popupAnimation = popupItem.PopupAnimation;
						popupItem.PopupAnimation = ePopupAnimation.None;
						moreItems.Expanded = true;
						popupItem.PopupAnimation = popupAnimation;
					}
					else
					{
						moreItems.Expanded = true;
					}
				}
				m_HotSubItem = moreItems;
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

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Invalid comparison between Unknown and I4
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Invalid comparison between Unknown and I4
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		base.InternalKeyDown(objArg);
		if (SubItems.Count == 0 || objArg.Handled)
		{
			return;
		}
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null)
		{
			baseItem.InternalKeyDown(objArg);
			if (objArg.Handled)
			{
				return;
			}
		}
		if ((int)objArg.KeyCode == 39 || (int)objArg.KeyCode == 9 || (int)objArg.KeyCode == 37)
		{
			m_IgnoreDuplicateMouseMove = true;
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				if (m_AutoExpand && m_HotSubItem.Expanded)
				{
					m_HotSubItem.Expanded = false;
				}
			}
			if ((int)objArg.KeyCode == 37)
			{
				int num = 0;
				if (m_HotSubItem != null)
				{
					num = SubItems.IndexOf(m_HotSubItem) - 1;
				}
				if (num < 0)
				{
					num = SubItems.Count - 1;
				}
				BaseItem baseItem2 = null;
				bool flag = false;
				do
				{
					for (int num2 = num; num2 >= 0; num2--)
					{
						baseItem2 = SubItems[num2];
						if (baseItem2.Visible && baseItem2.GetEnabled())
						{
							num = num2;
							break;
						}
					}
					if (!SubItems[num].Visible || !SubItems[num].GetEnabled())
					{
						if (!flag)
						{
							num = SubItems.Count - 1;
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				while (flag);
				m_HotSubItem = SubItems[num];
			}
			else
			{
				int i = 0;
				if (m_HotSubItem != null)
				{
					i = SubItems.IndexOf(m_HotSubItem) + 1;
				}
				for (; i < SubItems.Count && (!SubItems[i].Visible || !SubItems[i].GetEnabled()); i++)
				{
				}
				if (i >= SubItems.Count)
				{
					i = 0;
				}
				BaseItem baseItem3 = null;
				for (int j = i; j < SubItems.Count; j++)
				{
					baseItem3 = SubItems[j];
					if (baseItem3.Visible && baseItem3.GetEnabled())
					{
						i = j;
						break;
					}
				}
				m_HotSubItem = SubItems[i];
			}
			m_HotSubItem.InternalMouseEnter();
			m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
			BaseItem baseItem4 = ExpandedItem();
			if (baseItem4 != null && baseItem4 != m_HotSubItem)
			{
				baseItem4.Expanded = false;
			}
			if (m_AutoExpand && m_HotSubItem.ShowSubItems && (IsOnMenuBar || (base.IsOnMenu && m_HotSubItem.SubItems.Count > 0)))
			{
				m_HotSubItem.Expanded = true;
				if (m_HotSubItem is PopupItem && ((PopupItem)m_HotSubItem).PopupControl is MenuPanel)
				{
					((MenuPanel)(object)((PopupItem)m_HotSubItem).PopupControl).SelectFirstItem();
				}
			}
			objArg.Handled = true;
		}
		if ((int)objArg.KeyCode == 40 && IsOnMenuBar)
		{
			m_IgnoreDuplicateMouseMove = true;
			if (m_HotSubItem != null && !m_HotSubItem.Expanded && m_HotSubItem.VisibleSubItems > 0)
			{
				m_HotSubItem.Expanded = true;
			}
			return;
		}
		if ((int)objArg.KeyCode == 27)
		{
			if (baseItem != null)
			{
				baseItem.Expanded = false;
				objArg.Handled = true;
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is Bar)
			{
				Bar bar = val as Bar;
				if (bar.BarState == eBarState.Popup)
				{
					bar.ParentItem.Expanded = false;
				}
				else if (AutoExpand)
				{
					AutoExpand = false;
				}
				else if (((Control)bar).Focused || bar.MenuFocus)
				{
					bar.MenuFocus = false;
					bar.ReleaseFocus();
				}
				objArg.Handled = true;
			}
			else if (val is ItemControl)
			{
				ItemControl itemControl = val as ItemControl;
				if (AutoExpand)
				{
					AutoExpand = false;
				}
				else if (((Control)itemControl).Focused || itemControl.MenuFocus)
				{
					itemControl.MenuFocus = false;
					itemControl.ReleaseFocus();
				}
			}
			return;
		}
		BaseItem baseItem5 = ExpandedItem();
		if (baseItem5 != null)
		{
			baseItem5.InternalKeyDown(objArg);
			return;
		}
		int num3 = 0;
		if (objArg.Shift)
		{
			try
			{
				byte[] array = new byte[256];
				if (NativeFunctions.GetKeyboardState(array))
				{
					byte[] array2 = new byte[2];
					if (NativeFunctions.ToAscii((uint)objArg.KeyValue, 0u, array, array2, 0u) != 0)
					{
						num3 = array2[0];
					}
				}
			}
			catch (Exception)
			{
				num3 = 0;
			}
		}
		if (num3 == 0)
		{
			num3 = (int)NativeFunctions.MapVirtualKey((uint)objArg.KeyValue, 2u);
		}
		bool flag2 = false;
		if (num3 > 0)
		{
			flag2 = AccessKeyDown(num3);
		}
		if (flag2)
		{
			objArg.Handled = true;
		}
		if (!flag2 && m_HotSubItem != null)
		{
			m_HotSubItem.InternalKeyDown(objArg);
		}
	}

	private bool AccessKeyDown(int KeyCode)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(KeyCode);
			Encoding.Default.GetDecoder().GetChars(array2, 0, 1, array, 0);
		}
		catch (Exception)
		{
			return false;
		}
		string text = array[0].ToString();
		if (text == "")
		{
			return false;
		}
		array[0] = text.ToLower()[0];
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null && baseItem is PopupItem && ((PopupItem)baseItem).PopupType == ePopupType.Menu)
		{
			KeyEventArgs val = new KeyEventArgs((Keys)KeyCode);
			baseItem.InternalKeyDown(val);
			if (val.Handled)
			{
				m_IgnoreDuplicateMouseMove = true;
				return true;
			}
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.Visible || !subItem.GetEnabled() || !subItem.Displayed || subItem.AccessKey != array[0] || (subItem is ButtonItem && ((ButtonItem)subItem).ButtonStyle == eButtonStyle.Default && !IsOnMenuBar && !base.IsOnMenu))
			{
				continue;
			}
			if (subItem.SubItems.Count > 0 && subItem.ShowSubItems && subItem.GetEnabled())
			{
				if (subItem.Expanded)
				{
					subItem.InternalKeyDown(new KeyEventArgs((Keys)KeyCode));
				}
				else
				{
					ExpandItem(subItem);
				}
			}
			else
			{
				subItem.RaiseClick();
			}
			m_IgnoreDuplicateMouseMove = true;
			return true;
		}
		return false;
	}

	internal bool SysKeyDown(int KeyCode)
	{
		return AccessKeyDown(KeyCode);
	}

	private void ExpandItem(BaseItem objItem)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is Bar && !val.Focused)
		{
			((Bar)(object)val).MenuFocus = true;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
		}
		m_HotSubItem = objItem;
		objItem.InternalMouseEnter();
		objItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, objItem.LeftInternal + 2, objItem.TopInternal + 2, 0));
		objItem.Expanded = true;
		if (objItem is PopupItem)
		{
			AutoExpand = true;
			PopupItem popupItem = objItem as PopupItem;
			if (popupItem.PopupType == ePopupType.Menu && popupItem.PopupControl is MenuPanel)
			{
				((MenuPanel)(object)popupItem.PopupControl).SelectFirstItem();
			}
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		if (m_MoreItems != null)
		{
			m_MoreItems.ContainerLostFocus(appLostFocus);
		}
		IOwnerMenuSupport ownerMenuSupport = GetOwner() as IOwnerMenuSupport;
		if (IsOnMenuBar && ownerMenuSupport != null)
		{
			ownerMenuSupport.PersonalizedAllVisible = false;
		}
	}

	protected override void OnOwnerChanged()
	{
		base.OnOwnerChanged();
		if (GetOwner() is Bar)
		{
			RefreshImageSize();
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

	internal void SetSystemFocus()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		if (m_HotSubItem != null || SubItems.Count == 0)
		{
			return;
		}
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null)
		{
			m_HotSubItem = baseItem;
			m_HotSubItem.InternalMouseEnter();
			m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.SystemItem && subItem.Displayed && subItem.Visible)
			{
				m_HotSubItem = subItem;
				m_HotSubItem.InternalMouseEnter();
				m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
				break;
			}
		}
	}

	internal void ReleaseSystemFocus()
	{
		BaseItem.CollapseSubItems(this);
		if (m_HotSubItem == null)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Point pt = val.PointToClient(Control.MousePosition);
			if (m_HotSubItem.DisplayRectangle.Contains(pt))
			{
				return;
			}
		}
		m_HotSubItem.InternalMouseLeave();
		m_HotSubItem = null;
	}

	protected internal override void OnItemAdded(BaseItem objItem)
	{
		base.OnItemAdded(objItem);
		if (ItemAdded != null)
		{
			ItemAdded(objItem, new EventArgs());
		}
		if (objItem is CustomizeItem)
		{
			m_HaveCustomizeItem = true;
		}
		else if (SystemContainer && objItem is DockContainerItem && ContainerControl is Bar { LayoutType: eLayoutType.DockContainer } bar)
		{
			if (base.VisibleSubItems > 1)
			{
				objItem.Displayed = false;
			}
			bar.RefreshDockTab(bFireEvents: true);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem objItem)
	{
		base.OnAfterItemRemoved(objItem);
		if (objItem is CustomizeItem)
		{
			m_HaveCustomizeItem = false;
		}
		else if (SystemContainer && ContainerControl is Bar bar)
		{
			bar.OnSubItemRemoved(objItem);
		}
	}

	protected internal override bool IsAnyOnHandle(IntPtr iHandle)
	{
		if (m_MoreItems != null && m_MoreItems.IsAnyOnHandle(iHandle))
		{
			return true;
		}
		return base.IsAnyOnHandle(iHandle);
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ItemAtLocation(x, y);
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				else if ((subItem.Visible || base.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y))
				{
					return subItem;
				}
			}
		}
		return null;
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, DragItem);
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}

	private int GetAppendPosition(BaseItem objParent)
	{
		int result = -1;
		int num = objParent.SubItems.Count - 1;
		while (num >= 0 && objParent.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}
}
