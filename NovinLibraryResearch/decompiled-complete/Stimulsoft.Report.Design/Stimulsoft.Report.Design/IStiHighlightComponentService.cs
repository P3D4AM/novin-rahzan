using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public interface IStiHighlightComponentService
{
	bool IsOpen { get; }

	void Activate(StiText highlightedComp, StiText dragingComponent, StiComponent labelComp);

	void ClosePopup();
}
