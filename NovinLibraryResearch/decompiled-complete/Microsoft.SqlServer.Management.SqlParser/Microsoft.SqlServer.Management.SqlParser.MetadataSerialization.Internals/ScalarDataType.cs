using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ScalarDataType : IScalarDataType, IDataType, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly string name;

	bool IScalarDataType.IsSystem => this is ISystemDataType;

	bool IScalarDataType.IsClr => this is IClrDataType;

	bool IScalarDataType.IsXml => this is IXmlDataType;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => null;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => true;

	string IMetadataObject.Name => name;

	public ScalarDataType(DeserializationModel model, string name)
	{
		this.model = model;
		this.name = name;
	}

	public ScalarDataType(DeserializationModel model, object[] values)
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
