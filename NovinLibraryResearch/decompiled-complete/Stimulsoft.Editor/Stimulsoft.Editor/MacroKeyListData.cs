using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class MacroKeyListData : KeyListData
{
	private string fullName;

	private KeyEventType paramType;

	private string paramStr;

	public KeyEventType ParamType
	{
		get
		{
			if (Param is int)
			{
				return KeyEventType.IntParam;
			}
			if (Param is char)
			{
				return KeyEventType.CharParam;
			}
			if (Param is SelectionType)
			{
				return KeyEventType.SelectionParam;
			}
			return KeyEventType.None;
		}
		set
		{
			paramType = value;
		}
	}

	public string ParamStr
	{
		get
		{
			if (Param == null)
			{
				return string.Empty;
			}
			if (!(Param is char))
			{
				return Param.ToString();
			}
			return ((int)(char)Param).ToString();
		}
		set
		{
			paramStr = value;
		}
	}

	public string FullName
	{
		get
		{
			if (Action != null)
			{
				return Action.Target.GetType().Name + "." + Action.Method.Name;
			}
			if (ActionEx != null)
			{
				return ActionEx.Target.GetType().Name + "." + ActionEx.Method.Name;
			}
			return string.Empty;
		}
		set
		{
			fullName = value;
		}
	}

	public MacroKeyListData(Keys keyData, KeyEvent action, KeyEventEx actionEx, object param, int state, int leaveState)
		: base(keyData, action, actionEx, param, state, leaveState)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public MacroKeyListData()
	{
	}

	private object FindTarget(object owner, string target)
	{
		if (owner != null)
		{
			if (owner.GetType().Name == target)
			{
				return owner;
			}
			PropertyInfo[] properties = owner.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.PropertyType.Name == target || propertyInfo.PropertyType.Name == "I" + target)
				{
					return propertyInfo.GetValue(owner, null);
				}
			}
		}
		return null;
	}

	public void Fixup(ISyntaxEdit owner)
	{
		if (fullName != null && fullName != string.Empty)
		{
			Action = null;
			ActionEx = null;
			int num = fullName.LastIndexOf('.');
			if (num >= 0)
			{
				EventHandlers handlers = owner.KeyList.Handlers;
				object obj = FindTarget(owner, fullName.Substring(0, num));
				if (obj != null)
				{
					string text = fullName.Substring(num + 1);
					FieldInfo[] fields = handlers.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
					foreach (FieldInfo fieldInfo in fields)
					{
						object value = fieldInfo.GetValue(handlers);
						if (value is KeyEvent)
						{
							KeyEvent keyEvent = (KeyEvent)value;
							if (keyEvent.Target == obj && keyEvent.Method.Name == text)
							{
								Action = keyEvent;
								break;
							}
						}
						if (value is KeyEventEx)
						{
							KeyEventEx keyEventEx = (KeyEventEx)value;
							if (keyEventEx.Target == obj && keyEventEx.Method.Name == text)
							{
								ActionEx = keyEventEx;
								break;
							}
						}
					}
				}
			}
		}
		if (paramType == KeyEventType.None || !(paramStr != string.Empty))
		{
			return;
		}
		switch (paramType)
		{
		case KeyEventType.IntParam:
			Param = int.Parse(paramStr);
			break;
		case KeyEventType.CharParam:
			Param = (char)int.Parse(paramStr);
			break;
		case KeyEventType.SelectionParam:
			if (string.Compare(paramStr, SelectionType.Block.ToString(), ignoreCase: true) == 0)
			{
				Param = SelectionType.Block;
			}
			else if (string.Compare(paramStr, SelectionType.Stream.ToString(), ignoreCase: true) == 0)
			{
				Param = SelectionType.Stream;
			}
			else
			{
				Param = SelectionType.None;
			}
			break;
		}
	}
}
