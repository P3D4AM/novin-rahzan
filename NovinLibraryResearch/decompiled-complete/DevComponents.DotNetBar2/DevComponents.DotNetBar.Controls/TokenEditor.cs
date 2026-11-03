using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Interop;
using DevComponents.DotNetBar.Primitives;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.TokenEditorDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("SelectedTokensChanged")]
[ToolboxBitmap(typeof(TokenEditor), "Controls.TokenEditor.ico")]
[ToolboxItem(true)]
public class TokenEditor : Control, ሎ
{
	private const int __2BA6 = 36;

	private TextBoxX __2BA7;

	private VScrollBarAdv __059D;

	private Point __0619 = Point.Empty;

	private int __2BA8 = 2;

	private int __2BA9 = 2;

	private readonly Size __2BAA = new Size(2, 2);

	private Point __2BAB = Point.Empty;

	private ElementStyle _ᠩ;

	private CustomCollection<EditToken> __2BAC;

	private EventHandler _2BAD;

	private CustomCollection<EditToken> __2BAE;

	private Size __2BAF = new Size(14, 14);

	private bool __2BB0 = true;

	private EditToken __2BB1;

	private EventHandler _2BB2;

	private EventHandler _2BB3;

	private MouseEventHandler _2BB4;

	private MouseEventHandler _2BB5;

	private EventHandler _2BB6;

	private RemovingTokenEventHandler _2BB7;

	private bool _ᆿ;

	private EditToken __2BB8;

	private bool __2BB9;

	private int __28B3 = 120;

	private int __28B4 = 180;

	private bool __2BBA = true;

	private _26B0 _ᛕ;

	private ListBoxAdv __2BBB;

	private DateTime _ᛓ = DateTime.MinValue;

	private DateTime _ᛔ = DateTime.MinValue;

	private CancelEventHandler _2BBC;

	private EventHandler _2BBD;

	private TokenEditorPopupEventHandler _28D3;

	private eTokenFilterBehavior __2BBE = eTokenFilterBehavior.TextAndValue;

	private bool _ᛙ = true;

	private bool _ᛚ = true;

	private bool _ᛛ = true;

	private bool _ᚱ;

	private List<string> __2BBF = new List<string>();

	private ValidateTokenEventHandler _2BC0;

	private bool __2BC1 = true;

	private int __2BC2 = 5;

	private bool __2BC3 = true;

	private string _ᠥ = "";

	private Font _ᠣ;

	private Color _ᠤ = SystemColors.GrayText;

	private bool _ᚿ = true;

	private Image _ᶅ;

	private ContentAlignment _ᶆ = (ContentAlignment)16;

	private eWatermarkBehavior _ᛀ;

	private _0CB4 __1A56;

	private bool __2BC4;

	private string __2BC5 = ",";

	private bool __2BC6;

	private bool _ᬯ;

	private VisualGroup _ᶇ;

	private VisualDropDownButton __2BC7;

	private VisualUpDownButton __2BC8;

	private ToolTip _ݪ;

	private EventHandler ݫ;

	private bool _ו = true;

	private bool __2B9B;

