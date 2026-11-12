using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Design.Styles;

internal sealed class StiStylesDragDropAction : IDisposable
{
	internal enum DropPosition
	{
		Up,
		Down
	}

	private StiStyleDesigner styleWindow;

	private StiStylesTreeView currentTreeView;

	private bool isDragDropCheckStarting;

	private Point dragDropCheckPos;

	private TreeNode lastDropNode;

	private TreeNode dragNode;

	private bool isHandledDrop;

	public bool IsDragDropActive { get; private set; }

	public void Dispose()
	{
		styleWindow = null;
		currentTreeView = null;
		lastDropNode = null;
		dragNode = null;
	}

	private void DragDropCollectEvents()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		isDragDropCheckStarting = false;
		if (currentTreeView != null)
		{
			((Control)currentTreeView).MouseMove -= new MouseEventHandler(OnDragDropMouseMove);
			((Control)currentTreeView).MouseUp -= new MouseEventHandler(OnDragDropMouseUp);
			((Control)currentTreeView).MouseEnter -= OnDragDropMouseEnter;
			((Control)currentTreeView).MouseLeave -= OnDragDropMouseLeave;
		}
	}

	private void StartDragDrop()
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		dragNode = ((TreeView)currentTreeView).SelectedNode;
		if (dragNode == null || !(dragNode.Tag is StiBaseStyle))
		{
			dragNode = null;
			return;
		}
		StiBaseStyle stiBaseStyle = (StiBaseStyle)dragNode.Tag;
		isHandledDrop = false;
		IsDragDropActive = true;
		TreeNode parent = dragNode.Parent;
		int num = ((parent != null) ? parent.Nodes.IndexOf(dragNode) : ((TreeView)currentTreeView).Nodes.IndexOf(dragNode));
		((TreeView)currentTreeView).SelectedNode = null;
		if (parent != null)
		{
			parent.Nodes.RemoveAt(num);
		}
		else
		{
			((TreeView)currentTreeView).Nodes.RemoveAt(num);
		}
		lastDropNode = null;
		bool allowDrop = ((Control)styleWindow).AllowDrop;
		((Control)styleWindow).DragEnter += new DragEventHandler(OnDragEnter);
		((Control)styleWindow).DragLeave += OnDragLeave;
		((Control)styleWindow).DragOver += new DragEventHandler(OnDragOver);
		((Control)styleWindow).DragDrop += new DragEventHandler(OnDrop);
		((Control)styleWindow).AllowDrop = true;
		((Control)styleWindow).DoDragDrop((object)stiBaseStyle, (DragDropEffects)2);
		if (!isHandledDrop)
		{
			if (parent != null)
			{
				parent.Nodes.Insert(num, dragNode);
			}
			else
			{
				((TreeView)currentTreeView).Nodes.Insert(num, dragNode);
			}
		}
		((Control)styleWindow).DragEnter -= new DragEventHandler(OnDragEnter);
		((Control)styleWindow).DragLeave -= OnDragLeave;
		((Control)styleWindow).DragOver -= new DragEventHandler(OnDragOver);
		((Control)styleWindow).DragDrop -= new DragEventHandler(OnDrop);
		((Control)styleWindow).AllowDrop = allowDrop;
		IsDragDropActive = false;
		((TreeView)currentTreeView).SelectedNode = dragNode;
		dragNode = null;
		lastDropNode = null;
		dragNode = null;
	}

	private DragDropEffects ProcessDragOver(DragEventArgs e)
	{
		Point point = ((Control)currentTreeView).PointToClient(new Point(e.X, e.Y));
		if (new Rectangle(0, 0, ((Control)currentTreeView).Width, ((Control)currentTreeView).Height).Contains(point))
		{
			TreeNode nodeAt = ((TreeView)currentTreeView).GetNodeAt(point);
			if (lastDropNode != nodeAt)
			{
				lastDropNode = nodeAt;
			}
			if (nodeAt == null)
			{
				if (((TreeView)currentTreeView).SelectedNode != null)
				{
					((TreeView)currentTreeView).SelectedNode = null;
				}
			}
			else
			{
				((TreeView)currentTreeView).SelectedNode = nodeAt;
			}
			return (DragDropEffects)2;
		}
		lastDropNode = null;
		return (DragDropEffects)0;
	}

	private void OnTreeViewLeftButtonDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		if ((int)e.Button != 1048576 || !styleWindow.AllowMoveStyles)
		{
			return;
		}
		if (!isDragDropCheckStarting)
		{
			TreeNode nodeAt = ((TreeView)currentTreeView).GetNodeAt(e.Location);
			if (nodeAt != null && nodeAt.Tag is StiBaseStyle)
			{
				dragDropCheckPos = e.Location;
				isDragDropCheckStarting = true;
				((Control)currentTreeView).MouseMove += new MouseEventHandler(OnDragDropMouseMove);
				((Control)currentTreeView).MouseUp += new MouseEventHandler(OnDragDropMouseUp);
				((Control)currentTreeView).MouseEnter += OnDragDropMouseEnter;
				((Control)currentTreeView).MouseLeave += OnDragDropMouseLeave;
			}
		}
		else
		{
			DragDropCollectEvents();
		}
	}

	private void OnDragDropMouseMove(object sender, MouseEventArgs e)
	{
		Point location = e.Location;
		if (location.X < 0 || location.Y < 0 || location.X > ((Control)currentTreeView).Width || location.Y > ((Control)currentTreeView).Height)
		{
			DragDropCollectEvents();
			return;
		}
		double num = location.X - dragDropCheckPos.X;
		double num2 = location.Y - dragDropCheckPos.Y;
		int i = StiScale.I10;
		if (num >= (double)i || num <= (double)(-i) || num2 >= (double)i || num2 <= (double)(-i))
		{
			DragDropCollectEvents();
			StartDragDrop();
		}
	}

	private void OnDragDropMouseUp(object sender, MouseEventArgs e)
	{
		DragDropCollectEvents();
	}

	private void OnDragDropMouseLeave(object sender, EventArgs e)
	{
		DragDropCollectEvents();
	}

	private void OnDragDropMouseEnter(object sender, EventArgs e)
	{
		DragDropCollectEvents();
	}

	private void OnDragEnter(object sender, DragEventArgs e)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		e.Effect = ProcessDragOver(e);
	}

	private void OnDragLeave(object sender, EventArgs e)
	{
		if (((TreeView)currentTreeView).SelectedNode != null)
		{
			((TreeView)currentTreeView).SelectedNode = null;
		}
		lastDropNode = null;
	}

	private void OnDragOver(object sender, DragEventArgs e)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		e.Effect = ProcessDragOver(e);
	}

	private void OnDrop(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
		if (dragNode != null && lastDropNode != null)
		{
			isHandledDrop = true;
			styleWindow.ProcessDropStyleTo(dragNode, lastDropNode, isUp: true);
		}
	}

	public StiStylesDragDropAction(StiStyleDesigner styleWindow)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		this.styleWindow = styleWindow;
		currentTreeView = styleWindow.TreeViewStyles;
		((Control)currentTreeView).MouseDown += new MouseEventHandler(OnTreeViewLeftButtonDown);
	}
}
