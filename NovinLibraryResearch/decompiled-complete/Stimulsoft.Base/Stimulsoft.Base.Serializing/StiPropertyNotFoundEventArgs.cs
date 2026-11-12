using System;

namespace Stimulsoft.Base.Serializing;

public class StiPropertyNotFoundEventArgs : EventArgs
{
	public string PropertyName { get; set; }

	public Type PropertyType { get; }

	public StiPropertyNotFoundEventArgs(string propertyName, Type propertyType)
	{
		PropertyName = propertyName;
		PropertyType = propertyType;
	}
}
