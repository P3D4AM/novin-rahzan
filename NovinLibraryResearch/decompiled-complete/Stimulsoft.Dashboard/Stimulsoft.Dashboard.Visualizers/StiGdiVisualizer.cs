using System.Windows.Forms;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Visualizers;

public abstract class StiGdiVisualizer : StiVisualizer
{
	public abstract Control Render(IStiElement element);
}
