using System.Xml;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export.Services.Helpers;

public class StiCardsElementSvgHelper
{
	public static void WriteCards(XmlTextWriter writer, StiSvgData svgData, float zoom, bool isStyleSample = false)
	{
		StiDashboardHelperCreator.CreateCardsVisualSvgHelper()?.WriteCards(writer, svgData, isStyleSample);
	}
}
