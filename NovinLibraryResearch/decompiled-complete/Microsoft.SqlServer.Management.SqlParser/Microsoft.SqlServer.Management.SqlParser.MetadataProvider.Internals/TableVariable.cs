using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class TableVariable : LocalVariable<ITableDataType>, ITableVariable, ITabular, IMetadataObject, ILocalVariable
{
	public override bool IsParameter => false;

	public new ITableDataType DataType => m_dataType;

	public TabularType TabularType => TabularType.TableVariable;

	public IMetadataOrderedCollection<IColumn> Columns => m_dataType.Columns;

	public ITabular Unaliased => this;

	public TableVariable(string name, ITableDataType dataType)
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
