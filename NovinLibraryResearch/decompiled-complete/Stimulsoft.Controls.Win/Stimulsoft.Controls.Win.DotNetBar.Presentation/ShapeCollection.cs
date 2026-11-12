using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class ShapeCollection : CollectionBase
{
	public Shape this[int index]
	{
		get
		{
			return (Shape)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public int Add(Shape shape)
	{
		return base.List.Add(shape);
	}

	public void Insert(int index, Shape value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(Shape value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(Shape value)
	{
		return base.List.Contains(value);
	}

	public void Remove(Shape value)
	{
		base.List.Remove(value);
	}

	public void CopyTo(Shape[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(Shape[] array)
	{
		base.List.CopyTo(array, 0);
	}
}
