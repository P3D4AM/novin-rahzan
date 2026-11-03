using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class PropertyValueListEventArgs : EventArgs
{
	public readonly string PropertyName;

	public readonly object TargetComponent;

	public bool IsListValid;

	public List<string> ValueList;

	public readonly PropertyDescriptor PropertyDescriptor;

	public PropertyValueListEventArgs(string propertyName, object targetComponent, PropertyDescriptor descriptor)
	{
		PropertyName = propertyName;
		TargetComponent = targetComponent;
		PropertyDescriptor = descriptor;
	}
}
