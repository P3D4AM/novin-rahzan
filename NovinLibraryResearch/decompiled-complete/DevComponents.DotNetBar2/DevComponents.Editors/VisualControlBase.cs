using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Interop;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors;

[ToolboxItem(false)]
public class VisualControlBase : PopupItemControl, IInputButtonControl, ISupportInitialize
{
	private VisualItem __181C;

	private bool _ᚼ;

	private Color __181D = Color.Empty;

	private static Color _ᚽ = Color.FromArgb(255, 255, 136);

	private Color _ᚾ = _ᚽ;

	private bool _ኗ;

	private EventHandler _181E;

	private EventHandler _181F;

	private bool _ᠠ = true;

	private bool _ᒐ = true;

	private bool _ᠡ = true;

	private bool _י;

	private eHorizontalAlignment _ᠢ;

	private Font _ᠣ;

	private Color _ᠤ = SystemColors.GrayText;

	private bool _ᚿ = true;

	private string _ᠥ = "";

	private eTextAlignment _ᠦ;

	private InputButtonSettings _ᠧ;

	private InputButtonSettings _ᠨ;

	private ElementStyle _ᠩ = new ElementStyle();

	private int _ᠪ = -1;

	private int _ᠫ;

	private eInputFieldNavigation _ᠬ = eInputFieldNavigation.All;

	private Color _ᖣ = Color.Empty;

	private Color _ᛋ = Color.Empty;

	private InputControlColors _ቿ;

	private bool _ᠭ;

