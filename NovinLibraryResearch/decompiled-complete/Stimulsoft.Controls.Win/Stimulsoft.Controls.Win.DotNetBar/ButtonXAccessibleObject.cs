using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ButtonXAccessibleObject : ControlAccessibleObject
{
	private ButtonX m_Owner;

	protected ButtonX Owner => m_Owner;

	public override AccessibleRole Role
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (m_Owner == null || ((Control)m_Owner).IsDisposed)
			{
				return (AccessibleRole)0;
			}
			return ((Control)m_Owner).AccessibleRole;
		}
	}

	public override AccessibleObject Parent
	{
		get
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed)
			{
				return ((Control)m_Owner).Parent.AccessibilityObject;
			}
			return null;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			if (m_Owner != null && !((Control)m_Owner).IsDisposed && ((Control)m_Owner).Parent != null)
			{
				return ((Control)m_Owner).Parent.RectangleToScreen(((Control)m_Owner).Bounds);
			}
			return Rectangle.Empty;
		}
	}

	public override AccessibleStates State => m_Owner.InternalItem.AccessibleObject.State;

	public override string Value
	{
		get
		{
			return ((Control)m_Owner).Text;
		}
		set
		{
			((Control)m_Owner).Text = value;
		}
	}

	public override string DefaultAction
	{
		get
		{
			if (m_Owner != null && ((Control)m_Owner).AccessibleDefaultActionDescription != "")
			{
				return ((Control)m_Owner).AccessibleDefaultActionDescription;
			}
			return "Click";
		}
	}

	public ButtonXAccessibleObject(ButtonX owner)
		: base((Control)(object)owner)
	{
		m_Owner = owner;
	}

	public override int GetChildCount()
	{
		if (m_Owner != null && !((Control)m_Owner).IsDisposed)
		{
			return m_Owner.InternalItem.AccessibleObject.GetChildCount();
		}
		return 0;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (m_Owner != null && !((Control)m_Owner).IsDisposed)
		{
			return m_Owner.InternalItem.AccessibleObject.GetChild(iIndex);
		}
		return null;
	}

	public override void DoDefaultAction()
	{
		if (m_Owner != null)
		{
			m_Owner.PerformClick();
		}
	}
}
