using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedViewSynonym : ResolvedSynonym<IView>, IResolvedViewSynonym, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IView, ITableViewBase, IDatabaseTable, ITabular
{
	public bool IsEncrypted => base.TargetObject.IsEncrypted;

	public bool IsSchemaBound => base.TargetObject.IsSchemaBound;

	public string QueryText => base.TargetObject.QueryText;

	public bool ReturnsViewMetadata => base.TargetObject.ReturnsViewMetadata;

	public bool HasCheckOption => base.TargetObject.HasCheckOption;

	public bool HasColumnSpecification => base.TargetObject.HasColumnSpecification;

	public IMetadataCollection<IDmlTrigger> Triggers => base.TargetObject.Triggers;

	public bool IsQuotedIdentifierOn => base.TargetObject.IsQuotedIdentifierOn;

	public IMetadataCollection<IConstraint> Constraints => base.TargetObject.Constraints;

	public IMetadataCollection<IIndex> Indexes => base.TargetObject.Indexes;

	public IMetadataCollection<IStatistics> Statistics => base.TargetObject.Statistics;

	public TabularType TabularType => base.TargetObject.TabularType;

	public IMetadataOrderedCollection<IColumn> Columns => base.TargetObject.Columns;

	public ITabular Unaliased => this;

	public ResolvedViewSynonym(ISynonym synonym, IView targetObject)
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
