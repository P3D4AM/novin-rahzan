using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class BarBaseControlDesigner : ParentControlDesigner, IDesignerServices, IBarItemDesigner
{
	private const string TEMP_NAME = "tempDragDropItem";

	public const string CreatingItemTransactionDescription = "Creating item";

	private bool m_EnableItemDragDrop;

	private bool m_AcceptExternalControls = true;

	private Point m_MouseDownPoint = Point.Empty;

	private BaseItem m_DragItem;

	private bool m_DragInProgress;

	private IDesignTimeProvider m_DesignTimeProvider;

	private int m_InsertPosition;

	private bool m_InsertBefore;

	private bool m_Capture;

	private Control m_DesignerHost;

	private Timer m_TimerAdded;

	private Timer m_TimerDragDrop;

	private bool m_NewControlAdded;

	private bool m_SuspendInternalCursor;

	private bool m_DragLeave;

	private bool m_ControlRemoved;

	private DateTime m_JustAdded = DateTime.MinValue;

	private bool m_MouseDownSelectionPerformed;

	private bool m_PassiveContainer;

	protected bool m_AddingItem;

	protected bool m_CreatingItem;

	protected DesignerTransaction m_InsertItemTransaction;

	private bool m_InternalRemoveInProgress;

	private bool _IsSelected;

	protected bool m_InternalRemoving;

	public bool IsSelected
	{
		get
		{
			return _IsSelected;
		}
		internal set
		{
			if (value != _IsSelected)
			{
				bool isSelected = _IsSelected;
				_IsSelected = value;
				OnIsSelectedChanged(isSelected, value);
			}
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			BaseItem itemContainer = GetItemContainer();
			if (itemContainer != null)
			{
				foreach (BaseItem subItem in itemContainer.SubItems)
				{
					if (subItem.DesignMode)
					{
						arrayList.Add(subItem);
					}
				}
			}
			return arrayList;
		}
	}

	protected ICollection BaseAssociatedComponents => ((ControlDesigner)this).AssociatedComponents;

	protected virtual eDotNetBarStyle InternalStyle => eDotNetBarStyle.Office2003;

	protected virtual bool DragInProgress
	{
		get
		{
			return m_DragInProgress;
		}
		set
		{
			m_DragInProgress = value;
		}
	}

	internal BaseItem DragItem => m_DragItem;

	protected virtual bool EnableItemDragDrop
	{
		get
		{
			return m_EnableItemDragDrop;
		}
		set
		{
			m_EnableItemDragDrop = value;
		}
	}

	protected virtual bool AcceptExternalControls
	{
		get
		{
			return m_AcceptExternalControls;
		}
		set
		{
			m_AcceptExternalControls = value;
		}
	}

	protected virtual bool IsDockableWindow => false;

	protected virtual bool MouseDownSelectionPerformed
	{
		get
		{
			return m_MouseDownSelectionPerformed;
		}
		set
		{
			m_MouseDownSelectionPerformed = value;
		}
	}

	protected virtual bool PassiveContainer
	{
		get
		{
			return m_PassiveContainer;
		}
		set
		{
			m_PassiveContainer = value;
		}
	}

	public override void Initialize(IComponent component)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
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
				componentChangeService.ComponentRemoved += OnComponentRemoved;
				componentChangeService.ComponentAdding += ComponentChangeComponentAdding;
				componentChangeService.ComponentAdded += ComponentChangeComponentAdded;
			}
			if (((ComponentDesigner)this).GetService(typeof(IDesignerEventService)) is IDesignerEventService designerEventService)
			{
				designerEventService.ActiveDesignerChanged += OnActiveDesignerChanged;
				designerEventService.SelectionChanged += DesignerSelectionChanged;
			}
			if (((ControlDesigner)this).Control is IBarDesignerServices)
			{
				((IBarDesignerServices)((ControlDesigner)this).Control).Designer = this;
			}
			if (((ControlDesigner)this).Control is ItemControl)
			{
				((ItemControl)(object)((ControlDesigner)this).Control).SetDesignMode(mode: true);
			}
			if (component is Control)
			{
				((Control)component).ControlAdded += new ControlEventHandler(ComponentAdded);
				((Control)component).ControlRemoved += new ControlEventHandler(ComponentRemoved);
			}
		}
	}

	protected virtual void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (m_AddingItem)
		{
			m_AddingItem = false;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ControlDesigner)this).Control, null);
			GetDefaultNewItemContainer().SubItems.Add(e.Component as BaseItem);
			componentChangeService?.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
			m_InsertItemTransaction.Commit();
			m_InsertItemTransaction = null;
			RecalcLayout();
		}
	}

	private void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
	{
		if (m_InsertItemTransaction == null && !m_AddingItem && !m_CreatingItem && e.Component is BaseItem && ((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService && selectionService.PrimarySelection == ((ControlDesigner)this).Control)
		{
			IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (!designerHost.InTransaction || !(designerHost.TransactionDescription == "Creating item"))
			{
				m_AddingItem = true;
				m_InsertItemTransaction = designerHost.CreateTransaction("Adding Item Clip");
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
			componentChangeService.ComponentRemoved -= OnComponentRemoved;
		}
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		if (((ComponentDesigner)this).GetService(typeof(IDesignerEventService)) is IDesignerEventService designerEventService)
		{
			designerEventService.ActiveDesignerChanged -= OnActiveDesignerChanged;
			designerEventService.SelectionChanged -= DesignerSelectionChanged;
		}
		if (((ComponentDesigner)this).Component is Control)
		{
			((Control)((ComponentDesigner)this).Component).ControlAdded -= new ControlEventHandler(ComponentAdded);
			((Control)((ComponentDesigner)this).Component).ControlRemoved -= new ControlEventHandler(ComponentRemoved);
		}
		((ParentControlDesigner)this).Dispose(disposing);
	}

	public override bool CanParent(Control control)
	{
		BaseItem controlItem = GetControlItem(control);
		if (controlItem != null && controlItem != m_DragItem && !m_NewControlAdded)
		{
			return false;
		}
		return ((ParentControlDesigner)this).CanParent(control);
	}

	private void ComponentAdded(object sender, ControlEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if ((m_NewControlAdded && m_EnableItemDragDrop && m_AcceptExternalControls && !IsDockableWindow) || (!m_NewControlAdded && OnControlAdded(e)))
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

	private void ComponentRemoved(object sender, ControlEventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null || designerHost.Loading || m_InternalRemoveInProgress)
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
		if (selectionService != null && selectionService.PrimarySelection == e.Control && GetControlItem(e.Control) != null)
		{
			ControlRemoved(e.Control);
		}
	}

	private void ControlRemoved(Control control)
	{
		if (control == null || m_InternalRemoveInProgress)
		{
			return;
		}
		BaseItem controlItem = GetControlItem(control);
		if (controlItem != null)
		{
			MouseDragDrop(-1, -1);
			if (controlItem.Parent != null)
			{
				controlItem.Parent.SubItems.Remove(controlItem);
			}
			if (controlItem is ControlContainerItem)
			{
				((ControlContainerItem)controlItem).Control = null;
			}
			DestroyComponent(controlItem);
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

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		DesignTimeSelectionChanged(sender as ISelectionService);
	}

	private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
	{
		ActiveDesignerChanged(e);
	}

	protected virtual void ActiveDesignerChanged(ActiveDesignerEventArgs e)
	{
		ClosePopups();
	}

	protected virtual void ClosePopups()
	{
		GetIOwner()?.OnApplicationDeactivate();
	}

	protected virtual IOwner GetIOwner()
	{
		return GetItemContainerControl() as IOwner;
	}

	protected virtual IOwnerMenuSupport GetIOwnerMenuSupport()
	{
		return GetItemContainerControl() as IOwnerMenuSupport;
	}

	protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
	{
	}

	private bool IsItemContained(BaseItem item)
	{
		if (item.ContainerControl == ((ControlDesigner)this).Control)
		{
			return true;
		}
		for (BaseItem parent = item.Parent; parent != null; parent = parent.Parent)
		{
			if (parent.ContainerControl == ((ControlDesigner)this).Control)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void DesignTimeSelectionChanged(ISelectionService ss)
	{
		if (ss == null || ((ControlDesigner)this).Control == null || ((ControlDesigner)this).Control.IsDisposed || IsDockableWindow)
		{
			return;
		}
		if (ss.PrimarySelection != ((ControlDesigner)this).Control)
		{
			BaseItem itemContainer = GetItemContainer();
			if (itemContainer == null)
			{
				return;
			}
			if (ss.PrimarySelection is BaseItem)
			{
				BaseItem baseItem = ss.PrimarySelection as BaseItem;
				if (IsItemContained(baseItem))
				{
					IsSelected = true;
				}
				else
				{
					IsSelected = false;
				}
				if (baseItem.ContainerControl == GetItemContainerControl())
				{
					if ((!baseItem.IsOnMenu || (baseItem.IsOnBar && ((Bar)baseItem.ContainerControl).BarState == eBarState.Popup)) && (!baseItem.Expanded || !(baseItem is PopupItem)) && (!(baseItem is GalleryContainer) || !((GalleryContainer)baseItem).PopupGalleryItem.Expanded))
					{
						GetIOwnerMenuSupport().ClosePopups();
					}
					return;
				}
				if (GetAllAssociatedComponents().Contains(baseItem))
				{
					if (!baseItem.IsOnMenu && (!baseItem.IsOnBar || ((Bar)baseItem.ContainerControl).BarState != 0))
					{
						GetIOwnerMenuSupport().ClosePopups();
					}
					return;
				}
				IOwner iOwner = GetIOwner();
				if (iOwner != null && iOwner != baseItem.GetOwner())
				{
					iOwner.SetFocusItem(null);
					iOwner.OnApplicationDeactivate();
				}
			}
			else
			{
				IOwner iOwner2 = GetIOwner();
				if (iOwner2 != null)
				{
					iOwner2.SetFocusItem(null);
					iOwner2.OnApplicationDeactivate();
				}
				IsSelected = false;
			}
		}
		else
		{
			GetIOwner()?.OnApplicationDeactivate();
			IsSelected = true;
		}
	}

	protected virtual BaseItem GetDefaultNewItemContainer()
	{
		return GetItemContainer();
	}

	protected virtual BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is BarBaseControl barBaseControl)
		{
			return barBaseControl.GetBaseItemContainer();
		}
		if (((ControlDesigner)this).Control is ItemControl)
		{
			return ((ItemControl)(object)((ControlDesigner)this).Control).GetBaseItemContainer();
		}
		return null;
	}

	protected virtual Control GetItemContainerControl()
	{
		return ((ControlDesigner)this).Control;
	}

	private void OnComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component == ((ComponentDesigner)this).Component)
		{
			ThisComponentRemoving(sender, e);
		}
		else
		{
			OtherComponentRemoving(sender, e);
		}
	}

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		ComponentRemoved(sender, e);
	}

	protected virtual void ComponentRemoved(object sender, ComponentEventArgs e)
	{
	}

	protected virtual void ThisComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (m_InternalRemoving)
		{
			return;
		}
		m_InternalRemoving = true;
		try
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving -= OnComponentRemoving;
			}
			BaseItem itemContainer = GetItemContainer();
			IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			if (designerHost == null || itemContainer == null)
			{
				return;
			}
			for (int num = itemContainer.SubItems.Count - 1; num >= 0; num--)
			{
				BaseItem baseItem = itemContainer.SubItems[num];
				if (baseItem.Parent == itemContainer)
				{
					DestroySubItems(baseItem, designerHost);
					designerHost.DestroyComponent(baseItem);
				}
			}
		}
		finally
		{
			m_InternalRemoving = false;
		}
	}

	protected virtual void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (!(e.Component is BaseItem))
		{
			return;
		}
		BaseItem baseItem = e.Component as BaseItem;
		if (baseItem.ContainerControl == GetItemContainerControl())
		{
			if (baseItem.Parent != null && baseItem.Parent.SubItems.Contains(baseItem))
			{
				baseItem.Parent.SubItems.Remove(baseItem);
			}
			if (baseItem != null)
			{
				DestroySubItems(baseItem);
			}
			RecalcLayout();
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

	protected override void WndProc(ref Message m)
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (m_DesignerHost == null)
		{
			m_DesignerHost = Control.FromHandle(((Message)(ref m)).HWnd);
		}
		BaseItem itemContainer = GetItemContainer();
		Control itemContainerControl = GetItemContainerControl();
		if (itemContainer == null || itemContainerControl == null || itemContainerControl.IsDisposed)
		{
			((ControlDesigner)this).WndProc(ref m);
			return;
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
			if (OnMouseUp(ref m))
			{
				return;
			}
			break;
		case 512:
			if (OnMouseMove(ref m))
			{
				((Message)(ref m)).Result = IntPtr.Zero;
				return;
			}
			break;
		case 675:
			if (OnMouseLeave(ref m))
			{
				return;
			}
			break;
		case 515:
			if (OnMouseDoubleClick(m))
			{
				return;
			}
			break;
		case 276:
		case 277:
			((ControlDesigner)this).WndProc(ref m);
			RecalcLayout();
			return;
		}
		((ControlDesigner)this).WndProc(ref m);
	}

	protected virtual bool OnMouseDown(ref Message m, MouseButtons button)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (IsDockableWindow)
		{
			return false;
		}
		Control itemContainerControl = GetItemContainerControl();
		BaseItem itemContainer = GetItemContainer();
		IOwner iOwner = GetIOwner();
		if (itemContainerControl == null || iOwner == null || itemContainer == null)
		{
			return false;
		}
		if (((Message)(ref m)).Msg == 516)
		{
			Point designTimeMouseDownPoint = itemContainerControl.PointToClient(Control.MousePosition);
			MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 0, designTimeMouseDownPoint.X, designTimeMouseDownPoint.Y, 0);
			itemContainer.InternalMouseDown(objArg);
			BaseItem focusItem = iOwner.GetFocusItem();
			if (focusItem != null)
			{
				ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					ArrayList arrayList = new ArrayList(1);
					arrayList.Add(focusItem);
					selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
					OnItemSelected(focusItem);
					if (arrayList[0] is PopupItem && ((PopupItem)arrayList[0]).SubItems.Count > 0 && !((PopupItem)arrayList[0]).Expanded)
					{
						((PopupItem)arrayList[0]).Expanded = true;
					}
					focusItem.DesignTimeMouseDownPoint = designTimeMouseDownPoint;
					((ControlDesigner)this).OnContextMenu(Control.MousePosition.X, Control.MousePosition.Y);
					return true;
				}
			}
		}
		return false;
	}

	protected virtual bool OnMouseUp(ref Message m)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		bool result = false;
		if (m_Capture)
		{
			m_Capture = false;
			Control val = Control.FromHandle(((Message)(ref m)).HWnd);
			if (val != null)
			{
				val.Capture = false;
			}
		}
		Control itemContainerControl = GetItemContainerControl();
		BaseItem itemContainer = GetItemContainer();
		IOwner iOwner = GetIOwner();
		if (itemContainerControl == null || iOwner == null || itemContainer == null)
		{
			return false;
		}
		Point point = itemContainerControl.PointToClient(Control.MousePosition);
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
		itemContainer.InternalMouseUp(objArg);
		if (m_MouseDownSelectionPerformed)
		{
			result = true;
		}
		m_MouseDownSelectionPerformed = false;
		if (m_DragItem != null && m_DragItem is ControlContainerItem && ((ControlContainerItem)m_DragItem).Control != null)
		{
			MouseDragDrop(point.X, point.Y);
		}
		return result;
	}

	protected virtual bool OnMouseMove(ref Message m)
	{
		return false;
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (m_PassiveContainer)
		{
			((ParentControlDesigner)this).OnMouseDragBegin(x, y);
			return;
		}
		Control itemContainerControl = GetItemContainerControl();
		Control itemContainerControl2 = GetItemContainerControl();
		IOwner iOwner = GetIOwner();
		BaseItem itemContainer = GetItemContainer();
		Point point = itemContainerControl2.PointToClient(new Point(x, y));
		MouseEventArgs objArg = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
		BaseItem baseItem = null;
		iOwner.GetFocusItem();
		itemContainer.InternalMouseDown(objArg);
		baseItem = iOwner.GetFocusItem();
		if (baseItem != null)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(baseItem);
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
				OnItemSelected(iOwner.GetFocusItem());
			}
		}
		if (itemContainerControl == null || baseItem == null || IsDockableWindow || !m_EnableItemDragDrop || !CanDragItem(baseItem))
		{
			if (baseItem == null)
			{
				((ParentControlDesigner)this).OnMouseDragBegin(x, y);
			}
			else
			{
				((ControlDesigner)this).Control.Capture = true;
			}
			return;
		}
		itemContainerControl.Capture = true;
		if (IsDockableWindow)
		{
			WinApi.RECT r = new WinApi.RECT(0, 0, 0, 0);
			WinApi.GetWindowRect(itemContainerControl.Handle, ref r);
			Rectangle clip = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
			Cursor.Clip = clip;
		}
		StartItemDrag(baseItem);
		((ControlDesigner)this).Control.Capture = true;
	}

	protected virtual bool CanDragItem(BaseItem item)
	{
		if (item is ControlContainerItem && ((ControlContainerItem)item).Control != null)
		{
			return false;
		}
		return true;
	}

	protected virtual Point GetClientPoint(int x, int y)
	{
		return ((ControlDesigner)this).Control.PointToClient(new Point(x, y));
	}

	protected override void OnMouseDragMove(int x, int y)
	{
		if (m_DragInProgress)
		{
			Point clientPoint = GetClientPoint(x, y);
			MouseDragOver(clientPoint.X, clientPoint.Y);
		}
	}

	protected override void OnMouseDragEnd(bool cancel)
	{
		if (!IsDockableWindow && !m_PassiveContainer)
		{
			((ControlDesigner)this).Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			if (m_DragInProgress)
			{
				if (cancel)
				{
					MouseDragDrop(-1, -1);
				}
				else
				{
					Point point = ((ControlDesigner)this).Control.PointToClient(Control.MousePosition);
					MouseDragDrop(point.X, point.Y);
				}
				cancel = true;
			}
			else
			{
				GetItemContainerControl();
				IOwner iOwner = GetIOwner();
				if (iOwner != null && iOwner.GetFocusItem() != null)
				{
					cancel = true;
				}
			}
		}
		((ParentControlDesigner)this).OnMouseDragEnd(cancel);
	}

	protected virtual bool OnMouseLeave(ref Message m)
	{
		return false;
	}

	protected virtual bool OnMouseDoubleClick(Message m)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		bool result = false;
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null && selectionService.PrimarySelection == ((ControlDesigner)this).Control)
		{
			Point point = ((ControlDesigner)this).Control.PointToClient(Control.MousePosition);
			IOwner iOwner = GetIOwner();
			BaseItem itemContainer = GetItemContainer();
			itemContainer.InternalMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
			BaseItem focusItem = iOwner.GetFocusItem();
			if (focusItem != null)
			{
				ArrayList arrayList = new ArrayList(1);
				arrayList.Add(focusItem);
				selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
				selectionService.SetSelectedComponents(arrayList, SelectionTypes.Click);
			}
		}
		if (selectionService != null && selectionService.PrimarySelection is ButtonItem && ((ButtonItem)selectionService.PrimarySelection).ContainerControl == GetItemContainerControl())
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

	protected virtual void OnItemSelected(BaseItem item)
	{
	}

	protected virtual void RecalcLayout()
	{
		Control itemContainerControl = GetItemContainerControl();
		if (itemContainerControl is BarBaseControl)
		{
			((BarBaseControl)(object)itemContainerControl).RecalcLayout();
		}
		else if (itemContainerControl is ItemControl)
		{
			((ItemControl)(object)itemContainerControl).RecalcLayout();
		}
		else if (itemContainerControl is ButtonX)
		{
			((ButtonX)(object)itemContainerControl).RecalcLayout();
		}
	}

	protected virtual void SetComponentSelected(bool selected)
	{
		if (GetItemContainerControl() is ItemControl)
		{
			((ItemControl)(object)GetItemContainerControl()).DesignerSelection = selected;
		}
		else if (GetItemContainerControl() is Bar)
		{
			((Bar)(object)GetItemContainerControl()).DesignerSelection = selected;
		}
		else if (GetItemContainerControl() is ExplorerBar)
		{
			((ExplorerBar)(object)GetItemContainerControl()).DesignerSelection = selected;
		}
	}

	protected virtual ArrayList GetAllAssociatedComponents()
	{
		ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
		BaseItem itemContainer = GetItemContainer();
		if (itemContainer != null)
		{
			AddSubItems(itemContainer, arrayList);
		}
		return arrayList;
	}

	protected override bool GetHitTest(Point pt)
	{
		if (((ControlDesigner)this).GetHitTest(pt))
		{
			return true;
		}
		Control control = ((ControlDesigner)this).Control;
		ScrollableControl val = (ScrollableControl)(object)((control is ScrollableControl) ? control : null);
		if (val != null && ((Control)val).IsHandleCreated && val.AutoScroll)
		{
			int num = WinApi.SendMessage(((Control)val).Handle, 132, 0, WinApi.MAKELPARAM(pt.X, pt.Y));
			if (num == 7 || num == 6)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void AddSubItems(BaseItem parent, ArrayList list)
	{
		foreach (BaseItem subItem in parent.SubItems)
		{
			if (subItem.DesignMode)
			{
				list.Add(subItem);
			}
			AddSubItems(subItem, list);
		}
	}

	protected virtual void CreateButton(object sender, EventArgs e)
	{
		CreateButton();
	}

	public virtual ButtonItem CreateButton()
	{
		OnSubItemsChanging();
		ButtonItem result = CreateButton(GetDefaultNewItemContainer());
		OnSubItemsChanged();
		return result;
	}

	protected virtual ButtonItem CreateButton(BaseItem parent)
	{
		if (parent == null)
		{
			return null;
		}
		ButtonItem buttonItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
			try
			{
				m_CreatingItem = true;
				buttonItem = designerHost.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				if (buttonItem == null)
				{
					return null;
				}
				TypeDescriptor.GetProperties(buttonItem)["Text"].SetValue(buttonItem, buttonItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(buttonItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(buttonItem);
			}
			catch
			{
				designerTransaction.Cancel();
			}
			finally
			{
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
				m_CreatingItem = false;
			}
		}
		return buttonItem;
	}

	protected virtual void CreateRatingItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateRatingItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateRatingItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(RatingItem)) is RatingItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
			m_CreatingItem = false;
		}
	}

	protected virtual void CreateCircularProgressItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateCircularProgressItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateCircularProgressItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(CircularProgressItem)) is CircularProgressItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
			m_CreatingItem = false;
		}
	}

	protected virtual void CreateCheckBox(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateCheckBox(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateCheckBox(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(CheckBoxItem)) is CheckBoxItem checkBoxItem)
			{
				TypeDescriptor.GetProperties(checkBoxItem)["Text"].SetValue(checkBoxItem, checkBoxItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(checkBoxItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(checkBoxItem);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
			m_CreatingItem = false;
		}
	}

	protected virtual void CreateTextBox(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateTextBox(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateTextBox(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(TextBoxItem)) is TextBoxItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
			m_CreatingItem = false;
		}
	}

	protected virtual void CreateComboBox(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateComboBox(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateComboBox(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ComboBoxItem)) is ComboBoxItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateLabel(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateLabel(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual LabelItem CreateLabel(BaseItem parent)
	{
		if (parent == null)
		{
			return null;
		}
		LabelItem labelItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
			try
			{
				m_CreatingItem = true;
				labelItem = designerHost.CreateComponent(typeof(LabelItem)) as LabelItem;
				if (labelItem == null)
				{
					return null;
				}
				TypeDescriptor.GetProperties(labelItem)["Text"].SetValue(labelItem, labelItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(labelItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(labelItem);
			}
			catch
			{
				designerTransaction.Cancel();
				throw;
			}
			finally
			{
				m_CreatingItem = false;
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
			}
		}
		return labelItem;
	}

	protected virtual void CreateColorPicker(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateColorPicker(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateColorPicker(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ColorPickerDropDown)) is ColorPickerDropDown colorPickerDropDown)
			{
				TypeDescriptor.GetProperties(colorPickerDropDown)["Text"].SetValue(colorPickerDropDown, colorPickerDropDown.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(colorPickerDropDown);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(colorPickerDropDown);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateContainer(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateContainer(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateContainer(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ItemContainer)) is ItemContainer item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateProgressBar(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateProgressBar(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateProgressBar(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ProgressBarItem)) is ProgressBarItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateSwitch(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateSwitch(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateSwitch(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(SwitchButtonItem)) is SwitchButtonItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateMicroChart(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateMicroChart(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateMicroChart(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(MicroChartItem)) is MicroChartItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateDocument(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			DockContainerItem dockContainerItem = designerHost.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
			dockContainerItem.Text = dockContainerItem.Name;
			OnSubItemsChanging();
			GetDefaultNewItemContainer().SubItems.Add(dockContainerItem);
			PanelDockContainer panelDockContainer = designerHost.CreateComponent(typeof(PanelDockContainer)) as PanelDockContainer;
			((ControlDesigner)this).Control.Controls.Add((Control)(object)panelDockContainer);
			panelDockContainer.ColorSchemeStyle = InternalStyle;
			panelDockContainer.ApplyLabelStyle();
			dockContainerItem.Control = (Control)(object)panelDockContainer;
			OnSubItemsChanged();
			RecalcLayout();
			if (((ControlDesigner)this).Control is Bar)
			{
				Bar bar = ((ControlDesigner)this).Control as Bar;
				bar.SelectedDockTab = bar.Items.IndexOf(dockContainerItem);
			}
			((Control)panelDockContainer).BringToFront();
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateMetroTile(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateMetroTile(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateMetroTile(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(MetroTileItem)) is MetroTileItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateControlContainer(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateControlContainer(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateControlContainer(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ControlContainerItem)) is ControlContainerItem controlContainerItem)
			{
				controlContainerItem.AllowItemResize = false;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(controlContainerItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(controlContainerItem);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateGalleryContainer(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateGalleryContainer(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateGalleryContainer(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(GalleryContainer)) is GalleryContainer item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateCustomizeItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateCustomizeItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateCustomizeItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(CustomizeItem)) is CustomizeItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateMdiWindowList(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateMdiWindowList(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateMdiWindowList(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(MdiWindowListItem)) is MdiWindowListItem item)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(item);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(item);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateSliderItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateSliderItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateRangeSliderItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateRangeSliderItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateRangeSliderItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(RangeSliderItem)) is RangeSliderItem rangeSliderItem)
			{
				TypeDescriptor.GetProperties(rangeSliderItem)["Text"].SetValue(rangeSliderItem, rangeSliderItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(rangeSliderItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(rangeSliderItem);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void CreateSliderItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(SliderItem)) is SliderItem sliderItem)
			{
				TypeDescriptor.GetProperties(sliderItem)["Text"].SetValue(sliderItem, sliderItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(sliderItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(sliderItem);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void OnitemCreated(BaseItem item)
	{
	}

	protected virtual void OnSubItemsChanging()
	{
	}

	protected virtual void OnSubItemsChanged()
	{
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
		else if (m_DragInProgress && m_DragLeave)
		{
			ControlContainerItem controlContainerItem = m_DragItem as ControlContainerItem;
			MouseDragDrop(-1, -1);
			if (controlContainerItem != null)
			{
				controlContainerItem.Control = null;
				DestroyComponent(controlContainerItem);
			}
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		if (m_DragInProgress && m_AcceptExternalControls && !IsDockableWindow)
		{
			MouseDragDrop(-1, -1);
		}
		((ParentControlDesigner)this).OnDragLeave(e);
	}

	protected override void OnDragOver(DragEventArgs de)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		if (m_DragInProgress)
		{
			Point point = ((ControlDesigner)this).Control.PointToClient(new Point(de.X, de.Y));
			MouseDragOver(point.X, point.Y);
			de.Effect = (DragDropEffects)2;
			return;
		}
		if (m_EnableItemDragDrop && m_AcceptExternalControls && !IsDockableWindow)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null && selectionService.PrimarySelection != ((ComponentDesigner)this).Component)
			{
				if (selectionService.PrimarySelection is Control && ((ControlDesigner)this).Control.Controls.Contains((Control)selectionService.PrimarySelection))
				{
					object primarySelection = selectionService.PrimarySelection;
					BaseItem controlItem = GetControlItem((Control)((primarySelection is Control) ? primarySelection : null));
					if (controlItem != null)
					{
						m_MouseDownPoint = ((ControlDesigner)this).Control.PointToClient(new Point(de.X, de.Y));
						m_SuspendInternalCursor = true;
						StartItemDrag(controlItem);
						if (m_TimerDragDrop == null)
						{
							m_TimerDragDrop = new Timer();
							m_TimerDragDrop.Tick += TimerTickDragDrop;
							m_TimerDragDrop.Interval = 100;
							m_TimerDragDrop.Enabled = true;
							m_TimerDragDrop.Start();
						}
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
					BaseItem baseItem = null;
					if (IsDockableWindow)
					{
						DockContainerItem dockContainerItem = new DockContainerItem();
						dockContainerItem.Name = "tempDragDropItem";
						baseItem = dockContainerItem;
					}
					else
					{
						ControlContainerItem controlContainerItem = new ControlContainerItem();
						controlContainerItem.AllowItemResize = false;
						controlContainerItem.Name = "tempDragDropItem";
						baseItem = controlContainerItem;
					}
					m_MouseDownPoint = ((ControlDesigner)this).Control.PointToClient(new Point(de.X, de.Y));
					m_SuspendInternalCursor = true;
					StartItemDrag(baseItem);
					if (m_TimerDragDrop == null)
					{
						m_TimerDragDrop = new Timer();
						m_TimerDragDrop.Tick += TimerTickDragDrop;
						m_TimerDragDrop.Interval = 100;
						m_TimerDragDrop.Enabled = true;
						m_TimerDragDrop.Start();
					}
				}
			}
		}
		((ParentControlDesigner)this).OnDragOver(de);
	}

	protected override void OnDragDrop(DragEventArgs de)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (m_EnableItemDragDrop && m_AcceptExternalControls && !IsDockableWindow)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null && selectionService.PrimarySelection is Control && ((ControlDesigner)this).Control.Controls.Contains((Control)selectionService.PrimarySelection))
			{
				de.Effect = (DragDropEffects)2;
				Point point = ((ControlDesigner)this).Control.PointToClient(new Point(de.X, de.Y));
				MouseDragDrop(point.X, point.Y);
			}
			else
			{
				if (selectionService.SelectionCount > 1)
				{
					de.Effect = (DragDropEffects)0;
					return;
				}
				if (m_DragItem != null && m_DragItem.Name == "tempDragDropItem" && (m_DragItem is ControlContainerItem || m_DragItem is DockContainerItem))
				{
					m_JustAdded = DateTime.Now;
					BaseItem dragItem = null;
					if (m_DragItem is ControlContainerItem)
					{
						ControlContainerItem controlContainerItem = m_DragItem as ControlContainerItem;
						controlContainerItem.Control = null;
						controlContainerItem = CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
						PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(controlContainerItem)["Control"];
						ControlContainerItem component = controlContainerItem;
						object primarySelection = selectionService.PrimarySelection;
						propertyDescriptor.SetValue(component, (primarySelection is Control) ? primarySelection : null);
						TypeDescriptor.GetProperties(controlContainerItem)["AllowItemResize"].SetValue(controlContainerItem, false);
						dragItem = controlContainerItem;
					}
					else if (m_DragItem is DockContainerItem)
					{
						DockContainerItem dockContainerItem = m_DragItem as DockContainerItem;
						dockContainerItem.Control = null;
						dockContainerItem = CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
						PropertyDescriptor? propertyDescriptor2 = TypeDescriptor.GetProperties(dockContainerItem)["Control"];
						DockContainerItem component2 = dockContainerItem;
						object primarySelection2 = selectionService.PrimarySelection;
						propertyDescriptor2.SetValue(component2, (primarySelection2 is Control) ? primarySelection2 : null);
						dragItem = dockContainerItem;
					}
					m_DragItem = dragItem;
					Point point2 = ((ControlDesigner)this).Control.PointToClient(new Point(de.X, de.Y));
					MouseDragDrop(point2.X, point2.Y);
				}
				m_NewControlAdded = true;
			}
		}
		((ParentControlDesigner)this).OnDragDrop(de);
	}

	protected virtual BaseItem GetControlItem(Control control)
	{
		BaseItem itemContainer = GetItemContainer();
		if (itemContainer == null)
		{
			return null;
		}
		return GetControlItem(control, itemContainer);
	}

	internal BaseItem GetControlItem(Control control, BaseItem parent)
	{
		if (parent is ControlContainerItem && ((ControlContainerItem)parent).Control == control)
		{
			return parent;
		}
		if (parent is DockContainerItem && ((DockContainerItem)parent).Control == control)
		{
			return parent;
		}
		foreach (BaseItem subItem in parent.SubItems)
		{
			BaseItem controlItem = GetControlItem(control, subItem);
			if (controlItem != null)
			{
				return controlItem;
			}
		}
		return null;
	}

	protected void StartItemDrag(BaseItem item)
	{
		if (item != null && m_DragItem == null)
		{
			m_DragItem = item;
			if (!m_SuspendInternalCursor)
			{
				Cursor.Current = Cursors.Hand;
			}
			DragInProgress = true;
		}
	}

	protected void MouseDragOver(int x, int y)
	{
		if (!m_DragInProgress)
		{
			return;
		}
		BaseItem dragItem = m_DragItem;
		BaseItem itemContainer = GetItemContainer();
		if (m_DesignTimeProvider != null)
		{
			m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			m_DesignTimeProvider = null;
		}
		Point pScreen = ((ControlDesigner)this).Control.PointToScreen(new Point(x, y));
		GetItemContainerControl();
		InsertPosition insertPosition = ((IDesignTimeProvider)itemContainer).GetInsertPosition(pScreen, dragItem);
		if (insertPosition != null)
		{
			if (insertPosition.TargetProvider == null)
			{
				if (!m_SuspendInternalCursor)
				{
					Cursor.Current = Cursors.No;
				}
				return;
			}
			insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
			m_InsertPosition = insertPosition.Position;
			m_InsertBefore = insertPosition.Before;
			m_DesignTimeProvider = insertPosition.TargetProvider;
			if (!m_SuspendInternalCursor)
			{
				Cursor.Current = Cursors.Hand;
			}
		}
		else if (!m_SuspendInternalCursor)
		{
			Cursor.Current = Cursors.No;
		}
	}

	protected virtual void OnItemDragAndDropped(BaseItem dragItem)
	{
		((IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService)))?.OnComponentChanged(dragItem.Parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
	}

	internal void MouseDragDrop(int x, int y)
	{
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		if (!m_DragInProgress)
		{
			return;
		}
		if (m_DesignerHost != null && m_DesignerHost.Capture)
		{
			m_DesignerHost.Capture = false;
		}
		BaseItem dragItem = m_DragItem;
		BaseItem itemContainer = GetItemContainer();
		bool flag = false;
		if (m_DesignTimeProvider != null)
		{
			if (x == -1 && y == -1)
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
			}
			else
			{
				m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
				if (dragItem != null)
				{
					m_InternalRemoveInProgress = true;
					try
					{
						BaseItem parent = dragItem.Parent;
						if (parent != null)
						{
							if (parent == (BaseItem)m_DesignTimeProvider && m_InsertPosition > 0 && parent.SubItems.IndexOf(dragItem) < m_InsertPosition)
							{
								m_InsertPosition--;
							}
							parent.SubItems.Remove(dragItem);
						}
						m_DesignTimeProvider.InsertItemAt(dragItem, m_InsertPosition, m_InsertBefore);
						m_DesignTimeProvider = null;
						flag = true;
						if (dragItem.Parent != null && dragItem.Parent != GetItemContainer())
						{
							OnItemDragAndDropped(dragItem);
						}
					}
					finally
					{
						m_InternalRemoveInProgress = false;
					}
				}
			}
		}
		else if (x != -1 && y != -1 && dragItem is ControlContainerItem && ((ControlContainerItem)dragItem).Control != null)
		{
			BaseItem baseItem = itemContainer;
			if (dragItem.Parent != null)
			{
				dragItem.Parent.SubItems.Remove(dragItem);
			}
			baseItem.SubItems.Add(dragItem);
			OnItemDragAndDropped(dragItem);
			flag = true;
		}
		m_DesignTimeProvider = null;
		DragInProgress = false;
		if (!m_SuspendInternalCursor)
		{
			Cursor.Current = Cursors.Default;
		}
		if (dragItem != null)
		{
			dragItem._IgnoreClick = true;
		}
		itemContainer.InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (dragItem != null)
		{
			dragItem._IgnoreClick = false;
		}
		m_DragItem = null;
		RecalcLayout();
		if (flag)
		{
			((IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService)))?.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
		}
	}

	public void StartExternalDrag(BaseItem item)
	{
		if (!m_DragInProgress && m_DesignerHost != null)
		{
			m_SuspendInternalCursor = false;
			m_MouseDownPoint = ((ControlDesigner)this).Control.PointToClient(Control.MousePosition);
			StartItemDrag(item);
			m_DesignerHost.Capture = true;
		}
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

	public object CreateComponent(Type componentClass)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		object obj = null;
		try
		{
			m_CreatingItem = true;
			return designerHost.CreateComponent(componentClass);
		}
		finally
		{
			m_CreatingItem = false;
		}
	}

	public object CreateComponent(Type componentClass, string name)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		object obj = null;
		try
		{
			m_CreatingItem = true;
			return designerHost.CreateComponent(componentClass, name);
		}
		finally
		{
			m_CreatingItem = false;
		}
	}

	public void DestroyComponent(IComponent c)
	{
		((IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost)))?.DestroyComponent(c);
	}

	object IDesignerServices.GetService(Type serviceType)
	{
		return ((ComponentDesigner)this).GetService(serviceType);
	}

	private void DesignerSelectionChanged(object sender, EventArgs e)
	{
		if (!(((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService))
		{
			SetComponentSelected(selected: false);
			return;
		}
		bool componentSelected = false;
		if (selectionService.PrimarySelection == ((ControlDesigner)this).Control || selectionService.PrimarySelection == GetItemContainerControl())
		{
			componentSelected = true;
		}
		else if (selectionService.PrimarySelection is BaseItem && ((BaseItem)selectionService.PrimarySelection).ContainerControl == GetItemContainerControl())
		{
			componentSelected = true;
		}
		SetComponentSelected(componentSelected);
	}
}