	internal bool _᠙
	{
		get
		{
			return _ᠠ;
		}
		set
		{
			_ᠠ = value;
			ᠰ();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal bool _181A
	{
		get
		{
			return _ᒐ;
		}
		set
		{
			_ᒐ = value;
		}
	}

	protected virtual bool IsWatermarkRendered => false;

	[DefaultValue(true)]
	[Description("Indicates whether mouse wheel affects the input control.")]
	[Category("Behavior")]
	public bool InputMouseWheelEnabled
	{
		get
		{
			return _ᠡ;
		}
		set
		{
			_ᠡ = value;
		}
	}

	[Browsable(false)]
	public bool IsKeyboardFocusWithin
	{
		get
		{
			return _י;
		}
		internal set
		{
			_י = value;
			OnIsKeyboardFocusWithinChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual VisualItem RootVisualItem => __181C;

	[Description("Indicates alignment of input fields inside of the control.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eHorizontalAlignment.Left)]
	public virtual eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return _ᠢ;
		}
		set
		{
			if (_ᠢ != value)
			{
				eHorizontalAlignment ᠢ = _ᠢ;
				_ᠢ = value;
				__181C.InvalidateArrange();
				OnInputHorizontalAlignmentChanged(ᠢ, value);
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public virtual Font WatermarkFont
	{
		get
		{
			return _ᠣ;
		}
		set
		{
			_ᠣ = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	[Browsable(true)]
	public virtual Color WatermarkColor
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

	[Description("Indicates whether watermark text is displayed if set for the input items.")]
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

	[Category("Watermark")]
	[Localizable(true)]
	[Description("Indicates watermark text displayed on the input control when control is empty.")]
	[DefaultValue("")]
	public string WatermarkText
	{
		get
		{
			return _ᠥ;
		}
		set
		{
			if (value != null)
			{
				_ᠥ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Watermark")]
	[Description("Indicates watermark text alignment.")]
	[DefaultValue(eTextAlignment.Left)]
	[Browsable(true)]
	public eTextAlignment WatermarkAlignment
	{
		get
		{
			return _ᠦ;
		}
		set
		{
			if (_ᠦ != value)
			{
				_ᠦ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public virtual bool FocusHighlightEnabled
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
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	[Category("Appearance")]
	public virtual Color FocusHighlightColor
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
					((Control)this).Invalidate();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	public InputButtonSettings ButtonCustom => _ᠧ;

	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Buttons")]
	public InputButtonSettings ButtonCustom2 => _ᠨ;

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets control background style.")]
	public ElementStyle BackgroundStyle => _ᠩ;

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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int PreferredHeight => GetPreferredHeight();

	internal int _181B
	{
		get
		{
			return _ᠫ;
		}
		set
		{
			_ᠫ = value;
			_ᠪ = -1;
		}
	}

	protected override Size DefaultSize => new Size(80, GetPreferredHeight());

	[DefaultValue(eInputFieldNavigation.All)]
	[Description("Indicates keys used to navigate between the input fields provided by this control")]
	public eInputFieldNavigation FieldNavigation
	{
		get
		{
			return _ᠬ;
		}
		set
		{
			if (_ᠬ != value)
			{
				_ᠬ = value;
				ApplyFieldNavigation();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates control background color when control is disabled")]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			if (_ᖣ != value)
			{
				_ᖣ = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates control background color when control is disabled")]
	public Color DisabledForeColor
	{
		get
		{
			return _ᛋ;
		}
		set
		{
			if (_ᛋ != value)
			{
				_ᛋ = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Colors")]
	[Description("System colors used by the control.")]
	public InputControlColors Colors => _ቿ;

	protected virtual bool SupportsFreeTextEntry => false;

	protected virtual bool IsFreeTextEntryVisible => false;

	[Browsable(false)]
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
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	[Browsable(false)]
	public override bool DisabledImagesGrayScale
	{
		get
		{
			return base.DisabledImagesGrayScale;
		}
		set
		{
			base.DisabledImagesGrayScale = value;
		}
	}

	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[Browsable(false)]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	protected virtual bool IsInitializing => _ᠭ;

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

	public VisualControlBase()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(SystemOptions._2335, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).IsAccessible = true;
		_ቿ = new InputControlColors();
		_ቿ.ColorChanged += ᠯ;
		_ᠧ = new InputButtonSettings(this);
		_ᠨ = new InputButtonSettings(this);
		__181C = CreateRootVisual();
		__181C.ArrangeInvalid += ᠳ;
		__181C.RenderInvalid += ᠴ;
		__181C.ResetMouseHover += ᠮ;
		_ᠩ.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
		_ᠩ.SetColorScheme(base.ColorScheme);
		_ᠩ.StyleChanged += ઐ;
		ApplyFieldNavigation();
	}

	private void ᠮ(object _٠, EventArgs ؿ)
	{
		DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
	}

	public void ResetInputPosition()
	{
		if (__181C is VisualInputBase)
		{
			((VisualInputBase)__181C).ResetInputPosition();
		}
		else if (__181C is VisualInputGroup)
		{
			((VisualInputGroup)__181C).ResetInputPosition();
		}
	}

	private void ᠯ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	protected virtual PaintInfo CreatePaintInfo(PaintEventArgs e)
	{
		PaintInfo paintInfo = new PaintInfo();
		paintInfo.Graphics = e.Graphics;
		paintInfo.DefaultFont = ((Control)this).Font;
		paintInfo.ForeColor = ((Control)this).ForeColor;
		paintInfo.RenderOffset = default(Point);
		paintInfo.WatermarkColor = _ᠤ;
		paintInfo.WatermarkEnabled = _ᚿ;
		paintInfo.WatermarkFont = _ᠣ;
		paintInfo.AvailableSize = ((Control)this).ClientRectangle.Size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _ኗ || ((Control)this).Focused;
		paintInfo.Colors = _ቿ;
		if (!_ᛋ.IsEmpty)
		{
			paintInfo.DisabledForeColor = _ᛋ;
		}
		return paintInfo;
	}

	private void ᠰ()
	{
		if (__181C is VisualGroup)
		{
			VisualGroup visualGroup = (VisualGroup)__181C;
			VisualItem[] array = new VisualItem[visualGroup.Items.Count];
			visualGroup.Items._0CD0(array);
			VisualItem[] array2 = array;
			foreach (VisualItem visualItem in array2)
			{
				if (visualItem.ᡇ == _1885._1886 || visualItem is VisualUpDownButton || visualItem is LockUpdateCheckBox)
				{
					visualItem.IsRendered = _ᠠ;
				}
			}
		}
		((Control)this).Invalidate();
	}

	internal void ઔ(PaintEventArgs ؿ)
	{
		((Control)this).OnPaint(ؿ);
	}

	private void ᠱ()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		MethodInvoker val = null;
		if (((Control)this).InvokeRequired)
		{
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					ᠱ();
				};
			}
			((Control)this).BeginInvoke((Delegate)(object)val);
			return;
		}
		Graphics val2 = CreateGraphics();
		try
		{
			PaintEventArgs val3 = new PaintEventArgs(val2, Rectangle.Empty);
			PaintInfo paintInfo = CreatePaintInfo(val3);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			bool ᠲ = false;
			ElementStyle elementStyle = _1400(out ᠲ);
			if (elementStyle.Custom)
			{
				new ElementStyleDisplayInfo(elementStyle, val3.Graphics, clientRectangle);
				clientRectangle.X += ᝈ.ᝐ(elementStyle);
				clientRectangle.Y += ᝈ._1753(elementStyle);
				clientRectangle.Width -= ᝈ.ᝍ(elementStyle);
				clientRectangle.Height -= ᝈ.ᝏ(elementStyle);
				paintInfo.RenderOffset = clientRectangle.Location;
				paintInfo.AvailableSize = clientRectangle.Size;
			}
			__181C.PerformLayout(paintInfo);
			val3.Dispose();
			if (ᠲ)
			{
				elementStyle.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		PaintInfo paintInfo = CreatePaintInfo(e);
		bool enabled = ((Control)this).Enabled;
		if (!enabled)
		{
			Color color = _ᖣ;
			if (color.IsEmpty)
			{
				color = SystemColors.Control;
			}
			SolidBrush val = new SolidBrush(color);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else if (((Control)this).BackColor != Color.Transparent)
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, clientRectangle);
		}
		else if (((Control)this).BackColor == Color.Transparent && _ᒐ)
		{
			((Control)this).OnPaintBackground(e);
		}
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		if (elementStyle.Custom)
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(elementStyle, e.Graphics, clientRectangle);
			if (!enabled)
			{
				ElementStyleDisplay.PaintBorder(e2);
			}
			else
			{
				ElementStyleDisplay.Paint(e2);
			}
			clientRectangle.X += ᝈ.ᝐ(elementStyle);
			clientRectangle.Y += ᝈ._1753(elementStyle);
			clientRectangle.Width -= ᝈ.ᝍ(elementStyle);
			clientRectangle.Height -= ᝈ.ᝏ(elementStyle);
			paintInfo.RenderOffset = clientRectangle.Location;
			paintInfo.AvailableSize = clientRectangle.Size;
		}
		if (_ᚼ && ((Control)this).Focused && !_ᚾ.IsEmpty)
		{
			SolidBrush val2 = new SolidBrush(_ᚾ);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!__181C.IsLayoutValid)
		{
			if (__181C is VisualGroup)
			{
				((VisualGroup)__181C).HorizontalItemAlignment = _ᠢ;
			}
			__181C.PerformLayout(paintInfo);
		}
		if (SupportsFreeTextEntry)
		{
			if (!IsFreeTextEntryVisible)
			{
				HideFreeTextBoxEntry();
			}
			else
			{
				Control freeTextBox = GetFreeTextBox();
				Rectangle freeTextBounds = GetFreeTextBounds(freeTextBox.PreferredSize, clientRectangle);
				if (freeTextBox.Bounds != freeTextBounds)
				{
					freeTextBox.Bounds = freeTextBounds;
				}
				paintInfo.RenderSystemItemsOnly = true;
			}
		}
		if (_ᠢ != 0)
		{
			if (_ᠢ == eHorizontalAlignment.Right)
			{
				paintInfo.RenderOffset = new Point(clientRectangle.Width - __181C.Size.Width, (clientRectangle.Height - __181C.Size.Height) / 2);
			}
			else
			{
				paintInfo.RenderOffset = new Point((clientRectangle.Width - __181C.Size.Width) / 2, (clientRectangle.Height - __181C.Size.Height) / 2);
			}
		}
		else
		{
			paintInfo.RenderOffset = new Point(0, (clientRectangle.Height - __181C.Size.Height) / 2);
		}
		if (WatermarkEnabled && WatermarkText.Length > 0 && IsWatermarkRendered)
		{
			Rectangle r = clientRectangle;
			r.Inflate(-1, -1);
			DrawWatermark(paintInfo, r);
		}
		else
		{
			__181C.ᡤ(paintInfo);
		}
		((Control)this).OnPaint(e);
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
	}

	private ElementStyle _1400(out bool ᠲ)
	{
		ᠲ = false;
		_ᠩ.SetColorScheme(base.ColorScheme);
		return ElementStyleDisplay.റ(_ᠩ, out ᠲ);
	}

	protected virtual void DrawWatermark(PaintInfo p, Rectangle r)
	{
		if (WatermarkText.Length != 0)
		{
			Font font = p.DefaultFont;
			if (WatermarkFont != null)
			{
				font = WatermarkFont;
			}
			eTextFormat eTextFormat = eTextFormat.Default;
			if (_ᠦ == eTextAlignment.Center)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if (_ᠦ == eTextAlignment.Right)
			{
				eTextFormat |= eTextFormat.Right;
			}
			TextDrawing.DrawString(p.Graphics, WatermarkText, font, WatermarkColor, r, eTextFormat);
		}
	}

	protected override bool IsInputChar(char charCode)
	{
		return true;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		((Control)this).OnKeyDown(e);
		if (!e.Handled)
		{
			__181C.ᡟ(e);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
		if (!e.Handled)
		{
			__181C.ᡡ(e);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
		if (!e.Handled)
		{
			__181C.ᡠ(e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsFreeTextEntryVisible && __181C.ᘭ(ref msg, keyData))
		{
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (!((Control)this).Focused)
		{
			if (((Control)this).Parent is MenuPanel)
			{
				((Control)this).Focus();
			}
			else
			{
				((Control)this).Select();
			}
		}
		__181C.ᡚ(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (_ᠡ)
		{
			__181C.ᡙ(e);
		}
		((Control)this).OnMouseWheel(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		__181C.ᡘ(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		__181C.ᡛ(e);
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_ኗ = true;
		((Control)this).Invalidate();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_ኗ = false;
		__181C.ᡗ();
		((Control)this).Invalidate();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		__181C.ᡜ(e);
		((Control)this).OnMouseHover(e);
	}

	protected override void OnClick(EventArgs e)
	{
		__181C.ᡝ();
		((Control)this).OnClick(e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		__181C.ᡞ(e);
		((Control)this).OnMouseClick(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		__181C.ᡢ();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		__181C.ᡣ();
		((Control)this).OnLostFocus(e);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		if (SupportsFreeTextEntry && (keyData & 0x60000) == 0 && (keyData & 0x10000) == 65536 && IsFreeTextEntryVisible)
		{
			Keys val = (Keys)(keyData & 0xFFFF);
			Keys val2 = val;
			if ((int)val2 == 9 && ProcessTabKey(forward: false))
			{
				return true;
			}
		}
		return ((Control)this).ProcessDialogKey(keyData);
	}

	protected virtual bool ProcessTabKey(bool forward)
	{
		return ((Control)this).Parent.SelectNextControl((Control)(object)this, forward, true, false, false);
	}

	protected virtual void OnIsKeyboardFocusWithinChanged()
	{
		if (SupportsFreeTextEntry && !IsKeyboardFocusWithin)
		{
			__181C.ᡣ();
		}
		if (FocusHighlightEnabled)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		IsKeyboardFocusWithin = true;
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		IsKeyboardFocusWithin = false;
		((Control)this).OnLeave(e);
	}

	protected virtual VisualItem CreateRootVisual()
	{
		return null;
	}

	private void ᠳ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	private void ᠴ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	protected virtual void OnInputHorizontalAlignmentChanged(eHorizontalAlignment oldValue, eHorizontalAlignment newValue)
	{
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_ᚾ.Equals((object?)_ᚽ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = _ᚽ;
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
		RecreateButtons();
	}

	protected virtual void RecreateButtons()
	{
		VisualItem[] items = ᠹ();
		if (!(__181C is VisualGroup))
		{
			return;
		}
		VisualGroup visualGroup = __181C as VisualGroup;
		VisualItem[] array = new VisualItem[visualGroup.Items.Count];
		visualGroup.Items._0CD0(array);
		VisualItem[] array2 = array;
		foreach (VisualItem visualItem in array2)
		{
			if (visualItem.ᡇ == _1885._1886)
			{
				visualGroup.Items.Remove(visualItem);
				if (visualItem == _ᠧ.ItemReference)
				{
					visualItem.Click -= ᠷ;
				}
				else if (visualItem == _ᠨ.ItemReference)
				{
					visualItem.Click -= ᠸ;
				}
				visualItem.IsRendered = _ᠠ;
			}
		}
		visualGroup.Items.AddRange(items);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		__181C.InvalidateArrange();
		RecreateButtons();
		base.ScaleControl(factor, specified);
	}

	private void ᠷ(object _٠, EventArgs ؿ)
	{
		OnButtonCustomClick(ؿ);
	}

	protected virtual void OnButtonCustomClick(EventArgs e)
	{
		if (_181E != null)
		{
			_181E(this, e);
		}
	}

	private void ᠸ(object _٠, EventArgs ؿ)
	{
		OnButtonCustom2Click(ؿ);
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
		SortedList sortedList = new SortedList(4);
		if (_ᠧ.Visible)
		{
			VisualItem visualItem = CreateButton(_ᠧ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.Click -= ᠷ;
			}
			_ᠧ.ItemReference = visualItem;
			visualItem.Click += ᠷ;
			visualItem.Enabled = _ᠧ.Enabled;
			sortedList.Add(_ᠧ, visualItem);
		}
		if (_ᠨ.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ᠨ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.Click -= ᠸ;
			}
			_ᠨ.ItemReference = visualItem2;
			visualItem2.Click += ᠸ;
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
		button.Alignment = eItemAlignment.Right;
		button.ᡇ = _1885._1886;
		button.Enabled = buttonSettings.Enabled;
		button.Shortcut = buttonSettings.Shortcut;
		button.Tooltip = buttonSettings.Tooltip;
		button.Symbol = buttonSettings.Symbol;
		button.SymbolSet = buttonSettings.SymbolSet;
		button.SymbolColor = buttonSettings.SymbolColor;
	}

	protected override void OnResize(EventArgs e)
	{
		ᠺ();
		__181C.InvalidateArrange();
		((Control)this).OnResize(e);
	}

	protected override PopupItem CreatePopupItem()
	{
		return new ButtonItem();
	}

	protected override void RecalcSize()
	{
	}

	public override void PerformClick()
	{
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
		__181C.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (_ᠩ != null)
		{
			_ᠩ.StyleChanged -= ઐ;
		}
		_ᠩ.Dispose();
		base.Dispose(disposing);
	}

	protected virtual int GetPreferredHeight()
	{
		if (_ᠪ > -1)
		{
			return _ᠪ;
		}
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		int num = 0;
		num = (_ᠪ = ((_ᠫ == 0 && (elementStyle.BorderTopWidth != 0 || elementStyle.BorderBottomWidth != 0)) ? (((Control)this).Font.Height + (elementStyle.BorderTopWidth * 2 + elementStyle.BorderBottomWidth * 2) + 3) : (((Control)this).Font.Height + ((_ᠫ != 0) ? _ᠫ : (SystemInformation.BorderSize.Height * 4 + 3)))));
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
		return num;
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_ᠪ = -1;
		((Control)this).Height = PreferredHeight;
		base.OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size result = new Size(0, GetPreferredHeight());
		if (!__181C.IsLayoutValid)
		{
			ᠱ();
		}
		result.Width = __181C.CalculatedSize.Width + 4;
		return result;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		ᠺ();
		base.OnHandleCreated(e);
	}

	private void ᠺ()
	{
		if (((Control)this).Height != PreferredHeight)
		{
			((Control)this).Height = PreferredHeight;
		}
	}

	protected virtual void ApplyFieldNavigation()
	{
		if (__181C is VisualInputGroup)
		{
			VisualInputGroup visualInputGroup = __181C as VisualInputGroup;
			visualInputGroup.ArrowNavigationEnabled = (_ᠬ & eInputFieldNavigation.Arrows) == eInputFieldNavigation.Arrows;
			visualInputGroup.TabNavigationEnabled = (_ᠬ & eInputFieldNavigation.Tab) == eInputFieldNavigation.Tab;
			visualInputGroup.EnterNavigationEnabled = (_ᠬ & eInputFieldNavigation.Enter) == eInputFieldNavigation.Enter;
		}
	}

	protected virtual bool IsNull(object value)
	{
		if (value == null || value is DBNull)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledBackColor()
	{
		return !_ᖣ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_ᛋ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	public bool SelectNextInputField()
	{
		if (__181C is VisualInputGroup visualInputGroup)
		{
			return visualInputGroup._22D3();
		}
		return false;
	}

	public bool SelectPreviousInputField()
	{
		if (__181C is VisualInputGroup visualInputGroup)
		{
			return visualInputGroup._22D4();
		}
		return false;
	}

	protected virtual Rectangle GetFreeTextBounds(Size preferedSize, Rectangle clientRect)
	{
		Rectangle empty = Rectangle.Empty;
		if (__181C == null)
		{
			return empty;
		}
		empty = clientRect;
		VisualGroup visualGroup = __181C as VisualGroup;
		foreach (VisualItem item in visualGroup.Items)
		{
			if (item.ᡇ == _1885._1886)
			{
				DisplayHelp.ഠ(ref empty, item.RenderBounds);
			}
		}
		if (empty.Width != clientRect.Width)
		{
			empty.Inflate(-1, 0);
		}
		if (preferedSize.Height > 0 && preferedSize.Height < empty.Height)
		{
			empty.Y += (empty.Height - preferedSize.Height) / 2;
			empty.Height = preferedSize.Height;
		}
		return empty;
	}

	protected virtual Control GetFreeTextBox()
	{
		throw new Exception("The method or operation is not implemented.");
	}

	protected virtual void HideFreeTextBoxEntry()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void BeginInit()
	{
		_ᠭ = true;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndInit()
	{
		_ᠭ = false;
	}

	[CompilerGenerated]
	private void ᠻ()
	{
		ᠱ();
	}
}
