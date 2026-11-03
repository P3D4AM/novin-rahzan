using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class RibbonForm : Form
{
	private IContainer Պ;

	private int Ừ = 6;

	private int ừ = 6;

	private int Ử = 6;

	private int ử = 6;

	protected int m_DisplayRectangleReductionTop = 1;

	protected int m_DisplayRectangleReductionBottom = 2;

	protected int m_DisplayRectangleReductionHorizontal = 5;

	private RibbonControl ᰘ;

	private FormWindowState Ữ;

	private bool ữ;

	private bool Ự;

	private bool ự = true;

	private bool Ỳ = true;

	private Point ỳ = Point.Empty;

	protected bool m_EnableCustomStyle = true;

	private int Ỵ;

	private bool ỵ;

	private bool Ỷ;

	private bool ỷ = true;

	private bool _Ỹ;

	private BufferedBitmap Ề;

	private Rectangle[] ỹ;

	private Point ề = Point.Empty;

	private bool Ỻ;

	private bool ỻ;

	private bool _Ỽ = true;

	private bool _ỽ;

	private bool _Ỿ = true;

	private bool _ỿ = true;

	internal int ụ => m_DisplayRectangleReductionHorizontal;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool BackColorSet
	{
		get
		{
			return _Ỹ;
		}
		set
		{
			_Ỹ = value;
		}
	}

	public override Color BackColor
	{
		get
		{
			return ((Form)this).BackColor;
		}
		set
		{
			((Form)this).BackColor = value;
			if (!(value == SystemColors.Control) || !((Component)this).DesignMode)
			{
				_Ỹ = true;
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether Windows Vista Glass support is enabled when form is running on Windows Vista with Glass support.")]
	[DefaultValue(true)]
	public virtual bool EnableGlass
	{
		get
		{
			return ỷ;
		}
		set
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Invalid comparison between Unknown and I4
			if (ỷ == value)
			{
				return;
			}
			ỷ = value;
			if (ᰘ != null)
			{
				ᰘ.CanSupportGlass = IsGlassEnabled;
			}
			if (ỵ && ((Control)this).IsHandleCreated && !((Component)this).DesignMode)
			{
				bool flag = false;
				if (((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null && (int)((Form)this).ActiveMdiChild.WindowState == 2)
				{
					flag = true;
				}
				_089F.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
				UpdateFormGlassState();
				if (flag && ((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null)
				{
					((Form)this).ActiveMdiChild.WindowState = (FormWindowState)2;
				}
			}
		}
	}

	[Browsable(false)]
	protected virtual RibbonControl RibbonControl
	{
		get
		{
			return ᰘ;
		}
		set
		{
			ᰘ = value;
			if (ᰘ != null)
			{
				ἂ();
			}
		}
	}

	protected virtual bool EnableGlassExtend => true;

	internal int Ủ => Ỵ;

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	public FormWindowState WindowState
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (ữ)
			{
				return Ữ;
			}
			return ((Form)this).WindowState;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			((Form)this).WindowState = value;
			if ((int)value == 2 && !((Control)this).IsHandleCreated && !Ự)
			{
				((Form)this).StartPosition = (FormStartPosition)1;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool EnableCustomStyle
	{
		get
		{
			return m_EnableCustomStyle;
		}
		set
		{
			m_EnableCustomStyle = value;
			OnEnableCustomStyleChanged();
		}
	}

	[Category("Border")]
	[DefaultValue(6)]
	[Browsable(true)]
	[Description("Indicates top left rounded corner size")]
	public int TopLeftCornerSize
	{
		get
		{
			return Ừ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			Ừ = value;
			((Control)this).Region = null;
		}
	}

	[Browsable(true)]
	[DefaultValue(6)]
	[Description("Indicates top right rounded corner size")]
	[Category("Border")]
	public int TopRightCornerSize
	{
		get
		{
			return ừ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			ừ = value;
			((Control)this).Region = null;
		}
	}

	[Category("Border")]
	[Description("Indicates bottom left rounded corner size")]
	[DefaultValue(6)]
	[Browsable(true)]
	public int BottomLeftCornerSize
	{
		get
		{
			return Ử;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			Ử = value;
			((Control)this).Region = null;
		}
	}

	[DefaultValue(6)]
	[Description("Indicates bottom right rounded corner size")]
	[Browsable(true)]
	[Category("Border")]
	public int BottomRightCornerSize
	{
		get
		{
			return ử;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			ử = value;
			((Control)this).Region = null;
		}
	}

	protected virtual bool PaintClientBorder
	{
		get
		{
			if (_Ỽ)
			{
				return IsCustomFormStyleEnabled();
			}
			return false;
		}
	}

	protected virtual bool PaintRibbonCaption
	{
		get
		{
			if (_Ỽ)
			{
				return ᰘ != null;
			}
			return false;
		}
	}

	internal int ủ => GetBorderColors(0).Length;

	[Browsable(true)]
	[Description("Indicates whether 3D MDI border is removed.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool FlattenMDIBorder
	{
		get
		{
			return ự;
		}
		set
		{
			ự = value;
			ἆ();
		}
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			Rectangle displayRectangle = ((ScrollableControl)this).DisplayRectangle;
			return ReduceDisplayRectangle(displayRectangle);
		}
	}

	protected virtual bool HasCustomRegion
	{
		get
		{
			if (IsCustomFormStyleEnabled())
			{
				return !IsGlassEnabled;
			}
			return false;
		}
	}

	internal bool Ứ
	{
		get
		{
			return Ỳ;
		}
		set
		{
			Ỳ = value;
		}
	}

	[Browsable(false)]
	public bool IsGlassEnabled
	{
		get
		{
			if (((Component)this).DesignMode || ((Form)this).IsMdiChild || StyleManager.IsMetro(StyleManager.Style) || BarFunctions.IsWindows10)
			{
				return false;
			}
			if (ỵ)
			{
				return ỷ;
			}
			return false;
		}
	}

	protected virtual bool IsSizable
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if (((int)((Form)this).FormBorderStyle == 4 || (int)((Form)this).FormBorderStyle == 6) && (int)WindowState == 0)
			{
				return true;
			}
			return false;
		}
	}

	internal bool ứ
	{
		get
		{
			return ỻ;
		}
		set
		{
			ỻ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool NonClientPaintEnabled
	{
		get
		{
			return _Ỽ;
		}
		set
		{
			_Ỽ = value;
		}
	}

	[Description("Indicates whether form BackgroundImage is pulled up and rendered under form caption as well.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool BackgroundImageUnderCaption
	{
		get
		{
			return _ỽ;
		}
		set
		{
			if (value != _ỽ)
			{
				bool ỽ = _ỽ;
				_ỽ = value;
				OnBackgroundImageUnderCaptionChanged(ỽ, value);
			}
		}
	}

	[Description("Indicates whether form icon is drawn. This is different from ShowIcon property which when set to false also removes the form icon from task-bar. Setting this property to false will keep form icon in task-bar but will hide on the form.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool RenderFormIcon
	{
		get
		{
			return _Ỿ;
		}
		set
		{
			if (value != _Ỿ)
			{
				bool ỿ = _Ỿ;
				_Ỿ = value;
				OnRenderFormIconChanged(ỿ, value);
			}
		}
	}

	[Description("Indicates whether form Text is rendered in form caption.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool RenderFormText
	{
		get
		{
			return _ỿ;
		}
		set
		{
			if (value != _ỿ)
			{
				bool ỿ = _ỿ;
				_ỿ = value;
				OnRenderFormTextChanged(ỿ, value);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		if (Ề != null)
		{
			Ề.Dispose();
			Ề = null;
		}
		((Form)this).Dispose(disposing);
	}

	private void ـ()
	{
	}

	public RibbonForm()
	{
		ỵ = Ἃ();
		ـ();
		UpdateFormControlStyle();
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateFormStyling();
		ἂ();
	}

	protected virtual void UpdateFormControlStyle()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (IsCustomFormStyleEnabled())
		{
			if (!((Control)this).GetStyle((ControlStyles)8192))
			{
				((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag | 2), true);
			}
		}
		else if (((Control)this).GetStyle((ControlStyles)8192))
		{
			((Control)this).SetStyle((ControlStyles)(0x2010 | DisplayHelp.DoubleBufferFlag), false);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void ResetBackColor()
	{
		((Control)this).ResetBackColor();
		_Ỹ = false;
	}

	private void ἀ()
	{
		if (_Ỹ)
		{
			return;
		}
		if (IsCustomFormStyleEnabled())
		{
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Color backColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
				((Form)this).BackColor = backColor;
			}
		}
		else
		{
			((Form)this).BackColor = SystemColors.Control;
		}
	}

	protected virtual void UpdateFormStyling()
	{
		if (HasCustomRegion)
		{
			((Control)this).Region = GetRegion();
		}
		else
		{
			((Control)this).Region = null;
		}
		ἀ();
		if (IsGlassEnabled)
		{
			if (StyleManager.IsMetro(StyleManager.PreviousStyle))
			{
				Ἂ();
			}
			else
			{
				ἃ();
			}
		}
		ἁ();
		((Control)this).Invalidate();
	}

	private void ἁ()
	{
		if (!((Form)this).IsMdiContainer)
		{
			return;
		}
		MdiClient mdiClient = BarFunctions.GetMdiClient((Form)(object)this);
		if (mdiClient != null)
		{
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				((Control)mdiClient).BackgroundImage = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.MdiClientBackgroundImage;
			}
			if (StyleManager.IsMetro(StyleManager.Style) && mdiClient != null)
			{
				((Control)mdiClient).BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
			}
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).OnLoad(e);
		UpdateFormStyling();
		Ự = true;
		if (ữ)
		{
			((Form)this).WindowState = Ữ;
			ữ = false;
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		((Form)this).OnActivated(e);
		ἄ();
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Form)this).OnDeactivate(e);
		ἄ();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Form)this).OnTextChanged(e);
		ἄ();
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		ἂ();
		UpdateFormControlStyle();
		((Form)this).OnStyleChanged(e);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Dpi.SetScaling(factor);
		((Form)this).ScaleControl(factor, specified);
		if (ᰘ != null)
		{
			ᰘ.RecalcLayout();
		}
	}

	private void ἂ()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		SystemCaptionItem systemCaptionItem = Ꮧ();
		if (systemCaptionItem != null)
		{
			systemCaptionItem.MinimizeVisible = ((Form)this).MinimizeBox;
			if (IsGlassEnabled && ((Form)this).MinimizeBox && (int)((Form)this).FormBorderStyle == 4)
			{
				systemCaptionItem.RestoreMaximizeVisible = true;
			}
			else
			{
				systemCaptionItem.RestoreMaximizeVisible = ((Form)this).MaximizeBox;
			}
			systemCaptionItem.HelpVisible = ((Form)this).HelpButton;
			systemCaptionItem.Visible = ((Form)this).ControlBox;
			ᰘ.RecalcLayout();
		}
	}

	private SystemCaptionItem Ꮧ()
	{
		if (ᰘ != null && IsCustomFormStyleEnabled())
		{
			return ᰘ.RibbonStrip._൧;
		}
		return null;
	}

	internal void ἃ()
	{
		if (IsGlassEnabled && BarFunctions.IsHandleValid((Control)(object)this) && EnableGlassExtend)
		{
			int num = 0;
			if (ᰘ == null || !ᰘ.CaptionVisible)
			{
				Ἇ ἇ = Ꮛ();
				num = ἇ.ἑ;
			}
			else
			{
				num = ᰘ.RibbonStrip.ග();
			}
			_0EFE._0F13(((Control)this).Handle, num);
			Ỵ = num;
		}
	}

	internal void ἄ()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		if (!IsCustomFormStyleEnabled())
		{
			return;
		}
		if (ᰘ != null && ᰘ.RibbonStrip.CaptionVisible)
		{
			((Control)ᰘ.RibbonStrip).Invalidate(ᰘ.RibbonStrip._0D65);
		}
		if (IsGlassEnabled)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-m_DisplayRectangleReductionHorizontal * 2, -(m_DisplayRectangleReductionTop * 2 + m_DisplayRectangleReductionBottom * 2));
		Region val = new Region(((Control)this).ClientRectangle);
		GraphicsPath formPath = GetFormPath(clientRectangle);
		try
		{
			val.Exclude(formPath);
			((Control)this).Invalidate(val);
		}
		finally
		{
			((IDisposable)formPath)?.Dispose();
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (e.Control is RibbonControl)
		{
			ᰘ = e.Control as RibbonControl;
			ᰘ.CanSupportGlass = IsGlassEnabled;
			ἂ();
			ἃ();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control == ᰘ)
		{
			ᰘ.CanSupportGlass = false;
			ᰘ = null;
		}
		((Control)this).OnControlRemoved(e);
	}

	protected override void OnMdiChildActivate(EventArgs e)
	{
		ἄ();
		((Form)this).OnMdiChildActivate(e);
		if (IsGlassEnabled)
		{
			IntPtr menu = _0EFE.GetMenu(((Control)this).Handle);
			if (menu != IntPtr.Zero)
			{
				_0EFE.SetMenu(((Control)this).Handle, IntPtr.Zero);
			}
		}
	}

	private bool ἅ()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)WindowState == 2 && RibbonControl != null)
		{
			return !IsGlassEnabled;
		}
		return false;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (ἅ() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal) && e.Y <= m_DisplayRectangleReductionHorizontal)
		{
			MouseEventArgs ؿ = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				ؿ = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.ඬ(ؿ);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (ἅ() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal) && e.Y <= m_DisplayRectangleReductionHorizontal)
		{
			MouseEventArgs ؿ = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				ؿ = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.ථ(ؿ);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (ἅ() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal))
		{
			MouseEventArgs ؿ = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				ؿ = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.ත(ؿ);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (ἅ())
		{
			Point point = ((Control)this).PointToClient(Control.MousePosition);
			if (point.X <= m_DisplayRectangleReductionHorizontal || point.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				RibbonControl.RibbonStrip.ද(e);
			}
		}
		((Control)this).OnClick(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Form)this).IsMdiContainer)
		{
			BarUtilities.ChangeMDIClientBorder((Form)(object)this, ự);
		}
		ἃ();
		((Form)this).OnHandleCreated(e);
	}

	private void ἆ()
	{
		if (((Form)this).IsMdiContainer && ((Control)this).IsHandleCreated)
		{
			BarUtilities.ChangeMDIClientBorder((Form)(object)this, ự);
		}
	}

	protected virtual void OnEnableCustomStyleChanged()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).Region = null;
		FormBorderStyle formBorderStyle = ((Form)this).FormBorderStyle;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).FormBorderStyle = formBorderStyle;
		if (IsGlassEnabled)
		{
			_0EFE._0F13(((Control)this).Handle, 0);
		}
	}

	protected virtual bool IsCustomFormStyleEnabled()
	{
		return m_EnableCustomStyle;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (HasCustomRegion && ((Control)this).Region == null)
		{
			((Control)this).Region = GetRegion();
		}
		if (IsGlassEnabled && Ỵ > 0)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, Ỵ), (CombineMode)4);
		}
		((Form)this).OnPaint(e);
		if (PaintClientBorder && !IsGlassEnabled)
		{
			PaintFormBorder(e);
		}
		if (PaintRibbonCaption && !IsGlassEnabled)
		{
			PaintCaptionParts(e.Graphics);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (IsGlassEnabled && Ỵ > 0)
		{
			graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, Ỵ), (CombineMode)4);
		}
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (_ỽ && ((Control)this).BackgroundImage != null)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			Ἇ ἇ = Ꮛ();
			clientRectangle.Y -= ἇ.ἐ;
			clientRectangle.Height += ἇ.ἐ;
			clientRectangle.X -= ἇ.ἒ;
			BarFunctions._0982(graphics, clientRectangle, ((Control)this).BackgroundImage, GetBackgroundImagePosition(((Control)this).BackgroundImageLayout), 255);
		}
		else
		{
			if (((Control)this).BackgroundImage != null)
			{
				BarFunctions._0982(graphics, ((Control)this).ClientRectangle, ((Control)this).BackgroundImage, GetBackgroundImagePosition(((Control)this).BackgroundImageLayout), 255);
			}
			((ScrollableControl)this).OnPaintBackground(e);
		}
	}

	protected eStyleBackgroundImage GetBackgroundImagePosition(ImageLayout layout)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		if ((int)layout == 2)
		{
			return eStyleBackgroundImage.Center;
		}
		if ((int)layout == 0)
		{
			return eStyleBackgroundImage.TopLeft;
		}
		if ((int)layout == 3)
		{
			return eStyleBackgroundImage.Stretch;
		}
		if ((int)layout == 1)
		{
			return eStyleBackgroundImage.Tile;
		}
		if ((int)layout == 4)
		{
			return eStyleBackgroundImage.Zoom;
		}
		return eStyleBackgroundImage.TopLeft;
	}

	protected virtual void PaintFormBorder(PaintEventArgs e)
	{
		PaintFormBorder(e.Graphics, ủ, borderOnly: false);
	}

	protected virtual void PaintNonClientBackgroundImage(Graphics g, Rectangle targetRectangle)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (_ỽ && ((Control)this).BackgroundImage != null)
		{
			BarFunctions._0982(g, targetRectangle, ((Control)this).BackgroundImage, GetBackgroundImagePosition(((Control)this).BackgroundImageLayout), 255);
		}
	}

	protected virtual void PaintFormBorder(Graphics g, int borderSize, bool borderOnly)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		if (IsGlassEnabled)
		{
			return;
		}
		if (!borderOnly && !((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(1, 1);
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!borderOnly)
		{
			PaintNonClientBackgroundImage(g, ((Control)this).ClientRectangle);
		}
		Color[] borderColors = GetBorderColors(borderSize);
		Rectangle bounds = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)3;
		if (!borderOnly && borderColors.Length > 1)
		{
			SolidBrush val2 = new SolidBrush(borderColors[^1]);
			try
			{
				g.FillRectangle((Brush)(object)val2, new Rectangle(0, 0, 8, 8));
				g.FillRectangle((Brush)(object)val2, new Rectangle(((Control)this).Width - 8, 0, 8, 8));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		int num = 0;
		for (int i = 0; i < borderSize; i++)
		{
			Color color = borderColors[num];
			if (borderOnly && color == ((Control)this).BackColor)
			{
				break;
			}
			GraphicsPath formPath = GetFormPath(bounds);
			try
			{
				DisplayHelp.DrawGradientPathBorder(g, formPath, color, Color.Empty, 90, 1);
			}
			finally
			{
				((IDisposable)formPath)?.Dispose();
			}
			bounds.Inflate(-1, -1);
			if (num < borderColors.Length - 1)
			{
				num++;
			}
		}
		if (!borderOnly && ᰘ != null && ((Control)ᰘ).Visible && ᰘ.Expanded && StyleManager.IsMetro(StyleManager.Style) && GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007RibbonColorTable ribbonControl = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.RibbonControl;
			Point point = ((Control)this).PointToClient(((Control)ᰘ.RibbonStrip).PointToScreen(new Point(0, ((Control)ᰘ.RibbonStrip).Height)));
			SolidBrush val3 = new SolidBrush(ribbonControl.PanelTopBackground.Start);
			try
			{
				int y = point.Y;
				int width = borderSize - 1;
				int num2 = ((Control)ᰘ).Height - ((Control)ᰘ.RibbonStrip).Height;
				Padding padding = ((Control)ᰘ).Padding;
				Rectangle rectangle = new Rectangle(1, y, width, num2 - ((Padding)(ref padding)).Bottom);
				g.FillRectangle((Brush)(object)val3, rectangle);
				rectangle.X = ((Control)this).Width - borderSize;
				g.FillRectangle((Brush)(object)val3, rectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintCaptionParts(Graphics g)
	{
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		if (ᰘ == null || !ᰘ.CaptionVisible || m_DisplayRectangleReductionHorizontal <= 2)
		{
			return;
		}
		RibbonControlRendererEventArgs ribbonControlRendererEventArgs = new RibbonControlRendererEventArgs(g, ᰘ, IsGlassEnabled);
		ribbonControlRendererEventArgs.က = ᰘ.RibbonStrip._0A92(g);
		BaseRenderer renderer = ᰘ.RibbonStrip.GetRenderer();
		if (renderer == null)
		{
			return;
		}
		Region clip = g.Clip;
		Rectangle ശ = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 3);
		eDotNetBarStyle effectiveStyle = ᰘ.EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			ശ = new Rectangle(2, 2, ((Control)this).Width - 5, ((Control)this).Height - 4);
		}
		else if (StyleManager.IsMetro(effectiveStyle))
		{
			ശ = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 3);
		}
		Region val = ᗊ(ശ);
		Rectangle rectangle = new Rectangle(ശ.X, ᰘ.RibbonStrip._0D65.Height + 5, ശ.Width, ((Control)this).Height - (ᰘ.RibbonStrip._0D65.Height + 2));
		g.SetClip(val, (CombineMode)0);
		g.SetClip(rectangle, (CombineMode)4);
		g.TranslateTransform(1f, 1f);
		renderer.DrawRibbonControlBackground(ribbonControlRendererEventArgs);
		renderer.DrawQuickAccessToolbarBackground(ribbonControlRendererEventArgs);
		if (StyleManager.IsMetro(effectiveStyle))
		{
			if (ᰘ.GetApplicationButton() != null)
			{
				ButtonItem buttonItem = ᰘ.GetApplicationButton() as ButtonItem;
				g.TranslateTransform(-1f, 0f);
				if (buttonItem != null && buttonItem.Displayed)
				{
					g.SetClip(val, (CombineMode)0);
					renderer.DrawButtonItem(new ButtonItemRendererEventArgs(g, buttonItem, ᰘ.RibbonStrip._0A92(g)));
					if (ᰘ.Expanded)
					{
						g.TranslateTransform(0f, 1f);
						renderer.DrawButtonItem(new ButtonItemRendererEventArgs(g, buttonItem, ᰘ.RibbonStrip._0A92(g)));
					}
				}
			}
			g.ResetTransform();
			if (ᰘ.TabGroups.Count > 0)
			{
				g.SetClip(val, (CombineMode)0);
				g.TranslateTransform((float)((Control)ᰘ).Left, 0f);
				ᰘ.RibbonStrip.ඐ(ᰘ.RibbonStrip._0A92(g));
				g.ResetTransform();
			}
			Office2007ColorTable colorTable = ((Office2007Renderer)renderer).ColorTable;
			if (ᰘ.Expanded && !colorTable.RibbonControl.OuterBorder.Start.IsEmpty)
			{
				Pen val2 = new Pen(colorTable.RibbonControl.OuterBorder.Start);
				try
				{
					g.DrawLine(val2, ((Control)this).Width - 5, ((Control)ᰘ.RibbonStrip).Height, ((Control)this).Width - 1, ((Control)ᰘ.RibbonStrip).Height);
					int num = ((Control)this).Width - 5;
					int height = ((Control)ᰘ).Height;
					Padding padding = ((Control)ᰘ).Padding;
					int num2 = height - ((Padding)(ref padding)).Bottom;
					int num3 = ((Control)this).Width - 1;
					int height2 = ((Control)ᰘ).Height;
					Padding padding2 = ((Control)ᰘ).Padding;
					g.DrawLine(val2, num, num2, num3, height2 - ((Padding)(ref padding2)).Bottom);
					int height3 = ((Control)ᰘ).Height;
					Padding padding3 = ((Control)ᰘ).Padding;
					int num4 = height3 - ((Padding)(ref padding3)).Bottom;
					int height4 = ((Control)ᰘ).Height;
					Padding padding4 = ((Control)ᰘ).Padding;
					g.DrawLine(val2, 1, num4, 5, height4 - ((Padding)(ref padding4)).Bottom);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			if ((StyleManager.Style == eStyle.OfficeMobile2014 || StyleManager.Style == eStyle.Office2016) && !colorTable.RibbonControl.TabsBackground.IsEmpty)
			{
				Region clip2 = g.Clip;
				g.ResetClip();
				DisplayHelp.FillRectangle(g, new Rectangle(((Control)this).Width - 14, ᰘ.RibbonStrip._0D65.Height, 14, ((Control)ᰘ.RibbonStrip).Height - ᰘ.RibbonStrip._0D65.Height + 1), colorTable.RibbonControl.TabsBackground);
				DisplayHelp.FillRectangle(g, new Rectangle(1, ᰘ.RibbonStrip._0D65.Height, 5, ((Control)ᰘ.RibbonStrip).Height - ᰘ.RibbonStrip._0D65.Height + 1), colorTable.RibbonControl.TabsBackground);
				g.Clip = clip2;
				if (clip2 != null)
				{
					clip2.Dispose();
				}
			}
		}
		g.ResetTransform();
		if (((Control)ᰘ).BackgroundImage != null && !ᰘ.RibbonStrip._൭)
		{
			int width = ((Control)this).Width - 2;
			int height5 = ((Control)ᰘ.RibbonStrip).Height;
			Padding padding5 = ((Control)ᰘ).Padding;
			Rectangle rectangle2 = new Rectangle(1, 1, width, height5 - ((Padding)(ref padding5)).Bottom);
			BarFunctions._0982(g, rectangle2, ((Control)ᰘ).BackgroundImage, ᰘ.BackgroundImagePosition, 255);
		}
		Bar bar = ἇ();
		if (bar != null && BarFunctions.IsOffice2007Style(bar.Style) && !bar.IsThemed)
		{
			renderer = bar.GetRenderer();
			if (renderer != null)
			{
				g.SetClip(val, (CombineMode)0);
				ToolbarRendererEventArgs toolbarRendererEventArgs = new ToolbarRendererEventArgs(bar, g, new Rectangle(0, ((Control)bar).Top, ((Control)this).Width, ((Control)bar).Height + 4));
				toolbarRendererEventArgs.က = bar._0A92(g);
				toolbarRendererEventArgs.က.ໟ = true;
				renderer.DrawToolbarBackground(toolbarRendererEventArgs);
			}
		}
		g.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
		val.Dispose();
	}

	private Bar ἇ()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if ((int)val.Dock == 2 && val is Bar)
			{
				return val as Bar;
			}
		}
		return null;
	}

	protected virtual Color[] GetBorderColors(int borderSize)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007FormStateColorTable office2007FormStateColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Active;
			if (!Ỳ && !((Component)this).DesignMode)
			{
				office2007FormStateColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Inactive;
			}
			return office2007FormStateColorTable.BorderColors;
		}
		return new Color[2]
		{
			Color.FromArgb(59, 90, 130),
			Color.FromArgb(177, 198, 225)
		};
	}

	protected virtual Rectangle ReduceDisplayRectangle(Rectangle r)
	{
		if (IsCustomFormStyleEnabled() && !IsGlassEnabled)
		{
			r.Inflate(-m_DisplayRectangleReductionHorizontal, 0);
			r.Y += m_DisplayRectangleReductionTop;
			r.Height -= m_DisplayRectangleReductionTop + m_DisplayRectangleReductionBottom;
		}
		return r;
	}

	protected override void OnResize(EventArgs e)
	{
		if (HasCustomRegion)
		{
			((Control)this).Region = GetRegion();
			((Control)this).Invalidate();
		}
		((Form)this).OnResize(e);
	}

	protected virtual Region GetRegion()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		bool flag = IsCustomFormStyleEnabled();
		if (flag && (int)WindowState == 2 && BarFunctions.ThemedOS && !_0EEE._0EF0)
		{
			Ἇ ἇ = Ꮛ();
			int ἒ = ἇ.ἒ;
			return ᗊ(new Rectangle(ἒ, 0, ((Control)this).Width - (ἒ * 2 + 1), ((Control)this).Height - 1));
		}
		if ((int)WindowState == 2 || !flag)
		{
			return null;
		}
		return ᗊ(new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
	}

	private Region ᗊ(Rectangle ശ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		GraphicsPath formPath = GetFormPath(ശ);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(formPath);
		formPath.Widen(SystemPens.Control);
		new Region(formPath);
		val.Union(formPath);
		formPath.Dispose();
		return val;
	}

	protected virtual GraphicsPath GetFormPath(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = bounds;
		if (StyleManager.IsMetro(StyleManager.GetEffectiveStyle()))
		{
			val.AddRectangle(rectangle);
			return val;
		}
		bool flag = (int)((Control)this).RightToLeft == 1;
		int num = (flag ? ừ : (Ừ + ((Ừ > 0) ? 1 : 0)));
		if (num > 0)
		{
			ᒇ ᒇ2 = ElementStyleDisplay.സ(rectangle, num, eCornerArc.TopLeft);
			val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		}
		else
		{
			val.AddLine(bounds.X, bounds.Y + 2, bounds.X, bounds.Y);
			val.AddLine(bounds.X, bounds.Y, bounds.X + 2, bounds.Y);
		}
		num = ((!flag) ? ừ : ((Ừ + Ừ > 0) ? 1 : 0));
		if (num > 0)
		{
			ᒇ ᒇ2 = ElementStyleDisplay.സ(rectangle, num, eCornerArc.TopRight);
			val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		}
		else
		{
			val.AddLine(bounds.Right - 2, bounds.Y, bounds.Right, bounds.Y);
			val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y + 2);
		}
		if (ử > 0)
		{
			ᒇ ᒇ2 = ElementStyleDisplay.സ(rectangle, ử, eCornerArc.BottomRight);
			val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		}
		else
		{
			val.AddLine(bounds.Right, bounds.Bottom - 2, bounds.Right, bounds.Bottom);
			val.AddLine(bounds.Right, bounds.Bottom, bounds.Right - 2, bounds.Bottom);
		}
		if (Ử > 0)
		{
			ᒇ ᒇ2 = ElementStyleDisplay.സ(rectangle, Ử, eCornerArc.BottomLeft);
			val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		}
		else
		{
			val.AddLine(bounds.X + 2, bounds.Bottom, bounds.X, bounds.Bottom);
			val.AddLine(bounds.X, bounds.Bottom, bounds.X, bounds.Bottom - 2);
		}
		val.CloseAllFigures();
		return val;
	}

	protected virtual bool WindowsMessageNCPaint(ref Message m)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (IsGlassEnabled)
		{
			return true;
		}
		if (BarFunctions.IsVista && !BarFunctions.IsWindows7 && _Ỽ)
		{
			IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
			try
			{
				Graphics val = Graphics.FromHdc(windowDC);
				try
				{
					BufferedBitmap bufferedBitmap = new BufferedBitmap(val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
					try
					{
						bufferedBitmap.Graphics.SetClip(GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)), (CombineMode)4);
						PaintFormBorder(new PaintEventArgs(bufferedBitmap.Graphics, Rectangle.Empty));
						bufferedBitmap.Render(val, GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)));
					}
					finally
					{
						bufferedBitmap.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
			}
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	protected virtual bool WindowsMessageNCCalcSize(ref Message m)
	{
		if (((Message)(ref m)).WParam == IntPtr.Zero)
		{
			_0EFE.RECT rECT = _0EFE.RECT.FromRectangle(GetClientRectangle(((_0EFE.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.RECT))).ToRectangle()));
			Marshal.StructureToPtr((object)rECT, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = IntPtr.Zero;
		}
		else
		{
			_0EFE.NCCALCSIZE_PARAMS nCCALCSIZE_PARAMS = (_0EFE.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.NCCALCSIZE_PARAMS));
			_0EFE.WINDOWPOS wINDOWPOS = (_0EFE.WINDOWPOS)Marshal.PtrToStructure(nCCALCSIZE_PARAMS.lppos, typeof(_0EFE.WINDOWPOS));
			nCCALCSIZE_PARAMS.rgrc1 = (nCCALCSIZE_PARAMS.rgrc0 = _0EFE.RECT.FromRectangle(GetClientRectangle(new Rectangle(wINDOWPOS.x, wINDOWPOS.y, wINDOWPOS.cx, wINDOWPOS.cy))));
			Marshal.StructureToPtr((object)nCCALCSIZE_PARAMS, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = new IntPtr(1024);
		}
		return false;
	}

	internal Ἇ Ꮛ()
	{
		_0EFE.RECT lpRect = new _0EFE.RECT(0, 0, 200, 200);
		CreateParams createParams = ((Control)this).CreateParams;
		_0EFE.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		Ἇ ἇ = new Ἇ();
		ἇ.ἐ = Math.Abs(lpRect.Top);
		ἇ.ἑ = lpRect.Height - 200 - ἇ.ἐ;
		ἇ.ἒ = Math.Abs(lpRect.Left);
		ἇ.ἓ = lpRect.Width - 200 - ἇ.ἒ;
		return ἇ;
	}

	protected virtual Rectangle GetClientRectangle(Rectangle r)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Invalid comparison between Unknown and I4
		if ((int)WindowState == 2 && !IsGlassEnabled)
		{
			_0EFE.RECT lpRect = new _0EFE.RECT(0, 0, 100, 100);
			CreateParams createParams = ((Control)this).CreateParams;
			_0EFE.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
			Rectangle rectangle = lpRect.ToRectangle();
			int num = Math.Abs(rectangle.X);
			_ = rectangle.Bottom;
			int num2 = 3;
			int num3 = m_DisplayRectangleReductionHorizontal + 1 - 3;
			ỳ.X = num - num3;
			ỳ.Y = num - num2 - 1;
			r.X += ỳ.X;
			r.Width -= num * 2 - num3 * 2 - 1;
			r.Height -= num * 2 - num2 * 2 - 1;
			r.Y += ỳ.Y;
			Screen val = Screen.FromHandle(((Control)this).Handle);
			if (r.Height > val.Bounds.Height && val.Primary)
			{
				r.Height = val.Bounds.Height;
			}
		}
		else
		{
			ỳ = Point.Empty;
			if (IsGlassEnabled)
			{
				Ἇ ἇ = Ꮛ();
				int ἒ = ἇ.ἒ;
				int ἑ = ἇ.ἑ;
				r.X += ἒ;
				r.Width -= ἒ * 2;
				r.Height -= ἑ;
				if ((int)WindowState == 2 || _0EFE.IsZoomed(((Control)this).Handle))
				{
					Screen val2 = Screen.FromHandle(((Control)this).Handle);
					if (r.Height > val2.Bounds.Height && val2.Primary)
					{
						r.Height = val2.Bounds.Height + ἑ - 2;
					}
				}
			}
			else if (BarFunctions.IsVista && !BarFunctions.IsWindows7)
			{
				r.Height--;
			}
		}
		return r;
	}

	protected virtual bool WindowsMessageNCActivate(ref Message m)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).WParam != IntPtr.Zero)
		{
			Ỳ = true;
		}
		else
		{
			Ỳ = false;
		}
		if (ᰘ != null && StyleManager.IsMetro(ᰘ.EffectiveStyle) && ᰘ.GetApplicationButton() != null && ᰘ.GetApplicationButton().Expanded)
		{
			_089F.LockWindowUpdate(((Control)this).Handle);
			((Form)this).WndProc(ref m);
			_089F.LockWindowUpdate(IntPtr.Zero);
			if ((int)WindowState == 2)
			{
				if (ᰘ.GetApplicationButton() is ApplicationButton { BackstageTab: not null } applicationButton)
				{
					((Control)applicationButton.BackstageTab).Refresh();
				}
				((Control)ᰘ).Refresh();
			}
			return false;
		}
		ị();
		IntPtr lParam = ((Message)(ref m)).LParam;
		((Message)(ref m)).LParam = new IntPtr(-1);
		((Form)this).WndProc(ref m);
		((Message)(ref m)).LParam = lParam;
		Ọ();
		if (!Ỳ && BarFunctions.IsWindowsXP && (int)WindowState == 2 && ᰘ != null)
		{
			((Control)ᰘ).Invalidate(true);
		}
		return false;
	}

	private void ị()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		if (ᰘ == null || (int)WindowState == 1 || !((Control)this).IsHandleCreated || !((Control)ᰘ).IsHandleCreated || ᰘ.RibbonStrip == null || ((Control)ᰘ.RibbonStrip).Width <= 0 || ((Control)ᰘ.RibbonStrip).Height <= 0 || !((Control)ᰘ).Visible)
		{
			return;
		}
		if (Ề != null)
		{
			Ề.Dispose();
			Ề = null;
		}
		if (ỹ != null)
		{
			ỹ = null;
		}
		Rectangle bounds = ((Control)ᰘ.RibbonStrip).Bounds;
		bounds.Location = ((Control)ᰘ).PointToScreen(bounds.Location);
		bounds.Location = ((Control)this).PointToClient(bounds.Location);
		Ἇ ἇ = Ꮛ();
		if (IsGlassEnabled)
		{
			bounds.X += ἇ.ἒ;
		}
		ề = bounds.Location;
		if ((int)WindowState == 2)
		{
			ề.Offset(ỳ.X, ỳ.Y);
		}
		ArrayList arrayList = new ArrayList(10);
		Size size = new Size(((Control)ᰘ.RibbonStrip).Width, Math.Min(((Control)ᰘ.RibbonStrip).Height, ((Control)ᰘ).Height));
		IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
		Ề = new BufferedBitmap(windowDC, new Rectangle(ề, size));
		_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
		PaintEventArgs val = new PaintEventArgs(Ề.Graphics, Rectangle.Empty);
		try
		{
			ᰘ.RibbonStrip.ઔ(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		foreach (Control item in (ArrangedElementCollection)((Control)ᰘ.RibbonStrip).Controls)
		{
			Control val2 = item;
			if (val2.Visible)
			{
				Rectangle bounds2 = val2.Bounds;
				bounds2.Location = ((Control)ᰘ.RibbonStrip).PointToScreen(bounds2.Location);
				bounds2.Location = ((Control)this).PointToClient(bounds2.Location);
				arrayList.Add(bounds2);
			}
		}
		ỹ = new Rectangle[arrayList.Count];
		arrayList.CopyTo(ỹ);
	}

	private void Ọ()
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		if (Ề == null)
		{
			return;
		}
		try
		{
			IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
			try
			{
				Graphics val = Graphics.FromHdc(windowDC);
				try
				{
					Ề.Render(val, ỹ);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
			}
		}
		finally
		{
			Ề.Dispose();
			Ề = null;
			ỹ = null;
		}
		if (Environment.OSVersion.Version.Major >= 6)
		{
			RedrawBorder();
		}
		else
		{
			if (ᰘ == null)
			{
				return;
			}
			foreach (Control item in (ArrangedElementCollection)((Control)ᰘ.RibbonStrip).Controls)
			{
				Control val2 = item;
				if (val2.Visible)
				{
					val2.Refresh();
				}
			}
		}
	}

	protected virtual void Redraw()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)WindowState != 1)
		{
			_0EFE.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (_0EFE.RedrawWindowFlags)387u);
		}
	}

	protected virtual void RedrawBorder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		Region val = new Region();
		Size size = new Size(SystemInformation.Border3DSize.Width, SystemInformation.Border3DSize.Height);
		val.Union(new Rectangle(0, 0, ((Control)this).Width, size.Height));
		val.Union(new Rectangle(((Control)this).Height - size.Height, 0, ((Control)this).Width, size.Height));
		val.Union(new Rectangle(0, 0, size.Width, ((Control)this).Height));
		val.Union(new Rectangle(((Control)this).Width - size.Width, 0, size.Width, ((Control)this).Height));
		((Control)this).Invalidate(val, true);
		val.Dispose();
		((Control)this).Update();
	}

	protected virtual bool WindowsMessageNCHitTest(ref Message m)
	{
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Invalid comparison between Unknown and I4
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Invalid comparison between Unknown and I4
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Invalid comparison between Unknown and I4
		int x = _0EFE._0F0B(((Message)(ref m)).LParam);
		int y = _0EFE._0F0D(((Message)(ref m)).LParam);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		int num = Dpi.Width5;
		int num2 = 1;
		int width = Dpi.Width5;
		Ἇ ἇ = Ꮛ();
		if (IsGlassEnabled)
		{
			num = ἇ.ἒ;
			IntPtr plResult = IntPtr.Zero;
			_0EFE.DwmDefWindowProc(((Control)this).Handle, ((Message)(ref m)).Msg, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam, out plResult);
			int num3 = _0EFE._0F11(plResult);
			if (num3 == 8 || num3 == 9 || num3 == 20 || num3 == 9 || num3 == 21)
			{
				((Message)(ref m)).Result = plResult;
				Ỷ = true;
				return false;
			}
			pt.X += ἇ.ἒ;
		}
		Rectangle rectangle = Rectangle.Empty;
		if (IsSizable)
		{
			int num4 = Dpi.Width(Ừ);
			int num5 = Dpi.Width(Ử);
			int num6 = Dpi.Width(ử);
			if (new Rectangle(num4, 0, ((Control)this).Width - (num5 + num6), num).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(12);
				return false;
			}
			if (new Rectangle(num5, ((Control)this).Height - num, ((Control)this).Width - (num5 + num6), num).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(15);
				return false;
			}
			int num7 = Dpi.Width(ừ);
			if (((Form)this).RightToLeftLayout && (int)((Control)this).RightToLeft == 1)
			{
				if (new Rectangle(((Control)this).Width - num7, 0, num7 + num2, num7 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(0, 0, num4 + num2, num4 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num6, ((Control)this).Height - num6, num6 + num2, num6 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - num5, num5 + num2, num5 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				rectangle = new Rectangle(0, 0, num, ((Control)this).Height);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
			else
			{
				if (new Rectangle(0, 0, num4 + num2, num4 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num7, 0, num7 + num2, num7 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - num5, num5 + num2, num5 + num2).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(((Control)this).Width - num6, ((Control)this).Height - num6, num6 + width, num6 + width).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(0, 0, num, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				rectangle = new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
		}
		RibbonControl ribbonControl = ᰘ;
		if (ribbonControl != null && (ribbonControl.EffectiveStyle == eDotNetBarStyle.Office2010 || ribbonControl.EffectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(ribbonControl.EffectiveStyle)))
		{
			rectangle = new Rectangle(((int)((Control)this).RightToLeft == 0) ? num : (((Control)this).Width - num - Dpi.Width24), num, Dpi.Width24, Dpi.Height24);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(5);
				return false;
			}
		}
		if ((int)WindowState == 2 && pt.X >= ((Control)this).ClientRectangle.Width - (num + 12) && pt.Y <= ((Control)this).ClientRectangle.Y + num + 22 && ribbonControl != null && ribbonControl.CaptionVisible && Ꮧ() != null)
		{
			SystemCaptionItem systemCaptionItem = Ꮧ();
			if (systemCaptionItem.CloseEnabled && systemCaptionItem.Visible)
			{
				((Message)(ref m)).Result = new IntPtr(1);
				return false;
			}
		}
		if (BarFunctions.IsWindows7 && (int)WindowState == 2 && ribbonControl != null && ribbonControl.CaptionVisible)
		{
			pt = ((Control)ribbonControl.RibbonStrip).PointToClient(new Point(x, y));
			if (ribbonControl.RibbonStrip._0D65.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(2);
				return false;
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageSetText(ref Message m)
	{
		bool flag = _0EFE._0F07(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (ᰘ != null && ᰘ.RibbonStrip != null)
		{
			((Control)ᰘ.RibbonStrip).Refresh();
		}
		if (flag)
		{
			_0EFE._0F07(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageInitMenuPopup(ref Message m)
	{
		((Form)this).WndProc(ref m);
		((Control)this).Invalidate(true);
		return false;
	}

	protected virtual bool WindowsMessageWindowsPosChanged(ref Message m)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)WindowState == 1 || (int)WindowState == 2)
		{
			ỻ = true;
			((Form)this).WndProc(ref m);
			ỻ = true;
			return false;
		}
		return true;
	}

	protected virtual bool WindowsMessageNCMouseMove(ref Message m)
	{
		return true;
	}

	protected virtual bool WindowsMessageNCMouseLeave(ref Message m)
	{
		if (Ỷ)
		{
			IntPtr plResult = IntPtr.Zero;
			_0EFE.DwmDefWindowProc(((Control)this).Handle, 132, IntPtr.Zero, IntPtr.Zero, out plResult);
			Ỷ = false;
		}
		return true;
	}

	protected virtual bool WindowsMessageNCLButtonDown(ref Message m)
	{
		return true;
	}

	protected virtual bool WindowsMessageNCLButtonUp(ref Message m)
	{
		return true;
	}

	protected virtual bool WindowsMessageNCDblClk(ref Message m)
	{
		return true;
	}

	protected virtual bool WindowsMessageMouseActivate(ref Message m)
	{
		((Form)this).Activate();
		return true;
	}

	protected virtual bool WindowsMessageMdiSetMenu(ref Message m)
	{
		return false;
	}

	protected virtual bool WindowsMessageMdiRefreshMenu(ref Message m)
	{
		return false;
	}

	private bool Ἀ(ref Message Ⴘ)
	{
		if (IsGlassEnabled && ᰘ != null)
		{
			IntPtr menu = _0EFE.GetMenu(((Control)this).Handle);
			if (menu != IntPtr.Zero)
			{
				_0EFE.SetMenu(((Control)this).Handle, IntPtr.Zero);
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageSetIcon(ref Message m)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)WindowState == 2 && !((Form)this).IsMdiChild)
		{
			bool flag = _0EFE._0F07(((Message)(ref m)).HWnd, 12582912, 0);
			ị();
			((Form)this).WndProc(ref m);
			if (flag)
			{
				_0EFE._0F07(((Message)(ref m)).HWnd, 0, 12582912);
			}
			Ọ();
			return false;
		}
		return true;
	}

	internal void ڣ(MethodInvoker ڤ)
	{
		ڣ(ڤ, 10);
	}

	internal void ڣ(MethodInvoker ڤ, int Ἁ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = ڤ;
		val.Interval = Ἁ;
		val.Tick += ڥ;
		val.Start();
	}

	internal void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	private void Ἂ()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		if (ᰘ != null)
		{
			ᰘ.CanSupportGlass = IsGlassEnabled;
		}
		UpdateFormGlassState();
		ἀ();
		if (IsGlassEnabled && (int)WindowState == 2)
		{
			WindowState = (FormWindowState)0;
			EnableGlass = false;
			Application.DoEvents();
			EnableGlass = true;
			WindowState = (FormWindowState)2;
		}
	}

	protected virtual bool WindowsMessageDwmCompositionChanged(ref Message m)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		ڣ((MethodInvoker)delegate
		{
			Ἂ();
		}, 800);
		return false;
	}

	protected void UpdateFormGlassState()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		bool flag = Ἃ();
		bool flag2 = ỵ != flag;
		ỵ = flag;
		bool flag3 = false;
		if (((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null && (int)((Form)this).ActiveMdiChild.WindowState == 2)
		{
			((Form)this).ActiveMdiChild.WindowState = (FormWindowState)0;
			flag3 = true;
		}
		string text = ((Control)this).Text;
		_089F.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
		if (IsGlassEnabled)
		{
			((Control)this).Region = null;
			if (ᰘ != null)
			{
				((Control)ᰘ).Region = null;
			}
			ἃ();
		}
		if (flag3 && ((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null)
		{
			((Form)this).ActiveMdiChild.WindowState = (FormWindowState)2;
		}
		((Control)this).Text = text;
		if (IsGlassEnabled && flag2 && (int)WindowState != 1)
		{
			FormWindowState windowState = WindowState;
			WindowState = (FormWindowState)1;
			WindowState = windowState;
		}
		if (flag2)
		{
			OnWindowsGlassStateChanged();
		}
	}

	protected virtual void OnWindowsGlassStateChanged()
	{
	}

	private bool Ἃ()
	{
		if (((Component)this).DesignMode)
		{
			return false;
		}
		return _0EFE.ষ;
	}

	protected virtual bool WindowsMessageStyleChanged(ref Message m)
	{
		if (!Ỻ)
		{
			ị();
			((Form)this).WndProc(ref m);
			Ọ();
			return false;
		}
		return true;
	}

	protected virtual bool WindowsMessageSetCursor(ref Message m)
	{
		if ((!Ỳ && (_0EFE._0F0D(((Message)(ref m)).LParam) == 514 || _0EFE._0F0D(((Message)(ref m)).LParam) == 513) && _0EFE._0F0B(((Message)(ref m)).LParam) == -2) || IsGlassEnabled || !((Control)this).Enabled || BarUtilities.ᆖ)
		{
			return true;
		}
		Ỻ = true;
		IntPtr _0F06 = _0EFE._0F02(((Control)this).Handle, -16);
		IntPtr _0F062;
		if (IntPtr.Size == 8)
		{
			long num = _0F06.ToInt64();
			_0F062 = new IntPtr(num & ~(num & 0x10000000));
		}
		else
		{
			int num2 = _0F06.ToInt32();
			_0F062 = new IntPtr(num2 & ~(num2 & 0x10000000));
		}
		_0EFE._0F04(((Control)this).Handle, -16, _0F062);
		((Form)this).WndProc(ref m);
		_0EFE._0F04(((Control)this).Handle, -16, _0F06);
		Ỻ = false;
		return false;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		if (IsCustomFormStyleEnabled() && (ỻ || (((Component)this).DesignMode && ((specified & 8) == 8 || (specified & 4) == 4))))
		{
			if (width != ((Control)this).Width || height != ((Control)this).Height)
			{
				AdjustBounds(ref width, ref height, specified);
			}
			ỻ = false;
		}
		((Form)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected virtual void AdjustBounds(ref int width, ref int height)
	{
		AdjustBounds(ref width, ref height, (BoundsSpecified)12);
	}

	protected virtual void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		_0EFE.RECT lpRect = new _0EFE.RECT(0, 0, width, height);
		CreateParams createParams = ((Control)this).CreateParams;
		_0EFE.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		Ἇ ἇ = Ꮛ();
		if ((specified & 8) == 8)
		{
			height -= lpRect.Height - height;
			if (IsGlassEnabled)
			{
				height += ἇ.ἑ;
			}
		}
		if ((specified & 4) == 4 && !IsGlassEnabled)
		{
			width -= lpRect.Width - width;
		}
	}

	private Size Ἄ()
	{
		Ἇ ἇ = Ꮛ();
		return new Size(ἇ.ἒ, ἇ.ἑ);
	}

	protected virtual bool SetClientSizeCoreCustom(int x, int y)
	{
		ỻ = true;
		if (IsGlassEnabled)
		{
			Size size = Ἄ();
			((Form)this).Size = new Size(x + size.Width * 2, y + size.Height * 2 + SystemInformation.CaptionHeight);
		}
		else
		{
			((Form)this).Size = new Size(x, y);
		}
		Type typeFromHandle = typeof(Control);
		try
		{
			typeFromHandle.InvokeMember("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { x });
			typeFromHandle.InvokeMember("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { y });
		}
		catch
		{
		}
		((Control)this).OnClientSizeChanged(EventArgs.Empty);
		return false;
	}

	protected override void SetClientSizeCore(int x, int y)
	{
		bool flag = true;
		if (!((Component)this).DesignMode && IsCustomFormStyleEnabled())
		{
			flag = SetClientSizeCoreCustom(x, y);
		}
		if (flag)
		{
			((Form)this).SetClientSizeCore(x, y);
		}
	}

	private Type Ἅ()
	{
		Type baseType = ((object)this).GetType().BaseType;
		while ((object)baseType != null && baseType.Name != "Control")
		{
			baseType = baseType.BaseType;
		}
		return baseType;
	}

	protected virtual void BaseWndProc(ref Message m)
	{
		((Form)this).WndProc(ref m);
	}

	protected override void WndProc(ref Message m)
	{
		if (!IsCustomFormStyleEnabled())
		{
			if (!ỵ && _0EFE.ষ)
			{
				UpdateFormGlassState();
			}
			((Form)this).WndProc(ref m);
			return;
		}
		bool flag = true;
		switch (((Message)(ref m)).Msg)
		{
		case 125:
			flag = WindowsMessageStyleChanged(ref m);
			break;
		case 133:
			flag = WindowsMessageNCPaint(ref m);
			break;
		case 131:
			((Form)this).WndProc(ref m);
			WindowsMessageNCCalcSize(ref m);
			flag = false;
			break;
		case 134:
			flag = WindowsMessageNCActivate(ref m);
			break;
		case 12:
			flag = WindowsMessageSetText(ref m);
			break;
		case 279:
			flag = WindowsMessageInitMenuPopup(ref m);
			break;
		case 71:
			flag = WindowsMessageWindowsPosChanged(ref m);
			break;
		case 132:
			flag = WindowsMessageNCHitTest(ref m);
			break;
		case 20:
			flag = false;
			((Message)(ref m)).Result = new IntPtr(1);
			break;
		case 160:
			flag = WindowsMessageNCMouseMove(ref m);
			break;
		case 674:
			flag = WindowsMessageNCMouseLeave(ref m);
			break;
		case 161:
			flag = WindowsMessageNCLButtonDown(ref m);
			break;
		case 162:
			flag = WindowsMessageNCLButtonUp(ref m);
			break;
		case 32:
			flag = WindowsMessageSetCursor(ref m);
			break;
		case 128:
			flag = WindowsMessageSetIcon(ref m);
			break;
		case 163:
			flag = WindowsMessageNCDblClk(ref m);
			break;
		case 33:
			flag = WindowsMessageMouseActivate(ref m);
			break;
		case 560:
			flag = WindowsMessageMdiSetMenu(ref m);
			break;
		case 564:
			flag = WindowsMessageMdiRefreshMenu(ref m);
			break;
		case 798:
			flag = WindowsMessageDwmCompositionChanged(ref m);
			break;
		case 546:
			flag = Ἀ(ref m);
			break;
		case 146:
		case 174:
			flag = false;
			break;
		}
		if (flag)
		{
			((Form)this).WndProc(ref m);
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27)
		{
			RibbonControl ribbonControl = RibbonControl;
			if (ribbonControl != null && ribbonControl.GetApplicationButton() is ApplicationButton applicationButton && applicationButton.ProcessEscapeKey(keyData))
			{
				return true;
			}
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void RedrawNonClient()
	{
		_0EFE.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (_0EFE.RedrawWindowFlags)1281u);
	}

	protected virtual void OnBackgroundImageUnderCaptionChanged(bool oldValue, bool newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate();
			RedrawNonClient();
		}
	}

	protected virtual void OnRenderFormIconChanged(bool oldValue, bool newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			if (ᰘ != null)
			{
				ᰘ.RibbonStrip.GetBaseItemContainer().NeedRecalcSize = true;
			}
			RedrawNonClient();
		}
	}

	protected virtual void OnRenderFormTextChanged(bool oldValue, bool newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			RedrawNonClient();
		}
	}

	[CompilerGenerated]
	private void Ἆ()
	{
		Ἂ();
	}
}
