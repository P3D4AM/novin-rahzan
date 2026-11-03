using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class DialogLauncherAccessibleObject : AccessibleObject
{
	private RibbonBar _0AC3;

	private bool _104A;

	public override string Name
	{
		get
		{
			if (_0AC3 == null)
			{
				return "";
			}
			if (_0AC3.DialogLauncherAccessibleName != "")
			{
				return _0AC3.DialogLauncherAccessibleName;
			}
			if (((Control)_0AC3).Text != null)
			{
				return ((Control)_0AC3).Text.Replace("&", "");
			}
			return "";
		}
		set
		{
			_0AC3.DialogLauncherAccessibleName = value;
		}
	}

	public override string Description => ((Control)_0AC3).Text + " Dialog Launcher";

	public override AccessibleRole Role
	{
		get
		{
			if (_0AC3 != null && ((Control)_0AC3).IsAccessible)
			{
				return (AccessibleRole)43;
			}
			return (AccessibleRole)0;
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
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 == null)
			{
				return (AccessibleStates)1;
			}
			AccessibleStates val = (AccessibleStates)0;
			if (!((Control)_0AC3).IsAccessible)
			{
				return (AccessibleStates)1;
			}
			if (!((Control)_0AC3).Visible)
			{
				return (AccessibleStates)(val | 0x8000);
			}
			if (!((Control)_0AC3).Enabled)
			{
				return (AccessibleStates)1;
			}
			val = (AccessibleStates)(val | 0x40000000);
			if (_104A)
			{
				return (AccessibleStates)(val | 0x84);
			}
			return (AccessibleStates)(val | 0x100000);
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
			return ((Control)_0AC3).AccessibilityObject;
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
			return ((Control)_0AC3).RectangleToScreen(_0AC3.ჶ);
		}
	}

	public override string DefaultAction => "Press";

	public override string KeyboardShortcut => "";

	public DialogLauncherAccessibleObject(RibbonBar owner)
	{
		_0AC3 = owner;
		_0AC3.DialogLauncherMouseEnter += _226C;
		_0AC3.DialogLauncherMouseLeave += _226D;
	}

	private void _226C(object _٠, EventArgs ؿ)
	{
		_104A = true;
	}

	private void _226D(object _٠, EventArgs ؿ)
	{
		_104A = false;
	}

	public override int GetChildCount()
	{
		return 0;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		return null;
	}

	public override void DoDefaultAction()
	{
		if (_0AC3 != null)
		{
			_0AC3.DoLaunchDialog();
			((AccessibleObject)this).DoDefaultAction();
		}
	}

	public override AccessibleObject GetSelected()
	{
		if (_0AC3 == null)
		{
			return ((AccessibleObject)this).GetSelected();
		}
		if (_104A)
		{
			return (AccessibleObject)(object)this;
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
		Point pt = ((Control)_0AC3).PointToClient(point);
		if (_0AC3.ჶ.Contains(pt))
		{
			return (AccessibleObject)(object)this;
		}
		return ((AccessibleObject)this).HitTest(x, y);
	}

	public override AccessibleObject Navigate(AccessibleNavigation navdir)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((AccessibleObject)this).Navigate(navdir);
	}
}
