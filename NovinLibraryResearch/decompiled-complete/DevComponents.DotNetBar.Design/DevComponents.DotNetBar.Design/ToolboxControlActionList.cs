using System.ComponentModel;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

internal class ToolboxControlActionList : DesignerActionList
{
	private ToolboxControlDesigner _Designer;

	public bool Expanded
	{
		get
		{
			return _Designer.Expanded;
		}
		set
		{
			_Designer.Expanded = value;
		}
	}

	public bool ItemDragDropEnabled
	{
		get
		{
			return ToolboxControl.ItemDragDropEnabled;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["ItemDragDropEnabled"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool SearchBoxVisible
	{
		get
		{
			return ToolboxControl.SearchBoxVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["SearchBoxVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool TitleVisible
	{
		get
		{
			return ToolboxControl.TitleVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["TitleVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool MenuVisible
	{
		get
		{
			return ToolboxControl.MenuVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["MenuVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public ToolboxControl ToolboxControl => (ToolboxControl)((DesignerActionList)this).Component;

	public ToolboxControlActionList(ToolboxControlDesigner designer)
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Items"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Position"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateGroup", "Create Group", "Items", true));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("Expanded", "Toolbox Expanded?", "Behavior", "Indicates whether toolbox control is expanded and displays items at their full size."));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("ItemDragDropEnabled", "Item Drag and Drop?", "Behavior", "Indicates whether item drag and drop is enabled."));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("SearchBoxVisible", "Search Visible?", "Appearance", "Indicates whether search text-box is visible."));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("TitleVisible", "Title Visible?", "Appearance", "Indicates whether title bar is visible."));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("MenuVisible", "Menu Visible?", "Appearance", "Indicates whether menu bar is visible."));
		return val;
	}

	public void CreateGroup()
	{
		_Designer.CreateGroup();
	}
}
