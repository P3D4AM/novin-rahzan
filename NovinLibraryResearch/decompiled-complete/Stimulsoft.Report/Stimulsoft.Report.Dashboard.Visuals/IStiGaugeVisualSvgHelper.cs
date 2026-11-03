using System.Xml;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard.Visuals;

public interface IStiGaugeVisualSvgHelper
{
	void WriteGauge(XmlTextWriter writer, StiSvgData svgData, bool needAnimation, bool isStyleSample = false);

	void WriteGauge(XmlTextWriter writer, StiSvgData svgData, bool needAnimation, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false);
}
