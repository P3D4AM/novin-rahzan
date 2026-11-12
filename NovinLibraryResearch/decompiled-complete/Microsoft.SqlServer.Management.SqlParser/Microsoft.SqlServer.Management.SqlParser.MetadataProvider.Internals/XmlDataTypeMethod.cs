using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class XmlDataTypeMethod : IXmlDataTypeMethod, IScalarFunction, IFunction, IMetadataObject, IFunctionModuleBase, IScalar
{
	private readonly string m_name;

	private readonly IScalarDataType m_dataType;

	private readonly bool m_nullable;

	private readonly IMetadataOrderedCollection<IParameter> m_parameters;

	public bool IsAggregateFunction => false;

	public string Name => m_name;

	public IMetadataOrderedCollection<IParameter> Parameters => m_parameters;

	public ScalarType ScalarType => ScalarType.ScalarFunction;

	public IScalarDataType DataType => m_dataType;

	public bool Nullable => m_nullable;

	public XmlDataTypeMethod(string name, IScalarDataType dataType, bool nullable, params IParameter[] parameters)
	{
		m_name = name;
		m_dataType = dataType;
		m_nullable = nullable;
		m_parameters = Collection<IParameter>.ConvertArray(parameters, CollationInfo.OrdinalIgnoreCase);
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitior");
		}
		return visitor.Visit(this);
	}
}
