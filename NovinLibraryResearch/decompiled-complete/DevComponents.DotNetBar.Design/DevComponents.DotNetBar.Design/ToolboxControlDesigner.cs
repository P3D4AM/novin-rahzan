using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class ToolboxControlDesigner : ControlDesigner
{
	private bool _InternalRemoving;

	private Control _DesignerHost;

	private bool _MouseDownSelectionPerformed;

	private DesignerActionListCollection _ActionLists;

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			ToolboxControl toolboxControl = (ToolboxControl)(object)((ControlDesigner)this).Control;
			if (toolboxControl != null)
			{
				foreach (ToolboxGroup group in toolboxControl.Groups)
				{
					if (group.DesignMode)
					{
						arrayList.Add(group);
					}
				}
			}
			return arrayList;
		}
	}

	private bool IsControlSelected
	{
		get
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService == null)
			{
				return false;
			}
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			if (IsSelected(selectedComponents, ((ControlDesigner)this).Control))
			{
				return true;
			}
			return false;
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
				_ActionLists.Add((DesignerActionList)(object)new ToolboxControlActionList(this));
			}
			return _ActionLists;
		}
	}

	public bool Expanded
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Expanded"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Expanded"] = value;
		}
	}

	public void SelectionRefresh()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Message val = default(Message);
		((Message)(ref val)).Msg = 277;
		((ControlDesigner)this).WndProc(ref val);
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete += dh_LoadComplete;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved += ComponentRemoved;
			componentChangeService.ComponentRemoving += ComponentRemoving;
		}
	}

	private void ComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component != ((ComponentDesigner)this).Component || _InternalRemoving)
		{
			return;
		}
		_InternalRemoving = true;
		try
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving -= ComponentRemoving;
			}
			ToolboxControl toolboxControl = (ToolboxControl)((ComponentDesigner)this).Component;
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost == null || toolboxControl == null)
			{
				return;
			}
			for (int num = toolboxControl.Groups.Count - 1; num >= 0; num--)
			{
				BaseItem baseItem = toolboxControl.Groups[num];
				if (baseItem.Parent == toolboxControl.ItemsPanel.GetBaseItemContainer())
				{
					DestroySubItems(baseItem, designerHost);
					designerHost.DestroyComponent(baseItem);
				}
			}
		}
		finally
		{
			_InternalRemoving = false;
		}
	}

	protected virtual void DestroySubItems(BaseItem parent, IDesignerHost dh)
	{
		if (parent is ControlContainerItem)
		{
			if (((ControlContainerItem)parent).Control != null)
			{
				Control control = ((ControlContainerItem)parent).Control;
				((ControlContainerItem)parent).Control = null;
				dh.DestroyComponent((IComponent)control);
			}
		}
		else if (parent is DockContainerItem && ((DockContainerItem)parent).Control != null)
		{
			Control control2 = ((DockContainerItem)parent).Control;
			((DockContainerItem)parent).Control = null;
			dh.DestroyComponent((IComponent)control2);
		}
		BaseItem[] array = new BaseItem[parent.SubItems.Count];
		parent.SubItems.CopyTo(array, 0);
		BaseItem[] array2 = array;
		foreach (BaseItem baseItem in array2)
		{
			DestroySubItems(baseItem, dh);
			dh.DestroyComponent(baseItem);
		}
	}

	protected virtual void DestroySubItems(BaseItem parent)
	{
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost dh)
		{
			DestroySubItems(parent, dh);
		}
	}

	private void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		ToolboxGroup toolboxGroup = e.Component as ToolboxGroup;
		ToolboxControl toolboxControl = (ToolboxControl)(object)((ControlDesigner)this).Control;
		if (toolboxGroup != null && toolboxControl != null)
		{
			if (toolboxControl.Groups.Contains(toolboxGroup))
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
				componentChangeService?.OnComponentChanging(toolboxControl, TypeDescriptor.GetProperties(toolboxControl)["Groups"]);
				toolboxControl.Groups.Remove(toolboxGroup);
				componentChangeService?.OnComponentChanged(toolboxControl, TypeDescriptor.GetProperties(toolboxControl)["Groups"], null, null);
			}
			RecalcLayout();
		}
	}

	private void RecalcLayout()
	{
		((ToolboxControl)(object)((ControlDesigner)this).Control)?.RecalcLayout();
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		SetDefaults();
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}

	private void SetDefaults()
	{
		ToolboxGroup parentGroup = CreateToolboxGroup(addToCollections: true);
		CreateToolboxItem(parentGroup, addToCollections: true);
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			((ControlDesigner)this).Control.Size = new Size(200, 200);
		}
	}

	public void CreateGroup()
	{
		CreateToolboxGroup(addToCollections: true);
	}

	private ToolboxGroup CreateToolboxGroup(bool addToCollections)
	{
		ToolboxControl toolboxControl = ((ControlDesigner)this).Control as ToolboxControl;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		ToolboxGroup toolboxGroup = null;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (addToCollections)
		{
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(toolboxControl).Find("Groups", ignoreCase: true));
		}
		toolboxGroup = designerHost.CreateComponent(typeof(ToolboxGroup)) as ToolboxGroup;
		if (toolboxGroup != null)
		{
			toolboxGroup.TitleText = toolboxGroup.Name;
			toolboxGroup.Expanded = true;
			toolboxGroup.LayoutOrientation = eOrientation.Vertical;
			toolboxGroup.MultiLine = false;
			toolboxGroup.ResizeItemsToFit = true;
			if (addToCollections)
			{
				toolboxControl.Groups.Add(toolboxGroup);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(toolboxControl).Find("Groups", ignoreCase: true), null, null);
			}
		}
		return toolboxGroup;
	}

	private ToolboxItem CreateToolboxItem(ToolboxGroup parentGroup, bool addToCollections)
	{
		_ = ((ControlDesigner)this).Control;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		if (parentGroup == null)
		{
			throw new ArgumentNullException("parentGroup", "ToolboxGroup must be specified");
		}
		ToolboxItem toolboxItem = null;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (addToCollections)
		{
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parentGroup).Find("SubItems", ignoreCase: true));
		}
		toolboxItem = designerHost.CreateComponent(typeof(ToolboxItem)) as ToolboxItem;
		if (toolboxItem != null)
		{
			toolboxItem.Text = toolboxItem.Name;
			toolboxItem.Symbol = "\ue251";
			toolboxItem.SymbolSet = eSymbolSet.Material;
			if (addToCollections)
			{
				parentGroup.SubItems.Add(toolboxItem);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parentGroup).Find("Nodes", ignoreCase: true), null, null);
			}
		}
		return toolboxItem;
	}

	protected override bool GetHitTest(Point point)
	{
		if (((ControlDesigner)this).GetHitTest(point))
		{
			return true;
		}
		if (!(((ControlDesigner)this).Control is ToolboxControl toolboxControl))
		{
			return false;
		}
		Point pt = ((Control)toolboxControl.ItemsPanel).PointToClient(point);
		if (toolboxControl.ItemsPanel.VScrollBar != null && ((Control)toolboxControl.ItemsPanel.VScrollBar).Visible && ((Control)toolboxControl.ItemsPanel.VScrollBar).Bounds.Contains(pt))
		{
			return true;
		}
		if (toolboxControl.ItemsPanel.HScrollBar != null && ((Control)toolboxControl.ItemsPanel.HScrollBar).Visible && ((Control)toolboxControl.ItemsPanel.HScrollBar).Bounds.Contains(pt))
		{
			return true;
		}
		return ((ControlDesigner)this).GetHitTest(point);
	}

	private bool IsSelected(ICollection selectedComponents, object item)
	{
		foreach (object selectedComponent in selectedComponents)
		{
			if (selectedComponent == item)
			{
				return true;
			}
		}
		return false;
	}

	protected override void WndProc(ref Message m)
	{
		if (_DesignerHost == null)
		{
			_DesignerHost = Control.FromHandle(((Message)(ref m)).HWnd);
		}
		switch (((Message)(ref m)).Msg)
		{
		case 513:
		case 516:
			if (OnMouseDown(ref m, (MouseButtons)((((Message)(ref m)).Msg == 513) ? 1048576 : 2097152)))
			{
				return;
			}
			break;
		case 514:
		case 517:
			if (OnMouseUp(ref m, (MouseButtons)((((Message)(ref m)).Msg == 513) ? 1048576 : 2097152)))
			{
				return;
			}
			break;
		}
		((ControlDesigner)this).WndProc(ref m);
	}

	protected virtual bool OnMouseUp(ref Message m, MouseButtons param1)
	{
		if (_MouseDownSelectionPerformed)
		{
			_MouseDownSelectionPerformed = false;
			return true;
		}
		return false;
	}

	protected virtual bool OnMouseDown(ref Message m, MouseButtons button)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		if (!(((ControlDesigner)this).Control is ToolboxControl toolboxControl))
		{
			return false;
		}
		if (((Message)(ref m)).HWnd != ((Control)toolboxControl.ItemsPanel).Handle)
		{
			return false;
		}
		Point p = new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam));
		BaseItem baseItem = toolboxControl.ItemsPanel.HitTest(p);
		if (baseItem != null)
		{
			SelectComponent(baseItem, ((int)Control.ModifierKeys == 131072 || (int)Control.ModifierKeys == 65536) ? SelectionTypes.Add : SelectionTypes.Click);
			_MouseDownSelectionPerformed = true;
			return true;
		}
		if (!IsControlSelected)
		{
			SelectComponent((IComponent)((ControlDesigner)this).Control, SelectionTypes.Replace);
		}
		return false;
	}

	protected virtual void SelectComponent(IComponent comp, SelectionTypes selectionType)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && comp != null)
		{
			ArrayList arrayList = new ArrayList(1);
			arrayList.Add(comp);
			selectionService.SetSelectedComponents(arrayList, selectionType);
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		properties["Expanded"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "Expanded", typeof(bool), new BrowsableAttribute(browsable: true), new DefaultValueAttribute(value: true), new CategoryAttribute("Appearance"), new DescriptionAttribute("Indicates whether control is expanded and shows items in full size with images and text"));
	}

	private void dh_LoadComplete(object sender, EventArgs e)
	{
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete -= dh_LoadComplete;
		}
		RecalcLayout();
	}
}
