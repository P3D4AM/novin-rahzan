using System.Collections;

namespace DevComponents.AdvTree;

public class NodeComparer : IComparer
{
	private int __17CD;

	public int ColumnIndex
	{
		get
		{
			return __17CD;
		}
		set
		{
			__17CD = value;
		}
	}

	public NodeComparer()
	{
	}

	public NodeComparer(int columnIndex)
	{
		__17CD = columnIndex;
	}

	public virtual int Compare(object x, object y)
	{
		Node node = (Node)x;
		Node node2 = (Node)y;
		if (__17CD < node.Cells.Count && __17CD < node2.Cells.Count)
		{
			if (AdvTreeSettings.UseSortAlphaComparer)
			{
				return Utilities._17D2(node.Cells[__17CD].Text, node2.Cells[__17CD].Text);
			}
			return Utilities._17D5(node.Cells[__17CD].Text, node2.Cells[__17CD].Text);
		}
		return 0;
	}
}
