using System;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar;

public class GalleryGroupCollection : CollectionBase
{
	private EventHandler ᦧ;

	private EventHandler ᦨ;

	private GalleryContainer _0AC3;

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

	internal GalleryContainer _၉
	{
		get
		{
			return _0AC3;
		}
		set
		{
			_0AC3 = value;
		}
	}

	public event EventHandler GroupRemoved
	{
		add
		{
			EventHandler eventHandler = ᦧ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᦧ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler GroupAdded
	{
		add
		{
			EventHandler eventHandler = ᦨ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦨ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᦨ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦨ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		if (ᦧ != null)
		{
			ᦧ(galleryGroup, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		GalleryGroup galleryGroup = value as GalleryGroup;
		if (_0AC3 != null)
		{
			galleryGroup.SetParentGallery(_0AC3);
		}
		if (ᦨ != null)
		{
			ᦨ(galleryGroup, new EventArgs());
		}
	}

	public void CopyTo(GalleryGroup[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(GalleryGroup[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
