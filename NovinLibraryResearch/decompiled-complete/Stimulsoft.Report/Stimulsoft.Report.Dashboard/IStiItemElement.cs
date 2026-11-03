namespace Stimulsoft.Report.Dashboard;

public interface IStiItemElement
{
	StiItemSelectionMode SelectionMode { get; set; }

	bool ShowBlanks { get; set; }

	bool ShowAllValue { get; set; }
}
