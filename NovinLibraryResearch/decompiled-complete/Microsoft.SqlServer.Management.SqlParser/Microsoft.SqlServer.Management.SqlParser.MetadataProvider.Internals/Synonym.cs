using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Synonym : SchemaOwnedObject, IMutableSynonym, ISynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	private SynonymBaseType baseType;

	private string baseObjectName;

	public SynonymBaseType BaseType
	{
		get
		{
			return baseType;
		}
		set
		{
			baseType = value;
		}
	}

	public string BaseObjectName
	{
		get
		{
			return baseObjectName;
		}
		set
		{
			baseObjectName = value;
		}
	}

	public Synonym(ISchema schema, string name)
		: base(schema, name)
	{
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
