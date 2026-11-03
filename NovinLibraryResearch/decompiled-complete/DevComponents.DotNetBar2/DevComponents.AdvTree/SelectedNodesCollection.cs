using System;
using System.Collections.Generic;

namespace DevComponents.AdvTree;

public class SelectedNodesCollection : NodeCollection
{
	internal AdvTree _0700;

	internal bool _0701;

	internal bool _0702;

	private int __0703;

	private List<AdvTreeNodeEventArgs> __0704 = new List<AdvTreeNodeEventArgs>();

	private bool __0705;

	internal bool ۿ
	{
		get
		{
			return __0705;
		}
		set
		{
			__0705 = value;
		}
	}

	public SelectedNodesCollection()
	{
		base.ԗ = true;
	}

	public override int Add(Node node, eTreeAction action)
	{
		if (base.List.Contains(node))
		{
			return -1;
		}
		if (_0700.MultiSelect)
		{
			if (_0700.MultiSelectRule == eMultiSelectRule.SameParent && base.Count > 0 && base[0].Parent != node.Parent)
			{
				throw new InvalidOperationException("Node being added does not belong to the same parent as currently selected nodes. See AdvTree.MultiSelectRule property");
			}
			if (!_0701)
			{
				AdvTreeNodeCancelEventArgs advTreeNodeCancelEventArgs = new AdvTreeNodeCancelEventArgs(action, node);
				_0700.ۀ(advTreeNodeCancelEventArgs);
				if (advTreeNodeCancelEventArgs.Cancel)
				{
					return -1;
				}
			}
		}
		return base.Add(node, action);
	}

	public override void AddRange(Node[] nodes)
	{
		ۿ = true;
		base.AddRange(nodes);
		ۿ = false;
		if (!_0701)
		{
			_0700.ٱ(EventArgs.Empty);
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (_0700.MultiSelect)
		{
			Node node = value as Node;
			node.IsSelected = true;
			_0700._0651(node);
			if (node.SelectedCell == null)
			{
				node.SelectFirstCell(base.Ԗ);
			}
			AdvTreeNodeEventArgs ھ = new AdvTreeNodeEventArgs(base.Ԗ, node);
			_0700.ڿ(ھ);
			node.InternalSelected(base.Ԗ);
			if (!__0705)
			{
				_0700.ٱ(EventArgs.Empty);
			}
		}
		base.OnInsertComplete(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		if (_0700.MultiSelect)
		{
			Node node = value as Node;
			node.IsSelected = false;
			_0700._0651(node);
			if (node.SelectedCell != null)
			{
				node.SelectedCell.SetSelected(selected: false, base.Ԗ);
			}
			_0700.ڽ(new AdvTreeNodeEventArgs(base.Ԗ, node));
			node.InternalDeselected(base.Ԗ);
			if (!__0705)
			{
				_0700.ٱ(EventArgs.Empty);
			}
		}
		base.OnRemoveComplete(index, value);
	}

	protected override void OnClear()
	{
		if (_0700.MultiSelect)
		{
			__0703 = base.List.Count;
			Node[] array = new Node[__0703];
			base.List.CopyTo(array, 0);
			Node[] array2 = array;
			foreach (Node node in array2)
			{
				node.IsSelected = false;
				_0700._0651(node);
				if (node.SelectedCell == null)
				{
					node.Cells[0].SetSelected(selected: false, base.Ԗ);
				}
				__0704.Add(new AdvTreeNodeEventArgs(base.Ԗ, node));
				node.InternalDeselected(base.Ԗ);
			}
		}
		base.OnClear();
	}

	protected override void OnClearComplete()
	{
		if (_0700.MultiSelect)
		{
			if (_0700 != null && !_0702)
			{
				foreach (AdvTreeNodeEventArgs item in __0704)
				{
					_0700.ڽ(item);
				}
			}
			__0704.Clear();
			if (!_0702 && __0703 > 0)
			{
				_0700.ٱ(EventArgs.Empty);
			}
		}
		base.OnClearComplete();
	}
}
