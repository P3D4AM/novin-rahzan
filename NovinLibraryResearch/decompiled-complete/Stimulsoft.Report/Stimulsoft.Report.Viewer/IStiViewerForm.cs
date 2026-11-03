namespace Stimulsoft.Report.Viewer;

public interface IStiViewerForm
{
	StiReport Report { get; set; }

	bool ShowInTaskbar { get; set; }

	bool TopLevel { get; set; }

	object ViewerOwner { get; set; }

	object GetOwner(object form);

	void ShowViewer();

	void ShowDialogViewer();

	void ShowViewer(object win32Window);

	void ShowDialogViewer(object win32Window);
}
