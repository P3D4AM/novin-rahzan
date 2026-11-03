using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar.Controls;

internal class ᶨ : _1DDD
{
	public delegate void InvokeMouseMethodDelegate(IntPtr handle, Point mousePos);

	private const int ᒼ = 0;

	private INonClientControl _10CF;

	private Rectangle ᶯ = Rectangle.Empty;

	private ScrollBarCore ᶰ;

	private ScrollBarCore ᶱ;

	private eScrollBarSkin ᶲ = eScrollBarSkin.Optimized;

	private bool _055D;

	private bool _0949;

	private bool ᶳ;

	private bool ᘐ;

	private bool _0DD3;

	public bool ᶫ
	{
		get
		{
			return ᘐ;
		}
		set
		{
			ᘐ = value;
		}
	}

	private bool ᶬ
	{
		get
		{
			if (ᶲ != 0)
			{
				return !_0949;
			}
			return false;
		}
	}

	public bool _19CA
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

	public eScrollBarSkin ᶭ
	{
		get
		{
			return ᶲ;
		}
		set
		{
			if (ᶲ == value)
			{
				return;
			}
			if (ᶲ != 0)
			{
				if (ᶰ != null)
				{
					ᶰ.Dispose();
					ᶰ = null;
				}
				if (ᶱ != null)
				{
					ᶱ.Dispose();
					ᶱ = null;
				}
				ᶸ();
			}
			ᶲ = value;
			if (ᶬ)
			{
				ᶴ();
				if (_10CF.IsHandleCreated)
				{
					ᶷ();
				}
			}
		}
	}

	public Rectangle ᶮ => ᶯ;

	public event _1DDE ᶩ;

	public event _1DDE ᶪ;

