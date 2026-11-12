using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class RibbonTabItemContainer : GenericItemContainer
{
	protected override void OnStyleChanged()
	{
		switch (EffectiveStyle)
		{
		case eDotNetBarStyle.Windows7:
			base.PaddingLeft = 0;
			base.PaddingRight = 0;
			base.PaddingTop = 0;
			break;
		case eDotNetBarStyle.Office2010:
		case eDotNetBarStyle.Metro:
			base.PaddingLeft = 0;
			base.PaddingRight = 1;
			base.PaddingTop = 1;
			break;
		default:
			base.PaddingLeft = 1;
			base.PaddingRight = 1;
			base.PaddingTop = 1;
			break;
		}
		base.OnStyleChanged();
	}

	protected override bool OnBeforeLayout()
	{
		if (Orientation != 0)
		{
			return true;
		}
		ArrayList arrayList = new ArrayList();
		int num = 0;
		int num2 = 0;
		int num3 = 24;
		int num4 = WidthInternal - (base.PaddingLeft + base.PaddingRight);
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.RecalcSize();
				num += subItem.WidthInternal + base.ItemSpacing;
				if (subItem is RibbonTabItem)
				{
					((RibbonTabItem)subItem).ReducedSize = false;
					arrayList.Add(subItem);
					num2 += subItem.WidthInternal + base.ItemSpacing;
				}
			}
		}
		int num5 = num - num4;
		if (num > num4 && num2 > 0)
		{
			if (num5 >= num2 - (num3 * arrayList.Count + arrayList.Count - 1))
			{
				foreach (RibbonTabItem item in arrayList)
				{
					item.WidthInternal = num3;
					item.ReducedSize = true;
				}
			}
			else
			{
				float num6 = 1f - (float)num5 / (float)num2;
				bool reducedSize = false;
				if ((double)num6 <= 0.75)
				{
					reducedSize = true;
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					RibbonTabItem ribbonTabItem2 = arrayList[i] as RibbonTabItem;
					ribbonTabItem2.ReducedSize = reducedSize;
					if (i == arrayList.Count - 1)
					{
						ribbonTabItem2.WidthInternal -= num5;
						continue;
					}
					int num7 = (int)((float)ribbonTabItem2.WidthInternal * num6);
					if (num7 < num3)
					{
						num7 = num3;
					}
					num5 -= ribbonTabItem2.WidthInternal - num7;
					ribbonTabItem2.WidthInternal = num7;
				}
			}
		}
		return false;
	}
}
