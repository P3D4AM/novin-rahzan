using System;
using System.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiHeatmapGradientRange : ICloneable
{
	Color Color { get; set; }

	double Start { get; set; }

	string StartExpression { get; set; }

	int GetUniqueCode();
}
