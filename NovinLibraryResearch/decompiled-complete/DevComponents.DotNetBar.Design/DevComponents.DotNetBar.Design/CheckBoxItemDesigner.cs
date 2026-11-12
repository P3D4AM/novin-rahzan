using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class CheckBoxItemDesigner : BaseItemDesigner
{
	public override DesignerVerbCollection Verbs => new DesignerVerbCollection();

	protected override void SetDesignTimeDefaults()
	{
		if (((ComponentDesigner)this).Component is CheckBoxItem checkBoxItem)
		{
			checkBoxItem.AutoCollapseOnClick = false;
		}
	}
}
