using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;
using Stimulsoft.Controls.Win.Editors.DateTimeAdv;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DateTimeInput.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewDateTimeInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private DateTimeInput _DateTimeInput;

	private Bitmap _CellBitmap;

	private bool _DisplayControlForCurrentCellOnly = true;

	internal DateTimeInput DateTimeInput => _DateTimeInput;

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return _DateTimeInput.AllowEmptyState;
		}
		set
		{
			_DateTimeInput.AllowEmptyState = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
	public bool AutoAdvance
	{
		get
		{
			return _DateTimeInput.AutoAdvance;
		}
		set
		{
			_DateTimeInput.AutoAdvance = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
	public bool AutoSelectDate
	{
		get
		{
			return _DateTimeInput.AutoSelectDate;
		}
		set
		{
			_DateTimeInput.AutoSelectDate = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return _DateTimeInput.AutoOverwrite;
		}
		set
		{
			_DateTimeInput.AutoOverwrite = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _DateTimeInput.BackgroundStyle;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _DateTimeInput.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _DateTimeInput.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _DateTimeInput.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _DateTimeInput.ButtonDropDown;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => _DateTimeInput.ButtonFreeText;

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the custom date/time format string. ")]
	[Localizable(true)]
	public string CustomFormat
	{
		get
		{
			return _DateTimeInput.CustomFormat;
		}
		set
		{
			_DateTimeInput.CustomFormat = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	public bool DisplayControlForCurrentCellOnly
	{
		get
		{
			return _DisplayControlForCurrentCellOnly;
		}
		set
		{
			if (_DisplayControlForCurrentCellOnly != value)
			{
				_DisplayControlForCurrentCellOnly = value;
				((Control)_DateTimeInput).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
	public bool DefaultInputValues
	{
		get
		{
			return _DateTimeInput.DefaultInputValues;
		}
		set
		{
			_DateTimeInput.DefaultInputValues = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	public bool Enabled
	{
		get
		{
			return ((Control)_DateTimeInput).Enabled;
		}
		set
		{
			((Control)_DateTimeInput).Enabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eDateTimePickerFormat.Short)]
	[Description("Indicates the format date/time is displayed in. To specify custom format set this value to Custom and specify custom format using CustomFormat property.")]
	public eDateTimePickerFormat Format
	{
		get
		{
			return _DateTimeInput.Format;
		}
		set
		{
			_DateTimeInput.Format = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eHorizontalAlignment.Right)]
	[Category("Appearance")]
	[Description("Indicates alignment of input fields inside of the control.")]
	public eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return _DateTimeInput.InputHorizontalAlignment;
		}
		set
		{
			_DateTimeInput.InputHorizontalAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return _DateTimeInput.IsInputReadOnly;
		}
		set
		{
			_DateTimeInput.IsInputReadOnly = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return _DateTimeInput.LockUpdateChecked;
		}
		set
		{
			_DateTimeInput.LockUpdateChecked = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates maximum date and time that can be selected in the control.")]
	public DateTime MaxDate
	{
		get
		{
			return _DateTimeInput.MaxDate;
		}
		set
		{
			_DateTimeInput.MaxDate = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates minimum date and time that can be selected in the control.")]
	public DateTime MinDate
	{
		get
		{
			return _DateTimeInput.MinDate;
		}
		set
		{
			_DateTimeInput.MinDate = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
	public MonthCalendarItem MonthCalendar => _DateTimeInput.MonthCalendar;

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	public string SelectNextInputCharacters
	{
		get
		{
			return _DateTimeInput.SelectNextInputCharacters;
		}
		set
		{
			_DateTimeInput.SelectNextInputCharacters = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return _DateTimeInput.ShowCheckBox;
		}
		set
		{
			_DateTimeInput.ShowCheckBox = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return _DateTimeInput.ShowUpDown;
		}
		set
		{
			_DateTimeInput.ShowUpDown = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a DateTimeInput Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick;

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick;

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click;

	[Description("Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick;

	[Description("Occurs when Free-Text button is clicked and allows you to cancel its default action.")]
	public event EventHandler<CancelEventArgs> ButtonFreeTextClick;

	[Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value the control expects.")]
	public event EventHandler<FreeTextEntryConversionEventArgs> ConvertFreeTextEntry;

	public DataGridViewDateTimeInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewDateTimeInputCell();
		_DateTimeInput = new DateTimeInput();
		_DateTimeInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewDateTimeBorderKey;
		((Control)_DateTimeInput).Visible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)_DateTimeInput).Dispose();
		}
		if (_CellBitmap != null)
		{
			((Image)_CellBitmap).Dispose();
			_CellBitmap = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_DateTimeInput.ResetBackgroundStyle();
		_DateTimeInput.BackgroundStyle.Class = "TextBoxBorder";
		_DateTimeInput.BackgroundStyle.CornerType = eCornerType.Square;
	}

	public bool ShouldSerializeMaxDate()
	{
		return !MaxDate.Equals(DateTimeGroup.MaxDateTime);
	}

	public bool ShouldSerializeMinDate()
	{
		return !MinDate.Equals(DateTimeGroup.MinDateTime);
	}

	internal void DoButtonClearClick(object sender, CancelEventArgs e)
	{
		if (this.ButtonClearClick != null)
		{
			this.ButtonClearClick(this, e);
		}
	}

	internal void DoButtonCustomClick(object sender, EventArgs e)
	{
		if (this.ButtonCustomClick != null)
		{
			this.ButtonCustomClick(this, e);
		}
	}

	internal void DoButtonCustom2Click(object sender, EventArgs e)
	{
		if (this.ButtonCustom2Click != null)
		{
			this.ButtonCustom2Click(this, e);
		}
	}

	internal void DoButtonDropDownClick(object sender, CancelEventArgs e)
	{
		if (this.ButtonDropDownClick != null)
		{
			this.ButtonDropDownClick(this, e);
		}
	}

	internal void DoButtonFreeTextClick(object sender, CancelEventArgs e)
	{
		if (this.ButtonFreeTextClick != null)
		{
			this.ButtonFreeTextClick(this, e);
		}
	}

	internal void DoConvertFreeTextEntry(object sender, FreeTextEntryConversionEventArgs e)
	{
		if (this.ConvertFreeTextEntry != null)
		{
			this.ConvertFreeTextEntry(this, e);
		}
	}

	internal Bitmap GetCellBitmap(Rectangle cellBounds)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (_CellBitmap == null || ((Image)_CellBitmap).Width != cellBounds.Width || ((Image)_CellBitmap).Height < cellBounds.Height)
		{
			if (_CellBitmap != null)
			{
				((Image)_CellBitmap).Dispose();
			}
			_CellBitmap = new Bitmap(cellBounds.Width, cellBounds.Height);
		}
		return _CellBitmap;
	}

	internal void OnBeforeCellPaint(int rowIndex, int columnIndex)
	{
		if (this.BeforeCellPaint != null)
		{
			this.BeforeCellPaint(this, new BeforeCellPaintEventArgs(rowIndex, columnIndex));
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
			DateTimeInput.MonthCalendar.InternalCopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.MonthCalendar);
			dataGridViewDateTimeInputColumn.BackgroundStyle.ApplyStyle(DateTimeInput.BackgroundStyle);
			dataGridViewDateTimeInputColumn.BackgroundStyle.Class = DateTimeInput.BackgroundStyle.Class;
			DateTimeInput.ButtonClear.CopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.ButtonClear);
			DateTimeInput.ButtonDropDown.CopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.ButtonDropDown);
			DateTimeInput.ButtonFreeText.CopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.ButtonFreeText);
			DateTimeInput.ButtonCustom.CopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.ButtonCustom);
			DateTimeInput.ButtonCustom2.CopyToItem(dataGridViewDateTimeInputColumn.DateTimeInput.ButtonCustom2);
		}
		return dataGridViewDateTimeInputColumn;
	}
}
