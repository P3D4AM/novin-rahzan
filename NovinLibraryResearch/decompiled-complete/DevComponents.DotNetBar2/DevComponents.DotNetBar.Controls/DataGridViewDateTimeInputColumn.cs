using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DateTimeInput.ico")]
[ToolboxItem(false)]
public class DataGridViewDateTimeInputColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private EventHandler<CancelEventArgs> _17FC;

	private EventHandler<FreeTextEntryConversionEventArgs> _17FB;

	private DateTimeInput __2B30;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	internal DateTimeInput _2B2F => __2B30;

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return __2B30.AllowEmptyState;
		}
		set
		{
			__2B30.AllowEmptyState = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool AutoAdvance
	{
		get
		{
			return __2B30.AutoAdvance;
		}
		set
		{
			__2B30.AutoAdvance = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
	[Browsable(true)]
	[Category("Behavior")]
	public bool AutoSelectDate
	{
		get
		{
			return __2B30.AutoSelectDate;
		}
		set
		{
			__2B30.AutoSelectDate = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return __2B30.AutoOverwrite;
		}
		set
		{
			__2B30.AutoOverwrite = value;
		}
	}

	[Description("Gets or sets control background style.")]
	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => __2B30.BackgroundStyle;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Buttons")]
	public InputButtonSettings ButtonClear => __2B30.ButtonClear;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom => __2B30.ButtonCustom;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom2 => __2B30.ButtonCustom2;

	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	public InputButtonSettings ButtonDropDown => __2B30.ButtonDropDown;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	public InputButtonSettings ButtonFreeText => __2B30.ButtonFreeText;

	[Localizable(true)]
	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the custom date/time format string. ")]
	public string CustomFormat
	{
		get
		{
			return __2B30.CustomFormat;
		}
		set
		{
			__2B30.CustomFormat = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	[Browsable(true)]
	[Category("Appearance")]
	public bool DisplayControlForCurrentCellOnly
	{
		get
		{
			return __2B23;
		}
		set
		{
			if (__2B23 != value)
			{
				__2B23 = value;
				((Control)__2B30).Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
	[Browsable(true)]
	public bool DefaultInputValues
	{
		get
		{
			return __2B30.DefaultInputValues;
		}
		set
		{
			__2B30.DefaultInputValues = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the control can respond to user interaction.")]
	[Browsable(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B30).Enabled;
		}
		set
		{
			((Control)__2B30).Enabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eDateTimePickerFormat.Short)]
	[Description("Indicates the format date/time is displayed in. To specify custom format set this value to Custom and specify custom format using CustomFormat property.")]
	public eDateTimePickerFormat Format
	{
		get
		{
			return __2B30.Format;
		}
		set
		{
			__2B30.Format = value;
		}
	}

	[Description("Indicates alignment of input fields inside of the control.")]
	[DefaultValue(eHorizontalAlignment.Right)]
	[Browsable(true)]
	[Category("Appearance")]
	public eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return __2B30.InputHorizontalAlignment;
		}
		set
		{
			__2B30.InputHorizontalAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return __2B30.IsInputReadOnly;
		}
		set
		{
			__2B30.IsInputReadOnly = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	[DefaultValue(true)]
	public bool LockUpdateChecked
	{
		get
		{
			return __2B30.LockUpdateChecked;
		}
		set
		{
			__2B30.LockUpdateChecked = value;
		}
	}

	[Description("Indicates maximum date and time that can be selected in the control.")]
	[Browsable(true)]
	public DateTime MaxDate
	{
		get
		{
			return __2B30.MaxDate;
		}
		set
		{
			__2B30.MaxDate = value;
		}
	}

	[Description("Indicates minimum date and time that can be selected in the control.")]
	[Browsable(true)]
	public DateTime MinDate
	{
		get
		{
			return __2B30.MinDate;
		}
		set
		{
			__2B30.MinDate = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
	public MonthCalendarItem MonthCalendar => __2B30.MonthCalendar;

	[DefaultValue("")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	[Browsable(true)]
	public string SelectNextInputCharacters
	{
		get
		{
			return __2B30.SelectNextInputCharacters;
		}
		set
		{
			__2B30.SelectNextInputCharacters = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	[Browsable(true)]
	public bool ShowCheckBox
	{
		get
		{
			return __2B30.ShowCheckBox;
		}
		set
		{
			__2B30.ShowCheckBox = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return __2B30.ShowUpDown;
		}
		set
		{
			__2B30.ShowUpDown = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a DateTimeInput Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint
	{
		add
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _៦;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៦, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _៦;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៦, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick
	{
		add
		{
			EventHandler<EventArgs> eventHandler = _181E;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = _181E;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click
	{
		add
		{
			EventHandler<EventArgs> eventHandler = _181F;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = _181F;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _៧;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៧, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _៧;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៧, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Free-Text button is clicked and allows you to cancel its default action.")]
	public event EventHandler<CancelEventArgs> ButtonFreeTextClick
	{
		add
		{
			EventHandler<CancelEventArgs> eventHandler = _17FC;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<CancelEventArgs> eventHandler = _17FC;
			EventHandler<CancelEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<CancelEventArgs> value2 = (EventHandler<CancelEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value the control expects.")]
	public event EventHandler<FreeTextEntryConversionEventArgs> ConvertFreeTextEntry
	{
		add
		{
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler = _17FB;
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<FreeTextEntryConversionEventArgs> value2 = (EventHandler<FreeTextEntryConversionEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler = _17FB;
			EventHandler<FreeTextEntryConversionEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<FreeTextEntryConversionEventArgs> value2 = (EventHandler<FreeTextEntryConversionEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _17FB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public DataGridViewDateTimeInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewDateTimeInputCell();
		__2B30 = new DateTimeInput();
		__2B30.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewDateTimeBorderKey;
		((Control)__2B30).Visible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__2B30).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__2B30.ResetBackgroundStyle();
		__2B30.BackgroundStyle.Class = "TextBoxBorder";
		__2B30.BackgroundStyle.CornerType = eCornerType.Square;
	}

	public bool ShouldSerializeMaxDate()
	{
		return !MaxDate.Equals(DateTimeGroup.MaxDateTime);
	}

	public bool ShouldSerializeMinDate()
	{
		return !MinDate.Equals(DateTimeGroup.MinDateTime);
	}

	internal void _2B31(object _٠, CancelEventArgs ؿ)
	{
		if (_៦ != null)
		{
			_៦(this, ؿ);
		}
	}

	internal void _2B32(object _٠, EventArgs ؿ)
	{
		if (_181E != null)
		{
			_181E(this, ؿ);
		}
	}

	internal void _2B33(object _٠, EventArgs ؿ)
	{
		if (_181F != null)
		{
			_181F(this, ؿ);
		}
	}

	internal void _2B34(object _٠, CancelEventArgs ؿ)
	{
		if (_៧ != null)
		{
			_៧(this, ؿ);
		}
	}

	internal void _2B35(object _٠, CancelEventArgs ؿ)
	{
		if (_17FC != null)
		{
			_17FC(this, ؿ);
		}
	}

	internal void _2B36(object _٠, FreeTextEntryConversionEventArgs ؿ)
	{
		if (_17FB != null)
		{
			_17FB(this, ؿ);
		}
	}

	internal Bitmap _2B01(Rectangle _2AE9)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (__2AFE == null || ((Image)__2AFE).Width != _2AE9.Width || ((Image)__2AFE).Height < _2AE9.Height)
		{
			if (__2AFE != null)
			{
				((Image)__2AFE).Dispose();
			}
			__2AFE = new Bitmap(_2AE9.Width, _2AE9.Height);
		}
		return __2AFE;
	}

	internal void _2B02(int _2AEA, int _21A5)
	{
		if (_2AF8 != null)
		{
			_2AF8(this, new BeforeCellPaintEventArgs(_2AEA, _21A5));
		}
	}

	public override object Clone()
	{
		DataGridViewDateTimeInputColumn dataGridViewDateTimeInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewDateTimeInputColumn;
		if (dataGridViewDateTimeInputColumn != null)
		{
			dataGridViewDateTimeInputColumn.AllowEmptyState = AllowEmptyState;
			dataGridViewDateTimeInputColumn.AutoAdvance = AutoAdvance;
			dataGridViewDateTimeInputColumn.AutoOverwrite = AutoOverwrite;
			dataGridViewDateTimeInputColumn.AutoSelectDate = AutoSelectDate;
			dataGridViewDateTimeInputColumn.CustomFormat = CustomFormat;
			dataGridViewDateTimeInputColumn.DefaultInputValues = DefaultInputValues;
			dataGridViewDateTimeInputColumn.Enabled = Enabled;
			dataGridViewDateTimeInputColumn.Format = Format;
			dataGridViewDateTimeInputColumn.InputHorizontalAlignment = InputHorizontalAlignment;
			dataGridViewDateTimeInputColumn.IsInputReadOnly = IsInputReadOnly;
			dataGridViewDateTimeInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewDateTimeInputColumn.MinDate = MinDate;
			dataGridViewDateTimeInputColumn.MaxDate = MaxDate;
			dataGridViewDateTimeInputColumn.SelectNextInputCharacters = SelectNextInputCharacters;
			dataGridViewDateTimeInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewDateTimeInputColumn.ShowUpDown = ShowUpDown;
			dataGridViewDateTimeInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			_2B2F.MonthCalendar._0EBA(dataGridViewDateTimeInputColumn._2B2F.MonthCalendar);
			dataGridViewDateTimeInputColumn.BackgroundStyle.ApplyStyle(_2B2F.BackgroundStyle);
			dataGridViewDateTimeInputColumn.BackgroundStyle.Class = _2B2F.BackgroundStyle.Class;
			_2B2F.ButtonClear.ᡄ(dataGridViewDateTimeInputColumn._2B2F.ButtonClear);
			_2B2F.ButtonDropDown.ᡄ(dataGridViewDateTimeInputColumn._2B2F.ButtonDropDown);
			_2B2F.ButtonFreeText.ᡄ(dataGridViewDateTimeInputColumn._2B2F.ButtonFreeText);
			_2B2F.ButtonCustom.ᡄ(dataGridViewDateTimeInputColumn._2B2F.ButtonCustom);
			_2B2F.ButtonCustom2.ᡄ(dataGridViewDateTimeInputColumn._2B2F.ButtonCustom2);
		}
		return dataGridViewDateTimeInputColumn;
	}
}
