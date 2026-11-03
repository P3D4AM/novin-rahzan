using System.Xml;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export.Services.Helpers;

public class StiProgressElementSvgHelper
{
	public static void WriteProgress(XmlTextWriter writer, StiSvgData svgData, float zoom, bool isStyleSample = false)
	{
		StiDashboardHelperCreator.CreateProgressVisualSvgHelper()?.WriteProgress(writer, svgData, isStyleSample);
	}
}
