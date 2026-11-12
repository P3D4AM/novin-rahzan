using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DoubleInput.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewDoubleInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private DoubleInput _DoubleInput;

	private Bitmap _CellBitmap;

	private bool _DisplayControlForCurrentCellOnly = true;

	internal DoubleInput DoubleInput => _DoubleInput;

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _DoubleInput.BackgroundStyle;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _DoubleInput.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _DoubleInput.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _DoubleInput.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _DoubleInput.ButtonDropDown;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonFreeText => _DoubleInput.ButtonFreeText;

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	public string DisplayFormat
	{
		get
		{
			return _DoubleInput.DisplayFormat;
		}
		set
		{
			_DoubleInput.DisplayFormat = value;
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
				((Control)_DoubleInput).Invalidate();
			}
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
			return ((Control)_DoubleInput).Enabled;
		}
		set
		{
			((Control)_DoubleInput).Enabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(1)]
	[Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
	public double Increment
	{
		get
		{
			return _DoubleInput.Increment;
		}
		set
		{
			_DoubleInput.Increment = value;
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
			return _DoubleInput.InputHorizontalAlignment;
		}
		set
		{
			_DoubleInput.InputHorizontalAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return _DoubleInput.LockUpdateChecked;
		}
		set
		{
			_DoubleInput.LockUpdateChecked = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(double.MaxValue)]
	[Description("Indicates maximum value that can be entered.")]
	public double MaxValue
	{
		get
		{
			return _DoubleInput.MaxValue;
		}
		set
		{
			_DoubleInput.MaxValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(double.MinValue)]
	[Description("Indicates minimum value that can be entered.")]
	public double MinValue
	{
		get
		{
			return _DoubleInput.MinValue;
		}
		set
		{
			_DoubleInput.MinValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return _DoubleInput.ShowCheckBox;
		}
		set
		{
			_DoubleInput.ShowCheckBox = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public bool ShowUpDown
	{
		get
		{
			return _DoubleInput.ShowUpDown;
		}
		set
		{
			_DoubleInput.ShowUpDown = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a DoubleInput Cell is painted.")]
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

	public DataGridViewDoubleInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewDoubleInputCell();
		_DoubleInput = new DoubleInput();
		_DoubleInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewNumericBorderKey;
		((Control)_DoubleInput).Visible = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_DoubleInput.ResetBackgroundStyle();
		_DoubleInput.BackgroundStyle.Class = "TextBoxBorder";
		_DoubleInput.BackgroundStyle.CornerType = eCornerType.Square;
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
		DataGridViewDoubleInputColumn dataGridViewDoubleInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewDoubleInputColumn;
		if (dataGridViewDoubleInputColumn != null)
		{
			dataGridViewDoubleInputColumn.DisplayFormat = DisplayFormat;
			dataGridViewDoubleInputColumn.Enabled = Enabled;
			dataGridViewDoubleInputColumn.Increment = Increment;
			dataGridViewDoubleInputColumn.InputHorizontalAlignment = InputHorizontalAlignment;
			dataGridViewDoubleInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewDoubleInputColumn.MaxValue = MaxValue;
			dataGridViewDoubleInputColumn.MinValue = MinValue;
			dataGridViewDoubleInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewDoubleInputColumn.ShowUpDown = ShowUpDown;
			dataGridViewDoubleInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewDoubleInputColumn.BackgroundStyle.ApplyStyle(DoubleInput.BackgroundStyle);
			dataGridViewDoubleInputColumn.BackgroundStyle.Class = DoubleInput.BackgroundStyle.Class;
			DoubleInput.ButtonClear.CopyToItem(dataGridViewDoubleInputColumn.DoubleInput.ButtonClear);
			DoubleInput.ButtonDropDown.CopyToItem(dataGridViewDoubleInputColumn.DoubleInput.ButtonDropDown);
			DoubleInput.ButtonFreeText.CopyToItem(dataGridViewDoubleInputColumn.DoubleInput.ButtonFreeText);
			DoubleInput.ButtonCustom.CopyToItem(dataGridViewDoubleInputColumn.DoubleInput.ButtonCustom);
			DoubleInput.ButtonCustom2.CopyToItem(dataGridViewDoubleInputColumn.DoubleInput.ButtonCustom2);
		}
		return dataGridViewDoubleInputColumn;
	}
}
