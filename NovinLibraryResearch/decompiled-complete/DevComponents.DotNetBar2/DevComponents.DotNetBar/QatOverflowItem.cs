using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class QatOverflowItem : DisplayMoreItem
{
	public QatOverflowItem()
	{
		KeyTips = "00";
	}

	public override BaseItem Copy()
	{
		QatOverflowItem qatOverflowItem = new QatOverflowItem();
		CopyToItem(qatOverflowItem);
		return qatOverflowItem;
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			ໝ.DrawQatOverflowItem(new QatOverflowItemRendererEventArgs(this, p.Graphics));
			return;
		}
		_20A8 _20A8 = ᣓ.ᤛ(this);
		if (_20A8 != null)
		{
			_20A8.Paint(new QatOverflowItemRendererEventArgs(this, p.Graphics));
		}
		else
		{
			base.Paint(p);
		}
	}

	protected override int GetReInsertIndex()
	{
		int num = m_Parent.SubItems.Count;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			if (m_Parent.SubItems[num2] is CustomizeItem || m_Parent.SubItems[num2].ItemAlignment == eItemAlignment.Far)
			{
				num = num2;
			}
			else if (m_Parent.SubItems[num2].ItemAlignment == eItemAlignment.Near)
			{
				break;
			}
		}
		return num;
	}
}
