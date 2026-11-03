namespace Stimulsoft.Report;

public interface IStiStateSaveRestore
{
	void SaveState(string stateName);

	void RestoreState(string stateName);

	void ClearAllStates();
}
