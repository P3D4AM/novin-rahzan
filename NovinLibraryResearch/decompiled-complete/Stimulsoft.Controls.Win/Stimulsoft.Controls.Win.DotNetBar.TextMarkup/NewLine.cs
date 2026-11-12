using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class NewLine : MarkupElement
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
