using System.Collections;
using System.Drawing;

namespace Stimulsoft.Controls.Win.UI.ContentManager;

public abstract class BlockLayoutManager
{
	private Graphics m_Graphics;

	public Graphics Graphics
	{
		get
		{
			return m_Graphics;
		}
		set
		{
			m_Graphics = value;
		}
	}

	public abstract void Layout(IBlock block, Size availableSize);

	public abstract Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines);
}
