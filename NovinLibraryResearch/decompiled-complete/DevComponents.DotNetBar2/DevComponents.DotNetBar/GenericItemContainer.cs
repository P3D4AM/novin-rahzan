using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class GenericItemContainer : ImageItem, IDesignTimeProvider
{
	protected int m_PaddingLeft;

	protected int m_PaddingTop;

	protected int m_PaddingBottom;

	protected int m_PaddingRight;

	protected int m_ItemSpacing;

	private int ෂ;

	protected eBorderType m_BorderType;

	protected internal Color m_BackgroundColor;

	protected bool m_WrapItems;

	protected DisplayMoreItem m_MoreItems;

	private bool ස;

	private bool හ;

	private int ළ;

	private bool ෆ;

	private bool _0DC7;

	private eLayoutType _0DC8;

	private bool _0DC9;

	private int _0DCA;

	private bool _0DCB;

	private bool _0DCC;

	private _1DFE _0DCD;

	private _1DFE _0DCE;

	private bool _0DCF;

	private MouseEventArgs _0DD0;

	private int _0DD1;

	private int _0DD2;

	private bool _0DD3;

	private bool _0DD4 = true;

	private bool _0DD5 = true;

	private eContainerVerticalAlignment _0DD6 = eContainerVerticalAlignment.Bottom;

	private bool _0DD7 = true;

	internal bool _0DD8 = true;

	internal EventHandler ৎ;

	private bool __0DD9;

	internal int _0B03
	{
		get
		{
			return _0DD2;
		}
		set
		{
			_0DD2 = value;
		}
	}

	internal int ර
	{
		get
		{
			return _0DD1;
		}
		set
		{
			_0DD1 = value;
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
			return ෂ;
		}
		set
		{
			ෂ = value;
		}
	}

	public bool MoreItemsOnMenu
	{
		get
		{
			return ස;
		}
		set
		{
			ස = value;
		}
	}

	internal DisplayMoreItem _0DBC => m_MoreItems;

	private int ල
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
			return හ;
		}
		set
		{
			if (හ != value)
			{
				හ = value;
				NeedRecalcSize = true;
			}
		}
	}

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
			return ෆ;
		}
		set
		{
			ෆ = value;
		}
	}

	public bool HaveCustomizeItem => _0DC7;

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
				if (AutoExpand && !bar._09A9)
				{
					bar._09A9 = true;
				}
				else if (!AutoExpand && bar._09A9)
				{
					bar._09A9 = false;
				}
			}
		}
	}

	internal bool _0DBE
	{
		get
		{
			return _0DD4;
		}
		set
		{
			_0DD4 = value;
		}
	}

	internal bool _0DBF
	{
		get
		{
			return _0DD5;
		}
		set
		{
			_0DD5 = value;
		}
	}

	internal eContainerVerticalAlignment ව
	{
		get
		{
			return _0DD6;
		}
		set
		{
			_0DD6 = value;
		}
	}

	public virtual eLayoutType LayoutType
	{
		get
		{
			return _0DC8;
		}
		set
		{
			if (_0DC8 == value)
			{
				return;
			}
			_0DC8 = value;
			NeedRecalcSize = true;
			if (_0DC8 == eLayoutType.TaskList)
			{
				base.Orientation = eOrientation.Horizontal;
			}
			else if (ContainerControl != null && ContainerControl is Bar)
			{
				Bar bar = ContainerControl as Bar;
				if (bar.DockedSite != null && bar.DockedSite is DockSite)
				{
					base.Orientation = ((DockSite)(object)bar.DockedSite).ಅ;
				}
			}
		}
	}

	[Browsable(false)]
	public override eOrientation Orientation
	{
		get
		{
			if (_0DC8 == eLayoutType.TaskList)
			{
				return eOrientation.Vertical;
			}
			return base.Orientation;
		}
		set
		{
			if (_0DC8 != eLayoutType.TaskList)
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Size SubItemsImageSize
	{
		get
		{
			return base.SubItemsImageSize;
		}
		set
		{
			if (_0DD7)
			{
				base.SubItemsImageSize = value;
			}
		}
	}

	internal bool ශ
	{
		get
		{
			return _0DD7;
		}
		set
		{
			_0DD7 = value;
			if (!_0DD7)
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
			return __0DD9;
		}
		set
		{
			__0DD9 = value;
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
		හ = false;
		ස = false;
		ෆ = false;
		AccessibleRole = (AccessibleRole)20;
	}

	protected override void Dispose(bool disposing)
	{
		if (_0DCD != null)
		{
			if (((Control)_0DCD).Parent != null)
			{
				((Control)_0DCD).Parent.Controls.Remove((Control)(object)_0DCD);
			}
			((Component)(object)_0DCD).Dispose();
			_0DCD = null;
		}
		if (_0DCE != null)
		{
			if (((Control)_0DCE).Parent != null)
			{
				((Control)_0DCE).Parent.Controls.Remove((Control)(object)_0DCE);
			}
			((Component)(object)_0DCE).Dispose();
			_0DCE = null;
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
		genericItemContainer.EqualButtonSize = හ;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		if (SuspendLayout || _0DD3)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		_0DD3 = true;
		try
		{
			Graphics graphics = pa.Graphics;
			if (m_NeedRecalcSize)
			{
				RecalcSize();
			}
			_ = m_Rect.Left;
			_ = m_Rect.Top;
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
				_089F.र(graphics, val, m_Rect);
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
				_0A7B obj = null;
				_0F6F obj2 = _0F6F._0F74;
				_0F76 ཊ = _0F76.ཊ;
				if (base.IsThemed)
				{
					obj = pa._0A7B;
				}
				for (int i = 0; i < m_SubItems.Count; i++)
				{
					baseItem = m_SubItems[i];
					if (baseItem.Visible && baseItem.Displayed)
					{
						if (baseItem.BeginGroup && num > 0 && _0DC8 != eLayoutType.DockContainer && (!(baseItem is ItemContainer) || !BarFunctions.IsOffice2007Style(baseItem.EffectiveStyle)))
						{
							if (m_Orientation == eOrientation.Vertical || _0DC8 == eLayoutType.TaskList)
							{
								if (baseItem.TopInternal == m_PaddingTop + m_Rect.Top || baseItem.LeftInternal > m_PaddingLeft + m_Rect.Left)
								{
									if (effectiveStyle == eDotNetBarStyle.Office2000)
									{
										ControlPaint.DrawBorder3D(graphics, baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 2, m_Rect.Height - m_PaddingTop - m_PaddingBottom - 4, (Border3DStyle)6, (Border3DSide)1);
									}
									else if (obj != null)
									{
										obj2 = _0F6F._0F74;
										obj._0EE5(graphics, obj2, ཊ, new Rectangle(baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 6, m_Rect.Height - m_PaddingBottom - 2));
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
									else if (obj != null)
									{
										obj2 = _0F6F._0F75;
										obj._0EE5(graphics, obj2, ཊ, new Rectangle(point.X, point.Y, point2.X - point.X, 6));
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
								else if (obj != null)
								{
									obj2 = _0F6F._0F74;
									obj._0EE5(graphics, obj2, ཊ, new Rectangle(baseItem.LeftInternal - 4, baseItem.TopInternal + 1, 6, baseItem.HeightInternal - 3));
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
										else
										{
											graphics.DrawLine(val7, baseItem.LeftInternal - 2, baseItem.TopInternal + 4, baseItem.LeftInternal - 2, baseItem.TopInternal + 4 + baseItem.HeightInternal - 8);
											Pen val8 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
											graphics.DrawLine(val8, baseItem.LeftInternal - 1, baseItem.TopInternal + 5, baseItem.LeftInternal - 1, baseItem.TopInternal + 5 + baseItem.HeightInternal - 8);
											val8.Dispose();
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
							else if (obj != null)
							{
								obj2 = _0F6F._0F75;
								obj._0EE5(graphics, obj2, ཊ, new Rectangle(m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 4, m_Rect.Width - m_PaddingRight - 2, 6));
							}
							else
							{
								Pen val9 = new Pen(pa.Colors.ItemSeparator);
								try
								{
									if (effectiveStyle == eDotNetBarStyle.OfficeXP)
									{
										graphics.DrawLine(val9, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, baseItem.TopInternal - 2);
									}
									else
									{
										graphics.DrawLine(val9, m_Rect.Left + m_PaddingLeft + 2, baseItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, baseItem.TopInternal - 2);
										Pen val10 = new Pen(pa.Colors.ItemSeparatorShade, 1f);
										graphics.DrawLine(val10, m_Rect.Left + m_PaddingLeft + 6, baseItem.TopInternal - 1, m_Rect.Right - m_PaddingRight - 4, baseItem.TopInternal - 1);
										val10.Dispose();
									}
								}
								finally
								{
									((IDisposable)val9)?.Dispose();
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
			_0DD3 = false;
		}
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			base.IsLayoutPass = true;
			m_RecalculatingSize = true;
			switch (_0DC8)
			{
			case eLayoutType.Toolbar:
				_0DDC();
				break;
			case eLayoutType.TaskList:
				_0DDD();
				break;
			case eLayoutType.DockContainer:
				_0DDA(_0DDB: false);
				break;
			}
			base.RecalcSize();
			m_RecalculatingSize = false;
			base.IsLayoutPass = false;
		}
	}

	private void _0DDA(bool _0DDB)
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
		ළ = 0;
		int num5 = Dpi.Width(m_PaddingLeft);
		num = num5 + m_Rect.Left;
		int num6 = Dpi.Height(m_PaddingTop);
		num2 = num6 + m_Rect.Top;
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
				ළ++;
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
		int num7 = Dpi.Width(m_PaddingRight);
		int num8 = Dpi.Height(m_PaddingBottom);
		if (_0DDB || Stretch)
		{
			if (num3 + num5 + num7 < m_Rect.Width || num4 + num6 + num8 < m_Rect.Height)
			{
				if (num3 + num5 + num7 < m_Rect.Width)
				{
					num3 = m_Rect.Width - num5 - num7;
				}
				if (num4 + num6 + num8 < m_Rect.Height)
				{
					num4 = m_Rect.Height - num6 - num8;
				}
			}
			if (ContainerControl is Bar bar && (bar.BarState == eBarState.Docked || bar.BarState == eBarState.AutoHide))
			{
				if (ර > 0 && num3 + num5 + num7 < ර && Orientation == eOrientation.Vertical)
				{
					num3 = ර - (num5 + num7);
				}
				else if (_0B03 > 0 && num4 + num6 + num8 < _0B03 && Orientation == eOrientation.Horizontal)
				{
					num4 = _0B03 - (num6 + num8);
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
				m_Rect.Height = num6 + num4 + num8;
				if (m_Rect.Width < num3 + num5 + num7)
				{
					m_Rect.Width = num5 + num3 + num7;
				}
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = num5 + num3 + num7;
				if (m_Rect.Height < num4 + num8 + num6)
				{
					m_Rect.Height = num6 + num4 + num8;
				}
			}
		}
		else
		{
			m_Rect.Width = num5 + num3 + num7;
			m_Rect.Height = num6 + num4 + num8;
		}
	}

	private void _0DDC()
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
		int num11 = Dpi.Width(m_PaddingLeft);
		num = num11 + m_Rect.Left;
		int num12 = Dpi.Height(m_PaddingTop);
		num2 = num12 + m_Rect.Top;
		int visibleSubItems = base.VisibleSubItems;
		int num13 = 0;
		int num14 = Dpi.Width(m_PaddingRight);
		int num15 = Dpi.Height(m_PaddingBottom);
		if (m_SubItems != null)
		{
			bool flag5 = false;
			int num16 = 0;
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
			int num17 = Dpi.Width(m_ItemSpacing);
			int num18 = Dpi.Width(ෂ);
			do
			{
				num10 = 0;
				for (int i = 0; i < m_SubItems.Count; i++)
				{
					baseItem = m_SubItems[i];
					if (baseItem != null && baseItem.Visible)
					{
						num13++;
						if ((baseItem.SupportedOrientation == eSupportedOrientation.Horizontal && m_Orientation == eOrientation.Vertical) || (baseItem.SupportedOrientation == eSupportedOrientation.Vertical && m_Orientation == eOrientation.Horizontal))
						{
							baseItem.Displayed = false;
							continue;
						}
						int num19 = ((num10 > 1 || num18 == 0) ? num17 : num18);
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
								if (num + baseItem.WidthInternal + num19 > m_Rect.Right && num > num11 + m_Rect.Left)
								{
									num2 += num3 + num19;
									if (baseItem.BeginGroup)
									{
										num2 += num8;
									}
									num = num11 + m_Rect.Left;
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
								if (_0DD8 && ((num + baseItem.WidthInternal + num19 > m_Rect.Right - num14 && num > num11 + m_Rect.Left) || (num + baseItem.WidthInternal + num19 + DisplayMoreItem.FixedSize > m_Rect.Right && i + 1 < m_SubItems.Count && num13 < visibleSubItems)))
								{
									if (i == 0)
									{
										num3 = baseItem.HeightInternal;
									}
									num += num19 + DisplayMoreItem.FixedSize;
									flag2 = false;
								}
							}
							if (flag2)
							{
								if (num10 > 0)
								{
									num += num19;
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
								if (num2 + baseItem.HeightInternal + num19 > m_Rect.Bottom && num2 > num12 + m_Rect.Top)
								{
									num += num3 + num19;
									if (baseItem.BeginGroup)
									{
										num += num8;
									}
									num2 = num12 + m_Rect.Top;
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
								if ((num2 + baseItem.HeightInternal + num19 > m_Rect.Bottom && num2 > num12 + m_Rect.Top) || (num2 + baseItem.HeightInternal + num19 + DisplayMoreItem.FixedSize > m_Rect.Bottom && i + 1 < m_SubItems.Count && num13 < visibleSubItems))
								{
									if (i == 0)
									{
										num3 = baseItem.WidthInternal;
									}
									num2 += num19 + DisplayMoreItem.FixedSize;
									flag2 = false;
								}
							}
							if (flag2)
							{
								if (num10 > 0)
								{
									num2 += num19;
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
				if (m_WrapItems && num9 > 0 && num16 < 1)
				{
					if (m_Orientation == eOrientation.Horizontal && (num6 > m_Rect.Width || flag3))
					{
						flag5 = true;
						m_Rect.Width = num6 + num11 + num14;
						num = num11 + m_Rect.Left;
						num2 = num12 + m_Rect.Top;
						num6 = 0;
						num7 = 0;
					}
					else if (m_Orientation == eOrientation.Vertical && (num7 > m_Rect.Height || flag3))
					{
						flag5 = true;
						m_Rect.Height = num7 + num12 + num15;
						num = num11 + m_Rect.Left;
						num2 = num12 + m_Rect.Top;
						num6 = 0;
						num7 = 0;
					}
					if (flag3)
					{
						if (_0DD4)
						{
							flag4 = true;
						}
						flag3 = false;
					}
					if (flag5 && num16 + 1 < 2)
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
				num16++;
			}
			while (flag5 && num16 < 2);
		}
		if (හ)
		{
			num = num11 + m_Rect.Left;
			num2 = num12 + m_Rect.Top;
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
				int num20 = ((num10 > 1 || ෂ == 0) ? m_ItemSpacing : ෂ);
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
						if (num + baseItem.WidthInternal + num20 > m_Rect.Right && num > num11 + num14)
						{
							num2 += num3 + num20;
							if (baseItem.BeginGroup)
							{
								num2 += num8;
							}
							num = num11 + m_Rect.Left;
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
						if ((num + baseItem.WidthInternal + num20 > m_Rect.Right && num > num11 + m_Rect.Left) || (num + baseItem.WidthInternal + num20 + DisplayMoreItem.FixedSize > m_Rect.Right && j + 1 < m_SubItems.Count && num13 < visibleSubItems))
						{
							num += num20 + DisplayMoreItem.FixedSize;
							flag2 = false;
						}
					}
					if (flag2)
					{
						baseItem.LeftInternal = num;
						baseItem.TopInternal = num2;
						num += baseItem.WidthInternal + num20;
						if (baseItem.HeightInternal > num3)
						{
							num3 = baseItem.HeightInternal;
						}
					}
					break;
				case eOrientation.Vertical:
					if (m_WrapItems)
					{
						if (num2 + baseItem.HeightInternal + num20 > m_Rect.Bottom && num2 > num12 + num15)
						{
							num += num3 + num20;
							num2 = num12 + m_Rect.Top;
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
					else if (flag2 && ((num2 + baseItem.HeightInternal + num20 > m_Rect.Bottom && num2 > num12 + m_Rect.Top) || (num2 + baseItem.HeightInternal + num20 + DisplayMoreItem.FixedSize > m_Rect.Bottom && j + 1 < m_SubItems.Count && num13 < visibleSubItems)))
					{
						num2 += num20 + DisplayMoreItem.FixedSize;
						flag2 = false;
					}
					if (flag2)
					{
						baseItem.LeftInternal = num;
						baseItem.TopInternal = num2;
						num2 += baseItem.HeightInternal + num20;
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
		ළ = num10;
		if (__0DD9)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				num4 = m_Rect.Width - num11;
			}
			else
			{
				num5 = m_Rect.Height - num12;
			}
			flag3 = true;
		}
		if (flag3)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				foreach (BaseItem subItem in SubItems)
				{
					if (_0DD4)
					{
						subItem.WidthInternal = num4;
					}
					else if (_0DD6 == eContainerVerticalAlignment.Bottom)
					{
						subItem.LeftInternal += num4 - subItem.WidthInternal;
					}
					else if (_0DD6 == eContainerVerticalAlignment.Middle)
					{
						subItem.LeftInternal += (num4 - subItem.WidthInternal) / 2;
					}
				}
			}
			else
			{
				foreach (BaseItem subItem2 in SubItems)
				{
					if (_0DD4)
					{
						subItem2.HeightInternal = num5;
					}
					else if (_0DD6 == eContainerVerticalAlignment.Bottom)
					{
						subItem2.TopInternal += num5 - subItem2.HeightInternal;
					}
					else if (_0DD6 == eContainerVerticalAlignment.Middle && num5 != subItem2.HeightInternal)
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
				m_Rect.Height = num7 + num15 - m_Rect.Top;
				if (arrayList2 != null && arrayList2.Count > 0)
				{
					_0DE5(arrayList2, arrayList3, num6 + num14 - m_Rect.Left);
				}
				if (m_Rect.Width < num6 + num14 - m_Rect.Left)
				{
					m_Rect.Width = num6 + num14 - m_Rect.Left;
				}
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = num6 + num14 - m_Rect.Left;
				if (arrayList2 != null && arrayList2.Count > 0)
				{
					_0DE5(arrayList2, arrayList3, num7 + num15 - m_Rect.Top);
				}
				if (m_Rect.Height < num7 + num15 - m_Rect.Top)
				{
					m_Rect.Height = num7 + num15 - m_Rect.Top;
				}
			}
			else
			{
				m_Rect.Width = num6 + num14 - m_Rect.Left;
				m_Rect.Height = num7 + num15 - m_Rect.Top;
			}
		}
		else if (!m_WrapItems && !flag2)
		{
			if (m_Orientation == eOrientation.Horizontal)
			{
				m_Rect.Height = num7 + num15 - m_Rect.Top;
			}
			else if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Width = num6 + num14 - m_Rect.Left;
			}
		}
		else if (ළ > 0)
		{
			m_Rect.Width = num6 + num14 - m_Rect.Left;
			m_Rect.Height = num7 + num15 - m_Rect.Top;
		}
		if (!m_WrapItems && !flag2 && _0DD5)
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
			_0DE1(arrayList);
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

	private void _0DDD()
	{
		int num = Dpi.Width(m_PaddingTop);
		int num2 = Dpi.Height(m_PaddingLeft);
		int leftInternal = m_Rect.Left + num2;
		int num3 = m_Rect.Top + num;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 3;
		bool flag = true;
		int num9 = 0;
		Stack stack = null;
		if (EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			num8 = 6;
		}
		if (හ)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				subItem.Orientation = eOrientation.Horizontal;
				subItem.RecalcSize();
				if (subItem.WidthInternal > num4)
				{
					num4 = subItem.WidthInternal;
				}
				if (subItem.HeightInternal > num5)
				{
					num5 = subItem.HeightInternal;
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
			if (!හ)
			{
				baseItem2.RecalcSize();
			}
			if (baseItem2.BeginGroup && num9 > 0)
			{
				num3 += num8;
			}
			if (!හ)
			{
				if (baseItem2.WidthInternal > num4)
				{
					num4 = baseItem2.WidthInternal;
				}
				if (baseItem2.HeightInternal > num5)
				{
					num5 = baseItem2.HeightInternal;
				}
			}
			else
			{
				baseItem2.WidthInternal = num4;
				baseItem2.HeightInternal = num5;
			}
			baseItem2.TopInternal = num3;
			baseItem2.LeftInternal = leftInternal;
			num3 += baseItem2.HeightInternal;
			baseItem2.Displayed = flag;
			if (!flag)
			{
				continue;
			}
			if (num3 > m_Rect.Bottom)
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
			num9++;
			if (baseItem2.ItemAlignment == eItemAlignment.Far)
			{
				if (stack == null)
				{
					stack = new Stack(10);
				}
				stack.Push(baseItem2);
			}
		}
		int num10 = Dpi.Width(m_PaddingRight);
		num6 = num4 + num2 + num10;
		int num11 = Dpi.Width(m_PaddingBottom);
		num7 = num3 + num11 - m_Rect.Top;
		ළ = num9;
		if (Stretch)
		{
			if (flag && stack != null && stack.Count > 0)
			{
				_0DE1(stack, eOrientation.Vertical);
			}
			if (!(ContainerControl is Bar bar) || (bar != null && bar.DockSide != eDockSide.Top && bar.DockSide != eDockSide.Bottom))
			{
				m_Rect.Width = num6;
			}
			if (m_Rect.Height < num7)
			{
				if (m_Rect.Height < 36)
				{
					m_Rect.Height = 36;
				}
				_0DC9 = true;
				_0DDE();
			}
			else
			{
				_0DC9 = false;
				_0DCA = 0;
			}
		}
		else
		{
			m_Rect.Width = num6;
			if (m_Rect.Height > num7 && flag)
			{
				m_Rect.Height = num7;
			}
			if (!flag)
			{
				if (m_Rect.Height < 36)
				{
					m_Rect.Height = 36;
				}
				_0DC9 = true;
				_0DDE();
			}
			else
			{
				_0DC9 = false;
				_0DCA = 0;
			}
		}
		if (m_MoreItems != null)
		{
			m_MoreItems.Dispose();
			m_MoreItems = null;
		}
		_0DDF();
	}

	private void _0DDE()
	{
		int num = 0;
		int num2 = 0;
		int num3 = ල;
		bool flag = true;
		_0DCB = false;
		_0DCC = false;
		for (int num4 = _0DCA - 1; num4 >= 0; num4--)
		{
			BaseItem baseItem = m_SubItems[num4];
			if (baseItem.Visible)
			{
				num = (baseItem.TopInternal = num - baseItem.HeightInternal);
				baseItem.Displayed = false;
				if (baseItem.BeginGroup && num4 > 0)
				{
					num -= num3;
				}
			}
		}
		num = m_Rect.Top;
		for (int i = _0DCA; i < m_SubItems.Count; i++)
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
					num += num3;
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
		ළ = num2;
		if (!flag)
		{
			_0DCC = true;
		}
		if (_0DCA > 0)
		{
			_0DCB = true;
		}
	}

	private void _0DDF()
	{
		bool flag = false;
		bool flag2 = false;
		if (!_0DC9)
		{
			flag = true;
			flag2 = true;
		}
		else
		{
			if (!_0DCB)
			{
				flag = true;
			}
			if (!_0DCC)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			if (_0DCD != null)
			{
				if (((Control)_0DCD).Parent != null)
				{
					((Control)_0DCD).Parent.Controls.Remove((Control)(object)_0DCD);
				}
				((Component)(object)_0DCD).Dispose();
				_0DCD = null;
			}
		}
		else
		{
			if (_0DCD == null)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					_0DCD = new _1DFE();
					_0DCD.ᡥ = eOrientation.Vertical;
					_0DCD._1DFF = eItemAlignment.Near;
					val.Controls.Add((Control)(object)_0DCD);
					((Control)_0DCD).Click += _0DE0;
				}
			}
			if (_0DCD != null)
			{
				((Control)_0DCD).Location = m_Rect.Location;
				((Control)_0DCD).Size = new Size(m_Rect.Width, 10);
				((Control)_0DCD).BringToFront();
			}
		}
		if (flag2)
		{
			if (_0DCE != null)
			{
				if (((Control)_0DCE).Parent != null)
				{
					((Control)_0DCE).Parent.Controls.Remove((Control)(object)_0DCE);
				}
				((Component)(object)_0DCE).Dispose();
				_0DCE = null;
			}
			return;
		}
		if (_0DCE == null)
		{
			object containerControl2 = ContainerControl;
			Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
			if (val2 != null)
			{
				_0DCE = new _1DFE();
				_0DCE.ᡥ = eOrientation.Vertical;
				_0DCE._1DFF = eItemAlignment.Far;
				val2.Controls.Add((Control)(object)_0DCE);
				((Control)_0DCE).Click += _0DE0;
			}
		}
		if (_0DCE != null)
		{
			((Control)_0DCE).Location = new Point(m_Rect.X, m_Rect.Bottom - 10);
			((Control)_0DCE).Size = new Size(m_Rect.Width, 10);
			((Control)_0DCE).BringToFront();
		}
	}

	private void _0DE0(object _٠, EventArgs ؿ)
	{
		if (_٠ == _0DCD)
		{
			_0DCA--;
		}
		else
		{
			_0DCA++;
		}
		_0DDE();
		_0DDF();
		Refresh();
	}

	private void _0DE1(Stack _0DE2, eOrientation _0DE3)
	{
		int num = 0;
		int num2 = ල;
		if (_0DE3 == eOrientation.Horizontal)
		{
			int num3 = Dpi.Width(m_PaddingRight);
			num = m_Rect.Right - num3;
			if (m_MoreItems != null)
			{
				num -= m_MoreItems.WidthInternal;
			}
			while (_0DE2.Count > 0)
			{
				BaseItem baseItem = _0DE2.Pop() as BaseItem;
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.LeftInternal = num - GetItemLayoutWidth(baseItem);
					num = baseItem.LeftInternal - m_ItemSpacing;
					if (baseItem.BeginGroup)
					{
						num -= num2;
					}
				}
			}
			return;
		}
		int num4 = Dpi.Height(m_PaddingBottom);
		num = m_Rect.Bottom - num4;
		if (m_MoreItems != null)
		{
			num -= m_MoreItems.HeightInternal;
		}
		while (_0DE2.Count > 0)
		{
			BaseItem baseItem2 = _0DE2.Pop() as BaseItem;
			if (baseItem2.Displayed && baseItem2.Visible)
			{
				baseItem2.TopInternal = num - GetItemLayoutHeight(baseItem2);
				num = baseItem2.TopInternal - m_ItemSpacing;
				if (baseItem2.BeginGroup)
				{
					num -= num2;
				}
			}
		}
	}

	private void _0DE1(ArrayList _0DE4, eOrientation _0DE3)
	{
		if (_0DE3 == eOrientation.Horizontal)
		{
			foreach (Stack item in _0DE4)
			{
				_0DE1(item, _0DE3);
			}
			return;
		}
		foreach (Stack item2 in _0DE4)
		{
			_0DE1(item2, _0DE3);
		}
	}

	private void _0DE1(ArrayList _0DE4)
	{
		_0DE1(_0DE4, Orientation);
	}

	private void _0DE5(ArrayList _෦, ArrayList _෧, int _෨)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < _෦.Count; i++)
		{
			ArrayList arrayList = _෦[i] as ArrayList;
			num2 = ((m_Orientation != 0) ? ((HeightInternal - _෨) / (int)_෧[i]) : ((WidthInternal - _෨) / (int)_෧[i]));
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
		if (ස)
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
			int num = Dpi.Width(m_PaddingLeft);
			int num2 = Dpi.Width(m_PaddingRight);
			m_MoreItems.WidthInternal = m_Rect.Width - (num + num2);
			m_MoreItems.RecalcSize();
		}
		else
		{
			int num3 = Dpi.Width(m_PaddingTop);
			int num4 = Dpi.Height(m_PaddingBottom);
			m_MoreItems.HeightInternal = m_Rect.Height - (num3 + num4);
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
		int num = Dpi.Width(m_PaddingLeft);
		if (m_Orientation == eOrientation.Vertical)
		{
			empty.X = m_Rect.Left + num;
			int num2 = Dpi.Height(m_PaddingBottom);
			empty.Y = m_Rect.Bottom - m_MoreItems.HeightInternal - num2;
		}
		else
		{
			if (isRightToLeft)
			{
				empty.X = m_Rect.X + num;
			}
			else
			{
				int num3 = Dpi.Width(m_PaddingRight);
				empty.X = m_Rect.Right - m_MoreItems.WidthInternal - num3;
			}
			empty.Y = m_Rect.Top + Dpi.Height(m_PaddingTop);
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		if (_0DCF)
		{
			if (_0DD0 == null)
			{
				_0DD0 = new MouseEventArgs(objArg.Button, objArg.Clicks, objArg.X, objArg.Y, objArg.Delta);
				return;
			}
			if (_0DD0.X == objArg.X && _0DD0.Y == objArg.Y && _0DD0.Button == objArg.Button)
			{
				return;
			}
			_0DCF = false;
			_0DD0 = null;
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
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Invalid comparison between Unknown and I4
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Invalid comparison between Unknown and I4
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
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
			_0DCF = true;
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
					((MenuPanel)(object)((PopupItem)m_HotSubItem).PopupControl)._1030();
				}
			}
			objArg.Handled = true;
		}
		if ((int)objArg.KeyCode == 40 && IsOnMenuBar)
		{
			_0DCF = true;
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
				else if (((Control)bar).Focused || bar._09A9)
				{
					bar._09A9 = false;
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
				else if (((Control)itemControl).Focused || itemControl._09A9)
				{
					itemControl._09A9 = false;
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
				if (_089F.GetKeyboardState(array))
				{
					byte[] array2 = new byte[2];
					if (_089F.ToAscii((uint)objArg.KeyValue, 0u, array, array2, 0u) != 0)
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
			num3 = (int)_089F.MapVirtualKey((uint)objArg.KeyValue, 2u);
		}
		bool flag2 = false;
		if (num3 > 0)
		{
			flag2 = _෩(num3);
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

	private bool _෩(int _෪)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(_෪);
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
			KeyEventArgs val = new KeyEventArgs((Keys)_෪);
			baseItem.InternalKeyDown(val);
			if (val.Handled)
			{
				_0DCF = true;
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
					subItem.InternalKeyDown(new KeyEventArgs((Keys)_෪));
				}
				else
				{
					_෬(subItem);
				}
			}
			else
			{
				subItem.RaiseClick();
			}
			_0DCF = true;
			return true;
		}
		return false;
	}

	internal bool _෫(int _෪)
	{
		return _෩(_෪);
	}

	private void _෬(BaseItem ਤ)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is Bar && !val.Focused)
		{
			((Bar)(object)val)._09A9 = true;
		}
		if (m_HotSubItem != null)
		{
			m_HotSubItem.InternalMouseLeave();
		}
		m_HotSubItem = ਤ;
		ਤ.InternalMouseEnter();
		ਤ.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, ਤ.LeftInternal + 2, ਤ.TopInternal + 2, 0));
		ਤ.Expanded = true;
		if (ਤ is PopupItem)
		{
			AutoExpand = true;
			PopupItem popupItem = ਤ as PopupItem;
			if (popupItem.PopupType == ePopupType.Menu && popupItem.PopupControl is MenuPanel)
			{
				((MenuPanel)(object)popupItem.PopupControl)._1030();
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

	internal void ௐ()
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

	internal void ඹ()
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
		if (ৎ != null)
		{
			ৎ(objItem, new EventArgs());
		}
		if (objItem is CustomizeItem)
		{
			_0DC7 = true;
		}
		else if (SystemContainer && objItem is DockContainerItem && ContainerControl is Bar { LayoutType: eLayoutType.DockContainer } bar)
		{
			if (base.VisibleSubItems > 1)
			{
				objItem.Displayed = false;
			}
			bar.ౘ(ౙ: true);
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem objItem, int itemIndex)
	{
		base.OnAfterItemRemoved(objItem, itemIndex);
		if (objItem is CustomizeItem)
		{
			_0DC7 = false;
		}
		else if (SystemContainer && ContainerControl is Bar bar)
		{
			bar._0C5D(objItem);
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

	private int _෭(BaseItem _෮)
	{
		int result = -1;
		int num = _෮.SubItems.Count - 1;
		while (num >= 0 && _෮.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}
}
