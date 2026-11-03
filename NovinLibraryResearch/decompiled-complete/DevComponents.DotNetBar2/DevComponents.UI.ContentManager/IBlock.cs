using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.UI.ContentManager;

public interface IBlock
{
	Rectangle Bounds { get; set; }

	bool Visible { get; set; }

	Padding Margin { get; set; }
}
