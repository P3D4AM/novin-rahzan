using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሥ : _0CBF
{
	public override bool IsSizeValid
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public override bool IsNewLineAfterElement => true;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = new Rectangle(0, 0, 0, d.CurrentFont.Height);
	}

	public override void Render(MarkupDrawContext d)
	{
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
