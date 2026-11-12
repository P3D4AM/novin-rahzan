using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxTree : ISyntaxTree
{
	private class PointComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Point startPoint = ((ISyntaxNode)x).Range.StartPoint;
			Point point = (Point)y;
			int num = startPoint.Y - point.Y;
			if (num == 0)
			{
				num = startPoint.X - point.X;
			}
			return num;
		}
	}

	private ISyntaxNode root;

	private ArrayList stack;

	private IComparer pointComparer;

	public ISyntaxNode Root => root;

	public ISyntaxNode Current
	{
		get
		{
			if (stack.Count <= 0)
			{
				return root;
			}
			return stack[stack.Count - 1] as ISyntaxNode;
		}
	}

	public SyntaxTree()
	{
		stack = new ArrayList();
		root = new SyntaxNode();
		pointComparer = new PointComparer();
	}

	public void BlockDeleting(Rectangle rect)
	{
		((SyntaxNode)root).BlockDeleting(rect, pointComparer);
	}

	public void PositionChanged(int x, int y, int deltaX, int deltaY, UpdateReason reason)
	{
		((SyntaxNode)root).PositionChanged(x, y, deltaX, deltaY, pointComparer);
	}

	public void Push(ISyntaxNode node)
	{
		stack.Add(node);
	}

	public ISyntaxNode Pop()
	{
		ISyntaxNode syntaxNode = ((stack.Count > 0) ? (stack[stack.Count - 1] as ISyntaxNode) : null);
		if (syntaxNode != null)
		{
			stack.RemoveAt(stack.Count - 1);
		}
		return syntaxNode;
	}

	public void Sort(IComparer comparer)
	{
		Root.Sort(comparer);
	}

	public void Clear()
	{
		root.Clear();
		stack.Clear();
	}

	public ISyntaxNode FindNode(object obj, IComparer comparer)
	{
		ISyntaxNode syntaxNode = Root;
		ISyntaxNode result = null;
		while (syntaxNode != null)
		{
			result = syntaxNode;
			syntaxNode = syntaxNode.FindNode(obj, comparer);
		}
		return result;
	}
}
