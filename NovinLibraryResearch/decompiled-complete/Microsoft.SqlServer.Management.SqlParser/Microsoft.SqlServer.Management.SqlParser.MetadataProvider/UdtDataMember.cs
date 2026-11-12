using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal sealed class UdtDataMember : UdtMember<IUdtDataMember>, IUdtDataMember, IUdtMember, IScalar, IMetadataObject
{
	public UdtDataMember(string name, IClrDataType userDefinedType, IScalarDataType dataType, bool isStatic)
		: base(name, userDefinedType, dataType, nullable: true, isStatic)
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
