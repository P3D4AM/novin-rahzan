using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(TextBoxDropDown), "Controls.TextBoxDropDown.ico")]
[ToolboxItem(false)]
[DefaultProperty("Text")]
[DefaultBindingProperty("Text")]
[DefaultEvent("TextChanged")]
public class TextBoxDropDown : PopupItemControl, IInputButtonControl, ICommandSource
{
	private TextBoxX _TextBox;

	private ButtonItem _PopupItem;

	private static string _DropDownItemContainerName = "sysPopupItemContainer";

	private static string _DropDownControlContainerName = "sysPopupControlContainer";

	private Color _FocusHighlightColor = ColorScheme.GetColor(16777096);

	private bool _FocusHighlightEnabled;

	private bool _AutoSelectAll;

	private Color _OldBackColor = Color.Empty;

	private ElementStyle _BackgroundStyle = new ElementStyle();

	private InputButtonSettings _ButtonDropDown;

	private InputButtonSettings _ButtonClear;

	private InputButtonSettings _ButtonCustom;

	private InputButtonSettings _ButtonCustom2;

	private VisualGroup _ButtonGroup;

	private bool _MouseOver;

	private bool _InternalSizeUpdate;

	private Control _PreviousDropDownControlParent;

	private bool _ShowingPopup;

	private DateTime _CloseTime = DateTime.MinValue;

	private Control _DropDownControl;

	private bool _InternalReadOnly;

	private ICommand _Command;

