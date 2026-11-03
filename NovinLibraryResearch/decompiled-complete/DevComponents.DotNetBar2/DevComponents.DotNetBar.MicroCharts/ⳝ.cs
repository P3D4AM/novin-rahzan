using System;
using System.Collections.Generic;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal class ⳝ
{
	public List<double> Ⳟ;

	public _2CE7[] ⳟ;

	public Graphics ܓ;

	public int Ⳡ;

	public int ⳡ;

	public double Ⳣ;

	public double ⳣ;

	public double ⳤ;

	public _2CE5 _2CE5;

	public ⳝ(List<double> dataPoints, Graphics graphics, int chartWidth, int chartHeight, double dataMax, double dataMin)
	{
		Ⳟ = dataPoints;
		ܓ = graphics;
		Ⳡ = chartWidth;
		ⳡ = chartHeight;
		ⳟ = null;
		Ⳣ = dataMax;
		ⳣ = dataMin;
		ⳤ = 0.0;
		_2CE5 = default(_2CE5);
		_2CE6();
	}

	private void _2CE6()
	{
		if (Ⳟ == null || Ⳟ.Count == 0)
		{
			return;
		}
		double num = Ⳟ[0];
		double num2 = Ⳟ[0];
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		ⳤ = Math.Abs(Ⳟ[0]);
		for (int i = 1; i < Ⳟ.Count; i++)
		{
			double num7 = Ⳟ[i];
			if (num7 < num)
			{
				num = num7;
			}
			if (num7 > num2)
			{
				num2 = num7;
			}
			ⳤ += Math.Abs(num7);
			num4 += num7 * (double)(i + 1);
			num3 = num7 * num7;
			num6 += num7;
			num5 += (double)(i + 1);
		}
		double num8 = 0.0;
		double num9 = 0.0;
		double num10 = 0.0;
		double num11 = 0.0;
		try
		{
			num8 = ((double)Ⳟ.Count * num4 - num5 * num6) / ((double)Ⳟ.Count * num3 - num5 * num5);
		}
		catch (DivideByZeroException)
		{
		}
		num9 = (num6 - num8 * num5) / (double)Ⳟ.Count;
		num10 = Math.Max(0.0, num8 * Ⳟ[0] + num9);
		num11 = num8 * Ⳟ[Ⳟ.Count - 1] + num9;
		_2CE5 = new _2CE5(num8, num9, num10, num11);
		if (double.IsNaN(Ⳣ))
		{
			Ⳣ = num2;
		}
		if (double.IsNaN(ⳣ))
		{
			ⳣ = num;
		}
	}
}
