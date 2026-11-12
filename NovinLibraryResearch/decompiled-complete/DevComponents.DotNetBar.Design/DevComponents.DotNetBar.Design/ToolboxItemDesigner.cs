using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class ToolboxItemDesigner : ComponentDesignerWithAction
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
				_ActionLists.Add((DesignerActionList)(object)new ToolboxItemActionList(this));
			}
			return _ActionLists;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ToolboxItem toolboxItem = ((ComponentDesigner)this).Component as ToolboxItem;
			if (toolboxItem.SubItems.Count == 0)
			{
				return ((ComponentDesigner)this).AssociatedComponents;
			}
			ArrayList arrayList = new ArrayList();
			toolboxItem.SubItems.CopyTo(arrayList);
			return arrayList;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates visiblity of the item.")]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Visible"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Visible"] = value;
		}
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ComponentDesigner)this).InitializeNewComponent(defaultValues);
		ToolboxItem toolboxItem = (ToolboxItem)((ComponentDesigner)this).Component;
		toolboxItem.Text = toolboxItem.Name;
		toolboxItem.Symbol = "\ue251";
		toolboxItem.SymbolSet = eSymbolSet.Material;
	}

	public void MoveItemUp()
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		ToolboxItem toolboxItem = (ToolboxItem)((ComponentDesigner)this).Component;
		BaseItem parent = toolboxItem.Parent;
		if (parent == null)
		{
			return;
		}
		int num = parent.SubItems.IndexOf(toolboxItem);
		if (num == 0)
		{
			if (parent.Parent != null && parent.Parent.SubItems.IndexOf(parent) > 0)
			{
				int num2 = parent.Parent.SubItems.IndexOf(parent);
				for (int num3 = num2; num3 >= 0; num3--)
				{
					BaseItem baseItem = parent.Parent.SubItems[num3];
					if (baseItem is ToolboxGroup && baseItem.Visible)
					{
						componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
						parent.SubItems.Remove(toolboxItem);
						componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
						componentChangeService.OnComponentChanging(parent.Parent, TypeDescriptor.GetProperties(parent.Parent)["SubItems"]);
						parent.Parent.SubItems.Add(toolboxItem);
						componentChangeService.OnComponentChanged(parent.Parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
						break;
					}
				}
			}
		}
		else
		{
			componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
			parent.SubItems.Remove(toolboxItem);
			parent.SubItems.Insert(num - 1, toolboxItem);
			componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
		}
		RecalcLayout();
	}

	public override void RecalcLayout()
	{
		if (((ComponentDesigner)this).Component is ToolboxItem toolboxItem && toolboxItem.GetToolboxControl() != null)
		{
			toolboxItem.GetToolboxControl().RecalcLayout();
		}
	}

	public void MoveItemDown()
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		ToolboxItem toolboxItem = (ToolboxItem)((ComponentDesigner)this).Component;
		BaseItem parent = toolboxItem.Parent;
		if (parent == null)
		{
			return;
		}
		int num = parent.SubItems.IndexOf(toolboxItem);
		if (num == parent.SubItems.Count - 1)
		{
			if (parent.Parent != null && parent.Parent.SubItems.IndexOf(parent) < parent.Parent.SubItems.Count - 1)
			{
				int num2 = parent.Parent.SubItems.IndexOf(parent);
				for (int i = num2; i < parent.Parent.SubItems.Count; i++)
				{
					BaseItem baseItem = parent.Parent.SubItems[i];
					if (baseItem is ToolboxGroup && baseItem.Visible)
					{
						componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
						parent.SubItems.Remove(toolboxItem);
						componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
						componentChangeService.OnComponentChanging(parent.Parent, TypeDescriptor.GetProperties(parent.Parent)["SubItems"]);
						parent.Parent.SubItems.Add(toolboxItem);
						componentChangeService.OnComponentChanged(parent.Parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
						break;
					}
				}
			}
		}
		else
		{
			componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
			parent.SubItems.Remove(toolboxItem);
			parent.SubItems.Insert(num + 1, toolboxItem);
			componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
		}
		RecalcLayout();
	}

	protected override void OnDesignerSelectionChanged(bool isSelected)
	{
		ToolboxItem toolboxItem = (ToolboxItem)((ComponentDesigner)this).Component;
		toolboxItem.Checked = isSelected;
		base.OnDesignerSelectionChanged(isSelected);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ComponentDesigner)this).PreFilterProperties(properties);
		properties["Visible"] = TypeDescriptor.CreateProperty(typeof(ToolboxItemDesigner), (PropertyDescriptor)properties["Visible"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Appearance"));
	}

	public override Control GetHostControl()
	{
		return GetParentControl().Parent;
	}
}
