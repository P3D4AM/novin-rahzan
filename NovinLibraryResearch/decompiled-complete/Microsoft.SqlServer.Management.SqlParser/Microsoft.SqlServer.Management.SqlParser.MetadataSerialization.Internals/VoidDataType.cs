using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class VoidDataType : IVoidDataType, IScalarDataType, IDataType, IMetadataObject
{
	private readonly DeserializationModel model;

	bool IScalarDataType.IsSystem => this is ISystemDataType;

	bool IScalarDataType.IsClr => this is IClrDataType;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this != null;

	ISystemDataType IScalarDataType.BaseSystemDataType => null;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => "void";

	public VoidDataType(DeserializationModel model)
	{
		this.model = model;
	}

	public VoidDataType(DeserializationModel model, object[] values)
		: this(model)
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
