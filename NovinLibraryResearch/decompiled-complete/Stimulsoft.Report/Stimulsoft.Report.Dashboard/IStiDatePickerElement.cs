using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiDatePickerElement : IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiDataFilters
{
	StiDateCondition Condition { get; set; }

	StiDateSelectionMode SelectionMode { get; set; }

	StiInitialDateRangeSelection InitialRangeSelection { get; set; }

	StiInitialDateRangeSelectionSource InitialRangeSelectionSource { get; set; }

	void AddValueMeter(IStiAppDataCell cell);

	void AddValueMeter(IStiMeter meter);

	IStiMeter GetValueMeter();

	void RemoveValueMeter();

	void CreateNewValueMeter();
}
