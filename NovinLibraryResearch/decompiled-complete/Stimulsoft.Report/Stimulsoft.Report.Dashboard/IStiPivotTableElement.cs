using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiPivotTableElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement
{
	StiSummaryDirection SummaryDirection { get; set; }

	List<IStiPivotTableElementCondition> PivotTableConditions { get; set; }

	void CreateNewColumn();

	IStiMeter GetColumn(IStiAppDataCell cell);

	IStiMeter GetColumn(IStiMeter meter);

	IStiMeter GetColumnByIndex(int index);

	void InsertColumn(int index, IStiMeter meter);

	void RemoveColumn(int index);

	void RemoveAllColumns();

	void CreateNewRow();

	IStiMeter GetRow(IStiAppDataCell cell);

	IStiMeter GetRow(IStiMeter meter);

	IStiMeter GetRowByIndex(int index);

	void InsertRow(int index, IStiMeter meter);

	void RemoveRow(int index);

	void RemoveAllRows();

	void CreateNewSummary();

	IStiMeter GetSummary(IStiAppDataCell cell);

	IStiMeter GetSummary(IStiMeter meter);

	IStiMeter GetSummaryByIndex(int index);

	void InsertSummary(int index, IStiMeter meter);

	void RemoveSummary(int index);

	void RemoveAllSummaries();

	void CreateNextMeter(IStiAppDataCell cell);

	List<IStiMeter> GetAllMeters();

	void AddPivotTableCondition(string keyValueMeter, StiFilterDataType dataType, StiFilterCondition condition, string value, Font font, Color textColor, Color backColor, StiConditionPermissions permissions, StiFontIcons icon, StiIconAlignment iconAlignment, byte[] customIcon, Color iconColor);
}
