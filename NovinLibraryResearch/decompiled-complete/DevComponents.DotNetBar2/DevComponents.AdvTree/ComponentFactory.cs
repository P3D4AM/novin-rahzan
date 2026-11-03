using System;
using System.ComponentModel.Design;

namespace DevComponents.AdvTree;

public class ComponentFactory
{
	private IDesignerHost _17D8;

	public ComponentFactory(IDesignerHost designer)
	{
		_17D8 = designer;
	}

	public ComponentFactory()
	{
	}

	public object CreateComponent(Type type)
	{
		object obj = null;
		if (_17D8 != null)
		{
			return _17D8.CreateComponent(type);
		}
		return type.Assembly.CreateInstance(type.FullName);
	}
}
