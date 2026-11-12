using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class UserDefinedAggregate : IUserDefinedAggregate, IScalarFunction, IFunction, IMetadataObject, IFunctionModuleBase, IScalar, ISchemaOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly int dataType;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly bool nullable;

	private readonly int parameters;

	private readonly int schema;

	bool IScalarFunction.IsAggregateFunction => true;

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => model.GetOrderedCollectionById<IParameter>(parameters);

	string IMetadataObject.Name => name;

	ScalarType IScalar.ScalarType => ScalarType.ScalarFunction;

	IScalarDataType IScalar.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IScalar.Nullable => nullable;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public UserDefinedAggregate(DeserializationModel model, int dataType, bool isSystemObject, string name, bool nullable, int parameters, int schema)
	{
		this.model = model;
		this.dataType = dataType;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.nullable = nullable;
		this.parameters = parameters;
		this.schema = schema;
	}

	public UserDefinedAggregate(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (string)values[2], (bool)values[3], (int)values[4], (int)values[5])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}

	T ISchemaOwnedObject.Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}
}
