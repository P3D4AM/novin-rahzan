using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public abstract class PropertyDifference
{
	private readonly string name;

	public string Name => name;

	internal PropertyDifference(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("name");
		}
		this.name = name;
	}
}
