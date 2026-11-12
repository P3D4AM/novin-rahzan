using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal abstract class UdtMember<T> : MetadataObjectBase, IUdtMember, IScalar, IMetadataObject where T : class, IUdtMember
{
	private readonly IClrDataType m_userDefinedType;

	private readonly IScalarDataType m_dataType;

	private readonly bool m_nullable;

	private readonly bool m_isStatic;

	public IClrDataType UserDefinedType => m_userDefinedType;

	public bool IsStatic => m_isStatic;

	public ScalarType ScalarType => ScalarType.UdtMember;

	public IScalarDataType DataType => m_dataType;

	public bool Nullable => m_nullable;

	protected UdtMember(string name, IClrDataType userDefinedType, IScalarDataType dataType, bool nullable, bool isStatic)
		: base(name)
	{
		m_userDefinedType = userDefinedType;
		m_dataType = dataType;
		m_nullable = nullable;
		m_isStatic = isStatic;
	}
}
