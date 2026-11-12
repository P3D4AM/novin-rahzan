using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.Win.DotNetBar;

public static class StiScale2
{
	private static bool isInitialized;

	private static SystemScaleID id;

	private static double x;

	private static double y;

	public static SystemScaleID Id
	{
		get
		{
			Initialize();
			return id;
		}
	}

	public static double X
	{
		get
		{
			Initialize();
			return x;
		}
	}

	public static double Y
	{
		get
		{
			Initialize();
			return y;
		}
	}

	public static int I1 => (int)Factor;

	public static int I2 => (int)(Factor * 2.0);

	public static int I3 => (int)(Factor * 3.0);

	public static int I4 => (int)(Factor * 4.0);

	public static int I5 => (int)(Factor * 5.0);

	public static int I6 => (int)(Factor * 6.0);

	public static int I7 => (int)(Factor * 7.0);

	public static int I8 => (int)(Factor * 8.0);

	public static int I9 => (int)(Factor * 9.0);

	public static double Factor => X;

	public static double System => 1.0 / X;

	public static double Step
	{
		get
		{
			if (Factor < 1.5)
			{
				return 1.0;
			}
			if (Factor < 2.0)
			{
				return 1.5;
			}
			return 2.0;
		}
	}

	public static string StepName => GetStepName(Step);

	public static string BigStepName => GetStepName(Step * 2.0);

	public static double XX(double value)
	{
		return X * value;
	}

	public static double YY(double value)
	{
		return Y * value;
	}

	public static int XXI(double value)
	{
		return (int)Math.Ceiling(X * value);
	}

	public static int YYI(double value)
	{
		return (int)Math.Ceiling(Y * value);
	}

	private static string GetStepName(double scale)
	{
		if (scale == 1.5)
		{
			return "_x1_5";
		}
		if (scale == 2.0)
		{
			return "_x2";
		}
		if (scale == 3.0)
		{
			return "_x3";
		}
		if (scale == 4.0)
		{
			return "_x4";
		}
		return "";
	}

	private static void Initialize()
	{
		if (isInitialized)
		{
			return;
		}
		int num = 96;
		int num2 = 96;
		try
		{
			IntPtr dC = GetDC(IntPtr.Zero);
			if (dC != IntPtr.Zero)
			{
				num = GetDeviceCaps(dC, 88);
				num2 = GetDeviceCaps(dC, 90);
				ReleaseDC(IntPtr.Zero, dC);
			}
		}
		catch
		{
		}
		x = (double)num / 96.0;
		y = (double)num2 / 96.0;
		int num3 = Math.Max(num, num2);
		if (num3 <= 96)
		{
			id = SystemScaleID.x1;
		}
		else if (num3 <= 192)
		{
			id = SystemScaleID.x2;
		}
		else
		{
			id = SystemScaleID.x3;
		}
		isInitialized = true;
	}

	[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
}
