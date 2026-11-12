using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class CursorDataType : ICursorDataType, IDataType, IMetadataObject
{
	private readonly DeserializationModel model;

	bool IDataType.IsScalar => this is IScalarDataType;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this != null;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => "cursor";

	public CursorDataType(DeserializationModel model)
	{
		this.model = model;
	}

	public CursorDataType(DeserializationModel model, object[] values)
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
