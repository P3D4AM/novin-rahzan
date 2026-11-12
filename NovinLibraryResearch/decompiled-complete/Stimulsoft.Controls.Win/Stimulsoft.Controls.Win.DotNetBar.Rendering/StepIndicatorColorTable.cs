using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class StepIndicatorColorTable
{
	public Color BackgroundColor = Color.Empty;

	public Color IndicatorColor = Color.Empty;

	public StepIndicatorColorTable()
	{
	}

	public StepIndicatorColorTable(Color backgroundColor, Color indicatorColor)
	{
		BackgroundColor = backgroundColor;
		IndicatorColor = indicatorColor;
	}
}
