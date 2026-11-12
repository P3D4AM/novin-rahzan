namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlTableElement
{
	bool IsColumnDefinition { get; }

	SqlColumnDefinition AsColumnDefinition { get; }

	bool IsConstraint { get; }

	SqlConstraint AsConstraint { get; }

	SqlTemporalPeriodDefinition AsTemporalPeriodDefinition { get; }

	bool IsTemporalPeriodDefinition { get; }
}
