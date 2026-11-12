using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ScalarValuedFunction : IScalarValuedFunction, IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, ICallableModule, IScalarFunction, IScalar
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly int dataType;

	private readonly int executionContext;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSchemaBound;

	private readonly bool isSqlClr;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly bool nullable;

	private readonly int parameters;

	private readonly bool returnsNullOnNullInput;

	private readonly int schema;

	bool IScalarValuedFunction.ReturnsNullOnNullInput => returnsNullOnNullInput;

	string IUserDefinedFunction.BodyText => bodyText;

	bool IUserDefinedFunction.IsSchemaBound => isSchemaBound;

	bool IUserDefinedFunction.IsSqlClr => isSqlClr;

	bool IUserDefinedFunction.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => model.GetOrderedCollectionById<IParameter>(parameters);

	string IMetadataObject.Name => name;

	bool IUserDefinedFunctionModuleBase.IsEncrypted => isEncrypted;

	IExecutionContext IUserDefinedFunctionModuleBase.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	CallableModuleType ICallableModule.ModuleType => CallableModuleType.ScalarFunction;

	IScalarDataType ICallableModule.ReturnType => ((IScalar)this).DataType;

	bool IScalarFunction.IsAggregateFunction => false;

	ScalarType IScalar.ScalarType => ScalarType.ScalarFunction;

	IScalarDataType IScalar.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IScalar.Nullable => nullable;

	public ScalarValuedFunction(DeserializationModel model, string bodyText, int dataType, int executionContext, bool isEncrypted, bool isQuotedIdentifierOn, bool isSchemaBound, bool isSqlClr, bool isSystemObject, string name, bool nullable, int parameters, bool returnsNullOnNullInput, int schema)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.dataType = dataType;
		this.executionContext = executionContext;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSchemaBound = isSchemaBound;
		this.isSqlClr = isSqlClr;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.nullable = nullable;
		this.parameters = parameters;
		this.returnsNullOnNullInput = returnsNullOnNullInput;
		this.schema = schema;
	}

	public ScalarValuedFunction(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (int)values[1], (int)values[2], (bool)values[3], (bool)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (string)values[8], (bool)values[9], (int)values[10], (bool)values[11], (int)values[12])
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
