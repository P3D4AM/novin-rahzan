using System.Collections;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Design;

public interface IStiDesignerBase
{
	IStiDesignerBase DesignerControl { get; }

	StiDesignerInfo Info { get; }

	bool IsModalMode { get; set; }

	StiNodesMemory NodesMemory { get; }

	bool UseAliases { get; set; }

	StiReport Report { get; set; }

	Hashtable TextAliasesHash { get; }

	StiComponentsCollection SelectedComponentsOnPage { get; }

	StiComponentsCollection SelectedTextComponentsOnPage { get; }

	StiComponentsCollection ComponentsOnReport { get; }

	StiComponentsCollection ComponentsOnPage { get; }

	StiQuickButton[] GetQuickButtons(StiComponent component);

	double XToPage(int x);

	double YToPage(int y);

	void ShowReportCheckerError(StiReportCheckerErrorParameters parameters);

	void UndoRedoSave(string name);

	void UndoRedoSave(string name, bool onlyCurrentPage);

	void AddOwnedForm(Form form);

	void Refresh();

	void Invalidate(StiComponent component);

	StiComponentsCollection GetSelected();
}
