using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisDateTimeStep : ICloneable, IStiJsonReportObject, IStiDefault
{
	StiTimeDateStep Step { get; set; }

	int NumberOfValues { get; set; }

	bool Interpolation { get; set; }
}
