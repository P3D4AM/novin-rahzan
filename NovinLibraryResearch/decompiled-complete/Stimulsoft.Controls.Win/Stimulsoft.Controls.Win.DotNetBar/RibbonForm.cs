using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonForm : Form
{
	private IContainer components;

	private int m_TopLeftCornerSize = 6;

	private int m_TopRightCornerSize = 6;

	private int m_BottomLeftCornerSize = 6;

	private int m_BottomRightCornerSize = 6;

	protected int m_DisplayRectangleReductionTop = 1;

	protected int m_DisplayRectangleReductionBottom = 2;

	protected int m_DisplayRectangleReductionHorizontal = 5;

	private RibbonControl m_RibbonControl;

	private FormWindowState m_WindowStateDelayed;

	private bool m_TrackWindowStateSetting;

	private bool m_IsLoaded;

	private bool m_FlattenMDIBorder = true;

	private bool m_NonClientActive = true;

	private Point m_NonClientOffset = Point.Empty;

	protected bool m_EnableCustomStyle = true;

	private int m_GlassHeight;

	private bool m_IsGlassEnabled;

	private bool m_GlassHitTestTrack;

	private bool m_EnableGlass = true;

	private bool _BackColorSet;

	private BufferedBitmap m_CashedCaptionBitmap;

	private Rectangle[] m_CashedCaptionRegion;

	private Point m_CashedCaptionBitmapLocation = Point.Empty;

	private bool m_NoStyleChangeProcessing;

	private bool m_TrackSetBoundsCore;

	private bool _NonClientPaintEnabled = true;

	public static int OffsetHitZone { get; set; }

	internal int DisplayRectangleReductionHorizontal => m_DisplayRectangleReductionHorizontal;

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
				_BackColorSet = true;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Windows Vista Glass support is enabled when form is running on Windows Vista with Glass support.")]
	public virtual bool EnableGlass
	{
		get
		{
			return m_EnableGlass;
		}
		set
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			if (m_EnableGlass == value)
			{
				return;
			}
			m_EnableGlass = value;
			if (m_RibbonControl != null)
			{
				m_RibbonControl.CanSupportGlass = IsGlassEnabled;
			}
			if (m_IsGlassEnabled && ((Control)this).IsHandleCreated && !((Component)this).DesignMode)
			{
				bool flag = false;
				if (((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null && (int)((Form)this).ActiveMdiChild.WindowState == 2)
				{
					flag = true;
				}
				NativeFunctions.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
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
			return m_RibbonControl;
		}
		set
		{
			m_RibbonControl = value;
			if (m_RibbonControl != null)
			{
				UpdateRibbonSystemCaptionItem();
			}
		}
	}

	protected virtual bool EnableGlassExtend => true;

	internal int GlassHeight => m_GlassHeight;

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public FormWindowState WindowState
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (m_TrackWindowStateSetting)
			{
				return m_WindowStateDelayed;
			}
			return ((Form)this).WindowState;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			((Form)this).WindowState = value;
			if ((int)value == 2 && !((Control)this).IsHandleCreated && !m_IsLoaded)
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

	protected bool IsWin11 { get; set; }

	protected bool IsWin11Corners
	{
		get
		{
			if (IsWin11)
			{
				return ((Control)this).Bounds.Height != Screen.PrimaryScreen.WorkingArea.Height;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(6)]
	[Description("Indicates top left rounded corner size")]
	[Category("Border")]
	public int TopLeftCornerSize
	{
		get
		{
			return m_TopLeftCornerSize;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			m_TopLeftCornerSize = value;
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
			return m_TopRightCornerSize;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			m_TopRightCornerSize = value;
			((Control)this).Region = null;
		}
	}

	[Browsable(true)]
	[DefaultValue(6)]
	[Description("Indicates bottom left rounded corner size")]
	[Category("Border")]
	public int BottomLeftCornerSize
	{
		get
		{
			return m_BottomLeftCornerSize;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			m_BottomLeftCornerSize = value;
			((Control)this).Region = null;
		}
	}

	[Browsable(true)]
	[DefaultValue(6)]
	[Description("Indicates bottom right rounded corner size")]
	[Category("Border")]
	public int BottomRightCornerSize
	{
		get
		{
			return m_BottomRightCornerSize;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			m_BottomRightCornerSize = value;
			((Control)this).Region = null;
		}
	}

	protected virtual bool PaintClientBorder
	{
		get
		{
			if (_NonClientPaintEnabled)
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
			if (_NonClientPaintEnabled)
			{
				return m_RibbonControl != null;
			}
			return false;
		}
	}

	internal int BorderSize => GetBorderColors(0).Length;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether 3D MDI border is removed.")]
	public bool FlattenMDIBorder
	{
		get
		{
			return m_FlattenMDIBorder;
		}
		set
		{
			m_FlattenMDIBorder = value;
			OnFlattenMDIBorderChanged();
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

	internal bool NonClientActive
	{
		get
		{
			return m_NonClientActive;
		}
		set
		{
			m_NonClientActive = value;
		}
	}

	[Browsable(false)]
	public bool IsGlassEnabled
	{
		get
		{
			if (((Component)this).DesignMode || ((Form)this).IsMdiChild || StyleManager.Style == eStyle.Metro)
			{
				return false;
			}
			if (m_IsGlassEnabled)
			{
				return m_EnableGlass;
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

	internal bool TrackSetBoundsCore
	{
		get
		{
			return m_TrackSetBoundsCore;
		}
		set
		{
			m_TrackSetBoundsCore = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool NonClientPaintEnabled
	{
		get
		{
			return _NonClientPaintEnabled;
		}
		set
		{
			_NonClientPaintEnabled = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (m_CashedCaptionBitmap != null)
		{
			m_CashedCaptionBitmap.Dispose();
			m_CashedCaptionBitmap = null;
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
	}

	public RibbonForm()
	{
		m_IsGlassEnabled = GetGlassEnabled();
		InitializeComponent();
		UpdateFormControlStyle();
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateFormStyling();
		UpdateRibbonSystemCaptionItem();
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
		_BackColorSet = false;
	}

	private void UpdateFormBackColor()
	{
		if (_BackColorSet)
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
		UpdateFormBackColor();
		if (IsGlassEnabled)
		{
			if (StyleManager.PreviousStyle == eStyle.Metro)
			{
				RefreshGlassState();
			}
			else
			{
				UpdateGlass();
			}
		}
		UpdateMdiClientBackgroundImage();
		((Control)this).Invalidate();
	}

	private void UpdateMdiClientBackgroundImage()
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
			if (StyleManager.Style == eStyle.Metro && mdiClient != null)
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
		m_IsLoaded = true;
		if (m_TrackWindowStateSetting)
		{
			((Form)this).WindowState = m_WindowStateDelayed;
			m_TrackWindowStateSetting = false;
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		((Form)this).OnActivated(e);
		InvalidateRibbonCaption();
	}

	protected override void OnDeactivate(EventArgs e)
	{
		try
		{
			((Form)this).OnDeactivate(e);
			InvalidateRibbonCaption();
		}
		catch
		{
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Form)this).OnTextChanged(e);
		InvalidateRibbonCaption();
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		UpdateRibbonSystemCaptionItem();
		UpdateFormControlStyle();
		((Form)this).OnStyleChanged(e);
	}

	private void UpdateRibbonSystemCaptionItem()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		SystemCaptionItem systemCaptionItem = GetSystemCaptionItem();
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
			m_RibbonControl.RecalcLayout();
		}
	}

	private SystemCaptionItem GetSystemCaptionItem()
	{
		if (m_RibbonControl != null && IsCustomFormStyleEnabled())
		{
			return m_RibbonControl.RibbonStrip.SystemCaptionItem;
		}
		return null;
	}

	internal void UpdateGlass()
	{
		if (IsGlassEnabled && BarFunctions.IsHandleValid((Control)(object)this) && EnableGlassExtend)
		{
			int num = 0;
			if (m_RibbonControl == null || !m_RibbonControl.CaptionVisible)
			{
				NonClientInfo nonClientInfo = GetNonClientInfo();
				num = nonClientInfo.BottomBorder;
			}
			else
			{
				num = m_RibbonControl.RibbonStrip.GetGlassCaptionHeight();
			}
			WinApi.ExtendGlass(((Control)this).Handle, num);
			m_GlassHeight = num;
		}
	}

	internal void InvalidateRibbonCaption()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		if (!IsCustomFormStyleEnabled())
		{
			return;
		}
		if (m_RibbonControl != null && m_RibbonControl.RibbonStrip.CaptionVisible)
		{
			((Control)m_RibbonControl.RibbonStrip).Invalidate(m_RibbonControl.RibbonStrip.CaptionBounds);
		}
		if (IsGlassEnabled && !IsWin11Corners)
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
			m_RibbonControl = e.Control as RibbonControl;
			m_RibbonControl.CanSupportGlass = IsGlassEnabled;
			UpdateRibbonSystemCaptionItem();
			UpdateGlass();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control == m_RibbonControl)
		{
			m_RibbonControl.CanSupportGlass = false;
			m_RibbonControl = null;
		}
		((Control)this).OnControlRemoved(e);
	}

	protected override void OnMdiChildActivate(EventArgs e)
	{
		InvalidateRibbonCaption();
		((Form)this).OnMdiChildActivate(e);
		if (IsGlassEnabled)
		{
			IntPtr menu = WinApi.GetMenu(((Control)this).Handle);
			if (menu != IntPtr.Zero)
			{
				WinApi.SetMenu(((Control)this).Handle, IntPtr.Zero);
			}
		}
	}

	private bool ShouldForwardCornerEvents()
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
		if (ShouldForwardCornerEvents() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal) && e.Y <= m_DisplayRectangleReductionHorizontal)
		{
			MouseEventArgs e2 = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				e2 = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.InvokeMouseDown(e2);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (ShouldForwardCornerEvents() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal) && e.Y <= m_DisplayRectangleReductionHorizontal)
		{
			MouseEventArgs e2 = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				e2 = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.InvokeMouseUp(e2);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (ShouldForwardCornerEvents() && (e.X <= m_DisplayRectangleReductionHorizontal || e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal))
		{
			MouseEventArgs e2 = e;
			if (e.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				e2 = new MouseEventArgs(e.Button, e.Clicks, e.X - m_DisplayRectangleReductionHorizontal * 2, e.Y, e.Delta);
			}
			RibbonControl.RibbonStrip.InvokeMouseMove(e2);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (ShouldForwardCornerEvents())
		{
			Point point = ((Control)this).PointToClient(Control.MousePosition);
			if (point.X <= m_DisplayRectangleReductionHorizontal || point.X >= ((Control)this).ClientRectangle.Width - m_DisplayRectangleReductionHorizontal)
			{
				RibbonControl.RibbonStrip.InvokeClick(e);
			}
		}
		((Control)this).OnClick(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Form)this).IsMdiContainer)
		{
			BarUtilities.ChangeMDIClientBorder((Form)(object)this, m_FlattenMDIBorder);
		}
		UpdateGlass();
		((Form)this).OnHandleCreated(e);
	}

	private void OnFlattenMDIBorderChanged()
	{
		if (((Form)this).IsMdiContainer && ((Control)this).IsHandleCreated)
		{
			BarUtilities.ChangeMDIClientBorder((Form)(object)this, m_FlattenMDIBorder);
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
			WinApi.ExtendGlass(((Control)this).Handle, 0);
		}
	}

	protected virtual bool IsCustomFormStyleEnabled()
	{
		return m_EnableCustomStyle;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		if (HasCustomRegion && ((Control)this).Region == null)
		{
			((Control)this).Region = GetRegion();
		}
		if (IsGlassEnabled && m_GlassHeight > 0)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, m_GlassHeight), (CombineMode)4);
		}
		if (((Form)this).RightToLeftLayout)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			((Form)this).OnPaint(e);
		}
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
		if (IsGlassEnabled && m_GlassHeight > 0)
		{
			e.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, m_GlassHeight), (CombineMode)4);
		}
		((ScrollableControl)this).OnPaintBackground(e);
	}

	protected virtual void PaintFormBorder(PaintEventArgs e)
	{
		PaintFormBorder(e.Graphics, BorderSize);
	}

	protected virtual void PaintFormBorder(Graphics g, int borderSize)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (IsGlassEnabled)
		{
			return;
		}
		Color[] borderColors = GetBorderColors(borderSize);
		Rectangle bounds = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)3;
		SolidBrush val = new SolidBrush(borderColors[^1]);
		try
		{
			g.FillRectangle((Brush)(object)val, new Rectangle(0, 0, 8, 8));
			g.FillRectangle((Brush)(object)val, new Rectangle(((Control)this).Width - 8, 0, 8, 8));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		int num = 0;
		for (int i = 0; i < borderSize; i++)
		{
			Color color = borderColors[num];
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
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintCaptionParts(Graphics g)
	{
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		if (m_RibbonControl == null || !m_RibbonControl.CaptionVisible || m_DisplayRectangleReductionHorizontal <= 2)
		{
			return;
		}
		RibbonControlRendererEventArgs ribbonControlRendererEventArgs = new RibbonControlRendererEventArgs(g, m_RibbonControl, IsGlassEnabled);
		ribbonControlRendererEventArgs.ItemPaintArgs = m_RibbonControl.RibbonStrip.GetItemPaintArgs(g);
		BaseRenderer renderer = m_RibbonControl.RibbonStrip.GetRenderer();
		if (renderer == null)
		{
			return;
		}
		Region clip = g.Clip;
		Rectangle bounds = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 3);
		eDotNetBarStyle effectiveStyle = m_RibbonControl.EffectiveStyle;
		if (IsWin11Corners)
		{
			bounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		else
		{
			switch (effectiveStyle)
			{
			case eDotNetBarStyle.Office2010:
				bounds = new Rectangle(2, 2, ((Control)this).Width - 5, ((Control)this).Height - 4);
				break;
			case eDotNetBarStyle.Metro:
				bounds = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 3);
				break;
			}
		}
		Region region = GetRegion(bounds);
		int displayRectangleReductionHorizontal = m_DisplayRectangleReductionHorizontal;
		Rectangle rectangle = new Rectangle(bounds.X, m_RibbonControl.RibbonStrip.CaptionBounds.Height + 5, bounds.Width, ((Control)this).Height - (m_RibbonControl.RibbonStrip.CaptionBounds.Height + 2));
		g.SetClip(region, (CombineMode)0);
		g.TranslateTransform(1f, 1f);
		renderer.DrawRibbonControlBackground(ribbonControlRendererEventArgs);
		renderer.DrawQuickAccessToolbarBackground(ribbonControlRendererEventArgs);
		if (effectiveStyle == eDotNetBarStyle.Metro)
		{
			int num = 0;
			if (m_RibbonControl.GetApplicationButton() != null)
			{
				ButtonItem buttonItem = m_RibbonControl.GetApplicationButton() as ButtonItem;
				g.TranslateTransform(-1f, 0f);
				if (buttonItem != null && buttonItem.Displayed)
				{
					g.SetClip(region, (CombineMode)0);
					renderer.DrawButtonItem(new ButtonItemRendererEventArgs(g, buttonItem, m_RibbonControl.RibbonStrip.GetItemPaintArgs(g)));
					if (m_RibbonControl.Expanded)
					{
						g.TranslateTransform(0f, 1f);
						renderer.DrawButtonItem(new ButtonItemRendererEventArgs(g, buttonItem, m_RibbonControl.RibbonStrip.GetItemPaintArgs(g)));
					}
				}
			}
			g.ResetTransform();
			if (m_RibbonControl.TabGroups.Count > 0)
			{
				g.SetClip(region, (CombineMode)0);
				g.TranslateTransform((float)((Control)m_RibbonControl).Left, 0f);
				m_RibbonControl.RibbonStrip.PaintTabGroups(m_RibbonControl.RibbonStrip.GetItemPaintArgs(g));
				g.ResetTransform();
			}
			Office2007ColorTable colorTable = ((Office2007Renderer)renderer).ColorTable;
			if (!colorTable.RibbonControl.OuterBorder.Start.IsEmpty)
			{
				Point point = ((Control)this).PointToClient(((Control)m_RibbonControl.RibbonStrip).PointToScreen(new Point(0, ((Control)m_RibbonControl.RibbonStrip).Bottom - 1)));
				Point point2 = ((Control)this).PointToClient(((Control)m_RibbonControl).PointToScreen(new Point(0, ((Control)m_RibbonControl).Bottom - 2)));
				Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)m_RibbonControl.RibbonStrip).Height);
				Rectangle rectangle3 = new Rectangle(0, point2.Y - m_RibbonControl.GetQatToolbarHeight(), ((Control)this).Width, m_RibbonControl.GetQatToolbarHeight());
				Color start = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Active.CaptionTopBackground.Start;
				if (!m_RibbonControl.Expanded)
				{
					rectangle3.Height++;
				}
				ribbonControlRendererEventArgs.Graphics.FillRectangle((Brush)new SolidBrush(start), rectangle2);
				ribbonControlRendererEventArgs.Graphics.FillRectangle((Brush)new SolidBrush(start), rectangle3);
				if (m_RibbonControl.Expanded)
				{
					if (rectangle3.Height > 0)
					{
						point2.Y--;
					}
					Pen val = new Pen(colorTable.RibbonControl.OuterBorder.Start);
					try
					{
						g.DrawLine(val, 1, point.Y, 6, point.Y);
						g.DrawLine(val, 1, point2.Y - rectangle3.Height, 6, point2.Y - rectangle3.Height);
						g.DrawLine(val, ((Control)this).Width - 6, point.Y, ((Control)this).Width - 1, point.Y);
						g.DrawLine(val, ((Control)this).Width - 6, point2.Y - rectangle3.Height, ((Control)this).Width - 1, point2.Y - rectangle3.Height);
						num = point2.Y;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
		}
		g.ResetTransform();
		Bar statusBar = GetStatusBar();
		if (statusBar != null && BarFunctions.IsOffice2007Style(statusBar.Style) && !statusBar.IsThemed)
		{
			renderer = statusBar.GetRenderer();
			if (renderer != null)
			{
				Point point3 = ((Control)this).PointToClient(((Control)statusBar).Parent.PointToScreen(new Point(0, ((Control)statusBar).Top)));
				g.SetClip(region, (CombineMode)0);
				ToolbarRendererEventArgs toolbarRendererEventArgs = new ToolbarRendererEventArgs(statusBar, g, new Rectangle(0, point3.Y, ((Control)this).Width, ((Control)statusBar).Height + 4));
				toolbarRendererEventArgs.ItemPaintArgs = statusBar.GetItemPaintArgs(g);
				toolbarRendererEventArgs.ItemPaintArgs.CachedPaint = true;
				renderer.DrawToolbarBackground(toolbarRendererEventArgs);
			}
		}
		g.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
		region.Dispose();
	}

	protected Bar GetStatusBar()
	{
		return GetStatusBar(((Control)this).Controls);
	}

	private Bar GetStatusBar(ControlCollection controls)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)controls)
		{
			Control val = item;
			if ((int)val.Dock == 2 && val.Visible && val is Bar)
			{
				return val as Bar;
			}
		}
		IEnumerator enumerator2 = ((ArrangedElementCollection)controls).GetEnumerator();
		try
		{
			if (enumerator2.MoveNext())
			{
				Control val2 = (Control)enumerator2.Current;
				return GetStatusBar(val2.Controls) ?? null;
			}
		}
		finally
		{
			IDisposable disposable = enumerator2 as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	protected virtual Color[] GetBorderColors(int borderSize)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007FormStateColorTable office2007FormStateColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Active;
			if (!m_NonClientActive && !((Component)this).DesignMode)
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
		if (flag && (int)WindowState == 2 && BarFunctions.ThemedOS && !Themes.ThemesActive)
		{
			NonClientInfo nonClientInfo = GetNonClientInfo();
			int leftBorder = nonClientInfo.LeftBorder;
			return GetRegion(new Rectangle(leftBorder, 0, ((Control)this).Width - (leftBorder * 2 + 1), ((Control)this).Height - 1));
		}
		if ((int)WindowState == 2 || !flag)
		{
			return null;
		}
		return GetRegion(new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
	}

	private Region GetRegion(Rectangle bounds)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		GraphicsPath formPath = GetFormPath(bounds);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(formPath);
		formPath.Widen(SystemPens.Control);
		Region val2 = new Region(formPath);
		val.Union(formPath);
		formPath.Dispose();
		return val;
	}

	protected virtual GraphicsPath GetFormPath(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = bounds;
		if (StyleManager.GetEffectiveStyle() == eDotNetBarStyle.Metro && !IsWin11Corners)
		{
			val.AddRectangle(rectangle);
			return val;
		}
		bool flag = (int)((Control)this).RightToLeft == 1;
		int num = (flag ? m_TopRightCornerSize : (m_TopLeftCornerSize + ((m_TopLeftCornerSize > 0) ? 1 : 0)));
		if (num > 0)
		{
			ArcData cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, num, eCornerArc.TopLeft);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		}
		else
		{
			val.AddLine(bounds.X, bounds.Y + 2, bounds.X, bounds.Y);
			val.AddLine(bounds.X, bounds.Y, bounds.X + 2, bounds.Y);
		}
		num = ((!flag) ? m_TopRightCornerSize : ((m_TopLeftCornerSize + m_TopLeftCornerSize > 0) ? 1 : 0));
		if (num > 0)
		{
			ArcData cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, num, eCornerArc.TopRight);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		}
		else
		{
			val.AddLine(bounds.Right - 2, bounds.Y, bounds.Right, bounds.Y);
			val.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y + 2);
		}
		if (m_BottomRightCornerSize > 0)
		{
			ArcData cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, m_BottomRightCornerSize, eCornerArc.BottomRight);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		}
		else
		{
			val.AddLine(bounds.Right, bounds.Bottom - 2, bounds.Right, bounds.Bottom);
			val.AddLine(bounds.Right, bounds.Bottom, bounds.Right - 2, bounds.Bottom);
		}
		if (m_BottomLeftCornerSize > 0)
		{
			ArcData cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, m_BottomLeftCornerSize, eCornerArc.BottomLeft);
			val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
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
		if (BarFunctions.IsVista && !BarFunctions.IsWindows7 && _NonClientPaintEnabled)
		{
			IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
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
				WinApi.ReleaseDC(((Control)this).Handle, windowDC);
			}
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	protected virtual bool WindowsMessageNCCalcSize(ref Message m)
	{
		if (((Message)(ref m)).WParam == IntPtr.Zero)
		{
			WinApi.RECT structure = WinApi.RECT.FromRectangle(GetClientRectangle(((WinApi.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.RECT))).ToRectangle()));
			Marshal.StructureToPtr(structure, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = IntPtr.Zero;
		}
		else
		{
			WinApi.NCCALCSIZE_PARAMS structure2 = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.NCCALCSIZE_PARAMS));
			WinApi.WINDOWPOS wINDOWPOS = (WinApi.WINDOWPOS)Marshal.PtrToStructure(structure2.lppos, typeof(WinApi.WINDOWPOS));
			structure2.rgrc1 = (structure2.rgrc0 = WinApi.RECT.FromRectangle(GetClientRectangle(new Rectangle(wINDOWPOS.x, wINDOWPOS.y, wINDOWPOS.cx, wINDOWPOS.cy))));
			Marshal.StructureToPtr(structure2, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = new IntPtr(1024);
		}
		return false;
	}

	internal NonClientInfo GetNonClientInfo()
	{
		WinApi.RECT lpRect = new WinApi.RECT(0, 0, 200, 200);
		CreateParams createParams = ((Control)this).CreateParams;
		WinApi.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		NonClientInfo nonClientInfo = new NonClientInfo();
		nonClientInfo.CaptionTotalHeight = Math.Abs(lpRect.Top);
		nonClientInfo.BottomBorder = lpRect.Height - 200 - nonClientInfo.CaptionTotalHeight;
		nonClientInfo.LeftBorder = Math.Abs(lpRect.Left);
		nonClientInfo.RightBorder = lpRect.Width - 200 - nonClientInfo.LeftBorder;
		return nonClientInfo;
	}

	protected virtual Rectangle GetClientRectangle(Rectangle r)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Invalid comparison between Unknown and I4
		if ((int)WindowState == 2 && !IsGlassEnabled)
		{
			WinApi.RECT lpRect = new WinApi.RECT(0, 0, 100, 100);
			CreateParams createParams = ((Control)this).CreateParams;
			WinApi.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
			Rectangle rectangle = lpRect.ToRectangle();
			int num = Math.Abs(rectangle.X);
			int num2 = rectangle.Bottom - 100;
			int num3 = 1;
			int num4 = m_DisplayRectangleReductionHorizontal + 1 - 1;
			m_NonClientOffset.X = num - num4;
			m_NonClientOffset.Y = num - num3 - 1;
			r.X += m_NonClientOffset.X;
			r.Width -= num * 2 - num4 * 2 - 1;
			r.Height -= num * 2 - num3 * 2 - 1;
			r.Y += m_NonClientOffset.Y;
			Screen val = Screen.FromHandle(((Control)this).Handle);
			if (r.Height > val.Bounds.Height && val.Primary)
			{
				r.Height = val.Bounds.Height;
			}
		}
		else
		{
			m_NonClientOffset = Point.Empty;
			if (IsGlassEnabled)
			{
				NonClientInfo nonClientInfo = GetNonClientInfo();
				int leftBorder = nonClientInfo.LeftBorder;
				int bottomBorder = nonClientInfo.BottomBorder;
				r.X += leftBorder;
				r.Width -= leftBorder * 2;
				r.Height -= bottomBorder;
				if ((int)WindowState == 2 || WinApi.IsZoomed(((Control)this).Handle))
				{
					Screen val2 = Screen.FromHandle(((Control)this).Handle);
					if (r.Height > val2.Bounds.Height && val2.Primary)
					{
						r.Height = val2.Bounds.Height + bottomBorder - 2;
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
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).WParam != IntPtr.Zero)
		{
			m_NonClientActive = true;
		}
		else
		{
			m_NonClientActive = false;
		}
		if (m_RibbonControl != null && m_RibbonControl.EffectiveStyle == eDotNetBarStyle.Metro && m_RibbonControl.GetApplicationButton() != null && m_RibbonControl.GetApplicationButton().Expanded)
		{
			NativeFunctions.LockWindowUpdate(((Control)this).Handle);
			((Form)this).WndProc(ref m);
			NativeFunctions.LockWindowUpdate(IntPtr.Zero);
			if ((int)WindowState == 2)
			{
				if (m_RibbonControl.GetApplicationButton() is ApplicationButton { BackstageTab: not null } applicationButton)
				{
					((Control)applicationButton.BackstageTab).Refresh();
				}
				((Control)m_RibbonControl).Refresh();
			}
			return false;
		}
		CreateCashedCaptionBitmap();
		IntPtr lParam = ((Message)(ref m)).LParam;
		((Message)(ref m)).LParam = new IntPtr(-1);
		((Form)this).WndProc(ref m);
		((Message)(ref m)).LParam = lParam;
		DrawCashedCaptionBitmap();
		if (!m_NonClientActive && BarFunctions.IsWindowsXP && (int)WindowState == 2 && m_RibbonControl != null)
		{
			((Control)m_RibbonControl).Invalidate(true);
		}
		return false;
	}

	private void CreateCashedCaptionBitmap()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		if (m_RibbonControl == null || (int)WindowState == 1 || !((Control)this).IsHandleCreated || !((Control)m_RibbonControl).IsHandleCreated || m_RibbonControl.RibbonStrip == null || ((Control)m_RibbonControl.RibbonStrip).Width <= 0 || ((Control)m_RibbonControl.RibbonStrip).Height <= 0 || !((Control)m_RibbonControl).Visible)
		{
			return;
		}
		if (m_CashedCaptionBitmap != null)
		{
			m_CashedCaptionBitmap.Dispose();
			m_CashedCaptionBitmap = null;
		}
		if (m_CashedCaptionRegion != null)
		{
			m_CashedCaptionRegion = null;
		}
		Rectangle bounds = ((Control)m_RibbonControl.RibbonStrip).Bounds;
		bounds.Location = ((Control)m_RibbonControl).PointToScreen(bounds.Location);
		bounds.Location = ((Control)this).PointToClient(bounds.Location);
		NonClientInfo nonClientInfo = GetNonClientInfo();
		if (IsGlassEnabled)
		{
			bounds.X += nonClientInfo.LeftBorder;
		}
		m_CashedCaptionBitmapLocation = bounds.Location;
		if ((int)WindowState == 2)
		{
			m_CashedCaptionBitmapLocation.Offset(m_NonClientOffset.X, m_NonClientOffset.Y);
		}
		ArrayList arrayList = new ArrayList(10);
		Size size = new Size(((Control)m_RibbonControl.RibbonStrip).Width, Math.Min(((Control)m_RibbonControl.RibbonStrip).Height, ((Control)m_RibbonControl).Height));
		IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
		m_CashedCaptionBitmap = new BufferedBitmap(windowDC, new Rectangle(m_CashedCaptionBitmapLocation, size));
		WinApi.ReleaseDC(((Control)this).Handle, windowDC);
		PaintEventArgs val = new PaintEventArgs(m_CashedCaptionBitmap.Graphics, Rectangle.Empty);
		try
		{
			m_RibbonControl.RibbonStrip.InternalPaint(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		foreach (Control item in (ArrangedElementCollection)((Control)m_RibbonControl.RibbonStrip).Controls)
		{
			Control val2 = item;
			if (val2.Visible)
			{
				Rectangle bounds2 = val2.Bounds;
				bounds2.Location = ((Control)m_RibbonControl.RibbonStrip).PointToScreen(bounds2.Location);
				bounds2.Location = ((Control)this).PointToClient(bounds2.Location);
				arrayList.Add(bounds2);
			}
		}
		m_CashedCaptionRegion = new Rectangle[arrayList.Count];
		arrayList.CopyTo(m_CashedCaptionRegion);
	}

	private void DrawCashedCaptionBitmap()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		if (m_CashedCaptionBitmap == null)
		{
			return;
		}
		try
		{
			IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
			try
			{
				Graphics val = Graphics.FromHdc(windowDC);
				try
				{
					m_CashedCaptionBitmap.Render(val, m_CashedCaptionRegion);
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
		finally
		{
			m_CashedCaptionBitmap.Dispose();
			m_CashedCaptionBitmap = null;
			m_CashedCaptionRegion = null;
		}
		if (Environment.OSVersion.Version.Major >= 6)
		{
			RedrawBorder();
		}
		else
		{
			if (m_RibbonControl == null)
			{
				return;
			}
			foreach (Control item in (ArrangedElementCollection)((Control)m_RibbonControl.RibbonStrip).Controls)
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
			WinApi.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (WinApi.RedrawWindowFlags)387u);
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
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Invalid comparison between Unknown and I4
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		int x = WinApi.LOWORD(((Message)(ref m)).LParam);
		int y = WinApi.HIWORD(((Message)(ref m)).LParam);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		int num = StiScale2.I5;
		int i = StiScale2.I1;
		int i2 = StiScale2.I5;
		NonClientInfo nonClientInfo = GetNonClientInfo();
		if (IsGlassEnabled)
		{
			num = nonClientInfo.LeftBorder;
			IntPtr plResult = IntPtr.Zero;
			WinApi.DwmDefWindowProc(((Control)this).Handle, ((Message)(ref m)).Msg, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam, out plResult);
			int num2 = WinApi.ToInt(plResult);
			if (num2 == 8 || num2 == 9 || num2 == 20 || num2 == 9 || num2 == 21)
			{
				((Message)(ref m)).Result = plResult;
				m_GlassHitTestTrack = true;
				return false;
			}
			pt.X += nonClientInfo.LeftBorder;
		}
		Rectangle rectangle = Rectangle.Empty;
		if (IsSizable)
		{
			if (new Rectangle(m_TopLeftCornerSize, 0, ((Control)this).Width - (m_BottomLeftCornerSize + m_BottomRightCornerSize), num).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(12);
				return false;
			}
			if (new Rectangle(m_BottomLeftCornerSize, ((Control)this).Height - num, ((Control)this).Width - (m_BottomLeftCornerSize + m_BottomRightCornerSize), num).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(15);
				return false;
			}
			if (((Form)this).RightToLeftLayout && (int)((Control)this).RightToLeft == 1)
			{
				if (new Rectangle(((Control)this).Width - m_TopRightCornerSize, 0, m_TopRightCornerSize + i, m_TopRightCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(0, 0, m_TopLeftCornerSize + i, m_TopLeftCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(((Control)this).Width - m_BottomRightCornerSize, ((Control)this).Height - m_BottomRightCornerSize, m_BottomRightCornerSize + i, m_BottomRightCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - m_BottomLeftCornerSize, m_BottomLeftCornerSize + i, m_BottomLeftCornerSize + i).Contains(pt))
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
				if (new Rectangle(0, 0, m_TopLeftCornerSize + i, m_TopLeftCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(((Control)this).Width - m_TopRightCornerSize, 0, m_TopRightCornerSize + i, m_TopRightCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - m_BottomLeftCornerSize, m_BottomLeftCornerSize + i, m_BottomLeftCornerSize + i).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(((Control)this).Width - m_BottomRightCornerSize, ((Control)this).Height - m_BottomRightCornerSize, m_BottomRightCornerSize + i2, m_BottomRightCornerSize + i2).Contains(pt))
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
		RibbonControl ribbonControl = m_RibbonControl;
		if (ribbonControl != null && (ribbonControl.EffectiveStyle == eDotNetBarStyle.Office2010 || ribbonControl.EffectiveStyle == eDotNetBarStyle.Windows7 || ribbonControl.EffectiveStyle == eDotNetBarStyle.Metro))
		{
			rectangle = new Rectangle(((int)((Control)this).RightToLeft == 0) ? num : (((Control)this).Width - num - StiScale2.XXI(24.0) - OffsetHitZone), num, StiScale2.XXI(24.0) - OffsetHitZone, StiScale2.YYI(24.0) - OffsetHitZone);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(5);
				return false;
			}
		}
		if ((int)WindowState == 2 && pt.X >= ((Control)this).ClientRectangle.Width - (num + StiScale2.XXI(12.0)) && pt.Y <= ((Control)this).ClientRectangle.Y + num + StiScale2.YYI(22.0) && ribbonControl != null && ribbonControl.CaptionVisible && GetSystemCaptionItem() != null)
		{
			SystemCaptionItem systemCaptionItem = GetSystemCaptionItem();
			if (systemCaptionItem.CloseEnabled && systemCaptionItem.Visible)
			{
				((Message)(ref m)).Result = new IntPtr(1);
				return false;
			}
		}
		if (BarFunctions.IsWindows7 && (int)WindowState == 2 && ribbonControl != null && ribbonControl.CaptionVisible)
		{
			pt = ((Control)ribbonControl.RibbonStrip).PointToClient(new Point(x, y));
			if (ribbonControl.RibbonStrip.CaptionBounds.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(2);
				return false;
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageSetText(ref Message m)
	{
		bool flag = WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (m_RibbonControl != null && m_RibbonControl.RibbonStrip != null)
		{
			((Control)m_RibbonControl.RibbonStrip).Refresh();
		}
		if (flag)
		{
			WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 0, 268435456);
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
			m_TrackSetBoundsCore = true;
			((Form)this).WndProc(ref m);
			m_TrackSetBoundsCore = true;
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
		if (m_GlassHitTestTrack)
		{
			IntPtr plResult = IntPtr.Zero;
			WinApi.DwmDefWindowProc(((Control)this).Handle, 132, IntPtr.Zero, IntPtr.Zero, out plResult);
			m_GlassHitTestTrack = false;
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

	private bool WindowsMessageMdiActivate(ref Message m)
	{
		if (IsGlassEnabled && m_RibbonControl != null)
		{
			IntPtr menu = WinApi.GetMenu(((Control)this).Handle);
			if (menu != IntPtr.Zero)
			{
				WinApi.SetMenu(((Control)this).Handle, IntPtr.Zero);
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageSetIcon(ref Message m)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)WindowState == 2)
		{
			bool flag = WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 12582912, 0);
			CreateCashedCaptionBitmap();
			((Form)this).WndProc(ref m);
			if (flag)
			{
				WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 0, 12582912);
			}
			DrawCashedCaptionBitmap();
			return false;
		}
		return true;
	}

	internal void InvokeDelayed(MethodInvoker method)
	{
		InvokeDelayed(method, 10);
	}

	internal void InvokeDelayed(MethodInvoker method, int timeOut)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = timeOut;
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	internal void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	private void RefreshGlassState()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		if (m_RibbonControl != null)
		{
			m_RibbonControl.CanSupportGlass = IsGlassEnabled;
		}
		UpdateFormGlassState();
		UpdateFormBackColor();
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
		InvokeDelayed((MethodInvoker)delegate
		{
			RefreshGlassState();
		}, 800);
		return false;
	}

	protected void UpdateFormGlassState()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		bool glassEnabled = GetGlassEnabled();
		bool flag = m_IsGlassEnabled != glassEnabled;
		m_IsGlassEnabled = glassEnabled;
		bool flag2 = false;
		if (((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null && (int)((Form)this).ActiveMdiChild.WindowState == 2)
		{
			((Form)this).ActiveMdiChild.WindowState = (FormWindowState)0;
			flag2 = true;
		}
		string text = ((Control)this).Text;
		NativeFunctions.SetWindowPos(((Control)this).Handle, IntPtr.Zero, 0, 0, 0, 0, 55);
		if (IsGlassEnabled)
		{
			((Control)this).Region = null;
			if (m_RibbonControl != null)
			{
				((Control)m_RibbonControl).Region = null;
			}
			UpdateGlass();
		}
		if (flag2 && ((Form)this).IsMdiContainer && ((Form)this).ActiveMdiChild != null)
		{
			((Form)this).ActiveMdiChild.WindowState = (FormWindowState)2;
		}
		((Control)this).Text = text;
		if (IsGlassEnabled && flag && (int)WindowState != 1)
		{
			FormWindowState windowState = WindowState;
			WindowState = (FormWindowState)1;
			WindowState = windowState;
		}
		if (flag)
		{
			OnWindowsGlassStateChanged();
		}
	}

	protected virtual void OnWindowsGlassStateChanged()
	{
	}

	private bool GetGlassEnabled()
	{
		if (((Component)this).DesignMode)
		{
			return false;
		}
		return WinApi.IsGlassEnabled;
	}

	protected virtual bool WindowsMessageStyleChanged(ref Message m)
	{
		if (!m_NoStyleChangeProcessing)
		{
			CreateCashedCaptionBitmap();
			((Form)this).WndProc(ref m);
			DrawCashedCaptionBitmap();
			return false;
		}
		return true;
	}

	protected virtual bool WindowsMessageSetCursor(ref Message m)
	{
		if ((!m_NonClientActive && (WinApi.HIWORD(((Message)(ref m)).LParam) == 514 || WinApi.HIWORD(((Message)(ref m)).LParam) == 513) && WinApi.LOWORD(((Message)(ref m)).LParam) == -2) || IsGlassEnabled || !((Control)this).Enabled || BarUtilities.IsModalFormOpen)
		{
			return true;
		}
		m_NoStyleChangeProcessing = true;
		IntPtr windowLongPtr = WinApi.GetWindowLongPtr(((Control)this).Handle, -16);
		IntPtr dwNewLong;
		if (IntPtr.Size == 8)
		{
			long num = windowLongPtr.ToInt64();
			dwNewLong = new IntPtr(num & ~(num & 0x10000000));
		}
		else
		{
			int num2 = windowLongPtr.ToInt32();
			dwNewLong = new IntPtr(num2 & ~(num2 & 0x10000000));
		}
		WinApi.SetWindowLongPtr(((Control)this).Handle, -16, dwNewLong);
		((Form)this).WndProc(ref m);
		WinApi.SetWindowLongPtr(((Control)this).Handle, -16, windowLongPtr);
		m_NoStyleChangeProcessing = false;
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
		if (IsCustomFormStyleEnabled() && (m_TrackSetBoundsCore || (((Component)this).DesignMode && ((specified & 8) == 8 || (specified & 4) == 4))))
		{
			if (width != ((Control)this).Width || height != ((Control)this).Height)
			{
				AdjustBounds(ref width, ref height, specified);
			}
			m_TrackSetBoundsCore = false;
		}
		((Form)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected virtual void AdjustBounds(ref int width, ref int height)
	{
		AdjustBounds(ref width, ref height, (BoundsSpecified)12);
	}

	protected virtual void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		WinApi.RECT lpRect = new WinApi.RECT(0, 0, width, height);
		CreateParams createParams = ((Control)this).CreateParams;
		WinApi.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		NonClientInfo nonClientInfo = GetNonClientInfo();
		if ((specified & 8) == 8)
		{
			height -= lpRect.Height - height;
			if (IsGlassEnabled)
			{
				height += nonClientInfo.BottomBorder;
			}
		}
		if ((specified & 4) == 4 && !IsGlassEnabled)
		{
			width -= lpRect.Width - width;
		}
	}

	private Size GetSystemFormBorderSize()
	{
		NonClientInfo nonClientInfo = GetNonClientInfo();
		return new Size(nonClientInfo.LeftBorder, nonClientInfo.BottomBorder);
	}

	protected virtual bool SetClientSizeCoreCustom(int x, int y)
	{
		m_TrackSetBoundsCore = true;
		if (IsGlassEnabled)
		{
			Size systemFormBorderSize = GetSystemFormBorderSize();
			((Form)this).Size = new Size(x + systemFormBorderSize.Width * 2, y + systemFormBorderSize.Height * 2 + SystemInformation.CaptionHeight);
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

	private Type GetControlType()
	{
		Type baseType = ((object)this).GetType().BaseType;
		while (baseType != null && baseType.Name != "Control")
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
			if (!m_IsGlassEnabled && WinApi.IsGlassEnabled)
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
			flag = WindowsMessageMdiActivate(ref m);
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
		WinApi.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (WinApi.RedrawWindowFlags)1281u);
	}
}
