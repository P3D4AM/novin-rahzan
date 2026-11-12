using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
public class VisualControlBase : PopupItemControl, IInputButtonControl, ISupportInitialize
{
	private VisualItem _RootVisual;

	private bool _FocusHighlightEnabled;

	private Color _LastBackColor = Color.Empty;

	private static Color _DefaultHighlightColor = Color.FromArgb(255, 255, 136);

	private Color _FocusHighlightColor = _DefaultHighlightColor;

	private bool _MouseOver;

	private bool _RenderControlButtons = true;

	private bool _CallBasePaintBackground = true;

	private bool _InputMouseWheelEnabled = true;

	private bool _IsKeyboardFocusWithin;

	private eHorizontalAlignment _InputHorizontalAlignment;

	private Font _WatermarkFont;

	private Color _WatermarkColor = SystemColors.GrayText;

	private bool _WatermarkEnabled = true;

	private string _WatermarkText = "";

	private eTextAlignment _WatermarkAlignment;

	private InputButtonSettings _ButtonCustom;

	private InputButtonSettings _ButtonCustom2;

	private ElementStyle _BackgroundStyle = new ElementStyle();

	private int _PreferredHeight = -1;

	private int _AutoBorderSize;

	private eInputFieldNavigation _FieldNavigation = eInputFieldNavigation.All;

	private Color _DisabledBackColor = Color.Empty;

	private Color _DisabledForeColor = Color.Empty;

	private InputControlColors _Colors;

	private bool _Initializing;

