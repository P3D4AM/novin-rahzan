using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Events;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class SingleMonthCalendar : CalendarBase
{
	private const string NavigationContainerName = "navigationContainer";

	internal static readonly Size _DefaultDaySize = new Size(24, 15);

	private const string NavMonthLabel = "monthLabel";

	private const string NavYearLabel = "yearLabel";

	private const string NavDecreaseMonth = "decreaseMonth";

	private const string NavIncreaseMonth = "increaseMonth";

	private const string NavDecreaseYear = "decreaseYear";

	private const string NavIncreaseYear = "increaseYear";

	private const string MonthsPopupMenu = "monthsPopupMenu";

	private ButtonItem _MonthsPopupMenu;

	private YearSelectorControl _YearSelector;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseDown;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_LabelMouseMove;

	private bool _YearSelectionEnabled = true;

	private eCurrentSingleMonthCalendarOperation _CurrentOperation;

	private DateTime _DisplayMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

	private DayOfWeek _FirstDayOfWeek;

	internal bool _TrailingDaysVisible = true;

	internal bool _TrailingDaysBeforeVisible = true;

	internal bool _TrailingDaysAfterVisible = true;

	private bool _ShowWeekNumbers;

	private CalendarWeekRule _WeekOfYearRule;

	private Size _DaySize = _DefaultDaySize;

	private MonthCalendarColors _Colors = new MonthCalendarColors();

	private bool _MultiSelect;

	private bool _MouseSelectionEnabled = true;

	private DateTime _SelectedDate = DateTime.MinValue;

	private bool _WeekendDaysSelectable = true;

	private eLabelPartAlignment _DefaultDayLabelTextAlign = eLabelPartAlignment.MiddleCenter;

	private bool _TwoLetterDayName = true;

	private string[] _DayNames;

	private bool _HeaderNavigationEnabled = true;

	private bool _ShowTodayMarker = true;

	private DateTime _TodayDate = DateTime.MinValue;

	private DateTime _MinDate = DateTimeGroup.MinDateTime;

	private DateTime _MaxDate = DateTimeGroup.MaxDateTime;

	private int _NavigationMonthsAheadVisibility = 1;

	private bool _DayClickAutoClosePopup = true;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Year/Century selection is enabled.")]
	public bool YearSelectionEnabled
	{
		get
		{
			return _YearSelectionEnabled;
		}
		set
		{
			if (_YearSelectionEnabled != value)
			{
				_YearSelectionEnabled = value;
				ItemContainer navigationContainer = GetNavigationContainer();
				if (navigationContainer != null && navigationContainer.SubItems["yearLabel"] is LabelItem labelItem)
				{
					labelItem.Cursor = (_YearSelectionEnabled ? Cursors.Hand : null);
				}
			}
		}
	}

	[Description("")]
	public DateTime DisplayMonth
	{
		get
		{
			return _DisplayMonth;
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
			SetDisplayMonth(value.Month, value.Year, eEventSource.Code);
		}
	}

	[DefaultValue(DayOfWeek.Sunday)]
	[Description("Indicates first day of week displayed on the calendar.")]
	public DayOfWeek FirstDayOfWeek
	{
		get
		{
			return _FirstDayOfWeek;
		}
		set
		{
			if (_FirstDayOfWeek != value)
			{
				_FirstDayOfWeek = value;
				SetDisplayMonth(_DisplayMonth.Month, _DisplayMonth.Year, eEventSource.Code);
			}
		}
	}

	[DefaultValue(true)]
	public bool TrailingDaysVisible
	{
		get
		{
			return _TrailingDaysVisible;
		}
		set
		{
			if (_TrailingDaysVisible != value)
			{
				_TrailingDaysVisible = value;
				OnTrailingDaysVisibleChanged();
			}
		}
	}

	internal bool TrailingDaysBeforeVisible
	{
		get
		{
			return _TrailingDaysBeforeVisible;
		}
		set
		{
			if (_TrailingDaysBeforeVisible != value)
			{
				_TrailingDaysBeforeVisible = value;
				OnTrailingDaysVisibleChanged();
			}
		}
	}

	internal bool TrailingDaysAfterVisible
	{
		get
		{
			return _TrailingDaysAfterVisible;
		}
		set
		{
			if (_TrailingDaysAfterVisible != value)
			{
				_TrailingDaysAfterVisible = value;
				OnTrailingDaysVisibleChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether week of year is visible.")]
	public bool ShowWeekNumbers
	{
		get
		{
			return _ShowWeekNumbers;
		}
		set
		{
			if (_ShowWeekNumbers != value)
			{
				_ShowWeekNumbers = value;
				OnShowWeekNumbersChanged();
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
			return _WeekOfYearRule;
		}
		set
		{
			_WeekOfYearRule = value;
			if (_ShowWeekNumbers)
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
			return _DaySize;
		}
		set
		{
			_DaySize = value;
			OnDaySizeChanged();
		}
	}

	[NotifyParentProperty(true)]
	[Category("Appearance")]
	[Description("Gets colors used by control.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public MonthCalendarColors Colors => _Colors;

	[DefaultValue(false)]
	[Description("Indicates whether multiple days can be selected by clicking each day.")]
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
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether selection of dates using mouse is enabled.")]
	public bool MouseSelectionEnabled
	{
		get
		{
			return _MouseSelectionEnabled;
		}
		set
		{
			if (_MouseSelectionEnabled != value)
			{
				_MouseSelectionEnabled = value;
			}
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
			_SelectedDate = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is DayLabel dayLabel && !(dayLabel.Date == DateTime.MinValue))
				{
					if (dayLabel.Date == _SelectedDate.Date)
					{
						dayLabel.IsSelected = true;
					}
					else
					{
						dayLabel.IsSelected = false;
					}
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
			return _WeekendDaysSelectable;
		}
		set
		{
			if (_WeekendDaysSelectable != value)
			{
				_WeekendDaysSelectable = value;
				SetDisplayMonth(_DisplayMonth.Month, _DisplayMonth.Year, eEventSource.Code);
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

	[DefaultValue(eLabelPartAlignment.MiddleCenter)]
	[Description("Indicates default text alignment for the DayLabel items representing calendar days.")]
	public eLabelPartAlignment DefaultDayLabelTextAlign
	{
		get
		{
			return _DefaultDayLabelTextAlign;
		}
		set
		{
			if (_DefaultDayLabelTextAlign != value)
			{
				_DefaultDayLabelTextAlign = value;
				SetDisplayMonth(_DisplayMonth.Month, _DisplayMonth.Year, eEventSource.Code);
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
			return _TwoLetterDayName;
		}
		set
		{
			if (_TwoLetterDayName != value)
			{
				_TwoLetterDayName = value;
				SetDisplayMonth(_DisplayMonth.Month, _DisplayMonth.Year, eEventSource.Code);
				Refresh();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates array of custom names for days displayed on calendar header.")]
	[Localizable(true)]
	public string[] DayNames
	{
		get
		{
			return _DayNames;
		}
		set
		{
			if (value != null && value.Length != 7)
			{
				throw new ArgumentException("DayNames must have exactly 7 items in collection.");
			}
			_DayNames = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether header navigation buttons for month and year are visible.")]
	public bool HeaderNavigationEnabled
	{
		get
		{
			return _HeaderNavigationEnabled;
		}
		set
		{
			if (_HeaderNavigationEnabled != value)
			{
				_HeaderNavigationEnabled = value;
				OnHeaderNavigationEnabledChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates navigation container background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle NavigationBackgroundStyle => GetNavigationContainer().BackgroundStyle;

	[DefaultValue(true)]
	[Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
	public bool ShowTodayMarker
	{
		get
		{
			return _ShowTodayMarker;
		}
		set
		{
			if (_ShowTodayMarker != value)
			{
				_ShowTodayMarker = value;
				UpdateTodayMarker();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime TodayDate
	{
		get
		{
			return _TodayDate;
		}
		set
		{
			value = value.Date;
			if (_TodayDate != value)
			{
				_TodayDate = value;
				if (_ShowTodayMarker)
				{
					UpdateTodayMarker();
				}
			}
		}
	}

	[Browsable(false)]
	public bool TodayDateSet => _TodayDate != DateTime.MinValue;

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return _MinDate;
		}
		set
		{
			_MinDate = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return _MaxDate;
		}
		set
		{
			_MaxDate = value;
		}
	}

	internal int NavigationMonthsAheadVisibility
	{
		get
		{
			return _NavigationMonthsAheadVisibility;
		}
		set
		{
			if (_NavigationMonthsAheadVisibility != value)
			{
				_NavigationMonthsAheadVisibility = value;
				UpdateNavigationButtonsEnabled();
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

	public SingleMonthCalendar()
	{
		_FirstDayOfWeek = DateTimeInput.GetActiveCulture().DateTimeFormat.FirstDayOfWeek;
		m_IsContainer = true;
		AutoCollapseOnClick = true;
		MouseUpNotification = true;
		AccessibleRole = (AccessibleRole)20;
		_Colors.Parent = this;
		for (int i = 0; i < 49; i++)
		{
			DayLabel item = new DayLabel
			{
				Visible = false
			};
			SubItems.Add(item);
		}
		Size fixedSize = new Size(13, 18);
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
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += MonthNavigationDecreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		LabelItem labelItem = new LabelItem("monthLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Click += ShowMonthSelectionPopupMenu;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("increaseMonth");
		buttonItem.GlobalItem = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += MonthNavigationIncreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("decreaseYear");
		buttonItem.GlobalItem = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.Click += YearNavigationDecreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		labelItem = new LabelItem("yearLabel");
		labelItem.AutoCollapseOnClick = false;
		labelItem.GlobalItem = false;
		labelItem.TextAlignment = (StringAlignment)1;
		labelItem.TextLineAlignment = (StringAlignment)1;
		labelItem.PaddingBottom = 2;
		labelItem.Click += ShowYearSelection;
		labelItem.Cursor = Cursors.Hand;
		itemContainer.SubItems.Add(labelItem);
		buttonItem = new ButtonItem("increaseYear");
		buttonItem.GlobalItem = false;
		buttonItem._FadeEnabled = false;
		buttonItem.Text = "<expand direction=\"right\"/>";
		buttonItem.Click += YearNavigationIncreaseClick;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem.FixedSize = fixedSize;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		itemContainer.SubItems.Add(buttonItem);
		_MonthsPopupMenu = new ButtonItem("monthsPopupMenu");
		_MonthsPopupMenu.Visible = false;
		itemContainer.SubItems.Add(_MonthsPopupMenu);
		SubItems.Add(itemContainer);
	}

	private void ShowYearSelection(object sender, EventArgs e)
	{
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		if (_YearSelectionEnabled && _CurrentOperation == eCurrentSingleMonthCalendarOperation.None && (!(Parent is MonthCalendarItem) || (((MonthCalendarItem)Parent).CalendarDimensions.Width <= 1 && ((MonthCalendarItem)Parent).CalendarDimensions.Height <= 1)))
		{
			_CurrentOperation = eCurrentSingleMonthCalendarOperation.AnimatingShowYearSelector;
			_YearSelector = new YearSelectorControl();
			((Control)_YearSelector).Size = new Size(8, 8);
			((Control)_YearSelector).Location = new Point(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2);
			((Control)_YearSelector).Visible = true;
			_YearSelector.Style = eDotNetBarStyle.StyleManagerControlled;
			if (BarFunctions.IsOffice2007Style(Style))
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
				_YearSelector.BackgroundStyle.BackColor = elementStyle.BackColor;
				_YearSelector.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
			}
			else
			{
				_YearSelector.BackgroundStyle.BackColor = SystemColors.Window;
			}
			ItemContainer navigationContainer = GetNavigationContainer();
			LabelItem labelItem = navigationContainer.SubItems["monthLabel"] as LabelItem;
			if (EffectiveStyle != eDotNetBarStyle.Metro)
			{
				_YearSelector.YearSelector.TextColor = labelItem.ForeColor;
			}
			Control val = (Control)ContainerControl;
			val.Controls.Add((Control)(object)_YearSelector);
			_YearSelector.YearSelector.CenturyYearStart = DisplayMonth.Year / 10 * 10;
			_YearSelector.YearSelector.SelectedYear = DisplayMonth.Year;
			Rectangle bounds = Bounds;
			_YearSelector.YearSelector.SelectedYearChanged += YearSelectorSelectedYearChanged;
			AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(_YearSelector, "Bounds", bounds), AnimationEasing.EaseInOutQuad, 200);
			animationRectangle.AutoDispose = true;
			animationRectangle.AnimationCompleted += ShowAnimationCompleted;
			animationRectangle.Start();
		}
	}

	private void ShowAnimationCompleted(object sender, EventArgs e)
	{
		_CurrentOperation = eCurrentSingleMonthCalendarOperation.None;
	}

	private void YearSelectorSelectedYearChanged(object sender, EventArgs e)
	{
		if (_CurrentOperation == eCurrentSingleMonthCalendarOperation.None)
		{
			RemoveYearSelector(animate: true);
			DateTime dateTime = new DateTime(_YearSelector.YearSelector.SelectedYear, DisplayMonth.Month, DisplayMonth.Day);
			if (dateTime < MinDate)
			{
				dateTime = MinDate;
			}
			else if (dateTime > MaxDate)
			{
				dateTime = MaxDate;
			}
			SetDisplayMonth(dateTime.Month, dateTime.Year, eEventSource.Mouse);
		}
	}

	private void RemoveYearSelector(bool animate)
	{
		if (_CurrentOperation == eCurrentSingleMonthCalendarOperation.AnimatingHideYearSelector)
		{
			return;
		}
		if (animate)
		{
			_CurrentOperation = eCurrentSingleMonthCalendarOperation.AnimatingHideYearSelector;
			AnimationRectangle animationRectangle = new AnimationRectangle(new AnimationRequest(to: new Rectangle(LeftInternal + (WidthInternal - 8) / 2, TopInternal + (HeightInternal - 8) / 2, 8, 8), target: _YearSelector, targetPropertyName: "Bounds"), AnimationEasing.EaseInOutQuad, 200);
			animationRectangle.AnimationCompleted += AnimationCompleted;
			animationRectangle.Start();
			return;
		}
		if (((Control)_YearSelector).Parent != null)
		{
			((Control)_YearSelector).Parent.Controls.Remove((Control)(object)_YearSelector);
		}
		_YearSelector.YearSelector.SelectedYearChanged -= YearSelectorSelectedYearChanged;
		((Component)(object)_YearSelector).Dispose();
		_YearSelector = null;
	}

	private void AnimationCompleted(object sender, EventArgs e)
	{
		AnimationRectangle animationRectangle = (AnimationRectangle)sender;
		animationRectangle.Dispose();
		if (((Control)_YearSelector).Parent != null)
		{
			((Control)_YearSelector).Parent.Controls.Remove((Control)(object)_YearSelector);
		}
		_YearSelector.YearSelector.SelectedYearChanged -= YearSelectorSelectedYearChanged;
		((Component)(object)_YearSelector).Dispose();
		_YearSelector = null;
		_CurrentOperation = eCurrentSingleMonthCalendarOperation.None;
	}

	private void ShowMonthSelectionPopupMenu(object sender, EventArgs e)
	{
		if (!HeaderNavigationEnabled)
		{
			return;
		}
		ButtonItem monthsPopupMenu = _MonthsPopupMenu;
		foreach (BaseItem subItem in monthsPopupMenu.SubItems)
		{
			subItem.Click -= PopupMonthSelectionClick;
		}
		monthsPopupMenu.SubItems.Clear();
		string[] monthNames = DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames;
		for (int i = 0; i < monthNames.Length; i++)
		{
			if (monthNames[i] != null && monthNames[i].Length != 0)
			{
				ButtonItem buttonItem = new ButtonItem((i + 1).ToString(), monthNames[i]);
				buttonItem.AutoCollapseOnClick = false;
				monthsPopupMenu.SubItems.Add(buttonItem);
				buttonItem.Click += PopupMonthSelectionClick;
			}
		}
		monthsPopupMenu.Popup(Control.MousePosition);
	}

	private void PopupMonthSelectionClick(object sender, EventArgs e)
	{
		if (sender is ButtonItem buttonItem)
		{
			int month = int.Parse(buttonItem.Name);
			((PopupItem)buttonItem.Parent).Expanded = false;
			SetDisplayMonth(month, DisplayMonth.Year, eEventSource.Mouse);
		}
	}

	public override void RecalcSize()
	{
		int num = 49;
		int num2 = 0;
		int num3 = 0;
		int num4 = 2;
		Point location = m_Rect.Location;
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(base.BackgroundStyle, out dispose);
		location.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		location.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		Point location2 = location;
		Point point = location;
		if (_ShowWeekNumbers)
		{
			point.X += _DaySize.Width;
		}
		ItemContainer navigationContainer = GetNavigationContainer();
		navigationContainer.LeftInternal = location.X;
		navigationContainer.TopInternal = location.Y;
		navigationContainer.WidthInternal = _DaySize.Width * (_ShowWeekNumbers ? 8 : 7);
		navigationContainer.RecalcSize();
		navigationContainer.WidthInternal = _DaySize.Width * (_ShowWeekNumbers ? 8 : 7);
		location2.Y += navigationContainer.HeightInternal + 1;
		point.Y += navigationContainer.HeightInternal + 1;
		Point location3 = point;
		int num5 = 0;
		for (int i = 0; i < num; i++)
		{
			BaseItem baseItem = SubItems[i];
			if (baseItem is YearSelectorContainer)
			{
				continue;
			}
			if (baseItem is DayLabel dayLabel)
			{
				dayLabel.RecalcSize();
				dayLabel.Bounds = new Rectangle(location3, _DaySize);
			}
			else
			{
				baseItem.RecalcSize();
			}
			if (baseItem.Visible)
			{
				if (baseItem.HeightInternal > num3)
				{
					num3 = baseItem.HeightInternal;
				}
				location3.X += baseItem.WidthInternal;
			}
			num2++;
			if (num2 != 7)
			{
				continue;
			}
			if (_ShowWeekNumbers)
			{
				if (num5 > 0)
				{
					DayLabel dayLabel2 = SubItems[num + num5 - 1] as DayLabel;
					dayLabel2.RecalcSize();
					dayLabel2.Bounds = new Rectangle(location2, _DaySize);
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
		m_Rect.Width = _DaySize.Width * 7;
		m_Rect.Height = _DaySize.Height * 7 + num4;
		if (_ShowWeekNumbers)
		{
			m_Rect.Width += _DaySize.Width;
		}
		m_Rect.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		m_Rect.Height += ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle) + navigationContainer.HeightInternal + 1;
		if (dispose)
		{
			elementStyle.Dispose();
		}
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		ItemContainer navigationContainer = GetNavigationContainer();
		LabelItem labelItem = navigationContainer.SubItems["monthLabel"] as LabelItem;
		labelItem.ForeColor = p.Colors.ItemText;
		labelItem = navigationContainer.SubItems["yearLabel"] as LabelItem;
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

	private void SetDisplayMonth(DateTime date, eEventSource source)
	{
		SetDisplayMonth(date.Month, date.Year, source);
	}

	private void SetDisplayMonth(int month, int year, eEventSource source)
	{
		DateTime dateTime = new DateTime(year, month, 1);
		bool flag = dateTime != _DisplayMonth;
		if (flag)
		{
			OnMonthChanging(new EventSourceArgs(source));
		}
		_DisplayMonth = dateTime;
		string[] array = DateTimeInput.GetActiveCulture().DateTimeFormat.AbbreviatedDayNames;
		string[] shortestDayNames = DateTimeInput.GetActiveCulture().DateTimeFormat.ShortestDayNames;
		if (_DayNames != null)
		{
			array = _DayNames;
		}
		int firstDayOfWeek = (int)_FirstDayOfWeek;
		int num = firstDayOfWeek;
		for (int i = 0; i < 7; i++)
		{
			DayLabel dayLabel = SubItems[i] as DayLabel;
			dayLabel.Text = (_TwoLetterDayName ? GetTwoLetterDayName(shortestDayNames[num]) : array[num]);
			dayLabel.Visible = true;
			dayLabel.Displayed = true;
			dayLabel.TrackMouse = false;
			dayLabel.Selectable = false;
			dayLabel.IsDayLabel = true;
			ApplyDayOfWeekMarker(dayLabel);
			num++;
			if (num > 6)
			{
				num = 0;
			}
		}
		bool[] monthlyMarkers = GetMonthlyMarkers();
		bool[] weeklyMarkers = GetWeeklyMarkers();
		bool[] annualMarkers = GetAnnualMarkers(month);
		bool[] markedDays = GetMarkedDays(month, year);
		DateTime dateTime2 = ((!_ShowTodayMarker) ? DateTime.MinValue : ((_TodayDate == DateTime.MinValue) ? DateTime.Today : _TodayDate));
		bool flag2 = dateTime2 != DateTime.MinValue;
		int num2 = (int)(dateTime.DayOfWeek - firstDayOfWeek);
		if (num2 < 0)
		{
			num2 = 7 + num2;
		}
		int num3 = 7;
		if (num2 > 0)
		{
			DateTime dateTime3 = dateTime;
			dateTime3 = dateTime3.AddDays(-num2);
			annualMarkers = GetAnnualMarkers(dateTime3.Month);
			markedDays = GetMarkedDays(dateTime3.Month, dateTime3.Year);
			for (int j = num3; j < num3 + num2; j++)
			{
				DayLabel dayLabel2 = SubItems[j] as DayLabel;
				dayLabel2.Date = dateTime3;
				dayLabel2.Visible = true;
				ResetDayLabel(dayLabel2);
				if (weeklyMarkers[(int)dateTime3.DayOfWeek])
				{
					ApplyWeeklyMarker(dayLabel2);
				}
				if (monthlyMarkers[dateTime3.Day - 1])
				{
					ApplyMonthlyMarker(dayLabel2);
				}
				if (annualMarkers[dateTime3.Day - 1])
				{
					ApplyAnnualMarker(dayLabel2);
				}
				if (markedDays[dateTime3.Day - 1])
				{
					ApplyDayMarker(dayLabel2);
				}
				if (flag2 && dateTime2 == dateTime3)
				{
					dayLabel2.IsToday = true;
				}
				if (!_WeekendDaysSelectable && DayLabel.IsWeekend(dateTime3))
				{
					dayLabel2.Selectable = false;
					dayLabel2.TrackMouse = false;
				}
				dayLabel2.Enabled = CanSelect(dateTime3);
				dayLabel2.Displayed = _TrailingDaysVisible && _TrailingDaysBeforeVisible;
				dayLabel2.IsTrailing = true;
				dateTime3 = dateTime3.AddDays(1.0);
			}
			num3 += num2;
		}
		annualMarkers = GetAnnualMarkers(month);
		markedDays = GetMarkedDays(month, year);
		bool flag3 = true;
		int num4 = 0;
		int num5 = num3 % 7;
		for (int k = num3; k < 49; k++)
		{
			DayLabel dayLabel3 = SubItems[k] as DayLabel;
			dayLabel3.Date = dateTime;
			ResetDayLabel(dayLabel3);
			if (dateTime.Month != month && flag3)
			{
				annualMarkers = GetAnnualMarkers(dateTime.Month);
				markedDays = GetMarkedDays(dateTime.Month, dateTime.Year);
				flag3 = false;
			}
			if (weeklyMarkers[(int)dateTime.DayOfWeek])
			{
				ApplyWeeklyMarker(dayLabel3);
			}
			if (monthlyMarkers[dateTime.Day - 1])
			{
				ApplyMonthlyMarker(dayLabel3);
			}
			if (annualMarkers[dateTime.Day - 1])
			{
				ApplyAnnualMarker(dayLabel3);
			}
			if (markedDays[dateTime.Day - 1])
			{
				ApplyDayMarker(dayLabel3);
			}
			if (flag2 && dateTime2 == dateTime)
			{
				dayLabel3.IsToday = true;
			}
			if (!_WeekendDaysSelectable && DayLabel.IsWeekend(dateTime))
			{
				dayLabel3.Selectable = false;
				dayLabel3.TrackMouse = false;
			}
			dayLabel3.Enabled = CanSelect(dateTime);
			dayLabel3.Visible = true;
			if (dateTime.Month != month)
			{
				dayLabel3.IsTrailing = true;
				dayLabel3.Displayed = _TrailingDaysVisible && _TrailingDaysAfterVisible;
			}
			else
			{
				dayLabel3.Displayed = true;
				dayLabel3.IsTrailing = false;
				if (dayLabel3.Date == _SelectedDate)
				{
					dayLabel3.IsSelected = true;
				}
			}
			if (num5 == 6)
			{
				if (_ShowWeekNumbers)
				{
					DayLabel dayLabel4 = SubItems[49 + num4] as DayLabel;
					dayLabel4.Text = DateTimeInput.GetActiveCulture().Calendar.GetWeekOfYear(dateTime, _WeekOfYearRule, _FirstDayOfWeek).ToString();
					dayLabel4.Visible = true;
					dayLabel4.TrackMouse = false;
					dayLabel4.Selectable = false;
					if (dateTime.Month != month && dateTime.AddDays(-num5).Month != month)
					{
						dayLabel4.Displayed = _TrailingDaysVisible;
					}
					else
					{
						dayLabel4.Displayed = true;
					}
					ApplyWeekOfYearMarker(dayLabel4);
				}
				num4++;
				num5 = -1;
			}
			dateTime = dateTime.AddDays(1.0);
			num5++;
		}
		ItemContainer navigationContainer = GetNavigationContainer();
		navigationContainer.Visible = true;
		navigationContainer.Displayed = true;
		LabelItem labelItem = navigationContainer.SubItems["monthLabel"] as LabelItem;
		labelItem.Width = _DaySize.Width * 3;
		labelItem.Text = DateTimeInput.GetActiveCulture().DateTimeFormat.MonthNames[month - 1];
		labelItem = navigationContainer.SubItems["yearLabel"] as LabelItem;
		labelItem.Text = year.ToString();
		labelItem.Width = (int)((double)_DaySize.Width * 1.4);
		UpdateNavigationButtonsEnabled();
		OnAppearanceChanged();
		Refresh();
		if (flag)
		{
			OnMonthChanged(new EventSourceArgs(source));
			if (_YearSelector != null)
			{
				RemoveYearSelector(animate: false);
			}
		}
	}

	private void UpdateNavigationButtonsEnabled()
	{
		ItemContainer navigationContainer = GetNavigationContainer();
		DateTime dateTime = _DisplayMonth.AddMonths(_NavigationMonthsAheadVisibility);
		if (dateTime > MaxDate)
		{
			navigationContainer.SubItems["increaseMonth"].Enabled = false;
			navigationContainer.SubItems["increaseYear"].Enabled = false;
		}
		else
		{
			navigationContainer.SubItems["increaseMonth"].Enabled = true;
			dateTime = _DisplayMonth.AddYears(1);
			navigationContainer.SubItems["increaseYear"].Enabled = MaxDate > dateTime;
		}
		dateTime = _DisplayMonth.AddMonths(-1);
		if (dateTime < GetBeginningOfTheMonth(MinDate))
		{
			navigationContainer.SubItems["decreaseMonth"].Enabled = false;
			navigationContainer.SubItems["decreaseYear"].Enabled = false;
		}
		else
		{
			navigationContainer.SubItems["decreaseMonth"].Enabled = true;
			dateTime = _DisplayMonth.AddYears(-1);
			navigationContainer.SubItems["decreaseYear"].Enabled = dateTime > MinDate;
		}
	}

	private DateTime GetBeginningOfTheMonth(DateTime date)
	{
		if (date.Day > 1)
		{
			date = date.AddDays(-date.Day);
		}
		return date;
	}

	private bool CanSelect(DateTime d)
	{
		if (d > MaxDate || d < MinDate)
		{
			return false;
		}
		return true;
	}

	private void ApplyWeekOfYearMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.WeekOfYear.IsCustomized)
		{
			ApplyMarker(day, colors.WeekOfYear);
		}
	}

	private void ApplyDayOfWeekMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.DayLabel.IsCustomized)
		{
			ApplyMarker(day, colors.DayLabel);
		}
	}

	private void ApplyDayMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.DayMarker.IsCustomized)
		{
			ApplyMarker(day, colors.DayMarker);
		}
	}

	private bool[] GetMarkedDays(int month, int year)
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
				if (dateTime.Month == month && dateTime.Year == year)
				{
					array[dateTime.Day - 1] = true;
				}
			}
		}
		return array;
	}

	private void ApplyAnnualMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.AnnualMarker.IsCustomized)
		{
			ApplyMarker(day, colors.AnnualMarker);
		}
	}

	internal MonthCalendarColors GetColors()
	{
		MonthCalendarItem monthCalendarItem = Parent as MonthCalendarItem;
		MonthCalendarColors monthCalendarColors = null;
		if (monthCalendarItem != null)
		{
			return monthCalendarItem.Colors;
		}
		return _Colors;
	}

	private void ApplyWeeklyMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.WeeklyMarker.IsCustomized)
		{
			ApplyMarker(day, colors.WeeklyMarker);
		}
	}

	private void ApplyMonthlyMarker(DayLabel day)
	{
		MonthCalendarColors colors = GetColors();
		if (colors != null && colors.MonthlyMarker.IsCustomized)
		{
			ApplyMarker(day, colors.MonthlyMarker);
		}
	}

	private void ApplyMarker(DayLabel day, DateAppearanceDescription c)
	{
		day.BackgroundStyle.BackColor = c.BackColor;
		day.BackgroundStyle.BackColor2 = c.BackColor2;
		day.BackgroundStyle.BackColorGradientAngle = c.BackColorGradientAngle;
		if (!c.BorderColor.IsEmpty)
		{
			day.BackgroundStyle.Border = eStyleBorderType.Solid;
			day.BackgroundStyle.BorderColor = c.BorderColor;
			day.BackgroundStyle.BorderWidth = 1;
		}
		day.IsBold = c.IsBold;
		if (!c.TextColor.IsEmpty)
		{
			day.TextColor = c.TextColor;
		}
		day.Selectable = c.Selectable;
		if (!c.Selectable)
		{
			day.TrackMouse = false;
		}
	}

	private bool[] GetMonthlyMarkers()
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

	private bool[] GetAnnualMarkers(int month)
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
				if (dateTime.Month == month)
				{
					array[dateTime.Day - 1] = true;
				}
			}
		}
		return array;
	}

	private bool[] GetWeeklyMarkers()
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

	private ItemContainer GetNavigationContainer()
	{
		return SubItems["navigationContainer"] as ItemContainer;
	}

	private void ResetDayLabel(DayLabel day)
	{
		day.IsSelected = false;
		day.Selectable = true;
		day.TrackMouse = true;
		day.IsToday = false;
		day.IsBold = false;
		day.Image = null;
		day.ImageAlign = eLabelPartAlignment.MiddleRight;
		day.TextAlign = _DefaultDayLabelTextAlign;
		day.TextColor = Color.Empty;
		day.Tooltip = "";
		day.ExpandOnMouseDown = false;
		day.SubItems.Clear();
		day.BackgroundStyle.Reset();
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

	private string GetTwoLetterDayName(string s)
	{
		if (s.Length > 2)
		{
			return s.Substring(0, 2);
		}
		return s;
	}

	private void OnTrailingDaysVisibleChanged()
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is DayLabel { IsTrailing: not false } dayLabel)
			{
				bool trailingDaysVisible = _TrailingDaysVisible;
				trailingDaysVisible = ((!(dayLabel.Date < DisplayMonth)) ? (trailingDaysVisible & _TrailingDaysAfterVisible) : (trailingDaysVisible & _TrailingDaysBeforeVisible));
				dayLabel.Displayed = trailingDaysVisible;
			}
		}
		Refresh();
	}

	private void OnShowWeekNumbersChanged()
	{
		if (_ShowWeekNumbers)
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

	private void OnDaySizeChanged()
	{
		ItemContainer navigationContainer = GetNavigationContainer();
		if (navigationContainer.MinimumSize.Height < _DaySize.Height)
		{
			navigationContainer.MinimumSize = new Size(navigationContainer.MinimumSize.Width, _DaySize.Height);
		}
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDaySize()
	{
		int width = _DaySize.Width;
		Size defaultDaySize = _DefaultDaySize;
		if (width == defaultDaySize.Width)
		{
			int height = _DaySize.Height;
			defaultDaySize = _DefaultDaySize;
			return height != defaultDaySize.Height;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDaySize()
	{
		DaySize = _DefaultDaySize;
	}

	internal void OnPaintLabel(DayLabel label, DayPaintEventArgs e)
	{
		if (this.PaintLabel != null)
		{
			this.PaintLabel(label, e);
		}
	}

	internal void DayLabelClick(DayLabel day)
	{
		if (!_MouseSelectionEnabled || !day.Selectable || !(day.Date != DateTime.MinValue) || (!_WeekendDaysSelectable && DayLabel.IsWeekend(day.Date)))
		{
			return;
		}
		if (MultiSelect)
		{
			if (_SelectedDate == DateTime.MinValue)
			{
				_SelectedDate = day.Date;
			}
			day.IsSelected = !day.IsSelected;
		}
		else
		{
			SelectedDate = day.Date;
		}
	}

	protected virtual void OnSelectedDateChanged(EventArgs e)
	{
		if (this.DateChanged != null)
		{
			this.DateChanged(this, e);
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

	internal void OnLabelMouseDown(DayLabel sender, MouseEventArgs e)
	{
		if (this.LabelMouseDown != null)
		{
			this.LabelMouseDown.Invoke((object)sender, e);
		}
	}

	internal void OnLabelMouseUp(DayLabel sender, MouseEventArgs e)
	{
		if (this.LabelMouseUp != null)
		{
			this.LabelMouseUp.Invoke((object)sender, e);
		}
	}

	internal void OnLabelMouseEnter(DayLabel sender, EventArgs e)
	{
		if (this.LabelMouseEnter != null)
		{
			this.LabelMouseEnter(sender, e);
		}
	}

	internal void OnLabelMouseLeave(DayLabel sender, EventArgs e)
	{
		if (this.LabelMouseLeave != null)
		{
			this.LabelMouseLeave(sender, e);
		}
	}

	internal void OnLabelMouseMove(DayLabel sender, MouseEventArgs e)
	{
		if (this.LabelMouseMove != null)
		{
			this.LabelMouseMove.Invoke((object)sender, e);
		}
	}

	internal void OnLabelMouseHover(DayLabel sender, EventArgs e)
	{
		if (this.LabelMouseHover != null)
		{
			this.LabelMouseHover(sender, e);
		}
	}

	private void MonthNavigationDecreaseClick(object sender, EventArgs e)
	{
		SetDisplayMonth(_DisplayMonth.AddMonths(-1), eEventSource.Mouse);
		if (AutoSelectDate())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private bool AutoSelectDate()
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

	private void MonthNavigationIncreaseClick(object sender, EventArgs e)
	{
		SetDisplayMonth(_DisplayMonth.AddMonths(1), eEventSource.Mouse);
		if (AutoSelectDate())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void YearNavigationDecreaseClick(object sender, EventArgs e)
	{
		SetDisplayMonth(_DisplayMonth.AddMonths(-12), eEventSource.Mouse);
		if (AutoSelectDate())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void YearNavigationIncreaseClick(object sender, EventArgs e)
	{
		SetDisplayMonth(_DisplayMonth.AddMonths(12), eEventSource.Mouse);
		if (AutoSelectDate())
		{
			SelectedDate = DisplayMonth;
		}
	}

	private void OnHeaderNavigationEnabledChanged()
	{
		ItemContainer navigationContainer = GetNavigationContainer();
		bool headerNavigationEnabled = _HeaderNavigationEnabled;
		navigationContainer.SubItems["decreaseMonth"].Visible = headerNavigationEnabled;
		navigationContainer.SubItems["increaseMonth"].Visible = headerNavigationEnabled;
		navigationContainer.SubItems["decreaseYear"].Visible = headerNavigationEnabled;
		navigationContainer.SubItems["increaseYear"].Visible = headerNavigationEnabled;
		navigationContainer.NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	private void UpdateTodayMarker()
	{
		DateTime dateTime = ((!_ShowTodayMarker) ? DateTime.MinValue : ((_TodayDate == DateTime.MinValue) ? DateTime.Today : DateTime.MinValue));
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
		return !_MaxDate.Equals(DateTimeGroup.MaxDateTime);
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