	public ᶨ(INonClientControl c, eScrollBarSkin skinScrollbars)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		_0949 = Environment.OSVersion.Version.Major >= 6;
		ᶲ = skinScrollbars;
		_10CF = c;
		if (_10CF is Control)
		{
			((Control)_10CF).HandleCreated += ᶶ;
			((Control)_10CF).HandleDestroyed += ᶵ;
		}
		ᶳ = _10CF is ListView;
		if (ᶬ)
		{
			ᶴ();
		}
		if (_10CF.IsHandleCreated && ᶬ)
		{
			ᶷ();
		}
	}

	public void _1A17()
	{
		ᶸ();
	}

	private void ᶴ()
	{
		ref ScrollBarCore reference = ref ᶰ;
		INonClientControl nonClientControl = _10CF;
		reference = new ScrollBarCore((Control)((nonClientControl is Control) ? nonClientControl : null), isPassive: true);
		ᶰ.IsAppScrollBarStyle = ᘐ;
		ref ScrollBarCore reference2 = ref ᶱ;
		INonClientControl nonClientControl2 = _10CF;
		reference2 = new ScrollBarCore((Control)((nonClientControl2 is Control) ? nonClientControl2 : null), isPassive: true);
		ᶱ.IsAppScrollBarStyle = ᘐ;
		ᶱ.Orientation = eOrientation.Horizontal;
		ᶱ.Enabled = false;
	}

	private void ᶵ(object _٠, EventArgs ؿ)
	{
		ᶸ();
	}

	private void ᶶ(object _٠, EventArgs ؿ)
	{
		if (ᶬ)
		{
			ᶷ();
		}
	}

	private void ᶷ()
	{
		_1DD2();
		_1DE0.ᶷ(this);
	}

	private void ᶸ()
	{
		if (ᶬ)
		{
			_1DE0._1DE4(this);
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
			_1DCE();
			result = false;
			break;
		case 131:
			result = WindowsMessageNCCalcSize(ref m);
			break;
		case 277:
			result = _1DC6(ref m);
			break;
		case 276:
			result = _1DC7(ref m);
			break;
		case 224:
			result = _1DC4(ref m);
			break;
		case 20:
			result = _1DC1(ref m);
			break;
		case 184:
			result = _1DC0(ref m);
			break;
		case 161:
			result = ᶿ(ref m);
			break;
		case 162:
			result = ᶾ(ref m);
			break;
		case 674:
			result = ᶼ(ref m);
			break;
		case 160:
			result = ᶽ(ref m);
			break;
		case 522:
			result = ᶻ(ref m);
			break;
		case 5:
			result = ᶹ(ref m);
			break;
		case 273:
			if (_0EFE._0F0D(((Message)(ref m)).WParam) == 256)
			{
				_1DC8(ref m);
				result = false;
			}
			break;
		case 309:
			_1DC8(ref m);
			result = false;
			break;
		case 288:
			ᶺ(ref m);
			result = false;
			break;
		}
		return result;
	}

	private bool ᶹ(ref Message Ⴘ)
	{
		if (ᶳ)
		{
			_1DC8(ref Ⴘ);
			return false;
		}
		return true;
	}

	private void ᶺ(ref Message Ⴘ)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		switch (_0EFE._0F11(((Message)(ref Ⴘ)).WParam))
		{
		case 0:
			_1DCE();
			break;
		case 1:
		{
			Point pt = _1DD7(Control.MousePosition);
			if (ᶰ.Visible)
			{
				if (ᶰ.IsMouseDown)
				{
					ᶰ.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, pt.X, pt.Y, 0));
				}
				else if (!ᶰ.DisplayRectangle.Contains(pt))
				{
					ᶰ.MouseLeave();
				}
			}
			if (ᶱ.Visible)
			{
				if (ᶱ.IsMouseDown)
				{
					ᶱ.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, pt.X, pt.Y, 0));
				}
				else if (!ᶱ.DisplayRectangle.Contains(pt))
				{
					ᶱ.MouseLeave();
				}
			}
			_1DCE();
			break;
		}
		}
	}

	private bool ᶻ(ref Message Ⴘ)
	{
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool ᶼ(ref Message Ⴘ)
	{
		if (!ᶬ)
		{
			return true;
		}
		if (ᶰ.Visible)
		{
			ᶰ.MouseLeave();
		}
		if (ᶱ.Visible)
		{
			ᶱ.MouseLeave();
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool ᶽ(ref Message Ⴘ)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ᶬ)
		{
			return true;
		}
		Point point = _1DD7(new Point(_0EFE._0F0B(((Message)(ref Ⴘ)).LParam), _0EFE._0F0D(((Message)(ref Ⴘ)).LParam)));
		if (ᶰ.Visible)
		{
			ᶰ.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
		}
		if (ᶱ.Visible)
		{
			ᶱ.MouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool ᶾ(ref Message Ⴘ)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ᶬ)
		{
			return true;
		}
		Point point = _1DD7(new Point(_0EFE._0F0B(((Message)(ref Ⴘ)).LParam), _0EFE._0F0D(((Message)(ref Ⴘ)).LParam)));
		if (ᶰ.Visible)
		{
			ᶰ.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		if (ᶱ.Visible)
		{
			ᶱ.MouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool ᶿ(ref Message Ⴘ)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!ᶬ)
		{
			return true;
		}
		Point point = _1DD7(new Point(_0EFE._0F0B(((Message)(ref Ⴘ)).LParam), _0EFE._0F0D(((Message)(ref Ⴘ)).LParam)));
		if (ᶰ.Visible)
		{
			ᶰ.MouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		if (ᶱ.Visible)
		{
			ᶱ.MouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool _1DC0(ref Message Ⴘ)
	{
		if (!ᶬ)
		{
			return true;
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private bool _1DC1(ref Message Ⴘ)
	{
		if (!ᶳ || !ᶬ)
		{
			return true;
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private void _1DC2(IntPtr _0A65, Point _1DC3)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		if ((int)Control.MouseButtons == 1048576 && _0A65 == _10CF.Handle && ᶬ)
		{
			_0EFE.PostMessage(_0A65, 288, IntPtr.Zero, IntPtr.Zero);
		}
	}

	private bool _1DC4(ref Message Ⴘ)
	{
		if (!ᶬ)
		{
			return true;
		}
		_1DC8(ref Ⴘ);
		return false;
	}

	private void _1DC5(ref Message Ⴘ)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		IntPtr intPtr = _0EFE._0F02(_10CF.Handle, -16);
		IntPtr _0F06;
		if (IntPtr.Size == 8)
		{
			long num = intPtr.ToInt64();
			_0F06 = new IntPtr(num & ~(num & 0x10000000));
		}
		else
		{
			int num2 = _0EFE._0F11(intPtr);
			_0F06 = new IntPtr(num2 & ~(num2 & 0x10000000));
		}
		_0EFE._0F04(_10CF.Handle, -16, _0F06);
		using (BufferedBitmap bufferedBitmap = _1DCF())
		{
			_10CF.BaseWndProc(ref Ⴘ);
			_0EFE._0F04(_10CF.Handle, -16, intPtr);
			_1DD0(bufferedBitmap);
		}
		if (_10CF is Control)
		{
			((Control)_10CF).Invalidate(true);
		}
	}

	private bool _1DC6(ref Message Ⴘ)
	{
		if (!ᶬ)
		{
			return true;
		}
		if (ᶲ == eScrollBarSkin.Optimized)
		{
			_1DC5(ref Ⴘ);
		}
		else
		{
			_1DC8(ref Ⴘ);
		}
		return false;
	}

	private bool _1DC7(ref Message Ⴘ)
	{
		if (!ᶬ)
		{
			return true;
		}
		if (ᶲ == eScrollBarSkin.Optimized)
		{
			_1DC5(ref Ⴘ);
		}
		else
		{
			_1DC8(ref Ⴘ);
		}
		return false;
	}

	private void _1DC8(ref Message Ⴘ)
	{
		using BufferedBitmap bufferedBitmap = _1DCF();
		_10CF.BaseWndProc(ref Ⴘ);
		_1DD0(bufferedBitmap);
	}

	protected virtual bool WindowsMessageNCCalcSize(ref Message m)
	{
		ElementStyle borderStyle = _10CF.BorderStyle;
		if (borderStyle != null && borderStyle.ނ)
		{
			_10CF.BaseWndProc(ref m);
			if (((Message)(ref m)).WParam == IntPtr.Zero)
			{
				_0EFE.RECT rECT = (_0EFE.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.RECT));
				Rectangle rectangle = _1DCD(rECT.ToRectangle(), borderStyle);
				_0EFE.RECT rECT2 = _0EFE.RECT.FromRectangle(rectangle);
				Marshal.StructureToPtr((object)rECT2, ((Message)(ref m)).LParam, fDeleteOld: false);
				ᶯ = rectangle;
				ᶯ.X -= rECT.Left;
				ᶯ.Y -= rECT.Top;
			}
			else
			{
				_0EFE.NCCALCSIZE_PARAMS nCCALCSIZE_PARAMS = (_0EFE.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.NCCALCSIZE_PARAMS));
				_0EFE.WINDOWPOS wINDOWPOS = (_0EFE.WINDOWPOS)Marshal.PtrToStructure(nCCALCSIZE_PARAMS.lppos, typeof(_0EFE.WINDOWPOS));
				_0EFE.RECT rgrc = nCCALCSIZE_PARAMS.rgrc0;
				nCCALCSIZE_PARAMS.rgrc1 = (nCCALCSIZE_PARAMS.rgrc0 = _0EFE.RECT.FromRectangle(_1DCD(new Rectangle(rgrc.Left, rgrc.Top, rgrc.Width, rgrc.Height), borderStyle)));
				Marshal.StructureToPtr((object)nCCALCSIZE_PARAMS, ((Message)(ref m)).LParam, fDeleteOld: false);
				ᶯ = nCCALCSIZE_PARAMS.rgrc0.ToRectangle();
				ᶯ.X -= wINDOWPOS.x;
				ᶯ.Y -= wINDOWPOS.y;
			}
		}
		else if (((Message)(ref m)).WParam == IntPtr.Zero)
		{
			_0EFE.RECT rECT3 = (_0EFE.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.RECT));
			_10CF.BaseWndProc(ref m);
			_0EFE.RECT rECT4 = (_0EFE.RECT)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.RECT));
			_0EFE.RECT rECT5 = _0EFE.RECT.FromRectangle(_1DCD(new Rectangle(rECT4.Left, rECT4.Top, rECT4.Width, rECT4.Height), borderStyle));
			Marshal.StructureToPtr((object)rECT5, ((Message)(ref m)).LParam, fDeleteOld: false);
			ᶯ = rECT5.ToRectangle();
			ᶯ.X -= rECT3.Left;
			ᶯ.Y -= rECT3.Top;
		}
		else
		{
			_10CF.BaseWndProc(ref m);
			_0EFE.NCCALCSIZE_PARAMS nCCALCSIZE_PARAMS2 = (_0EFE.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.NCCALCSIZE_PARAMS));
			_0EFE.WINDOWPOS wINDOWPOS2 = (_0EFE.WINDOWPOS)Marshal.PtrToStructure(nCCALCSIZE_PARAMS2.lppos, typeof(_0EFE.WINDOWPOS));
			_0EFE.RECT rgrc2 = nCCALCSIZE_PARAMS2.rgrc0;
			_0EFE.RECT rgrc3 = _0EFE.RECT.FromRectangle(_1DCD(new Rectangle(rgrc2.Left, rgrc2.Top, rgrc2.Width, rgrc2.Height), borderStyle));
			nCCALCSIZE_PARAMS2.rgrc0 = rgrc3;
			Marshal.StructureToPtr((object)nCCALCSIZE_PARAMS2, ((Message)(ref m)).LParam, fDeleteOld: false);
			ᶯ = nCCALCSIZE_PARAMS2.rgrc0.ToRectangle();
			ᶯ.X -= wINDOWPOS2.x;
			ᶯ.Y -= wINDOWPOS2.y;
		}
		return false;
	}

	internal int _1DC9(ElementStyle _0657)
	{
		if (_0657.ݾ)
		{
			int num = _0657.BorderLeftWidth;
			if (_0657.CornerTypeTopLeft == eCornerType.Rounded || _0657.CornerTypeTopLeft == eCornerType.Diagonal || _0657.CornerTypeBottomLeft == eCornerType.Rounded || _0657.CornerTypeBottomLeft == eCornerType.Diagonal || ((_0657.CornerType == eCornerType.Rounded || _0657.CornerType == eCornerType.Diagonal) && (_0657.CornerTypeTopLeft == eCornerType.Inherit || _0657.CornerTypeBottomLeft == eCornerType.Inherit)))
			{
				num = Math.Max(num, _0657.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int _1DCA(ElementStyle _0657)
	{
		if (_0657.ހ)
		{
			int num = _0657.BorderTopWidth;
			if (_0657.CornerTypeTopLeft == eCornerType.Rounded || _0657.CornerTypeTopLeft == eCornerType.Diagonal || _0657.CornerTypeTopRight == eCornerType.Rounded || _0657.CornerTypeTopRight == eCornerType.Diagonal || ((_0657.CornerType == eCornerType.Rounded || _0657.CornerType == eCornerType.Diagonal) && (_0657.CornerTypeTopLeft == eCornerType.Inherit || _0657.CornerTypeTopRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, _0657.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int _1DCB(ElementStyle _0657)
	{
		if (_0657.ݿ)
		{
			int num = _0657.BorderRightWidth;
			if (_0657.CornerTypeTopRight == eCornerType.Rounded || _0657.CornerTypeBottomRight == eCornerType.Rounded || _0657.CornerTypeTopRight == eCornerType.Diagonal || _0657.CornerTypeBottomRight == eCornerType.Diagonal || ((_0657.CornerType == eCornerType.Rounded || _0657.CornerType == eCornerType.Diagonal) && (_0657.CornerTypeTopRight == eCornerType.Inherit || _0657.CornerTypeBottomRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, _0657.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal int _1DCC(ElementStyle _0657)
	{
		if (_0657.ށ)
		{
			int num = _0657.BorderBottomWidth;
			if (_0657.CornerTypeBottomLeft == eCornerType.Rounded || _0657.CornerTypeBottomRight == eCornerType.Rounded || _0657.CornerTypeBottomLeft == eCornerType.Diagonal || _0657.CornerTypeBottomRight == eCornerType.Diagonal || ((_0657.CornerType == eCornerType.Rounded || _0657.CornerType == eCornerType.Diagonal) && (_0657.CornerTypeBottomLeft == eCornerType.Inherit || _0657.CornerTypeBottomRight == eCornerType.Inherit)))
			{
				num = Math.Max(num, _0657.CornerDiameter / 2 + 1);
			}
			return num;
		}
		return 0;
	}

	internal Rectangle _1DCD(Rectangle भ, ElementStyle _0657)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Invalid comparison between Unknown and I4
		if (_0657 == null)
		{
			return भ;
		}
		if (_0657.ݾ)
		{
			int num = _1DC9(_0657);
			भ.X += num;
			भ.Width -= num;
		}
		if (_0657.ހ)
		{
			int num2 = _1DCA(_0657);
			भ.Y += num2;
			भ.Height -= num2;
		}
		if (_0657.ݿ)
		{
			int num3 = _1DCB(_0657);
			भ.Width -= num3;
		}
		if (_0657.ށ)
		{
			int num4 = _1DCC(_0657);
			भ.Height -= num4;
		}
		INonClientControl nonClientControl = _10CF;
		TextBox val = (TextBox)((nonClientControl is TextBox) ? nonClientControl : null);
		if (val != null && ((TextBoxBase)val).Multiline && (int)val.ScrollBars != 0)
		{
			if ((int)val.ScrollBars == 2 || (int)val.ScrollBars == 3)
			{
				if ((int)((Control)val).RightToLeft == 1)
				{
					भ.Width -= _0657.PaddingRight;
				}
				else
				{
					भ.X += _0657.PaddingLeft;
					भ.Width -= _0657.PaddingLeft;
				}
			}
			return भ;
		}
		भ.X += _0657.PaddingLeft;
		भ.Width -= _0657.PaddingLeft + _0657.PaddingRight;
		भ.Y += _0657.PaddingTop;
		भ.Height -= _0657.PaddingTop + _0657.PaddingBottom;
		_10CF.AdjustClientRectangle(ref भ);
		return भ;
	}

	protected virtual bool WindowsMessageNCPaint(ref Message m)
	{
		if (!ᶬ)
		{
			_10CF.BaseWndProc(ref m);
			if (_10CF.IsHandleCreated && _10CF.Handle != IntPtr.Zero)
			{
				_1DCE();
			}
			return false;
		}
		using (BufferedBitmap bufferedBitmap = _1DCF())
		{
			_10CF.BaseWndProc(ref m);
			_1DD0(bufferedBitmap);
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	public void _1DCE()
	{
		if (_10CF.Width <= 0 || _10CF.Height <= 0 || _055D)
		{
			return;
		}
		using BufferedBitmap bufferedBitmap = _1DCF();
		_1DD0(bufferedBitmap);
	}

	private BufferedBitmap _1DCF()
	{
		if (_10CF.Width <= 0 || _10CF.Height <= 0 || _055D || !_10CF.IsHandleCreated)
		{
			return null;
		}
		BufferedBitmap bufferedBitmap = null;
		IntPtr windowDC = _0EFE.GetWindowDC(_10CF.Handle);
		if (windowDC == IntPtr.Zero)
		{
			return null;
		}
		Rectangle rectangle = _1DD3();
		try
		{
			bufferedBitmap = new BufferedBitmap(windowDC, new Rectangle(0, 0, rectangle.Width, rectangle.Height));
			PaintNonClientArea(bufferedBitmap.Graphics);
			return bufferedBitmap;
		}
		finally
		{
			_0EFE.ReleaseDC(_10CF.Handle, windowDC);
		}
	}

	private void _1DD0(BufferedBitmap _1DD1)
	{
		if (_1DD1 == null)
		{
			return;
		}
		IntPtr windowDC = _0EFE.GetWindowDC(_10CF.Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				Rectangle[] array = new Rectangle[3]
				{
					ᦑ(),
					default(Rectangle),
					default(Rectangle)
				};
				if (!ᶬ)
				{
					_0EFE.SCROLLBARINFO psbi = default(_0EFE.SCROLLBARINFO);
					psbi.cbSize = Marshal.SizeOf((object)psbi);
					_0EFE.GetScrollBarInfo(_10CF.Handle, 4294967291u, ref psbi);
					if (psbi.rgstate[0] != 32768)
					{
						Rectangle rectangle = _1DD8(psbi.rcScrollBar.ToRectangle());
						array[1] = rectangle;
					}
					psbi = default(_0EFE.SCROLLBARINFO);
					psbi.cbSize = Marshal.SizeOf((object)psbi);
					_0EFE.GetScrollBarInfo(_10CF.Handle, 4294967290u, ref psbi);
					if (psbi.rgstate[0] != 32768)
					{
						Rectangle rectangle2 = _1DD8(psbi.rcScrollBar.ToRectangle());
						array[2] = rectangle2;
					}
				}
				_1DD1.Render(val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			_0EFE.ReleaseDC(_10CF.Handle, windowDC);
		}
	}

	private Rectangle ᦑ()
	{
		return ᶯ;
	}

	public void _1DD2()
	{
		if (!ᶬ)
		{
			return;
		}
		_0EFE.SCROLLBARINFO psbi = default(_0EFE.SCROLLBARINFO);
		psbi.cbSize = Marshal.SizeOf((object)psbi);
		_0EFE.GetScrollBarInfo(_10CF.Handle, 4294967291u, ref psbi);
		if (psbi.rgstate[0] != 32768)
		{
			Rectangle rectangle = _1DD8(psbi.rcScrollBar.ToRectangle());
			if (ᶰ.DisplayRectangle != rectangle)
			{
				ᶰ.DisplayRectangle = rectangle;
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, psbi.dxyLineButton);
			if (ᶰ.ThumbDecreaseRectangle != rectangle2)
			{
				ᶰ.ThumbDecreaseRectangle = rectangle2;
			}
			rectangle2 = new Rectangle(rectangle.X, rectangle.Bottom - psbi.dxyLineButton, rectangle.Width, psbi.dxyLineButton);
			if (ᶰ.ThumbIncreaseRectangle != rectangle2)
			{
				ᶰ.ThumbIncreaseRectangle = rectangle2;
			}
			rectangle2 = new Rectangle(rectangle.X, rectangle.Y + psbi.xyThumbTop, rectangle.Width, psbi.xyThumbBottom - psbi.xyThumbTop);
			if (ᶰ.TrackRectangle != rectangle2)
			{
				ᶰ.TrackRectangle = rectangle2;
			}
			if (psbi.rgstate[0] == 1)
			{
				if (ᶰ.Enabled)
				{
					ᶰ.Enabled = false;
				}
			}
			else if (!ᶰ.Enabled)
			{
				ᶰ.Enabled = true;
			}
			ᶰ.Visible = true;
		}
		else
		{
			ᶰ.Visible = false;
		}
		psbi = default(_0EFE.SCROLLBARINFO);
		psbi.cbSize = Marshal.SizeOf((object)psbi);
		_0EFE.GetScrollBarInfo(_10CF.Handle, 4294967290u, ref psbi);
		if (psbi.rgstate[0] != 32768)
		{
			Rectangle rectangle3 = _1DD8(psbi.rcScrollBar.ToRectangle());
			if (ᶱ.DisplayRectangle != rectangle3)
			{
				ᶱ.DisplayRectangle = rectangle3;
			}
			Rectangle rectangle4 = new Rectangle(rectangle3.X, rectangle3.Y, psbi.dxyLineButton, rectangle3.Height);
			if (ᶱ.ThumbDecreaseRectangle != rectangle4)
			{
				ᶱ.ThumbDecreaseRectangle = rectangle4;
			}
			rectangle4 = new Rectangle(rectangle3.Right - psbi.dxyLineButton, rectangle3.Y, psbi.dxyLineButton, rectangle3.Height);
			if (ᶱ.ThumbIncreaseRectangle != rectangle4)
			{
				ᶱ.ThumbIncreaseRectangle = rectangle4;
			}
			rectangle4 = new Rectangle(rectangle3.X + psbi.xyThumbTop, rectangle3.Y, psbi.xyThumbBottom - psbi.xyThumbTop, rectangle3.Height);
			if (ᶱ.TrackRectangle != rectangle4)
			{
				ᶱ.TrackRectangle = rectangle4;
			}
			if (psbi.rgstate[0] == 1)
			{
				if (ᶱ.Enabled)
				{
					ᶰ.Enabled = false;
				}
			}
			else if (!ᶱ.Enabled)
			{
				ᶱ.Enabled = true;
			}
			ᶱ.Visible = true;
		}
		else
		{
			ᶱ.Visible = false;
		}
	}

	protected virtual void PaintNonClientArea(Graphics g)
	{
		if (_0DD3)
		{
			return;
		}
		_0DD3 = true;
		try
		{
			ނ(g);
			if (ᶬ)
			{
				_1DD2();
				if (ᶰ.Visible)
				{
					ᶰ.Paint(_10CF.GetItemPaintArgs(g));
				}
				if (ᶱ.Visible)
				{
					ᶱ.Paint(_10CF.GetItemPaintArgs(g));
				}
			}
		}
		finally
		{
			_0DD3 = false;
		}
	}

	private Rectangle _1DD3()
	{
		if (_10CF.IsHandleCreated)
		{
			_0EFE.RECT r = default(_0EFE.RECT);
			_0EFE.GetWindowRect(_10CF.Handle, ref r);
			return new Rectangle(0, 0, r.Width, r.Height);
		}
		return new Rectangle(0, 0, _10CF.Width, _10CF.Height);
	}

	private void ނ(Graphics _٥)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		ElementStyle borderStyle = _10CF.BorderStyle;
		Rectangle r = _1DD3();
		if (borderStyle == null || r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(borderStyle, _٥, r);
		if (borderStyle.BackColor == Color.Transparent && (borderStyle.BackColor2.IsEmpty || borderStyle.BackColor2 == Color.Transparent))
		{
			_10CF.PaintBackground(new PaintEventArgs(_٥, r));
		}
		else
		{
			if (borderStyle.BackColor.IsEmpty && _10CF.BackColor != Color.Transparent)
			{
				SolidBrush val = new SolidBrush(_1DD4());
				try
				{
					_٥.FillRectangle((Brush)(object)val, r);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (_10CF.BackColor == Color.Transparent || (borderStyle.ނ && (borderStyle.CornerType == eCornerType.Rounded || borderStyle.CornerType == eCornerType.Diagonal || borderStyle.CornerTypeBottomLeft == eCornerType.Rounded || borderStyle.CornerTypeBottomLeft == eCornerType.Diagonal || borderStyle.CornerTypeBottomRight == eCornerType.Rounded || borderStyle.CornerTypeBottomRight == eCornerType.Diagonal || borderStyle.CornerTypeTopLeft == eCornerType.Rounded || borderStyle.CornerTypeTopLeft == eCornerType.Diagonal || borderStyle.CornerTypeTopRight == eCornerType.Rounded || borderStyle.CornerTypeTopRight == eCornerType.Diagonal)))
			{
				if (_10CF is TextBox || _10CF.BackColor == Color.Transparent)
				{
					_10CF.PaintBackground(new PaintEventArgs(_٥, r));
				}
				else
				{
					SolidBrush val2 = new SolidBrush(_10CF.BackColor);
					try
					{
						_٥.FillRectangle((Brush)(object)val2, r);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val3 = new SolidBrush(_10CF.BackColor);
				try
				{
					_٥.FillRectangle((Brush)(object)val3, r);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Rectangle r2 = r;
			if (borderStyle.ނ)
			{
				if (borderStyle.ݿ && borderStyle.BorderRightWidth > 1)
				{
					r2.Width--;
				}
				if (borderStyle.ށ && borderStyle.BorderBottomWidth > 1)
				{
					r2.Height--;
				}
			}
			_10CF.AdjustBorderRectangle(ref r2);
			elementStyleDisplayInfo.Bounds = r2;
			ElementStyleDisplay.PaintBackground(elementStyleDisplayInfo);
			_10CF.RenderNonClient(_٥);
		}
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		if (borderStyle.ނ && (borderStyle.CornerType == eCornerType.Rounded || borderStyle.CornerType == eCornerType.Diagonal))
		{
			_٥.SmoothingMode = (SmoothingMode)2;
		}
		_10CF.AdjustBorderRectangle(ref r);
		elementStyleDisplayInfo.Bounds = r;
		_1DD6(_٥, r);
		ElementStyleDisplay.PaintBorder(elementStyleDisplayInfo);
		_٥.SmoothingMode = smoothingMode;
		_1DD5(_٥, r);
	}

	private Color _1DD4()
	{
		if (!_10CF.Enabled)
		{
			if (!_10CF.DisabledBackColor.IsEmpty)
			{
				return _10CF.DisabledBackColor;
			}
			return SystemColors.Control;
		}
		return _10CF.BackColor;
	}

	private void _1DD5(Graphics _٥, Rectangle ल)
	{
		if (this.ᶪ != null)
		{
			this.ᶪ(this, new _1DDF(_٥, ल));
		}
	}

	private void _1DD6(Graphics _٥, Rectangle ल)
	{
		if (this.ᶩ != null)
		{
			this.ᶩ(this, new _1DDF(_٥, ल));
		}
	}

	private Point _1DD7(Point _065C)
	{
		Point point = _10CF.PointToScreen(new Point(-ᶯ.X, -ᶯ.Y));
		_065C.X -= point.X;
		_065C.Y -= point.Y;
		return _065C;
	}

	private Rectangle _1DD8(Rectangle ल)
	{
		Point point = _10CF.PointToScreen(new Point(-ᶯ.X, -ᶯ.Y));
		ल.X -= point.X;
		ल.Y -= point.Y;
		return ल;
	}

	public void _1DD9(IntPtr _0A65, Point ఋ)
	{
		if (_10CF is Control)
		{
			INonClientControl nonClientControl = _10CF;
			Control val = (Control)((nonClientControl is Control) ? nonClientControl : null);
			if (val != null && !val.IsDisposed && val.IsHandleCreated)
			{
				if (val.InvokeRequired)
				{
					val.BeginInvoke((Delegate)new InvokeMouseMethodDelegate(_1DDA), new object[2] { _0A65, ఋ });
				}
				else
				{
					_1DDA(_0A65, ఋ);
				}
			}
		}
		else
		{
			_1DDA(_0A65, ఋ);
		}
	}

	private void _1DDA(IntPtr _0A65, Point ఋ)
	{
		if (_0A65 == _10CF.Handle)
		{
			_1DC2(_0A65, ఋ);
		}
	}

	public void _1DDB(IntPtr _0A65, Point ఋ)
	{
		if (_10CF is Control)
		{
			INonClientControl nonClientControl = _10CF;
			Control val = (Control)((nonClientControl is Control) ? nonClientControl : null);
			if (val != null && !val.IsDisposed && val.IsHandleCreated)
			{
				if (val.InvokeRequired)
				{
					val.BeginInvoke((Delegate)new InvokeMouseMethodDelegate(_1DDC), new object[2] { _0A65, ఋ });
				}
				else
				{
					_1DDC(_0A65, ఋ);
				}
			}
		}
		else
		{
			_1DDC(_0A65, ఋ);
		}
	}

	public void _1DDC(IntPtr _0A65, Point ఋ)
	{
		if (_0A65 == _10CF.Handle && ᶬ)
		{
			if (ᶰ.IsMouseDown)
			{
				_0EFE.PostMessage(_10CF.Handle, 288, (IntPtr)1, (IntPtr)0);
			}
			else if (ᶱ.IsMouseDown)
			{
				_0EFE.PostMessage(_10CF.Handle, 288, (IntPtr)1, (IntPtr)0);
			}
		}
	}
}
