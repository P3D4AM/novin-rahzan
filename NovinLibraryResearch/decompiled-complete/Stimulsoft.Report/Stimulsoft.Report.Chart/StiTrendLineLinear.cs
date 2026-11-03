using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineLinear : StiTrendLine, IStiTrendLineLinear, IStiTrendLine, ICloneable, IStiDefault, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiTrendLineLinear;

	public override StiTrendLine CreateNew()
	{
		return new StiTrendLineLinear();
	}

	public StiTrendLineLinear()
	{
		base.Core = new StiTrendLineLinearCoreXF(this);
	}
}
