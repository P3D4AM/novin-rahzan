using System;
using System.Threading;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display;

public abstract class TreeRenderer
{
	private NodeRendererEventHandler _2761;

	private NodeExpandPartRendererEventHandler _2762;

	private NodeCellRendererEventHandler _2763;

	private NodeCellRendererEventHandler _2764;

	private NodeCellRendererEventHandler _2765;

	private NodeCellRendererEventHandler _2766;

	private SelectionRendererEventHandler _2767;

	private SelectionRendererEventHandler _2768;

	private ConnectorRendererEventHandler _2769;

	private TreeBackgroundRendererEventHandler _276A;

	private DragDropMarkerRendererEventHandler _276B;

	private ColumnHeaderRendererEventHandler _276C;

	private NodeRendererEventHandler _276D;

	private TreeColorTable __137C;

	private Office2007ColorTable __276E;

	public TreeColorTable ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	internal Office2007ColorTable _2760
	{
		get
		{
			return __276E;
		}
		set
		{
			__276E = value;
		}
	}

	public event NodeRendererEventHandler RenderNodeBackground
	{
		add
		{
			NodeRendererEventHandler nodeRendererEventHandler = _2761;
			NodeRendererEventHandler nodeRendererEventHandler2;
			do
			{
				nodeRendererEventHandler2 = nodeRendererEventHandler;
				NodeRendererEventHandler value2 = (NodeRendererEventHandler)Delegate.Combine(nodeRendererEventHandler2, value);
				nodeRendererEventHandler = Interlocked.CompareExchange(ref _2761, value2, nodeRendererEventHandler2);
			}
			while ((object)nodeRendererEventHandler != nodeRendererEventHandler2);
		}
		remove
		{
			NodeRendererEventHandler nodeRendererEventHandler = _2761;
			NodeRendererEventHandler nodeRendererEventHandler2;
			do
			{
				nodeRendererEventHandler2 = nodeRendererEventHandler;
				NodeRendererEventHandler value2 = (NodeRendererEventHandler)Delegate.Remove(nodeRendererEventHandler2, value);
				nodeRendererEventHandler = Interlocked.CompareExchange(ref _2761, value2, nodeRendererEventHandler2);
			}
			while ((object)nodeRendererEventHandler != nodeRendererEventHandler2);
		}
	}

	public event NodeExpandPartRendererEventHandler RenderNodeExpandPart
	{
		add
		{
			NodeExpandPartRendererEventHandler nodeExpandPartRendererEventHandler = _2762;
			NodeExpandPartRendererEventHandler nodeExpandPartRendererEventHandler2;
			do
			{
				nodeExpandPartRendererEventHandler2 = nodeExpandPartRendererEventHandler;
				NodeExpandPartRendererEventHandler value2 = (NodeExpandPartRendererEventHandler)Delegate.Combine(nodeExpandPartRendererEventHandler2, value);
				nodeExpandPartRendererEventHandler = Interlocked.CompareExchange(ref _2762, value2, nodeExpandPartRendererEventHandler2);
			}
			while ((object)nodeExpandPartRendererEventHandler != nodeExpandPartRendererEventHandler2);
		}
		remove
		{
			NodeExpandPartRendererEventHandler nodeExpandPartRendererEventHandler = _2762;
			NodeExpandPartRendererEventHandler nodeExpandPartRendererEventHandler2;
			do
			{
				nodeExpandPartRendererEventHandler2 = nodeExpandPartRendererEventHandler;
				NodeExpandPartRendererEventHandler value2 = (NodeExpandPartRendererEventHandler)Delegate.Remove(nodeExpandPartRendererEventHandler2, value);
				nodeExpandPartRendererEventHandler = Interlocked.CompareExchange(ref _2762, value2, nodeExpandPartRendererEventHandler2);
			}
			while ((object)nodeExpandPartRendererEventHandler != nodeExpandPartRendererEventHandler2);
		}
	}

