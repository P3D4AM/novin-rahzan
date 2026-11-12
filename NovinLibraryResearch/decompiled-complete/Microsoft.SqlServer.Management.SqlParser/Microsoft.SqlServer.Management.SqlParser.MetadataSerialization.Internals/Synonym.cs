using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Synonym : ISynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly string baseObjectName;

	private readonly SynonymBaseType baseType;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int schema;

	SynonymBaseType ISynonym.BaseType => baseType;

	string ISynonym.BaseObjectName => baseObjectName;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Synonym(DeserializationModel model, string baseObjectName, SynonymBaseType baseType, bool isSystemObject, string name, int schema)
	{
		this.model = model;
		this.baseObjectName = baseObjectName;
		this.baseType = baseType;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.schema = schema;
	}

	public Synonym(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (SynonymBaseType)values[1], (bool)values[2], (string)values[3], (int)values[4])
	{
	}

	T ISchemaOwnedObject.Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}
}
