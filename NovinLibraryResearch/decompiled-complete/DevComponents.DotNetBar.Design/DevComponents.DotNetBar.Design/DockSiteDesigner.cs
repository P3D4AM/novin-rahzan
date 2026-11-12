using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar.Design;

public class DockSiteDesigner : ParentControlDesigner
{
	private bool m_MouseDrag;

	internal bool DotNetBarManagerRemoving;

	public override SelectionRules SelectionRules => (SelectionRules)0;

	protected override void OnContextMenu(int x, int y)
	{
	}

	public override bool CanParent(Control control)
	{
		if (control is IDockInfo && !(control is ContextMenuBar))
		{
			return true;
		}
		return false;
	}

	public override void Initialize(IComponent component)
	{
		((ParentControlDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoving;
			}
			if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: not false } designerHost)
			{
				((DockSite)component)._DesignerLoading = true;
				designerHost.LoadComplete += DesignerHostLoadComplete;
			}
		}
	}

	private void DesignerHostLoadComplete(object sender, EventArgs e)
	{
		DockSite dockSite = (DockSite)(object)((ControlDesigner)this).Control;
		dockSite._DesignerLoading = false;
		if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
		{
			designerHost.LoadComplete -= DesignerHostLoadComplete;
		}
	}

	protected override void Dispose(bool disposing)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		((ParentControlDesigner)this).Dispose(disposing);
	}

	private void OnComponentRemoving(object sender, ComponentEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (e.Component != ((ComponentDesigner)this).Component)
		{
			return;
		}
		DockSite dockSite = ((ControlDesigner)this).Control as DockSite;
		if ((int)((Control)dockSite).Dock != 5 && !DotNetBarManagerRemoving)
		{
			throw new InvalidOperationException("DotNetBarManager dock sites must not be removed manually. Delete DotNetBarManager component to remove all dock sites.");
		}
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			Bar[] array = new Bar[((ArrangedElementCollection)((Control)dockSite).Controls).Count];
			((ArrangedElementCollection)((Control)dockSite).Controls).CopyTo((Array)array, 0);
			Bar[] array2 = array;
			foreach (Bar bar in array2)
			{
				if (bar != null)
				{
					designerHost.DestroyComponent((IComponent)bar);
				}
			}
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoving -= OnComponentRemoving;
		}
		if (dockSite != null && dockSite.Owner != null)
		{
			if (dockSite.Owner.FillDockSite == dockSite)
			{
				dockSite.Owner.FillDockSite = null;
			}
			else if (dockSite.Owner.LeftDockSite == dockSite)
			{
				dockSite.Owner.LeftDockSite = null;
			}
			else if (dockSite.Owner.RightDockSite == dockSite)
			{
				dockSite.Owner.RightDockSite = null;
			}
			else if (dockSite.Owner.TopDockSite == dockSite)
			{
				dockSite.Owner.TopDockSite = null;
			}
			else if (dockSite.Owner.BottomDockSite == dockSite)
			{
				dockSite.Owner.BottomDockSite = null;
			}
		}
	}

	protected override void OnSetCursor()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		DockSite dockSite = ((ControlDesigner)this).Control as DockSite;
		if ((int)((Control)dockSite).Dock != 5 && dockSite.DocumentDockContainer == null)
		{
			return;
		}
		Point mousePosition = Control.MousePosition;
		if (dockSite != null && ((ArrangedElementCollection)((Control)dockSite).Controls).Count != 0)
		{
			DocumentDockUIManager documentUIManager = dockSite.GetDocumentUIManager();
			Point point = ((Control)dockSite).PointToClient(mousePosition);
			Cursor cursor = documentUIManager.GetCursor(point.X, point.Y);
			if (cursor != (Cursor)null)
			{
				Cursor.Current = cursor;
			}
			else
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		DockSite dockSite = ((ControlDesigner)this).Control as DockSite;
		if (((int)((ControlDesigner)this).Control.Dock == 5 && dockSite != null) || dockSite.DocumentDockContainer != null)
		{
			DocumentDockUIManager documentUIManager = dockSite.GetDocumentUIManager();
			Point point = ((Control)dockSite).PointToClient(new Point(x, y));
			Cursor cursor = documentUIManager.GetCursor(point.X, point.Y);
			if (cursor != (Cursor)null)
			{
				documentUIManager.OnMouseDown(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
				m_MouseDrag = true;
			}
		}
		((ParentControlDesigner)this).OnMouseDragBegin(x, y);
		if (m_MouseDrag)
		{
			((Control)dockSite).Capture = true;
			if ((int)((Control)dockSite).Dock == 5 || dockSite.GetDocumentUIManager().IsResizingDocument)
			{
				WinApi.RECT r = new WinApi.RECT(0, 0, 0, 0);
				WinApi.GetWindowRect(((Control)dockSite).Handle, ref r);
				Rectangle clip = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
				Cursor.Clip = clip;
			}
		}
	}

	protected override void OnMouseDragMove(int x, int y)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		DockSite dockSite = ((ControlDesigner)this).Control as DockSite;
		if (m_MouseDrag && dockSite != null)
		{
			DocumentDockUIManager documentUIManager = dockSite.GetDocumentUIManager();
			Point point = ((Control)dockSite).PointToClient(new Point(x, y));
			documentUIManager.OnMouseMove(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
		}
		else
		{
			((ParentControlDesigner)this).OnMouseDragMove(x, y);
		}
	}

	protected override void OnMouseDragEnd(bool cancel)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		if (m_MouseDrag)
		{
			((ControlDesigner)this).Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
		}
		DockSite dockSite = ((ControlDesigner)this).Control as DockSite;
		if (m_MouseDrag && dockSite != null)
		{
			DocumentDockUIManager documentUIManager = dockSite.GetDocumentUIManager();
			Point point = ((Control)dockSite).PointToClient(Control.MousePosition);
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			componentChangeService?.OnComponentChanging(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
			documentUIManager.OnMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0));
			componentChangeService?.OnComponentChanged(dockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"], null, null);
		}
		m_MouseDrag = false;
		((ParentControlDesigner)this).OnMouseDragEnd(cancel);
	}
}
