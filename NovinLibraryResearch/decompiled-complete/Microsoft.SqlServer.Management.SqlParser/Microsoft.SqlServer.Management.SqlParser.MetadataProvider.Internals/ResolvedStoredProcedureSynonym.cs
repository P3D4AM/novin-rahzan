using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedStoredProcedureSynonym : ResolvedSynonym<IStoredProcedure>, IResolvedStoredProcedureSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, IFunctionModuleBase
{
	public string BodyText => base.TargetObject.BodyText;

	public bool ForReplication => base.TargetObject.ForReplication;

	public bool IsRecompiled => base.TargetObject.IsRecompiled;

	public bool IsSqlClr => base.TargetObject.IsSqlClr;

	public bool Startup => base.TargetObject.Startup;

	public bool IsQuotedIdentifierOn => base.TargetObject.IsQuotedIdentifierOn;

	public CallableModuleType ModuleType => base.TargetObject.ModuleType;

	public IScalarDataType ReturnType => base.TargetObject.ReturnType;

	public bool IsEncrypted => base.TargetObject.IsEncrypted;

	public IExecutionContext ExecutionContext => base.TargetObject.ExecutionContext;

	public IMetadataOrderedCollection<IParameter> Parameters => base.TargetObject.Parameters;

	public ResolvedStoredProcedureSynonym(ISynonym synonym, IStoredProcedure targetObject)
		: base(synonym, targetObject)
	{
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
