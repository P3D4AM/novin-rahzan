using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class DayLabel : PopupItem
{
	private ElementStyle _BackgroundStyle = new ElementStyle();

	private Rectangle _ImageRenderBounds = Rectangle.Empty;

	private DateTime _Date = DateTime.MinValue;

	private bool _IsDayLabel;

	private bool _IsWeekOfYear;

	private bool _IsTrailing;

	private bool _IsMouseOver;

	private bool _IsMouseDown;

	private bool _IsSelected;

	private bool _TrackMouse = true;

	private bool _Selectable = true;

	private Color _TextColor = Color.Empty;

	private bool _IsBold;

	private eLabelPartAlignment _TextAlign = eLabelPartAlignment.MiddleCenter;

	private eLabelPartAlignment _ImageAlign = eLabelPartAlignment.MiddleRight;

	private Image _Image;

	private bool _FlatOffice2007Style;

	private bool _IsToday;

	private bool _ExpandOnMouseDown;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime Date
	{
		get
		{
			return _Date;
		}
		set
		{
			if (_Date != value)
			{
				_Date = value;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsDayLabel
	{
		get
		{
			return _IsDayLabel;
		}
		set
		{
			if (_IsDayLabel != value)
			{
				_IsDayLabel = value;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsWeekOfYear
	{
		get
		{
			return _IsWeekOfYear;
		}
		set
		{
			if (_IsWeekOfYear != value)
			{
				_IsWeekOfYear = value;
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
			return _IsTrailing;
		}
		set
		{
			if (_IsTrailing != value)
			{
				_IsTrailing = value;
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
			return _IsMouseOver;
		}
		set
		{
			_IsMouseOver = value;
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseDown
	{
		get
		{
			return _IsMouseDown;
		}
		set
		{
			_IsMouseDown = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		set
		{
			if (_IsSelected != value)
			{
				_IsSelected = value;
				Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether label provides visual indicator when mouse is over the label or pressed while over the label.")]
	public bool TrackMouse
	{
		get
		{
			return _TrackMouse;
		}
		set
		{
			if (_TrackMouse != value)
			{
				_TrackMouse = value;
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
			return _Selectable;
		}
		set
		{
			if (_Selectable != value)
			{
				_Selectable = value;
				Refresh();
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates label text color.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether text is drawn using Bold font.")]
	public bool IsBold
	{
		get
		{
			return _IsBold;
		}
		set
		{
			if (_IsBold != value)
			{
				_IsBold = value;
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
			return _TextAlign;
		}
		set
		{
			if (_TextAlign != value)
			{
				_TextAlign = value;
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
			return _ImageAlign;
		}
		set
		{
			if (_ImageAlign != value)
			{
				_ImageAlign = value;
				Refresh();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates image displayed on the label.")]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (_Image != value)
			{
				_Image = value;
				_ImageRenderBounds = Rectangle.Empty;
				Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool FlatOffice2007Style
	{
		get
		{
			return _FlatOffice2007Style;
		}
		set
		{
			_FlatOffice2007Style = value;
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsToday
	{
		get
		{
			return _IsToday;
		}
		set
		{
			if (_IsToday != value)
			{
				_IsToday = value;
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
			return _ExpandOnMouseDown;
		}
		set
		{
			if (_ExpandOnMouseDown != value)
			{
				_ExpandOnMouseDown = value;
			}
		}
	}

	public event DayPaintEventHandler PaintLabel;

	public DayLabel()
	{
		_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
		MouseUpNotification = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref _Image);
		}
		_BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
		_BackgroundStyle.Dispose();
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
			singleMonthCalendar.OnPaintLabel(this, dayPaintEventArgs);
		}
		if (dayPaintEventArgs.RenderParts != 0)
		{
			if (Enabled && (dayPaintEventArgs.RenderParts & eDayPaintParts.Background) == eDayPaintParts.Background)
			{
				PaintBackground(p);
			}
			if ((dayPaintEventArgs.RenderParts & eDayPaintParts.Text) == eDayPaintParts.Text)
			{
				PaintText(p, null, Color.Empty, _TextAlign);
			}
			if ((dayPaintEventArgs.RenderParts & eDayPaintParts.Image) == eDayPaintParts.Image)
			{
				PaintImage(p, _Image, _ImageAlign);
			}
		}
	}

	internal void PaintImage(ItemPaintArgs p, Image image, eLabelPartAlignment imageAlign)
	{
		if (image != null)
		{
			Graphics graphics = p.Graphics;
			Rectangle alignedRect = GetAlignedRect(DisplayRectangle, image.Size, imageAlign);
			CompositeImage compositeImage = new CompositeImage(image, dispose: false);
			compositeImage.DrawImage(graphics, alignedRect);
			compositeImage.Dispose();
			_ImageRenderBounds = alignedRect;
		}
	}

	private Rectangle GetAlignedRect(Rectangle bounds, Size innerSize, eLabelPartAlignment partAlign)
	{
		Rectangle result = new Rectangle(bounds.Right - innerSize.Width, bounds.Y, innerSize.Width, innerSize.Height);
		switch (partAlign)
		{
		case eLabelPartAlignment.BottomCenter:
			result.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Bottom - innerSize.Height);
			break;
		case eLabelPartAlignment.BottomLeft:
			result.Location = new Point(bounds.X, bounds.Bottom - innerSize.Height);
			break;
		case eLabelPartAlignment.BottomRight:
			result.Location = new Point(bounds.Right - innerSize.Width, bounds.Bottom - innerSize.Height);
			break;
		case eLabelPartAlignment.MiddleCenter:
			result.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Y + (bounds.Height - innerSize.Height) / 2);
			break;
		case eLabelPartAlignment.MiddleLeft:
			result.Location = new Point(bounds.X, bounds.Y + (bounds.Height - innerSize.Height) / 2);
			break;
		case eLabelPartAlignment.MiddleRight:
			result.Location = new Point(bounds.Right - innerSize.Width, bounds.Y + (bounds.Height - innerSize.Height) / 2);
			break;
		case eLabelPartAlignment.TopCenter:
			result.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Y);
			break;
		case eLabelPartAlignment.TopLeft:
			result.Location = new Point(bounds.X, bounds.Y);
			break;
		}
		return result;
	}

	protected virtual void OnPaintLabel(DayPaintEventArgs e)
	{
		if (this.PaintLabel != null)
		{
			this.PaintLabel(this, e);
		}
	}

	internal void PaintBackground(ItemPaintArgs p)
	{
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = DisplayRectangle;
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		Color color3 = Color.Empty;
		if (_BackgroundStyle.Custom && ((!IsMouseOver && !IsMouseDown) || !_TrackMouse))
		{
			_BackgroundStyle.SetColorScheme(p.Colors);
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out dispose);
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(elementStyle, graphics, displayRectangle));
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
		bool flag = false;
		SingleMonthCalendar singleMonthCalendar = Parent as SingleMonthCalendar;
		MonthCalendarColors monthCalendarColors = null;
		if (singleMonthCalendar != null)
		{
			monthCalendarColors = singleMonthCalendar.GetColors();
		}
		if (IsMouseDown && _TrackMouse)
		{
			color = p.Colors.ItemPressedBackground;
			color2 = p.Colors.ItemPressedBackground2;
			color3 = p.Colors.ItemPressedBorder;
		}
		else if (IsMouseOver && _TrackMouse)
		{
			color = p.Colors.ItemHotBackground;
			color2 = p.Colors.ItemHotBackground2;
			color3 = p.Colors.ItemHotBorder;
		}
		else if (IsSelected)
		{
			color = p.Colors.ItemCheckedBackground;
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
		else if (_IsToday)
		{
			color3 = p.Colors.ItemCheckedBorder;
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
			if (IsWeekend(Date) && monthCalendarColors != null && monthCalendarColors.TrailingWeekend.IsCustomized)
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
			if (IsWeekend(Date) && monthCalendarColors != null && monthCalendarColors.Weekend.IsCustomized)
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
			Office2007ButtonItemStateColorTable office2007StateColorTable = GetOffice2007StateColorTable(p);
			if (office2007StateColorTable != null)
			{
				Office2007ButtonItemPainter.PaintBackground(graphics, office2007StateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RectangleShape);
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
			color3 = p.Colors.BarDockedBorder;
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
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !FlatOffice2007Style && p.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)p.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (!Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (IsMouseDown && _TrackMouse)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (IsMouseOver && _TrackMouse)
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

	internal static bool IsWeekend(DateTime d)
	{
		if (d.DayOfWeek != DayOfWeek.Saturday)
		{
			return d.DayOfWeek == DayOfWeek.Sunday;
		}
		return true;
	}

	internal void PaintText(ItemPaintArgs p, Font textFont, Color textColor, eLabelPartAlignment textAlign)
	{
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		string text = Text;
		if (_Date != DateTime.MinValue)
		{
			text = _Date.Day.ToString();
		}
		bool isBold = _IsBold;
		if (textColor.IsEmpty)
		{
			if (!_TextColor.IsEmpty)
			{
				textColor = _TextColor;
			}
			else if (!Enabled)
			{
				textColor = p.Colors.ItemDisabledText;
			}
			else
			{
				textColor = (_IsTrailing ? p.Colors.ItemDisabledText : p.Colors.ItemText);
				SingleMonthCalendar singleMonthCalendar = Parent as SingleMonthCalendar;
				MonthCalendarColors monthCalendarColors = null;
				if (singleMonthCalendar != null)
				{
					monthCalendarColors = singleMonthCalendar.GetColors();
				}
				if (monthCalendarColors != null)
				{
					if (_Date != DateTime.MinValue)
					{
						if (_IsSelected && monthCalendarColors.Selection.IsCustomized)
						{
							if (!monthCalendarColors.Selection.TextColor.IsEmpty)
							{
								textColor = monthCalendarColors.Selection.TextColor;
							}
							if (monthCalendarColors.Selection.IsBold)
							{
								isBold = monthCalendarColors.Selection.IsBold;
							}
						}
						else
						{
							if (_IsTrailing)
							{
								if (!monthCalendarColors.TrailingDay.TextColor.IsEmpty)
								{
									textColor = monthCalendarColors.TrailingDay.TextColor;
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
									textColor = monthCalendarColors.Day.TextColor;
								}
								if (monthCalendarColors.Day.IsBold)
								{
									isBold = monthCalendarColors.Day.IsBold;
								}
							}
							if (IsWeekend(_Date))
							{
								if (_IsTrailing)
								{
									if (!monthCalendarColors.TrailingWeekend.TextColor.IsEmpty)
									{
										textColor = monthCalendarColors.TrailingWeekend.TextColor;
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
										textColor = monthCalendarColors.Weekend.TextColor;
									}
									if (monthCalendarColors.Weekend.IsBold)
									{
										isBold = monthCalendarColors.TrailingWeekend.IsBold;
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
								textColor = monthCalendarColors.WeekOfYear.TextColor;
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
							textColor = monthCalendarColors.DayLabel.TextColor;
						}
						if (monthCalendarColors.DayLabel.IsBold)
						{
							isBold = monthCalendarColors.DayLabel.IsBold;
						}
					}
				}
			}
		}
		if (_IsMouseOver && TrackMouse && EffectiveStyle == eDotNetBarStyle.Metro)
		{
			textColor = p.Colors.ItemHotText;
		}
		bool flag = false;
		if (textFont == null)
		{
			if (isBold)
			{
				textFont = new Font(p.Font, (FontStyle)1);
				flag = true;
			}
			else
			{
				textFont = p.Font;
			}
		}
		if (_Date != DateTime.MinValue)
		{
			Size innerSize = TextDrawing.MeasureString(graphics, "32", textFont);
			Rectangle alignedRect = GetAlignedRect(DisplayRectangle, innerSize, textAlign);
			eTextFormat format = eTextFormat.Right | eTextFormat.VerticalCenter;
			TextDrawing.DrawString(graphics, text, textFont, textColor, alignedRect, format);
		}
		else
		{
			eTextFormat format2 = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
			switch (textAlign)
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
			TextDrawing.DrawString(graphics, text, textFont, textColor, Bounds, format2);
		}
		if (flag)
		{
			textFont.Dispose();
		}
	}

	public override void RecalcSize()
	{
		Bounds = new Rectangle(Bounds.Location, SingleMonthCalendar._DefaultDaySize);
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
			singleMonthCalendar.OnLabelMouseEnter(this, new EventArgs());
		}
	}

	public override void InternalMouseLeave()
	{
		IsMouseOver = false;
		base.InternalMouseLeave();
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar.OnLabelMouseLeave(this, new EventArgs());
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
		if (IsMouseDown && SubItems.Count > 0 && ShowSubItems && (_ImageRenderBounds.Contains(objArg.X, objArg.Y) || _ExpandOnMouseDown))
		{
			Expanded = !Expanded;
		}
		base.InternalMouseDown(objArg);
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar.OnLabelMouseDown(this, objArg);
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
			singleMonthCalendar.OnLabelMouseUp(this, objArg);
		}
	}

	protected override void OnClick()
	{
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar.DayLabelClick(this);
		}
		base.OnClick();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar.OnLabelMouseMove(this, objArg);
		}
	}

	public override void InternalMouseHover()
	{
		base.InternalMouseHover();
		if (Parent is SingleMonthCalendar singleMonthCalendar)
		{
			singleMonthCalendar.OnLabelMouseHover(this, new EventArgs());
		}
	}

	private void BackgroundStyleChanged(object sender, EventArgs e)
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
