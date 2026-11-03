using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiPanel : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiBackColor
{
	List<IStiElement> GetElements(bool nested, string group = null);

	List<IStiMeter> GetMeters(bool nested, string group = null);
}
