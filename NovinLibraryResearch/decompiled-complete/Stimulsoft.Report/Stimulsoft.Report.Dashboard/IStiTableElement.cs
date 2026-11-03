using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTableElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiUserFilters, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiFont, IStiConvertibleElement
{
	Font HeaderFont { get; set; }

	Color HeaderForeColor { get; set; }

	StiTableSizeMode SizeMode { get; set; }

	Color ForeColor { get; set; }

	Font FooterFont { get; set; }

	Color FooterForeColor { get; set; }

	int FrozenColumns { get; set; }

	List<IStiTableElementCondition> TableConditions { get; set; }

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

	void AddTableCondition(string[] keyDataFiledMeters, string[] keyDestinationMeters, StiFilterDataType dataType, StiFilterCondition condition, string value, StiTableConditionPermissions permissions, Font font, Color foreColor, Color backColor, bool isExpression);
}
