using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal struct _2CE5
{
	public double ⳬ;

	public double Ⳮ;

	public double ᦱ;

	public double _1AA1;

	public _2CE5(double slope, double intercept, double start, double end)
	{
		ⳬ = slope;
		Ⳮ = intercept;
		ᦱ = start;
		_1AA1 = end;
	}

	static _2CE5()
	{
	}
}
internal abstract class _2CF9 : ⳛ
{
	private BarMicroChartStyle _ᎌ;

	internal static int _2CEF => Dpi.Width4;

	public virtual BarMicroChartStyle Style
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

	protected virtual Rectangle GetHotPointBounds(Rectangle barBounds, bool isPositiveValue)
	{
		Rectangle empty = Rectangle.Empty;
		return (!isPositiveValue) ? new Rectangle(barBounds.X + (barBounds.Width - _2CEF * 2) / 2, barBounds.Bottom - _2CEF, _2CEF * 2, _2CEF * 2) : new Rectangle(barBounds.X + (barBounds.Width - _2CEF * 2) / 2, barBounds.Y - _2CEF, _2CEF * 2, _2CEF * 2);
	}
}
