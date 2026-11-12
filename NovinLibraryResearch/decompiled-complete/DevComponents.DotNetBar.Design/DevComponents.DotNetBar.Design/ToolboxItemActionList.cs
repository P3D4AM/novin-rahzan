using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

internal class ToolboxItemActionList : DesignerActionList
{
	private ToolboxItemDesigner _Designer;

	public ToolboxItemActionList(ToolboxItemDesigner designer)
		: base(((ComponentDesigner)designer).Component)
	{
		_Designer = designer;
	}

	public override DesignerActionItemCollection GetSortedActionItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Position"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveItemUp", "Move Up", "Position", true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveItemDown", "Move Down", "Position", true));
		return val;
	}

	public void MoveItemUp()
	{
		_Designer.MoveItemUp();
	}

	public void MoveItemDown()
	{
		_Designer.MoveItemDown();
	}
}
