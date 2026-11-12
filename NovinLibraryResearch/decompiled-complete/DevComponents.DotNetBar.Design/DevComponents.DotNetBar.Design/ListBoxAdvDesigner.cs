using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ListBoxAdvDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[1]
			{
				new DesignerVerb("Add ListBoxItem", CreateListBoxItem)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public ListBoxAdvDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component != null && component.Site != null)
		{
			_ = component.Site.DesignMode;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	protected virtual void SetDesignTimeDefaults()
	{
		ListBoxAdv listBoxAdv = ((ControlDesigner)this).Control as ListBoxAdv;
		listBoxAdv.BackgroundStyle.Class = ElementStyleClassKeys.ListBoxAdvKey;
		listBoxAdv.AutoScroll = true;
	}

	protected virtual void CreateListBoxItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateListBoxItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual void CreateListBoxItem(BaseItem parent)
	{
		if (parent == null)
		{
			return;
		}
		ListBoxAdv listBoxAdv = ((ControlDesigner)this).Control as ListBoxAdv;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(typeof(ListBoxItem)) is ListBoxItem listBoxItem)
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (listBoxAdv != null)
				{
					componentChangeService?.OnComponentChanging(listBoxAdv, TypeDescriptor.GetProperties(typeof(ListBoxAdv))["Items"]);
				}
				listBoxItem.Text = $"Item {parent.SubItems.Count + 1}";
				listBoxAdv.Items.Add(listBoxItem);
				RecalcLayout();
				if (listBoxAdv != null)
				{
					componentChangeService?.OnComponentChanged(listBoxAdv, TypeDescriptor.GetProperties(typeof(ListBoxAdv))["Items"], null, null);
				}
				OnitemCreated(listBoxItem);
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

	protected override bool GetHitTest(Point pt)
	{
		if (((ControlDesigner)this).Control is ListBoxAdv listBoxAdv && ((Control)listBoxAdv).IsHandleCreated && listBoxAdv.AutoScroll)
		{
			Point pt2 = ((Control)listBoxAdv).PointToClient(pt);
			if (listBoxAdv.VScrollBar != null && ((Control)listBoxAdv.VScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
			if (listBoxAdv.HScrollBar != null && ((Control)listBoxAdv.HScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
		}
		return base.GetHitTest(pt);
	}
}
