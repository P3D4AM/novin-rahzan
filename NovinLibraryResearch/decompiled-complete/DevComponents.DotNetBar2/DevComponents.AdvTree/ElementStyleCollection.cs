using System.Collections;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

public class ElementStyleCollection : CollectionBase
{
	private AdvTree Ԛ;

	internal AdvTree Ԙ
	{
		get
		{
			return Ԛ;
		}
		set
		{
			Ԛ = value;
		}
	}

	public ElementStyle this[int index]
	{
		get
		{
			return (ElementStyle)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public ElementStyle this[string name]
	{
		get
		{
			foreach (ElementStyle item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
	}

	public int Add(ElementStyle style)
	{
		return base.List.Add(style);
	}

	public void Insert(int index, ElementStyle value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(ElementStyle value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(ElementStyle value)
	{
		return base.List.Contains(value);
	}

	public void Remove(ElementStyle value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		ElementStyle elementStyle = value as ElementStyle;
		elementStyle.ކ = null;
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		ElementStyle elementStyle = value as ElementStyle;
		if (elementStyle.ކ != null && elementStyle.ކ != this)
		{
			elementStyle.ކ.Remove(elementStyle);
		}
		elementStyle.ކ = this;
	}

	public void CopyTo(ElementStyle[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(ElementStyle[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
