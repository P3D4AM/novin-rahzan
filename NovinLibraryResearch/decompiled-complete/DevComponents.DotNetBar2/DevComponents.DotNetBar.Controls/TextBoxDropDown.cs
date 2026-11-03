using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Interop;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(TextBoxDropDown), "Controls.TextBoxDropDown.ico")]
[DefaultProperty("Text")]
[DefaultBindingProperty("Text")]
[Designer("DevComponents.DotNetBar.Design.TextBoxDropDownDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("TextChanged")]
public class TextBoxDropDown : PopupItemControl, ICommandSource, IInputButtonControl
{
	private TextBoxX __28D2;

	private ButtonItem _០;

	private static string __22BB = "sysPopupItemContainer";

	private static string __22BA = "sysPopupControlContainer";

	private Color _ᚾ = ColorScheme.GetColor(16777096);

	private bool _ᚼ;

	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private EventHandler _181E;

	private EventHandler _181F;

	private EventHandler _28AC;

	private EventHandler ᮊ;

	private CancelEventHandler _28D3;

	private bool _ᶂ;

	private Color __28D4 = Color.Empty;

	private ElementStyle _ᠩ = new ElementStyle();

	private InputButtonSettings __17F1;

	private InputButtonSettings __17F2;

	private InputButtonSettings _ᠧ;

	private InputButtonSettings _ᠨ;

	private VisualGroup _ᶇ;

	private bool _ኗ;

	private bool __28B0;

	private Control __22C0;

	private bool __28B1;

	private DateTime __17F4 = DateTime.MinValue;

	private bool __28D5;

	private Control __22C3;

	private bool __28D6;

	private ICommand _૦;

	private object _૧;

	[Category("Behavior")]
	[Description("Indicates whether all text is auto-selected when control gets input focus.")]
	[DefaultValue(false)]
	public bool AutoSelectAll
	{
		get
		{
			return _ᶂ;
		}
		set
		{
			_ᶂ = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public bool FocusHighlightEnabled
	{
		get
		{
			return _ᚼ;
		}
		set
		{
			if (_ᚼ != value)
			{
				_ᚼ = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color FocusHighlightColor
	{
		get
		{
			return _ᚾ;
		}
		set
		{
			if (_ᚾ != value)
			{
				_ᚾ = value;
				__28D2.FocusHighlightColor = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Description("Gets or sets control background style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	public InputButtonSettings ButtonDropDown => __17F1;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __17F2;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	public InputButtonSettings ButtonCustom => _ᠧ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom2 => _ᠨ;

	internal VisualGroup _28C0 => _ᶇ;

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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsPopupOpen
	{
		get
		{
			return _០.Expanded;
		}
		set
		{
			if (value != _០.Expanded)
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

	[DefaultValue(false)]
	[Description("Indicates whether DropDownControl is automatically focused when popup is open.")]
	[Category("Behavior")]
	public bool AutoFocusDropDownControl
	{
		get
		{
			return __28D5;
		}
		set
		{
			__28D5 = value;
		}
	}

	[Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
	[DefaultValue(null)]
	public Control DropDownControl
	{
		get
		{
			return __22C3;
		}
		set
		{
			if (value != this)
			{
				__22C3 = value;
				OnDropDownControlChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SubItemsCollection DropDownItems => _០.SubItems;

	private bool ᵸ
	{
		get
		{
			if ((_ᠧ == null || !_ᠧ.Visible) && (_ᠨ == null || !_ᠨ.Visible) && (__17F1 == null || !__17F1.Visible))
			{
				if (__17F2 != null)
				{
					return __17F2.Visible;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TextBoxX TextBox => __28D2;

	internal bool _28D1
	{
		get
		{
			return __28D6;
		}
		set
		{
			__28D6 = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return __28D2.WatermarkEnabled;
		}
		set
		{
			__28D2.WatermarkEnabled = value;
			((Control)this).Invalidate(true);
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Browsable(true)]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return __28D2.WatermarkText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			__28D2.WatermarkText = value;
			((Control)this).Invalidate(true);
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark font.")]
	public Font WatermarkFont
	{
		get
		{
			return __28D2.WatermarkFont;
		}
		set
		{
			__28D2.WatermarkFont = value;
			((Control)this).Invalidate(true);
		}
	}

	[Description("Indicates watermark text color.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color WatermarkColor
	{
		get
		{
			return __28D2.WatermarkColor;
		}
		set
		{
			__28D2.WatermarkColor = value;
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
			return __28D2.WatermarkBehavior;
		}
		set
		{
			__28D2.WatermarkBehavior = value;
			((Control)this).Invalidate(true);
		}
	}

	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image WatermarkImage
	{
		get
		{
			return __28D2.WatermarkImage;
		}
		set
		{
			__28D2.WatermarkImage = value;
		}
	}

	[Description("Indicates watermark image alignment.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public ContentAlignment WatermarkImageAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __28D2.WatermarkImageAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__28D2.WatermarkImageAlignment = value;
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Commands")]
	[DefaultValue(null)]
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

	[DefaultValue("Indicates text as it is currently displayed to the user")]
	[Bindable(true)]
	[Category("Appearance")]
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
			((Control)__28D2).Text = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int PreferredHeight
	{
		get
		{
			int preferredHeight = ((TextBoxBase)__28D2).PreferredHeight;
			bool ᠲ = false;
			ElementStyle elementStyle = _1400(out ᠲ);
			preferredHeight += ᝈ.ᝏ(elementStyle);
			if (ᠲ)
			{
				elementStyle.Dispose();
			}
			return preferredHeight;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Localizable(true)]
	[Browsable(true)]
	public AutoCompleteStringCollection AutoCompleteCustomSource
	{
		get
		{
			return ((TextBox)__28D2).AutoCompleteCustomSource;
		}
		set
		{
			((TextBox)__28D2).AutoCompleteCustomSource = value;
		}
	}

	[DefaultValue(0)]
	[Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)__28D2).AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)__28D2).AutoCompleteMode = value;
		}
	}

	[Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DefaultValue(128)]
	[TypeConverter(typeof(Ӓ))]
	[Browsable(true)]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)__28D2).AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)__28D2).AutoCompleteSource = value;
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates whether the TextBox control modifies the case of characters as they are typed.")]
	public CharacterCasing CharacterCasing
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBox)__28D2).CharacterCasing;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)__28D2).CharacterCasing = value;
		}
	}

	[Localizable(true)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	[Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
	[DefaultValue('\0')]
	public char PasswordChar
	{
		get
		{
			return ((TextBox)__28D2).PasswordChar;
		}
		set
		{
			((TextBox)__28D2).PasswordChar = value;
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
			return ((TextBox)__28D2).TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((TextBox)__28D2).TextAlign = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.")]
	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return ((TextBox)__28D2).UseSystemPasswordChar;
		}
		set
		{
			((TextBox)__28D2).UseSystemPasswordChar = value;
		}
	}

	[Category("Behavior")]
	[Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
	[DefaultValue(true)]
	public bool HideSelection
	{
		get
		{
			return ((TextBoxBase)__28D2).HideSelection;
		}
		set
		{
			((TextBoxBase)__28D2).HideSelection = value;
		}
	}

	[Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
	[Category("Behavior")]
	[Localizable(true)]
	[DefaultValue(32767)]
	public virtual int MaxLength
	{
		get
		{
			return ((TextBoxBase)__28D2).MaxLength;
		}
		set
		{
			((TextBoxBase)__28D2).MaxLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Modified
	{
		get
		{
			return ((TextBoxBase)__28D2).Modified;
		}
		set
		{
			((TextBoxBase)__28D2).Modified = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Gets or sets a value indicating whether text in the text box is read-only.")]
	public bool ReadOnly
	{
		get
		{
			return ((TextBoxBase)__28D2).ReadOnly;
		}
		set
		{
			((TextBoxBase)__28D2).ReadOnly = value;
		}
	}

	[Description("Gets or sets a value indicating the currently selected text in the control.")]
	[Category("Appearance")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string SelectedText
	{
		get
		{
			return ((TextBoxBase)__28D2).SelectedText;
		}
		set
		{
			((TextBoxBase)__28D2).SelectedText = value;
		}
	}

	[Description("Gets or sets the number of characters selected in the text box.")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int SelectionLength
	{
		get
		{
			return ((TextBoxBase)__28D2).SelectionLength;
		}
		set
		{
			((TextBoxBase)__28D2).SelectionLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets the starting point of text selected in the text box.")]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)__28D2).SelectionStart;
		}
		set
		{
			((TextBoxBase)__28D2).SelectionStart = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => ((TextBoxBase)__28D2).TextLength;

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

	public event EventHandler ButtonCustomClick
	{
		add
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ButtonCustom2Click
	{
		add
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the text alignment is changed.")]
	public event EventHandler TextAlignChanged
	{
		add
		{
			EventHandler eventHandler = _28AC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28AC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _28AC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28AC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ModifiedChanged
	{
		add
		{
			EventHandler eventHandler = ᮊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before the popup is opened and it allows canceling of popup by setting CancelEventArgs.Cancel=true.")]
	public event CancelEventHandler BeforePopupOpen
	{
		add
		{
			CancelEventHandler cancelEventHandler = _28D3;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _28D3, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _28D3;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _28D3, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	protected virtual void OnBeforePopupOpen(CancelEventArgs e)
	{
		_28D3?.Invoke(this, e);
	}

	public TextBoxDropDown()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		__28D2 = new TextBoxX();
		((Control)this).BackColor = SystemColors.Window;
		კ();
	}

	private void კ()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		_ᠩ.SetColorScheme(base.ColorScheme);
		_ᠩ.StyleChanged += ઐ;
		_ᠧ = new InputButtonSettings(this);
		_ᠨ = new InputButtonSettings(this);
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		ᶑ();
		__28D2.BorderStyle = (BorderStyle)0;
		((Control)__28D2).TextChanged += ᵶ;
		((TextBox)__28D2).TextAlignChanged += _28D9;
		((Control)__28D2).SizeChanged += _28D8;
		((TextBoxBase)__28D2).ModifiedChanged += _28D7;
		((Control)__28D2).KeyPress += new KeyPressEventHandler(ᵲ);
		((Control)__28D2).KeyDown += new KeyEventHandler(ᵱ);
		((Control)this).Controls.Add((Control)(object)__28D2);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		if (__22C3 != null)
		{
			__22C3.BindingContext = ((Control)this).BindingContext;
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
			if (_០.Expanded)
			{
				CloseDropDown();
			}
			else
			{
				_28DC();
			}
			return true;
		}
		if ((int)keyData == 27 && IsPopupOpen)
		{
			CloseDropDown();
			return true;
		}
		if (_ᶇ.ᘭ(ref msg, keyData))
		{
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private void ᵲ(object _٠, KeyPressEventArgs ؿ)
	{
		if (__28D6)
		{
			ؿ.Handled = true;
		}
	}

	private void ᵱ(object _٠, KeyEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if ((int)ؿ.KeyCode == 40 && (int)ؿ.Modifiers == 262144 && ButtonDropDown.Visible)
		{
			_28DC();
			ؿ.Handled = true;
		}
	}

	private void _28D7(object _٠, EventArgs ؿ)
	{
		OnModifiedChanged(ؿ);
	}

	protected virtual void OnModifiedChanged(EventArgs e)
	{
		ᮊ?.Invoke(this, e);
	}

	private void _28D8(object _٠, EventArgs ؿ)
	{
		if (!__28B0)
		{
			ᘥ();
		}
	}

	private void ᵶ(object _٠, EventArgs ؿ)
	{
		((Control)this).Text = ((Control)__28D2).Text;
		ExecuteCommand();
	}

	private void _28D9(object _٠, EventArgs ؿ)
	{
		OnTextAlignChanged(ؿ);
	}

	protected virtual void OnTextAlignChanged(EventArgs e)
	{
		_28AC?.Invoke(this, e);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (!((Control)this).Enabled)
		{
			((Control)__28D2).ResetBackColor();
		}
		else if (((Control)__28D2).BackColor != ((Control)this).BackColor)
		{
			((Control)__28D2).BackColor = ((Control)this).BackColor;
		}
		base.OnEnabledChanged(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		if (_ᚼ)
		{
			__28D4 = ((Control)this).BackColor;
			((Control)this).BackColor = _ᚾ;
		}
		if (_ᶂ)
		{
			((TextBoxBase)__28D2).SelectAll();
		}
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (_ᚼ && !__28D4.IsEmpty)
		{
			((Control)this).BackColor = __28D4;
			if (__28D4 == SystemColors.Window)
			{
				((Control)__28D2).ResetBackColor();
			}
			__28D4 = Color.Empty;
		}
		((Control)this).OnLeave(e);
	}

	protected override bool QueryPopupCloseMouseDown()
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			return false;
		}
		return base.QueryPopupCloseMouseDown();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_ᚾ.Equals((object?)ColorScheme.GetColor(16777096));
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
		_ᠩ.StyleChanged -= ઐ;
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		_ᶇ.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (_ᠩ != null)
		{
			_ᠩ.StyleChanged -= ઐ;
		}
		base.Dispose(disposing);
	}

	private void ᠵ(InputButtonSettings ᠶ)
	{
		OnInputButtonSettingsChanged(ᠶ);
	}

	void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings ᠶ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᠵ
		this.ᠵ(ᠶ);
	}

	protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		ᶎ();
	}

	private void ᶎ()
	{
		RecreateButtons();
		_ᶇ.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected virtual void RecreateButtons()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		VisualItem[] items = ᠹ();
		VisualGroup ᶇ = _ᶇ;
		VisualItem[] array = new VisualItem[ᶇ.Items.Count];
		ᶇ.Items._0CD0(array);
		VisualItem[] array2 = array;
		foreach (VisualItem visualItem in array2)
		{
			if (visualItem.ᡇ == _1885._1886)
			{
				ᶇ.Items.Remove(visualItem);
				if (visualItem == _ᠧ.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(_28DA);
				}
				else if (visualItem == _ᠨ.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(_28DB);
				}
			}
		}
		ᶇ.Items.AddRange(items);
	}

	private void _28DA(object _٠, MouseEventArgs ؿ)
	{
		if (_ᠧ.ItemReference.RenderBounds.Contains(ؿ.X, ؿ.Y))
		{
			OnButtonCustomClick((EventArgs)(object)ؿ);
		}
	}

	protected virtual void OnButtonCustomClick(EventArgs e)
	{
		if (_181E != null)
		{
			_181E(this, e);
		}
	}

	private void _28DB(object _٠, MouseEventArgs ؿ)
	{
		if (_ᠨ.ItemReference.RenderBounds.Contains(ؿ.X, ؿ.Y))
		{
			OnButtonCustom2Click((EventArgs)(object)ؿ);
		}
	}

	protected virtual void OnButtonCustom2Click(EventArgs e)
	{
		if (_181F != null)
		{
			_181F(this, e);
		}
	}

	private VisualItem[] ᠹ()
	{
		SortedList sortedList = CreateSortedButtonList();
		VisualItem[] array = new VisualItem[sortedList.Count];
		sortedList.Values.CopyTo(array, 0);
		return array;
	}

	private void ᠷ(object _٠, EventArgs ؿ)
	{
		OnButtonCustomClick(ؿ);
	}

	private void ᠸ(object _٠, EventArgs ؿ)
	{
		OnButtonCustom2Click(ؿ);
	}

	private void _180F(object _٠, EventArgs ؿ)
	{
		_180F();
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
		if (_ᠧ.Visible)
		{
			VisualItem visualItem = CreateButton(_ᠧ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(_28DA);
				_ᠧ.ItemReference.Click -= ᠷ;
			}
			_ᠧ.ItemReference = visualItem;
			visualItem.Click += ᠷ;
			visualItem.MouseUp += new MouseEventHandler(_28DA);
			visualItem.Enabled = _ᠧ.Enabled;
			sortedList.Add(_ᠧ, visualItem);
		}
		if (_ᠨ.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ᠨ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(_28DB);
				_ᠧ.ItemReference.Click -= ᠸ;
			}
			_ᠨ.ItemReference = visualItem2;
			visualItem2.Click += ᠸ;
			visualItem2.MouseUp += new MouseEventHandler(_28DB);
			visualItem2.Enabled = _ᠨ.Enabled;
			sortedList.Add(_ᠨ, visualItem2);
		}
		if (__17F2.Visible)
		{
			VisualItem visualItem3 = CreateButton(__17F2);
			if (__17F2.ItemReference != null)
			{
				__17F2.ItemReference.MouseUp -= new MouseEventHandler(_28DD);
				__17F2.ItemReference.Click -= _180F;
			}
			__17F2.ItemReference = visualItem3;
			visualItem3.MouseUp += new MouseEventHandler(_28DD);
			visualItem3.Click += _180F;
			sortedList.Add(__17F2, visualItem3);
		}
		if (__17F1.Visible)
		{
			VisualItem visualItem4 = CreateButton(__17F1);
			if (__17F1.ItemReference != null)
			{
				__17F1.ItemReference.MouseDown -= new MouseEventHandler(_180C);
				__17F1.ItemReference.Click -= _180B;
			}
			__17F1.ItemReference = visualItem4;
			visualItem4.MouseDown += new MouseEventHandler(_180C);
			visualItem4.Click += _180B;
			sortedList.Add(__17F1, visualItem4);
		}
		return sortedList;
	}

	private void _180B(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			if (_០.Expanded)
			{
				CloseDropDown();
			}
			else
			{
				_28DC();
			}
		}
	}

	protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == __17F1)
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
		if (buttonSettings == __17F2 && buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
		{
			visualButton.Symbol = "\uf00d";
		}
		return visualItem;
	}

	protected virtual void ApplyButtonSettings(InputButtonSettings buttonSettings, VisualButton button)
	{
		button.Text = buttonSettings.Text;
		button.Image = buttonSettings.Image;
		button.ᡇ = _1885._1886;
		button.Enabled = buttonSettings.Enabled;
		button.Shortcut = buttonSettings.Shortcut;
		button.Tooltip = buttonSettings.Tooltip;
		button.Symbol = buttonSettings.Symbol;
		button.SymbolSet = buttonSettings.SymbolSet;
		button.SymbolColor = buttonSettings.SymbolColor;
	}

	private void ᶑ()
	{
		VisualGroup visualGroup = new VisualGroup();
		visualGroup.HorizontalItemSpacing = 0;
		visualGroup.ArrangeInvalid += ᶓ;
		visualGroup.RenderInvalid += ᶒ;
		visualGroup.ResetMouseHover += ᶔ;
		_ᶇ = visualGroup;
	}

	private void ᶒ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	private void ᶓ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	private void ᶔ(object _٠, EventArgs ؿ)
	{
		DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
	}

	private PaintInfo ᶕ(Graphics _٥)
	{
		PaintInfo paintInfo = new PaintInfo();
		paintInfo.Graphics = _٥;
		paintInfo.DefaultFont = ((Control)this).Font;
		paintInfo.ForeColor = ((Control)this).ForeColor;
		paintInfo.RenderOffset = default(Point);
		Size size = ((Control)this).Size;
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		int num = ᝈ._1753(elementStyle, eSpacePart.Border) + ᝈ._1754(elementStyle, eSpacePart.Border);
		if (num > 0)
		{
			num += 2;
		}
		size.Height -= num;
		num = ᝈ.ᝐ(elementStyle, eSpacePart.Border) + ᝈ._1752(elementStyle, eSpacePart.Border);
		if (num > 0)
		{
			num += 2;
		}
		size.Width -= num;
		paintInfo.AvailableSize = size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _ኗ || ((Control)this).Focused;
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
		return paintInfo;
	}

	private ElementStyle _1400(out bool ᠲ)
	{
		ᠲ = false;
		_ᠩ.SetColorScheme(GetColorScheme());
		return ElementStyleDisplay.റ(_ᠩ, out ᠲ);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
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
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		if (elementStyle.Custom)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (base.AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(elementStyle, graphics, clientRectangle);
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
		ᶖ(graphics);
		if (ᠲ)
		{
			elementStyle.Dispose();
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
		if (((Control)__28D2).Parent != null)
		{
			((Control)__28D2).BackColor = ((Control)this).BackColor;
		}
	}

	private void ᶖ(Graphics _٥)
	{
		PaintInfo paintInfo = ᶕ(_٥);
		if (!_ᶇ.IsLayoutValid)
		{
			ᘥ(paintInfo);
		}
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		_ᶇ.RenderBounds = _28BA(elementStyle);
		_ᶇ.ᡤ(paintInfo);
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
	}

	private Rectangle _28BA(ElementStyle _0657)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		int num = ᝈ._1753(_0657, eSpacePart.Border);
		if (num > 0)
		{
			num++;
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Rectangle(ᝈ.ᝐ(_0657, eSpacePart.Border) + 1, num, _ᶇ.Size.Width, _ᶇ.Size.Height);
		}
		return new Rectangle(((Control)this).Width - (ᝈ._1752(_0657, eSpacePart.Border) + 1) - _ᶇ.Size.Width, num, _ᶇ.Size.Width, _ᶇ.Size.Height);
	}

	protected override void OnResize(EventArgs e)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_ᶇ.InvalidateArrange();
			ᘥ();
			((Control)this).Invalidate();
		}
		((Control)this).OnResize(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)__28D2).Focus();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		ᘥ();
		((Control)this).Invalidate();
		((Control)this).OnRightToLeftChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		ᘥ();
		((Control)this).Invalidate();
		base.OnFontChanged(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)__28D2).ForeColor = ((Control)this).ForeColor;
		((Control)this).OnForeColorChanged(e);
	}

	private void ᘥ()
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
		try
		{
			ᘥ(ᶕ(val));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ᘥ(PaintInfo _065C)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		if (__28B0)
		{
			return;
		}
		__28B0 = true;
		try
		{
			if (!_ᶇ.IsLayoutValid)
			{
				_ᶇ.PerformLayout(_065C);
			}
			bool ᠲ = false;
			ElementStyle elementStyle = _1400(out ᠲ);
			Rectangle bounds = ᝈ._1756(elementStyle, ((Control)this).ClientRectangle);
			if (ᵸ)
			{
				Rectangle rectangle = _28BA(elementStyle);
				if ((int)((Control)this).RightToLeft == 1)
				{
					bounds.X += rectangle.Right;
					bounds.Width -= rectangle.Right;
				}
				else
				{
					bounds.Width -= bounds.Right - rectangle.X;
				}
			}
			if (!((TextBoxBase)__28D2).Multiline && ((TextBoxBase)__28D2).PreferredHeight < bounds.Height)
			{
				bounds.Y += (bounds.Height - ((TextBoxBase)__28D2).PreferredHeight) / 2;
			}
			((Control)__28D2).Bounds = bounds;
			if (ᠲ)
			{
				elementStyle.Dispose();
			}
		}
		finally
		{
			__28B0 = false;
		}
	}

	private void _180C(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if ((int)ؿ.Button != 1048576 || (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 250.0))
		{
			__17F4 = DateTime.MinValue;
			return;
		}
		_28DC();
		if ((Control.MouseButtons & 0x100000) == 0)
		{
			_ᶇ.ᡛ(ؿ);
		}
	}

	private void _28DC()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonDropDownClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			if (IsPopupOpen)
			{
				CloseDropDown();
			}
			else
			{
				ShowDropDown();
			}
		}
	}

	public void ShowDropDown()
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		if ((__22C3 == null && _០.SubItems.Count == 0) || __28B1 || _០.Expanded)
		{
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforePopupOpen(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		__28B1 = true;
		try
		{
			ControlContainerItem controlContainerItem = null;
			ItemContainer itemContainer = null;
			if (__22C3 != null)
			{
				itemContainer = new ItemContainer();
				itemContainer.Name = __22BB;
				controlContainerItem = new ControlContainerItem(__22BA);
				itemContainer.SubItems.Add(controlContainerItem);
				_០.SubItems.Insert(0, itemContainer);
			}
			if (_០.SubItems.Count == 0)
			{
				if (itemContainer != null)
				{
					_០.SubItems.Remove(itemContainer);
				}
				return;
			}
			if (__22C3 != null)
			{
				__22C0 = __22C3.Parent;
				controlContainerItem.Control = __22C3;
				if (!__22C3.IsHandleCreated)
				{
					_ = __22C3.Handle;
				}
			}
			_០.SetDisplayRectangle(((Control)this).ClientRectangle);
			if ((int)((Control)this).RightToLeft == 0)
			{
				Point point = new Point(((Control)this).Width - _០.PopupSize.Width, ((Control)this).Height);
				ঊ ঊ = BarFunctions.ॻ((Control)(object)this);
				Point point2 = ((Control)this).PointToScreen(point);
				if (ঊ != null && ঊ.ঌ.X > point2.X)
				{
					point.X = 0;
				}
				_០.PopupLocation = point;
			}
			_០.Expanded = !_០.Expanded;
		}
		finally
		{
			__28B1 = false;
		}
		if (__22C3 != null && __28D5 && __22C3.CanFocus)
		{
			BarUtilities.InvokeDelayed((MethodInvoker)delegate
			{
				__22C3.Focus();
			});
		}
	}

	public void CloseDropDown()
	{
		if (_០.Expanded)
		{
			_០.Expanded = false;
		}
	}

	private void _180E(object _٠, EventArgs ؿ)
	{
		__17F4 = DateTime.Now;
		ItemContainer itemContainer = null;
		if (_០.SubItems.Contains(__22BB))
		{
			itemContainer = _០.SubItems[__22BB] as ItemContainer;
		}
		if (itemContainer == null)
		{
			return;
		}
		if (itemContainer.SubItems[__22BA] is ControlContainerItem controlContainerItem)
		{
			controlContainerItem.Control = null;
			itemContainer.SubItems.Remove(controlContainerItem);
			if (__22C3 != null)
			{
				__22C3.Parent = __22C0;
				__22C0 = null;
			}
		}
		_០.SubItems.Remove(itemContainer);
		itemContainer.Dispose();
	}

	private void _180F()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			((Control)__28D2).Text = "";
		}
	}

	private void _28DD(object _٠, EventArgs ؿ)
	{
		_180F();
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

	protected virtual void OnDropDownControlChanged()
	{
		if (__22C3 != null)
		{
			__22C3.BindingContext = ((Control)this).BindingContext;
			if (!((Component)(object)this).DesignMode)
			{
				__22C3.Visible = false;
			}
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += _180E;
		_០ = buttonItem;
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
		if (ᵸ)
		{
			_ᶇ.ᡘ(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡗ();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡜ(e);
		}
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡚ(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡛ(e);
		}
		((Control)this).OnMouseUp(e);
	}

	internal void _28DE()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (ᵸ)
		{
			_ᶇ.ᡛ(new MouseEventArgs((MouseButtons)1048576, 0, -10, -10, 0));
		}
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return __28D2.WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
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

	public override string ToString()
	{
		return ((object)__28D2).ToString();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		ᘥ();
		base.OnHandleCreated(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size preferredSize = ((Control)__28D2).GetPreferredSize(proposedSize);
		preferredSize.Height = PreferredHeight;
		return preferredSize;
	}

	public void AppendText(string text)
	{
		((TextBoxBase)__28D2).AppendText(text);
	}

	public void Clear()
	{
		((TextBoxBase)__28D2).Clear();
	}

	public void ClearUndo()
	{
		((TextBoxBase)__28D2).ClearUndo();
	}

	public void Copy()
	{
		((TextBoxBase)__28D2).Copy();
	}

	public void Cut()
	{
		((TextBoxBase)__28D2).Cut();
	}

	public void DeselectAll()
	{
		((TextBoxBase)__28D2).DeselectAll();
	}

	public virtual char GetCharFromPosition(Point pt)
	{
		return ((TextBoxBase)__28D2).GetCharFromPosition(pt);
	}

	public virtual int GetCharIndexFromPosition(Point pt)
	{
		return ((TextBoxBase)__28D2).GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return ((TextBoxBase)__28D2).GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return ((TextBoxBase)__28D2).GetFirstCharIndexOfCurrentLine();
	}

	public virtual int GetLineFromCharIndex(int index)
	{
		return ((TextBoxBase)__28D2).GetLineFromCharIndex(index);
	}

	public virtual Point GetPositionFromCharIndex(int index)
	{
		return ((TextBoxBase)__28D2).GetPositionFromCharIndex(index);
	}

	public void Paste()
	{
		((TextBoxBase)__28D2).Paste();
	}

	public void Select(int start, int length)
	{
		((TextBoxBase)__28D2).Select(start, length);
	}

	public void SelectAll()
	{
		((TextBoxBase)__28D2).SelectAll();
	}

	public void Undo()
	{
		((TextBoxBase)__28D2).Undo();
	}

	public void Paste(string text)
	{
		((TextBox)__28D2).Paste(text);
	}

	[CompilerGenerated]
	private void _28DF()
	{
		__22C3.Focus();
	}
}
