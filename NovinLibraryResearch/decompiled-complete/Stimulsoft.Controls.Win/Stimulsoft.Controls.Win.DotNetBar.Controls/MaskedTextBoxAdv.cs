using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(MaskedTextBoxAdv), "Controls.MaskedTextBoxAdv.ico")]
[ToolboxItem(false)]
[DefaultProperty("Mask")]
[DefaultBindingProperty("Text")]
[DefaultEvent("MaskInputRejected")]
public class MaskedTextBoxAdv : PopupItemControl, IInputButtonControl, ICommandSource
{
	private class MaskedTextBoxInternal : MaskedTextBox
	{
		private bool _IgnoreFocus;

		private bool _Focused;

		private Color _LastBackColor = Color.Empty;

		private bool RenderWatermark
		{
			get
			{
				MaskedTextBoxAdv maskedTextBoxAdv = MaskedTextBoxAdv;
				if (maskedTextBoxAdv == null)
				{
					return false;
				}
				if (!maskedTextBoxAdv.WatermarkEnabled)
				{
					return false;
				}
				if (maskedTextBoxAdv.WatermarkBehavior == eWatermarkBehavior.HideOnFocus)
				{
					if (!_Focused && ((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
					{
						return maskedTextBoxAdv.WatermarkText.Length > 0;
					}
					return false;
				}
				if (((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
				{
					return maskedTextBoxAdv.WatermarkText.Length > 0;
				}
				return false;
			}
		}

		private MaskedTextBoxAdv MaskedTextBoxAdv => ((Control)this).Parent as MaskedTextBoxAdv;

		private void DrawWatermark()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Invalid comparison between Unknown and I4
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Invalid comparison between Unknown and I4
			MaskedTextBoxAdv maskedTextBoxAdv = MaskedTextBoxAdv;
			if (maskedTextBoxAdv == null)
			{
				return;
			}
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				if (maskedTextBoxAdv._TextMarkup != null)
				{
					MarkupDrawContext markupDrawContext = maskedTextBoxAdv.GetMarkupDrawContext(val);
					maskedTextBoxAdv._TextMarkup.Render(markupDrawContext);
					return;
				}
				eTextFormat eTextFormat = eTextFormat.Default;
				if ((int)((Control)this).RightToLeft == 1)
				{
					eTextFormat |= eTextFormat.RightToLeft;
				}
				if ((int)((MaskedTextBox)this).TextAlign == 0)
				{
					eTextFormat |= eTextFormat.Default;
				}
				else if ((int)((MaskedTextBox)this).TextAlign == 1)
				{
					eTextFormat |= eTextFormat.Right;
				}
				else if ((int)((MaskedTextBox)this).TextAlign == 2)
				{
					eTextFormat |= eTextFormat.HorizontalCenter;
				}
				eTextFormat |= eTextFormat.EndEllipsis;
				eTextFormat |= eTextFormat.WordBreak;
				TextDrawing.DrawString(val, maskedTextBoxAdv.WatermarkText, (maskedTextBoxAdv.WatermarkFont == null) ? ((Control)this).Font : maskedTextBoxAdv.WatermarkFont, maskedTextBoxAdv.WatermarkColor, maskedTextBoxAdv.GetWatermarkBounds(), eTextFormat);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			((MaskedTextBox)this).OnTextChanged(e);
			MaskedTextBoxAdv maskedTextBoxAdv = MaskedTextBoxAdv;
			if (maskedTextBoxAdv != null && maskedTextBoxAdv.WatermarkText.Length > 0)
			{
				((Control)this).Invalidate();
			}
		}

		protected override void WndProc(ref Message m)
		{
			((MaskedTextBox)this).WndProc(ref m);
			MaskedTextBoxAdv maskedTextBoxAdv = MaskedTextBoxAdv;
			if (maskedTextBoxAdv == null)
			{
				return;
			}
			switch (((Message)(ref m)).Msg)
			{
			case 7:
				if (_IgnoreFocus)
				{
					_IgnoreFocus = false;
					break;
				}
				_Focused = true;
				if (maskedTextBoxAdv.FocusHighlightEnabled && ((Control)this).Enabled)
				{
					_LastBackColor = ((Control)this).BackColor;
					((Control)this).BackColor = maskedTextBoxAdv.FocusHighlightColor;
					((Control)maskedTextBoxAdv).BackColor = maskedTextBoxAdv.FocusHighlightColor;
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				break;
			case 8:
				if (!_Focused)
				{
					_IgnoreFocus = true;
					break;
				}
				_Focused = false;
				if (((Control)this).Text == null || ((Control)this).Text.Length == 0)
				{
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				if (maskedTextBoxAdv.FocusHighlightEnabled && !_LastBackColor.IsEmpty)
				{
					((Control)this).BackColor = _LastBackColor;
					((Control)maskedTextBoxAdv).BackColor = _LastBackColor;
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				break;
			case 15:
				if (RenderWatermark)
				{
					DrawWatermark();
				}
				break;
			}
		}
	}

	private MaskedTextBox _MaskedTextBox;

	private ButtonItem _PopupItem;

	private static string _DropDownItemContainerName = "sysPopupItemContainer";

	private static string _DropDownControlContainerName = "sysPopupControlContainer";

	private Color _FocusHighlightColor = ColorScheme.GetColor(16777096);

	private bool _FocusHighlightEnabled;

	private string _WatermarkText = "";

	private Font _WatermarkFont;

	private Color _WatermarkColor = SystemColors.GrayText;

	[CompilerGenerated]
	private MaskInputRejectedEventHandler m_MaskInputRejected;

	[CompilerGenerated]
	private TypeValidationEventHandler m_TypeValidationCompleted;

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

	private bool _WatermarkEnabled = true;

	private eWatermarkBehavior _WatermarkBehavior;

	private BodyElement _TextMarkup;

	private ICommand _Command;

	private object _CommandParameter;

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
			_DropDownControl = value;
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
	public MaskedTextBox MaskedTextBox => _MaskedTextBox;

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _WatermarkEnabled;
		}
		set
		{
			_WatermarkEnabled = value;
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
			return _WatermarkText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_WatermarkText = value;
			MarkupTextChanged();
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
			return _WatermarkFont;
		}
		set
		{
			_WatermarkFont = value;
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
			return _WatermarkColor;
		}
		set
		{
			_WatermarkColor = value;
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
			return _WatermarkBehavior;
		}
		set
		{
			_WatermarkBehavior = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether PromptChar can be entered as valid data by the user")]
	public bool AllowPromptAsInput
	{
		get
		{
			return _MaskedTextBox.AllowPromptAsInput;
		}
		set
		{
			_MaskedTextBox.AllowPromptAsInput = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the MaskedTextBox control accepts characters outside of the ASCII character set")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool AsciiOnly
	{
		get
		{
			return _MaskedTextBox.AsciiOnly;
		}
		set
		{
			_MaskedTextBox.AsciiOnly = value;
		}
	}

	[Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool BeepOnError
	{
		get
		{
			return _MaskedTextBox.BeepOnError;
		}
		set
		{
			_MaskedTextBox.BeepOnError = value;
		}
	}

	[Description("Indicates culture information associated with the masked text box.")]
	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public CultureInfo Culture
	{
		get
		{
			return _MaskedTextBox.Culture;
		}
		set
		{
			_MaskedTextBox.Culture = value;
		}
	}

	[DefaultValue(2)]
	[Category("Behavior")]
	[Description("Indicates a value that determines whether literals and prompt characters are copied to the clipboard")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public MaskFormat CutCopyMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBox.CutCopyMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBox.CutCopyMaskFormat = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public IFormatProvider FormatProvider
	{
		get
		{
			return _MaskedTextBox.FormatProvider;
		}
		set
		{
			_MaskedTextBox.FormatProvider = value;
		}
	}

	[Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool HidePromptOnLeave
	{
		get
		{
			return _MaskedTextBox.HidePromptOnLeave;
		}
		set
		{
			_MaskedTextBox.HidePromptOnLeave = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates text insertion mode of the masked text box control.")]
	[Category("Behavior")]
	public InsertKeyMode InsertKeyMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBox.InsertKeyMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBox.InsertKeyMode = value;
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.MaskAdvPropertyEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Indicates input mask to use at run time.")]
	[MergableProperty(false)]
	[Category("Behavior")]
	[DefaultValue("")]
	[Localizable(true)]
	public string Mask
	{
		get
		{
			return _MaskedTextBox.Mask;
		}
		set
		{
			_MaskedTextBox.Mask = value;
		}
	}

	[Browsable(false)]
	public bool MaskCompleted => _MaskedTextBox.MaskCompleted;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public MaskedTextProvider MaskedTextProvider => _MaskedTextBox.MaskedTextProvider;

	[Browsable(false)]
	public bool MaskFull => _MaskedTextBox.MaskFull;

	[DefaultValue('\0')]
	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates character to be displayed in substitute for user input.")]
	public char PasswordChar
	{
		get
		{
			return _MaskedTextBox.PasswordChar;
		}
		set
		{
			_MaskedTextBox.PasswordChar = value;
		}
	}

	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue('_')]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates character used to represent the absence of user input in MaskedTextBox.")]
	public char PromptChar
	{
		get
		{
			return _MaskedTextBox.PromptChar;
		}
		set
		{
			_MaskedTextBox.PromptChar = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether text in control is read only.")]
	[Category("Behavior")]
	public bool ReadOnly
	{
		get
		{
			return _MaskedTextBox.ReadOnly;
		}
		set
		{
			_MaskedTextBox.ReadOnly = value;
		}
	}

	[Description("Indicates whether the parsing of user input should stop after the first invalid character is reached.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool RejectInputOnFirstFailure
	{
		get
		{
			return _MaskedTextBox.RejectInputOnFirstFailure;
		}
		set
		{
			_MaskedTextBox.RejectInputOnFirstFailure = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates value that determines how an input character that matches the prompt character should be handled.")]
	public bool ResetOnPrompt
	{
		get
		{
			return _MaskedTextBox.ResetOnPrompt;
		}
		set
		{
			_MaskedTextBox.ResetOnPrompt = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates value that determines how a space input character should be handled.")]
	public bool ResetOnSpace
	{
		get
		{
			return _MaskedTextBox.ResetOnSpace;
		}
		set
		{
			_MaskedTextBox.ResetOnSpace = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual string SelectedText
	{
		get
		{
			return ((TextBoxBase)_MaskedTextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)_MaskedTextBox).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SelectionLength
	{
		get
		{
			return ((TextBoxBase)_MaskedTextBox).SelectionLength;
		}
		set
		{
			((TextBoxBase)_MaskedTextBox).SelectionLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)_MaskedTextBox).SelectionStart;
		}
		set
		{
			((TextBoxBase)_MaskedTextBox).SelectionStart = value;
		}
	}

	[Description("Indicates whether the defined shortcuts are enabled.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public virtual bool ShortcutsEnabled
	{
		get
		{
			return ((TextBoxBase)_MaskedTextBox).ShortcutsEnabled;
		}
		set
		{
			((TextBoxBase)_MaskedTextBox).ShortcutsEnabled = value;
		}
	}

	[Description("MaskedTextBoxSkipLiterals")]
	[Category("CatBehavior")]
	[DefaultValue(true)]
	public bool SkipLiterals
	{
		get
		{
			return _MaskedTextBox.SkipLiterals;
		}
		set
		{
			_MaskedTextBox.SkipLiterals = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue("Indicates text as it is currently displayed to the user")]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Editor("System.Windows.Forms.Design.MaskedTextBoxTextEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Localizable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)_MaskedTextBox).Text = value;
		}
	}

	[DefaultValue(0)]
	[Localizable(true)]
	[Description("Indicates how text is aligned in a masked text box control.")]
	[Category("Appearance")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBox.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBox.TextAlign = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => ((TextBoxBase)_MaskedTextBox).TextLength;

	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[DefaultValue(2)]
	[Description("Indicates a value that determines whether literals and prompt characters are included in the formatted string.")]
	public MaskFormat TextMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _MaskedTextBox.TextMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_MaskedTextBox.TextMaskFormat = value;
		}
	}

	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	[Description("Indicates whether the operating system-supplied password character should be used")]
	public bool UseSystemPasswordChar
	{
		get
		{
			return _MaskedTextBox.UseSystemPasswordChar;
		}
		set
		{
			_MaskedTextBox.UseSystemPasswordChar = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public Type ValidatingType
	{
		get
		{
			return _MaskedTextBox.ValidatingType;
		}
		set
		{
			_MaskedTextBox.ValidatingType = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int PreferredHeight
	{
		get
		{
			int preferredHeight = ((TextBoxBase)_MaskedTextBox).PreferredHeight;
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

	public event CancelEventHandler ButtonClearClick;

	public event CancelEventHandler ButtonDropDownClick;

	public event EventHandler ButtonCustomClick;

	public event EventHandler ButtonCustom2Click;

	[Description("Occurs after the insert mode has changed")]
	public event EventHandler IsOverwriteModeChanged;

	[Description("Occurs after the input mask is changed.")]
	public event EventHandler MaskChanged;

	[Category("Behavior")]
	[Description("Occurs when the user's input or assigned character does not match the corresponding format element of the input mask.")]
	public event MaskInputRejectedEventHandler MaskInputRejected
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MaskInputRejectedEventHandler val = this.m_MaskInputRejected;
			MaskInputRejectedEventHandler val2;
			do
			{
				val2 = val;
				MaskInputRejectedEventHandler value2 = (MaskInputRejectedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MaskInputRejected, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MaskInputRejectedEventHandler val = this.m_MaskInputRejected;
			MaskInputRejectedEventHandler val2;
			do
			{
				val2 = val;
				MaskInputRejectedEventHandler value2 = (MaskInputRejectedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_MaskInputRejected, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when the text alignment is changed.")]
	public event EventHandler TextAlignChanged;

	[Category("Focus")]
	[Description("Occurs when MaskedTextBox has finished parsing the current value using the ValidatingType property.")]
	public event TypeValidationEventHandler TypeValidationCompleted
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			TypeValidationEventHandler val = this.m_TypeValidationCompleted;
			TypeValidationEventHandler val2;
			do
			{
				val2 = val;
				TypeValidationEventHandler value2 = (TypeValidationEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TypeValidationCompleted, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			TypeValidationEventHandler val = this.m_TypeValidationCompleted;
			TypeValidationEventHandler val2;
			do
			{
				val2 = val;
				TypeValidationEventHandler value2 = (TypeValidationEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TypeValidationCompleted, value2, val2);
			}
			while (val != val2);
		}
	}

	public MaskedTextBoxAdv()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_MaskedTextBox = (MaskedTextBox)(object)new MaskedTextBoxInternal();
		((Control)this).BackColor = SystemColors.Window;
		InitControl();
	}

	public MaskedTextBoxAdv(MaskedTextProvider maskedTextProvider)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_MaskedTextBox = new MaskedTextBox(maskedTextProvider);
		InitControl();
	}

	public MaskedTextBoxAdv(string mask)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_MaskedTextBox = new MaskedTextBox(mask);
		InitControl();
	}

	private void InitControl()
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		_BackgroundStyle.SetColorScheme(base.ColorScheme);
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		_ButtonCustom = new InputButtonSettings(this);
		_ButtonCustom2 = new InputButtonSettings(this);
		_ButtonClear = new InputButtonSettings(this);
		_ButtonDropDown = new InputButtonSettings(this);
		CreateButtonGroup();
		((TextBoxBase)_MaskedTextBox).BorderStyle = (BorderStyle)0;
		((Control)_MaskedTextBox).TextChanged += MaskedTextBoxTextChanged;
		_MaskedTextBox.IsOverwriteModeChanged += MaskedTextBoxIsOverwriteModeChanged;
		_MaskedTextBox.MaskChanged += MaskedTextBoxMaskChanged;
		_MaskedTextBox.MaskInputRejected += new MaskInputRejectedEventHandler(MaskedTextBoxMaskInputRejected);
		_MaskedTextBox.TextAlignChanged += MaskedTextBoxTextAlignChanged;
		_MaskedTextBox.TypeValidationCompleted += new TypeValidationEventHandler(MaskedTextBoxTypeValidationCompleted);
		((Control)_MaskedTextBox).SizeChanged += MaskedTextBoxSizeChanged;
		((Control)this).Controls.Add((Control)(object)_MaskedTextBox);
	}

	private void MaskedTextBoxSizeChanged(object sender, EventArgs e)
	{
		if (!_InternalSizeUpdate)
		{
			UpdateLayout();
		}
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

	private void MaskedTextBoxTypeValidationCompleted(object sender, TypeValidationEventArgs e)
	{
		OnTypeValidationCompleted(e);
	}

	protected virtual void OnTypeValidationCompleted(TypeValidationEventArgs e)
	{
		TypeValidationEventHandler typeValidationCompleted = this.TypeValidationCompleted;
		if (typeValidationCompleted != null)
		{
			typeValidationCompleted.Invoke((object)this, e);
		}
	}

	private void MaskedTextBoxTextAlignChanged(object sender, EventArgs e)
	{
		OnTextAlignChanged(e);
	}

	protected virtual void OnTextAlignChanged(EventArgs e)
	{
		this.TextAlignChanged?.Invoke(this, e);
	}

	private void MaskedTextBoxMaskInputRejected(object sender, MaskInputRejectedEventArgs e)
	{
		OnMaskInputRejected(e);
	}

	protected virtual void OnMaskInputRejected(MaskInputRejectedEventArgs e)
	{
		MaskInputRejectedEventHandler maskInputRejected = this.MaskInputRejected;
		if (maskInputRejected != null)
		{
			maskInputRejected.Invoke((object)this, e);
		}
	}

	private void MaskedTextBoxMaskChanged(object sender, EventArgs e)
	{
		OnMaskChanged(e);
	}

	protected virtual void OnMaskChanged(EventArgs e)
	{
		this.MaskChanged?.Invoke(this, e);
	}

	private void MaskedTextBoxIsOverwriteModeChanged(object sender, EventArgs e)
	{
		OnIsOverwriteModeChanged(e);
	}

	protected virtual void OnIsOverwriteModeChanged(EventArgs e)
	{
		this.IsOverwriteModeChanged?.Invoke(this, e);
	}

	private void MaskedTextBoxTextChanged(object sender, EventArgs e)
	{
		((Control)this).Text = ((Control)_MaskedTextBox).Text;
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
					visualItem.MouseUp -= new MouseEventHandler(CustomButtonClick);
				}
				else if (visualItem == _ButtonCustom2.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(CustomButton2Click);
				}
			}
		}
		buttonGroup.Items.AddRange(items);
	}

	private void CustomButtonClick(object sender, MouseEventArgs e)
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

	private void CustomButton2Click(object sender, MouseEventArgs e)
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

	protected virtual SortedList CreateSortedButtonList()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		SortedList sortedList = new SortedList(4);
		if (_ButtonCustom.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonCustom);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButtonClick);
			}
			_ButtonCustom.ItemReference = visualItem;
			visualItem.MouseUp += new MouseEventHandler(CustomButtonClick);
			visualItem.Enabled = _ButtonCustom.Enabled;
			sortedList.Add(_ButtonCustom, visualItem);
		}
		if (_ButtonCustom2.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonCustom2);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButton2Click);
			}
			_ButtonCustom2.ItemReference = visualItem2;
			visualItem2.MouseUp += new MouseEventHandler(CustomButton2Click);
			visualItem2.Enabled = _ButtonCustom2.Enabled;
			sortedList.Add(_ButtonCustom2, visualItem2);
		}
		if (_ButtonClear.Visible)
		{
			VisualItem visualItem3 = CreateButton(_ButtonClear);
			if (_ButtonClear.ItemReference != null)
			{
				_ButtonClear.ItemReference.Click -= ClearButtonClick;
			}
			_ButtonClear.ItemReference = visualItem3;
			visualItem3.MouseUp += new MouseEventHandler(ClearButtonClick);
			sortedList.Add(_ButtonClear, visualItem3);
		}
		if (_ButtonDropDown.Visible)
		{
			VisualItem visualItem4 = CreateButton(_ButtonDropDown);
			if (_ButtonDropDown.ItemReference != null)
			{
				_ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
			}
			_ButtonDropDown.ItemReference = visualItem4;
			visualItem4.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
			sortedList.Add(_ButtonDropDown, visualItem4);
		}
		return sortedList;
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
		size.Height -= ElementStyleLayout.TopWhiteSpace(backgroundStyle, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(backgroundStyle, eSpacePart.Border) + 2;
		size.Width -= ElementStyleLayout.LeftWhiteSpace(backgroundStyle, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(backgroundStyle, eSpacePart.Border) + 2;
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
		_BackgroundStyle.SetColorScheme(GetColorScheme());
		disposeStyle = false;
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
		((Control)_MaskedTextBox).BackColor = ((Control)this).BackColor;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Rectangle(ElementStyleLayout.LeftWhiteSpace(style, eSpacePart.Border) + 1, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1, _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
		}
		return new Rectangle(((Control)this).Width - (ElementStyleLayout.RightWhiteSpace(style, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(style, eSpacePart.Border) + 1, _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
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
		((Control)_MaskedTextBox).Focus();
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
			if (((TextBoxBase)_MaskedTextBox).PreferredHeight < innerRect.Height)
			{
				innerRect.Y += (innerRect.Height - ((TextBoxBase)_MaskedTextBox).PreferredHeight) / 2;
			}
			((Control)_MaskedTextBox).Bounds = innerRect;
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
		if ((int)e.Button != 1048576 || (_CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 250.0))
		{
			_CloseTime = DateTime.MinValue;
		}
		else
		{
			ShowDropDown();
		}
	}

	public void ShowDropDown()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
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
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnButtonDropDownClick(cancelEventArgs);
			if (cancelEventArgs.Cancel || _PopupItem.SubItems.Count == 0)
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
		if (!_PopupItem.SubItems.Contains(_DropDownItemContainerName) || !(_PopupItem.SubItems[_DropDownItemContainerName] is ItemContainer itemContainer))
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
	}

	private void ClearButtonClick(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			((Control)_MaskedTextBox).Text = "";
		}
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

	protected override void OnForeColorChanged(EventArgs e)
	{
		if (_MaskedTextBox != null)
		{
			((Control)_MaskedTextBox).ForeColor = ((Control)this).ForeColor;
		}
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	private void MarkupTextChanged()
	{
		_TextMarkup = null;
		if (MarkupParser.IsMarkup(ref _WatermarkText))
		{
			_TextMarkup = MarkupParser.Parse(_WatermarkText);
			ResizeMarkup();
		}
	}

	private void ResizeMarkup()
	{
		if (_TextMarkup != null)
		{
			Graphics val = CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = GetMarkupDrawContext(val);
				_TextMarkup.Measure(GetWatermarkBounds().Size, markupDrawContext);
				Size size = _TextMarkup.Bounds.Size;
				_TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, size), markupDrawContext);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private Rectangle GetWatermarkBounds()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-1, 0);
		return clientRectangle;
	}

	private MarkupDrawContext GetMarkupDrawContext(Graphics g)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(g, (_WatermarkFont == null) ? ((Control)this).Font : _WatermarkFont, _WatermarkColor, (int)((Control)this).RightToLeft == 1);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCulture()
	{
		return !CultureInfo.CurrentCulture.Equals(Culture);
	}

	public object ValidateText()
	{
		return _MaskedTextBox.ValidateText();
	}

	public override string ToString()
	{
		return ((object)_MaskedTextBox).ToString();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size preferredSize = ((Control)_MaskedTextBox).GetPreferredSize(proposedSize);
		preferredSize.Height = PreferredHeight;
		return preferredSize;
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
}
