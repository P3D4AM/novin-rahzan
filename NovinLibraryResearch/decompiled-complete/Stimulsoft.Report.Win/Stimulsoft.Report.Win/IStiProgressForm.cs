using System.Windows.Forms;

namespace Stimulsoft.Report.Win;

public interface IStiProgressForm
{
	bool AllowUseDoEvents { get; set; }

	int Maximum { get; set; }

	int Minimum { get; set; }

	int Value { get; set; }

	string ProgressText { get; set; }

	bool IsBreaked { get; set; }

	ProgressBarStyle ProgressBarStyle { get; set; }

	bool ProgressBarVisible { get; set; }

	bool CancelButtonEnabled { get; set; }

	void CancelButtonFocus();

	void InvalidateProgress();
}
