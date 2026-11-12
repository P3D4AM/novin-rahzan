using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class CursorParameter : LocalVariable<ICursorDataType>, ICursorParameter, ICursorVariable, ILocalVariable, IMetadataObject, IParameter
{
	public override bool IsParameter => true;

	public new ICursorDataType DataType => m_dataType;

	public bool IsOutput => true;

	public bool IsReadOnly => false;

	public string DefaultValue => null;

	public CursorParameter(string name)
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
