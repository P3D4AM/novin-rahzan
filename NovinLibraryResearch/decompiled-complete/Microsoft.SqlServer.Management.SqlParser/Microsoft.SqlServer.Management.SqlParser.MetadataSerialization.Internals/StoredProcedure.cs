using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class StoredProcedure : IStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly int executionContext;

	private readonly bool forReplication;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isRecompiled;

	private readonly bool isSqlClr;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int parameters;

	private readonly int schema;

	private readonly bool startup;

	string IStoredProcedure.BodyText => bodyText;

	bool IStoredProcedure.ForReplication => forReplication;

	bool IStoredProcedure.IsRecompiled => isRecompiled;

	bool IStoredProcedure.IsSqlClr => isSqlClr;

	bool IStoredProcedure.Startup => startup;

	bool IStoredProcedure.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	CallableModuleType ICallableModule.ModuleType => CallableModuleType.StoredProcedure;

	IScalarDataType ICallableModule.ReturnType => model.GetIntDataType();

	bool IUserDefinedFunctionModuleBase.IsEncrypted => isEncrypted;

	IExecutionContext IUserDefinedFunctionModuleBase.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => model.GetOrderedCollectionById<IParameter>(parameters);

	public StoredProcedure(DeserializationModel model, string bodyText, int executionContext, bool forReplication, bool isEncrypted, bool isQuotedIdentifierOn, bool isRecompiled, bool isSqlClr, bool isSystemObject, string name, int parameters, int schema, bool startup)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.executionContext = executionContext;
		this.forReplication = forReplication;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isRecompiled = isRecompiled;
		this.isSqlClr = isSqlClr;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.parameters = parameters;
		this.schema = schema;
		this.startup = startup;
	}

	public StoredProcedure(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (int)values[1], (bool)values[2], (bool)values[3], (bool)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (string)values[8], (int)values[9], (int)values[10], (bool)values[11])
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
