using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal abstract class LocalVariable<D> : MetadataObjectBase, ILocalVariable, IMetadataObject where D : class, IDataType
{
	protected readonly D m_dataType;

	public bool IsScalarVariable => m_dataType.IsScalar;

	public bool IsTableVariable => m_dataType.IsTable;

	public bool IsCursorVariable => m_dataType.IsCursor;

	public abstract bool IsParameter { get; }

	public IDataType DataType => m_dataType;

	protected LocalVariable(string name, D dataType)
		: base(name)
	{
		m_dataType = dataType;
	}
}