	internal bool RenderControlButtons
	{
		get
		{
			return _RenderControlButtons;
		}
		set
		{
			_RenderControlButtons = value;
			OnRenderControlButtonsChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool CallBasePaintBackground
	{
		get
		{
			return _CallBasePaintBackground;
		}
		set
		{
			_CallBasePaintBackground = value;
		}
	}

	protected virtual bool IsWatermarkRendered => false;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether mouse wheel affects the input control.")]
	public bool InputMouseWheelEnabled
	{
		get
		{
			return _InputMouseWheelEnabled;
		}
		set
		{
			_InputMouseWheelEnabled = value;
		}
	}

	[Browsable(false)]
	public bool IsKeyboardFocusWithin
	{
		get
		{
			return _IsKeyboardFocusWithin;
		}
		internal set
		{
			_IsKeyboardFocusWithin = value;
			OnIsKeyboardFocusWithinChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual VisualItem RootVisualItem => _RootVisual;

	[Browsable(true)]
	[DefaultValue(eHorizontalAlignment.Left)]
	[Category("Appearance")]
	[Description("Indicates alignment of input fields inside of the control.")]
	public virtual eHorizontalAlignment InputHorizontalAlignment
	{
		get
		{
			return _InputHorizontalAlignment;
		}
		set
		{
			if (_InputHorizontalAlignment != value)
			{
				eHorizontalAlignment inputHorizontalAlignment = _InputHorizontalAlignment;
				_InputHorizontalAlignment = value;
				_RootVisual.InvalidateArrange();
				OnInputHorizontalAlignmentChanged(inputHorizontalAlignment, value);
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public virtual Font WatermarkFont
	{
		get
		{
			return _WatermarkFont;
		}
		set
		{
			_WatermarkFont = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public virtual Color WatermarkColor
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

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed if set for the input items.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _WatermarkEnabled;
		}
		set
		{
			_WatermarkEnabled = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue("")]
	[Description("Indicates watermark text displayed on the input control when control is empty.")]
	[Category("Watermark")]
	[Localizable(true)]
	public string WatermarkText
	{
		get
		{
			return _WatermarkText;
		}
		set
		{
			if (value != null)
			{
				_WatermarkText = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eTextAlignment.Left)]
	[Description("Indicates watermark text alignment.")]
	[Category("Watermark")]
	public eTextAlignment WatermarkAlignment
	{
		get
		{
			return _WatermarkAlignment;
		}
		set
		{
			if (_WatermarkAlignment != value)
			{
				_WatermarkAlignment = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public virtual bool FocusHighlightEnabled
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
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	public virtual Color FocusHighlightColor
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
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _ButtonCustom;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _ButtonCustom2;

	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _BackgroundStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int PreferredHeight => GetPreferredHeight();

	internal int AutoBorderSize
	{
		get
		{
			return _AutoBorderSize;
		}
		set
		{
			_AutoBorderSize = value;
			_PreferredHeight = -1;
		}
	}

	protected override Size DefaultSize => new Size(80, GetPreferredHeight());

	[DefaultValue(eInputFieldNavigation.All)]
	[Description("Indicates keys used to navigate between the input fields provided by this control")]
	public eInputFieldNavigation FieldNavigation
	{
		get
		{
			return _FieldNavigation;
		}
		set
		{
			if (_FieldNavigation != value)
			{
				_FieldNavigation = value;
				ApplyFieldNavigation();
			}
		}
	}

	[Description("Indicates control background color when control is disabled")]
	[Category("Appearance")]
	public Color DisabledBackColor
	{
		get
		{
			return _DisabledBackColor;
		}
		set
		{
			if (_DisabledBackColor != value)
			{
				_DisabledBackColor = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Description("Indicates control background color when control is disabled")]
	[Category("Appearance")]
	public Color DisabledForeColor
	{
		get
		{
			return _DisabledForeColor;
		}
		set
		{
			if (_DisabledForeColor != value)
			{
				_DisabledForeColor = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Colors")]
	[Description("System colors used by the control.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputControlColors Colors => _Colors;

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

	protected virtual bool IsInitializing => _Initializing;

	public event EventHandler ButtonCustomClick;

	public event EventHandler ButtonCustom2Click;

	public VisualControlBase()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(SystemOptions.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).IsAccessible = true;
		_Colors = new InputControlColors();
		_Colors.ColorChanged += ColorsColorChanged;
		_ButtonCustom = new InputButtonSettings(this);
		_ButtonCustom2 = new InputButtonSettings(this);
		_RootVisual = CreateRootVisual();
		_RootVisual.ArrangeInvalid += VisualArrangeInvalid;
		_RootVisual.RenderInvalid += VisualRenderInvalid;
		_BackgroundStyle.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
		_BackgroundStyle.SetColorScheme(base.ColorScheme);
		_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		ApplyFieldNavigation();
	}

	public void ResetInputPosition()
	{
		if (_RootVisual is VisualInputBase)
		{
			((VisualInputBase)_RootVisual).ResetInputPosition();
		}
		else if (_RootVisual is VisualInputGroup)
		{
			((VisualInputGroup)_RootVisual).ResetInputPosition();
		}
	}

	private void ColorsColorChanged(object sender, EventArgs e)
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
		paintInfo.WatermarkColor = _WatermarkColor;
		paintInfo.WatermarkEnabled = _WatermarkEnabled;
		paintInfo.WatermarkFont = _WatermarkFont;
		paintInfo.AvailableSize = ((Control)this).ClientRectangle.Size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _MouseOver || ((Control)this).Focused;
		paintInfo.Colors = _Colors;
		if (!_DisabledForeColor.IsEmpty)
		{
			paintInfo.DisabledForeColor = _DisabledForeColor;
		}
		return paintInfo;
	}

	private void OnRenderControlButtonsChanged()
	{
		if (_RootVisual is VisualGroup)
		{
			VisualGroup visualGroup = (VisualGroup)_RootVisual;
			VisualItem[] array = new VisualItem[visualGroup.Items.Count];
			visualGroup.Items.CopyTo(array);
			VisualItem[] array2 = array;
			foreach (VisualItem visualItem in array2)
			{
				if (visualItem.ItemType == eSystemItemType.SystemButton || visualItem is VisualUpDownButton || visualItem is LockUpdateCheckBox)
				{
					visualItem.IsRendered = _RenderControlButtons;
				}
			}
		}
		((Control)this).Invalidate();
	}

	internal void InternalPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
	}

	private void PerformRootVisualLayout()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).BeginInvoke((Delegate)(MethodInvoker)delegate
			{
				PerformRootVisualLayout();
			});
			return;
		}
		Graphics val = CreateGraphics();
		try
		{
			PaintEventArgs val2 = new PaintEventArgs(val, Rectangle.Empty);
			PaintInfo paintInfo = CreatePaintInfo(val2);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			bool disposeStyle = false;
			ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
			if (backgroundStyle.Custom)
			{
				ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(backgroundStyle, val2.Graphics, clientRectangle);
				clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(backgroundStyle);
				clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(backgroundStyle);
				clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle);
				clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
				paintInfo.RenderOffset = clientRectangle.Location;
				paintInfo.AvailableSize = clientRectangle.Size;
			}
			_RootVisual.PerformLayout(paintInfo);
			val2.Dispose();
			if (disposeStyle)
			{
				backgroundStyle.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		PaintInfo paintInfo = CreatePaintInfo(e);
		bool enabled = ((Control)this).Enabled;
		if (!enabled)
		{
			Color color = _DisabledBackColor;
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
		else if (((Control)this).BackColor == Color.Transparent && _CallBasePaintBackground)
		{
			((Control)this).OnPaintBackground(e);
		}
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		if (backgroundStyle.Custom)
		{
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, clientRectangle);
			if (!enabled)
			{
				ElementStyleDisplay.PaintBorder(e2);
			}
			else
			{
				ElementStyleDisplay.Paint(e2);
			}
			clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(backgroundStyle);
			clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(backgroundStyle);
			clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle);
			clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
			paintInfo.RenderOffset = clientRectangle.Location;
			paintInfo.AvailableSize = clientRectangle.Size;
		}
		if (_FocusHighlightEnabled && ((Control)this).Focused && !_FocusHighlightColor.IsEmpty)
		{
			SolidBrush val2 = new SolidBrush(_FocusHighlightColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!_RootVisual.IsLayoutValid)
		{
			if (_RootVisual is VisualGroup)
			{
				((VisualGroup)_RootVisual).HorizontalItemAlignment = _InputHorizontalAlignment;
			}
			_RootVisual.PerformLayout(paintInfo);
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
		if (_InputHorizontalAlignment != 0)
		{
			if (_InputHorizontalAlignment == eHorizontalAlignment.Right)
			{
				paintInfo.RenderOffset = new Point(clientRectangle.Width - _RootVisual.Size.Width, (clientRectangle.Height - _RootVisual.Size.Height) / 2);
			}
			else
			{
				paintInfo.RenderOffset = new Point((clientRectangle.Width - _RootVisual.Size.Width) / 2, (clientRectangle.Height - _RootVisual.Size.Height) / 2);
			}
		}
		else
		{
			paintInfo.RenderOffset = new Point(0, (clientRectangle.Height - _RootVisual.Size.Height) / 2);
		}
		if (WatermarkEnabled && WatermarkText.Length > 0 && IsWatermarkRendered)
		{
			Rectangle r = clientRectangle;
			r.Inflate(-1, -1);
			DrawWatermark(paintInfo, r);
		}
		else
		{
			_RootVisual.ProcessPaint(paintInfo);
		}
		((Control)this).OnPaint(e);
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
	}

	private ElementStyle GetBackgroundStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		_BackgroundStyle.SetColorScheme(base.ColorScheme);
		return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
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
			if (_WatermarkAlignment == eTextAlignment.Center)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if (_WatermarkAlignment == eTextAlignment.Right)
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
			_RootVisual.ProcessKeyDown(e);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
		if (!e.Handled)
		{
			_RootVisual.ProcessKeyPress(e);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
		if (!e.Handled)
		{
			_RootVisual.ProcessKeyUp(e);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsFreeTextEntryVisible && _RootVisual.ProcessCmdKey(ref msg, keyData))
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
		_RootVisual.ProcessMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (_InputMouseWheelEnabled)
		{
			_RootVisual.ProcessMouseWheel(e);
		}
		((Control)this).OnMouseWheel(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		_RootVisual.ProcessMouseMove(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		_RootVisual.ProcessMouseUp(e);
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_MouseOver = true;
		((Control)this).Invalidate();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_MouseOver = false;
		_RootVisual.ProcessMouseLeave();
		((Control)this).Invalidate();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnClick(EventArgs e)
	{
		_RootVisual.ProcessClick();
		((Control)this).OnClick(e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		_RootVisual.ProcessMouseClick(e);
		((Control)this).OnMouseClick(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		_RootVisual.ProcessGotFocus();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		_RootVisual.ProcessLostFocus();
		((Control)this).OnLostFocus(e);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		if (SupportsFreeTextEntry && (keyData & 0x60000) == 0 && (keyData & 0x10000) == 65536 && IsFreeTextEntryVisible)
		{
			Keys val = (Keys)(keyData & 0xFFFF);
			if ((int)val == 9 && ProcessTabKey(forward: false))
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
			_RootVisual.ProcessLostFocus();
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

	private void VisualArrangeInvalid(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void VisualRenderInvalid(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	protected virtual void OnInputHorizontalAlignmentChanged(eHorizontalAlignment oldValue, eHorizontalAlignment newValue)
	{
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_FocusHighlightColor.Equals((object?)_DefaultHighlightColor);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = _DefaultHighlightColor;
	}

	void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		OnInputButtonSettingsChanged(inputButtonSettings);
	}

	protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		RecreateButtons();
	}

	protected virtual void RecreateButtons()
	{
		VisualItem[] items = CreateOrderedButtonList();
		if (!(_RootVisual is VisualGroup))
		{
			return;
		}
		VisualGroup visualGroup = _RootVisual as VisualGroup;
		VisualItem[] array = new VisualItem[visualGroup.Items.Count];
		visualGroup.Items.CopyTo(array);
		VisualItem[] array2 = array;
		foreach (VisualItem visualItem in array2)
		{
			if (visualItem.ItemType == eSystemItemType.SystemButton)
			{
				visualGroup.Items.Remove(visualItem);
				if (visualItem == _ButtonCustom.ItemReference)
				{
					visualItem.Click -= CustomButtonClick;
				}
				else if (visualItem == _ButtonCustom2.ItemReference)
				{
					visualItem.Click -= CustomButton2Click;
				}
				visualItem.IsRendered = _RenderControlButtons;
			}
		}
		visualGroup.Items.AddRange(items);
	}

	private void CustomButtonClick(object sender, EventArgs e)
	{
		OnButtonCustomClick(e);
	}

	protected virtual void OnButtonCustomClick(EventArgs e)
	{
		if (this.ButtonCustomClick != null)
		{
			this.ButtonCustomClick(this, e);
		}
	}

	private void CustomButton2Click(object sender, EventArgs e)
	{
		OnButtonCustom2Click(e);
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
		SortedList sortedList = new SortedList(4);
		if (_ButtonCustom.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonCustom);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.Click -= CustomButtonClick;
			}
			_ButtonCustom.ItemReference = visualItem;
			visualItem.Click += CustomButtonClick;
			visualItem.Enabled = _ButtonCustom.Enabled;
			sortedList.Add(_ButtonCustom, visualItem);
		}
		if (_ButtonCustom2.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonCustom2);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.Click -= CustomButton2Click;
			}
			_ButtonCustom2.ItemReference = visualItem2;
			visualItem2.Click += CustomButton2Click;
			visualItem2.Enabled = _ButtonCustom2.Enabled;
			sortedList.Add(_ButtonCustom2, visualItem2);
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
		button.ItemType = eSystemItemType.SystemButton;
		button.Enabled = buttonSettings.Enabled;
		button.Shortcut = buttonSettings.Shortcut;
	}

	protected override void OnResize(EventArgs e)
	{
		AutoAdjustHeight();
		_RootVisual.InvalidateArrange();
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
		_RootVisual.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (_BackgroundStyle != null)
		{
			_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		}
		_BackgroundStyle.Dispose();
		base.Dispose(disposing);
	}

	protected virtual int GetPreferredHeight()
	{
		if (_PreferredHeight > -1)
		{
			return _PreferredHeight;
		}
		bool disposeStyle = false;
		ElementStyle backgroundStyle = GetBackgroundStyle(out disposeStyle);
		int num = 0;
		num = (_PreferredHeight = ((_AutoBorderSize == 0 && (backgroundStyle.BorderTopWidth != 0 || backgroundStyle.BorderBottomWidth != 0)) ? (((Control)this).Font.Height + (backgroundStyle.BorderTopWidth * 2 + backgroundStyle.BorderBottomWidth * 2) + 3) : (((Control)this).Font.Height + ((_AutoBorderSize != 0) ? _AutoBorderSize : (SystemInformation.BorderSize.Height * 4 + 3)))));
		if (disposeStyle)
		{
			backgroundStyle.Dispose();
		}
		return num;
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_PreferredHeight = -1;
		((Control)this).Height = PreferredHeight;
		base.OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size result = new Size(0, GetPreferredHeight());
		if (!_RootVisual.IsLayoutValid)
		{
			PerformRootVisualLayout();
		}
		result.Width = _RootVisual.CalculatedSize.Width + 4;
		return result;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		AutoAdjustHeight();
		base.OnHandleCreated(e);
	}

	private void AutoAdjustHeight()
	{
		if (((Control)this).Height != PreferredHeight)
		{
			((Control)this).Height = PreferredHeight;
		}
	}

	protected virtual void ApplyFieldNavigation()
	{
		if (_RootVisual is VisualInputGroup)
		{
			VisualInputGroup visualInputGroup = _RootVisual as VisualInputGroup;
			visualInputGroup.ArrowNavigationEnabled = (_FieldNavigation & eInputFieldNavigation.Arrows) == eInputFieldNavigation.Arrows;
			visualInputGroup.TabNavigationEnabled = (_FieldNavigation & eInputFieldNavigation.Tab) == eInputFieldNavigation.Tab;
			visualInputGroup.EnterNavigationEnabled = (_FieldNavigation & eInputFieldNavigation.Enter) == eInputFieldNavigation.Enter;
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
		return !_DisabledBackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_DisabledForeColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	public bool SelectNextInputField()
	{
		if (_RootVisual is VisualInputGroup visualInputGroup)
		{
			return visualInputGroup.SelectNextInput();
		}
		return false;
	}

	public bool SelectPreviousInputField()
	{
		if (_RootVisual is VisualInputGroup visualInputGroup)
		{
			return visualInputGroup.SelectPreviousInput();
		}
		return false;
	}

	protected virtual Rectangle GetFreeTextBounds(Size preferedSize, Rectangle clientRect)
	{
		Rectangle empty = Rectangle.Empty;
		if (_RootVisual == null)
		{
			return empty;
		}
		empty = clientRect;
		VisualGroup visualGroup = _RootVisual as VisualGroup;
		foreach (VisualItem item in visualGroup.Items)
		{
			if (item.ItemType == eSystemItemType.SystemButton)
			{
				DisplayHelp.ExcludeEdgeRect(ref empty, item.RenderBounds);
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
		_Initializing = true;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndInit()
	{
		_Initializing = false;
	}
}
