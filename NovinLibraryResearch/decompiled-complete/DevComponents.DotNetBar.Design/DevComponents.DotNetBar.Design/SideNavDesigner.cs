using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class SideNavDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[3]
			{
				new DesignerVerb("Create Tab", CreateSideNavTab),
				new DesignerVerb("Create Button", CreateButton),
				new DesignerVerb("Create Separator", CreateSeparator)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public SideNavDesigner()
	{
		EnableItemDragDrop = true;
		AcceptExternalControls = false;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode)
		{
			if (component is SideNav sideNav)
			{
				sideNav.SetDesignMode();
			}
			((ControlDesigner)this).EnableDragDrop(false);
		}
	}

	public override bool CanParent(Control control)
	{
		if (control is SideNavPanel && !((ControlDesigner)this).Control.Controls.Contains(control))
		{
			return true;
		}
		return false;
	}

	private void CreateSeparator(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateSeparator(GetNewItemContainer());
		}
	}

	private void CreateSeparator(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		Separator separator = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(Separator)) is Separator separator2)
			{
				TypeDescriptor.GetProperties(separator2)["FixedSize"].SetValue(separator2, new Size(3, 1));
				TypeDescriptor.GetProperties(separator2)["SeparatorOrientation"].SetValue(separator2, eDesignMarkerOrientation.Vertical);
				separator2.Padding.Left = 6;
				separator2.Padding.Right = 6;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(separator2);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(separator2);
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

	private BaseItem GetNewItemContainer()
	{
		SideNav sideNav = ((ControlDesigner)this).Control as SideNav;
		return sideNav.SideNavStrip.GetBaseItemContainer();
	}

	protected override void CreateButton(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateSideNavButton(GetNewItemContainer());
		}
	}

	protected virtual SideNavItem CreateSideNavButton(BaseItem parent)
	{
		if (parent == null)
		{
			return null;
		}
		SideNavItem sideNavItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
			try
			{
				m_CreatingItem = true;
				sideNavItem = designerHost.CreateComponent(typeof(SideNavItem)) as SideNavItem;
				if (sideNavItem == null)
				{
					return null;
				}
				TypeDescriptor.GetProperties(sideNavItem)["Text"].SetValue(sideNavItem, sideNavItem.Name);
				TypeDescriptor.GetProperties(sideNavItem)["Symbol"].SetValue(sideNavItem, "\uf10c");
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(sideNavItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(sideNavItem);
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
		return sideNavItem;
	}

	protected override void CreateTextBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateTextBox(GetNewItemContainer());
		}
	}

	protected override void CreateComboBox(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateComboBox(GetNewItemContainer());
		}
	}

	protected override void CreateLabel(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateLabel(GetNewItemContainer());
		}
	}

	protected override void CreateColorPicker(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateColorPicker(GetNewItemContainer());
		}
	}

	protected override void CreateProgressBar(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateProgressBar(GetNewItemContainer());
		}
	}

	protected override void CreateRatingItem(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateRatingItem(GetNewItemContainer());
		}
	}

	protected override void CreateSwitch(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav)
		{
			CreateSwitch(GetNewItemContainer());
		}
	}

	protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (m_AddingItem)
		{
			m_AddingItem = false;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ControlDesigner)this).Control, null);
			GetNewItemContainer().SubItems.Add(e.Component as BaseItem);
			componentChangeService?.OnComponentChanged(((ControlDesigner)this).Control, null, null, null);
			m_InsertItemTransaction.Commit();
			m_InsertItemTransaction = null;
			RecalcLayout();
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		CreateSideNavTab("Menu", isSysMenu: true);
		CreateSeparator(GetNewItemContainer());
		CreateSideNavTab("Home");
		SideNav sideNav = ((ControlDesigner)this).Control as SideNav;
		((Control)sideNav).Size = new Size(400, 200);
		((Control)sideNav).Dock = (DockStyle)3;
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is SideNav sideNav)
		{
			return sideNav.SideNavStrip.GetBaseItemContainer();
		}
		return null;
	}

	protected override Control GetItemContainerControl()
	{
		if (((ControlDesigner)this).Control is SideNav sideNav)
		{
			return (Control)(object)sideNav.SideNavStrip;
		}
		return null;
	}

	protected override void DesignTimeSelectionChanged(ISelectionService ss)
	{
		if (ss == null || ((ControlDesigner)this).Control == null || ((ControlDesigner)this).Control.IsDisposed)
		{
			return;
		}
		base.DesignTimeSelectionChanged(ss);
		if (!(((ControlDesigner)this).Control is SideNav sideNav))
		{
			return;
		}
		BaseItem baseItemContainer = sideNav.SideNavStrip.GetBaseItemContainer();
		if (baseItemContainer != null && ss.PrimarySelection is SideNavItem)
		{
			SideNavItem sideNavItem = ss.PrimarySelection as SideNavItem;
			if (baseItemContainer.SubItems.Contains(sideNavItem) && !sideNavItem.IsSystemMenu && sideNavItem.Panel != null)
			{
				TypeDescriptor.GetProperties(sideNavItem)["Checked"].SetValue(sideNavItem, true);
			}
		}
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (((ControlDesigner)this).Control is SideNav sideNav)
		{
			BaseItem baseItemContainer = sideNav.SideNavStrip.GetBaseItemContainer();
			if (e.Component is SideNavItem && baseItemContainer != null && baseItemContainer.SubItems != null && baseItemContainer.SubItems.Contains((SideNavItem)e.Component) && ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost && ((SideNavItem)e.Component).Panel != null)
			{
				designerHost.DestroyComponent((IComponent)((SideNavItem)e.Component).Panel);
			}
		}
		base.OtherComponentRemoving(sender, e);
	}

	protected override void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		SideNav sideNav = ((ControlDesigner)this).Control as SideNav;
		if (e.Component is BaseItem && sideNav != null)
		{
			BaseItem baseItem = e.Component as BaseItem;
			if (sideNav.Items.Contains(baseItem))
			{
				sideNav.Items.Remove(baseItem);
			}
			DestroySubItems(baseItem);
		}
	}

	protected virtual void CreateSideNavTab(object sender, EventArgs e)
	{
		CreateSideNavTab(string.Empty);
	}

	private void CreateSideNavTab(string text)
	{
		CreateSideNavTab(text, isSysMenu: false);
	}

	private void CreateSideNavTab(string text, bool isSysMenu)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating SideNav Tab");
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		SideNav sideNav = ((ControlDesigner)this).Control as SideNav;
		try
		{
			m_CreatingItem = true;
			OnSubItemsChanging();
			SideNavItem sideNavItem = designerHost.CreateComponent(typeof(SideNavItem)) as SideNavItem;
			if (string.IsNullOrEmpty(text))
			{
				TypeDescriptor.GetProperties(sideNavItem)["Text"].SetValue(sideNavItem, sideNavItem.Name);
			}
			else
			{
				TypeDescriptor.GetProperties(sideNavItem)["Text"].SetValue(sideNavItem, text);
			}
			if (isSysMenu)
			{
				TypeDescriptor.GetProperties(sideNavItem)["Symbol"].SetValue(sideNavItem, "\uf0c9");
				TypeDescriptor.GetProperties(sideNavItem)["IsSystemMenu"].SetValue(sideNavItem, true);
			}
			else
			{
				TypeDescriptor.GetProperties(sideNavItem)["Symbol"].SetValue(sideNavItem, "\uf015");
				SideNavPanel sideNavPanel = designerHost.CreateComponent(typeof(SideNavPanel)) as SideNavPanel;
				TypeDescriptor.GetProperties(sideNavPanel)["Dock"].SetValue(sideNavPanel, (object)(DockStyle)5);
				componentChangeService.OnComponentChanging(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
				((ControlDesigner)this).Control.Controls.Add((Control)(object)sideNavPanel);
				((Control)sideNavPanel).SendToBack();
				componentChangeService.OnComponentChanged(((ControlDesigner)this).Control, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
				TypeDescriptor.GetProperties(sideNavItem)["Panel"].SetValue(sideNavItem, sideNavPanel);
			}
			SimpleItemContainer simpleItemContainer = (SimpleItemContainer)sideNav.SideNavStrip.GetBaseItemContainer();
			componentChangeService.OnComponentChanging(simpleItemContainer, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
			simpleItemContainer.SubItems.Add(sideNavItem);
			componentChangeService.OnComponentChanged(simpleItemContainer, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
			if (sideNav.SelectedItem == null && !isSysMenu)
			{
				TypeDescriptor.GetProperties(sideNavItem)["Checked"].SetValue(sideNavItem, true);
			}
			RecalcLayout();
			OnSubItemsChanged();
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
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
}
