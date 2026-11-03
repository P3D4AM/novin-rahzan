using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Dashboard;

public interface IStiCardsElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiConvertibleElement, IStiDataFilters
{
	void CreateMeters(IStiTableElement tableElement);

	void CreateMeters(StiDataSource dataSource);

	void CreateMeter(IStiAppDataCell cell);

	void RemoveMeter(int index);

	void RemoveAllMeters();

	void InsertMeter(int index, IStiMeter meter);

	void InsertNewDimension(int index);

	void InsertNewMeasure(int index);

	IStiMeter GetMeasure(IStiAppDataCell cell);

	IStiMeter GetDimension(IStiAppDataCell cell);
}
