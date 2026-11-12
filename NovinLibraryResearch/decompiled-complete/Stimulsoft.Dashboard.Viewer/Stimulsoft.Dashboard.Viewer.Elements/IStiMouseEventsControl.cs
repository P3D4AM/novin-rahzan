using System.Windows.Forms;

namespace Stimulsoft.Dashboard.Viewer.Elements;

public interface IStiMouseEventsControl
{
	void InvokeMouseEnter();

	void InvokeMouseLeave();

	void InvokeMouseMove(MouseEventArgs e);
}
