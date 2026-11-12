using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class SystemClrDataType : ISystemClrDataType, ISystemDataType, IScalarDataType, IDataType, IMetadataObject, IClrDataType
{
	private readonly DeserializationModel model;

	private readonly int length;

	private readonly int numericPrecision;

	private readonly int numericScale;

	private readonly DataTypeSpec typeSpec;

	DataTypeSpec ISystemDataType.TypeSpec => typeSpec;

	int ISystemDataType.Length => length;

	int ISystemDataType.NumericPrecision => numericPrecision;

	int ISystemDataType.NumericScale => numericScale;

	bool IScalarDataType.IsSystem => this != null;

	bool IScalarDataType.IsClr => this != null;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => this;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => ((ISystemDataType)this).TypeSpec.Name;

	string IClrDataType.AssemblyName => null;

	string IClrDataType.ClassName => null;

	bool IClrDataType.IsBinaryOrdered => false;

	bool IClrDataType.IsComVisible => false;

	bool IClrDataType.IsNullable => false;

	IMetadataCollection<IUdtMethod> IClrDataType.Methods => null;

	IMetadataCollection<IUdtDataMember> IClrDataType.DataMembers => null;

	public SystemClrDataType(DeserializationModel model, int length, int numericPrecision, int numericScale, DataTypeSpec typeSpec)
	{
		this.model = model;
		this.length = length;
		this.numericPrecision = numericPrecision;
		this.numericScale = numericScale;
		this.typeSpec = typeSpec;
	}

	public SystemClrDataType(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (DataTypeSpec)values[3])
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
