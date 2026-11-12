using System.Collections;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class SuperTabItemDesigner : BaseItemDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[0];
			return new DesignerVerbCollection(value);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			SuperTabItem tabItem = GetTabItem();
			if (tabItem != null && tabItem.AttachedControl != null)
			{
				arrayList.Add(tabItem.AttachedControl);
			}
			return arrayList;
		}
	}

	protected virtual SuperTabItem GetTabItem()
	{
		return ((ComponentDesigner)this).Component as SuperTabItem;
	}

	protected override void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
	{
	}
}
