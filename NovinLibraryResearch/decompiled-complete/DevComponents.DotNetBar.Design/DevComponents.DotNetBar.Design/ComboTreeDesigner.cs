using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.AdvTree;
using DevComponents.AdvTree.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ComboTreeDesigner : ControlDesigner
{
	private DesignerActionListCollection _ActionLists;

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			ComboTree comboTree = ((ComponentDesigner)this).Component as ComboTree;
			foreach (Node node in comboTree.Nodes)
			{
				arrayList.Add(node);
				AddChildNodes(node, arrayList);
			}
			return arrayList;
		}
	}

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_ActionLists == null)
			{
				_ActionLists = new DesignerActionListCollection();
				_ActionLists.Add((DesignerActionList)(object)new ComboTreeActionList(this));
			}
			return _ActionLists;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site == null || component.Site.DesignMode)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoved;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved -= OnComponentRemoved;
		}
		((ControlDesigner)this).Dispose(disposing);
	}

	private void SetDesignTimeDefaults()
	{
		ComboTree comboTree = ((ControlDesigner)this).Control as ComboTree;
		PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(comboTree)["Text"];
		if (propertyDescriptor != null && (object)propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
		{
			propertyDescriptor.SetValue(comboTree, "");
		}
		comboTree.ButtonDropDown.Visible = true;
		comboTree.BackgroundStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
		comboTree.Style = eDotNetBarStyle.StyleManagerControlled;
	}

	private void AddChildNodes(Node parent, ArrayList al)
	{
		foreach (Node node in parent.Nodes)
		{
			al.Add(node);
		}
	}

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component != ((ComponentDesigner)this).Component)
		{
			return;
		}
		ICollection associatedComponents = ((ComponentDesigner)this).AssociatedComponents;
		if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			return;
		}
		foreach (object item in associatedComponents)
		{
			if (item is Node)
			{
				Node component = item as Node;
				designerHost.DestroyComponent(component);
			}
		}
	}

	internal void EditColumns()
	{
		ComboTree objectToChange = ((ComponentDesigner)this).Component as ComboTree;
		AdvTreeDesigner.EditValue((ComponentDesigner)(object)this, objectToChange, "Columns");
	}

	internal void CreateNode()
	{
		CreateNode(((ComponentDesigner)this).Component as ComboTree);
	}

	private Node CreateNode(ComboTree tree)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		Node node = null;
		tree.AdvTree.BeginUpdate();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(tree).Find("Nodes", ignoreCase: true));
			node = designerHost.CreateComponent(typeof(Node)) as Node;
			if (node != null)
			{
				node.Text = node.Name;
				node.Expanded = true;
				tree.Nodes.Add(node);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(tree).Find("Nodes", ignoreCase: true), null, null);
			}
		}
		finally
		{
			tree.AdvTree.EndUpdate();
		}
		return node;
	}
}
