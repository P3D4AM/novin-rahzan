using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public abstract class ImageItem : BaseItem
{
	private Size m_SubItemsImageSize;

	private Size m_ImageSize;

	internal static Size _InitalImageSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual Size ImageSize
	{
		get
		{
			return m_ImageSize;
		}
		set
		{
			m_ImageSize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual Size SubItemsImageSize
	{
		get
		{
			return m_SubItemsImageSize;
		}
		set
		{
			m_SubItemsImageSize = value;
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
		m_SubItemsImageSize = _InitalImageSize;
		m_ImageSize = _InitalImageSize;
	}

	protected internal override void OnItemAdded(BaseItem objItem)
	{
		base.OnItemAdded(objItem);
		if (objItem is ImageItem imageItem)
		{
			Size subItemsImageSize = m_SubItemsImageSize;
			if (imageItem.ImageSize.Width > m_SubItemsImageSize.Width)
			{
				subItemsImageSize.Width = imageItem.ImageSize.Width;
			}
			if (imageItem.ImageSize.Height > m_SubItemsImageSize.Height)
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
		Size subItemsImageSize = m_SubItemsImageSize;
		if (imageItem.ImageSize.Width > m_SubItemsImageSize.Width)
		{
			subItemsImageSize.Width = imageItem.ImageSize.Width;
		}
		if (imageItem.ImageSize.Height > m_SubItemsImageSize.Height)
		{
			subItemsImageSize.Height = imageItem.ImageSize.Height;
		}
		SubItemsImageSize = subItemsImageSize;
	}

	protected override void OnIsOnCustomizeDialogChanged()
	{
		if (base.IsOnCustomizeDialog && m_ImageSize.Width == _InitalImageSize.Width && m_ImageSize.Height == _InitalImageSize.Height)
		{
			m_ImageSize = new Size((int)Math.Ceiling(16.0 * StiScale2.X), (int)Math.Ceiling(16.0 * StiScale2.Y));
		}
		base.OnIsOnCustomizeDialogChanged();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void OnImageChanged()
	{
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void RefreshImageSize()
	{
		SubItemsImageSize = _InitalImageSize;
		m_ImageSize = _InitalImageSize;
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

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnAfterItemRemoved(item);
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
					Size subItemsImageSize = m_SubItemsImageSize;
					if (imageItem.ImageSize.Width > m_SubItemsImageSize.Width)
					{
						subItemsImageSize.Width = imageItem.ImageSize.Width;
					}
					if (imageItem.ImageSize.Height > m_SubItemsImageSize.Height)
					{
						subItemsImageSize.Height = imageItem.ImageSize.Height;
					}
					SubItemsImageSize = subItemsImageSize;
				}
			}
		}
		if (SubItemsImageSize.IsEmpty)
		{
			SubItemsImageSize = _InitalImageSize;
		}
	}
}
