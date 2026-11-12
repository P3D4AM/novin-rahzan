using System.ComponentModel;
using System.Drawing.Design;

namespace Stimulsoft.Controls.Win.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class GalleryGroup : Component
{
	private string m_Text = "";

	private string m_Name = "";

	private GalleryContainer m_ParentGallery;

	private int m_DisplayOrder;

	private SubItemsCollection m_Items;

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Localizable(true)]
	[Description("Indicates title of the group that will be displayed on the group label when on popup gallery.")]
	public string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_Text = value;
		}
	}

	[Browsable(false)]
	[Category("Design")]
	[Description("Indicates the name used to identify the group.")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				m_Name = Site.Name;
			}
			return m_Name;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				m_Name = "";
			}
			else
			{
				m_Name = value;
			}
		}
	}

	[Browsable(false)]
	public GalleryContainer ParentGallery => m_ParentGallery;

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates display order for the group when displayed on the popup.")]
	[Category("Layout")]
	public int DisplayOrder
	{
		get
		{
			return m_DisplayOrder;
		}
		set
		{
			m_DisplayOrder = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection Items => m_Items;

	public GalleryGroup()
	{
		m_Items = new SubItemsCollection(null);
		m_Items.IgnoreEvents = true;
		m_Items.AllowParentRemove = false;
	}

	internal void SetParentGallery(GalleryContainer value)
	{
		m_ParentGallery = value;
	}

	public override string ToString()
	{
		if (m_Text.Length > 0)
		{
			return m_Text;
		}
		return base.ToString();
	}
}
