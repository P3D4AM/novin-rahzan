using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiControlElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat
{
}
