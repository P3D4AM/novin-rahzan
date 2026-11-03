using System;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class ConvertValueEventArgs : EventArgs
{
	public string StringValue;

	public object TypedValue;

	public readonly string PropertyName;

	public readonly object TargetComponent;

	public bool IsConverted;

	public readonly PropertyDescriptor PropertyDescriptor;

	public ConvertValueEventArgs(string stringValue, object typedValue, string propertyName, object targetComponent, PropertyDescriptor descriptor)
	{
		StringValue = stringValue;
		TypedValue = typedValue;
		PropertyName = propertyName;
		TargetComponent = targetComponent;
		PropertyDescriptor = descriptor;
	}
}
