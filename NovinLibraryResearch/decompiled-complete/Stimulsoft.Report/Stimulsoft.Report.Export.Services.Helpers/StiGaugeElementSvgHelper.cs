using System.Xml;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export.Services.Helpers;

public class StiGaugeElementSvgHelper
{
	public static void WriteGauge(XmlTextWriter writer, StiSvgData svgData, float zoom, bool isStyleSample = false)
	{
		StiDashboardHelperCreator.CreateGaugeVisualSvgHelper()?.WriteGauge(writer, svgData, needAnimation: false, isStyleSample);
	}
}
