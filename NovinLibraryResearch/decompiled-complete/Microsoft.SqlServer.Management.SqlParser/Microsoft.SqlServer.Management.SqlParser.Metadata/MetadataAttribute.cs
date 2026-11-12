using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Interface)]
internal class MetadataAttribute : Attribute
{
	private readonly MetadataType type;

	public MetadataType Type => type;

	public MetadataAttribute()
		: this(MetadataType.Reference)
	{
	}

	public MetadataAttribute(MetadataType type)
	{
		this.type = type;
	}
}
