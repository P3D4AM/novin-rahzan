using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTreeViewBoxElement : IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiItemElement, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiDataFilters
{
	IStiMeter GetKeyMeter(IStiAppDataCell cell);

	IStiMeter GetKeyMeterByIndex(int index);

	void InsertKeyMeter(int index, IStiMeter meter);

	void RemoveKeyMeter(int index);

	void RemoveAllKeyMeters();

	void AddKey(IStiAppDataCell cell);

	IStiMeter AddNewKeyMeter();
}
