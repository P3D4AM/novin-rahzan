using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public interface IStiPivotTableCreator
{
	void Create(IStiPivotTableElement element, StiDataTable dataTable);
}
