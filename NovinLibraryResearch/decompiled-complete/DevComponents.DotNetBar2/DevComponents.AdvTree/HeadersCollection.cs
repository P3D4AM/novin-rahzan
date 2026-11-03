using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree;

public class HeadersCollection : CollectionBase
{
	private AdvTree ӭ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public AdvTree Parent => ӭ;

	public HeaderDefinition this[int index]
	{
		get
		{
			return (HeaderDefinition)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal void _0CC5(AdvTree ԟ)
	{
		ӭ = ԟ;
	}

	public int Add(HeaderDefinition ch)
	{
		return base.List.Add(ch);
	}

	public void Insert(int index, HeaderDefinition value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(HeaderDefinition value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(HeaderDefinition value)
	{
		return base.List.Contains(value);
	}

	public void Remove(HeaderDefinition value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
	}

	public void CopyTo(HeaderDefinition[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(HeaderDefinition[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}

	public HeaderDefinition GetByName(string name)
	{
		foreach (HeaderDefinition item in base.List)
		{
			if (item.Name == name)
			{
				return item;
			}
		}
		return null;
	}
}
