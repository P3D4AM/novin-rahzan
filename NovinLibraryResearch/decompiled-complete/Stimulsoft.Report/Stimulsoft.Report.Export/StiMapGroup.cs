using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiMapGroup
{
	public double MinValue { get; set; }

	public double MaxValue { get; set; }

	public Brush Fill { get; set; }

	public StiBrush Fill1 { get; set; }

	public double GetOpacity(double value)
	{
		if (MaxValue == MinValue)
		{
			return 1.0;
		}
		double num = MaxValue - MinValue;
		double num2 = (value - MinValue) / num;
		return num2 * 0.8 + 0.2;
	}
}
