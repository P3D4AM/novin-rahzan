using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Collation : ICollation, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly string name;

	string IMetadataObject.Name => name;

	public Collation(DeserializationModel model, string name)
	{
		this.model = model;
		this.name = name;
	}

	public Collation(DeserializationModel model, object[] values)
		: this(model, (string)values[0])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
