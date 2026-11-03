using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Animation;

namespace DevComponents.DotNetBar;

public static class ToastNotification
{
	private static Padding __2FCE = new Padding(16);

	private static eToastPosition __2FCF = eToastPosition.BottomCenter;

	private static eToastGlowColor __2FD0 = eToastGlowColor.Blue;

	private static int __2FD1 = 2500;

	private static Color __2FC5 = Color.FromArgb(7, 7, 7);

	private static Color __2FD2 = Color.White;

	private static Font __2FD3 = null;

	private static Color __2FD4 = Color.Brown;

	public static Padding ToastMargin
	{
		get
		{
			return __2FCE;
		}
		set
		{
			__2FCE = value;
		}
	}

	public static eToastPosition DefaultToastPosition
	{
		get
		{
			return __2FCF;
		}
		set
		{
			__2FCF = value;
		}
	}

	public static eToastGlowColor DefaultToastGlowColor
	{
		get
		{
			return __2FD0;
		}
		set
		{
			__2FD0 = value;
		}
	}

	public static int DefaultTimeoutInterval
	{
		get
		{
			return __2FD1;
		}
		set
		{
			__2FD1 = value;
		}
	}

	public static Color ToastBackColor
	{
		get
		{
			return __2FC5;
		}
		set
		{
			__2FC5 = value;
		}
	}

	public static Color ToastForeColor
	{
		get
		{
			return __2FD2;
		}
		set
		{
			__2FD2 = value;
		}
	}

	public static Font ToastFont
	{
		get
		{
			return __2FD3;
		}
		set
		{
			__2FD3 = value;
		}
	}

	public static Color CustomGlowColor
	{
		get
		{
			return __2FD4;
		}
		set
		{
			__2FD4 = value;
		}
	}

	public static void Close(Control parent)
	{
		if (parent == null)
		{
			throw new NullReferenceException("parent parameter for toast notification must be set.");
		}
		Close(parent, IntPtr.Zero);
	}

