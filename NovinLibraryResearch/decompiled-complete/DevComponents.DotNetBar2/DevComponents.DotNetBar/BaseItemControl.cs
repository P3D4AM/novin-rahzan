using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public abstract class BaseItemControl : Control
{
	private BaseItem _᭑;

	private ElementStyle _0599;

	private ColorScheme լ;

	private bool _09FA;

	private bool ե = true;

	private bool _ᒐ = true;

	private bool _ᒖ = true;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private bool _᭒;

	private int ਝ;

	internal bool _᭐
	{
		get
		{
			PropertyInfo property = ((object)this).GetType().GetProperty("ValidationCancelled", BindingFlags.Instance | BindingFlags.NonPublic);
			if ((object)property != null)
			{
				return (bool)property.GetValue(this, null);
			}
			return false;
		}
	}

	protected virtual BaseItem HostItem
	{
		get
		{
			return _᭑;
		}
		set
		{
			_᭑ = value;
			if (_᭑ != null)
			{
				_᭑.Displayed = true;
				_᭑.ContainerControl = this;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CallBasePaintBackground
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

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether control displays focus cues when focused.")]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return _ᒖ;
		}
		set
		{
			_ᒖ = value;
			if (((Control)this).Focused)
			{
				((Control)this).Invalidate();
			}
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

	[Description("Gets or sets bar background style.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackgroundStyle => _0599;

	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Description("Gets or sets Bar Color Scheme.")]
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
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
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

	protected virtual MouseButtons MouseDownFocusButtons => (MouseButtons)1048576;

	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[Browsable(true)]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return _᭑.Style;
		}
		set
		{
			_᭑.Style = value;
			if (!BarFunctions.IsOffice2007Style(value))
			{
				GetColorScheme().ߕ = value;
			}
			RecalcLayout();
		}
	}

	[Browsable(false)]
	public bool IsUpdateSuspended => ਝ > 0;

	public BaseItemControl()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
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
		((Control)this).SetStyle((ControlStyles)4096, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		լ = new ColorScheme(eDotNetBarStyle.Office2007);
		_0599 = new ElementStyle();
		_0599.SetColorScheme(լ);
		_0599.StyleChanged += ઐ;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _᭑ != null)
		{
			_᭑.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		if (_᭑ != null)
		{
			_᭑.NotifyScaleItem(factor);
			RecalcLayout();
		}
		((Control)this).ScaleControl(factor, specified);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (_᭑ != null)
		{
			_᭑.UpdateBindings();
		}
	}

	protected bool GetDesignMode()
	{
		if (!_09FA)
		{
			return ((Component)this).DesignMode;
		}
		return _09FA;
	}

	protected virtual ElementStyle GetBackgroundStyle()
	{
		return _0599;
	}

	internal void ᒶ(bool _᭓)
	{
		_09FA = _᭓;
		if (_᭑ != null)
		{
			_᭑.SetDesignMode(_᭓);
		}
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (_᭑ != null && BarFunctions.IsOffice2007Style(_᭑.EffectiveStyle))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return լ;
	}

	internal void ઔ(PaintEventArgs ؿ)
	{
		((Control)this).OnPaint(ؿ);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if ((((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent) && _ᒐ)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (_0599 != null)
		{
			_0599.SetColorScheme(GetColorScheme());
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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
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
			if (ե)
			{
				graphics.SmoothingMode = (SmoothingMode)2;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(backgroundStyle, e.Graphics, clientRectangle);
			ElementStyleDisplay.Paint(e2);
			if (ե)
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
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		Graphics graphics = e.Graphics;
		if (ե)
		{
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(graphics);
		itemPaintArgs.ClipRectangle = e.ClipRectangle;
		if (_᭑ != null)
		{
			_᭑.Paint(itemPaintArgs);
		}
		if (ե)
		{
			graphics.SmoothingMode = smoothingMode;
			graphics.TextRenderingHint = textRenderingHint;
		}
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

	protected virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
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
		return ਜ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_0599.StyleChanged -= ઐ;
		_0599 = new ElementStyle();
		_0599.StyleChanged += ઐ;
		OnBackgroundStyleChanged();
		((Control)this).Invalidate();
	}

	protected virtual void OnBackgroundStyleChanged()
	{
	}

	private void ઐ(object _٠, EventArgs ؿ)
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Component)this).DesignMode && _᭑ != null)
		{
			_᭑.SetDesignMode(((Component)this).DesignMode);
		}
	}

	public virtual void RecalcLayout()
	{
		if (_᭑ != null)
		{
			Rectangle itemBounds = GetItemBounds();
			_᭑.Bounds = itemBounds;
			RecalcSize();
			_᭑.Bounds = itemBounds;
			((Control)this).Invalidate();
		}
	}

	protected virtual void RecalcSize()
	{
		_᭑.RecalcSize();
	}

	protected virtual Rectangle GetItemBounds()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ᝈ.ᝐ(BackgroundStyle);
		clientRectangle.Width -= ᝈ.ᝍ(BackgroundStyle);
		clientRectangle.Y += ᝈ._1753(BackgroundStyle);
		clientRectangle.Height -= ᝈ.ᝏ(BackgroundStyle);
		return clientRectangle;
	}

	protected override void OnResize(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnResize(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		_᭑.Text = ((Control)this).Text;
		RecalcLayout();
		((Control)this).OnTextChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		BarUtilities.ડ(_᭑);
		RecalcLayout();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		_᭑.Enabled = ((Control)this).Enabled;
		((Control)this).OnEnabledChanged(e);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		if (!_᭒)
		{
			_᭑.OnGotFocus();
		}
		_᭒ = false;
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		_᭑.OnLostFocus();
		((Control)this).OnLostFocus(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_᭑.InternalMouseEnter();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		_᭑.InternalMouseMove(e);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_᭑.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		_᭑.InternalMouseHover();
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if ((e.Button & MouseDownFocusButtons) != 0 && !((Control)this).Focused && ((Control)this).GetStyle((ControlStyles)512))
		{
			_᭒ = true;
			((Control)this).Select();
		}
		_᭑.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		_᭑.InternalMouseUp(e);
		((Control)this).OnMouseUp(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_᭑.InternalClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_᭑.InternalDoubleClick(Control.MouseButtons, ((Control)this).PointToClient(Control.MousePosition));
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_᭑.InternalKeyDown(e);
		((Control)this).OnKeyDown(e);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (Control.IsMnemonic(charCode, _᭑.Text) && ProcessAccelerator())
		{
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected virtual bool ProcessAccelerator()
	{
		_᭑.RaiseClick(eEventSource.Keyboard);
		return true;
	}

	public void BeginUpdate()
	{
		ਝ++;
	}

	public void EndUpdate()
	{
		EndUpdate(callRecalcLayout: true);
	}

	public void EndUpdate(bool callRecalcLayout)
	{
		if (ਝ > 0)
		{
			ਝ--;
			if (ਝ == 0 && callRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}
}
