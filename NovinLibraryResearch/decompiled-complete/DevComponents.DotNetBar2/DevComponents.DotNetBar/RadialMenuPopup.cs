using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class RadialMenuPopup : Control
{
	private const uint च = 2147483648u;

	private const uint छ = 67108864u;

	private const uint ज = 33554432u;

	private const uint झ = 8u;

	private const uint ञ = 128u;

	private bool __19DA;

	private RadialMenuContainer __19DB;

	private Bitmap __19DC;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private ColorScheme լ;

	public RadialMenuContainer DisplayItem
	{
		get
		{
			return __19DB;
		}
		set
		{
			if (value != __19DB)
			{
				RadialMenuContainer oldValue = __19DB;
				__19DB = value;
				OnDisplayItemChanged(oldValue, value);
			}
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 524424;
			createParams.Caption = "";
			return createParams;
		}
	}

	public Bitmap ContentImage
	{
		get
		{
			return __19DC;
		}
		set
		{
			__19DC = value;
			if (__19DC != null && ((Image)__19DC).Size != ((Control)this).Size)
			{
				((Control)this).Size = ((Image)__19DC).Size;
			}
			_19DD(__19DC);
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
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

	public RadialMenuPopup()
	{
		((Control)this).Text = "RadialMenuPopup";
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
	}

	protected override void OnInvalidated(InvalidateEventArgs e)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		if (!((Control)this).IsHandleCreated || ((Control)this).IsDisposed || __19DB == null || __19DA)
		{
			return;
		}
		__19DA = true;
		try
		{
			if (__19DB.NeedRecalcSize)
			{
				__19DB.RecalcSize();
			}
			Bitmap contentImage = ContentImage;
			Bitmap val = null;
			bool flag = false;
			if (contentImage != null && ((Image)contentImage).Width == __19DB.WidthInternal && ((Image)contentImage).Height == __19DB.HeightInternal)
			{
				val = contentImage;
				flag = true;
			}
			else
			{
				if (contentImage != null)
				{
					((Image)contentImage).Dispose();
				}
				val = new Bitmap(__19DB.WidthInternal, __19DB.HeightInternal, (PixelFormat)2498570);
			}
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				if (flag)
				{
					val2.Clear(Color.Transparent);
				}
				val2.SmoothingMode = (SmoothingMode)2;
				val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				ItemPaintArgs p = _0A92(val2);
				__19DB.Paint(p);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			ContentImage = val;
		}
		finally
		{
			__19DA = false;
		}
		((Control)this).OnInvalidated(e);
	}

	internal ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		if (__19DB.DesignMode)
		{
			ISite site = ಌ();
			if (site != null && site.DesignMode)
			{
				itemPaintArgs.DesignerSelection = true;
			}
		}
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
	}

	private ISite ಌ()
	{
		ISite site = null;
		if (site == null && __19DB != null)
		{
			BaseItem baseItem = __19DB;
			while (site == null && baseItem != null)
			{
				if (baseItem.Site != null && baseItem.Site.DesignMode)
				{
					site = baseItem.Site;
				}
				else
				{
					baseItem = baseItem.Parent;
				}
			}
		}
		return site;
	}

	protected virtual void OnDisplayItemChanged(RadialMenuContainer oldValue, RadialMenuContainer newValue)
	{
		if (oldValue != null)
		{
			oldValue.ContainerControl = null;
		}
		if (newValue != null)
		{
			newValue.ContainerControl = this;
		}
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		_19DD(__19DC);
	}

	private void _19DD(Bitmap _19DE)
	{
		if (!((Control)this).IsHandleCreated)
		{
			return;
		}
		IntPtr dC = _089F.GetDC(IntPtr.Zero);
		IntPtr intPtr = _0EFE.CreateCompatibleDC(dC);
		IntPtr zero = IntPtr.Zero;
		IntPtr zero2 = IntPtr.Zero;
		try
		{
			zero = _19DE.GetHbitmap(Color.FromArgb(0));
			zero2 = _0EFE.SelectObject(intPtr, zero);
			Size psize = ((Image)_19DE).Size;
			Point pprSrc = new Point(0, 0);
			Point pptDst = new Point(((Control)this).Left, ((Control)this).Top);
			_089F.BLENDFUNCTION pblend = default(_089F.BLENDFUNCTION);
			pblend.BlendOp = 0;
			pblend.BlendFlags = 0;
			pblend.SourceConstantAlpha = byte.MaxValue;
			pblend.AlphaFormat = 1;
			_089F.UpdateLayeredWindow(((Control)this).Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
			_0EFE.ReleaseDC(IntPtr.Zero, dC);
			if (zero != IntPtr.Zero)
			{
				_0EFE.SelectObject(intPtr, zero2);
				_0EFE.DeleteObject(zero);
			}
			_0EFE.DeleteDC(intPtr);
		}
		catch (Exception)
		{
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Point mousePos = ((Control)this).PointToClient(Control.MousePosition);
		InternalOnClick(Control.MouseButtons, mousePos);
		((Control)this).OnClick(e);
	}

	protected virtual void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		__19DB.InternalClick(mb, mousePos);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		__19DB.InternalDoubleClick(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_0A81(e);
		((Control)this).OnKeyDown(e);
	}

	internal virtual void _0A81(KeyEventArgs ؿ)
	{
		__19DB.InternalKeyDown(ؿ);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		__19DB.InternalMouseDown(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		__19DB.InternalMouseHover();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		__19DB.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		__19DB.InternalMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		InternalMouseMove(e);
	}

	protected virtual void InternalMouseMove(MouseEventArgs e)
	{
		__19DB.InternalMouseMove(e);
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

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (լ == null)
		{
			լ = new ColorScheme(eDotNetBarStyle.Metro);
		}
		return լ;
	}
}
