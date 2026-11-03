using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiElement : IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval
{
	string Key { get; set; }

	StiReport Report { get; set; }

	StiPage Page { get; set; }

	string Name { get; set; }

	double Zoom { get; }

	bool IsDefined { get; }

	bool IsDesigning { get; }

	bool IsEnabled { get; }

	bool IsQuerable { get; }

	List<IStiMeter> FetchAllMeters();

	List<IStiMeter> GetMeters();

	List<StiPage> GetNestedPages();
}
