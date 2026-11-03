using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Events;

namespace DevComponents.Editors.DateTimeAdv;

public class MonthCalendarItem : CalendarBase
{
	private Size __2569 = new Size(10, 8);

	private List<DateTime> __256A = new List<DateTime>();

	private List<DateTime> __256B = new List<DateTime>();

	private List<DateTime> __256C = new List<DateTime>();

	private static string __256D = "sysCalBottomContainer";

	private static string __256E = "sysTodayButton";

	private static string __256F = "sysClearSelectionButton";

	private ItemContainer __2570;

	private ButtonItem __2571;

	private ButtonItem __2572;

	private EventHandler _2573;

	private EventHandler _2574;

	private DayPaintEventHandler _2398;

	private MouseEventHandler _2575;

	private MouseEventHandler _2576;

	private EventHandler _2577;

	private EventHandler _2578;

	private MouseEventHandler _2579;

	private EventHandler _257A;

	private EventHandler _257B;

	private DateRangeEventHandler _257C;

	private int __257D = 7;

	private DateTime __257E = DateTime.MinValue;

	private DateTime __257F = DateTime.MinValue;

	private bool __05EB;

	private Point _ᾖ = Point.Empty;

	private DateTime __2580 = DateTime.MinValue;

	private List<DayOfWeek> __2581 = new List<DayOfWeek>();

	private DateTime __2582 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

	private MonthCalendarColors _ቿ = new MonthCalendarColors();

	private Size __2583 = Size.Empty;

	private bool __2584;

	private DateTime __2585 = DateTime.MinValue;

	private bool __2586;

	private bool __2587;

	private bool __2588 = true;

	private bool __2589 = true;

	private bool __258A = true;

	private bool __258B;

