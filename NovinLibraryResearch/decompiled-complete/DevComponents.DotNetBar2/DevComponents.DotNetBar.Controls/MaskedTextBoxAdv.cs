using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Interop;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.MaskedTextBoxAdvDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[DefaultEvent("MaskInputRejected")]
[DefaultBindingProperty("Text")]
[ToolboxBitmap(typeof(MaskedTextBoxAdv), "Controls.MaskedTextBoxAdv.ico")]
[DefaultProperty("Mask")]
public class MaskedTextBoxAdv : PopupItemControl, ICommandSource, IInputButtonControl
{
	private class _28CD : MaskedTextBox
	{
		private bool __28CF;

		private bool __28D0;

		private Color __181D = Color.Empty;

		private bool ᵷ
		{
			get
			{
				MaskedTextBoxAdv maskedTextBoxAdv = _28CE;
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
					if (!__28D0 && ((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
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

		private MaskedTextBoxAdv _28CE => ((Control)this).Parent as MaskedTextBoxAdv;

		private void _16F0()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Invalid comparison between Unknown and I4
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Invalid comparison between Unknown and I4
			MaskedTextBoxAdv maskedTextBoxAdv = _28CE;
			if (maskedTextBoxAdv == null)
			{
				return;
			}
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				if (maskedTextBoxAdv.__1A56 != null)
				{
					MarkupDrawContext d = maskedTextBoxAdv.ᛟ(val);
					maskedTextBoxAdv.__1A56.Render(d);
					return;
				}
				eTextFormat eTextFormat = eTextFormat.Default;
				if ((int)((Control)this).RightToLeft == 1)
				{
					eTextFormat |= eTextFormat.RightToLeft;
				}
				if ((int)((MaskedTextBox)this).TextAlign == 0)
				{
					eTextFormat = eTextFormat;
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
				TextDrawing.DrawString(val, maskedTextBoxAdv.WatermarkText, (maskedTextBoxAdv.WatermarkFont == null) ? ((Control)this).Font : maskedTextBoxAdv.WatermarkFont, maskedTextBoxAdv.WatermarkColor, maskedTextBoxAdv.ᛱ(), eTextFormat);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			((MaskedTextBox)this).OnTextChanged(e);
			MaskedTextBoxAdv maskedTextBoxAdv = _28CE;
			if (maskedTextBoxAdv != null && maskedTextBoxAdv.WatermarkText.Length > 0)
			{
				((Control)this).Invalidate();
			}
		}

		protected override void WndProc(ref Message m)
		{
			((MaskedTextBox)this).WndProc(ref m);
			MaskedTextBoxAdv maskedTextBoxAdv = _28CE;
			if (maskedTextBoxAdv == null)
			{
				return;
			}
			switch (((Message)(ref m)).Msg)
			{
			case 7:
				if (__28CF)
				{
					__28CF = false;
					break;
				}
				__28D0 = true;
				if (maskedTextBoxAdv.FocusHighlightEnabled && ((Control)this).Enabled)
				{
					__181D = ((Control)this).BackColor;
					((Control)this).BackColor = maskedTextBoxAdv.FocusHighlightColor;
					((Control)maskedTextBoxAdv).BackColor = maskedTextBoxAdv.FocusHighlightColor;
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				break;
			case 8:
				if (!__28D0)
				{
					__28CF = true;
					break;
				}
				__28D0 = false;
				if (((Control)this).Text == null || ((Control)this).Text.Length == 0)
				{
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				if (maskedTextBoxAdv.FocusHighlightEnabled && !__181D.IsEmpty)
				{
					((Control)this).BackColor = __181D;
					((Control)maskedTextBoxAdv).BackColor = __181D;
					((Control)maskedTextBoxAdv).Invalidate(true);
				}
				break;
			case 15:
				if (ᵷ)
				{
					_16F0();
				}
				break;
			}
		}
	}

	private MaskedTextBox __28C1;

	private ButtonItem _០;

	private static string __22BB = "sysPopupItemContainer";

	private static string __22BA = "sysPopupControlContainer";

	private Color _ᚾ = ColorScheme.GetColor(16777096);

	private bool _ᚼ;

	private string _ᠥ = "";

	private Font _ᠣ;

	private Color _ᠤ = SystemColors.GrayText;

	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private EventHandler _181E;

	private EventHandler _181F;

	private EventHandler _28C2;

	private EventHandler _28C3;

	private MaskInputRejectedEventHandler _28C4;

	private EventHandler _28AC;

	private TypeValidationEventHandler _28C5;

	private Color _ᶄ = Color.Empty;

	private Color _ᖣ = Color.Empty;

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

	private Control __22C3;

	private bool _ᚿ = true;

	private eWatermarkBehavior _ᛀ;

	private _0CB4 __1A56;

	private ICommand _૦;

	private object _૧;

	[Description("Specifies back color when Enabled=false")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			_ᖣ = value;
			if (!((Control)this).Enabled)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
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

	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
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
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	public InputButtonSettings ButtonDropDown => __17F1;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __17F2;

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _ᠧ;

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
	public Control DropDownControl
	{
		get
		{
			return __22C3;
		}
		set
		{
			__22C3 = value;
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
	public MaskedTextBox MaskedTextBox => __28C1;

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _ᚿ;
		}
		set
		{
			_ᚿ = value;
			((Control)this).Invalidate(true);
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Category("Appearance")]
	public string WatermarkText
	{
		get
		{
			return _ᠥ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ᠥ = value;
			ᛝ();
			((Control)this).Invalidate(true);
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates watermark font.")]
	public Font WatermarkFont
	{
		get
		{
			return _ᠣ;
		}
		set
		{
			_ᠣ = value;
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
			return _ᠤ;
		}
		set
		{
			_ᠤ = value;
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
			return _ᛀ;
		}
		set
		{
			_ᛀ = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates whether PromptChar can be entered as valid data by the user")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool AllowPromptAsInput
	{
		get
		{
			return __28C1.AllowPromptAsInput;
		}
		set
		{
			__28C1.AllowPromptAsInput = value;
		}
	}

	[Description("Indicates whether the MaskedTextBox control accepts characters outside of the ASCII character set")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool AsciiOnly
	{
		get
		{
			return __28C1.AsciiOnly;
		}
		set
		{
			__28C1.AsciiOnly = value;
		}
	}

	[Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool BeepOnError
	{
		get
		{
			return __28C1.BeepOnError;
		}
		set
		{
			__28C1.BeepOnError = value;
		}
	}

	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates culture information associated with the masked text box.")]
	public CultureInfo Culture
	{
		get
		{
			return __28C1.Culture;
		}
		set
		{
			__28C1.Culture = value;
		}
	}

	[Description("Indicates a value that determines whether literals and prompt characters are copied to the clipboard")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[DefaultValue(2)]
	[Category("Behavior")]
	public MaskFormat CutCopyMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __28C1.CutCopyMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__28C1.CutCopyMaskFormat = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public IFormatProvider FormatProvider
	{
		get
		{
			return __28C1.FormatProvider;
		}
		set
		{
			__28C1.FormatProvider = value;
		}
	}

	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus.")]
	[Category("Behavior")]
	public bool HidePromptOnLeave
	{
		get
		{
			return __28C1.HidePromptOnLeave;
		}
		set
		{
			__28C1.HidePromptOnLeave = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates text insertion mode of the masked text box control.")]
	[DefaultValue(0)]
	public InsertKeyMode InsertKeyMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __28C1.InsertKeyMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__28C1.InsertKeyMode = value;
		}
	}

	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.MaskAdvPropertyEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[RefreshProperties(RefreshProperties.Repaint)]
	[MergableProperty(false)]
	[Localizable(true)]
	[Description("Indicates input mask to use at run time.")]
	[Category("Behavior")]
	public string Mask
	{
		get
		{
			return __28C1.Mask;
		}
		set
		{
			__28C1.Mask = value;
		}
	}

	[Browsable(false)]
	public bool MaskCompleted => __28C1.MaskCompleted;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MaskedTextProvider MaskedTextProvider => __28C1.MaskedTextProvider;

	[Browsable(false)]
	public bool MaskFull => __28C1.MaskFull;

	[DefaultValue('\0')]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	[Description("Indicates character to be displayed in substitute for user input.")]
	public char PasswordChar
	{
		get
		{
			return __28C1.PasswordChar;
		}
		set
		{
			__28C1.PasswordChar = value;
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates character used to represent the absence of user input in MaskedTextBox.")]
	[DefaultValue('_')]
	[Category("Appearance")]
	[Localizable(true)]
	public char PromptChar
	{
		get
		{
			return __28C1.PromptChar;
		}
		set
		{
			__28C1.PromptChar = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether text in control is read only.")]
	[Category("Behavior")]
	public bool ReadOnly
	{
		get
		{
			return __28C1.ReadOnly;
		}
		set
		{
			__28C1.ReadOnly = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the parsing of user input should stop after the first invalid character is reached.")]
	public bool RejectInputOnFirstFailure
	{
		get
		{
			return __28C1.RejectInputOnFirstFailure;
		}
		set
		{
			__28C1.RejectInputOnFirstFailure = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates value that determines how an input character that matches the prompt character should be handled.")]
	[Category("Behavior")]
	public bool ResetOnPrompt
	{
		get
		{
			return __28C1.ResetOnPrompt;
		}
		set
		{
			__28C1.ResetOnPrompt = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates value that determines how a space input character should be handled.")]
	[Category("Behavior")]
	public bool ResetOnSpace
	{
		get
		{
			return __28C1.ResetOnSpace;
		}
		set
		{
			__28C1.ResetOnSpace = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string SelectedText
	{
		get
		{
			return ((TextBoxBase)__28C1).SelectedText;
		}
		set
		{
			((TextBoxBase)__28C1).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SelectionLength
	{
		get
		{
			return ((TextBoxBase)__28C1).SelectionLength;
		}
		set
		{
			((TextBoxBase)__28C1).SelectionLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)__28C1).SelectionStart;
		}
		set
		{
			((TextBoxBase)__28C1).SelectionStart = value;
		}
	}

	[Description("Indicates whether the defined shortcuts are enabled.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public virtual bool ShortcutsEnabled
	{
		get
		{
			return ((TextBoxBase)__28C1).ShortcutsEnabled;
		}
		set
		{
			((TextBoxBase)__28C1).ShortcutsEnabled = value;
		}
	}

	[Category("CatBehavior")]
	[Description("MaskedTextBoxSkipLiterals")]
	[DefaultValue(true)]
	public bool SkipLiterals
	{
		get
		{
			return __28C1.SkipLiterals;
		}
		set
		{
			__28C1.SkipLiterals = value;
		}
	}

	[DefaultValue("Indicates text as it is currently displayed to the user")]
	[Localizable(true)]
	[Editor("System.Windows.Forms.Design.MaskedTextBoxTextEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.Repaint)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)__28C1).Text = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates how text is aligned in a masked text box control.")]
	[Localizable(true)]
	[DefaultValue(0)]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __28C1.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__28C1.TextAlign = value;
		}
	}

	[Browsable(false)]
	public virtual int TextLength => ((TextBoxBase)__28C1).TextLength;

	[Description("Indicates a value that determines whether literals and prompt characters are included in the formatted string.")]
	[Category("Behavior")]
	[DefaultValue(2)]
	[RefreshProperties(RefreshProperties.Repaint)]
	public MaskFormat TextMaskFormat
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __28C1.TextMaskFormat;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__28C1.TextMaskFormat = value;
		}
	}

	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates whether the operating system-supplied password character should be used")]
	[DefaultValue(false)]
	public bool UseSystemPasswordChar
	{
		get
		{
			return __28C1.UseSystemPasswordChar;
		}
		set
		{
			__28C1.UseSystemPasswordChar = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public Type ValidatingType
	{
		get
		{
			return __28C1.ValidatingType;
		}
		set
		{
			__28C1.ValidatingType = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int PreferredHeight
	{
		get
		{
			int preferredHeight = ((TextBoxBase)__28C1).PreferredHeight;
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

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
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

	[DefaultValue(null)]
	[Category("Commands")]
	[Browsable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
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

	[Description("Occurs after the insert mode has changed")]
	public event EventHandler IsOverwriteModeChanged
	{
		add
		{
			EventHandler eventHandler = _28C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _28C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after the input mask is changed.")]
	public event EventHandler MaskChanged
	{
		add
		{
			EventHandler eventHandler = _28C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _28C3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28C3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the user's input or assigned character does not match the corresponding format element of the input mask.")]
	public event MaskInputRejectedEventHandler MaskInputRejected
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MaskInputRejectedEventHandler val = _28C4;
			MaskInputRejectedEventHandler val2;
			do
			{
				val2 = val;
				MaskInputRejectedEventHandler value2 = (MaskInputRejectedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _28C4, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MaskInputRejectedEventHandler val = _28C4;
			MaskInputRejectedEventHandler val2;
			do
			{
				val2 = val;
				MaskInputRejectedEventHandler value2 = (MaskInputRejectedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _28C4, value2, val2);
			}
			while (val != val2);
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

	[Description("Occurs when MaskedTextBox has finished parsing the current value using the ValidatingType property.")]
	[Category("Focus")]
	public event TypeValidationEventHandler TypeValidationCompleted
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			TypeValidationEventHandler val = _28C5;
			TypeValidationEventHandler val2;
			do
			{
				val2 = val;
				TypeValidationEventHandler value2 = (TypeValidationEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _28C5, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			TypeValidationEventHandler val = _28C5;
			TypeValidationEventHandler val2;
			do
			{
				val2 = val;
				TypeValidationEventHandler value2 = (TypeValidationEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _28C5, value2, val2);
			}
			while (val != val2);
		}
	}

	public MaskedTextBoxAdv()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		__28C1 = (MaskedTextBox)(object)new _28CD();
		((Control)this).BackColor = SystemColors.Window;
		კ();
	}

	public MaskedTextBoxAdv(MaskedTextProvider maskedTextProvider)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		__28C1 = new MaskedTextBox(maskedTextProvider);
		კ();
	}

	public MaskedTextBoxAdv(string mask)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		__28C1 = new MaskedTextBox(mask);
		კ();
	}

	private void კ()
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		_ᠩ.SetColorScheme(base.ColorScheme);
		_ᠩ.StyleChanged += ઐ;
		_ᠧ = new InputButtonSettings(this);
		_ᠨ = new InputButtonSettings(this);
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		ᶑ();
		((TextBoxBase)__28C1).BorderStyle = (BorderStyle)0;
		((Control)__28C1).TextChanged += _28CC;
		__28C1.IsOverwriteModeChanged += _28CB;
		__28C1.MaskChanged += _28CA;
		__28C1.MaskInputRejected += new MaskInputRejectedEventHandler(_28C9);
		__28C1.TextAlignChanged += _28C8;
		__28C1.TypeValidationCompleted += new TypeValidationEventHandler(_28C7);
		((Control)__28C1).SizeChanged += _28C6;
		((Control)this).Controls.Add((Control)(object)__28C1);
	}

	private void _28C6(object _٠, EventArgs ؿ)
	{
		if (!__28B0)
		{
			ᘥ();
		}
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (_ᠥ.Length > 0)
		{
			((Control)this).Invalidate();
		}
		ᶌ();
		base.OnEnabledChanged(e);
	}

	private void ᶌ()
	{
		if (!_ᖣ.IsEmpty)
		{
			if (!((Control)this).Enabled)
			{
				_ᶄ = ((Control)this).BackColor;
				((Control)this).BackColor = _ᖣ;
				((Control)__28C1).BackColor = _ᖣ;
			}
			else if (!_ᶄ.IsEmpty)
			{
				((Control)this).BackColor = _ᶄ;
				((Control)__28C1).BackColor = _ᶄ;
				_ᶄ = Color.Empty;
			}
		}
	}

	public bool ShouldSerializeDisabledBackColor()
	{
		return !_ᖣ.IsEmpty;
	}

	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_ᚾ.Equals((object?)ColorScheme.GetColor(16777096));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = ColorScheme.GetColor(16777096);
	}

	private void _28C7(object _٠, TypeValidationEventArgs ؿ)
	{
		OnTypeValidationCompleted(ؿ);
	}

	protected virtual void OnTypeValidationCompleted(TypeValidationEventArgs e)
	{
		TypeValidationEventHandler val = _28C5;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void _28C8(object _٠, EventArgs ؿ)
	{
		OnTextAlignChanged(ؿ);
	}

	protected virtual void OnTextAlignChanged(EventArgs e)
	{
		_28AC?.Invoke(this, e);
	}

	private void _28C9(object _٠, MaskInputRejectedEventArgs ؿ)
	{
		OnMaskInputRejected(ؿ);
	}

	protected virtual void OnMaskInputRejected(MaskInputRejectedEventArgs e)
	{
		MaskInputRejectedEventHandler val = _28C4;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void _28CA(object _٠, EventArgs ؿ)
	{
		OnMaskChanged(ؿ);
	}

	protected virtual void OnMaskChanged(EventArgs e)
	{
		_28C3?.Invoke(this, e);
	}

	private void _28CB(object _٠, EventArgs ؿ)
	{
		OnIsOverwriteModeChanged(ؿ);
	}

	protected virtual void OnIsOverwriteModeChanged(EventArgs e)
	{
		_28C2?.Invoke(this, e);
	}

	private void _28CC(object _٠, EventArgs ؿ)
	{
		((Control)this).Text = ((Control)__28C1).Text;
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
					visualItem.MouseUp -= new MouseEventHandler(ᠷ);
				}
				else if (visualItem == _ᠨ.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(ᠸ);
				}
			}
		}
		ᶇ.Items.AddRange(items);
	}

	private void ᠷ(object _٠, MouseEventArgs ؿ)
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

	private void ᠸ(object _٠, MouseEventArgs ؿ)
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
		if (_ᠧ.Visible)
		{
			VisualItem visualItem = CreateButton(_ᠧ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(ᠷ);
			}
			_ᠧ.ItemReference = visualItem;
			visualItem.MouseUp += new MouseEventHandler(ᠷ);
			visualItem.Enabled = _ᠧ.Enabled;
			sortedList.Add(_ᠧ, visualItem);
		}
		if (_ᠨ.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ᠨ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(ᠸ);
			}
			_ᠨ.ItemReference = visualItem2;
			visualItem2.MouseUp += new MouseEventHandler(ᠸ);
			visualItem2.Enabled = _ᠨ.Enabled;
			sortedList.Add(_ᠨ, visualItem2);
		}
		if (__17F2.Visible)
		{
			VisualItem visualItem3 = CreateButton(__17F2);
			if (__17F2.ItemReference != null)
			{
				__17F2.ItemReference.Click -= _180F;
			}
			__17F2.ItemReference = visualItem3;
			visualItem3.MouseUp += new MouseEventHandler(_180F);
			sortedList.Add(__17F2, visualItem3);
		}
		if (__17F1.Visible)
		{
			VisualItem visualItem4 = CreateButton(__17F1);
			if (__17F1.ItemReference != null)
			{
				__17F1.ItemReference.MouseDown -= new MouseEventHandler(_180C);
			}
			__17F1.ItemReference = visualItem4;
			visualItem4.MouseDown += new MouseEventHandler(_180C);
			sortedList.Add(__17F1, visualItem4);
		}
		return sortedList;
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
		button.Tooltip = buttonSettings.Tooltip;
		button.Shortcut = buttonSettings.Shortcut;
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
		visualGroup.ResetMouseHover += _28B7;
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

	private void _28B7(object _٠, EventArgs ؿ)
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
		size.Height -= ᝈ._1753(elementStyle, eSpacePart.Border) + ᝈ._1754(elementStyle, eSpacePart.Border) + 2;
		size.Width -= ᝈ.ᝐ(elementStyle, eSpacePart.Border) + ᝈ._1752(elementStyle, eSpacePart.Border) + 2;
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
		_ᠩ.SetColorScheme(GetColorScheme());
		ᠲ = false;
		return ElementStyleDisplay.റ(_ᠩ, out ᠲ);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		bool enabled = ((Control)this).Enabled;
		if (!((Control)this).Enabled)
		{
			if (_ᖣ.IsEmpty)
			{
				e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
			}
			else
			{
				SolidBrush val = new SolidBrush(_ᖣ);
				try
				{
					e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
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
		((Control)__28C1).BackColor = ((Control)this).BackColor;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Rectangle(ᝈ.ᝐ(_0657, eSpacePart.Border) + 1, ᝈ._1753(_0657, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
		}
		return new Rectangle(((Control)this).Width - (ᝈ._1752(_0657, eSpacePart.Border) + 1) - _ᶇ.Size.Width, ᝈ._1753(_0657, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
	}

	protected override void OnResize(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		ᘥ();
		((Control)this).Invalidate();
		((Control)this).OnResize(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)__28C1).Focus();
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

	private void ᘥ()
	{
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
			if (((TextBoxBase)__28C1).PreferredHeight < bounds.Height)
			{
				bounds.Y += (bounds.Height - ((TextBoxBase)__28C1).PreferredHeight) / 2;
			}
			((Control)__28C1).Bounds = bounds;
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
		if ((int)ؿ.Button != 1048576 || (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 250.0))
		{
			__17F4 = DateTime.MinValue;
		}
		else
		{
			ShowDropDown();
		}
	}

	public void ShowDropDown()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if ((__22C3 == null && _០.SubItems.Count == 0) || __28B1 || _០.Expanded)
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
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnButtonDropDownClick(cancelEventArgs);
			if (cancelEventArgs.Cancel || _០.SubItems.Count == 0)
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
		if (!_០.SubItems.Contains(__22BB) || !(_០.SubItems[__22BB] is ItemContainer itemContainer))
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
	}

	private void _180F(object _٠, EventArgs ؿ)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnButtonClearClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			((Control)__28C1).Text = "";
		}
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

	protected override void OnMouseHover(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡜ(e);
		}
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡗ();
		}
		((Control)this).OnMouseLeave(e);
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

	protected override void OnForeColorChanged(EventArgs e)
	{
		if (__28C1 != null)
		{
			((Control)__28C1).ForeColor = ((Control)this).ForeColor;
		}
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _ᠤ != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	private void ᛝ()
	{
		__1A56 = null;
		if (ሒ.መ(ref _ᠥ))
		{
			__1A56 = ሒ.ሕ(_ᠥ);
			ᛞ();
		}
	}

	private void ᛞ()
	{
		if (__1A56 != null)
		{
			Graphics val = CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = ᛟ(val);
				__1A56.Measure(ᛱ().Size, markupDrawContext);
				Size size = __1A56.Bounds.Size;
				__1A56._0CC6(new Rectangle(ᛱ().Location, size), markupDrawContext);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private Rectangle ᛱ()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-1, 0);
		return clientRectangle;
	}

	private MarkupDrawContext ᛟ(Graphics _٥)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(_٥, (_ᠣ == null) ? ((Control)this).Font : _ᠣ, _ᠤ, (int)((Control)this).RightToLeft == 1);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCulture()
	{
		return !CultureInfo.CurrentCulture.Equals(Culture);
	}

	public object ValidateText()
	{
		return __28C1.ValidateText();
	}

	public override string ToString()
	{
		return ((object)__28C1).ToString();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size preferredSize = ((Control)__28C1).GetPreferredSize(proposedSize);
		preferredSize.Height = PreferredHeight;
		return preferredSize;
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
}
