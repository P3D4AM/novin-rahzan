using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

[DefaultProperty("ValueObject")]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(DotNetBarManager), "DateTimeInput.ico")]
[Designer("DevComponents.DotNetBar.Design.DateTimeInputDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultBindingProperty("ValueObject")]
public class DateTimeInput : VisualControlBase, ICommandSource
{
	private DateTimeGroup __17DF;

	private ButtonItem _០;

	private MonthCalendarItem _១;

	private TimeSelectorItem _២;

	private LabelItem _៣;

	private EventHandler ᗲ;

	private EventHandler _៤;

	private EventHandler _៥;

	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private ParseDateTimeValueEventHandler _៨;

	private EventHandler _៩;

	private bool __17EA = true;

	public static DateTime DateTimeDefaults = DateTime.Today;

	private bool __17EB;

	private bool __17EC;

	private eDateTimePickerFormat __17ED = eDateTimePickerFormat.Short;

	private string __17EE = "";

	private static CultureInfo __17EF = null;

	private bool __17F0 = true;

	private InputButtonSettings __17F1;

	private InputButtonSettings __17F2;

	private bool __17F3;

	private DateTime __17F4 = DateTime.MinValue;

	private InputButtonSettings __17F5;

	private bool __17F6;

	private bool __17F7;

	private eDateTimeSelectorVisibility __17F8;

	private eTimeSelectorFormat __17F9;

	private eTimeSelectorType __17FA;

	private ICommand _૦;

	private object _૧;

	private FreeTextEntryConversionEventHandler _17FB;

	private CancelEventHandler _17FC;

	private bool __17FD = true;

	private bool __17FE;

	private bool __17FF;

	private _232C __1800;

	[Browsable(false)]
	public bool IsPopupCalendarOpen
	{
		get
		{
			return _០.Expanded;
		}
		set
		{
			if (_០.Expanded != value)
			{
				_180D();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool PopupCalendarKeyboardNavigationEnabled
	{
		get
		{
			return __17EA;
		}
		set
		{
			__17EA = value;
		}
	}

	protected override bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused && (__1800 == null || !((Control)__1800).Focused))
			{
				return __17DF.IsEmpty;
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
			return __17EB;
		}
		set
		{
			__17EB = value;
			_1806();
		}
	}

	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return __17EC;
		}
		set
		{
			__17EC = value;
			_1806();
		}
	}

	[Bindable(BindableSupport.Yes)]
	[Description("Indicates date time value of the control")]
	public DateTime Value
	{
		get
		{
			return __17DF.Value;
		}
		set
		{
			if (AllowEmptyState && value.Equals(new DateTime(0L)))
			{
				IsEmpty = true;
			}
			else
			{
				__17DF.Value = value;
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(DateTimeConverter))]
	[RefreshProperties(RefreshProperties.All)]
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
			if (_1805(value))
			{
				return;
			}
			if (IsNull(value) || (value is string && value == ""))
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
			return __17DF.Values;
		}
		set
		{
			__17DF.Values = value;
		}
	}

	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	[DefaultValue(true)]
	public bool AllowEmptyState
	{
		get
		{
			return __17DF.AllowEmptyState;
		}
		set
		{
			__17DF.AllowEmptyState = value;
			((Control)this).Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			return __17DF.IsEmpty;
		}
		set
		{
			__17DF.IsEmpty = value;
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return __17DF.MinDate;
		}
		set
		{
			__17DF.MinDate = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return __17DF.MaxDate;
		}
		set
		{
			__17DF.MaxDate = value;
		}
	}

	[DefaultValue(eDateTimePickerFormat.Short)]
	public eDateTimePickerFormat Format
	{
		get
		{
			return __17ED;
		}
		set
		{
			if (__17ED != value)
			{
				__17ED = value;
				_1806();
			}
		}
	}

	[Description("Indicates the custom date/time format string. ")]
	[DefaultValue("")]
	[Localizable(true)]
	public string CustomFormat
	{
		get
		{
			return __17EE;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != __17EE)
			{
				__17EE = value;
				_1806();
			}
		}
	}

	public static CultureInfo CurrentCulture
	{
		get
		{
			return __17EF;
		}
		set
		{
			__17EF = value;
		}
	}

	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	[DefaultValue(true)]
	public bool AutoOverwrite
	{
		get
		{
			return __17DF.AutoOverwrite;
		}
		set
		{
			__17DF.AutoOverwrite = value;
		}
	}

	private LockUpdateCheckBox _17DE
	{
		get
		{
			if (__17DF.Items[0] is LockUpdateCheckBox)
			{
				return (LockUpdateCheckBox)__17DF.Items[0];
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
			return __17F0;
		}
		set
		{
			if (__17F0 != value)
			{
				__17F0 = value;
				LockUpdateCheckBox lockUpdateCheckBox = _17DE;
				if (lockUpdateCheckBox != null)
				{
					lockUpdateCheckBox.Checked = __17F0;
				}
			}
		}
	}

	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that shows drop-down calendar when clicked.")]
	public InputButtonSettings ButtonDropDown => __17F1;

	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	public InputButtonSettings ButtonClear => __17F2;

	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => __17F5;

	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return __17DF.IsReadOnly;
		}
		set
		{
			__17DF.IsReadOnly = value;
			if (__1800 != null)
			{
				((TextBoxBase)__1800).ReadOnly = value;
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
	[Category("Behavior")]
	public bool DefaultInputValues
	{
		get
		{
			return __17DF.DefaultInputValues;
		}
		set
		{
			__17DF.DefaultInputValues = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
	public MonthCalendarItem MonthCalendar => _១;

	protected override Size DefaultSize => new Size(200, base.DefaultSize.Height);

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
	[Category("Behavior")]
	public bool AutoSelectDate
	{
		get
		{
			return __17F6;
		}
		set
		{
			__17F6 = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
	[DefaultValue(false)]
	public bool AutoAdvance
	{
		get
		{
			return __17DF.AutoAdvance;
		}
		set
		{
			if (__17DF.AutoAdvance != value)
			{
				__17DF.AutoAdvance = value;
			}
		}
	}

	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	[DefaultValue("")]
	public string SelectNextInputCharacters
	{
		get
		{
			return __17DF.SelectNextInputCharacters;
		}
		set
		{
			if (__17DF.SelectNextInputCharacters != value)
			{
				__17DF.SelectNextInputCharacters = value;
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
			return __17F8;
		}
		set
		{
			if (value != __17F8)
			{
				eDateTimeSelectorVisibility oldValue = __17F8;
				__17F8 = value;
				OnDateTimeSelectorVisibilityChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public TimeSelectorItem TimeSelectorItem => _២;

	[Description("Indicates popup time selector time format used to present time by the selector i.e. 12H or 24H format.")]
	[Localizable(true)]
	[DefaultValue(eTimeSelectorFormat.System)]
	[Category("Appearance")]
	public eTimeSelectorFormat TimeSelectorTimeFormat
	{
		get
		{
			return __17F9;
		}
		set
		{
			if (value != __17F9)
			{
				eTimeSelectorFormat oldValue = __17F9;
				__17F9 = value;
				OnTimeSelectorTimeFormatChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the type of popup time selector used.")]
	[DefaultValue(eTimeSelectorType.MonthCalendarStyle)]
	[Category("Appearance")]
	public eTimeSelectorType TimeSelectorType
	{
		get
		{
			return __17FA;
		}
		set
		{
			if (value != __17FA)
			{
				eTimeSelectorType oldValue = __17FA;
				__17FA = value;
				OnTimeSelectorTypeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
	[Category("Commands")]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
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

	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	[Category("Commands")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	[Category("Free-Text")]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
	[DefaultValue(true)]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return __17FD;
		}
		set
		{
			__17FD = value;
		}
	}

	[Category("Free-Text")]
	[DefaultValue(false)]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return __17FE;
		}
		set
		{
			__17FE = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether control input is in free-text input mode.")]
	[Category("Free-Text")]
	public bool FreeTextEntryMode
	{
		get
		{
			return __17FF;
		}
		set
		{
			__17FF = value;
			_᠒();
		}
	}

	protected override bool SupportsFreeTextEntry => true;

	protected override bool IsFreeTextEntryVisible
	{
		get
		{
			if (__17FF)
			{
				return base.IsKeyboardFocusWithin;
			}
			return false;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ValueObjectChanged
	{
		add
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler FormatChanged
	{
		add
		{
			EventHandler eventHandler = _៥;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៥, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៥;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៥, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelEventHandler ButtonClearClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event CancelEventHandler ButtonDropDownClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event ParseDateTimeValueEventHandler ParseValue
	{
		add
		{
			ParseDateTimeValueEventHandler parseDateTimeValueEventHandler = _៨;
			ParseDateTimeValueEventHandler parseDateTimeValueEventHandler2;
			do
			{
				parseDateTimeValueEventHandler2 = parseDateTimeValueEventHandler;
				ParseDateTimeValueEventHandler value2 = (ParseDateTimeValueEventHandler)Delegate.Combine(parseDateTimeValueEventHandler2, value);
				parseDateTimeValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseDateTimeValueEventHandler2);
			}
			while ((object)parseDateTimeValueEventHandler != parseDateTimeValueEventHandler2);
		}
		remove
		{
			ParseDateTimeValueEventHandler parseDateTimeValueEventHandler = _៨;
			ParseDateTimeValueEventHandler parseDateTimeValueEventHandler2;
			do
			{
				parseDateTimeValueEventHandler2 = parseDateTimeValueEventHandler;
				ParseDateTimeValueEventHandler value2 = (ParseDateTimeValueEventHandler)Delegate.Remove(parseDateTimeValueEventHandler2, value);
				parseDateTimeValueEventHandler = Interlocked.CompareExchange(ref _៨, value2, parseDateTimeValueEventHandler2);
			}
			while ((object)parseDateTimeValueEventHandler != parseDateTimeValueEventHandler2);
		}
	}

	public event EventHandler LockUpdateChanged
	{
		add
		{
			EventHandler eventHandler = _៩;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៩, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៩;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៩, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Free-Text")]
	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects.")]
	public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry
	{
		add
		{
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler = _17FB;
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler2;
			do
			{
				freeTextEntryConversionEventHandler2 = freeTextEntryConversionEventHandler;
				FreeTextEntryConversionEventHandler value2 = (FreeTextEntryConversionEventHandler)Delegate.Combine(freeTextEntryConversionEventHandler2, value);
				freeTextEntryConversionEventHandler = Interlocked.CompareExchange(ref _17FB, value2, freeTextEntryConversionEventHandler2);
			}
			while ((object)freeTextEntryConversionEventHandler != freeTextEntryConversionEventHandler2);
		}
		remove
		{
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler = _17FB;
			FreeTextEntryConversionEventHandler freeTextEntryConversionEventHandler2;
			do
			{
				freeTextEntryConversionEventHandler2 = freeTextEntryConversionEventHandler;
				FreeTextEntryConversionEventHandler value2 = (FreeTextEntryConversionEventHandler)Delegate.Remove(freeTextEntryConversionEventHandler2, value);
				freeTextEntryConversionEventHandler = Interlocked.CompareExchange(ref _17FB, value2, freeTextEntryConversionEventHandler2);
			}
			while ((object)freeTextEntryConversionEventHandler != freeTextEntryConversionEventHandler2);
		}
	}

	public event CancelEventHandler ButtonFreeTextClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _17FC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _17FC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _17FC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _17FC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public virtual void Copy()
	{
		if (__17DF != null)
		{
			Clipboard.SetText(((Control)this).Text);
		}
	}

	public virtual void Paste()
	{
		if (__17DF != null)
		{
			((Control)this).Text = Clipboard.GetText();
		}
	}

	public virtual void Cut()
	{
		if (__17DF != null)
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
		buttonItem.PopupClose += _180E;
		ItemContainer itemContainer = new ItemContainer();
		itemContainer.LayoutOrientation = eOrientation.Horizontal;
		itemContainer.BackgroundStyle.Padding = 4;
		MonthCalendarItem monthCalendarItem = new MonthCalendarItem();
		monthCalendarItem.CalendarDimensions = new Size(1, 1);
		monthCalendarItem.DateChanged += _1803;
		itemContainer.SubItems.Add(monthCalendarItem);
		buttonItem.SubItems.Add(itemContainer);
		LabelItem labelItem = new LabelItem();
		labelItem.Width = 8;
		labelItem.AutoCollapseOnClick = false;
		labelItem.Visible = false;
		itemContainer.SubItems.Add(labelItem);
		TimeSelectorItem timeSelectorItem = new TimeSelectorItem();
		timeSelectorItem.SelectorType = __17FA;
		timeSelectorItem.SelectedTimeChanged += _1802;
		timeSelectorItem.TimeFormat = __17F9;
		timeSelectorItem.Visible = false;
		itemContainer.SubItems.Add(timeSelectorItem);
		_០ = buttonItem;
		_១ = monthCalendarItem;
		_២ = timeSelectorItem;
		_៣ = labelItem;
		_1801();
		return buttonItem;
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.ScaleControl(factor, specified);
		_០.NotifyScaleItem(factor);
		_1801();
	}

	private void _1801()
	{
		if (_២ != null)
		{
			_២._265A = new Size(_២._265A.Width, (((Control)this).Font.Height <= 15) ? 15 : (Dpi.DescaleHeight(((Control)this).Font.Height) + 1));
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_1801();
		base.OnFontChanged(e);
	}

	protected override bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (IsPopupCalendarOpen)
		{
			int num = 0;
			int num2 = _0EFE._0F11(_੦);
			if (__17EA)
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
			else if (num2 == 13 && _១.SelectedDate != DateTime.MinValue)
			{
				_180D();
				return true;
			}
			if (num != 0)
			{
				if (_១.SelectedDate == DateTime.MinValue)
				{
					_១.SelectedDate = DateTime.Today;
				}
				else
				{
					_១.SelectedDate = _១.SelectedDate.AddDays(num);
				}
				if (_១.DisplayMonth.Month != _១.SelectedDate.Month)
				{
					_១.DisplayMonth = _១.SelectedDate;
				}
				return true;
			}
		}
		return base._੪(_0A65, _੦, _੧);
	}

	private void _1802(object _٠, EventArgs ؿ)
	{
		DateTime dateTime = Value;
		if (dateTime == DateTime.MinValue)
		{
			dateTime = DateTimeDefaults;
		}
		if (dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue)
		{
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, _២.SelectedTime.Hours, _២.SelectedTime.Minutes, _២.SelectedTime.Seconds);
		}
		if (MinDate != DateTime.MinValue && dateTime < MinDate)
		{
			dateTime = MinDate;
		}
		__17DF.Value = dateTime;
	}

	private void _1803(object _٠, EventArgs ؿ)
	{
		if (_១.SelectedDate == DateTime.MinValue)
		{
			IsEmpty = true;
			return;
		}
		DateTime dateTime = _១.SelectedDate;
		DateTime value = Value;
		if (value != DateTime.MinValue && value != DateTime.MaxValue)
		{
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, value.Hour, value.Minute, value.Second);
		}
		if (MinDate != DateTime.MinValue && dateTime < MinDate)
		{
			dateTime = MinDate;
		}
		__17DF.Value = dateTime;
	}

	protected override VisualItem CreateRootVisual()
	{
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		__17F5 = new InputButtonSettings(this);
		__17DF = new DateTimeGroup();
		__17DF.IsRootVisual = true;
		__17DF.ValueChanged += _1804;
		_1807();
		return __17DF;
	}

	private void _1804(object _٠, EventArgs ؿ)
	{
		if (!IsPopupCalendarOpen)
		{
			OnValueChanged(ؿ);
		}
		else
		{
			__17F3 = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeValue()
	{
		if (!AllowEmptyState && Value.Date == DateTime.Today)
		{
			return false;
		}
		return !__17DF.IsEmpty;
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

	private bool _1805(object ԝ)
	{
		ParseDateTimeValueEventArgs parseDateTimeValueEventArgs = new ParseDateTimeValueEventArgs(ԝ);
		OnParseValue(parseDateTimeValueEventArgs);
		if (parseDateTimeValueEventArgs.IsParsed)
		{
			Value = parseDateTimeValueEventArgs.ParsedValue;
		}
		return parseDateTimeValueEventArgs.IsParsed;
	}

	protected virtual void OnParseValue(ParseDateTimeValueEventArgs e)
	{
		if (_៨ != null)
		{
			_៨(this, e);
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
		return !__17DF.MaxDate.Equals(DateTimeGroup.MaxDateTime);
	}

	public void ResetMaxDate()
	{
		MaxDate = DateTimeGroup.MaxDateTime;
	}

	private void _1806()
	{
		_1807();
		((Control)this).Invalidate();
		_᠐();
		if (_៥ != null)
		{
			_៥(this, new EventArgs());
		}
	}

	private void _1807()
	{
		if (__17EE.Length > 0 && __17ED == eDateTimePickerFormat.Custom)
		{
			_1809(CustomFormat);
		}
		else if (__17ED != 0)
		{
			_1809(_1808(__17ED));
		}
		else
		{
			__17DF.Items.Clear();
		}
	}

	private string _1808(eDateTimePickerFormat _0EE9)
	{
		return _0EE9 switch
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
		if (__17EF == null)
		{
			return CultureInfo.CurrentCulture;
		}
		return __17EF;
	}

	private void _1809(string _0EE9)
	{
		__17DF.Items.Clear();
		if (__17EB)
		{
			LockUpdateCheckBox lockUpdateCheckBox = new LockUpdateCheckBox();
			__17DF.Items.Add(lockUpdateCheckBox);
			lockUpdateCheckBox.CheckedChanged += _180A;
		}
		if (_0EE9.Length == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder(_0EE9.Length);
		bool flag = false;
		Stack<VisualGroup> stack = new Stack<VisualGroup>();
		VisualGroup visualGroup = __17DF;
		for (int i = 0; i < _0EE9.Length; i++)
		{
			if (_0EE9[i] == '\'')
			{
				flag = !flag;
				continue;
			}
			if (flag)
			{
				stringBuilder.Append(_0EE9[i]);
				continue;
			}
			string text = _0EE9.Substring(i, Math.Min(4, _0EE9.Length - i));
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
				switch (_0EE9.Substring(i, Math.Min(3, _0EE9.Length - i)))
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
				switch (_0EE9.Substring(i, Math.Min(2, _0EE9.Length - i)))
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
				switch (_0EE9[i].ToString())
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
						stringBuilder = new StringBuilder(_0EE9.Length);
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
					stringBuilder = new StringBuilder(_0EE9.Length);
				}
			}
			else
			{
				stringBuilder.Append(_0EE9[i]);
			}
		}
		if (stringBuilder.Length > 0)
		{
			VisualLabel visualLabel3 = new VisualLabel();
			visualLabel3.Text = stringBuilder.ToString();
			visualGroup.Items.Add(visualLabel3);
		}
		if (__17EC)
		{
			VisualUpDownButton visualUpDownButton = new VisualUpDownButton();
			visualUpDownButton.Alignment = eItemAlignment.Right;
			visualUpDownButton.AutoChange = eUpDownButtonAutoChange.FocusedItem;
			__17DF.Items.Add(visualUpDownButton);
		}
		RecreateButtons();
		if (__17EB)
		{
			_17DE.Checked = __17F0;
		}
	}

	private void _180A(object _٠, EventArgs ؿ)
	{
		LockUpdateCheckBox lockUpdateCheckBox = _17DE;
		if (lockUpdateCheckBox != null)
		{
			__17F0 = lockUpdateCheckBox.Checked;
		}
		OnLockUpdateChanged(ؿ);
	}

	protected virtual void OnLockUpdateChanged(EventArgs e)
	{
		if (_៩ != null)
		{
			_៩(this, e);
		}
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (IsInitializing)
		{
			return;
		}
		if (ᗲ != null)
		{
			ᗲ(this, e);
		}
		if (_៤ != null)
		{
			_៤(this, e);
		}
		if (IsEmpty)
		{
			base.Text = "";
		}
		else
		{
			string text = "";
			text = ((Format != 0) ? _1808(Format) : CustomFormat);
			if (text == "")
			{
				text = _1808(eDateTimePickerFormat.Short);
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
		if (FreeTextEntryMode && __1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
		ExecuteCommand();
	}

	protected override void OnValidating(CancelEventArgs e)
	{
		__17DF.UpdateValue(updateDirect: false);
		((Control)this).OnValidating(e);
	}

	protected override SortedList CreateSortedButtonList()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		SortedList sortedList = base.CreateSortedButtonList();
		if (__17F2.Visible)
		{
			VisualItem visualItem = CreateButton(__17F2);
			if (__17F2.ItemReference != null)
			{
				__17F2.ItemReference.Click -= _180F;
			}
			__17F2.ItemReference = visualItem;
			visualItem.Click += _180F;
			sortedList.Add(__17F2, visualItem);
		}
		if (__17F1.Visible)
		{
			VisualItem visualItem2 = CreateButton(__17F1);
			if (__17F1.ItemReference != null)
			{
				__17F1.ItemReference.MouseDown -= new MouseEventHandler(_180C);
				__17F1.ItemReference.Click -= _180B;
			}
			__17F1.ItemReference = visualItem2;
			visualItem2.MouseDown += new MouseEventHandler(_180C);
			visualItem2.Click += _180B;
			sortedList.Add(__17F1, visualItem2);
		}
		if (__17F5.Visible)
		{
			VisualItem visualItem3 = CreateButton(__17F5);
			if (__17F5.ItemReference != null)
			{
				__17F5.ItemReference.Click -= _᠑;
			}
			__17F5.ItemReference = visualItem3;
			visualItem3.Click += _᠑;
			sortedList.Add(__17F5, visualItem3);
		}
		return sortedList;
	}

	private void _180B(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			_180D();
		}
	}

	private void _180C(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button != 1048576 || (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 150.0))
		{
			__17F4 = DateTime.MinValue;
		}
		else
		{
			_180D();
		}
	}

	private void _180D()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			_០.SetDisplayRectangle(((Control)this).ClientRectangle);
			_១.ReloadLocalizedStrings();
			if (((Control)this).Font.Height > Dpi.Height(_១.DaySize.Height))
			{
				_១.DaySize = new Size((int)Math.Ceiling((float)((Control)this).Font.Height * 1.6f), ((Control)this).Font.Height + 1);
			}
			if ((int)((Control)this).RightToLeft == 0)
			{
				_០.PopupLocation = new Point(((Control)this).Width - _០.PopupSize.Width, ((Control)this).Height);
			}
			if (MinDate != DateTime.MinValue)
			{
				_១.MinDate = MinDate.Date;
			}
			else
			{
				_១.MinDate = MinDate;
			}
			_១.MaxDate = MaxDate.Date;
			if (!IsEmpty)
			{
				_១.SelectedDate = Value;
				_២.SelectedDateTime = Value;
			}
			else
			{
				_១.SelectedDate = DateTime.MinValue;
				_២.SelectedTime = TimeSpan.Zero;
			}
			if (_១.SelectedDate != DateTime.MinValue)
			{
				_១.DisplayMonth = _១.SelectedDate;
			}
			else
			{
				_១.DisplayMonth = DateTime.Today;
			}
			_០.Expanded = !_០.Expanded;
		}
	}

	private void _180E(object _٠, EventArgs ؿ)
	{
		if (__17F3)
		{
			OnValueChanged(EventArgs.Empty);
		}
		__17F3 = false;
		__17F4 = DateTime.Now;
		if (_១ != null)
		{
			_១._258C();
		}
		if (_២ != null)
		{
			_២._258C();
		}
	}

	private void _180F(object _٠, EventArgs ؿ)
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
		if (buttonSettings == __17F1)
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
		if (buttonSettings == __17F2)
		{
			if (buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
			{
				visualButton.Symbol = "\uf00d";
			}
		}
		else if (buttonSettings == __17F5)
		{
			if (buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
			{
				visualButton.Symbol = "\uf040";
			}
			visualButton.Checked = buttonSettings.Checked;
		}
		return visualItem;
	}

	protected virtual void OnButtonClearClick(CancelEventArgs e)
	{
		if (_៦ != null)
		{
			_៦(this, e);
		}
	}

	protected virtual void OnButtonDropDownClick(CancelEventArgs e)
	{
		if (_៧ != null)
		{
			_៧(this, e);
		}
	}

	private void _᠐()
	{
		bool flag = false;
		bool flag2 = true;
		if (__17F8 == eDateTimeSelectorVisibility.Auto)
		{
			if (__17ED == eDateTimePickerFormat.LongTime || __17ED == eDateTimePickerFormat.ShortTime)
			{
				flag = true;
				flag2 = false;
			}
		}
		else if (__17F8 == eDateTimeSelectorVisibility.TimeSelector)
		{
			flag = true;
			flag2 = false;
		}
		else if (__17F8 == eDateTimeSelectorVisibility.Both)
		{
			flag = true;
		}
		_១.Visible = flag2;
		_២.Visible = flag;
		if (flag2 && flag)
		{
			_៣.Visible = true;
			_១.DayClickAutoClosePopup = false;
		}
		else
		{
			_៣.Visible = false;
			_១.DayClickAutoClosePopup = true;
		}
	}

	protected virtual void OnDateTimeSelectorVisibilityChanged(eDateTimeSelectorVisibility oldValue, eDateTimeSelectorVisibility newValue)
	{
		_᠐();
	}

	protected virtual void OnTimeSelectorTimeFormatChanged(eTimeSelectorFormat oldValue, eTimeSelectorFormat newValue)
	{
		if (_២ != null)
		{
			_២.TimeFormat = __17F9;
		}
	}

	protected virtual void OnTimeSelectorTypeChanged(eTimeSelectorType oldValue, eTimeSelectorType newValue)
	{
		if (_២ != null)
		{
			_២.SelectorType = __17FA;
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}

	private void _᠑(object _٠, EventArgs ؿ)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonFreeTextClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			FreeTextEntryMode = !FreeTextEntryMode;
			__17F5.Checked = FreeTextEntryMode;
			if (FreeTextEntryMode && __1800 != null && !((Control)__1800).Focused)
			{
				((Control)__1800).Focus();
			}
		}
	}

	protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
	{
		_17FC?.Invoke(this, e);
	}

	private void _᠒()
	{
		if (!__17FF)
		{
			if (__1800 != null)
			{
				__1800._232E -= _᠗;
				__1800._232D -= _᠔;
				((Control)__1800).LostFocus -= _᠘;
				((Control)this).Controls.Remove((Control)(object)__1800);
				((Component)(object)__1800).Dispose();
				__1800 = null;
			}
		}
		else
		{
			_᠓();
		}
		if (__17F5 != null)
		{
			__17F5.Checked = __17FF;
		}
	}

	protected override void OnIsKeyboardFocusWithinChanged()
	{
		if (__17FF)
		{
			_᠓();
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

	private void _᠓()
	{
		_232C _232C = (_232C)(object)GetFreeTextBox();
		if (base.IsKeyboardFocusWithin)
		{
			((Control)_232C).Visible = true;
			((Control)_232C).Text = ((Control)this).Text;
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
		else if (((Control)_232C).Visible)
		{
			if (((Control)_232C).Focused)
			{
				_232C._2334();
			}
			else
			{
				((Control)_232C).Visible = false;
			}
			RootVisualItem.InvalidateArrange();
			((Control)this).Invalidate();
		}
	}

	protected override Control GetFreeTextBox()
	{
		if (__1800 == null)
		{
			__1800 = new _232C();
			__1800._232E += _᠗;
			__1800._232D += _᠔;
			((Control)__1800).LostFocus += _᠘;
			((TextBoxBase)__1800).BorderStyle = (BorderStyle)0;
			((TextBoxBase)__1800).ReadOnly = IsInputReadOnly;
			((Control)this).Controls.Add((Control)(object)__1800);
		}
		return (Control)(object)__1800;
	}

	private void _᠔(object _٠, EventArgs ؿ)
	{
		if (__1800 != null)
		{
			((Control)__1800).Text = ((Control)this).Text;
		}
	}

	protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
	{
		_17FB?.Invoke(this, e);
	}

	private bool _᠕(out DateTime _᠖)
	{
		try
		{
			return ((Format == eDateTimePickerFormat.Custom && DateTime.TryParseExact(((Control)__1800).Text, CustomFormat, null, DateTimeStyles.None, out _᠖)) || DateTime.TryParse(((Control)__1800).Text, out _᠖)) && __17FD;
		}
		catch
		{
			_᠖ = DateTime.MinValue;
			return false;
		}
	}

	private void _᠗(object _٠, EventArgs ؿ)
	{
		if (__1800 == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(((Control)__1800).Text))
		{
			ValueObject = null;
			return;
		}
		if (_᠕(out var _᠖))
		{
			Value = _᠖;
			return;
		}
		FreeTextEntryConversionEventArgs freeTextEntryConversionEventArgs = new FreeTextEntryConversionEventArgs(((Control)__1800).Text);
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
		else if (__17FD)
		{
			_᠖ = DateTime.MinValue;
			switch (((Control)__1800).Text.ToLower())
			{
			case "now":
				_᠖ = DateTime.Now;
				break;
			case "today":
				_᠖ = DateTime.Today;
				break;
			case "tomorrow":
				_᠖ = DateTime.Today.AddDays(1.0);
				break;
			case "yesterday":
				_᠖ = DateTime.Today.AddDays(-1.0);
				break;
			}
			if (_᠖ == DateTime.MinValue)
			{
				ValueObject = null;
			}
			else
			{
				Value = _᠖;
			}
		}
	}

	private void _᠘(object _٠, EventArgs ؿ)
	{
		if (__17FE && !base.IsKeyboardFocusWithin)
		{
			FreeTextEntryMode = false;
		}
	}

	protected override void HideFreeTextBoxEntry()
	{
		if (__1800 != null)
		{
			((Control)__1800).Visible = false;
		}
	}
}