	public static void Close(Control parent, IntPtr toastId)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (parent == null)
		{
			throw new NullReferenceException("parent parameter for toast notification must be set.");
		}
		if (toastId == IntPtr.Zero)
		{
			foreach (Control item in (ArrangedElementCollection)parent.Controls)
			{
				Control val = item;
				if (val is ToastDisplay)
				{
					val.Visible = false;
				}
			}
			return;
		}
		foreach (Control item2 in (ArrangedElementCollection)parent.Controls)
		{
			Control val2 = item2;
			if (val2.Handle == toastId)
			{
				val2.Visible = false;
				break;
			}
		}
	}

	public static void UpdateToast(Control parent, IntPtr toastId, string text)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (toastId == IntPtr.Zero)
		{
			throw new ArgumentException("toastId must be non zero value.");
		}
		if (parent == null)
		{
			throw new ArgumentException("parent Control must be specified.");
		}
		foreach (Control item in (ArrangedElementCollection)parent.Controls)
		{
			Control val = item;
			if (val.Handle == toastId)
			{
				ToastDisplay toastDisplay = (ToastDisplay)(object)val;
				((Control)toastDisplay).Text = text;
				Size size = toastDisplay._2FC9(__2FCE, parent.ClientRectangle);
				if (toastDisplay.ToastPosition.HasValue)
				{
					((Control)toastDisplay).Bounds = _2FD8(toastDisplay.ToastPosition.Value, parent.ClientRectangle, size);
				}
				else
				{
					((Control)toastDisplay).Bounds = new Rectangle(((Control)toastDisplay).Bounds.X, ((Control)toastDisplay).Bounds.Y, size.Width, size.Height);
				}
				break;
			}
		}
	}

	private static IntPtr ⳋ(Control ԟ, string ⶽ, Image ແ, int _2FD5, eToastGlowColor _2FD6, eToastPosition? _2FD7, int و, int ى)
	{
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		if (ԟ == null)
		{
			throw new NullReferenceException("parent parameter for toast notification must be set.");
		}
		if (_2FD5 < 1)
		{
			_2FD5 = 0;
		}
		ToastDisplay toast = new ToastDisplay();
		((Control)toast).BackColor = Color.Transparent;
		toast.ToastBackColor = __2FC5;
		((Control)toast).ForeColor = __2FD2;
		if (__2FD3 != null)
		{
			((Control)toast).Font = __2FD3;
		}
		bool flag = _089F.ष();
		toast.Alpha = (flag ? 255 : 0);
		((Control)toast).Text = ⶽ;
		toast.Image = ແ;
		toast.GlowColor = _2FD6;
		toast.ToastPosition = _2FD7;
		ԟ.Controls.Add((Control)(object)toast);
		Size size = toast._2FC9(__2FCE, ԟ.ClientRectangle);
		((Control)toast).BringToFront();
		if (_2FD7.HasValue)
		{
			((Control)toast).Bounds = _2FD8(_2FD7.Value, ԟ.ClientRectangle, size);
		}
		else
		{
			((Control)toast).Bounds = new Rectangle(و, ى, size.Width, size.Height);
		}
		((Control)toast).Visible = true;
		IntPtr handle = ((Control)toast).Handle;
		if (!flag)
		{
			_1A6A obj = new _1A6A(new ခ(toast, "Alpha", 0, 255), AnimationEasing.EaseOutQuad, 250);
			obj.AutoDispose = true;
			obj.Start();
		}
		if (_2FD5 > 0)
		{
			BarUtilities.InvokeDelayed((MethodInvoker)delegate
			{
				_2FDB(toast);
			}, _2FD5);
		}
		return handle;
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor, eToastPosition toastPosition)
	{
		return ⳋ(parent, message, image, timeoutInterval, toastGlowColor, toastPosition, 0, 0);
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor, int x, int y)
	{
		return ⳋ(parent, message, image, timeoutInterval, toastGlowColor, null, x, y);
	}

	private static Rectangle _2FD8(eToastPosition _2FD7, Rectangle _2FD9, Size _2FDA)
	{
		Rectangle result = Rectangle.Empty;
		switch (_2FD7)
		{
		case eToastPosition.BottomCenter:
			result = new Rectangle(_2FD9.X + (_2FD9.Width - _2FDA.Width) / 2, _2FD9.Bottom - _2FDA.Height - __2FCE.Bottom, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.BottomLeft:
			result = new Rectangle(_2FD9.X + __2FCE.Left, _2FD9.Bottom - _2FDA.Height - __2FCE.Bottom, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.BottomRight:
			result = new Rectangle(_2FD9.Right - __2FCE.Right - _2FDA.Width, _2FD9.Bottom - _2FDA.Height - __2FCE.Bottom, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.MiddleCenter:
			result = new Rectangle(_2FD9.X + (_2FD9.Width - _2FDA.Width) / 2, _2FD9.Y + (_2FD9.Height - _2FDA.Height) / 2, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.MiddleLeft:
			result = new Rectangle(_2FD9.X + __2FCE.Left, _2FD9.Y + (_2FD9.Height - _2FDA.Height) / 2, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.MiddleRight:
			result = new Rectangle(_2FD9.Right - __2FCE.Right - _2FDA.Width, _2FD9.Y + (_2FD9.Height - _2FDA.Height) / 2, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.TopCenter:
			result = new Rectangle(_2FD9.X + (_2FD9.Width - _2FDA.Width) / 2, _2FD9.Y + __2FCE.Top, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.TopLeft:
			result = new Rectangle(_2FD9.X + __2FCE.Left, _2FD9.Y + __2FCE.Top, _2FDA.Width, _2FDA.Height);
			break;
		case eToastPosition.TopRight:
			result = new Rectangle(_2FD9.Right - __2FCE.Right - _2FDA.Width, _2FD9.Y + __2FCE.Top, _2FDA.Width, _2FDA.Height);
			break;
		}
		return result;
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor)
	{
		return Show(parent, message, image, timeoutInterval, toastGlowColor, __2FCF);
	}

	public static IntPtr Show(Control parent, string message)
	{
		return Show(parent, message, null, __2FD1, __2FD0);
	}

	public static IntPtr Show(Control parent, string message, eToastPosition toastPosition)
	{
		return Show(parent, message, null, __2FD1, __2FD0, toastPosition);
	}

	public static IntPtr Show(Control parent, string message, Image image)
	{
		return Show(parent, message, image, __2FD1, __2FD0);
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval)
	{
		return Show(parent, message, image, timeoutInterval, __2FD0);
	}

	public static IntPtr Show(Control parent, string message, int timeoutInterval)
	{
		return Show(parent, message, null, timeoutInterval, __2FD0);
	}

	public static IntPtr Show(Control parent, string message, int timeoutInterval, eToastPosition toastPosition)
	{
		return Show(parent, message, null, timeoutInterval, __2FD0, toastPosition);
	}

	private static void _2FDB(ToastDisplay _2FDC)
	{
		bool flag = _089F.ष();
		if (((Control)_2FDC).IsDisposed)
		{
			if (((Control)_2FDC).Parent != null)
			{
				((Control)_2FDC).Parent.Controls.Remove((Control)(object)_2FDC);
			}
			return;
		}
		if (flag)
		{
			((Control)_2FDC).Visible = false;
			((Control)_2FDC).Parent.Controls.Remove((Control)(object)_2FDC);
			((Component)(object)_2FDC).Dispose();
			return;
		}
		_1A6A obj = new _1A6A(new ခ(_2FDC, "Alpha", 255, 0), AnimationEasing.EaseInQuad, 250);
		obj.AutoDispose = true;
		obj.Start();
		obj.AnimationCompleted += delegate
		{
			if (((Control)_2FDC).Parent != null)
			{
				((Control)_2FDC).Parent.Controls.Remove((Control)(object)_2FDC);
			}
			((Component)(object)_2FDC).Dispose();
		};
	}
}
