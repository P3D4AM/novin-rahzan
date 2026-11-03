namespace Stimulsoft.Report.Components;

public interface IStiEditable
{
	bool Editable { get; set; }

	string SaveState();

	void RestoreState(string value);
}
