using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class TabStripAccessibleObject : ControlAccessibleObject
{
	private TabStrip _ᕹ;

	public TabStrip Item
	{
		get
		{
			return _ᕹ;
		}
		set
		{
			_ᕹ = value;
		}
	}

	public override string Description => ((Control)Item).AccessibleDescription;

	public override AccessibleObject Parent => ((Control)Item).Parent.AccessibilityObject;

	public override Rectangle Bounds
	{
		get
		{
			Rectangle displayRectangle = ((Control)Item).DisplayRectangle;
			displayRectangle.Location = ((Control)Item).PointToScreen(Point.Empty);
			return displayRectangle;
		}
	}

	public override AccessibleRole Role => ((Control)Item).AccessibleRole;

	public TabStripAccessibleObject(TabStrip tabStrip)
		: base((Control)(object)tabStrip)
	{
		if (tabStrip == null)
		{
			throw new ArgumentNullException("tabStrip");
		}
		Item = tabStrip;
	}

	public override int GetChildCount()
	{
		return Item.Tabs.Count;
	}

	public override AccessibleObject GetChild(int index)
	{
		return Item.Tabs[index].AccessibleObject;
	}

	public override AccessibleObject Navigate(AccessibleNavigation navdir)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((AccessibleObject)this).Navigate(navdir);
	}

	public override void Select(AccessibleSelection flags)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((AccessibleObject)this).Select(flags);
	}
}