	private object _CommandParameter;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether all text is auto-selected when control gets input focus.")]
	public bool AutoSelectAll
	{
		get
		{
			return _AutoSelectAll;
		}
		set
		{
			_AutoSelectAll = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return _FocusHighlightEnabled;
		}
		set
		{
			if (_FocusHighlightEnabled != value)
			{
				_FocusHighlightEnabled = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	public Color FocusHighlightColor
	{
		get
		{
			return _FocusHighlightColor;
		}
		set
		{
			if (_FocusHighlightColor != value)
			{
				_FocusHighlightColor = value;
				_TextBox.FocusHighlightColor = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	[Category("Buttons")]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonDropDown => _ButtonDropDown;

	[Category("Buttons")]
	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => _ButtonClear;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _ButtonCustom;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _ButtonCustom2;

	internal VisualGroup ButtonGroup => _ButtonGroup;

	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPopupOpen
	{
		get
		{
			return _PopupItem.Expanded;
		}
		set
		{
			if (value != _PopupItem.Expanded)
			{
				if (value)
				{
					ShowDropDown();
				}
				else
				{
					CloseDropDown();
				}
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return _DropDownControl;
		}
		set
		{
			if (value != this)
			{
				_DropDownControl = value;
				OnDropDownControlChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection DropDownItems => _PopupItem.SubItems;

	private bool RenderButtons
	{
		get
		{
			if ((_ButtonCustom == null || !_ButtonCustom.Visible) && (_ButtonCustom2 == null || !_ButtonCustom2.Visible) && (_ButtonDropDown == null || !_ButtonDropDown.Visible))
			{
				if (_ButtonClear != null)
				{
					return _ButtonClear.Visible;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TextBoxX TextBox => _TextBox;

	internal bool InternalReadOnly
	{
		get
		{
			return _InternalReadOnly;
		}
		set
		{
			_InternalReadOnly = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _TextBox.WatermarkEnabled;
		}
		set
		{
			_TextBox.WatermarkEnabled = value;
			((Control)this).Invalidate(true);
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
			return _TextBox.WatermarkText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_TextBox.WatermarkText = value;
			((Control)this).Invalidate(true);
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return _TextBox.WatermarkFont;
		}
		set
		{
			_TextBox.WatermarkFont = value;
			((Control)this).Invalidate(true);
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return _TextBox.WatermarkColor;
		}
		set
		{
			_TextBox.WatermarkColor = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return _TextBox.WatermarkBehavior;
		}
		set
		{
			_TextBox.WatermarkBehavior = value;
			((Control)this).Invalidate(true);
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	public Image WatermarkImage
	{
		get
		{
			return _TextBox.WatermarkImage;
		}
		set
		{
			_TextBox.WatermarkImage = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates watermark image alignment.")]
	public ContentAlignment WatermarkImageAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _TextBox.WatermarkImageAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_TextBox.WatermarkImageAlignment = value;
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

	[Category("Appearance")]
	[DefaultValue("Indicates text as it is currently displayed to the user")]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Localizable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)_TextBox).Text = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int PreferredHeight
	{
		get
		{
			int preferredHeight = ((TextBoxBase)_TextBox).PreferredHeight;
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			preferredHeight += ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
			if (disposeStyle)
			{
				backgroundStyle.Dispose();
			}
			return preferredHeight;
		}
	}

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
			return ((TextBox)_TextBox).AutoCompleteCustomSource;
		}
		set
		{
			((TextBox)_TextBox).AutoCompleteCustomSource = value;
		}
	}

	[Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue(0)]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)_TextBox).AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)_TextBox).AutoCompleteMode = value;
		}
	}

	[DefaultValue(128)]
	[TypeConverter(typeof(TextBoxAutoCompleteSourceConverter))]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)_TextBox).AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)_TextBox).AutoCompleteSource = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether the TextBox control modifies the case of characters as they are typed.")]
	[DefaultValue(0)]
	public CharacterCasing CharacterCasing
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)_TextBox).CharacterCasing;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)_TextBox).CharacterCasing = value;
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[Localizable(true)]
	[Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
	[Category("Behavior")]
	[DefaultValue('\0')]
	public char PasswordChar
	{
		get
		{
			return ((TextBox)_TextBox).PasswordChar;
		}
		set
		{
			((TextBox)_TextBox).PasswordChar = value;
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Localizable(true)]
	[Description("Indicates how text is aligned in a TextBox control.")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)_TextBox).TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)_TextBox).TextAlign = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return ((TextBox)_TextBox).UseSystemPasswordChar;
		}
		set
		{
			((TextBox)_TextBox).UseSystemPasswordChar = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
	public bool HideSelection
	{
		get
		{
			return ((TextBoxBase)_TextBox).HideSelection;
		}
		set
		{
			((TextBoxBase)_TextBox).HideSelection = value;
		}
	}

	[Category("Behavior")]
	[Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
	[DefaultValue(32767)]
	[Localizable(true)]
	public virtual int MaxLength
	{
		get
		{
			return ((TextBoxBase)_TextBox).MaxLength;
		}
		set
		{
			((TextBoxBase)_TextBox).MaxLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool Modified
	{
		get
		{
			return ((TextBoxBase)_TextBox).Modified;
		}
		set
		{
			((TextBoxBase)_TextBox).Modified = value;
		}
	}

	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	[Description("Gets or sets a value indicating whether text in the text box is read-only.")]
	public bool ReadOnly
	{
		get
		{
			return ((TextBoxBase)_TextBox).ReadOnly;
		}
		set
		{
			((TextBoxBase)_TextBox).ReadOnly = value;
		}
	}

	[Description("Gets or sets a value indicating the currently selected text in the control.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string SelectedText
	{
		get
		{
			return ((TextBoxBase)_TextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)_TextBox).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the number of characters selected in the text box.")]
	[Category("Appearance")]
	public virtual int SelectionLength
	{
		get
		{
			return ((TextBoxBase)_TextBox).SelectionLength;
		}
		set
		{
			((TextBoxBase)_TextBox).SelectionLength = value;
		}
	}

	[Description("Gets or sets the starting point of text selected in the text box.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Appearance")]
	[Browsable(false)]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)_TextBox).SelectionStart;
		}
		set
		{
			((TextBoxBase)_TextBox).SelectionStart = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => ((TextBoxBase)_TextBox).TextLength;

	public event CancelEventHandler ButtonClearClick;

	public event CancelEventHandler ButtonDropDownClick;

	public event EventHandler ButtonCustomClick;

	public event EventHandler ButtonCustom2Click;

	[Description("Occurs when the text alignment is changed.")]
	public event EventHandler TextAlignChanged;

	public event EventHandler ModifiedChanged;

	public TextBoxDropDown()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_TextBox = new TextBoxX();
		((Control)this).BackColor = SystemColors.Window;
		InitControl();
	}

	private void InitControl()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		_BackgroundStyle.SetColorScheme(base.ColorScheme);
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		_ButtonCustom = new InputButtonSettings(this);
		_ButtonCustom2 = new InputButtonSettings(this);
		_ButtonClear = new InputButtonSettings(this);
		_ButtonDropDown = new InputButtonSettings(this);
		CreateButtonGroup();
		_TextBox.BorderStyle = (BorderStyle)0;
		((Control)_TextBox).TextChanged += TextBoxTextChanged;
		((TextBox)_TextBox).TextAlignChanged += TextBoxTextAlignChanged;
		((Control)_TextBox).SizeChanged += TextBoxSizeChanged;
		((TextBoxBase)_TextBox).ModifiedChanged += TextBoxModifiedChanged;
		((Control)_TextBox).KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
		((Control)_TextBox).KeyDown += new KeyEventHandler(TextBoxKeyDown);
		((Control)this).Controls.Add((Control)(object)_TextBox);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		if (_DropDownControl != null)
		{
			_DropDownControl.BindingContext = ((Control)this).BindingContext;
		}
		base.OnBindingContextChanged(e);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 115)
		{
			if (_PopupItem.Expanded)
			{
				CloseDropDown();
			}
			else
			{
				InvokeShowDropDown();
			}
			return true;
		}
		if ((int)keyData == 27 && IsPopupOpen)
		{
			CloseDropDown();
			return true;
		}
		if (_ButtonGroup.ProcessCmdKey(ref msg, keyData))
		{
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
	{
		if (_InternalReadOnly)
		{
			e.Handled = true;
		}
	}

	private void TextBoxKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 40 && (int)e.Modifiers == 262144 && ButtonDropDown.Visible)
		{
			InvokeShowDropDown();
			e.Handled = true;
		}
	}

	private void TextBoxModifiedChanged(object sender, EventArgs e)
	{
		OnModifiedChanged(e);
	}

	protected virtual void OnModifiedChanged(EventArgs e)
	{
		this.ModifiedChanged?.Invoke(this, e);
	}

	private void TextBoxSizeChanged(object sender, EventArgs e)
	{
		if (!_InternalSizeUpdate)
		{
			UpdateLayout();
		}
	}

	private void TextBoxTextChanged(object sender, EventArgs e)
	{
		((Control)this).Text = ((Control)_TextBox).Text;
		ExecuteCommand();
	}

	private void TextBoxTextAlignChanged(object sender, EventArgs e)
	{
		OnTextAlignChanged(e);
	}

	protected virtual void OnTextAlignChanged(EventArgs e)
	{
		this.TextAlignChanged?.Invoke(this, e);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (!((Control)this).Enabled)
		{
			((Control)_TextBox).ResetBackColor();
		}
		else if (((Control)_TextBox).BackColor != ((Control)this).BackColor)
		{
			((Control)_TextBox).BackColor = ((Control)this).BackColor;
		}
		base.OnEnabledChanged(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		if (_FocusHighlightEnabled)
		{
			_OldBackColor = ((Control)this).BackColor;
			((Control)this).BackColor = _FocusHighlightColor;
		}
		if (_AutoSelectAll)
		{
			((TextBoxBase)_TextBox).SelectAll();
		}
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (_FocusHighlightEnabled && !_OldBackColor.IsEmpty)
		{
			((Control)this).BackColor = _OldBackColor;
			if (_OldBackColor == SystemColors.Window)
			{
				((Control)_TextBox).ResetBackColor();
			}
			_OldBackColor = Color.Empty;
		}
		((Control)this).OnLeave(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_FocusHighlightColor.Equals((object?)ColorScheme.GetColor(16777096));
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = ColorScheme.GetColor(16777096);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		_BackgroundStyle = new ElementStyle();
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		((Control)this).Invalidate();
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		_ButtonGroup.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (_BackgroundStyle != null)
		{
			_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		}
		base.Dispose(disposing);
	}

	void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		OnInputButtonSettingsChanged(inputButtonSettings);
	}

	protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		RecreateButtons();
		_ButtonGroup.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected virtual void RecreateButtons()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		VisualItem[] items = CreateOrderedButtonList();
		VisualGroup buttonGroup = _ButtonGroup;
		VisualItem[] array = new VisualItem[buttonGroup.Items.Count];
		buttonGroup.Items.CopyTo(array);
		VisualItem[] array2 = array;
		foreach (VisualItem visualItem in array2)
		{
			if (visualItem.ItemType == eSystemItemType.SystemButton)
			{
				buttonGroup.Items.Remove(visualItem);
				if (visualItem == _ButtonCustom.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(CustomButtonMouseUp);
				}
				else if (visualItem == _ButtonCustom2.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(CustomButton2MouseUp);
				}
			}
		}
		buttonGroup.Items.AddRange(items);
	}

	private void CustomButtonMouseUp(object sender, MouseEventArgs e)
	{
		if (_ButtonCustom.ItemReference.RenderBounds.Contains(e.X, e.Y))
		{
			OnButtonCustomClick((EventArgs)(object)e);
		}
	}

	protected virtual void OnButtonCustomClick(EventArgs e)
	{
		if (this.ButtonCustomClick != null)
		{
			this.ButtonCustomClick(this, e);
		}
	}

	private void CustomButton2MouseUp(object sender, MouseEventArgs e)
	{
		if (_ButtonCustom2.ItemReference.RenderBounds.Contains(e.X, e.Y))
		{
			OnButtonCustom2Click((EventArgs)(object)e);
		}
	}

	protected virtual void OnButtonCustom2Click(EventArgs e)
	{
		if (this.ButtonCustom2Click != null)
		{
			this.ButtonCustom2Click(this, e);
		}
	}

	private VisualItem[] CreateOrderedButtonList()
	{
		SortedList sortedList = CreateSortedButtonList();
		VisualItem[] array = new VisualItem[sortedList.Count];
		sortedList.Values.CopyTo(array, 0);
		return array;
	}

	private void CustomButtonClick(object sender, EventArgs e)
	{
		OnButtonCustomClick(e);
	}

	private void CustomButton2Click(object sender, EventArgs e)
	{
		OnButtonCustom2Click(e);
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		ClearButtonClick();
	}

	protected virtual SortedList CreateSortedButtonList()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		SortedList sortedList = new SortedList(4);
		if (_ButtonCustom.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonCustom);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButtonMouseUp);
				_ButtonCustom.ItemReference.Click -= CustomButtonClick;
			}
			_ButtonCustom.ItemReference = visualItem;
			visualItem.Click += CustomButtonClick;
			visualItem.MouseUp += new MouseEventHandler(CustomButtonMouseUp);
			visualItem.Enabled = _ButtonCustom.Enabled;
			sortedList.Add(_ButtonCustom, visualItem);
		}
		if (_ButtonCustom2.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonCustom2);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButton2MouseUp);
				_ButtonCustom.ItemReference.Click -= CustomButton2Click;
			}
			_ButtonCustom2.ItemReference = visualItem2;
			visualItem2.Click += CustomButton2Click;
			visualItem2.MouseUp += new MouseEventHandler(CustomButton2MouseUp);
			visualItem2.Enabled = _ButtonCustom2.Enabled;
			sortedList.Add(_ButtonCustom2, visualItem2);
		}
		if (_ButtonClear.Visible)
		{
			VisualItem visualItem3 = CreateButton(_ButtonClear);
			if (_ButtonClear.ItemReference != null)
			{
				_ButtonClear.ItemReference.MouseUp -= new MouseEventHandler(ClearButtonMouseUp);
				_ButtonClear.ItemReference.Click -= ClearButtonClick;
			}
			_ButtonClear.ItemReference = visualItem3;
			visualItem3.MouseUp += new MouseEventHandler(ClearButtonMouseUp);
			visualItem3.Click += ClearButtonClick;
			sortedList.Add(_ButtonClear, visualItem3);
		}
		if (_ButtonDropDown.Visible)
		{
			VisualItem visualItem4 = CreateButton(_ButtonDropDown);
			if (_ButtonDropDown.ItemReference != null)
			{
				_ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
				_ButtonDropDown.ItemReference.Click -= DropDownButtonClick;
			}
			_ButtonDropDown.ItemReference = visualItem4;
			visualItem4.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
			visualItem4.Click += DropDownButtonClick;
			sortedList.Add(_ButtonDropDown, visualItem4);
		}
		return sortedList;
	}

	private void DropDownButtonClick(object sender, EventArgs e)
	{
		if (e is KeyEventArgs)
		{
			if (_PopupItem.Expanded)
			{
				CloseDropDown();
			}
			else
			{
				InvokeShowDropDown();
			}
		}
	}

	protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == _ButtonDropDown)
		{
			visualItem = new VisualDropDownButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		else
		{
			visualItem = new VisualCustomButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		VisualButton visualButton = visualItem as VisualButton;
		visualButton.ClickAutoRepeat = false;
		if (buttonSettings == _ButtonClear && buttonSettings.Image == null)
		{
			visualButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.DateReset.png");
		}
		return visualItem;
	}

	protected virtual void ApplyButtonSettings(InputButtonSettings buttonSettings, VisualButton button)
	{
		button.Text = buttonSettings.Text;
		button.Image = buttonSettings.Image;
		button.ItemType = eSystemItemType.SystemButton;
		button.Enabled = buttonSettings.Enabled;
		button.Shortcut = buttonSettings.Shortcut;
	}

	private void CreateButtonGroup()
	{
		VisualGroup visualGroup = new VisualGroup();
		visualGroup.HorizontalItemSpacing = 0;
		visualGroup.ArrangeInvalid += ButtonGroupArrangeInvalid;
		visualGroup.RenderInvalid += ButtonGroupRenderInvalid;
		_ButtonGroup = visualGroup;
	}

	private void ButtonGroupRenderInvalid(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void ButtonGroupArrangeInvalid(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private PaintInfo CreatePaintInfo(Graphics g)
	{
		PaintInfo paintInfo = new PaintInfo();
		paintInfo.Graphics = g;
		paintInfo.DefaultFont = ((Control)this).Font;
		paintInfo.ForeColor = ((Control)this).ForeColor;
		paintInfo.RenderOffset = default(Point);
		Size size = ((Control)this).Size;
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		int num = ElementStyleLayout.TopWhiteSpace(backgroundStyle, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(backgroundStyle, eSpacePart.Border);
		if (num > 0)
		{
			num += 2;
		}
		size.Height -= num;
		num = ElementStyleLayout.LeftWhiteSpace(backgroundStyle, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(backgroundStyle, eSpacePart.Border);
		if (num > 0)
		{
			num += 2;
		}
		size.Width -= num;
		paintInfo.AvailableSize = size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _MouseOver || ((Control)this).Focused;
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
		return paintInfo;
	}

	private ElementStyle GetBackgroundStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		_BackgroundStyle.SetColorScheme(GetColorScheme());
		return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		bool enabled = ((Control)this).Enabled;
		if (!((Control)this).Enabled)
		{
			e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
		}
		else
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		if (backgroundStyle.Custom)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (base.AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, graphics, clientRectangle);
			if (!enabled)
			{
				ElementStyleDisplay.PaintBorder(e2);
			}
			else
			{
				ElementStyleDisplay.Paint(e2);
			}
			if (base.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
		}
		PaintButtons(graphics);
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Window;
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		if (((Control)_TextBox).Parent != null)
		{
			((Control)_TextBox).BackColor = ((Control)this).BackColor;
		}
	}

	private void PaintButtons(Graphics g)
	{
		PaintInfo p = CreatePaintInfo(g);
		if (!_ButtonGroup.IsLayoutValid)
		{
			UpdateLayout(p);
		}
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		_ButtonGroup.RenderBounds = GetButtonsRenderBounds(backgroundStyle);
		_ButtonGroup.ProcessPaint(p);
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
	}

	private Rectangle GetButtonsRenderBounds(ElementStyle style)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		int num = ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border);
		if (num > 0)
		{
			num++;
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Rectangle(ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1, num, _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
		}
		return new Rectangle(((Control)this).Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, num, _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
	}

	protected override void OnResize(EventArgs e)
	{
		_ButtonGroup.InvalidateArrange();
		UpdateLayout();
		((Control)this).Invalidate();
		((Control)this).OnResize(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)_TextBox).Focus();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		_ButtonGroup.InvalidateArrange();
		UpdateLayout();
		((Control)this).Invalidate();
		((Control)this).OnRightToLeftChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_ButtonGroup.InvalidateArrange();
		UpdateLayout();
		((Control)this).Invalidate();
		base.OnFontChanged(e);
	}

	private void UpdateLayout()
	{
		Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
		try
		{
			UpdateLayout(CreatePaintInfo(val));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void UpdateLayout(PaintInfo p)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		if (_InternalSizeUpdate)
		{
			return;
		}
		_InternalSizeUpdate = true;
		try
		{
			if (!_ButtonGroup.IsLayoutValid)
			{
				_ButtonGroup.PerformLayout(p);
			}
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			Rectangle innerRect = ElementStyleLayout.GetInnerRect(backgroundStyle, ((Control)this).ClientRectangle);
			if (RenderButtons)
			{
				Rectangle buttonsRenderBounds = GetButtonsRenderBounds(backgroundStyle);
				if ((int)((Control)this).RightToLeft == 1)
				{
					innerRect.X += buttonsRenderBounds.Right;
					innerRect.Width -= buttonsRenderBounds.Right;
				}
				else
				{
					innerRect.Width -= innerRect.Right - buttonsRenderBounds.X;
				}
			}
			if (!((TextBoxBase)_TextBox).Multiline && ((TextBoxBase)_TextBox).PreferredHeight < innerRect.Height)
			{
				innerRect.Y += (innerRect.Height - ((TextBoxBase)_TextBox).PreferredHeight) / 2;
			}
			((Control)_TextBox).Bounds = innerRect;
			if (disposeStyle)
			{
				backgroundStyle.Dispose();
			}
		}
		finally
		{
			_InternalSizeUpdate = false;
		}
	}

	private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Button != 1048576 || (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 250.0))
		{
			_CloseTime = DateTime.MinValue;
			return;
		}
		InvokeShowDropDown();
		if ((Control.MouseButtons & 0x100000) == 0)
		{
			_ButtonGroup.ProcessMouseUp(e);
		}
	}

	private void InvokeShowDropDown()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			ShowDropDown();
		}
	}

	public void ShowDropDown()
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		if ((_DropDownControl == null && _PopupItem.SubItems.Count == 0) || _ShowingPopup || _PopupItem.Expanded)
		{
			return;
		}
		_ShowingPopup = true;
		try
		{
			ControlContainerItem controlContainerItem = null;
			ItemContainer itemContainer = null;
			if (_DropDownControl != null)
			{
				itemContainer = new ItemContainer();
				itemContainer.Name = _DropDownItemContainerName;
				controlContainerItem = new ControlContainerItem(_DropDownControlContainerName);
				itemContainer.SubItems.Add(controlContainerItem);
				_PopupItem.SubItems.Insert(0, itemContainer);
			}
			if (_PopupItem.SubItems.Count == 0)
			{
				if (itemContainer != null)
				{
					_PopupItem.SubItems.Remove(itemContainer);
				}
				return;
			}
			if (_DropDownControl != null)
			{
				_PreviousDropDownControlParent = _DropDownControl.Parent;
				controlContainerItem.Control = _DropDownControl;
				if (!_DropDownControl.IsHandleCreated)
				{
					IntPtr handle = _DropDownControl.Handle;
				}
			}
			_PopupItem.SetDisplayRectangle(((Control)this).ClientRectangle);
			if ((int)((Control)this).RightToLeft == 0)
			{
				Point point = new Point(((Control)this).Width - _PopupItem.PopupSize.Width, ((Control)this).Height);
				ScreenInformation screenInformation = BarFunctions.ScreenFromControl((Control)(object)this);
				Point point2 = ((Control)this).PointToScreen(point);
				if (screenInformation != null && screenInformation.WorkingArea.X > point2.X)
				{
					point.X = 0;
				}
				_PopupItem.PopupLocation = point;
			}
			_PopupItem.Expanded = !_PopupItem.Expanded;
		}
		finally
		{
			_ShowingPopup = false;
		}
	}

	public void CloseDropDown()
	{
		if (_PopupItem.Expanded)
		{
			_PopupItem.Expanded = false;
		}
	}

	private void DropDownPopupClose(object sender, EventArgs e)
	{
		_CloseTime = DateTime.Now;
		ItemContainer itemContainer = null;
		if (_PopupItem.SubItems.Contains(_DropDownItemContainerName))
		{
			itemContainer = _PopupItem.SubItems[_DropDownItemContainerName] as ItemContainer;
		}
		if (itemContainer == null)
		{
			return;
		}
		if (itemContainer.SubItems[_DropDownControlContainerName] is ControlContainerItem controlContainerItem)
		{
			controlContainerItem.Control = null;
			itemContainer.SubItems.Remove(controlContainerItem);
			if (_DropDownControl != null)
			{
				_DropDownControl.Parent = _PreviousDropDownControlParent;
				_PreviousDropDownControlParent = null;
			}
		}
		_PopupItem.SubItems.Remove(itemContainer);
		itemContainer.Dispose();
	}

	private void ClearButtonClick()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			((Control)_TextBox).Text = "";
		}
	}

	private void ClearButtonMouseUp(object sender, EventArgs e)
	{
		ClearButtonClick();
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

	protected virtual void OnDropDownControlChanged()
	{
		if (_DropDownControl != null)
		{
			_DropDownControl.BindingContext = ((Control)this).BindingContext;
			if (!((Component)(object)this).DesignMode)
			{
				_DropDownControl.Visible = false;
			}
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += DropDownPopupClose;
		_PopupItem = buttonItem;
		return buttonItem;
	}

	protected override void RecalcSize()
	{
	}

	public override void PerformClick()
	{
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseMove(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseLeave();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	internal void ProcessMouseUpOnGroup()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, -10, -10, 0));
		}
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _TextBox.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
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

	public override string ToString()
	{
		return ((object)_TextBox).ToString();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size preferredSize = ((Control)_TextBox).GetPreferredSize(proposedSize);
		preferredSize.Height = PreferredHeight;
		return preferredSize;
	}

	public void AppendText(string text)
	{
		((TextBoxBase)_TextBox).AppendText(text);
	}

	public void Clear()
	{
		((TextBoxBase)_TextBox).Clear();
	}

	public void ClearUndo()
	{
		((TextBoxBase)_TextBox).ClearUndo();
	}

	public void Copy()
	{
		((TextBoxBase)_TextBox).Copy();
	}

	public void Cut()
	{
		((TextBoxBase)_TextBox).Cut();
	}

	public void DeselectAll()
	{
		((TextBoxBase)_TextBox).DeselectAll();
	}

	public virtual char GetCharFromPosition(Point pt)
	{
		return ((TextBoxBase)_TextBox).GetCharFromPosition(pt);
	}

	public virtual int GetCharIndexFromPosition(Point pt)
	{
		return ((TextBoxBase)_TextBox).GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return ((TextBoxBase)_TextBox).GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return ((TextBoxBase)_TextBox).GetFirstCharIndexOfCurrentLine();
	}

	public virtual int GetLineFromCharIndex(int index)
	{
		return ((TextBoxBase)_TextBox).GetLineFromCharIndex(index);
	}

	public virtual Point GetPositionFromCharIndex(int index)
	{
		return ((TextBoxBase)_TextBox).GetPositionFromCharIndex(index);
	}

	public void Paste()
	{
		((TextBoxBase)_TextBox).Paste();
	}

	public void Select(int start, int length)
	{
		((TextBoxBase)_TextBox).Select(start, length);
	}

	public void SelectAll()
	{
		((TextBoxBase)_TextBox).SelectAll();
	}

	public void Undo()
	{
		((TextBoxBase)_TextBox).Undo();
	}

	public void Paste(string text)
	{
		((TextBox)_TextBox).Paste(text);
	}
}
