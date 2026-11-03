using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public interface IStiManuallyEnteredData
{
	StiDataMode DataMode { get; set; }

	string ManuallyEnteredData { get; set; }

	StiDataTable GetManuallyEnteredDataTable();
}
