using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineLogarithmic : StiTrendLine, IStiTrendLineLogarithmic, IStiTrendLine, ICloneable, IStiDefault, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiTrendLineLogarithmic;

	public override StiTrendLine CreateNew()
	{
		return new StiTrendLineLogarithmic();
	}

	public StiTrendLineLogarithmic()
	{
		base.Core = new StiTrendLineLogarithmicCoreXF(this);
	}
}
