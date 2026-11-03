using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class Office2010RangeSliderColorTable
{
	public Office2010RangeSliderPartStateColorTable MinRangeSlider = new Office2010RangeSliderPartStateColorTable();

	public Office2010RangeSliderPartStateColorTable MaxRangeSlider = new Office2010RangeSliderPartStateColorTable();

	public Color TickLineColor = Color.Empty;

	public Color RangeValueLineColor = Color.Empty;

	public GradientColorTable RangeValueBackground;

	public Color RangeLineColor = Color.Empty;

	public GradientColorTable RangeBackground;

	public int RangeCornerRadius = 1;
}
