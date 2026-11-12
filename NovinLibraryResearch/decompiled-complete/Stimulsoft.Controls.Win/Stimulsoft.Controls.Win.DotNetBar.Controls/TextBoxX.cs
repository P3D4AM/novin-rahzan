using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(TextBoxX), "Controls.TextBoxX.ico")]
[ToolboxItem(false)]
public class TextBoxX : TextBox, INonClientControl, IInputButtonControl
{
	private NonClientPaintHandler m_NCPainter;

	private string m_WatermarkText = "";

	private bool m_Focused;

	private Font m_WatermarkFont;

	private Color m_WatermarkColor = SystemColors.GrayText;

	private ElementStyle m_BorderStyle;

	private IntPtr m_LastFocusWindow;

	private string m_OriginalText;

	private bool m_IsTextBoxItem;

	private Color m_FocusHighlightColor = ColorScheme.GetColor(16777096);

	private bool m_FocusHighlightEnabled;

	private Color m_LastBackColor = Color.Empty;

	private bool m_IgnoreFocus;

	private bool _AutoSelectAll;

	private bool _PreventEnterBeep;

	[CompilerGenerated]
	private KeyEventHandler m_CommandKeyDown;

	private BodyElement m_TextMarkup;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool _WatermarkEnabled = true;

	private Image _WatermarkImage;

	private ContentAlignment _WatermarkImageAlignment = (ContentAlignment)16;

	private eWatermarkBehavior _WatermarkBehavior;

	private InputButtonSettings _ButtonCustom;

	private InputButtonSettings _ButtonCustom2;

	private VisualGroup _ButtonGroup;

	private bool _MouseOver;

	private Cursor _ControlCursor;

	private bool _InternalCursorChange;

	private ColorScheme m_ColorScheme;

