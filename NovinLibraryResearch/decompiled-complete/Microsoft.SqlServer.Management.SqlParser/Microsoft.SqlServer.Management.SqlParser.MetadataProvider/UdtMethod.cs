using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal class UdtMethod : UdtMember<IUdtMethod>, IUdtMethod, IUdtMember, IScalar, IMetadataObject, IScalarFunction, IFunction, IFunctionModuleBase
{
	private readonly IMetadataOrderedCollection<IParameter> m_parameters;

	public bool IsAggregateFunction => false;

	public IMetadataOrderedCollection<IParameter> Parameters => m_parameters;

	public bool IsUserDefinedFunction => false;

	public IUserDefinedFunction AsUserDefinedFunction => null;

	public UdtMethod(string name, IClrDataType userDefinedType, IScalarDataType dataType, bool nullable, bool isStatic, params IParameter[] parameters)
		: this(name, userDefinedType, dataType, nullable, isStatic, Collection<IParameter>.ConvertArray(parameters, CollationInfo.OrdinalIgnoreCase))
	{
	}

	public UdtMethod(string name, IClrDataType userDefinedType, IScalarDataType dataType, bool nullable, bool isStatic, IMetadataOrderedCollection<IParameter> parameters)
		: base(name, userDefinedType, dataType, nullable, isStatic)
	{
		m_parameters = parameters;
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
