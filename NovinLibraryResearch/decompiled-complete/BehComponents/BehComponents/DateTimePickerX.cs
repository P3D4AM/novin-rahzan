using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BehComponents;

[ToolboxBitmap(typeof(DateTimePickerX), "DateTimePickerX.bmp")]
[ToolboxItem(true)]
[Description("DateTime Picker v2")]
public class DateTimePickerX : DropDownEmpty
{
	public enum FormatDate
	{
		Short,
		Long,
		Custom
	}

	public delegate void OnSelectedDateChanged(DateTime selectedDateTime, PersianDateTime selectedPersianDateTime);

	private FormatDate _format = FormatDate.Long;

	private string _customFormat = "";

	private MonthCalendarX _monthCalendarX = new MonthCalendarX();

	private DateTime dtPrevSelected = default(DateTime);

	private bool _DropDownClosedWhenSelectedDateChanged;

	private bool _DropDownClosedWhenClickOnDays;

	private bool _UserSelectDate;

	private string _ClearDateTimeText = "";

	private bool _ClearDateTimeWhenDownDeleteKey = true;

	private string _format4Binding = "yyyy/MM/dd";

	private IContainer components;

	[Browsable(false)]
	public string Programmer => "Saeed Mohammadi";

	[Browsable(false)]
	public string ProgrammerWebSite => "http://BehSoft.blogfa.com/";

	internal string SelectedDate
	{
		get
		{
			string formatThis = GetFormatThis();
			string text = "";
			if (CalendarType == CalendarTypes.Persian)
			{
				return _monthCalendarX.GetSelectedDateInPersianDateTime().ToStringFormat(formatThis);
			}
			return _monthCalendarX.GetSelectedDateInDateTime().ToString(formatThis);
		}
		set
		{
			string formatThis = GetFormatThis();
			DateTime selectedDate;
			if (CalendarType == CalendarTypes.Persian)
			{
				PersianDateTime date = PersianDateTime.Parse(value, formatThis);
				selectedDate = PersianDateTime.ToDateTime(date);
			}
			else
			{
				selectedDate = DateTime.Parse(value);
			}
			_monthCalendarX.SetSelectedDate(selectedDate);
		}
	}

	[Bindable(true)]
	[Browsable(false)]
	[DesignOnly(true)]
	public DateTime SelectedDateInDateTime
	{
		get
		{
			return _monthCalendarX.GetSelectedDateInDateTime();
		}
		set
		{
			_monthCalendarX.SetSelectedDate(value);
		}
	}

	[Browsable(false)]
	[DesignOnly(true)]
	[Bindable(true)]
	public string SelectedDateInStringPersian
	{
		get
		{
			return _monthCalendarX.GetSelectedDateInPersianDateTime().ToStringFormat(_format4Binding);
		}
		set
		{
			PersianDateTime selectedDate = PersianDateTime.Parse(value, _format4Binding);
			_monthCalendarX.SetSelectedDate(selectedDate);
		}
	}

	[DesignOnly(true)]
	[Bindable(true)]
	[Browsable(false)]
	public string SelectedDateInStringEnglish
	{
		get
		{
			return _monthCalendarX.GetSelectedDateInDateTime().ToString(_format4Binding);
		}
		set
		{
			DateTime selectedDate = DateTime.Parse(value);
			_monthCalendarX.SetSelectedDate(selectedDate);
		}
	}

	public string Format4Binding
	{
		get
		{
			return _format4Binding;
		}
		set
		{
			_format4Binding = value;
		}
	}

	public CalendarTypes CalendarType
	{
		get
		{
			return _monthCalendarX.CalendarType;
		}
		set
		{
			_monthCalendarX.CalendarType = value;
			ShowSelectedDate(!_UserSelectDate);
		}
	}

	public Font CalendarTitleFont
	{
		get
		{
			return _monthCalendarX.TitleFont;
		}
		set
		{
			_monthCalendarX.TitleFont = value;
		}
	}

	public Color CalendarTitleForeColor
	{
		get
		{
			return _monthCalendarX.TitleForeColor;
		}
		set
		{
			_monthCalendarX.TitleForeColor = value;
		}
	}

	public Color CalendarTitleBackColor
	{
		get
		{
			return _monthCalendarX.TitleBackColor;
		}
		set
		{
			_monthCalendarX.TitleBackColor = value;
		}
	}

