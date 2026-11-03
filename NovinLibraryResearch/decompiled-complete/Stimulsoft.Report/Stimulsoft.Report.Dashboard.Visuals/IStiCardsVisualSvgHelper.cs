using System.Xml;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard.Visuals;

public interface IStiCardsVisualSvgHelper
{
	void WriteCards(XmlTextWriter writer, StiSvgData svgData, bool isStyleSample = false);

	void WriteCards(XmlTextWriter writer, StiSvgData svgData, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false);
}
