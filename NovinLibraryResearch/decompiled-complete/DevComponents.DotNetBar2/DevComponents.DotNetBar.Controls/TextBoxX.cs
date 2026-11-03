using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Interop;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(TextBoxX), "Controls.TextBoxX.ico")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.TextBoxXDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TextBoxX : TextBox, INonClientControl, IInputButtonControl
{
	private ᶨ ᵹ;

	private string ᚷ = "";

	private bool હ;

	private Font ᚸ;

	private Color ᚹ = SystemColors.GrayText;

	private ElementStyle ᇧ;

	private IntPtr _0A03;

	private string ᵺ;

	private bool ᵻ;

	private Color ᵼ = ColorScheme.GetColor(16777096);

	private bool ᵽ;

	private Color ᵾ = Color.Empty;

	private EventHandler _181E;

	private EventHandler _181F;

	private bool ᵿ;

	private bool _ᶀ;

	private bool _ᶁ;

	private bool _ᶂ;

	private bool __17C5;

	private KeyEventHandler ᶃ;

	private _0CB4 ݶ;

	private Color _ᶄ = Color.Empty;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private bool _ᚿ = true;

	private Image _ᶅ;

	private ContentAlignment _ᶆ = (ContentAlignment)16;

	private eWatermarkBehavior _ᛀ;

	private InputButtonSettings _ᠧ;

	private InputButtonSettings _ᠨ;

	private VisualGroup _ᶇ;

	private bool _ኗ;

	private Cursor _ᶈ;

	private bool _ᶉ;

	private ColorScheme լ;

	private Color _ᖣ = Color.Empty;

	private bool ᵷ
	{
		get
		{
			if (!_ᚿ)
			{
				return false;
			}
			if (_ᛀ == eWatermarkBehavior.HideOnFocus)
			{
				if (!હ && ((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
				{
					if (ᚷ.Length <= 0)
					{
						return _ᶅ != null;
					}
					return true;
				}
				return false;
			}
			if (((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
			{
				if (ᚷ.Length <= 0)
				{
					return _ᶅ != null;
				}
				return true;
			}
			return false;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether internal override for IsInputKey returns true for the Enter key.")]
	public bool IsEnterInputKey
	{
		get
		{
			return _ᶁ;
		}
		set
		{
			_ᶁ = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether all text is auto-selected when control gets input focus.")]
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

	[Description("Gets or sets whether control prevents Beep sound when Enter key is pressed.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool PreventEnterBeep
	{
		get
		{
			return __17C5;
		}
		set
		{
			__17C5 = value;
		}
	}

	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool FocusHighlightEnabled
	{
		get
		{
			return ᵽ;
		}
		set
		{
			if (ᵽ != value)
			{
				ᵽ = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate();
				}
				if (!ᵽ)
				{
					((Control)this).ResetBackColor();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	[Category("Appearance")]
	public Color FocusHighlightColor
	{
		get
		{
			return ᵼ;
		}
		set
		{
			if (ᵼ != value)
			{
				ᵼ = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eScrollBarSkin ScrollbarSkin
	{
		get
		{
			return ᵹ.ᶭ;
		}
		set
		{
			ᵹ.ᶭ = value;
		}
	}

	[Category("Style")]
	[Description("Specifies the control border style. Default value has Class property set so the system style for the control is used.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle Border => ᇧ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((TextBoxBase)this).BorderStyle;
		}
		set
		{
		}
	}

	[DefaultValue(eRenderMode.Global)]
	[Browsable(false)]
	public eRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BaseRenderer Renderer
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[DefaultValue(true)]
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
		}
	}

	[Category("Appearance")]
	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	[DefaultValue(null)]
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
		}
	}

	[Description("Indicates watermark image alignment.")]
	[Category("Appearance")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
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
		}
	}

	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	public string WatermarkText
	{
		get
		{
			return ᚷ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			ᚷ = value;
			ᛝ();
			((Control)this).Invalidate();
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
			return ᚸ;
		}
		set
		{
			ᚸ = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return ᚹ;
		}
		set
		{
			ᚹ = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
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

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _ᠧ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom2 => _ᠨ;

	private bool ᵸ
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((TextBox)this).ScrollBars == 0)
			{
				if (_ᠧ == null || !_ᠧ.Visible)
				{
					if (_ᠨ != null)
					{
						return _ᠨ.Visible;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	ElementStyle INonClientControl.ᖕ
	{
		get
		{
			bool ᠲ = false;
			return ᶍ(out ᠲ);
		}
	}

	IntPtr INonClientControl.ᖖ => ((Control)this).Handle;

	int INonClientControl.ᖗ => ((Control)this).Width;

	int INonClientControl.ᖘ => ((Control)this).Height;

	bool INonClientControl.ᖙ => ((Control)this).IsHandleCreated;

	Color INonClientControl.ᖚ => ((Control)this).BackColor;

	[Category("Appearance")]
	[Description("Specifies back color when Enabled=false")]
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

	[Description("Occurs during preprocessing to handle command keys. Command keys are keys that always take precedence over regular input keys. Examples of command keys include accelerators and menu shortcuts. Set Handled=true to indicate that you handled the key and that it should not be passed for further processing.")]
	public event KeyEventHandler CommandKeyDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᶃ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᶃ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = ᶃ;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᶃ, value2, val2);
			}
			while (val != val2);
		}
	}

	public TextBoxX()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_ᠧ = new InputButtonSettings(this);
		_ᠨ = new InputButtonSettings(this);
		ᶑ();
		ᇧ = new ElementStyle();
		ᇧ.StyleChanged += ᶋ;
		ᵹ = new ᶨ(this, eScrollBarSkin.Optimized);
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
		((Control)this).AutoSize = false;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (StyleManager.IsMetro(newStyle) || (StyleManager.IsMetro(StyleManager.PreviousStyle) && !StyleManager.IsMetro(newStyle)))
		{
			StyleManager.UpdateAmbientColors((Control)(object)this);
		}
		ᶌ();
	}

	internal TextBoxX(bool isTextBoxItem)
		: this()
	{
		ᵻ = isTextBoxItem;
	}

	protected override void Dispose(bool disposing)
	{
		if (ᵹ != null)
		{
			ᵹ._1A17();
		}
		if (ᇧ != null)
		{
			ᇧ.StyleChanged -= ᶋ;
		}
		StyleManager.Unregister((Control)(object)this);
		((TextBox)this).Dispose(disposing);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Control)this).IsDisposed)
		{
			((TextBox)this).WndProc(ref m);
			return;
		}
		if (ᵹ != null)
		{
			if (ᵹ.WndProc(ref m))
			{
				((TextBox)this).WndProc(ref m);
			}
		}
		else
		{
			((TextBox)this).WndProc(ref m);
		}
		if (ᵸ)
		{
			int msg = ((Message)(ref m)).Msg;
			if (msg == 132 || msg == 160)
			{
				Point pt = ((Control)this).PointToClient(new Point(_0EFE._0F0B(((Message)(ref m)).LParam), _0EFE._0F0D(((Message)(ref m)).LParam)));
				if (_ᶇ.RenderBounds.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(1);
					return;
				}
			}
		}
		switch (((Message)(ref m)).Msg)
		{
		case 7:
			if (ᵿ)
			{
				ᵿ = false;
				break;
			}
			હ = true;
			_0A03 = ((Message)(ref m)).WParam;
			ᵺ = ((Control)this).Text;
			if (FocusHighlightEnabled && ((Control)this).Enabled)
			{
				ᵾ = ((Control)this).BackColor;
				((Control)this).BackColor = FocusHighlightColor;
				InvalidateNonClient();
			}
			break;
		case 8:
			if (!હ)
			{
				ᵿ = true;
				break;
			}
			હ = false;
			if (((Control)this).Text.Length == 0)
			{
				((Control)this).Invalidate();
			}
			if (FocusHighlightEnabled && !ᵾ.IsEmpty)
			{
				((Control)this).BackColor = ᵾ;
				InvalidateNonClient();
			}
			break;
		case 15:
			if (ᵷ)
			{
				_16F0();
			}
			if (((Control)this).Parent is ItemControl)
			{
				((ItemControl)(object)((Control)this).Parent).ટ();
			}
			break;
		}
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13 && _ᶁ)
		{
			return true;
		}
		return ((TextBox)this).IsInputKey(keyData);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		_ᶇ.ᡡ(e);
		if (!((TextBoxBase)this).Multiline && __17C5 && e.KeyChar == '\r')
		{
			e.Handled = true;
		}
		((Control)this).OnKeyPress(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !ᵼ.Equals((object?)ColorScheme.GetColor(16777096));
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = ColorScheme.GetColor(16777096);
	}

	internal void ᶊ()
	{
		if (!((Control)this).Focused || !(_0A03 != IntPtr.Zero))
		{
			return;
		}
		IntPtr intPtr = _0A03;
		_0A03 = IntPtr.Zero;
		Control val = Control.FromChildHandle(intPtr);
		if (val == this)
		{
			return;
		}
		for (Control parent = ((Control)this).Parent; parent != null; parent = parent.Parent)
		{
			if (parent == val)
			{
				if (val is MenuPanel)
				{
					val.Focus();
				}
				return;
			}
		}
		if (val != null)
		{
			val.Focus();
		}
		else
		{
			_089F.SetFocus(intPtr);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		_ᶇ.ᡠ(e);
		((Control)this).OnKeyUp(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		_ᶇ.ᡟ(e);
		((Control)this).OnKeyDown(e);
		if (ᵻ && !e.Handled && (int)e.KeyCode == 27)
		{
			((Control)this).Text = ᵺ;
			((TextBoxBase)this).SelectionStart = 0;
			ᶊ();
		}
	}

	protected virtual void OnCommandKeyDown(KeyEventArgs e)
	{
		KeyEventHandler val = ᶃ;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		if ((keyData & 0x41) == 65 && (int)Control.ModifierKeys == 17)
		{
			((TextBoxBase)this).SelectAll();
			return true;
		}
		if (_ᶇ.ᘭ(ref msg, keyData))
		{
			return true;
		}
		if (ᶃ != null)
		{
			KeyEventArgs val = new KeyEventArgs(keyData);
			OnCommandKeyDown(val);
			if (val.Handled)
			{
				return true;
			}
		}
		return ((TextBoxBase)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (_ᶂ)
		{
			((TextBoxBase)this).SelectAll();
		}
		((TextBox)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		_0A03 = IntPtr.Zero;
		((Control)this).OnLostFocus(e);
	}

	private void ᶋ(object _٠, EventArgs ؿ)
	{
		InvalidateNonClient();
	}

	protected override void OnReadOnlyChanged(EventArgs e)
	{
		InvalidateNonClient();
		((TextBoxBase)this).OnReadOnlyChanged(e);
	}

	public void InvalidateNonClient()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_089F.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
			SetAutoHeight();
			_089F.RECT lprcUpdate = new _089F.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			_089F.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	private void ᛝ()
	{
		ݶ = null;
		if (ሒ.መ(ref ᚷ))
		{
			ݶ = ሒ.ሕ(ᚷ);
			ᛞ();
		}
	}

	private void ᛞ()
	{
		if (ݶ != null)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = ᛟ(val);
				ݶ.Measure(ᛱ().Size, markupDrawContext);
				Size size = ݶ.Bounds.Size;
				ݶ._0CC6(new Rectangle(ᛱ().Location, size), markupDrawContext);
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

	private void _16F0()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Invalid comparison between Unknown and I4
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Invalid comparison between Unknown and I4
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Invalid comparison between Unknown and I4
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Invalid comparison between Unknown and I4
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Invalid comparison between Unknown and I4
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Invalid comparison between Unknown and I4
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Invalid comparison between Unknown and I4
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Invalid comparison between Unknown and I4
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Invalid comparison between Unknown and I4
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Invalid comparison between Unknown and I4
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Invalid comparison between Unknown and I4
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Invalid comparison between Unknown and I4
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Invalid comparison between Unknown and I4
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Invalid comparison between Unknown and I4
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Invalid comparison between Unknown and I4
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
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
				val.DrawImage(_ᶅ, rectangle);
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
			if (ݶ != null)
			{
				MarkupDrawContext d = ᛟ(val);
				ݶ.Render(d);
				return;
			}
			eTextFormat eTextFormat = eTextFormat.Default;
			if (((TextBoxBase)this).Multiline)
			{
				if ((int)((TextBox)this).TextAlign == 2)
				{
					eTextFormat |= eTextFormat.VerticalCenter;
				}
				else if ((int)((TextBox)this).TextAlign == 1)
				{
					eTextFormat |= eTextFormat.Bottom;
				}
			}
			if ((int)((Control)this).RightToLeft == 1)
			{
				eTextFormat |= eTextFormat.RightToLeft;
			}
			if ((int)((TextBox)this).TextAlign == 0)
			{
				eTextFormat = eTextFormat;
			}
			else if ((int)((TextBox)this).TextAlign == 1)
			{
				eTextFormat |= eTextFormat.Right;
			}
			else if ((int)((TextBox)this).TextAlign == 2)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			eTextFormat |= eTextFormat.EndEllipsis;
			eTextFormat |= eTextFormat.WordBreak;
			TextDrawing.DrawString(val, ᚷ, (ᚸ == null) ? ((Control)this).Font : ᚸ, ᚹ, bounds, eTextFormat);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		SetAutoHeight();
		((TextBox)this).OnFontChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetAutoHeight()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize || ((TextBoxBase)this).Multiline || (int)BorderStyle != 0 || ((Control)this).IsDisposed || ((Control)this).Parent == null || ᵻ)
		{
			return;
		}
		int num = ((Control)this).FontHeight;
		if (((Control)this).Font != null)
		{
			num = Math.Max(num, ((Control)this).Font.Height);
		}
		num++;
		bool ᠲ = false;
		ElementStyle elementStyle = ᶍ(out ᠲ);
		if (elementStyle != null)
		{
			if (elementStyle.ހ)
			{
				num = ((elementStyle.CornerType != eCornerType.Rounded && elementStyle.CornerType != eCornerType.Diagonal) ? (num + elementStyle.BorderTopWidth) : (num + Math.Max(elementStyle.BorderTopWidth, elementStyle.CornerDiameter / 2 + 1)));
			}
			if (elementStyle.ށ)
			{
				num = ((elementStyle.CornerType != eCornerType.Rounded && elementStyle.CornerType != eCornerType.Diagonal) ? (num + elementStyle.BorderBottomWidth) : (num + Math.Max(elementStyle.BorderBottomWidth, elementStyle.CornerDiameter / 2 + 1)));
			}
			num += elementStyle.PaddingTop + elementStyle.PaddingBottom;
			if (ᠲ)
			{
				elementStyle.Dispose();
			}
		}
		if (_ᠧ != null && _ᠧ.Visible && _ᠧ.Image != null)
		{
			num = Math.Max(num, _ᠧ.Image.Height + 6);
		}
		if (_ᠨ != null && _ᠨ.Visible && _ᠨ.Image != null)
		{
			num = Math.Max(num, _ᠨ.Image.Height + 6);
		}
		((Control)this).Height = num;
	}

	protected override void OnResize(EventArgs e)
	{
		ᛞ();
		_ᶇ.InvalidateArrange();
		((Control)this).OnResize(e);
	}

	private MarkupDrawContext ᛟ(Graphics _٥)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(_٥, (ᚸ == null) ? ((Control)this).Font : ᚸ, ᚹ, (int)((Control)this).RightToLeft == 1);
	}

	protected override void OnTextAlignChanged(EventArgs e)
	{
		if (ᚷ.Length > 0)
		{
			((Control)this).Invalidate();
		}
		((TextBox)this).OnTextAlignChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((TextBox)this).OnHandleCreated(e);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (ᚷ.Length > 0)
		{
			((Control)this).Invalidate();
		}
		ᶌ();
		((Control)this).OnEnabledChanged(e);
	}

	private void ᶌ()
	{
		if (!_ᖣ.IsEmpty)
		{
			if (!((Control)this).Enabled)
			{
				_ᶄ = ((Control)this).BackColor;
				((Control)this).BackColor = _ᖣ;
			}
			else if (!_ᶄ.IsEmpty)
			{
				((Control)this).BackColor = _ᶄ;
				_ᶄ = Color.Empty;
			}
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (ᚷ.Length > 0)
		{
			((Control)this).Invalidate();
		}
		((TextBoxBase)this).OnTextChanged(e);
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null)
		{
			ਛ = new Office2007Renderer();
		}
		return ਛ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return ᚹ != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	private ElementStyle ᶍ(out bool ᠲ)
	{
		ᠲ = false;
		ᇧ.SetColorScheme(GetColorScheme());
		return ElementStyleDisplay.റ(ᇧ, out ᠲ);
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
		InvalidateNonClient();
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

	public void PerformButtonCustomClick()
	{
		OnButtonCustomClick(EventArgs.Empty);
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

	public void PerformButtonCustom2Click()
	{
		OnButtonCustom2Click(EventArgs.Empty);
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

	private void ᶏ(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			OnButtonCustomClick(ؿ);
		}
	}

	private void ᶐ(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			OnButtonCustom2Click(ؿ);
		}
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
		SortedList sortedList = new SortedList(4);
		if (_ᠧ.Visible)
		{
			VisualItem visualItem = CreateButton(_ᠧ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(ᠷ);
				_ᠧ.ItemReference.Click -= ᶏ;
			}
			_ᠧ.ItemReference = visualItem;
			visualItem.Click += ᶏ;
			visualItem.MouseUp += new MouseEventHandler(ᠷ);
			visualItem.Enabled = _ᠧ.Enabled;
			sortedList.Add(_ᠧ, visualItem);
		}
		if (_ᠨ.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ᠨ);
			if (_ᠨ.ItemReference != null)
			{
				_ᠨ.ItemReference.MouseUp -= new MouseEventHandler(ᠸ);
				_ᠨ.ItemReference.Click -= ᶐ;
			}
			_ᠨ.ItemReference = visualItem2;
			visualItem2.Click += ᶐ;
			visualItem2.MouseUp += new MouseEventHandler(ᠸ);
			visualItem2.Enabled = _ᠨ.Enabled;
			sortedList.Add(_ᠨ, visualItem2);
		}
		return sortedList;
	}

	protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualCustomButton visualCustomButton = new VisualCustomButton();
		ApplyButtonSettings(buttonSettings, visualCustomButton);
		return visualCustomButton;
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
		button.Checked = buttonSettings.Checked;
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
		InvalidateNonClient();
	}

	private void ᶓ(object _٠, EventArgs ؿ)
	{
		InvalidateNonClient();
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
		ElementStyle elementStyle = ᶍ(out ᠲ);
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

	private void ᶖ(Graphics _٥)
	{
		PaintInfo paintInfo = ᶕ(_٥);
		if (!_ᶇ.IsLayoutValid)
		{
			_ᶇ.PerformLayout(paintInfo);
		}
		bool ᠲ = false;
		ElementStyle elementStyle = ᶍ(out ᠲ);
		_ᶇ.RenderBounds = new Rectangle(((Control)this).Width - (ᝈ._1752(elementStyle, eSpacePart.Border) + 1) - _ᶇ.Size.Width, ᝈ._1753(elementStyle, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
		_ᶇ.ᡤ(paintInfo);
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡘ(e);
			if (_ᶇ.RenderBounds.Contains(e.X, e.Y))
			{
				if (_ᶈ == (Cursor)null)
				{
					_ᶉ = true;
					_ᶈ = ((Control)this).Cursor;
					((Control)this).Cursor = Cursors.Arrow;
					_ᶉ = false;
				}
			}
			else if (_ᶈ != (Cursor)null)
			{
				_ᶉ = true;
				((Control)this).Cursor = _ᶈ;
				_ᶈ = null;
				_ᶉ = false;
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_ኗ = true;
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_ኗ = false;
		if (ᵸ)
		{
			_ᶇ.ᡗ();
			if (_ᶈ != (Cursor)null)
			{
				_ᶉ = true;
				((Control)this).Cursor = _ᶈ;
				_ᶈ = null;
				_ᶉ = false;
			}
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnCursorChanged(EventArgs e)
	{
		if (_ᶈ != (Cursor)null && !_ᶉ)
		{
			_ᶈ = ((Control)this).Cursor;
		}
		((Control)this).OnCursorChanged(e);
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
		((TextBoxBase)this).OnMouseUp(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡜ(e);
		}
		((Control)this).OnMouseHover(e);
	}

	private void ᖫ(ref Message Ⴘ)
	{
		((TextBox)this).WndProc(ref Ⴘ);
	}

	void INonClientControl.BaseWndProc(ref Message Ⴘ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖫ
		this.ᖫ(ref Ⴘ);
	}

	private ItemPaintArgs ᖬ(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖬ
		return this.ᖬ(_٥);
	}

	private void ᖮ(PaintEventArgs ؿ)
	{
		if (((Control)this).Parent != null)
		{
			Type typeFromHandle = typeof(Control);
			typeFromHandle.GetMethod("PaintTransparentBackground", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[2]
			{
				typeof(PaintEventArgs),
				typeof(Rectangle)
			}, null)?.Invoke(this, new object[2]
			{
				ؿ,
				new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)
			});
		}
	}

	void INonClientControl.PaintBackground(PaintEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖮ
		this.ᖮ(ؿ);
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (լ == null)
		{
			լ = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return լ;
	}

	private Point ᖱ(Point ᖲ)
	{
		return ((Control)this).PointToScreen(ᖲ);
	}

	Point INonClientControl.PointToScreen(Point ᖲ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖱ
		return this.ᖱ(ᖲ);
	}

	private void ᖴ(ref Rectangle ल)
	{
		if (!ᵸ)
		{
			return;
		}
		if (!_ᶇ.IsLayoutValid)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				PaintInfo pi = ᶕ(val);
				_ᶇ.PerformLayout(pi);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		ल.Width -= _ᶇ.Size.Width + 1;
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖴ
		this.ᖴ(ref ल);
	}

	private void ᖵ(ref Rectangle ल)
	{
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖵ
		this.ᖵ(ref ल);
	}

	private void ᖶ(Graphics _٥)
	{
		if (ᵸ)
		{
			ᶖ(_٥);
		}
	}

	void INonClientControl.RenderNonClient(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖶ
		this.ᖶ(_٥);
	}

	public bool ShouldSerializeDisabledBackColor()
	{
		return !_ᖣ.IsEmpty;
	}

	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	[SpecialName]
	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	[SpecialName]
	void INonClientControl.set_Enabled(bool P_0)
	{
		((Control)this).Enabled = P_0;
	}
}
