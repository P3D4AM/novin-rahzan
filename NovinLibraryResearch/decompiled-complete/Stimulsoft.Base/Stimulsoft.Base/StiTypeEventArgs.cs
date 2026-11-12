using System;

namespace Stimulsoft.Base;

public class StiTypeEventArgs : EventArgs
{
	public string TypeName { get; }

	public Type Type { get; set; }

	public StiTypeEventArgs(string typeName)
	{
		TypeName = typeName;
	}
}
