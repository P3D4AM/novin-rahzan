using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class PageSliderDesigner : ControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[4]
			{
				new DesignerVerb("Create Page", CreatePage),
				new DesignerVerb("Delete Page", DeletePage),
				new DesignerVerb("Next Page", NextPage),
				new DesignerVerb("Previous Page", PreviousPage)
			};
			return new DesignerVerbCollection(array);
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
				componentChangeService.ComponentRemoved += OnComponentRemoved;
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

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component is PageSliderPage)
		{
			PageSliderPage pageSliderPage = e.Component as PageSliderPage;
			PageSlider pageSlider = ((ControlDesigner)this).Control as PageSlider;
			if (pageSliderPage != null && ((Control)pageSlider).Controls.Contains((Control)(object)pageSliderPage))
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(pageSlider, TypeDescriptor.GetProperties(pageSlider)["Controls"]);
				((Control)pageSlider).Controls.Remove((Control)(object)pageSliderPage);
				componentChangeService?.OnComponentChanged(pageSlider, TypeDescriptor.GetProperties(pageSlider)["Controls"], null, null);
			}
		}
	}

	public override void DoDefaultAction()
	{
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
	}

	private void CreatePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		ISelectionService ss = ((ComponentDesigner)this).GetService(typeof(ISelectionService)) as ISelectionService;
		CreatePage(((ControlDesigner)this).Control as PageSlider, dh, cc, ss);
	}

	internal static PageSliderPage CreatePage(PageSlider parent, IDesignerHost dh, IComponentChangeService cc, ISelectionService ss)
	{
		DesignerTransaction designerTransaction = dh.CreateTransaction();
		PageSliderPage pageSliderPage = null;
		try
		{
			pageSliderPage = dh.CreateComponent(typeof(PageSliderPage)) as PageSliderPage;
			cc?.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["Controls"]);
			((Control)parent).Controls.Add((Control)(object)pageSliderPage);
			cc?.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["Controls"], null, null);
			ss?.SetSelectedComponents(new PageSliderPage[1] { pageSliderPage }, SelectionTypes.Replace);
			TypeDescriptor.GetProperties(parent)["SelectedPage"].SetValue(parent, pageSliderPage);
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
		return pageSliderPage;
	}

	private void DeletePage(object sender, EventArgs e)
	{
		IDesignerHost dh = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService cc = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (((ControlDesigner)this).Control is PageSlider pageSlider)
		{
			DeletePage(pageSlider.SelectedPage, dh, cc);
		}
	}

	internal static void DeletePage(PageSliderPage page, IDesignerHost dh, IComponentChangeService cc)
	{
		if (page == null || !(((Control)page).Parent is PageSlider))
		{
			return;
		}
		PageSlider pageSlider = (PageSlider)(object)((Control)page).Parent;
		DesignerTransaction designerTransaction = dh.CreateTransaction("Deleting page");
		try
		{
			cc?.OnComponentChanging(pageSlider, TypeDescriptor.GetProperties(pageSlider)["Controls"]);
			((Control)pageSlider).Controls.Remove((Control)(object)page);
			cc?.OnComponentChanged(pageSlider, TypeDescriptor.GetProperties(pageSlider)["Controls"], null, null);
			dh.DestroyComponent((IComponent)page);
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

	private void NextPage(object sender, EventArgs e)
	{
		SelectNextPage(((ControlDesigner)this).Control as PageSlider);
	}

	private void PreviousPage(object sender, EventArgs e)
	{
		SelectPreviousPage(((ControlDesigner)this).Control as PageSlider);
	}

	internal static bool SelectNextPage(PageSlider slider)
	{
		if (slider == null)
		{
			return false;
		}
		if (slider.PageCount > 0)
		{
			if (slider.SelectedPageIndex < slider.PageCount - 1)
			{
				slider.SelectedPageIndex++;
			}
			else
			{
				slider.SelectedPageIndex = 0;
			}
			return true;
		}
		return false;
	}

	internal static bool SelectPreviousPage(PageSlider slider)
	{
		if (slider == null)
		{
			return false;
		}
		if (slider.PageCount > 0)
		{
			if (slider.SelectedPageIndex > 0)
			{
				slider.SelectedPageIndex--;
			}
			else
			{
				slider.SelectedPageIndex = slider.PageCount - 1;
			}
			return true;
		}
		return false;
	}
}
