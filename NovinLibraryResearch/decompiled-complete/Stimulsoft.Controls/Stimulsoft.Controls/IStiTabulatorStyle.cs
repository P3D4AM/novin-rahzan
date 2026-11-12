using System.Drawing;

namespace Stimulsoft.Controls;

public interface IStiTabulatorStyle
{
	void DrawPage(Graphics g, StiTabTitlePosition position, StiTabulatorPage page);

	void DrawPageTitle(Graphics g, StiTabTitlePosition position, StiTabulatorPage page);
}
