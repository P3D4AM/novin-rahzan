using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public abstract class BaseItemControl : Control
{
	private BaseItem m_Item;

	private ElementStyle m_BackgroundStyle;

	private ColorScheme m_ColorScheme;

	private bool m_DesignModeInternal;

	private bool m_AntiAlias = true;

	private bool _CallBasePaintBackground = true;

	private bool _FocusCuesEnabled = true;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private bool m_MouseFocus;

	private int m_UpdateSuspendCount;

	protected virtual BaseItem HostItem
	{
		get
		{
			return m_Item;
		}
		set
		{
			m_Item = value;
			if (m_Item != null)
			{
				m_Item.Displayed = true;
				m_Item.ContainerControl = this;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CallBasePaintBackground
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

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return _FocusCuesEnabled;
		}
		set
		{
			_FocusCuesEnabled = value;
			if (((Control)this).Focused)
			{
				((Control)this).Invalidate();
			}
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => m_BackgroundStyle;

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
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
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

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return m_Item.Style;
		}
		set
		{
			m_Item.Style = value;
			if (!BarFunctions.IsOffice2007Style(value))
			{
				GetColorScheme().Style = value;
			}
			RecalcLayout();
		}
	}

	[Browsable(false)]
	public bool IsUpdateSuspended => m_UpdateSuspendCount > 0;

	public BaseItemControl()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
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
		((Control)this).SetStyle((ControlStyles)4096, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
		m_BackgroundStyle = new ElementStyle();
		m_BackgroundStyle.SetColorScheme(m_ColorScheme);
		m_BackgroundStyle.StyleChanged += VisualPropertyChanged;
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (m_Item != null)
		{
			m_Item.UpdateBindings();
		}
	}

	protected bool GetDesignMode()
	{
		if (!m_DesignModeInternal)
		{
			return ((Component)this).DesignMode;
		}
		return m_DesignModeInternal;
	}

	protected virtual ElementStyle GetBackgroundStyle()
	{
		return m_BackgroundStyle;
	}

	internal void SetDesignMode(bool mode)
	{
		m_DesignModeInternal = mode;
		if (m_Item != null)
		{
			m_Item.SetDesignMode(mode);
		}
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (m_Item != null && BarFunctions.IsOffice2007Style(m_Item.EffectiveStyle))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return m_ColorScheme;
	}

	internal void InternalPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
	}

	protected virtual void InternalPaintBackground(PaintEventArgs e)
	{
		if ((((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent) && _CallBasePaintBackground)
		{
			((Control)this).OnPaintBackground(e);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		InternalPaintBackground(e);
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.SetColorScheme(GetColorScheme());
		}
		PaintBackground(e);
		PaintControl(e);
		if (((Control)this).Focused && FocusCuesEnabled)
		{
			PaintFocusCues(e);
		}
		((Control)this).OnPaint(e);
	}

	protected virtual void PaintFocusCues(PaintEventArgs e)
	{
		ControlPaint.DrawFocusRectangle(e.Graphics, ((Control)this).ClientRectangle);
	}

	protected virtual void PaintBackground(PaintEventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		ElementStyle backgroundStyle = GetBackgroundStyle();
		if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, ((Control)this).BackColor);
		}
		if (((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (backgroundStyle.Custom)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (m_AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, clientRectangle);
			ElementStyleDisplay.Paint(e2);
			if (m_AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
		}
	}

	protected virtual void PaintControl(PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		Graphics graphics = e.Graphics;
		if (m_AntiAlias)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(graphics);
		itemPaintArgs.ClipRectangle = e.ClipRectangle;
		if (m_Item != null)
		{
			m_Item.Paint(itemPaintArgs);
		}
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
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

	protected virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && WinApi.IsGlassEnabled;
		return itemPaintArgs;
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
		return m_Renderer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		m_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		m_BackgroundStyle = new ElementStyle();
		m_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		OnBackgroundStyleChanged();
		((Control)this).Invalidate();
	}

	protected virtual void OnBackgroundStyleChanged()
	{
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		if (GetDesignMode() || (((Control)this).Parent != null && ((Component)(object)((Control)this).Parent).Site != null && ((Component)(object)((Control)this).Parent).Site.DesignMode))
		{
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		return m_ColorScheme.SchemeChanged;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Component)this).DesignMode && m_Item != null)
		{
			m_Item.SetDesignMode(((Component)this).DesignMode);
		}
	}

	public virtual void RecalcLayout()
	{
		if (m_Item != null)
		{
			Rectangle itemBounds = GetItemBounds();
			m_Item.Bounds = itemBounds;
			RecalcSize();
			m_Item.Bounds = itemBounds;
			((Control)this).Invalidate();
		}
	}

	protected virtual void RecalcSize()
	{
		m_Item.RecalcSize();
	}

	protected virtual Rectangle GetItemBounds()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(BackgroundStyle);
		clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(BackgroundStyle);
		clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(BackgroundStyle);
		clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(BackgroundStyle);
		return clientRectangle;
	}

	protected override void OnResize(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnResize(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		m_Item.Text = ((Control)this).Text;
		RecalcLayout();
		((Control)this).OnTextChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		BarUtilities.InvalidateFontChange(m_Item);
		RecalcLayout();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		m_Item.Enabled = ((Control)this).Enabled;
		((Control)this).OnEnabledChanged(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (!m_MouseFocus)
		{
			m_Item.OnGotFocus();
		}
		m_MouseFocus = false;
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		m_Item.OnLostFocus();
		((Control)this).OnLostFocus(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		m_Item.InternalMouseEnter();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		m_Item.InternalMouseMove(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		m_Item.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		m_Item.InternalMouseHover();
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && !((Control)this).Focused && ((Control)this).GetStyle((ControlStyles)512))
		{
			m_MouseFocus = true;
			((Control)this).Focus();
		}
		m_Item.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		m_Item.InternalMouseUp(e);
		((Control)this).OnMouseUp(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Item.InternalClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Item.InternalDoubleClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		m_Item.InternalKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (Control.IsMnemonic(charCode, m_Item.Text) && ProcessAccelerator())
		{
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected virtual bool ProcessAccelerator()
	{
		m_Item.RaiseClick(eEventSource.Keyboard);
		return true;
	}

	public void BeginUpdate()
	{
		m_UpdateSuspendCount++;
	}

	public void EndUpdate()
	{
		EndUpdate(callRecalcLayout: true);
	}

	public void EndUpdate(bool callRecalcLayout)
	{
		if (m_UpdateSuspendCount > 0)
		{
			m_UpdateSuspendCount--;
			if (m_UpdateSuspendCount == 0 && callRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}
}
