using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report.Dashboard;

public interface IStiGaugeElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiManuallyEnteredData
{
	StiGaugeRangeType RangeType { get; set; }

	StiGaugeRangeMode RangeMode { get; set; }

	StiGaugeCalculationMode CalculationMode { get; set; }

	StiGaugeType Type { get; set; }

	decimal Minimum { get; set; }

	decimal Maximum { get; set; }

	bool ShortValue { get; set; }

	StiFormatService ValueFormat { get; set; }

	List<StiAnimation> PreviousAnimations { get; set; }

	void AddValue(IStiAppDataCell cell);

	void AddValue(IStiMeter meter);

	void RemoveValue();

	IStiMeter GetValue();

	IStiMeter GetValue(IStiMeter meter);

	void CreateNewValue();

	void AddSeries(IStiAppDataCell cell);

	void AddSeries(IStiMeter meter);

	void RemoveSeries();

	IStiMeter GetSeries();

	IStiMeter GetSeries(IStiMeter meter);

	void CreateNewSeries();

	void AddTarget(IStiAppDataCell cell);

	void AddTarget(IStiMeter meter);

	void RemoveTarget();

	IStiMeter GetTarget();

	IStiMeter GetTarget(IStiMeter meter);

	void CreateNewTarget();

	List<IStiGaugeRange> GetRanges();

	IStiGaugeRange AddRange();

	void RemoveRange(int index);

	void CreatedDefaultRanges();

	StiGauge GetGaugeComponent(SizeD? size = null);
}
