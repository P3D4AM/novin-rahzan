using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal sealed class ScalarVariable : LocalVariable<IScalarDataType>, IScalarVariable, IScalar, IMetadataObject, ILocalVariable
{
	public override bool IsParameter => false;

	public ScalarType ScalarType => ScalarType.ScalarVariable;

	public new IScalarDataType DataType => m_dataType;

	public bool Nullable => true;

	public ScalarVariable(string name, IScalarDataType dataType)
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
