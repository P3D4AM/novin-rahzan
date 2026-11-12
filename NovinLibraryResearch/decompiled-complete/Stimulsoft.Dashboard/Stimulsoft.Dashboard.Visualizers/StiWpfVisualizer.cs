using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Visualizers;

public abstract class StiWpfVisualizer : StiVisualizer
{
	public abstract object Render(object viewer, IStiElement element);
}
