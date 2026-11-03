using System;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⴂ : ⳛ
{
	private HundredPctMicroChartStyle _ᎌ;

	public virtual HundredPctMicroChartStyle Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			_ᎌ = value;
		}
	}

	private static int _2CEF => Dpi.Width4;

	public override void CreateChart(ⳝ info)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		int ⳡ = info.ⳡ;
		int ⳡ2 = info.Ⳡ;
		int num = 0;
		int y = 0;
		HundredPctMicroChartStyle style = Style;
		int num2 = Math.Max(style.MinBarSize, ⳡ2 / Math.Max(1, info.Ⳟ.Count));
		int num3 = Math.Max(1, info.Ⳟ.Count * num2 / ⳡ2);
		Graphics ܓ = info.ܓ;
		double num4 = info.ⳤ;
		if (num3 > 1)
		{
			num4 = 0.0;
			for (int i = 0; i < info.Ⳟ.Count; i += num3)
			{
				num4 += Math.Abs(info.Ⳟ[i]);
			}
		}
		int count = style.BarColors.Count;
		int num5 = 0;
		int num6 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num3);
		_2CE7[] array = new _2CE7[num6];
		int num7 = 0;
		Pen val = new Pen(style.BarOutlineColor, 1f);
		try
		{
			for (int j = 0; j < info.Ⳟ.Count; j += num3)
			{
				double num8 = Math.Abs(info.Ⳟ[j]);
				Rectangle rectangle = new Rectangle(num, y, (int)Math.Round((double)ⳡ2 * num8 / num4), ⳡ);
				SolidBrush val2 = new SolidBrush(style.BarColors[num5]);
				try
				{
					ܓ.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				ܓ.DrawRectangle(val, rectangle);
				ref _2CE7 reference = ref array[num7];
				reference = new _2CE7(_2CF1(rectangle), rectangle, style.BarColors[num5], num8, j);
				num7++;
				num += rectangle.Width;
				num5++;
				if (num5 >= count)
				{
					num5 = 0;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		info.ⳟ = array;
	}

	private Rectangle _2CF1(Rectangle ⴃ)
	{
		return new Rectangle(ⴃ.X + (ⴃ.Width - _2CEF * 2) / 2, ⴃ.Y + (ⴃ.Height - _2CEF * 2) / 2, _2CEF * 2, _2CEF * 2);
	}
}
