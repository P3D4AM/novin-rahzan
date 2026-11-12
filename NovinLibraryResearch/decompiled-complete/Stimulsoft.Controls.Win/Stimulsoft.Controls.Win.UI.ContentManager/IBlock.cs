using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.UI.ContentManager;

public interface IBlock
{
	Rectangle Bounds { get; set; }

	bool Visible { get; set; }

	Padding Margin { get; set; }
}
