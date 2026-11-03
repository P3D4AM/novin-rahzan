using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiIndicatorElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTopN, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData
{
	StiFontIconSet IconSet { get; set; }

	StiFontIcons Icon { get; set; }

	StiIconAlignment IconAlignment { get; set; }

	Color GlyphColor { get; set; }

	byte[] CustomIcon { get; set; }

	StiTargetMode TargetMode { get; set; }

	StiFontSizeMode FontSizeMode { get; set; }

	Font Font { get; set; }

	StiIndicatorIconMode IconMode { get; set; }

	StiIndicatorIconRangeMode IconRangeMode { get; set; }

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

	void AddIndicatorCondition(StiIndicatorFieldCondition field, StiFilterCondition condition, string value, StiFontIcons icon, Color iconColor, StiFontIcons targetIcon, Color targetIconColor, byte[] customIcon, StiIconAlignment iconAlignment, StiIconAlignment targetIconAlignment, StiIndicatorConditionPermissions permissions, Font font, Color textColor, Color backColor);

	List<IStiIndicatorElementCondition> FetchIndicatorConditions();

	void ClearIndicatorConditions();

	List<IStiIndicatorIconRange> GetIconRanges();

	IStiIndicatorIconRange AddRange();

	void RemoveRange(int index);

	void CreatedDefaultRanges();
}