	[DefaultValue(7)]
	[Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
	public int MaxSelectionCount
	{
		get
		{
			return __257D;
		}
		set
		{
			__257D = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionStart
	{
		get
		{
			return __257E;
		}
		set
		{
			if (__257E != value)
			{
				SetSelectionRange(value, (SelectionEnd == DateTime.MinValue) ? value : SelectionEnd);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionEnd
	{
		get
		{
			return __257F;
		}
		set
		{
			if (__257F != value)
			{
				SetSelectionRange((SelectionStart == DateTime.MinValue) ? value : SelectionStart, value);
			}
		}
	}

	[Description("Indicates selected range of dates for a month calendar control.")]
	[Bindable(true)]
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

	[Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
	[DefaultValue(false)]
	public bool MultiSelect
	{
		get
		{
			return __05EB;
		}
		set
		{
			if (__05EB != value)
			{
				__05EB = value;
				_258E();
			}
		}
	}

	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	[Localizable(true)]
	public DateTime[] MonthlyMarkedDates
	{
		get
		{
			return __256B.ToArray();
		}
		set
		{
			_258F(value);
		}
	}

	[Localizable(true)]
	[Description("Indicates array of DayOfWeek members that determine which days of week to mark using Colors.WeeklyMarker settings. ")]
	public DayOfWeek[] WeeklyMarkedDays
	{
		get
		{
			return __2581.ToArray();
		}
		set
		{
			_2591(value);
		}
	}

	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
	public DateTime[] AnnuallyMarkedDates
	{
		get
		{
			return __256A.ToArray();
		}
		set
		{
			_2593(value);
		}
	}

	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which nonrecurring dates are marked using Colors.DayMarker settings.")]
	public DateTime[] MarkedDates
	{
		get
		{
			return __256C.ToArray();
		}
		set
		{
			_2594(value);
		}
	}

	[Description("Indicates the first month displayed on the control.")]
	public DateTime DisplayMonth
	{
		get
		{
			return __2582;
		}
		set
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			__2582 = value.Date;
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

	[Description("Gets colors used by control.")]
	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	public MonthCalendarColors Colors => _ቿ;

	private SingleMonthCalendar _2568 => (SingleMonthCalendar)SubItems[0];

	[Description("Indicate size of each day item on the calendar.")]
	public Size DaySize
	{
		get
		{
			return _2568.DaySize;
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

	[Description("Indicate size of date navigation buttons on the calendar. If you increase size of the navigation buttons change DaySize as well so everything fits.")]
	public Size NavigationButtonSize
	{
		get
		{
			return _2568.NavigationButtonSize;
		}
		set
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.NavigationButtonSize = value;
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
			return _2568.MinDate;
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
			return _2568.MaxDate;
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

	[Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Style")]
	public ElementStyle CommandsBackgroundStyle => __2570.BackgroundStyle;

	[Browsable(true)]
	[Description("Indicates navigation container background style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle NavigationBackgroundStyle => _2568.NavigationBackgroundStyle;

	[Description("Indicates number of columns and rows of months displayed on control.")]
	public Size CalendarDimensions
	{
		get
		{
			return __2583;
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
			if (__2583 != value)
			{
				__2583 = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Description("Indicates whether weekend days can be selected.")]
	[DefaultValue(true)]
	public bool WeekendDaysSelectable
	{
		get
		{
			return _2568.WeekendDaysSelectable;
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

	[Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
	[DefaultValue(CalendarWeekRule.FirstDay)]
	public CalendarWeekRule WeekOfYearRule
	{
		get
		{
			return _2568.WeekOfYearRule;
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime TodayDate
	{
		get
		{
			return _2568.TodayDate;
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
	public bool TodayDateSet => _2568.TodayDateSet;

	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	[DefaultValue(true)]
	public bool ShowTodayMarker
	{
		get
		{
			return _2568.ShowTodayMarker;
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
			return _2568.ShowWeekNumbers;
		}
		set
		{
			if (ShowWeekNumbers != value)
			{
				_2568.ShowWeekNumbers = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Description("Indicates whether control uses the two letter day names.")]
	[DefaultValue(true)]
	public bool TwoLetterDayName
	{
		get
		{
			return _2568.TwoLetterDayName;
		}
		set
		{
			_2568.TwoLetterDayName = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Indicates array of custom names for days displayed on calendar header.")]
	[DefaultValue(null)]
	[Localizable(true)]
	public string[] DayNames
	{
		get
		{
			return _2568.DayNames;
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

	[Description("Indicates first day of week displayed on the calendar.")]
	[DefaultValue(DayOfWeek.Sunday)]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return _2568.FirstDayOfWeek;
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
			return __2585;
		}
		set
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			if (!(__2585 != value))
			{
				return;
			}
			__2585 = value;
			try
			{
				__2584 = true;
				foreach (BaseItem subItem in SubItems)
				{
					if (subItem is SingleMonthCalendar ⴘ)
					{
						_259F(ⴘ);
					}
				}
				OnDateChanged(new EventArgs());
				OnDateSelected(new DateRangeEventArgs(value, value));
			}
			finally
			{
				__2584 = false;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ItemContainer BottomContainer => __2570;

	[Browsable(false)]
	public ButtonItem TodayButton => __2571;

	[Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
	[DefaultValue(false)]
	public bool TodayButtonVisible
	{
		get
		{
			return __2586;
		}
		set
		{
			if (__2586 != value)
			{
				__2586 = value;
				NeedRecalcSize = true;
				_25A6();
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
	[DefaultValue(false)]
	public bool ClearButtonVisible
	{
		get
		{
			return __2587;
		}
		set
		{
			if (__2587 != value)
			{
				__2587 = value;
				NeedRecalcSize = true;
				_25A6();
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
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
			return __2588;
		}
		set
		{
			if (value != __2588)
			{
				bool oldValue = __2588;
				__2588 = value;
				OnDayClickAutoClosePopupChanged(oldValue, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether Year/Century selection is enabled when calendar is displaying single month.")]
	[DefaultValue(true)]
	public bool YearSelectionEnabled
	{
		get
		{
			return __2589;
		}
		set
		{
			if (__2589 == value)
			{
				return;
			}
			__2589 = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.YearSelectionEnabled = __2589;
				}
			}
		}
	}

	[Description("Indicates whether month selector which is displayed when month label is clicked is using abbreviated month names instead of month number.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool MonthSelectorShortMonthNames
	{
		get
		{
			return __258A;
		}
		set
		{
			if (__258A == value)
			{
				return;
			}
			__258A = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.MonthSelectorShortMonthNames = __258A;
				}
			}
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether calendar is used in month selection mode which shows only month and year.")]
	public bool MonthSelectionMode
	{
		get
		{
			return __258B;
		}
		set
		{
			if (value == __258B)
			{
				return;
			}
			__258B = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is SingleMonthCalendar singleMonthCalendar)
				{
					singleMonthCalendar.MonthSelectionMode = __258B;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	public event EventHandler MonthChanged
	{
		add
		{
			EventHandler eventHandler = _2573;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2573, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2573;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2573, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler MonthChanging
	{
		add
		{
			EventHandler eventHandler = _2574;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2574, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2574;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2574, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
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

	[Description("Occurs when mouse button is pressed over the day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2575;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2575, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2575;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2575, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse button is released over day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2576;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2576, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2576;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2576, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters the day/week label inside of the calendar.")]
	public event EventHandler LabelMouseEnter
	{
		add
		{
			EventHandler eventHandler = _2577;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2577, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2577;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2577, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves the day/week label inside of the calendar.")]
	public event EventHandler LabelMouseLeave
	{
		add
		{
			EventHandler eventHandler = _2578;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2578, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2578;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2578, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse moves over the day/week label inside of the calendar.")]
	public event MouseEventHandler LabelMouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2579;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2579, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2579;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2579, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse remains still inside an day/week label of the calendar for an amount of time.")]
	public event EventHandler LabelMouseHover
	{
		add
		{
			EventHandler eventHandler = _257A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _257A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _257A;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _257A, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when SelectedDate property has changed.")]
	public event EventHandler DateChanged
	{
		add
		{
			EventHandler eventHandler = _257B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _257B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _257B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _257B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the user makes an explicit date selection using the mouse.")]
	public event DateRangeEventHandler DateSelected
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DateRangeEventHandler val = _257C;
			DateRangeEventHandler val2;
			do
			{
				val2 = val;
				DateRangeEventHandler value2 = (DateRangeEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _257C, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DateRangeEventHandler val = _257C;
			DateRangeEventHandler val2;
			do
			{
				val2 = val;
				DateRangeEventHandler value2 = (DateRangeEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _257C, value2, val2);
			}
			while (val != val2);
		}
	}

	public MonthCalendarItem()
	{
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		_ቿ.ކ = this;
		SingleMonthCalendar singleMonthCalendar = new SingleMonthCalendar();
		singleMonthCalendar.MonthChanging += _2595;
		singleMonthCalendar.MonthChanged += _2596;
		singleMonthCalendar.PaintLabel += _2597;
		singleMonthCalendar.LabelMouseDown += new MouseEventHandler(_259D);
		singleMonthCalendar.LabelMouseEnter += _259C;
		singleMonthCalendar.LabelMouseHover += _259B;
		singleMonthCalendar.LabelMouseLeave += _259A;
		singleMonthCalendar.LabelMouseMove += new MouseEventHandler(_2599);
		singleMonthCalendar.LabelMouseUp += new MouseEventHandler(_2598);
		singleMonthCalendar.NavigationBackgroundStyle.StyleChanged += _259E;
		singleMonthCalendar.DateChanged += _25A0;
		SubItems.Add(singleMonthCalendar);
		__2570 = new ItemContainer();
		__2570.AutoCollapseOnClick = false;
		__2570.Visible = false;
		__2570.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		__2570.LayoutOrientation = eOrientation.Horizontal;
		__2570.ItemSpacing = 6;
		__2570.Name = __256D;
		__2571 = _25A1();
		__2570.SubItems.Add(__2571);
		__2572 = _25A4();
		__2570.SubItems.Add(__2572);
		SubItems.Add(__2570);
		_25A6();
	}

	internal void _258C()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar)
			{
				singleMonthCalendar._258C();
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		if (!(SubItems[0] is SingleMonthCalendar singleMonthCalendar))
		{
			return;
		}
		__2584 = true;
		int num = 1;
		int num2 = 1;
		DateTime displayMonth = __2582;
		Size empty = Size.Empty;
		bool disposeStyle = false;
		ElementStyle renderBackgroundStyle = GetRenderBackgroundStyle(out disposeStyle);
		Rectangle rectangle = ᝈ._1756(renderBackgroundStyle, Bounds);
		SingleMonthCalendar singleMonthCalendar2 = singleMonthCalendar;
		SingleMonthCalendar singleMonthCalendar3 = null;
		singleMonthCalendar.TrailingDaysVisible = false;
		singleMonthCalendar.DisplayMonth = displayMonth;
		singleMonthCalendar.LeftInternal = rectangle.X;
		singleMonthCalendar.TopInternal = rectangle.Y;
		singleMonthCalendar.RecalcSize();
		singleMonthCalendar.Displayed = true;
		singleMonthCalendar.MouseSelectionEnabled = !__05EB;
		if (!__05EB)
		{
			_259F(singleMonthCalendar);
		}
		else
		{
			singleMonthCalendar.SelectedDate = DateTime.MinValue;
		}
		empty = singleMonthCalendar.Size;
		Size size = singleMonthCalendar.Size;
		Point location = rectangle.Location;
		location.X += size.Width + __2569.Width;
		int num3 = empty.Width;
		Size size2 = new Size(1, 1);
		while ((location.Y < rectangle.Bottom && __2583.IsEmpty) || (!__2583.IsEmpty && __2583.Height + 1 > size2.Height))
		{
			if (num < SubItems.Count && SubItems[num] == __2570)
			{
				num++;
			}
			if ((location.X + size.Width > rectangle.Right && __2583.IsEmpty) || (!__2583.IsEmpty && size2.Width >= __2583.Width))
			{
				location.Y += size.Height + __2569.Height;
				location.X = rectangle.X;
				if (num3 > empty.Width)
				{
					empty.Width = num3 - ((size2.Height > 1) ? __2569.Width : 0);
				}
				num3 = 0;
				size2.Height++;
				size2.Width = 0;
				if ((location.Y + size.Height > rectangle.Bottom && __2583.IsEmpty) || (!__2583.IsEmpty && size2.Height > __2583.Height))
				{
					break;
				}
				empty.Height += size.Height + __2569.Height;
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
				singleMonthCalendar4.PaintLabel += _2597;
				singleMonthCalendar4.LabelMouseDown += new MouseEventHandler(_259D);
				singleMonthCalendar4.LabelMouseEnter += _259C;
				singleMonthCalendar4.LabelMouseHover += _259B;
				singleMonthCalendar4.LabelMouseLeave += _259A;
				singleMonthCalendar4.LabelMouseMove += new MouseEventHandler(_2599);
				singleMonthCalendar4.LabelMouseUp += new MouseEventHandler(_2598);
				singleMonthCalendar4.NavigationBackgroundStyle.ApplyStyle(singleMonthCalendar2.NavigationBackgroundStyle);
				singleMonthCalendar4.DateChanged += _25A0;
				_258D(singleMonthCalendar4);
				if (SubItems[SubItems.Count - 1] == __2570)
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
			singleMonthCalendar4.MouseSelectionEnabled = !__05EB;
			if (!__05EB)
			{
				_259F(singleMonthCalendar4);
			}
			else
			{
				singleMonthCalendar4.SelectedDate = DateTime.MinValue;
			}
			num++;
			num2++;
			size2.Width++;
			num3 += __2569.Width;
			singleMonthCalendar4.TopInternal = location.Y;
			singleMonthCalendar4.LeftInternal = location.X;
			displayMonth = (singleMonthCalendar4.DisplayMonth = displayMonth.AddMonths(1));
			singleMonthCalendar4.RecalcSize();
			singleMonthCalendar3 = singleMonthCalendar4;
			num3 += singleMonthCalendar4.WidthInternal;
			location.X += singleMonthCalendar4.WidthInternal + __2569.Width;
		}
		for (int i = num; i < SubItems.Count; i++)
		{
			if (SubItems[i] is SingleMonthCalendar singleMonthCalendar5)
			{
				singleMonthCalendar5.PaintLabel -= _2597;
				singleMonthCalendar5.LabelMouseDown -= new MouseEventHandler(_259D);
				singleMonthCalendar5.LabelMouseEnter -= _259C;
				singleMonthCalendar5.LabelMouseHover -= _259B;
				singleMonthCalendar5.LabelMouseLeave -= _259A;
				singleMonthCalendar5.LabelMouseMove -= new MouseEventHandler(_2599);
				singleMonthCalendar5.LabelMouseUp -= new MouseEventHandler(_2598);
				singleMonthCalendar5.DateChanged -= _25A0;
			}
			if (SubItems[i] != __2570)
			{
				SubItems.Remove(i);
			}
		}
		if (__256D != null && __2570.Visible)
		{
			ItemContainer itemContainer = __2570;
			itemContainer.Displayed = true;
			itemContainer.TopInternal = location.Y;
			itemContainer.LeftInternal = location.X;
			itemContainer.WidthInternal = empty.Width;
			itemContainer.RecalcSize();
			itemContainer.WidthInternal = empty.Width;
			empty.Height += itemContainer.HeightInternal + __2569.Height;
		}
		empty.Width += ᝈ.ᝍ(renderBackgroundStyle);
		empty.Height += ᝈ.ᝏ(renderBackgroundStyle);
		if (SubItems.Count == 1 || (SubItems.Count == 2 && __2570 != null))
		{
			singleMonthCalendar2.__25CF = true;
			singleMonthCalendar2.__25CE = true;
			singleMonthCalendar2.TrailingDaysVisible = true;
		}
		else
		{
			singleMonthCalendar2.__25CF = false;
			singleMonthCalendar2.__25CE = true;
			singleMonthCalendar2.TrailingDaysVisible = true;
			singleMonthCalendar3.__25CF = true;
			singleMonthCalendar3.__25CE = false;
			singleMonthCalendar3.TrailingDaysVisible = true;
		}
		singleMonthCalendar2._25BD = num2;
		Bounds = new Rectangle(Bounds.Location, empty);
		base.RecalcSize();
		ApplySelection();
		OnMonthChanged(new EventArgs());
		__2584 = false;
		if (disposeStyle)
		{
			renderBackgroundStyle.Dispose();
		}
	}

	private void _258D(SingleMonthCalendar Ⴘ)
	{
		Ⴘ.DisplayMonth = DateTime.Today;
		Ⴘ.DayClickAutoClosePopup = __2588;
		Ⴘ.YearSelectionEnabled = __2589;
		Ⴘ.MonthSelectionMode = __258B;
		Ⴘ.MonthSelectorShortMonthNames = __258A;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		if (!(item is SingleMonthCalendar) && item != __2570)
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

	internal void _0EBA(MonthCalendarItem _0EBB)
	{
		CopyToItem(_0EBB);
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
		if (__257E != DateTime.MinValue || __257F != DateTime.MinValue)
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
		__257E = startDate;
		__257F = endDate;
		ApplySelection();
		OnDateChanged(new EventArgs());
	}

	public void ApplySelection()
	{
		if (!__05EB || __257E == DateTime.MinValue || __257F == DateTime.MinValue)
		{
			return;
		}
		DateTime dateTime = __257E;
		DateTime dateTime2 = __257F;
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
					if (dayLabel.Date >= dateTime && dayLabel.Date <= dateTime2)
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

	private void _258E()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar)
			{
				singleMonthCalendar.MouseSelectionEnabled = !__05EB;
				if (__05EB)
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
		if (__05EB && (int)objArg.Button == 1048576)
		{
			int x = objArg.X;
			int y = objArg.Y;
			_ᾖ = new Point(x, y);
			DayLabel dayAt = GetDayAt(x, y);
			if (dayAt != null && dayAt.Date > MinDate && dayAt.Date < MaxDate)
			{
				__2580 = dayAt.Date;
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
		if (!__05EB || (int)objArg.Button != 1048576)
		{
			return;
		}
		DayLabel dayAt = GetDayAt(objArg.X, objArg.Y);
		if (dayAt != null && SelectionStart > DateTime.MinValue)
		{
			DateTime dateTime = dayAt.Date;
			DateTime dateTime2 = __2580;
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
		if ((int)objArg.Button == 1048576 && __2580 > DateTime.MinValue)
		{
			__2580 = DateTime.MinValue;
			OnDateSelected(new DateRangeEventArgs(SelectionStart, SelectionEnd));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMonthlyMarkedDates()
	{
		return __256B.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMonthlyMarkedDates()
	{
		MonthlyMarkedDates = null;
	}

	private void _258F(DateTime[] _2590)
	{
		__256B.Clear();
		if (_2590 != null)
		{
			__256B.AddRange(_2590);
		}
		if (ఛ())
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
				__256B.Remove(item);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWeeklyMarkedDays()
	{
		return __2581.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeeklyMarkedDays()
	{
		MonthlyMarkedDates = null;
	}

	private void _2591(DayOfWeek[] _2592)
	{
		__2581.Clear();
		if (_2592 != null)
		{
			__2581.AddRange(_2592);
		}
		if (ఛ())
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
				__2581.Remove(dayOfWeek);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAnnuallyMarkedDates()
	{
		return __256A.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAnnuallyMarkedDates()
	{
		AnnuallyMarkedDates = null;
	}

	private void _2593(DateTime[] _2590)
	{
		__256A.Clear();
		if (_2590 != null)
		{
			__256A.AddRange(_2590);
		}
		if (ఛ())
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
				__256A.Remove(item);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMarkedDates()
	{
		return __256C.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMarkedDates()
	{
		MarkedDates = null;
	}

	private void _2594(DateTime[] _2590)
	{
		__256C.Clear();
		if (_2590 != null)
		{
			__256C.AddRange(_2590);
		}
		if (ఛ())
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
				__256C.Remove(dateTime);
			}
		}
	}

	public void UpdateMarkedDates()
	{
		DisplayMonth = __2582;
	}

	private bool ఛ()
	{
		if (DesignMode)
		{
			return true;
		}
		if (ContainerControl is MonthCalendarAdv)
		{
			return ((MonthCalendarAdv)ContainerControl)._2658();
		}
		return false;
	}

	private void _2595(object _٠, EventArgs ؿ)
	{
		OnMonthChanging(ؿ);
	}

	private void _2596(object _٠, EventArgs ؿ)
	{
		if (ؿ is EventSourceArgs { Source: eEventSource.Mouse })
		{
			SingleMonthCalendar singleMonthCalendar = SubItems[0] as SingleMonthCalendar;
			__2582 = singleMonthCalendar.DisplayMonth;
			DateTime displayMonth = __2582.AddMonths(1);
			__2584 = true;
			for (int i = 1; i < SubItems.Count; i++)
			{
				if (SubItems[i] is SingleMonthCalendar singleMonthCalendar2)
				{
					singleMonthCalendar2.DisplayMonth = displayMonth;
					displayMonth = displayMonth.AddMonths(1);
					_259F(singleMonthCalendar2);
				}
			}
			__2584 = false;
			NeedRecalcSize = false;
			ApplySelection();
		}
		if (!NeedRecalcSize)
		{
			OnMonthChanged(ؿ);
		}
	}

	protected virtual void OnMonthChanged(EventArgs e)
	{
		if (_2573 != null)
		{
			bool needRecalcSize = NeedRecalcSize;
			_2573(this, e);
			if (NeedRecalcSize != needRecalcSize)
			{
				NeedRecalcSize = false;
			}
		}
	}

	protected virtual void OnMonthChanging(EventArgs e)
	{
		if (_2574 != null)
		{
			_2574(this, e);
		}
	}

	private void _2597(object _٠, DayPaintEventArgs ؿ)
	{
		OnPaintLabel(ؿ._ᕹ, ؿ);
	}

	protected virtual void OnPaintLabel(DayLabel label, DayPaintEventArgs e)
	{
		if (_2398 != null)
		{
			_2398(label, e);
		}
	}

	private void _2598(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseUp(_٠, ؿ);
	}

	private void _2599(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseMove(_٠, ؿ);
	}

	private void _259A(object _٠, EventArgs ؿ)
	{
		OnLabelMouseLeave(_٠, ؿ);
	}

	private void _259B(object _٠, EventArgs ؿ)
	{
		OnLabelMouseHover(_٠, ؿ);
	}

	private void _259C(object _٠, EventArgs ؿ)
	{
		OnLabelMouseEnter(_٠, ؿ);
	}

	private void _259D(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseDown(_٠, ؿ);
	}

	protected virtual void OnLabelMouseDown(object sender, MouseEventArgs e)
	{
		if (_2575 != null)
		{
			_2575.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseUp(object sender, MouseEventArgs e)
	{
		if (_2576 != null)
		{
			_2576.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseEnter(object sender, EventArgs e)
	{
		if (_2577 != null)
		{
			_2577(sender, e);
		}
	}

	protected virtual void OnLabelMouseLeave(object sender, EventArgs e)
	{
		if (_2578 != null)
		{
			_2578(sender, e);
		}
	}

	protected virtual void OnLabelMouseMove(object sender, MouseEventArgs e)
	{
		if (_2579 != null)
		{
			_2579.Invoke(sender, e);
		}
	}

	protected virtual void OnLabelMouseHover(object sender, EventArgs e)
	{
		if (_257A != null)
		{
			_257A(sender, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDaySize()
	{
		int width = DaySize.Width;
		Size __25C5 = SingleMonthCalendar.__25C5;
		if (width == __25C5.Width)
		{
			int height = DaySize.Height;
			Size __25C52 = SingleMonthCalendar.__25C5;
			return height != __25C52.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		DaySize = SingleMonthCalendar.__25C5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNavigationButtonSize()
	{
		int width = NavigationButtonSize.Width;
		Size __25C6 = SingleMonthCalendar.__25C6;
		if (width == __25C6.Width)
		{
			int height = NavigationButtonSize.Height;
			Size __25C62 = SingleMonthCalendar.__25C6;
			return height != __25C62.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNavigationButtonSize()
	{
		NavigationButtonSize = SingleMonthCalendar.__25C6;
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

	private void _259E(object _٠, EventArgs ؿ)
	{
		SingleMonthCalendar singleMonthCalendar = _2568;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is SingleMonthCalendar singleMonthCalendar2 && singleMonthCalendar2 != singleMonthCalendar)
			{
				singleMonthCalendar2.NavigationBackgroundStyle.Reset();
				singleMonthCalendar2.NavigationBackgroundStyle.ApplyStyle(singleMonthCalendar.NavigationBackgroundStyle);
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

	private void _259F(SingleMonthCalendar Ⴘ)
	{
		if (Ⴘ.MonthSelectionMode)
		{
			Ⴘ.SelectedDate = __2585;
		}
		else if ((Ⴘ.DisplayMonth.Month == __2585.Month && Ⴘ.DisplayMonth.Year == __2585.Year) || (Ⴘ.TrailingDaysVisible && Ⴘ.GetDayLabel(__2585) != null))
		{
			Ⴘ.SelectedDate = __2585;
		}
		else if (Ⴘ.SelectedDate != DateTime.MinValue)
		{
			Ⴘ.SelectedDate = DateTime.MinValue;
		}
	}

	protected virtual void OnDateChanged(EventArgs e)
	{
		if (_257B != null)
		{
			_257B(this, e);
		}
	}

	protected virtual void OnDateSelected(DateRangeEventArgs e)
	{
		if (_257C != null)
		{
			_257C.Invoke((object)this, e);
		}
	}

	private void _25A0(object _٠, EventArgs ؿ)
	{
		if (!__2584)
		{
			SingleMonthCalendar singleMonthCalendar = _٠ as SingleMonthCalendar;
			SelectedDate = singleMonthCalendar.SelectedDate;
		}
	}

	private ButtonItem _25A1()
	{
		ButtonItem buttonItem = new ButtonItem(__256E);
		buttonItem.GlobalItem = false;
		buttonItem.Text = _25A3();
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._ຳ = false;
		buttonItem.Click += _25A2;
		return buttonItem;
	}

	private void _25A2(object _٠, EventArgs ؿ)
	{
		DateTime selectedDate = (DisplayMonth = (TodayDateSet ? TodayDate : DateTime.Today));
		SelectedDate = selectedDate;
	}

	private string _25A3()
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
		if (__2571 != null)
		{
			__2571.Text = _25A3();
		}
		if (__2572 != null)
		{
			__2572.Text = _25A5();
		}
	}

	private ButtonItem _25A4()
	{
		ButtonItem buttonItem = new ButtonItem(__256F);
		buttonItem.GlobalItem = false;
		buttonItem.Text = _25A5();
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._ຳ = false;
		buttonItem.Click += _180F;
		return buttonItem;
	}

	private void _180F(object _٠, EventArgs ؿ)
	{
		SelectedDate = DateTime.MinValue;
	}

	private string _25A5()
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

	private void _25A6()
	{
		if (__2586 != __2571.Visible)
		{
			__2571.Visible = __2586;
		}
		if (__2587 != __2572.Visible)
		{
			__2572.Visible = __2587;
		}
		bool flag = __2587 | __2586;
		if (flag != __2570.Visible)
		{
			__2570.Visible = flag;
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
