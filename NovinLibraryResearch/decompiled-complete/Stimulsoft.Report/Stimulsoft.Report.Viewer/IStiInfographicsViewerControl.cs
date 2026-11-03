using Stimulsoft.Base.Context;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Viewer;

public interface IStiInfographicsViewerControl
{
	void ShowToolTip(string str);

	void ShowHyperlink(string str);

	void RemoveToolTip();

	void RemoveHyperlink();

	bool InvokeHyperlinkProcessing(object sender, string hyperlink);

	bool ProcessHyperlinkString(string hyperlink);

	void ProcessDrillDown(StiComponent comp, StiInteraction interaction, StiSeriesInteractionData seriesInteraction);
}
