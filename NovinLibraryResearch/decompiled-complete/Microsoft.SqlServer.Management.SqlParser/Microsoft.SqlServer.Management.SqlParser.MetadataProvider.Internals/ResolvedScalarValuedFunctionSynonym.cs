using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedScalarValuedFunctionSynonym : ResolvedSynonym<IScalarValuedFunction>, IResolvedScalarValuedFunctionSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IScalarValuedFunction, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ICallableModule, IScalarFunction, IScalar
{
	public bool ReturnsNullOnNullInput => base.TargetObject.ReturnsNullOnNullInput;

	public string BodyText => base.TargetObject.BodyText;

	public bool IsSchemaBound => base.TargetObject.IsSchemaBound;

	public bool IsSqlClr => base.TargetObject.IsSqlClr;

	public bool IsQuotedIdentifierOn => base.TargetObject.IsQuotedIdentifierOn;

	public IMetadataOrderedCollection<IParameter> Parameters => base.TargetObject.Parameters;

	public bool IsEncrypted => base.TargetObject.IsEncrypted;

	public IExecutionContext ExecutionContext => base.TargetObject.ExecutionContext;

	public CallableModuleType ModuleType => base.TargetObject.ModuleType;

	public IScalarDataType ReturnType => base.TargetObject.ReturnType;

	public bool IsAggregateFunction => base.TargetObject.IsAggregateFunction;

	public ScalarType ScalarType => base.TargetObject.ScalarType;

	public IScalarDataType DataType => base.TargetObject.DataType;

	public bool Nullable => base.TargetObject.Nullable;

	public ResolvedScalarValuedFunctionSynonym(ISynonym synonym, IScalarValuedFunction targetObject)
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
