using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewMaskedTextBoxAdvColumn), "Controls.MaskedTextBoxAdv.ico")]
[ComVisible(false)]
[ToolboxItem(false)]
public class DataGridViewMaskedTextBoxAdvColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private MaskedTextBoxAdv __2B52;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	[Browsable(false)]
	internal MaskedTextBoxAdv _28CE => __2B52;

	[Description("Indicates whether PromptChar can be entered as valid data.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool AllowPromptAsInput
	{
		get
		{
			return __2B52.AllowPromptAsInput;
		}
		set
		{
			__2B52.AllowPromptAsInput = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether whether characters outside of the ASCII character set will be accepted.")]
	[DefaultValue(false)]
	public bool AsciiOnly
	{
		get
		{
			return __2B52.AsciiOnly;
		}
		set
		{
			__2B52.AsciiOnly = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)__2B52).BackColor;
		}
		set
		{
			((Control)__2B52).BackColor = value;
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets control background style.")]
	[Browsable(true)]
	public ElementStyle BackgroundStyle => __2B52.BackgroundStyle;

	[Browsable(true)]
	[Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects.")]
	[DefaultValue(false)]
	public bool BeepOnError
	{
		get
		{
			return __2B52.BeepOnError;
		}
		set
		{
			__2B52.BeepOnError = value;
		}
	}

	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonClear => __2B52.ButtonClear;

	[Category("Buttons")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom => __2B52.ButtonCustom;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Browsable(true)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom2 => __2B52.ButtonCustom2;

	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[Browsable(true)]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => __2B52.ButtonDropDown;

	[Description("Indicates the culture information associated with the masked text box.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public CultureInfo Culture
	{
		get
		{
			return __2B52.Culture;
		}
		set
		{
			__2B52.Culture = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Indicates whether literals and prompt characters are copied to the clipboard.")]
	public MaskFormat CutCopyMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B52.CutCopyMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B52.CutCopyMaskFormat = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
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
				((Control)__2B52).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return __2B52.DropDownControl;
		}
		set
		{
			__2B52.DropDownControl = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the control can respond to user interaction.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B52).Enabled;
		}
		set
		{
			((Control)__2B52).Enabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return __2B52.FocusHighlightColor;
		}
		set
		{
			__2B52.FocusHighlightColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight the text box when it has input focus.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool FocusHighlightEnabled
	{
		get
		{
			return __2B52.FocusHighlightEnabled;
		}
		set
		{
			__2B52.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)__2B52).ForeColor;
		}
		set
		{
			((Control)__2B52).ForeColor = value;
		}
	}

	[Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool HidePromptOnLeave
	{
		get
		{
			return __2B52.HidePromptOnLeave;
		}
		set
		{
			__2B52.HidePromptOnLeave = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)__2B52).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B52).ImeMode = value;
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
			return __2B52.InsertKeyMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B52.InsertKeyMode = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates the input mask to use at run time.")]
	[Browsable(true)]
	public string Mask
	{
		get
		{
			return __2B52.Mask;
		}
		set
		{
			__2B52.Mask = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the character to be displayed in substitute for user input.")]
	[DefaultValue(null)]
	public char PasswordChar
	{
		get
		{
			return __2B52.PasswordChar;
		}
		set
		{
			__2B52.PasswordChar = value;
		}
	}

	[Browsable(true)]
	[DefaultValue('_')]
	[Description("Indicates the character used to represent the absence of user input.")]
	public char PromptChar
	{
		get
		{
			return __2B52.PromptChar;
		}
		set
		{
			__2B52.PromptChar = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates the parsing of user input should stop after the first invalid character is reached.")]
	public bool RejectInputOnFirstFailure
	{
		get
		{
			return __2B52.RejectInputOnFirstFailure;
		}
		set
		{
			__2B52.RejectInputOnFirstFailure = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates how an input character that matches the prompt character should be handled.")]
	public bool ResetOnPrompt
	{
		get
		{
			return __2B52.ResetOnPrompt;
		}
		set
		{
			__2B52.ResetOnPrompt = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates how a space input character should be handled.")]
	public bool ResetOnSpace
	{
		get
		{
			return __2B52.ResetOnSpace;
		}
		set
		{
			__2B52.ResetOnSpace = value;
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
			return ((Control)__2B52).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B52).RightToLeft = value;
		}
	}

	[Description("Indicates the user is allowed to reenter literal values.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool SkipLiterals
	{
		get
		{
			return __2B52.SkipLiterals;
		}
		set
		{
			__2B52.SkipLiterals = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)__2B52).Text;
		}
		set
		{
			((Control)__2B52).Text = value;
		}
	}

	[Description("Indicates how text is aligned in a masked text box control.")]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B52.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B52.TextAlign = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether literals and prompt characters are included in the formatted string.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public MaskFormat TextMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B52.TextMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B52.TextMaskFormat = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the operating system-supplied password character should be used.")]
	[DefaultValue(false)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return __2B52.UseSystemPasswordChar;
		}
		set
		{
			__2B52.UseSystemPasswordChar = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[Description("Indicates the data type used to verify the data input by the user.")]
	public Type ValidatingType
	{
		get
		{
			return __2B52.ValidatingType;
		}
		set
		{
			__2B52.ValidatingType = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Description("Indicates watermark hiding behaviour.")]
	[Browsable(true)]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return __2B52.WatermarkBehavior;
		}
		set
		{
			__2B52.WatermarkBehavior = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return __2B52.WatermarkColor;
		}
		set
		{
			__2B52.WatermarkColor = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[Browsable(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return __2B52.WatermarkEnabled;
		}
		set
		{
			__2B52.WatermarkEnabled = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Font WatermarkFont
	{
		get
		{
			return __2B52.WatermarkFont;
		}
		set
		{
			__2B52.WatermarkFont = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Browsable(true)]
	public string WatermarkText
	{
		get
		{
			return __2B52.WatermarkText;
		}
		set
		{
			__2B52.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a MaskedTextBoxAdv Cell is painted.")]
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

	[Description("Occurs when Drop-Down button is clicked and allows you to cancel showing of the popup.")]
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

	public DataGridViewMaskedTextBoxAdvColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewMaskedTextBoxAdvCell();
		__2B52 = new MaskedTextBoxAdv();
		__2B52.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__2B52.ResetBackgroundStyle();
		__2B52.BackgroundStyle.Class = "TextBoxBorder";
		__2B52.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return __2B52.ShouldSerializeFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetFocusHighlightColor()
	{
		__2B52.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return __2B52.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		__2B52.ResetWatermarkColor();
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
			dataGridViewMaskedTextBoxAdvColumn.BackgroundStyle.ApplyStyle(_28CE.BackgroundStyle);
			dataGridViewMaskedTextBoxAdvColumn.BackgroundStyle.Class = _28CE.BackgroundStyle.Class;
			_28CE.ButtonClear.ᡄ(dataGridViewMaskedTextBoxAdvColumn._28CE.ButtonClear);
			_28CE.ButtonDropDown.ᡄ(dataGridViewMaskedTextBoxAdvColumn._28CE.ButtonDropDown);
			_28CE.ButtonCustom.ᡄ(dataGridViewMaskedTextBoxAdvColumn._28CE.ButtonCustom);
			_28CE.ButtonCustom2.ᡄ(dataGridViewMaskedTextBoxAdvColumn._28CE.ButtonCustom2);
		}
		return dataGridViewMaskedTextBoxAdvColumn;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__2B52).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
