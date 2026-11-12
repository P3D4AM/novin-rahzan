using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class GalleryContainerDesigner : ItemContainerDesigner
{
	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.AssociatedComponents);
			if (!(((ComponentDesigner)this).Component is GalleryContainer galleryContainer))
			{
				return arrayList;
			}
			galleryContainer.PopupGalleryItems.CopyTo(arrayList);
			return arrayList;
		}
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component is GalleryContainer)
		{
			((GalleryContainer)component).PopupGalleryItem.SetDesignMode(b: true);
		}
	}

	protected override void SetDesignTimeDefaults()
	{
		GalleryContainer galleryContainer = ((ComponentDesigner)this).Component as GalleryContainer;
		galleryContainer.StretchGallery = true;
		galleryContainer.BackgroundStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		base.SetDesignTimeDefaults();
	}

	protected override void AddNewItem(BaseItem newItem)
	{
		if (((ComponentDesigner)this).Component is GalleryContainer galleryContainer && !galleryContainer.DesignTimeMouseDownPoint.IsEmpty && galleryContainer.PopupGalleryButtonBounds.Contains(galleryContainer.DesignTimeMouseDownPoint))
		{
			galleryContainer.DesignTimeMouseDownPoint = Point.Empty;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(((ComponentDesigner)this).Component).Find("PopupGalleryItems", ignoreCase: true));
			galleryContainer.PopupGalleryItems.Add(newItem);
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(((ComponentDesigner)this).Component).Find("PopupGalleryItems", ignoreCase: true), null, null);
		}
		else
		{
			base.AddNewItem(newItem);
		}
	}

	protected override void NewItemAdded(BaseItem itemAdded)
	{
		base.NewItemAdded(itemAdded);
		if (((ComponentDesigner)this).Component is GalleryContainer galleryContainer && galleryContainer.SubItems.Contains(itemAdded) && !(galleryContainer.Parent is ButtonItem))
		{
			galleryContainer.EnsureVisible(itemAdded);
		}
	}

	protected override void RecalcLayout()
	{
		base.RecalcLayout();
		GalleryContainer galleryContainer = ((ComponentDesigner)this).Component as GalleryContainer;
		if (galleryContainer.PopupGalleryItem.Expanded && galleryContainer.PopupGalleryItem.PopupControl != null)
		{
			Control popupControl = galleryContainer.PopupGalleryItem.PopupControl;
			if (popupControl is MenuPanel)
			{
				((MenuPanel)(object)popupControl).RecalcSize();
			}
			else if (popupControl is Bar)
			{
				((Bar)(object)popupControl).RecalcLayout();
			}
		}
	}

	protected override void ComponentRemoved(ComponentEventArgs e)
	{
		if (e.Component is GalleryGroup)
		{
			GalleryGroup value = e.Component as GalleryGroup;
			if (((ComponentDesigner)this).Component is GalleryContainer galleryContainer && galleryContainer.GalleryGroups.Contains(value))
			{
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(galleryContainer, TypeDescriptor.GetProperties(galleryContainer)["GalleryGroups"]);
				galleryContainer.GalleryGroups.Remove(value);
				componentChangeService?.OnComponentChanged(galleryContainer, TypeDescriptor.GetProperties(galleryContainer)["GalleryGroups"], null, null);
			}
		}
		base.ComponentRemoved(e);
	}
}
