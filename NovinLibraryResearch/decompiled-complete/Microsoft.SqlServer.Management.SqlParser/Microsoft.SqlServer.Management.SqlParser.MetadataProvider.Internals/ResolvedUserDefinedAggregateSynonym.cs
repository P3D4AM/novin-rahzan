using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedUserDefinedAggregateSynonym : ResolvedSynonym<IUserDefinedAggregate>, IResolvedUserDefinedAggregateSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IUserDefinedAggregate, IScalarFunction, IFunction, IFunctionModuleBase, IScalar
{
	public bool IsAggregateFunction => base.TargetObject.IsAggregateFunction;

	public IMetadataOrderedCollection<IParameter> Parameters => base.TargetObject.Parameters;

	public ScalarType ScalarType => base.TargetObject.ScalarType;

	public IScalarDataType DataType => base.TargetObject.DataType;

	public bool Nullable => base.TargetObject.Nullable;

	public ResolvedUserDefinedAggregateSynonym(ISynonym synonym, IUserDefinedAggregate targetObject)
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
