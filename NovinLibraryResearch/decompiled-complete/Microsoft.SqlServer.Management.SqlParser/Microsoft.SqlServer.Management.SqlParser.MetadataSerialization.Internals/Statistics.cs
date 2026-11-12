using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Statistics : IStatistics, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly string filterDefinition;

	private readonly string name;

	private readonly bool noAutomaticRecomputation;

	private readonly int parent;

	private readonly StatisticsType type;

	StatisticsType IStatistics.Type => type;

	ITabular IStatistics.Parent => model.GetObjectById<ITabular>(parent);

	string IStatistics.FilterDefinition => filterDefinition;

	bool IStatistics.NoAutomaticRecomputation => noAutomaticRecomputation;

	IMetadataOrderedCollection<IColumn> IStatistics.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	string IMetadataObject.Name => name;

	public Statistics(DeserializationModel model, int columns, string filterDefinition, string name, bool noAutomaticRecomputation, int parent, StatisticsType type)
	{
		this.model = model;
		this.columns = columns;
		this.filterDefinition = filterDefinition;
		this.name = name;
		this.noAutomaticRecomputation = noAutomaticRecomputation;
		this.parent = parent;
		this.type = type;
	}

	public Statistics(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (string)values[1], (string)values[2], (bool)values[3], (int)values[4], (StatisticsType)values[5])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
