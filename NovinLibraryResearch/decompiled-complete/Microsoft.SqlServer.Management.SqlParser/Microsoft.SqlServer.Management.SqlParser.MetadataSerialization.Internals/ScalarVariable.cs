using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ScalarVariable : IScalarVariable, IScalar, IMetadataObject, ILocalVariable
{
	private readonly DeserializationModel model;

	private readonly int dataType;

	private readonly string name;

	private readonly bool nullable;

	IScalarDataType IScalarVariable.DataType => model.GetObjectById<IScalarDataType>(dataType);

	ScalarType IScalar.ScalarType => ScalarType.ScalarVariable;

	IScalarDataType IScalar.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IScalar.Nullable => nullable;

	string IMetadataObject.Name => name;

	bool ILocalVariable.IsScalarVariable => this != null;

	bool ILocalVariable.IsTableVariable => this is ITableVariable;

	bool ILocalVariable.IsCursorVariable => this is ICursorVariable;

	bool ILocalVariable.IsParameter => this is IParameter;

	IDataType ILocalVariable.DataType => model.GetObjectById<IScalarDataType>(dataType);

	public ScalarVariable(DeserializationModel model, int dataType, string name, bool nullable)
	{
		this.model = model;
		this.dataType = dataType;
		this.name = name;
		this.nullable = nullable;
	}

	public ScalarVariable(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (string)values[1], (bool)values[2])
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
