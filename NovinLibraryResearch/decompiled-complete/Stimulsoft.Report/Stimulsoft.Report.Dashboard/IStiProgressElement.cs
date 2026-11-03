using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public interface IStiProgressElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTopN, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData
{
	bool ColorEach { get; set; }

	StiProgressElementMode Mode { get; set; }

	Font Font { get; set; }

	void AddValue(IStiAppDataCell cell);

	void AddValue(IStiMeter meter);

	void RemoveValue();

	IStiMeter GetValue();

	IStiMeter GetValue(IStiMeter meter);

	void CreateNewValue();

	void AddTarget(IStiAppDataCell cell);

	void AddTarget(IStiMeter meter);

	void RemoveTarget();

	IStiMeter GetTarget();

	IStiMeter GetTarget(IStiMeter meter);

	void CreateNewTarget();

	void AddSeries(IStiAppDataCell cell);

	void AddSeries(IStiMeter meter);

	void RemoveSeries();

	IStiMeter GetSeries();

	IStiMeter GetSeries(IStiMeter meter);

	void CreateNewSeries();

	void AddProgressCondition(StiProgressFieldCondition field, StiFilterCondition condition, string value, StiProgressConditionPermissions permissions, Font font, Color textColor, Color color, Color trackColor);

	List<IStiProgressElementCondition> FetchProgressConditions();

	void ClearProgressConditions();
}
