using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public abstract class ImageItem : BaseItem
{
	private Size ಧ;

	private Size ਉ;

	internal static Size _ನ = new Size(16, 16);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual Size ImageSize
	{
		get
		{
			return ਉ;
		}
		set
		{
			ਉ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual Size SubItemsImageSize
	{
		get
		{
			return ಧ;
		}
		set
		{
			ಧ = value;
		}
	}

	public ImageItem()
		: this("", "")
	{
	}

	public ImageItem(string sName)
		: this(sName, "")
	{
	}

	public ImageItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		ಧ = _ನ;
		ਉ = _ನ;
	}

	protected internal override void OnItemAdded(BaseItem objItem)
	{
		base.OnItemAdded(objItem);
		if (objItem is ImageItem imageItem)
		{
			Size subItemsImageSize = ಧ;
			if (imageItem.ImageSize.Width > ಧ.Width)
			{
				subItemsImageSize.Width = imageItem.ImageSize.Width;
			}
			if (imageItem.ImageSize.Height > ಧ.Height)
			{
				subItemsImageSize.Height = imageItem.ImageSize.Height;
			}
			SubItemsImageSize = subItemsImageSize;
		}
	}

	public virtual void OnSubItemImageSizeChanged(BaseItem objItem)
	{
		if (!(objItem is ImageItem imageItem))
		{
			return;
		}
		if (SubItems.Count == 1)
		{
			SubItemsImageSize = new Size(imageItem.ImageSize.Width, imageItem.ImageSize.Height);
			return;
		}
		Size subItemsImageSize = ಧ;
		if (imageItem.ImageSize.Width > ಧ.Width)
		{
			subItemsImageSize.Width = imageItem.ImageSize.Width;
		}
		if (imageItem.ImageSize.Height > ಧ.Height)
		{
			subItemsImageSize.Height = imageItem.ImageSize.Height;
		}
		SubItemsImageSize = subItemsImageSize;
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		if (base.IsOnCustomizeDialog && ਉ.Width == _ನ.Width && ਉ.Height == _ನ.Height)
		{
			ਉ = new Size(16, 16);
		}
		base.OnIsOnCustomizeDialogChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual void OnImageChanged()
	{
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void RefreshImageSize()
	{
		SubItemsImageSize = Dpi.Size(_ನ);
		ਉ = _ನ;
		OnImageChanged();
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is ImageItem)
			{
				((ImageItem)subItem).RefreshImageSize();
			}
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		if (item != null)
		{
			RefreshSubItemImageSize();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void RefreshSubItemImageSize()
	{
		SubItemsImageSize = Size.Empty;
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ImageItem imageItem)
				{
					Size subItemsImageSize = ಧ;
					if (imageItem.ImageSize.Width > ಧ.Width)
					{
						subItemsImageSize.Width = imageItem.ImageSize.Width;
					}
					if (imageItem.ImageSize.Height > ಧ.Height)
					{
						subItemsImageSize.Height = imageItem.ImageSize.Height;
					}
					SubItemsImageSize = subItemsImageSize;
				}
			}
		}
		if (SubItemsImageSize.IsEmpty)
		{
			SubItemsImageSize = Dpi.Size(_ನ);
		}
	}

	protected override void ScaleItem(SizeF factor)
	{
		base.ScaleItem(factor);
		RefreshSubItemImageSize();
	}
}