	public event NodeCellRendererEventHandler RenderCellBackground
	{
		add
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2763;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Combine(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2763, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
		remove
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2763;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Remove(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2763, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
	}

	public event NodeCellRendererEventHandler RenderCellCheckBox
	{
		add
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2764;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Combine(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2764, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
		remove
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2764;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Remove(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2764, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
	}

	public event NodeCellRendererEventHandler RenderCellImage
	{
		add
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2765;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Combine(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2765, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
		remove
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2765;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Remove(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2765, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
	}

	public event NodeCellRendererEventHandler RenderCellText
	{
		add
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2766;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Combine(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2766, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
		remove
		{
			NodeCellRendererEventHandler nodeCellRendererEventHandler = _2766;
			NodeCellRendererEventHandler nodeCellRendererEventHandler2;
			do
			{
				nodeCellRendererEventHandler2 = nodeCellRendererEventHandler;
				NodeCellRendererEventHandler value2 = (NodeCellRendererEventHandler)Delegate.Remove(nodeCellRendererEventHandler2, value);
				nodeCellRendererEventHandler = Interlocked.CompareExchange(ref _2766, value2, nodeCellRendererEventHandler2);
			}
			while ((object)nodeCellRendererEventHandler != nodeCellRendererEventHandler2);
		}
	}

	public event SelectionRendererEventHandler RenderSelection
	{
		add
		{
			SelectionRendererEventHandler selectionRendererEventHandler = _2767;
			SelectionRendererEventHandler selectionRendererEventHandler2;
			do
			{
				selectionRendererEventHandler2 = selectionRendererEventHandler;
				SelectionRendererEventHandler value2 = (SelectionRendererEventHandler)Delegate.Combine(selectionRendererEventHandler2, value);
				selectionRendererEventHandler = Interlocked.CompareExchange(ref _2767, value2, selectionRendererEventHandler2);
			}
			while ((object)selectionRendererEventHandler != selectionRendererEventHandler2);
		}
		remove
		{
			SelectionRendererEventHandler selectionRendererEventHandler = _2767;
			SelectionRendererEventHandler selectionRendererEventHandler2;
			do
			{
				selectionRendererEventHandler2 = selectionRendererEventHandler;
				SelectionRendererEventHandler value2 = (SelectionRendererEventHandler)Delegate.Remove(selectionRendererEventHandler2, value);
				selectionRendererEventHandler = Interlocked.CompareExchange(ref _2767, value2, selectionRendererEventHandler2);
			}
			while ((object)selectionRendererEventHandler != selectionRendererEventHandler2);
		}
	}

	public event SelectionRendererEventHandler RenderHotTracking
	{
		add
		{
			SelectionRendererEventHandler selectionRendererEventHandler = _2768;
			SelectionRendererEventHandler selectionRendererEventHandler2;
			do
			{
				selectionRendererEventHandler2 = selectionRendererEventHandler;
				SelectionRendererEventHandler value2 = (SelectionRendererEventHandler)Delegate.Combine(selectionRendererEventHandler2, value);
				selectionRendererEventHandler = Interlocked.CompareExchange(ref _2768, value2, selectionRendererEventHandler2);
			}
			while ((object)selectionRendererEventHandler != selectionRendererEventHandler2);
		}
		remove
		{
			SelectionRendererEventHandler selectionRendererEventHandler = _2768;
			SelectionRendererEventHandler selectionRendererEventHandler2;
			do
			{
				selectionRendererEventHandler2 = selectionRendererEventHandler;
				SelectionRendererEventHandler value2 = (SelectionRendererEventHandler)Delegate.Remove(selectionRendererEventHandler2, value);
				selectionRendererEventHandler = Interlocked.CompareExchange(ref _2768, value2, selectionRendererEventHandler2);
			}
			while ((object)selectionRendererEventHandler != selectionRendererEventHandler2);
		}
	}

	public event ConnectorRendererEventHandler RenderConnector
	{
		add
		{
			ConnectorRendererEventHandler connectorRendererEventHandler = _2769;
			ConnectorRendererEventHandler connectorRendererEventHandler2;
			do
			{
				connectorRendererEventHandler2 = connectorRendererEventHandler;
				ConnectorRendererEventHandler value2 = (ConnectorRendererEventHandler)Delegate.Combine(connectorRendererEventHandler2, value);
				connectorRendererEventHandler = Interlocked.CompareExchange(ref _2769, value2, connectorRendererEventHandler2);
			}
			while ((object)connectorRendererEventHandler != connectorRendererEventHandler2);
		}
		remove
		{
			ConnectorRendererEventHandler connectorRendererEventHandler = _2769;
			ConnectorRendererEventHandler connectorRendererEventHandler2;
			do
			{
				connectorRendererEventHandler2 = connectorRendererEventHandler;
				ConnectorRendererEventHandler value2 = (ConnectorRendererEventHandler)Delegate.Remove(connectorRendererEventHandler2, value);
				connectorRendererEventHandler = Interlocked.CompareExchange(ref _2769, value2, connectorRendererEventHandler2);
			}
			while ((object)connectorRendererEventHandler != connectorRendererEventHandler2);
		}
	}

	public event TreeBackgroundRendererEventHandler RenderTreeBackground
	{
		add
		{
			TreeBackgroundRendererEventHandler treeBackgroundRendererEventHandler = _276A;
			TreeBackgroundRendererEventHandler treeBackgroundRendererEventHandler2;
			do
			{
				treeBackgroundRendererEventHandler2 = treeBackgroundRendererEventHandler;
				TreeBackgroundRendererEventHandler value2 = (TreeBackgroundRendererEventHandler)Delegate.Combine(treeBackgroundRendererEventHandler2, value);
				treeBackgroundRendererEventHandler = Interlocked.CompareExchange(ref _276A, value2, treeBackgroundRendererEventHandler2);
			}
			while ((object)treeBackgroundRendererEventHandler != treeBackgroundRendererEventHandler2);
		}
		remove
		{
			TreeBackgroundRendererEventHandler treeBackgroundRendererEventHandler = _276A;
			TreeBackgroundRendererEventHandler treeBackgroundRendererEventHandler2;
			do
			{
				treeBackgroundRendererEventHandler2 = treeBackgroundRendererEventHandler;
				TreeBackgroundRendererEventHandler value2 = (TreeBackgroundRendererEventHandler)Delegate.Remove(treeBackgroundRendererEventHandler2, value);
				treeBackgroundRendererEventHandler = Interlocked.CompareExchange(ref _276A, value2, treeBackgroundRendererEventHandler2);
			}
			while ((object)treeBackgroundRendererEventHandler != treeBackgroundRendererEventHandler2);
		}
	}

	public event DragDropMarkerRendererEventHandler RenderDragDropMarker
	{
		add
		{
			DragDropMarkerRendererEventHandler dragDropMarkerRendererEventHandler = _276B;
			DragDropMarkerRendererEventHandler dragDropMarkerRendererEventHandler2;
			do
			{
				dragDropMarkerRendererEventHandler2 = dragDropMarkerRendererEventHandler;
				DragDropMarkerRendererEventHandler value2 = (DragDropMarkerRendererEventHandler)Delegate.Combine(dragDropMarkerRendererEventHandler2, value);
				dragDropMarkerRendererEventHandler = Interlocked.CompareExchange(ref _276B, value2, dragDropMarkerRendererEventHandler2);
			}
			while ((object)dragDropMarkerRendererEventHandler != dragDropMarkerRendererEventHandler2);
		}
		remove
		{
			DragDropMarkerRendererEventHandler dragDropMarkerRendererEventHandler = _276B;
			DragDropMarkerRendererEventHandler dragDropMarkerRendererEventHandler2;
			do
			{
				dragDropMarkerRendererEventHandler2 = dragDropMarkerRendererEventHandler;
				DragDropMarkerRendererEventHandler value2 = (DragDropMarkerRendererEventHandler)Delegate.Remove(dragDropMarkerRendererEventHandler2, value);
				dragDropMarkerRendererEventHandler = Interlocked.CompareExchange(ref _276B, value2, dragDropMarkerRendererEventHandler2);
			}
			while ((object)dragDropMarkerRendererEventHandler != dragDropMarkerRendererEventHandler2);
		}
	}

	public event ColumnHeaderRendererEventHandler RenderColumnHeader
	{
		add
		{
			ColumnHeaderRendererEventHandler columnHeaderRendererEventHandler = _276C;
			ColumnHeaderRendererEventHandler columnHeaderRendererEventHandler2;
			do
			{
				columnHeaderRendererEventHandler2 = columnHeaderRendererEventHandler;
				ColumnHeaderRendererEventHandler value2 = (ColumnHeaderRendererEventHandler)Delegate.Combine(columnHeaderRendererEventHandler2, value);
				columnHeaderRendererEventHandler = Interlocked.CompareExchange(ref _276C, value2, columnHeaderRendererEventHandler2);
			}
			while ((object)columnHeaderRendererEventHandler != columnHeaderRendererEventHandler2);
		}
		remove
		{
			ColumnHeaderRendererEventHandler columnHeaderRendererEventHandler = _276C;
			ColumnHeaderRendererEventHandler columnHeaderRendererEventHandler2;
			do
			{
				columnHeaderRendererEventHandler2 = columnHeaderRendererEventHandler;
				ColumnHeaderRendererEventHandler value2 = (ColumnHeaderRendererEventHandler)Delegate.Remove(columnHeaderRendererEventHandler2, value);
				columnHeaderRendererEventHandler = Interlocked.CompareExchange(ref _276C, value2, columnHeaderRendererEventHandler2);
			}
			while ((object)columnHeaderRendererEventHandler != columnHeaderRendererEventHandler2);
		}
	}

	public event NodeRendererEventHandler RenderTileGroupLine
	{
		add
		{
			NodeRendererEventHandler nodeRendererEventHandler = _276D;
			NodeRendererEventHandler nodeRendererEventHandler2;
			do
			{
				nodeRendererEventHandler2 = nodeRendererEventHandler;
				NodeRendererEventHandler value2 = (NodeRendererEventHandler)Delegate.Combine(nodeRendererEventHandler2, value);
				nodeRendererEventHandler = Interlocked.CompareExchange(ref _276D, value2, nodeRendererEventHandler2);
			}
			while ((object)nodeRendererEventHandler != nodeRendererEventHandler2);
		}
		remove
		{
			NodeRendererEventHandler nodeRendererEventHandler = _276D;
			NodeRendererEventHandler nodeRendererEventHandler2;
			do
			{
				nodeRendererEventHandler2 = nodeRendererEventHandler;
				NodeRendererEventHandler value2 = (NodeRendererEventHandler)Delegate.Remove(nodeRendererEventHandler2, value);
				nodeRendererEventHandler = Interlocked.CompareExchange(ref _276D, value2, nodeRendererEventHandler2);
			}
			while ((object)nodeRendererEventHandler != nodeRendererEventHandler2);
		}
	}

	public TreeRenderer()
	{
	}

	public virtual void DrawNodeBackground(NodeRendererEventArgs e)
	{
		OnRenderNodeBackground(e);
	}

	protected virtual void OnRenderNodeBackground(NodeRendererEventArgs e)
	{
		if (_2761 != null)
		{
			_2761(this, e);
		}
	}

	public virtual void DrawNodeExpandPart(NodeExpandPartRendererEventArgs e)
	{
		OnRenderNodeExpandPart(e);
	}

	protected virtual void OnRenderNodeExpandPart(NodeExpandPartRendererEventArgs e)
	{
		if (_2762 != null)
		{
			_2762(this, e);
		}
	}

	public virtual void DrawCellBackground(NodeCellRendererEventArgs e)
	{
		OnRenderCellBackground(e);
	}

	protected virtual void OnRenderCellBackground(NodeCellRendererEventArgs e)
	{
		if (_2763 != null)
		{
			_2763(this, e);
		}
	}

	public virtual void DrawCellCheckBox(NodeCellRendererEventArgs e)
	{
		OnRenderCellCheckBox(e);
	}

	protected virtual void OnRenderCellCheckBox(NodeCellRendererEventArgs e)
	{
		if (_2764 != null)
		{
			_2764(this, e);
		}
	}

	public virtual void DrawCellImage(NodeCellRendererEventArgs e)
	{
		OnRenderCellImage(e);
	}

	protected virtual void OnRenderCellImage(NodeCellRendererEventArgs e)
	{
		if (_2765 != null)
		{
			_2765(this, e);
		}
	}

	public virtual void DrawCellText(NodeCellRendererEventArgs e)
	{
		OnRenderCellText(e);
	}

	protected virtual void OnRenderCellText(NodeCellRendererEventArgs e)
	{
		if (_2766 != null)
		{
			_2766(this, e);
		}
	}

	public virtual void DrawSelection(SelectionRendererEventArgs e)
	{
		OnRenderSelection(e);
	}

	protected virtual void OnRenderSelection(SelectionRendererEventArgs e)
	{
		if (_2767 != null)
		{
			_2767(this, e);
		}
	}

	public virtual void DrawHotTracking(SelectionRendererEventArgs e)
	{
		OnRenderHotTracking(e);
	}

	protected virtual void OnRenderHotTracking(SelectionRendererEventArgs e)
	{
		if (_2768 != null)
		{
			_2768(this, e);
		}
	}

	public virtual void DrawConnector(ConnectorRendererEventArgs e)
	{
		OnRenderConnector(e);
	}

	protected virtual void OnRenderConnector(ConnectorRendererEventArgs e)
	{
		if (_2769 != null)
		{
			_2769(this, e);
		}
	}

	public virtual void DrawTreeBackground(TreeBackgroundRendererEventArgs e)
	{
		OnRenderTreeBackground(e);
	}

	protected virtual void OnRenderTreeBackground(TreeBackgroundRendererEventArgs e)
	{
		if (_276A != null)
		{
			_276A(this, e);
		}
	}

	public virtual void DrawDragDropMarker(DragDropMarkerRendererEventArgs e)
	{
		OnRenderDragDropMarker(e);
	}

	protected virtual void OnRenderDragDropMarker(DragDropMarkerRendererEventArgs e)
	{
		if (_276B != null)
		{
			_276B(this, e);
		}
	}

	public virtual void DrawColumnHeader(ColumnHeaderRendererEventArgs e)
	{
		OnRenderColumnHeader(e);
	}

	protected virtual void OnRenderColumnHeader(ColumnHeaderRendererEventArgs e)
	{
		if (_276C != null)
		{
			_276C(this, e);
		}
	}

	public virtual void DrawTileGroupLine(NodeRendererEventArgs e)
	{
		OnRenderTileGroupLine(e);
	}

	protected virtual void OnRenderTileGroupLine(NodeRendererEventArgs e)
	{
		if (_276D != null)
		{
			_276D(this, e);
		}
	}
}
