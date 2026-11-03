namespace Stimulsoft.Report.Components;

public interface IStiPageBreak
{
	bool NewColumnBefore { get; set; }

	bool NewColumnAfter { get; set; }

	bool NewPageBefore { get; set; }

	bool NewPageAfter { get; set; }

	float BreakIfLessThan { get; set; }

	bool SkipFirst { get; set; }
}
