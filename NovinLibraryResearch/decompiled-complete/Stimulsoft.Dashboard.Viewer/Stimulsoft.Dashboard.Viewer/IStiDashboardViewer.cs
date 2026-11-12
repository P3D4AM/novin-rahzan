using System.Windows.Forms;
using Stimulsoft.Dashboard.Components;

namespace Stimulsoft.Dashboard.Viewer;

internal interface IStiDashboardViewer
{
	StiDashboard Dashboard { get; set; }

	Panel GetElementsPanel();
}
