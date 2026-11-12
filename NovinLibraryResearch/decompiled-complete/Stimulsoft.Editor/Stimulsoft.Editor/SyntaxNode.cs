using System;
using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxNode : ISyntaxNode
{
	private IRange range = new Range(0, 0, 0, 0);

	private string name = string.Empty;

	private ISyntaxNode parent;

	private int nodeType;

	private ISyntaxNodes childs;

	private ISyntaxAttributes attributes;

	private ISyntaxErrors errors;

	private SyntaxNodeOptions options;

	public ISyntaxNode Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	public ISyntaxNode Root
	{
		get
		{
			ISyntaxNode syntaxNode = parent;
			while (syntaxNode != null && syntaxNode.Parent != null)
			{
				syntaxNode = syntaxNode.Parent;
			}
			return syntaxNode;
		}
	}

	public IRange Range
	{
		get
		{
			return range;
		}
		set
		{
			range = value;
		}
	}

	public Point Position
	{
		get
		{
			return range.StartPoint;
		}
		set
		{
			range.StartPoint = value;
			range.EndPoint = new Point(value.X + name.Length, value.Y);
		}
	}

	public Size Size
	{
		get
		{
			return new Size(range.EndPoint.X - range.StartPoint.X, range.EndPoint.Y - range.StartPoint.Y);
		}
		set
		{
			range.EndPoint = new Point(range.StartPoint.X + value.Width, range.StartPoint.Y + value.Height);
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public int NodeType
	{
		get
		{
			return nodeType;
		}
		set
		{
			nodeType = value;
		}
	}

	public ISyntaxNode[] Childs
	{
		get
		{
			if (childs == null)
			{
				return new ISyntaxNode[0];
			}
			ISyntaxNode[] array = new ISyntaxNode[childs.Count];
			((SyntaxNodes)childs).CopyTo(array);
			return array;
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				if (childs != null)
				{
					childs.Clear();
				}
				childs = null;
				return;
			}
			if (childs == null)
			{
				childs = new SyntaxNodes();
			}
			childs.Clear();
			foreach (ISyntaxNode value2 in value)
			{
				childs.Add(value2);
			}
		}
	}

	public ISyntaxAttribute[] Attributes
	{
		get
		{
			if (attributes == null)
			{
				return new ISyntaxAttribute[0];
			}
			ISyntaxAttribute[] array = new ISyntaxAttribute[attributes.Count];
			((SyntaxAttributes)attributes).CopyTo(array);
			return array;
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				if (attributes != null)
				{
					attributes.Clear();
				}
				attributes = null;
				return;
			}
			if (attributes == null)
			{
				attributes = new SyntaxAttributes();
			}
			attributes.Clear();
			foreach (ISyntaxAttribute value2 in value)
			{
				attributes.Add(value2);
			}
		}
	}

	public ISyntaxError[] Errors
	{
		get
		{
			if (errors == null)
			{
				return new ISyntaxError[0];
			}
			ISyntaxError[] array = new ISyntaxError[errors.Count];
			((SyntaxErrors)errors).CopyTo(array);
			return array;
		}
		set
		{
			if (value == null || value.Length == 0)
			{
				if (errors != null)
				{
					errors.Clear();
				}
				errors = null;
				return;
			}
			if (errors == null)
			{
				errors = new SyntaxErrors();
			}
			errors.Clear();
			foreach (ISyntaxError value2 in value)
			{
				errors.Add(value2);
			}
		}
	}

	public ISyntaxNodes ChildList => childs;

	public ISyntaxAttributes AttributeList => attributes;

	public ISyntaxErrors ErrorList => errors;

	public bool HasChildren
	{
		get
		{
			if (childs != null)
			{
				return childs.Count > 0;
			}
			return false;
		}
	}

	public bool HasAttributes
	{
		get
		{
			if (attributes != null)
			{
				return attributes.Count > 0;
			}
			return false;
		}
	}

	public bool HasErrors
	{
		get
		{
			if (errors != null)
			{
				return errors.Count > 0;
			}
			return false;
		}
	}

	public virtual SyntaxNodeOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			options = value;
		}
	}

	public int ChildCount
	{
		get
		{
			if (childs == null)
			{
				return 0;
			}
			return childs.Count;
		}
	}

	public int AttributeCount
	{
		get
		{
			if (attributes == null)
			{
				return 0;
			}
			return attributes.Count;
		}
	}

	public int ErrorCount
	{
		get
		{
			if (errors == null)
			{
				return 0;
			}
			return errors.Count;
		}
	}

	public int Level
	{
		get
		{
			int num = 0;
			ISyntaxNode syntaxNode = parent;
			while (syntaxNode != null)
			{
				syntaxNode = syntaxNode.Parent;
				num++;
			}
			return num;
		}
	}

	public int Index
	{
		get
		{
			if (parent == null || parent.ChildList == null)
			{
				return -1;
			}
			return parent.ChildList.IndexOf(this);
		}
	}

	public SyntaxNode()
	{
	}

	public SyntaxNode(Point position, string name)
	{
		range.StartPoint = position;
		range.EndPoint = new Point(position.X + name.Length, position.Y);
		this.name = name;
	}

	~SyntaxNode()
	{
		Clear();
	}

	public SyntaxNode(Point position, string name, int nodeType)
		: this(position, name)
	{
		this.nodeType = nodeType;
	}

	public SyntaxNode(Point position, string name, int nodeType, SyntaxNodeOptions options)
		: this(position, name, nodeType)
	{
		this.options = options;
	}

	public override string ToString()
	{
		if (!(name != string.Empty))
		{
			return base.ToString();
		}
		return name;
	}

	public bool FindLast(object obj, out int index, IComparer comparer)
	{
		int num = 0;
		int num2 = childs.Count - 1;
		int num3 = num;
		int num4 = 0;
		bool flag = false;
		while (num <= num2)
		{
			num3 = num + num2 >> 1;
			int num5 = comparer.Compare(childs[num3], obj);
			if (num5 > 0)
			{
				num2 = num3 - 1;
				continue;
			}
			num = num3 + 1;
			if (num5 == 0)
			{
				num4 = num3;
				flag = true;
			}
		}
		index = (flag ? num4 : num);
		return flag;
	}

	protected internal bool BlockDeleting(Rectangle rect, IComparer comparer)
	{
		bool result = false;
		if (HasAttributes)
		{
			for (int num = attributes.Count - 1; num >= 0; num--)
			{
				if (SortList.InsideBlock(attributes[num].Position, rect))
				{
					attributes.RemoveAt(num);
					result = true;
				}
			}
		}
		if (HasErrors)
		{
			for (int num2 = errors.Count - 1; num2 >= 0; num2--)
			{
				IRange range = errors[num2].Range;
				if (SortList.InsideBlock(range.StartPoint, rect) && SortList.InsideBlock(new Point(Math.Max(range.EndPoint.X - 1, 0), range.EndPoint.Y), rect))
				{
					errors.RemoveAt(num2);
					result = true;
				}
			}
		}
		if (HasChildren)
		{
			if (FindLast(new Point(rect.Right, rect.Bottom), out var index, comparer))
			{
				index++;
			}
			for (int num3 = Math.Min(index, childs.Count - 1); num3 >= 0; num3--)
			{
				ISyntaxNode syntaxNode = childs[num3];
				IRange range2 = syntaxNode.Range;
				if (range2.EndPoint.Y < rect.Top)
				{
					break;
				}
				if (SortList.InsideBlock(range2.StartPoint, rect) && SortList.InsideBlock(new Point(Math.Max(range2.EndPoint.X - 1, 0), range2.EndPoint.Y), rect))
				{
					childs.RemoveAt(num3);
					result = true;
				}
				else
				{
					((SyntaxNode)syntaxNode).BlockDeleting(rect, comparer);
				}
			}
		}
		return result;
	}

	protected internal bool PositionChanged(int x, int y, int deltaX, int deltaY, IComparer comparer)
	{
		bool result = false;
		if (HasAttributes)
		{
			foreach (ISyntaxAttribute attribute in attributes)
			{
				Point pt = attribute.Position;
				if (deltaY == 0 && pt.Y > y)
				{
					break;
				}
				if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, attribute.Name == SyntaxConsts.DefinitionScopeEnd))
				{
					attribute.Position = pt;
					result = true;
				}
			}
		}
		if (HasErrors)
		{
			foreach (ISyntaxError error in errors)
			{
				IRange range = error.Range;
				Point pt = range.StartPoint;
				if (deltaY == 0 && pt.Y > y)
				{
					break;
				}
				if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: false))
				{
					range.StartPoint = pt;
					result = true;
				}
				pt = range.EndPoint;
				if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
				{
					range.EndPoint = pt;
					result = true;
				}
			}
		}
		if (HasChildren)
		{
			Point pt = new Point(x, y);
			if (!FindLast(pt, out var index, comparer))
			{
				index--;
			}
			for (int i = Math.Max(index, 0); i < childs.Count; i++)
			{
				SyntaxNode syntaxNode = (SyntaxNode)childs[i];
				IRange range2 = syntaxNode.Range;
				pt = range2.StartPoint;
				if (deltaY == 0 && pt.Y > y)
				{
					break;
				}
				if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: false))
				{
					range2.StartPoint = pt;
					result = true;
				}
				pt = range2.EndPoint;
				if (SortList.UpdatePos(x, y, deltaX, deltaY, ref pt, endPos: true))
				{
					range2.EndPoint = pt;
					result = true;
				}
				if (syntaxNode.PositionChanged(x, y, deltaX, deltaY, comparer))
				{
					result = true;
				}
			}
		}
		return result;
	}

	public virtual void Clear()
	{
		if (childs != null)
		{
			childs.Clear();
		}
		childs = null;
		if (attributes != null)
		{
			attributes.Clear();
		}
		attributes = null;
		if (errors != null)
		{
			errors.Clear();
		}
		errors = null;
	}

	public virtual int AddChild(ISyntaxNode node)
	{
		if (childs == null)
		{
			childs = new SyntaxNodes();
		}
		node.Parent = this;
		return childs.Add(node);
	}

	public virtual int InsertChild(ISyntaxNode node, IComparer comparer)
	{
		if (!HasChildren)
		{
			return AddChild(node);
		}
		((SortList)childs).FindLast(node, out var index, comparer);
		childs.Insert(index, node);
		node.Parent = this;
		return index;
	}

	public virtual ISyntaxNode AddChild()
	{
		if (childs == null)
		{
			childs = new SyntaxNodes();
		}
		ISyntaxNode syntaxNode = new SyntaxNode();
		syntaxNode.Parent = this;
		childs.Add(syntaxNode);
		return syntaxNode;
	}

	public virtual void AddChildren(ISyntaxNodes nodes)
	{
		if (childs == null)
		{
			childs = new SyntaxNodes();
		}
		foreach (ISyntaxNode node in nodes)
		{
			node.Parent = this;
			childs.Add(node);
		}
	}

	public virtual int AddAttribute(ISyntaxAttribute attr)
	{
		if (attributes == null)
		{
			attributes = new SyntaxAttributes();
		}
		return attributes.Add(attr);
	}

	public virtual ISyntaxAttribute AddAttribute()
	{
		if (attributes == null)
		{
			attributes = new SyntaxAttributes();
		}
		ISyntaxAttribute syntaxAttribute = new SyntaxAttribute();
		attributes.Add(syntaxAttribute);
		return syntaxAttribute;
	}

	public virtual void AddAttributes(ISyntaxAttributes attrs)
	{
		if (attributes == null)
		{
			attributes = new SyntaxAttributes();
		}
		foreach (ISyntaxAttribute attr in attrs)
		{
			attributes.Add(attr);
		}
	}

	public virtual int AddError(ISyntaxError err)
	{
		if (errors == null)
		{
			errors = new SyntaxErrors();
		}
		return errors.Add(err);
	}

	public virtual ISyntaxError AddError()
	{
		if (errors == null)
		{
			errors = new SyntaxErrors();
		}
		ISyntaxError syntaxError = new SyntaxError();
		errors.Add(syntaxError);
		return syntaxError;
	}

	public virtual void AddErrors(ISyntaxErrors errs)
	{
		if (errors == null)
		{
			errors = new SyntaxErrors();
		}
		foreach (ISyntaxError err in errs)
		{
			errors.Add(err);
		}
	}

	public virtual void Sort(IComparer comparer)
	{
		if (childs == null)
		{
			return;
		}
		((SyntaxNodes)childs).Sort(comparer);
		foreach (ISyntaxNode child in childs)
		{
			child.Sort(comparer);
		}
	}

	public virtual ISyntaxNode FindNode(string name)
	{
		if (childs != null)
		{
			foreach (ISyntaxNode child in childs)
			{
				if (child.Name == name)
				{
					return child;
				}
			}
		}
		return null;
	}

	public virtual ISyntaxNode FindNode(int nodeType)
	{
		if (childs != null)
		{
			foreach (ISyntaxNode child in childs)
			{
				if (child.NodeType == nodeType)
				{
					return child;
				}
			}
		}
		return null;
	}

	public virtual ISyntaxNode FindNode(object obj, IComparer comparer)
	{
		if (childs != null)
		{
			int num = ((SyntaxNodes)childs).BinarySearch(obj, comparer);
			if (num < 0)
			{
				return null;
			}
			return childs[num];
		}
		return null;
	}

	public virtual ISyntaxAttribute FindAttribute(string name)
	{
		if (attributes != null)
		{
			foreach (ISyntaxAttribute attribute in attributes)
			{
				if (attribute.Name == name)
				{
					return attribute;
				}
			}
		}
		return null;
	}

	public virtual int GetIndent(int index, int indent)
	{
		if ((Options & SyntaxNodeOptions.KeepIndentation) != 0)
		{
			indent = -1;
		}
		else if (indent > 0)
		{
			if (index == range.StartPoint.Y)
			{
				if ((options & SyntaxNodeOptions.Indentation) == 0 || (options & SyntaxNodeOptions.BackIndentation) == 0)
				{
					indent--;
				}
			}
			else if (HasAttributes)
			{
				foreach (ISyntaxAttribute attribute in attributes)
				{
					if ((attribute.Name == SyntaxConsts.DefinitionScope || attribute.Name == SyntaxConsts.DefinitionScopeEnd) && attribute.Position.Y == index)
					{
						indent--;
						break;
					}
				}
			}
		}
		return indent;
	}
}