	[Description("Indicates location of the auto-scroll position.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point AutoScrollPosition
	{
		get
		{
			return __0619;
		}
		set
		{
			if (!(__0619 == value))
			{
				__0619 = value;
				if (__059D != null && __059D.Value != -__0619.Y)
				{
					__059D.Value = Math.Min(__059D.Maximum, Math.Max(__059D.Minimum, -__0619.Y));
				}
				_2BD1();
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Description("Gets or sets bar background style.")]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CustomCollection<EditToken> SelectedTokens => __2BAC;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public CustomCollection<EditToken> Tokens => __2BAE;

	private bool _2BA3
	{
		get
		{
			if (__2BB0)
			{
				return !__2BB9;
			}
			return false;
		}
	}

	[Description("Indicates whether remove token button is displayed on individual tokens so they can be removed from the selection.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool RemoveTokenButtonVisible
	{
		get
		{
			return __2BB0;
		}
		set
		{
			if (value != __2BB0)
			{
				bool oldValue = __2BB0;
				__2BB0 = value;
				OnRemoveTokenButtonVisibleChanged(oldValue, value);
			}
		}
	}

	private EditToken _2BA4
	{
		get
		{
			return __2BB1;
		}
		set
		{
			if (__2BB1 != value)
			{
				if (__2BB1 != null)
				{
					__2BB1 = value;
				}
				((Control)this).Invalidate();
			}
		}
	}

	private EditToken _2BA5
	{
		get
		{
			return __2BB8;
		}
		set
		{
			if (__2BB8 != value)
			{
				if (__2BB8 != null)
				{
					__2BB8.IsFocused = false;
				}
				__2BB8 = value;
				if (__2BB8 != null)
				{
					__2BB8.IsFocused = true;
				}
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether tokens can be added or removed by end user. Default value is false.")]
	public bool ReadOnly
	{
		get
		{
			return __2BB9;
		}
		set
		{
			if (value != __2BB9)
			{
				bool oldValue = __2BB9;
				__2BB9 = value;
				OnReadOnlyChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(120)]
	[Description("Indicates the height of the auto-complete drop-down")]
	public int DropDownHeight
	{
		get
		{
			return __28B3;
		}
		set
		{
			if (value != __28B3)
			{
				int oldValue = __28B3;
				__28B3 = value;
				OnDropDownHeightChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(180)]
	[Description("Indicates the width of the auto-complete drop-down.")]
	public int DropDownWidth
	{
		get
		{
			return __28B4;
		}
		set
		{
			if (value != __28B4)
			{
				int oldValue = __28B4;
				__28B4 = value;
				OnDropDownWidthChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether when token text is entered into the text-box pressing the Enter key attempts to validate the token and converts the text to token.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool EnterKeyValidatesToken
	{
		get
		{
			return __2BBA;
		}
		set
		{
			__2BBA = value;
		}
	}

	[DefaultValue(eTokenFilterBehavior.TextAndValue)]
	[Description("Indicates how tokens are filtered based on the entered text")]
	[Category("Behavior")]
	public eTokenFilterBehavior TokenFilterBehavior
	{
		get
		{
			return __2BBE;
		}
		set
		{
			__2BBE = value;
		}
	}

	[Description("Indicates whether auto-complete popup size is preserved between popup displays if popup is resized by end-user.")]
	[DefaultValue(true)]
	[Category("Auto-Complete")]
	public bool PreservePopupSize
	{
		get
		{
			return _ᛙ;
		}
		set
		{
			_ᛙ = value;
		}
	}

	[Description("Indicates whether auto-complete popup can be resized by end user.")]
	[Category("Auto-Complete")]
	[DefaultValue(true)]
	public bool EnablePopupResize
	{
		get
		{
			return _ᛚ;
		}
		set
		{
			_ᛚ = value;
		}
	}

	[Category("Auto-Complete")]
	[Description("Indicates whether multi-column popup close button is visible.")]
	[DefaultValue(true)]
	public bool PopupCloseButtonVisible
	{
		get
		{
			return _ᛛ;
		}
		set
		{
			_ᛛ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPopupOpen
	{
		get
		{
			return _ᚱ;
		}
		set
		{
			if (value != _ᚱ)
			{
				if (value)
				{
					_2BE4();
				}
				else
				{
					_2BEE();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[MergableProperty(false)]
	[Localizable(true)]
	[Description("Gets the list of separators which are used to divide entered text into the tokens.")]
	[Category("Behavior")]
	public List<string> Separators => __2BBF;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control automatically increases its height as more tokens are selected. MaxHeightLines property controls the maximum number of lines control will grow to before showing scroll-bar.")]
	public bool AutoSizeHeight
	{
		get
		{
			return __2BC1;
		}
		set
		{
			if (__2BC1 != value)
			{
				__2BC1 = value;
				if (__2BC1)
				{
					_2BCE();
				}
			}
		}
	}

	[DefaultValue(5)]
	[Category("Behavior")]
	[Description("Indicates maximum number of lines control will grow to when AutoSizeHeight=true. Set to 0 to indicates unlimited growth.")]
	public int MaxHeightLines
	{
		get
		{
			return __2BC2;
		}
		set
		{
			__2BC2 = value;
		}
	}

	[Browsable(false)]
	public TextBox EditTextBox => (TextBox)(object)__2BA7;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether any text entered into the token editor is validated and converted to token when control loses focus.")]
	public bool ValidateTokenTextOnLostFocus
	{
		get
		{
			return __2BC3;
		}
		set
		{
			__2BC3 = value;
		}
	}

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
			((Control)this).Invalidate();
			_2BEF();
		}
	}

	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image WatermarkImage
	{
		get
		{
			return _ᶅ;
		}
		set
		{
			_ᶅ = value;
			((Control)this).Invalidate();
			_2BEF();
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates watermark image alignment.")]
	public ContentAlignment WatermarkImageAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ᶆ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_ᶆ = value;
			((Control)this).Invalidate();
			_2BEF();
		}
	}

	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
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
			_2BEF();
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Appearance")]
	public Font WatermarkFont
	{
		get
		{
			return _ᠣ;
		}
		set
		{
			_ᠣ = value;
			((Control)this).Invalidate();
			_2BEF();
		}
	}

	[Description("Indicates watermark text color.")]
	[Category("Appearance")]
	[Browsable(true)]
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
			_2BEF();
		}
	}

	protected virtual bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused || _ᛀ == eWatermarkBehavior.HideNonEmpty)
			{
				return SelectedTokens.Count == 0;
			}
			return false;
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
			_2BEF();
		}
	}

	[DefaultValue(",")]
	[Category("Behavior")]
	[Description("Indicates the character separator that is used to separate tokens when controls Text property is updated or parsed.")]
	public string TextSeparator
	{
		get
		{
			return __2BC5;
		}
		set
		{
			if (value != __2BC5)
			{
				string oldValue = __2BC5;
				__2BC5 = value;
				OnTextSeparatorChanged(oldValue, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether drop-down button which shows available token popup is displayed")]
	[DefaultValue(false)]
	public bool DropDownButtonVisible
	{
		get
		{
			return __2BC6;
		}
		set
		{
			if (value != __2BC6)
			{
				bool oldValue = __2BC6;
				__2BC6 = value;
				OnDropDownButtonVisibleChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether check-boxes are displayed on popup token selection list and used for token selection.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool CheckBoxesVisible
	{
		get
		{
			return _ᬯ;
		}
		set
		{
			if (value != _ᬯ)
			{
				bool ᬯ = _ᬯ;
				_ᬯ = value;
				OnCheckBoxesVisibleChanged(ᬯ, value);
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether tooltips are shown when mouse is over the cell when Tooltip property is set.")]
	public bool ShowToolTips
	{
		get
		{
			return _ו;
		}
		set
		{
			_ו = value;
		}
	}

	bool ሎ.প => false;

	[Description("Occurs when SelectedTokens collection changes.")]
	public event EventHandler SelectedTokensChanged
	{
		add
		{
			EventHandler eventHandler = _2BAD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BAD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2BAD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BAD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse enters one of the SelectedTokens token.")]
	public event EventHandler TokenMouseEnter
	{
		add
		{
			EventHandler eventHandler = _2BB2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2BB2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves one of the SelectedTokens token.")]
	public event EventHandler TokenMouseLeave
	{
		add
		{
			EventHandler eventHandler = _2BB3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2BB3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse clicks one of the SelectedTokens token.")]
	public event MouseEventHandler TokenMouseClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2BB4;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2BB4, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2BB4;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2BB4, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse double clicks one of the SelectedTokens token.")]
	public event MouseEventHandler TokenMouseDoubleClick
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2BB5;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2BB5, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = _2BB5;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2BB5, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse hovers one of the SelectedTokens token.")]
	public event EventHandler TokenMouseHover
	{
		add
		{
			EventHandler eventHandler = _2BB6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2BB6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BB6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before token is removed from the SelectedTokens by end user.")]
	public event RemovingTokenEventHandler RemovingToken
	{
		add
		{
			RemovingTokenEventHandler removingTokenEventHandler = _2BB7;
			RemovingTokenEventHandler removingTokenEventHandler2;
			do
			{
				removingTokenEventHandler2 = removingTokenEventHandler;
				RemovingTokenEventHandler value2 = (RemovingTokenEventHandler)Delegate.Combine(removingTokenEventHandler2, value);
				removingTokenEventHandler = Interlocked.CompareExchange(ref _2BB7, value2, removingTokenEventHandler2);
			}
			while ((object)removingTokenEventHandler != removingTokenEventHandler2);
		}
		remove
		{
			RemovingTokenEventHandler removingTokenEventHandler = _2BB7;
			RemovingTokenEventHandler removingTokenEventHandler2;
			do
			{
				removingTokenEventHandler2 = removingTokenEventHandler;
				RemovingTokenEventHandler value2 = (RemovingTokenEventHandler)Delegate.Remove(removingTokenEventHandler2, value);
				removingTokenEventHandler = Interlocked.CompareExchange(ref _2BB7, value2, removingTokenEventHandler2);
			}
			while ((object)removingTokenEventHandler != removingTokenEventHandler2);
		}
	}

	[Description("Occurs before token auto-complete popup is displayed and allows cancelation of popup display.")]
	public event CancelEventHandler BeforeAutoCompletePopupOpen
	{
		add
		{
			CancelEventHandler cancelEventHandler = _2BBC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2BBC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _2BBC;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2BBC, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs after auto-complete popup is open.")]
	public event EventHandler AutoCompletePopupOpened
	{
		add
		{
			EventHandler eventHandler = _2BBD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BBD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2BBD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2BBD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before the auto-complete popup is displayed and allows you to adjust popup location.")]
	public event TokenEditorPopupEventHandler BeforePopupOpen
	{
		add
		{
			TokenEditorPopupEventHandler tokenEditorPopupEventHandler = _28D3;
			TokenEditorPopupEventHandler tokenEditorPopupEventHandler2;
			do
			{
				tokenEditorPopupEventHandler2 = tokenEditorPopupEventHandler;
				TokenEditorPopupEventHandler value2 = (TokenEditorPopupEventHandler)Delegate.Combine(tokenEditorPopupEventHandler2, value);
				tokenEditorPopupEventHandler = Interlocked.CompareExchange(ref _28D3, value2, tokenEditorPopupEventHandler2);
			}
			while ((object)tokenEditorPopupEventHandler != tokenEditorPopupEventHandler2);
		}
		remove
		{
			TokenEditorPopupEventHandler tokenEditorPopupEventHandler = _28D3;
			TokenEditorPopupEventHandler tokenEditorPopupEventHandler2;
			do
			{
				tokenEditorPopupEventHandler2 = tokenEditorPopupEventHandler;
				TokenEditorPopupEventHandler value2 = (TokenEditorPopupEventHandler)Delegate.Remove(tokenEditorPopupEventHandler2, value);
				tokenEditorPopupEventHandler = Interlocked.CompareExchange(ref _28D3, value2, tokenEditorPopupEventHandler2);
			}
			while ((object)tokenEditorPopupEventHandler != tokenEditorPopupEventHandler2);
		}
	}

	[Description("Occurs when an token is selected from the auto-complete list or when text entry by end user is parsed into token to validate it.")]
	public event ValidateTokenEventHandler ValidateToken
	{
		add
		{
			ValidateTokenEventHandler validateTokenEventHandler = _2BC0;
			ValidateTokenEventHandler validateTokenEventHandler2;
			do
			{
				validateTokenEventHandler2 = validateTokenEventHandler;
				ValidateTokenEventHandler value2 = (ValidateTokenEventHandler)Delegate.Combine(validateTokenEventHandler2, value);
				validateTokenEventHandler = Interlocked.CompareExchange(ref _2BC0, value2, validateTokenEventHandler2);
			}
			while ((object)validateTokenEventHandler != validateTokenEventHandler2);
		}
		remove
		{
			ValidateTokenEventHandler validateTokenEventHandler = _2BC0;
			ValidateTokenEventHandler validateTokenEventHandler2;
			do
			{
				validateTokenEventHandler2 = validateTokenEventHandler;
				ValidateTokenEventHandler value2 = (ValidateTokenEventHandler)Delegate.Remove(validateTokenEventHandler2, value);
				validateTokenEventHandler = Interlocked.CompareExchange(ref _2BC0, value2, validateTokenEventHandler2);
			}
			while ((object)validateTokenEventHandler != validateTokenEventHandler2);
		}
	}

	[Description("Occurs when item's tooltip visibility has changed.")]
	public event EventHandler ToolTipVisibleChanged
	{
		add
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public TokenEditor()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		__2BAC = new CustomCollection<EditToken>();
		__2BAC.CollectionChanged += _2BD3;
		__2BAE = new CustomCollection<EditToken>();
		__2BAE.CollectionChanged += _2BD4;
		((Control)this).SetStyle((ControlStyles)(0x3216 | DisplayHelp.DoubleBufferFlag), true);
		_ᠩ = new ElementStyle();
		_ᠩ.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
		_ᠩ.StyleChanged += ઐ;
		__2BA7 = new TextBoxX();
		__2BA7.BorderStyle = (BorderStyle)0;
		((Control)__2BA7).Visible = false;
		((Control)__2BA7).TextChanged += _2BE3;
		((Control)__2BA7).KeyDown += new KeyEventHandler(_2BDC);
		((Control)__2BA7).GotFocus += _2BC9;
		((Control)this).Controls.Add((Control)(object)__2BA7);
		__2BA7.SetAutoHeight();
		__059D = new VScrollBarAdv();
		((Control)__059D).Visible = false;
		__059D.Scroll += new ScrollEventHandler(ک);
		((Control)this).Controls.Add((Control)(object)__059D);
		VisualGroup visualGroup = new VisualGroup();
		visualGroup.HorizontalItemSpacing = 0;
		visualGroup.ArrangeInvalid += ᶓ;
		visualGroup.RenderInvalid += ᶒ;
		visualGroup.ResetMouseHover += ᶔ;
		_ᶇ = visualGroup;
		CreateButtons();
	}

	protected override void Dispose(bool disposing)
	{
		if (__2B9B)
		{
			_1BE6._1BEB(this);
			__2B9B = false;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((Control)this).ScaleControl(factor, specified);
		__2BA7.SetAutoHeight();
		_2BCE();
	}

	private void _2BC9(object _٠, EventArgs ؿ)
	{
		_2BA5 = null;
	}

	private TokenEditorColorTable _2BCA()
	{
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.TokenEditor;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (_ᠩ != null)
		{
			_ᠩ.SetColorScheme(GetColorScheme());
		}
		TokenEditorColorTable tokenEditorColorTable = _2BCA();
		PaintBackground(e);
		Rectangle clip = _2BCD();
		e.Graphics.SetClip(clip);
		if (WatermarkEnabled && WatermarkText.Length > 0 && IsWatermarkRendered)
		{
			_16F0(e.Graphics);
		}
		foreach (EditToken item in __2BAC)
		{
			_2BCB(item, e, tokenEditorColorTable);
		}
		ᶖ(e.Graphics);
		e.Graphics.ResetClip();
		((Control)this).OnPaint(e);
	}

	private void _2BCB(EditToken _2BCC, PaintEventArgs ؿ, TokenEditorColorTable _139D)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = ؿ.Graphics;
		Rectangle bounds = _2BCC.Bounds;
		bounds.Offset(__0619);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)2;
		Color textColor = _139D.Normal.TextColor;
		LinearGradientColorTable background = _139D.Normal.Background;
		if (_2BCC.IsFocused)
		{
			textColor = _139D.Focused.TextColor;
			background = _139D.Focused.Background;
		}
		else if (_2BCC.MouseOverPart == eTokenPart.Token || _2BCC.MouseOverPart == eTokenPart.Image)
		{
			background = _139D.MouseOver.Background;
		}
		DisplayHelp.FillRoundedRectangle(graphics, bounds, 2, background.Start, background.End, background.GradientAngle);
		graphics.SmoothingMode = smoothingMode;
		string text = _2BD2(_2BCC);
		Rectangle bounds2 = bounds;
		if (_2BA3)
		{
			Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + (bounds.Height - __2BAF.Height) / 2, __2BAF.Width, __2BAF.Height);
			Color color = textColor;
			if (_2BCC.MouseOverPart == eTokenPart.RemoveButton)
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
				ᯊ.ᯓ(graphics, office2007ButtonItemColorTable.MouseOver, rectangle, RoundRectangleShapeDescriptor.RectangleShape);
				color = office2007ButtonItemColorTable.MouseOver.Text;
			}
			float fontSize = Math.Max(1f, (float)(Math.Min(rectangle.Width, rectangle.Height) * 72) / graphics.DpiX - 1.5f);
			TextDrawing.DrawStringLegacy(graphics, "\uf00d", Symbols.GetFont(fontSize, eSymbolSet.Awesome), color, rectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
			_2BCC.RemoveButtonBounds = rectangle;
			bounds2.X = rectangle.Right + __2BA9;
			bounds2.Width -= bounds2.Right - bounds.Right;
		}
		if (!string.IsNullOrEmpty(_2BCC.SymbolRealized))
		{
			Rectangle rectangle2 = new Rectangle(bounds2.X, bounds.Y, bounds.Height, bounds.Height);
			float fontSize2 = Math.Max(1f, (float)(Math.Min(rectangle2.Width, rectangle2.Height) * 72) / graphics.DpiX - 1.5f);
			TextDrawing.DrawStringLegacy(graphics, _2BCC.SymbolRealized, Symbols.GetFont(fontSize2, _2BCC.SymbolSet), _2BCC.SymbolColor.IsEmpty ? textColor : _2BCC.SymbolColor, rectangle2, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
			_2BCC.ImageBounds = rectangle2;
			bounds2.X = rectangle2.Right + __2BA9;
			bounds2.Width -= bounds2.Right - bounds.Right;
		}
		else if (_2BCC.Image != null)
		{
			Rectangle rectangle3 = new Rectangle(bounds2.X, bounds.Y, _2BCC.ImageBounds.Width, _2BCC.Image.Height);
			graphics.DrawImage(_2BCC.Image, rectangle3);
			_2BCC.ImageBounds = rectangle3;
			bounds2.X = rectangle3.Right + __2BA9;
			bounds2.Width -= bounds2.Right - bounds.Right;
		}
		TextDrawing.DrawString(graphics, text, ((Control)this).Font, textColor, bounds2, eTextFormat.VerticalCenter);
	}

	protected virtual void PaintBackground(PaintEventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		ElementStyle ᠩ = _ᠩ;
		if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, ((Control)this).BackColor);
		}
		if (((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (ᠩ.Custom)
		{
			_ = graphics.SmoothingMode;
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(ᠩ, e.Graphics, clientRectangle);
			ElementStyleDisplay.Paint(e2);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		_2BCE();
		((Control)this).OnResize(e);
	}

	private Rectangle _2BCD()
	{
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += elementStyle.PaddingLeft;
		clientRectangle.Width -= elementStyle.PaddingHorizontal;
		clientRectangle.Y += elementStyle.PaddingTop;
		clientRectangle.Height -= elementStyle.PaddingVertical;
		if (ല)
		{
			elementStyle.Dispose();
		}
		elementStyle = null;
		return clientRectangle;
	}

	private void _2BCE()
	{
		if (!((Control)this).IsHandleCreated || ((Control)this).IsDisposed || ((Control)this).Width == 0 || ((Control)this).Height == 0)
		{
			return;
		}
		int _2BD0 = 0;
		Rectangle rectangle = _2BCD();
		Graphics val = ((Control)this).CreateGraphics();
		Size size = _2BCE(val, rectangle, out _2BD0);
		if (__2BC1)
		{
			int num = Math.Min(__2BC2, _2BD0);
			int num2 = (int)(Math.Ceiling((double)size.Height / (double)_2BD0) * (double)num) + (((Control)this).Height - rectangle.Height);
			if (__2BC2 == 1)
			{
				num2 = _2BCF();
			}
			if (num2 != ((Control)this).Height)
			{
				val.Dispose();
				((Control)this).Height = num2;
				return;
			}
		}
		if (size.Height > rectangle.Height)
		{
			Rectangle _28B9 = rectangle;
			int num3 = (int)((double)rectangle.Height / Math.Ceiling((double)size.Height / (double)_2BD0));
			if ((__2BC6 && num3 <= 2) || num3 == 1)
			{
				((Control)__059D).Visible = false;
				__2BC8.Visible = true;
				_28B9.Width -= SystemInformation.VerticalScrollBarWidth * ((!__2BC6) ? 1 : 2);
				if (num3 > 1)
				{
					_ᶇ.ᡥ = eOrientation.Vertical;
				}
				else
				{
					_ᶇ.ᡥ = eOrientation.Horizontal;
				}
			}
			else
			{
				int num4 = (__2BC6 ? (_2BF1() + 1) : 0);
				((Control)__059D).Bounds = new Rectangle(rectangle.Right - SystemInformation.VerticalScrollBarWidth, rectangle.Y + num4, SystemInformation.VerticalScrollBarWidth, rectangle.Height - num4);
				((Control)__059D).Visible = true;
				__2BC8.Visible = false;
				_28B9.Width -= ((Control)__059D).Width;
			}
			size = _2BCE(val, _28B9, out _2BD0);
			__059D.Maximum = size.Height;
			__059D.LargeChange = rectangle.Height;
			if (SelectedTokens.Count > 0)
			{
				__059D.SmallChange = SelectedTokens[0].Bounds.Height;
			}
			else
			{
				__059D.SmallChange = 17;
			}
		}
		else
		{
			((Control)__059D).Visible = false;
			__0619 = Point.Empty;
			__2BC8.Visible = false;
		}
		val.Dispose();
		_2BD1();
		_2BDB();
		ᶎ();
		((Control)this).Invalidate();
	}

	private int _2BCF()
	{
		Font font = ((Control)this).Font;
		return font.Height + (SystemInformation.BorderSize.Height * 4 + 4);
	}

	private Size _2BCE(Graphics _٥, Rectangle _28B9, out int _2BD0)
	{
		_2BD0 = 0;
		if (!((Control)this).IsHandleCreated || ((Control)this).IsDisposed)
		{
			return Size.Empty;
		}
		_28B9.Inflate(-1, -1);
		Font font = ((Control)this).Font;
		Size empty = Size.Empty;
		int num = _2BCF() - 2;
		if (__2BAC.Count == 0)
		{
			_2BD0 = 1;
			empty = new Size(_28B9.Width, num);
			__2BAB = new Point(_28B9.X, _28B9.Y + (empty.Height - ((Control)__2BA7).Height) / 2);
			((Control)__2BA7).Width = _28B9.Width - (__2BC6 ? SystemInformation.VerticalScrollBarWidth : 0);
			return empty;
		}
		int num2 = 0;
		foreach (EditToken item in __2BAC)
		{
			item.RemoveButtonBounds = Rectangle.Empty;
			item.ImageBounds = Rectangle.Empty;
			string text = _2BD2(item);
			Size size = TextDrawing.MeasureString(_٥, string.IsNullOrEmpty(text) ? "A" : text, font);
			int width = size.Width;
			Size size2 = __2BAA;
			size.Width = width + size2.Width * 2;
			int height = size.Height;
			Size size3 = __2BAA;
			size.Height = height + size3.Height * 2;
			if (_2BA3)
			{
				size.Width += __2BAF.Width + __2BA9;
				if (__2BAF.Height > size.Height)
				{
					size.Height = __2BAF.Height;
				}
			}
			if (!string.IsNullOrEmpty(item.SymbolRealized))
			{
				size.Width += size.Height + __2BA9;
			}
			else if (item.Image != null)
			{
				size.Width += item.Image.Width + __2BA9;
				if (item.Image.Height > size.Height)
				{
					size.Height = item.Image.Height;
				}
			}
			num2 = Math.Max(num2, size.Height);
			item.Bounds = new Rectangle(Point.Empty, size);
		}
		_2BD0 = 1;
		empty.Height = num2;
		Point location = _28B9.Location;
		foreach (EditToken item2 in __2BAC)
		{
			Rectangle bounds = new Rectangle(location.X, location.Y, item2.Bounds.Width, num2);
			if (bounds.Right > _28B9.Right && location.X > _28B9.X)
			{
				location.Y += num2 + __2BA8;
				location.X = _28B9.X;
				bounds.Y = location.Y;
				bounds.X = location.X;
				empty.Height += num2 + __2BA8;
				_2BD0++;
			}
			location.X += bounds.Width + __2BA8;
			item2.Bounds = bounds;
			if (bounds.Right - _28B9.X > empty.Width)
			{
				empty.Width = bounds.Right - _28B9.X;
			}
		}
		if (_28B9.Right - location.X < 36)
		{
			location.Y += num2 + __2BA8;
			location.X = _28B9.X;
			_2BD0++;
			empty.Height += num2 + __2BA8;
		}
		if (_2BD0 == 1)
		{
			empty.Height = Math.Max(empty.Height, num);
		}
		__2BAB = new Point(location.X, location.Y + (num2 - ((Control)__2BA7).Height) / 2);
		((Control)__2BA7).Width = _28B9.Right - location.X - ((_2BD0 == 1 && __2BC6) ? SystemInformation.VerticalScrollBarWidth : 0);
		return empty;
	}

	private void _2BD1()
	{
		Point location = __2BAB;
		location.Offset(__0619);
		((Control)__2BA7).Location = location;
	}

	private void ک(object _٠, ScrollEventArgs ؿ)
	{
		if (ؿ.NewValue != ؿ.OldValue)
		{
			__0619.Y = -ؿ.NewValue;
			_2BD1();
			((Control)this).Invalidate();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		__2BA7.SetAutoHeight();
		_2BCE();
		((Control)this).OnHandleCreated(e);
	}

	private string _2BD2(EditToken _2BCC)
	{
		if (!string.IsNullOrEmpty(_2BCC.Text))
		{
			return _2BCC.Text;
		}
		return _2BCC.Value;
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GlobalManager.Renderer;
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return new ColorScheme();
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
		((Control)this).Invalidate();
	}

	private void _2BD3(object _٠, NotifyCollectionChangedEventArgs ؿ)
	{
		if (ؿ.Action == NotifyCollectionChangedAction.Add)
		{
			foreach (object newItem in ؿ.NewItems)
			{
				((EditToken)newItem).IsSelected = true;
			}
		}
		else if (ؿ.Action == NotifyCollectionChangedAction.Remove)
		{
			foreach (object oldItem in ؿ.OldItems)
			{
				((EditToken)oldItem).IsSelected = false;
			}
		}
		else if (ؿ.Action == NotifyCollectionChangedAction.Replace)
		{
			foreach (object newItem2 in ؿ.NewItems)
			{
				((EditToken)newItem2).IsSelected = true;
			}
			foreach (object oldItem2 in ؿ.OldItems)
			{
				((EditToken)oldItem2).IsSelected = false;
			}
		}
		else if (ؿ.Action == NotifyCollectionChangedAction.Reset)
		{
			foreach (EditToken item in __2BAE)
			{
				item.IsSelected = false;
			}
		}
		_2BCE();
		_2BF0();
		_2BEF();
		OnSelectedTokensChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedTokensChanged(EventArgs e)
	{
		_2BAD?.Invoke(this, e);
	}

	private void _2BD4(object _٠, NotifyCollectionChangedEventArgs ؿ)
	{
	}

	protected virtual void OnRemoveTokenButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (__2BAC.Count > 0)
		{
			_2BCE();
		}
	}

	public EditToken GetSelectedTokenAt(Point p)
	{
		foreach (EditToken item in __2BAC)
		{
			Rectangle bounds = item.Bounds;
			bounds.Offset(__0619);
			if (bounds.Contains(p))
			{
				return item;
			}
		}
		return null;
	}

	protected virtual void OnTokenMouseEnter(object sender, EventArgs e)
	{
		_2BB2?.Invoke(sender, e);
	}

	protected virtual void OnTokenMouseLeave(object sender, EventArgs e)
	{
		HideToolTip();
		_2BB3?.Invoke(sender, e);
	}

	protected virtual void OnTokenMouseClick(object sender, MouseEventArgs e)
	{
		MouseEventHandler val = _2BB4;
		if (val != null)
		{
			val.Invoke(sender, e);
		}
	}

	protected virtual void OnTokenMouseDoubleClick(object sender, MouseEventArgs e)
	{
		MouseEventHandler val = _2BB5;
		if (val != null)
		{
			val.Invoke(sender, e);
		}
	}

	protected virtual void OnTokenMouseHover(object sender, EventArgs e)
	{
		_2BB6?.Invoke(sender, e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		EditToken selectedTokenAt = GetSelectedTokenAt(e.Location);
		if (selectedTokenAt != __2BB1)
		{
			if (__2BB1 != null)
			{
				__2BB1.MouseOverPart = eTokenPart.None;
				OnTokenMouseLeave(__2BB1, (EventArgs)(object)e);
			}
			__2BB1 = selectedTokenAt;
			((Control)this).Invalidate();
			DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
			if (__2BB1 != null)
			{
				OnTokenMouseEnter(__2BB1, (EventArgs)(object)e);
			}
		}
		if (__2BB1 != null)
		{
			eTokenPart mouseOverPart = __2BB1.MouseOverPart;
			if (_2BA3 && !__2BB1.RemoveButtonBounds.IsEmpty && __2BB1.RemoveButtonBounds.Contains(e.Location))
			{
				__2BB1.MouseOverPart = eTokenPart.RemoveButton;
			}
			else if (!__2BB1.ImageBounds.IsEmpty && __2BB1.ImageBounds.Contains(e.Location))
			{
				__2BB1.MouseOverPart = eTokenPart.Image;
			}
			else
			{
				__2BB1.MouseOverPart = eTokenPart.Token;
			}
			if (mouseOverPart != __2BB1.MouseOverPart)
			{
				((Control)this).Invalidate();
			}
		}
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡘ(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		HideToolTip();
		if (__2BB1 == null && !__2BB9 && (!_ᶇ.Visible || !_ᶇ.RenderBounds.Contains(e.Location)))
		{
			_2BDA();
		}
		else
		{
			((Control)this).Select();
		}
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡚ(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡛ(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (__2BB1 != null && __2BB1.MouseOverPart == eTokenPart.RemoveButton)
		{
			EditToken editToken = __2BB1;
			RemovingTokenEventArgs removingTokenEventArgs = new RemovingTokenEventArgs(editToken, eEventSource.Mouse);
			OnRemovingToken(removingTokenEventArgs);
			if (removingTokenEventArgs.Cancel)
			{
				return;
			}
			_2BD5();
			SelectedTokens.Remove(editToken);
		}
		else if (__2BB1 != null)
		{
			OnTokenMouseClick(__2BB1, e);
		}
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡞ(e);
		}
		((Control)this).OnMouseClick(e);
	}

	protected override void OnMouseDoubleClick(MouseEventArgs e)
	{
		if (__2BB1 != null)
		{
			OnTokenMouseDoubleClick(__2BB1, e);
		}
		((Control)this).OnMouseDoubleClick(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		if (__2BB1 != null)
		{
			OnTokenMouseHover(__2BB1, e);
			ShowToolTip(__2BB1);
		}
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡜ(e);
		}
		((Control)this).OnMouseHover(e);
	}

	protected virtual void OnRemovingToken(RemovingTokenEventArgs e)
	{
		_2BB7?.Invoke(this, e);
	}

	private void _2BD5()
	{
		HideToolTip();
		if (__2BB1 != null)
		{
			__2BB1.MouseOverPart = eTokenPart.None;
			OnTokenMouseLeave(__2BB1, EventArgs.Empty);
			__2BB1 = null;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_ᆿ = false;
		_2BD5();
		if (_ᶇ.Visible)
		{
			_ᶇ.ᡗ();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_ᆿ = true;
		((Control)this).OnMouseEnter(e);
	}

	private void _2BD6()
	{
		if (SelectedTokens.Count != 0)
		{
			int index = __2BAC.Count - 1;
			if (__2BB8 != null)
			{
				index = Math.Max(0, __2BAC.IndexOf(__2BB8) - 1);
			}
			_2BA5 = __2BAC[index];
		}
	}

	private void _2BD7()
	{
		if (SelectedTokens.Count == 0)
		{
			return;
		}
		int index = 0;
		if (__2BB8 != null)
		{
			if (__2BAC.IndexOf(__2BB8) == __2BAC.Count - 1)
			{
				_2BA5 = null;
				((Control)__2BA7).Select();
				return;
			}
			index = Math.Min(__2BAC.Count - 1, __2BAC.IndexOf(__2BB8) + 1);
		}
		_2BA5 = __2BAC[index];
	}

	private void _2BD8(bool _2BD9, eEventSource ಥ)
	{
		EditToken editToken = __2BB8;
		if (editToken == null)
		{
			return;
		}
		RemovingTokenEventArgs removingTokenEventArgs = new RemovingTokenEventArgs(editToken, ಥ);
		OnRemovingToken(removingTokenEventArgs);
		if (!removingTokenEventArgs.Cancel)
		{
			_2BA5 = null;
			int index = Math.Max(0, __2BAC.IndexOf(editToken) - 1);
			__2BAC.Remove(editToken);
			if (__2BAC.Count > 0)
			{
				_2BA5 = __2BAC[index];
			}
			else
			{
				((Control)__2BA7).Select();
			}
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (!((Control)__2BA7).Focused)
		{
			if ((int)keyData == 37)
			{
				_2BD6();
				return true;
			}
			if ((int)keyData == 39)
			{
				_2BD7();
				return true;
			}
			if ((int)keyData == 8)
			{
				_2BD8(_2BD9: true, eEventSource.Keyboard);
				return true;
			}
			if ((int)keyData == 46)
			{
				_2BD8(_2BD9: false, eEventSource.Keyboard);
				return true;
			}
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (__2BB8 != null)
		{
			_2BA5 = null;
			((Control)__2BA7).Select();
		}
		((Control)this).OnKeyDown(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (((Control)__2BA7).Visible)
		{
			((Control)__2BA7).Visible = false;
			if (__2BC3 && !string.IsNullOrEmpty(((Control)__2BA7).Text))
			{
				_2BDF(((Control)__2BA7).Text);
			}
		}
		((Control)__2BA7).Text = "";
		_2BA5 = null;
		if (IsPopupOpen)
		{
			_2BEE();
		}
		HideToolTip();
		((Control)this).OnLeave(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		if (!__2BB9)
		{
			_2BDA();
		}
		((Control)this).OnEnter(e);
	}

	private void _2BDA()
	{
		((Control)__2BA7).Visible = true;
		((Control)__2BA7).Focus();
		_2BDB();
	}

	private void _2BDB()
	{
		if (((Control)__2BA7).Visible)
		{
			Rectangle rectangle = _2BCD();
			if (!rectangle.Contains(((Control)__2BA7).Bounds))
			{
				AutoScrollPosition = new Point(0, AutoScrollPosition.Y + (rectangle.Bottom - ((Control)__2BA7).Bounds.Bottom));
			}
		}
	}

	protected virtual void OnReadOnlyChanged(bool oldValue, bool newValue)
	{
		if (__2BB9)
		{
			if (((Control)__2BA7).Visible)
			{
				((Control)__2BA7).Visible = false;
			}
		}
		else if (((Control)this).Focused)
		{
			_2BDA();
			((Control)__2BA7).Focus();
		}
		if (__2BB0)
		{
			_2BCE();
		}
	}

	protected virtual void OnDropDownHeightChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnDropDownWidthChanged(int oldValue, int newValue)
	{
	}

	private void _2BDC(object _٠, KeyEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		if ((int)ؿ.KeyCode == 27 && IsPopupOpen)
		{
			_2BEE();
		}
		else if ((int)ؿ.KeyCode == 13 && IsPopupOpen && __2BBB != null && __2BBB.SelectedIndex >= 0)
		{
			_2BDE(__2BAE[__2BBB.SelectedIndex]);
		}
		else if ((int)ؿ.KeyCode == 13 && EnterKeyValidatesToken)
		{
			_2BDF(((Control)__2BA7).Text);
		}
		else if ((int)ؿ.KeyCode == 40)
		{
			if (IsPopupOpen && __2BBB != null)
			{
				__2BBB.SelectNextItem();
				ؿ.Handled = true;
			}
			else if (!IsPopupOpen && string.IsNullOrEmpty(((Control)__2BA7).Text))
			{
				IsPopupOpen = true;
				ؿ.Handled = true;
			}
		}
		else if (__2BBB != null && (int)ؿ.KeyCode == 38)
		{
			__2BBB.SelectPreviousItem();
			ؿ.Handled = true;
		}
		else if (((int)ؿ.KeyCode == 37 || (int)ؿ.KeyCode == 8) && string.IsNullOrEmpty(((Control)__2BA7).Text))
		{
			_2BEE();
			if (SelectedTokens.Count > 0)
			{
				((Control)this).Select();
				_2BD6();
			}
			ؿ.Handled = true;
		}
	}

	private void _2BDD(object _٠, EventArgs ؿ)
	{
		if (__2BBB.SelectedIndex >= 0)
		{
			_2BDE(__2BAE[__2BBB.SelectedIndex]);
		}
	}

	private bool _2BDE(EditToken _2BCC)
	{
		if (_2BCC.IsSelected)
		{
			return false;
		}
		ValidateTokenEventArgs validateTokenEventArgs = new ValidateTokenEventArgs(_2BCC, isNewToken: false);
		if (validateTokenEventArgs.IsValid && validateTokenEventArgs.Token != null)
		{
			_2BCC = validateTokenEventArgs.Token;
			SelectedTokens.Add(_2BCC);
			((Control)__2BA7).Text = "";
			_2BEE();
			return true;
		}
		return false;
	}

	private bool _2BDF(string _06EA)
	{
		if (string.IsNullOrEmpty(_06EA))
		{
			return false;
		}
		IsPopupOpen = false;
		bool _2BE2 = false;
		EditToken token = _2BE1(_06EA, out _2BE2);
		ValidateTokenEventArgs validateTokenEventArgs = new ValidateTokenEventArgs(token, _2BE2);
		OnValidateToken(validateTokenEventArgs);
		if (validateTokenEventArgs.IsValid && validateTokenEventArgs.Token != null)
		{
			token = validateTokenEventArgs.Token;
			SelectedTokens.Add(token);
			((Control)__2BA7).Text = "";
			_2BEE();
			return true;
		}
		return false;
	}

	private bool _2BE0(EditToken _06EC)
	{
		return _06EC.IsSelected;
	}

	private EditToken _2BE1(string _06EA, out bool _2BE2)
	{
		_2BE2 = false;
		foreach (EditToken item in __2BAE)
		{
			if (!item.IsSelected && _2BEA(_06EA, item, __2BBE))
			{
				return item;
			}
		}
		_2BE2 = true;
		return new EditToken(_06EA);
	}

	private void _2BE3(object _٠, EventArgs ؿ)
	{
		string text = ((Control)__2BA7).Text;
		if (!_ᚱ && _2BE6(text))
		{
			_2BE4();
		}
		if (__2BBB != null)
		{
			_2BEB(__2BBB, text);
		}
		foreach (string item in __2BBF)
		{
			if (text.EndsWith(item))
			{
				text = text.Substring(0, text.Length - item.Length);
				_2BDF(text);
				break;
			}
		}
	}

	protected virtual void OnAutoCompletePopupOpened(EventArgs e)
	{
		_2BBD?.Invoke(this, e);
	}

	protected virtual void OnBeforeAutoCompletePopupOpen(CancelEventArgs e)
	{
		_2BBC?.Invoke(this, e);
	}

	private void _2BE4()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (_ᛕ == null)
		{
			_ᛕ = new _26B0();
			_ᛕ._19E5 += new ToolStripDropDownClosedEventHandler(_16FE);
		}
		if (__2BBB == null)
		{
			__2BBB = _2BEC();
		}
		if (!__2B9B && __2BC6)
		{
			_1BE6._1BEA(this);
			__2B9B = true;
		}
		_2BE5(__2BBB);
		if (!_ᛕ._26B3 || !_ᛙ)
		{
			((Control)__2BBB).Height = Dpi.Height(DropDownHeight);
			((Control)__2BBB).Width = ((DropDownWidth > 0) ? Dpi.Width(DropDownWidth) : ((Control)this).Width);
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeAutoCompletePopupOpen(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			Point popupLocation = ((Control)this).PointToScreen(new Point(0, ((Control)this).Height));
			TokenEditorPopupEventArgs tokenEditorPopupEventArgs = new TokenEditorPopupEventArgs(popupLocation, ((Control)__2BBB).Size);
			OnBeforePopupOpen(tokenEditorPopupEventArgs);
			popupLocation = tokenEditorPopupEventArgs.PopupLocation;
			ePopupResizeEdge _26BB = (_ᛚ ? ePopupResizeEdge.BottomRight : ePopupResizeEdge.None);
			_ᛕ._2285 = false;
			_ᛕ._26B1 = new Rectangle(((Control)this).PointToScreen(Point.Empty), ((Control)this).Size);
			_ᛕ._26BA((Control)(object)__2BBB, popupLocation.X, popupLocation.Y, 0, 0, _26BB);
			((Control)__2BA7).Focus();
			((TextBoxBase)__2BA7).SelectionLength = 0;
			if (((TextBoxBase)__2BA7).TextLength > 0)
			{
				((TextBoxBase)__2BA7).SelectionStart = ((TextBoxBase)__2BA7).TextLength;
			}
			_ᛕ._2285 = true;
			_ᚱ = true;
			_ᛓ = DateTime.Now;
			OnAutoCompletePopupOpened(EventArgs.Empty);
		}
	}

	protected virtual void OnBeforePopupOpen(TokenEditorPopupEventArgs e)
	{
		_28D3?.Invoke(this, e);
	}

	private void _2BE5(ListBoxAdv _29AC)
	{
		_29AC.BeginUpdate();
		_29AC.Items.Clear();
		_29AC.CheckBoxesVisible = _ᬯ;
		_29AC.ShowToolTips = _ו;
		foreach (EditToken item in __2BAE)
		{
			ListBoxItem listBoxItem = new ListBoxItem();
			listBoxItem.Text = _2BD2(item);
			listBoxItem.Tooltip = item.Tooltip;
			if (_ᬯ)
			{
				listBoxItem.CheckState = (CheckState)(item.IsSelected ? 1 : 0);
			}
			else
			{
				listBoxItem.Visible = !item.IsSelected;
			}
			_29AC.Items.Add(listBoxItem);
		}
		_29AC.EndUpdate();
	}

	private bool _2BE6(string _06EA)
	{
		foreach (EditToken item in __2BAE)
		{
			if (!item.IsSelected && _2BE7(_06EA, item, __2BBE))
			{
				return true;
			}
		}
		return false;
	}

	private static bool _2BE7(string _2BE8, EditToken _2BCC, eTokenFilterBehavior _2BE9)
	{
		_2BE8 = _2BE8.ToLower();
		switch (_2BE9)
		{
		case eTokenFilterBehavior.Text:
			if (!string.IsNullOrEmpty(_2BCC.Text))
			{
				return _2BCC.Text.ToLower().Contains(_2BE8);
			}
			return false;
		case eTokenFilterBehavior.Value:
			if (!string.IsNullOrEmpty(_2BCC.Value))
			{
				return _2BCC.Value.ToLower().Contains(_2BE8);
			}
			return false;
		default:
			if (string.IsNullOrEmpty(_2BCC.Text) || !_2BCC.Text.ToLower().Contains(_2BE8))
			{
				if (!string.IsNullOrEmpty(_2BCC.Value))
				{
					return _2BCC.Value.ToLower().Contains(_2BE8);
				}
				return false;
			}
			return true;
		}
	}

	private static bool _2BEA(string _2BE8, EditToken _2BCC, eTokenFilterBehavior _2BE9)
	{
		_2BE8 = _2BE8.ToLower();
		switch (_2BE9)
		{
		case eTokenFilterBehavior.Text:
			if (!string.IsNullOrEmpty(_2BCC.Text))
			{
				return _2BCC.Text.ToLower() == _2BE8;
			}
			return false;
		case eTokenFilterBehavior.Value:
			if (!string.IsNullOrEmpty(_2BCC.Value))
			{
				return _2BCC.Value.ToLower() == _2BE8;
			}
			return false;
		default:
			if (string.IsNullOrEmpty(_2BCC.Text) || !(_2BCC.Text.ToLower() == _2BE8))
			{
				if (!string.IsNullOrEmpty(_2BCC.Value))
				{
					return _2BCC.Value.ToLower() == _2BE8;
				}
				return false;
			}
			return true;
		}
	}

	private void _2BEB(ListBoxAdv _29AC, string _2BE8)
	{
		bool flag = false;
		_29AC.BeginUpdate();
		if (string.IsNullOrEmpty(_2BE8))
		{
			if (!_ᬯ)
			{
				foreach (ListBoxItem item in _29AC.Items)
				{
					item.IsSelected = false;
					item.Visible = !item.IsSelected;
					if (item.Visible)
					{
						flag = true;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < _29AC.Items.Count; i++)
			{
				ListBoxItem listBoxItem2 = (ListBoxItem)_29AC.Items[i];
				EditToken editToken = __2BAE[i];
				if (!editToken.IsSelected && _2BE7(_2BE8, editToken, __2BBE))
				{
					if (!_ᬯ)
					{
						listBoxItem2.Visible = true;
					}
					if (!flag)
					{
						listBoxItem2.IsSelected = true;
					}
					else
					{
						listBoxItem2.IsSelected = false;
					}
					flag = true;
				}
				else if (!_ᬯ)
				{
					listBoxItem2.Visible = false;
				}
				else
				{
					listBoxItem2.IsSelected = false;
				}
			}
		}
		_29AC.EndUpdate();
		if (!flag && !_ᬯ)
		{
			_2BEE();
		}
	}

	private ListBoxAdv _2BEC()
	{
		ListBoxAdv listBoxAdv = new ListBoxAdv();
		listBoxAdv.BackgroundStyle.Class = ElementStyleClassKeys.ListBoxAdvKey;
		listBoxAdv.ItemClick += _2BDD;
		listBoxAdv.ItemCheck += _2BED;
		listBoxAdv.AutoScroll = true;
		return listBoxAdv;
	}

	private void _2BED(object _٠, ListBoxAdvItemCheckEventArgs ؿ)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		int index = __2BBB.Items.IndexOf(ؿ.Item);
		EditToken editToken = __2BAE[index];
		if ((int)ؿ.Item.CheckState == 0 && editToken.IsSelected)
		{
			SelectedTokens.Remove(editToken);
		}
		else if (!editToken.IsSelected)
		{
			SelectedTokens.Add(editToken);
		}
	}

	private void _16FE(object _٠, ToolStripDropDownClosedEventArgs ؿ)
	{
		_ᚱ = false;
		_ᛔ = DateTime.Now;
	}

	private void _2BEE()
	{
		if (_ᛕ != null && _ᚱ)
		{
			_ᛕ._26BC();
		}
	}

	protected virtual void OnValidateToken(ValidateTokenEventArgs e)
	{
		_2BC0?.Invoke(this, e);
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

	private void _2BEF()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		__2BA7.WatermarkText = _ᠥ;
		__2BA7.WatermarkColor = _ᠤ;
		__2BA7.WatermarkFont = _ᠣ;
		__2BA7.WatermarkImage = _ᶅ;
		__2BA7.WatermarkImageAlignment = _ᶆ;
		if (SelectedTokens.Count > 0 || !_ᚿ)
		{
			__2BA7.WatermarkEnabled = false;
		}
		else if (_ᛀ == eWatermarkBehavior.HideNonEmpty)
		{
			__2BA7.WatermarkEnabled = true;
			__2BA7.WatermarkBehavior = eWatermarkBehavior.HideNonEmpty;
		}
		else
		{
			__2BA7.WatermarkBehavior = eWatermarkBehavior.HideOnFocus;
		}
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

	private MarkupDrawContext ᛟ(Graphics _٥)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(_٥, (_ᠣ == null) ? ((Control)this).Font : _ᠣ, _ᠤ, (int)((Control)this).RightToLeft == 1);
	}

	private void ᛞ()
	{
		if (__1A56 != null)
		{
			Graphics val = ((Control)this).CreateGraphics();
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
		clientRectangle.X += 2;
		clientRectangle.Width -= 2;
		return clientRectangle;
	}

	private void _16F0(Graphics _٥)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Invalid comparison between Unknown and I4
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Invalid comparison between Unknown and I4
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Invalid comparison between Unknown and I4
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Invalid comparison between Unknown and I4
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Invalid comparison between Unknown and I4
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Invalid comparison between Unknown and I4
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Invalid comparison between Unknown and I4
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Invalid comparison between Unknown and I4
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Invalid comparison between Unknown and I4
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Invalid comparison between Unknown and I4
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Invalid comparison between Unknown and I4
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Invalid comparison between Unknown and I4
		Rectangle bounds = ᛱ();
		if (_ᶅ != null)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, _ᶅ.Size);
			if ((int)_ᶆ == 512)
			{
				rectangle.Location = new Point(bounds.X + (bounds.Width - rectangle.Width) / 2, bounds.Bottom - rectangle.Height);
			}
			else if ((int)_ᶆ == 256)
			{
				rectangle.Location = new Point(bounds.X, bounds.Bottom - rectangle.Height);
			}
			else if ((int)_ᶆ == 1024)
			{
				rectangle.Location = new Point(bounds.Right - rectangle.Width, bounds.Bottom - rectangle.Height);
			}
			else if ((int)_ᶆ == 32)
			{
				rectangle.Location = new Point(bounds.X + (bounds.Width - rectangle.Width) / 2, bounds.Y + (bounds.Height - rectangle.Height) / 2);
			}
			else if ((int)_ᶆ == 16)
			{
				rectangle.Location = new Point(bounds.X, bounds.Y + (bounds.Height - rectangle.Height) / 2);
			}
			else if ((int)_ᶆ == 64)
			{
				rectangle.Location = new Point(bounds.Right - rectangle.Width, bounds.Y + (bounds.Height - rectangle.Height) / 2);
			}
			else if ((int)_ᶆ == 2)
			{
				rectangle.Location = new Point(bounds.X + (bounds.Width - rectangle.Width) / 2, bounds.Y);
			}
			else if ((int)_ᶆ == 1)
			{
				rectangle.Location = new Point(bounds.X, bounds.Y);
			}
			else if ((int)_ᶆ == 4)
			{
				rectangle.Location = new Point(bounds.Right - rectangle.Width, bounds.Y);
			}
			_٥.DrawImage(_ᶅ, rectangle);
			if ((int)_ᶆ == 256 || (int)_ᶆ == 16 || (int)_ᶆ == 1)
			{
				bounds.X = rectangle.Right;
				bounds.Width = Math.Max(0, bounds.Width - rectangle.Width);
			}
			else if ((int)_ᶆ == 1024 || (int)_ᶆ == 64 || (int)_ᶆ == 4)
			{
				bounds.Width = Math.Max(0, bounds.Width - rectangle.Width);
			}
		}
		if (bounds.Width <= 0)
		{
			return;
		}
		if (__1A56 != null)
		{
			MarkupDrawContext d = ᛟ(_٥);
			if (__1A56.Bounds.Height < bounds.Height)
			{
				__1A56.Bounds = new Rectangle(bounds.X, bounds.Y + (bounds.Height - __1A56.Bounds.Height) / 2, __1A56.Bounds.Width, __1A56.Bounds.Height);
			}
			__1A56.Render(d);
			return;
		}
		eTextFormat eTextFormat = eTextFormat.VerticalCenter;
		if ((int)((Control)this).RightToLeft == 1)
		{
			eTextFormat |= eTextFormat.RightToLeft;
		}
		eTextFormat |= eTextFormat.EndEllipsis;
		eTextFormat |= eTextFormat.WordBreak;
		TextDrawing.DrawString(_٥, _ᠥ, (_ᠣ == null) ? ((Control)this).Font : _ᠣ, _ᠤ, bounds, eTextFormat);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (!__2BC4)
		{
			SelectedTokens.Clear();
			if (!string.IsNullOrEmpty(((Control)this).Text))
			{
				if (!string.IsNullOrEmpty(__2BC5))
				{
					string[] array = ((Control)this).Text.Split(new string[1] { __2BC5 }, StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						_ = array2[i];
						bool _2BE2 = false;
						_2BDE(_2BE1(((Control)this).Text, out _2BE2));
					}
				}
				else
				{
					bool _2BE22 = false;
					_2BDE(_2BE1(((Control)this).Text, out _2BE22));
				}
			}
		}
		((Control)this).OnTextChanged(e);
	}

	private void _2BF0()
	{
		__2BC4 = true;
		string text = "";
		for (int i = 0; i < __2BAC.Count; i++)
		{
			text += _2BD2(__2BAC[i]);
			if (i < __2BAC.Count - 1)
			{
				text += __2BC5;
			}
		}
		((Control)this).Text = text;
		__2BC4 = false;
	}

	protected virtual void OnTextSeparatorChanged(string oldValue, string newValue)
	{
		_2BF0();
	}

	protected virtual void OnDropDownButtonVisibleChanged(bool oldValue, bool newValue)
	{
		ᶎ();
		_2BCE();
		((Control)this).Invalidate();
	}

	protected virtual void OnCheckBoxesVisibleChanged(bool oldValue, bool newValue)
	{
	}

	private int _2BF1()
	{
		return _2BCF() - 4;
	}

	private void ᶎ()
	{
		if (!__2BC6 && !__2BC8.Visible)
		{
			_ᶇ.Visible = false;
		}
		else
		{
			_ᶇ.Visible = true;
			__2BC7.Height = _2BF1();
		}
		__2BC7.Visible = __2BC6;
		_ᶇ.InvalidateArrange();
	}

	protected virtual void CreateButtons()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		__2BC7 = new VisualDropDownButton();
		__2BC7.MouseClick += new MouseEventHandler(_180B);
		__2BC7.Visible = false;
		_ᶇ.Items.Add(__2BC7);
		__2BC8 = new VisualUpDownButton();
		__2BC8.UpClick += _2BF3;
		__2BC8.DownClick += _2BF2;
		__2BC8.Visible = false;
		_ᶇ.Items.Add(__2BC8);
	}

	private void _2BF2(object _٠, EventArgs ؿ)
	{
		if (Math.Abs(__0619.Y) < __059D.Maximum - __059D.LargeChange - 1)
		{
			AutoScrollPosition = new Point(__0619.X, __0619.Y - __059D.SmallChange);
		}
	}

	private void _2BF3(object _٠, EventArgs ؿ)
	{
		if (__0619.Y < 0)
		{
			AutoScrollPosition = new Point(__0619.X, Math.Min(0, __0619.Y + __059D.SmallChange));
		}
	}

	private void _180B(object _٠, MouseEventArgs ؿ)
	{
		IsPopupOpen = !IsPopupOpen;
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

	private void ᶖ(Graphics _٥)
	{
		PaintInfo paintInfo = ᶕ(_٥);
		if (!_ᶇ.IsLayoutValid)
		{
			_ᶇ.PerformLayout(paintInfo);
		}
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
		_ᶇ.RenderBounds = new Rectangle(((Control)this).Width - (ᝈ._1752(elementStyle, eSpacePart.Border) + 1) - _ᶇ.Size.Width, ᝈ._1753(elementStyle, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
		_ᶇ.ᡤ(paintInfo);
		if (ല)
		{
			elementStyle.Dispose();
		}
	}

	private PaintInfo ᶕ(Graphics _٥)
	{
		PaintInfo paintInfo = new PaintInfo();
		paintInfo.Graphics = _٥;
		paintInfo.DefaultFont = ((Control)this).Font;
		paintInfo.ForeColor = ((Control)this).ForeColor;
		paintInfo.RenderOffset = default(Point);
		Size size = ((Control)this).Size;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
		size.Height -= ᝈ._1753(elementStyle, eSpacePart.Border) + ᝈ._1754(elementStyle, eSpacePart.Border) + 2;
		size.Width -= ᝈ.ᝐ(elementStyle, eSpacePart.Border) + ᝈ._1752(elementStyle, eSpacePart.Border) + 2;
		paintInfo.AvailableSize = size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _ᆿ || ((Control)this).Focused;
		if (ല)
		{
			elementStyle.Dispose();
		}
		return paintInfo;
	}

	public virtual void ShowToolTip(EditToken token)
	{
		if (((Component)this).DesignMode || token == null || !token.IsSelected || !((Control)this).Visible || !ShowToolTips)
		{
			return;
		}
		string tooltip = token.Tooltip;
		if (!string.IsNullOrEmpty(tooltip))
		{
			if (_ݪ == null)
			{
				_ݪ = new ToolTip();
			}
			_ݪ.Style = StyleManager.GetEffectiveStyle();
			_ݪ.Text = tooltip;
			_ݪ.ReferenceRectangle = new Rectangle(((Control)this).PointToScreen(token.Bounds.Location), token.Bounds.Size);
			_2BF4(new EventArgs());
			_ݪ.ShowToolTip();
		}
	}

	protected internal void HideToolTip()
	{
		if (_ݪ == null)
		{
			return;
		}
		Rectangle bounds = ((Control)_ݪ).Bounds;
		bounds.Width += 5;
		bounds.Height += 6;
		_2BF4(new EventArgs());
		try
		{
			if (_ݪ != null)
			{
				((Control)_ݪ).Hide();
				((Component)(object)_ݪ).Dispose();
				_ݪ = null;
			}
		}
		catch
		{
		}
		((Control)this).Invalidate();
	}

	private void _2BF4(EventArgs _2BF5)
	{
		EventHandler eventHandler = ݫ;
		if (eventHandler != null)
		{
			ݫ(this, _2BF5);
		}
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (IsPopupOpen && _ᛕ != null && !_ᛕ.ঋ.Contains(Control.MousePosition))
		{
			IsPopupOpen = false;
		}
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}
}
