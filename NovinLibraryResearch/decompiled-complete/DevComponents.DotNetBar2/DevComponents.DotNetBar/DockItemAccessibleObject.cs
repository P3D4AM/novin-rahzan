using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class DockItemAccessibleObject : AccessibleObject
{
	private DockContainerItem _0AC3;

	private bool _104A;

	internal DockContainerItem _၉ => _0AC3;

	public override string Name
	{
		get
		{
			if (_0AC3 == null)
			{
				return "";
			}
			if (_0AC3.AccessibleName != "")
			{
				return _0AC3.AccessibleName;
			}
			if (_0AC3.Text != null)
			{
				return _0AC3.Text.Replace("&", "");
			}
			return _0AC3.Tooltip;
		}
		set
		{
			_0AC3.AccessibleName = value;
		}
	}

	public override string Description
	{
		get
		{
			if (_0AC3 == null)
			{
				return "";
			}
			if (_0AC3.AccessibleDescription != "")
			{
				return _0AC3.AccessibleDescription;
			}
			return "";
		}
	}

	public override AccessibleRole Role
	{
		get
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 == null || !_0AC3.IsAccessible)
			{
				return (AccessibleRole)0;
			}
			if ((int)_0AC3.AccessibleRole != -1)
			{
				return _0AC3.AccessibleRole;
			}
			return (AccessibleRole)16;
		}
	}

	public override AccessibleStates State
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 == null)
			{
				return (AccessibleStates)1;
			}
			AccessibleStates val = (AccessibleStates)0;
			if (!_0AC3.IsAccessible)
			{
				return (AccessibleStates)1;
			}
			if (!_0AC3.Displayed || !_0AC3.Visible)
			{
				return (AccessibleStates)(val | 0x8000);
			}
			if (!_0AC3.GetEnabled())
			{
				return (AccessibleStates)1;
			}
			if (_0AC3.Selected)
			{
				return (AccessibleStates)(val | 2);
			}
			return (AccessibleStates)(val | 0x300000);
		}
	}

	public override AccessibleObject Parent
	{
		get
		{
			if (_0AC3 == null)
			{
				return null;
			}
			if (_0AC3.Parent != null && (!(_0AC3.Parent is GenericItemContainer) || !((GenericItemContainer)_0AC3.Parent).SystemContainer))
			{
				return _0AC3.Parent.AccessibleObject;
			}
			object containerControl = _0AC3.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				return val.AccessibilityObject;
			}
			return null;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			if (_0AC3 == null)
			{
				return Rectangle.Empty;
			}
			object containerControl = _0AC3.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				return val.RectangleToScreen(_0AC3.DisplayRectangle);
			}
			return _0AC3.Bounds;
		}
	}

	public override string DefaultAction
	{
		get
		{
			if (_0AC3.AccessibleDefaultActionDescription != "")
			{
				return _0AC3.AccessibleDefaultActionDescription;
			}
			return "Select";
		}
	}

	public override string KeyboardShortcut => _0AC3.ShortcutString;

	public DockItemAccessibleObject(DockContainerItem owner)
	{
		_0AC3 = owner;
	}

	public override int GetChildCount()
	{
		if (_0AC3 == null)
		{
			return 0;
		}
		if (_0AC3.Control == null)
		{
			return 0;
		}
		return 1;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (_0AC3 == null || iIndex < 0 || iIndex >= 1 || _0AC3.Control == null)
		{
			return null;
		}
		return _0AC3.Control.AccessibilityObject;
	}

	public override void DoDefaultAction()
	{
		if (_0AC3 != null)
		{
			_0AC3.Selected = true;
			((AccessibleObject)this).DoDefaultAction();
		}
	}

	public override AccessibleObject GetSelected()
	{
		if (_0AC3 == null)
		{
			return ((AccessibleObject)this).GetSelected();
		}
		return ((AccessibleObject)this).GetSelected();
	}

	public override AccessibleObject HitTest(int x, int y)
	{
		if (_0AC3 == null)
		{
			return ((AccessibleObject)this).HitTest(x, y);
		}
		Point point = new Point(x, y);
		if (_0AC3.Control != null)
		{
			Control control = _0AC3.Control;
			if (control != null)
			{
				Point pt = control.PointToClient(point);
				if (control.ClientRectangle.Contains(pt))
				{
					return control.AccessibilityObject;
				}
			}
		}
		return ((AccessibleObject)this).HitTest(x, y);
	}

	public override AccessibleObject Navigate(AccessibleNavigation navdir)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (_0AC3 == null)
		{
			return ((AccessibleObject)this).Navigate(navdir);
		}
		if (((int)navdir == 7 || (int)navdir == 8) && _0AC3.Control != null)
		{
			return _0AC3.Control.AccessibilityObject;
		}
		return ((AccessibleObject)this).Navigate(navdir);
	}
}
