using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ExtendedStoredProcedure : IExtendedStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase
{
	private readonly DeserializationModel model;

	private readonly int executionContext;

	private readonly bool isEncrypted;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int parameters;

	private readonly int returnType;

	private readonly int schema;

	CallableModuleType ICallableModule.ModuleType => CallableModuleType.ExtendedStoredProcedure;

	IScalarDataType ICallableModule.ReturnType => model.GetObjectById<IScalarDataType>(returnType);

	bool IUserDefinedFunctionModuleBase.IsEncrypted => isEncrypted;

	IExecutionContext IUserDefinedFunctionModuleBase.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => model.GetOrderedCollectionById<IParameter>(parameters);

	public ExtendedStoredProcedure(DeserializationModel model, int executionContext, bool isEncrypted, bool isSystemObject, string name, int parameters, int returnType, int schema)
	{
		this.model = model;
		this.executionContext = executionContext;
		this.isEncrypted = isEncrypted;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.parameters = parameters;
		this.returnType = returnType;
		this.schema = schema;
	}

	public ExtendedStoredProcedure(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (bool)values[2], (string)values[3], (int)values[4], (int)values[5], (int)values[6])
	{
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

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}
}
