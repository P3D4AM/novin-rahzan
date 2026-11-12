using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

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
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			renderer.DrawQatOverflowItem(new QatOverflowItemRendererEventArgs(this, p.Graphics));
			return;
		}
		QatOverflowPainter qatOverflowPainter = PainterFactory.CreateQatOverflowItemPainter(this);
		if (qatOverflowPainter != null)
		{
			qatOverflowPainter.Paint(new QatOverflowItemRendererEventArgs(this, p.Graphics));
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
