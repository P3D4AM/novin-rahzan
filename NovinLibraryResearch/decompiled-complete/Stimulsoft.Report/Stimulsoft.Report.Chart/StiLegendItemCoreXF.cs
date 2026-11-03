namespace Stimulsoft.Report.Chart;

public class StiLegendItemCoreXF
{
	public string Text { get; }

	public IStiSeries Series { get; }

	public int Index { get; } = -1;


	public int ColorIndex { get; }

	public StiLegendItemCoreXF(string text, IStiSeries series, int index, int colorIndex)
	{
		Text = text;
		Series = series;
		Index = index;
		ColorIndex = colorIndex;
	}
}
