using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedTableValuedFunctionSynonym : ResolvedSynonym<ITableValuedFunction>, IResolvedTableValuedFunctionSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, ITableValuedFunction, IDatabaseTable, ITabular, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase
{
	public bool IsInline => base.TargetObject.IsInline;

	public string TableVariableName => base.TargetObject.TableVariableName;

	public IMetadataCollection<IConstraint> Constraints => base.TargetObject.Constraints;

	public IMetadataCollection<IIndex> Indexes => base.TargetObject.Indexes;

	public IMetadataCollection<IStatistics> Statistics => base.TargetObject.Statistics;

	public TabularType TabularType => base.TargetObject.TabularType;

	public IMetadataOrderedCollection<IColumn> Columns => base.TargetObject.Columns;

	public ITabular Unaliased => this;

	public string BodyText => base.TargetObject.BodyText;

	public bool IsSchemaBound => base.TargetObject.IsSchemaBound;

	public bool IsSqlClr => base.TargetObject.IsSqlClr;

	public bool IsQuotedIdentifierOn => base.TargetObject.IsQuotedIdentifierOn;

	public IMetadataOrderedCollection<IParameter> Parameters => base.TargetObject.Parameters;

	public bool IsEncrypted => base.TargetObject.IsEncrypted;

	public IExecutionContext ExecutionContext => base.TargetObject.ExecutionContext;

	public ResolvedTableValuedFunctionSynonym(ISynonym synonym, ITableValuedFunction targetObject)
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