	private bool RenderWatermark
	{
		get
		{
			if (!_WatermarkEnabled)
			{
				return false;
			}
			if (_WatermarkBehavior == eWatermarkBehavior.HideOnFocus)
			{
				if (!m_Focused && ((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
				{
					if (m_WatermarkText.Length <= 0)
					{
						return _WatermarkImage != null;
					}
					return true;
				}
				return false;
			}
			if (((Control)this).Enabled && ((Control)this).Text != null && ((Control)this).Text.Length == 0)
			{
				if (m_WatermarkText.Length <= 0)
				{
					return _WatermarkImage != null;
				}
				return true;
			}
			return false;
		}
	}

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
	[Category("Behavior")]
	[Description("Gets or sets whether control prevents Beep sound when Enter key is pressed.")]
	public bool PreventEnterBeep
	{
		get
		{
			return _PreventEnterBeep;
		}
		set
		{
			_PreventEnterBeep = value;
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
			return m_FocusHighlightEnabled;
		}
		set
		{
			if (m_FocusHighlightEnabled != value)
			{
				m_FocusHighlightEnabled = value;
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
	public Color FocusHighlightColor
	{
		get
		{
			return m_FocusHighlightColor;
		}
		set
		{
			if (m_FocusHighlightColor != value)
			{
				m_FocusHighlightColor = value;
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
			return m_NCPainter.SkinScrollbars;
		}
		set
		{
			m_NCPainter.SkinScrollbars = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies the control border style. Default value has Class property set so the system style for the control is used.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle Border => m_BorderStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return m_RenderMode;
		}
		set
		{
			if (m_RenderMode != value)
			{
				m_RenderMode = value;
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
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
		}
	}

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
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates watermark image displayed inside of the control when Text is not set and control does not have input focus.")]
	public Image WatermarkImage
	{
		get
		{
			return _WatermarkImage;
		}
		set
		{
			_WatermarkImage = value;
			((Control)this).Invalidate();
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
			return _WatermarkImageAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_WatermarkImageAlignment = value;
			((Control)this).Invalidate();
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
			return m_WatermarkText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_WatermarkText = value;
			MarkupTextChanged();
			((Control)this).Invalidate();
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
			return m_WatermarkFont;
		}
		set
		{
			m_WatermarkFont = value;
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
			return m_WatermarkColor;
		}
		set
		{
			m_WatermarkColor = value;
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

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom => _ButtonCustom;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _ButtonCustom2;

	private bool RenderButtons
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((TextBox)this).ScrollBars == 0)
			{
				if (_ButtonCustom == null || !_ButtonCustom.Visible)
				{
					if (_ButtonCustom2 != null)
					{
						return _ButtonCustom2.Visible;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	ElementStyle INonClientControl.BorderStyle
	{
		get
		{
			bool disposeStyle = false;
			return GetBorderStyle(out disposeStyle);
		}
	}

	IntPtr INonClientControl.Handle => ((Control)this).Handle;

	int INonClientControl.Width => ((Control)this).Width;

	int INonClientControl.Height => ((Control)this).Height;

	bool INonClientControl.IsHandleCreated => ((Control)this).IsHandleCreated;

	Color INonClientControl.BackColor => ((Control)this).BackColor;

	public event EventHandler ButtonCustomClick;

	public event EventHandler ButtonCustom2Click;

	[Description("Occurs during preprocessing to handle command keys. Command keys are keys that always take precedence over regular input keys. Examples of command keys include accelerators and menu shortcuts. Set Handled=true to indicate that you handled the key and that it should not be passed for further processing.")]
	public event KeyEventHandler CommandKeyDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_CommandKeyDown;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_CommandKeyDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyEventHandler val = this.m_CommandKeyDown;
			KeyEventHandler val2;
			do
			{
				val2 = val;
				KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_CommandKeyDown, value2, val2);
			}
			while (val != val2);
		}
	}

	public TextBoxX()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		_ButtonCustom = new InputButtonSettings(this);
		_ButtonCustom2 = new InputButtonSettings(this);
		CreateButtonGroup();
		m_BorderStyle = new ElementStyle();
		m_BorderStyle.StyleChanged += BorderStyle_StyleChanged;
		m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
		((Control)this).AutoSize = false;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (newStyle == eDotNetBarStyle.Metro || (StyleManager.PreviousStyle == eStyle.Metro && newStyle != eDotNetBarStyle.Metro))
		{
			StyleManager.UpdateAmbientColors((Control)(object)this);
		}
	}

	internal TextBoxX(bool isTextBoxItem)
		: this()
	{
		m_IsTextBoxItem = isTextBoxItem;
	}

	protected override void Dispose(bool disposing)
	{
		if (m_NCPainter != null)
		{
			m_NCPainter.Dispose();
		}
		if (m_BorderStyle != null)
		{
			m_BorderStyle.StyleChanged -= BorderStyle_StyleChanged;
		}
		StyleManager.Unregister((Control)(object)this);
		((TextBox)this).Dispose(disposing);
	}

	protected override void WndProc(ref Message m)
	{
		if (m_NCPainter != null)
		{
			if (m_NCPainter.WndProc(ref m))
			{
				((TextBox)this).WndProc(ref m);
			}
		}
		else
		{
			((TextBox)this).WndProc(ref m);
		}
		if (RenderButtons)
		{
			int msg = ((Message)(ref m)).Msg;
			if (msg == 132 || msg == 160)
			{
				Point pt = ((Control)this).PointToClient(new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam)));
				if (_ButtonGroup.RenderBounds.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(1);
					return;
				}
			}
		}
		switch (((Message)(ref m)).Msg)
		{
		case 7:
			if (m_IgnoreFocus)
			{
				m_IgnoreFocus = false;
				break;
			}
			m_Focused = true;
			m_LastFocusWindow = ((Message)(ref m)).WParam;
			m_OriginalText = ((Control)this).Text;
			if (FocusHighlightEnabled && ((Control)this).Enabled)
			{
				m_LastBackColor = ((Control)this).BackColor;
				((Control)this).BackColor = FocusHighlightColor;
				InvalidateNonClient();
			}
			break;
		case 8:
			if (!m_Focused)
			{
				m_IgnoreFocus = true;
				break;
			}
			m_Focused = false;
			if (((Control)this).Text.Length == 0)
			{
				((Control)this).Invalidate();
			}
			if (FocusHighlightEnabled && !m_LastBackColor.IsEmpty)
			{
				((Control)this).BackColor = m_LastBackColor;
				InvalidateNonClient();
			}
			break;
		case 15:
			if (RenderWatermark)
			{
				DrawWatermark();
			}
			if (((Control)this).Parent is ItemControl)
			{
				((ItemControl)(object)((Control)this).Parent).UpdateKeyTipsCanvas();
			}
			break;
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		_ButtonGroup.ProcessKeyPress(e);
		if (!((TextBoxBase)this).Multiline && _PreventEnterBeep && e.KeyChar == '\r')
		{
			e.Handled = true;
		}
		((Control)this).OnKeyPress(e);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !m_FocusHighlightColor.Equals((object?)ColorScheme.GetColor(16777096));
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = ColorScheme.GetColor(16777096);
	}

	internal void ReleaseFocus()
	{
		if (!((Control)this).Focused || !(m_LastFocusWindow != IntPtr.Zero))
		{
			return;
		}
		IntPtr lastFocusWindow = m_LastFocusWindow;
		m_LastFocusWindow = IntPtr.Zero;
		Control val = Control.FromChildHandle(lastFocusWindow);
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
			NativeFunctions.SetFocus(lastFocusWindow);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		_ButtonGroup.ProcessKeyUp(e);
		((Control)this).OnKeyUp(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		_ButtonGroup.ProcessKeyDown(e);
		((Control)this).OnKeyDown(e);
		if (m_IsTextBoxItem && (int)e.KeyCode == 27)
		{
			((Control)this).Text = m_OriginalText;
			((TextBoxBase)this).SelectionStart = 0;
			ReleaseFocus();
		}
	}

	protected virtual void OnCommandKeyDown(KeyEventArgs e)
	{
		KeyEventHandler commandKeyDown = this.CommandKeyDown;
		if (commandKeyDown != null)
		{
			commandKeyDown.Invoke((object)this, e);
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
		if (_ButtonGroup.ProcessCmdKey(ref msg, keyData))
		{
			return true;
		}
		if (this.CommandKeyDown != null)
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
		if (_AutoSelectAll)
		{
			((TextBoxBase)this).SelectAll();
		}
		((TextBox)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		m_LastFocusWindow = IntPtr.Zero;
		((Control)this).OnLostFocus(e);
	}

	private void BorderStyle_StyleChanged(object sender, EventArgs e)
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
			NativeFunctions.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
			SetAutoHeight();
			NativeFunctions.RECT lprcUpdate = new NativeFunctions.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			NativeFunctions.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	private void MarkupTextChanged()
	{
		m_TextMarkup = null;
		if (MarkupParser.IsMarkup(ref m_WatermarkText))
		{
			m_TextMarkup = MarkupParser.Parse(m_WatermarkText);
			ResizeMarkup();
		}
	}

	private void ResizeMarkup()
	{
		if (m_TextMarkup != null)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = GetMarkupDrawContext(val);
				m_TextMarkup.Measure(GetWatermarkBounds().Size, markupDrawContext);
				Size size = m_TextMarkup.Bounds.Size;
				m_TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, size), markupDrawContext);
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

	private void DrawWatermark()
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
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Invalid comparison between Unknown and I4
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Invalid comparison between Unknown and I4
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Invalid comparison between Unknown and I4
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
			Rectangle watermarkBounds = GetWatermarkBounds();
			if (_WatermarkImage != null)
			{
				Rectangle rectangle = new Rectangle(Point.Empty, _WatermarkImage.Size);
				if ((int)_WatermarkImageAlignment == 512)
				{
					rectangle.Location = new Point(watermarkBounds.X + (watermarkBounds.Width - rectangle.Width) / 2, watermarkBounds.Bottom - rectangle.Height);
				}
				else if ((int)_WatermarkImageAlignment == 256)
				{
					rectangle.Location = new Point(watermarkBounds.X, watermarkBounds.Bottom - rectangle.Height);
				}
				else if ((int)_WatermarkImageAlignment == 1024)
				{
					rectangle.Location = new Point(watermarkBounds.Right - rectangle.Width, watermarkBounds.Bottom - rectangle.Height);
				}
				else if ((int)_WatermarkImageAlignment == 32)
				{
					rectangle.Location = new Point(watermarkBounds.X + (watermarkBounds.Width - rectangle.Width) / 2, watermarkBounds.Y + (watermarkBounds.Height - rectangle.Height) / 2);
				}
				else if ((int)_WatermarkImageAlignment == 16)
				{
					rectangle.Location = new Point(watermarkBounds.X, watermarkBounds.Y + (watermarkBounds.Height - rectangle.Height) / 2);
				}
				else if ((int)_WatermarkImageAlignment == 64)
				{
					rectangle.Location = new Point(watermarkBounds.Right - rectangle.Width, watermarkBounds.Y + (watermarkBounds.Height - rectangle.Height) / 2);
				}
				else if ((int)_WatermarkImageAlignment == 2)
				{
					rectangle.Location = new Point(watermarkBounds.X + (watermarkBounds.Width - rectangle.Width) / 2, watermarkBounds.Y);
				}
				else if ((int)_WatermarkImageAlignment == 1)
				{
					rectangle.Location = new Point(watermarkBounds.X, watermarkBounds.Y);
				}
				else if ((int)_WatermarkImageAlignment == 4)
				{
					rectangle.Location = new Point(watermarkBounds.Right - rectangle.Width, watermarkBounds.Y);
				}
				val.DrawImage(_WatermarkImage, rectangle);
				if ((int)_WatermarkImageAlignment == 256 || (int)_WatermarkImageAlignment == 16 || (int)_WatermarkImageAlignment == 1)
				{
					watermarkBounds.X = rectangle.Right;
					watermarkBounds.Width = Math.Max(0, watermarkBounds.Width - rectangle.Width);
				}
				else if ((int)_WatermarkImageAlignment == 1024 || (int)_WatermarkImageAlignment == 64 || (int)_WatermarkImageAlignment == 4)
				{
					watermarkBounds.Width = Math.Max(0, watermarkBounds.Width - rectangle.Width);
				}
			}
			if (watermarkBounds.Width <= 0)
			{
				return;
			}
			if (m_TextMarkup != null)
			{
				MarkupDrawContext markupDrawContext = GetMarkupDrawContext(val);
				m_TextMarkup.Render(markupDrawContext);
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
				eTextFormat |= eTextFormat.Default;
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
			TextDrawing.DrawString(val, m_WatermarkText, (m_WatermarkFont == null) ? ((Control)this).Font : m_WatermarkFont, m_WatermarkColor, watermarkBounds, eTextFormat);
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
		if (((Control)this).AutoSize || ((TextBoxBase)this).Multiline || (int)BorderStyle != 0 || ((Control)this).IsDisposed || ((Control)this).Parent == null || m_IsTextBoxItem)
		{
			return;
		}
		int num = ((Control)this).FontHeight;
		if (((Control)this).Font != null)
		{
			num = Math.Max(num, ((Control)this).Font.Height);
		}
		num++;
		bool disposeStyle = false;
		ElementStyle borderStyle = GetBorderStyle(out disposeStyle);
		if (borderStyle != null)
		{
			if (borderStyle.PaintTopBorder)
			{
				num = ((borderStyle.CornerType != eCornerType.Rounded && borderStyle.CornerType != eCornerType.Diagonal) ? (num + borderStyle.BorderTopWidth) : (num + Math.Max(borderStyle.BorderTopWidth, borderStyle.CornerDiameter / 2 + 1)));
			}
			if (borderStyle.PaintBottomBorder)
			{
				num = ((borderStyle.CornerType != eCornerType.Rounded && borderStyle.CornerType != eCornerType.Diagonal) ? (num + borderStyle.BorderBottomWidth) : (num + Math.Max(borderStyle.BorderBottomWidth, borderStyle.CornerDiameter / 2 + 1)));
			}
			num += borderStyle.PaddingTop + borderStyle.PaddingBottom;
			if (disposeStyle)
			{
				borderStyle.Dispose();
			}
		}
		if (_ButtonCustom != null && _ButtonCustom.Visible && _ButtonCustom.Image != null)
		{
			num = Math.Max(num, _ButtonCustom.Image.Height + 6);
		}
		if (_ButtonCustom2 != null && _ButtonCustom2.Visible && _ButtonCustom2.Image != null)
		{
			num = Math.Max(num, _ButtonCustom2.Image.Height + 6);
		}
		((Control)this).Height = num;
	}

	protected override void OnResize(EventArgs e)
	{
		ResizeMarkup();
		_ButtonGroup.InvalidateArrange();
		((Control)this).OnResize(e);
	}

	private MarkupDrawContext GetMarkupDrawContext(Graphics g)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(g, (m_WatermarkFont == null) ? ((Control)this).Font : m_WatermarkFont, m_WatermarkColor, (int)((Control)this).RightToLeft == 1);
	}

	protected override void OnTextAlignChanged(EventArgs e)
	{
		if (m_WatermarkText.Length > 0)
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
		if (m_WatermarkText.Length > 0)
		{
			((Control)this).Invalidate();
		}
		((Control)this).OnEnabledChanged(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (m_WatermarkText.Length > 0)
		{
			((Control)this).Invalidate();
		}
		((TextBoxBase)this).OnTextChanged(e);
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (m_RenderMode == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
		{
			return m_Renderer;
		}
		if (m_DefaultRenderer == null)
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_DefaultRenderer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return m_WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	private ElementStyle GetBorderStyle(out bool disposeStyle)
	{
		disposeStyle = false;
		m_BorderStyle.SetColorScheme(GetColorScheme());
		return ElementStyleDisplay.GetElementStyle(m_BorderStyle, out disposeStyle);
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
		InvalidateNonClient();
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

	public void PerformButtonCustomClick()
	{
		OnButtonCustomClick(EventArgs.Empty);
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

	public void PerformButtonCustom2Click()
	{
		OnButtonCustom2Click(EventArgs.Empty);
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

	private void CustomButtonKeyClick(object sender, EventArgs e)
	{
		if (e is KeyEventArgs)
		{
			OnButtonCustomClick(e);
		}
	}

	private void CustomButton2KeyClick(object sender, EventArgs e)
	{
		if (e is KeyEventArgs)
		{
			OnButtonCustom2Click(e);
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
		if (_ButtonCustom.Visible)
		{
			VisualItem visualItem = CreateButton(_ButtonCustom);
			if (_ButtonCustom.ItemReference != null)
			{
				_ButtonCustom.ItemReference.MouseUp -= new MouseEventHandler(CustomButtonClick);
				_ButtonCustom.ItemReference.Click -= CustomButtonKeyClick;
			}
			_ButtonCustom.ItemReference = visualItem;
			visualItem.Click += CustomButtonKeyClick;
			visualItem.MouseUp += new MouseEventHandler(CustomButtonClick);
			visualItem.Enabled = _ButtonCustom.Enabled;
			sortedList.Add(_ButtonCustom, visualItem);
		}
		if (_ButtonCustom2.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ButtonCustom2);
			if (_ButtonCustom2.ItemReference != null)
			{
				_ButtonCustom2.ItemReference.MouseUp -= new MouseEventHandler(CustomButton2Click);
				_ButtonCustom2.ItemReference.Click -= CustomButton2KeyClick;
			}
			_ButtonCustom2.ItemReference = visualItem2;
			visualItem2.Click += CustomButton2KeyClick;
			visualItem2.MouseUp += new MouseEventHandler(CustomButton2Click);
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
		InvalidateNonClient();
	}

	private void ButtonGroupArrangeInvalid(object sender, EventArgs e)
	{
		InvalidateNonClient();
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
		ElementStyle borderStyle = GetBorderStyle(out disposeStyle);
		size.Height -= ElementStyleLayout.TopWhiteSpace(borderStyle, eSpacePart.Border) + ElementStyleLayout.BottomWhiteSpace(borderStyle, eSpacePart.Border) + 2;
		size.Width -= ElementStyleLayout.LeftWhiteSpace(borderStyle, eSpacePart.Border) + ElementStyleLayout.RightWhiteSpace(borderStyle, eSpacePart.Border) + 2;
		paintInfo.AvailableSize = size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _MouseOver || ((Control)this).Focused;
		if (disposeStyle)
		{
			borderStyle.Dispose();
		}
		return paintInfo;
	}

	private void PaintButtons(Graphics g)
	{
		PaintInfo paintInfo = CreatePaintInfo(g);
		if (!_ButtonGroup.IsLayoutValid)
		{
			_ButtonGroup.PerformLayout(paintInfo);
		}
		bool disposeStyle = false;
		ElementStyle borderStyle = GetBorderStyle(out disposeStyle);
		_ButtonGroup.RenderBounds = new Rectangle(((Control)this).Width - (ElementStyleLayout.RightWhiteSpace(borderStyle, eSpacePart.Border) + 1) - _ButtonGroup.Size.Width, ElementStyleLayout.TopWhiteSpace(borderStyle, eSpacePart.Border) + 1, _ButtonGroup.Size.Width, _ButtonGroup.Size.Height);
		_ButtonGroup.ProcessPaint(paintInfo);
		if (disposeStyle)
		{
			borderStyle.Dispose();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseMove(e);
			if (_ButtonGroup.RenderBounds.Contains(e.X, e.Y))
			{
				if (_ControlCursor == (Cursor)null)
				{
					_InternalCursorChange = true;
					_ControlCursor = ((Control)this).Cursor;
					((Control)this).Cursor = Cursors.Arrow;
					_InternalCursorChange = false;
				}
			}
			else if (_ControlCursor != (Cursor)null)
			{
				_InternalCursorChange = true;
				((Control)this).Cursor = _ControlCursor;
				_ControlCursor = null;
				_InternalCursorChange = false;
			}
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_MouseOver = true;
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_MouseOver = false;
		if (RenderButtons)
		{
			_ButtonGroup.ProcessMouseLeave();
			if (_ControlCursor != (Cursor)null)
			{
				_InternalCursorChange = true;
				((Control)this).Cursor = _ControlCursor;
				_ControlCursor = null;
				_InternalCursorChange = false;
			}
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnCursorChanged(EventArgs e)
	{
		if (_ControlCursor != (Cursor)null && !_InternalCursorChange)
		{
			_ControlCursor = ((Control)this).Cursor;
		}
		((Control)this).OnCursorChanged(e);
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
		((TextBoxBase)this).OnMouseUp(e);
	}

	void INonClientControl.BaseWndProc(ref Message m)
	{
		((TextBox)this).WndProc(ref m);
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && WinApi.IsGlassEnabled;
		return itemPaintArgs;
	}

	void INonClientControl.PaintBackground(PaintEventArgs e)
	{
		if (((Control)this).Parent != null)
		{
			Type typeFromHandle = typeof(Control);
			MethodInfo method = typeFromHandle.GetMethod("PaintTransparentBackground", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[2]
			{
				typeof(PaintEventArgs),
				typeof(Rectangle)
			}, null);
			if (method != null)
			{
				method.Invoke(this, new object[2]
				{
					e,
					new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)
				});
			}
		}
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (m_ColorScheme == null)
		{
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return m_ColorScheme;
	}

	Point INonClientControl.PointToScreen(Point client)
	{
		return ((Control)this).PointToScreen(client);
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle r)
	{
		if (!RenderButtons)
		{
			return;
		}
		if (!_ButtonGroup.IsLayoutValid)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				PaintInfo pi = CreatePaintInfo(val);
				_ButtonGroup.PerformLayout(pi);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		r.Width -= _ButtonGroup.Size.Width + 1;
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle r)
	{
	}

	void INonClientControl.RenderNonClient(Graphics g)
	{
		if (RenderButtons)
		{
			PaintButtons(g);
		}
	}

	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	void INonClientControl.set_Enabled(bool value)
	{
		((Control)this).Enabled = value;
	}
}