	public Color CalendarWeekDaysForeColor
	{
		get
		{
			return _monthCalendarX.WeekDaysForeColor;
		}
		set
		{
			_monthCalendarX.WeekDaysForeColor = value;
		}
	}

	public Color CalendarWeekDaysBackColor
	{
		get
		{
			return _monthCalendarX.WeekDaysBackColor;
		}
		set
		{
			_monthCalendarX.WeekDaysBackColor = value;
		}
	}

	public Font CalendarWeekDaysFont
	{
		get
		{
			return _monthCalendarX.WeekDaysFont;
		}
		set
		{
			_monthCalendarX.WeekDaysFont = value;
		}
	}

	public Color CalendarLineWeekColor
	{
		get
		{
			return _monthCalendarX.LineWeekColor;
		}
		set
		{
			_monthCalendarX.LineWeekColor = value;
		}
	}

	public Font CalendarDaysFont
	{
		get
		{
			return _monthCalendarX.DaysFont;
		}
		set
		{
			_monthCalendarX.DaysFont = value;
		}
	}

	public Color CalendarDaysForeColor
	{
		get
		{
			return _monthCalendarX.DaysForeColor;
		}
		set
		{
			_monthCalendarX.DaysForeColor = value;
		}
	}

	public Color CalendarDaysBackColor
	{
		get
		{
			return _monthCalendarX.DaysBackColor;
		}
		set
		{
			_monthCalendarX.DaysBackColor = value;
		}
	}

	public float CalendarDayRectTickness
	{
		get
		{
			return _monthCalendarX.DayRectTickness;
		}
		set
		{
			_monthCalendarX.DayRectTickness = value;
		}
	}

	public Font CalendarTodayFont
	{
		get
		{
			return _monthCalendarX.TodayFont;
		}
		set
		{
			_monthCalendarX.TodayFont = value;
		}
	}

	public Color CalendarTodayForeColor
	{
		get
		{
			return _monthCalendarX.TodayForeColor;
		}
		set
		{
			_monthCalendarX.TodayForeColor = value;
		}
	}

	public Color CalendarTodayBackColor
	{
		get
		{
			return _monthCalendarX.TodayBackColor;
		}
		set
		{
			_monthCalendarX.TodayBackColor = value;
		}
	}

	public Color CalendarBoldedDayForeColor
	{
		get
		{
			return _monthCalendarX.BoldedDayForeColor;
		}
		set
		{
			_monthCalendarX.BoldedDayForeColor = value;
		}
	}

	public Color CalendarTrailingForeColor
	{
		get
		{
			return _monthCalendarX.TrailingForeColor;
		}
		set
		{
			_monthCalendarX.TrailingForeColor = value;
		}
	}

	public bool CalendarShowTrailing
	{
		get
		{
			return _monthCalendarX.ShowTrailing;
		}
		set
		{
			_monthCalendarX.ShowTrailing = value;
		}
	}

	public Color CalendarTodayRectColor
	{
		get
		{
			return _monthCalendarX.TodayRectColor;
		}
		set
		{
			_monthCalendarX.TodayRectColor = value;
		}
	}

	public bool CalendarShowToday
	{
		get
		{
			return _monthCalendarX.ShowToday;
		}
		set
		{
			_monthCalendarX.ShowToday = value;
		}
	}

	public bool CalendarShowTodayRect
	{
		get
		{
			return _monthCalendarX.ShowTodayRect;
		}
		set
		{
			_monthCalendarX.ShowTodayRect = value;
		}
	}

	public float CalendarTodayRectTickness
	{
		get
		{
			return _monthCalendarX.TodayRectTickness;
		}
		set
		{
			_monthCalendarX.TodayRectTickness = value;
		}
	}

	public MonthCalendarX.WeekDays CalendarWeekStartsOn
	{
		get
		{
			return _monthCalendarX.WeekStartsOn;
		}
		set
		{
			_monthCalendarX.WeekStartsOn = value;
		}
	}

	public Color CalendarHolidayForeColor
	{
		get
		{
			return _monthCalendarX.HolidayForeColor;
		}
		set
		{
			_monthCalendarX.HolidayForeColor = value;
		}
	}

	public MonthCalendarX.DayOfWeekForHoliday CalendarHolidayWeekly
	{
		get
		{
			return _monthCalendarX.HolidayWeekly;
		}
		set
		{
			_monthCalendarX.HolidayWeekly = value;
		}
	}

