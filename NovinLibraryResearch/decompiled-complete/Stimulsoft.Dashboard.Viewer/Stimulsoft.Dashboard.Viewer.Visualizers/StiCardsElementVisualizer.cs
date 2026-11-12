using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Cards;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiCardsElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiCardsElementControl(element);
	}
}
