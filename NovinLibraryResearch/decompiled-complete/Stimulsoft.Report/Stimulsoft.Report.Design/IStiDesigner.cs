using System.Windows.Forms;

namespace Stimulsoft.Report.Design;

public interface IStiDesigner : IStiDesignerBase
{
	bool ShowInTaskbar { get; set; }

	Form MdiParent { get; set; }

	bool TopLevel { get; set; }

	void ShowDesigner();

	void ShowDesigner(object win32Window);

	DialogResult ShowDialogDesigner();

	DialogResult ShowDialogDesigner(object win32Window);
}
