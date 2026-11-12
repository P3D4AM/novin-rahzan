using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[ToolboxBitmap(typeof(DotNetBarManager), "DateTimeInput.ico")]
[ToolboxItem(false)]
public class DateTimeInput : VisualControlBase, ICommandSource
{
	private DateTimeGroup _DateInputGroup;

	private ButtonItem _PopupItem;

	private MonthCalendarItem _MonthCalendar;

	private TimeSelectorItem _TimeSelector;

	private LabelItem _Spacer;

	private bool _PopupCalendarKeyboardNavigationEnabled = true;

	public static DateTime DateTimeDefaults = DateTime.Today;

	private bool _ShowCheckBox;

	private bool _ShowUpDown;

	private eDateTimePickerFormat _Format = eDateTimePickerFormat.Short;

	private string _CustomFormat = "";

	private static CultureInfo _CurrentCulture = null;

	private bool _LockUpdateChecked = true;

	private InputButtonSettings _ButtonDropDown;

	private InputButtonSettings _ButtonClear;

	private bool _FireValueChangedOnPopupClose;

	private DateTime _CloseTime = DateTime.MinValue;

	private InputButtonSettings _ButtonFreeText;

	private bool _AutoSelectDate;

	private eDateTimeSelectorVisibility _DateTimeSelectorVisibility;

	private eTimeSelectorFormat _TimeSelectorTimeFormat;

	private ICommand _Command;

	private object _CommandParameter;

	private bool _AutoResolveFreeTextEntries = true;

	private bool _AutoOffFreeTextEntry;

	private bool _FreeTextEntryMode;

	private FreeTextEntryBox _FreeTextEntryBox;

	[Browsable(false)]
	public bool IsPopupCalendarOpen
	{
		get
		{
			return _PopupItem.Expanded;
		}
		set
		{
			if (_PopupItem.Expanded != value)
			{
				ToggleCalendarPopup();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool PopupCalendarKeyboardNavigationEnabled
	{
		get
		{
			return _PopupCalendarKeyboardNavigationEnabled;
		}
		set
		{
			_PopupCalendarKeyboardNavigationEnabled = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (_FreeTextEntryBox == null || !((Control)_FreeTextEntryBox).Focused))
			{
				return _DateInputGroup.IsEmpty;
			}
			return false;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return _ShowCheckBox;
		}
		set
		{
			_ShowCheckBox = value;
			OnFormatChanged();
		}
	}

	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return _ShowUpDown;
		}
		set
		{
			_ShowUpDown = value;
			OnFormatChanged();
		}
	}

