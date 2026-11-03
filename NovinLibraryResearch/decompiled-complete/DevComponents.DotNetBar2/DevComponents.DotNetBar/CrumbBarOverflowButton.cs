using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class CrumbBarOverflowButton : ButtonItem
{
	protected override void RenderButton(ItemPaintArgs p)
	{
		if (!p.IsOnMenu)
		{
			BaseRenderer ໝ = p.ໝ;
			if (ໝ != null)
			{
				p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
				p.ButtonItemRendererEventArgs.ButtonItem = this;
				p.ButtonItemRendererEventArgs.က = p;
				ໝ.DrawCrumbBarOverflowItem(p.ButtonItemRendererEventArgs);
				return;
			}
		}
		base.RenderButton(p);
	}

	public override void RecalcSize()
	{
		m_Rect.Width = 16;
		m_Rect.Height = 11;
		m_NeedRecalcSize = false;
	}

	public override void InternalMouseEnter()
	{
		if (Parent is _21E3 { Expanded: not false } _21E32)
		{
			_21E32.Expanded = false;
			Expanded = true;
		}
		base.InternalMouseEnter();
	}

	public CrumbBarOverflowButton()
		: this("", "")
	{
	}

	public CrumbBarOverflowButton(string sItemName)
		: this(sItemName, "")
	{
	}

	public CrumbBarOverflowButton(string itemName, string itemText)
		: base(itemName, itemText)
	{
		AutoExpandOnClick = true;
		ShowSubItems = false;
	}

	public override BaseItem Copy()
	{
		CrumbBarOverflowButton crumbBarOverflowButton = new CrumbBarOverflowButton();
		CopyToItem(crumbBarOverflowButton);
		return crumbBarOverflowButton;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		CrumbBarOverflowButton copy2 = copy as CrumbBarOverflowButton;
		base.CopyToItem((BaseItem)copy2);
	}
}
