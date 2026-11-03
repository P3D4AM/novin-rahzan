using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors.DateTimeAdv;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.ItemContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class SingleMonthCalendar : CalendarBase
{
	private const string _25BE = "navigationContainer";

	private const string _25BF = "monthLabel";

	private const string _25C0 = "yearLabel";

	private const string _25C1 = "decreaseMonth";

	private const string _25C2 = "increaseMonth";

	private const string _25C3 = "decreaseYear";

	private const string _25C4 = "increaseYear";

	internal static readonly Size __25C5 = new Size(24, 15);

	internal static readonly Size __25C6 = new Size(13, 18);

	private _2611 __25C7;

	private _262D __25C8;

	private _2613 __25C9;

	private _262F __25CA;

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

	private bool __258B;

	private bool __2589 = true;

	private bool __258A = true;

	private _2639 __25CB;

	private DateTime __2582 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

	private DayOfWeek __25CC;

	internal bool __25CD = true;

	internal bool __25CE = true;

	internal bool __25CF = true;

	private bool __25D0;

	private CalendarWeekRule __25D1;

	private Size __25D2 = __25C5;

	private Size __25D3 = __25C6;

	private MonthCalendarColors _ቿ = new MonthCalendarColors();

	private bool __05EB;

	private bool __25D4 = true;

	private DateTime __2585 = DateTime.MinValue;

	private bool __25D5 = true;

	private eLabelPartAlignment __25D6 = eLabelPartAlignment.MiddleCenter;

	private bool __25D7 = true;

	private string[] __25D8;

	private bool __25D9 = true;

	private bool __25DA = true;

	private DateTime __25DB = DateTime.MinValue;

	private DateTime __253D = DateTimeGroup.MinDateTime;

	private DateTime __253E = DateTimeGroup.MaxDateTime;

	private int __25DC = 1;

	private bool __2588 = true;

	[Category("Behavior")]
	[Description("Indicates whether calendar is used in month selection mode which shows only month and year.")]
	[DefaultValue(false)]
	public bool MonthSelectionMode
	{
		get
		{
			return __258B;
		}
		set
		{
			if (value != __258B)
			{
				bool oldValue = __258B;
				__258B = value;
				OnMonthSelectionModeChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Year/Century selection is enabled.")]
	public bool YearSelectionEnabled
	{
		get
		{
			return __2589;
		}
		set
		{
			if (__2589 != value)
			{
				__2589 = value;
				ItemContainer itemContainer = _25FA();
				if (itemContainer != null && itemContainer.SubItems["yearLabel"] is LabelItem labelItem)
				{
					labelItem.Cursor = (__2589 ? Cursors.Hand : null);
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
			__258A = value;
			if (__25CA != null)
			{
				__25CA._2632 = __258A;
			}
		}
	}

	[Description("")]
	public DateTime DisplayMonth
	{
		get
		{
			return __2582;
		}
		set
		{
			if (value < MinDate)
			{
				value = MinDate;
			}
			else if (value > MaxDate)
			{
				value = MaxDate;
			}
			_25E9(value.Month, value.Year, eEventSource.Code);
		}
	}

	[DefaultValue(DayOfWeek.Sunday)]
	[Description("Indicates first day of week displayed on the calendar.")]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return __25CC;
		}
		set
		{
			if (__25CC != value)
			{
				__25CC = value;
				_25E9(__2582.Month, __2582.Year, eEventSource.Code);
			}
		}
	}

	[DefaultValue(true)]
	public bool TrailingDaysVisible
	{
		get
		{
			return __25CD;
		}
		set
		{
			if (__25CD != value)
			{
				__25CD = value;
				_25FD();
			}
		}
	}

	internal bool _25BB
	{
		get
		{
			return __25CE;
		}
		set
		{
			if (__25CE != value)
			{
				__25CE = value;
				_25FD();
			}
		}
	}

	internal bool _25BC
	{
		get
		{
			return __25CF;
		}
		set
		{
			if (__25CF != value)
			{
				__25CF = value;
				_25FD();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether week of year is visible.")]
	public bool ShowWeekNumbers
	{
		get
		{
			return __25D0;
		}
		set
		{
			if (__25D0 != value)
			{
				__25D0 = value;
				_25FE();
				NeedRecalcSize = true;
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(CalendarWeekRule.FirstDay)]
	[Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
	public CalendarWeekRule WeekOfYearRule
	{
		get
		{
			return __25D1;
		}
		set
		{
			__25D1 = value;
			if (__25D0)
			{
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Description("Indicate size of each day item on the calendar.")]
	public Size DaySize
	{
		get
		{
			return __25D2;
		}
		set
		{
			__25D2 = value;
			_25FF();
		}
	}

	[Description("Indicate size of date navigation buttons on the calendar. If you increase size of the navigation buttons change DaySize as well so everything fits.")]
	public Size NavigationButtonSize
	{
		get
		{
			return __25D3;
		}
		set
		{
			__25D3 = value;
			_2600();
		}
	}

	[NotifyParentProperty(true)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets colors used by control.")]
	public MonthCalendarColors Colors => _ቿ;

	[Description("Indicates whether multiple days can be selected by clicking each day.")]
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
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether selection of dates using mouse is enabled.")]
	public bool MouseSelectionEnabled
	{
		get
		{
			return __25D4;
		}
		set
		{
			if (__25D4 != value)
			{
				__25D4 = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectedDate
	{
		get
		{
			return __2585;
		}
		set
		{
			__2585 = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is DayLabel dayLabel && !(dayLabel.Date == DateTime.MinValue))
				{
					if (dayLabel.Date == __2585.Date)
					{
						dayLabel.IsSelected = true;
					}
					else
					{
						dayLabel.IsSelected = false;
					}
				}
			}
			if (__258B)
			{
				if (value != DateTime.MinValue && value != DateTime.MaxValue)
				{
					__25CA._2631 = __2585.Month;
					__25C9._2617 = __2585.Year;
				}
				else
				{
					__25CA.ᮄ = true;
					__25C9._2617 = 0;
				}
			}
			OnSelectedDateChanged(new EventArgs());
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether weekend days can be selected.")]
	public bool WeekendDaysSelectable
	{
		get
		{
			return __25D5;
		}
		set
		{
			if (__25D5 != value)
			{
				__25D5 = value;
				_25E9(__2582.Month, __2582.Year, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
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

	[Description("Indicates default text alignment for the DayLabel items representing calendar days.")]
	[DefaultValue(eLabelPartAlignment.MiddleCenter)]
	public eLabelPartAlignment DefaultDayLabelTextAlign
	{
		get
		{
			return __25D6;
		}
		set
		{
			if (__25D6 != value)
			{
				__25D6 = value;
				_25E9(__2582.Month, __2582.Year, eEventSource.Code);
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
			return __25D7;
		}
		set
		{
			if (__25D7 != value)
			{
				__25D7 = value;
				_25E9(__2582.Month, __2582.Year, eEventSource.Code);
				Refresh();
			}
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates array of custom names for days displayed on calendar header.")]
	public string[] DayNames
	{
		get
		{
			return __25D8;
		}
		set
		{
			if (value != null && value.Length != 7)
			{
				throw new ArgumentException("DayNames must have exactly 7 items in collection.");
			}
			__25D8 = value;
		}
	}

	[Description("Indicates whether header navigation buttons for month and year are visible.")]
	[DefaultValue(true)]
	public bool HeaderNavigationEnabled
	{
		get
		{
			return __25D9;
		}
		set
		{
			if (__25D9 != value)
			{
				__25D9 = value;
				_260F();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Description("Indicates navigation container background style.")]
	[Browsable(true)]
	public ElementStyle NavigationBackgroundStyle => _25FA().BackgroundStyle;

	[DefaultValue(true)]
	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	public bool ShowTodayMarker
	{
		get
		{
			return __25DA;
		}
		set
		{
			if (__25DA != value)
			{
				__25DA = value;
				_2610();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime TodayDate
	{
		get
		{
			return __25DB;
		}
		set
		{
			value = value.Date;
			if (__25DB != value)
			{
				__25DB = value;
				if (__25DA)
				{
					_2610();
				}
			}
		}
	}

	[Browsable(false)]
	public bool TodayDateSet => __25DB != DateTime.MinValue;

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return __253D;
		}
		set
		{
			__253D = value;
			_25EB();
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return __253E;
		}
		set
		{
			__253E = value;
			_25EB();
		}
	}

	internal int _25BD
	{
		get
		{
			return __25DC;
		}
		set
		{
			if (__25DC != value)
			{
				__25DC = value;
				_25EB();
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

	public SingleMonthCalendar()
	{
		__25CC = DateTimeInput.GetActiveCulture().DateTimeFormat.FirstDayOfWeek;
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		MouseUpNotification = true;
		AccessibleRole = (AccessibleRole)20;
		_ቿ.ކ = this;
		for (int i = 0; i < 49; i++)
		{
			DayLabel item = new DayLabel
			{
				Visible = false
			};
			SubItems.Add(item);
		}
		Size fixedSize = __25C6;
		ItemContainer itemContainer = new ItemContainer
		{
			Name = "navigationContainer",
			AutoCollapseOnClick = false,
			MinimumSize = new Size(0, fixedSize.Height + 3),
			LayoutOrientation = eOrientation.Horizontal,
			HorizontalItemAlignment = eHorizontalItemsAlignment.Center,
			VerticalItemAlignment = eVerticalItemsAlignment.Middle,
			ItemSpacing = 2
		};
		ButtonItem buttonItem = new ButtonItem("decreaseMonth");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += _260A;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		LabelItem labelItem = new LabelItem("monthLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Click += _25E4;
		labelItem.Cursor = Cursors.Hand;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("increaseMonth");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += _260C;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("decreaseYear");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += _260D;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		labelItem = new LabelItem("yearLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Click += _25DF;
		labelItem.Cursor = Cursors.Hand;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("increaseYear");
		buttonItem.GlobalItem = false;
		buttonItem._ຳ = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += _260E;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		SubItems.Add(itemContainer);
	}

	protected virtual void OnMonthSelectionModeChanged(bool oldValue, bool newValue)
	{
		if (newValue)
		{
			__25CA = new _262F();
			__25CA.FixedSize = new Size(162, 140);
			if (__2585 != DateTime.MinValue)
			{
				__25CA._2631 = SelectedDate.Month;
			}
			__25CA._2630 += _25DE;
			__25CA._2632 = __258A;
			SubItems.Add(__25CA);
			__25C9 = new _2613();
			__25C9.FixedSize = new Size(162, 140);
			if (__2585 != DateTime.MinValue)
			{
				__25C9._2617 = SelectedDate.Year;
			}
			__25C9._2614 += _25DD;
			SubItems.Add(__25C9);
		}
		else
		{
			SubItems.Remove(__25CA);
			SubItems.Remove(__25C9);
			__25CA.Dispose();
			__25CA = null;
			__25C9.Dispose();
			__25C9 = null;
			DisplayMonth = __2582;
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	internal void _258C()
	{
		if (__25C9 != null)
		{
			__25C9._2627();
		}
	}

	private void _25DD(object _٠, EventArgs ؿ)
	{
		if (__2585 != DateTime.MinValue)
		{
			DateTime selectedDate = (DisplayMonth = new DateTime(__25C9._2617, __2585.Month, __2585.Day, __2585.Hour, __2585.Minute, __2585.Second, __2585.Kind));
			SelectedDate = selectedDate;
		}
		else if (!__25CA.ᮄ)
		{
			DateTime selectedDate2 = (DisplayMonth = new DateTime(__25C9._2617, __25CA._2631, 1));
			SelectedDate = selectedDate2;
		}
	}

	private void _25DE(object _٠, EventArgs ؿ)
	{
		if (__2585 != DateTime.MinValue)
		{
			int day = Math.Min(__2585.Day, DateTimeInput.GetActiveCulture().Calendar.GetDaysInMonth(__2585.Year, __25CA._2631));
			DateTime selectedDate = (DisplayMonth = new DateTime(__2585.Year, __25CA._2631, day, __2585.Hour, __2585.Minute, __2585.Second, __2585.Kind));
			SelectedDate = selectedDate;
		}
		else if (__25C9._2617 > 0)
		{
			DateTime selectedDate2 = (DisplayMonth = new DateTime(__25C9._2617, __25CA._2631, 1));
			SelectedDate = selectedDate2;
		}
	}

	private void _25DF(object _٠, EventArgs ؿ)
	{
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		if (__25C7 == null && __2589 && __25CB == _2639.ᘶ && (!(Parent is MonthCalendarItem) || (((MonthCalendarItem)Parent).CalendarDimensions.Width <= 1 && ((MonthCalendarItem)Parent).CalendarDimensions.Height <= 1)))
		{
			__25CB = _2639._263A;
			__25C7 = new _2611();
			((Control)__25C7).Size = new Size(8, 8);
			((Control)__25C7).Location = new Point(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2);
			((Control)__25C7).Visible = true;
			__25C7.Style = eDotNetBarStyle.StyleManagerControlled;
			if (BarFunctions.IsOffice2007Style(Style))
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
				__25C7.BackgroundStyle.BackColor = elementStyle.BackColor;
				__25C7.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
			}
			else
			{
				__25C7.BackgroundStyle.BackColor = SystemColors.Window;
			}
			ItemContainer itemContainer = _25FA();
			LabelItem labelItem = itemContainer.SubItems["monthLabel"] as LabelItem;
			if (!StyleManager.IsMetro(EffectiveStyle))
			{
				__25C7._2612._2615 = labelItem.ForeColor;
			}
			Control val = (Control)ContainerControl;
			val.Controls.Add((Control)(object)__25C7);
			__25C7._2612._2616 = DisplayMonth.Year / 10 * 10;
			__25C7._2612._2617 = DisplayMonth.Year;
			Rectangle bounds = Bounds;
			__25C7._2612._2614 += _25E1;
			_20EE obj = new _20EE(new ခ(__25C7, "Bounds", bounds), AnimationEasing.EaseInOutQuad, 200);
			obj.AutoDispose = true;
			obj.AnimationCompleted += _25E0;
			obj.Start();
		}
	}

	private void _25E0(object _٠, EventArgs ؿ)
	{
		__25CB = _2639.ᘶ;
	}

	private void _25E1(object _٠, EventArgs ؿ)
	{
		if (__25CB == _2639.ᘶ)
		{
			_25E2(_25E3: true);
			DateTime dateTime = new DateTime(__25C7._2612._2617, DisplayMonth.Month, DisplayMonth.Day);
			if (dateTime < MinDate)
			{
				dateTime = MinDate;
			}
			else if (dateTime > MaxDate)
			{
				dateTime = MaxDate;
			}
			_25E9(dateTime.Month, dateTime.Year, eEventSource.Mouse);
		}
	}

	private void _25E2(bool _25E3)
	{
		if (__25CB == _2639._263B)
		{
			return;
		}
		if (_25E3)
		{
			__25CB = _2639._263B;
			_20EE obj = new _20EE(new ခ(to: new Rectangle(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2, 8, 8), target: __25C7, targetPropertyName: "Bounds"), AnimationEasing.EaseInOutQuad, 200);
			obj.AnimationCompleted += ચ;
			obj.Start();
			return;
		}
		if (((Control)__25C7).Parent != null)
		{
			((Control)__25C7).Parent.Controls.Remove((Control)(object)__25C7);
		}
		__25C7._2612._2614 -= _25E1;
		((Component)(object)__25C7).Dispose();
		__25C7 = null;
	}

	private void ચ(object _٠, EventArgs ؿ)
	{
		_20EE obj = (_20EE)_٠;
		obj.Dispose();
		if (((Control)__25C7).Parent != null)
		{
			((Control)__25C7).Parent.Controls.Remove((Control)(object)__25C7);
		}
		__25C7._2612._2614 -= _25E1;
		((Component)(object)__25C7).Dispose();
		__25C7 = null;
		__25CB = _2639.ᘶ;
	}

	private void _25E4(object _٠, EventArgs ؿ)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		if (__25C8 == null && __25CB == _2639.ᘶ && (!(Parent is MonthCalendarItem) || (((MonthCalendarItem)Parent).CalendarDimensions.Width <= 1 && ((MonthCalendarItem)Parent).CalendarDimensions.Height <= 1)) && HeaderNavigationEnabled)
		{
			__25CB = _2639._263C;
			__25C8 = new _262D();
			((Control)__25C8).Size = new Size(8, 8);
			((Control)__25C8).Location = new Point(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2);
			__25C8._262E._2632 = __258A;
			((Control)__25C8).Visible = true;
			__25C8.Style = eDotNetBarStyle.StyleManagerControlled;
			if (BarFunctions.IsOffice2007Style(Style))
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
				__25C8.BackgroundStyle.BackColor = elementStyle.BackColor;
				__25C8.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
			}
			else
			{
				__25C8.BackgroundStyle.BackColor = SystemColors.Window;
			}
			ItemContainer itemContainer = _25FA();
			LabelItem labelItem = itemContainer.SubItems["monthLabel"] as LabelItem;
			if (!StyleManager.IsMetro(EffectiveStyle))
			{
				__25C8._262E._2615 = labelItem.ForeColor;
			}
			Control val = (Control)ContainerControl;
			val.Controls.Add((Control)(object)__25C8);
			__25C8._262E._2631 = DisplayMonth.Month;
			Rectangle bounds = Bounds;
			__25C8._262E._2630 += _25E5;
			_20EE obj = new _20EE(new ခ(__25C8, "Bounds", bounds), AnimationEasing.EaseInOutQuad, 200);
			obj.AutoDispose = true;
			obj.AnimationCompleted += _25E0;
			obj.Start();
		}
	}

	private void _25E5(object _٠, EventArgs ؿ)
	{
		if (__25CB == _2639.ᘶ)
		{
			_25E6(_25E3: true);
			DateTime dateTime = new DateTime(DisplayMonth.Year, __25C8._262E._2631, DisplayMonth.Day);
			if (dateTime < MinDate)
			{
				dateTime = MinDate;
			}
			else if (dateTime > MaxDate)
			{
				dateTime = MaxDate;
			}
			_25E9(dateTime.Month, dateTime.Year, eEventSource.Mouse);
		}
	}

	private void _25E6(bool _25E3)
	{
		if (__25CB == _2639._263D)
		{
			return;
		}
		if (_25E3)
		{
			__25CB = _2639._263D;
			_20EE obj = new _20EE(new ခ(to: new Rectangle(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2, 8, 8), target: __25C8, targetPropertyName: "Bounds"), AnimationEasing.EaseInOutQuad, 200);
			obj.AnimationCompleted += _25E7;
			obj.Start();
			return;
		}
		if (((Control)__25C8).Parent != null)
		{
			((Control)__25C8).Parent.Controls.Remove((Control)(object)__25C8);
		}
		__25C8._262E._2630 -= _25E5;
		((Component)(object)__25C8).Dispose();
		__25C8 = null;
	}

	private void _25E7(object _٠, EventArgs ؿ)
	{
		_20EE obj = (_20EE)_٠;
		obj.Dispose();
		if (((Control)__25C8).Parent != null)
		{
			((Control)__25C8).Parent.Controls.Remove((Control)(object)__25C8);
		}
		__25C8._262E._2630 -= _25E5;
		((Component)(object)__25C8).Dispose();
		__25C8 = null;
		__25CB = _2639.ᘶ;
	}

	private void _25E8(object _٠, EventArgs ؿ)
	{
		if (_٠ is ButtonItem buttonItem)
		{
			int _25EA = int.Parse(buttonItem.Name);
			((PopupItem)buttonItem.Parent).Expanded = false;
			_25E9(_25EA, DisplayMonth.Year, eEventSource.Mouse);
		}
	}

	public override void RecalcSize()
	{
		Point location = m_Rect.Location;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(base.BackgroundStyle, out ല);
		location.X += ᝈ.ᝐ(elementStyle);
		location.Y += ᝈ._1753(elementStyle);
		if (__258B)
		{
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Displayed = false;
			}
			__25CA.LeftInternal = location.X;
			__25CA.TopInternal = location.Y + 22;
			__25CA.RecalcSize();
			__25CA.Displayed = true;
			__25C9.LeftInternal = location.X + __25CA.WidthInternal;
			__25C9.TopInternal = location.Y;
			__25C9.RecalcSize();
			__25C9.Displayed = true;
			m_Rect.Width = __25CA.WidthInternal + __25C9.WidthInternal;
			m_Rect.Height = Math.Max(__25CA.HeightInternal, __25C9.HeightInternal);
			m_Rect.Width += ᝈ.ᝍ(elementStyle);
			m_Rect.Height += ᝈ.ᝏ(elementStyle);
		}
		else
		{
			int num = 49;
			int num2 = 0;
			int num3 = 0;
			int num4 = 2;
			Point location2 = location;
			Point point = location;
			Size size = Dpi.Size(__25D2);
			if (__25D0)
			{
				point.X += size.Width;
			}
			ItemContainer itemContainer = _25FA();
			itemContainer.LeftInternal = location.X;
			itemContainer.TopInternal = location.Y;
			itemContainer.WidthInternal = size.Width * (__25D0 ? 8 : 7);
			itemContainer.RecalcSize();
			itemContainer.WidthInternal = size.Width * (__25D0 ? 8 : 7);
			location2.Y += itemContainer.HeightInternal + 1;
			point.Y += itemContainer.HeightInternal + 1;
			Point location3 = point;
			int num5 = 0;
			for (int i = 0; i < num; i++)
			{
				BaseItem baseItem2 = SubItems[i];
				if (baseItem2 is _2613)
				{
					continue;
				}
				if (baseItem2 is DayLabel dayLabel)
				{
					dayLabel.RecalcSize();
					dayLabel.Bounds = new Rectangle(location3, size);
				}
				else
				{
					baseItem2.RecalcSize();
				}
				if (baseItem2.Visible)
				{
					if (baseItem2.HeightInternal > num3)
					{
						num3 = baseItem2.HeightInternal;
					}
					location3.X += baseItem2.WidthInternal;
				}
				num2++;
				if (num2 != 7)
				{
					continue;
				}
				if (__25D0)
				{
					if (num5 > 0)
					{
						DayLabel dayLabel2 = SubItems[num + num5 - 1] as DayLabel;
						dayLabel2.RecalcSize();
						dayLabel2.Bounds = new Rectangle(location2, size);
						location2.Y += num3;
					}
					else
					{
						location2.Y += num3 + num4;
					}
				}
				num2 = 0;
				location3.Y += num3;
				if (num5 == 0)
				{
					location3.Y += num4;
				}
				location3.X = point.X;
				num3 = 0;
				num5++;
			}
			m_Rect.Width = size.Width * 7;
			m_Rect.Height = size.Height * 7 + num4;
			if (__25D0)
			{
				m_Rect.Width += size.Width;
			}
			m_Rect.Width += ᝈ.ᝍ(elementStyle);
			m_Rect.Height += ᝈ.ᝏ(elementStyle) + itemContainer.HeightInternal + 1;
		}
		if (ല)
		{
			elementStyle.Dispose();
		}
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		ItemContainer itemContainer = _25FA();
		LabelItem labelItem = itemContainer.SubItems["monthLabel"] as LabelItem;
		labelItem.ForeColor = p.Colors.ItemText;
		labelItem = itemContainer.SubItems["yearLabel"] as LabelItem;
		labelItem.ForeColor = p.Colors.ItemText;
		base.Paint(p);
	}

	public override BaseItem Copy()
	{
		SingleMonthCalendar singleMonthCalendar = new SingleMonthCalendar();
		CopyToItem(singleMonthCalendar);
		return singleMonthCalendar;
	}

	protected override void CopyToItem(BaseItem c)
	{
		SingleMonthCalendar c2 = c as SingleMonthCalendar;
		base.CopyToItem((BaseItem)c2);
	}

	private void _25E9(DateTime _᠖, eEventSource ᎂ)
	{
		_25E9(_᠖.Month, _᠖.Year, ᎂ);
	}

	private void _25E9(int _25EA, int _2544, eEventSource ᎂ)
	{
		DateTime dateTime = new DateTime(_2544, _25EA, 1);
		bool flag = dateTime != __2582;
		if (__258B)
		{
			__2582 = dateTime;
			if (flag)
			{
				OnMonthChanged(new EventSourceArgs(ᎂ));
			}
			return;
		}
		if (flag)
		{
			OnMonthChanging(new EventSourceArgs(ᎂ));
		}
		__2582 = dateTime;
		string[] abbreviatedDayNames = DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedDayNames;
		string[] shortestDayNames = DateTimeInput.GetActiveCulture().DateTimeFormat.ShortestDayNames;
		if (__25D8 != null)
		{
			abbreviatedDayNames = __25D8;
		}
		int num = (int)__25CC;
		int num2 = num;
		for (int i = 0; i < 7; i++)
		{
			DayLabel dayLabel = SubItems[i] as DayLabel;
			dayLabel.Text = (__25D7 ? _25FC(shortestDayNames[num2]) : abbreviatedDayNames[num2]);
			dayLabel.Visible = true;
			dayLabel.Displayed = true;
			dayLabel.TrackMouse = false;
			dayLabel.Selectable = false;
			dayLabel.IsDayLabel = true;
			_25F0(dayLabel);
			num2++;
			if (num2 > 6)
			{
				num2 = 0;
			}
		}
		bool[] array = _25F7();
		bool[] array2 = _25F9();
		bool[] array3 = _25F8(_25EA);
		bool[] array4 = _25F2(_25EA, _2544);
		DateTime dateTime2 = ((!__25DA) ? DateTime.MinValue : ((__25DB == DateTime.MinValue) ? DateTime.Today : __25DB));
		bool flag2 = dateTime2 != DateTime.MinValue;
		int num3 = (int)(dateTime.DayOfWeek - num);
		if (num3 < 0)
		{
			num3 = 7 + num3;
		}
		int num4 = 7;
		if (num3 > 0)
		{
			DateTime dateTime3 = dateTime;
			dateTime3 = dateTime3.AddDays(-num3);
			array3 = _25F8(dateTime3.Month);
			array4 = _25F2(dateTime3.Month, dateTime3.Year);
			for (int j = num4; j < num4 + num3; j++)
			{
				DayLabel dayLabel2 = SubItems[j] as DayLabel;
				dayLabel2.Date = dateTime3;
				dayLabel2.Visible = true;
				_25FB(dayLabel2);
				if (array2[(int)dateTime3.DayOfWeek])
				{
					_25F4(dayLabel2);
				}
				if (array[dateTime3.Day - 1])
				{
					_25F5(dayLabel2);
				}
				if (array3[dateTime3.Day - 1])
				{
					_25F3(dayLabel2);
				}
				if (array4[dateTime3.Day - 1])
				{
					_25F1(dayLabel2);
				}
				if (flag2 && dateTime2 == dateTime3)
				{
					dayLabel2.IsToday = true;
				}
				if (!__25D5 && DayLabel._25B8(dateTime3))
				{
					dayLabel2.Selectable = false;
					dayLabel2.TrackMouse = false;
				}
				dayLabel2.Enabled = _25ED(dateTime3);
				dayLabel2.Displayed = __25CD && __25CE;
				dayLabel2.IsTrailing = true;
				dateTime3 = dateTime3.AddDays(1.0);
			}
			num4 += num3;
		}
		array3 = _25F8(_25EA);
		array4 = _25F2(_25EA, _2544);
		bool flag3 = true;
		int num5 = 0;
		int num6 = num4 % 7;
		for (int k = num4; k < 49; k++)
		{
			DayLabel dayLabel3 = SubItems[k] as DayLabel;
			dayLabel3.Date = dateTime;
			_25FB(dayLabel3);
			if (dateTime.Month != _25EA && flag3)
			{
				array3 = _25F8(dateTime.Month);
				array4 = _25F2(dateTime.Month, dateTime.Year);
				flag3 = false;
			}
			if (array2[(int)dateTime.DayOfWeek])
			{
				_25F4(dayLabel3);
			}
			if (array[dateTime.Day - 1])
			{
				_25F5(dayLabel3);
			}
			if (array3[dateTime.Day - 1])
			{
				_25F3(dayLabel3);
			}
			if (array4[dateTime.Day - 1])
			{
				_25F1(dayLabel3);
			}
			if (flag2 && dateTime2 == dateTime)
			{
				dayLabel3.IsToday = true;
			}
			if (!__25D5 && DayLabel._25B8(dateTime))
			{
				dayLabel3.Selectable = false;
				dayLabel3.TrackMouse = false;
			}
			dayLabel3.Enabled = _25ED(dateTime);
			dayLabel3.Visible = true;
			if (dateTime.Month != _25EA)
			{
				dayLabel3.IsTrailing = true;
				dayLabel3.Displayed = __25CD && __25CF;
			}
			else
			{
				dayLabel3.Displayed = true;
				dayLabel3.IsTrailing = false;
				if (dayLabel3.Date == __2585)
				{
					dayLabel3.IsSelected = true;
				}
			}
			if (num6 == 6)
			{
				if (__25D0)
				{
					DayLabel dayLabel4 = SubItems[49 + num5] as DayLabel;
					dayLabel4.Text = DateTimeInput.GetActiveCulture().Calendar.GetWeekOfYear(dateTime, __25D1, __25CC).ToString();
					dayLabel4.Visible = true;
					dayLabel4.TrackMouse = false;
					dayLabel4.Selectable = false;
					if (dateTime.Month != _25EA && dateTime.AddDays(-num6).Month != _25EA)
					{
						dayLabel4.Displayed = __25CD;
					}
					else
					{
						dayLabel4.Displayed = true;
					}
					_25EE(dayLabel4);
				}
				num5++;
				num6 = -1;
			}
			dateTime = dateTime.AddDays(1.0);
			num6++;
		}
		ItemContainer itemContainer = _25FA();
		itemContainer.Visible = true;
		itemContainer.Displayed = true;
		LabelItem labelItem = itemContainer.SubItems["monthLabel"] as LabelItem;
		labelItem.Width = __25D2.Width * 3;
		labelItem.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames[_25EA - 1];
		labelItem = itemContainer.SubItems["yearLabel"] as LabelItem;
		labelItem.Text = _2544.ToString();
		labelItem.Width = (int)((double)__25D2.Width * 1.4);
		_25EB();
		OnAppearanceChanged();
		Refresh();
		if (flag)
		{
			OnMonthChanged(new EventSourceArgs(ᎂ));
			if (__25C7 != null)
			{
				_25E2(_25E3: false);
			}
			if (__25C8 != null)
			{
				_25E6(_25E3: false);
			}
		}
	}

	private void _25EB()
	{
		ItemContainer itemContainer = _25FA();
		DateTime dateTime = __2582.AddMonths(__25DC);
		if (dateTime > MaxDate)
		{
			itemContainer.SubItems["increaseMonth"].Enabled = false;
			itemContainer.SubItems["increaseYear"].Enabled = false;
		}
		else
		{
			itemContainer.SubItems["increaseMonth"].Enabled = true;
			dateTime = __2582.AddYears(1);
			itemContainer.SubItems["increaseYear"].Enabled = MaxDate > dateTime;
		}
		dateTime = __2582.AddMonths(-1);
		if (dateTime < _25EC(MinDate))
		{
			itemContainer.SubItems["decreaseMonth"].Enabled = false;
			itemContainer.SubItems["decreaseYear"].Enabled = false;
		}
		else
		{
			itemContainer.SubItems["decreaseMonth"].Enabled = true;
			dateTime = __2582.AddYears(-1);
			itemContainer.SubItems["decreaseYear"].Enabled = dateTime > MinDate;
		}
		dateTime = __2582;
		if (MinDate != DateTimeGroup.MinDateTime && dateTime.Year > MinDate.Year && dateTime.Month >= MinDate.Month)
		{
			itemContainer.SubItems["decreaseYear"].Enabled = true;
		}
	}

	private DateTime _25EC(DateTime _᠖)
	{
		if (_᠖.Day > 1)
		{
			_᠖ = _᠖.AddDays(-_᠖.Day);
		}
		return _᠖;
	}

	private bool _25ED(DateTime _0CC8)
	{
		if (_0CC8 > MaxDate || _0CC8 < MinDate)
		{
			return false;
		}
		return true;
	}

	private void _25EE(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.WeekOfYear.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.WeekOfYear);
		}
	}

	private void _25F0(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.DayLabel.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.DayLabel);
		}
	}

	private void _25F1(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.DayMarker.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.DayMarker);
		}
	}

	private bool[] _25F2(int _25EA, int _2544)
	{
		bool[] array = new bool[31];
		if (Parent is MonthCalendarItem)
		{
			MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
			DateTime[] markedDates = monthCalendarItem.MarkedDates;
			DateTime[] array2 = markedDates;
			for (int i = 0; i < array2.Length; i++)
			{
				DateTime dateTime = array2[i];
				if (dateTime.Month == _25EA && dateTime.Year == _2544)
				{
					array[dateTime.Day - 1] = true;
				}
			}
		}
		return array;
	}

	private void _25F3(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.AnnualMarker.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.AnnualMarker);
		}
	}

	internal MonthCalendarColors _2517()
	{
		MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
		MonthCalendarColors monthCalendarColors = null;
		if (monthCalendarItem != null)
		{
			return monthCalendarItem.Colors;
		}
		return _ቿ;
	}

	private void _25F4(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.WeeklyMarker.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.WeeklyMarker);
		}
	}

	private void _25F5(DayLabel _25EF)
	{
		MonthCalendarColors monthCalendarColors = _2517();
		if (monthCalendarColors != null && monthCalendarColors.MonthlyMarker.IsCustomized)
		{
			_25F6(_25EF, monthCalendarColors.MonthlyMarker);
		}
	}

	private void _25F6(DayLabel _25EF, DateAppearanceDescription _0BDE)
	{
		_25EF.BackgroundStyle.BackColor = _0BDE.BackColor;
		_25EF.BackgroundStyle.BackColor2 = _0BDE.BackColor2;
		_25EF.BackgroundStyle.BackColorGradientAngle = _0BDE.BackColorGradientAngle;
		if (!_0BDE.BorderColor.IsEmpty)
		{
			_25EF.BackgroundStyle.Border = eStyleBorderType.Solid;
			_25EF.BackgroundStyle.BorderColor = _0BDE.BorderColor;
			_25EF.BackgroundStyle.BorderWidth = 1;
		}
		_25EF.IsBold = _0BDE.IsBold;
		if (!_0BDE.TextColor.IsEmpty)
		{
			_25EF.TextColor = _0BDE.TextColor;
		}
		_25EF.Selectable = _0BDE.Selectable;
		if (!_0BDE.Selectable)
		{
			_25EF.TrackMouse = false;
		}
	}

	private bool[] _25F7()
	{
		bool[] array = new bool[31];
		if (Parent is MonthCalendarItem)
		{
			MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
			DateTime[] monthlyMarkedDates = monthCalendarItem.MonthlyMarkedDates;
			DateTime[] array2 = monthlyMarkedDates;
			foreach (DateTime dateTime in array2)
			{
				array[dateTime.Day - 1] = true;
			}
		}
		return array;
	}

	private bool[] _25F8(int _25EA)
	{
		bool[] array = new bool[31];
		if (Parent is MonthCalendarItem)
		{
			MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
			DateTime[] annuallyMarkedDates = monthCalendarItem.AnnuallyMarkedDates;
			DateTime[] array2 = annuallyMarkedDates;
			for (int i = 0; i < array2.Length; i++)
			{
				DateTime dateTime = array2[i];
				if (dateTime.Month == _25EA)
				{
					array[dateTime.Day - 1] = true;
				}
			}
		}
		return array;
	}

	private bool[] _25F9()
	{
		bool[] array = new bool[7];
		if (Parent is MonthCalendarItem)
		{
			MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
			DayOfWeek[] weeklyMarkedDays = monthCalendarItem.WeeklyMarkedDays;
			DayOfWeek[] array2 = weeklyMarkedDays;
			foreach (DayOfWeek dayOfWeek in array2)
			{
				array[(int)dayOfWeek] = true;
			}
		}
		return array;
	}

	private ItemContainer _25FA()
	{
		return SubItems["navigationContainer"] as ItemContainer;
	}

	private void _25FB(DayLabel _25EF)
	{
		_25EF.IsSelected = false;
		_25EF.Selectable = true;
		_25EF.TrackMouse = true;
		_25EF.IsToday = false;
		_25EF.IsBold = false;
		_25EF.Image = null;
		_25EF.ImageAlign = eLabelPartAlignment.MiddleRight;
		_25EF.TextAlign = __25D6;
		_25EF.TextColor = Color.Empty;
		_25EF.Tooltip = "";
		_25EF.ExpandOnMouseDown = false;
		_25EF.SubItems.Clear();
		_25EF.BackgroundStyle.Reset();
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

	private string _25FC(string ٮ)
	{
		if (ٮ.Length > 2)
		{
			return ٮ.Substring(0, 2);
		}
		return ٮ;
	}

	private void _25FD()
	{
		if (__258B)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is DayLabel { IsTrailing: not false } dayLabel)
			{
				bool flag = __25CD;
				flag = ((!(dayLabel.Date < DisplayMonth)) ? (flag & __25CF) : (flag & __25CE));
				dayLabel.Displayed = flag;
			}
		}
		Refresh();
	}

	private void _25FE()
	{
		if (__25D0)
		{
			for (int i = 0; i < 6; i++)
			{
				DayLabel dayLabel = new DayLabel();
				dayLabel.IsWeekOfYear = true;
				SubItems.Insert(49 + i, dayLabel);
			}
			return;
		}
		for (int j = 0; j < 7; j++)
		{
			if (SubItems[49] is DayLabel { IsWeekOfYear: not false })
			{
				SubItems.Remove(49);
			}
		}
	}

	private void _25FF()
	{
		ItemContainer itemContainer = _25FA();
		Size size = __25D2;
		if (itemContainer.MinimumSize.Height < size.Height)
		{
			itemContainer.MinimumSize = new Size(itemContainer.MinimumSize.Width, size.Height);
		}
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDaySize()
	{
		int width = __25D2.Width;
		Size size = __25C5;
		if (width == size.Width)
		{
			int height = __25D2.Height;
			Size size2 = __25C5;
			return height != size2.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		DaySize = __25C5;
	}

	protected override void ScaleItem(SizeF factor)
	{
		base.ScaleItem(factor);
		_2600();
	}

	private void _2600()
	{
		Size fixedSize = __25D3;
		ItemContainer itemContainer = _25FA();
		itemContainer.MinimumSize = new Size(0, fixedSize.Height + 3);
		ButtonItem buttonItem = (ButtonItem)itemContainer.SubItems["decreaseMonth"];
		buttonItem.FixedSize = fixedSize;
		buttonItem = (ButtonItem)itemContainer.SubItems["increaseMonth"];
		buttonItem.FixedSize = fixedSize;
		buttonItem = (ButtonItem)itemContainer.SubItems["decreaseYear"];
		buttonItem.FixedSize = fixedSize;
		buttonItem = (ButtonItem)itemContainer.SubItems["increaseYear"];
		buttonItem.FixedSize = fixedSize;
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNavigationButtonSize()
	{
		int width = __25D3.Width;
		Size size = __25C6;
		if (width == size.Width)
		{
			int height = __25D3.Height;
			Size size2 = __25C6;
			return height != size2.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNavigationButtonSize()
	{
		NavigationButtonSize = __25C6;
	}

	internal void _2601(DayLabel _2602, DayPaintEventArgs ؿ)
	{
		if (_2398 != null)
		{
			_2398(_2602, ؿ);
		}
	}

	internal void _2603(DayLabel _25EF)
	{
		if (!__25D4 || !_25EF.Selectable || !(_25EF.Date != DateTime.MinValue) || (!__25D5 && DayLabel._25B8(_25EF.Date)))
		{
			return;
		}
		if (MultiSelect)
		{
			if (__2585 == DateTime.MinValue)
			{
				__2585 = _25EF.Date;
			}
			_25EF.IsSelected = !_25EF.IsSelected;
		}
		else
		{
			SelectedDate = _25EF.Date;
		}
	}

	protected virtual void OnSelectedDateChanged(EventArgs e)
	{
		if (_257B != null)
		{
			_257B(this, e);
		}
	}

	public DayLabel GetDayLabel(DateTime date)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is DayLabel { Date: var date2 } dayLabel && date2.Date == date.Date && dayLabel.Visible && dayLabel.Displayed)
			{
				return dayLabel;
			}
		}
		return null;
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		base.OnSubItemExpandChange(item);
		if (item.Expanded)
		{
			Expanded = true;
		}
	}

	internal void _2604(DayLabel _٠, MouseEventArgs ؿ)
	{
		if (_2575 != null)
		{
			_2575.Invoke((object)_٠, ؿ);
		}
	}

	internal void _2605(DayLabel _٠, MouseEventArgs ؿ)
	{
		if (_2576 != null)
		{
			_2576.Invoke((object)_٠, ؿ);
		}
	}

	internal void _2606(DayLabel _٠, EventArgs ؿ)
	{
		if (_2577 != null)
		{
			_2577(_٠, ؿ);
		}
	}

	internal void _2607(DayLabel _٠, EventArgs ؿ)
	{
		if (_2578 != null)
		{
			_2578(_٠, ؿ);
		}
	}

	internal void _2608(DayLabel _٠, MouseEventArgs ؿ)
	{
		if (_2579 != null)
		{
			_2579.Invoke((object)_٠, ؿ);
		}
	}

	internal void _2609(DayLabel _٠, EventArgs ؿ)
	{
		if (_257A != null)
		{
			_257A(_٠, ؿ);
		}
	}

	private void _260A(object _٠, EventArgs ؿ)
	{
		_25E9(__2582.AddMonths(-1), eEventSource.Mouse);
		if (_260B())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private bool _260B()
	{
		BaseItem parent = Parent;
		while (parent != null)
		{
			parent = parent.Parent;
			if (parent != null && parent.Name == "sysPopupProvider" && parent.ContainerControl is DateTimeInput dateTimeInput)
			{
				return dateTimeInput.AutoSelectDate;
			}
		}
		return false;
	}

	private void _260C(object _٠, EventArgs ؿ)
	{
		_25E9(__2582.AddMonths(1), eEventSource.Mouse);
		if (_260B())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void _260D(object _٠, EventArgs ؿ)
	{
		_25E9(__2582.AddMonths(-12), eEventSource.Mouse);
		if (_260B())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void _260E(object _٠, EventArgs ؿ)
	{
		_25E9(__2582.AddMonths(12), eEventSource.Mouse);
		if (_260B())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void _260F()
	{
		ItemContainer itemContainer = _25FA();
		bool visible = __25D9;
		itemContainer.SubItems["decreaseMonth"].Visible = visible;
		itemContainer.SubItems["increaseMonth"].Visible = visible;
		itemContainer.SubItems["decreaseYear"].Visible = visible;
		itemContainer.SubItems["increaseYear"].Visible = visible;
		itemContainer.NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	private void _2610()
	{
		DateTime dateTime = ((!__25DA) ? DateTime.MinValue : ((__25DB == DateTime.MinValue) ? DateTime.Today : DateTime.MinValue));
		bool flag = dateTime == DateTime.MinValue;
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is DayLabel dayLabel)
			{
				if (flag)
				{
					dayLabel.IsToday = false;
				}
				else
				{
					dayLabel.IsToday = dayLabel.Date == dateTime;
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinDate()
	{
		return !MinDate.Equals(DateTimeGroup.MinDateTime);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMinDate()
	{
		MinDate = DateTimeGroup.MinDateTime;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMaxDate()
	{
		return !__253E.Equals(DateTimeGroup.MaxDateTime);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMaxDate()
	{
		MaxDate = DateTimeGroup.MaxDateTime;
	}

	protected virtual void OnDayClickAutoClosePopupChanged(bool oldValue, bool newValue)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is DayLabel)
			{
				subItem.AutoCollapseOnClick = newValue;
			}
		}
	}
}
