using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Base.Viewer;

public interface IStiViewerAccessor
{
	bool BlockFormClose { get; set; }

	void CheckParametersButton();

	void UncheckParametersButton();

	void EnableParametersButton();

	IStiReport GetReport();

	IStiReport GetCurrentReport();

	Panel GetPanelDialogs();

	IContainer GetIContainer();

	void OnShowParameters();

	bool OnSubmit();

	bool OnReset();
}
