using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.AdvTree;

public class ConnectorPointsCollection : CollectionBase
{
	private Node ԙ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Node ParentNode => ԙ;

	public Point this[int index]
	{
		get
		{
			return (Point)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal void Ԟ(Node ԟ)
	{
		ԙ = ԟ;
	}

	public int Add(Point p)
	{
		return base.List.Add(p);
	}

	public void AddRange(Point[] ap)
	{
		foreach (Point p in ap)
		{
			Add(p);
		}
	}

	public Point[] ToArray()
	{
		Point[] array = new Point[base.Count];
		_0CD0(array);
		return array;
	}

	public void Insert(int index, Point value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(Point value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(Point value)
	{
		return base.List.Contains(value);
	}

	public void Remove(Point value)
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

	public void CopyTo(Point[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(Point[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
