using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class FileGroup : IFileGroup, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int database;

	private readonly bool isDefault;

	private readonly bool isFileStream;

	private readonly bool isReadOnly;

	private readonly bool isSystemObject;

	private readonly string name;

	bool IFileGroup.IsDefault => isDefault;

	bool IFileGroup.IsFileStream => isFileStream;

	bool IFileGroup.IsReadOnly => isReadOnly;

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public FileGroup(DeserializationModel model, int database, bool isDefault, bool isFileStream, bool isReadOnly, bool isSystemObject, string name)
	{
		this.model = model;
		this.database = database;
		this.isDefault = isDefault;
		this.isFileStream = isFileStream;
		this.isReadOnly = isReadOnly;
		this.isSystemObject = isSystemObject;
		this.name = name;
	}

	public FileGroup(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (bool)values[2], (bool)values[3], (bool)values[4], (string)values[5])
	{
	}

	T IDatabaseOwnedObject.Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
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
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}
}
