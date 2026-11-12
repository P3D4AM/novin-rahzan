using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class UdtDataMember : IUdtDataMember, IUdtMember, IScalar, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int dataType;

	private readonly bool isStatic;

	private readonly string name;

	private readonly bool nullable;

	private readonly int userDefinedType;

	IClrDataType IUdtMember.UserDefinedType => model.GetObjectById<IClrDataType>(userDefinedType);

	bool IUdtMember.IsStatic => isStatic;

	ScalarType IScalar.ScalarType => ScalarType.UdtMember;

	IScalarDataType IScalar.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IScalar.Nullable => nullable;

	string IMetadataObject.Name => name;

	public UdtDataMember(DeserializationModel model, int dataType, bool isStatic, string name, bool nullable, int userDefinedType)
	{
		this.model = model;
		this.dataType = dataType;
		this.isStatic = isStatic;
		this.name = name;
		this.nullable = nullable;
		this.userDefinedType = userDefinedType;
	}

	public UdtDataMember(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (string)values[2], (bool)values[3], (int)values[4])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
