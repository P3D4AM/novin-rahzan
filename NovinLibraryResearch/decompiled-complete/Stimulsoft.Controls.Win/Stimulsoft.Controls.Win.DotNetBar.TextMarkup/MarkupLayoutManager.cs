using System.Collections;
using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class MarkupLayoutManager : BlockLayoutManager
{
	private MarkupDrawContext m_MarkupDrawContext;

	public MarkupDrawContext MarkupDrawContext
	{
		get
		{
			return m_MarkupDrawContext;
		}
		set
		{
			m_MarkupDrawContext = value;
		}
	}

	public override void Layout(IBlock block, Size availableSize)
	{
		if (block is MarkupElement)
		{
			MarkupElement markupElement = block as MarkupElement;
			if (!markupElement.IsSizeValid)
			{
				markupElement.Measure(availableSize, m_MarkupDrawContext);
			}
		}
	}

	public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
	{
		return blocksBounds;
	}
}
