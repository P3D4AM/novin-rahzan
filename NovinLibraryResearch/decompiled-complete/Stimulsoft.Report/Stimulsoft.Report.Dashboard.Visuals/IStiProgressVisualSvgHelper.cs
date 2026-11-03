using System.Xml;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard.Visuals;

public interface IStiProgressVisualSvgHelper
{
	void WriteProgress(XmlTextWriter writer, StiSvgData svgData, bool isStyleSample = false);

	void WriteProgress(XmlTextWriter writer, StiSvgData svgData, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false);
}
