using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class XmlDataTypeTableMethod : IXmlDataTypeTableMethod, IFunction, IMetadataObject, IFunctionModuleBase
{
	private readonly string m_name;

	private readonly IMetadataOrderedCollection<IParameter> m_parameters;

	public string Name => m_name;

	public IMetadataOrderedCollection<IParameter> Parameters => m_parameters;

	public XmlDataTypeTableMethod(string name, params IParameter[] parameters)
	{
		m_name = name;
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
