using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class GalleryGroupTypeEditor : UITypeEditor
{
	private const string CREATE_NEW_GROUP = "<Create new group>";

	private IWindowsFormsEditorService m_EdSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null && provider != null)
		{
			m_EdSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (m_EdSvc != null)
			{
				GalleryGroupCollection galleryGroupCollection = null;
				BaseItem baseItem = null;
				GalleryContainer galleryContainer = null;
				if (context.Instance is BaseItem)
				{
					baseItem = (BaseItem)context.Instance;
					galleryContainer = baseItem.Parent as GalleryContainer;
					if (galleryContainer == null)
					{
						MessageBox.Show("Item does not belong to the Gallery. Cannot edit groups.");
						return value;
					}
					galleryGroupCollection = galleryContainer.GalleryGroups;
				}
				if (galleryGroupCollection == null && context.Instance != null)
				{
					MessageBox.Show("Unknow control using GalleryGroupTypeEditor. Cannot edit groups. [" + context.Instance.ToString() + "]");
				}
				else if (galleryGroupCollection == null)
				{
					MessageBox.Show("Unknow control using GalleryGroupTypeEditor. Cannot edit groups. [context instance null]");
				}
				GalleryGroup galleryGroup = galleryContainer.GetGalleryGroup(baseItem);
				ListBox val = new ListBox();
				foreach (GalleryGroup item in galleryGroupCollection)
				{
					val.Items.Add((object)item);
					if (item == galleryGroup)
					{
						val.SelectedItem = item;
					}
				}
				val.Items.Add((object)"<Create new group>");
				val.SelectedIndexChanged += SelectedChanged;
				m_EdSvc.DropDownControl((Control)(object)val);
				value = ((!(val.SelectedItem is string) || !(val.SelectedItem.ToString() == "<Create new group>")) ? val.SelectedItem : DesignerSupport.CreateGalleryGroup(galleryContainer, provider));
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (m_EdSvc != null)
		{
			m_EdSvc.CloseDropDown();
		}
	}
}
