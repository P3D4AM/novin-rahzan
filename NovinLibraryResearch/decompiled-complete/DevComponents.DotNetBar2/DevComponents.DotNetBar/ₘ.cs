using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

internal class ₘ : ῲ
{
	private const int ₙ = 5;

	private const int ₚ = 10;

	private const int ₛ = 7;

	private const int ₜ = 10;

	internal override Size Ό
	{
		get
		{
			if (base._1FD4.ᓙ && base._1FD4.HorizontalText)
			{
				return new Size(28, 30);
			}
			return new Size(28, 16);
		}
	}

	internal override int _136F
	{
		get
		{
			if (!base._1FD4.ᓙ)
			{
				return 10;
			}
			return 10;
		}
	}

	internal override int _1FFF
	{
		get
		{
			switch (base._1FD4.TabAlignment)
			{
			case eTabStripAlignment.Top:
			case eTabStripAlignment.Bottom:
				return 5;
			case eTabStripAlignment.Right:
				if (base._1FD4.HorizontalText)
				{
					return 7;
				}
				return 5;
			default:
				return 7;
			}
		}
	}

	public ₘ(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}

	protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
		int num = base.ᇃ.IndexOf((BaseItem)e.Block);
		if (num >= 0 && num + 1 < base.ᇃ.Count && base.ᇃ[num] is SuperTabItem && base.ᇃ[num + 1] is SuperTabItem)
		{
			e.NextPosition = e.CurrentPosition;
			if (base._1FD4.ᓙ)
			{
				e.NextPosition.Y += e.Block.Bounds.Height - 10;
			}
			else
			{
				e.NextPosition.X += e.Block.Bounds.Width - 10;
			}
			e.Cancel = true;
		}
	}

	internal override Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle result = base._2013(_06EC, _2014);
		if (_06EC is SuperTabItem && _2014 is SuperTabItem)
		{
			if (base._1FD4.ᓙ)
			{
				result.Y -= 10;
			}
			else
			{
				result.X -= 10;
			}
		}
		return result;
	}
}
