using System.Collections;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class BubbleBarTabDesigner : ComponentDesigner
{
	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			if (!(((ComponentDesigner)this).Component is BubbleBarTab bubbleBarTab))
			{
				return ((ComponentDesigner)this).AssociatedComponents;
			}
			bubbleBarTab.Buttons.CopyTo(arrayList);
			return arrayList;
		}
	}
}
