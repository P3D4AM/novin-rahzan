using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using DevComponents.DotNetBar.Design;

namespace DevComponents.DotNetBar;

internal class DesignerSupport
{
	public static ItemContainer CreateItemContainer(IDesignerServices designer, BaseItem parent, eOrientation containerOrientation)
	{
		IDesignerHost designerHost = designer.GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = designer.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null || parent == null || componentChangeService == null)
		{
			return null;
		}
		ItemContainer itemContainer = null;
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("New DotNetBar Item Container");
		try
		{
			itemContainer = designerHost.CreateComponent(typeof(ItemContainer)) as ItemContainer;
			TypeDescriptor.GetProperties(itemContainer)["LayoutOrientation"].SetValue(itemContainer, containerOrientation);
			componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(itemContainer)["SubItems"]);
			parent.SubItems.Add(itemContainer);
			componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(itemContainer)["SubItems"], null, null);
			return itemContainer;
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	public static ItemDockContainer CreateItemDockContainer(IDesignerServices designer, BaseItem parent)
	{
		IDesignerHost designerHost = designer.GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = designer.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null || parent == null || componentChangeService == null)
		{
			return null;
		}
		ItemDockContainer itemDockContainer = null;
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("New DotNetBar Item Dock Container");
		try
		{
			itemDockContainer = designerHost.CreateComponent(typeof(ItemDockContainer)) as ItemDockContainer;
			componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(itemDockContainer)["SubItems"]);
			parent.SubItems.Add(itemDockContainer);
			componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(itemDockContainer)["SubItems"], null, null);
			return itemDockContainer;
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	public static RibbonTabItemGroup CreateRibbonTabItemGroup(RibbonStrip strip, IServiceProvider provider)
	{
		IDesignerHost designerHost = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null || componentChangeService == null)
		{
			return null;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("New RibbonTabItemGroup");
		RibbonTabItemGroup ribbonTabItemGroup = null;
		try
		{
			ribbonTabItemGroup = designerHost.CreateComponent(typeof(RibbonTabItemGroup)) as RibbonTabItemGroup;
			componentChangeService.OnComponentChanging(strip, TypeDescriptor.GetProperties(strip)["TabGroups"]);
			strip.TabGroups.Add(ribbonTabItemGroup);
			componentChangeService.OnComponentChanged(strip, TypeDescriptor.GetProperties(strip)["TabGroups"], null, null);
			SetDefaults(ribbonTabItemGroup);
			return ribbonTabItemGroup;
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
		}
	}

	public static GalleryGroup CreateGalleryGroup(GalleryContainer gallery, IServiceProvider provider)
	{
		IDesignerHost designerHost = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null || componentChangeService == null)
		{
			return null;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("New GalleryGroup");
		GalleryGroup galleryGroup = null;
		try
		{
			galleryGroup = designerHost.CreateComponent(typeof(GalleryGroup)) as GalleryGroup;
			componentChangeService.OnComponentChanging(gallery, TypeDescriptor.GetProperties(gallery)["GalleryGroups"]);
			gallery.GalleryGroups.Add(galleryGroup);
			componentChangeService.OnComponentChanged(gallery, TypeDescriptor.GetProperties(gallery)["GalleryGroups"], null, null);
			galleryGroup.Text = galleryGroup.Name;
			return galleryGroup;
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
		}
	}

	public static void SetDefaults(RibbonTabItemGroup group)
	{
		TypeDescriptor.GetProperties(group)["GroupTitle"].SetValue(group, "New Group");
		TypeDescriptor.GetProperties(group.Style)["Border"].SetValue(group.Style, eStyleBorderType.Solid);
		TypeDescriptor.GetProperties(group.Style)["BorderColor"].SetValue(group.Style, ColorScheme.GetColor("9A3A3B"));
		TypeDescriptor.GetProperties(group.Style)["CornerType"].SetValue(group.Style, eCornerType.Square);
		TypeDescriptor.GetProperties(group.Style)["BackColor"].SetValue(group.Style, ColorScheme.GetColor("AE6D94"));
		TypeDescriptor.GetProperties(group.Style)["BackColor2"].SetValue(group.Style, ColorScheme.GetColor("90487B"));
		TypeDescriptor.GetProperties(group.Style)["BackColorGradientAngle"].SetValue(group.Style, 90);
		TypeDescriptor.GetProperties(group.Style)["BorderWidth"].SetValue(group.Style, 1);
		TypeDescriptor.GetProperties(group.Style)["TextColor"].SetValue(group.Style, Color.White);
		TypeDescriptor.GetProperties(group.Style)["TextShadowColor"].SetValue(group.Style, Color.Black);
		TypeDescriptor.GetProperties(group.Style)["TextShadowOffset"].SetValue(group.Style, new Point(1, 1));
		TypeDescriptor.GetProperties(group.Style)["TextAlignment"].SetValue(group.Style, eStyleTextAlignment.Center);
		TypeDescriptor.GetProperties(group.Style)["TextLineAlignment"].SetValue(group.Style, eStyleTextAlignment.Near);
	}
}
