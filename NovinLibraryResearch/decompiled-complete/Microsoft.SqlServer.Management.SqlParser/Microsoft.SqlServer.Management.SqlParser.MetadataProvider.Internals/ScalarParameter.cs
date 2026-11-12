using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class ScalarParameter : LocalVariable<IScalarDataType>, IScalarParameter, IScalarVariable, IScalar, IMetadataObject, ILocalVariable, IParameter
{
	private readonly bool m_isOutput;

	private readonly string m_defaultValue;

	public override bool IsParameter => true;

	public ScalarType ScalarType => ScalarType.ScalarVariable;

	public new IScalarDataType DataType => m_dataType;

	public bool Nullable => true;

	public bool IsOutput => m_isOutput;

	public bool IsReadOnly => false;

	public string DefaultValue => m_defaultValue;

	public ScalarParameter(string name, IScalarDataType dataType)
		: this(name, dataType, isOutput: false, null)
	{
	}

	public ScalarParameter(string name, IScalarDataType dataType, bool isOutput, string defaultValue)
		: base(name, dataType)
	{
		m_isOutput = isOutput;
		m_defaultValue = defaultValue;
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