	public Color CalendarBorderColor
	{
		get
		{
			return _monthCalendarX.BorderColor;
		}
		set
		{
			_monthCalendarX.BorderColor = value;
		}
	}

	public DateTime[] CalendarEnglishBoldedDates
	{
		get
		{
			return _monthCalendarX.EnglishBoldedDates;
		}
		set
		{
			_monthCalendarX.EnglishBoldedDates = value;
		}
	}

	public DateTime[] CalendarEnglishMonthlyBoldedDates
	{
		get
		{
			return _monthCalendarX.EnglishMonthlyBoldedDates;
		}
		set
		{
			_monthCalendarX.EnglishMonthlyBoldedDates = value;
		}
	}

	public DateTime[] CalendarEnglishAnnuallyBoldedDates
	{
		get
		{
			return _monthCalendarX.EnglishAnnuallyBoldedDates;
		}
		set
		{
			_monthCalendarX.EnglishAnnuallyBoldedDates = value;
		}
	}

	public DateTime[] CalendarEnglishHolidayDates
	{
		get
		{
			return _monthCalendarX.EnglishHolidayDates;
		}
		set
		{
			_monthCalendarX.EnglishHolidayDates = value;
		}
	}

	public PersianDateTime[] CalendarPersianBoldedDates
	{
		get
		{
			return _monthCalendarX.PersianBoldedDates;
		}
		set
		{
			_monthCalendarX.PersianBoldedDates = value;
		}
	}

	public PersianDateTime[] CalendarPersianMonthlyBoldedDates
	{
		get
		{
			return _monthCalendarX.PersianMonthlyBoldedDates;
		}
		set
		{
			_monthCalendarX.PersianMonthlyBoldedDates = value;
		}
	}

	public PersianDateTime[] CalendarPersianAnnuallyBoldedDates
	{
		get
		{
			return _monthCalendarX.PersianAnnuallyBoldedDates;
		}
		set
		{
			_monthCalendarX.PersianAnnuallyBoldedDates = value;
		}
	}

