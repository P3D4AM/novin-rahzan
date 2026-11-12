using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Helpers;

internal static class DoubleHelpers
{
	[StructLayout(LayoutKind.Explicit)]
	private struct NanUnion
	{
		[FieldOffset(0)]
		internal double DoubleValue;

		[FieldOffset(0)]
		internal ulong UintValue;
	}

	public static bool AreClose(double value1, double value2)
	{
		if (value1 == value2)
		{
			return true;
		}
		double num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.220446049250313E-16;
		double num2 = value1 - value2;
		if (0.0 - num < num2)
		{
			return num > num2;
		}
		return false;
	}

	public static bool IsZero(double value)
	{
		return Math.Abs(value) < 2.220446049250313E-15;
	}

	public static bool IsNaN(double value)
	{
		NanUnion nanUnion = default(NanUnion);
		nanUnion.DoubleValue = value;
		ulong num = nanUnion.UintValue & 0xFFF0000000000000uL;
		ulong num2 = nanUnion.UintValue & 0xFFFFFFFFFFFFFuL;
		if (num != 9218868437227405312L && num != 18442240474082181120uL)
		{
			return false;
		}
		return num2 != 0;
	}
}
