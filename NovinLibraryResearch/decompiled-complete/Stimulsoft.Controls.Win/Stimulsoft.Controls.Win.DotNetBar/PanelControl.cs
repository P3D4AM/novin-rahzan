using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class PanelControl : Panel, IButtonControl
{
	private ElementStyle m_Style;

	private ElementStyle m_StyleMouseOver;

	private ElementStyle m_StyleMouseDown;

	private ColorScheme m_ColorScheme;

	private eDotNetBarStyle m_ColorSchemeStyle = eDotNetBarStyle.Office2003;

	private bool m_MouseOver;

	private bool m_MouseDown;

	private Rectangle m_ClientTextRectangle = Rectangle.Empty;

	private bool m_TextDockConstrained = true;

	private bool m_ShowFocusRectangle;

	private string m_Text = "";

	private ThemeTab m_ThemeTab;

	private DialogResult m_DialogResult;

	private bool m_IsDefault;

	private bool m_AntiAlias = true;

	private Color m_CanvasColor = Color.White;

	protected Bitmap m_ThemeCachedBitmap;

	private bool m_SuspendPaint;

	private BodyElement m_TextMarkup;

	private eCornerType m_CurrentCornerType = eCornerType.Square;

	private int m_CornerDiameter = 8;

	internal bool CanSetRegion = true;

	private bool m_Painting;

	internal bool DrawThemedPane;

	internal BodyElement TextMarkupElement => m_TextMarkup;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			m_ColorScheme = value;
			OnColorSchemeChanged();
			if (((Control)this).Visible)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Gets or sets the text displayed on panel.")]
	public override string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (value.Length >= 0)
			{
				((Panel)this).Text = value;
				m_Text = value;
				((Control)this).OnTextChanged(new EventArgs());
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether focus rectangle is displayed when control has focus.")]
	public bool ShowFocusRectangle
	{
		get
		{
			return m_ShowFocusRectangle;
		}
		set
		{
			m_ShowFocusRectangle = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Background")]
	[Description("Gets or sets the canvas color.")]
	public Color CanvasColor
	{
		get
		{
			return m_CanvasColor;
		}
		set
		{
			m_CanvasColor = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Indicates panel style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ElementStyle Style => m_Style;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the panel style when mouse hovers over the panel.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ElementStyle StyleMouseOver => m_StyleMouseOver;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the panel style when mouse button is pressed on the panel.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public virtual ElementStyle StyleMouseDown => m_StyleMouseDown;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return m_AntiAlias;
		}
		set
		{
			if (m_AntiAlias != value)
			{
				m_AntiAlias = value;
				((Control)this).Invalidate();
			}
		}
	}

	internal eDotNetBarStyle EffectiveColorSchemeStyle
	{
		get
		{
			if (m_ColorSchemeStyle == eDotNetBarStyle.StyleManagerControlled)
			{
				return StyleManager.GetEffectiveStyle();
			}
			return m_ColorSchemeStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets color scheme style.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle ColorSchemeStyle
	{
		get
		{
			return m_ColorSchemeStyle;
		}
		set
		{
			m_ColorSchemeStyle = value;
			m_ColorScheme = new ColorScheme(EffectiveColorSchemeStyle);
			OnColorSchemeChanged();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	public virtual bool TextDockConstrained
	{
		get
		{
			return m_TextDockConstrained;
		}
		set
		{
			m_TextDockConstrained = value;
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
			return m_ClientTextRectangle;
		}
		set
		{
			m_ClientTextRectangle = value;
		}
	}

	protected virtual bool SuspendPaint
	{
		get
		{
			return m_SuspendPaint;
		}
		set
		{
			m_SuspendPaint = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value returned to the parent form when the button is clicked.")]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_DialogResult;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Enum.IsDefined(typeof(DialogResult), value))
			{
				m_DialogResult = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public virtual bool ThemeAware
	{
		get
		{
			return DrawThemedPane;
		}
		set
		{
			DisposeThemeCachedBitmap();
			DrawThemedPane = value;
			((Control)this).Invalidate();
		}
	}

	internal ThemeTab ThemeTab
	{
		get
		{
			if (m_ThemeTab == null)
			{
				m_ThemeTab = new ThemeTab((Control)(object)this);
			}
			return m_ThemeTab;
		}
	}

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public PanelControl()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
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
		m_ColorScheme = new ColorScheme(m_ColorSchemeStyle);
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	internal ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle) && GlobalManager.Renderer is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return m_ColorScheme;
	}

	private void OnVisualPropertyChanged(object sender, EventArgs e)
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
		SetRegion(bResize: false);
	}

	private void SetRegion(bool bResize)
	{
		if (!CanSetRegion)
		{
			return;
		}
		ElementStyle style = GetStyle();
		if (bResize || m_CurrentCornerType != style.CornerType || m_CornerDiameter != style.CornerDiameter)
		{
			m_CurrentCornerType = style.CornerType;
			m_CornerDiameter = style.CornerDiameter;
			if (style != null && (!DrawThemedPane || !BarFunctions.ThemedOS) && style.CornerType != eCornerType.Square)
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
		if (m_Style != null)
		{
			m_Style.SetColorScheme(colorScheme);
		}
		if (m_StyleMouseOver != null)
		{
			m_StyleMouseOver.SetColorScheme(colorScheme);
		}
		if (m_StyleMouseDown != null)
		{
			m_StyleMouseDown.SetColorScheme(colorScheme);
		}
	}

	protected override void WndProc(ref Message m)
	{
		int msg = ((Message)(ref m)).Msg;
		if ((uint)(msg - 276) <= 1u || msg == 522)
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count != 0)
			{
				((ScrollableControl)this).WndProc(ref m);
				((Control)this).Refresh();
				return;
			}
			RefreshTextClientRectangle();
			((Control)this).Invalidate();
		}
		((ScrollableControl)this).WndProc(ref m);
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (m_AntiAlias)
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
		return m_Style;
	}

	protected virtual ElementStyle GetStyleMouseDown()
	{
		return m_StyleMouseDown;
	}

	protected virtual ElementStyle GetStyleMouseOver()
	{
		return m_StyleMouseOver;
	}

	protected virtual void PaintThemed(PaintEventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ((Control)this).ClientRectangle.Size)
		{
			DisposeThemeCachedBitmap();
			Bitmap val = new Bitmap(((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height, e.Graphics);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				ThemeTab.DrawBackground(val2, ThemeTabParts.Pane, ThemeTabStates.Normal, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height));
			}
			finally
			{
				val2.Dispose();
			}
			e.Graphics.DrawImage((Image)(object)val, 0, 0, ((Image)val).Width, ((Image)val).Height);
			ElementStyle style = GetStyle();
			if (style.BackgroundImage != null)
			{
				BarFunctions.PaintBackgroundImage(e.Graphics, ((Control)this).ClientRectangle, style.BackgroundImage, style.BackgroundImagePosition, style.BackgroundImageAlpha);
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
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		SolidBrush val = new SolidBrush(m_CanvasColor);
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
		if (m_AntiAlias)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
	}

	protected virtual void PaintStyled(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		using ElementStyle elementStyle = GetStyle().Copy();
		if (m_MouseDown && ((Control)this).Enabled)
		{
			elementStyle.ApplyStyle(GetStyleMouseDown());
		}
		else if (m_MouseOver && ((Control)this).Enabled)
		{
			elementStyle.ApplyStyle(GetStyleMouseOver());
		}
		PaintInnerContent(e, elementStyle, paintText: true);
		if (((Control)this).Focused && m_ShowFocusRectangle)
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
		if (m_TextMarkup == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Rectangle clientTextRectangle = m_ClientTextRectangle;
		if (!((Control)this).Enabled)
		{
			style.TextColor = GetColorScheme().ItemDisabledText;
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, bounds);
		elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		if (paintText)
		{
			if (m_TextMarkup == null)
			{
				elementStyleDisplayInfo.Bounds = clientTextRectangle;
				ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, ((Control)this).Font);
			}
			else
			{
				MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, style.TextColor, (int)((Control)this).RightToLeft == 1, e.ClipRectangle, hotKeyPrefixVisible: true);
				m_TextMarkup.Render(d);
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
		if (m_SuspendPaint || m_Painting)
		{
			return;
		}
		m_Painting = true;
		try
		{
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
			PaintPrepare(e);
			if (DrawThemedPane && BarFunctions.ThemedOS)
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
			m_Painting = false;
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Panel)this).OnResize(e);
		RefreshTextClientRectangle();
		SetRegion(bResize: true);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		SetRegion(bResize: true);
	}

	protected override void NotifyInvalidate(Rectangle invalidatedArea)
	{
		((Control)this).NotifyInvalidate(invalidatedArea);
		SetRegion(bResize: true);
	}

	protected override void OnChangeUICues(UICuesEventArgs e)
	{
		if (m_ShowFocusRectangle && e.ChangeFocus)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseMove((Control)(object)this, e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseDown((Control)(object)this, e);
		}
		if ((int)e.Button == 1048576 && !m_MouseDown)
		{
			m_MouseDown = true;
			if (GetStyleMouseDown().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseUp((Control)(object)this, e);
		}
		((Control)this).OnMouseUp(e);
		if (m_MouseDown)
		{
			m_MouseDown = false;
			if (GetStyleMouseDown().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnClick(EventArgs e)
	{
		if (m_TextMarkup != null)
		{
			m_TextMarkup.Click((Control)(object)this);
		}
		((Control)this).OnClick(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			if (GetStyleMouseOver().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseLeave((Control)(object)this);
		}
		((Control)this).OnMouseLeave(e);
		if (m_MouseOver)
		{
			m_MouseOver = false;
			if (GetStyleMouseOver().Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		if (m_TextMarkup != null)
		{
			m_TextMarkup.InvalidateElementsSize();
		}
		RefreshTextClientRectangle();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string text = ((Control)this).Text;
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseLeave((Control)(object)this);
			m_TextMarkup.HyperLinkClick -= TextMarkupLinkClicked;
			m_TextMarkup = null;
		}
		if (MarkupParser.IsMarkup(ref text))
		{
			m_TextMarkup = MarkupParser.Parse(text);
		}
		if (m_TextMarkup != null)
		{
			m_TextMarkup.HyperLinkClick += TextMarkupLinkClicked;
			RefreshTextClientRectangle();
		}
		((Control)this).OnTextChanged(e);
	}

	private void TextMarkupLinkClicked(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}

	protected virtual void ResizeMarkup()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (m_TextMarkup == null)
		{
			return;
		}
		Rectangle clientTextRectangle = ClientTextRectangle;
		clientTextRectangle.Inflate(-2, 0);
		clientTextRectangle.X += ElementStyleLayout.LeftWhiteSpace(Style);
		clientTextRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(Style);
		clientTextRectangle.Y += ElementStyleLayout.TopWhiteSpace(Style);
		clientTextRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(Style);
		if (clientTextRectangle.Width <= 0 || clientTextRectangle.Height <= 0)
		{
			return;
		}
		Graphics val = CreateGraphics();
		try
		{
			if (m_AntiAlias)
			{
				val.SmoothingMode = (SmoothingMode)4;
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			m_TextMarkup.Measure(clientTextRectangle.Size, d);
			m_TextMarkup.Arrange(clientTextRectangle, d);
			if (((ScrollableControl)this).AutoScroll)
			{
				Size empty = Size.Empty;
				if (m_TextMarkup.Bounds.Height > clientTextRectangle.Height)
				{
					empty.Height = m_TextMarkup.Bounds.Height + m_Style.MarginTop + m_Style.MarginBottom + 2;
				}
				if (m_TextMarkup.Bounds.Width > clientTextRectangle.Width)
				{
					empty.Width = m_TextMarkup.Bounds.Width + m_Style.MarginLeft + m_Style.MarginRight + 2;
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
		m_ColorScheme.Refresh(null, bSystemColorEvent: true);
		RefreshStyleSystemColors();
		if (m_ThemeTab != null)
		{
			RefreshThemes();
		}
		((Control)this).Invalidate(true);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DisposeThemes();
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
		return m_CanvasColor != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCanvasColor()
	{
		m_CanvasColor = Color.White;
	}

	public void ResetStyle()
	{
		if (m_Style == null)
		{
			m_Style = new ElementStyle();
			m_Style.SetColorScheme(GetColorScheme());
			m_Style.StyleChanged += OnVisualPropertyChanged;
		}
		else
		{
			m_Style.Reset();
		}
		RefreshStyleSystemColors();
		((Control)this).Invalidate();
	}

	public void ResetMouseTracking()
	{
		m_MouseOver = false;
		m_MouseDown = false;
		((Control)this).Invalidate();
	}

	public void ResetStyleMouseOver()
	{
		if (m_StyleMouseOver == null)
		{
			m_StyleMouseOver = new ElementStyle();
			m_StyleMouseOver.SetColorScheme(GetColorScheme());
			m_StyleMouseOver.StyleChanged += OnVisualPropertyChanged;
		}
		else
		{
			m_StyleMouseOver.Reset();
		}
		((Control)this).Invalidate();
	}

	public void ResetStyleMouseDown()
	{
		if (m_StyleMouseDown == null)
		{
			m_StyleMouseDown = new ElementStyle();
			m_StyleMouseDown.SetColorScheme(GetColorScheme());
			m_StyleMouseDown.StyleChanged += OnVisualPropertyChanged;
		}
		else
		{
			m_StyleMouseDown.Reset();
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
		Rectangle clientTextRectangle = ((Control)this).DisplayRectangle;
		if (!m_TextDockConstrained)
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
					clientTextRectangle = Rectangle.Empty;
					break;
				}
				DockStyle dock = val.Dock;
				switch (dock - 1)
				{
				case 2:
					clientTextRectangle.X += val.Width;
					clientTextRectangle.Width -= val.Width;
					break;
				case 3:
					clientTextRectangle.Width -= val.Width;
					break;
				case 0:
					clientTextRectangle.Y += val.Height;
					clientTextRectangle.Height -= val.Height;
					break;
				case 1:
					clientTextRectangle.Height -= val.Height;
					break;
				}
			}
		}
		if (clientTextRectangle.Width <= 0 || clientTextRectangle.Height <= 0)
		{
			clientTextRectangle = Rectangle.Empty;
		}
		m_ClientTextRectangle = clientTextRectangle;
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
		if (m_TextDockConstrained && ((Component)this).DesignMode)
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
		m_Style.Border = eStyleBorderType.Solid;
		m_Style.BorderWidth = 1;
		m_Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		m_Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		m_Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		m_Style.BackColorGradientAngle = GetColorScheme().PanelBackgroundGradientAngle;
		m_Style.TextColorSchemePart = eColorSchemePart.PanelText;
		m_Style.TextAlignment = eStyleTextAlignment.Center;
		m_Style.TextLineAlignment = eStyleTextAlignment.Center;
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
		if (m_IsDefault != value)
		{
			m_IsDefault = value;
		}
	}

	public void PerformClick()
	{
		if (((Control)this).CanSelect)
		{
			((Control)this).OnClick(EventArgs.Empty);
		}
	}

	private void RefreshThemes()
	{
		DisposeThemeCachedBitmap();
		if (m_ThemeTab != null)
		{
			m_ThemeTab.Dispose();
			m_ThemeTab = new ThemeTab((Control)(object)this);
		}
	}

	private void DisposeThemes()
	{
		if (m_ThemeTab != null)
		{
			m_ThemeTab.Dispose();
			m_ThemeTab = null;
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
