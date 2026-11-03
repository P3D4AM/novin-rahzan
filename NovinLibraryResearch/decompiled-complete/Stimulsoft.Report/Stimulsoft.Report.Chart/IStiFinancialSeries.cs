namespace Stimulsoft.Report.Chart;

public interface IStiFinancialSeries
{
	double?[] ValuesOpen { get; set; }

	double?[] ValuesClose { get; set; }

	double?[] ValuesHigh { get; set; }

	double?[] ValuesLow { get; set; }

	string ValueDataColumnOpen { get; set; }

	string ValueDataColumnClose { get; set; }

	string ValueDataColumnHigh { get; set; }

	string ValueDataColumnLow { get; set; }
}
