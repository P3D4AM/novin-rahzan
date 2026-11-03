using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Chart;

public interface IStiAxisTicks : ICloneable, IStiJsonReportObject, IStiDefault
{
	float LengthUnderLabels { get; set; }

	float Length { get; set; }

	float MinorLength { get; set; }

	int MinorCount { get; set; }

	int Step { get; set; }

	bool MinorVisible { get; set; }

	bool Visible { get; set; }
}
