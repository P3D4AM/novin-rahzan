using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class CursorVariable : LocalVariable<ICursorDataType>, ICursorVariable, ILocalVariable, IMetadataObject
{
	public override bool IsParameter => false;

	public new ICursorDataType DataType => m_dataType;

	public CursorVariable(string name)
		: base(name, ObjectFactoryBase<DataTypeFactory>.Instance.Cursor)
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
