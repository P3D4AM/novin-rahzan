using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class UserDefinedDataType : IUserDefinedDataType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IScalarDataType
{
	private readonly DeserializationModel model;

	private readonly int baseSystemDataType;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly bool nullable;

	private readonly int schema;

	bool IUserDefinedDataType.Nullable => nullable;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => name;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	bool IScalarDataType.IsSystem => this is ISystemDataType;

	bool IScalarDataType.IsClr => this is IClrDataType;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => model.GetObjectById<ISystemDataType>(baseSystemDataType);

	public UserDefinedDataType(DeserializationModel model, int baseSystemDataType, bool isSystemObject, string name, bool nullable, int schema)
	{
		this.model = model;
		this.baseSystemDataType = baseSystemDataType;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.nullable = nullable;
		this.schema = schema;
	}

	public UserDefinedDataType(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (string)values[2], (bool)values[3], (int)values[4])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
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
}
