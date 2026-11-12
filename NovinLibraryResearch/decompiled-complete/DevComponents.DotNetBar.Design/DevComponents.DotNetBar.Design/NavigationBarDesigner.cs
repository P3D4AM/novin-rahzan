using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class NavigationBarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[1]
			{
				new DesignerVerb("Create New Button", CreateNewButton)
			};
			return new DesignerVerbCollection(value);
		}
	}

	public NavigationBarDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void OnSetComponentDefaults()
	{
		if (((ControlDesigner)this).Control is NavigationBar navigationBar)
		{
			navigationBar.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		((ControlDesigner)this).OnSetComponentDefaults();
	}

	private void CreateNewButton(object sender, EventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (!(((ControlDesigner)this).Control is NavigationBar navigationBar) || designerHost == null)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction();
		try
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			ButtonItem buttonItem = null;
			try
			{
				m_CreatingItem = true;
				buttonItem = designerHost.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				buttonItem.Text = buttonItem.Name;
				buttonItem.OptionGroup = "navBar";
				buttonItem.Image = (Image)(object)Helpers.LoadBitmap("SystemImages.DefaultNavBarImage.png");
				navigationBar.Items.Add(buttonItem);
			}
			finally
			{
				m_CreatingItem = false;
			}
			if (navigationBar.Items.Count == 1)
			{
				buttonItem.Checked = true;
			}
			navigationBar.RecalcLayout();
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
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

	protected override bool OnMouseDown(ref Message m, MouseButtons mb)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (base.OnMouseDown(ref m, mb))
		{
			return true;
		}
		if (!(GetItemContainerControl() is NavigationBar navigationBar) || ((Control)navigationBar).IsDisposed || !navigationBar.SplitterVisible)
		{
			return false;
		}
		Point point = ((Control)navigationBar).PointToClient(Control.MousePosition);
		MouseEventArgs val = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
		if (((Message)(ref m)).Msg == 513)
		{
			if (navigationBar.HitTestSplitter(val.X, val.Y))
			{
				navigationBar.SplitterMouseDown(val);
			}
			if (navigationBar.IsSplitterMouseDown)
			{
				return true;
			}
		}
		return false;
	}

	protected override bool OnMouseUp(ref Message m)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		if (GetItemContainerControl() is NavigationBar navigationBar && !((Control)navigationBar).IsDisposed && navigationBar.SplitterVisible)
		{
			Point point = ((Control)navigationBar).PointToClient(Control.MousePosition);
			MouseEventArgs e = new MouseEventArgs((MouseButtons)1048576, 0, point.X, point.Y, 0);
			if (navigationBar.IsSplitterMouseDown)
			{
				navigationBar.SplitterMouseUp(e);
				OnNavigationBarHeightChanged(((Control)navigationBar).Height);
			}
			else
			{
				navigationBar.SplitterMouseUp(e);
			}
		}
		if (base.OnMouseUp(ref m))
		{
			return true;
		}
		return false;
	}

	protected virtual void OnNavigationBarHeightChanged(int newHeight)
	{
	}

	protected override bool OnMouseMove(ref Message m)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		if (base.OnMouseMove(ref m))
		{
			return true;
		}
		if (GetItemContainerControl() is NavigationBar navigationBar && !((Control)navigationBar).IsDisposed && navigationBar.SplitterVisible)
		{
			Point point = ((Control)navigationBar).PointToClient(Control.MousePosition);
			MouseEventArgs e = new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0);
			navigationBar.SplitterMouseMove(e);
		}
		return false;
	}

	protected override bool OnMouseLeave(ref Message m)
	{
		if (base.OnMouseLeave(ref m))
		{
			return true;
		}
		if (GetItemContainerControl() is NavigationBar navigationBar && !((Control)navigationBar).IsDisposed && navigationBar.SplitterVisible)
		{
			navigationBar.SplitterMouseLeave();
		}
		return false;
	}

	protected override void OnItemSelected(BaseItem item)
	{
		base.OnItemSelected(item);
		if (item is ButtonItem && !((ButtonItem)item).Checked)
		{
			ButtonItem buttonItem = item as ButtonItem;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(buttonItem, TypeDescriptor.GetProperties(buttonItem).Find("Checked", ignoreCase: true));
			buttonItem.Checked = true;
			componentChangeService?.OnComponentChanged(buttonItem, TypeDescriptor.GetProperties(buttonItem).Find("Checked", ignoreCase: true), null, null);
		}
	}
}
