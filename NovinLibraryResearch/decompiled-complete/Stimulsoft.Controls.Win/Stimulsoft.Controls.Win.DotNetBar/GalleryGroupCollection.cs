using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class GalleryGroupCollection : CollectionBase
{
	private GalleryContainer m_Owner;

	public GalleryGroup this[int index]
	{
		get
		{
			return (GalleryGroup)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public GalleryGroup this[string name]
	{
		get
		{
			foreach (GalleryGroup item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
	}

	internal GalleryContainer Owner
	{
		get
		{
			return m_Owner;
		}
		set
		{
			m_Owner = value;
		}
	}

	public event EventHandler GroupRemoved;

	public event EventHandler GroupAdded;

	public int Add(GalleryGroup tab)
	{
		return base.List.Add(tab);
	}

	public void AddRange(GalleryGroup[] groups)
	{
		foreach (GalleryGroup value in groups)
		{
			base.List.Add(value);
		}
	}

	public void Insert(int index, GalleryGroup value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(GalleryGroup value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(GalleryGroup value)
	{
		return base.List.Contains(value);
	}

	public void Remove(GalleryGroup value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		GalleryGroup galleryGroup = value as GalleryGroup;
		galleryGroup.SetParentGallery(null);
		if (this.GroupRemoved != null)
		{
			this.GroupRemoved(galleryGroup, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		GalleryGroup galleryGroup = value as GalleryGroup;
		if (m_Owner != null)
		{
			galleryGroup.SetParentGallery(m_Owner);
		}
		if (this.GroupAdded != null)
		{
			this.GroupAdded(galleryGroup, new EventArgs());
		}
	}

	public void CopyTo(GalleryGroup[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(GalleryGroup[] array)
	{
		base.List.CopyTo(array, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
