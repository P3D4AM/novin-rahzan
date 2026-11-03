using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineNone : StiTrendLine, IStiTrendLineNone, IStiTrendLine, ICloneable, IStiDefault, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiTrendLineNone;

	public override StiTrendLine CreateNew()
	{
		return new StiTrendLineNone();
	}

	public StiTrendLineNone()
	{
		base.Core = new StiTrendLineNoneCoreXF(this);
	}
}
