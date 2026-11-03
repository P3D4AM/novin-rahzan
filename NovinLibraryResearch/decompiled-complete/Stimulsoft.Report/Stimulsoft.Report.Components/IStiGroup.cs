namespace Stimulsoft.Report.Components;

public interface IStiGroup
{
	StiGroupConditionExpression Condition { get; set; }

	StiGroupSortDirection SortDirection { get; set; }
}
