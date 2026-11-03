using System;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⵑ : ⳛ
{
	private PieMicroChartStyle _ᎌ;

	public virtual PieMicroChartStyle Style
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

	public override void CreateChart(ⳝ info)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		int num = Math.Min(info.ⳡ, info.Ⳡ) - 1;
		int num2 = (info.Ⳡ - num) / 2;
		int num3 = (info.ⳡ - num) / 2;
		int num4 = Math.Max(1, info.Ⳟ.Count * 15 / 360);
		_ = Style;
		Graphics ܓ = info.ܓ;
		double num5 = info.ⳤ;
		if (num4 > 1)
		{
			num5 = 0.0;
			for (int i = 0; i < info.Ⳟ.Count; i += num4)
			{
				num5 += Math.Abs(info.Ⳟ[i]);
			}
		}
		float num6 = 0f;
		int count = _ᎌ.SliceColors.Count;
		int num7 = 0;
		int num8 = (int)Math.Ceiling((double)info.Ⳟ.Count / (double)num4);
		_2CE7[] array = new _2CE7[num8];
		int num9 = 0;
		Pen val = new Pen(_ᎌ.SliceOutlineColor, 1f);
		try
		{
			for (int j = 0; j < info.Ⳟ.Count; j += num4)
			{
				double num10 = Math.Abs(info.Ⳟ[j]);
				float num11 = (float)Math.Max(1.0, Math.Round((float)(360.0 * (num10 / num5))));
				SolidBrush val2 = new SolidBrush(_ᎌ.SliceColors[num7]);
				try
				{
					ܓ.FillPie((Brush)(object)val2, (float)num2, (float)num3, (float)num, (float)num, num6, num11);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				ܓ.DrawPie(val, (float)num2, (float)num3, (float)num, (float)num, num6, num11);
				Rectangle bounds = new Rectangle(num2 + (int)((double)(num / 2) + (double)(num / 3) * Math.Cos((double)(num6 + num11 / 2f) * Math.PI / 180.0)), num3 + (int)((double)(num / 2) + (double)(num / 3) * Math.Sin((double)(num6 + num11 / 2f) * Math.PI / 180.0)), 1, 1);
				bounds.Inflate(4, 4);
				ref _2CE7 reference = ref array[num9];
				reference = new _2CE7(bounds, _ᎌ.SliceColors[num7], num10, num6, num11, j);
				num9++;
				num6 += num11;
				num7++;
				if (num7 >= count)
				{
					num7 = 0;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		info.ⳟ = array;
	}
}
