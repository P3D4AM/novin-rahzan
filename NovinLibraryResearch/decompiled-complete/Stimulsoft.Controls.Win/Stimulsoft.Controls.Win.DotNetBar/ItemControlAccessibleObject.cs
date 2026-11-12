using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemControlAccessibleObject : ControlAccessibleObject
{
	private ItemControl m_Owner;

	protected ItemControl Owner => m_Owner;

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

	public override AccessibleStates State
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (m_Owner == null || ((Control)m_Owner).IsDisposed)
			{
				return (AccessibleStates)0;
			}
			return (AccessibleStates)0;
		}
	}

	public ItemControlAccessibleObject(ItemControl owner)
		: base((Control)(object)owner)
	{
		m_Owner = owner;
	}

	internal void GenerateEvent(BaseItem sender, AccessibleEvents e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		int num = m_Owner.GetBaseItemContainer().SubItems.IndexOf(sender);
		if (num >= 0 && m_Owner != null && !((Control)m_Owner).IsDisposed)
		{
			m_Owner.InternalAccessibilityNotifyClients(e, num);
		}
	}

	public override int GetChildCount()
	{
		if (m_Owner != null && !((Control)m_Owner).IsDisposed && m_Owner.GetBaseItemContainer() != null)
		{
			return m_Owner.GetBaseItemContainer().SubItems.Count;
		}
		return 0;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (m_Owner != null && !((Control)m_Owner).IsDisposed && m_Owner.GetBaseItemContainer() != null)
		{
			return m_Owner.GetBaseItemContainer().SubItems[iIndex].AccessibleObject;
		}
		return null;
	}
}
