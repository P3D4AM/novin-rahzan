using System.Xml;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export.Services.Helpers;

public class StiIndicatorElementSvgHelper
{
	public static void WriteIndicator(XmlTextWriter writer, StiSvgData svgData, float zoom, bool isStyleSample = false)
	{
		StiDashboardHelperCreator.CreateIndicatorVisualSvgHelper()?.WriteIndicator(writer, svgData, isStyleSample);
	}
}
