using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewTextBoxDropDownColumn), "Controls.TextBoxDropDown.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewTextBoxDropDownColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private TextBoxDropDown _TextBoxDropDown;

	private Bitmap _CellBitmap;

	private bool _DisplayControlForCurrentCellOnly = true;

	[Browsable(false)]
	internal TextBoxDropDown TextBoxDropDown => _TextBoxDropDown;

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Localizable(true)]
	[Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public AutoCompleteStringCollection AutoCompleteCustomSource
	{
		get
		{
			return _TextBoxDropDown.AutoCompleteCustomSource;
		}
		set
		{
			_TextBoxDropDown.AutoCompleteCustomSource = value;
		}
	}

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue(0)]
	[Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _TextBoxDropDown.AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_TextBoxDropDown.AutoCompleteMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(128)]
	[TypeConverter(typeof(TextBoxAutoCompleteSourceConverter))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _TextBoxDropDown.AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_TextBoxDropDown.AutoCompleteSource = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)_TextBoxDropDown).BackColor;
		}
		set
		{
			((Control)_TextBoxDropDown).BackColor = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _TextBoxDropDown.BackgroundStyle;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _TextBoxDropDown.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _TextBoxDropDown.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _TextBoxDropDown.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _TextBoxDropDown.ButtonDropDown;

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates whether the TextBox control modifies the case of characters as they are typed.")]
	public CharacterCasing CharacterCasing
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _TextBoxDropDown.CharacterCasing;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_TextBoxDropDown.CharacterCasing = value;
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
				((Control)_TextBoxDropDown).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return _TextBoxDropDown.DropDownControl;
		}
		set
		{
			_TextBoxDropDown.DropDownControl = value;
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
			return ((Control)_TextBoxDropDown).Enabled;
		}
		set
		{
			((Control)_TextBoxDropDown).Enabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return _TextBoxDropDown.FocusHighlightColor;
		}
		set
		{
			_TextBoxDropDown.FocusHighlightColor = value;
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
			return _TextBoxDropDown.FocusHighlightEnabled;
		}
		set
		{
			_TextBoxDropDown.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)_TextBoxDropDown).ForeColor;
		}
		set
		{
			((Control)_TextBoxDropDown).ForeColor = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
	public bool HideSelection
	{
		get
		{
			return _TextBoxDropDown.HideSelection;
		}
		set
		{
			_TextBoxDropDown.HideSelection = value;
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
			return ((Control)_TextBoxDropDown).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_TextBoxDropDown).ImeMode = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(32767)]
	[Localizable(true)]
	[Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
	public int MaxLength
	{
		get
		{
			return _TextBoxDropDown.MaxLength;
		}
		set
		{
			_TextBoxDropDown.MaxLength = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates the character to be displayed in substitute for user input.")]
	public char PasswordChar
	{
		get
		{
			return _TextBoxDropDown.PasswordChar;
		}
		set
		{
			_TextBoxDropDown.PasswordChar = value;
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
			return ((Control)_TextBoxDropDown).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_TextBoxDropDown).RightToLeft = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets a value indicating the currently selected text in the control.")]
	public virtual string SelectedText
	{
		get
		{
			return _TextBoxDropDown.SelectedText;
		}
		set
		{
			_TextBoxDropDown.SelectedText = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the number of characters selected in the text box.")]
	public int SelectionLength
	{
		get
		{
			return _TextBoxDropDown.SelectionLength;
		}
		set
		{
			_TextBoxDropDown.SelectionLength = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets the starting point of text selected in the text box.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			return _TextBoxDropDown.SelectionStart;
		}
		set
		{
			_TextBoxDropDown.SelectionStart = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)_TextBoxDropDown).Text;
		}
		set
		{
			((Control)_TextBoxDropDown).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates how text is aligned in a masked text box control.")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _TextBoxDropDown.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_TextBoxDropDown.TextAlign = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => _TextBoxDropDown.TextLength;

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether the operating system-supplied password character should be used.")]
	public bool UseSystemPasswordChar
	{
		get
		{
			return _TextBoxDropDown.UseSystemPasswordChar;
		}
		set
		{
			_TextBoxDropDown.UseSystemPasswordChar = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return _TextBoxDropDown.WatermarkBehavior;
		}
		set
		{
			_TextBoxDropDown.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return _TextBoxDropDown.WatermarkColor;
		}
		set
		{
			_TextBoxDropDown.WatermarkColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _TextBoxDropDown.WatermarkEnabled;
		}
		set
		{
			_TextBoxDropDown.WatermarkEnabled = value;
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
			return _TextBoxDropDown.WatermarkFont;
		}
		set
		{
			_TextBoxDropDown.WatermarkFont = value;
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
			return _TextBoxDropDown.WatermarkText;
		}
		set
		{
			_TextBoxDropDown.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a TextBoxDropDown Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick;

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick;

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click;

	[Description("Occurs when Drop-Down button is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick;

	public DataGridViewTextBoxDropDownColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewTextBoxDropDownCell();
		_TextBoxDropDown = new TextBoxDropDown();
		_TextBoxDropDown.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_TextBoxDropDown.ResetBackgroundStyle();
		_TextBoxDropDown.BackgroundStyle.Class = "TextBoxBorder";
		_TextBoxDropDown.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return _TextBoxDropDown.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		_TextBoxDropDown.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _TextBoxDropDown.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		_TextBoxDropDown.ResetWatermarkColor();
	}

	public void AppendText(string text)
	{
		_TextBoxDropDown.AppendText(text);
	}

	public void Clear()
	{
		_TextBoxDropDown.Clear();
	}

	public void ClearUndo()
	{
		_TextBoxDropDown.ClearUndo();
	}

	public void Copy()
	{
		_TextBoxDropDown.Copy();
	}

	public void Cut()
	{
		_TextBoxDropDown.Cut();
	}

	public void DeselectAll()
	{
		_TextBoxDropDown.DeselectAll();
	}

	public char GetCharFromPosition(Point pt)
	{
		return _TextBoxDropDown.GetCharFromPosition(pt);
	}

	public int GetCharIndexFromPosition(Point pt)
	{
		return _TextBoxDropDown.GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return _TextBoxDropDown.GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return _TextBoxDropDown.GetFirstCharIndexOfCurrentLine();
	}

	public int GetLineFromCharIndex(int index)
	{
		return _TextBoxDropDown.GetLineFromCharIndex(index);
	}

	public virtual Point GetPositionFromCharIndex(int index)
	{
		return _TextBoxDropDown.GetPositionFromCharIndex(index);
	}

	public void Paste()
	{
		_TextBoxDropDown.Paste();
	}

	public void Select(int start, int length)
	{
		_TextBoxDropDown.Select(start, length);
	}

	public void SelectAll()
	{
		_TextBoxDropDown.SelectAll();
	}

	public void Undo()
	{
		_TextBoxDropDown.Undo();
	}

	public void Paste(string text)
	{
		_TextBoxDropDown.Paste(text);
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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewTextBoxDropDownColumn dataGridViewTextBoxDropDownColumn = ((DataGridViewColumn)this).Clone() as DataGridViewTextBoxDropDownColumn;
		if (dataGridViewTextBoxDropDownColumn != null)
		{
			dataGridViewTextBoxDropDownColumn.AutoCompleteCustomSource = AutoCompleteCustomSource;
			dataGridViewTextBoxDropDownColumn.AutoCompleteMode = AutoCompleteMode;
			dataGridViewTextBoxDropDownColumn.AutoCompleteSource = AutoCompleteSource;
			dataGridViewTextBoxDropDownColumn.BackColor = BackColor;
			dataGridViewTextBoxDropDownColumn.CharacterCasing = CharacterCasing;
			dataGridViewTextBoxDropDownColumn.DropDownControl = DropDownControl;
			dataGridViewTextBoxDropDownColumn.Enabled = Enabled;
			dataGridViewTextBoxDropDownColumn.FocusHighlightColor = FocusHighlightColor;
			dataGridViewTextBoxDropDownColumn.FocusHighlightEnabled = FocusHighlightEnabled;
			dataGridViewTextBoxDropDownColumn.ForeColor = ForeColor;
			dataGridViewTextBoxDropDownColumn.HideSelection = HideSelection;
			dataGridViewTextBoxDropDownColumn.ImeMode = ImeMode;
			dataGridViewTextBoxDropDownColumn.MaxLength = MaxLength;
			dataGridViewTextBoxDropDownColumn.PasswordChar = PasswordChar;
			dataGridViewTextBoxDropDownColumn.RightToLeft = RightToLeft;
			dataGridViewTextBoxDropDownColumn.TextAlign = TextAlign;
			dataGridViewTextBoxDropDownColumn.UseSystemPasswordChar = UseSystemPasswordChar;
			dataGridViewTextBoxDropDownColumn.WatermarkBehavior = WatermarkBehavior;
			dataGridViewTextBoxDropDownColumn.WatermarkColor = WatermarkColor;
			dataGridViewTextBoxDropDownColumn.WatermarkEnabled = WatermarkEnabled;
			dataGridViewTextBoxDropDownColumn.WatermarkFont = WatermarkFont;
			dataGridViewTextBoxDropDownColumn.WatermarkText = WatermarkText;
			dataGridViewTextBoxDropDownColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewTextBoxDropDownColumn.BackgroundStyle.ApplyStyle(TextBoxDropDown.BackgroundStyle);
			dataGridViewTextBoxDropDownColumn.BackgroundStyle.Class = TextBoxDropDown.BackgroundStyle.Class;
			TextBoxDropDown.ButtonClear.CopyToItem(dataGridViewTextBoxDropDownColumn.TextBoxDropDown.ButtonClear);
			TextBoxDropDown.ButtonDropDown.CopyToItem(dataGridViewTextBoxDropDownColumn.TextBoxDropDown.ButtonDropDown);
			TextBoxDropDown.ButtonCustom.CopyToItem(dataGridViewTextBoxDropDownColumn.TextBoxDropDown.ButtonCustom);
			TextBoxDropDown.ButtonCustom2.CopyToItem(dataGridViewTextBoxDropDownColumn.TextBoxDropDown.ButtonCustom2);
		}
		return dataGridViewTextBoxDropDownColumn;
	}
}
