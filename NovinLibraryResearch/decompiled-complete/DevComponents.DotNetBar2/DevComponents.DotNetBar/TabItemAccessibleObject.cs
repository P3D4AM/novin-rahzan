using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class TabItemAccessibleObject : AccessibleObject
{
	private TabItem _ᕹ;

	public TabItem Item
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

	public override string Description => Item.Name;

	public override string Value
	{
		get
		{
			return Item.Text;
		}
		set
		{
			Item.Text = value;
		}
	}

	public override AccessibleObject Parent => ((Control)Item.Parent).AccessibilityObject;

	public override Rectangle Bounds
	{
		get
		{
			Rectangle displayRectangle = Item.DisplayRectangle;
			Point location = ((Control)Item.Parent).PointToScreen(displayRectangle.Location);
			return new Rectangle(location, displayRectangle.Size);
		}
	}

	public override AccessibleRole Role => (AccessibleRole)36;

	public TabItemAccessibleObject(TabItem tabItem)
	{
		if (tabItem == null)
		{
			throw new ArgumentNullException("tabItem");
		}
		Item = tabItem;
	}

	public override AccessibleObject Navigate(AccessibleNavigation navdir)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((AccessibleObject)this).Navigate(navdir);
	}

	public override void Select(AccessibleSelection flags)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Item.Parent.SelectedTab = Item;
		((AccessibleObject)this).Select(flags);
	}
}
