using System;

namespace Stimulsoft.Base;

public struct StiNameTypeEntry
{
	public Type Type { get; }

	public string Name { get; }

	public StiNameTypeEntry(string name, Type type)
	{
		Name = name;
		Type = type;
	}
}
