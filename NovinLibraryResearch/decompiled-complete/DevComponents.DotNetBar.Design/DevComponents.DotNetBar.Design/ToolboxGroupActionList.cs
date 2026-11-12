using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

internal class ToolboxGroupActionList : DesignerActionList
{
	private ToolboxGroupDesigner _Designer;

	public ToolboxGroupActionList(ToolboxGroupDesigner designer)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Items"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Position"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateItem", "New Toolbox Item", "Items", true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveItemUp", "Move Up", "Position", true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveItemDown", "Move Down", "Position", true));
		return val;
	}

	public void CreateItem()
	{
		_Designer.CreateItem();
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
