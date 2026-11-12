using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DialogLauncherAccessibleObject : AccessibleObject
{
	private RibbonBar m_Owner;

	private bool m_Hot;

	public override string Name
	{
		get
		{
			if (m_Owner == null)
			{
				return "";
			}
			if (m_Owner.DialogLauncherAccessibleName != "")
			{
				return m_Owner.DialogLauncherAccessibleName;
			}
			if (((Control)m_Owner).Text != null)
			{
				return ((Control)m_Owner).Text.Replace("&", "");
			}
			return "";
		}
		set
		{
			m_Owner.DialogLauncherAccessibleName = value;
		}
	}

	public override string Description => ((Control)m_Owner).Text + " Dialog Launcher";

	public override AccessibleRole Role
	{
		get
		{
			if (m_Owner != null && ((Control)m_Owner).IsAccessible)
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
			if (m_Owner == null)
			{
				return (AccessibleStates)1;
			}
			AccessibleStates val = (AccessibleStates)0;
			if (!((Control)m_Owner).IsAccessible)
			{
				return (AccessibleStates)1;
			}
			if (!((Control)m_Owner).Visible)
			{
				return (AccessibleStates)(val | 0x8000);
			}
			if (!((Control)m_Owner).Enabled)
			{
				return (AccessibleStates)1;
			}
			val = (AccessibleStates)(val | 0x40000000);
			if (m_Hot)
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
			if (m_Owner == null)
			{
				return null;
			}
			return ((Control)m_Owner).AccessibilityObject;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			if (m_Owner == null)
			{
				return Rectangle.Empty;
			}
			return ((Control)m_Owner).RectangleToScreen(m_Owner.DialogLauncherRect);
		}
	}

	public override string DefaultAction => "Press";

	public override string KeyboardShortcut => "";

	public DialogLauncherAccessibleObject(RibbonBar owner)
	{
		m_Owner = owner;
		m_Owner.DialogLauncherMouseEnter += OnMouseEnter;
		m_Owner.DialogLauncherMouseLeave += OnMouseLeave;
	}

	private void OnMouseEnter(object sender, EventArgs e)
	{
		m_Hot = true;
	}

	private void OnMouseLeave(object sender, EventArgs e)
	{
		m_Hot = false;
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
		if (m_Owner != null)
		{
			m_Owner.DoLaunchDialog();
			((AccessibleObject)this).DoDefaultAction();
		}
	}

	public override AccessibleObject GetSelected()
	{
		if (m_Owner == null)
		{
			return ((AccessibleObject)this).GetSelected();
		}
		if (m_Hot)
		{
			return (AccessibleObject)(object)this;
		}
		return ((AccessibleObject)this).GetSelected();
	}

	public override AccessibleObject HitTest(int x, int y)
	{
		if (m_Owner == null)
		{
			return ((AccessibleObject)this).HitTest(x, y);
		}
		Point point = new Point(x, y);
		Point pt = ((Control)m_Owner).PointToClient(point);
		if (m_Owner.DialogLauncherRect.Contains(pt))
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
