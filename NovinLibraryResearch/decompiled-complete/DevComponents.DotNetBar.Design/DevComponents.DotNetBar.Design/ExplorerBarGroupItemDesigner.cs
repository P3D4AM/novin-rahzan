using System.Collections;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class ExplorerBarGroupItemDesigner : BaseItemDesigner
{
	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			if (((ComponentDesigner)this).Component is BaseItem baseItem)
			{
				foreach (BaseItem subItem in baseItem.SubItems)
				{
					arrayList.Add(subItem);
				}
			}
			return arrayList;
		}
	}

	protected override void BeforeNewItemAdded(BaseItem item)
	{
		base.BeforeNewItemAdded(item);
		if (item is ButtonItem)
		{
			ExplorerBarGroupItem.SetDesignTimeDefaults((ButtonItem)item, ((ExplorerBarGroupItem)((ComponentDesigner)this).Component).StockStyle);
		}
	}

	protected override void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component is ExplorerBarGroupItem)
		{
			ExplorerBarGroupItem explorerBarGroupItem = ((ComponentDesigner)this).Component as ExplorerBarGroupItem;
			explorerBarGroupItem.SetDefaultAppearance();
			explorerBarGroupItem.StockStyle = eExplorerBarStockStyle.SystemColors;
		}
	}
}
