using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Animation;

namespace Stimulsoft.Controls.Win.DotNetBar;

public static class ToastNotification
{
	private static Padding _ToastMargin = new Padding(16);

	private static eToastPosition _DefaultToastPosition = eToastPosition.BottomCenter;

	private static eToastGlowColor _DefaultToastGlowColor = eToastGlowColor.Blue;

	private static int _DefaultTimeoutInterval = 2500;

	private static Color _ToastBackColor = Color.FromArgb(7, 7, 7);

	private static Color _ToastForeColor = Color.White;

	private static Font _ToastFont = null;

	private static Color _CustomGlowColor = Color.Brown;

	public static Padding ToastMargin
	{
		get
		{
			return _ToastMargin;
		}
		set
		{
			_ToastMargin = value;
		}
	}

	public static eToastPosition DefaultToastPosition
	{
		get
		{
			return _DefaultToastPosition;
		}
		set
		{
			_DefaultToastPosition = value;
		}
	}

	public static eToastGlowColor DefaultToastGlowColor
	{
		get
		{
			return _DefaultToastGlowColor;
		}
		set
		{
			_DefaultToastGlowColor = value;
		}
	}

	public static int DefaultTimeoutInterval
	{
		get
		{
			return _DefaultTimeoutInterval;
		}
		set
		{
			_DefaultTimeoutInterval = value;
		}
	}

	public static Color ToastBackColor
	{
		get
		{
			return _ToastBackColor;
		}
		set
		{
			_ToastBackColor = value;
		}
	}

	public static Color ToastForeColor
	{
		get
		{
			return _ToastForeColor;
		}
		set
		{
			_ToastForeColor = value;
		}
	}

	public static Font ToastFont
	{
		get
		{
			return _ToastFont;
		}
		set
		{
			_ToastFont = value;
		}
	}

