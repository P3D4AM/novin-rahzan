using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ExplorerBarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[2]
			{
				new DesignerVerb("Create Group", OnAddGroup),
				new DesignerVerb("Create Button", OnAddButton)
			};
			return new DesignerVerbCollection(value);
		}
	}

	public ExplorerBarDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode && ((ControlDesigner)this).Control is ExplorerBar explorerBar)
		{
			explorerBar.SetDesignMode();
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is ExplorerBar)
		{
			ExplorerBar explorerBar = ((ControlDesigner)this).Control as ExplorerBar;
			explorerBar.ApplyDefaultSettings();
			explorerBar.StockStyle = eExplorerBarStockStyle.SystemColors;
		}
	}

	protected override BaseItem GetItemContainer()
	{
		if (((ControlDesigner)this).Control is ExplorerBar explorerBar)
		{
			return explorerBar.ItemsContainer;
		}
		return null;
	}

	protected override void RecalcLayout()
	{
		if (GetItemContainerControl() is ExplorerBar explorerBar)
		{
			explorerBar.RecalcLayout();
		}
	}

	protected override void OnSubItemsChanging()
	{
		base.OnSubItemsChanging();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			ExplorerBar component = GetItemContainerControl() as ExplorerBar;
			componentChangeService.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Groups", ignoreCase: true));
		}
	}

	protected override void OnSubItemsChanged()
	{
		base.OnSubItemsChanged();
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			ExplorerBar component = GetItemContainerControl() as ExplorerBar;
			componentChangeService.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(component).Find("Groups", ignoreCase: true), null, null);
		}
	}

	private void OnAddGroup(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			CreateGroup();
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
		}
	}

	private ExplorerBarGroupItem CreateGroup()
	{
		ExplorerBar explorerBar = ((ComponentDesigner)this).Component as ExplorerBar;
		ExplorerBarGroupItem explorerBarGroupItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (explorerBar != null && designerHost != null)
		{
			OnSubItemsChanging();
			try
			{
				m_CreatingItem = true;
				explorerBarGroupItem = designerHost.CreateComponent(typeof(ExplorerBarGroupItem)) as ExplorerBarGroupItem;
				if (explorerBarGroupItem == null)
				{
					return null;
				}
				explorerBarGroupItem.SetDefaultAppearance();
				explorerBarGroupItem.Text = "New Group";
				explorerBarGroupItem.Expanded = true;
				explorerBar.Groups.Add(explorerBarGroupItem);
				OnSubItemsChanged();
			}
			finally
			{
				m_CreatingItem = false;
			}
		}
		return explorerBarGroupItem;
	}

	private void OnAddButton(object sender, EventArgs e)
	{
		ExplorerBar explorerBar = ((ComponentDesigner)this).Component as ExplorerBar;
		ExplorerBarGroupItem explorerBarGroupItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			if (selectionService != null && selectionService.PrimarySelection is ExplorerBarGroupItem)
			{
				explorerBarGroupItem = selectionService.PrimarySelection as ExplorerBarGroupItem;
			}
			else if (explorerBar.Groups.Count > 0)
			{
				Point pt = ((Control)explorerBar).PointToClient(Control.MousePosition);
				if (((Control)explorerBar).Bounds.Contains(pt))
				{
					foreach (BaseItem group in explorerBar.Groups)
					{
						if (group.DisplayRectangle.Contains(pt))
						{
							explorerBarGroupItem = group as ExplorerBarGroupItem;
							break;
						}
					}
				}
				if (explorerBarGroupItem == null)
				{
					foreach (BaseItem group2 in explorerBar.Groups)
					{
						if (group2.Visible)
						{
							explorerBarGroupItem = group2 as ExplorerBarGroupItem;
							break;
						}
					}
				}
			}
			if (explorerBarGroupItem == null)
			{
				explorerBarGroupItem = CreateGroup();
			}
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(explorerBarGroupItem).Find("SubItems", ignoreCase: true));
			try
			{
				m_CreatingItem = true;
				if (!(designerHost.CreateComponent(typeof(ButtonItem)) is ButtonItem buttonItem))
				{
					return;
				}
				ExplorerBarGroupItem.SetDesignTimeDefaults(buttonItem, explorerBarGroupItem.StockStyle);
				explorerBarGroupItem.SubItems.Add(buttonItem);
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(explorerBarGroupItem).Find("SubItems", ignoreCase: true), null, null);
			}
			finally
			{
				m_CreatingItem = false;
			}
			RecalcLayout();
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
		}
	}

	protected override bool CanDragItem(BaseItem item)
	{
		if (item is ExplorerBarGroupItem)
		{
			return false;
		}
		return base.CanDragItem(item);
	}
}
