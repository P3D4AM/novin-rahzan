using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class OfficeForm : RibbonForm
{
	private int m_BorderWidth = 2;

	private int m_CornerSize = 5;

	private GenericItemContainer m_Caption;

	private ColorScheme m_ColorScheme;

	private LabelItem m_TitleLabel;

	private SystemCaptionItem m_SystemIcon;

	private SystemCaptionItem m_SystemButtons;

	private Timer m_MouseTimer;

	private Font m_CaptionFont;

	private string m_TitleText = "";

	private bool _WasMdiChild;

	private bool _UpdateFromGlassStateOnHandleCreated;

	private BufferedBitmap m_CashedCaptionBitmap;

	private Point m_CashedCaptionBitmapLocation = Point.Empty;

	private DateTime _LastNCMouseMove = DateTime.MinValue;

	private bool _IsActive;

	private Timer m_HoverTimer;

	private bool m_AntiAlias = true;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Indicates text displayed in form caption instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return m_TitleText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_TitleText = value;
			if (value == "")
			{
				m_TitleLabel.Text = ((Control)this).Text;
			}
			else
			{
				m_TitleLabel.Text = value;
			}
			if (((Control)this).IsHandleCreated)
			{
				RecalcSize();
			}
			InvalidateNonClient(invalidateForm: false);
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Caption")]
	[Description("Indicates font for the form caption text when CaptionVisible=true.")]
	public Font CaptionFont
	{
		get
		{
			return m_CaptionFont;
		}
		set
		{
			m_CaptionFont = value;
			OnCaptionFontChanged();
		}
	}

	protected override bool EnableGlassExtend => false;

	[Browsable(true)]
	public Icon Icon
	{
		get
		{
			return ((Form)this).Icon;
		}
		set
		{
			((Form)this).Icon = value;
			m_SystemIcon.Icon = value;
			UpdateSystemButton();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public override bool AutoScroll
	{
		get
		{
			return ((Form)this).AutoScroll;
		}
		set
		{
			((Form)this).AutoScroll = value;
		}
	}

	protected bool CloseEnabled
	{
		get
		{
			return m_SystemButtons.CloseEnabled;
		}
		set
		{
			if (m_SystemButtons.CloseEnabled != value)
			{
				m_SystemButtons.CloseEnabled = value;
				OnCloseEnabledChanged();
			}
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates the Office 2007 Renderer global Color Table.")]
	public eOffice2007ColorScheme Office2007ColorTable
	{
		get
		{
			if (GetRenderer() is Office2007Renderer office2007Renderer)
			{
				return office2007Renderer.ColorTable.InitialColorScheme;
			}
			return eOffice2007ColorScheme.Blue;
		}
		set
		{
			RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(value);
		}
	}

	protected override bool PaintClientBorder => false;

	protected override bool PaintRibbonCaption => false;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting form caption.")]
	public bool CaptionAntiAlias
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
	[DefaultValue("")]
	[Description("Indicates tooltip for the form system icon.")]
	[Localizable(true)]
	public string IconTooltip
	{
		get
		{
			return m_SystemIcon.Tooltip;
		}
		set
		{
			m_SystemIcon.Tooltip = value;
		}
	}

	public OfficeForm()
	{
		m_BorderWidth = SystemInformation.Border3DSize.Width + NativeFunctions.BorderMultiplierFactor;
		((ScrollableControl)this).DockPadding.All = 0;
		m_Caption = new GenericItemContainer
		{
			GlobalItem = false,
			ContainerControl = this,
			WrapItems = false,
			EventHeight = false,
			UseMoreItemsButton = false,
			Stretch = true,
			Displayed = true,
			SystemContainer = true,
			PaddingTop = 0,
			PaddingBottom = 0,
			PaddingLeft = 2,
			ItemSpacing = 1
		};
		m_Caption.SetOwner(this);
		m_Caption.Style = eDotNetBarStyle.StyleManagerControlled;
		m_Caption.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
		m_SystemIcon = new SystemCaptionItem
		{
			GlobalItem = false,
			Enabled = false
		};
		m_Caption.SubItems.Add(m_SystemIcon);
		m_SystemIcon.IsSystemIcon = true;
		m_SystemIcon.Icon = Icon;
		m_TitleLabel = CreateTitleLabel();
		m_Caption.SubItems.Add(m_TitleLabel);
		CreateAdditionalCaptionItems(m_Caption);
		m_SystemButtons = new SystemCaptionItem
		{
			GlobalItem = false
		};
		m_SystemButtons.Click += SystemButtons_Click;
		m_SystemButtons.MouseEnter += SystemButtons_MouseEnter;
		m_Caption.SubItems.Add(m_SystemButtons);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			m_SystemButtons.Click -= SystemButtons_Click;
			m_SystemButtons.MouseEnter -= SystemButtons_MouseEnter;
			m_Caption.Dispose();
		}
		base.Dispose(disposing);
	}

	protected virtual void CreateAdditionalCaptionItems(GenericItemContainer captionContainer)
	{
	}

	protected virtual LabelItem CreateTitleLabel()
	{
		return new LabelItem
		{
			GlobalItem = false,
			Font = SystemFonts.CaptionFont,
			Stretch = true,
			TextLineAlignment = (StringAlignment)1,
			Text = ((Control)this).Text,
			PaddingLeft = 3,
			PaddingRight = 3
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		base.StyleManagerStyleChanged(newStyle);
		InvalidateNonClient(invalidateForm: true);
	}

	protected override bool SetClientSizeCoreCustom(int x, int y)
	{
		base.TrackSetBoundsCore = true;
		AdjustBounds(ref x, ref y);
		return true;
	}

	protected override bool IsCustomFormStyleEnabled()
	{
		if (base.IsCustomFormStyleEnabled())
		{
			if (WinApi.IsGlassEnabled && EnableGlass && !((Component)(object)this).DesignMode)
			{
				return ((Form)this).IsMdiChild;
			}
			return true;
		}
		return false;
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (EnableGlass && WinApi.IsGlassEnabled)
		{
			if (((Control)this).IsHandleCreated)
			{
				UpdateFormGlassState();
			}
			else if (_WasMdiChild)
			{
				_UpdateFromGlassStateOnHandleCreated = true;
			}
		}
		_WasMdiChild = ((Form)this).IsMdiChild;
	}

	private void OnCaptionFontChanged()
	{
		if (m_TitleLabel != null)
		{
			if (m_CaptionFont != null)
			{
				m_TitleLabel.Font = m_CaptionFont;
			}
			else
			{
				m_TitleLabel.Font = SystemFonts.CaptionFont;
			}
			((Control)this).Invalidate(m_Caption.DisplayRectangle);
		}
		InvalidateNonClient(invalidateForm: false);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Form)this).OnFontChanged(e);
		InvalidateNonClient(invalidateForm: false);
	}

	protected override bool WindowsMessageSetIcon(ref Message m)
	{
		UpdateSystemButton();
		return base.WindowsMessageSetIcon(ref m);
	}

	public void InvalidateNonClient(bool invalidateForm)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			NativeFunctions.RECT lprcUpdate = new NativeFunctions.RECT(0, 0, ((Control)this).Width, m_Caption.HeightInternal);
			if (invalidateForm)
			{
				lprcUpdate = new NativeFunctions.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			NativeFunctions.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	protected override Rectangle ReduceDisplayRectangle(Rectangle r)
	{
		return r;
	}

	protected override void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
	}

	protected override bool WindowsMessageDwmCompositionChanged(ref Message m)
	{
		bool result = base.WindowsMessageDwmCompositionChanged(ref m);
		UpdateFormControlStyle();
		return result;
	}

	protected override bool WindowsMessageNCCalcSize(ref Message m)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if (m_SystemButtons != null)
		{
			m_SystemButtons.ToolWindowButtons = (int)((Form)this).FormBorderStyle == 5 || (int)((Form)this).FormBorderStyle == 6;
		}
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

	protected override bool WindowsMessageNCActivate(ref Message m)
	{
		if (((Message)(ref m)).WParam != IntPtr.Zero)
		{
			base.NonClientActive = true;
		}
		else
		{
			base.NonClientActive = false;
		}
		CreateCashedCaptionBitmap();
		BaseWndProc(ref m);
		DrawCashedCaptionBitmap();
		if (base.NonClientPaintEnabled)
		{
			PaintNonClientAreaBuffered();
			Redraw();
		}
		return false;
	}

	private void CreateCashedCaptionBitmap()
	{
		if (m_CashedCaptionBitmap != null)
		{
			m_CashedCaptionBitmap.Dispose();
			m_CashedCaptionBitmap = null;
		}
		if (m_Caption.DisplayRectangle.Width > 0 && m_Caption.DisplayRectangle.Height > 0 && ((Control)this).IsHandleCreated && !((Control)this).IsDisposed)
		{
			IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
			try
			{
				m_CashedCaptionBitmap = new BufferedBitmap(windowDC, new Rectangle(m_CashedCaptionBitmapLocation.X, m_CashedCaptionBitmapLocation.Y, m_Caption.DisplayRectangle.Width, m_Caption.DisplayRectangle.Height));
			}
			finally
			{
				WinApi.ReleaseDC(((Control)this).Handle, windowDC);
			}
			PaintFormCaptionTo(m_CashedCaptionBitmap.Graphics);
		}
	}

	private void DrawCashedCaptionBitmap()
	{
		if (m_CashedCaptionBitmap != null)
		{
			IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				m_CashedCaptionBitmap.Render(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			WinApi.ReleaseDC(((Control)this).Handle, windowDC);
			m_CashedCaptionBitmap.Dispose();
			m_CashedCaptionBitmap = null;
		}
	}

	protected override Rectangle GetClientRectangle(Rectangle rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		NonClientInfo nonClientInfo = GetNonClientInfo();
		if ((int)base.WindowState == 1 && ((Form)this).IsMdiChild)
		{
			return new Rectangle(rect.X + nonClientInfo.LeftBorder, rect.Y + nonClientInfo.CaptionTotalHeight, rect.Width - (nonClientInfo.LeftBorder + nonClientInfo.RightBorder), 0);
		}
		return new Rectangle(rect.X + nonClientInfo.LeftBorder, rect.Y + nonClientInfo.CaptionTotalHeight, rect.Width - (nonClientInfo.LeftBorder + nonClientInfo.RightBorder), rect.Height - (nonClientInfo.CaptionTotalHeight + nonClientInfo.BottomBorder));
	}

	protected override GraphicsPath GetFormPath(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = bounds;
		if (base.TopLeftCornerSize == 0 && base.TopRightCornerSize == 0)
		{
			val.AddRectangle(rectangle);
			return val;
		}
		ArcData cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, m_CornerSize, eCornerArc.TopLeft);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		cornerArc = ElementStyleDisplay.GetCornerArc(rectangle, m_CornerSize, eCornerArc.TopRight);
		val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
		val.AddLine(rectangle.Right, rectangle.Bottom - 3, rectangle.Right, rectangle.Bottom);
		val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.Right - 3, rectangle.Bottom);
		val.AddLine(rectangle.X + 3, rectangle.Bottom, rectangle.X, rectangle.Bottom);
		val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Bottom - 3);
		val.CloseAllFigures();
		return val;
	}

	private Point GetMousePosition()
	{
		if (((Control)this).IsDisposed)
		{
			return Point.Empty;
		}
		Point result = ((Control)this).PointToClient(Control.MousePosition);
		result.X += m_BorderWidth;
		result.Y += m_Caption.HeightInternal;
		return result;
	}

	protected override bool WindowsMessageNCMouseMove(ref Message m)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Invalid comparison between Unknown and I4
		Point mousePosition = GetMousePosition();
		m_Caption.InternalMouseMove(new MouseEventArgs(Control.MouseButtons, 0, mousePosition.X, mousePosition.Y, 0));
		if (m_SystemIcon.Tooltip != "" && m_Caption.HotSubItem == m_SystemIcon)
		{
			SetupHoverTimer();
		}
		if ((int)Control.MouseButtons == 1048576)
		{
			BaseItem baseItem = m_Caption.ItemAtLocation(mousePosition.X, mousePosition.Y);
			if (baseItem == m_TitleLabel)
			{
				if ((int)base.WindowState == 0 || ((int)base.WindowState == 1 && ((Form)this).IsMdiChild))
				{
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					NativeFunctions.SendMessage(((Control)this).Handle, 274, 61458, lParam);
					return false;
				}
				if ((int)base.WindowState == 2 && BarFunctions.IsWindows7 && _IsActive)
				{
					if (_LastNCMouseMove != DateTime.MinValue)
					{
						if (DateTime.Now.Subtract(_LastNCMouseMove).TotalSeconds > 0.0)
						{
							base.WindowState = (FormWindowState)0;
							_LastNCMouseMove = DateTime.MinValue;
						}
					}
					else
					{
						_LastNCMouseMove = DateTime.Now;
					}
				}
			}
		}
		return true;
	}

	protected override void OnActivated(EventArgs e)
	{
		_IsActive = true;
		base.OnActivated(e);
	}

	protected override bool WindowsMessageNCMouseLeave(ref Message m)
	{
		StopHoverTimer();
		m_Caption.InternalMouseLeave();
		return true;
	}

	private void SetupHoverTimer()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (m_HoverTimer != null)
		{
			m_HoverTimer.Stop();
			m_HoverTimer.Start();
			return;
		}
		m_HoverTimer = new Timer();
		m_HoverTimer.Interval = Math.Max(500, SystemInformation.MouseHoverTime);
		m_HoverTimer.Tick += HoverTimer_Tick;
		m_HoverTimer.Start();
	}

	private void HoverTimer_Tick(object sender, EventArgs e)
	{
		m_HoverTimer.Stop();
		if (m_SystemIcon != null && m_SystemIcon.Tooltip != "" && m_Caption != null && m_Caption.HotSubItem == m_SystemIcon)
		{
			m_Caption.InternalMouseHover();
		}
		StopHoverTimer();
	}

	private void StopHoverTimer()
	{
		Timer hoverTimer = m_HoverTimer;
		if (m_HoverTimer != null)
		{
			m_HoverTimer = null;
			hoverTimer.Stop();
			hoverTimer.Tick -= HoverTimer_Tick;
		}
	}

	protected override bool WindowsMessageNCHitTest(ref Message m)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		int x = WinApi.LOWORD(((Message)(ref m)).LParam);
		int y = WinApi.HIWORD(((Message)(ref m)).LParam);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		pt.X += m_BorderWidth;
		pt.Y += m_BorderWidth + SystemInformation.CaptionHeight;
		int borderWidth = m_BorderWidth;
		int num = 2;
		if (IsSizable)
		{
			if (((Form)this).RightToLeftLayout && (int)((Control)this).RightToLeft == 1)
			{
				if (new Rectangle(((Control)this).Width - (m_CornerSize + num), 0, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(0, 0, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(((Control)this).Width - (m_CornerSize + num), ((Control)this).Height - (m_CornerSize + num), m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - (m_CornerSize + num), m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(((Control)this).Width - borderWidth, 0, borderWidth, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				if (new Rectangle(0, 0, borderWidth, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
			else
			{
				if (new Rectangle(0, 0, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(13);
					return false;
				}
				if (new Rectangle(((Control)this).Width - (m_CornerSize + num), 0, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(14);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - m_CornerSize, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(16);
					return false;
				}
				if (new Rectangle(((Control)this).Width - m_CornerSize, ((Control)this).Height - m_CornerSize, m_CornerSize + num, m_CornerSize + num).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(17);
					return false;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, borderWidth).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(12);
					return false;
				}
				if (new Rectangle(0, ((Control)this).Height - borderWidth, ((Control)this).Width, borderWidth).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(15);
					return false;
				}
				if (new Rectangle(0, 0, borderWidth, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(10);
					return false;
				}
				if (new Rectangle(((Control)this).Width - borderWidth, 0, borderWidth, ((Control)this).Height).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(11);
					return false;
				}
			}
		}
		BaseItem baseItem = m_Caption.ItemAtLocation(pt.X, pt.Y);
		if (baseItem == m_TitleLabel)
		{
			((Message)(ref m)).Result = new IntPtr(2);
			return false;
		}
		if (baseItem == m_SystemIcon)
		{
			((Message)(ref m)).Result = new IntPtr(5);
			return false;
		}
		if (baseItem == m_SystemButtons)
		{
			switch (m_SystemButtons.GetButton(pt.X, pt.Y))
			{
			case SystemButton.Minimize:
				((Message)(ref m)).Result = new IntPtr(8);
				break;
			case SystemButton.Maximize:
				((Message)(ref m)).Result = new IntPtr(9);
				break;
			case SystemButton.Close:
				((Message)(ref m)).Result = new IntPtr(20);
				break;
			case SystemButton.Restore:
				((Message)(ref m)).Result = new IntPtr(9);
				break;
			case SystemButton.Help:
				((Message)(ref m)).Result = new IntPtr(21);
				break;
			}
			return false;
		}
		if (baseItem is ButtonItem)
		{
			((Message)(ref m)).Result = new IntPtr(19);
			return false;
		}
		return true;
	}

	protected override bool WindowsMessageNCLButtonDown(ref Message m)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		int num = WinApi.ToInt(((Message)(ref m)).WParam);
		if (num == 12 || num == 14)
		{
			return true;
		}
		Point mousePosition = GetMousePosition();
		m_Caption.InternalMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, mousePosition.X, mousePosition.Y, 0));
		BaseItem baseItem = m_Caption.ItemAtLocation(mousePosition.X, mousePosition.Y);
		if (baseItem != null && (baseItem == m_SystemButtons || baseItem == m_TitleLabel || (mousePosition.X >= m_SystemButtons.DisplayRectangle.X && mousePosition.X <= m_SystemButtons.DisplayRectangle.Right)))
		{
			if (!base.NonClientActive)
			{
				((Form)this).Activate();
			}
			return false;
		}
		return true;
	}

	protected override bool WindowsMessageNCLButtonUp(ref Message m)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		Point mousePosition = GetMousePosition();
		BaseItem baseItem = m_Caption.ItemAtLocation(mousePosition.X, mousePosition.Y);
		m_Caption.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, mousePosition.X, mousePosition.Y, 0));
		if (baseItem == m_SystemButtons)
		{
			return false;
		}
		return true;
	}

	private void SystemButtons_Click(object sender, EventArgs e)
	{
		SystemCaptionItem systemCaptionItem = sender as SystemCaptionItem;
		if (systemCaptionItem.MouseDownButton == SystemButton.Minimize)
		{
			NativeFunctions.PostMessage(((Control)this).Handle, 274, 61472, 0);
			m_Caption.InternalMouseLeave();
		}
		else if (systemCaptionItem.MouseDownButton == SystemButton.Maximize)
		{
			NativeFunctions.PostMessage(((Control)this).Handle, 274, 61488, 0);
			m_Caption.InternalMouseLeave();
		}
		else if (systemCaptionItem.MouseDownButton == SystemButton.Restore)
		{
			NativeFunctions.PostMessage(((Control)this).Handle, 274, 61728, 0);
			m_Caption.InternalMouseLeave();
		}
		else if (systemCaptionItem.MouseDownButton == SystemButton.Close)
		{
			NativeFunctions.PostMessage(((Control)this).Handle, 274, 61536, 0);
			m_Caption.InternalMouseLeave();
		}
		else if (systemCaptionItem.MouseDownButton == SystemButton.Help)
		{
			NativeFunctions.PostMessage(((Control)this).Handle, 274, 61824, 0);
			m_Caption.InternalMouseLeave();
		}
	}

	private void SystemButtons_MouseEnter(object sender, EventArgs e)
	{
		if (m_MouseTimer == null)
		{
			SetupMouseTimer();
		}
	}

	private void SetupMouseTimer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		m_MouseTimer = new Timer();
		m_MouseTimer.Interval = 200;
		m_MouseTimer.Tick += MouseTimer_Tick;
		m_MouseTimer.Start();
	}

	private void MouseTimer_Tick(object sender, EventArgs e)
	{
		Point mousePosition = GetMousePosition();
		if (m_SystemButtons != null && !m_SystemButtons.DisplayRectangle.Contains(mousePosition))
		{
			m_MouseTimer.Enabled = false;
			StopMouseTimer();
			m_SystemButtons.InternalMouseLeave();
		}
	}

	private void StopMouseTimer()
	{
		if (m_MouseTimer != null)
		{
			m_MouseTimer.Enabled = false;
			m_MouseTimer.Tick -= MouseTimer_Tick;
			((Component)(object)m_MouseTimer).Dispose();
			m_MouseTimer = null;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible)
		{
			StopMouseTimer();
		}
		((Form)this).OnVisibleChanged(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		StopMouseTimer();
		((Form)this).OnClosed(e);
	}

	protected override void OnResize(EventArgs e)
	{
		RecalcSize();
		base.OnResize(e);
	}

	internal void RecalcSize()
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and I4
		if (m_Caption != null)
		{
			NonClientInfo nonClientInfo = GetNonClientInfo();
			m_Caption.LeftInternal = 1;
			m_Caption.TopInternal = 0;
			m_Caption.PaddingLeft = nonClientInfo.LeftBorder;
			m_Caption.PaddingRight = nonClientInfo.RightBorder;
			m_Caption.PaddingTop = nonClientInfo.BottomBorder;
			m_Caption.WidthInternal = ((Control)this).Width - 1;
			m_Caption.HeightInternal = nonClientInfo.CaptionTotalHeight - nonClientInfo.BottomBorder / 2;
			m_Caption.IsRightToLeft = (int)((Control)this).RightToLeft == 1;
			if (m_TitleLabel != null)
			{
				m_TitleLabel.Height = m_Caption.HeightInternal - nonClientInfo.BottomBorder - 2;
				if (m_TitleText != "")
				{
					m_TitleLabel.Width = 0;
				}
				else
				{
					m_TitleLabel.Width = 16;
				}
				m_TitleLabel.TextLineAlignment = (StringAlignment)1;
			}
			m_Caption.RecalcSize();
			m_Caption.HeightInternal = nonClientInfo.CaptionTotalHeight;
			if (m_SystemIcon != null)
			{
				m_SystemIcon.TopInternal += (m_Caption.HeightInternal - m_SystemIcon.HeightInternal - nonClientInfo.BottomBorder * 2) / 2;
			}
			m_SystemButtons.TopInternal -= 2;
		}
		if ((int)base.WindowState == 2 || (int)base.WindowState == 1)
		{
			m_SystemButtons.RestoreEnabled = true;
		}
		else
		{
			m_SystemButtons.RestoreEnabled = false;
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		m_Caption.InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		_IsActive = false;
		base.OnDeactivate(e);
		StopMouseTimer();
		m_Caption.InternalMouseLeave();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		UpdateFormText();
	}

	public void UpdateFormText()
	{
		bool flag = false;
		if (m_TitleLabel != null && m_TitleText == "")
		{
			flag = m_TitleLabel.Text != ((Control)this).Text;
			m_TitleLabel.Text = ((Control)this).Text;
		}
		if (flag)
		{
			if (((Form)this).IsMdiChild && ((Form)this).MdiParent is RibbonForm)
			{
				((RibbonForm)(object)((Form)this).MdiParent).InvalidateRibbonCaption();
			}
			InvalidateNonClient(invalidateForm: false);
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if (e.Control is MdiClient)
		{
			e.Control.ControlRemoved += new ControlEventHandler(MdiClientControlRemoved);
			e.Control.ControlAdded += new ControlEventHandler(MdiClientControlAdded);
		}
		base.OnControlAdded(e);
	}

	private void MdiClientControlRemoved(object sender, ControlEventArgs e)
	{
		e.Control.Resize -= MdiChildResize;
	}

	private void MdiClientControlAdded(object sender, ControlEventArgs e)
	{
		e.Control.Resize += MdiChildResize;
	}

	protected override void OnMdiChildActivate(EventArgs e)
	{
		UpdateFormText();
		base.OnMdiChildActivate(e);
	}

	private void MdiChildResize(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		InvokeDelayed((MethodInvoker)delegate
		{
			UpdateFormText();
		});
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		UpdateSystemButton();
		InvalidateNonClient(invalidateForm: false);
		base.OnStyleChanged(e);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		OnCaptionFontChanged();
		((Control)this).OnSystemColorsChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		UpdateSystemButton();
		if (_UpdateFromGlassStateOnHandleCreated)
		{
			_UpdateFromGlassStateOnHandleCreated = false;
			UpdateFormGlassState();
		}
		base.OnHandleCreated(e);
	}

	private void UpdateSystemButton()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		if (m_SystemButtons != null)
		{
			m_SystemButtons.MinimizeVisible = ((Form)this).MinimizeBox;
			m_SystemButtons.RestoreMaximizeVisible = ((Form)this).MaximizeBox;
			m_SystemButtons.Visible = ((Form)this).ControlBox;
			m_SystemIcon.Visible = ((Form)this).ControlBox;
			m_SystemButtons.HelpVisible = ((Form)this).HelpButton;
		}
		if (m_SystemIcon != null)
		{
			m_SystemIcon.Visible = ((Form)this).ShowIcon;
		}
		if ((int)((Form)this).FormBorderStyle == 3)
		{
			m_SystemIcon.Visible = false;
		}
		if (((Control)this).IsHandleCreated)
		{
			RecalcSize();
		}
	}

	protected virtual void OnCloseEnabledChanged()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			IntPtr handle = ((Control)this).Handle;
			IntPtr systemMenu = NativeFunctions.GetSystemMenu(handle, bRevert: false);
			uint num = 0u;
			num = ((!CloseEnabled) ? 1u : 4294967294u);
			WinApi.EnableMenuItem(systemMenu, 61536u, num);
		}
	}

	protected override bool WindowsMessageNCPaint(ref Message m)
	{
		((Message)(ref m)).Result = IntPtr.Zero;
		if (((Control)this).IsHandleCreated && !((Control)this).IsDisposed && ((Control)this).Handle != IntPtr.Zero && base.EnableCustomStyle && base.NonClientPaintEnabled)
		{
			PaintNonClientAreaBuffered();
		}
		return false;
	}

	protected virtual void PaintNonClientAreaBuffered()
	{
		IntPtr windowDC = WinApi.GetWindowDC(((Control)this).Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				PaintNonClientAreaBuffered(val);
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

	protected virtual void PaintNonClientAreaBuffered(Graphics targetGraphics)
	{
		BufferedBitmap bufferedBitmap = new BufferedBitmap(targetGraphics, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		try
		{
			bufferedBitmap.Graphics.SetClip(GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)), (CombineMode)4);
			PaintNonClientArea(bufferedBitmap.Graphics);
			bufferedBitmap.Render(targetGraphics, GetClientRectangle(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height)));
		}
		finally
		{
			bufferedBitmap.Dispose();
		}
	}

	protected virtual Rectangle GetInnerFormBounds()
	{
		Rectangle result = new Rectangle(1, 1, ((Control)this).Width - 3, ((Control)this).Height - 1);
		if (((Form)this).RightToLeftLayout)
		{
			result = new Rectangle(1, 1, ((Control)this).Width - 2, ((Control)this).Height - 1);
		}
		return result;
	}

	protected virtual void PaintNonClientArea(Graphics g)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		NonClientInfo nonClientInfo = GetNonClientInfo();
		PaintFormBorder(g, nonClientInfo.LeftBorder);
		Rectangle innerFormBounds = GetInnerFormBounds();
		GraphicsPath formPath = GetFormPath(innerFormBounds);
		try
		{
			Region val = new Region();
			val.MakeEmpty();
			val.Union(formPath);
			formPath.Widen(SystemPens.Control);
			Region val2 = new Region(formPath);
			val.Union(formPath);
			g.SetClip(val, (CombineMode)0);
			PaintFormCaptionTo(g);
		}
		finally
		{
			((IDisposable)formPath)?.Dispose();
		}
		g.ResetClip();
	}

	private void PaintFormCaptionTo(Graphics g)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		ItemPaintArgs itemPaintArgs = GetItemPaintArgs(g);
		if (itemPaintArgs.Renderer != null)
		{
			if (itemPaintArgs.Renderer is Office2007Renderer)
			{
				if (base.NonClientActive)
				{
					m_TitleLabel.ForeColor = ((Office2007Renderer)itemPaintArgs.Renderer).ColorTable.Form.Active.CaptionText;
				}
				else
				{
					m_TitleLabel.ForeColor = ((Office2007Renderer)itemPaintArgs.Renderer).ColorTable.Form.Inactive.CaptionText;
				}
			}
			itemPaintArgs.Renderer.DrawFormCaptionBackground(new FormCaptionRendererEventArgs(g, m_Caption.DisplayRectangle, (Form)(object)this));
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		if (m_AntiAlias)
		{
			g.SmoothingMode = (SmoothingMode)4;
			g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		m_Caption.Paint(itemPaintArgs);
		if (m_AntiAlias)
		{
			g.SmoothingMode = smoothingMode;
			g.TextRenderingHint = textRenderingHint;
		}
	}

	private ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(null, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		return itemPaintArgs;
	}

	protected virtual ColorScheme GetColorScheme()
	{
		if (m_ColorScheme == null)
		{
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			return new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return m_ColorScheme;
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
}
