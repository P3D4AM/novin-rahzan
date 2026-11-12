using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class CursorParameter : ICursorParameter, ICursorVariable, ILocalVariable, IMetadataObject, IParameter
{
	private readonly DeserializationModel model;

	private readonly string name;

	ICursorDataType ICursorVariable.DataType => model.GetCursorDataType();

	bool ILocalVariable.IsScalarVariable => this is IScalarVariable;

	bool ILocalVariable.IsTableVariable => this is ITableVariable;

	bool ILocalVariable.IsCursorVariable => this != null;

	bool ILocalVariable.IsParameter => this != null;

	IDataType ILocalVariable.DataType => model.GetCursorDataType();

	string IMetadataObject.Name => name;

	bool IParameter.IsOutput => true;

	bool IParameter.IsReadOnly => false;

	string IParameter.DefaultValue => null;

	public CursorParameter(DeserializationModel model, string name)
	{
		this.model = model;
		this.name = name;
	}

	public CursorParameter(DeserializationModel model, object[] values)
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
