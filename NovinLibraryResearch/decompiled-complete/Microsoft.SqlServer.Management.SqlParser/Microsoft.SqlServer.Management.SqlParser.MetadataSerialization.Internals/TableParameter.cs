using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class TableParameter : ITableParameter, ITableVariable, ITabular, IMetadataObject, ILocalVariable, IParameter
{
	private readonly DeserializationModel model;

	private readonly int dataType;

	private readonly string name;

	ITableDataType ITableVariable.DataType => model.GetObjectById<ITableDataType>(dataType);

	TabularType ITabular.TabularType => TabularType.TableVariable;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => ((ITableVariable)this).DataType.Columns;

	ITabular ITabular.Unaliased => this;

	string IMetadataObject.Name => name;

	bool ILocalVariable.IsScalarVariable => this is IScalarVariable;

	bool ILocalVariable.IsTableVariable => this != null;

	bool ILocalVariable.IsCursorVariable => this is ICursorVariable;

	bool ILocalVariable.IsParameter => this != null;

	IDataType ILocalVariable.DataType => model.GetObjectById<ITableDataType>(dataType);

	bool IParameter.IsOutput => false;

	bool IParameter.IsReadOnly => true;

	string IParameter.DefaultValue => null;

	public TableParameter(DeserializationModel model, int dataType, string name)
	{
		this.model = model;
		this.dataType = dataType;
		this.name = name;
	}

	public TableParameter(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (string)values[1])
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
