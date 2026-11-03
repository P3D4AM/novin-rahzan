using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(DotNetBarManager), "MonthCalendarAdv.ico")]
[Designer("DevComponents.DotNetBar.Design.MonthCalendarAdvDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class MonthCalendarAdv : ItemControl
{
	private MonthCalendarItem _១;

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

	private Size __21D2 = Size.Empty;

	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	[Localizable(true)]
	public DateTime[] MonthlyMarkedDates
	{
		get
		{
			return _១.MonthlyMarkedDates;
		}
		set
		{
			_១.MonthlyMarkedDates = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	public DayOfWeek[] WeeklyMarkedDays
	{
		get
		{
			return _១.WeeklyMarkedDays;
		}
		set
		{
			_១.WeeklyMarkedDays = value;
		}
	}

	[Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
	[Localizable(true)]
	public DateTime[] AnnuallyMarkedDates
	{
		get
		{
			return _១.AnnuallyMarkedDates;
		}
		set
		{
			_១.AnnuallyMarkedDates = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which non-recurring dates are marked using Colors.DayMarker settings.")]
	public DateTime[] MarkedDates
	{
		get
		{
			return _១.MarkedDates;
		}
		set
		{
			_១.MarkedDates = value;
		}
	}

	[Description("Indicates the first month displayed on the control.")]
	public DateTime DisplayMonth
	{
		get
		{
			return _១.DisplayMonth;
		}
		set
		{
			_១.DisplayMonth = value;
		}
	}

	[Browsable(false)]
	public int DisplayedMonthCount => _១.DisplayedMonthCount;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets colors used by control.")]
	[Category("Appearance")]
	[NotifyParentProperty(true)]
	public MonthCalendarColors Colors => _១.Colors;

	[Description("Indicate size of each day item on the calendar.")]
	public Size DaySize
	{
		get
		{
			return _១.DaySize;
		}
		set
		{
			_១.DaySize = value;
			InvalidateAutoSize();
		}
	}

	[Description("Indicate size of date navigation buttons on the calendar. If you increase size of the navigation buttons change DaySize as well so everything fits.")]
	public Size NavigationButtonSize
	{
		get
		{
			return _១.NavigationButtonSize;
		}
		set
		{
			_១.NavigationButtonSize = value;
			InvalidateAutoSize();
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return _១.MinDate;
		}
		set
		{
			_១.MinDate = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return _១.MaxDate;
		}
		set
		{
			_១.MaxDate = value;
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.")]
	public ElementStyle CommandsBackgroundStyle => _១.CommandsBackgroundStyle;

	[Description("Indicates navigation container background style.")]
	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle NavigationBackgroundStyle => _១.NavigationBackgroundStyle;

	[Description("Indicates number of columns and rows of months displayed on control.")]
	public Size CalendarDimensions
	{
		get
		{
			return _១.CalendarDimensions;
		}
		set
		{
			_១.CalendarDimensions = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether weekend days can be selected.")]
	public bool WeekendDaysSelectable
	{
		get
		{
			return _១.WeekendDaysSelectable;
		}
		set
		{
			_១.WeekendDaysSelectable = value;
		}
	}

	[DefaultValue(CalendarWeekRule.FirstDay)]
	[Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
	public CalendarWeekRule WeekOfYearRule
	{
		get
		{
			return _១.WeekOfYearRule;
		}
		set
		{
			_១.WeekOfYearRule = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime TodayDate
	{
		get
		{
			return _១.TodayDate;
		}
		set
		{
			_១.TodayDate = value;
		}
	}

	[Browsable(false)]
	public bool TodayDateSet => _១.TodayDateSet;

	[DefaultValue(true)]
	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	public bool ShowTodayMarker
	{
		get
		{
			return _១.ShowTodayMarker;
		}
		set
		{
			_១.ShowTodayMarker = value;
		}
	}

	[Description("Indicates whether week of year is visible.")]
	[DefaultValue(false)]
	public bool ShowWeekNumbers
	{
		get
		{
			return _១.ShowWeekNumbers;
		}
		set
		{
			_១.ShowWeekNumbers = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of custom names for days displayed on calendar header.")]
	public string[] DayNames
	{
		get
		{
			return _១.DayNames;
		}
		set
		{
			_១.DayNames = value;
		}
	}

	[Description("Indicates first day of week displayed on the calendar.")]
	[DefaultValue(DayOfWeek.Sunday)]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return _១.FirstDayOfWeek;
		}
		set
		{
			_១.FirstDayOfWeek = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether control uses the two letter day names.")]
	public bool TwoLetterDayName
	{
		get
		{
			return _១.TwoLetterDayName;
		}
		set
		{
			_១.TwoLetterDayName = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectedDate
	{
		get
		{
			return _១.SelectedDate;
		}
		set
		{
			_១.SelectedDate = value;
		}
	}

	[Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
	[DefaultValue(false)]
	public bool MultiSelect
	{
		get
		{
			return _១.MultiSelect;
		}
		set
		{
			_១.MultiSelect = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ItemContainer BottomContainer => _១.BottomContainer;

	[DefaultValue(false)]
	[Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
	public bool TodayButtonVisible
	{
		get
		{
			return _១.TodayButtonVisible;
		}
		set
		{
			_១.TodayButtonVisible = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
	public bool ClearButtonVisible
	{
		get
		{
			return _១.ClearButtonVisible;
		}
		set
		{
			_១.ClearButtonVisible = value;
		}
	}

	[Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
	[DefaultValue(7)]
	public int MaxSelectionCount
	{
		get
		{
			return _១.MaxSelectionCount;
		}
		set
		{
			_១.MaxSelectionCount = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionStart
	{
		get
		{
			return _១.SelectionStart;
		}
		set
		{
			_១.SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionEnd
	{
		get
		{
			return _១.SelectionEnd;
		}
		set
		{
			_១.SelectionEnd = value;
		}
	}

	[Description("Indicates selected range of dates for a month calendar control. ")]
	[Bindable(true)]
	public SelectionRange SelectionRange
	{
		get
		{
			return _១.SelectionRange;
		}
		set
		{
			_១.SelectionRange = value;
		}
	}

	[Description("Indicates whether Year/Century selection is enabled when calendar is displaying single month.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool YearSelectionEnabled
	{
		get
		{
			return _១.YearSelectionEnabled;
		}
		set
		{
			_១.YearSelectionEnabled = value;
		}
	}

	[Description("Indicates whether month selector which is displayed when month label is clicked is using abbreviated month names instead of month number.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool MonthSelectorShortMonthNames
	{
		get
		{
			return _១.MonthSelectorShortMonthNames;
		}
		set
		{
			_១.MonthSelectorShortMonthNames = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether calendar is used in month selection mode which shows only month and year.")]
	[DefaultValue(false)]
	public bool MonthSelectionMode
	{
		get
		{
			return _១.MonthSelectionMode;
		}
		set
		{
			_១.MonthSelectionMode = value;
			InvalidateAutoSize();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return base.AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				base.AutoSize = value;
				_21DF();
			}
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
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

	public MonthCalendarAdv()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		_១ = new MonthCalendarItem();
		_១.GlobalItem = false;
		_១.ContainerControl = this;
		_១.Stretch = false;
		_១.Displayed = true;
		_១.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		_១.SetOwner(this);
		_១.MonthChanged += _2657;
		_១.MonthChanging += _2656;
		_១.PaintLabel += _2655;
		_១.LabelMouseDown += new MouseEventHandler(_2654);
		_១.LabelMouseUp += new MouseEventHandler(_2653);
		_១.LabelMouseEnter += _2652;
		_១.LabelMouseLeave += _2651;
		_១.LabelMouseMove += new MouseEventHandler(_2650);
		_១.LabelMouseHover += _264F;
		_១.DateChanged += _264E;
		_១.DateSelected += new DateRangeEventHandler(_264D);
		SetBaseItemContainer(_១);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		base.ScaleControl(factor, specified);
		_21DF();
	}

	private void _264D(object _٠, DateRangeEventArgs ؿ)
	{
		OnDateSelected(ؿ);
	}

	private void _264E(object _٠, EventArgs ؿ)
	{
		OnDateChanged(ؿ);
	}

	private void _264F(object _٠, EventArgs ؿ)
	{
		OnLabelMouseHover(_٠, ؿ);
	}

	private void _2650(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseMove(_٠, ؿ);
	}

	private void _2651(object _٠, EventArgs ؿ)
	{
		OnLabelMouseLeave(_٠, ؿ);
	}

	private void _2652(object _٠, EventArgs ؿ)
	{
		OnLabelMouseEnter(_٠, ؿ);
	}

	private void _2653(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseUp(_٠, ؿ);
	}

	private void _2654(object _٠, MouseEventArgs ؿ)
	{
		OnLabelMouseDown(_٠, ؿ);
	}

	private void _2655(object _٠, DayPaintEventArgs ؿ)
	{
		OnPaintLabel(_٠, ؿ);
	}

	private void _2656(object _٠, EventArgs ؿ)
	{
		OnMonthChanging(ؿ);
	}

	private void _2657(object _٠, EventArgs ؿ)
	{
		OnMonthChanged(ؿ);
	}

	public DayLabel GetDayAt(int x, int y)
	{
		return _១.GetDayAt(x, y);
	}

	public DayLabel GetDay(DateTime date)
	{
		return _១.GetDay(date);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMonthlyMarkedDates()
	{
		return _១.ShouldSerializeMonthlyMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMonthlyMarkedDates()
	{
		_១.ResetMonthlyMarkedDates();
	}

	public void RemoveAllMonthlyMarkedDates()
	{
		_១.RemoveAllMonthlyMarkedDates();
	}

	public void RemoveMonthlyMarkedDate(DateTime dt)
	{
		_១.RemoveMonthlyMarkedDate(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWeeklyMarkedDays()
	{
		return _១.ShouldSerializeWeeklyMarkedDays();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeeklyMarkedDays()
	{
		_១.ResetWeeklyMarkedDays();
	}

	public void RemoveAllWeeklyMarkedDays()
	{
		_១.RemoveAllWeeklyMarkedDays();
	}

	public void RemoveWeeklyMarkedDay(DayOfWeek dt)
	{
		_១.RemoveWeeklyMarkedDay(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAnnuallyMarkedDates()
	{
		return _១.ShouldSerializeAnnuallyMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAnnuallyMarkedDates()
	{
		_១.ResetAnnuallyMarkedDates();
	}

	public void RemoveAllAnnuallyMarkedDates()
	{
		_១.RemoveAllAnnuallyMarkedDates();
	}

	public void RemoveAnnuallyMarkedDate(DateTime dt)
	{
		_១.RemoveAnnuallyMarkedDate(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMarkedDates()
	{
		return _១.ShouldSerializeMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMarkedDates()
	{
		_១.ResetMarkedDates();
	}

	public void RemoveAllMarkedDates()
	{
		_១.RemoveAllMarkedDates();
	}

	public void RemoveMarkedDate(DateTime dt)
	{
		_១.RemoveMarkedDate(dt);
	}

	public void UpdateMarkedDates()
	{
		_១.UpdateMarkedDates();
	}

	internal bool _2658()
	{
		return ((Component)(object)this).DesignMode;
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

	protected virtual void OnMonthChanged(EventArgs e)
	{
		if (_2573 != null)
		{
			_2573(this, e);
		}
	}

	protected virtual void OnMonthChanging(EventArgs e)
	{
		if (_2574 != null)
		{
			_2574(this, e);
		}
	}

	protected virtual void OnPaintLabel(object label, DayPaintEventArgs e)
	{
		if (_2398 != null)
		{
			_2398(label, e);
		}
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
		return _១.ShouldSerializeDaySize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		_១.ResetDaySize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNavigationButtonSize()
	{
		return _១.ShouldSerializeNavigationButtonSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNavigationButtonSize()
	{
		_១.ResetNavigationButtonSize();
	}

	public bool ShouldSerializeMinDate()
	{
		return _១.ShouldSerializeMinDate();
	}

	public void ResetMinDate()
	{
		_១.ResetMinDate();
	}

	public bool ShouldSerializeMaxDate()
	{
		return _១.ShouldSerializeMaxDate();
	}

	public void ResetMaxDate()
	{
		_១.ResetMaxDate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCalendarDimensions()
	{
		_១.ResetCalendarDimensions();
		InvalidateAutoSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCalendarDimensions()
	{
		return _១.ShouldSerializeCalendarDimensions();
	}

	public void SetCalendarDimensions(int columns, int rows)
	{
		_១.SetCalendarDimensions(columns, rows);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectionRange()
	{
		return _១.ShouldSerializeSelectionRange();
	}

	public void SetSelectionRange(DateTime startDate, DateTime endDate)
	{
		_១.SetSelectionRange(startDate, endDate);
	}

	public void InvalidateAutoSize()
	{
		__21D2 = Size.Empty;
		_21DF();
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!__21D2.IsEmpty)
		{
			return __21D2;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		ElementStyle backgroundStyle = GetBackgroundStyle();
		_១.RecalcSize();
		__21D2 = _១.Size;
		if (backgroundStyle != null)
		{
			__21D2.Width += ᝈ.ᝍ(backgroundStyle);
			__21D2.Height += ᝈ.ᝏ(backgroundStyle);
		}
		return __21D2;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			width = preferredSize.Width;
			height = preferredSize.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
		base.OnHandleCreated(e);
	}
}
