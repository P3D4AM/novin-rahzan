namespace Stimulsoft.Report.Dashboard;

public interface IStiButtonVisualStates
{
	IStiButtonVisualState GetHoverState();

	IStiButtonVisualState GetPressedState();

	IStiButtonVisualState GetCheckedState();
}
