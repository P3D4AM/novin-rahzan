using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class ButtonXAccessibleObject : ControlAccessibleObject
{
	private ButtonX _0AC3;

	protected ButtonX Owner => _0AC3;

	public override AccessibleStates State
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			AccessibleStates val = _0AC3.InternalItem.AccessibleObject.State;
			if (((Control)_0AC3).Focused)
			{
				val = (AccessibleStates)(val | 4);
			}
			return val;
		}
	}

	public override string DefaultAction
	{
		get
		{
			if (_0AC3 != null && ((Control)_0AC3).AccessibleDefaultActionDescription != "")
			{
				return ((Control)_0AC3).AccessibleDefaultActionDescription;
			}
			return "Click";
		}
	}

	public ButtonXAccessibleObject(ButtonX owner)
		: base((Control)(object)owner)
	{
		_0AC3 = owner;
	}

	public override int GetChildCount()
	{
		if (_0AC3 != null && !((Control)_0AC3).IsDisposed)
		{
			return _0AC3.InternalItem.AccessibleObject.GetChildCount();
		}
		return 0;
	}

	public override AccessibleObject GetChild(int iIndex)
	{
		if (_0AC3 != null && !((Control)_0AC3).IsDisposed)
		{
			return _0AC3.InternalItem.AccessibleObject.GetChild(iIndex);
		}
		return null;
	}

	public override void DoDefaultAction()
	{
		if (_0AC3 != null)
		{
			_0AC3.PerformClick();
		}
	}
}
