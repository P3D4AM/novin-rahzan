using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

internal class NonClientPaintHandler : ISkinHook
{
	public delegate void InvokeMouseMethodDelegate(IntPtr handle, Point mousePos);

	private INonClientControl m_Control;

	private Rectangle m_ClientRectangle = Rectangle.Empty;

	private ScrollBarCore m_VScrollBar;

	private ScrollBarCore m_HScrollBar;

	private const int HC_ACTION = 0;

	private eScrollBarSkin m_SkinScrollbars = eScrollBarSkin.Optimized;

	private bool m_SuspendPaint;

	private bool m_IsVista;

	private bool m_IsListView;

	private bool m_IsAppScrollBarStyle;

	private bool m_Painting;

	public bool IsAppScrollBarStyle
	{
		get
		{
			return m_IsAppScrollBarStyle;
		}
		set
		{
			m_IsAppScrollBarStyle = value;
		}
	}

	private bool ShouldSkinScrollbars
	{
		get
		{
			if (m_SkinScrollbars != 0)
			{
				return !m_IsVista;
			}
			return false;
		}
	}

	public bool SuspendPaint
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

	public eScrollBarSkin SkinScrollbars
	{
		get
		{
			return m_SkinScrollbars;
		}
		set
		{
			if (m_SkinScrollbars == value)
			{
				return;
			}
			if (m_SkinScrollbars != 0)
			{
				if (m_VScrollBar != null)
				{
					m_VScrollBar.Dispose();
					m_VScrollBar = null;
				}
				if (m_HScrollBar != null)
				{
					m_HScrollBar.Dispose();
					m_HScrollBar = null;
				}
				UnRegisterHook();
			}
			m_SkinScrollbars = value;
			if (ShouldSkinScrollbars)
			{
				CreateScrollbars();
				if (m_Control.IsHandleCreated)
				{
					RegisterHook();
				}
			}
		}
	}

	public Rectangle ClientRectangle => m_ClientRectangle;

	public event CustomNCPaintEventHandler BeforeBorderPaint;

	public event CustomNCPaintEventHandler AfterBorderPaint;

