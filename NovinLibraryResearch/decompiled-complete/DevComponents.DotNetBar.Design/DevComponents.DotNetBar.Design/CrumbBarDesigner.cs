using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class CrumbBarDesigner : ControlDesigner
{
	[Description("Indicates visual style of the control.")]
	[DefaultValue(eCrumbBarStyle.Vista)]
	[Category("Appearance")]
	public eCrumbBarStyle Style
	{
		get
		{
			return ((CrumbBar)(object)((ControlDesigner)this).Control).Style;
		}
		set
		{
			CrumbBar crumbBar = ((ControlDesigner)this).Control as CrumbBar;
			bool flag = crumbBar.Style != value;
			crumbBar.Style = value;
			if (flag && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false })
			{
				switch (value)
				{
				case eCrumbBarStyle.Vista:
					SetVistaBackgroundStyle();
					break;
				case eCrumbBarStyle.Office2007:
					SetOffice2007BackgroundStyle();
					break;
				}
			}
		}
	}

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[1]
			{
				new DesignerVerb("Edit Items...", EditItems)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			if (((ControlDesigner)this).Control is CrumbBar crumbBar)
			{
				foreach (BaseItem item in crumbBar.Items)
				{
					GetItemsRecursive(item, arrayList);
				}
			}
			return arrayList;
		}
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoving;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		((ControlDesigner)this).Dispose(disposing);
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		SetVistaBackgroundStyle();
		CrumbBar crumbBar = ((ControlDesigner)this).Control as CrumbBar;
		((Control)crumbBar).AutoSize = true;
		if (IsMetroHosted())
		{
			crumbBar.Style = eCrumbBarStyle.Office2007;
		}
	}

	private bool IsMetroHosted()
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null && (designerHost.RootComponent is MetroAppForm || designerHost.RootComponent is MetroForm))
		{
			return true;
		}
		return false;
	}

	private void SetVistaBackgroundStyle()
	{
		CrumbBar crumbBar = ((ControlDesigner)this).Control as CrumbBar;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		componentChangeService?.OnComponentChanging(this, TypeDescriptor.GetProperties(crumbBar)["BackgroundStyle"]);
		crumbBar.BackgroundStyle.Reset();
		crumbBar.BackgroundStyle.Border = eStyleBorderType.Solid;
		crumbBar.BackgroundStyle.BorderWidth = 1;
		crumbBar.BackgroundStyle.BorderColor = ColorScheme.GetColor("53595E");
		crumbBar.BackgroundStyle.BorderColor2 = ColorScheme.GetColor("A9B4BF");
		crumbBar.BackgroundStyle.BackColor = ColorScheme.GetColor("F8FAFD");
		componentChangeService?.OnComponentChanged(this, TypeDescriptor.GetProperties(crumbBar)["BackgroundStyle"], null, null);
	}

	private void SetOffice2007BackgroundStyle()
	{
		CrumbBar crumbBar = ((ControlDesigner)this).Control as CrumbBar;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		componentChangeService?.OnComponentChanging(this, TypeDescriptor.GetProperties(crumbBar)["BackgroundStyle"]);
		crumbBar.BackgroundStyle.Reset();
		crumbBar.BackgroundStyle.Class = ElementStyleClassKeys.CrumbBarBackgroundKey;
		componentChangeService?.OnComponentChanged(this, TypeDescriptor.GetProperties(crumbBar)["BackgroundStyle"], null, null);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		properties["Style"] = TypeDescriptor.CreateProperty(typeof(CrumbBarDesigner), (PropertyDescriptor)properties["Style"], new DefaultValueAttribute(eCrumbBarStyle.Vista), new BrowsableAttribute(browsable: true), new CategoryAttribute("Appearance"));
	}

	private void EditItems(object sender, EventArgs e)
	{
		EditItems();
	}

	internal void EditItems()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		CrumbBar crumbBar = ((ComponentDesigner)this).Component as CrumbBar;
		Form val = new Form();
		((Control)val).Text = "CrumbBar control editor";
		val.FormBorderStyle = (FormBorderStyle)4;
		val.MinimizeBox = false;
		val.StartPosition = (FormStartPosition)1;
		CrumbBarItemsEditor crumbBarItemsEditor = new CrumbBarItemsEditor();
		((Control)crumbBarItemsEditor).Dock = (DockStyle)5;
		val.Size = new Size(800, 600);
		((Control)val).Controls.Add((Control)(object)crumbBarItemsEditor);
		crumbBarItemsEditor.CrumbBar = crumbBar;
		crumbBarItemsEditor.Designer = this;
		crumbBarItemsEditor.UpdateDisplay();
		val.ShowDialog();
		((Component)(object)val).Dispose();
	}

	private void CreateItem(object sender, EventArgs e)
	{
		CreateItem();
	}

	internal void CreateItem()
	{
		CrumbBarItem crumbBarItem = CreateItem(((ComponentDesigner)this).Component as CrumbBar);
		if (crumbBarItem == null)
		{
			return;
		}
		ISelectionService selectionService = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		ArrayList arrayList = new ArrayList(1);
		arrayList.Add(crumbBarItem);
		if (selectionService != null)
		{
			selectionService.SetSelectedComponents(arrayList, SelectionTypes.MouseDown);
			if (((ComponentDesigner)this).Component is CrumbBar { SelectedItem: null } crumbBar)
			{
				crumbBar.SelectedItem = crumbBarItem;
			}
		}
	}

	public object GetDesignService(Type serviceType)
	{
		return ((ComponentDesigner)this).GetService(serviceType);
	}

	private CrumbBarItem CreateItem(CrumbBar crumbBar)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		CrumbBarItem crumbBarItem = null;
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(crumbBar).Find("Items", ignoreCase: true));
			crumbBarItem = designerHost.CreateComponent(typeof(CrumbBarItem)) as CrumbBarItem;
			if (crumbBarItem != null)
			{
				crumbBarItem.Text = crumbBarItem.Name;
				crumbBar.Items.Add(crumbBarItem);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(crumbBar).Find("Items", ignoreCase: true), null, null);
			}
		}
		finally
		{
			crumbBar.RecalcLayout();
		}
		return crumbBarItem;
	}

	public void OnComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component == ((ComponentDesigner)this).Component)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(((ComponentDesigner)this).AssociatedComponents);
			{
				foreach (IComponent item in arrayList)
				{
					designerHost.DestroyComponent(item);
				}
				return;
			}
		}
		if (e.Component is CrumbBarItem && ((CrumbBarItem)e.Component).GetOwner() == ((ControlDesigner)this).Control)
		{
			OnNodeRemoving(e.Component as CrumbBarItem);
		}
	}

	private void OnNodeRemoving(CrumbBarItem node)
	{
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		CrumbBar crumbBar = ((ControlDesigner)this).Control as CrumbBar;
		bool isInSelectedPath = crumbBar.GetIsInSelectedPath(node);
		if (node.Parent != null)
		{
			if (node.Parent is CrumbBarItem crumbBarItem)
			{
				componentChangeService?.OnComponentChanging(crumbBarItem, TypeDescriptor.GetProperties(crumbBarItem)["SubItems"]);
				crumbBarItem.SubItems.Remove(node);
				componentChangeService?.OnComponentChanged(crumbBarItem, TypeDescriptor.GetProperties(crumbBarItem)["SubItems"], null, null);
			}
		}
		else
		{
			componentChangeService?.OnComponentChanging(crumbBar, TypeDescriptor.GetProperties(crumbBar)["Items"]);
			crumbBar.Items.Remove(node);
			componentChangeService?.OnComponentChanged(crumbBar, TypeDescriptor.GetProperties(crumbBar)["Items"], null, null);
		}
		if (node.SubItems.Count > 0)
		{
			BaseItem[] array = new BaseItem[node.SubItems.Count];
			node.SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				node.SubItems.Remove(baseItem);
				designerHost?.DestroyComponent(baseItem);
			}
		}
		if (isInSelectedPath)
		{
			crumbBar.SelectedItem = crumbBar.GetFirstVisibleItem();
		}
		RecalcLayout();
	}

	private void RecalcLayout()
	{
		if (((ControlDesigner)this).Control is CrumbBar crumbBar)
		{
			crumbBar.RecalcLayout();
		}
	}

	private void GetItemsRecursive(BaseItem parent, ArrayList c)
	{
		c.Add(parent);
		foreach (BaseItem subItem in parent.SubItems)
		{
			c.Add(subItem);
			GetItemsRecursive(subItem, c);
		}
	}
}
