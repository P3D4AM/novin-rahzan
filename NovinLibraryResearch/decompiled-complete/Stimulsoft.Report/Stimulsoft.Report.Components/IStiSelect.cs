namespace Stimulsoft.Report.Components;

public interface IStiSelect
{
	int SelectionTick { get; }

	bool IsSelected { get; }

	void Select();

	void Invert();

	void Reset();
}