	public PersianDateTime[] CalendarPersianHolidayDates
	{
		get
		{
			return _monthCalendarX.PersianHolidayDates;
		}
		set
		{
			_monthCalendarX.PersianHolidayDates = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_NextMonthButton
	{
		get
		{
			return _monthCalendarX.Style_NextMonthButton;
		}
		set
		{
			_monthCalendarX.Style_NextMonthButton = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_PreviousMonthButton
	{
		get
		{
			return _monthCalendarX.Style_PreviousMonthButton;
		}
		set
		{
			_monthCalendarX.Style_PreviousMonthButton = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_MonthButton
	{
		get
		{
			return _monthCalendarX.Style_MonthButton;
		}
		set
		{
			_monthCalendarX.Style_MonthButton = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_YearButton
	{
		get
		{
			return _monthCalendarX.Style_YearButton;
		}
		set
		{
			_monthCalendarX.Style_YearButton = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_DaysButton
	{
		get
		{
			return _monthCalendarX.Style_DaysButton;
		}
		set
		{
			_monthCalendarX.Style_DaysButton = value;
		}
	}

	public ButtonX.ButtonStyles CalendarStyle_GotoTodayButton
	{
		get
		{
			return _monthCalendarX.Style_GotoTodayButton;
		}
		set
		{
			_monthCalendarX.Style_GotoTodayButton = value;
		}
	}

	public bool CalendarShowToolTips
	{
		get
		{
			return _monthCalendarX.ShowToolTips;
		}
		set
		{
			_monthCalendarX.ShowToolTips = value;
		}
	}

	public FormatDate Format
	{
		get
		{
			return _format;
		}
		set
		{
			if (_format != value)
			{
				_format = value;
				ShowSelectedDate(!_UserSelectDate);
			}
		}
	}

	public string CustomFormat
	{
		get
		{
			return _customFormat;
		}
		set
		{
			if (!(_customFormat == value))
			{
				_customFormat = value;
				ShowSelectedDate(!_UserSelectDate);
			}
		}
	}

	public bool DropDownClosedWhenSelectedDateChanged
	{
		get
		{
			return _DropDownClosedWhenSelectedDateChanged;
		}
		set
		{
			_DropDownClosedWhenSelectedDateChanged = value;
		}
	}

	public bool DropDownClosedWhenClickOnDays
	{
		get
		{
			return _DropDownClosedWhenClickOnDays;
		}
		set
		{
			_DropDownClosedWhenClickOnDays = value;
		}
	}

	[Browsable(false)]
	public bool DateIsSelected => _UserSelectDate;

	public string TextWhenClearButtonClicked
	{
		get
		{
			return _ClearDateTimeText;
		}
		set
		{
			if (!(_ClearDateTimeText == value))
			{
				_ClearDateTimeText = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool ClearDateTimeWhenDownDeleteKey
	{
		get
		{
			return _ClearDateTimeWhenDownDeleteKey;
		}
		set
		{
			_ClearDateTimeWhenDownDeleteKey = value;
		}
	}

	public event OnSelectedDateChanged SelectedDateChanged;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		base.AnchorSize = new Size(150, 21);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Name = "DateTimePickerX2";
		((Control)this).Size = new Size(150, 21);
		((Control)this).ResumeLayout(false);
	}

	protected override void OnLoad(EventArgs e)
	{
		((UserControl)this).OnLoad(e);
		ShowSelectedDate(bIsClearDate: false);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 13)
		{
			OpenDropDown();
		}
		else if ((int)e.KeyCode == 46 && base.ShowClearButton && _ClearDateTimeWhenDownDeleteKey)
		{
			ShowSelectedDate(bIsClearDate: true);
		}
	}

	public DateTimePickerX()
	{
		InitializeComponent();
		ConstructDropDownEvents();
		InitMonthCalendar();
	}

	public DateTimePickerX(MonthCalendarX monthCalendarX)
	{
		InitializeComponent();
		_monthCalendarX = new MonthCalendarX(monthCalendarX);
		ConstructDropDownEvents();
		InitMonthCalendar();
	}

	public MonthCalendarX GetMonthCalendar()
	{
		return new MonthCalendarX(_monthCalendarX);
	}

	public void SetMonthCalendar(MonthCalendarX monthCalendarX)
	{
		_monthCalendarX = new MonthCalendarX(monthCalendarX);
		InitMonthCalendar();
		ShowSelectedDate(bIsClearDate: false);
	}

	public PersianDateTime GetSelectedDateInPersianDateTime()
	{
		return _monthCalendarX.GetSelectedDateInPersianDateTime();
	}

	public DateTime GetSelectedDateInDateTime()
	{
		return _monthCalendarX.GetSelectedDateInDateTime();
	}

	public void SetSelectedDate(CalendarTypes type, int year, int month, int day)
	{
		_monthCalendarX.SetSelectedDate(type, year, month, day);
		ShowSelectedDate(bIsClearDate: false);
	}

	public void SetSelectedDate(DateTime date)
	{
		_monthCalendarX.SetSelectedDate(date);
		ShowSelectedDate(bIsClearDate: false);
	}

	public void SetSelectedDate(PersianDateTime date)
	{
		_monthCalendarX.SetSelectedDate(date);
		ShowSelectedDate(bIsClearDate: false);
	}

	public void SetTodayDate(CalendarTypes type, int year, int month, int day)
	{
		_monthCalendarX.SetTodayDate(type, year, month, day);
	}

	public void SetTodayDate(DateTime date)
	{
		_monthCalendarX.SetTodayDate(date);
	}

	public void SetTodayDate(PersianDateTime date)
	{
		_monthCalendarX.SetTodayDate(date);
	}

	private void ConstructDropDownEvents()
	{
		base.OnOpenDropDown += DateTimePicker2X_OnOpenDropDown;
		base.OnCloseDropDown += DateTimePicker2X_OnCloseDropDown;
	}

	private void DateTimePicker2X_OnOpenDropDown(object sender, EventArgs e)
	{
		dtPrevSelected = _monthCalendarX.GetSelectedDateInDateTime();
	}

	private void DateTimePicker2X_OnCloseDropDown(object sender, EventArgs e)
	{
		ShowSelectedDate(bIsClearDate: false);
	}

	private void InitMonthCalendar()
	{
		InitializeDropDown((Control)(object)_monthCalendarX);
		_monthCalendarX.SelectedDateChanged += _monthCalendarX_SelectedDateChanged;
		_monthCalendarX.DayKeyDown += _monthCalendarX_DayKeyDown;
		_monthCalendarX.DayMouseClick += _monthCalendarX_DayMouseClick;
		base.ClearButtonClick += DateTimePickerX_ClearButtonClick;
	}

	private void DateTimePickerX_ClearButtonClick(object sender, EventArgs e)
	{
		ShowSelectedDate(bIsClearDate: true);
	}

	private void ShowSelectedDate(bool bIsClearDate)
	{
		PersianDateTime selectedDateInPersianDateTime = _monthCalendarX.GetSelectedDateInPersianDateTime();
		DateTime selectedDateInDateTime = _monthCalendarX.GetSelectedDateInDateTime();
		string text = "";
		_UserSelectDate = true;
		if (bIsClearDate)
		{
			_UserSelectDate = false;
			text = _ClearDateTimeText;
		}
		else if (Format == FormatDate.Short || (Format == FormatDate.Custom && _customFormat.Trim() == ""))
		{
			text = ((_monthCalendarX.CalendarType != CalendarTypes.Persian) ? selectedDateInDateTime.ToShortDateString() : selectedDateInPersianDateTime.ToShortDateString());
		}
		else if (Format == FormatDate.Long)
		{
			text = ((_monthCalendarX.CalendarType != CalendarTypes.Persian) ? selectedDateInDateTime.ToLongDateString() : selectedDateInPersianDateTime.ToLongDateString());
		}
		else if (Format == FormatDate.Custom)
		{
			text = ((_monthCalendarX.CalendarType != CalendarTypes.Persian) ? selectedDateInDateTime.ToString(CustomFormat) : selectedDateInPersianDateTime.ToStringFormat(CustomFormat));
		}
		((Control)this).Text = text;
	}

	private string GetFormatThis()
	{
		string text = "";
		switch (Format)
		{
		case FormatDate.Short:
			if (CalendarType == CalendarTypes.Persian)
			{
				text = PersianDateTime.ShortDateFormat;
				break;
			}
			try
			{
				CultureInfo cultureInfo = new CultureInfo("en-US");
				text = cultureInfo.DateTimeFormat.ShortDatePattern;
			}
			catch
			{
				text = "M/d/yyyy";
			}
			break;
		case FormatDate.Long:
			if (CalendarType == CalendarTypes.Persian)
			{
				text = PersianDateTime.LongDateFormat;
				break;
			}
			try
			{
				CultureInfo cultureInfo = new CultureInfo("en-US");
				text = cultureInfo.DateTimeFormat.LongDatePattern;
			}
			catch
			{
				text = "dddd, MMMM dd, yyyy";
			}
			break;
		case FormatDate.Custom:
			text = CustomFormat;
			if (!(text.Trim() == ""))
			{
				break;
			}
			if (CalendarType == CalendarTypes.Persian)
			{
				text = PersianDateTime.ShortDateFormat;
				break;
			}
			try
			{
				CultureInfo cultureInfo = new CultureInfo("en-US");
				text = cultureInfo.DateTimeFormat.ShortDatePattern;
			}
			catch
			{
				text = "M/d/yyyy";
			}
			break;
		}
		return text;
	}

	private void _monthCalendarX_DayKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			_monthCalendarX.SetSelectedDate(CalendarTypes.English, dtPrevSelected.Year, dtPrevSelected.Month, dtPrevSelected.Day);
			CloseDropDown();
		}
		else if ((int)e.KeyCode == 13 || (int)e.KeyCode == 13)
		{
			CloseDropDown();
		}
	}

	private void _monthCalendarX_SelectedDateChanged(DateTime selectedDateTime, PersianDateTime selectedPersianDateTime)
	{
		_UserSelectDate = true;
		if (_DropDownClosedWhenSelectedDateChanged)
		{
			CloseDropDown();
		}
		else
		{
			ShowSelectedDate(bIsClearDate: false);
		}
		if (this.SelectedDateChanged != null)
		{
			this.SelectedDateChanged(selectedDateTime, selectedPersianDateTime);
		}
	}

	private void _monthCalendarX_DayMouseClick(MonthCalendarX.DayMouseEventArgs e)
	{
		_UserSelectDate = true;
		if (_DropDownClosedWhenClickOnDays)
		{
			CloseDropDown();
		}
	}

	public void ClearDateTime()
	{
		ShowSelectedDate(bIsClearDate: true);
	}
}
