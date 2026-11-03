using System;
using System.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiChartCondition : IStiChartFilter, ICloneable
{
	Color Color { get; set; }
}
