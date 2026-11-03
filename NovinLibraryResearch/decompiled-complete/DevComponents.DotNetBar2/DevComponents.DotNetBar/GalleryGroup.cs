using System.ComponentModel;
using System.Drawing.Design;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[TypeConverter("DevComponents.DotNetBar.Design.GalleryGroupConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
public class GalleryGroup : Component
{
	private string ө = "";

	private string Ӿ = "";

	private GalleryContainer ᦩ;

	private int ᦪ;

	private SubItemsCollection ณ;

	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates title of the group that will be displayed on the group label when on popup gallery.")]
	[Browsable(true)]
	[DefaultValue("")]
	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			ө = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates the name used to identify the group.")]
	[Category("Design")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				Ӿ = Site.Name;
			}
			return Ӿ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[Browsable(false)]
	public GalleryContainer ParentGallery => ᦩ;

	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates display order for the group when displayed on the popup.")]
	public int DisplayOrder
	{
		get
		{
			return ᦪ;
		}
		set
		{
			ᦪ = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection Items => ณ;

	public GalleryGroup()
	{
		ณ = new SubItemsCollection(null);
		ณ._0AF0 = true;
		ณ.AllowParentRemove = false;
	}

	internal void SetParentGallery(GalleryContainer value)
	{
		ᦩ = value;
	}

	public override string ToString()
	{
		if (ө.Length > 0)
		{
			return ө;
		}
		return base.ToString();
	}
}
