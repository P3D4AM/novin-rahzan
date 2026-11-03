using System.Collections;
using System.ComponentModel;

namespace DevComponents.AdvTree;

public class NodeCollection : CollectionBase
{
	private Node ԙ;

	private AdvTree Ԛ;

	private eTreeAction ԛ = eTreeAction.Code;

	private bool _Ԝ;

	internal eTreeAction Ԗ
	{
		get
		{
			return ԛ;
		}
		set
		{
			ԛ = value;
		}
	}

	internal bool ԗ
	{
		get
		{
			return _Ԝ;
		}
		set
		{
			_Ԝ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Node ParentNode => ԙ;

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

	public Node this[int index]
	{
		get
		{
			return (Node)base.List[index];
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

	public virtual int Add(Node node)
	{
		return Add(node, eTreeAction.Code);
	}

	public virtual int Add(Node node, eTreeAction action)
	{
		ԛ = action;
		return base.List.Add(node);
	}

	public virtual void AddRange(Node[] nodes)
	{
		foreach (Node node in nodes)
		{
			Add(node);
		}
	}

	public virtual void Insert(int index, Node value)
	{
		base.List.Insert(index, value);
	}

	public virtual void Insert(int index, Node value, eTreeAction action)
	{
		ԛ = action;
		base.List.Insert(index, value);
	}

	public int IndexOf(Node value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(Node value)
	{
		return base.List.Contains(value);
	}

	public virtual void Remove(Node value)
	{
		base.List.Remove(value);
	}

	public virtual void Remove(Node node, eTreeAction action)
	{
		ԛ = action;
		base.List.Remove(node);
	}

	protected override void OnRemove(int index, object value)
	{
		if (!_Ԝ)
		{
			ԣ()?.InvokeBeforeNodeRemove(ԛ, value as Node, ԙ);
		}
		base.OnRemove(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		Ԡ(index, value);
	}

	private void Ԡ(int ԡ, object ԝ)
	{
		if (!_Ԝ)
		{
			Node node = ԝ as Node;
			node.SetParent(null);
			node.ӵ = null;
			if (ԙ != null)
			{
				ԙ.OnChildNodeRemoved(node);
			}
			ԣ()?.NodeRemoved(ԛ, ԝ as Node, ԙ, ԡ);
		}
	}

	protected override void OnSetComplete(int index, object oldValue, object newValue)
	{
		base.OnSetComplete(index, oldValue, newValue);
		Ԡ(index, oldValue);
		Ԣ(newValue);
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (!_Ԝ)
		{
			AdvTree advTree = ԣ();
			advTree?.InvokeBeforeNodeRemove(ԛ, oldValue as Node, ԙ);
			advTree?.InvokeBeforeNodeInsert(ԛ, newValue as Node, ԙ);
		}
		base.OnSet(index, oldValue, newValue);
	}

	protected override void OnInsert(int index, object value)
	{
		if (!_Ԝ)
		{
			ԣ()?.InvokeBeforeNodeInsert(ԛ, value as Node, ԙ);
		}
		base.OnInsert(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		Ԣ(value);
	}

	private void Ԣ(object ԝ)
	{
		if (!_Ԝ)
		{
			Node node = ԝ as Node;
			if (ԙ != null)
			{
				if (node.Parent != null && node.Parent != ԙ)
				{
					node.Remove();
				}
				node.SetParent(ԙ);
				if (ԙ.NodesColumns.IsSorted)
				{
					AdvTree treeControl = Ԛ;
					if (treeControl == null)
					{
						treeControl = ԙ.TreeControl;
					}
					treeControl?.ژ(ԙ);
				}
			}
			else
			{
				if (node.Parent != null)
				{
					node.Remove();
				}
				else
				{
					node.InvokeOnParentChanged();
				}
				if (Ԛ != null && Ԛ.Columns.IsSorted)
				{
					Ԛ.ژ();
				}
			}
			node.ӵ = Ԛ;
			if (ԙ != null)
			{
				ԙ.OnChildNodeInserted(node);
			}
			else
			{
				node.SizeChanged = true;
			}
			ԣ()?.InvokeAfterNodeInsert(ԛ, ԝ as Node, ԙ);
		}
		ԛ = eTreeAction.Code;
	}

	public void CopyTo(Node[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CopyTo(Node[] array)
	{
		base.List.CopyTo(array, 0);
	}

	protected override void OnClear()
	{
		if (!_Ԝ)
		{
			foreach (Node item in base.List)
			{
				item.SetParent(null);
				item.ӵ = null;
			}
		}
		base.OnClear();
	}

	protected override void OnClearComplete()
	{
		if (Ԛ != null && !ԗ)
		{
			Ԛ._06D8();
		}
		else if (ԙ != null && !ԗ)
		{
			ԙ.OnNodesCleared();
		}
		ԣ()?.ٷ();
		base.OnClearComplete();
	}

	private AdvTree ԣ()
	{
		if (Ԛ != null)
		{
			return Ԛ;
		}
		if (ԙ != null)
		{
			return ԙ.TreeControl;
		}
		return null;
	}

	public virtual void Sort()
	{
		Sort(0, base.Count, Comparer.Default);
	}

	public virtual void Sort(IComparer comparer)
	{
		Sort(0, base.Count, comparer);
	}

	public virtual void Sort(int index, int count, IComparer comparer)
	{
		AdvTree advTree = ԣ();
		if (!_Ԝ)
		{
			advTree?.BeginUpdate();
		}
		base.InnerList.Sort(index, count, comparer);
		if (advTree != null && advTree.DeepSort)
		{
			foreach (Node inner in base.InnerList)
			{
				inner.Nodes.Sort(0, inner.Nodes.Count, comparer);
			}
		}
		if (!_Ԝ)
		{
			advTree?.EndUpdate();
		}
	}

	public Node[] Find(string name, bool searchAllChildren)
	{
		ArrayList arrayList = new ArrayList();
		_175C.ច(this, name, searchAllChildren, arrayList);
		Node[] array = new Node[arrayList.Count];
		if (arrayList.Count > 0)
		{
			arrayList.CopyTo(array);
		}
		return array;
	}
}
