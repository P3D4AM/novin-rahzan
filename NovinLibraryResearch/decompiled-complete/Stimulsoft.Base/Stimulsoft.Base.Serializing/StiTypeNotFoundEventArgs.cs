using System;

namespace Stimulsoft.Base.Serializing;

public class StiTypeNotFoundEventArgs : EventArgs
{
	public string TypeName { get; }

	public object CreatedObject { get; set; }

	public StiTypeNotFoundEventArgs(string typeName)
	{
		TypeName = typeName;
	}
}
