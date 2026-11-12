using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewMaskedTextBoxAdvColumn), "Controls.MaskedTextBoxAdv.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewMaskedTextBoxAdvColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private MaskedTextBoxAdv _MaskedTextBoxAdv;

	private Bitmap _CellBitmap;

	private bool _DisplayControlForCurrentCellOnly = true;

	[Browsable(false)]
	internal MaskedTextBoxAdv MaskedTextBoxAdv => _MaskedTextBoxAdv;

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether PromptChar can be entered as valid data.")]
	public bool AllowPromptAsInput
	{
		get
		{
			return _MaskedTextBoxAdv.AllowPromptAsInput;
		}
		set
		{
			_MaskedTextBoxAdv.AllowPromptAsInput = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether whether characters outside of the ASCII character set will be accepted.")]
	public bool AsciiOnly
	{
		get
		{
			return _MaskedTextBoxAdv.AsciiOnly;
		}
		set
		{
			_MaskedTextBoxAdv.AsciiOnly = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)_MaskedTextBoxAdv).BackColor;
		}
		set
		{
			((Control)_MaskedTextBoxAdv).BackColor = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _MaskedTextBoxAdv.BackgroundStyle;

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects.")]
	public bool BeepOnError
	{
		get
		{
			return _MaskedTextBoxAdv.BeepOnError;
		}
		set
		{
			_MaskedTextBoxAdv.BeepOnError = value;
		}
	}

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _MaskedTextBoxAdv.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _MaskedTextBoxAdv.ButtonCustom;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _MaskedTextBoxAdv.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _MaskedTextBoxAdv.ButtonDropDown;

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates the culture information associated with the masked text box.")]
	public CultureInfo Culture
	{
		get
		{
			return _MaskedTextBoxAdv.Culture;
		}
		set
		{
			_MaskedTextBoxAdv.Culture = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates whether literals and prompt characters are copied to the clipboard.")]
	public MaskFormat CutCopyMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBoxAdv.CutCopyMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBoxAdv.CutCopyMaskFormat = value;
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
				((Control)_MaskedTextBoxAdv).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return _MaskedTextBoxAdv.DropDownControl;
		}
		set
		{
			_MaskedTextBoxAdv.DropDownControl = value;
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
			return ((Control)_MaskedTextBoxAdv).Enabled;
		}
		set
		{
			((Control)_MaskedTextBoxAdv).Enabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return _MaskedTextBoxAdv.FocusHighlightColor;
		}
		set
		{
			_MaskedTextBoxAdv.FocusHighlightColor = value;
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
			return _MaskedTextBoxAdv.FocusHighlightEnabled;
		}
		set
		{
			_MaskedTextBoxAdv.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)_MaskedTextBoxAdv).ForeColor;
		}
		set
		{
			((Control)_MaskedTextBoxAdv).ForeColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus.")]
	public bool HidePromptOnLeave
	{
		get
		{
			return _MaskedTextBoxAdv.HidePromptOnLeave;
		}
		set
		{
			_MaskedTextBoxAdv.HidePromptOnLeave = value;
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
			return ((Control)_MaskedTextBoxAdv).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_MaskedTextBoxAdv).ImeMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the text insertion mode of the masked text box control.")]
	public InsertKeyMode InsertKeyMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBoxAdv.InsertKeyMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBoxAdv.InsertKeyMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates the input mask to use at run time.")]
	public string Mask
	{
		get
		{
			return _MaskedTextBoxAdv.Mask;
		}
		set
		{
			_MaskedTextBoxAdv.Mask = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates the character to be displayed in substitute for user input.")]
	public char PasswordChar
	{
		get
		{
			return _MaskedTextBoxAdv.PasswordChar;
		}
		set
		{
			_MaskedTextBoxAdv.PasswordChar = value;
		}
	}

	[Browsable(true)]
	[DefaultValue('_')]
	[Description("Indicates the character used to represent the absence of user input.")]
	public char PromptChar
	{
		get
		{
			return _MaskedTextBoxAdv.PromptChar;
		}
		set
		{
			_MaskedTextBoxAdv.PromptChar = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates the parsing of user input should stop after the first invalid character is reached.")]
	public bool RejectInputOnFirstFailure
	{
		get
		{
			return _MaskedTextBoxAdv.RejectInputOnFirstFailure;
		}
		set
		{
			_MaskedTextBoxAdv.RejectInputOnFirstFailure = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates how an input character that matches the prompt character should be handled.")]
	public bool ResetOnPrompt
	{
		get
		{
			return _MaskedTextBoxAdv.ResetOnPrompt;
		}
		set
		{
			_MaskedTextBoxAdv.ResetOnPrompt = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates how a space input character should be handled.")]
	public bool ResetOnSpace
	{
		get
		{
			return _MaskedTextBoxAdv.ResetOnSpace;
		}
		set
		{
			_MaskedTextBoxAdv.ResetOnSpace = value;
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
			return ((Control)_MaskedTextBoxAdv).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_MaskedTextBoxAdv).RightToLeft = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates the user is allowed to reenter literal values.")]
	public bool SkipLiterals
	{
		get
		{
			return _MaskedTextBoxAdv.SkipLiterals;
		}
		set
		{
			_MaskedTextBoxAdv.SkipLiterals = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)_MaskedTextBoxAdv).Text;
		}
		set
		{
			((Control)_MaskedTextBoxAdv).Text = value;
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
			return _MaskedTextBoxAdv.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBoxAdv.TextAlign = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates whether literals and prompt characters are included in the formatted string.")]
	public MaskFormat TextMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBoxAdv.TextMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBoxAdv.TextMaskFormat = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether the operating system-supplied password character should be used.")]
	public bool UseSystemPasswordChar
	{
		get
		{
			return _MaskedTextBoxAdv.UseSystemPasswordChar;
		}
		set
		{
			_MaskedTextBoxAdv.UseSystemPasswordChar = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[Description("Indicates the data type used to verify the data input by the user.")]
	public Type ValidatingType
	{
		get
		{
			return _MaskedTextBoxAdv.ValidatingType;
		}
		set
		{
			_MaskedTextBoxAdv.ValidatingType = value;
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
			return _MaskedTextBoxAdv.WatermarkBehavior;
		}
		set
		{
			_MaskedTextBoxAdv.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return _MaskedTextBoxAdv.WatermarkColor;
		}
		set
		{
			_MaskedTextBoxAdv.WatermarkColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _MaskedTextBoxAdv.WatermarkEnabled;
		}
		set
		{
			_MaskedTextBoxAdv.WatermarkEnabled = value;
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
			return _MaskedTextBoxAdv.WatermarkFont;
		}
		set
		{
			_MaskedTextBoxAdv.WatermarkFont = value;
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
			return _MaskedTextBoxAdv.WatermarkText;
		}
		set
		{
			_MaskedTextBoxAdv.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a MaskedTextBoxAdv Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
	public event EventHandler<CancelEventArgs> ButtonClearClick;

	[Description("Occurs when ButtonCustom control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustomClick;

	[Description("Occurs when ButtonCustom2 control is clicked.")]
	public event EventHandler<EventArgs> ButtonCustom2Click;

	[Description("Occurs when Drop-Down button is clicked and allows you to cancel showing of the popup.")]
	public event EventHandler<CancelEventArgs> ButtonDropDownClick;

	public DataGridViewMaskedTextBoxAdvColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewMaskedTextBoxAdvCell();
		_MaskedTextBoxAdv = new MaskedTextBoxAdv();
		_MaskedTextBoxAdv.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_MaskedTextBoxAdv.ResetBackgroundStyle();
		_MaskedTextBoxAdv.BackgroundStyle.Class = "TextBoxBorder";
		_MaskedTextBoxAdv.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return _MaskedTextBoxAdv.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		_MaskedTextBoxAdv.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _MaskedTextBoxAdv.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		_MaskedTextBoxAdv.ResetWatermarkColor();
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewMaskedTextBoxAdvColumn dataGridViewMaskedTextBoxAdvColumn = ((DataGridViewColumn)this).Clone() as DataGridViewMaskedTextBoxAdvColumn;
		if (dataGridViewMaskedTextBoxAdvColumn != null)
		{
			dataGridViewMaskedTextBoxAdvColumn.AllowPromptAsInput = AllowPromptAsInput;
			dataGridViewMaskedTextBoxAdvColumn.AsciiOnly = AsciiOnly;
			dataGridViewMaskedTextBoxAdvColumn.BeepOnError = BeepOnError;
			dataGridViewMaskedTextBoxAdvColumn.Culture = Culture;
			dataGridViewMaskedTextBoxAdvColumn.CutCopyMaskFormat = CutCopyMaskFormat;
			dataGridViewMaskedTextBoxAdvColumn.DropDownControl = DropDownControl;
			dataGridViewMaskedTextBoxAdvColumn.Enabled = Enabled;
			dataGridViewMaskedTextBoxAdvColumn.FocusHighlightColor = FocusHighlightColor;
			dataGridViewMaskedTextBoxAdvColumn.FocusHighlightEnabled = FocusHighlightEnabled;
			dataGridViewMaskedTextBoxAdvColumn.HidePromptOnLeave = HidePromptOnLeave;
			dataGridViewMaskedTextBoxAdvColumn.ImeMode = ImeMode;
			dataGridViewMaskedTextBoxAdvColumn.InsertKeyMode = InsertKeyMode;
			dataGridViewMaskedTextBoxAdvColumn.Mask = Mask;
			dataGridViewMaskedTextBoxAdvColumn.PasswordChar = PasswordChar;
			dataGridViewMaskedTextBoxAdvColumn.PromptChar = PromptChar;
			dataGridViewMaskedTextBoxAdvColumn.RejectInputOnFirstFailure = RejectInputOnFirstFailure;
			dataGridViewMaskedTextBoxAdvColumn.ResetOnPrompt = ResetOnPrompt;
			dataGridViewMaskedTextBoxAdvColumn.ResetOnSpace = ResetOnSpace;
			dataGridViewMaskedTextBoxAdvColumn.RightToLeft = RightToLeft;
			dataGridViewMaskedTextBoxAdvColumn.SkipLiterals = SkipLiterals;
			dataGridViewMaskedTextBoxAdvColumn.TextAlign = TextAlign;
			dataGridViewMaskedTextBoxAdvColumn.TextMaskFormat = TextMaskFormat;
			dataGridViewMaskedTextBoxAdvColumn.UseSystemPasswordChar = UseSystemPasswordChar;
			dataGridViewMaskedTextBoxAdvColumn.ValidatingType = ValidatingType;
			dataGridViewMaskedTextBoxAdvColumn.WatermarkBehavior = WatermarkBehavior;
			dataGridViewMaskedTextBoxAdvColumn.WatermarkColor = WatermarkColor;
			dataGridViewMaskedTextBoxAdvColumn.WatermarkEnabled = WatermarkEnabled;
			dataGridViewMaskedTextBoxAdvColumn.WatermarkFont = WatermarkFont;
			dataGridViewMaskedTextBoxAdvColumn.WatermarkText = WatermarkText;
			dataGridViewMaskedTextBoxAdvColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
			dataGridViewMaskedTextBoxAdvColumn.BackgroundStyle.ApplyStyle(MaskedTextBoxAdv.BackgroundStyle);
			dataGridViewMaskedTextBoxAdvColumn.BackgroundStyle.Class = MaskedTextBoxAdv.BackgroundStyle.Class;
			MaskedTextBoxAdv.ButtonClear.CopyToItem(dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv.ButtonClear);
			MaskedTextBoxAdv.ButtonDropDown.CopyToItem(dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv.ButtonDropDown);
			MaskedTextBoxAdv.ButtonCustom.CopyToItem(dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv.ButtonCustom);
			MaskedTextBoxAdv.ButtonCustom2.CopyToItem(dataGridViewMaskedTextBoxAdvColumn.MaskedTextBoxAdv.ButtonCustom2);
		}
		return dataGridViewMaskedTextBoxAdvColumn;
	}
}
