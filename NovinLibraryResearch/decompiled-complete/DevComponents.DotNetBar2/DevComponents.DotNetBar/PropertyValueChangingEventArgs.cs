using System;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class PropertyValueChangingEventArgs : EventArgs
{
	public readonly string PropertyName;

	public readonly object NewValue;

	public bool Handled;

	public readonly PropertyDescriptor PropertyDescriptor;

	public readonly string PropertyPath;

	public PropertyValueChangingEventArgs(string propertyName, object newValue)
	{
		PropertyName = propertyName;
		NewValue = newValue;
		PropertyDescriptor = null;
		PropertyPath = null;
	}

	public PropertyValueChangingEventArgs(string propertyName, object newValue, PropertyDescriptor propertyDesc, string propertyPath)
	{
		PropertyName = propertyName;
		NewValue = newValue;
		PropertyDescriptor = propertyDesc;
		PropertyPath = propertyPath;
	}
}
