using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts;

internal abstract class ⳛ
{
	internal const int Ⳝ = 3;

	public abstract void CreateChart(ⳝ info);

	protected virtual Point[] GetChartPointBounds(Point p)
	{
		return new Point[5]
		{
			new Point(p.X, p.Y - 3),
			new Point(p.X - 3, p.Y),
			new Point(p.X, p.Y + 3),
			new Point(p.X + 3, p.Y),
			new Point(p.X, p.Y - 3)
		};
	}
}
