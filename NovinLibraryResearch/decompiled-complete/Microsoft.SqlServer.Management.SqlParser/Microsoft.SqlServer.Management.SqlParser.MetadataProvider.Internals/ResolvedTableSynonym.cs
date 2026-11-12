using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedTableSynonym : ResolvedSynonym<ITable>, IResolvedTableSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, ITable, ITableViewBase, IDatabaseTable, ITabular
{
	public IMetadataCollection<IDmlTrigger> Triggers => base.TargetObject.Triggers;

	public bool IsQuotedIdentifierOn => base.TargetObject.IsQuotedIdentifierOn;

	public IMetadataCollection<IConstraint> Constraints => base.TargetObject.Constraints;

	public IMetadataCollection<IIndex> Indexes => base.TargetObject.Indexes;

	public IMetadataCollection<IStatistics> Statistics => base.TargetObject.Statistics;

	public TabularType TabularType => base.TargetObject.TabularType;

	public IMetadataOrderedCollection<IColumn> Columns => base.TargetObject.Columns;

	public ITabular Unaliased => this;

	public ResolvedTableSynonym(ISynonym synonym, ITable targetObject)
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
