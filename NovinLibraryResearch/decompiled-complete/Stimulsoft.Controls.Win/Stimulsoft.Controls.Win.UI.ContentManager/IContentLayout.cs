using System.Drawing;

namespace Stimulsoft.Controls.Win.UI.ContentManager;

public interface IContentLayout
{
	Rectangle Layout(Rectangle containerBounds, IBlock[] contentBlocks, BlockLayoutManager blockLayout);
}
