namespace Stimulsoft.Report.Dashboard;

public interface IStiInteractionLayout
{
	bool ShowFullScreenButton { get; set; }

	bool ShowSaveButton { get; set; }

	bool ShowViewDataButton { get; set; }

	string HeaderText { get; set; }

	string FooterText { get; set; }

	string FileName { get; set; }

	bool IsDefaultLayout { get; }
}
