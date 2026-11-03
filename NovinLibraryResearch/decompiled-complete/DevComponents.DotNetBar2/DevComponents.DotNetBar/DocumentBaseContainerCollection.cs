using System;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar;

public class DocumentBaseContainerCollection : CollectionBase
{
	private EventHandler Ⴜ;

	private EventHandler Ⴛ;

	private DocumentBaseContainer _0AC3;

	public DocumentBaseContainer this[int index]
	{
		get
		{
			return (DocumentBaseContainer)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal DocumentBaseContainer _၉
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

	public int VisibleCount
	{
		get
		{
			int num = 0;
			foreach (DocumentBaseContainer item in base.List)
			{
				if (item.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	public event EventHandler DocumentRemoved
	{
		add
		{
			EventHandler eventHandler = Ⴜ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⴜ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⴜ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⴜ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler DocumentAdded
	{
		add
		{
			EventHandler eventHandler = Ⴛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⴛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⴛ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⴛ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public int Add(DocumentBaseContainer tab)
	{
		return base.List.Add(tab);
	}

	public void AddRange(DocumentBaseContainer[] documents)
	{
		foreach (DocumentBaseContainer value in documents)
		{
			base.List.Add(value);
		}
	}

	public void Insert(int index, DocumentBaseContainer value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(DocumentBaseContainer value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(DocumentBaseContainer value)
	{
		return base.List.Contains(value);
	}

	public void Remove(DocumentBaseContainer value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		DocumentBaseContainer documentBaseContainer = value as DocumentBaseContainer;
		documentBaseContainer._0CC5(null);
		if (Ⴜ != null)
		{
			Ⴜ(documentBaseContainer, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		DocumentBaseContainer documentBaseContainer = value as DocumentBaseContainer;
		if (_0AC3 != null)
		{
			documentBaseContainer._0CC5(_0AC3);
		}
		if (Ⴛ != null)
		{
			Ⴛ(documentBaseContainer, new EventArgs());
		}
	}

	public void CopyTo(DocumentBaseContainer[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(DocumentBaseContainer[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
