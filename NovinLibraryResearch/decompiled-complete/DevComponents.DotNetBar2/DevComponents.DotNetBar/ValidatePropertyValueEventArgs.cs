using System;

namespace DevComponents.DotNetBar;

public class ValidatePropertyValueEventArgs : EventArgs
{
	public bool Cancel;

	public readonly string PropertyName;

	public readonly object NewValue;

	public readonly object Target;

	public string Message = string.Empty;

	public ValidatePropertyValueEventArgs(string propertyName, object newValue, object target)
	{
		PropertyName = propertyName;
		NewValue = newValue;
		Target = target;
	}
}
