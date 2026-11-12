using System;

namespace Stimulsoft.Base.Design;

public class StiProcessDescriptionEventArgs : EventArgs
{
	public string Name { get; }

	public Type ComponentType { get; }

	public string Description { get; set; }

	public StiProcessDescriptionEventArgs(Type componentType, string name, string description)
	{
		ComponentType = componentType;
		Name = name;
		Description = description;
	}
}
