using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class MarkupElementCollection : CollectionBase
{
	private MarkupElement m_Parent;

	public MarkupElement Parent
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
		}
	}

	public MarkupElement this[int index]
	{
		get
		{
			return (MarkupElement)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public MarkupElementCollection(MarkupElement parent)
	{
		m_Parent = parent;
	}

	public int Add(MarkupElement MarkupElement)
	{
		return base.List.Add(MarkupElement);
	}

	public void Insert(int index, MarkupElement value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(MarkupElement value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(MarkupElement value)
	{
		return base.List.Contains(value);
	}

	public void Remove(MarkupElement value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		MarkupElement markupElement = value as MarkupElement;
		if (m_Parent != null)
		{
			markupElement.SetParent(null);
			m_Parent.IsSizeValid = false;
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		MarkupElement markupElement = value as MarkupElement;
		if (m_Parent != null)
		{
			markupElement.SetParent(m_Parent);
			m_Parent.IsSizeValid = false;
		}
	}

	public void CopyTo(MarkupElement[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(MarkupElement[] array)
	{
		base.List.CopyTo(array, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
