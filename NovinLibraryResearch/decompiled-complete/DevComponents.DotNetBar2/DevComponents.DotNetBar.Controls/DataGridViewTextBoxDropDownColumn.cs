using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
[ToolboxBitmap(typeof(DataGridViewTextBoxDropDownColumn), "Controls.TextBoxDropDown.ico")]
public class DataGridViewTextBoxDropDownColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<CancelEventArgs> _៦;

	private EventHandler<EventArgs> _181E;

	private EventHandler<EventArgs> _181F;

	private EventHandler<CancelEventArgs> _៧;

	private TextBoxDropDown __2B65;

	private Bitmap __2AFE;

	private bool __2B23 = true;

	[Browsable(false)]
	internal TextBoxDropDown _2B64 => __2B65;

	[Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Localizable(true)]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public AutoCompleteStringCollection AutoCompleteCustomSource
	{
		get
		{
			return __2B65.AutoCompleteCustomSource;
		}
		set
		{
			__2B65.AutoCompleteCustomSource = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue(0)]
	[Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
	[Browsable(true)]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B65.AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B65.AutoCompleteMode = value;
		}
	}

	[TypeConverter(typeof(Ӓ))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
	[DefaultValue(128)]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B65.AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B65.AutoCompleteSource = value;
		}
	}

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)__2B65).BackColor;
		}
		set
		{
			((Control)__2B65).BackColor = value;
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets control background style.")]
	public ElementStyle BackgroundStyle => __2B65.BackgroundStyle;

	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonClear => __2B65.ButtonClear;

	[Browsable(true)]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom => __2B65.ButtonCustom;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public InputButtonSettings ButtonCustom2 => __2B65.ButtonCustom2;

	[Browsable(true)]
	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => __2B65.ButtonDropDown;

	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates whether the TextBox control modifies the case of characters as they are typed.")]
	public CharacterCasing CharacterCasing
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B65.CharacterCasing;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B65.CharacterCasing = value;
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
			return __2B23;
		}
		set
		{
			if (__2B23 != value)
			{
				__2B23 = value;
				((Control)__2B65).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return __2B65.DropDownControl;
		}
		set
		{
			__2B65.DropDownControl = value;
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
			return ((Control)__2B65).Enabled;
		}
		set
		{
			((Control)__2B65).Enabled = value;
		}
	}

	[Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color FocusHighlightColor
	{
		get
		{
			return __2B65.FocusHighlightColor;
		}
		set
		{
			__2B65.FocusHighlightColor = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight the text box when it has input focus.")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return __2B65.FocusHighlightEnabled;
		}
		set
		{
			__2B65.FocusHighlightEnabled = value;
		}
	}

	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return ((Control)__2B65).ForeColor;
		}
		set
		{
			((Control)__2B65).ForeColor = value;
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool HideSelection
	{
		get
		{
			return __2B65.HideSelection;
		}
		set
		{
			__2B65.HideSelection = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	[Browsable(true)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)__2B65).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B65).ImeMode = value;
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
			return __2B65.MaxLength;
		}
		set
		{
			__2B65.MaxLength = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the character to be displayed in substitute for user input.")]
	[DefaultValue(null)]
	public char PasswordChar
	{
		get
		{
			return __2B65.PasswordChar;
		}
		set
		{
			__2B65.PasswordChar = value;
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
			return ((Control)__2B65).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B65).RightToLeft = value;
		}
	}

	[Description("Gets or sets a value indicating the currently selected text in the control.")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual string SelectedText
	{
		get
		{
			return __2B65.SelectedText;
		}
		set
		{
			__2B65.SelectedText = value;
		}
	}

	[Browsable(false)]
	[Description("Gets or sets the number of characters selected in the text box.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Appearance")]
	public int SelectionLength
	{
		get
		{
			return __2B65.SelectionLength;
		}
		set
		{
			__2B65.SelectionLength = value;
		}
	}

	[Description("Gets or sets the starting point of text selected in the text box.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			return __2B65.SelectionStart;
		}
		set
		{
			__2B65.SelectionStart = value;
		}
	}

	[Browsable(false)]
	public string Text
	{
		get
		{
			return ((Control)__2B65).Text;
		}
		set
		{
			((Control)__2B65).Text = value;
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
			return __2B65.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B65.TextAlign = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => __2B65.TextLength;

	[Browsable(true)]
	[Description("Indicates whether the operating system-supplied password character should be used.")]
	[DefaultValue(false)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return __2B65.UseSystemPasswordChar;
		}
		set
		{
			__2B65.UseSystemPasswordChar = value;
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Browsable(true)]
	[Description("Indicates watermark hiding behaviour.")]
	[Category("Behavior")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return __2B65.WatermarkBehavior;
		}
		set
		{
			__2B65.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates watermark text color.")]
	[Category("Appearance")]
	public Color WatermarkColor
	{
		get
		{
			return __2B65.WatermarkColor;
		}
		set
		{
			__2B65.WatermarkColor = value;
		}
	}

	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[Browsable(true)]
	[DefaultValue(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return __2B65.WatermarkEnabled;
		}
		set
		{
			__2B65.WatermarkEnabled = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return __2B65.WatermarkFont;
		}
		set
		{
			__2B65.WatermarkFont = value;
		}
	}

	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Category("Appearance")]
	[Localizable(true)]
	[Browsable(true)]
	public string WatermarkText
	{
		get
		{
			return __2B65.WatermarkText;
		}
		set
		{
			__2B65.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a TextBoxDropDown Cell is painted.")]
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

	public DataGridViewTextBoxDropDownColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewTextBoxDropDownCell();
		__2B65 = new TextBoxDropDown();
		__2B65.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		__2B65.ResetBackgroundStyle();
		__2B65.BackgroundStyle.Class = "TextBoxBorder";
		__2B65.BackgroundStyle.CornerType = eCornerType.Square;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return __2B65.ShouldSerializeFocusHighlightColor();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		__2B65.ResetFocusHighlightColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return __2B65.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		__2B65.ResetWatermarkColor();
	}

	public void AppendText(string text)
	{
		__2B65.AppendText(text);
	}

	public void Clear()
	{
		__2B65.Clear();
	}

	public void ClearUndo()
	{
		__2B65.ClearUndo();
	}

	public void Copy()
	{
		__2B65.Copy();
	}

	public void Cut()
	{
		__2B65.Cut();
	}

	public void DeselectAll()
	{
		__2B65.DeselectAll();
	}

	public char GetCharFromPosition(Point pt)
	{
		return __2B65.GetCharFromPosition(pt);
	}

	public int GetCharIndexFromPosition(Point pt)
	{
		return __2B65.GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return __2B65.GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return __2B65.GetFirstCharIndexOfCurrentLine();
	}

	public int GetLineFromCharIndex(int index)
	{
		return __2B65.GetLineFromCharIndex(index);
	}

	public virtual Point GetPositionFromCharIndex(int index)
	{
		return __2B65.GetPositionFromCharIndex(index);
	}

	public void Paste()
	{
		__2B65.Paste();
	}

	public void Select(int start, int length)
	{
		__2B65.Select(start, length);
	}

	public void SelectAll()
	{
		__2B65.SelectAll();
	}

	public void Undo()
	{
		__2B65.Undo();
	}

	public void Paste(string text)
	{
		__2B65.Paste(text);
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
			dataGridViewTextBoxDropDownColumn.BackgroundStyle.ApplyStyle(_2B64.BackgroundStyle);
			dataGridViewTextBoxDropDownColumn.BackgroundStyle.Class = _2B64.BackgroundStyle.Class;
			_2B64.ButtonClear.ᡄ(dataGridViewTextBoxDropDownColumn._2B64.ButtonClear);
			_2B64.ButtonDropDown.ᡄ(dataGridViewTextBoxDropDownColumn._2B64.ButtonDropDown);
			_2B64.ButtonCustom.ᡄ(dataGridViewTextBoxDropDownColumn._2B64.ButtonCustom);
			_2B64.ButtonCustom2.ᡄ(dataGridViewTextBoxDropDownColumn._2B64.ButtonCustom2);
		}
		return dataGridViewTextBoxDropDownColumn;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Component)(object)__2B65).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
