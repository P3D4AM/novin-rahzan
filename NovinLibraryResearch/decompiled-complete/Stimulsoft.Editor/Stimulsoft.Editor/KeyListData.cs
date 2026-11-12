using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class KeyListData
{
	[XmlIgnore]
	public Keys KeyData;

	[XmlIgnore]
	public object Param;

	public int State;

	public int LeaveState;

	[XmlIgnore]
	public KeyEventEx ActionEx;

	[XmlIgnore]
	public KeyEvent Action;

	[XmlIgnore]
	public string EventName
	{
		get
		{
			if (Action != null)
			{
				return Action.Method.Name;
			}
			if (ActionEx != null)
			{
				return ActionEx.Method.Name;
			}
			return string.Empty;
		}
		set
		{
		}
	}

	public KeyListData()
	{
	}

	public KeyListData(Keys keyData, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		KeyData = keyData;
		Action = action;
		ActionEx = actionEx;
		Param = param;
		State = state;
		LeaveState = leaveState;
	}

	public override string ToString()
	{
		if (Action != null)
		{
			return Action.Target.GetType().Name + "." + Action.Method.Name;
		}
		if (ActionEx != null)
		{
			return ActionEx.Target.GetType().Name + "." + ActionEx.Method.Name + "(" + Param.ToString() + ")";
		}
		return base.ToString();
	}
}
