using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiListBoxElement : IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiItemElement, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiDataFilters
{
	StiItemOrientation Orientation { get; set; }

	void AddKeyMeter(IStiAppDataCell cell);

	void AddKeyMeter(IStiMeter meter);

	IStiMeter GetKeyMeter();

	void RemoveKeyMeter();

	void CreateNewKeyMeter();

	void AddNameMeter(IStiAppDataCell cell);

	void AddNameMeter(IStiMeter meter);

	IStiMeter GetNameMeter();

	void RemoveNameMeter();

	void CreateNewNameMeter();

	void CreateNextMeter(IStiAppDataCell cell);
}
