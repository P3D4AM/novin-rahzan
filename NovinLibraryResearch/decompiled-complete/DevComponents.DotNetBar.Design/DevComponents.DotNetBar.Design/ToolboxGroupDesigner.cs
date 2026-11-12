using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class ToolboxGroupDesigner : ComponentDesignerWithAction
{
	private DesignerActionListCollection _ActionLists;

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_ActionLists == null)
			{
				_ActionLists = new DesignerActionListCollection();
				_ActionLists.Add((DesignerActionList)(object)new ToolboxGroupActionList(this));
			}
			return _ActionLists;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ToolboxGroup toolboxGroup = ((ComponentDesigner)this).Component as ToolboxGroup;
			if (toolboxGroup.SubItems.Count == 0)
			{
				return ((ComponentDesigner)this).AssociatedComponents;
			}
			ArrayList arrayList = new ArrayList();
			toolboxGroup.SubItems.CopyTo(arrayList);
			return arrayList;
		}
	}

	protected override void OnDesignerSelectionChanged(bool isSelected)
	{
		ToolboxGroup toolboxGroup = (ToolboxGroup)((ComponentDesigner)this).Component;
		if (isSelected && !toolboxGroup.Expanded)
		{
			toolboxGroup.Expanded = true;
		}
		base.OnDesignerSelectionChanged(isSelected);
	}

	public override void RecalcLayout()
	{
		if (((ComponentDesigner)this).Component is ToolboxGroup toolboxGroup && toolboxGroup.GetToolboxControl() != null)
		{
			toolboxGroup.GetToolboxControl().RecalcLayout();
		}
	}

	public void MoveItemDown()
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		ToolboxGroup toolboxGroup = (ToolboxGroup)((ComponentDesigner)this).Component;
		BaseItem parent = toolboxGroup.Parent;
		if (parent != null)
		{
			int num = parent.SubItems.IndexOf(toolboxGroup);
			if (num != parent.SubItems.Count - 1)
			{
				componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
				parent.SubItems.Remove(toolboxGroup);
				parent.SubItems.Insert(num + 1, toolboxGroup);
				componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
				RecalcLayout();
			}
		}
	}

	public void MoveItemUp()
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		ToolboxGroup toolboxGroup = (ToolboxGroup)((ComponentDesigner)this).Component;
		BaseItem parent = toolboxGroup.Parent;
		if (parent != null)
		{
			int num = parent.SubItems.IndexOf(toolboxGroup);
			if (num != 0)
			{
				componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
				parent.SubItems.Remove(toolboxGroup);
				parent.SubItems.Insert(num - 1, toolboxGroup);
				componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
				RecalcLayout();
			}
		}
	}

	public void CreateItem()
	{
		ToolboxGroup toolboxGroup = (ToolboxGroup)((ComponentDesigner)this).Component;
		toolboxGroup.GetToolboxControl();
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			ToolboxItem toolboxItem = null;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(toolboxGroup).Find("SubItems", ignoreCase: true));
			if (designerHost.CreateComponent(typeof(ToolboxItem)) is ToolboxItem toolboxItem2)
			{
				toolboxItem2.Text = toolboxItem2.Name;
				toolboxItem2.Symbol = "\ue251";
				toolboxItem2.SymbolSet = eSymbolSet.Material;
				toolboxGroup.SubItems.Add(toolboxItem2);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(toolboxGroup).Find("Nodes", ignoreCase: true), null, null);
			}
			RecalcLayout();
		}
	}

	public override Control GetHostControl()
	{
		return GetParentControl().Parent;
	}
}
