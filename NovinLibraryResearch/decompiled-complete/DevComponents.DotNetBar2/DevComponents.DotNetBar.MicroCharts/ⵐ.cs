using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⵐ : _2CF9
{
	public override void CreateChart(ⳝ info)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		Graphics ܓ = info.ܓ;
		int ⳡ = info.ⳡ;
		int ⳡ2 = info.Ⳡ;
		BarMicroChartStyle style = Style;
		int num = Math.Max(style.MinBarSize, ⳡ2 / Math.Max(1, info.Ⳟ.Count - 1));
		int num2 = Math.Max(1, info.Ⳟ.Count * num / ⳡ2);
		int num3 = 0;
		_ = style.ZeroLineValue;
		int num4 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num2);
		_2CE7[] array = new _2CE7[num4];
		int num5 = 0;
		int num6 = ⳡ / 2;
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
					double num7 = info.Ⳟ[i];
					if (num7 > style.ZeroLineValue)
					{
						Rectangle rectangle = new Rectangle(num3, 0, num, Math.Max(1, num6));
						if (num7 == info.Ⳣ && !style.HighPointBarColor.IsEmpty)
						{
							SolidBrush val3 = new SolidBrush(style.HighPointBarColor);
							try
							{
								ܓ.FillRectangle((Brush)(object)val3, rectangle);
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						else
						{
							ܓ.FillRectangle((Brush)(object)val, rectangle);
						}
						ref _2CE7 reference = ref array[num5];
						reference = new _2CE7(GetHotPointBounds(rectangle, isPositiveValue: true), new Rectangle(rectangle.X, 0, rectangle.Width, ⳡ), style.PositiveBarColor, num7, num5);
					}
					else
					{
						Rectangle rectangle2 = new Rectangle(num3, num6, num, Math.Max(1, ⳡ - num6));
						if (num7 == info.ⳣ && !style.LowPointBarColor.IsEmpty)
						{
							SolidBrush val4 = new SolidBrush(style.LowPointBarColor);
							try
							{
								ܓ.FillRectangle((Brush)(object)val4, rectangle2);
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						else
						{
							ܓ.FillRectangle((Brush)(object)val2, rectangle2);
						}
						ref _2CE7 reference2 = ref array[num5];
						reference2 = new _2CE7(GetHotPointBounds(rectangle2, isPositiveValue: false), new Rectangle(rectangle2.X, 0, rectangle2.Width, ⳡ), style.NegativeBarColor, num7, num5);
					}
					num5++;
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
