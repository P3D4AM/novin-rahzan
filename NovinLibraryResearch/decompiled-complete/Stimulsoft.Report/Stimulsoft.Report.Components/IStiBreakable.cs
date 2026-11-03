namespace Stimulsoft.Report.Components;

public interface IStiBreakable
{
	bool CanBreak { get; set; }

	bool Break(StiComponent dividedComponent, double devideFactor, ref double divideLine);
}
