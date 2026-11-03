using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⴁ : _2CF9
{
	public override void CreateChart(ⳝ info)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		Graphics ܓ = info.ܓ;
		int ⳡ = info.ⳡ;
		int ⳡ2 = info.Ⳡ;
		BarMicroChartStyle style = Style;
		int num = Math.Max(style.MinBarSize, ⳡ2 / Math.Max(1, info.Ⳟ.Count - 1));
		int num2 = Math.Max(1, info.Ⳟ.Count * num / ⳡ2);
		int num3 = 0;
		double num4 = info.ⳣ;
		if (num4 > style.ZeroLineValue)
		{
			num4 = style.ZeroLineValue;
		}
		double num5 = info.Ⳣ - num4;
		if (num5 == 0.0)
		{
			num5 = 1.0;
		}
		int num6 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num2);
		_2CE7[] array = new _2CE7[num6];
		int num7 = 0;
		int num8 = Math.Min((int)((double)ⳡ * (1.0 - (style.ZeroLineValue - num4) / num5)), ⳡ);
		SmoothingMode smoothingMode = ܓ.SmoothingMode;
		ܓ.SmoothingMode = (SmoothingMode)0;
		SolidBrush val = new SolidBrush(style.PositiveBarColor);
		try
		{
			SolidBrush val2 = new SolidBrush(style.NegativeBarColor);
			try
			{
				for (int i = 0; i < info.Ⳟ.Count; i += num2)
				{
					double num9 = info.Ⳟ[i];
					int num10 = Math.Min((int)((double)ⳡ * (1.0 - (num9 - num4) / num5)), ⳡ - 1);
					Rectangle empty = Rectangle.Empty;
					if (num9 > style.ZeroLineValue)
					{
						if (num8 == ⳡ && num10 == num8)
						{
							num10--;
						}
						empty = new Rectangle(num3, num10, num, Math.Max(1, num8 - num10));
						if (num9 == info.Ⳣ && !style.HighPointBarColor.IsEmpty)
						{
							SolidBrush val3 = new SolidBrush(style.HighPointBarColor);
							try
							{
								ܓ.FillRectangle((Brush)(object)val3, empty);
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						else
						{
							ܓ.FillRectangle((Brush)(object)val, empty);
						}
						ref _2CE7 reference = ref array[num7];
						reference = new _2CE7(GetHotPointBounds(empty, isPositiveValue: true), style.PositiveBarColor, num9, num7);
					}
					else
					{
						empty = new Rectangle(num3, num8, num, Math.Max(1, num10 - num8));
						if (num9 == info.ⳣ && !style.LowPointBarColor.IsEmpty)
						{
							SolidBrush val4 = new SolidBrush(style.LowPointBarColor);
							try
							{
								ܓ.FillRectangle((Brush)(object)val4, empty);
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						else
						{
							ܓ.FillRectangle((Brush)(object)val2, empty);
						}
						ref _2CE7 reference2 = ref array[num7];
						reference2 = new _2CE7(GetHotPointBounds(empty, isPositiveValue: false), style.NegativeBarColor, num9, num7);
					}
					num7++;
					num3 += num;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		ܓ.SmoothingMode = smoothingMode;
		info.ⳟ = array;
	}
}
