using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DocumentBaseContainerCollection : CollectionBase
{
	private DocumentBaseContainer m_Owner;

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

	internal DocumentBaseContainer Owner
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

	public event EventHandler DocumentRemoved;

	public event EventHandler DocumentAdded;

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
		documentBaseContainer.SetParent(null);
		if (this.DocumentRemoved != null)
		{
			this.DocumentRemoved(documentBaseContainer, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		DocumentBaseContainer documentBaseContainer = value as DocumentBaseContainer;
		if (m_Owner != null)
		{
			documentBaseContainer.SetParent(m_Owner);
		}
		if (this.DocumentAdded != null)
		{
			this.DocumentAdded(documentBaseContainer, new EventArgs());
		}
	}

	public void CopyTo(DocumentBaseContainer[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(DocumentBaseContainer[] array)
	{
		base.List.CopyTo(array, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
