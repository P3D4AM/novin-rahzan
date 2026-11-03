using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

public interface IStiRangeSeries
{
	double?[] ValuesEnd { get; set; }

	string ValueDataColumnEnd { get; set; }

	StiListOfValuesEndExpression ListOfValuesEnd { get; set; }
}
