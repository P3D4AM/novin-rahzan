using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineExponential : StiTrendLine, IStiTrendLineExponential, IStiTrendLine, ICloneable, IStiDefault, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiTrendLineExponential;

	public override StiTrendLine CreateNew()
	{
		return new StiTrendLineExponential();
	}

	public StiTrendLineExponential()
	{
		base.Core = new StiTrendLineExponentialCoreXF(this);
	}
}
