using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.AdvTree;

public class NodeAccessibleObject : AccessibleObject
{
	private Node __2487;

	internal Node _၉ => __2487;

	public override string Name
	{
		get
		{
			if (__2487 == null)
			{
				return "";
			}
			if (__2487.AccessibleName != "")
			{
				return __2487.AccessibleName;
			}
			return __2487.Text.Replace("&", "");
		}
		set
		{
			__2487.AccessibleName = value;
		}
	}

	public override string Description
	{
		get
		{
			if (__2487 == null)
			{
				return "";
			}
			if (__2487.AccessibleDescription != "")
			{
				return __2487.AccessibleDescription;
			}
			return ((AccessibleObject)this).Name + " Tree Node";
		}
	}

	public override AccessibleRole Role
	{
		get
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (__2487 == null || !__2487.IsAccessible)
			{
				return (AccessibleRole)0;
			}
			if ((int)__2487.AccessibleRole != -1)
			{
				return __2487.AccessibleRole;
			}
			return (AccessibleRole)36;
		}
	}

	public override AccessibleStates State
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (__2487 == null)
			{
				return (AccessibleStates)1;
			}
			AccessibleStates val = (AccessibleStates)0;
			if (!__2487.IsAccessible)
			{
				return (AccessibleStates)1;
			}
			if (!__2487.Visible)
			{
				return (AccessibleStates)(val | 0x8000);
			}
			if (__2487.IsSelected)
			{
				val = (AccessibleStates)2;
			}
			if (__2487.Expanded)
			{
				return (AccessibleStates)(val | 0x200);
			}
			return (AccessibleStates)(val | 0x400);
		}
	}

	public override AccessibleObject Parent
	{
		get
		{
			if (__2487 == null)
			{
				return null;
			}
			if (__2487.Parent != null)
			{
				return __2487.Parent.AccessibleObject;
			}
			Control treeControl = (Control)(object)__2487.TreeControl;
			if (treeControl != null)
			{
				return treeControl.AccessibilityObject;
			}
			return null;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			if (__2487 == null)
			{
				return Rectangle.Empty;
			}
			Control treeControl = (Control)(object)__2487.TreeControl;
			if (treeControl != null)
			{
				return treeControl.RectangleToScreen(__2487.Bounds);
			}
			return __2487.Bounds;
		}
	}

	public override string DefaultAction
	{
		get
		{
			if (__2487.AccessibleDefaultActionDescription != "")
			{
				return __2487.AccessibleDefaultActionDescription;
			}
			if (__2487.Expanded)
			{
				return "Collapse";
			}
			if (__2487.Nodes.Count > 0 || __2487.ExpandVisibility == eNodeExpandVisibility.Visible)
			{
				return "Expand";
			}
			return "Press";
		}
	}

	public override string KeyboardShortcut => "";

	public NodeAccessibleObject(Node owner)
	{
		__2487 = owner;
	}

	public override int GetChildCount()
	{
		if (__2487 == null || !__2487.HasChildNodes)
		{
			return 0;
		}
		return __2487.Nodes.Count;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (__2487 == null)
		{
			return null;
		}
		return __2487.Nodes[iIndex].AccessibleObject;
	}

	public override void DoDefaultAction()
	{
		if (__2487 != null)
		{
			__2487.Toggle();
			((AccessibleObject)this).DoDefaultAction();
		}
	}

	public override AccessibleObject GetSelected()
	{
		if (__2487 == null)
		{
			return ((AccessibleObject)this).GetSelected();
		}
		if (__2487.IsSelected)
		{
			return __2487.AccessibleObject;
		}
		return ((AccessibleObject)this).GetSelected();
	}

	public override AccessibleObject HitTest(int x, int y)
	{
		if (__2487 == null)
		{
			return ((AccessibleObject)this).HitTest(x, y);
		}
		Point point = new Point(x, y);
		AdvTree treeControl = __2487.TreeControl;
		if (treeControl == null)
		{
			return ((AccessibleObject)this).HitTest(x, y);
		}
		Point p = ((Control)treeControl).PointToClient(point);
		Node nodeAt = treeControl.GetNodeAt(p);
		if (nodeAt != null)
		{
			return nodeAt.AccessibleObject;
		}
		return ((AccessibleObject)this).HitTest(x, y);
	}

	public override AccessibleObject Navigate(AccessibleNavigation navDir)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		if (__2487 == null)
		{
			return ((AccessibleObject)this).Navigate(navDir);
		}
		Node node = null;
		if ((int)navDir == 2 || (int)navDir == 4 || (int)navDir == 5)
		{
			node = _175C.ᝩ(__2487);
		}
		else if ((int)navDir == 7)
		{
			node = _104B(__2487.Nodes, 0);
		}
		else if ((int)navDir == 8)
		{
			node = _104E(__2487.Nodes, __2487.Nodes.Count - 1);
		}
		else if ((int)navDir == 1 || (int)navDir == 3 || (int)navDir == 6)
		{
			node = _175C.ᝯ(__2487);
		}
		if (node != null)
		{
			return node.AccessibleObject;
		}
		return ((AccessibleObject)this).Navigate(navDir);
	}

	private Node _104B(NodeCollection _104C, int _104D)
	{
		int count = _104C.Count;
		for (int i = _104D; i < count; i++)
		{
			if (_104C[i].Visible)
			{
				return _104C[i];
			}
		}
		return null;
	}

	private Node _104E(NodeCollection _104C, int _104D)
	{
		for (int num = _104D; num >= 0; num--)
		{
			if (_104C[num].Visible)
			{
				return _104C[num];
			}
		}
		return null;
	}
}
