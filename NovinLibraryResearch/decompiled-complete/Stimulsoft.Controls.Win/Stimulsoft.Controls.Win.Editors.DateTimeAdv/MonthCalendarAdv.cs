using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[ToolboxBitmap(typeof(DotNetBarManager), "MonthCalendarAdv.ico")]
[ToolboxItem(false)]
public class MonthCalendarAdv : ItemControl
{
	private MonthCalendarItem _MonthCalendar;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseMove;

	[CompilerGenerated]
	private DateRangeEventHandler m_DateSelected;

	private Size _PreferredSize = Size.Empty;

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	public DateTime[] MonthlyMarkedDates
	{
		get
		{
			return _MonthCalendar.MonthlyMarkedDates;
		}
		set
		{
			_MonthCalendar.MonthlyMarkedDates = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
	public DayOfWeek[] WeeklyMarkedDays
	{
		get
		{
			return _MonthCalendar.WeeklyMarkedDays;
		}
		set
		{
			_MonthCalendar.WeeklyMarkedDays = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
	public DateTime[] AnnuallyMarkedDates
	{
		get
		{
			return _MonthCalendar.AnnuallyMarkedDates;
		}
		set
		{
			_MonthCalendar.AnnuallyMarkedDates = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of DateTime objects that determines which non-recurring dates are marked using Colors.DayMarker settings.")]
	public DateTime[] MarkedDates
	{
		get
		{
			return _MonthCalendar.MarkedDates;
		}
		set
		{
			_MonthCalendar.MarkedDates = value;
		}
	}

	[Description("Indicates the first month displayed on the control.")]
	public DateTime DisplayMonth
	{
		get
		{
			return _MonthCalendar.DisplayMonth;
		}
		set
		{
			_MonthCalendar.DisplayMonth = value;
		}
	}

	[Browsable(false)]
	public int DisplayedMonthCount => _MonthCalendar.DisplayedMonthCount;

	[NotifyParentProperty(true)]
	[Category("Appearance")]
	[Description("Gets colors used by control.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public MonthCalendarColors Colors => _MonthCalendar.Colors;

	[Description("Indicate size of each day item on the calendar.")]
	public Size DaySize
	{
		get
		{
			return _MonthCalendar.DaySize;
		}
		set
		{
			_MonthCalendar.DaySize = value;
			InvalidateAutoSize();
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return _MonthCalendar.MinDate;
		}
		set
		{
			_MonthCalendar.MinDate = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return _MonthCalendar.MaxDate;
		}
		set
		{
			_MonthCalendar.MaxDate = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle CommandsBackgroundStyle => _MonthCalendar.CommandsBackgroundStyle;

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates navigation container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle NavigationBackgroundStyle => _MonthCalendar.NavigationBackgroundStyle;

	[Description("Indicates number of columns and rows of months displayed on control.")]
	public Size CalendarDimensions
	{
		get
		{
			return _MonthCalendar.CalendarDimensions;
		}
		set
		{
			_MonthCalendar.CalendarDimensions = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether weekend days can be selected.")]
	public bool WeekendDaysSelectable
	{
		get
		{
			return _MonthCalendar.WeekendDaysSelectable;
		}
		set
		{
			_MonthCalendar.WeekendDaysSelectable = value;
		}
	}

	[DefaultValue(CalendarWeekRule.FirstDay)]
	[Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
	public CalendarWeekRule WeekOfYearRule
	{
		get
		{
			return _MonthCalendar.WeekOfYearRule;
		}
		set
		{
			_MonthCalendar.WeekOfYearRule = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime TodayDate
	{
		get
		{
			return _MonthCalendar.TodayDate;
		}
		set
		{
			_MonthCalendar.TodayDate = value;
		}
	}

	[Browsable(false)]
	public bool TodayDateSet => _MonthCalendar.TodayDateSet;

	[DefaultValue(true)]
	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	public bool ShowTodayMarker
	{
		get
		{
			return _MonthCalendar.ShowTodayMarker;
		}
		set
		{
			_MonthCalendar.ShowTodayMarker = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether week of year is visible.")]
	public bool ShowWeekNumbers
	{
		get
		{
			return _MonthCalendar.ShowWeekNumbers;
		}
		set
		{
			_MonthCalendar.ShowWeekNumbers = value;
			InvalidateAutoSize();
		}
	}

	[DefaultValue(null)]
	[Description("Indicates array of custom names for days displayed on calendar header.")]
	[Localizable(true)]
	public string[] DayNames
	{
		get
		{
			return _MonthCalendar.DayNames;
		}
		set
		{
			_MonthCalendar.DayNames = value;
		}
	}

	[DefaultValue(DayOfWeek.Sunday)]
	[Description("Indicates first day of week displayed on the calendar.")]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return _MonthCalendar.FirstDayOfWeek;
		}
		set
		{
			_MonthCalendar.FirstDayOfWeek = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether control uses the two letter day names.")]
	public bool TwoLetterDayName
	{
		get
		{
			return _MonthCalendar.TwoLetterDayName;
		}
		set
		{
			_MonthCalendar.TwoLetterDayName = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDate
	{
		get
		{
			return _MonthCalendar.SelectedDate;
		}
		set
		{
			_MonthCalendar.SelectedDate = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
	public bool MultiSelect
	{
		get
		{
			return _MonthCalendar.MultiSelect;
		}
		set
		{
			_MonthCalendar.MultiSelect = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ItemContainer BottomContainer => _MonthCalendar.BottomContainer;

	[DefaultValue(false)]
	[Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
	public bool TodayButtonVisible
	{
		get
		{
			return _MonthCalendar.TodayButtonVisible;
		}
		set
		{
			_MonthCalendar.TodayButtonVisible = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
	public bool ClearButtonVisible
	{
		get
		{
			return _MonthCalendar.ClearButtonVisible;
		}
		set
		{
			_MonthCalendar.ClearButtonVisible = value;
		}
	}

	[DefaultValue(7)]
	[Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
	public int MaxSelectionCount
	{
		get
		{
			return _MonthCalendar.MaxSelectionCount;
		}
		set
		{
			_MonthCalendar.MaxSelectionCount = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionStart
	{
		get
		{
			return _MonthCalendar.SelectionStart;
		}
		set
		{
			_MonthCalendar.SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectionEnd
	{
		get
		{
			return _MonthCalendar.SelectionEnd;
		}
		set
		{
			_MonthCalendar.SelectionEnd = value;
		}
	}

	[Bindable(true)]
	[Description("Indicates selected range of dates for a month calendar control. ")]
	public SelectionRange SelectionRange
	{
		get
		{
			return _MonthCalendar.SelectionRange;
		}
		set
		{
			_MonthCalendar.SelectionRange = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Year/Century selection is enabled when calendar is displaying single month.")]
	public bool YearSelectionEnabled
	{
		get
		{
			return _MonthCalendar.YearSelectionEnabled;
		}
		set
		{
			_MonthCalendar.YearSelectionEnabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
				AdjustSize();
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

	public MonthCalendarAdv()
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		_MonthCalendar = new MonthCalendarItem();
		_MonthCalendar.GlobalItem = false;
		_MonthCalendar.ContainerControl = this;
		_MonthCalendar.Stretch = false;
		_MonthCalendar.Displayed = true;
		_MonthCalendar.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.Style = eDotNetBarStyle.StyleManagerControlled;
		_MonthCalendar.SetOwner(this);
		_MonthCalendar.MonthChanged += MonthCalendar_MonthChanged;
		_MonthCalendar.MonthChanging += MonthCalendar_MonthChanging;
		_MonthCalendar.PaintLabel += MonthCalendar_PaintLabel;
		_MonthCalendar.LabelMouseDown += new MouseEventHandler(MonthCalendar_LabelMouseDown);
		_MonthCalendar.LabelMouseUp += new MouseEventHandler(MonthCalendar_LabelMouseUp);
		_MonthCalendar.LabelMouseEnter += MonthCalendar_LabelMouseEnter;
		_MonthCalendar.LabelMouseLeave += MonthCalendar_LabelMouseLeave;
		_MonthCalendar.LabelMouseMove += new MouseEventHandler(MonthCalendar_LabelMouseMove);
		_MonthCalendar.LabelMouseHover += MonthCalendar_LabelMouseHover;
		_MonthCalendar.DateChanged += MonthCalendar_DateChanged;
		_MonthCalendar.DateSelected += new DateRangeEventHandler(MonthCalendar_DateSelected);
		SetBaseItemContainer(_MonthCalendar);
	}

	private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
	{
		OnDateSelected(e);
	}

	private void MonthCalendar_DateChanged(object sender, EventArgs e)
	{
		OnDateChanged(e);
	}

	private void MonthCalendar_LabelMouseHover(object sender, EventArgs e)
	{
		OnLabelMouseHover(sender, e);
	}

	private void MonthCalendar_LabelMouseMove(object sender, MouseEventArgs e)
	{
		OnLabelMouseMove(sender, e);
	}

	private void MonthCalendar_LabelMouseLeave(object sender, EventArgs e)
	{
		OnLabelMouseLeave(sender, e);
	}

	private void MonthCalendar_LabelMouseEnter(object sender, EventArgs e)
	{
		OnLabelMouseEnter(sender, e);
	}

	private void MonthCalendar_LabelMouseUp(object sender, MouseEventArgs e)
	{
		OnLabelMouseUp(sender, e);
	}

	private void MonthCalendar_LabelMouseDown(object sender, MouseEventArgs e)
	{
		OnLabelMouseDown(sender, e);
	}

	private void MonthCalendar_PaintLabel(object sender, DayPaintEventArgs e)
	{
		OnPaintLabel(sender, e);
	}

	private void MonthCalendar_MonthChanging(object sender, EventArgs e)
	{
		OnMonthChanging(e);
	}

	private void MonthCalendar_MonthChanged(object sender, EventArgs e)
	{
		OnMonthChanged(e);
	}

	public DayLabel GetDayAt(int x, int y)
	{
		return _MonthCalendar.GetDayAt(x, y);
	}

	public DayLabel GetDay(DateTime date)
	{
		return _MonthCalendar.GetDay(date);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMonthlyMarkedDates()
	{
		return _MonthCalendar.ShouldSerializeMonthlyMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMonthlyMarkedDates()
	{
		_MonthCalendar.ResetMonthlyMarkedDates();
	}

	public void RemoveAllMonthlyMarkedDates()
	{
		_MonthCalendar.RemoveAllMonthlyMarkedDates();
	}

	public void RemoveMonthlyMarkedDate(DateTime dt)
	{
		_MonthCalendar.RemoveMonthlyMarkedDate(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWeeklyMarkedDays()
	{
		return _MonthCalendar.ShouldSerializeWeeklyMarkedDays();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWeeklyMarkedDays()
	{
		_MonthCalendar.ResetWeeklyMarkedDays();
	}

	public void RemoveAllWeeklyMarkedDays()
	{
		_MonthCalendar.RemoveAllWeeklyMarkedDays();
	}

	public void RemoveWeeklyMarkedDay(DayOfWeek dt)
	{
		_MonthCalendar.RemoveWeeklyMarkedDay(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeAnnuallyMarkedDates()
	{
		return _MonthCalendar.ShouldSerializeAnnuallyMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetAnnuallyMarkedDates()
	{
		_MonthCalendar.ResetAnnuallyMarkedDates();
	}

	public void RemoveAllAnnuallyMarkedDates()
	{
		_MonthCalendar.RemoveAllAnnuallyMarkedDates();
	}

	public void RemoveAnnuallyMarkedDate(DateTime dt)
	{
		_MonthCalendar.RemoveAnnuallyMarkedDate(dt);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMarkedDates()
	{
		return _MonthCalendar.ShouldSerializeMarkedDates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMarkedDates()
	{
		_MonthCalendar.ResetMarkedDates();
	}

	public void RemoveAllMarkedDates()
	{
		_MonthCalendar.RemoveAllMarkedDates();
	}

	public void RemoveMarkedDate(DateTime dt)
	{
		_MonthCalendar.RemoveMarkedDate(dt);
	}

	public void UpdateMarkedDates()
	{
		_MonthCalendar.UpdateMarkedDates();
	}

	internal bool GetDesignModeInternal()
	{
		return ((Component)(object)this).DesignMode;
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

	protected virtual void OnMonthChanged(EventArgs e)
	{
		if (this.MonthChanged != null)
		{
			this.MonthChanged(this, e);
		}
	}

	protected virtual void OnMonthChanging(EventArgs e)
	{
		if (this.MonthChanging != null)
		{
			this.MonthChanging(this, e);
		}
	}

	protected virtual void OnPaintLabel(object label, DayPaintEventArgs e)
	{
		if (this.PaintLabel != null)
		{
			this.PaintLabel(label, e);
		}
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
		return _MonthCalendar.ShouldSerializeDaySize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		_MonthCalendar.ResetDaySize();
	}

	public bool ShouldSerializeMinDate()
	{
		return _MonthCalendar.ShouldSerializeMinDate();
	}

	public void ResetMinDate()
	{
		_MonthCalendar.ResetMinDate();
	}

	public bool ShouldSerializeMaxDate()
	{
		return _MonthCalendar.ShouldSerializeMaxDate();
	}

	public void ResetMaxDate()
	{
		_MonthCalendar.ResetMaxDate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCalendarDimensions()
	{
		_MonthCalendar.ResetCalendarDimensions();
		InvalidateAutoSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCalendarDimensions()
	{
		return _MonthCalendar.ShouldSerializeCalendarDimensions();
	}

	public void SetCalendarDimensions(int columns, int rows)
	{
		_MonthCalendar.SetCalendarDimensions(columns, rows);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectionRange()
	{
		return _MonthCalendar.ShouldSerializeSelectionRange();
	}

	public void SetSelectionRange(DateTime startDate, DateTime endDate)
	{
		_MonthCalendar.SetSelectionRange(startDate, endDate);
	}

	public void InvalidateAutoSize()
	{
		_PreferredSize = Size.Empty;
		AdjustSize();
	}

	private void AdjustSize()
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
		if (!_PreferredSize.IsEmpty)
		{
			return _PreferredSize;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		ElementStyle backgroundStyle = GetBackgroundStyle();
		_MonthCalendar.RecalcSize();
		_PreferredSize = _MonthCalendar.Size;
		if (backgroundStyle != null)
		{
			_PreferredSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle);
			_PreferredSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
		}
		return _PreferredSize;
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
			AdjustSize();
		}
		base.OnHandleCreated(e);
	}
}
