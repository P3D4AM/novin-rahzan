using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "IpAddressInput.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewIpAddressInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private IpAddressInput _IpAddressInput;

	private Bitmap _CellBitmap;

	private bool _DisplayControlForCurrentCellOnly = true;

	[Browsable(false)]
	internal IpAddressInput IpAddressInput => _IpAddressInput;

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether empty null/nothing state of the control is allowed.")]
	public bool AllowEmptyState
	{
		get
		{
			return _IpAddressInput.AllowEmptyState;
		}
		set
		{
			_IpAddressInput.AllowEmptyState = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
	public bool AutoOffFreeTextEntry
	{
		get
		{
			return _IpAddressInput.AutoOffFreeTextEntry;
		}
		set
		{
			_IpAddressInput.AutoOffFreeTextEntry = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether auto-overwrite functionality for input is enabled.")]
	public bool AutoOverwrite
	{
		get
		{
			return _IpAddressInput.AutoOverwrite;
		}
		set
		{
			_IpAddressInput.AutoOverwrite = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Free-Text")]
	[Description("Indicates whether free text entries are attempted to be auto-resolved to IP address as host/domain names.")]
	public bool AutoResolveFreeTextEntries
	{
		get
		{
			return _IpAddressInput.AutoResolveFreeTextEntries;
		}
		set
		{
			_IpAddressInput.AutoResolveFreeTextEntries = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)_IpAddressInput).BackColor;
		}
		set
		{
			((Control)_IpAddressInput).BackColor = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _IpAddressInput.BackgroundStyle;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _IpAddressInput.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _IpAddressInput.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _IpAddressInput.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _IpAddressInput.ButtonDropDown;

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
				((Control)_IpAddressInput).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return _IpAddressInput.DropDownControl;
		}
		set
		{
			_IpAddressInput.DropDownControl = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection DropDownItems => _IpAddressInput.DropDownItems;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	public bool Enabled
	{
		get
		{
			return ((Control)_IpAddressInput).Enabled;
		}
		set
		{
			((Control)_IpAddressInput).Enabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return _IpAddressInput.FocusHighlightColor;
		}
		set
		{
			_IpAddressInput.FocusHighlightColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight the text box when it has input focus.")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return _IpAddressInput.FocusHighlightEnabled;
		}
		set
		{
			_IpAddressInput.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)_IpAddressInput).ForeColor;
		}
		set
		{
			((Control)_IpAddressInput).ForeColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Free-Text")]
	[Description("Indicates whether control input is in free-text input mode.")]
	public bool FreeTextEntryMode
	{
		get
		{
			return _IpAddressInput.FreeTextEntryMode;
		}
		set
		{
			_IpAddressInput.FreeTextEntryMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)_IpAddressInput).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_IpAddressInput).ImeMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether input part of the control is read-only.")]
	public bool IsInputReadOnly
	{
		get
		{
			return _IpAddressInput.IsInputReadOnly;
		}
		set
		{
			_IpAddressInput.IsInputReadOnly = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
	public bool LockUpdateChecked
	{
		get
		{
			return _IpAddressInput.LockUpdateChecked;
		}
		set
		{
			_IpAddressInput.LockUpdateChecked = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the control's elements are aligned to support locales using right-to-left fonts.")]
	public RightToLeft RightToLeft
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)_IpAddressInput).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_IpAddressInput).RightToLeft = value;
		}
	}

	[DefaultValue(".")]
	[Category("Behavior")]
	[Description("List of characters that when pressed would select next input field.")]
	public string SelectNextInputCharacters
	{
		get
		{
			return _IpAddressInput.SelectNextInputCharacters;
		}
		set
		{
			_IpAddressInput.SelectNextInputCharacters = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
	public bool ShowCheckBox
	{
		get
		{
			return _IpAddressInput.ShowCheckBox;
		}
		set
		{
			_IpAddressInput.ShowCheckBox = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)_IpAddressInput).Text;
		}
		set
		{
			((Control)_IpAddressInput).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return _IpAddressInput.WatermarkColor;
		}
		set
		{
			_IpAddressInput.WatermarkColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _IpAddressInput.WatermarkEnabled;
		}
		set
		{
			_IpAddressInput.WatermarkEnabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates watermark font.")]
	public Font WatermarkFont
	{
		get
		{
			return _IpAddressInput.WatermarkFont;
		}
		set
		{
			_IpAddressInput.WatermarkFont = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return _IpAddressInput.WatermarkText;
		}
		set
		{
			_IpAddressInput.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a IpAddressInput Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick;

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick;

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click;

	[Description("Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick;

	public DataGridViewIpAddressInputColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewIpAddressInputCell();
		_IpAddressInput = new IpAddressInput();
		_IpAddressInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_IpAddressInput.ResetBackgroundStyle();
		_IpAddressInput.BackgroundStyle.Class = "TextBoxBorder";
		_IpAddressInput.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return _IpAddressInput.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		_IpAddressInput.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _IpAddressInput.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		_IpAddressInput.ResetWatermarkColor();
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
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewIpAddressInputColumn dataGridViewIpAddressInputColumn = ((DataGridViewColumn)this).Clone() as DataGridViewIpAddressInputColumn;
		if (dataGridViewIpAddressInputColumn != null)
		{
			dataGridViewIpAddressInputColumn.AllowEmptyState = AllowEmptyState;
			dataGridViewIpAddressInputColumn.AutoOffFreeTextEntry = AutoOffFreeTextEntry;
			dataGridViewIpAddressInputColumn.AutoOverwrite = AutoOverwrite;
			dataGridViewIpAddressInputColumn.AutoResolveFreeTextEntries = AutoResolveFreeTextEntries;
			dataGridViewIpAddressInputColumn.DropDownControl = DropDownControl;
			dataGridViewIpAddressInputColumn.Enabled = Enabled;
			dataGridViewIpAddressInputColumn.FocusHighlightColor = FocusHighlightColor;
			dataGridViewIpAddressInputColumn.FocusHighlightEnabled = FocusHighlightEnabled;
			dataGridViewIpAddressInputColumn.FreeTextEntryMode = FreeTextEntryMode;
			dataGridViewIpAddressInputColumn.ImeMode = ImeMode;
			dataGridViewIpAddressInputColumn.IsInputReadOnly = IsInputReadOnly;
			dataGridViewIpAddressInputColumn.LockUpdateChecked = LockUpdateChecked;
			dataGridViewIpAddressInputColumn.RightToLeft = RightToLeft;
			dataGridViewIpAddressInputColumn.SelectNextInputCharacters = SelectNextInputCharacters;
			dataGridViewIpAddressInputColumn.ShowCheckBox = ShowCheckBox;
			dataGridViewIpAddressInputColumn.WatermarkColor = WatermarkColor;
			dataGridViewIpAddressInputColumn.WatermarkEnabled = WatermarkEnabled;
			dataGridViewIpAddressInputColumn.WatermarkFont = WatermarkFont;
			dataGridViewIpAddressInputColumn.WatermarkText = WatermarkText;
			dataGridViewIpAddressInputColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewIpAddressInputColumn.BackgroundStyle.ApplyStyle(IpAddressInput.BackgroundStyle);
			dataGridViewIpAddressInputColumn.BackgroundStyle.Class = IpAddressInput.BackgroundStyle.Class;
			IpAddressInput.ButtonClear.CopyToItem(dataGridViewIpAddressInputColumn.IpAddressInput.ButtonClear);
			IpAddressInput.ButtonDropDown.CopyToItem(dataGridViewIpAddressInputColumn.IpAddressInput.ButtonDropDown);
			IpAddressInput.ButtonCustom.CopyToItem(dataGridViewIpAddressInputColumn.IpAddressInput.ButtonCustom);
			IpAddressInput.ButtonCustom2.CopyToItem(dataGridViewIpAddressInputColumn.IpAddressInput.ButtonCustom2);
		}
		return dataGridViewIpAddressInputColumn;
	}
}
