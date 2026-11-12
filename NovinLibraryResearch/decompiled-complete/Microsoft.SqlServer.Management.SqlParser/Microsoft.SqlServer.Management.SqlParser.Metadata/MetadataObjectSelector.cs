using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public struct MetadataObjectSelector : IEquatable<MetadataObjectSelector>
{
	private readonly MetadataTypeInfo typeInfo;

	private readonly string name;

	public MetadataTypeInfo TypeInfo => typeInfo;

	public string Name => name;

	public MetadataObjectSelector(MetadataTypeInfo typeInfo, string name)
	{
		this.typeInfo = typeInfo;
		this.name = name;
	}

	public override bool Equals(object obj)
	{
		return Equals((MetadataObjectSelector)obj);
	}

	public bool Equals(MetadataObjectSelector other)
	{
		if (typeInfo == other.typeInfo)
		{
			return name == other.name;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return typeInfo.GetHashCode() ^ name.GetHashCode();
	}
}
