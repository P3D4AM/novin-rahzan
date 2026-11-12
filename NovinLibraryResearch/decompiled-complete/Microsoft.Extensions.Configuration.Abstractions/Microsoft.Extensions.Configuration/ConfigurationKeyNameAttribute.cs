using System;

namespace Microsoft.Extensions.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConfigurationKeyNameAttribute : Attribute
{
	public string Name { get; }

	public ConfigurationKeyNameAttribute(string name)
	{
		Name = name;
	}
}
