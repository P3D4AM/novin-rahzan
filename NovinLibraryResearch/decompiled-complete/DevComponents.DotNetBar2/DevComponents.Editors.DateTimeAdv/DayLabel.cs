using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors.DateTimeAdv;

public class DayLabel : PopupItem
{
	private ElementStyle _ᠩ = new ElementStyle();

	private Rectangle __1379 = Rectangle.Empty;

	private DayPaintEventHandler _2398;

	private DateTime __25A8 = DateTime.MinValue;

	private bool __25A9;

	private bool __25AA;

	private bool __25AB;

	private bool _ᆿ;

	private bool __171A;

	private bool _ԅ;

	private bool __25AC = true;

	private bool _Ԇ = true;

	private Color __0CD1 = Color.Empty;

	private bool __25AD;

	private eLabelPartAlignment __25AE = eLabelPartAlignment.MiddleCenter;

	private eLabelPartAlignment __25AF = eLabelPartAlignment.MiddleRight;

	private Image _ነ;

	private bool __25B0;

	private bool __25B1;

	private bool __25B2;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime Date
	{
		get
		{
			return __25A8;
		}
		set
		{
			if (__25A8 != value)
			{
				__25A8 = value;
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsDayLabel
	{
		get
		{
			return __25A9;
		}
		set
		{
			if (__25A9 != value)
			{
				__25A9 = value;
				Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsWeekOfYear
	{
		get
		{
			return __25AA;
		}
		set
		{
			if (__25AA != value)
			{
				__25AA = value;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsTrailing
	{
		get
		{
			return __25AB;
		}
		set
		{
			if (__25AB != value)
			{
				__25AB = value;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		set
		{
			_ᆿ = value;
			Refresh();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return __171A;
		}
		set
		{
			__171A = value;
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Gets or sets container background style.")]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsSelected
	{
		get
		{
			return _ԅ;
		}
		set
		{
			if (_ԅ != value)
			{
				_ԅ = value;
				Refresh();
			}
		}
	}

	[Description("Indicates whether label provides visual indicator when mouse is over the label or pressed while over the label.")]
	[DefaultValue(true)]
	public bool TrackMouse
	{
		get
		{
			return __25AC;
		}
		set
		{
			if (__25AC != value)
			{
				__25AC = value;
				Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether label is selectable.")]
	public bool Selectable
	{
		get
		{
			return _Ԇ;
		}
		set
		{
			if (_Ԇ != value)
			{
				_Ԇ = value;
				Refresh();
			}
		}
	}

	[Description("Indicates label text color.")]
	[Category("Colors")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether text is drawn using Bold font.")]
	public bool IsBold
	{
		get
		{
			return __25AD;
		}
		set
		{
			if (__25AD != value)
			{
				__25AD = value;
				Refresh();
			}
		}
	}

	[DefaultValue(eLabelPartAlignment.MiddleCenter)]
	[Description("Indicates text alignment.")]
	public eLabelPartAlignment TextAlign
	{
		get
		{
			return __25AE;
		}
		set
		{
			if (__25AE != value)
			{
				__25AE = value;
				Refresh();
			}
		}
	}

	[DefaultValue(eLabelPartAlignment.MiddleRight)]
	[Description("Indicates image alignment.")]
	public eLabelPartAlignment ImageAlign
	{
		get
		{
			return __25AF;
		}
		set
		{
			if (__25AF != value)
			{
				__25AF = value;
				Refresh();
			}
		}
	}

	[Description("Indicates image displayed on the label.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (_ነ != value)
			{
				_ነ = value;
				__1379 = Rectangle.Empty;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FlatOffice2007Style
	{
		get
		{
			return __25B0;
		}
		set
		{
			__25B0 = value;
			Refresh();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsToday
	{
		get
		{
			return __25B1;
		}
		set
		{
			if (__25B1 != value)
			{
				__25B1 = value;
				Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether popup is displayed when mouse is pressed anywhere over the item.")]
	public bool ExpandOnMouseDown
	{
		get
		{
			return __25B2;
		}
		set
		{
			if (__25B2 != value)
			{
				__25B2 = value;
			}
		}
	}

	public event DayPaintEventHandler PaintLabel
	{
		add
		{
			DayPaintEventHandler dayPaintEventHandler = _2398;
			DayPaintEventHandler dayPaintEventHandler2;
			do
			{
				dayPaintEventHandler2 = dayPaintEventHandler;
				DayPaintEventHandler value2 = (DayPaintEventHandler)Delegate.Combine(dayPaintEventHandler2, value);
				dayPaintEventHandler = Interlocked.CompareExchange(ref _2398, value2, dayPaintEventHandler2);
			}
			while ((object)dayPaintEventHandler != dayPaintEventHandler2);
		}
		remove
		{
			DayPaintEventHandler dayPaintEventHandler = _2398;
			DayPaintEventHandler dayPaintEventHandler2;
			do
			{
				dayPaintEventHandler2 = dayPaintEventHandler;
				DayPaintEventHandler value2 = (DayPaintEventHandler)Delegate.Remove(dayPaintEventHandler2, value);
				dayPaintEventHandler = Interlocked.CompareExchange(ref _2398, value2, dayPaintEventHandler2);
			}
			while ((object)dayPaintEventHandler != dayPaintEventHandler2);
		}
	}

	public DayLabel()
	{
		_ᠩ.StyleChanged += ᅻ;
		MouseUpNotification = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _ነ);
		}
		_ᠩ.StyleChanged -= ᅻ;
		_ᠩ.Dispose();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		DayPaintEventArgs dayPaintEventArgs = new DayPaintEventArgs(p, this);
		OnPaintLabel(dayPaintEventArgs);
		if (dayPaintEventArgs.RenderParts == eDayPaintParts.None)
		{
			return;
		}
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2601(this, dayPaintEventArgs);
		}
		if (dayPaintEventArgs.RenderParts != 0)
		{
			if (Enabled && (dayPaintEventArgs.RenderParts & eDayPaintParts.Background) == eDayPaintParts.Background)
			{
				ᯓ(p);
			}
			if ((dayPaintEventArgs.RenderParts & eDayPaintParts.Text) == eDayPaintParts.Text)
			{
				_2422(p, null, Color.Empty, __25AE);
			}
			if ((dayPaintEventArgs.RenderParts & eDayPaintParts.Image) == eDayPaintParts.Image)
			{
				_25B3(p, _ነ, __25AF);
			}
		}
	}

	internal void _25B3(ItemPaintArgs _065C, Image ແ, eLabelPartAlignment _25B4)
	{
		if (ແ != null)
		{
			Graphics graphics = _065C.Graphics;
			Rectangle भ = _25B5(DisplayRectangle, ແ.Size, _25B4);
			ᇎ ᇎ = new ᇎ(ແ, dispose: false);
			ᇎ.ᇗ(graphics, भ);
			ᇎ.Dispose();
			__1379 = भ;
		}
	}

	private Rectangle _25B5(Rectangle ശ, Size _25B6, eLabelPartAlignment _25B7)
	{
		Rectangle result = new Rectangle(ശ.Right - _25B6.Width, ശ.Y, _25B6.Width, _25B6.Height);
		switch (_25B7)
		{
		case eLabelPartAlignment.BottomCenter:
			result.Location = new Point(ശ.X + (ശ.Width - _25B6.Width) / 2, ശ.Bottom - _25B6.Height);
			break;
		case eLabelPartAlignment.BottomLeft:
			result.Location = new Point(ശ.X, ശ.Bottom - _25B6.Height);
			break;
		case eLabelPartAlignment.BottomRight:
			result.Location = new Point(ശ.Right - _25B6.Width, ശ.Bottom - _25B6.Height);
			break;
		case eLabelPartAlignment.MiddleCenter:
			result.Location = new Point(ശ.X + (ശ.Width - _25B6.Width) / 2, ശ.Y + (ശ.Height - _25B6.Height) / 2);
			break;
		case eLabelPartAlignment.MiddleLeft:
			result.Location = new Point(ശ.X, ശ.Y + (ശ.Height - _25B6.Height) / 2);
			break;
		case eLabelPartAlignment.MiddleRight:
			result.Location = new Point(ശ.Right - _25B6.Width, ശ.Y + (ശ.Height - _25B6.Height) / 2);
			break;
		case eLabelPartAlignment.TopCenter:
			result.Location = new Point(ശ.X + (ശ.Width - _25B6.Width) / 2, ശ.Y);
			break;
		case eLabelPartAlignment.TopLeft:
			result.Location = new Point(ശ.X, ശ.Y);
			break;
		}
		return result;
	}

	protected virtual void OnPaintLabel(DayPaintEventArgs e)
	{
		if (_2398 != null)
		{
			_2398(this, e);
		}
	}

	internal void ᯓ(ItemPaintArgs _065C)
	{
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = _065C.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		Color color3 = Color.Empty;
		if (_ᠩ.Custom && ((!IsMouseOver && !IsMouseDown) || !__25AC))
		{
			_ᠩ.SetColorScheme(_065C.Colors);
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(elementStyle, graphics, displayRectangle));
			if (ല)
			{
				elementStyle.Dispose();
			}
		}
		bool flag = false;
		SingleMonthCalendar singleMonthCalendar = Parent as SingleMonthCalendar;
		MonthCalendarColors monthCalendarColors = null;
		if (singleMonthCalendar != null)
		{
			monthCalendarColors = singleMonthCalendar._2517();
		}
		if (IsMouseDown && __25AC)
		{
			color = _065C.Colors.ItemPressedBackground;
			color2 = _065C.Colors.ItemPressedBackground2;
			color3 = _065C.Colors.ItemPressedBorder;
		}
		else if (IsMouseOver && __25AC)
		{
			color = _065C.Colors.ItemHotBackground;
			color2 = _065C.Colors.ItemHotBackground2;
			color3 = _065C.Colors.ItemHotBorder;
		}
		else if (IsSelected)
		{
			color = _065C.Colors.ItemCheckedBackground;
			if (monthCalendarColors != null && monthCalendarColors.Selection.IsCustomized)
			{
				if (!monthCalendarColors.Selection.BackColor.IsEmpty)
				{
					color = monthCalendarColors.Selection.BackColor;
					if (!monthCalendarColors.Selection.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.Selection.BackColor2;
					}
				}
				if (!monthCalendarColors.Selection.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.Selection.BorderColor;
				}
				flag = true;
			}
		}
		else if (__25B1)
		{
			color3 = _065C.Colors.ItemCheckedBorder;
			if (monthCalendarColors != null && monthCalendarColors.Today.IsCustomized)
			{
				if (!monthCalendarColors.Today.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.Today.BorderColor;
				}
				if (!monthCalendarColors.Today.BackColor.IsEmpty)
				{
					color = monthCalendarColors.Today.BackColor;
					if (!monthCalendarColors.Today.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.Today.BackColor2;
					}
				}
				flag = true;
			}
		}
		else if (IsWeekOfYear)
		{
			color = Color.LightGray;
			if (monthCalendarColors != null && monthCalendarColors.WeekOfYear.IsCustomized)
			{
				if (!monthCalendarColors.WeekOfYear.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.WeekOfYear.BorderColor;
				}
				if (!monthCalendarColors.WeekOfYear.BackColor.IsEmpty)
				{
					color = monthCalendarColors.WeekOfYear.BackColor;
					if (!monthCalendarColors.WeekOfYear.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.WeekOfYear.BackColor2;
					}
				}
				flag = true;
			}
		}
		else if (IsTrailing)
		{
			if (monthCalendarColors != null && monthCalendarColors.TrailingDay.IsCustomized)
			{
				if (!monthCalendarColors.TrailingDay.BackColor.IsEmpty)
				{
					color = monthCalendarColors.TrailingDay.BackColor;
					if (!monthCalendarColors.TrailingDay.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.TrailingDay.BackColor2;
					}
				}
				if (!monthCalendarColors.TrailingDay.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.TrailingDay.BorderColor;
				}
				flag = true;
			}
			if (_25B8(Date) && monthCalendarColors != null && monthCalendarColors.TrailingWeekend.IsCustomized)
			{
				if (!monthCalendarColors.TrailingWeekend.BackColor.IsEmpty)
				{
					color = monthCalendarColors.TrailingWeekend.BackColor;
					if (!monthCalendarColors.TrailingWeekend.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.TrailingWeekend.BackColor2;
					}
				}
				if (!monthCalendarColors.TrailingWeekend.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.TrailingWeekend.BorderColor;
				}
				flag = true;
			}
		}
		else if (IsDayLabel)
		{
			if (monthCalendarColors != null && monthCalendarColors.DayLabel.IsCustomized)
			{
				if (!monthCalendarColors.DayLabel.BackColor.IsEmpty)
				{
					color = monthCalendarColors.DayLabel.BackColor;
					if (!monthCalendarColors.DayLabel.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.DayLabel.BackColor2;
					}
				}
				if (!monthCalendarColors.DayLabel.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.DayLabel.BorderColor;
				}
				flag = true;
			}
		}
		else if (Date != DateTime.MinValue)
		{
			if (monthCalendarColors != null && monthCalendarColors.Day.IsCustomized)
			{
				if (!monthCalendarColors.Day.BackColor.IsEmpty)
				{
					color = monthCalendarColors.Day.BackColor;
					if (!monthCalendarColors.Day.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.Day.BackColor2;
					}
				}
				if (!monthCalendarColors.Day.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.Day.BorderColor;
				}
				flag = true;
			}
			if (_25B8(Date) && monthCalendarColors != null && monthCalendarColors.Weekend.IsCustomized)
			{
				if (!monthCalendarColors.Weekend.BackColor.IsEmpty)
				{
					color = monthCalendarColors.Weekend.BackColor;
					if (!monthCalendarColors.Weekend.BackColor2.IsEmpty)
					{
						color2 = monthCalendarColors.Weekend.BackColor2;
					}
				}
				if (!monthCalendarColors.Weekend.BorderColor.IsEmpty)
				{
					color3 = monthCalendarColors.Weekend.BorderColor;
				}
				flag = true;
			}
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !FlatOffice2007Style && !flag)
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable = GetOffice2007StateColorTable(_065C);
			if (office2007StateColorTable != null)
			{
				ᯊ.ᯓ(graphics, office2007StateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RectangleShape);
				color = Color.Empty;
				color2 = Color.Empty;
				color3 = Color.Empty;
			}
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		if (!color.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, displayRectangle, color, color2);
		}
		if (!color3.IsEmpty)
		{
			DisplayHelp.DrawRectangle(graphics, color3, displayRectangle);
		}
		if (IsDayLabel)
		{
			color3 = _065C.Colors.BarDockedBorder;
			if (monthCalendarColors != null && !monthCalendarColors.DaysDividerBorderColors.IsEmpty)
			{
				color3 = monthCalendarColors.DaysDividerBorderColors;
			}
			if (!color3.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, displayRectangle.X, displayRectangle.Bottom - 1, displayRectangle.Right, displayRectangle.Bottom - 1, color3, 1);
			}
		}
		graphics.SmoothingMode = smoothingMode;
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(ItemPaintArgs p)
	{
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !FlatOffice2007Style && p.ໝ is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)p.ໝ).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (!Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (IsMouseDown && __25AC)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (IsMouseOver && __25AC)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			if (IsSelected)
			{
				return office2007ButtonItemColorTable.Checked;
			}
		}
		return null;
	}

	internal static bool _25B8(DateTime _0CC8)
	{
		if (_0CC8.DayOfWeek != DayOfWeek.Saturday)
		{
			return _0CC8.DayOfWeek == DayOfWeek.Sunday;
		}
		return true;
	}

	internal void _2422(ItemPaintArgs _065C, Font _25B9, Color ໆ, eLabelPartAlignment _25BA)
	{
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		Graphics graphics = _065C.Graphics;
		string text = Text;
		if (__25A8 != DateTime.MinValue)
		{
			text = __25A8.Day.ToString();
		}
		bool isBold = __25AD;
		if (ໆ.IsEmpty)
		{
			if (!__0CD1.IsEmpty)
			{
				ໆ = __0CD1;
			}
			else if (!Enabled)
			{
				ໆ = _065C.Colors.ItemDisabledText;
			}
			else
			{
				ໆ = (__25AB ? _065C.Colors.ItemDisabledText : _065C.Colors.ItemText);
				SingleMonthCalendar singleMonthCalendar = Parent as SingleMonthCalendar;
				MonthCalendarColors monthCalendarColors = null;
				if (singleMonthCalendar != null)
				{
					monthCalendarColors = singleMonthCalendar._2517();
				}
				if (monthCalendarColors != null)
				{
					if (__25A8 != DateTime.MinValue)
					{
						if (_ԅ && monthCalendarColors.Selection.IsCustomized)
						{
							if (!monthCalendarColors.Selection.TextColor.IsEmpty)
							{
								ໆ = monthCalendarColors.Selection.TextColor;
							}
							if (monthCalendarColors.Selection.IsBold)
							{
								isBold = monthCalendarColors.Selection.IsBold;
							}
						}
						else
						{
							if (__25AB)
							{
								if (!monthCalendarColors.TrailingDay.TextColor.IsEmpty)
								{
									ໆ = monthCalendarColors.TrailingDay.TextColor;
								}
								if (monthCalendarColors.TrailingDay.IsBold)
								{
									isBold = monthCalendarColors.TrailingDay.IsBold;
								}
							}
							else if (monthCalendarColors.Day.IsCustomized)
							{
								if (!monthCalendarColors.Day.TextColor.IsEmpty)
								{
									ໆ = monthCalendarColors.Day.TextColor;
								}
								if (monthCalendarColors.Day.IsBold)
								{
									isBold = monthCalendarColors.Day.IsBold;
								}
							}
							if (_25B8(__25A8))
							{
								if (__25AB)
								{
									if (!monthCalendarColors.TrailingWeekend.TextColor.IsEmpty)
									{
										ໆ = monthCalendarColors.TrailingWeekend.TextColor;
									}
									if (monthCalendarColors.TrailingWeekend.IsBold)
									{
										isBold = monthCalendarColors.TrailingWeekend.IsBold;
									}
								}
								else
								{
									if (!monthCalendarColors.Weekend.TextColor.IsEmpty)
									{
										ໆ = monthCalendarColors.Weekend.TextColor;
									}
									if (monthCalendarColors.Weekend.IsBold)
									{
										isBold = monthCalendarColors.Weekend.IsBold;
									}
								}
							}
						}
					}
					else if (IsWeekOfYear)
					{
						if (monthCalendarColors.WeekOfYear.IsCustomized)
						{
							if (!monthCalendarColors.WeekOfYear.TextColor.IsEmpty)
							{
								ໆ = monthCalendarColors.WeekOfYear.TextColor;
							}
							if (monthCalendarColors.WeekOfYear.IsBold)
							{
								isBold = monthCalendarColors.WeekOfYear.IsBold;
							}
						}
					}
					else if (IsDayLabel)
					{
						if (!monthCalendarColors.DayLabel.TextColor.IsEmpty)
						{
							ໆ = monthCalendarColors.DayLabel.TextColor;
						}
						if (monthCalendarColors.DayLabel.IsBold)
						{
							isBold = monthCalendarColors.DayLabel.IsBold;
						}
					}
				}
			}
		}
		if (_ᆿ && TrackMouse && StyleManager.IsMetro(EffectiveStyle))
		{
			ໆ = _065C.Colors.ItemHotText;
		}
		bool flag = false;
		if (_25B9 == null)
		{
			if (isBold)
			{
				_25B9 = new Font(_065C.Font, (FontStyle)1);
				flag = true;
			}
			else
			{
				_25B9 = _065C.Font;
			}
		}
		if (__25A8 != DateTime.MinValue)
		{
			Size _25B6 = TextDrawing.MeasureString(graphics, "32", _25B9);
			Rectangle bounds = _25B5(DisplayRectangle, _25B6, _25BA);
			eTextFormat format = eTextFormat.Right | eTextFormat.VerticalCenter;
			TextDrawing.DrawString(graphics, text, _25B9, ໆ, bounds, format);
		}
		else
		{
			eTextFormat format2 = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
			switch (_25BA)
			{
			case eLabelPartAlignment.BottomCenter:
				format2 = eTextFormat.Bottom | eTextFormat.HorizontalCenter;
				break;
			case eLabelPartAlignment.BottomLeft:
				format2 = eTextFormat.Bottom;
				break;
			case eLabelPartAlignment.BottomRight:
				format2 = eTextFormat.Bottom | eTextFormat.Right;
				break;
			case eLabelPartAlignment.MiddleLeft:
				format2 = eTextFormat.VerticalCenter;
				break;
			case eLabelPartAlignment.MiddleRight:
				format2 = eTextFormat.Right | eTextFormat.VerticalCenter;
				break;
			case eLabelPartAlignment.TopCenter:
				format2 = eTextFormat.VerticalCenter;
				break;
			case eLabelPartAlignment.TopLeft:
				format2 = eTextFormat.Default;
				break;
			case eLabelPartAlignment.TopRight:
				format2 = eTextFormat.Right;
				break;
			}
			TextDrawing.DrawString(graphics, text, _25B9, ໆ, Bounds, format2);
		}
		if (flag)
		{
			_25B9.Dispose();
		}
	}

	public override void RecalcSize()
	{
		Bounds = new Rectangle(Bounds.Location, SingleMonthCalendar.__25C5);
		base.RecalcSize();
	}

	public override BaseItem Copy()
	{
		DayLabel dayLabel = new DayLabel();
		CopyToItem(dayLabel);
		return dayLabel;
	}

	protected override void CopyToItem(BaseItem c)
	{
		DayLabel copy = c as DayLabel;
		base.CopyToItem((BaseItem)copy);
	}

	public override void InternalMouseEnter()
	{
		IsMouseOver = true;
		base.InternalMouseEnter();
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2606(this, new EventArgs());
		}
	}

	public override void InternalMouseLeave()
	{
		IsMouseOver = false;
		base.InternalMouseLeave();
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2607(this, new EventArgs());
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			IsMouseDown = true;
		}
		if (IsMouseDown && SubItems.Count > 0 && ShowSubItems && (__1379.Contains(objArg.X, objArg.Y) || __25B2))
		{
			Expanded = !Expanded;
		}
		base.InternalMouseDown(objArg);
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2604(this, objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			IsMouseDown = false;
		}
		base.InternalMouseUp(objArg);
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2605(this, objArg);
		}
	}

	protected override void OnClick()
	{
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2603(this);
		}
		base.OnClick();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2608(this, objArg);
		}
	}

	public override void InternalMouseHover()
	{
		base.InternalMouseHover();
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar._2609(this, new EventArgs());
		}
	}

	private void ᅻ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}
}
