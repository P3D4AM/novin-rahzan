namespace Stimulsoft.Report.Components;

public interface IStiIndicatorCondition
{
	StiIndicator CreateIndicator(StiText component);

	void Reset();
}
