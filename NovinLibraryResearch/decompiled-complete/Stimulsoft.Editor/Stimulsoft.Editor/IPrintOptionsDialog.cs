using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IPrintOptionsDialog
{
	PrintOptions Options { get; set; }

	PrintOptions AllowedOptions { get; set; }

	string FileName { get; set; }

	DialogResult ShowDialog();

	void ResetOptions();

	void ResetAllowedOptions();
}
