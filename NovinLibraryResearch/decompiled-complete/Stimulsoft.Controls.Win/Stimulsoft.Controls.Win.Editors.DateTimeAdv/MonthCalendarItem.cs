using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Events;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class MonthCalendarItem : CalendarBase
{
	private Size _Spacing = new Size(10, 8);

	private List<DateTime> _AnnuallyMarkedDates = new List<DateTime>();

	private List<DateTime> _MonthlyMarkedDates = new List<DateTime>();

	private List<DateTime> _MarkedDates = new List<DateTime>();

	private static string _BottomContainerName = "sysCalBottomContainer";

	private static string _TodayButtonName = "sysTodayButton";

	private static string _ClearButtonName = "sysClearSelectionButton";

	private ItemContainer _BottomContainer;

	private ButtonItem _TodayButton;

	private ButtonItem _ClearButton;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseMove;

	[CompilerGenerated]
	private DateRangeEventHandler m_DateSelected;

	private int _MaxSelectionCount = 7;

	private DateTime _SelectionStart = DateTime.MinValue;

	private DateTime _SelectionEnd = DateTime.MinValue;

	private bool _MultiSelect;

	private Point _MouseDownLocation = Point.Empty;

	private DateTime _MouseDownDate = DateTime.MinValue;

	private List<DayOfWeek> _WeeklyMarkedDays = new List<DayOfWeek>();

	private DateTime _DisplayMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

	private MonthCalendarColors _Colors = new MonthCalendarColors();

	private Size _CalendarDimensions = Size.Empty;

	private bool _UpdatingSelectedDate;

	private DateTime _SelectedDate = DateTime.MinValue;

	private bool _TodayButtonVisible;

	private bool _ClearButtonVisible;

	private bool _DayClickAutoClosePopup = true;

	private bool _YearSelectionEnabled = true;

	[DefaultValue(7)]
	[Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
	public int MaxSelectionCount
	{
		get
		{
			return _MaxSelectionCount;
		}
		set
		{
			_MaxSelectionCount = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionStart
	{
		get
		{
			return _SelectionStart;
		}
		set
		{
			if (_SelectionStart != value)
			{
				SetSelectionRange(value, (SelectionEnd == DateTime.MinValue) ? value : SelectionEnd);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionEnd
	{
		get
		{
			return _SelectionEnd;
		}
		set
		{
			if (_SelectionEnd != value)
			{
				SetSelectionRange((SelectionStart == DateTime.MinValue) ? value : SelectionStart, value);
			}
		}
	}

	[Bindable(true)]
	[Description("Indicates selected range of dates for a month calendar control.")]
	public SelectionRange SelectionRange
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			return new SelectionRange(SelectionStart, SelectionEnd);
		}
		set
		{
			SetSelectionRange(value.Start, value.End);
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
	public bool MultiSelect
	{
		get
		{
			return _MultiSelect;
		}
		set
		{
			if (_MultiSelect != value)
			{
				_MultiSelect = value;
				OnMultiSelectChanged();
			}
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	public DateTime[] MonthlyMarkedDates
	{
		get
		{
			return _MonthlyMarkedDates.ToArray();
		}
		set
		{
			OnMonthlyMarkedDatesChanged(value);
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DayOfWeek members that determine which days of week to mark using Colors.WeeklyMarker settings. ")]
	public DayOfWeek[] WeeklyMarkedDays
	{
		get
		{
			return _WeeklyMarkedDays.ToArray();
		}
		set
		{
			OnWeeklyMarkedDaysChanged(value);
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
	public DateTime[] AnnuallyMarkedDates
	{
		get
		{
			return _AnnuallyMarkedDates.ToArray();
		}
		set
		{
			OnAnnuallyMarkedDatesChanged(value);
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which nonrecurring dates are marked using Colors.DayMarker settings.")]
	public DateTime[] MarkedDates
	{
		get
		{
			return _MarkedDates.ToArray();
		}
		set
		{
			OnMarkedDatesChanged(value);
		}
	}

	[Description("Indicates the first month displayed on the control.")]
	public DateTime DisplayMonth
	{
		get
		{
			return _DisplayMonth;
		}
		set
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			_DisplayMonth = value.Date;
			m_NeedRecalcSize = true;
			if (ContainerControl is Control)
			{
				BarFunctions.InvokeRecalcLayout((Control)ContainerControl, invalidate: true);
			}
			ApplySelection();
			Refresh();
		}
	}

	[Browsable(false)]
	public int DisplayedMonthCount
	{
		get
		{
			int num = 0;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar)
				{
					num++;
				}
			}
			return num;
		}
	}

	[NotifyParentProperty(true)]
	[Category("Appearance")]
	[Description("Gets colors used by control.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public MonthCalendarColors Colors => _Colors;

	private SingleMonthCalendar FirstCalendar => (SingleMonthCalendar)SubItems[0];

	[Description("Indicate size of each day item on the calendar.")]
	public Size DaySize
	{
		get
		{
			return FirstCalendar.DaySize;
		}
		set
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.DaySize = value;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return FirstCalendar.MinDate;
		}
		set
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.MinDate = value;
				}
			}
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return FirstCalendar.MaxDate;
		}
		set
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.MaxDate = value;
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle CommandsBackgroundStyle => _BottomContainer.BackgroundStyle;

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates navigation container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle NavigationBackgroundStyle => FirstCalendar.NavigationBackgroundStyle;

	[Description("Indicates number of columns and rows of months displayed on control.")]
	public Size CalendarDimensions
	{
		get
		{
			return _CalendarDimensions;
		}
		set
		{
			if (value.Width < 0 || value.Height < 0)
			{
				throw new ArgumentOutOfRangeException("Calendar dimension values must be greater or equal to zero.");
			}
			if ((value.Width == 0 && value.Height > 0) || (value.Width > 0 && value.Height == 0))
			{
				throw new ArgumentOutOfRangeException("Calendar dimension values must be both set to values grater than zero.");
			}
			if (_CalendarDimensions != value)
			{
				_CalendarDimensions = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether weekend days can be selected.")]
	public bool WeekendDaysSelectable
	{
		get
		{
			return FirstCalendar.WeekendDaysSelectable;
		}
		set
		{
			if (WeekendDaysSelectable == value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.WeekendDaysSelectable = value;
				}
			}
		}
	}

	[DefaultValue(CalendarWeekRule.FirstDay)]
	[Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
	public CalendarWeekRule WeekOfYearRule
	{
		get
		{
			return FirstCalendar.WeekOfYearRule;
		}
		set
		{
			if (WeekOfYearRule == value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.WeekOfYearRule = value;
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime TodayDate
	{
		get
		{
			return FirstCalendar.TodayDate;
		}
		set
		{
			value = value.Date;
			if (!(TodayDate != value))
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.TodayDate = value;
				}
			}
		}
	}

	[Browsable(false)]
	public bool TodayDateSet => FirstCalendar.TodayDateSet;

	[DefaultValue(true)]
	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	public bool ShowTodayMarker
	{
		get
		{
			return FirstCalendar.ShowTodayMarker;
		}
		set
		{
			if (ShowTodayMarker == value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.ShowTodayMarker = value;
				}
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether week of year is visible.")]
	public bool ShowWeekNumbers
	{
		get
		{
			return FirstCalendar.ShowWeekNumbers;
		}
		set
		{
			if (ShowWeekNumbers != value)
			{
				FirstCalendar.ShowWeekNumbers = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether control uses the two letter day names.")]
	public bool TwoLetterDayName
	{
		get
		{
			return FirstCalendar.TwoLetterDayName;
		}
		set
		{
			FirstCalendar.TwoLetterDayName = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[DefaultValue(null)]
	[Description("Indicates array of custom names for days displayed on calendar header.")]
	[Localizable(true)]
	public string[] DayNames
	{
		get
		{
			return FirstCalendar.DayNames;
		}
		set
		{
			if (value != null && value.Length != 7)
			{
				throw new ArgumentException("DayNames must have exactly 7 items in collection.");
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.DayNames = value;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[DefaultValue(DayOfWeek.Sunday)]
	[Description("Indicates first day of week displayed on the calendar.")]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return FirstCalendar.FirstDayOfWeek;
		}
		set
		{
			if (FirstDayOfWeek != value)
			{
				foreach (BaseItem subItem in SubItems)
				{
					if (subItem is SingleMonthCalendar singleMonthCalendar)
					{
						singleMonthCalendar.FirstDayOfWeek = value;
					}
				}
			}
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDate
	{
		get
		{
			return _SelectedDate;
		}
		set
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			if (!(_SelectedDate != value))
			{
				return;
			}
			_SelectedDate = value;
			try
			{
				_UpdatingSelectedDate = true;
				foreach (BaseItem subItem in SubItems)
				{
					if (subItem is SingleMonthCalendar m)
					{
						UpdateSelectedDates(m);
					}
				}
				OnDateChanged(new EventArgs());
				OnDateSelected(new DateRangeEventArgs(value, value));
			}
			finally
			{
				_UpdatingSelectedDate = false;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ItemContainer BottomContainer => _BottomContainer;

	[Browsable(false)]
	public ButtonItem TodayButton => _TodayButton;

	[DefaultValue(false)]
	[Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
	public bool TodayButtonVisible
	{
		get
		{
			return _TodayButtonVisible;
		}
		set
		{
			if (_TodayButtonVisible != value)
			{
				_TodayButtonVisible = value;
				NeedRecalcSize = true;
				UpdateSystemButtonsVisibility();
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
	public bool ClearButtonVisible
	{
		get
		{
			return _ClearButtonVisible;
		}
		set
		{
			if (_ClearButtonVisible != value)
			{
				_ClearButtonVisible = value;
				NeedRecalcSize = true;
				UpdateSystemButtonsVisibility();
				OnAppearanceChanged();
				Refresh();
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
			if (!value)
			{
				BaseItem.CollapseSubItems(this);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool DayClickAutoClosePopup
	{
		get
		{
			return _DayClickAutoClosePopup;
		}
		set
		{
			if (value != _DayClickAutoClosePopup)
			{
				bool dayClickAutoClosePopup = _DayClickAutoClosePopup;
				_DayClickAutoClosePopup = value;
				OnDayClickAutoClosePopupChanged(dayClickAutoClosePopup, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Year/Century selection is enabled when calendar is displaying single month.")]
	public bool YearSelectionEnabled
	{
		get
		{
			return _YearSelectionEnabled;
		}
		set
		{
			if (_YearSelectionEnabled == value)
			{
				return;
			}
			_YearSelectionEnabled = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.YearSelectionEnabled = _YearSelectionEnabled;
				}
			}
		}
	}

	public event EventHandler MonthChanged;

	public event EventHandler MonthChanging;

	public event DayPaintEventHandler PaintLabel;

	[Description("Occurs when mouse button is pressed over the day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released over day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseUp, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the day/week label inside of the calendar.")]
	public event EventHandler LabelMouseEnter;

	[Description("Occurs when mouse leaves the day/week label inside of the calendar.")]
	public event EventHandler LabelMouseLeave;

	[Description("Occurs when mouse moves over the day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseMove
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseMove, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_LabelMouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LabelMouseMove, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse remains still inside an day/week label of the calendar for an amount of time.")]
	public event EventHandler LabelMouseHover;

	[Description("Occurs when SelectedDate property has changed.")]
	public event EventHandler DateChanged;

	[Description("Occurs when the user makes an explicit date selection using the mouse.")]
	public event DateRangeEventHandler DateSelected
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DateRangeEventHandler val = this.m_DateSelected;
			DateRangeEventHandler val2;
			do
			{
				val2 = val;
				DateRangeEventHandler value2 = (DateRangeEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DateSelected, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DateRangeEventHandler val = this.m_DateSelected;
			DateRangeEventHandler val2;
			do
			{
				val2 = val;
				DateRangeEventHandler value2 = (DateRangeEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DateSelected, value2, val2);
			}
			while (val != val2);
		}
	}

	public MonthCalendarItem()
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		_Colors.Parent = this;
		SingleMonthCalendar singleMonthCalendar = new SingleMonthCalendar();
		singleMonthCalendar.MonthChanging += FirstCalendarMonthChanging;
		singleMonthCalendar.MonthChanged += FirstCalendarMonthChanged;
		singleMonthCalendar.PaintLabel += InternalPaintLabel;
		singleMonthCalendar.LabelMouseDown += new MouseEventHandler(InternalLabelMouseDown);
		singleMonthCalendar.LabelMouseEnter += InternalLabelMouseEnter;
		singleMonthCalendar.LabelMouseHover += InternalLabelMouseHover;
		singleMonthCalendar.LabelMouseLeave += InternalLabelMouseLeave;
		singleMonthCalendar.LabelMouseMove += new MouseEventHandler(InternalLabelMouseMove);
		singleMonthCalendar.LabelMouseUp += new MouseEventHandler(InternalLabelMouseUp);
		singleMonthCalendar.NavigationBackgroundStyle.StyleChanged += FirstCalendarBackStyleChanged;
		singleMonthCalendar.DateChanged += SingleMonthSelectedDateChanged;
		SubItems.Add(singleMonthCalendar);
		_BottomContainer = new ItemContainer();
		_BottomContainer.AutoCollapseOnClick = false;
		_BottomContainer.Visible = false;
		_BottomContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		_BottomContainer.LayoutOrientation = eOrientation.Horizontal;
		_BottomContainer.ItemSpacing = 6;
		_BottomContainer.Name = _BottomContainerName;
		_TodayButton = CreateTodayButton();
		_BottomContainer.SubItems.Add(_TodayButton);
		_ClearButton = CreateClearButton();
		_BottomContainer.SubItems.Add(_ClearButton);
		SubItems.Add(_BottomContainer);
		UpdateSystemButtonsVisibility();
	}

	public override void RecalcSize()
	{
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		if (!(SubItems[0] is SingleMonthCalendar singleMonthCalendar))
		{
			return;
		}
		_UpdatingSelectedDate = true;
		int num = 1;
		int num2 = 1;
		DateTime displayMonth = _DisplayMonth;
		Size empty = Size.Empty;
		bool disposeStyle = false;
		ElementStyle renderBackgroundStyle = GetRenderBackgroundStyle(out disposeStyle);
		Rectangle innerRect = ElementStyleLayout.GetInnerRect(renderBackgroundStyle, Bounds);
		SingleMonthCalendar singleMonthCalendar2 = singleMonthCalendar;
		SingleMonthCalendar singleMonthCalendar3 = null;
		singleMonthCalendar.TrailingDaysVisible = false;
		singleMonthCalendar.DisplayMonth = displayMonth;
		singleMonthCalendar.LeftInternal = innerRect.X;
		singleMonthCalendar.TopInternal = innerRect.Y;
		singleMonthCalendar.RecalcSize();
		singleMonthCalendar.Displayed = true;
		singleMonthCalendar.MouseSelectionEnabled = !_MultiSelect;
		if (!_MultiSelect)
		{
			UpdateSelectedDates(singleMonthCalendar);
		}
		else
		{
			singleMonthCalendar.SelectedDate = DateTime.MinValue;
		}
		empty = singleMonthCalendar.Size;
		Size size = singleMonthCalendar.Size;
		Point location = innerRect.Location;
		location.X += size.Width + _Spacing.Width;
		int num3 = empty.Width;
		Size size2 = new Size(1, 1);
		while ((location.Y < innerRect.Bottom && _CalendarDimensions.IsEmpty) || (!_CalendarDimensions.IsEmpty && _CalendarDimensions.Height + 1 > size2.Height))
		{
			if (num < SubItems.Count && SubItems[num] == _BottomContainer)
			{
				num++;
			}
			if ((location.X + size.Width > innerRect.Right && _CalendarDimensions.IsEmpty) || (!_CalendarDimensions.IsEmpty && size2.Width >= _CalendarDimensions.Width))
			{
				location.Y += size.Height + _Spacing.Height;
				location.X = innerRect.X;
				if (num3 > empty.Width)
				{
					empty.Width = num3 - ((size2.Height > 1) ? _Spacing.Width : 0);
				}
				num3 = 0;
				size2.Height++;
				size2.Width = 0;
				if ((location.Y + size.Height > innerRect.Bottom && _CalendarDimensions.IsEmpty) || (!_CalendarDimensions.IsEmpty && size2.Height > _CalendarDimensions.Height))
				{
					break;
				}
				empty.Height += size.Height + _Spacing.Height;
			}
			SingleMonthCalendar singleMonthCalendar4;
			if (SubItems.Count == num)
			{
				singleMonthCalendar4 = new SingleMonthCalendar();
				singleMonthCalendar4.DaySize = singleMonthCalendar2.DaySize;
				singleMonthCalendar4.Visible = true;
				singleMonthCalendar4.Displayed = true;
				singleMonthCalendar4.TrailingDaysVisible = false;
				singleMonthCalendar4.HeaderNavigationEnabled = false;
				singleMonthCalendar4.PaintLabel += InternalPaintLabel;
				singleMonthCalendar4.LabelMouseDown += new MouseEventHandler(InternalLabelMouseDown);
				singleMonthCalendar4.LabelMouseEnter += InternalLabelMouseEnter;
				singleMonthCalendar4.LabelMouseHover += InternalLabelMouseHover;
				singleMonthCalendar4.LabelMouseLeave += InternalLabelMouseLeave;
				singleMonthCalendar4.LabelMouseMove += new MouseEventHandler(InternalLabelMouseMove);
				singleMonthCalendar4.LabelMouseUp += new MouseEventHandler(InternalLabelMouseUp);
				singleMonthCalendar4.NavigationBackgroundStyle.ApplyStyle(singleMonthCalendar2.NavigationBackgroundStyle);
				singleMonthCalendar4.DateChanged += SingleMonthSelectedDateChanged;
				ApplyCommonProperties(singleMonthCalendar4);
				if (SubItems[SubItems.Count - 1] == _BottomContainer)
				{
					SubItems.Insert(SubItems.Count - 1, singleMonthCalendar4);
				}
				else
				{
					SubItems.Add(singleMonthCalendar4);
				}
			}
			else
			{
				singleMonthCalendar4 = (SingleMonthCalendar)SubItems[num];
			}
			singleMonthCalendar4.TwoLetterDayName = TwoLetterDayName;
			singleMonthCalendar4.MinDate = MinDate;
			singleMonthCalendar4.MaxDate = MaxDate;
			singleMonthCalendar4.WeekOfYearRule = WeekOfYearRule;
			singleMonthCalendar4.WeekendDaysSelectable = WeekendDaysSelectable;
			singleMonthCalendar4.TodayDate = TodayDate;
			singleMonthCalendar4.ShowTodayMarker = ShowTodayMarker;
			singleMonthCalendar4.ShowWeekNumbers = ShowWeekNumbers;
			singleMonthCalendar4.FirstDayOfWeek = FirstDayOfWeek;
			singleMonthCalendar4.DayNames = DayNames;
			singleMonthCalendar4.TrailingDaysVisible = false;
			singleMonthCalendar4.MouseSelectionEnabled = !_MultiSelect;
			if (!_MultiSelect)
			{
				UpdateSelectedDates(singleMonthCalendar4);
			}
			else
			{
				singleMonthCalendar4.SelectedDate = DateTime.MinValue;
			}
			num++;
			num2++;
			size2.Width++;
			num3 += _Spacing.Width;
			singleMonthCalendar4.TopInternal = location.Y;
			singleMonthCalendar4.LeftInternal = location.X;
			displayMonth = (singleMonthCalendar4.DisplayMonth = displayMonth.AddMonths(1));
			singleMonthCalendar4.RecalcSize();
			singleMonthCalendar3 = singleMonthCalendar4;
			num3 += singleMonthCalendar4.WidthInternal;
			location.X += singleMonthCalendar4.WidthInternal + _Spacing.Width;
		}
		for (int i = num; i < SubItems.Count; i++)
		{
			if (SubItems[i] is SingleMonthCalendar singleMonthCalendar5)
			{
				singleMonthCalendar5.PaintLabel -= InternalPaintLabel;
				singleMonthCalendar5.LabelMouseDown -= new MouseEventHandler(InternalLabelMouseDown);
				singleMonthCalendar5.LabelMouseEnter -= InternalLabelMouseEnter;
				singleMonthCalendar5.LabelMouseHover -= InternalLabelMouseHover;
				singleMonthCalendar5.LabelMouseLeave -= InternalLabelMouseLeave;
				singleMonthCalendar5.LabelMouseMove -= new MouseEventHandler(InternalLabelMouseMove);
				singleMonthCalendar5.LabelMouseUp -= new MouseEventHandler(InternalLabelMouseUp);
				singleMonthCalendar5.DateChanged -= SingleMonthSelectedDateChanged;
			}
			if (SubItems[i] != _BottomContainer)
			{
				SubItems.Remove(i);
			}
		}
		if (_BottomContainerName != null && _BottomContainer.Visible)
		{
			ItemContainer bottomContainer = _BottomContainer;
			bottomContainer.Displayed = true;
			bottomContainer.TopInternal = location.Y;
			bottomContainer.LeftInternal = location.X;
			bottomContainer.WidthInternal = empty.Width;
			bottomContainer.RecalcSize();
			bottomContainer.WidthInternal = empty.Width;
			empty.Height += bottomContainer.HeightInternal + _Spacing.Height;
		}
		empty.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(renderBackgroundStyle);
		empty.Height += ElementStyleLayout.VerticalStyleWhiteSpace(renderBackgroundStyle);
		if (SubItems.Count == 1 || (SubItems.Count == 2 && _BottomContainer != null))
		{
			singleMonthCalendar2._TrailingDaysAfterVisible = true;
			singleMonthCalendar2._TrailingDaysBeforeVisible = true;
			singleMonthCalendar2.TrailingDaysVisible = true;
		}
		else
		{
			singleMonthCalendar2._TrailingDaysAfterVisible = false;
			singleMonthCalendar2._TrailingDaysBeforeVisible = true;
			singleMonthCalendar2.TrailingDaysVisible = true;
			singleMonthCalendar3._TrailingDaysAfterVisible = true;
			singleMonthCalendar3._TrailingDaysBeforeVisible = false;
			singleMonthCalendar3.TrailingDaysVisible = true;
		}
		singleMonthCalendar2.NavigationMonthsAheadVisibility = num2;
		Bounds = new Rectangle(Bounds.Location, empty);
		base.RecalcSize();
		ApplySelection();
		OnMonthChanged(new EventArgs());
		_UpdatingSelectedDate = false;
		if (disposeStyle)
		{
			renderBackgroundStyle.Dispose();
		}
	}

	private void ApplyCommonProperties(SingleMonthCalendar m)
	{
		m.DisplayMonth = DateTime.Today;
		m.DayClickAutoClosePopup = _DayClickAutoClosePopup;
		m.YearSelectionEnabled = _YearSelectionEnabled;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		if (!(item is SingleMonthCalendar) && item != _BottomContainer)
		{
			throw new InvalidOperationException("MultiMonthCalendar can only accept CalendarMonth items.");
		}
		base.OnItemAdded(item);
	}

	public override BaseItem Copy()
	{
		MonthCalendarItem monthCalendarItem = new MonthCalendarItem();
		CopyToItem(monthCalendarItem);
		return monthCalendarItem;
	}

	internal void InternalCopyToItem(MonthCalendarItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem c)
	{
		MonthCalendarItem monthCalendarItem = c as MonthCalendarItem;
		monthCalendarItem.AnnuallyMarkedDates = AnnuallyMarkedDates;
		monthCalendarItem.CalendarDimensions = CalendarDimensions;
		monthCalendarItem.ClearButtonVisible = ClearButtonVisible;
		monthCalendarItem.CommandsBackgroundStyle.ApplyStyle(CommandsBackgroundStyle);
		monthCalendarItem.DayNames = DayNames;
		monthCalendarItem.DaySize = DaySize;
		monthCalendarItem.DisplayMonth = DisplayMonth;
		monthCalendarItem.FirstDayOfWeek = FirstDayOfWeek;
		monthCalendarItem.MarkedDates = MarkedDates;
		monthCalendarItem.MaxDate = MaxDate;
		monthCalendarItem.MaxSelectionCount = MaxSelectionCount;
		monthCalendarItem.MinDate = MinDate;
		monthCalendarItem.MonthlyMarkedDates = MonthlyMarkedDates;
		monthCalendarItem.MultiSelect = MultiSelect;
		monthCalendarItem.NavigationBackgroundStyle.ApplyStyle(NavigationBackgroundStyle);
		monthCalendarItem.SelectedDate = SelectedDate;
		monthCalendarItem.SelectionRange = SelectionRange;
		monthCalendarItem.ShowTodayMarker = ShowTodayMarker;
		monthCalendarItem.ShowWeekNumbers = ShowWeekNumbers;
		monthCalendarItem.TodayButtonVisible = TodayButtonVisible;
		monthCalendarItem.TodayDate = TodayDate;
		monthCalendarItem.TwoLetterDayName = TwoLetterDayName;
		monthCalendarItem.WeekendDaysSelectable = WeekendDaysSelectable;
		monthCalendarItem.WeeklyMarkedDays = WeeklyMarkedDays;
		monthCalendarItem.WeekOfYearRule = WeekOfYearRule;
		Colors.AnnualMarker.ApplyTo(monthCalendarItem.Colors.AnnualMarker);
		Colors.Day.ApplyTo(monthCalendarItem.Colors.Day);
		Colors.DayLabel.ApplyTo(monthCalendarItem.Colors.DayLabel);
		Colors.DayMarker.ApplyTo(monthCalendarItem.Colors.DayMarker);
		monthCalendarItem.Colors.DaysDividerBorderColors = Colors.DaysDividerBorderColors;
		Colors.MonthlyMarker.ApplyTo(monthCalendarItem.Colors.MonthlyMarker);
		Colors.Selection.ApplyTo(monthCalendarItem.Colors.Selection);
		Colors.Today.ApplyTo(monthCalendarItem.Colors.Today);
		Colors.TrailingDay.ApplyTo(monthCalendarItem.Colors.TrailingDay);
		Colors.TrailingWeekend.ApplyTo(monthCalendarItem.Colors.TrailingWeekend);
		Colors.Weekend.ApplyTo(monthCalendarItem.Colors.Weekend);
		Colors.WeeklyMarker.ApplyTo(monthCalendarItem.Colors.WeeklyMarker);
		Colors.WeekOfYear.ApplyTo(monthCalendarItem.Colors.WeekOfYear);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectionRange()
	{
		return !DateTime.Equals(SelectionEnd, SelectionStart);
	}

	public void SetSelectionRange(DateTime startDate, DateTime endDate)
	{
		if (_SelectionStart != DateTime.MinValue || _SelectionEnd != DateTime.MinValue)
		{
			if (startDate > endDate)
			{
				DateTime dateTime = startDate;
				startDate = endDate;
				endDate = dateTime;
			}
			if (startDate < MinDate)
			{
				throw new ArgumentOutOfRangeException("startDate is less than the minimum date allowable for a month calendar control");
			}
			if (startDate > MaxDate)
			{
				throw new ArgumentOutOfRangeException("startDate is greater than the maximum allowable date for a month calendar control");
			}
			if (endDate < MinDate)
			{
				throw new ArgumentOutOfRangeException("endDate is less than the minimum date allowable for a month calendar control");
			}
			if (endDate > MaxDate)
			{
				throw new ArgumentOutOfRangeException("endDate is greater than the maximum allowable date for a month calendar control");
			}
		}
		_SelectionStart = startDate;
		_SelectionEnd = endDate;
		ApplySelection();
		OnDateChanged(new EventArgs());
	}

	public void ApplySelection()
	{
		if (!_MultiSelect || _SelectionStart == DateTime.MinValue || _SelectionEnd == DateTime.MinValue)
		{
			return;
		}
		DateTime selectionStart = _SelectionStart;
		DateTime selectionEnd = _SelectionEnd;
		foreach (BaseItem subItem in SubItems)
		{
			if (!(subItem is SingleMonthCalendar singleMonthCalendar))
			{
				continue;
			}
			foreach (BaseItem subItem2 in singleMonthCalendar.SubItems)
			{
				if (subItem2 is DayLabel { Selectable: not false } dayLabel && !(dayLabel.Date == DateTime.MinValue))
				{
					if (dayLabel.Date >= selectionStart && dayLabel.Date <= selectionEnd)
					{
						dayLabel.IsSelected = true;
					}
					else
					{
						dayLabel.IsSelected = false;
					}
				}
			}
		}
	}

	private void OnMultiSelectChanged()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar)
			{
				singleMonthCalendar.MouseSelectionEnabled = !_MultiSelect;
				if (_MultiSelect)
				{
					singleMonthCalendar.SelectedDate = DateTime.MinValue;
				}
			}
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if (_MultiSelect && (int)objArg.Button == 1048576)
		{
			int x = objArg.X;
			int y = objArg.Y;
			_MouseDownLocation = new Point(x, y);
			DayLabel dayAt = GetDayAt(x, y);
			if (dayAt != null && dayAt.Date > MinDate && dayAt.Date < MaxDate)
			{
				_MouseDownDate = dayAt.Date;
				SetSelectionRange(dayAt.Date, dayAt.Date);
			}
		}
	}

	public DayLabel GetDayAt(int x, int y)
	{
		BaseItem baseItem = ItemAtLocation(x, y);
		if (baseItem != null && baseItem is SingleMonthCalendar && baseItem.ItemAtLocation(x, y) is DayLabel dayLabel && dayLabel.Date > DateTime.MinValue)
		{
			return dayLabel;
		}
		return null;
	}

	public DayLabel GetDay(DateTime date)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar)
			{
				DayLabel dayLabel = singleMonthCalendar.GetDayLabel(date);
				if (dayLabel != null)
				{
					return dayLabel;
				}
			}
		}
		return null;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		base.InternalMouseMove(objArg);
		if (!_MultiSelect || (int)objArg.Button != 1048576)
		{
			return;
		}
		DayLabel dayAt = GetDayAt(objArg.X, objArg.Y);
		if (dayAt != null && SelectionStart > DateTime.MinValue)
		{
			DateTime dateTime = dayAt.Date;
			DateTime dateTime2 = _MouseDownDate;
			if (Math.Abs(dateTime2.Subtract(dateTime).TotalDays) > (double)MaxSelectionCount)
			{
				dateTime = dateTime2.AddDays((MaxSelectionCount - 1) * Math.Sign(dateTime.Subtract(dateTime2).TotalDays));
			}
			if (dateTime2 < MinDate)
			{
				dateTime2 = MinDate;
			}
			if (dateTime2 > MaxDate)
			{
				dateTime2 = MaxDate;
			}
			if (dateTime > MaxDate)
			{
				dateTime = MaxDate;
			}
			if (dateTime < MinDate)
			{
				dateTime = MinDate;
			}
			SetSelectionRange(dateTime2, dateTime);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		base.InternalMouseUp(objArg);
		if ((int)objArg.Button == 1048576 && _MouseDownDate > DateTime.MinValue)
		{
			_MouseDownDate = DateTime.MinValue;
			OnDateSelected(new DateRangeEventArgs(SelectionStart, SelectionEnd));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMonthlyMarkedDates()
	{
		return _MonthlyMarkedDates.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMonthlyMarkedDates()
	{
		MonthlyMarkedDates = null;
	}

	private void OnMonthlyMarkedDatesChanged(DateTime[] dates)
	{
		_MonthlyMarkedDates.Clear();
		if (dates != null)
		{
			_MonthlyMarkedDates.AddRange(dates);
		}
		if (GetDesignMode())
		{
			UpdateMarkedDates();
		}
	}

	public void RemoveAllMonthlyMarkedDates()
	{
		MonthlyMarkedDates = null;
	}

	public void RemoveMonthlyMarkedDate(DateTime dt)
	{
		DateTime[] monthlyMarkedDates = MonthlyMarkedDates;
		DateTime[] array = monthlyMarkedDates;
		for (int i = 0; i < array.Length; i++)
		{
			DateTime item = array[i];
			if (item.Day == dt.Day)
			{
				_MonthlyMarkedDates.Remove(item);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWeeklyMarkedDays()
	{
		return _WeeklyMarkedDays.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeeklyMarkedDays()
	{
		MonthlyMarkedDates = null;
	}

	private void OnWeeklyMarkedDaysChanged(DayOfWeek[] days)
	{
		_WeeklyMarkedDays.Clear();
		if (days != null)
		{
			_WeeklyMarkedDays.AddRange(days);
		}
		if (GetDesignMode())
		{
			UpdateMarkedDates();
		}
	}

	public void RemoveAllWeeklyMarkedDays()
	{
		WeeklyMarkedDays = null;
	}

	public void RemoveWeeklyMarkedDay(DayOfWeek day)
	{
		DayOfWeek[] weeklyMarkedDays = WeeklyMarkedDays;
		DayOfWeek[] array = weeklyMarkedDays;
		foreach (DayOfWeek dayOfWeek in array)
		{
			if (dayOfWeek == day)
			{
				_WeeklyMarkedDays.Remove(dayOfWeek);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAnnuallyMarkedDates()
	{
		return _AnnuallyMarkedDates.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAnnuallyMarkedDates()
	{
		AnnuallyMarkedDates = null;
	}

	private void OnAnnuallyMarkedDatesChanged(DateTime[] dates)
	{
		_AnnuallyMarkedDates.Clear();
		if (dates != null)
		{
			_AnnuallyMarkedDates.AddRange(dates);
		}
		if (GetDesignMode())
		{
			UpdateMarkedDates();
		}
	}

	public void RemoveAllAnnuallyMarkedDates()
	{
		AnnuallyMarkedDates = null;
	}

	public void RemoveAnnuallyMarkedDate(DateTime dt)
	{
		DateTime[] annuallyMarkedDates = AnnuallyMarkedDates;
		DateTime[] array = annuallyMarkedDates;
		for (int i = 0; i < array.Length; i++)
		{
			DateTime item = array[i];
			if (item.Month == dt.Month && dt.Day == item.Day)
			{
				_AnnuallyMarkedDates.Remove(item);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMarkedDates()
	{
		return _MarkedDates.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMarkedDates()
	{
		MarkedDates = null;
	}

	private void OnMarkedDatesChanged(DateTime[] dates)
	{
		_MarkedDates.Clear();
		if (dates != null)
		{
			_MarkedDates.AddRange(dates);
		}
		if (GetDesignMode())
		{
			UpdateMarkedDates();
		}
	}

	public void RemoveAllMarkedDates()
	{
		MarkedDates = null;
	}

	public void RemoveMarkedDate(DateTime dt)
	{
		DateTime[] markedDates = MarkedDates;
		DateTime[] array = markedDates;
		foreach (DateTime dateTime in array)
		{
			if (dateTime == dt)
			{
				_MarkedDates.Remove(dateTime);
			}
		}
	}

	public void UpdateMarkedDates()
	{
		DisplayMonth = _DisplayMonth;
	}

	private bool GetDesignMode()
	{
		if (DesignMode)
		{
			return true;
		}
		if (ContainerControl is MonthCalendarAdv)
		{
			return ((MonthCalendarAdv)ContainerControl).GetDesignModeInternal();
		}
		return false;
	}

	private void FirstCalendarMonthChanging(object sender, EventArgs e)
	{
		OnMonthChanging(e);
	}

	private void FirstCalendarMonthChanged(object sender, EventArgs e)
	{
		if (e is EventSourceArgs { Source: eEventSource.Mouse })
		{
			SingleMonthCalendar singleMonthCalendar = SubItems[0] as SingleMonthCalendar;
			_DisplayMonth = singleMonthCalendar.DisplayMonth;
			DateTime displayMonth = _DisplayMonth.AddMonths(1);
			_UpdatingSelectedDate = true;
			for (int i = 1; i < SubItems.Count; i++)
			{
				if (SubItems[i] is SingleMonthCalendar singleMonthCalendar2)
				{
					singleMonthCalendar2.DisplayMonth = displayMonth;
					displayMonth = displayMonth.AddMonths(1);
					UpdateSelectedDates(singleMonthCalendar2);
				}
			}
			_UpdatingSelectedDate = false;
			NeedRecalcSize = false;
			ApplySelection();
		}
		if (!NeedRecalcSize)
		{
			OnMonthChanged(e);
		}
	}

	protected virtual void OnMonthChanged(EventArgs e)
	{
		if (this.MonthChanged != null)
		{
			bool needRecalcSize = NeedRecalcSize;
			this.MonthChanged(this, e);
			if (NeedRecalcSize != needRecalcSize)
			{
				NeedRecalcSize = false;
			}
		}
	}

	protected virtual void OnMonthChanging(EventArgs e)
	{
		if (this.MonthChanging != null)
		{
			this.MonthChanging(this, e);
		}
	}

	private void InternalPaintLabel(object sender, DayPaintEventArgs e)
	{
		OnPaintLabel(e._Item, e);
	}

	protected virtual void OnPaintLabel(DayLabel label, DayPaintEventArgs e)
	{
		if (this.PaintLabel != null)
		{
			this.PaintLabel(label, e);
		}
	}

	private void InternalLabelMouseUp(object sender, MouseEventArgs e)
	{
		OnLabelMouseUp(sender, e);
	}

	private void InternalLabelMouseMove(object sender, MouseEventArgs e)
	{
		OnLabelMouseMove(sender, e);
	}

	private void InternalLabelMouseLeave(object sender, EventArgs e)
	{
		OnLabelMouseLeave(sender, e);
	}

	private void InternalLabelMouseHover(object sender, EventArgs e)
	{
		OnLabelMouseHover(sender, e);
	}

	private void InternalLabelMouseEnter(object sender, EventArgs e)
	{
		OnLabelMouseEnter(sender, e);
	}

	private void InternalLabelMouseDown(object sender, MouseEventArgs e)
	{
		OnLabelMouseDown(sender, e);
	}

	protected virtual void OnLabelMouseDown(object sender, MouseEventArgs e)
	{
		if (this.LabelMouseDown != null)
		{
			this.LabelMouseDown.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseUp(object sender, MouseEventArgs e)
	{
		if (this.LabelMouseUp != null)
		{
			this.LabelMouseUp.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseEnter(object sender, EventArgs e)
	{
		if (this.LabelMouseEnter != null)
		{
			this.LabelMouseEnter(sender, e);
		}
	}

	protected virtual void OnLabelMouseLeave(object sender, EventArgs e)
	{
		if (this.LabelMouseLeave != null)
		{
			this.LabelMouseLeave(sender, e);
		}
	}

	protected virtual void OnLabelMouseMove(object sender, MouseEventArgs e)
	{
		if (this.LabelMouseMove != null)
		{
			this.LabelMouseMove.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseHover(object sender, EventArgs e)
	{
		if (this.LabelMouseHover != null)
		{
			this.LabelMouseHover(sender, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDaySize()
	{
		int width = DaySize.Width;
		Size defaultDaySize = SingleMonthCalendar._DefaultDaySize;
		if (width == defaultDaySize.Width)
		{
			int height = DaySize.Height;
			defaultDaySize = SingleMonthCalendar._DefaultDaySize;
			return height != defaultDaySize.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		DaySize = SingleMonthCalendar._DefaultDaySize;
	}

	public bool ShouldSerializeMinDate()
	{
		return !MinDate.Equals(DateTimeGroup.MinDateTime);
	}

	public void ResetMinDate()
	{
		MinDate = DateTimeGroup.MinDateTime;
	}

	public bool ShouldSerializeMaxDate()
	{
		return !MaxDate.Equals(DateTimeGroup.MaxDateTime);
	}

	public void ResetMaxDate()
	{
		MaxDate = DateTimeGroup.MaxDateTime;
	}

	private void FirstCalendarBackStyleChanged(object sender, EventArgs e)
	{
		SingleMonthCalendar firstCalendar = FirstCalendar;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar && singleMonthCalendar != firstCalendar)
			{
				singleMonthCalendar.NavigationBackgroundStyle.Reset();
				singleMonthCalendar.NavigationBackgroundStyle.ApplyStyle(firstCalendar.NavigationBackgroundStyle);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCalendarDimensions()
	{
		CalendarDimensions = Size.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCalendarDimensions()
	{
		return CalendarDimensions != Size.Empty;
	}

	public void SetCalendarDimensions(int columns, int rows)
	{
		CalendarDimensions = new Size(columns, rows);
	}

	private void UpdateSelectedDates(SingleMonthCalendar m)
	{
		if ((m.DisplayMonth.Month == _SelectedDate.Month && m.DisplayMonth.Year == _SelectedDate.Year) || (m.TrailingDaysVisible && m.GetDayLabel(_SelectedDate) != null))
		{
			m.SelectedDate = _SelectedDate;
		}
		else if (m.SelectedDate != DateTime.MinValue)
		{
			m.SelectedDate = DateTime.MinValue;
		}
	}

	protected virtual void OnDateChanged(EventArgs e)
	{
		if (this.DateChanged != null)
		{
			this.DateChanged(this, e);
		}
	}

	protected virtual void OnDateSelected(DateRangeEventArgs e)
	{
		if (this.DateSelected != null)
		{
			this.DateSelected.Invoke((object)this, e);
		}
	}

	private void SingleMonthSelectedDateChanged(object sender, EventArgs e)
	{
		if (!_UpdatingSelectedDate)
		{
			SingleMonthCalendar singleMonthCalendar = sender as SingleMonthCalendar;
			SelectedDate = singleMonthCalendar.SelectedDate;
		}
	}

	private ButtonItem CreateTodayButton()
	{
		ButtonItem buttonItem = new ButtonItem(_TodayButtonName);
		buttonItem.GlobalItem = false;
		buttonItem.Text = GetTodayButtonText();
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Click += TodayButtonClick;
		return buttonItem;
	}

	private void TodayButtonClick(object sender, EventArgs e)
	{
		DateTime selectedDate = (DisplayMonth = (TodayDateSet ? TodayDate : DateTime.Today));
		SelectedDate = selectedDate;
	}

	private string GetTodayButtonText()
	{
		string text = "";
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			text = localizationManager.GetLocalizedString(LocalizationKeys.MonthCalendarTodayButtonText);
		}
		if (text == "")
		{
			text = "Today";
		}
		return text;
	}

	public void ReloadLocalizedStrings()
	{
		if (_TodayButton != null)
		{
			_TodayButton.Text = GetTodayButtonText();
		}
		if (_ClearButton != null)
		{
			_ClearButton.Text = GetClearButtonText();
		}
	}

	private ButtonItem CreateClearButton()
	{
		ButtonItem buttonItem = new ButtonItem(_ClearButtonName);
		buttonItem.GlobalItem = false;
		buttonItem.Text = GetClearButtonText();
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Click += ClearButtonClick;
		return buttonItem;
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		SelectedDate = DateTime.MinValue;
	}

	private string GetClearButtonText()
	{
		string text = "";
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			text = localizationManager.GetLocalizedString(LocalizationKeys.MonthCalendarClearButtonText);
		}
		if (text == "")
		{
			text = "Clear";
		}
		return text;
	}

	private void UpdateSystemButtonsVisibility()
	{
		if (_TodayButtonVisible != _TodayButton.Visible)
		{
			_TodayButton.Visible = _TodayButtonVisible;
		}
		if (_ClearButtonVisible != _ClearButton.Visible)
		{
			_ClearButton.Visible = _ClearButtonVisible;
		}
		bool flag = _ClearButtonVisible | _TodayButtonVisible;
		if (flag != _BottomContainer.Visible)
		{
			_BottomContainer.Visible = flag;
		}
	}

	protected virtual void OnDayClickAutoClosePopupChanged(bool oldValue, bool newValue)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar)
			{
				((SingleMonthCalendar)subItem).DayClickAutoClosePopup = newValue;
			}
		}
	}
}
