using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class ItemControlAccessibleObject : ControlAccessibleObject
{
	private ItemControl _0AC3;

	protected ItemControl Owner => _0AC3;

	public override AccessibleRole Role
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
			{
				return (AccessibleRole)0;
			}
			return ((Control)_0AC3).AccessibleRole;
		}
	}

	public override AccessibleObject Parent
	{
		get
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed)
			{
				return ((Control)_0AC3).Parent.AccessibilityObject;
			}
			return null;
		}
	}

	public override Rectangle Bounds
	{
		get
		{
			if (_0AC3 != null && !((Control)_0AC3).IsDisposed && ((Control)_0AC3).Parent != null)
			{
				return ((Control)_0AC3).Parent.RectangleToScreen(((Control)_0AC3).Bounds);
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
			if (_0AC3 == null || ((Control)_0AC3).IsDisposed)
			{
				return (AccessibleStates)0;
			}
			return (AccessibleStates)0;
		}
	}

	public ItemControlAccessibleObject(ItemControl owner)
		: base((Control)(object)owner)
	{
		_0AC3 = owner;
	}

	internal void _၈(BaseItem _٠, AccessibleEvents ؿ)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		int num = _0AC3.GetBaseItemContainer().SubItems.IndexOf(_٠);
		if (num >= 0 && _0AC3 != null && !((Control)_0AC3).IsDisposed)
		{
			_0AC3.ਟ(ؿ, num);
		}
	}

	public override int GetChildCount()
	{
		if (_0AC3 != null && !((Control)_0AC3).IsDisposed && _0AC3.GetBaseItemContainer() != null)
		{
			return _0AC3.GetBaseItemContainer().SubItems.Count;
		}
		return 0;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (_0AC3 != null && !((Control)_0AC3).IsDisposed && _0AC3.GetBaseItemContainer() != null)
		{
			return _0AC3.GetBaseItemContainer().SubItems[iIndex].AccessibleObject;
		}
		return null;
	}
}
