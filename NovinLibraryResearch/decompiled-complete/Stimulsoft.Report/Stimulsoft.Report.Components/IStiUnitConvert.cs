using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public interface IStiUnitConvert
{
	void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false);
}