	[Description("Indicates date time value of the control")]
	[Bindable(BindableSupport.Yes)]
	public DateTime Value
	{
		get
		{
			return _DateInputGroup.Value;
		}
		set
		{
			if (AllowEmptyState && value.Equals(new DateTime(0L)))
			{
				IsEmpty = true;
			}
			else
			{
				_DateInputGroup.Value = value;
			}
		}
	}

	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			ValueObject = value;
		}
	}

	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(DateTimeConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object ValueObject
	{
		get
		{
			if (IsEmpty)
			{
				return null;
			}
			return Value;
		}
		set
		{
			if (AcceptCustomValueObject(value))
			{
				return;
			}
			if (IsNull(value) || (value is string && value as string == ""))
			{
				IsEmpty = true;
				return;
			}
			if (value is DateTime)
			{
				Value = (DateTime)value;
				return;
			}
			if (value is string)
			{
				DateTime result = default(DateTime);
				if (DateTime.TryParse(value.ToString(), out result))
				{
					Value = result;
					return;
				}
				throw new ArgumentException("ValueObject property expects either null/nothing value or DateTime type.");
			}
			throw new ArgumentException("ValueObject property expects either null/nothing value or DateTime type.");
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime[] Values
	{
		get
		{
			return _DateInputGroup.Values;
		}
		set
		{
			_DateInputGroup.Values = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return _DateInputGroup.AllowEmptyState;
		}
		set
		{
			_DateInputGroup.AllowEmptyState = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsEmpty
	{
		get
		{
			return _DateInputGroup.IsEmpty;
		}
		set
		{
			_DateInputGroup.IsEmpty = value;
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return _DateInputGroup.MinDate;
		}
		set
		{
			_DateInputGroup.MinDate = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return _DateInputGroup.MaxDate;
		}
		set
		{
			_DateInputGroup.MaxDate = value;
		}
	}

	[DefaultValue(eDateTimePickerFormat.Short)]
	public eDateTimePickerFormat Format
	{
		get
		{
			return _Format;
		}
		set
		{
			if (_Format != value)
			{
				_Format = value;
				OnFormatChanged();
			}
		}
	}

	[DefaultValue("")]
	[Description("Indicates the custom date/time format string. ")]
	[Localizable(true)]
	public string CustomFormat
	{
		get
		{
			return _CustomFormat;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _CustomFormat)
			{
				_CustomFormat = value;
				OnFormatChanged();
			}
		}
	}

	public static CultureInfo CurrentCulture
	{
		get
		{
			return _CurrentCulture;
		}
		set
		{
			_CurrentCulture = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return _DateInputGroup.AutoOverwrite;
		}
		set
		{
			_DateInputGroup.AutoOverwrite = value;
		}
	}

	private LockUpdateCheckBox LockUpdateCheckBox
	{
		get
		{
			if (_DateInputGroup.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)_DateInputGroup.Items[0];
			}
			return null;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return _LockUpdateChecked;
		}
		set
		{
			if (_LockUpdateChecked != value)
			{
				_LockUpdateChecked = value;
				LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
				if (lockUpdateCheckBox != null)
				{
					lockUpdateCheckBox.Checked = _LockUpdateChecked;
				}
			}
		}
	}

	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down calendar when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _ButtonDropDown;

	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _ButtonClear;

	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => _ButtonFreeText;

	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return _DateInputGroup.IsReadOnly;
		}
		set
		{
			_DateInputGroup.IsReadOnly = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
	public bool DefaultInputValues
	{
		get
		{
			return _DateInputGroup.DefaultInputValues;
		}
		set
		{
			_DateInputGroup.DefaultInputValues = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
	public MonthCalendarItem MonthCalendar => _MonthCalendar;

	protected override Size DefaultSize => new Size(200, base.DefaultSize.Height);

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
	public bool AutoSelectDate
	{
		get
		{
			return _AutoSelectDate;
		}
		set
		{
			_AutoSelectDate = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
	public bool AutoAdvance
	{
		get
		{
			return _DateInputGroup.AutoAdvance;
		}
		set
		{
			if (_DateInputGroup.AutoAdvance != value)
			{
				_DateInputGroup.AutoAdvance = value;
			}
		}
	}

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	public string SelectNextInputCharacters
	{
		get
		{
			return _DateInputGroup.SelectNextInputCharacters;
		}
		set
		{
			if (_DateInputGroup.SelectNextInputCharacters != value)
			{
				_DateInputGroup.SelectNextInputCharacters = value;
			}
		}
	}

	[DefaultValue(eDateTimeSelectorVisibility.Auto)]
	[Category("Behavior")]
	[Description("")]
	public eDateTimeSelectorVisibility DateTimeSelectorVisibility
	{
		get
		{
			return _DateTimeSelectorVisibility;
		}
		set
		{
			if (value != _DateTimeSelectorVisibility)
			{
				eDateTimeSelectorVisibility dateTimeSelectorVisibility = _DateTimeSelectorVisibility;
				_DateTimeSelectorVisibility = value;
				OnDateTimeSelectorVisibilityChanged(dateTimeSelectorVisibility, value);
			}
		}
	}

	[DefaultValue(eTimeSelectorFormat.System)]
	[Category("Appearance")]
	[Description("Indicates popup time selector time format used to present time by the selector i.e. 12H or 24H format.")]
	[Localizable(true)]
	public eTimeSelectorFormat TimeSelectorTimeFormat
	{
		get
		{
			return _TimeSelectorTimeFormat;
		}
		set
		{
			if (value != _TimeSelectorTimeFormat)
			{
				eTimeSelectorFormat timeSelectorTimeFormat = _TimeSelectorTimeFormat;
				_TimeSelectorTimeFormat = value;
				OnTimeSelectorTimeFormatChanged(timeSelectorTimeFormat, value);
			}
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	[DefaultValue(true)]
	[Category("Free-Text")]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return _AutoResolveFreeTextEntries;
		}
		set
		{
			_AutoResolveFreeTextEntries = value;
		}
	}

	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return _AutoOffFreeTextEntry;
		}
		set
		{
			_AutoOffFreeTextEntry = value;
		}
	}

	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether control input is in free-text input mode.")]
	public bool FreeTextEntryMode
	{
		get
		{
			return _FreeTextEntryMode;
		}
		set
		{
			_FreeTextEntryMode = value;
			OnFreeTextEntryModeChanged();
		}
	}

	protected override bool SupportsFreeTextEntry => true;

	protected override bool IsFreeTextEntryVisible
	{
		get
		{
			if (_FreeTextEntryMode)
			{
				return base.IsKeyboardFocusWithin;
			}
			return false;
		}
	}

	public event EventHandler ValueChanged;

	public event EventHandler ValueObjectChanged;

	public event EventHandler FormatChanged;

	public event CancelEventHandler ButtonClearClick;

	public event CancelEventHandler ButtonDropDownClick;

	public event ParseDateTimeValueEventHandler ParseValue;

	public event EventHandler LockUpdateChanged;

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	[Category("Free-Text")]
	public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry;

	public event CancelEventHandler ButtonFreeTextClick;

	public virtual void Copy()
	{
		if (_DateInputGroup != null)
		{
			Clipboard.SetText(((Control)this).Text);
		}
	}

	public virtual void Paste()
	{
		if (_DateInputGroup != null)
		{
			((Control)this).Text = Clipboard.GetText();
		}
	}

	public virtual void Cut()
	{
		if (_DateInputGroup != null)
		{
			Copy();
			if (AllowEmptyState)
			{
				ValueObject = null;
			}
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupClose += DropDownPopupClose;
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.LayoutOrientation = eOrientation.Horizontal;
		itemContainer.BackgroundStyle.Padding = 4;
		MonthCalendarItem monthCalendarItem = new MonthCalendarItem();
		monthCalendarItem.CalendarDimensions = new Size(1, 1);
		monthCalendarItem.DateChanged += PopupSelectedDateChanged;
		itemContainer.SubItems.Add(monthCalendarItem);
		buttonItem.SubItems.Add(itemContainer);
		LabelItem labelItem = new LabelItem();
		labelItem.Width = 8;
		labelItem.AutoCollapseOnClick = false;
		labelItem.Visible = false;
		itemContainer.SubItems.Add(labelItem);
		TimeSelectorItem timeSelectorItem = new TimeSelectorItem();
		timeSelectorItem.SelectorType = eTimeSelectorType.MonthCalendarStyle;
		timeSelectorItem.SelectedTimeChanged += TimeSelectorChanged;
		timeSelectorItem.TimeFormat = _TimeSelectorTimeFormat;
		timeSelectorItem.Visible = false;
		itemContainer.SubItems.Add(timeSelectorItem);
		_PopupItem = buttonItem;
		_MonthCalendar = monthCalendarItem;
		_TimeSelector = timeSelectorItem;
		_Spacer = labelItem;
		UpdateTimeSelectorItemSize();
		return buttonItem;
	}

	private void UpdateTimeSelectorItemSize()
	{
		if (_TimeSelector != null)
		{
			_TimeSelector.HourMinuteButtonSize = new Size(_TimeSelector.HourMinuteButtonSize.Width, (((Control)this).Font.Height <= 15) ? 15 : (((Control)this).Font.Height + 1));
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		UpdateTimeSelectorItemSize();
		base.OnFontChanged(e);
	}

	protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (IsPopupCalendarOpen)
		{
			int num = 0;
			int num2 = WinApi.ToInt(wParam);
			if (_PopupCalendarKeyboardNavigationEnabled)
			{
				switch (num2)
				{
				case 37:
					num = -1;
					break;
				case 39:
					num = 1;
					break;
				case 40:
					num = 7;
					break;
				case 38:
					num = -7;
					break;
				}
			}
			else if (num2 == 13 && _MonthCalendar.SelectedDate != DateTime.MinValue)
			{
				ToggleCalendarPopup();
				return true;
			}
			if (num != 0)
			{
				if (_MonthCalendar.SelectedDate == DateTime.MinValue)
				{
					_MonthCalendar.SelectedDate = DateTime.Today;
				}
				else
				{
					_MonthCalendar.SelectedDate = _MonthCalendar.SelectedDate.AddDays(num);
				}
				if (_MonthCalendar.DisplayMonth.Month != _MonthCalendar.SelectedDate.Month)
				{
					_MonthCalendar.DisplayMonth = _MonthCalendar.SelectedDate;
				}
				return true;
			}
		}
		return base.OnKeyDown(hWnd, wParam, lParam);
	}

	private void TimeSelectorChanged(object sender, EventArgs e)
	{
		DateTime dateTime = Value;
		if (dateTime == DateTime.MinValue)
		{
			dateTime = DateTimeDefaults;
		}
		if (dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue)
		{
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, _TimeSelector.SelectedTime.Hours, _TimeSelector.SelectedTime.Minutes, _TimeSelector.SelectedTime.Seconds);
		}
		if (MinDate != DateTime.MinValue && dateTime < MinDate)
		{
			dateTime = MinDate;
		}
		_DateInputGroup.Value = dateTime;
	}

	private void PopupSelectedDateChanged(object sender, EventArgs e)
	{
		if (_MonthCalendar.SelectedDate == DateTime.MinValue)
		{
			IsEmpty = true;
			return;
		}
		DateTime dateTime = _MonthCalendar.SelectedDate;
		DateTime value = Value;
		if (value != DateTime.MinValue && value != DateTime.MaxValue)
		{
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, value.Hour, value.Minute, value.Second);
		}
		if (MinDate != DateTime.MinValue && dateTime < MinDate)
		{
			dateTime = MinDate;
		}
		_DateInputGroup.Value = dateTime;
	}

	protected override VisualItem CreateRootVisual()
	{
		_ButtonClear = new InputButtonSettings(this);
		_ButtonDropDown = new InputButtonSettings(this);
		_ButtonFreeText = new InputButtonSettings(this);
		_DateInputGroup = new DateTimeGroup();
		_DateInputGroup.IsRootVisual = true;
		_DateInputGroup.ValueChanged += InputGroup_ValueChanged;
		FormatToDateTimeGroup();
		return _DateInputGroup;
	}

	private void InputGroup_ValueChanged(object sender, EventArgs e)
	{
		if (!IsPopupCalendarOpen)
		{
			OnValueChanged(e);
		}
		else
		{
			_FireValueChangedOnPopupClose = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeValue()
	{
		if (!AllowEmptyState && Value.Date == DateTime.Today)
		{
			return false;
		}
		return !_DateInputGroup.IsEmpty;
	}

	public void ResetValue()
	{
		if (AllowEmptyState)
		{
			IsEmpty = true;
		}
		else
		{
			Value = DateTime.Now;
		}
	}

	private bool AcceptCustomValueObject(object value)
	{
		ParseDateTimeValueEventArgs parseDateTimeValueEventArgs = new ParseDateTimeValueEventArgs(value);
		OnParseValue(parseDateTimeValueEventArgs);
		if (parseDateTimeValueEventArgs.IsParsed)
		{
			Value = parseDateTimeValueEventArgs.ParsedValue;
		}
		return parseDateTimeValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseDateTimeValueEventArgs e)
	{
		if (this.ParseValue != null)
		{
			this.ParseValue(this, e);
		}
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
		return !_DateInputGroup.MaxDate.Equals(DateTimeGroup.MaxDateTime);
	}

	public void ResetMaxDate()
	{
		MaxDate = DateTimeGroup.MaxDateTime;
	}

	private void OnFormatChanged()
	{
		FormatToDateTimeGroup();
		((Control)this).Invalidate();
		UpdateSelectorVisibility();
		if (this.FormatChanged != null)
		{
			this.FormatChanged(this, new EventArgs());
		}
	}

	private void FormatToDateTimeGroup()
	{
		if (_CustomFormat.Length > 0 && _Format == eDateTimePickerFormat.Custom)
		{
			ParseFormat(CustomFormat);
		}
		else if (_Format != 0)
		{
			ParseFormat(GetSystemFormatString(_Format));
		}
		else
		{
			_DateInputGroup.Items.Clear();
		}
	}

	private string GetSystemFormatString(eDateTimePickerFormat format)
	{
		return format switch
		{
			eDateTimePickerFormat.Long => GetActiveCulture().DateTimeFormat.LongDatePattern, 
			eDateTimePickerFormat.Short => GetActiveCulture().DateTimeFormat.ShortDatePattern, 
			eDateTimePickerFormat.ShortTime => GetActiveCulture().DateTimeFormat.ShortTimePattern, 
			eDateTimePickerFormat.LongTime => GetActiveCulture().DateTimeFormat.LongTimePattern, 
			_ => "", 
		};
	}

	public static CultureInfo GetActiveCulture()
	{
		if (_CurrentCulture == null)
		{
			return CultureInfo.CurrentCulture;
		}
		return _CurrentCulture;
	}

	private void ParseFormat(string format)
	{
		_DateInputGroup.Items.Clear();
		if (_ShowCheckBox)
		{
			LockUpdateCheckBox lockUpdateCheckBox = new LockUpdateCheckBox();
			_DateInputGroup.Items.Add(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged += LockCheckedChanged;
		}
		if (format.Length == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder(format.Length);
		bool flag = false;
		Stack<VisualGroup> stack = new Stack<VisualGroup>();
		VisualGroup visualGroup = _DateInputGroup;
		for (int i = 0; i < format.Length; i++)
		{
			if (format[i] == '\'')
			{
				flag = !flag;
				continue;
			}
			if (flag)
			{
				stringBuilder.Append(format[i]);
				continue;
			}
			string text = format.Substring(i, Math.Min(4, format.Length - i));
			bool flag2 = false;
			switch (text)
			{
			case "dddd":
			{
				DayLabelItem item3 = new DayLabelItem();
				visualGroup.Items.Add(item3);
				i += 3;
				flag2 = true;
				break;
			}
			case "MMMM":
			{
				MonthNameInput item2 = new MonthNameInput();
				visualGroup.Items.Add(item2);
				i += 3;
				flag2 = true;
				break;
			}
			case "yyyy":
			{
				NumericYearInput item = new NumericYearInput();
				visualGroup.Items.Add(item);
				i += 3;
				flag2 = true;
				break;
			}
			}
			if (!flag2)
			{
				switch (format.Substring(i, Math.Min(3, format.Length - i)))
				{
				case "ddd":
				{
					DayLabelItem dayLabelItem = new DayLabelItem();
					dayLabelItem.UseAbbreviatedNames = true;
					visualGroup.Items.Add(dayLabelItem);
					i += 2;
					flag2 = true;
					break;
				}
				case "MMM":
				{
					MonthNameInput monthNameInput = new MonthNameInput();
					monthNameInput.UseAbbreviatedNames = true;
					visualGroup.Items.Add(monthNameInput);
					i += 2;
					flag2 = true;
					break;
				}
				case "jjj":
				{
					NumericDayOfYearInput numericDayOfYearInput = new NumericDayOfYearInput();
					numericDayOfYearInput.DisplayFormat = "000";
					visualGroup.Items.Add(numericDayOfYearInput);
					i += 2;
					flag2 = true;
					break;
				}
				}
			}
			if (!flag2)
			{
				switch (format.Substring(i, Math.Min(2, format.Length - i)))
				{
				case "dd":
				{
					NumericDayInput numericDayInput = new NumericDayInput();
					numericDayInput.DisplayFormat = "00";
					visualGroup.Items.Add(numericDayInput);
					i++;
					flag2 = true;
					break;
				}
				case "hh":
				{
					NumericHourInput numericHourInput2 = new NumericHourInput();
					numericHourInput2.DisplayFormat = "00";
					numericHourInput2.Is24HourFormat = false;
					visualGroup.Items.Add(numericHourInput2);
					i++;
					flag2 = true;
					break;
				}
				case "HH":
				{
					NumericHourInput numericHourInput = new NumericHourInput();
					numericHourInput.DisplayFormat = "00";
					numericHourInput.Is24HourFormat = true;
					visualGroup.Items.Add(numericHourInput);
					i++;
					flag2 = true;
					break;
				}
				case "mm":
				{
					NumericMinuteInput numericMinuteInput = new NumericMinuteInput();
					numericMinuteInput.DisplayFormat = "00";
					visualGroup.Items.Add(numericMinuteInput);
					i++;
					flag2 = true;
					break;
				}
				case "MM":
				{
					NumericMonthInput numericMonthInput = new NumericMonthInput();
					numericMonthInput.DisplayFormat = "00";
					visualGroup.Items.Add(numericMonthInput);
					i++;
					flag2 = true;
					break;
				}
				case "ss":
				{
					NumericSecondInput numericSecondInput = new NumericSecondInput();
					numericSecondInput.DisplayFormat = "00";
					visualGroup.Items.Add(numericSecondInput);
					i++;
					flag2 = true;
					break;
				}
				case "tt":
				{
					HourPeriodInput item4 = new HourPeriodInput();
					visualGroup.Items.Add(item4);
					i++;
					flag2 = true;
					break;
				}
				case "yy":
				{
					NumericYearInput numericYearInput = new NumericYearInput();
					numericYearInput.YearDisplayFormat = eYearDisplayFormat.TwoDigit;
					visualGroup.Items.Add(numericYearInput);
					i++;
					flag2 = true;
					break;
				}
				}
			}
			if (!flag2)
			{
				switch (format[i].ToString())
				{
				case "d":
				{
					NumericDayInput item5 = new NumericDayInput();
					visualGroup.Items.Add(item5);
					flag2 = true;
					break;
				}
				case "h":
				{
					NumericHourInput numericHourInput3 = new NumericHourInput();
					numericHourInput3.Is24HourFormat = false;
					visualGroup.Items.Add(numericHourInput3);
					flag2 = true;
					break;
				}
				case "H":
				{
					NumericHourInput numericHourInput4 = new NumericHourInput();
					numericHourInput4.Is24HourFormat = true;
					visualGroup.Items.Add(numericHourInput4);
					flag2 = true;
					break;
				}
				case "m":
				{
					NumericMinuteInput item9 = new NumericMinuteInput();
					visualGroup.Items.Add(item9);
					flag2 = true;
					break;
				}
				case "M":
				{
					NumericMonthInput item8 = new NumericMonthInput();
					visualGroup.Items.Add(item8);
					flag2 = true;
					break;
				}
				case "s":
				{
					NumericSecondInput item7 = new NumericSecondInput();
					visualGroup.Items.Add(item7);
					flag2 = true;
					break;
				}
				case "t":
				{
					HourPeriodInput hourPeriodInput = new HourPeriodInput();
					hourPeriodInput.UseSingleLetterLabel = true;
					visualGroup.Items.Add(hourPeriodInput);
					flag2 = true;
					break;
				}
				case "y":
				{
					NumericYearInput numericYearInput2 = new NumericYearInput();
					numericYearInput2.YearDisplayFormat = eYearDisplayFormat.OneDigit;
					visualGroup.Items.Add(numericYearInput2);
					flag2 = true;
					break;
				}
				case "j":
				{
					NumericDayOfYearInput item6 = new NumericDayOfYearInput();
					visualGroup.Items.Add(item6);
					flag2 = true;
					break;
				}
				case "{":
				{
					if (stringBuilder.Length > 0)
					{
						VisualLabel visualLabel = new VisualLabel();
						visualLabel.Text = stringBuilder.ToString();
						visualGroup.Items.Add(visualLabel);
						stringBuilder = new StringBuilder(format.Length);
					}
					DateTimeGroup dateTimeGroup = new DateTimeGroup();
					visualGroup.Items.Add(dateTimeGroup);
					stack.Push(visualGroup);
					visualGroup = dateTimeGroup;
					flag2 = true;
					break;
				}
				case "}":
					visualGroup = stack.Pop();
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				if (stringBuilder.Length > 0)
				{
					VisualLabel visualLabel2 = new VisualLabel();
					visualLabel2.Text = stringBuilder.ToString();
					visualGroup.Items.Insert(visualGroup.Items.Count - 1, visualLabel2);
					stringBuilder = new StringBuilder(format.Length);
				}
			}
			else
			{
				stringBuilder.Append(format[i]);
			}
		}
		if (stringBuilder.Length > 0)
		{
			VisualLabel visualLabel3 = new VisualLabel();
			visualLabel3.Text = stringBuilder.ToString();
			visualGroup.Items.Add(visualLabel3);
		}
		if (_ShowUpDown)
		{
			VisualUpDownButton visualUpDownButton = new VisualUpDownButton();
			visualUpDownButton.Alignment = eItemAlignment.Right;
			visualUpDownButton.AutoChange = eUpDownButtonAutoChange.FocusedItem;
			_DateInputGroup.Items.Add(visualUpDownButton);
		}
		RecreateButtons();
		if (_ShowCheckBox)
		{
			LockUpdateCheckBox.Checked = _LockUpdateChecked;
		}
	}

	private void LockCheckedChanged(object sender, EventArgs e)
	{
		LockUpdateCheckBox lockUpdateCheckBox = LockUpdateCheckBox;
		if (lockUpdateCheckBox != null)
		{
			_LockUpdateChecked = lockUpdateCheckBox.Checked;
		}
		OnLockUpdateChanged(e);
	}

	protected virtual void OnLockUpdateChanged(EventArgs e)
	{
		if (this.LockUpdateChanged != null)
		{
			this.LockUpdateChanged(this, e);
		}
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (IsInitializing)
		{
			return;
		}
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, e);
		}
		if (this.ValueObjectChanged != null)
		{
			this.ValueObjectChanged(this, e);
		}
		if (IsEmpty)
		{
			base.Text = "";
		}
		else
		{
			string text = "";
			text = ((Format != 0) ? GetSystemFormatString(Format) : CustomFormat);
			if (text == "")
			{
				text = GetSystemFormatString(eDateTimePickerFormat.Short);
			}
			if (text == "")
			{
				base.Text = Value.ToString();
			}
			else
			{
				try
				{
					base.Text = Value.ToString(text);
				}
				catch
				{
					base.Text = "";
				}
			}
		}
		if (FreeTextEntryMode && _FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
		ExecuteCommand();
	}

	protected override void OnValidating(CancelEventArgs e)
	{
		_DateInputGroup.UpdateValue(updateDirect: false);
		((Control)this).OnValidating(e);
	}

	protected override SortedList CreateSortedButtonList()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		SortedList sortedList = base.CreateSortedButtonList();
		if (_ButtonClear.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonClear);
			if (_ButtonClear.ItemReference != null)
			{
				_ButtonClear.ItemReference.Click -= ClearButtonClick;
			}
			_ButtonClear.ItemReference = visualItem;
			visualItem.Click += ClearButtonClick;
			sortedList.Add(_ButtonClear, visualItem);
		}
		if (_ButtonDropDown.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonDropDown);
			if (_ButtonDropDown.ItemReference != null)
			{
				_ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
				_ButtonDropDown.ItemReference.Click -= DropDownButtonClick;
			}
			_ButtonDropDown.ItemReference = visualItem2;
			visualItem2.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
			visualItem2.Click += DropDownButtonClick;
			sortedList.Add(_ButtonDropDown, visualItem2);
		}
		if (_ButtonFreeText.Visible)
		{
			VisualItem visualItem3 = CreateButton(_ButtonFreeText);
			if (_ButtonFreeText.ItemReference != null)
			{
				_ButtonFreeText.ItemReference.Click -= FreeTextButtonClick;
			}
			_ButtonFreeText.ItemReference = visualItem3;
			visualItem3.Click += FreeTextButtonClick;
			sortedList.Add(_ButtonFreeText, visualItem3);
		}
		return sortedList;
	}

	private void DropDownButtonClick(object sender, EventArgs e)
	{
		if (e is KeyEventArgs)
		{
			ToggleCalendarPopup();
		}
	}

	private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576 || (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150.0))
		{
			_CloseTime = DateTime.MinValue;
		}
		else
		{
			ToggleCalendarPopup();
		}
	}

	private void ToggleCalendarPopup()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			_PopupItem.SetDisplayRectangle(((Control)this).ClientRectangle);
			_MonthCalendar.ReloadLocalizedStrings();
			if (((Control)this).Font.Height > _MonthCalendar.DaySize.Height)
			{
				_MonthCalendar.DaySize = new Size((int)Math.Ceiling((float)((Control)this).Font.Height * 1.6f), ((Control)this).Font.Height + 1);
			}
			if ((int)((Control)this).RightToLeft == 0)
			{
				_PopupItem.PopupLocation = new Point(((Control)this).Width - _PopupItem.PopupSize.Width, ((Control)this).Height);
			}
			if (MinDate != DateTime.MinValue)
			{
				_MonthCalendar.MinDate = MinDate.Date;
			}
			else
			{
				_MonthCalendar.MinDate = MinDate;
			}
			_MonthCalendar.MaxDate = MaxDate.Date;
			if (!IsEmpty)
			{
				_MonthCalendar.SelectedDate = Value;
				_TimeSelector.SelectedDateTime = Value;
			}
			else
			{
				_MonthCalendar.SelectedDate = DateTime.MinValue;
				_TimeSelector.SelectedTime = TimeSpan.Zero;
			}
			if (_MonthCalendar.SelectedDate != DateTime.MinValue)
			{
				_MonthCalendar.DisplayMonth = _MonthCalendar.SelectedDate;
			}
			else
			{
				_MonthCalendar.DisplayMonth = DateTime.Today;
			}
			_PopupItem.Expanded = !_PopupItem.Expanded;
		}
	}

	private void DropDownPopupClose(object sender, EventArgs e)
	{
		if (_FireValueChangedOnPopupClose)
		{
			OnValueChanged(EventArgs.Empty);
		}
		_FireValueChangedOnPopupClose = false;
		_CloseTime = DateTime.Now;
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			IsEmpty = true;
		}
	}

	protected override VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == _ButtonDropDown)
		{
			visualItem = new VisualDropDownButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		else
		{
			visualItem = base.CreateButton(buttonSettings);
		}
		VisualButton visualButton = visualItem as VisualButton;
		visualButton.ClickAutoRepeat = false;
		if (buttonSettings == _ButtonClear)
		{
			if (buttonSettings.Image == null)
			{
				visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.DateReset.png");
			}
		}
		else if (buttonSettings == _ButtonFreeText)
		{
			if (buttonSettings.Image == null)
			{
				visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.FreeText.png");
			}
			visualButton.Checked = buttonSettings.Checked;
		}
		return visualItem;
	}

	protected virtual void OnButtonClearClick(CancelEventArgs e)
	{
		if (this.ButtonClearClick != null)
		{
			this.ButtonClearClick(this, e);
		}
	}

	protected virtual void OnButtonDropDownClick(CancelEventArgs e)
	{
		if (this.ButtonDropDownClick != null)
		{
			this.ButtonDropDownClick(this, e);
		}
	}

	private void UpdateSelectorVisibility()
	{
		bool flag = false;
		bool flag2 = true;
		if (_DateTimeSelectorVisibility == eDateTimeSelectorVisibility.Auto)
		{
			if (_Format == eDateTimePickerFormat.LongTime || _Format == eDateTimePickerFormat.ShortTime)
			{
				flag = true;
				flag2 = false;
			}
		}
		else if (_DateTimeSelectorVisibility == eDateTimeSelectorVisibility.TimeSelector)
		{
			flag = true;
			flag2 = false;
		}
		else if (_DateTimeSelectorVisibility == eDateTimeSelectorVisibility.Both)
		{
			flag = true;
		}
		_MonthCalendar.Visible = flag2;
		_TimeSelector.Visible = flag;
		if (flag2 && flag)
		{
			_Spacer.Visible = true;
			_MonthCalendar.DayClickAutoClosePopup = false;
		}
		else
		{
			_Spacer.Visible = false;
			_MonthCalendar.DayClickAutoClosePopup = true;
		}
	}

	protected virtual void OnDateTimeSelectorVisibilityChanged(eDateTimeSelectorVisibility oldValue, eDateTimeSelectorVisibility newValue)
	{
		UpdateSelectorVisibility();
	}

	protected virtual void OnTimeSelectorTimeFormatChanged(eTimeSelectorFormat oldValue, eTimeSelectorFormat newValue)
	{
		if (_TimeSelector != null)
		{
			_TimeSelector.TimeFormat = _TimeSelectorTimeFormat;
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}

	private void FreeTextButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonFreeTextClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			_ButtonFreeText.Checked = FreeTextEntryMode;
			if (FreeTextEntryMode && _FreeTextEntryBox != null && !((Control)_FreeTextEntryBox).Focused)
			{
				((Control)_FreeTextEntryBox).Focus();
			}
		}
	}

	protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
	{
		this.ButtonFreeTextClick?.Invoke(this, e);
	}

	private void OnFreeTextEntryModeChanged()
	{
		if (!_FreeTextEntryMode)
		{
			if (_FreeTextEntryBox != null)
			{
				_FreeTextEntryBox.ApplyValue -= ApplyFreeTextValue;
				_FreeTextEntryBox.RevertValue -= RevertFreeTextValue;
				((Control)_FreeTextEntryBox).LostFocus -= FreeTextLostFocus;
				((Control)this).Controls.Remove((Control)(object)_FreeTextEntryBox);
				((Component)(object)_FreeTextEntryBox).Dispose();
				_FreeTextEntryBox = null;
			}
		}
		else
		{
			UpdateFreeTextBoxVisibility();
		}
		if (_ButtonFreeText != null)
		{
			_ButtonFreeText.Checked = _FreeTextEntryMode;
		}
	}

	protected override void OnIsKeyboardFocusWithinChanged()
	{
		if (_FreeTextEntryMode)
		{
			UpdateFreeTextBoxVisibility();
			if (base.IsKeyboardFocusWithin)
			{
				Control freeTextBox = GetFreeTextBox();
				if (!freeTextBox.Focused)
				{
					freeTextBox.Focus();
				}
				if (FocusHighlightEnabled)
				{
					freeTextBox.BackColor = FocusHighlightColor;
				}
			}
			else if (FocusHighlightEnabled)
			{
				Control freeTextBox2 = GetFreeTextBox();
				freeTextBox2.BackColor = SystemColors.Window;
			}
		}
		base.OnIsKeyboardFocusWithinChanged();
	}

	private void UpdateFreeTextBoxVisibility()
	{
		FreeTextEntryBox freeTextEntryBox = (FreeTextEntryBox)(object)GetFreeTextBox();
		if (base.IsKeyboardFocusWithin)
		{
			((Control)freeTextEntryBox).Visible = true;
			((Control)freeTextEntryBox).Text = ((Control)this).Text;
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
		else if (((Control)freeTextEntryBox).Visible)
		{
			if (((Control)freeTextEntryBox).Focused)
			{
				freeTextEntryBox.HideOnLostFocus();
			}
			else
			{
				((Control)freeTextEntryBox).Visible = false;
			}
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
	}

	protected override Control GetFreeTextBox()
	{
		if (_FreeTextEntryBox == null)
		{
			_FreeTextEntryBox = new FreeTextEntryBox();
			_FreeTextEntryBox.ApplyValue += ApplyFreeTextValue;
			_FreeTextEntryBox.RevertValue += RevertFreeTextValue;
			((Control)_FreeTextEntryBox).LostFocus += FreeTextLostFocus;
			((TextBoxBase)_FreeTextEntryBox).BorderStyle = (BorderStyle)0;
			((Control)this).Controls.Add((Control)(object)_FreeTextEntryBox);
		}
		return (Control)(object)_FreeTextEntryBox;
	}

	private void RevertFreeTextValue(object sender, EventArgs e)
	{
		if (_FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Text = ((Control)this).Text;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		this.ConvertFreeTextEntry?.Invoke(this, e);
	}

	private bool ParseFreeTextValue(out DateTime date)
	{
		try
		{
			return ((Format == eDateTimePickerFormat.Custom && DateTime.TryParseExact(((Control)_FreeTextEntryBox).Text, CustomFormat, null, DateTimeStyles.None, out date)) || DateTime.TryParse(((Control)_FreeTextEntryBox).Text, out date)) && _AutoResolveFreeTextEntries;
		}
		catch
		{
			date = DateTime.MinValue;
			return false;
		}
	}

	private void ApplyFreeTextValue(object sender, EventArgs e)
	{
		if (_FreeTextEntryBox == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)_FreeTextEntryBox).Text))
		{
			ValueObject = null;
			return;
		}
		if (ParseFreeTextValue(out var date))
		{
			Value = date;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)_FreeTextEntryBox).Text);
		OnConvertFreeTextEntry(freeTextEntryConversionEventArgs);
		if (freeTextEntryConversionEventArgs.IsValueConverted)
		{
			if (freeTextEntryConversionEventArgs.ControlValue is DateTime)
			{
				Value = (DateTime)freeTextEntryConversionEventArgs.ControlValue;
				return;
			}
			if (freeTextEntryConversionEventArgs.ControlValue != null)
			{
				throw new ArgumentException("ControlValue assigned is not DateTime type.");
			}
			ValueObject = null;
		}
		else if (_AutoResolveFreeTextEntries)
		{
			date = DateTime.MinValue;
			switch (((Control)_FreeTextEntryBox).Text.ToLower())
			{
			case "now":
				date = DateTime.Now;
				break;
			case "today":
				date = DateTime.Today;
				break;
			case "tomorrow":
				date = DateTime.Today.AddDays(1.0);
				break;
			case "yesterday":
				date = DateTime.Today.AddDays(-1.0);
				break;
			}
			if (date == DateTime.MinValue)
			{
				ValueObject = null;
			}
			else
			{
				Value = date;
			}
		}
	}

	private void FreeTextLostFocus(object sender, EventArgs e)
	{
		if (_AutoOffFreeTextEntry && !base.IsKeyboardFocusWithin)
		{
			FreeTextEntryMode = false;
		}
	}

	protected override void HideFreeTextBoxEntry()
	{
		if (_FreeTextEntryBox != null)
		{
			((Control)_FreeTextEntryBox).Visible = false;
		}
	}
}
