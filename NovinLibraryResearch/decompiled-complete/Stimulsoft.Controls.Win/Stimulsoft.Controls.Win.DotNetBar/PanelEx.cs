using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class PanelEx : Panel, IButtonControl, INonClientControl
{
	private ItemStyle m_Style;

	private ItemStyle m_StyleMouseOver;

	private ItemStyle m_StyleMouseDown;

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

	private bool m_RenderText = true;

	private bool m_RightToLeftLayout;

	private bool m_MarkupUsesStyleAlignment;

	private NonClientPaintHandler m_NCPainter;

	private eCornerType m_CurrentCornerType = eCornerType.Square;

	private int m_CornerDiameter = 8;

	private bool _TextMarkupEnabled = true;

	internal bool DrawThemedPane;

	private BaseRenderer m_DefaultRenderer;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether right-to-left mirror placement is turned on. ")]
	public bool RightToLeftLayout
	{
		get
		{
			return m_RightToLeftLayout;
		}
		set
		{
			if (m_RightToLeftLayout != value)
			{
				m_RightToLeftLayout = value;
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).RecreateHandle();
				}
			}
		}
	}

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
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool SuspendPaint
	{
		get
		{
			return m_SuspendPaint;
		}
		set
		{
			if (m_SuspendPaint != value)
			{
				m_SuspendPaint = value;
				if (!m_SuspendPaint)
				{
					SetRegion();
					((Control)this).Invalidate();
				}
			}
		}
	}

	private bool IsThemed
	{
		get
		{
			if (DrawThemedPane)
			{
				return BarFunctions.ThemedOS;
			}
			return false;
		}
	}

	internal bool RenderText
	{
		get
		{
			return m_RenderText;
		}
		set
		{
			m_RenderText = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether Text supports and renders text markup.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _TextMarkupEnabled;
		}
		set
		{
			if (_TextMarkupEnabled != value)
			{
				_TextMarkupEnabled = value;
				((Control)this).OnTextChanged(EventArgs.Empty);
				if (_TextMarkupEnabled)
				{
					ResizeMarkup();
				}
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
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
			if (m_Text != value)
			{
				((Panel)this).Text = value;
				m_Text = value;
				((Control)this).OnTextChanged(new EventArgs());
				((Control)this).Refresh();
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
				((Control)this).Refresh();
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
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets panel style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle Style => m_Style;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the panel style when mouse hovers over the panel.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle StyleMouseOver => m_StyleMouseOver;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the panel style when mouse button is pressed on the panel.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle StyleMouseDown => m_StyleMouseDown;

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
				OnAntiAliasChanged();
				((Control)this).Refresh();
			}
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
			if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle) && GlobalManager.Renderer is Office2007Renderer)
			{
				m_ColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			else
			{
				m_ColorScheme = new ColorScheme(m_ColorSchemeStyle);
			}
			if (m_NCPainter != null)
			{
				if (BarFunctions.IsOffice2007Style(value))
				{
					m_NCPainter.SkinScrollbars = eScrollBarSkin.Optimized;
				}
				else
				{
					m_NCPainter.SkinScrollbars = eScrollBarSkin.None;
				}
			}
			OnColorSchemeChanged();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	public override bool AutoScroll
	{
		get
		{
			return ((ScrollableControl)this).AutoScroll;
		}
		set
		{
			if (!value)
			{
				((ScrollableControl)this).AutoScrollMinSize = Size.Empty;
			}
			((ScrollableControl)this).AutoScroll = value;
			RefreshTextClientRectangle();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	public bool TextDockConstrained
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
				((Control)this).Refresh();
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

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			if (m_RightToLeftLayout)
			{
				createParams.ExStyle |= 0x500000;
			}
			return createParams;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
	public virtual bool MarkupUsesStyleAlignment
	{
		get
		{
			return m_MarkupUsesStyleAlignment;
		}
		set
		{
			if (m_MarkupUsesStyleAlignment != value)
			{
				m_MarkupUsesStyleAlignment = value;
				if (BarFunctions.IsHandleValid((Control)(object)this) && m_TextMarkup != null)
				{
					ResizeMarkup();
					((Control)this).Invalidate();
				}
			}
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
			((Control)this).Refresh();
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

	ElementStyle INonClientControl.BorderStyle => null;

	IntPtr INonClientControl.Handle => ((Control)this).Handle;

	int INonClientControl.Width => ((Control)this).Width;

	int INonClientControl.Height => ((Control)this).Height;

	bool INonClientControl.IsHandleCreated => ((Control)this).IsHandleCreated;

	Color INonClientControl.BackColor => ((Control)this).BackColor;

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public PanelEx()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
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
		m_ColorScheme = new ColorScheme(m_ColorSchemeStyle);
		ResetStyle();
		ResetStyleMouseOver();
		ResetStyleMouseDown();
		m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
		((Control)this).BackColor = Color.Transparent;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (m_NCPainter != null)
		{
			m_NCPainter.Dispose();
		}
		if (m_Style != null)
		{
			m_Style.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		if (m_StyleMouseDown != null)
		{
			m_StyleMouseDown.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		if (m_StyleMouseOver != null)
		{
			m_StyleMouseOver.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		OnColorSchemeChanged();
	}

	internal void DisableSelection()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Panel)this).TabStop = false;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		if (m_ColorScheme.SchemeChanged)
		{
			return !BarFunctions.IsOffice2007Style(m_ColorSchemeStyle);
		}
		return false;
	}

	private void OnVisualPropertyChanged(object sender, EventArgs e)
	{
		if (m_MarkupUsesStyleAlignment)
		{
			ResizeMarkup();
		}
		if (!m_SuspendPaint)
		{
			RefreshStyleSystemColors();
			SetRegion();
			((Control)this).Invalidate();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle))
		{
			RefreshStyleSystemColors();
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	private void SetRegion()
	{
		SetRegion(bResize: false);
	}

	private void SetRegion(bool bResize)
	{
		if (bResize || m_CurrentCornerType != m_Style.CornerType || m_CornerDiameter != m_Style.CornerDiameter)
		{
			m_CurrentCornerType = m_Style.CornerType;
			m_CornerDiameter = m_Style.CornerDiameter;
			if (m_Style != null && (!DrawThemedPane || !BarFunctions.ThemedOS) && m_Style.CornerType != eCornerType.Square)
			{
				((Control)this).Region = m_Style.GetRegion(((Control)this).ClientRectangle);
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
			m_Style.ApplyColorScheme(colorScheme);
		}
		if (m_StyleMouseOver != null)
		{
			m_StyleMouseOver.ApplyColorScheme(colorScheme);
		}
		if (m_StyleMouseDown != null)
		{
			m_StyleMouseDown.ApplyColorScheme(colorScheme);
		}
	}

	protected override void WndProc(ref Message m)
	{
		bool flag = true;
		int msg = ((Message)(ref m)).Msg;
		if ((uint)(msg - 276) <= 1u || msg == 522)
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
			{
				m_SuspendPaint = true;
			}
			if (m_NCPainter != null)
			{
				m_NCPainter.SuspendPaint = true;
			}
			try
			{
				if (m_NCPainter != null)
				{
					flag = m_NCPainter.WndProc(ref m);
				}
				if (flag)
				{
					((ScrollableControl)this).WndProc(ref m);
				}
			}
			finally
			{
				if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
				{
					m_SuspendPaint = false;
				}
				if (m_NCPainter != null)
				{
					m_NCPainter.SuspendPaint = false;
				}
			}
			if (m_NCPainter != null)
			{
				m_NCPainter.PaintNonClientAreaBuffered();
			}
			if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
			{
				RefreshTextClientRectangle();
				((Control)this).Invalidate();
			}
		}
		else
		{
			if (m_NCPainter != null)
			{
				flag = m_NCPainter.WndProc(ref m);
			}
			if ((((Message)(ref m)).Msg == 133 || ((Message)(ref m)).Msg == 15) && ((ScrollableControl)this).AutoScroll && ((ScrollableControl)this).VScroll && ((ScrollableControl)this).HScroll)
			{
				PaintScrollCorner();
			}
			if (flag)
			{
				((ScrollableControl)this).WndProc(ref m);
			}
		}
	}

	private void PaintScrollCorner()
	{
		IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				val.FillRectangle(SystemBrushes.Control, ((Control)this).Width - SystemInformation.VerticalScrollBarWidth, ((Control)this).Height - SystemInformation.HorizontalScrollBarHeight, SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			WinApi.ReleaseDC(((Control)this).Handle, windowDC);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Invalid comparison between Unknown and I4
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Invalid comparison between Unknown and I4
		if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || m_SuspendPaint)
		{
			return;
		}
		SolidBrush val = new SolidBrush(m_CanvasColor);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((m_Style.BackColor1.Color.IsEmpty || m_Style.BackColor1.Color.A < byte.MaxValue) && (m_Style.BackColor2.Color.IsEmpty || m_Style.BackColor2.Color.A < byte.MaxValue))
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (m_AntiAlias)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		ItemStyle itemStyle = m_Style.Clone() as ItemStyle;
		if (m_MouseDown && ((Control)this).Enabled && m_StyleMouseDown.Custom)
		{
			itemStyle.ApplyStyle(m_StyleMouseDown);
		}
		else if (m_MouseOver && ((Control)this).Enabled && m_StyleMouseOver.Custom)
		{
			itemStyle.ApplyStyle(m_StyleMouseOver);
		}
		if (m_TextMarkup == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		int x = displayRectangle.X;
		Padding padding = ((Control)this).Padding;
		displayRectangle.X = x - ((Padding)(ref padding)).Left;
		int y = displayRectangle.Y;
		padding = ((Control)this).Padding;
		displayRectangle.Y = y - ((Padding)(ref padding)).Top;
		int width = displayRectangle.Width;
		padding = ((Control)this).Padding;
		displayRectangle.Width = width + ((Padding)(ref padding)).Horizontal;
		int height = displayRectangle.Height;
		padding = ((Control)this).Padding;
		displayRectangle.Height = height + ((Padding)(ref padding)).Vertical;
		Rectangle clientTextRectangle = m_ClientTextRectangle;
		clientTextRectangle.Inflate(-1, -1);
		if (!((Control)this).Enabled)
		{
			itemStyle.ForeColor.Color = m_ColorScheme.ItemDisabledText;
		}
		Graphics graphics = e.Graphics;
		if (IsThemed)
		{
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ((Control)this).ClientRectangle.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val2 = new Bitmap(((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height, e.Graphics);
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					ThemeTab.DrawBackground(val3, ThemeTabParts.Pane, ThemeTabStates.Normal, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height));
				}
				finally
				{
					val3.Dispose();
				}
				e.Graphics.DrawImage((Image)(object)val2, 0, 0, ((Image)val2).Width, ((Image)val2).Height);
				if (m_Style.BackgroundImage != null)
				{
					BarFunctions.PaintBackgroundImage(e.Graphics, ((Control)this).ClientRectangle, m_Style.BackgroundImage, m_Style.BackgroundImagePosition, m_Style.BackgroundImageAlpha);
				}
				m_ThemeCachedBitmap = val2;
			}
			else
			{
				e.Graphics.DrawImage((Image)(object)m_ThemeCachedBitmap, 0, 0, ((Image)m_ThemeCachedBitmap).Width, ((Image)m_ThemeCachedBitmap).Height);
			}
			if (RenderText)
			{
				Region clip = graphics.Clip;
				Rectangle clientRectangle = ((Control)this).ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				clientRectangle.Height -= 3;
				graphics.SetClip(clientRectangle);
				if (m_TextMarkup == null)
				{
					itemStyle.PaintText(graphics, ((Control)this).Text, clientTextRectangle, ((Control)this).Font);
				}
				else
				{
					Rectangle clipRectangle = e.ClipRectangle;
					if (clipRectangle.Height == ((Control)this).ClientRectangle.Height && clipRectangle.Width < ((Control)this).ClientRectangle.Width)
					{
						clipRectangle.Width = ((Control)this).ClientRectangle.Width;
					}
					MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, itemStyle.ForeColor.Color, (int)((Control)this).RightToLeft == 1, clipRectangle, hotKeyPrefixVisible: true);
					m_TextMarkup.Render(d);
				}
				graphics.Clip = clip;
			}
		}
		else if (m_TextMarkup == null)
		{
			if (m_RenderText)
			{
				itemStyle.Paint(graphics, displayRectangle, ((Control)this).Text, clientTextRectangle, ((Control)this).Font);
			}
			else
			{
				itemStyle.Paint(graphics, displayRectangle);
			}
		}
		else
		{
			itemStyle.Paint(graphics, displayRectangle);
			if (m_RenderText)
			{
				Rectangle clipRectangle2 = e.ClipRectangle;
				if (clipRectangle2.Height == ((Control)this).ClientRectangle.Height && clipRectangle2.Width < ((Control)this).ClientRectangle.Width)
				{
					clipRectangle2.Width = ((Control)this).ClientRectangle.Width;
				}
				MarkupDrawContext d2 = new MarkupDrawContext(graphics, ((Control)this).Font, itemStyle.ForeColor.Color, (int)((Control)this).RightToLeft == 1, clipRectangle2, hotKeyPrefixVisible: true);
				m_TextMarkup.Render(d2);
			}
		}
		if (((Control)this).Focused && m_ShowFocusRectangle)
		{
			displayRectangle = ((Control)this).ClientRectangle;
			displayRectangle.Inflate(-2, -2);
			if (displayRectangle.Width > 0 && displayRectangle.Height > 0)
			{
				ControlPaint.DrawFocusRectangle(graphics, displayRectangle);
			}
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (m_NCPainter != null)
		{
			m_NCPainter.PaintNonClientAreaBuffered();
		}
		((Panel)this).OnResize(e);
		RefreshTextClientRectangle();
		SetRegion(bResize: true);
	}

	protected override void OnChangeUICues(UICuesEventArgs e)
	{
		((Control)this).OnChangeUICues(e);
		if (m_ShowFocusRectangle)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (m_ShowFocusRectangle)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (m_ShowFocusRectangle)
		{
			((Control)this).Refresh();
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
		if ((int)e.Button == 1048576)
		{
			SetMouseDown(mouseDown: true);
		}
		((Control)this).OnMouseDown(e);
	}

	public void SetMouseDown(bool mouseDown)
	{
		if (m_MouseDown != mouseDown)
		{
			m_MouseDown = mouseDown;
			if (m_StyleMouseDown.Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseUp((Control)(object)this, e);
		}
		SetMouseDown(mouseDown: false);
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
		SetMouseOver(mouseOver: true);
	}

	public void SetMouseOver(bool mouseOver)
	{
		if (m_MouseOver != mouseOver)
		{
			m_MouseOver = mouseOver;
			if (m_StyleMouseOver.Custom)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (m_TextMarkup != null)
		{
			m_TextMarkup.MouseLeave((Control)(object)this);
		}
		SetMouseOver(mouseOver: false);
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
		if (_TextMarkupEnabled && MarkupParser.IsMarkup(ref text))
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
		if (m_Style != null)
		{
			m_Style.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		m_Style = new ItemStyle();
		m_Style.VisualPropertyChanged += OnVisualPropertyChanged;
		RefreshStyleSystemColors();
	}

	public void ResetMouseTracking()
	{
		m_MouseOver = false;
		m_MouseDown = false;
		((Control)this).Refresh();
	}

	public void ResetStyleMouseOver()
	{
		if (m_StyleMouseOver != null)
		{
			m_StyleMouseOver.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		m_StyleMouseOver = new ItemStyle();
		m_StyleMouseOver.VisualPropertyChanged += OnVisualPropertyChanged;
	}

	public void ResetStyleMouseDown()
	{
		if (m_StyleMouseDown != null)
		{
			m_StyleMouseDown.VisualPropertyChanged -= OnVisualPropertyChanged;
		}
		m_StyleMouseDown = new ItemStyle();
		m_StyleMouseDown.VisualPropertyChanged += OnVisualPropertyChanged;
	}

	protected virtual void OnAntiAliasChanged()
	{
	}

	protected ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(m_ColorSchemeStyle) && GlobalManager.Renderer is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return m_ColorScheme;
	}

	protected virtual void OnColorSchemeChanged()
	{
		RefreshStyleSystemColors();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			if (!BarFunctions.IsHandleValid((Control)(object)this))
			{
				return proposedSize;
			}
			Size autoSize = GetAutoSize();
			if (autoSize.IsEmpty)
			{
				return proposedSize;
			}
			return autoSize;
		}
		return ((Control)this).GetPreferredSize(proposedSize);
	}

	public Size GetAutoSize()
	{
		return GetAutoSize(0);
	}

	public Size GetAutoSize(int preferedWidth)
	{
		if (m_TextMarkup == null && ((Control)this).Text.Length == 0)
		{
			return new Size(16, 16);
		}
		Size result = Size.Empty;
		if (m_TextMarkup != null)
		{
			result = ((preferedWidth != 0) ? GetMarkupSize(preferedWidth) : m_TextMarkup.Bounds.Size);
			result.Width += 4;
			result.Height += 6;
		}
		else if (((Control)this).Text.Length > 0)
		{
			Font font = ((Control)this).Font;
			if (m_Style.Font != null)
			{
				font = m_Style.Font;
			}
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				result = ((preferedWidth > 0) ? TextDrawing.MeasureString(val, ((Control)this).Text, font, preferedWidth) : TextDrawing.MeasureString(val, ((Control)this).Text, font));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			result.Width += 2;
			result.Height += 2;
		}
		if (result.IsEmpty)
		{
			return result;
		}
		result.Width += m_Style.MarginLeft + m_Style.MarginRight;
		result.Height += m_Style.MarginTop + m_Style.MarginBottom;
		return result;
	}

	protected virtual void RefreshTextClientRectangle()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected I4, but got Unknown
		Rectangle clientTextRectangle = ((Control)this).DisplayRectangle;
		if (m_TextDockConstrained)
		{
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
		}
		m_ClientTextRectangle = clientTextRectangle;
		ResizeMarkup();
	}

	private Size GetMarkupSize(int proposedWidth)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (m_TextMarkup != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, proposedWidth, 500);
			rectangle.Inflate(-2, -2);
			rectangle.Inflate(-(m_Style.MarginLeft + m_Style.MarginRight), -(m_Style.MarginTop + m_Style.MarginBottom));
			if (IsThemed)
			{
				rectangle.Height -= 4;
			}
			Graphics val = ((Control)this).CreateGraphics();
			BodyElement bodyElement = MarkupParser.Parse(((Control)this).Text);
			try
			{
				if (m_AntiAlias)
				{
					val.SmoothingMode = (SmoothingMode)4;
					val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
				bodyElement.Measure(rectangle.Size, d);
				result = bodyElement.Bounds.Size;
			}
			finally
			{
				val.Dispose();
			}
		}
		return result;
	}

	public void UpdateMarkupSize()
	{
		RefreshTextClientRectangle();
		((Control)this).Invalidate();
	}

	protected virtual void ResizeMarkup()
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Invalid comparison between Unknown and I4
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Invalid comparison between Unknown and I4
		if (m_TextMarkup == null)
		{
			return;
		}
		Rectangle clientTextRectangle = ClientTextRectangle;
		clientTextRectangle.Inflate(-2, -2);
		clientTextRectangle.Inflate(-(m_Style.MarginLeft + m_Style.MarginRight), -(m_Style.MarginTop + m_Style.MarginBottom));
		if (IsThemed)
		{
			clientTextRectangle.Height -= 4;
		}
		if (clientTextRectangle.Width <= 0 || clientTextRectangle.Height <= 0)
		{
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			if (m_AntiAlias)
			{
				val.SmoothingMode = (SmoothingMode)4;
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			m_TextMarkup.Measure(clientTextRectangle.Size, d);
			if (m_MarkupUsesStyleAlignment)
			{
				if (m_TextMarkup.Bounds.Height < clientTextRectangle.Height)
				{
					if ((int)Style.LineAlignment == 1)
					{
						clientTextRectangle.Y += (clientTextRectangle.Height - m_TextMarkup.Bounds.Height) / 2;
						clientTextRectangle.Height = m_TextMarkup.Bounds.Height;
					}
					else if ((int)Style.LineAlignment == 2)
					{
						clientTextRectangle.Y = clientTextRectangle.Bottom - m_TextMarkup.Bounds.Height;
						clientTextRectangle.Height = m_TextMarkup.Bounds.Height;
					}
				}
				if (m_TextMarkup.Bounds.Width < clientTextRectangle.Width)
				{
					if ((int)Style.Alignment == 1)
					{
						clientTextRectangle.X += (clientTextRectangle.Width - m_TextMarkup.Bounds.Width) / 2;
						clientTextRectangle.Width = m_TextMarkup.Bounds.Width;
					}
					else if (((int)Style.Alignment == 2 && (int)((Control)this).RightToLeft == 0) || ((int)Style.Alignment == 0 && (int)((Control)this).RightToLeft == 1))
					{
						clientTextRectangle.X = clientTextRectangle.Right - m_TextMarkup.Bounds.Width;
						clientTextRectangle.Width = m_TextMarkup.Bounds.Width;
					}
				}
			}
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

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		RefreshTextClientRectangle();
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		RefreshTextClientRectangle();
		if (m_TextDockConstrained && ((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	public void ApplyPanelStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		m_Style.Border = eBorderType.SingleLine;
		m_Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		m_Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		m_Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		m_Style.GradientAngle = m_ColorScheme.PanelBackgroundGradientAngle;
		m_Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		m_Style.Alignment = (StringAlignment)1;
		m_Style.LineAlignment = (StringAlignment)1;
		((Control)this).Refresh();
	}

	public void ApplyButtonStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		Style.Alignment = (StringAlignment)1;
		Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		Style.BackColor2.ColorSchemePart = eColorSchemePart.BarBackground2;
		Style.Border = eBorderType.SingleLine;
		Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		Style.GradientAngle = 90;
		StyleMouseDown.Alignment = (StringAlignment)1;
		StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		StyleMouseOver.Alignment = (StringAlignment)1;
		StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		((Control)this).Refresh();
	}

	public void ApplyLabelStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		TypeDescriptor.GetProperties(Style)["Alignment"].SetValue(Style, (object)(StringAlignment)1);
		TypeDescriptor.GetProperties(Style.BackColor1)["ColorSchemePart"].SetValue(Style.BackColor1, eColorSchemePart.BarBackground);
		TypeDescriptor.GetProperties(Style.BorderColor)["ColorSchemePart"].SetValue(Style.BorderColor, eColorSchemePart.BarDockedBorder);
		TypeDescriptor.GetProperties(Style.ForeColor)["ColorSchemePart"].SetValue(Style.ForeColor, eColorSchemePart.ItemText);
		TypeDescriptor.GetProperties(Style)["GradientAngle"].SetValue(Style, 90);
		((Control)this).Refresh();
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

	private BaseRenderer GetRenderer()
	{
		if (GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (m_DefaultRenderer == null)
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_DefaultRenderer;
	}

	void INonClientControl.BaseWndProc(ref Message m)
	{
		((ScrollableControl)this).WndProc(ref m);
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
		((ScrollableControl)this).OnPaintBackground(e);
	}

	Point INonClientControl.PointToScreen(Point client)
	{
		return ((Control)this).PointToScreen(client);
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle r)
	{
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle r)
	{
	}

	void INonClientControl.RenderNonClient(Graphics g)
	{
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
