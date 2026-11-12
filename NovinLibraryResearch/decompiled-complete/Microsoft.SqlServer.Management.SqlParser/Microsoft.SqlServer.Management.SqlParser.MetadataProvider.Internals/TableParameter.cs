using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class TableParameter : LocalVariable<ITableDataType>, ITableParameter, ITableVariable, ITabular, IMetadataObject, ILocalVariable, IParameter
{
	public override bool IsParameter => true;

	public new ITableDataType DataType => m_dataType;

	public bool IsOutput => false;

	public bool IsReadOnly => true;

	public string DefaultValue => null;

	public TabularType TabularType => TabularType.TableVariable;

	public IMetadataOrderedCollection<IColumn> Columns => m_dataType.Columns;

	public ITabular Unaliased => this;

	public TableParameter(string name, ITableDataType dataType)
		: base(name, dataType)
	{
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
