using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[DefaultEvent("Click")]
[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.PanelControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class PanelControl : Panel, IButtonControl
{
	private ElementStyle Ӷ;

	private ElementStyle Ӱ;

	private ElementStyle ݔ;

	private ColorScheme լ;

	private eDotNetBarStyle ᑨ = eDotNetBarStyle.Office2003;

	private bool ݡ;

	private bool ݠ;

	private Rectangle ᑩ = Rectangle.Empty;

	private bool ᑪ = true;

	private bool ᑫ;

	private ᑧ ᑬ;

	private DialogResult ᑭ;

	private bool ᑮ;

	private bool ե = true;

	private Color ᑯ = Color.White;

	protected Bitmap m_ThemeCachedBitmap;

	private bool _055D;

	private _0CB4 ݶ;

	private MarkupLinkClickEventHandler Ӟ;

	private eCornerType ᑰ = eCornerType.Square;

	private int _߃ = 8;

	internal bool ᑱ = true;

	private bool _0DD3;

	internal bool ᑲ;

	internal _0CB4 ᑥ => ݶ;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).BorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).BorderStyle = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[Description("Gets or sets Bar Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			լ = value;
			OnColorSchemeChanged();
			if (((Control)this).Visible)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Indicates whether focus rectangle is displayed when control has focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return ᑫ;
		}
		set
		{
			ᑫ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Gets or sets the canvas color.")]
	[Category("Background")]
	[Browsable(true)]
	public Color CanvasColor
	{
		get
		{
			return ᑯ;
		}
		set
		{
			ᑯ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates panel style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ElementStyle Style => Ӷ;

	[NotifyParentProperty(true)]
	[Description("Gets or sets the panel style when mouse hovers over the panel.")]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public virtual ElementStyle StyleMouseOver => Ӱ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the panel style when mouse button is pressed on the panel.")]
	[Browsable(true)]
	[Category("Style")]
	[NotifyParentProperty(true)]
	public virtual ElementStyle StyleMouseDown => ݔ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	internal eDotNetBarStyle ᑦ
	{
		get
		{
			if (ᑨ == eDotNetBarStyle.StyleManagerControlled)
			{
				return StyleManager.GetEffectiveStyle();
			}
			return ᑨ;
		}
	}

	[Category("Style")]
	[Description("Gets or sets color scheme style.")]
	[Browsable(true)]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	[DevCoBrowsable(true)]
	public eDotNetBarStyle ColorSchemeStyle
	{
		get
		{
			return ᑨ;
		}
		set
		{
			ᑨ = value;
			լ = new ColorScheme(ᑦ);
			OnColorSchemeChanged();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get
		{
			return ((Panel)this).Text;
		}
		set
		{
			((Panel)this).Text = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	[DefaultValue(true)]
	public virtual bool TextDockConstrained
	{
		get
		{
			return ᑪ;
		}
		set
		{
			ᑪ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle ClientTextRectangle
	{
		get
		{
			return ᑩ;
		}
		set
		{
			ᑩ = value;
		}
	}

	protected virtual bool SuspendPaint
	{
		get
		{
			return _055D;
		}
		set
		{
			_055D = value;
		}
	}

	[Description("Gets or sets the value returned to the parent form when the button is clicked.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Category("Behavior")]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᑭ;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Enum.IsDefined(typeof(DialogResult), value))
			{
				ᑭ = value;
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[DevCoBrowsable(true)]
	public virtual bool ThemeAware
	{
		get
		{
			return ᑲ;
		}
		set
		{
			DisposeThemeCachedBitmap();
			ᑲ = value;
			((Control)this).Invalidate();
		}
	}

	internal ᑧ ᑧ
	{
		get
		{
			if (ᑬ == null)
			{
				ᑬ = new ᑧ((Control)(object)this);
			}
			return ᑬ;
		}
	}

	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public PanelControl()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)4096, false);
		լ = new ColorScheme(ᑨ);
		ResetStyle();
		ResetStyleMouseOver();
		ResetStyleMouseDown();
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		OnColorSchemeChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	internal ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(ᑨ) && GlobalManager.Renderer is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return լ;
	}

	private void ᑳ(object _٠, EventArgs ؿ)
	{
		RefreshStyleSystemColors();
		SetRegion();
		((Control)this).Invalidate();
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		((Control)this).Invalidate();
		((ScrollableControl)this).OnRightToLeftChanged(e);
	}

	protected virtual void SetRegion()
	{
		ᑴ(ᑵ: false);
	}

	private void ᑴ(bool ᑵ)
	{
		if (!ᑱ)
		{
			return;
		}
		ElementStyle style = GetStyle();
		if (ᑵ || ᑰ != style.CornerType || _߃ != style.CornerDiameter)
		{
			ᑰ = style.CornerType;
			_߃ = style.CornerDiameter;
			if (style != null && (!ᑲ || !BarFunctions.ThemedOS) && style.CornerType != eCornerType.Square)
			{
				((Control)this).Region = ElementStyleDisplay.GetStyleRegion(new ElementStyleDisplayInfo(style, null, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)));
			}
			else
			{
				((Control)this).Region = null;
			}
		}
	}

	public void RefreshStyleSystemColors()
	{
		ColorScheme colorScheme = GetColorScheme();
		if (Ӷ != null)
		{
			Ӷ.SetColorScheme(colorScheme);
		}
		if (Ӱ != null)
		{
			Ӱ.SetColorScheme(colorScheme);
		}
		if (ݔ != null)
		{
			ݔ.SetColorScheme(colorScheme);
		}
	}

	protected override void WndProc(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 276:
		case 277:
		case 522:
			if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
			{
				RefreshTextClientRectangle();
				((Control)this).Invalidate();
				break;
			}
			((ScrollableControl)this).WndProc(ref m);
			((Control)this).Refresh();
			return;
		}
		((ScrollableControl)this).WndProc(ref m);
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (ե)
		{
			val.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		return val;
	}

	protected virtual void BaseWndProc(ref Message m)
	{
		((ScrollableControl)this).WndProc(ref m);
	}

	protected virtual ElementStyle GetStyle()
	{
		return Ӷ;
	}

	protected virtual ElementStyle GetStyleMouseDown()
	{
		return ݔ;
	}

	protected virtual ElementStyle GetStyleMouseOver()
	{
		return Ӱ;
	}

	protected virtual void PaintThemed(PaintEventArgs e)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ((Control)this).ClientRectangle.Size)
		{
			DisposeThemeCachedBitmap();
			Bitmap val = new Bitmap(((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height, e.Graphics);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				ᑧ._0EE5(val2, ᑷ.ᒀ, ᒂ.ཊ, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height));
			}
			finally
			{
				val2.Dispose();
			}
			e.Graphics.DrawImage((Image)(object)val, 0, 0, ((Image)val).Width, ((Image)val).Height);
			ElementStyle style = GetStyle();
			if (style.BackgroundImage != null)
			{
				BarFunctions._0982(e.Graphics, ((Control)this).ClientRectangle, style.BackgroundImage, style.BackgroundImagePosition, style.BackgroundImageAlpha);
			}
			m_ThemeCachedBitmap = val;
		}
		else
		{
			e.Graphics.DrawImage((Image)(object)m_ThemeCachedBitmap, 0, 0, ((Image)m_ThemeCachedBitmap).Width, ((Image)m_ThemeCachedBitmap).Height);
		}
	}

	protected virtual void PaintPrepare(PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		SolidBrush val = new SolidBrush(ᑯ);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		ElementStyle style = GetStyle();
		if ((style.BackColor.IsEmpty || style.BackColor.A < byte.MaxValue) && (style.BackColor2.IsEmpty || style.BackColor2.A < byte.MaxValue))
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		if (ե)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
	}

	protected virtual void PaintStyled(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		using ElementStyle elementStyle = GetStyle().Copy();
		if (ݠ && ((Control)this).Enabled)
		{
			elementStyle.ApplyStyle(GetStyleMouseDown());
		}
		else if (ݡ && ((Control)this).Enabled)
		{
			elementStyle.ApplyStyle(GetStyleMouseOver());
		}
		PaintInnerContent(e, elementStyle, paintText: true);
		if (((Control)this).Focused && ᑫ)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			if (clientRectangle.Width > 0 && clientRectangle.Height > 0)
			{
				ControlPaint.DrawFocusRectangle(graphics, clientRectangle);
			}
		}
	}

	protected virtual void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		Graphics graphics = e.Graphics;
		if (ݶ == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Rectangle bounds2 = ᑩ;
		if (!((Control)this).Enabled)
		{
			style.TextColor = GetColorScheme().ItemDisabledText;
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, bounds);
		elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		if (paintText)
		{
			if (ݶ == null)
			{
				elementStyleDisplayInfo.Bounds = bounds2;
				ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, ((Control)this).Font);
			}
			else
			{
				MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, style.TextColor, (int)((Control)this).RightToLeft == 1, e.ClipRectangle, hotKeyPrefixVisible: true);
				ݶ.Render(d);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (_055D || _0DD3)
		{
			return;
		}
		_0DD3 = true;
		try
		{
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
			PaintPrepare(e);
			if (ᑲ && BarFunctions.ThemedOS)
			{
				PaintThemed(e);
			}
			else
			{
				PaintStyled(e);
			}
			e.Graphics.SmoothingMode = smoothingMode;
			e.Graphics.TextRenderingHint = textRenderingHint;
			((Control)this).OnPaint(e);
		}
		finally
		{
			_0DD3 = false;
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Panel)this).OnResize(e);
		RefreshTextClientRectangle();
		ᑴ(ᑵ: true);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		ᑴ(ᑵ: true);
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		((Control)this).NotifyInvalidate(invalidatedArea);
		ᑴ(ᑵ: true);
	}

	protected override void OnChangeUICues(UICuesEventArgs e)
	{
		if (ᑫ && e.ChangeFocus)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.મ((Control)(object)this, e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (ݶ != null)
		{
			ݶ.ग़((Control)(object)this, e);
		}
		if ((int)e.Button == 1048576 && !ݠ)
		{
			ݠ = true;
			if (GetStyleMouseDown().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.ફ((Control)(object)this, e);
		}
		((Control)this).OnMouseUp(e);
		if (ݠ)
		{
			ݠ = false;
			if (GetStyleMouseDown().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnClick(EventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.દ((Control)(object)this);
		}
		((Control)this).OnClick(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (!ݡ)
		{
			ݡ = true;
			if (GetStyleMouseOver().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.ભ((Control)(object)this);
		}
		((Control)this).OnMouseLeave(e);
		if (ݡ)
		{
			ݡ = false;
			if (GetStyleMouseOver().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		if (ݶ != null)
		{
			ݶ._0CC4();
		}
		RefreshTextClientRectangle();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string _06EA = ((Control)this).Text;
		if (ݶ != null)
		{
			ݶ.ભ((Control)(object)this);
			ݶ.ವ -= ᑶ;
			ݶ = null;
		}
		if (ሒ.መ(ref _06EA))
		{
			ݶ = ሒ.ሕ(_06EA);
		}
		if (ݶ != null)
		{
			ݶ.ವ += ᑶ;
			RefreshTextClientRectangle();
		}
		((Control)this).Invalidate();
		((Control)this).OnTextChanged(e);
	}

	private void ᑶ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected virtual void ResizeMarkup()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (ݶ == null)
		{
			return;
		}
		Rectangle clientTextRectangle = ClientTextRectangle;
		clientTextRectangle.Inflate(-2, 0);
		clientTextRectangle.X += ᝈ.ᝐ(Style);
		clientTextRectangle.Width -= ᝈ.ᝍ(Style);
		clientTextRectangle.Y += ᝈ._1753(Style);
		clientTextRectangle.Height -= ᝈ.ᝏ(Style);
		if (clientTextRectangle.Width <= 0 || clientTextRectangle.Height <= 0)
		{
			return;
		}
		Graphics val = CreateGraphics();
		try
		{
			if (ե)
			{
				val.SmoothingMode = (SmoothingMode)4;
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			ݶ.Measure(clientTextRectangle.Size, markupDrawContext);
			ݶ._0CC6(clientTextRectangle, markupDrawContext);
			if (((ScrollableControl)this).AutoScroll)
			{
				Size empty = Size.Empty;
				if (ݶ.Bounds.Height > clientTextRectangle.Height)
				{
					empty.Height = ݶ.Bounds.Height + Ӷ.MarginTop + Ӷ.MarginBottom + 2;
				}
				if (ݶ.Bounds.Width > clientTextRectangle.Width)
				{
					empty.Width = ݶ.Bounds.Width + Ӷ.MarginLeft + Ӷ.MarginRight + 2;
				}
				if (((ScrollableControl)this).AutoScrollMinSize != empty)
				{
					((ScrollableControl)this).AutoScrollMinSize = empty;
				}
			}
			else if (!((ScrollableControl)this).AutoScrollMinSize.IsEmpty)
			{
				((ScrollableControl)this).AutoScrollMinSize = Size.Empty;
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		լ.Refresh(null, bSystemColorEvent: true);
		RefreshStyleSystemColors();
		if (ᑬ != null)
		{
			చ();
		}
		((Control)this).Invalidate(true);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		((Control)this).OnHandleDestroyed(e);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (Control.IsMnemonic(charCode, ((Control)this).Text))
		{
			((Control)this).OnClick(new EventArgs());
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCanvasColor()
	{
		return ᑯ != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCanvasColor()
	{
		ᑯ = Color.White;
	}

	public void ResetStyle()
	{
		if (Ӷ == null)
		{
			Ӷ = new ElementStyle();
			Ӷ.SetColorScheme(GetColorScheme());
			Ӷ.StyleChanged += ᑳ;
		}
		else
		{
			Ӷ.Reset();
		}
		RefreshStyleSystemColors();
		((Control)this).Invalidate();
	}

	public void ResetMouseTracking()
	{
		ݡ = false;
		ݠ = false;
		((Control)this).Invalidate();
	}

	public void ResetStyleMouseOver()
	{
		if (Ӱ == null)
		{
			Ӱ = new ElementStyle();
			Ӱ.SetColorScheme(GetColorScheme());
			Ӱ.StyleChanged += ᑳ;
		}
		else
		{
			Ӱ.Reset();
		}
		((Control)this).Invalidate();
	}

	public void ResetStyleMouseDown()
	{
		if (ݔ == null)
		{
			ݔ = new ElementStyle();
			ݔ.SetColorScheme(GetColorScheme());
			ݔ.StyleChanged += ᑳ;
		}
		else
		{
			ݔ.Reset();
		}
		((Control)this).Invalidate();
	}

	protected virtual void OnColorSchemeChanged()
	{
		RefreshStyleSystemColors();
	}

	protected virtual void RefreshTextClientRectangle()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected I4, but got Unknown
		Rectangle rectangle = ((Control)this).DisplayRectangle;
		if (!ᑪ)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if ((int)val.Dock != 0)
			{
				if ((int)val.Dock == 5)
				{
					rectangle = Rectangle.Empty;
					break;
				}
				DockStyle dock = val.Dock;
				switch (dock - 1)
				{
				case 2:
					rectangle.X += val.Width;
					rectangle.Width -= val.Width;
					break;
				case 3:
					rectangle.Width -= val.Width;
					break;
				case 0:
					rectangle.Y += val.Height;
					rectangle.Height -= val.Height;
					break;
				case 1:
					rectangle.Height -= val.Height;
					break;
				}
			}
		}
		if (rectangle.Width <= 0 || rectangle.Height <= 0)
		{
			rectangle = Rectangle.Empty;
		}
		ᑩ = rectangle;
		ResizeMarkup();
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (ᑪ && ((Component)this).DesignMode)
		{
			((Control)this).Invalidate();
		}
	}

	public void ApplyPanelStyle()
	{
		ApplyPanelStyle(eDotNetBarStyle.Office2003);
	}

	public void ApplyPanelStyle(eDotNetBarStyle style)
	{
		ColorSchemeStyle = style;
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		Ӷ.Border = eStyleBorderType.Solid;
		Ӷ.BorderWidth = 1;
		Ӷ.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Ӷ.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Ӷ.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Ӷ.BackColorGradientAngle = GetColorScheme().PanelBackgroundGradientAngle;
		Ӷ.TextColorSchemePart = eColorSchemePart.PanelText;
		Ӷ.TextAlignment = eStyleTextAlignment.Center;
		Ӷ.TextLineAlignment = eStyleTextAlignment.Center;
		((Control)this).Invalidate();
	}

	public void ApplyButtonStyle()
	{
		ColorSchemeStyle = eDotNetBarStyle.Office2003;
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		Style.TextAlignment = eStyleTextAlignment.Center;
		Style.TextLineAlignment = eStyleTextAlignment.Center;
		Style.BackColorSchemePart = eColorSchemePart.BarBackground;
		Style.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		Style.BackgroundImagePosition = eStyleBackgroundImage.Tile;
		Style.Border = eStyleBorderType.Solid;
		Style.BorderWidth = 1;
		Style.BorderColorSchemePart = eColorSchemePart.BarDockedBorder;
		Style.TextColorSchemePart = eColorSchemePart.ItemText;
		Style.BackColorGradientAngle = 90;
		StyleMouseDown.BackColorSchemePart = eColorSchemePart.ItemPressedBackground;
		StyleMouseDown.BackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
		StyleMouseDown.BorderColorSchemePart = eColorSchemePart.ItemPressedBorder;
		StyleMouseDown.TextColorSchemePart = eColorSchemePart.ItemPressedText;
		StyleMouseOver.BackColorSchemePart = eColorSchemePart.ItemHotBackground;
		StyleMouseOver.BackColor2SchemePart = eColorSchemePart.ItemHotBackground2;
		StyleMouseOver.BorderColorSchemePart = eColorSchemePart.ItemHotBorder;
		StyleMouseOver.TextColorSchemePart = eColorSchemePart.ItemHotText;
		((Control)this).Invalidate();
	}

	public void ApplyLabelStyle()
	{
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		ApplyLabelStyle(Style);
		((Control)this).Invalidate();
	}

	protected virtual void ApplyLabelStyle(ElementStyle style)
	{
		style.Reset();
		style.TextAlignment = eStyleTextAlignment.Center;
		style.BackColorSchemePart = eColorSchemePart.BarBackground;
		style.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		style.BackgroundImagePosition = eStyleBackgroundImage.Tile;
		style.BorderColorSchemePart = eColorSchemePart.BarDockedBorder;
		style.BorderWidth = 1;
		style.TextColorSchemePart = eColorSchemePart.ItemText;
		style.BackColorGradientAngle = 90;
	}

	public void NotifyDefault(bool value)
	{
		if (ᑮ != value)
		{
			ᑮ = value;
		}
	}

	public void PerformClick()
	{
		if (((Control)this).CanSelect)
		{
			((Control)this).OnClick(EventArgs.Empty);
		}
	}

	private void చ()
	{
		DisposeThemeCachedBitmap();
		if (ᑬ != null)
		{
			ᑬ.Dispose();
			ᑬ = new ᑧ((Control)(object)this);
		}
	}

	private void _0A78()
	{
		if (ᑬ != null)
		{
			ᑬ.Dispose();
			ᑬ = null;
		}
		DisposeThemeCachedBitmap();
	}

	protected void DisposeThemeCachedBitmap()
	{
		if (m_ThemeCachedBitmap != null)
		{
			((Image)m_ThemeCachedBitmap).Dispose();
			m_ThemeCachedBitmap = null;
		}
	}
}
