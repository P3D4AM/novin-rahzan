using System.Collections;
using System.Drawing;

namespace DevComponents.UI.ContentManager;

public abstract class BlockLayoutManager
{
	private Graphics ܟ;

	public Graphics Graphics
	{
		get
		{
			return ܟ;
		}
		set
		{
			ܟ = value;
		}
	}

	public abstract void Layout(IBlock block, Size availableSize);

	public abstract Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines);
}
