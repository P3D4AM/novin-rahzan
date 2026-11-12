using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Design;

public class AdvTreeDesigner : ParentControlDesigner
{
	private const string TEMP_NAME = "tempDragDropItem";

	private const int WM_RBUTTONDOWN = 516;

	private const int WM_LBUTTONDOWN = 513;

	private const int WM_LBUTTONDBLCLK = 515;

	private bool m_DragDropStarted;

	private Timer m_TimerAdded;

	private Timer m_TimerDragDrop;

	private bool m_DragLeave;

	private bool m_ControlRemoved;

	private DateTime m_JustAdded = DateTime.MinValue;

	private bool m_NewControlAdded;

	private Point m_MouseDownPoint = Point.Empty;

	private DesignerActionListCollection _ActionLists;

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			if (((ControlDesigner)this).Control is AdvTree advTree)
			{
				foreach (Node node in advTree.Nodes)
				{
					GetNodesRecursive(node, arrayList);
				}
				foreach (ElementStyle style in advTree.Styles)
				{
					arrayList.Add(style);
				}
				if (advTree.NodesConnector != null)
				{
					arrayList.Add(advTree.NodesConnector);
				}
				if (advTree.SelectedPathConnector != null)
				{
					arrayList.Add(advTree.SelectedPathConnector);
				}
				foreach (ColumnHeader column in advTree.Columns)
				{
					arrayList.Add(column);
				}
			}
			return arrayList;
		}
	}

	public override SelectionRules SelectionRules
	{
		get
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(((ComponentDesigner)this).Component);
			PropertyDescriptor propertyDescriptor = properties["Locked"];
			if (propertyDescriptor != null)
			{
				object value = propertyDescriptor.GetValue(((ComponentDesigner)this).Component);
				if (value is bool && (bool)value)
				{
					return (SelectionRules)(-1073741824);
				}
			}
			return (SelectionRules)1342177295;
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
				_ActionLists.Add((DesignerActionList)(object)new AdvTreeActionList(this));
			}
			return _ActionLists;
		}
	}

	public override void Initialize(IComponent component)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		((ParentControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += OnSelectionChanged;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoving;
			}
			if (component is Control)
			{
				((Control)component).ControlAdded += new ControlEventHandler(ControlAdded);
				((Control)component).ControlRemoved += new ControlEventHandler(ControlRemoved);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		if (((ControlDesigner)this).Control != null)
		{
			((ControlDesigner)this).Control.ControlAdded -= new ControlEventHandler(ControlAdded);
			((ControlDesigner)this).Control.ControlRemoved -= new ControlEventHandler(ControlRemoved);
		}
		((ParentControlDesigner)this).Dispose(disposing);
	}

	internal void HookControl(Control c)
	{
		((ControlDesigner)this).HookChildControls(c);
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		SetDefaults();
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
	}

	private void SetDefaults()
	{
		CreateNode(null, addToCollections: true);
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			Utilities.InitializeTree(((ControlDesigner)this).Control as AdvTree, new ComponentFactory(designerHost));
			((ControlDesigner)this).Control.Size = new Size(100, 100);
		}
	}

	private Node CreateNode(Node parentNode, bool addToCollections)
	{
		AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		Node node = null;
		advTree.BeginUpdate();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (addToCollections && componentChangeService != null)
			{
				if (parentNode != null)
				{
					componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parentNode).Find("Nodes", ignoreCase: true));
				}
				else
				{
					componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true));
				}
			}
			node = designerHost.CreateComponent(typeof(Node)) as Node;
			if (node != null)
			{
				node.Text = node.Name;
				node.Expanded = true;
				if (addToCollections)
				{
					if (parentNode == null)
					{
						advTree.Nodes.Add(node);
					}
					else
					{
						parentNode.Nodes.Add(node);
					}
					if (componentChangeService != null)
					{
						if (parentNode != null)
						{
							componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parentNode).Find("Nodes", ignoreCase: true), null, null);
						}
						else
						{
							componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
						}
					}
				}
			}
		}
		finally
		{
			advTree.EndUpdate();
		}
		return node;
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		ISelectionService selectionService = (ISelectionService)sender;
		if (selectionService.PrimarySelection == ((ComponentDesigner)this).Component)
		{
			AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
			advTree.SelectedNode = null;
		}
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
		if (e.Component is Node && ((Node)e.Component).TreeControl == ((ControlDesigner)this).Control)
		{
			OnNodeRemoving(e.Component as Node);
		}
	}

	private void OnNodeRemoving(Node node)
	{
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (node.Parent != null)
		{
			Node parent = node.Parent;
			componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["Nodes"]);
			node.Remove();
			componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["Nodes"], null, null);
		}
		else
		{
			AdvTree component = ((ControlDesigner)this).Control as AdvTree;
			componentChangeService?.OnComponentChanging(component, TypeDescriptor.GetProperties(component)["Nodes"]);
			node.Remove();
			componentChangeService?.OnComponentChanged(component, TypeDescriptor.GetProperties(component)["Nodes"], null, null);
		}
		if (node.Nodes.Count > 0)
		{
			Node[] array = new Node[node.Nodes.Count];
			node.Nodes.CopyTo(array);
			Node[] array2 = array;
			foreach (Node node2 in array2)
			{
				node2.Remove();
				if (node2.ParentConnector != null)
				{
					designerHost?.DestroyComponent(node2.ParentConnector);
				}
				designerHost?.DestroyComponent(node2);
			}
		}
		RecalcLayout();
	}

	private void GetNodesRecursive(Node parent, ArrayList c)
	{
		c.Add(parent);
		if (parent.ParentConnector != null)
		{
			c.Add(parent.ParentConnector);
		}
		foreach (Node node in parent.Nodes)
		{
			c.Add(node);
			if (node.Cells.Count > 1)
			{
				for (int i = 1; i < node.Cells.Count; i++)
				{
					c.Add(node.Cells[i]);
				}
			}
			if (node.NodesColumns.Count > 0)
			{
				foreach (ColumnHeader nodesColumn in node.NodesColumns)
				{
					c.Add(nodesColumn);
				}
			}
			GetNodesRecursive(node, c);
		}
	}

	protected override void WndProc(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 513:
			if (OnMouseDown(ref m, (MouseButtons)1048576))
			{
				return;
			}
			break;
		case 516:
			if (OnMouseDown(ref m, (MouseButtons)2097152))
			{
				return;
			}
			break;
		case 515:
			if (OnMouseDoubleClick())
			{
				return;
			}
			break;
		}
		((ControlDesigner)this).WndProc(ref m);
	}

	private bool OnMouseDoubleClick()
	{
		bool result = false;
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService.PrimarySelection is Node && ((Node)selectionService.PrimarySelection).TreeControl == ((ControlDesigner)this).Control)
		{
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IDesigner designer = designerHost.GetDesigner(selectionService.PrimarySelection as IComponent);
				if (designer != null)
				{
					designer.DoDefaultAction();
					result = true;
				}
			}
		}
		return result;
	}

	protected virtual bool OnMouseDown(ref Message m, MouseButtons button)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		if (!(((ControlDesigner)this).Control is AdvTree advTree))
		{
			return false;
		}
		Point p = ((Control)advTree).PointToClient(Control.MousePosition);
		Node nodeAt = advTree.GetNodeAt(p);
		if (nodeAt != null && (int)button == 2097152)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			ArrayList arrayList = new ArrayList(1);
			arrayList.Add(nodeAt);
			selectionService.SetSelectedComponents(arrayList, SelectionTypes.MouseDown);
			advTree.SelectedNode = nodeAt;
			((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
			return true;
		}
		return false;
	}

	private void DestroyComponent(IComponent c)
	{
		((IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost)))?.DestroyComponent(c);
	}

	private void RecalcLayout()
	{
		if (((ControlDesigner)this).Control is AdvTree advTree)
		{
			advTree.RecalcLayout();
			((Control)advTree).Refresh();
		}
	}

	private void ControlAdded(object sender, ControlEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if (m_NewControlAdded || (!m_NewControlAdded && OnControlAdded(e)))
		{
			m_TimerAdded = new Timer();
			m_TimerAdded.Tick += TimerTick;
			m_TimerAdded.Interval = 50;
			m_TimerAdded.Enabled = true;
			m_TimerAdded.Start();
			m_NewControlAdded = false;
		}
	}

	protected virtual bool OnControlAdded(ControlEventArgs e)
	{
		return false;
	}

	private void ControlRemoved(object sender, ControlEventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null || designerHost.Loading)
		{
			return;
		}
		if (m_JustAdded != DateTime.MinValue && DateTime.Now.Subtract(m_JustAdded).Seconds < 2)
		{
			m_JustAdded = DateTime.MinValue;
			return;
		}
		m_JustAdded = DateTime.MinValue;
		if (m_DragLeave)
		{
			ControlRemoved(e.Control);
			return;
		}
		if (m_TimerDragDrop != null)
		{
			m_ControlRemoved = true;
			return;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && selectionService.PrimarySelection == e.Control && Utilities.FindNodeForControl(((ControlDesigner)this).Control as AdvTree, e.Control) != null)
		{
			ControlRemoved(e.Control);
		}
	}

	private void ControlRemoved(Control control)
	{
		AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
		if (control == null)
		{
			return;
		}
		Node node = Utilities.FindNodeForControl(advTree, control);
		if (node != null)
		{
			if (m_DragDropStarted)
			{
				advTree.InternalDragLeave();
				m_DragDropStarted = false;
			}
			if (node.Parent != null)
			{
				Node parent = node.Parent;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["Nodes"]);
				node.Remove();
				componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["Nodes"], null, null);
			}
			DestroyComponent(node);
			RecalcLayout();
		}
	}

	private void TimerTick(object sender, EventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		m_TimerAdded.Stop();
		m_TimerAdded.Enabled = false;
		m_TimerAdded = null;
		RecalcLayout();
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && selectionService.PrimarySelection is Control && ((ControlDesigner)this).Control.Controls.Contains((Control)selectionService.PrimarySelection))
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService.OnComponentChanged(selectionService.PrimarySelection, null, null, null);
		}
	}

	private void TimerTickDragDrop(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		Point pt = ((ControlDesigner)this).Control.PointToClient(Control.MousePosition);
		if (((ControlDesigner)this).Control.Bounds.Contains(pt))
		{
			m_DragLeave = false;
		}
		else
		{
			m_DragLeave = true;
		}
		if ((int)Control.MouseButtons == 1048576)
		{
			return;
		}
		m_TimerDragDrop.Enabled = false;
		m_TimerDragDrop.Stop();
		m_TimerDragDrop.Tick -= TimerTickDragDrop;
		((Component)(object)m_TimerDragDrop).Dispose();
		m_TimerDragDrop = null;
		if (m_ControlRemoved)
		{
			m_ControlRemoved = false;
			if (((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService && selectionService.PrimarySelection is Control)
			{
				ControlRemoved((Control)selectionService.PrimarySelection);
			}
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (m_DragDropStarted)
		{
			AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
			advTree.InternalDragLeave();
			m_DragDropStarted = false;
		}
		((ParentControlDesigner)this).OnDragLeave(e);
	}

	protected override void OnDragOver(DragEventArgs de)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		if (!(((ControlDesigner)this).Control is AdvTree advTree))
		{
			((ParentControlDesigner)this).OnDragOver(de);
			return;
		}
		if (m_DragDropStarted)
		{
			DragEventArgs drgevent = new DragEventArgs((IDataObject)null, de.KeyState, de.X, de.Y, (DragDropEffects)(-2147483645), (DragDropEffects)2);
			advTree.InternalDragOver(drgevent);
			de.Effect = (DragDropEffects)2;
			return;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && selectionService.PrimarySelection != ((ComponentDesigner)this).Component)
		{
			if (selectionService.PrimarySelection is Control && ((ControlDesigner)this).Control.Controls.Contains((Control)selectionService.PrimarySelection))
			{
				object primarySelection = selectionService.PrimarySelection;
				Node node = Utilities.FindNodeForControl(advTree, (Control)((primarySelection is Control) ? primarySelection : null));
				if (node != null && advTree.StartDragDrop(node))
				{
					if (m_TimerDragDrop == null)
					{
						m_TimerDragDrop = new Timer();
						m_TimerDragDrop.Tick += TimerTickDragDrop;
						m_TimerDragDrop.Interval = 100;
						m_TimerDragDrop.Enabled = true;
						m_TimerDragDrop.Start();
					}
					m_DragDropStarted = true;
				}
				return;
			}
			if (selectionService.SelectionCount > 1)
			{
				de.Effect = (DragDropEffects)0;
				return;
			}
			if (selectionService.PrimarySelection is Control && ((Control)selectionService.PrimarySelection).Parent != null)
			{
				Node node2 = new Node();
				node2.Name = "tempDragDropItem";
				node2.Text = ((Control)selectionService.PrimarySelection).Name;
				if (advTree.StartDragDrop(node2))
				{
					m_DragDropStarted = true;
				}
			}
		}
		((ParentControlDesigner)this).OnDragOver(de);
	}

	protected override void OnDragDrop(DragEventArgs de)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		if (!(((ControlDesigner)this).Control is AdvTree advTree))
		{
			((ParentControlDesigner)this).OnDragDrop(de);
			return;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (selectionService != null && selectionService.PrimarySelection is Control && ((ControlDesigner)this).Control.Controls.Contains((Control)selectionService.PrimarySelection))
		{
			de.Effect = (DragDropEffects)2;
			advTree.InternalDragDrop(new DragEventArgs((IDataObject)null, 0, 0, 0, (DragDropEffects)2, (DragDropEffects)(-2147483645)));
			componentChangeService?.OnComponentChanged(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
		}
		else
		{
			if (selectionService.SelectionCount > 1)
			{
				de.Effect = (DragDropEffects)0;
				return;
			}
			Node dragNode = advTree.GetDragNode();
			Node[] array = dragNode.Tag as Node[];
			Node node = null;
			if (array != null && array.Length > 0)
			{
				node = array[0];
			}
			if (dragNode != null && node != null && node.Name == "tempDragDropItem")
			{
				m_JustAdded = DateTime.Now;
				node = CreateNode(null, addToCollections: false);
				PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(node)["HostedControl"];
				Node component = node;
				object primarySelection = selectionService.PrimarySelection;
				propertyDescriptor.SetValue(component, (primarySelection is Control) ? primarySelection : null);
				TypeDescriptor.GetProperties(node)["Text"].SetValue(node, node.HostedControl.Name);
				dragNode.Tag = new Node[1] { node };
				Node parent = advTree.GetDragNode().Parent;
				Node parent2 = node.Parent;
				advTree.InternalDragDrop(new DragEventArgs((IDataObject)null, 0, 0, 0, (DragDropEffects)2, (DragDropEffects)(-2147483645)));
				m_NewControlAdded = true;
				m_DragDropStarted = false;
				parent = node.Parent;
				if (componentChangeService != null)
				{
					if (parent2 != null)
					{
						componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parent2).Find("Nodes", ignoreCase: true), null, null);
					}
					else
					{
						componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
					}
					if (parent != null)
					{
						componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parent).Find("Nodes", ignoreCase: true), null, null);
					}
					else
					{
						componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
					}
				}
				advTree.RecalcLayout();
			}
		}
		((ParentControlDesigner)this).OnDragDrop(de);
	}

	protected virtual void OnNodeSelected(Node node)
	{
	}

	protected virtual bool CanDragNode(Node node)
	{
		return true;
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		if (!(((ControlDesigner)this).Control is AdvTree advTree))
		{
			((ParentControlDesigner)this).OnMouseDragBegin(x, y);
			return;
		}
		Point point = ((Control)advTree).PointToClient(new Point(x, y));
		Node nodeAt = advTree.GetNodeAt(point);
		if (nodeAt != null)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, nodeAt, advTree.NodeDisplay.Offset);
			if (!nodeRectangle.IsEmpty && nodeRectangle.Contains(point))
			{
				nodeAt.Expanded = !nodeAt.Expanded;
				return;
			}
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(nodeAt);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.MouseDown);
				OnNodeSelected(nodeAt);
			}
			advTree.SelectedNode = nodeAt;
		}
		else
		{
			advTree.SelectedNode = null;
		}
		if (nodeAt == null || !CanDragNode(nodeAt))
		{
			if (nodeAt == null)
			{
				((ParentControlDesigner)this).OnMouseDragBegin(x, y);
			}
			else
			{
				((ControlDesigner)this).Control.Capture = true;
			}
		}
		else
		{
			m_MouseDownPoint = new Point(x, y);
			((ControlDesigner)this).Control.Capture = true;
		}
	}

	protected override void OnMouseDragMove(int x, int y)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected I4, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
		if (!m_MouseDownPoint.IsEmpty && advTree.SelectedNode != null && (Math.Abs(m_MouseDownPoint.X - x) >= SystemInformation.DragSize.Width || Math.Abs(m_MouseDownPoint.Y - y) >= SystemInformation.DragSize.Height))
		{
			advTree.StartDragDrop(advTree.SelectedNode);
			m_MouseDownPoint = Point.Empty;
			m_DragDropStarted = true;
		}
		else if (m_DragDropStarted)
		{
			DragEventArgs drgevent = new DragEventArgs((IDataObject)null, (int)Control.ModifierKeys, x, y, (DragDropEffects)(-2147483645), (DragDropEffects)2);
			advTree.InternalDragOver(drgevent);
		}
	}

	protected override void OnMouseDragEnd(bool cancel)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		((ControlDesigner)this).Control.Capture = false;
		Cursor.Clip = Rectangle.Empty;
		AdvTree advTree = ((ControlDesigner)this).Control as AdvTree;
		if (m_DragDropStarted)
		{
			if (advTree != null && advTree.IsDragDropInProgress)
			{
				if (cancel)
				{
					advTree.InternalDragLeave();
				}
				else if (advTree.GetDragNode() != null)
				{
					IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					Node[] array = advTree.GetDragNode().Tag as Node[];
					Node node = null;
					if (array != null && array.Length > 0)
					{
						node = array[0];
					}
					if (node != null)
					{
						Node parent = advTree.GetDragNode().Parent;
						Node parent2 = node.Parent;
						if (componentChangeService != null)
						{
							if (parent2 != null)
							{
								componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parent2).Find("Nodes", ignoreCase: true));
							}
							else
							{
								componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true));
							}
						}
						advTree.InternalDragDrop(new DragEventArgs((IDataObject)null, 0, 0, 0, (DragDropEffects)0, (DragDropEffects)0));
						parent = node.Parent;
						if (componentChangeService != null)
						{
							if (parent2 != null)
							{
								componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parent2).Find("Nodes", ignoreCase: true), null, null);
							}
							else
							{
								componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
							}
							if (parent != null)
							{
								componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(parent).Find("Nodes", ignoreCase: true), null, null);
							}
							else
							{
								componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(advTree).Find("Nodes", ignoreCase: true), null, null);
							}
						}
					}
				}
			}
			cancel = true;
		}
		else if (advTree.SelectedNode != null)
		{
			cancel = true;
		}
		m_DragDropStarted = false;
		((ParentControlDesigner)this).OnMouseDragEnd(cancel);
	}

	private void EditColumns(object sender, EventArgs e)
	{
		EditColumns();
	}

	internal void EditColumns()
	{
		AdvTree objectToChange = ((ComponentDesigner)this).Component as AdvTree;
		EditValue((ComponentDesigner)(object)this, objectToChange, "Columns");
	}

	internal static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
	{
		Type type = Type.GetType("System.Windows.Forms.Design.EditorServiceContext");
		if ((object)type == null)
		{
			type = Type.GetType("System.Windows.Forms.Design.EditorServiceContext, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
		}
		if ((object)type != null)
		{
			MethodInfo method = type.GetMethod("EditValue");
			if ((object)method != null)
			{
				return method.Invoke(null, new object[3] { designer, objectToChange, propName });
			}
		}
		return null;
	}

	private void CreateNode(object sender, EventArgs e)
	{
		CreateNode();
	}

	internal void CreateNode()
	{
		Node node = CreateNode(((ComponentDesigner)this).Component as AdvTree);
		if (node != null)
		{
			ISelectionService selectionService = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
			ArrayList arrayList = new ArrayList(1);
			arrayList.Add(node);
			if (selectionService != null)
			{
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.MouseDown);
				node.TreeControl.SelectedNode = node;
			}
		}
	}

	private Node CreateNode(AdvTree tree)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		Node node = null;
		tree.BeginUpdate();
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
			tree.EndUpdate();
		}
		return node;
	}

	protected override bool GetHitTest(Point pt)
	{
		if (((ControlDesigner)this).Control is AdvTree advTree && ((Control)advTree).IsHandleCreated && advTree.AutoScroll)
		{
			Point pt2 = ((Control)advTree).PointToClient(pt);
			if (advTree.VScrollBar != null && ((Control)advTree.VScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
			if (advTree.HScrollBar != null && ((Control)advTree.HScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
		}
		return ((ControlDesigner)this).GetHitTest(pt);
	}
}
