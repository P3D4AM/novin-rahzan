using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ClrDataType : IClrDataType, IScalarDataType, IDataType, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly string name;

	string IClrDataType.AssemblyName => null;

	string IClrDataType.ClassName => null;

	bool IClrDataType.IsBinaryOrdered => false;

	bool IClrDataType.IsComVisible => false;

	bool IClrDataType.IsNullable => false;

	IMetadataCollection<IUdtMethod> IClrDataType.Methods => null;

	IMetadataCollection<IUdtDataMember> IClrDataType.DataMembers => null;

	bool IScalarDataType.IsSystem => this is ISystemDataType;

	bool IScalarDataType.IsClr => this != null;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => null;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => true;

	string IMetadataObject.Name => name;

	public ClrDataType(DeserializationModel model, string name)
	{
		this.model = model;
		this.name = name;
	}

	public ClrDataType(DeserializationModel model, object[] values)
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
