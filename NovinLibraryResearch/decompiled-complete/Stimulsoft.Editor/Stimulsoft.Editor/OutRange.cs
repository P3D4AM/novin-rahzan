using System.Drawing;

namespace Stimulsoft.Editor;

internal class OutRange : OutlineRange
{
	private OutlineList owner;

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (base.Visible != value)
			{
				base.Visible = value;
				if (owner != null)
				{
					owner.UpdateRange(this, update: true);
				}
			}
		}
	}

	public override string DisplayText
	{
		get
		{
			if (owner == null || (owner.Owner.OutlineOptions & OutlineOptions.DrawButtons) != 0)
			{
				return base.Text;
			}
			return string.Empty;
		}
	}

	public OutRange(OutlineList owner, Point startPt, Point endPt, int level, string text)
		: base(startPt, endPt, level, text)
	{
		this.owner = owner;
	}

	public OutRange(OutlineList owner, Point startPt, Point endPt, int level, string text, bool visible)
		: base(startPt, endPt, level, text, visible)
	{
		this.owner = owner;
	}
}