	public NonClientPaintHandler(INonClientControl c, eScrollBarSkin skinScrollbars)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		m_IsVista = Environment.OSVersion.Version.Major >= 6;
		m_SkinScrollbars = skinScrollbars;
		m_Control = c;
		if (m_Control is Control)
		{
			((Control)m_Control).HandleCreated += Control_HandleCreated;
			((Control)m_Control).HandleDestroyed += Control_HandleDestroyed;
		}
		m_IsListView = m_Control is ListView;
		if (ShouldSkinScrollbars)
		{
			CreateScrollbars();
		}
		if (m_Control.IsHandleCreated && ShouldSkinScrollbars)
		{
			RegisterHook();
		}
	}

	public void Dispose()
	{
		UnRegisterHook();
	}

	private void CreateScrollbars()
	{
		ref ScrollBarCore vScrollBar = ref m_VScrollBar;
		INonClientControl control = m_Control;
		vScrollBar = new ScrollBarCore((Control)((control is Control) ? control : null), isPassive: true);
		m_VScrollBar.IsAppScrollBarStyle = m_IsAppScrollBarStyle;
		ref ScrollBarCore hScrollBar = ref m_HScrollBar;
		INonClientControl control2 = m_Control;
		hScrollBar = new ScrollBarCore((Control)((control2 is Control) ? control2 : null), isPassive: true);
		m_HScrollBar.IsAppScrollBarStyle = m_IsAppScrollBarStyle;
		m_HScrollBar.Orientation = eOrientation.Horizontal;
		m_HScrollBar.Enabled = false;
	}

	private void Control_HandleDestroyed(object sender, EventArgs e)
	{
		UnRegisterHook();
	}

	private void Control_HandleCreated(object sender, EventArgs e)
	{
		if (ShouldSkinScrollbars)
		{
			RegisterHook();
		}
	}

	private void RegisterHook()
	{
		UpdateScrollValues();
		NonClientHook.RegisterHook(this);
	}

	private void UnRegisterHook()
	{
		if (ShouldSkinScrollbars)
		{
			NonClientHook.UnregisterHook(this);
		}
	}

	public virtual bool WndProc(ref Message m)
	{
		bool result = true;
		switch (((Message)(ref m)).Msg)
		{
		case 133:
			result = WindowsMessageNCPaint(ref m);
			break;
		case 311:
			PaintNonClientAreaBuffered();
			result = false;
			break;
		case 131:
			result = WindowsMessageNCCalcSize(ref m);
			break;
		case 277:
			result = WindowsMessageVScroll(ref m);
			break;
		case 276:
			result = WindowsMessageHScroll(ref m);
			break;
		case 224:
			result = WindowsMessageSbmSetPos(ref m);
			break;
		case 20:
			result = WindowsMessageEraseBkgnd(ref m);
			break;
		case 184:
			result = WindowsMessageEmGetModify(ref m);
			break;
		case 161:
			result = WindowsMessageNcLButtonDown(ref m);
			break;
		case 162:
			result = WindowsMessageNcLButtonUp(ref m);
			break;
		case 674:
			result = WindowsMessageNcMouseLeave(ref m);
			break;
		case 160:
			result = WindowsMessageNcMouseMove(ref m);
			break;
		case 522:
			result = WindowsMessageMouseWheel(ref m);
			break;
		case 5:
			result = WindowsMessageSize(ref m);
			break;
		case 273:
			if (WinApi.HIWORD(((Message)(ref m)).WParam) == 256)
			{
				CallBaseWndProcAndPaint(ref m);
				result = false;
			}
			break;
		case 309:
			CallBaseWndProcAndPaint(ref m);
			result = false;
			break;
		case 288:
			WindowsMessageCustomNotify(ref m);
			result = false;
			break;
		}
		return result;
	}

	private bool WindowsMessageSize(ref Message m)
	{
		if (m_IsListView)
		{
			CallBaseWndProcAndPaint(ref m);
			return false;
		}
		return true;
	}

	private void WindowsMessageCustomNotify(ref Message m)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		switch (WinApi.ToInt(((Message)(ref m)).WParam))
		{
		case 0:
			PaintNonClientAreaBuffered();
			break;
		case 1:
		{
			Point pt = ScreenToNonClientPoint(Control.MousePosition);
			if (m_VScrollBar.Visible)
			{
				if (m_VScrollBar.IsMouseDown)
				{
					m_VScrollBar.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, pt.X, pt.Y, 0));
				}
				else if (!m_VScrollBar.DisplayRectangle.Contains(pt))
				{
					m_VScrollBar.MouseLeave();
				}
			}
			if (m_HScrollBar.Visible)
			{
				if (m_HScrollBar.IsMouseDown)
				{
					m_HScrollBar.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, pt.X, pt.Y, 0));
				}
				else if (!m_HScrollBar.DisplayRectangle.Contains(pt))
				{
					m_HScrollBar.MouseLeave();
				}
			}
			PaintNonClientAreaBuffered();
			break;
		}
		}
	}

	private bool WindowsMessageMouseWheel(ref Message m)
	{
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageNcMouseLeave(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		if (m_VScrollBar.Visible)
		{
			m_VScrollBar.MouseLeave();
		}
		if (m_HScrollBar.Visible)
		{
			m_HScrollBar.MouseLeave();
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageNcMouseMove(ref Message m)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		Point point = ScreenToNonClientPoint(new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam)));
		if (m_VScrollBar.Visible)
		{
			m_VScrollBar.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
		}
		if (m_HScrollBar.Visible)
		{
			m_HScrollBar.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageNcLButtonUp(ref Message m)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		Point point = ScreenToNonClientPoint(new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam)));
		if (m_VScrollBar.Visible)
		{
			m_VScrollBar.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		if (m_HScrollBar.Visible)
		{
			m_HScrollBar.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageNcLButtonDown(ref Message m)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		Point point = ScreenToNonClientPoint(new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam)));
		if (m_VScrollBar.Visible)
		{
			m_VScrollBar.MouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		if (m_HScrollBar.Visible)
		{
			m_HScrollBar.MouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageEmGetModify(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private bool WindowsMessageEraseBkgnd(ref Message m)
	{
		if (!m_IsListView || !ShouldSkinScrollbars)
		{
			return true;
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private void WindowsMessageMouseMove(IntPtr hWnd, Point mousePosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if ((int)Control.MouseButtons == 1048576 && hWnd == m_Control.Handle && ShouldSkinScrollbars)
		{
			WinApi.PostMessage(hWnd, 288, IntPtr.Zero, IntPtr.Zero);
		}
	}

	private bool WindowsMessageSbmSetPos(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		CallBaseWndProcAndPaint(ref m);
		return false;
	}

	private void CallBaseWndProcAndPaintWndStyle(ref Message m)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		IntPtr windowLongPtr = WinApi.GetWindowLongPtr(m_Control.Handle, -16);
		IntPtr dwNewLong;
		if (IntPtr.Size == 8)
		{
			long num = windowLongPtr.ToInt64();
			dwNewLong = new IntPtr(num & ~(num & 0x10000000));
		}
		else
		{
			int num2 = WinApi.ToInt(windowLongPtr);
			dwNewLong = new IntPtr(num2 & ~(num2 & 0x10000000));
		}
		WinApi.SetWindowLongPtr(m_Control.Handle, -16, dwNewLong);
		using (BufferedBitmap bmp = GetNonClientAreaBitmap())
		{
			m_Control.BaseWndProc(ref m);
			WinApi.SetWindowLongPtr(m_Control.Handle, -16, windowLongPtr);
			RenderNonClientAreaBitmap(bmp);
		}
		if (m_Control is Control)
		{
			((Control)m_Control).Invalidate(true);
		}
	}

	private bool WindowsMessageVScroll(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		if (m_SkinScrollbars == eScrollBarSkin.Optimized)
		{
			CallBaseWndProcAndPaintWndStyle(ref m);
		}
		else
		{
			CallBaseWndProcAndPaint(ref m);
		}
		return false;
	}

	private bool WindowsMessageHScroll(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			return true;
		}
		if (m_SkinScrollbars == eScrollBarSkin.Optimized)
		{
			CallBaseWndProcAndPaintWndStyle(ref m);
		}
		else
		{
			CallBaseWndProcAndPaint(ref m);
		}
		return false;
	}

	private void CallBaseWndProcAndPaint(ref Message m)
	{
		using BufferedBitmap bmp = GetNonClientAreaBitmap();
		m_Control.BaseWndProc(ref m);
		RenderNonClientAreaBitmap(bmp);
	}

	protected virtual bool WindowsMessageNCCalcSize(ref Message m)
	{
		ElementStyle borderStyle = m_Control.BorderStyle;
		if (borderStyle != null && borderStyle.PaintBorder)
		{
			m_Control.BaseWndProc(ref m);
			if (((Message)(ref m)).WParam == IntPtr.Zero)
			{
				WinApi.RECT rECT = (WinApi.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.RECT));
				Rectangle clientRectangleForBorderStyle = GetClientRectangleForBorderStyle(rECT.ToRectangle(), borderStyle);
				WinApi.RECT structure = WinApi.RECT.FromRectangle(clientRectangleForBorderStyle);
				Marshal.StructureToPtr(structure, ((Message)(ref m)).LParam, fDeleteOld: false);
				m_ClientRectangle = clientRectangleForBorderStyle;
				m_ClientRectangle.X -= rECT.Left;
				m_ClientRectangle.Y -= rECT.Top;
			}
			else
			{
				WinApi.NCCALCSIZE_PARAMS structure2 = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.NCCALCSIZE_PARAMS));
				WinApi.WINDOWPOS wINDOWPOS = (WinApi.WINDOWPOS)Marshal.PtrToStructure(structure2.lppos, typeof(WinApi.WINDOWPOS));
				WinApi.RECT rgrc = structure2.rgrc0;
				structure2.rgrc1 = (structure2.rgrc0 = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(rgrc.Left, rgrc.Top, rgrc.Width, rgrc.Height), borderStyle)));
				Marshal.StructureToPtr(structure2, ((Message)(ref m)).LParam, fDeleteOld: false);
				m_ClientRectangle = structure2.rgrc0.ToRectangle();
				m_ClientRectangle.X -= wINDOWPOS.x;
				m_ClientRectangle.Y -= wINDOWPOS.y;
			}
		}
		else if (((Message)(ref m)).WParam == IntPtr.Zero)
		{
			WinApi.RECT rECT2 = (WinApi.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.RECT));
			m_Control.BaseWndProc(ref m);
			WinApi.RECT rECT3 = (WinApi.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.RECT));
			WinApi.RECT structure3 = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(rECT3.Left, rECT3.Top, rECT3.Width, rECT3.Height), borderStyle));
			Marshal.StructureToPtr(structure3, ((Message)(ref m)).LParam, fDeleteOld: false);
			m_ClientRectangle = structure3.ToRectangle();
			m_ClientRectangle.X -= rECT2.Left;
			m_ClientRectangle.Y -= rECT2.Top;
		}
		else
		{
			m_Control.BaseWndProc(ref m);
			WinApi.NCCALCSIZE_PARAMS structure4 = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.NCCALCSIZE_PARAMS));
			WinApi.WINDOWPOS wINDOWPOS2 = (WinApi.WINDOWPOS)Marshal.PtrToStructure(structure4.lppos, typeof(WinApi.WINDOWPOS));
			WinApi.RECT rgrc2 = structure4.rgrc0;
			WinApi.RECT rgrc3 = WinApi.RECT.FromRectangle(GetClientRectangleForBorderStyle(new Rectangle(rgrc2.Left, rgrc2.Top, rgrc2.Width, rgrc2.Height), borderStyle));
			structure4.rgrc0 = rgrc3;
			Marshal.StructureToPtr(structure4, ((Message)(ref m)).LParam, fDeleteOld: false);
			m_ClientRectangle = structure4.rgrc0.ToRectangle();
			m_ClientRectangle.X -= wINDOWPOS2.x;
			m_ClientRectangle.Y -= wINDOWPOS2.y;
		}
		return false;
	}

	internal int GetLeftBorderWidth(ElementStyle style)
	{
		if (style.PaintLeftBorder)
		{
			int num = style.BorderLeftWidth;
			if (style.CornerTypeTopLeft == eCornerType.Rounded || style.CornerTypeTopLeft == eCornerType.Diagonal || style.CornerTypeBottomLeft == eCornerType.Rounded || style.CornerTypeBottomLeft == eCornerType.Diagonal || ((style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) && (style.CornerTypeTopLeft == eCornerType.Inherit || style.CornerTypeBottomLeft == eCornerType.Inherit)))
			{
				num = Math.Max(num, style.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int GetTopBorderWidth(ElementStyle style)
	{
		if (style.PaintTopBorder)
		{
			int num = style.BorderTopWidth;
			if (style.CornerTypeTopLeft == eCornerType.Rounded || style.CornerTypeTopLeft == eCornerType.Diagonal || style.CornerTypeTopRight == eCornerType.Rounded || style.CornerTypeTopRight == eCornerType.Diagonal || ((style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) && (style.CornerTypeTopLeft == eCornerType.Inherit || style.CornerTypeTopRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, style.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int GetRightBorderWidth(ElementStyle style)
	{
		if (style.PaintRightBorder)
		{
			int num = style.BorderRightWidth;
			if (style.CornerTypeTopRight == eCornerType.Rounded || style.CornerTypeBottomRight == eCornerType.Rounded || style.CornerTypeTopRight == eCornerType.Diagonal || style.CornerTypeBottomRight == eCornerType.Diagonal || ((style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) && (style.CornerTypeTopRight == eCornerType.Inherit || style.CornerTypeBottomRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, style.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int GetBottomBorderWidth(ElementStyle style)
	{
		if (style.PaintBottomBorder)
		{
			int num = style.BorderBottomWidth;
			if (style.CornerTypeBottomLeft == eCornerType.Rounded || style.CornerTypeBottomRight == eCornerType.Rounded || style.CornerTypeBottomLeft == eCornerType.Diagonal || style.CornerTypeBottomRight == eCornerType.Diagonal || ((style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal) && (style.CornerTypeBottomLeft == eCornerType.Inherit || style.CornerTypeBottomRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, style.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal Rectangle GetClientRectangleForBorderStyle(Rectangle rect, ElementStyle style)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Invalid comparison between Unknown and I4
		if (style == null)
		{
			return rect;
		}
		if (style.PaintLeftBorder)
		{
			int leftBorderWidth = GetLeftBorderWidth(style);
			rect.X += leftBorderWidth;
			rect.Width -= leftBorderWidth;
		}
		if (style.PaintTopBorder)
		{
			int topBorderWidth = GetTopBorderWidth(style);
			rect.Y += topBorderWidth;
			rect.Height -= topBorderWidth;
		}
		if (style.PaintRightBorder)
		{
			int rightBorderWidth = GetRightBorderWidth(style);
			rect.Width -= rightBorderWidth;
		}
		if (style.PaintBottomBorder)
		{
			int bottomBorderWidth = GetBottomBorderWidth(style);
			rect.Height -= bottomBorderWidth;
		}
		INonClientControl control = m_Control;
		TextBox val = (TextBox)((control is TextBox) ? control : null);
		if (val != null && ((TextBoxBase)val).Multiline && (int)val.ScrollBars != 0)
		{
			if ((int)val.ScrollBars == 2 || (int)val.ScrollBars == 3)
			{
				if ((int)((Control)val).RightToLeft == 1)
				{
					rect.Width -= style.PaddingRight;
				}
				else
				{
					rect.X += style.PaddingLeft;
					rect.Width -= style.PaddingLeft;
				}
			}
			return rect;
		}
		rect.X += style.PaddingLeft;
		rect.Width -= style.PaddingLeft + style.PaddingRight;
		rect.Y += style.PaddingTop;
		rect.Height -= style.PaddingTop + style.PaddingBottom;
		m_Control.AdjustClientRectangle(ref rect);
		return rect;
	}

	protected virtual bool WindowsMessageNCPaint(ref Message m)
	{
		if (!ShouldSkinScrollbars)
		{
			m_Control.BaseWndProc(ref m);
			if (m_Control.IsHandleCreated && m_Control.Handle != IntPtr.Zero)
			{
				PaintNonClientAreaBuffered();
			}
			return false;
		}
		using (BufferedBitmap bmp = GetNonClientAreaBitmap())
		{
			m_Control.BaseWndProc(ref m);
			RenderNonClientAreaBitmap(bmp);
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	public void PaintNonClientAreaBuffered()
	{
		if (m_Control.Width <= 0 || m_Control.Height <= 0 || m_SuspendPaint)
		{
			return;
		}
		using BufferedBitmap bmp = GetNonClientAreaBitmap();
		RenderNonClientAreaBitmap(bmp);
	}

	private BufferedBitmap GetNonClientAreaBitmap()
	{
		if (m_Control.Width <= 0 || m_Control.Height <= 0 || m_SuspendPaint || !m_Control.IsHandleCreated)
		{
			return null;
		}
		BufferedBitmap bufferedBitmap = null;
		IntPtr windowDC = WinApi.GetWindowDC(m_Control.Handle);
		Rectangle controlRectangle = GetControlRectangle();
		try
		{
			bufferedBitmap = new BufferedBitmap(windowDC, new Rectangle(0, 0, controlRectangle.Width, controlRectangle.Height));
			PaintNonClientArea(bufferedBitmap.Graphics);
			return bufferedBitmap;
		}
		finally
		{
			WinApi.ReleaseDC(m_Control.Handle, windowDC);
		}
	}

	private void RenderNonClientAreaBitmap(BufferedBitmap bmp)
	{
		if (bmp == null)
		{
			return;
		}
		IntPtr windowDC = WinApi.GetWindowDC(m_Control.Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				Rectangle[] array = new Rectangle[3]
				{
					GetClientRectangle(),
					default(Rectangle),
					default(Rectangle)
				};
				if (!ShouldSkinScrollbars)
				{
					WinApi.SCROLLBARINFO psbi = default(WinApi.SCROLLBARINFO);
					psbi.cbSize = Marshal.SizeOf(psbi);
					WinApi.GetScrollBarInfo(m_Control.Handle, 4294967291u, ref psbi);
					if (psbi.rgstate[0] != 32768)
					{
						Rectangle rectangle = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
						array[1] = rectangle;
					}
					psbi = default(WinApi.SCROLLBARINFO);
					psbi.cbSize = Marshal.SizeOf(psbi);
					WinApi.GetScrollBarInfo(m_Control.Handle, 4294967290u, ref psbi);
					if (psbi.rgstate[0] != 32768)
					{
						Rectangle rectangle2 = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
						array[2] = rectangle2;
					}
				}
				bmp.Render(val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			WinApi.ReleaseDC(m_Control.Handle, windowDC);
		}
	}

	private Rectangle GetClientRectangle()
	{
		return m_ClientRectangle;
	}

	public void UpdateScrollValues()
	{
		if (!ShouldSkinScrollbars)
		{
			return;
		}
		WinApi.SCROLLBARINFO psbi = default(WinApi.SCROLLBARINFO);
		psbi.cbSize = Marshal.SizeOf(psbi);
		WinApi.GetScrollBarInfo(m_Control.Handle, 4294967291u, ref psbi);
		if (psbi.rgstate[0] != 32768)
		{
			Rectangle rectangle = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
			if (m_VScrollBar.DisplayRectangle != rectangle)
			{
				m_VScrollBar.DisplayRectangle = rectangle;
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, psbi.dxyLineButton);
			if (m_VScrollBar.ThumbDecreaseRectangle != rectangle2)
			{
				m_VScrollBar.ThumbDecreaseRectangle = rectangle2;
			}
			rectangle2 = new Rectangle(rectangle.X, rectangle.Bottom - psbi.dxyLineButton, rectangle.Width, psbi.dxyLineButton);
			if (m_VScrollBar.ThumbIncreaseRectangle != rectangle2)
			{
				m_VScrollBar.ThumbIncreaseRectangle = rectangle2;
			}
			rectangle2 = new Rectangle(rectangle.X, rectangle.Y + psbi.xyThumbTop, rectangle.Width, psbi.xyThumbBottom - psbi.xyThumbTop);
			if (m_VScrollBar.TrackRectangle != rectangle2)
			{
				m_VScrollBar.TrackRectangle = rectangle2;
			}
			if (psbi.rgstate[0] == 1)
			{
				if (m_VScrollBar.Enabled)
				{
					m_VScrollBar.Enabled = false;
				}
			}
			else if (!m_VScrollBar.Enabled)
			{
				m_VScrollBar.Enabled = true;
			}
			m_VScrollBar.Visible = true;
		}
		else
		{
			m_VScrollBar.Visible = false;
		}
		psbi = default(WinApi.SCROLLBARINFO);
		psbi.cbSize = Marshal.SizeOf(psbi);
		WinApi.GetScrollBarInfo(m_Control.Handle, 4294967290u, ref psbi);
		if (psbi.rgstate[0] != 32768)
		{
			Rectangle rectangle3 = ScreenToNonClientRectangle(psbi.rcScrollBar.ToRectangle());
			if (m_HScrollBar.DisplayRectangle != rectangle3)
			{
				m_HScrollBar.DisplayRectangle = rectangle3;
			}
			Rectangle rectangle4 = new Rectangle(rectangle3.X, rectangle3.Y, psbi.dxyLineButton, rectangle3.Height);
			if (m_HScrollBar.ThumbDecreaseRectangle != rectangle4)
			{
				m_HScrollBar.ThumbDecreaseRectangle = rectangle4;
			}
			rectangle4 = new Rectangle(rectangle3.Right - psbi.dxyLineButton, rectangle3.Y, psbi.dxyLineButton, rectangle3.Height);
			if (m_HScrollBar.ThumbIncreaseRectangle != rectangle4)
			{
				m_HScrollBar.ThumbIncreaseRectangle = rectangle4;
			}
			rectangle4 = new Rectangle(rectangle3.X + psbi.xyThumbTop, rectangle3.Y, psbi.xyThumbBottom - psbi.xyThumbTop, rectangle3.Height);
			if (m_HScrollBar.TrackRectangle != rectangle4)
			{
				m_HScrollBar.TrackRectangle = rectangle4;
			}
			if (psbi.rgstate[0] == 1)
			{
				if (m_HScrollBar.Enabled)
				{
					m_VScrollBar.Enabled = false;
				}
			}
			else if (!m_HScrollBar.Enabled)
			{
				m_HScrollBar.Enabled = true;
			}
			m_HScrollBar.Visible = true;
		}
		else
		{
			m_HScrollBar.Visible = false;
		}
	}

	protected virtual void PaintNonClientArea(Graphics g)
	{
		if (m_Painting)
		{
			return;
		}
		m_Painting = true;
		try
		{
			PaintBorder(g);
			if (ShouldSkinScrollbars)
			{
				UpdateScrollValues();
				if (m_VScrollBar.Visible)
				{
					m_VScrollBar.Paint(m_Control.GetItemPaintArgs(g));
				}
				if (m_HScrollBar.Visible)
				{
					m_HScrollBar.Paint(m_Control.GetItemPaintArgs(g));
				}
			}
		}
		finally
		{
			m_Painting = false;
		}
	}

	private Rectangle GetControlRectangle()
	{
		if (m_Control.IsHandleCreated)
		{
			WinApi.RECT r = default(WinApi.RECT);
			WinApi.GetWindowRect(m_Control.Handle, ref r);
			return new Rectangle(0, 0, r.Width, r.Height);
		}
		return new Rectangle(0, 0, m_Control.Width, m_Control.Height);
	}

	private void PaintBorder(Graphics g)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		ElementStyle borderStyle = m_Control.BorderStyle;
		Rectangle r = GetControlRectangle();
		if (borderStyle == null || r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(borderStyle, g, r);
		if (borderStyle.BackColor == Color.Transparent && (borderStyle.BackColor2.IsEmpty || borderStyle.BackColor2 == Color.Transparent))
		{
			m_Control.PaintBackground(new PaintEventArgs(g, r));
		}
		else
		{
			if (borderStyle.BackColor.IsEmpty && m_Control.BackColor != Color.Transparent)
			{
				SolidBrush val = new SolidBrush(m_Control.Enabled ? m_Control.BackColor : SystemColors.Control);
				try
				{
					g.FillRectangle((Brush)(object)val, r);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (m_Control.BackColor == Color.Transparent || (borderStyle.PaintBorder && (borderStyle.CornerType == eCornerType.Rounded || borderStyle.CornerType == eCornerType.Diagonal || borderStyle.CornerTypeBottomLeft == eCornerType.Rounded || borderStyle.CornerTypeBottomLeft == eCornerType.Diagonal || borderStyle.CornerTypeBottomRight == eCornerType.Rounded || borderStyle.CornerTypeBottomRight == eCornerType.Diagonal || borderStyle.CornerTypeTopLeft == eCornerType.Rounded || borderStyle.CornerTypeTopLeft == eCornerType.Diagonal || borderStyle.CornerTypeTopRight == eCornerType.Rounded || borderStyle.CornerTypeTopRight == eCornerType.Diagonal)))
			{
				if (m_Control is TextBox || m_Control.BackColor == Color.Transparent)
				{
					m_Control.PaintBackground(new PaintEventArgs(g, r));
				}
				else
				{
					SolidBrush val2 = new SolidBrush(m_Control.BackColor);
					try
					{
						g.FillRectangle((Brush)(object)val2, r);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val3 = new SolidBrush(m_Control.BackColor);
				try
				{
					g.FillRectangle((Brush)(object)val3, r);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Rectangle r2 = r;
			if (borderStyle.PaintBorder)
			{
				if (borderStyle.PaintRightBorder && borderStyle.BorderRightWidth > 1)
				{
					r2.Width--;
				}
				if (borderStyle.PaintBottomBorder && borderStyle.BorderBottomWidth > 1)
				{
					r2.Height--;
				}
			}
			m_Control.AdjustBorderRectangle(ref r2);
			elementStyleDisplayInfo.Bounds = r2;
			ElementStyleDisplay.PaintBackground(elementStyleDisplayInfo);
			m_Control.RenderNonClient(g);
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		if (borderStyle.PaintBorder && (borderStyle.CornerType == eCornerType.Rounded || borderStyle.CornerType == eCornerType.Diagonal))
		{
			g.SmoothingMode = (SmoothingMode)2;
		}
		m_Control.AdjustBorderRectangle(ref r);
		elementStyleDisplayInfo.Bounds = r;
		BeforePaintBorder(g, r);
		ElementStyleDisplay.PaintBorder(elementStyleDisplayInfo);
		g.SmoothingMode = smoothingMode;
		AfterPaintBorder(g, r);
	}

	private void AfterPaintBorder(Graphics g, Rectangle r)
	{
		if (this.AfterBorderPaint != null)
		{
			this.AfterBorderPaint(this, new CustomNCPaintEventArgs(g, r));
		}
	}

	private void BeforePaintBorder(Graphics g, Rectangle r)
	{
		if (this.BeforeBorderPaint != null)
		{
			this.BeforeBorderPaint(this, new CustomNCPaintEventArgs(g, r));
		}
	}

	private Point ScreenToNonClientPoint(Point p)
	{
		Point point = m_Control.PointToScreen(new Point(-m_ClientRectangle.X, -m_ClientRectangle.Y));
		p.X -= point.X;
		p.Y -= point.Y;
		return p;
	}

	private Rectangle ScreenToNonClientRectangle(Rectangle r)
	{
		Point point = m_Control.PointToScreen(new Point(-m_ClientRectangle.X, -m_ClientRectangle.Y));
		r.X -= point.X;
		r.Y -= point.Y;
		return r;
	}

	public void PostMouseMove(IntPtr hWnd, Point mousePos)
	{
		if (m_Control is Control)
		{
			INonClientControl control = m_Control;
			Control val = (Control)((control is Control) ? control : null);
			if (val != null && !val.IsDisposed && val.IsHandleCreated)
			{
				if (val.InvokeRequired)
				{
					val.BeginInvoke((Delegate)new InvokeMouseMethodDelegate(PostMouseMoveSafe), new object[2] { hWnd, mousePos });
				}
				else
				{
					PostMouseMoveSafe(hWnd, mousePos);
				}
			}
		}
		else
		{
			PostMouseMoveSafe(hWnd, mousePos);
		}
	}

	private void PostMouseMoveSafe(IntPtr hWnd, Point mousePos)
	{
		if (hWnd == m_Control.Handle)
		{
			WindowsMessageMouseMove(hWnd, mousePos);
		}
	}

	public void PostMouseUp(IntPtr hWnd, Point mousePos)
	{
		if (m_Control is Control)
		{
			INonClientControl control = m_Control;
			Control val = (Control)((control is Control) ? control : null);
			if (val != null && !val.IsDisposed && val.IsHandleCreated)
			{
				if (val.InvokeRequired)
				{
					val.BeginInvoke((Delegate)new InvokeMouseMethodDelegate(PostMouseUpSafe), new object[2] { hWnd, mousePos });
				}
				else
				{
					PostMouseUpSafe(hWnd, mousePos);
				}
			}
		}
		else
		{
			PostMouseUpSafe(hWnd, mousePos);
		}
	}

	public void PostMouseUpSafe(IntPtr hWnd, Point mousePos)
	{
		if (hWnd == m_Control.Handle && ShouldSkinScrollbars)
		{
			if (m_VScrollBar.IsMouseDown)
			{
				WinApi.PostMessage(m_Control.Handle, 288, (IntPtr)1, (IntPtr)0);
			}
			else if (m_HScrollBar.IsMouseDown)
			{
				WinApi.PostMessage(m_Control.Handle, 288, (IntPtr)1, (IntPtr)0);
			}
		}
	}
}
