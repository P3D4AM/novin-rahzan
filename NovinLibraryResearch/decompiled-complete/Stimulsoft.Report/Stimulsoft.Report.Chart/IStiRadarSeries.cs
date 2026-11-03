using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart;

public interface IStiRadarSeries : IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	IStiMarker Marker { get; set; }
}
