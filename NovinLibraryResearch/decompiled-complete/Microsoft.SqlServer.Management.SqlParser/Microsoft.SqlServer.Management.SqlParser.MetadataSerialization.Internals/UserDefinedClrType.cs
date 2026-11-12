using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class UserDefinedClrType : IUserDefinedClrType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IClrDataType, IScalarDataType
{
	private readonly DeserializationModel model;

	private readonly string assemblyName;

	private readonly string className;

	private readonly int dataMembers;

	private readonly bool isBinaryOrdered;

	private readonly bool isComVisible;

	private readonly bool isNullable;

	private readonly bool isSystemObject;

	private readonly int methods;

	private readonly string name;

	private readonly int schema;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => name;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IClrDataType.AssemblyName => assemblyName;

	string IClrDataType.ClassName => className;

	bool IClrDataType.IsBinaryOrdered => isBinaryOrdered;

	bool IClrDataType.IsComVisible => isComVisible;

	bool IClrDataType.IsNullable => isNullable;

	IMetadataCollection<IUdtMethod> IClrDataType.Methods => model.GetCollectionById<IUdtMethod>(methods);

	IMetadataCollection<IUdtDataMember> IClrDataType.DataMembers => model.GetCollectionById<IUdtDataMember>(dataMembers);

	bool IScalarDataType.IsSystem => this is ISystemDataType;

	bool IScalarDataType.IsClr => this != null;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => null;

	public UserDefinedClrType(DeserializationModel model, string assemblyName, string className, int dataMembers, bool isBinaryOrdered, bool isComVisible, bool isNullable, bool isSystemObject, int methods, string name, int schema)
	{
		this.model = model;
		this.assemblyName = assemblyName;
		this.className = className;
		this.dataMembers = dataMembers;
		this.isBinaryOrdered = isBinaryOrdered;
		this.isComVisible = isComVisible;
		this.isNullable = isNullable;
		this.isSystemObject = isSystemObject;
		this.methods = methods;
		this.name = name;
		this.schema = schema;
	}

	public UserDefinedClrType(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (string)values[1], (int)values[2], (bool)values[3], (bool)values[4], (bool)values[5], (bool)values[6], (int)values[7], (string)values[8], (int)values[9])
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
