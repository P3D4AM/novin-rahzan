using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ScalarParameter : IScalarParameter, IScalarVariable, IScalar, IMetadataObject, ILocalVariable, IParameter
{
	private readonly DeserializationModel model;

	private readonly int dataType;

	private readonly string defaultValue;

	private readonly bool isOutput;

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

	bool ILocalVariable.IsParameter => this != null;

	IDataType ILocalVariable.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IParameter.IsOutput => isOutput;

	bool IParameter.IsReadOnly => false;

	string IParameter.DefaultValue => defaultValue;

	public ScalarParameter(DeserializationModel model, int dataType, string defaultValue, bool isOutput, string name, bool nullable)
	{
		this.model = model;
		this.dataType = dataType;
		this.defaultValue = defaultValue;
		this.isOutput = isOutput;
		this.name = name;
		this.nullable = nullable;
	}

	public ScalarParameter(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (string)values[1], (bool)values[2], (string)values[3], (bool)values[4])
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