	public static Color CustomGlowColor
	{
		get
		{
			return _CustomGlowColor;
		}
		set
		{
			_CustomGlowColor = value;
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
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
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

	private static IntPtr ShowInternal(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor, eToastPosition? toastPosition, int x, int y)
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		if (parent == null)
		{
			throw new NullReferenceException("parent parameter for toast notification must be set.");
		}
		if (timeoutInterval < 1)
		{
			timeoutInterval = 0;
		}
		ToastDisplay toast = new ToastDisplay();
		((Control)toast).BackColor = Color.Transparent;
		toast.ToastBackColor = _ToastBackColor;
		((Control)toast).ForeColor = _ToastForeColor;
		if (_ToastFont != null)
		{
			((Control)toast).Font = _ToastFont;
		}
		bool flag = NativeFunctions.IsTerminalSession();
		toast.Alpha = (flag ? 255 : 0);
		((Control)toast).Text = message;
		toast.Image = image;
		toast.GlowColor = toastGlowColor;
		parent.Controls.Add((Control)(object)toast);
		Size toastSize = toast.DesiredSize(_ToastMargin, parent.ClientRectangle);
		((Control)toast).BringToFront();
		if (toastPosition.HasValue)
		{
			((Control)toast).Bounds = CalculateToastBounds(toastPosition.Value, parent.ClientRectangle, toastSize);
		}
		else
		{
			((Control)toast).Bounds = new Rectangle(x, y, toastSize.Width, toastSize.Height);
		}
		((Control)toast).Visible = true;
		IntPtr handle = ((Control)toast).Handle;
		if (!flag)
		{
			AnimationInt animationInt = new AnimationInt(new AnimationRequest(toast, "Alpha", 0, 255), AnimationEasing.EaseOutQuad, 250);
			animationInt.AutoDispose = true;
			animationInt.Start();
		}
		if (timeoutInterval > 0)
		{
			BarUtilities.InvokeDelayed((MethodInvoker)delegate
			{
				CloseToast(toast);
			}, timeoutInterval);
		}
		return handle;
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor, eToastPosition toastPosition)
	{
		return ShowInternal(parent, message, image, timeoutInterval, toastGlowColor, toastPosition, 0, 0);
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor, int x, int y)
	{
		return ShowInternal(parent, message, image, timeoutInterval, toastGlowColor, null, x, y);
	}

	private static Rectangle CalculateToastBounds(eToastPosition toastPosition, Rectangle parentClientRectangle, Size toastSize)
	{
		Rectangle result = Rectangle.Empty;
		switch (toastPosition)
		{
		case eToastPosition.BottomCenter:
			result = new Rectangle(parentClientRectangle.X + (parentClientRectangle.Width - toastSize.Width) / 2, parentClientRectangle.Bottom - toastSize.Height - _ToastMargin.Bottom, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.BottomLeft:
			result = new Rectangle(parentClientRectangle.X + _ToastMargin.Left, parentClientRectangle.Bottom - toastSize.Height - _ToastMargin.Bottom, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.BottomRight:
			result = new Rectangle(parentClientRectangle.Right - _ToastMargin.Right - toastSize.Width, parentClientRectangle.Bottom - toastSize.Height - _ToastMargin.Bottom, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.MiddleCenter:
			result = new Rectangle(parentClientRectangle.X + (parentClientRectangle.Width - toastSize.Width) / 2, parentClientRectangle.Y + (parentClientRectangle.Height - toastSize.Height) / 2, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.MiddleLeft:
			result = new Rectangle(parentClientRectangle.X + _ToastMargin.Left, parentClientRectangle.Y + (parentClientRectangle.Height - toastSize.Height) / 2, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.MiddleRight:
			result = new Rectangle(parentClientRectangle.Right - _ToastMargin.Right - toastSize.Width, parentClientRectangle.Y + (parentClientRectangle.Height - toastSize.Height) / 2, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.TopCenter:
			result = new Rectangle(parentClientRectangle.X + (parentClientRectangle.Width - toastSize.Width) / 2, parentClientRectangle.Y + _ToastMargin.Top, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.TopLeft:
			result = new Rectangle(parentClientRectangle.X + _ToastMargin.Left, parentClientRectangle.Y + _ToastMargin.Top, toastSize.Width, toastSize.Height);
			break;
		case eToastPosition.TopRight:
			result = new Rectangle(parentClientRectangle.Right - _ToastMargin.Right - toastSize.Width, parentClientRectangle.Y + _ToastMargin.Top, toastSize.Width, toastSize.Height);
			break;
		}
		return result;
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval, eToastGlowColor toastGlowColor)
	{
		return Show(parent, message, image, timeoutInterval, toastGlowColor, eToastPosition.BottomCenter);
	}

	public static IntPtr Show(Control parent, string message)
	{
		return Show(parent, message, null, _DefaultTimeoutInterval, _DefaultToastGlowColor);
	}

	public static IntPtr Show(Control parent, string message, eToastPosition toastPosition)
	{
		return Show(parent, message, null, _DefaultTimeoutInterval, _DefaultToastGlowColor, toastPosition);
	}

	public static IntPtr Show(Control parent, string message, Image image)
	{
		return Show(parent, message, image, _DefaultTimeoutInterval, _DefaultToastGlowColor);
	}

	public static IntPtr Show(Control parent, string message, Image image, int timeoutInterval)
	{
		return Show(parent, message, image, timeoutInterval, _DefaultToastGlowColor);
	}

	public static IntPtr Show(Control parent, string message, int timeoutInterval)
	{
		return Show(parent, message, null, timeoutInterval, _DefaultToastGlowColor);
	}

	public static IntPtr Show(Control parent, string message, int timeoutInterval, eToastPosition toastPosition)
	{
		return Show(parent, message, null, timeoutInterval, _DefaultToastGlowColor, toastPosition);
	}

	private static void CloseToast(ToastDisplay toast)
	{
		bool flag = NativeFunctions.IsTerminalSession();
		if (((Control)toast).IsDisposed)
		{
			if (((Control)toast).Parent != null)
			{
				((Control)toast).Parent.Controls.Remove((Control)(object)toast);
			}
			return;
		}
		if (flag)
		{
			((Control)toast).Visible = false;
			((Control)toast).Parent.Controls.Remove((Control)(object)toast);
			((Component)(object)toast).Dispose();
			return;
		}
		AnimationInt animationInt = new AnimationInt(new AnimationRequest(toast, "Alpha", 255, 0), AnimationEasing.EaseInQuad, 250);
		animationInt.AutoDispose = true;
		animationInt.Start();
		animationInt.AnimationCompleted += delegate
		{
			if (((Control)toast).Parent != null)
			{
				((Control)toast).Parent.Controls.Remove((Control)(object)toast);
			}
			((Component)(object)toast).Dispose();
		};
	}
}
